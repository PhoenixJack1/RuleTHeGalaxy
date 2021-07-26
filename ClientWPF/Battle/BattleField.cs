using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;

namespace Client
{
    public enum BattleFieldSize { Small, Medium, Large }; //Нужно для сервера
    public class BattleField
    {
        public static BattleField[] Fields;
        public BattleFieldSize Size; //Нужно для Сервера
        public int Rows;
        public int Columns;
        public int[,,,] DistAngleRot;
        public byte[,,][] IntersectHexes;
        public Hex[] Hexes;
        public int Count;
        //public SortedSet<byte>[,] AIHexes;
        public SortedSet<byte> RandomHexes;//Нужно для Сервера
        public byte[] CenterHexes;//Нужно для Сервера
        public byte[] Defense2Hexes;//Нужно для Сервера
        public byte[] Defense3Hexes;//Нужно для Сервера
        public int MaxHex;
        bool HasLoadIntersect = false;
        public BattleField(int rows, int columns, BattleFieldSize size)
        {
            Rows = rows; Columns = columns;
            Size = size;
            Count = rows * columns;
            MaxHex = Count - 1;
            DistAngleRot = new int[Count, Count, 7, 3];
            LoadFile();
            if (HasLoadIntersect == false)
                IntersectHexes = new byte[Count, Count, 6][];
            
            FillHexStatic();
            GetAIHexes();
        }
        public static void Fill()
        {
            Fields = new BattleField[] {
                new BattleField(5, 8, BattleFieldSize.Small),
                new BattleField(7,12,BattleFieldSize.Medium),
                new BattleField(9,16,BattleFieldSize.Large)};
        }
        /// <summary> Возвращает хексы, которые попадают в зону порталов</summary>
        public byte[] GetPortalHexes(byte portalhex)
        {
            SortedSet<byte> result = new SortedSet<byte>();
            SortedSet<Hex> InnerStep = new SortedSet<Hex>();
            SortedSet<Hex> NextStep = new SortedSet<Hex>();
            Hex hex = Hexes[portalhex];
            InnerStep.Add(hex);
            result.Add(hex.ID);
            for (int i = 0; i < 3; i++)
            {
                NextStep = new SortedSet<Hex>();
                foreach (Hex inhex in InnerStep)
                    foreach (Hex h in inhex.NearHexes)
                    {
                        result.Add(h.ID);
                        NextStep.Add(h);
                    }
                InnerStep = NextStep;
            }
            return result.ToArray();
        }
        public void FillHexStatic()
        {
            Hexes = new Hex[Count];
            for (int i = 0; i < Count; i++)
                Hexes[i] = new Hex((byte)i, (byte)(i / Columns), (byte)(i % Columns));
            foreach (Hex hex in Hexes)
            {
                List<Hex> list = new List<Hex>();
                int Left = hex.Column - 1;
                int Right = hex.Column + 1;
                int Top = hex.Row - 1;
                int Bottom = hex.Row + 1;
                if (Left >= 0) list.Add(Hexes[hex.Row * Columns + Left]);
                if (Right < Columns) list.Add(Hexes[hex.Row * Columns + Right]);
                if (Top >= 0) list.Add(Hexes[Top * Columns + hex.Column]);
                if (Bottom < Rows) list.Add(Hexes[Bottom * Columns + hex.Column]);
                if (hex.Column % 2 == 0)
                {
                    if (Left >= 0 && Top >= 0) list.Add(Hexes[Top * Columns + Left]);
                    if (Right < Columns && Top >= 0) list.Add(Hexes[Top * Columns + Right]);
                }
                else
                {
                    if (Left >= 0 && Bottom < Rows) list.Add(Hexes[Bottom * Columns + Left]);
                    if (Right < 8 && Bottom < Rows) list.Add(Hexes[Bottom * Columns + Right]);
                }
                hex.NearHexes = list.ToArray();
            }
            for (int i = 0; i < Count; i++)
            {
                Hex hex = Hexes[i];
                for (int p = 0; p < Count; p++)
                {
                    //IntersectHexes[i, p, 0] = new byte[0];
                    //IntersectHexes[i, p, 1] = new byte[0];
                    //IntersectHexes[i, p, 2] = new byte[0];
                    if (p == i) continue;
                    Hex hex1 = Hexes[p];
                    double maxX = hex.CenterX - hex1.CenterX;
                    double maxY = hex1.CenterY - hex.CenterY;
                    int angle = GetAngle(maxX, maxY);
                    int rot = GetRotate(angle);
                    for (int j = 0; j < 6; j++)
                    {
                        Point pt = hex.GetGunPoint(j, rot);
                        double dx = pt.X - hex1.CenterX;
                        double dy = pt.Y - hex1.CenterY;
                        DistAngleRot[i, p, j, 0] = (int)(Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2)));
                        DistAngleRot[i, p, j, 1] = GetAngle(dx, -dy);
                        DistAngleRot[i, p, j, 2] = rot;
                        //List<byte> list = new List<byte>();
                        //MLine l1 = new MLine(pt.X, pt.Y, hex1.CenterX, hex1.CenterY);
                        //for (int z = 0; z < Count; z++)
                       // {
                        //    if (z == i || z == p) continue;

                        //    if (l1.IsIntersect(Hexes[z].GetVerLine()) || l1.IsIntersect(Hexes[z].GetHorLine()))
                        //        list.Add((byte)z);
                        //}
                        //IntersectHexes[i, p, j] = list.ToArray();
                    }
                    DistAngleRot[i, p, 6, 0] = (int)(Math.Sqrt(Math.Pow(hex.CenterX - hex1.CenterX, 2) + Math.Pow(hex.CenterY - hex1.CenterY, 2)));
                    DistAngleRot[i, p, 6, 1] = DistAngleRot[i, p, 1, 1];
                    DistAngleRot[i, p, 6, 2] = DistAngleRot[i, p, 1, 2];
                }
            }
        }
        void LoadFile()
        {
            string filename = String.Format("GameData/{0}-{1}.txt", Rows, Columns);
            //byte[] lf = (byte[])Links.GameData.GameArchive1.Objects[filename].File;
            byte[] lf = System.IO.File.ReadAllBytes(filename);
            byte imax = lf[0];
            byte pmax = lf[1];
            byte jmax = lf[2];
            IntersectHexes = new byte[imax, pmax, jmax][];
            for (int count = 3; count < lf.Length;)
            {
                for (byte i = 0; i < imax; i++)
                    for (byte p = 0; p < pmax; p++)
                        for (byte j = 0; j < jmax; j++)
                        {
                            byte length = lf[count]; count++;
                            byte[] arr = new byte[length];
                            for (byte k = 0; k < length; k++)
                            {
                                arr[k] = lf[count];
                                count++;
                            }
                            IntersectHexes[i, p, j] = arr;
                        }
            }
            HasLoadIntersect = true;
        }
        static int GetAngle(double dx, double dy)
        {
            double alpha;
            if (dy == 0)
                if (dx > 0) return 90; else return 270;
            if (dx >= 0 && dy > 0) alpha = Math.Atan(dx / dy) * 180 / Math.PI;
            else if (dx >= 0 && dy < 0) alpha = 180 + Math.Atan(dx / dy) * 180 / Math.PI;
            else if (dx <= 0 && dy < 0) alpha = 180 + Math.Atan(dx / dy) * 180 / Math.PI;
            else alpha = 360 + Math.Atan(dx / dy) * 180 / Math.PI;
            return (int)alpha;
        }
        static int GetRotate(int angle)
        {
            if (angle >= 180) angle = angle - 180;
            else angle = angle + 180;
            return angle;
        }
        public void GetAIHexes()
        {
            SortedSet<byte>[,] array = new SortedSet<byte>[2, 3];
            switch (Size)
            {
                case BattleFieldSize.Small:
                    /*array[0, 0] = new SortedSet<byte>(new byte[] { 1, 8 });
                    array[0, 1] = new SortedSet<byte>(new byte[] { 2, 9, 10, 16, 1, 8 });
                    array[0, 2] = new SortedSet<byte>(new byte[] { 3, 11, 17, 18, 24, 2, 9, 10, 16, 1, 8 });
                    array[1, 0] = new SortedSet<byte>(new byte[] { 31, 38 });
                    array[1, 1] = new SortedSet<byte>(new byte[] { 23, 29, 30, 37, 31, 38 });
                    array[1, 2] = new SortedSet<byte>(new byte[] { 15, 21, 22, 28, 36, 23, 29, 30, 37, 31, 38 });
                    AIHexes = array;*/
                    CenterHexes = new byte[] { 19, 20 };
                    Defense2Hexes = new byte[] { 13, 19 };
                    Defense3Hexes = new byte[] { 14, 20, 37 };
                    RandomHexes = new SortedSet<byte>(new byte[] { 4, 5, 6, 7, 12, 13, 14, 19, 20, 25, 27, 32, 33, 34, 35 });
                    break;
                case BattleFieldSize.Medium:
                    /*array[0, 0] = new SortedSet<byte>(new byte[] { 1, 12 });
                    array[0, 1] = new SortedSet<byte>(new byte[] { 13, 14, 1, 12, 2, 24 });
                    array[0, 2] = new SortedSet<byte>(new byte[] { 26, 15, 25, 14, 13, 3, 36, 1, 12, 2, 24 });
                    array[1, 0] = new SortedSet<byte>(new byte[] { 82, 71 });
                    array[1, 1] = new SortedSet<byte>(new byte[] { 69, 70, 71, 82, 59, 81 });
                    array[1, 2] = new SortedSet<byte>(new byte[] { 57, 68, 58, 69, 70, 47, 80, 82, 71, 81, 59 });
                    AIHexes = array;*/
                    CenterHexes = new byte[] { 41, 42 };
                    Defense2Hexes = new byte[] { 45, 55 };
                    Defense3Hexes = new byte[] { 46, 56, 67 };
                    RandomHexes = new SortedSet<byte>(new byte[] { 16,17,18,19,20,21,22,27,28,29,30,31,32,33,34,37,38,39,40,41,42,43,44,
                    49,50,51,52,53,54,61,62,63,64,65,66});
                    break;
                case BattleFieldSize.Large:
                    /*array[0, 0] = new SortedSet<byte>(new byte[] { 1, 16 });
                    array[0, 1] = new SortedSet<byte>(new byte[] { 17, 18, 1, 16, 2, 32 });
                    array[0, 2] = new SortedSet<byte>(new byte[] { 34, 19, 33, 18, 17, 3, 48, 1, 16, 2, 32 });
                    array[1, 0] = new SortedSet<byte>(new byte[] { 142, 127 });
                    array[1, 1] = new SortedSet<byte>(new byte[] { 125, 126, 142, 127, 111, 141 });
                    array[1, 2] = new SortedSet<byte>(new byte[] { 109, 124, 110, 125, 126, 95, 140, 142, 127, 111, 141 });
                    AIHexes = array;*/
                    CenterHexes = new byte[] { 72, 73, 88 };
                    Defense2Hexes = new byte[] { 93, 107 };
                    Defense3Hexes = new byte[] { 94, 108, 123 };
                    RandomHexes = new SortedSet<byte>(new byte[] { 20,21,22,23,24,25,26,27,28,29,30,35,36,37,38,39,40,41,42,43,44,45,46,
                    49,50,51,52,53,54,55,56,57,58,59,60,61,62,65,66,67,68,69,70,71,72,73,74,75,76,77,78,
                    81,82,83,84,85,86,87,88,89,90,91,92,97,98,99,100,101,102,103,104,105,106,
                    113,114,115,116,117,118,119,120,121,122});
                    break;
            }
        }
        public static BattleFieldGroup GetBattleField(BattleType battletype) //Нужно для сервера
        {
            switch (battletype)
            {
                case BattleType.Planet: return BattleFieldGroup.GetGroup(1, 1, true, false, false, 0);
                //case BattleType.Planet: return BattleFieldGroup.GetGroup(1, 1, true, false, false, 0);
                //case BattleType.LastBattle: return BattleFieldGroup.GetGroup(2, 2, false, true, false, 0);
                //case BattleType.TestBattle: return BattleFieldGroup.GetGroup(0, 0, false, false, false, 3);
                default:
                    throw new Exception();
            }
        }
        public static SortedList<byte, ServerShipB> GetAsteroids(byte[] array) //Нужно для сервера
        {
            SortedList<byte, ServerShipB> result = new SortedList<byte, ServerShipB>();
            for (byte i = 0; i < array.Length; i++)
            {
                ServerShipB asteroid = ServerShipB.GetAsteroid(i, array[i], false);// new ShipB(i, null, array[i]);
                asteroid.HelpID = 255;
                result.Add(asteroid.Hex, asteroid);
            }
            return result;
        }
        public byte[] GetRandomAsteroids(byte size, byte[] array) //Нужно для сервера
        {
            if (size == 0) return array;
            int count = 0;
            switch (Size)
            {
                case BattleFieldSize.Small: count = 1 + size; break;
                case BattleFieldSize.Medium: count = 1 + size * 2; break;
                case BattleFieldSize.Large: count = size * 5; break;
            }
            SortedSet<byte> result = new SortedSet<byte>(array);
            byte hex;
            for (int i = 0; i < count; i++)
            {
                hex = RandomHexes.ElementAt(ServerLinks.BattleRandom.Next(RandomHexes.Count));
                if (result.Contains(hex))
                    i--;
                else
                    result.Add(hex);
            }
            return result.ToArray();
        }
    }
    public class BattleFieldGroup //Нужно для сервера
    {
        public BattleField Field;
        public SortedList<byte, ServerShipB> Asteroids;
        public byte[] Background;
        public BattleFieldGroup(BattleField field, SortedList<byte, ServerShipB> asteroids, byte[] background)
        {
            Field = field;
            Asteroids = asteroids;
            Background = background;
        }
        public static BattleFieldGroup GetGroup(int minsize, int maxsize, bool need2hexes, bool need3hexes, bool needcenter, byte randomsize)
        {
            BattleField field;
            if (minsize == maxsize)
                field = (BattleField.Fields[minsize]);
            else
                field = (BattleField.Fields[ServerLinks.BattleRandom.Next(minsize, maxsize + 1)]);
            List<byte> hexes = new List<byte>();
            if (need2hexes)
                hexes.AddRange(field.Defense2Hexes);
            if (need3hexes)
                hexes.AddRange(field.Defense3Hexes);
            if (needcenter)
                hexes.AddRange(field.CenterHexes);
            byte[] totalasteroids = field.GetRandomAsteroids(randomsize, hexes.ToArray());
            return new BattleFieldGroup(field, BattleField.GetAsteroids(totalasteroids), new byte[] { 0, 0 });
        }
    }
}
