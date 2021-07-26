using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Client
{
    public class GSPilot : IComparable
    {
        static Random rnd = new Random();
        public static ItemPrice PilotChangePrice = new ItemPrice(10000, 0, 0, 0);
        public static List<string[]> Nicks = new List<string[]>();
        public static List<string[]> JapCit = new List<string[]>();
        public static List<string[]> JapFem = new List<string[]>();
        public static List<string[]> JapMan = new List<string[]>();
        public static List<string[]> JapSur = new List<string[]>();
        public static List<string[]> RuCit = new List<string[]>();
        public static List<string[]> RuFem = new List<string[]>();
        public static List<string[]> RuMan = new List<string[]>();
        public static List<string[]> RuSur = new List<string[]>();
        public static List<string[]> AmerCit = new List<string[]>();
        public static List<string[]> AmerFem = new List<string[]>();
        public static List<string[]> AmerMan = new List<string[]>();
        public static List<string[]> AmerSur = new List<string[]>();
        public byte NameID;
        public byte SurNameID;
        public byte RaceID;
        public byte NickID;
        public byte PClass;
        public byte Exp;
        public byte Spec1;
        public byte Spec2;
        public byte Spec3;
        public byte Exp2;
        public byte Level;
        public byte Specialization;
        public byte Rang;
        public byte Talent;
        public readonly static GSPilot BasicPilot = GetBasicPilot();
        public GSPilot(byte[] array, int start)
        {
            NameID = array[start];
            SurNameID = array[start + 1];
            NickID = array[start + 2];
            RaceID = array[start + 3];
            PClass = array[start + 4];
            Exp = array[start + 5];
            Spec1 = array[start + 6];
            Spec2 = array[start + 7];
            Spec3 = array[start + 8];
            Exp2 = array[start + 9];
            CalcSpecs();
            double levelbase = 5 + Rang * 5;
            int sum = (Exp * 255 + Exp2);
            Level = (byte)(Math.Log(1 + sum / levelbase, 2));
            //Level = (byte)(0.5 + 0.5 * Math.Sqrt(1 + Exp * 1.6));
           
        }
        public GSPilot(int image, byte pclass, byte exp, byte spec1, byte spec2, byte spec3, byte exp2)
        {
            byte[] imarray = BitConverter.GetBytes(image);
            NameID = imarray[0]; SurNameID = imarray[1];
            NickID = imarray[2]; RaceID = imarray[3]; PClass = pclass; Exp = exp; Spec1 = spec1; Spec2 = spec2; Spec3 = spec3; Exp2 = exp2;
            CalcSpecs();
            int sum = (exp * 255 + exp2);
            double levelbase = 5 + Rang * 5;
            Level = (byte)(Math.Log(1 + sum / levelbase, 2));
            //Level = (byte)(0.5 + 0.5 * Math.Sqrt(1 + Exp * 1.6));

        }
        public static GSPilot GetBasicPilot()
        {
            return new GSPilot(new byte[] { 0, 0, 0, 100, 16, 0, 5, 15, 30, 0 }, 0);
        }
        public int CompareTo(object obj)
        {
            GSPilot p = (GSPilot)obj;
            int Image = BitConverter.ToInt32(new byte[] { NameID, SurNameID, NickID, RaceID }, 0);
            int Image2= BitConverter.ToInt32(new byte[] { p.NameID, p.SurNameID, p.NickID, p.RaceID }, 0);
            if (Image > Image2) return 1;
            else if (Image < Image2) return -1;
            else if (PClass > p.PClass) return 1;
            else if (PClass < p.PClass) return -1;
            else if (Exp > p.Exp) return 1;
            else if (Exp < p.Exp) return -1;
            else if (Spec1 > p.Spec1) return 1;
            else if (Spec1 < p.Spec1) return -1;
            else if (Spec2 > p.Spec2) return 1;
            else if (Spec2 < p.Spec2) return -1;
            else if (Spec3 > p.Spec3) return 1;
            else if (Spec3 < p.Spec3) return -1;
            else if (Exp2 > p.Exp2) return 1;
            else if (Exp2 < p.Exp2) return -1;
            else return 0;
        }
        public static byte[] GetPilotGroup()
        {
            List<byte> array = new List<byte>();
            array.AddRange(GetNewPilot());
            array.AddRange(GetNewPilot());
            array.AddRange(GetNewPilot());
            array.AddRange(GetNewPilot());
            return array.ToArray();
        }
        static byte[] spec2arr = new byte[] { 5, 6, 7, 8, 9, 10 };
        static byte[] spec3arr = new byte[] { 11, 16, 21 };
        static byte[] spec4arr = new byte[] { 30, 31, 32, 33, 34, 35, 36, 41, 46, 51 };
        public static byte[] GetNewPilot()
        {
            byte race = (byte)rnd.Next(6);
            byte name, surname;
            switch (race)
            {
                case 0: name = (byte)rnd.Next(RuMan.Count); surname = (byte)rnd.Next(RuSur.Count); break;
                case 1: name = (byte)rnd.Next(RuFem.Count); surname = (byte)rnd.Next(RuSur.Count); break;
                case 2: name = (byte)rnd.Next(JapMan.Count); surname = (byte)rnd.Next(JapSur.Count); break;
                case 3: name = (byte)rnd.Next(JapFem.Count); surname = (byte)rnd.Next(JapSur.Count); break;
                case 4: name = (byte)rnd.Next(AmerMan.Count); surname = (byte)rnd.Next(AmerSur.Count); break;
                default: name = (byte)rnd.Next(AmerFem.Count); surname = (byte)rnd.Next(AmerSur.Count); break;
            }
            byte nick = (byte)rnd.Next(Nicks.Count);
            //byte Specialization = 1; byte Rang = 4; byte Talent = 0;
            byte Specialization = (byte)(ServerCommon.GetRandomValue(new int[] { 30, 30, 18, 18, 4 }, rnd));
            byte Rang = (byte)(ServerCommon.GetRandomValue(new int[] { 50, 25, 15, 7, 3 }, rnd));
            byte Talent = (byte)(ServerCommon.GetRandomValue(new int[] { 50, 25, 15, 10 }, rnd));

            return GetNewPilotParams(name, surname, nick, race, Specialization, Rang, Talent, 0);
        }
        public static byte[] GetNewPilot(int levels, int bonus)
        {
            int[] rangarray = null;
            if (levels > 11)
                rangarray = new int[] { 50+bonus, 25+bonus, 15+bonus, 7+bonus, 3+bonus };
            else if (levels > 8)
                rangarray = new int[] { 50+bonus, 25+bonus, 15+bonus, 7+bonus };
            else if (levels > 5)
                rangarray = new int[] { 50+bonus, 25+bonus, 15+bonus };
            else if (levels > 2)
                rangarray = new int[] { 50+bonus, 25+bonus };
            else
                rangarray = new int[] { 50 };
            byte Rang = (byte)(ServerCommon.GetRandomValue(rangarray, rnd));
            levels -= Rang * 3;
            int[] talentarray = null;
            if (levels > 5)
                talentarray = new int[] { 50+bonus, 25+bonus, 15+bonus, 10+bonus };
            else if (levels > 3)
                talentarray = new int[] { 50+bonus, 25+bonus, 15+bonus };
            else if (levels > 1)
                talentarray = new int[] { 50+bonus, 25+bonus };
            else
                talentarray = new int[] { 50 };
            byte Talent = (byte)(ServerCommon.GetRandomValue(talentarray, rnd));
            levels -= Talent * 2;
            if (levels > 10) levels = 10;
            List<int> levelarray = new List<int>();
            for (int i = 0; i < levels; i++)
                levelarray.Add(100 - i * 9+bonus);
            levels = (byte)(ServerCommon.GetRandomValue(levelarray.ToArray(), rnd));
            byte Specialization = (byte)(ServerCommon.GetRandomValue(new int[] { 30, 30, 18, 18, 4 }, rnd));
            byte race = (byte)rnd.Next(6);
            byte name, surname;
            switch (race)
            {
                case 0: name = (byte)rnd.Next(RuMan.Count); surname = (byte)rnd.Next(RuSur.Count); break;
                case 1: name = (byte)rnd.Next(RuFem.Count); surname = (byte)rnd.Next(RuSur.Count); break;
                case 2: name = (byte)rnd.Next(JapMan.Count); surname = (byte)rnd.Next(JapSur.Count); break;
                case 3: name = (byte)rnd.Next(JapFem.Count); surname = (byte)rnd.Next(JapSur.Count); break;
                case 4: name = (byte)rnd.Next(AmerMan.Count); surname = (byte)rnd.Next(AmerSur.Count); break;
                default: name = (byte)rnd.Next(AmerFem.Count); surname = (byte)rnd.Next(AmerSur.Count); break;
            }
            byte nick = (byte)rnd.Next(Nicks.Count);
            double levelbase = 5 + Rang * 5;
            ushort sum = (ushort)((Math.Pow( 2,levels) - 1) * levelbase);
            return GetNewPilotParams(name, surname, nick, race, Specialization, Rang, Talent, sum);
        }
        static byte[] GetNewPilotParams(byte name, byte surname, byte nick, byte race, byte Specialization, byte Rang, byte Talent, ushort exp)
        {
            byte pclass = (byte)(Rang * 20 + Specialization * 4 + Talent);
            byte spec2 = spec2arr[rnd.Next(spec2arr.Length)];
            byte spec3 = (byte)(spec3arr[rnd.Next(spec3arr.Length)] + Specialization);

            byte spec4 = spec4arr[rnd.Next(spec4arr.Length)];
            if (spec4 >= 36) spec4 += Specialization;
            byte exp1 = (byte)(exp / 255);
            return new byte[] { name, surname, nick, race, pclass, exp1, spec2, spec3, spec4, (byte)(exp - exp1 * 255) };
        }
        public static GSPilot GetStartPilot(string param)
        {
            TextPosition[] positions = TextPosition.GetPositions(param);
            byte spec = 0; byte rang = 0; byte talent = 0; byte s1 = 255; byte s2 = 255; byte s3 = 255;
            foreach (TextPosition pos in positions)
            {
                switch (pos.Title)
                {
                    case "SPEC": spec = (byte)pos.IntValues[0]; break;
                    case "RANG": rang = (byte)pos.IntValues[0]; break;
                    case "TALENT": talent =(byte)pos.IntValues[0]; break;
                    case "S1": s1 = (byte)pos.IntValues[0]; break;
                    case "S2": s2 = (byte)pos.IntValues[0]; break;
                    case "S3": s3 = (byte)pos.IntValues[0]; break;
                }
            }
            return GetStartPilot(spec, rang, talent, s1, s2, s3);
        }
        public static GSPilot GetStartPilot(byte spec, byte rang, byte talent, byte s1, byte s2, byte s3)
        {
            byte race = (byte)rnd.Next(6);
            byte name, surname;
            switch (race)
            {
                case 0: name = (byte)rnd.Next(RuMan.Count); surname = (byte)rnd.Next(RuSur.Count); break;
                case 1: name = (byte)rnd.Next(RuFem.Count); surname = (byte)rnd.Next(RuSur.Count); break;
                case 2: name = (byte)rnd.Next(JapMan.Count); surname = (byte)rnd.Next(JapSur.Count); break;
                case 3: name = (byte)rnd.Next(JapFem.Count); surname = (byte)rnd.Next(JapSur.Count); break;
                case 4: name = (byte)rnd.Next(AmerMan.Count); surname = (byte)rnd.Next(AmerSur.Count); break;
                default: name = (byte)rnd.Next(AmerFem.Count); surname = (byte)rnd.Next(AmerSur.Count); break;
            }
            byte nick = (byte)rnd.Next(Nicks.Count);
            byte pclass = (byte)(rang * 20 + spec * 4 + talent);
            if (s1 == 255) s1 = spec2arr[rnd.Next(spec2arr.Length)];
            if (s2 == 255) s2 = (byte)(spec3arr[rnd.Next(spec3arr.Length)] + spec);
            if (s3 == 255)
            {
                s3 = spec4arr[rnd.Next(spec4arr.Length)];
                if (s3 >= 36) s3 += spec;
            }
            return new GSPilot(BitConverter.ToInt32(new byte[] { name, surname, nick, race }, 0), pclass, 0, s1, s2, s3, 0);
        }
        public bool IsBasicPilot()
        {
            if (RaceID == 100) return true; else return false;
        }
        public static string GetPilotLogInfo(byte[] array, ref int i)
        {
            //image[0], image[1], image[2], image[3], PClass, Exp, Spec1, Spec2, Spec3, Spec4
            if (array[i] == 255 && array[i + 1] == 255 && array[i + 2] == 255 && array[i + 3] == 255 && array[i + 4] == 255 && array[i + 5] == 255 && array[i + 6] == 255 && array[i + 7] == 255 && array[i + 8] == 255 && array[i + 9] == 255)
            {
                i += 10;
                return "Нет пилота";
            }
            else
            {
                int image = BitConverter.ToInt32(array, i); i += 4;
                GSPilot pilot = new GSPilot(array, i - 4);
                i += 6;
                return pilot.ToString();
            }
        }
        public void CalcSpecs()
        {
            Rang = (byte)(PClass / 20);
            Specialization = (byte)((PClass - Rang * 20) / 4);
            Talent = (byte)(PClass - Rang * 20 - Specialization * 4);
        }
        public static bool IsNullPilot(byte[] array, int i)
        {
            for (int j = i; j < i + 10; j++)
                if (array[j] != 255) return false;
            return true;
        }
        public static byte[] GetNullPilot()
        {
            return new byte[] { 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 };
        }
        public string GetName()
        {
            switch (RaceID)
            {
                case 0: return RuMan[NameID][Links.Lang];
                case 1: return RuFem[NameID][Links.Lang];
                case 2: return JapMan[NameID][Links.Lang];
                case 3: return JapFem[NameID][Links.Lang];
                case 4: return AmerMan[NameID][Links.Lang];
                case 5: return AmerFem[NameID][Links.Lang];
                case 100: return "";
                case 200: return StoryLine2.StoryPilotsInfo[NameID].Name;
                default: return "Error"; 
            }
        }
        public string GetSurname()
        {
            switch (RaceID)
            {
                case 0: return RuSur[SurNameID][Links.Lang];
                case 1: if (Links.Lang == 0) return RuSur[SurNameID][0] + "a";
                    else return RuSur[NameID][1] + "а";
                case 2:
                case 3: return JapSur[SurNameID][Links.Lang];
                case 4:
                case 5: return AmerSur[SurNameID][Links.Lang];
                case 100: return "";
                case 200: return StoryLine2.StoryPilotsInfo[NameID].Surname;
                default: return "Error";
            }
        }
        public string GetNick()
        {
            if (RaceID == 100) return Links.Interface("Autopilot");
            else if (RaceID==200) return StoryLine2.StoryPilotsInfo[NameID].Nick;
            return Nicks[NickID][Links.Lang];
        }
        public string GetCountry()
        {
            switch (RaceID)
            {
                case 0:
                case 1: return Links.Interface("Russia");
                case 2:
                case 3: return Links.Interface("Japan");
                case 4:
                case 5: return Links.Interface("American");
                case 100: return "";
                case 200: switch(StoryLine2.StoryPilotsInfo[NameID].Country)
                    {
                        case 0: return Links.Interface("Russia");
                        case 1: return Links.Interface("Japan");
                        case 2: return Links.Interface("American");
                        case 100: return "";
                        default: return "Error";
                    } 

                default: return "Error";
            }
        }
        public string GetCity()
        {
            int citybase = (NickID + NameID * SurNameID + RaceID * Spec1 + Spec2 * Spec3);
            switch (RaceID)
            {
                case 0:
                case 1: return RuCit[citybase % RuCit.Count][Links.Lang];
                case 2:
                case 3: return JapCit[citybase % JapCit.Count][Links.Lang];
                case 4:
                case 5: return AmerCit[citybase % AmerCit.Count][Links.Lang];
                case 100: return "";
                case 200: return StoryLine2.StoryPilotsInfo[NameID].City;
                default: return "Error";
            }
        }
        public string GetSex()
        {
            if (RaceID == 100) return "";
            if (RaceID == 200)
            {
                if (StoryLine2.StoryPilotsInfo[NameID].Sex == 0) return Links.Interface("Man");
                else return Links.Interface("Female");
            }
            if (RaceID == 0 || RaceID == 2 || RaceID== 4) return Links.Interface("Man");
            else return Links.Interface("Female");
        }
        public string GetAge()
        {
            if (RaceID == 100) return "";
            int age;
            if (RaceID == 200) age=StoryLine2.StoryPilotsInfo[NameID].Age;
            else age = 16 + (NickID * NameID + SurNameID * RaceID + Spec1 * Spec2 + Spec3 * 59) % 14;
            if (Links.Lang == 0)
                return age.ToString() + " years";
            else
            {
                int digits = age - ((int)age / 10) * 10;
                if (digits == 1) return age.ToString() + " год";
                else if (digits == 2 || digits == 3 || digits == 4) return age.ToString() + " года";
                else return age.ToString() + " лет";
            }
        }
        public byte[] GetArray()
        {
            return new byte[] { NameID, SurNameID, NickID, RaceID, PClass, Exp, Spec1, Spec2, Spec3, Exp2 };
        }
        public SecondaryEffect[] GetEffects()
        {
            List<SecondaryEffect> list = new List<SecondaryEffect>();
            list.Add(SecondaryEffect.GetEffect(Specialization, Level, Talent));
            if (Rang > 0) list.Add(SecondaryEffect.GetEffect(Spec1, Level, Talent));
            if (Rang > 1) list.Add(SecondaryEffect.GetEffect(Spec2, Level, Talent));
            if (Rang > 2) list.Add(SecondaryEffect.GetEffect(Spec3, Level, Talent));
            if (Rang == 4) list.Add(SecondaryEffect.GetEffect(GetSpec5Effect(), Level, Talent));
            return list.ToArray();
        }
        public ServerSecondaryEffect[] GetServerEffects()
        {
            List<ServerSecondaryEffect> list = new List<ServerSecondaryEffect>();
            list.Add(new ServerSecondaryEffect(Specialization, Level, Talent));
            if (Rang > 0) list.Add(new ServerSecondaryEffect(Spec1, Level, Talent));
            if (Rang > 1) list.Add(new ServerSecondaryEffect(Spec2, Level, Talent));
            if (Rang > 2) list.Add(new ServerSecondaryEffect(Spec3, Level, Talent));
            if (Rang == 4) list.Add(new ServerSecondaryEffect(GetSpec5Effect(), Level, Talent));
            return list.ToArray();
        }
        byte GetSpec5Effect()
        {
            if (Specialization == 4) return (byte)((BitConverter.ToInt32(new byte[] { NameID, SurNameID, NickID, RaceID },0) + Spec1 + Spec2 + Spec3) % 4 + 59);
            else return (byte)(Specialization + 59);
        }
        public override string ToString()
        {
            SecondaryEffect[] effects = GetEffects();
            string result = String.Format("Level={0} Specialization={1} Rang={2} Talent={3}", Level, ((WeaponGroup)Specialization).ToString(), Rang, Talent);
            for (int i = 0; i < effects.Length; i++)
                result += String.Format("\n Spec{0} {1}", i + 1, effects[i].ToString());
            return result;
        }
        public static GSPilot GetPilot(byte[] array, int i)
        {
            int Image = BitConverter.ToInt32(array, i);
            byte PClass = array[i + 4];
            byte Exp = array[i + 5];
            byte Spec1 = array[i + 6];
            byte Spec2 = array[i + 7];
            byte Spec3 = array[i + 8];
            byte Exp2 = array[i + 9];
            return new GSPilot(Image, PClass, Exp, Spec1, Spec2, Spec3, Exp2);
        }
        public enum Spec2E { None, Health = 5, HealthRegen = 6, Shield = 7, ShieldRegen = 8, Energy = 9, EnergyRegen = 10 }
        public enum Spec3E { None, Evasion = 11, Ignore = 16, Damage = 21 }
        public enum Spec4E { None, Health = 30, HealthRegen = 31, Shield = 32, ShieldRegen = 33, Energy = 34, EnergyRegen = 35, Accuracy = 36, Evasion = 41, Ignore = 46, Damage = 51 }
        public static GSPilot GetSpecPilot(byte level, byte spec, byte talent, Spec2E spec2, Spec3E spec3, Spec4E spec4, bool spec5)
        {
            byte rang = 0;
            if (spec5) rang = 4;
            else if (spec4 != Spec4E.None) rang = 3;
            else if (spec3 != Spec3E.None) rang = 2;
            else if (spec2 != Spec2E.None) rang = 1;
            double levelbase = 5 + rang * 5;
            int sum = (int)(levelbase * (Math.Pow(2, level) - 1));
            byte exp = (byte)(sum / 255);
            byte exp2 = (byte)(sum - exp * 255);
            byte spec2b;
            if (spec2 != Spec2E.None) spec2b = (byte)spec2; else spec2b = (byte)Spec2E.Health;
            byte spec3b;
            if (spec3 != Spec3E.None) spec3b = (byte)((byte)spec3 + spec); else spec3b = (byte)((byte)Spec3E.Evasion + spec);
            byte spec4b;
            if (spec4 != Spec4E.None)
            {
                spec4b = (byte)spec4;
                if (spec4b >= 36) spec4b += spec;
            }
            else
            {
                spec4b = (byte)Spec4E.Health;
            }
            byte pclass = (byte)(rang * 20 + spec * 4 + talent);
            return new GSPilot(0, pclass, exp, spec2b, spec3b, spec4b, exp2);
        }
        public static void DistributeExpirience(ServerFleet fleet, int experience) //Нужно для сервера
        {
            List<ServerShip> list = new List<ServerShip>();
            foreach (ServerShip ship in fleet.Ships.Values)
                if (ship.Health > 0)
                    list.Add(ship);
            if (list.Count == 0) return;
            if (experience >= list.Count)
            {
                byte addedvalue = (byte)(experience / list.Count);
                foreach (ServerShip ship in list)
                    ship.Pilot.AddExp(addedvalue);
                experience -= (byte)(addedvalue * list.Count);
            }
            for (int i = 0; i < experience; i++)
                list[ServerLinks.BattleRandom.Next(list.Count)].Pilot.AddExp(1);

        }
        public void AddExp(byte val) //Нужно для сервера
        {
            int sum = Exp * 255 + Exp2;
            if (sum + val > ushort.MaxValue)
                sum = ushort.MaxValue;
            else
                sum += val;
            double levelbase = 5 + Rang * 5;
            Level = (byte)(Math.Log(1 + sum / levelbase, 2));
            Exp = (byte)(sum / 255);
            Exp2 = (byte)(sum - Exp * 255);
        }
    }
    class PilotLargeInfo:Border
    {
        public PilotLargeInfo(GSPilot pilot)
        {
            Width = 350;
            BorderBrush = Links.Brushes.SkyBlue;
            BorderThickness = new Thickness(2);
            CornerRadius = new CornerRadius(10);
            Background = Brushes.Black;
            StackPanel panel = new StackPanel(); panel.Orientation = Orientation.Vertical;
            Child = panel;
            TextBlock title = Common.GetBlock(32, "Пилот", Links.Brushes.SkyBlue, 340);
            panel.Children.Add(title);
            TextBlock name = Common.GetBlock(30, String.Format("{0} \"{1}\" {2}", pilot.GetName(), pilot.GetNick(), pilot.GetSurname()), Brushes.White, 290);
            panel.Children.Add(name);

            Border BattleBorder = new Border(); BattleBorder.Width = 340; BattleBorder.BorderBrush = Links.Brushes.SkyBlue;
            BattleBorder.CornerRadius = new CornerRadius(10); BattleBorder.Background = Brushes.Black; BattleBorder.BorderThickness = new Thickness(2);
            panel.Children.Add(BattleBorder);
            StackPanel BattlePanel = new StackPanel();
            BattleBorder.Child = BattlePanel;
            BattlePanel.Orientation = Orientation.Vertical;
            TextBlock parameters = Common.GetBlock(26, "Достижения", Brushes.White, 330);
            BattlePanel.Children.Add(parameters);
            BattlePanel.Children.Add(GetRangBlock(pilot.Rang));
            BattlePanel.Children.Add(GetTalentBlock(pilot.Talent));
            BattlePanel.Children.Add(GetSpecializationBlock(pilot.Specialization));
            BattlePanel.Children.Add(GetLevelBlock(pilot.Level));

            Border BioBorder = new Border(); BioBorder.Width = 340; BioBorder.BorderBrush = Links.Brushes.SkyBlue;
            BioBorder.CornerRadius = new CornerRadius(10); BioBorder.Background = Brushes.Black; BioBorder.BorderThickness = new Thickness(2);
            panel.Children.Add(BioBorder);
            StackPanel BioPanel = new StackPanel();
            BioBorder.Child = BioPanel;
            BioPanel.Orientation = Orientation.Vertical;
            TextBlock bio = Common.GetBlock(26, "Биография", Brushes.White, 330);
            BioPanel.Children.Add(bio);
            BioPanel.Children.Add(GetCountyBlock(pilot.RaceID, pilot.NameID));
            BioPanel.Children.Add(GetCityBlock(pilot.GetCity()));
            BioPanel.Children.Add(GetSexBlock(pilot.GetSex()));
            BioPanel.Children.Add(GetAgeBlock(pilot.GetAge()));

            Border BonusBorder = new Border(); BonusBorder.Width = 340; BonusBorder.BorderBrush = Links.Brushes.SkyBlue;
            BonusBorder.CornerRadius = new CornerRadius(10); BonusBorder.Background = Brushes.Black; BonusBorder.BorderThickness = new Thickness(2);
            panel.Children.Add(BonusBorder);
            StackPanel BonusPanel = new StackPanel();
            BonusBorder.Child = BonusPanel;
            BonusPanel.Orientation = Orientation.Vertical;
            TextBlock bonus = Common.GetBlock(26, "Бонусы", Brushes.White, 330);
            BonusPanel.Children.Add(bonus);
            SecondaryEffect[] effects = pilot.GetEffects();
            for (int i=0;i<effects.Length;i++)
            {
                TextBlock effecttext = GetEffectBlock(i, effects[i].Title(), effects[i].Value, effects[i].Group);
                BonusPanel.Children.Add(effecttext);
            }

            
        }
        TextBlock GetEffectBlock(int pos, string text, int value, WeaponGroup group)
        {
            TextBlock block = Common.GetBlock(24, (pos + 1).ToString() + ") ", Brushes.White, 330);
            Run run = new Run(text);
            switch (group)
            {
                case WeaponGroup.Energy: run.Foreground = Brushes.Blue; break;
                case WeaponGroup.Physic: run.Foreground = Brushes.Red; break;
                case WeaponGroup.Irregular: run.Foreground = Brushes.Violet; break;
                case WeaponGroup.Cyber: run.Foreground = Brushes.Green; break;
                case WeaponGroup.Any: run.Foreground = Brushes.Gold; break;
        }
            block.Inlines.Add(run);
            Run run1 = new Run(value.ToString()); run1.Foreground = Links.Brushes.SkyBlue;
            block.Inlines.Add(run1);
            block.TextAlignment = TextAlignment.Left;
            return block;
        }
        TextBlock GetAgeBlock(string age)
        {
            TextBlock block = Common.GetBlock(24, "Возраст: ", Brushes.White, 330);
            Run run = new Run(age); run.Foreground = Links.Brushes.SkyBlue;
            block.Inlines.Add(run);
            block.TextAlignment = TextAlignment.Left;
            return block;
        }
        TextBlock GetSexBlock(string sex)
        {
            TextBlock block = Common.GetBlock(24, "Пол: ", Brushes.White, 330);
            Run run = new Run(sex); run.Foreground = Links.Brushes.SkyBlue;
            block.Inlines.Add(run);
            block.TextAlignment = TextAlignment.Left;
            return block;
        }
        TextBlock GetCityBlock(string city)
        {
            TextBlock block = Common.GetBlock(24, "Город: ", Brushes.White, 330);
            Run run = new Run(city); run.Foreground = Links.Brushes.SkyBlue;
            block.Inlines.Add(run);
            block.TextAlignment = TextAlignment.Left;
            return block;
        }
        TextBlock GetCountyBlock(byte raceid, byte nameid)
        {
            TextBlock block = Common.GetBlock(24, "Происхождение: ", Brushes.White, 330);
            Rectangle rect = new Rectangle(); rect.Width = 30; rect.Height = 20;
            rect.Fill = PilotsImage.GetFlagImage(raceid, nameid);
            block.Inlines.Add(new InlineUIContainer(rect));
            block.TextAlignment = TextAlignment.Left;
            return block;
        }
        TextBlock GetRangBlock(byte rang)
        {
            TextBlock block = Common.GetBlock(24, "Обученность: ", Brushes.White, 330);
            Run par = new Run((rang+1).ToString()); par.Foreground = Links.Brushes.SkyBlue;
            block.Inlines.Add(par);
            if (rang == 0)
                block.Inlines.Add(new Run(" навык"));
            else if (rang == 4)
                block.Inlines.Add(new Run(" навыков"));
            else
                block.Inlines.Add(new Run(" навыка"));
            block.TextAlignment = TextAlignment.Left;
            return block; 
        }
        TextBlock GetTalentBlock(byte talent)
        {
            TextBlock block = Common.GetBlock(24, "Талант: ", Brushes.White, 330);
            Run par = null;
            switch (talent)
            {
                case 0: par = new Run("отсутствует"); break;
                case 1: par = new Run("малый"); break;
                case 2: par = new Run("средний"); break;
                case 3: par = new Run("высокий"); break;
            }
            par.Foreground = Links.Brushes.SkyBlue;
            block.Inlines.Add(par);
            block.TextAlignment = TextAlignment.Left;
            return block;
        }
        TextBlock GetSpecializationBlock(byte spec)
        {
            TextBlock block = Common.GetBlock(24, "Специализация: ", Brushes.White, 330);
            Run par = null;
            switch (spec)
            {
                case 0: par = new Run("энергетическая"); par.Foreground = Brushes.SkyBlue; break;
                case 1: par = new Run("физическая"); par.Foreground = Brushes.Red; break;
                case 2: par = new Run("аномальная"); par.Foreground = Brushes.Violet; break;
                case 3: par = new Run("кибернетическая");par.Foreground = Brushes.Green;  break;
                case 4: par = new Run("универсальная"); par.Foreground = Brushes.Gold; break;
            }
            block.Inlines.Add(par);
            block.TextAlignment = TextAlignment.Left;
            return block;
        }
        TextBlock GetLevelBlock(byte level)
        {
            TextBlock block = Common.GetBlock(24, "Ранг: ", Brushes.White, 330);
            Run par = new Run(level.ToString()); par.Foreground = Links.Brushes.SkyBlue;
            block.Inlines.Add(par);
            block.TextAlignment = TextAlignment.Left;
            return block;
        }
    }
    class PilotGenerator : Canvas
    {
        ScrolledPanel Special;
        ScrolledPanel Rang;
        ScrolledPanel Talent;
        ScrolledPanel Level;
        ScrolledPanel Spec2;
        ScrolledPanel Spec3;
        ScrolledPanel Spec4;
        ScrolledPanel Spec5;
        Border PilotBorder;
        TextBox Result;
        public PilotGenerator()
        {
            Width = 1000;
            Height = 600;
            Background = Brushes.White;
            Grid grid = new Grid();
            grid.Width = 800;
            grid.Height = 500;
            Children.Add(grid);
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions[1].Height = new GridLength(450);
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            
            PutLabel(0, "Speial", grid);
            PutLabel(1, "Rang", grid);
            PutLabel(2, "Talent", grid);
            PutLabel(3, "Level", grid);
            PutLabel(4, "Spec2", grid);
            PutLabel(5, "Spec3", grid);
            PutLabel(6, "Spec4", grid);
            PutLabel(7, "Spec5", grid);

            Button CloseButton = new Button();
            CloseButton.Content = "Close";
            Children.Add(CloseButton);
            Canvas.SetLeft(CloseButton, 900);
            Canvas.SetTop(CloseButton, 560);
            CloseButton.Click += new RoutedEventHandler(CloseButton_Click);

            PilotBorder = Common.PutBorder(200, 200, Brushes.Black, 1, 800, 200, this);
            Special = new ScrolledPanel(new string[] { "Энергия", "Физика", "Аномалка", "Кибернетика", "Универсал" }, 5, 0);
            grid.Children.Add(Special);
            Grid.SetRow(Special, 1);

            Rang = new ScrolledPanel(new string[] { "Один навык", "Два навыка", "Три навыка", "Четыре навыка", "Пять навыков" }, 5, 0);
            grid.Children.Add(Rang);
            Grid.SetRow(Rang, 1);
            Grid.SetColumn(Rang, 1);

            Talent = new ScrolledPanel(new string[] { "Нет", "Низкий", "Средний", "Высокий" }, 4, 0);
            grid.Children.Add(Talent);
            Grid.SetRow(Talent, 1);
            Grid.SetColumn(Talent, 2);

            Level = new ScrolledPanel(new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" }, 11, 0);
            grid.Children.Add(Level);
            Grid.SetRow(Level, 1);
            Grid.SetColumn(Level, 3);

            Spec2 = new ScrolledPanel(new string[] { "ХП", "РегенХП", "Щит", "РегенЩит", "Энергия", "РегенЭнерг" }, 6, 0);
            grid.Children.Add(Spec2);
            Grid.SetRow(Spec2, 1);
            Grid.SetColumn(Spec2, 4);
            
            Spec3 = new ScrolledPanel(new string[] { "Уклонение", "Абсорб", "Дамаг" }, 3, 0);
            grid.Children.Add(Spec3);
            Grid.SetRow(Spec3, 1);
            Grid.SetColumn(Spec3,5);

            Spec4 = new ScrolledPanel(new string[] {"ХП", "РегенХП", "Щит", "РегенЩит", "Энергия", "РегенЭнерг", "Меткость", "Уклонение", "Абсорб", "Дамаг" }, 10, 0);
            grid.Children.Add(Spec4);
            Grid.SetRow(Spec4, 1);
            Grid.SetColumn(Spec4, 6);

            Spec5 = new ScrolledPanel(new string[] { "Энергия", "Физика", "Аномалка", "Кибернетика" }, 4, 0);
            grid.Children.Add(Spec5);
            Grid.SetRow(Spec5, 1);
            Grid.SetColumn(Spec5, 7);

            Result = new TextBox();
            Children.Add(Result);
            Canvas.SetTop(Result, 560);

            PutPilot();
            PreviewMouseWheel += new MouseWheelEventHandler(PilotGenerator_PreviewMouseWheel);
        }
        static byte[] spec4arr = new byte[] { 30, 31, 32, 33, 34, 35, 36, 41, 46, 51 };
        void PilotGenerator_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            PutPilot();
        }
        void PutPilot()
        {
            byte spec = Special.currentid;
            byte rang = Rang.currentid;
            byte talent = Talent.currentid;
            byte pclass = (byte)(rang * 20 + spec * 4 + talent);
            double levelbase = 5 + rang * 5;
            int sum = (int)(levelbase * Math.Pow(2, Level.currentid) - levelbase);
            byte exp1 = (byte)(sum / 255);
            byte exp2 = (byte)(sum - exp1 * 255);
            //byte exp=(byte)((Math.Pow((Level.currentid+0.5)*2,2)-1)/1.6);
            byte spec2 = (byte)(Spec2.currentid + 5);
            byte spec3 = (byte)(11 + Spec3.currentid * 5 + spec);
            byte spec4 = spec4arr[Spec4.currentid];
            if (spec4 >= 36) spec4 += spec;
            //byte spec5 = (byte)(spec + 59);
            //if (spec == 4) spec5 = (byte)(Spec5.currentid + 59);
            GSPilot pilot = new GSPilot(new byte[] { 0, 0, 0, 0, pclass, exp1, spec2, spec3, spec4, exp2 }, 0);
            PilotBorder.Child = new PilotsImage(pilot, PilotsListMode.Academy, null);

            Result.Text = String.Format("GSPilot.GetPilot(new byte[] {{ 0, 0, 0, 0, {0}, {1}, {2}, {3}, {4}, {5} }}, 0)", pclass, exp1, spec2, spec3, spec4, exp2);
            Clipboard.SetText(Result.Text);
        }
        void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Links.Controller.PopUpCanvas.Remove();
        }
        void PutLabel(int column, string text, Grid grid)
        {
            Label lbl = new Label();
            grid.Children.Add(lbl);
            Grid.SetColumn(lbl, column);
            lbl.Content = text;
        }
       
    }
}
