using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public enum BattleMode { Mode1, Mode2} //Режим 1 - планирование ходов, режим 2 - мгновенный ход
    public enum Mode2BattleStatus { AttackMove, DefenseMove, WaitAttack, WaitDefense}
    public enum Restriction { None, NoEnergy, NoPhysic, NoIrregular, NoCyber, DoubleEnergy, DoublePhysic, DoubleIrregular, DoubleCyber, LowAccuracy, DoubleAccuracy}
    /// <summary> интерфейс вызывает методы, необходимые для создания боя (поле боя, корабли) </summary>
    public interface ServerBattleSide2
    {
        BattleFieldGroup GetBattleField();
        Restriction GetRestriction();
        SideBattleParam GetSide1BattleParams(GSPlayer player, Restriction resctriction);
        SideBattleParam GetSide2BattleParams(GSPlayer player, Restriction resctriction);
    }
    public class ServerBattle
    {
        public long ID;
        public ServerSide Side1;
        public ServerSide Side2;
        public byte[] StartArray;
        public byte CurrentRound;
        public short CurrentTurn = -1;
        //public int CurrentTurn;
        public byte[] Description;
        public byte[] BasicInfo;
        public Movesclass Moves = new Movesclass();
        //массив занятых хексов на поле боя. Ключ - номер хекса, значение - объект
        public SortedList<byte, ServerShipB> BattleField = new SortedList<byte, ServerShipB>();
        public DateTime TurnEndTime;
        public bool IsFinished;
        public BattleType BattleType;
        public List<ServerShipB> DestroyedShips = new List<ServerShipB>();
        public BattleField Field;
        public AI AI;
        public SortedList<byte, ServerShipB> Asteroids;
        public List<ServerShipB> BigShips = new List<ServerShipB>();
        public Restriction Restriction;
        public int RestrictionLength;
        //public ServerShipB ProtectedShip;
        public int TurnTime;
        public BattleMode CurMode = BattleMode.Mode2;
        public int randomgroup = ServerLinks.BattleRandom.Next(20);
        public ServerBattle(ServerFleet fleet1, ServerBattleSide2 SideInterface)
        {
            BattleFieldGroup battlefieldgroup = SideInterface.GetBattleField();
            TurnTime = 5;
            ID = BattleID.GetID();
            Restriction = SideInterface.GetRestriction();
            if (Restriction != Restriction.None) RestrictionLength = Int32.MaxValue; else RestrictionLength = 0;
            Field = battlefieldgroup.Field;
            Asteroids = battlefieldgroup.Asteroids;
            AI = new AI(this);
            ServerSide side1 = new ServerSide(fleet1, SideInterface.GetSide1BattleParams(fleet1.Target.SelfPlayer, Restriction), ShipSide.Attack, this);
            ServerSide side2 = new ServerSide(SideInterface.GetSide2BattleParams(fleet1.Target.SelfPlayer, Restriction), ShipSide.Defense, this);
            if (side1.Fleet != null) side1.Fleet.Target.Order = FleetOrder.InBattle;
            if (side2.Fleet != null) side2.Fleet.Target.Order = FleetOrder.InBattle;
            Side1 = side1;
            if (side1.IsAutoControl) Moves.SetSide1Auto();
            Side2 = side2;
            Side1.CreateArray();
            Side2.CreateArray();
            if (side2.IsAutoControl) Moves.SetSide2Auto();
            ServerSide[] Sides = new ServerSide[] { Side1, Side2 };
            foreach (ServerSide side in Sides)
                foreach (ServerShipB ship in side.Ships.Values)
                {
                    if (Asteroids.ContainsKey(ship.Hex))
                        Asteroids.Remove(ship.Hex);
                    if (ship.States.BigSize == true)
                        foreach (Hex hex in Field.Hexes[ship.Hex].NearHexes)
                            if (Asteroids.ContainsKey(hex.ID))
                                Asteroids.Remove(hex.ID);
                }

            ServerLinks.Battles.Add(ID, this);
            List<byte> descr = new List<byte>();
            descr.Add((byte)CurMode);
            descr.Add((byte)Field.Size);
            descr.Add((byte)Asteroids.Count);
            foreach (ServerShipB asteroid in Asteroids.Values)
            {
                descr.Add(asteroid.Hex);
                descr.Add((byte)asteroid.HelpID);
                if (asteroid.States.BigSize == true)
                    descr.Add(1);
                else
                    descr.Add(0);
            }

            { descr.Add(1); descr.Add((byte)(DateTime.Now.Second % 6 + 1)); }
            descr.AddRange(ServerLinks.RandomArrays[randomgroup]);
            List<byte> descr2 = new List<byte>();
            descr2.AddRange(BitConverter.GetBytes(descr.Count));
            descr2.AddRange(descr);
            Description = descr2.ToArray();
            CreateBasicInfoAndStartArray();
            CurrentRound = 0;
            CurrentTurn++;
            DebugWindow.AddTB2("Ход " + CurrentTurn.ToString() + " ", true);
            DebugWindow.AddTB2("Start Battle\n", false);
            StartBattle();
           
            Moves.AddMove(new GameMove(0, 255, 255, 255));
            CurrentRound = 1;

            if (CurMode == BattleMode.Mode1)
            {
                RoundStart();
                RoundNext();
            }
            else if (CurMode == BattleMode.Mode2)
            {
                RoundNextMode2();
            }
        }
       
        public ServerBattle(ServerFleet fleet1, bool isAuto1, ServerFleet fleet2, bool isAuto2, BattleType type, BattleFieldGroup battlefieldgroup)
        {
            TurnTime = 5;
            ID = BattleID.GetID();
            Restriction = Restriction.None;
            RestrictionLength = 0;
            Field = battlefieldgroup.Field;
            Asteroids = battlefieldgroup.Asteroids;
            AI = new AI(this);
            ServerSide side1 = new ServerSide(fleet1, null, ShipSide.Attack, this); side1.IsAutoControl = isAuto1;
            ServerSide side2 = new ServerSide(fleet2, null, ShipSide.Defense, this);side2.IsAutoControl = isAuto2;
            side1.Fleet.Target.Order = FleetOrder.InBattle;
            side2.Fleet.Target.Order = FleetOrder.InBattle;
            Side1 = side1;
            if (side1.IsAutoControl) Moves.SetSide1Auto();
            Side2 = side2;
            Side1.CreateArray();
            Side2.CreateArray();
            if (side2.IsAutoControl) Moves.SetSide2Auto();

            foreach (ServerShipB ship in Side1.Ships.Values)
                if (Asteroids.ContainsKey(ship.Hex))
                    Asteroids.Remove(ship.Hex);
            foreach (ServerShipB ship in Side2.Ships.Values)
                if (Asteroids.ContainsKey(ship.Hex))
                    Asteroids.Remove(ship.Hex);

            ServerLinks.Battles.Add(ID, this);
            List<byte> descr = new List<byte>();
            descr.Add((byte)CurMode);
            descr.Add((byte)Field.Size);
            descr.Add((byte)Asteroids.Count);
            foreach (ServerShipB asteroid in Asteroids.Values)
            {
                descr.Add(asteroid.Hex);
                descr.Add((byte)asteroid.HelpID);
                if (asteroid.States.BigSize == true)
                    descr.Add(1);
                else
                    descr.Add(0);
            }

            { descr.Add(1); descr.Add((byte)(DateTime.Now.Second % 6 + 1)); }
            descr.AddRange(ServerLinks.RandomArrays[randomgroup]);
            List<byte> descr2 = new List<byte>();
            descr2.AddRange(BitConverter.GetBytes(descr.Count));
            descr2.AddRange(descr);
            Description = descr2.ToArray();
            CreateBasicInfoAndStartArray();
            CurrentRound = 0;
            CurrentTurn++;
            DebugWindow.AddTB2("Ход " + CurrentTurn.ToString() + " ", true);
            DebugWindow.AddTB2("Start Battle\n", false);
            StartBattle();

            Moves.AddMove(new GameMove(0, 255, 255, 255));
            CurrentRound = 1;

            if (CurMode == BattleMode.Mode1)
            {
                RoundStart();
                RoundNext();
            }
            else if (CurMode == BattleMode.Mode2)
            {
                RoundNextMode2();
            }
        }
        Mode2BattleStatus Mode2Status = Mode2BattleStatus.AttackMove;
        public void RoundNextMode2() //метод расчитывает раунд и определяет его результат для режима 2 - без планирования
        {
            byte RoundResult = 2; //флаг, показывающий победителя. 0 - победа атаки. 1 - победа защиты, 2 - ничья, 3 - бой не закончен
            for (;;)//начало цикла
            {
                RoundResult = 2; 
                if (Mode2Status == Mode2BattleStatus.AttackMove) //ход атакующей стороны
                {
                    bool Side1EndMove = Side1.Mode2RoundStart(); //выполнение ходов. если возвращается true, то ход завершён, если false - то ход не завершён
                    if (Side1EndMove == true) //если ход завершён то передаём ход защищающейся стороне
                        Mode2Status = Mode2BattleStatus.DefenseMove;
                    else //если ход атакующей стороны не заверщён то входим в режим ожидания
                    {
                        Mode2Status = Mode2BattleStatus.WaitAttack;
                        TurnEndTime = DateTime.Now + TimeSpan.FromMinutes(TurnTime);
                        return;
                    }
                }
                else if (Mode2Status==Mode2BattleStatus.DefenseMove) //если ход защищающейся стороны
                {
                    bool Side2EndMove = Side2.Mode2RoundStart(); //выполнение хода защищающейся стороны
                    if (Side2EndMove == true) //если ход защитника заверщён
                    {
                        RoundResult = Mode2CheckEnd(); //проверяем, выполнены ли условия для окончания боя
                        if (RoundResult != 3) break; //если 3 - то не выполнены, если меньше трёх - то выполнены
                        if (CurrentRound == 50) break; //если раунд 50-ый то всё равно конец
                        CurrentRound++; //новый раунд
                        Mode2Status = Mode2BattleStatus.AttackMove; //передаём ход атаке
                    }
                    else //если ход защитника не заверщён то входим в режим ожидания
                    {
                        Mode2Status = Mode2BattleStatus.WaitDefense;
                        TurnEndTime = DateTime.Now + TimeSpan.FromMinutes(TurnTime);
                        return;
                    }
                }
                else if (Mode2Status==Mode2BattleStatus.WaitAttack) //если мы ожидаем ходов атаки, то выполняем ходы атаки
                {
                    Side1.Mode2RoundContinue();
                    Mode2Status = Mode2BattleStatus.DefenseMove;
                }
                else if (Mode2Status==Mode2BattleStatus.WaitDefense) //если мы ожидаем ходов защиты, то выполняем ходы защиты
                {
                    Side2.Mode2RoundContinue();

                    RoundResult = Mode2CheckEnd(); //проверяем, выполнены ли условия для окончания боя
                    if (RoundResult != 3) break; //если 3 - то не выполнены, если меньше трёх - то выполнены
                    if (CurrentRound == 50) break; //если раунд 50-ый то всё равно конец
                    CurrentRound++; //новый раунд
                    Mode2Status = Mode2BattleStatus.AttackMove; //передаём ход атаке
                }
            }
            if (RoundResult == 3)
                Moves.AddMove(new GameMove(50, 2, 255, 255));
            else
                Moves.AddMove(new GameMove(50, RoundResult, 255, 255));
            IsFinished = true;
           // ServerTestBattle.AddBattleEnd(ID, (short)Side1.Shoots, (short)Side2.Shoots, Side1.Rating, Side2.Rating,
            //    (double)Side2.Shoots * Side2.Rating / Side1.Shoots / Side1.Rating, (double)Side1.Shoots * Side1.Rating / Side2.Shoots / Side2.Rating);
            if (RoundResult == 0) CalcBattleFinish(Side1);
            else if (RoundResult == 1) CalcBattleFinish(Side2);
            else CalcBattleFinish(null);
        }
        public byte Mode2CheckEnd()
        {
            if (RestrictionLength > 0)
                RestrictionLength--;
            if (RestrictionLength == 0)
                Restriction = Restriction.None;
            GameMove[] move = Side1.TryLoseBattle();
            if (move != null)
                foreach (GameMove m in move)
                    Moves.AddMove(m);
            move = Side2.TryLoseBattle();
            if (move != null)
                foreach (GameMove m in move)
                    Moves.AddMove(m);
            if (Side1.CheckProtectedLose() == true) return 1; //Победа стороны 2 из-за потери стороной 1 всех защищаемых кораблей
            else if (Side2.CheckProtectedLose() == true) return 0; //Победа Стороны 1 из-за потери стороной 2 всех защищаемых кораблей
            else if (Side1.OnField.Count > 0 && Side2.OnField.Count > 0) return 3;//Бой незакончен
            else if (Side1.OnField.Count == 0 && Side2.OnField.Count > 0) return 1; //победа Сторона 2
            else if (Side2.OnField.Count == 0 && Side1.OnField.Count > 0) return 0; //победа Сторона 1
            return 3;
        }
        public void RoundNext() //метод расчитывает раунд и определяет его результат
        {
            bool TimeForThinkingIsEnd = true; // флаг, определяющий что закончено время на раздумывание
            byte RoundResult = 2; //флаг, показывающий победителя. 0 - победа атаки. 1 - победа защиты, 2 - ничья, 3 - бой не закончен
            for (;;) //начало цикла
            {
                if (!TimeForThinkingIsEnd) //расчёты если время на раздумья не закончилось
                    if ((Side1.IsAutoControl || !Side1.ShipsCanMove()) && (Side2.IsAutoControl || !Side2.ShipsCanMove())) //расчёт раунда автоматический, если ни одна из сторон не нуждается во времени на раздумья
                    {
                        //Console.WriteLine("Расчёт раунда " + CurrentRound + " в связи с автобоем");
                        RoundResult = RoundEnd(); //расчёт раунда
                    }
                    else //если сторонам нужно время на раздумья
                    {
                        //Console.WriteLine("Бой перенесён в очередь");
                        TurnEndTime = DateTime.Now + TimeSpan.FromMinutes(TurnTime); //расположение боя в очереди на ожидание
                        BattleEvents.WaitBattle(this);
                        return; //выход из метода, пока его не вызовет контроллер
                    }
                else //если время на раздумье закончилось
                {
                    //Console.WriteLine("Расчёт раунда " + CurrentRound);
                    RoundResult = RoundEnd(); //расчёт раунда
                }
                /*if (ProtectedShip != null && ProtectedShip.Params.Health.GetCurrent == 0) //если защищаемый корабль уничтожен
                {
                    if (ProtectedShip.States.Side == ShipSide.Attack)
                    { RoundResult = 1; break; }
                    else
                    { RoundResult = 0; break; }
                }*/
                if (RoundResult != 3) break; //если бой завершился то выход из цикла на конец боя
                if (CurrentRound == 50) break; //если раунд пятидесятый, то выход их цикла на конец боя

                CurrentRound++; //следующий раунд
                RoundStart();
                TimeForThinkingIsEnd = false;
            } // конец цикла
            if (RoundResult == 3)
                Moves.AddMove(new GameMove(50, 2, 255, 255));
            else
                Moves.AddMove(new GameMove(50, RoundResult, 255, 255));
            IsFinished = true;
           // ServerTestBattle.AddBattleEnd(ID, (short)Side1.Shoots, (short)Side2.Shoots, Side1.Rating, Side2.Rating,
           //     (double)Side2.Shoots * Side2.Rating / Side1.Shoots / Side1.Rating, (double)Side1.Shoots * Side1.Rating / Side2.Shoots / Side2.Rating);
            if (RoundResult == 0) CalcBattleFinish(Side1);
            else if (RoundResult == 1) CalcBattleFinish(Side2);
            else CalcBattleFinish(null);


        }
        void RoundStart()
        {
            CurrentTurn++;
            //ServerTestBattle.AddRoundStart(ID, CurrentTurn, CurrentRound);

            Moves.AddMove(new GameMove(1, CurrentRound, 255, 255)); //начало раунда
            //Увеличить статы
            //Подвинуть корабли в очереди на запуск
            Side1.RoundStart();
            Side2.RoundStart();
            //Поставить новые корабли в очередь на запуск
            Moves.AddMove(3, Side1.MoveShipToPort(), 255, 255);
            Moves.AddMove(103, Side2.MoveShipToPort(), 255, 255);
        }
        byte RoundEnd()
        {
            Side1.IsHaveCommands = false;
            Side2.IsHaveCommands = false;
            //Сгенерировать ходы если необходимо
            if (Side1.CurrentCommand == null)
                AI.CreateMoveList(Side1, Side2, Asteroids);
            if (Side2.CurrentCommand == null)
                AI.CreateMoveList(Side2, Side1, Asteroids);
            //Выполнить ходы
            bool Side1MoveResult = true;
            bool Side2MoveResult = true;
            for (;;)
            {
                if (Side1MoveResult) Side1MoveResult = Side1.MakeMove() == "";
                if (Side2MoveResult) Side2MoveResult = Side2.MakeMove() == "";
                if (!Side1MoveResult && !Side2MoveResult) break; //если метод MakeMove выдал false по обеим сторонам, то раунд завершён
            }
            GameMove[] side1SelfDestroy = Side1.TryLoseBattle();
            GameMove[] side2SelfDestroy = Side2.TryLoseBattle();
            if (side1SelfDestroy != null) foreach (GameMove m in side1SelfDestroy) Moves.AddMove(m);
            if (side2SelfDestroy != null) foreach (GameMove m in side2SelfDestroy) Moves.AddMove(m);
            Moves.AddMove(new GameMove(2, CurrentRound, 255, 255)); //конец раунда
            CurrentTurn++;
           // ServerTestBattle.AddRoundEnd(ID, CurrentTurn, CurrentRound);
            GameMove[] endmoves = Side1.RoundEnd();
            foreach (GameMove move in endmoves)
                Moves.AddMove(move);
            endmoves = Side2.RoundEnd();
            foreach (GameMove move in endmoves)
                Moves.AddMove(move);
            //Проверить поражение
            Side1.CurrentCommand = null;
            Side2.CurrentCommand = null;
            if (Side1.CheckProtectedLose() == true) return 1; //Победа стороны 2 из-за потери стороной 1 всех защищаемых кораблей
            else if (Side2.CheckProtectedLose() == true) return 0; //Победа Стороны 1 из-за потери стороной 2 всех защищаемых кораблей
            else if (Side1.OnField.Count > 0 && Side2.OnField.Count > 0) return 3;//Бой незакончен
            else if (Side1.OnField.Count == 0 && Side2.OnField.Count > 0) return 1; //победа Сторона 2
            else if (Side2.OnField.Count == 0 && Side1.OnField.Count > 0) return 0; //победа Сторона 1
            else return 2; //Ничья
        }
        void StartBattle()
        {

            Side1.StartBattle();
            Side1.Enemy = Side2;
            Side2.StartBattle();
            Side2.Enemy = Side1;

            foreach (ServerShipB asteroid in Asteroids.Values)
            {
                BattleField.Add(asteroid.Hex, asteroid);
            }
        }
        void CreateBasicInfoAndStartArray()
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(Side1.Fleet.ID));
            list.Add(Side1.Behavior);
            list.AddRange(Side1.Array);
            if (Side2.IsReal)
                list.AddRange(BitConverter.GetBytes(Side2.Fleet.ID));
            else
                list.AddRange(BitConverter.GetBytes((long)-1));
            list.Add(Side2.Behavior);
            list.AddRange(Side2.Array);
            list.Add((byte)Restriction);
            list.AddRange(BitConverter.GetBytes(RestrictionLength));
            StartArray = list.ToArray();
            List<byte> array = new List<byte>();
            array.AddRange(BitConverter.GetBytes(ID));
            array.Add((byte)BattleType);
            array.AddRange(BitConverter.GetBytes(Side1.Fleet.ID));
            array.AddRange(Side1.Emblem);
            if (Side2.IsReal)
                array.AddRange(BitConverter.GetBytes(Side2.Fleet.ID));
            else
                array.AddRange(BitConverter.GetBytes((long)-1));
            array.AddRange(Side2.Emblem);
            array.AddRange(Description);
            BasicInfo = array.ToArray();
        }
        public static ServerBattle CreateStoryLineMissionBattle(ServerFleet fleet1, StoryLine2 story)
        {
            //EnemyType enemy = EnemyType.Pirates;
            //ServerSide side1 = new ServerSide(fleet1, ShipSide.Attack, null);
            //ServerSide side2 = new ServerSide(story.Side2BattleParam, ShipSide.Defense);
            //ServerFleet fleet2 = story.GetFleet();
            //BattleFieldGroup field = story.BattleFieldParam.GetGroup();
            ServerBattle battle = new ServerBattle(fleet1, story);
            //ServerBattle battle = new ServerBattle(fleet1, false, fleet2, true, story.BattleType, field);
            fleet1.CurBattle = battle;
            return battle;
        }
       /* public static ServerBattle CreateColonizeNewPlanetBattle(ServerFleet fleet1, ServerPlanet planet)
        {
            FinalMissionParam fmp = new FinalMissionParam();
            fmp.SetAsteroids(0, new byte[0], new byte[0], 3);
            ServerFleet fleet2 = Enemy.CalcPirateDefenseFleet(0, 6, 2, planet.StarID);
            fleet2.CustomParams = new FleetCustomParams(1000);
            BattleFieldGroup group = new BattleFieldGroup(Client.BattleField.Fields[fmp.FieldSize], Client.BattleField.GetAsteroids(fmp.Asteroids), new byte[2]);
            ServerBattle battle = new ServerBattle(fleet1, false, fleet2, true, BattleType.Planet, group);
            fleet1.CurBattle = battle;
            return battle;
        }*/
        /*public static ServerBattle CreateResourceMissionBattle(ServerFleet fleet1, Mission mission, out EnemyType enemy)
        {
            FinalMissionParam fmp = new FinalMissionParam();
            //EnemyType enemy = EnemyType.Pirates;
            byte custom = 0;
            byte level = (byte)((mission.MinLevel + mission.MaxLevel) / 20 * 10 + 5);

            switch (mission.Type)
            {
                case MissionType.AlienBases:
                    fmp.SetAsteroids(1, new byte[0], new byte[] { 43, 44, 45, 55, 56, 57, 68 }, 4);
                    custom = 1;
                    break;
                case MissionType.AlienBounds: fmp.SetAsteroids(0, new byte[] { 22, 28 }, new byte[0], 1); break;
                case MissionType.BigCompetition: fmp.SetAsteroids(1, new byte[] { 20, 21, 27, 28, 55, 56, 62, 63 }, new byte[0], 0); break;
                case MissionType.ArtifactSearch:
                    fmp.SetAsteroids(2, new byte[0], new byte[] { 61, 62, 63, 77, 78, 79, 94, 105, 106, 107, 121, 122, 123, 138 }, 8);
                    custom = 2;
                    break;
                case MissionType.Competition: fmp.SetAsteroids(0, new byte[] { 12, 18, 21, 27 }, new byte[0], 0); break;
                case MissionType.ConvoyDefense:
                    fmp.SetAsteroids(1, new byte[0], new byte[0], 4);
                    custom = 3;
                    break;
                case MissionType.ConvoyDestroy:
                    fmp.SetAsteroids(0, new byte[0], new byte[0], 4);
                    custom = 4;
                    break;
                case MissionType.LongRangeRaid: fmp.SetAsteroids(1, new byte[0], new byte[0], 7); break;
                case MissionType.MetheoritRaid: fmp.SetAsteroids(0, new byte[] { 19, 20 }, new byte[0], 0); break;
                case MissionType.OreBeltRaid: fmp.SetAsteroids(1, new byte[0], new byte[0], 9); break;
                case MissionType.PirateBase:
                    fmp.SetAsteroids(2, new byte[] { 94, 123 }, new byte[] { 91, 92, 93, 107, 108, 109, 124 }, 6);
                    custom = 6;
                    break;
                case MissionType.PirateShipyard:
                    fmp.SetAsteroids(1, new byte[] { 46, 56, 67 }, new byte[0], 2);
                    custom = 5;
                    break;
                case MissionType.ScienceExpedition:
                    fmp.SetAsteroids(2, new byte[0], new byte[] { }, 9);

                    break;
            }
            ServerFleet fleet2 = Enemy.CalcResourceMissionFleet(mission, out enemy);
            fleet2.CustomParams.CustomID = custom;
            fleet2.CustomParams.CustomLevel = level;
            BattleFieldGroup group = new BattleFieldGroup(Client.BattleField.Fields[fmp.FieldSize], Client.BattleField.GetAsteroids(fmp.Asteroids), new byte[2]);
            ServerBattle battle = new ServerBattle(fleet1, false, fleet2, true, BattleType.Space, group);
            battle.Mission = mission;
            fleet1.CurBattle = battle;
            return battle;
        }
        */
        void CalcBattleFinish(ServerSide winner)
        {
            winner = Side1;
            if (Side1.Fleet!=null && Side1.Fleet.Target!=null && Side1.Fleet.Target.Destroy == true) { Side1.Fleet.FleetDestroy(); Side1.Fleet = null; }
            if (Side2.Fleet!=null && Side2.Fleet.Target!=null && Side2.Fleet.Target.Destroy == true) { Side2.Fleet.FleetDestroy(); Side2.Fleet = null; }
            if (Side2.Fleet!=null && Side2.Fleet.Target.Mission==FleetMission.Pillage && winner==Side1)
            {
                ServerPlanet planet = Side2.Fleet.Target.GetPlanet();
                TargetLand land=null;
                if (planet.Lands.Count > 0) land = planet.Lands.Values[0]; else if (planet.NewLands.Count > 0) land = planet.NewLands.Values[0];
                if (land != null && land.GetPillageFleet() == Side2.Fleet)
                    land.SetPillageFleet(null); 
            }
            if (Side1.Fleet!=null && Side1.Fleet.Target.SelfPlayer != null)
            {
                SortedList<long, byte> Side1ShipsHealth = CalcShipHealth(Side1);
                Side1.Fleet.Target.SelfPlayer.RecieveBattleResult(Side1.Fleet, Side1ShipsHealth, winner == Side1);
               
            }
            if (Side2.Fleet == null) return;
            
            else if (Side2.Fleet.Target.SelfPlayer!=null)
            {
                SortedList<long, byte> Side2ShipsHealth = CalcShipHealth(Side2);
                Side2.Fleet.Target.SelfPlayer.RecieveBattleResult(Side2.Fleet, Side2ShipsHealth, winner == Side2);
            }
            
        }
        /// <summary> метод расчитывает долю оставшегося здоровья для реальных кораблей (не сгенерированных конкретно для боя) </summary>
        SortedList<long, byte> CalcShipHealth(ServerSide side)
        {
            SortedList<long,byte> ShipsHealth = new SortedList<long, byte>();
            foreach (ServerShipB ship in side.Ships.Values)
                if (ship.ShipID != -1) ShipsHealth.Add(ship.ShipID, ship.Params.Health.GetPercent);
            return ShipsHealth;
        }


    }
    class BattleResult
    {
        public GSPlayer Player;
        byte[] ShipsHealth;
        ServerFleet Fleet;
        bool IsWinner;
        bool IsAttacker;
        public TargetLand Target;
        /// <summary> Обычный результат, если флот игрока </summary>
        public BattleResult(GSPlayer player, byte[] shipshealth, ServerFleet fleet, bool iswinner, bool isattacker)
        {
            Player = player;
            ShipsHealth = shipshealth;
            Fleet = fleet;
            IsWinner = iswinner;
            IsAttacker = isattacker;
        }
        /// <summary> результат - если флот - нпс и он проиграл </summary>
        public BattleResult(TargetLand target)
        {
            Target = target;
        }
        public void Solve()
        {
          /*  if (Target != null)
            {
                ServerNPCLand land = (ServerNPCLand)Target;
                land.Defense -= 0.1f;
                if (land.Defense < 0)
                    land.Defense = 0;
                Console.WriteLine(land.Defense);
                return;
            }*/
            if (Fleet.FleetBase.Land.Player != Player)
            {
                Fleet.FleetDestroy();
                return;
            }
            if (IsAttacker)
            {
                //Player.RecieveBattleResult(Fleet, ShipsHealth, IsWinner);
            }
            else
            {
                //Player.RecieveDefenderResult(Fleet, ShipsHealth, IsWinner);
            }

        }
    }
    public class Movesclass
    {
        List<byte> MovesList = new List<byte>(new byte[] { 0 });
        public byte[] Array = new byte[] { 0 };
        public void AddMove(byte command, byte par1, byte par2, byte par3)
        {
            if (par1 == 255) return;
            MovesList.AddRange(new byte[] { command, par1, par2, par3 });
            Array = MovesList.ToArray();
        }
        public void AddMove(GameMove move)
        {
            if (!move.IsTrue) return;
            MovesList.AddRange(move.Array);
            Array = MovesList.ToArray();
        }
        public void SetSide1Auto()
        {
            MovesList[0] = (byte)(MovesList[0] | 1);
            Array[0] = MovesList[0];
        }
        public void SetSide2Auto()
        {
            MovesList[0] = (byte)(MovesList[0] | 2);
            Array[0] = MovesList[0];
        }
    }
    class BattleID
    {
        public static long ID = -1;
        public static long GetID()
        {
            ID++;
            return ID;
        }
    }
}
