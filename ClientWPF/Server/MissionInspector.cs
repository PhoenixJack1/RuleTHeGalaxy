using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public enum MissionRarity { VeryRare, Rare, Often, OreBelt, Metheorit }
    public enum MissionLocation { Perimeter, Central, AlienBounds, AlienCenters }
    /*public class MissionInspector
    {
        public static byte missionscount = 13;
        GSPlayer Player;
        SortedList<ushort, Mission> OftenMission = new SortedList<ushort, Mission>();
        SortedList<ushort, Mission> RareMission = new SortedList<ushort, Mission>();
        SortedList<ushort, Mission> VeryRareMission = new SortedList<ushort, Mission>();
        SortedList<ushort, Mission> OreBeltMission = new SortedList<ushort, Mission>();
        SortedList<ushort, Mission> MetheoritMission = new SortedList<ushort, Mission>();
        public byte[] Array;
        ushort PlayerMissionID = 0;
        public MissionInspector(GSPlayer player)
        {
            Player = player;
            Array = new byte[0];
            //AddMeteoritMission();
            //AddOreBeltMission();
            //AddOreBeltMission();
        }
        ushort GetID()
        {
            PlayerMissionID++;
            return PlayerMissionID;
        }

        public void AddRareMission()
        {
            if (RareMission.Count == 3)
                RareMission.RemoveAt(0);
            Mission mission = null;
            for (int i = 0; i < 5; i++)
            {
                //MissionParam missionparam = Links.Enemy.RareMissions[2];
                MissionParam missionparam = ServerLinks.Enemy.RareMissions[ServerLinks.BattleRandom.Next(ServerLinks.Enemy.RareMissions.Count)];
                mission = GetMission(missionparam);
                if (mission != null) break;
            }
            if (mission == null) return;
            RareMission.Add(mission.ID, mission);
            CreateArray(true);
        }
        public void AddVeryRareMission()
        {
            if (VeryRareMission.Count == 2)
                VeryRareMission.RemoveAt(0);
            Mission mission = null;
            for (int i = 0; i < 5; i++)
            {
                //MissionParam missionparam = Links.Enemy.VeryRareMissions[1];
                MissionParam missionparam = ServerLinks.Enemy.VeryRareMissions[ServerLinks.BattleRandom.Next(ServerLinks.Enemy.VeryRareMissions.Count)];
                mission = GetMission(missionparam);
                if (mission != null) break;
            }
            if (mission == null) return;
            VeryRareMission.Add(mission.ID, mission);
            CreateArray(true);
        }
        public void AddOftenMission()
        {
            if (OftenMission.Count == 3)
                OftenMission.RemoveAt(0);
            Mission mission = null;
            for (int i = 0; i < 5; i++)
            {
                //MissionParam missionparam = Links.Enemy.OftenMissions[2];
                MissionParam missionparam = ServerLinks.Enemy.OftenMissions[ServerLinks.BattleRandom.Next(ServerLinks.Enemy.OftenMissions.Count)];
                mission = GetMission(missionparam);
                if (mission != null) break;
            }
            if (mission == null) return;
            OftenMission.Add(mission.ID, mission);
            CreateArray(true);
        }
        public void AddMeteoritMission()
        {
            if (MetheoritMission.Count == 1)
                MetheoritMission.RemoveAt(0);
            Mission mission = GetMission(ServerLinks.Missions[MissionType.MetheoritRaid]);
            MetheoritMission.Add(mission.ID, mission);
            CreateArray(false);
        }
        public void AddOreBeltMission()
        {
            if (OreBeltMission.Count == 2)
                OreBeltMission.RemoveAt(0);
            Mission mission = GetMission(ServerLinks.Missions[MissionType.OreBeltRaid]);
            OreBeltMission.Add(mission.ID, mission);
            CreateArray(false);
        }
        void CreateArray(bool player)
        {
            List<byte> list = new List<byte>();
            list.Add(0); list.Add(10); list.AddRange(new byte[4]);
            foreach (Mission mission in VeryRareMission.Values)
                list.AddRange(mission.Array);
            foreach (Mission mission in RareMission.Values)
                list.AddRange(mission.Array);
            foreach (Mission mission in OftenMission.Values)
                list.AddRange(mission.Array);
            foreach (Mission mission in MetheoritMission.Values)
                list.AddRange(mission.Array);
            foreach (Mission mission in OreBeltMission.Values)
                list.AddRange(mission.Array);
            byte[] length = BitConverter.GetBytes(list.Count - 6);
            for (int i = 2; i < 6; i++)
                list[i] = length[i - 2];
            Array = list.ToArray();
            if (player) Player.CreateArrayForPlayer();
        }
        public Mission GetMission(byte missiontype, ushort missionid)
        {
            MissionType type = (MissionType)missiontype;
            MissionParam param = ServerLinks.Missions[type];
            Mission mission;
            switch (param.Rarity)
            {
                case MissionRarity.VeryRare: if (VeryRareMission.ContainsKey(missionid)) { mission = VeryRareMission[missionid]; VeryRareMission.Remove(missionid); break; } else return null;
                case MissionRarity.Rare: if (RareMission.ContainsKey(missionid)) { mission = RareMission[missionid]; RareMission.Remove(missionid); break; } else return null;
                case MissionRarity.Often: if (OftenMission.ContainsKey(missionid)) { mission = OftenMission[missionid]; OftenMission.Remove(missionid); break; } else return null;
                case MissionRarity.OreBelt:
                    if (OreBeltMission.ContainsKey(missionid))
                    {
                        mission = OreBeltMission[missionid];
                        OreBeltMission.Remove(missionid);
                        AddOreBeltMission();
                        break;
                    }
                    else
                        return null;
                case MissionRarity.Metheorit:
                    if (MetheoritMission.ContainsKey(missionid))
                    {
                        mission = MetheoritMission[missionid];
                        MetheoritMission.Remove(missionid);
                        AddMeteoritMission();
                        break;

                    }
                    else
                        return null;
                default: return null;
            }
            CreateArray(false);
            return mission;
        }
       
        public byte GetEnemy(byte Enemies)
        {
            List<byte> list = new List<byte>();
            if ((Enemies & 1) > 0) list.Add(0);
            if ((Enemies & 2) > 0) list.Add(1);
            if ((Enemies & 4) > 0) list.Add(2);
            if ((Enemies & 8) > 0) list.Add(3);
            if ((Enemies & 16) > 0) list.Add(4);
            return (byte)Math.Pow(2, list[ServerLinks.BattleRandom.Next(list.Count)]);
        }
        public Mission GetMission(MissionParam Missionparam)
        {
            int starid = Mission.GetStarID(Missionparam.Location);
            if (starid < 0) return null;
            ushort id = GetID();
            byte level = 0;
            if (Missionparam.OnlyTop == false) level = (byte)GetNotTopLevel(Player.Sciences.MaxLevel);
            else level = Player.Sciences.MaxLevel;
            byte maxlevel = (byte)(level + Missionparam.MaxLevel);
            if (maxlevel < 6) maxlevel = 6;
            int minlevel = maxlevel - 6;
            return new Mission(id, Missionparam.Type, starid, GetEnemy(Missionparam.Enemies), level, (byte)minlevel, maxlevel, Missionparam.Ships, Missionparam.Resources);
        }
       
        static int GetNotTopLevel(byte playerlevel)
        {
            int chance = ServerLinks.BattleRandom.Next(100);
            if (chance < 20)
                return playerlevel;
            else
            {
                if (playerlevel <= 6) return 6;
                else
                    return ServerLinks.BattleRandom.Next(6, playerlevel);
            }
        }
    }*/
    public class MissionParam
    {
        public static byte[] ResModificators = new byte[13];
        public MissionType Type;
        public MissionRarity Rarity;
        public MissionLocation Location;
        public byte Resources;
        public byte Enemies;
        public byte MaxLevel;
        public byte Ships;
        public bool OnlyTop;
        public byte ResModificator;
        public bool? RareScience;
        public bool ResurrectEnemies;
        public MissionParam(MissionType type, MissionRarity rarity, MissionLocation location, byte res, byte enemies,
            byte maxlevel, byte ships, bool onlytop, bool? rareScience, byte resmodi, bool resurrect)
        {
            Type = type; Rarity = rarity; Location = location; Resources = res; Enemies = enemies; MaxLevel = maxlevel; Ships = ships; OnlyTop = onlytop;
            RareScience = rareScience;
            ResModificator = resmodi;
            ResurrectEnemies = resurrect;
        }
        public static SortedList<MissionType, MissionParam> GetMissions()
        {
            SortedList<MissionType, MissionParam> list = new SortedList<MissionType, MissionParam>();
            list.Add(MissionType.PirateBase, new MissionParam(MissionType.PirateBase, MissionRarity.VeryRare, MissionLocation.Perimeter, 20, 2, 5, 10, true, false, 12, false));
            list.Add(MissionType.ConvoyDestroy, new MissionParam(MissionType.ConvoyDestroy, MissionRarity.Often, MissionLocation.Central, 6, 9, 1, 3, false, false, 2, true));
            list.Add(MissionType.ConvoyDefense, new MissionParam(MissionType.ConvoyDefense, MissionRarity.Often, MissionLocation.Central, 1, 18, 3, 8, false, false, 3, false));
            list.Add(MissionType.OreBeltRaid, new MissionParam(MissionType.OreBeltRaid, MissionRarity.OreBelt, MissionLocation.Central, 2, 11, 1, 4, false, false, 2, false));
            list.Add(MissionType.MetheoritRaid, new MissionParam(MissionType.MetheoritRaid, MissionRarity.Metheorit, MissionLocation.Perimeter, 10, 20, 0, 3, false, false, 1, false));
            list.Add(MissionType.LongRangeRaid, new MissionParam(MissionType.LongRangeRaid, MissionRarity.Rare, MissionLocation.Perimeter, 24, 31, 2, 10, true, false, 6, false));
            list.Add(MissionType.Competition, new MissionParam(MissionType.Competition, MissionRarity.Often, MissionLocation.Central, 5, 16, 4, 5, true, false, 4, false));
            list.Add(MissionType.BigCompetition, new MissionParam(MissionType.BigCompetition, MissionRarity.Rare, MissionLocation.Perimeter, 8, 9, 2, 10, false, false, 9, false));
            list.Add(MissionType.AlienBounds, new MissionParam(MissionType.AlienBounds, MissionRarity.Often, MissionLocation.AlienBounds, 10, 4, 0, 7, false, false, 3, false));
            list.Add(MissionType.AlienBases, new MissionParam(MissionType.AlienBases, MissionRarity.Rare, MissionLocation.AlienCenters, 14, 4, 2, 5, false, false, 5, false));
            list.Add(MissionType.ScienceExpedition, new MissionParam(MissionType.ScienceExpedition, MissionRarity.Rare, MissionLocation.Perimeter, 17, 6, 3, 14, true, null, 7, false));
            list.Add(MissionType.ArtifactSearch, new MissionParam(MissionType.ArtifactSearch, MissionRarity.VeryRare, MissionLocation.Perimeter, 18, 24, 5, 5, true, true, 15, false));
            list.Add(MissionType.PirateShipyard, new MissionParam(MissionType.PirateShipyard, MissionRarity.Rare, MissionLocation.Perimeter, 6, 2, 3, 6, false, false, 8, true));
            for (int i = 0; i < 13; i++)
                ResModificators[i] = (byte)list[(MissionType)i].ResModificator;
            return list;
        }
    }
}
