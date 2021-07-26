using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public class GSShip
    {
        public Schema Schema { get; private set; }
        public byte Health { get; set; }
        public long ID { get; private set; }
        public GSFleet Fleet;
        public int Model;
        public GSPilot Pilot;
        public int Colony = 0;
        public int Cargo = 0;
        public GSShip(long id, Schema schema, byte health, int model)
        {
            ID = id;
            Schema = schema;
            CalcCargoColony();
            Health = health;
            Model = model;
        }
        public static GSShip GetShip(byte[] array, ref int i)
        {
            if (array.Length<i+55) return null;
            long id = BitConverter.ToInt64(array, i);
            i += 8;
            int playerid = BitConverter.ToInt32(array, i);
            i += 4;
            Schema schema = Schema.GetSchema(array, ref i);
            byte health = array[i];
            i += 1;
            int model = BitConverter.ToInt32(array, i);
            i += 4;
            GSShip ship = new GSShip(id, schema, health, model);
            //ship.SetParents();

            if (!GSPilot.IsNullPilot(array, i))
                ship.Pilot = new GSPilot(array, i);
            i += 10;
            return ship;
        }
        public void SetParents()
        {
            if (Fleet != null)
                Fleet.Ships.Add(ID, this);
        }
        public void CalcCargoColony()
        {
            foreach (Equipmentclass equip in Schema.Equipments)
                if (equip == null) continue;
                else if (equip.Type == 57) Cargo += equip.Value;
                else if (equip.Type == 58) Colony += equip.Value;
        }
        public ShipB GetShipB()
        {
            return new ShipB(0, Schema, Health, Pilot, BitConverter.GetBytes(Model), ShipSide.Attack, null, false, null, ShipBMode.Battle, 255);
        }
    }
}
