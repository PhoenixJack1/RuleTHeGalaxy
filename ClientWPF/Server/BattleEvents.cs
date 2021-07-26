using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    class BattleEvents
    {
        public static void FleetFly(ServerFleet fleet)
        {
            GSTimer.AddFleetToQueue(fleet);
        }
        public static void CalcBattleResult(BattleResult result)
        {
            result.Solve();
        }
        public static void WaitBattle(ServerBattle battle)
        {
            
        }
    }
}
