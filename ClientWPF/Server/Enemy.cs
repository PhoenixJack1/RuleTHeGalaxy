using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    class Enemy
    {
        public static void LoseBattle(ServerStar star, byte value)
        {
 
        }
        public static Schema GetSchema(ushort shiptype, ushort generator, ushort shield, ushort engine, ushort computer, ushort weap1, ushort weap2, ushort weap3,
            ushort equip1, ushort equip2, ushort equip3, ushort equip4, byte n1, byte n2, byte n3, byte n4, byte armortype)
        {
            Schema schema = new Schema();
            schema.SetShipType(shiptype);
            schema.SetGenerator(generator);
            schema.SetShield(shield);
            schema.SetEngine(engine);
            schema.SetComputer(computer);
            schema.SetWeapon(weap1, 0);
            schema.SetWeapon(weap2, 1);
            schema.SetWeapon(weap3, 2);
            schema.SetEquipment(equip1, 0);
            schema.SetEquipment(equip2, 1);
            schema.SetEquipment(equip3, 2);
            schema.SetEquipment(equip4, 3);
            schema.SetName(BitConverter.ToInt32(new byte[] { n1, n2, n3, n4 }, 0));
            schema.Armor = (ArmorType)armortype;
            return schema;
        }
       /* public static ServerFleet CalcResourceMissionFleet(Mission mission, out EnemyType enemy)
        {
            List<EnemyType> sides = new List<EnemyType>();
            if ((mission.Enemy & 1) == 1) sides.Add(EnemyType.GreenParty);
            if ((mission.Enemy & 2) == 2) sides.Add(EnemyType.Pirates);
            if ((mission.Enemy & 4) == 4) sides.Add(EnemyType.Aliens);
            if ((mission.Enemy & 8) == 8) sides.Add(EnemyType.Techno);
            if ((mission.Enemy & 16) == 16) sides.Add(EnemyType.Mercenaries);
            enemy = EnemyType.Pirates;
            if (sides.Count == 1) enemy = sides[0];
            else
            {
                int rand = ServerLinks.BattleRandom.Next(sides.Count);
                enemy = sides[rand];
            }
            mission.RealEnemy = enemy;
            switch (enemy)
            {
                case EnemyType.Pirates: return CalcPirateDefenseFleet(mission.MinLevel, mission.MaxLevel, mission.Ships, mission.StarID);
                case EnemyType.Aliens: return CalcAlienDefenseFleet(mission.MinLevel, mission.MaxLevel, mission.Ships, mission.StarID);
                case EnemyType.GreenParty: return CalcGreenTeamDefenseFleet(mission.MinLevel, mission.MaxLevel, mission.Ships, mission.StarID);
                case EnemyType.Techno: return CalcTechTeamDefenseFleet(mission.MinLevel, mission.MaxLevel, mission.Ships, mission.StarID);
                case EnemyType.Mercenaries: return CalcMercenariesDefenseFleet(mission.MinLevel, mission.MaxLevel, mission.Ships, mission.StarID);
                default: return null;
            }
        }*/
        static int alienschemaname = BitConverter.ToInt32(new byte[] { 253, 0, 0, 0 }, 0);
       /* public static ServerFleet CalcAlienDefenseFleet(byte minlevel, byte maxlevel, byte ships, int starid)
        {
            ServerFleet fleet = new ServerFleet(1000, new byte[] { 253, 0, 0, 0 }, null, false, new long[0], new FleetCustomParams(1000));
            for (int i = 0; i < ships; i++)
            {
                ServerShip ship = ServerSchemeGenerator.GetShip(i, maxlevel, EnemyType.Aliens);
                if (ship != null)
                    fleet.Ships.Add(ship.ID, ship);
            }
            fleet.Target = new ServerFleetTarget(ServerLinks.GSStars[starid], fleet, FleetMission.NPC);
            return fleet;
        }
        public static ServerFleet CalcPirateDefenseFleet(byte minlevel, byte maxlevel, byte ships, int starid)
        {
            ServerFleet fleet = new ServerFleet(-1, new byte[] { 252, 0, 0, 0 }, null, false, new long[0], new FleetCustomParams(1000));

            for (int i = 0; i < ships; i++)
            {
                ServerShip ship = ServerSchemeGenerator.GetShip(i, maxlevel, EnemyType.Pirates);
                if (ship != null)
                    fleet.Ships.Add(ship.ID, ship);
            }
            fleet.Target = new ServerFleetTarget(ServerLinks.GSStars[starid], fleet, FleetMission.NPC);
            return fleet;
        }
        public static ServerFleet CalcMercenariesDefenseFleet(byte minlevel, byte maxlevel, byte ships, int starid)
        {
            ServerFleet fleet = new ServerFleet(1000, new byte[] { 255, 0, 0, 0 }, null, false, new long[0], new FleetCustomParams(1000));

            for (int i = 0; i < ships; i++)
            {
                ServerShip ship = ServerSchemeGenerator.GetShip(i, maxlevel, EnemyType.Mercenaries);
                if (ship != null)
                    fleet.Ships.Add(ship.ID, ship);
            }
            fleet.Target = new ServerFleetTarget(ServerLinks.GSStars[starid], fleet, FleetMission.NPC);
            return fleet;
        }
        public static ServerFleet CalcTechTeamDefenseFleet(byte minlevel, byte maxlevel, byte ships, int starid)
        {
            ServerFleet fleet = new ServerFleet(1000, new byte[] { 254, 0, 0, 0 }, null, false, new long[0], new FleetCustomParams(1000));

            for (int i = 0; i < ships; i++)
            {
                ServerShip ship = ServerSchemeGenerator.GetShip(i, maxlevel, EnemyType.Techno);
                if (ship != null)
                    fleet.Ships.Add(ship.ID, ship);
            }
            fleet.Target = new ServerFleetTarget(ServerLinks.GSStars[starid], fleet, FleetMission.NPC);
            return fleet;
        }
        public static ServerFleet CalcGreenTeamDefenseFleet(byte minlevel, byte maxlevel, byte ships, int starid)
        {
            ServerFleet fleet = new ServerFleet(1000, new byte[] { 251, 0, 0, 0 }, null, false, new long[0], new FleetCustomParams(1000));

            for (int i = 0; i < ships; i++)
            {
                ServerShip ship = ServerSchemeGenerator.GetShip(i, maxlevel, EnemyType.GreenParty);
                if (ship != null)
                    fleet.Ships.Add(ship.ID, ship);
            }
            fleet.Target = new ServerFleetTarget(ServerLinks.GSStars[starid], fleet, FleetMission.NPC);
            return fleet;
        }*/
    }
}
