using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public class ServerGameNotice
    {
        public List<byte> Array = new List<byte>();
        public int NoticeCount = 0;
        public int Element2500bpos = 0;
        void AddNotice(byte[] list)
        {
            if (Array.Count >= 5000)
            {
                Array.RemoveRange(0, Element2500bpos);
                Element2500bpos = Array.Count;
            }
            if (Element2500bpos < 2500)
                Element2500bpos += list.Length;
            Array.AddRange(list);
        }
        #region Colonize
        public void GetColonizeBattleStartNotice(ServerFleet fleet, ServerBattle battle, int planetid)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.ColonizeBattleStart);
            list.AddRange(fleet.Image);
            list.AddRange(BitConverter.GetBytes(battle.ID));
            list.AddRange(BitConverter.GetBytes(planetid));
            AddNotice(list.ToArray());
        }
        public void GetColonizeBattleWinNotice(ServerFleet fleet, int planetid)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.ColonizeBattleWin);
            list.AddRange(fleet.Image);
            list.AddRange(BitConverter.GetBytes(planetid));
            AddNotice(list.ToArray());
        }
        public void GetColonizeBattleLoseNotice(ServerFleet fleet, int planetid)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.ColonizeBattleLose);
            list.AddRange(fleet.Image);
            list.AddRange(BitConverter.GetBytes(planetid));
            AddNotice(list.ToArray());
        }
        #endregion
        #region Defense
        public void GetDefenseSelfStartNotice(ServerFleet fleet, ServerBattle battle, TargetLand land)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.DefenseSelfStart);
            list.AddRange(fleet.Image);
            list.AddRange(BitConverter.GetBytes(battle.ID));
            list.AddRange(BitConverter.GetBytes(land.GetID()));
            AddNotice(list.ToArray());
        }
        public void GetDefenseSelfWin(ServerFleet fleet, TargetLand land, Reward2 reward)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.DefenseSelfWin);
            list.AddRange(fleet.Image);
            list.AddRange(BitConverter.GetBytes(land.GetID()));
            list.AddRange(reward.Array);
            AddNotice(list.ToArray());
        }
        public void GetDefenseSelfLose(ServerFleet fleet, TargetLand land)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.DefenseSelfLose);
            list.AddRange(fleet.Image);
            list.AddRange(BitConverter.GetBytes(land.GetID()));
            AddNotice(list.ToArray());
        }
        public void GetDefenseClanStartNotice(ServerFleet fleet, ServerBattle battle, TargetLand target)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.DefenseClanStart);
            list.AddRange(fleet.Image);
            list.AddRange(BitConverter.GetBytes(battle.ID));
            list.AddRange(target.GetLandInfoArray());
            AddNotice(list.ToArray());
        }
        public void GetDefenseClanWin(ServerFleet fleet, TargetLand target, Reward2 reward)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.DefenseClanWin);
            list.AddRange(fleet.Image);
            list.AddRange(target.GetLandInfoArray());
            list.AddRange(reward.Array);
            AddNotice(list.ToArray());
        }
        public void GetDefenseClanLose(ServerFleet fleet, TargetLand target)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.DefenseClanLose);
            list.AddRange(fleet.Image);
            list.AddRange(target.GetLandInfoArray());
            AddNotice(list.ToArray());
        }
        public void GetDefenseByClanStartNotice(ServerBattle battle, TargetLand land)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.DefenseByClanStart);
            list.AddRange(BitConverter.GetBytes(battle.ID));
            list.AddRange(BitConverter.GetBytes(land.GetID()));
            AddNotice(list.ToArray());
        }
        public void GetDefenseByClanWin(TargetLand land)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.DefenseByClanWin);
            list.AddRange(BitConverter.GetBytes(land.GetID()));
            AddNotice(list.ToArray());
        }
        public void GetDefenseByClanLose(TargetLand land)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.DefenseByClanLose);
            list.AddRange(BitConverter.GetBytes(land.GetID()));
            AddNotice(list.ToArray());
        }
        #endregion
        #region Pillage
        public void GetPillageAttackStartNotice(ServerFleet fleet, ServerBattle battle, TargetLand target)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.PillageAttackStart);
            list.AddRange(fleet.Image);
            list.AddRange(BitConverter.GetBytes(battle.ID));
            list.AddRange(target.GetLandInfoArray());
            AddNotice(list.ToArray());
        }
        public void GetPillageAttackWinPillage(ServerFleet fleet, TargetLand target, Reward2 reward)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.PillageAttackWinPillage);
            list.AddRange(fleet.Image);
            list.AddRange(target.GetLandInfoArray());
            list.AddRange(reward.Array);
            AddNotice(list.ToArray());
        }
        public void GetPillageAttackWinNextNotice(ServerFleet fleet, ServerBattle battle, TargetLand target, int experience)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.PillageAttackWinNext);
            list.AddRange(fleet.Image);
            list.AddRange(BitConverter.GetBytes(battle.ID));
            list.AddRange(target.GetLandInfoArray());
            list.AddRange(BitConverter.GetBytes(experience));
            AddNotice(list.ToArray());
        }
        public void GetPillageAttackWinReturnNotice(ServerFleet fleet, TargetLand target, int experience)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.PillageAttackWinReturn);
            list.AddRange(fleet.Image);
            list.AddRange(target.GetLandInfoArray());
            list.AddRange(BitConverter.GetBytes(experience));
            AddNotice(list.ToArray());
        }
        public void GetPillageAttackLoseNotice(ServerFleet fleet, TargetLand target)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.PillageAttackLose);
            list.AddRange(fleet.Image);
            list.AddRange(target.GetLandInfoArray());
            AddNotice(list.ToArray());
        }

        public void GetPillageNoDefenseWinNotice(ServerFleet fleet, TargetLand target, Reward2 reward)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.PillageNoDefenseWin);
            list.AddRange(fleet.Image);
            list.AddRange(target.GetLandInfoArray());
            list.AddRange(reward.Array);
            AddNotice(list.ToArray());
        }
        public void GetPillageNoDefenseLoseNotice(TargetLand land, Reward2 reward)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.PillageNoDefenseLose);
            list.AddRange(BitConverter.GetBytes(land.GetID()));
            list.AddRange(reward.Array);
            AddNotice(list.ToArray());
        }
        #endregion
        #region ColonizeFree
        public void GetColonyNewLandWinNotice(ServerFleet fleet, ServerLand land)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.ColonyNewLandWin);
            list.AddRange(fleet.Image);
            list.AddRange(BitConverter.GetBytes(land.ID));
            AddNotice(list.ToArray());
        }
        public void GetColonyNewLandLoseNotice(ServerFleet fleet, ServerPlanet planet)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.ColonyNewLandLose);
            list.AddRange(fleet.Image);
            list.AddRange(BitConverter.GetBytes(planet.ID));
            AddNotice(list.ToArray());
        }
        #endregion
        #region ColonizeEnemy
        public void GetColonyAttackStartNotice(ServerFleet fleet, ServerBattle battle, TargetLand target)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.ColonyAttackStart);
            list.AddRange(fleet.Image);
            list.AddRange(BitConverter.GetBytes(battle.ID));
            list.AddRange(target.GetLandInfoArray());
            AddNotice(list.ToArray());
        }
        public void GetColonyAttackWinReturnNotice(ServerFleet fleet, TargetLand target, int experience)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.ColonyAttackWinReturn);
            list.AddRange(fleet.Image);
            list.AddRange(target.GetLandInfoArray());
            list.AddRange(BitConverter.GetBytes(experience));
            AddNotice(list.ToArray());
        }
        public void GetColonyAttackWinColonyNotice(ServerFleet fleet, ServerLand land, Reward2 reward)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.ColonyAttackWinColony);
            list.AddRange(fleet.Image);
            list.AddRange(BitConverter.GetBytes(land.ID));
            list.AddRange(reward.Array);
            AddNotice(list.ToArray());
        }
        public void GetColonyAttackWinFailNotice(ServerFleet fleet, TargetLand target, int experience)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.ColonyAttackWinFail);
            list.AddRange(fleet.Image);
            list.AddRange(target.GetLandInfoArray());
            list.AddRange(BitConverter.GetBytes(experience));
            AddNotice(list.ToArray());
        }
        public void GetColonyAttackWinFailLastNotice(ServerFleet fleet, TargetLand target, int experience)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.ColonyAttackWinLastFail);
            list.AddRange(fleet.Image);
            list.AddRange(target.GetLandInfoArray());
            list.AddRange(BitConverter.GetBytes(experience));
            AddNotice(list.ToArray());
        }
        public void GetColonyAttackWinLastNotice(ServerFleet fleet, ServerBattle battle, TargetLand target, int experience)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.ColonyAttackWinLast);
            list.AddRange(fleet.Image);
            list.AddRange(BitConverter.GetBytes(battle.ID));
            list.AddRange(target.GetLandInfoArray());
            list.AddRange(BitConverter.GetBytes(experience));
            AddNotice(list.ToArray());
        }
        public void GetColonyAttackWinNextNotice(ServerFleet fleet, ServerBattle battle, TargetLand target, int experience)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.ColonyAttackWinNext);
            list.AddRange(fleet.Image);
            list.AddRange(BitConverter.GetBytes(battle.ID));
            list.AddRange(target.GetLandInfoArray());
            list.AddRange(BitConverter.GetBytes(experience));
            AddNotice(list.ToArray());
        }
        public void GetColonyAttackWinUpriseNotice(ServerFleet fleet, TargetLand target, int experience)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.ColonyAttackWinUprise);
            list.AddRange(fleet.Image);
            list.AddRange(target.GetLandInfoArray());
            list.AddRange(BitConverter.GetBytes(experience));
            AddNotice(list.ToArray());
        }
        public void GetColonyAttackWinNoUpriseNotice(ServerFleet fleet, ServerLand target, int experience)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.ColonyAttackWinNoUprise);
            list.AddRange(fleet.Image);
            list.AddRange(target.GetLandInfoArray());
            list.AddRange(BitConverter.GetBytes(experience));
            AddNotice(list.ToArray());
        }
        public void GetColonyAttackLoseNotice(ServerFleet fleet, TargetLand target)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.ColonyAttackLose);
            list.AddRange(fleet.Image);
            list.AddRange(target.GetLandInfoArray());
            AddNotice(list.ToArray());
        }
        public void GetColonyLastAttackFailStartNotice(ServerFleet fleet, TargetLand target)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.ColonyLastAttackFailStart);
            list.AddRange(fleet.Image);
            list.AddRange(target.GetLandInfoArray());
            AddNotice(list.ToArray());
        }
        public void GetColonyLastAttackStartNotice(ServerFleet fleet, ServerBattle battle, TargetLand target)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.ColonyLastAttackStart);
            list.AddRange(fleet.Image);
            list.AddRange(BitConverter.GetBytes(battle.ID));
            list.AddRange(target.GetLandInfoArray());
            AddNotice(list.ToArray());
        }
        public void GetColonyLastAttackWinColonyNotice(ServerFleet fleet, TargetLand land, Reward2 reward)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.ColonyLastAttackWinColony);
            list.AddRange(fleet.Image);
            list.AddRange(BitConverter.GetBytes(land.GetID()));
            list.AddRange(reward.Array);
            AddNotice(list.ToArray());
        }
        public void GetColonyLastAttackWinFailNotice(ServerFleet fleet, TargetLand target, int experience)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.ColonyLastAttackWinFail);
            list.AddRange(fleet.Image);
            list.AddRange(target.GetLandInfoArray());
            list.AddRange(BitConverter.GetBytes(experience));
            AddNotice(list.ToArray());
        }
        public void GetColonyLastAttackWinUpriseNotice(ServerFleet fleet, TargetLand target, int experience)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.ColonyLastAttackWinUprise);
            list.AddRange(fleet.Image);
            list.AddRange(target.GetLandInfoArray());
            list.AddRange(BitConverter.GetBytes(experience));
            AddNotice(list.ToArray());
        }
        public void GetColonyLastAttackWinNoUpriseNotice(ServerFleet fleet, TargetLand target, int experience)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.ColonyLastAttackWinNoUprise);
            list.AddRange(fleet.Image);
            list.AddRange(target.GetLandInfoArray());
            list.AddRange(BitConverter.GetBytes(experience));
            AddNotice(fleet.Image);
        }
        public void GetColonyLastAttackLoseNotice(ServerFleet fleet, TargetLand target)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.ColonyLastAttackLose);
            list.AddRange(fleet.Image);
            list.AddRange(target.GetLandInfoArray());
            AddNotice(list.ToArray());
        }
        public void GetColonyNoDefenseColonyWinNotice(ServerFleet fleet, TargetLand land, Reward2 reward)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.ColonyNoDefenseColonyWin);
            list.AddRange(fleet.Image);
            list.AddRange(BitConverter.GetBytes(land.GetID()));
            list.AddRange(reward.Array);
            AddNotice(list.ToArray());
        }
        public void GetColonyNoDefenseFailNotice(ServerFleet fleet, TargetLand target)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.ColonyNoDefenseFail);
            list.AddRange(fleet.Image);
            list.AddRange(target.GetLandInfoArray());
            AddNotice(list.ToArray());
        }
        public void GetColonyNoDefenseColonyLoseNotice(TargetLand target, Reward2 reward)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.ColonyNoDefenseColonyLose);
            list.AddRange(target.GetLandInfoArray());
            list.AddRange(reward.Array);
            AddNotice(list.ToArray());
        }
        public void GetColonyNoDefenseUpriseWinNotice(ServerFleet fleet, TargetLand target)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.ColonyNoDefenseUpriseWin);
            list.AddRange(fleet.Image);
            list.AddRange(target.GetLandInfoArray());
            AddNotice(list.ToArray());
        }
        public void GetColonyNoDefenseUpriseLoseNotice(TargetLand land)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.ColonyNoDefenseUpriseLose);
            list.AddRange(BitConverter.GetBytes(land.GetID()));
            AddNotice(list.ToArray());
        }
        public void GetColonyNoDefenseNoUpriseNotice(ServerFleet fleet, TargetLand land)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.ColonyNoDefenseNoUprise);
            list.AddRange(fleet.Image);
            list.AddRange(land.GetLandInfoArray());
            AddNotice(list.ToArray());
        }
        public void GetColonyNewLoseNotice(TargetLand target)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.ColonyNewLose);
            list.AddRange(target.GetLandInfoArray());
            AddNotice(list.ToArray());
        }
        public void GetColonyNewDestroydNotice(ServerFleet fleet, TargetLand target)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.ColonyNewDestroyed);
            list.AddRange(fleet.Image);
            list.AddRange(target.GetLandInfoArray());
            AddNotice(list.ToArray());
        }
        public void GetLastDefenseStartNotice(ServerFleet fleet, ServerBattle battle, TargetLand land)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.LastDefenseStart);
            list.AddRange(fleet.Image);
            list.AddRange(BitConverter.GetBytes(battle.ID));
            list.AddRange(BitConverter.GetBytes(land.GetID()));
            AddNotice(list.ToArray());
        }
        public void GetLastDefenseWinNotice(ServerFleet fleet, TargetLand land, Reward2 reward)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.LastDefenseWin);
            list.AddRange(fleet.Image);
            list.AddRange(BitConverter.GetBytes(land.GetID()));
            list.AddRange(reward.Array);
            AddNotice(list.ToArray());
        }
        public void GetLastDefenseLoseColonyNotice(ServerFleet fleet, TargetLand target, Reward2 reward)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.LastDefenseLoseColony);
            list.AddRange(fleet.Image);
            list.AddRange(target.GetLandInfoArray());
            list.AddRange(reward.Array);
            AddNotice(list.ToArray());
        }
        public void GetLastDefenseLoseNoColonyNotice(ServerFleet fleet, TargetLand land)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.LastDefenseLoseNoColony);
            list.AddRange(fleet.Image);
            list.AddRange(BitConverter.GetBytes(land.GetID()));
            AddNotice(list.ToArray());
        }
        public void GetLastDefenseLoseUpriseNotice(ServerFleet fleet, TargetLand land)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.LastDefenseLoseUprise);
            list.AddRange(fleet.Image);
            list.AddRange(BitConverter.GetBytes(land.GetID()));
            AddNotice(list.ToArray());
        }
        public void GetLastDefenseLoseNoUpriseNotice(ServerFleet fleet, TargetLand land)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.LastDefenseLoseNoUprise);
            list.AddRange(fleet.Image);
            list.AddRange(BitConverter.GetBytes(land.GetID()));
            AddNotice(list.ToArray());
        }
        #endregion
        #region Resources
        public void GetPirateAttackNotice(ServerFleet fleet, ServerBattle battle)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.PirateBattleStart);
            list.AddRange(fleet.Image);
            list.AddRange(BitConverter.GetBytes(battle.ID));
            AddNotice(list.ToArray());
        }
        public void GetPirateWinNotice(ServerFleet fleet, Reward2 reward)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.PirateBattleWin);
            list.AddRange(fleet.Image);
            list.AddRange(reward.Array);
            AddNotice(list.ToArray());
        }
        public void GetPirateLoseNotice(ServerFleet fleet)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.PirateBattleLose);
            list.AddRange(fleet.Image);
            AddNotice(list.ToArray());
        }
        public void GetGreenTeamBattleStartNotice(ServerFleet fleet, ServerBattle battle)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.GreenTeamBattleStart);
            list.AddRange(fleet.Image);
            list.AddRange(BitConverter.GetBytes(battle.ID));
            AddNotice(list.ToArray());
        }
        public void GetGreenTeamBattleWinNotice(ServerFleet fleet, Reward2 reward)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.GreenTeamBattleWin);
            list.AddRange(fleet.Image);
            list.AddRange(reward.Array);
            AddNotice(list.ToArray());
        }
        public void GetGreenTeamBattleLoseNotice(ServerFleet fleet)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.GreenTeamBattleLose);
            list.AddRange(fleet.Image);
            AddNotice(list.ToArray());
        }
        public void GetAlienBattleStartNotice(ServerFleet fleet, ServerBattle battle)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.AlienBattleStart);
            list.AddRange(fleet.Image);
            list.AddRange(BitConverter.GetBytes(battle.ID));
            AddNotice(list.ToArray());
        }
        public void GetAlienBattleWinNotice(ServerFleet fleet, Reward2 reward)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.AlienBattleWin);
            list.AddRange(fleet.Image);
            list.AddRange(reward.Array);
            AddNotice(list.ToArray());
        }
        public void GetAlienLoseNotice(ServerFleet fleet)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.AlienBattleLose);
            list.AddRange(fleet.Image);
            AddNotice(list.ToArray());
        }
        public void GetTechnoTeamBattleStartNotice(ServerFleet fleet, ServerBattle battle)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.TechnoTeamBattleStart);
            list.AddRange(fleet.Image);
            list.AddRange(BitConverter.GetBytes(battle.ID));
            AddNotice(list.ToArray());
        }
        public void GetTechnoBattleWinNotice(ServerFleet fleet, Reward2 reward)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.TechnoTeamBattleWin);
            list.AddRange(fleet.Image);
            list.AddRange(reward.Array);
            AddNotice(list.ToArray());
        }
        public void GetTechnoBattleLoseNotice(ServerFleet fleet)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.TechnoTeamBattleLose);
            list.AddRange(fleet.Image);
            AddNotice(list.ToArray());
        }
        public void GetMercBattleStartNotice(ServerFleet fleet, ServerBattle battle)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.MercBattleStart);
            list.AddRange(fleet.Image);
            list.AddRange(BitConverter.GetBytes(battle.ID));
            AddNotice(list.ToArray());
        }
        public void GetMercBattleWinNotice(ServerFleet fleet, Reward2 reward)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.MercBattleWin);
            list.AddRange(fleet.Image);
            list.AddRange(reward.Array);
            AddNotice(list.ToArray());
        }
        public void GetMercBattleLoseNotice(ServerFleet fleet)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.MercBattleLose);
            list.AddRange(fleet.Image);
            AddNotice(list.ToArray());
        }
        #endregion
        #region StoryLine
        public void GetStoryLineStartNotice(ServerFleet fleet, ServerBattle battle, byte storyID)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.StoryLineStart);
            list.AddRange(fleet.Image);
            list.AddRange(BitConverter.GetBytes(battle.ID));
            list.Add(storyID);
            AddNotice(list.ToArray());
        }
        public void GetStoryLineWinNotice(ServerFleet fleet, Reward2 reward, byte storyID)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.StoryLineWin);
            list.AddRange(fleet.Image);
            list.Add(storyID);
            list.AddRange(reward.Array);
            AddNotice(list.ToArray());
        }
        public void GetStoryLineLoseNotice(ServerFleet fleet, byte storyID)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.StoryLineLose);
            list.AddRange(fleet.Image);
            list.Add(storyID);
            AddNotice(list.ToArray());
        }
       
        #endregion
        #region Others
        public void GetTargetColonyOwnerChanged(ServerFleet fleet, TargetLand target)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.TargetColonyOwnerChanged);
            list.AddRange(fleet.Image);
            list.AddRange(target.GetLandInfoArray());
            AddNotice(list.ToArray());
        }
        public void GetLearnNewScience(ushort scienceid)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.LearnNewScience);
            list.AddRange(BitConverter.GetBytes(scienceid));
            AddNotice(list.ToArray());
        }
        public void GetAddPremiumCurrency(int value)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.AddPremiumCurrency);
            list.AddRange(BitConverter.GetBytes(value));
            AddNotice(list.ToArray());
        }
        public void GetActivatePremium(int days)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.ActivatePremium);
            list.AddRange(BitConverter.GetBytes(days));
            AddNotice(list.ToArray());
        }
        public void GetQuickStartPremium(int sciencecount)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.AddPremiumQuickStart);
            list.AddRange(BitConverter.GetBytes(sciencecount));
            AddNotice(list.ToArray());
        }
        public void GetOrionPremium(int landid)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.AddPremiumOrion);
            list.AddRange(BitConverter.GetBytes(landid));
            AddNotice(list.ToArray());
        }
        public void GetLoseLandByEnemyNotice(ServerLand land)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.LoseLandByEnemy);
            list.AddRange(land.GetLandInfoArray());
            AddNotice(list.ToArray());
        }
        public void GetRecieceRewardNotice(Reward2 reward)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.RecieceReward);
            list.AddRange(reward.Array);
            AddNotice(list.ToArray());
        }
        public void GetRecieveRewardMissionNotice(Reward2 reward, Mission2Type type)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
            list.Add((byte)ENotice.RecieveMissionReward);
            list.Add((byte)type);
            list.AddRange(reward.Array);
            AddNotice(list.ToArray());
        }
        #endregion
    }
}
