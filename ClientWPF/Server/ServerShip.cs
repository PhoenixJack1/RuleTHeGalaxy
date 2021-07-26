using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public class ServerShip
    {
        public long ID;
        public ServerFleet Fleet;
        public GSPlayer Player;
        public Schema Schema;
        public byte Health;
        public byte[] Model;
        public GSPilot Pilot;
        public float AverageLevel;
        public int RepairPrice;
        public ServerShip(long id, Schema schema, byte health, byte[] model)
        {
            ID = id;
            Schema = schema;
            Schema.Calculate();
            Health = health;
            Model = model;
            AverageLevel = Schema.GetSchemaLevel();
            RepairPrice = (schema.Price.Money / 10 + schema.Price.Metall + schema.Price.Chips + schema.Price.Anti) / 10;
            Pilot = GSPilot.BasicPilot;
            //SetParants();
        }
        public static ServerShip GetNewShip(Schema schema, byte[] model)
        {
            long id = ShipID.GetShipID();
            byte health = 100;
            ServerShip ship = new ServerShip(id, schema, health, model);
            return ship;
        }
        public static ServerShip GetShip(byte[] array, ref int i)
        {
            if (array.Length < i + 55) return null;
            long id = BitConverter.ToInt64(array, i);
            i += 8;
            int playerid = BitConverter.ToInt32(array, i);
            i += 4;
            Schema schema = Schema.GetSchema(array, ref i);
            byte health = array[i];
            i += 1;
            byte[] model = new byte[4];
            for (int j = 0; j < 4; j++)
                model[j] = array[i + j];
            i += 4;
            ServerShip ship = new ServerShip(id, schema, health, model);
            if (!GSPilot.IsNullPilot(array, i))
            {
                ship.Pilot = GSPilot.GetPilot(array, i);
            }
            i += 10;
            ServerLinks.GSPlayersByID[playerid].AddShip(ship);
            ShipID.CheckShipID(id);


            return ship;
        }
        public ItemPrice GetRepairPrice()
        {
            if (Health == 0) return Schema.Price;
            else
            {
                return new ItemPrice(Schema.Price.Money / 100 * (100 - Health),
                    Schema.Price.Metall / 100 * (100 - Health),
                    Schema.Price.Chips / 100 * (100 - Health),
                    Schema.Price.Anti / 100 * (100 - Health));
            }
        }
        public int CalcRating()
        {
            ServerShipB shipb = new ServerShipB(this, 0, ShipSide.Attack, null, 255);
            return shipb.Rating;
        }
        public byte[] GetArray()
        {
            List<byte> result = new List<byte>();
            result.AddRange(BitConverter.GetBytes(ID));//8
            result.AddRange(BitConverter.GetBytes(Player.ID));//12
            result.AddRange(Schema.GetCrypt());//48
            result.Add(Health);//52
            result.AddRange(Model); //56
            if (Pilot == null)
                result.AddRange(GSPilot.GetNullPilot());//66
            else
                result.AddRange(Pilot.GetArray());
            return result.ToArray();
        }
    }
    class ShipID
    {
        public long ID = -1;
        static ShipID ShipIDobject = new ShipID();
        public static long GetShipID()
        {
            lock (ShipIDobject)
            {
                ShipIDobject.ID++;
                return ShipIDobject.ID;
            }
        }
        public static void CheckShipID(long id)
        {
            if (ShipIDobject.ID < id) ShipIDobject.ID = id;
        }
    }
}
