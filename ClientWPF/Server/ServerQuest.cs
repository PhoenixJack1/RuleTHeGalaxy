using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{

    public enum Reason_Finish { Schema_combined, Ship_Builded, Pilot_Hired, Fleet_Created, Ship_in_Fleet, Fleet_Sended, Battle_Finished, Science_Learned, Trade_Maked }
    public class ServerQuest
    {
        GSPlayer Player;
        public Quest_Position Position;
        public bool AllDone = false;
        public byte[] Array;
        public ServerQuest(GSPlayer player, Quest_Position position)
        {
            Player = player;
            Position = position;
            if (Position == Quest_Position.Q21_All_Done)
                AllDone = true;
            CreateArray();
        }
        void CreateArray()
        {
            List<byte> list = new List<byte>();
            list.Add(10); list.Add(3);
            list.AddRange(BitConverter.GetBytes(1));
            list.Add((byte)Position);
            Array = list.ToArray();
        }
        public bool SimpleFinish()
        {
            switch (Position)
            {
                case Quest_Position.Q01_Basic_Info:
                    Position = Quest_Position.Q02_Build_Bank;
                    Player.Resources.AddResources(new BuildingValues(3500, 200, 100, 0));
                    CreateArray();
                    Player.CreateArrayForPlayer();
                    return true;
                case Quest_Position.Q06_Scheme_Info:
                    Position = Quest_Position.Q07_Create_Scheme;
                    Player.Resources.AddResources(new BuildingValues(10000, 0, 0, 0));
                    CreateArray();
                    Player.CreateArrayForPlayer();
                    return true;
                case Quest_Position.Q18_Conquer_Info:
                    Position = Quest_Position.Q19_Alliance_Info;
                    Player.Resources.AddResources(new BuildingValues(10000, 0, 0, 0));
                    CreateArray();
                    Player.CreateArrayForPlayer();
                    return true;
                case Quest_Position.Q19_Alliance_Info:
                    Position = Quest_Position.Q20_Chat_Info;
                    Player.Resources.AddResources(new BuildingValues(10000, 0, 0, 0));
                    CreateArray();
                    Player.CreateArrayForPlayer();
                    return true;
                case Quest_Position.Q20_Chat_Info:
                    Position = Quest_Position.Q21_All_Done;
                    Player.Resources.AddResources(new BuildingValues(10000, 0, 0, 0));
                    CreateArray();
                    Player.CreateArrayForPlayer();
                    AllDone = true;
                    return true;
            }
            return false;
        }
        public void BuildBuilding(ushort buildingid, ushort count)
        {
            switch (Position)
            {
                case Quest_Position.Q02_Build_Bank:
                    if (buildingid != 13) return;
                    Position = Quest_Position.Q03_Build_Mine;
                    Player.Resources.AddResources(new BuildingValues(2000, 100, 200, 0));
                    CreateArray();
                    break;
                case Quest_Position.Q03_Build_Mine:
                    if (buildingid != 22) return;
                    Position = Quest_Position.Q04_Build_Chips;
                    Player.Resources.AddResources(new BuildingValues(3000, 300, 100, 0));
                    CreateArray();
                    break;
                case Quest_Position.Q04_Build_Chips:
                    if (buildingid != 33) return;
                    Position = Quest_Position.Q05_Build_Anti;
                    Player.Resources.AddResources(new BuildingValues(2500, 400, 600, 0));
                    CreateArray();
                    break;
                case Quest_Position.Q05_Build_Anti:
                    if (buildingid != 55) return;
                    Position = Quest_Position.Q06_Scheme_Info;
                    Player.Resources.AddResources(new BuildingValues(10000, 0, 0, 0));
                    CreateArray();
                    break;
                case Quest_Position.Q08_Build_Radar:
                    if (buildingid != 93 || count < 2) return;
                    Position = Quest_Position.Q09_Build_Ships;
                    Player.Resources.AddResources(new BuildingValues(20000, 1000, 1000, 1000));
                    CreateArray();
                    break;
                case Quest_Position.Q15_Build_Factory:
                    if (buildingid != 74) return;
                    Position = Quest_Position.Q16_Learn_Science;
                    Player.Resources.AddResources(new BuildingValues(1000, 100, 100, 100));
                    CreateArray();
                    break;
            }
        }
        public void ReasonFinish(Reason_Finish reason)
        {
            switch (Position)
            {
                case Quest_Position.Q07_Create_Scheme:
                    if (reason != Reason_Finish.Schema_combined || Player.Schemas.Length < 10) return;
                    Position = Quest_Position.Q08_Build_Radar;
                    Player.Resources.AddResources(new BuildingValues(6000, 400, 800, 0));
                    CreateArray();
                    break;
                case Quest_Position.Q09_Build_Ships:
                    if (reason != Reason_Finish.Ship_Builded || Player.Ships.Count < 2) return;
                    Position = Quest_Position.Q10_Hire_Pilots;
                    Player.Resources.AddResources(new BuildingValues(20000, 0, 0, 0));
                    CreateArray();
                    break;
                case Quest_Position.Q10_Hire_Pilots:
                    if (reason != Reason_Finish.Pilot_Hired || Player.Pilots.FreePilots.Count < 2) return;
                    Position = Quest_Position.Q11_Create_Fleet;
                    Player.Resources.AddResources(new BuildingValues(10000, 0, 0, 0));
                    CreateArray();
                    break;
                case Quest_Position.Q11_Create_Fleet:
                    if (reason != Reason_Finish.Fleet_Created) return;
                    Position = Quest_Position.Q12_Move_Ships_to_Fleet;
                    Player.Resources.AddResources(new BuildingValues(10000, 0, 0, 0));
                    CreateArray();
                    break;
                case Quest_Position.Q12_Move_Ships_to_Fleet:
                    if (reason != Reason_Finish.Ship_in_Fleet) return;
                    Position = Quest_Position.Q13_Send_To_Mission;
                    Player.Resources.AddResources(new BuildingValues(10000, 0, 0, 0));
                    CreateArray();
                    break;
                case Quest_Position.Q13_Send_To_Mission:
                    if (reason != Reason_Finish.Fleet_Sended) return;
                    Position = Quest_Position.Q14_Battle_End;
                    Player.Resources.AddResources(new BuildingValues(10000, 0, 0, 0));
                    CreateArray();
                    break;
                case Quest_Position.Q14_Battle_End:
                    if (reason != Reason_Finish.Battle_Finished) return;
                    Position = Quest_Position.Q15_Build_Factory;
                    Player.Resources.AddResources(new BuildingValues(3500, 300, 300, 0));
                    CreateArray();
                    break;
                case Quest_Position.Q16_Learn_Science:
                    if (reason != Reason_Finish.Science_Learned) return;
                    Position = Quest_Position.Q17_Make_Trade;
                    Player.Resources.AddResources(new BuildingValues(10000, 0, 0, 0));
                    CreateArray();
                    break;
                case Quest_Position.Q17_Make_Trade:
                    if (reason != Reason_Finish.Trade_Maked) return;
                    Position = Quest_Position.Q18_Conquer_Info;
                    Player.Resources.AddResources(new BuildingValues(10000, 0, 0, 0));
                    CreateArray();
                    Player.CreateArrayForPlayer();
                    break;
            }

        }
    }
}
