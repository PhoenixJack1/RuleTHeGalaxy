using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    class BattleMoveConverter
    {
        public static string[] Convert(Battle battle, byte[] array)
        {
            List<string> result=new List<string>();
            BattleMove[] Moves = BattleMove.GetMovesArray(array);
            ShipB ship;
            string shipname;
            foreach (BattleMove move in Moves)
            {
                switch (move.Command)
                {
                    case 0: result.Add("Начало боя, раунд 1"); break;
                    case 1: result.Add(string.Format("Конец раунда {0}",move.Par1)); break;
                    case 2: result.Add(string.Format("Начало раунда {0}", move.Par1)); break;
                    case 3: ship=battle.Side1.Ships[move.Par1];
                        if (move.Par1 != 0) shipname = SelectSchemaNameCanvas.GetNameResult(ship.Schema.GetName());
                        else shipname = "Портал";
                        result.Add(string.Format("Корабль атакующей стороны {0} вышел в порт, ходов до попадания в гейт {1}", 
                            shipname, ship.Params.TimeToEnter)); break;
                    case 103: ship = battle.Side2.Ships[move.Par1];
                        if (move.Par1 != 0) shipname = SelectSchemaNameCanvas.GetNameResult(ship.Schema.GetName());
                        else shipname = "Портал";
                        result.Add(string.Format("Корабль защищающийся стороны {0} вышел в порт, ходов до попадания в гейт {1}",
                            shipname, ship.Params.TimeToEnter)); break;
                    case 4: ship = battle.Side1.Ships[move.Par1];
                        if (move.Par1 != 0) shipname = SelectSchemaNameCanvas.GetNameResult(ship.Schema.GetName());
                        else shipname = "Портал";
                        result.Add(String.Format("Корабль атакующей стороны {0} вышел на поле боя на позицию {1}", shipname, move.Par2)); break;
                    case 104: ship = battle.Side2.Ships[move.Par1];
                        if (move.Par1 != 0) shipname = SelectSchemaNameCanvas.GetNameResult(ship.Schema.GetName());
                        else shipname = "Портал";
                        result.Add(String.Format("Корабль защищающейся стороны {0} вышел на поле боя на позицию {1}", shipname, move.Par2)); break;
                    default: result.Add(string.Format("command={0} {1} {2} {3}", move.Command, move.Par1, move.Par2, move.Par3)); break;
                }
            }
            return result.ToArray();
        }
    }
    class BattleMove
    {
        public byte Command;
        public byte Par1;
        public byte Par2;
        public byte Par3;
        public BattleMove(byte command, byte par1, byte par2, byte par3)
        {
            Command = command; Par1 = par1; Par2 = par2; Par3 = par3;
        }
        public static BattleMove[] GetMovesArray(byte[] array)
        {
            List<BattleMove> list = new List<BattleMove>();
            for (int i = 0; i < array.Length; i+=4)
            {
                BattleMove move = new BattleMove(array[i], array[i + 1], array[i + 2], array[i + 3]);
                list.Add(move);
            }
            return list.ToArray();
        }
    }
}
