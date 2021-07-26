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
using System.Windows.Media.Animation;
using System.Windows.Controls.Primitives;

namespace Client
{
    public enum MissionType { PirateBase, ConvoyDestroy, ConvoyDefense, OreBeltRaid, MetheoritRaid, LongRangeRaid, Competition, BigCompetition, AlienBounds, AlienBases, ScienceExpedition, ArtifactSearch, PirateShipyard }
    public enum EnemyType { GreenParty, Pirates, Aliens, Techno, Mercenaries, None }
    /*public class Mission:FleetTargetI
    {
        public static byte[] ResModificators;
        public int StarID;
        public byte Level; //Нужно для сервера
        public byte Enemy;
        public byte MinLevel;
        public byte MaxLevel;
        public byte Ships;
        public byte Reward;
        public MissionType Type;
        public ushort ID;
        public byte[] Array;
        public EnemyType RealEnemy;

        public bool IsStarted = false; //Нужно для сервера
        public Mission(byte[] array, ref int i)
        {
            ID = BitConverter.ToUInt16(array, i); i += 2;
            Type = (MissionType)array[i]; i++;
            StarID = BitConverter.ToInt32(array, i); i += 4;
            Enemy = array[i]; i++;
            MinLevel = array[i]; i++;
            MaxLevel = array[i]; i++;
            Ships = array[i]; i++;
            Reward = array[i]; i++;
        }
        public Mission(ushort id, MissionType type, int starid, byte enemy, byte level, byte minlevel, byte maxlevel, byte ships, byte reward) //Нужно для сервера
        {
            ID = id;
            Type = type;
            StarID = starid;
            Enemy = enemy;
            MinLevel = minlevel;
            MaxLevel = maxlevel;
            Ships = ships;
            Reward = reward;
            Level = level;
            //StartTime = DateTime.Now;
            SetArray();
        }
        public FleetTargetE GetTargetType()
        {
            return FleetTargetE.Mission;
        }
        public byte[] GetTargetArray()
        {
            throw new Exception();
        }
        #region ДляСервера
        public void SetArray() //Нужно для сервера
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(ID));
            list.Add((byte)Type);
            list.AddRange(BitConverter.GetBytes(StarID));
            list.Add((byte)Enemy);
            list.Add(MinLevel);
            list.Add(MaxLevel);
            list.Add(Ships);
            list.Add(Reward);
            Array = list.ToArray();
        }
        public static int GetStarID(MissionLocation Location) //Нужно для сервера
        {
            int id = -1;
            switch (Location)
            {
                case MissionLocation.AlienBounds:
                    if (ServerLinks.Enemy.EnemyBounds.Count != 0)
                        id = ServerLinks.Enemy.EnemyBounds.ElementAt(ServerLinks.BattleRandom.Next(ServerLinks.Enemy.EnemyBounds.Count));
                    break;
                case MissionLocation.AlienCenters:
                    if (ServerLinks.Enemy.EnemyBases.Count != 0)
                        id = ServerLinks.Enemy.EnemyBases.ElementAt(ServerLinks.BattleRandom.Next(ServerLinks.Enemy.EnemyBases.Count));
                    break;
                case MissionLocation.Central:
                    id = ServerLinks.Enemy.CentralStars.ElementAt(ServerLinks.BattleRandom.Next(ServerLinks.Enemy.CentralStars.Count));
                    break;
                case MissionLocation.Perimeter:
                    id = ServerLinks.Enemy.PerimeterStars.ElementAt(ServerLinks.BattleRandom.Next(ServerLinks.Enemy.PerimeterStars.Count));
                    break;
            }
            return id;
        }
        public Reward GetReward(GSPlayer player)
        {
            int Modificator = ServerLinks.Missions[Type].ResModificator;
            Reward reward = new Client.Reward();
            if ((Reward & 1) == 1) { reward.Money = 5000 * Level * Modificator; }
            if ((Reward & 2) == 2) { reward.Metall = 1000 * Level * Modificator; }
            if ((Reward & 4) == 4) { reward.Chips = 500 * Level * Modificator; }
            if ((Reward & 8) == 8) { reward.Anti = 300 * Level * Modificator; }
            reward.Experience = 3 + Level * Modificator / 20;
            ushort scienceid = 0;
            if ((Reward & 16) == 16) scienceid = player.Sciences.LearnNewScienceFromBattle(ServerLinks.Missions[Type].RareScience, MinLevel);
            reward.Science = new ushort[] { scienceid };
            reward.Array = reward.GetArray();
            return reward;
        }
        #endregion
        public Border GetMissionBorder()
        {
            Border border = new Border();
            border.Width = 200;
            border.Height = 150;
            border.BorderBrush = Brushes.White;
            border.BorderThickness = new Thickness(2);
            border.Background = Brushes.Black;
            border.CornerRadius = new System.Windows.CornerRadius(10);
            Canvas canvas = new Canvas();
            border.Child = canvas;
            TextBlock title=Common.PutBlock(22,Links.Interface("Mission"),canvas,0,0,200);
            TextBlock block = Common.PutBlock(18, GetMissionName(),canvas,0,22,200);
            TextBlock enblock = Common.PutBlock(16, Links.Interface("Enemy's"), canvas, 0, 50, 60);
            StackPanel enpanel = GetEnemyPanel();
            canvas.Children.Add(enpanel);
            Canvas.SetLeft(enpanel, 50);
            Canvas.SetTop(enpanel, 50);
            TextBlock shipsblock = Common.PutBlock(16, Links.Interface("MisShips") + Ships + "-" + Ships, canvas, 0, 80, 100);
            TextBlock levelblock = Common.PutBlock(16, Links.Interface("MisLevel") + MinLevel + "-" + MaxLevel, canvas, 100, 80, 100);
            TextBlock rewardblock = Common.PutBlock(16, Links.Interface("MisReward"), canvas, 0, 110, 60);
            StackPanel rewardpanel = GetRewardPanel();
            canvas.Children.Add(rewardpanel);
            Canvas.SetLeft(rewardpanel, 50);
            Canvas.SetTop(rewardpanel, 110);
            //border.Tag = this;
            border.PreviewMouseDown += new MouseButtonEventHandler(border_PreviewMouseDown);
            return border;
        }

        void border_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            //Events.SendFleetToResourceBattle(Links.Helper.FleetParamsPanel, this);
            StartBattleEventResult eventresult = Events.SendFleetToResourceBattle(Links.Helper.FleetParamsPanel, this);
            if (eventresult.Result == false)
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(eventresult.ErrorText), true);
            else
                new StartBattlePanel(eventresult.Battleid);
        }
        StackPanel GetEnemyPanel()
        {
            StackPanel panel = new StackPanel();
            panel.Orientation = Orientation.Horizontal;
            if ((Enemy&1)==1)
                panel.Children.Add(Common.GetRectangle(25,Links.Brushes.GreenTeamBrush));
            if ((Enemy&2)==2)
                panel.Children.Add(Common.GetRectangle(25,Links.Brushes.PirateBrush));
            if ((Enemy&4)==4)
                panel.Children.Add(Common.GetRectangle(25,Links.Brushes.AlienBrush));
            if ((Enemy & 8) == 8)
                panel.Children.Add(Common.GetRectangle(25, Links.Brushes.TechTeamBrush));
            if ((Enemy & 16) == 16)
                panel.Children.Add(Common.GetRectangle(25, Links.Brushes.MercTeamBrush));
            return panel;
        }
        StackPanel GetRewardPanel()
        {
            StackPanel panel = new StackPanel();
            panel.Orientation = Orientation.Horizontal;
            if ((Reward & 1) == 1)
                panel.Children.Add(Common.GetRectangle(25, Links.Brushes.MoneyImageBrush));
            if ((Reward & 2) == 2)
                panel.Children.Add(Common.GetRectangle(25, Links.Brushes.MetalImageBrush));
            if ((Reward & 4) == 4)
                panel.Children.Add(Common.GetRectangle(25, Links.Brushes.ChipsImageBrush));
            if ((Reward & 8) == 8)
                panel.Children.Add(Common.GetRectangle(25, Links.Brushes.AntiImageBrush));
            if ((Reward & 16) == 16)
                panel.Children.Add(Common.GetRectangle(25, Links.Brushes.SciencePict));
            return panel;
        }
        string GetMissionName()
        {
            switch (Type)
            {
                case MissionType.PirateBase: return Links.Interface("MisPirateBase");
                case MissionType.ConvoyDestroy: return Links.Interface("MisConvoyDestroy");
                case MissionType.ConvoyDefense: return Links.Interface("MisConvoyDefense");
                case MissionType.OreBeltRaid: return Links.Interface("MisOreBeltRaid");
                case MissionType.MetheoritRaid: return Links.Interface("MisMetheoritRaid");
                case MissionType.LongRangeRaid: return Links.Interface("MisLongRangeRaid");
                case MissionType.Competition: return Links.Interface("MisCompetition");
                case MissionType.BigCompetition: return Links.Interface("MisAnomalySearch");
                case MissionType.AlienBounds: return Links.Interface("MisAlienBounds");
                case MissionType.AlienBases: return Links.Interface("MisAlienBases");
                case MissionType.ScienceExpedition: return Links.Interface("MisScienceRaid");
                case MissionType.ArtifactSearch: return Links.Interface("MisArtefactRaid");
                case MissionType.PirateShipyard: return Links.Interface("MisPirateShipyard");
                default: return "";

           
            }
        }
        string GetText()
        {
            switch (Type)
            {
                case MissionType.PirateBase: return Links.Interface("PirateBaseText");
                case MissionType.ConvoyDefense: return Links.Interface("ConvoyDefenseText");
                case MissionType.ConvoyDestroy: return Links.Interface("ConvoyDestroyText");
                case MissionType.OreBeltRaid: return Links.Interface("OreBeltReidText");
                case MissionType.MetheoritRaid: return Links.Interface("MeteoritRaidText");
                case MissionType.LongRangeRaid: return Links.Interface("LongRangeRaidText");
                case MissionType.Competition: return Links.Interface("CompetitionText");
                case MissionType.BigCompetition: return Links.Interface("BigCompetitionText");
                case MissionType.AlienBounds: return Links.Interface("AlienBoundText");
                case MissionType.AlienBases: return Links.Interface("AlienBaseText");
                case MissionType.ScienceExpedition: return Links.Interface("ScienceExpeditionText");
                case MissionType.ArtifactSearch: return Links.Interface("ArtifactSearchText");
                case MissionType.PirateShipyard: return Links.Interface("PirateShipyardText");
                default: return "Тренировочный текст";
            }
        }
        public Canvas GetPanel()
        {
            Canvas canvas = new Canvas();
            canvas.Width = 1200;
            canvas.Height = 700;
            canvas.Background = new SolidColorBrush(Color.FromArgb(50, 0, 0, 0));
            Rectangle rect = new Rectangle(); rect.Width = 250; rect.Height = 500;
            rect.Fill = Links.Brushes.Helper0;
            canvas.Children.Add(rect);
             Canvas.SetLeft(rect, 950); Canvas.SetTop(rect, 200);
            
            Border textborder = new Border();
            textborder.Width = 700;
            textborder.Height = 200;
            textborder.Background = Brushes.Black;
            textborder.BorderBrush = Brushes.SkyBlue;
            textborder.BorderThickness = new Thickness(1);
            canvas.Children.Add(textborder);
            Canvas.SetLeft(textborder, 200);
            Canvas.SetTop(textborder, 200);
            FleetMissionTextBlock block = new FleetMissionTextBlock(GetText(), 26, 690);
            textborder.Child = block;
            //Canvas.SetLeft(block, 200);
            //Canvas.SetTop(block, 300);
            block.Start();
            TextBlock TitleBlock = Common.GetBlock(30, GetMissionName(), Brushes.White, new Thickness());
            TitleBlock.Width = 700;
            canvas.Children.Add(TitleBlock);
            Canvas.SetTop(TitleBlock, 150);
            Canvas.SetLeft(TitleBlock, 200);
            TitleBlock.TextAlignment = TextAlignment.Center;

            TextBlock RewardBlock = Common.GetBlock(26, Links.Interface("Reward"), Brushes.White, new Thickness());
            RewardBlock.Width = 700;
            canvas.Children.Add(RewardBlock);
            Canvas.SetTop(RewardBlock, 410);
            Canvas.SetLeft(RewardBlock, 200);
            RewardBlock.TextAlignment = TextAlignment.Center;

            Border rewardborder = new Border();
            rewardborder.Width = 700;
            rewardborder.Height = 150;
            rewardborder.Background = Brushes.Black;
            rewardborder.BorderBrush = Brushes.SkyBlue;
            rewardborder.BorderThickness = new Thickness(1);
            canvas.Children.Add(rewardborder);
            Canvas.SetLeft(rewardborder, 200);
            Canvas.SetTop(rewardborder, 445);
            Reward Reward = GetReward();
            int rewardpos = 205;
            if (Reward.Money != 0) { PutRewardInfo(Links.Brushes.MoneyImageBrush, rewardpos, Reward.Money, canvas); rewardpos += 110; }
            if (Reward.Metall != 0) { PutRewardInfo(Links.Brushes.MetalImageBrush, rewardpos, Reward.Metall, canvas); rewardpos += 110; }
            if (Reward.Chips != 0) { PutRewardInfo(Links.Brushes.ChipsImageBrush, rewardpos, Reward.Chips, canvas); rewardpos += 110; }
            if (Reward.Anti != 0) { PutRewardInfo(Links.Brushes.AntiImageBrush, rewardpos, Reward.Anti, canvas); rewardpos += 110; }
            if (Reward.Experience != 0) { PutRewardInfo(Links.Brushes.ExperienceBrush, rewardpos, Reward.Experience, canvas); rewardpos += 110; }
            if (Reward.Science[0] != 0)
            {
                GameObjectImage image = new GameObjectImage();
                image.Width = 130; image.Height = 143;
                canvas.Children.Add(image);
                Canvas.SetLeft(image, rewardpos);
                Canvas.SetTop(image, 450);
                rewardpos += 130;
            }

            InterfaceButton StartButton = new InterfaceButton(200, 70, 7, 26);
            if (Links.Controller.SelectModifier == SelectModifiers.Mission)
            {
                StartButton.SetText(Links.Interface("StartBattle"));
                StartButton.PreviewMouseDown += StartMission;
            }
            else
            {
                StartButton.SetText(Links.Interface("SelectFleet"));
                StartButton.PreviewMouseDown += SelectFleetForMission;
            }
            StartButton.PutToCanvas(canvas, 250, 600);

            InterfaceButton CloseButton = new InterfaceButton(200, 70, 7, 26);
            CloseButton.SetText(Links.Interface("Close"));
            CloseButton.PutToCanvas(canvas, 550, 600);
            CloseButton.PreviewMouseDown += CloseButton_PreviewMouseDown;
            return canvas;
        }
        private void StartMission(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Links.Controller.PopUpCanvas.Remove();
            StartBattleEventResult eventresult = Events.SendFleetToResourceBattle(Links.Helper.FleetParamsPanel, this);
            if (eventresult.Result == false)
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(eventresult.ErrorText), true);
            else
                new StartBattlePanel(eventresult.Battleid);
        }
        private void SelectFleetForMission(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Links.Controller.PopUpCanvas.Remove();
            Links.Helper.Mission = this;
            Links.Controller.SelectPanel(GamePanels.FleetsCanvas, SelectModifiers.FleetForMission);
        }

        private void CloseButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Links.Controller.PopUpCanvas.Remove();
        }
        public void PutRewardInfo(Brush brush, int left, int value, Canvas canvas)
        {
            Rectangle rect = Common.GetRectangle(100, brush);
            canvas.Children.Add(rect);
            Canvas.SetLeft(rect, left);
            Canvas.SetTop(rect, 450);
            TextBlock block = Common.GetBlock(20, value.ToString("### ### ### ###"), Brushes.White, new Thickness(0));
            canvas.Children.Add(block); block.Width = 100; block.TextAlignment = TextAlignment.Center;
            Canvas.SetLeft(block, left);
            Canvas.SetTop(block, 560);
        }
        Reward GetReward()
        {
            int Modificator = Mission.ResModificators[(int)Type];
            Reward reward = new Reward();
            if ((Reward & 1) == 1) { reward.Money = 5000 * MinLevel * Modificator; }
            if ((Reward & 2) == 2) { reward.Metall = 1000 * MinLevel * Modificator; }
            if ((Reward & 4) == 4) { reward.Chips = 500 * MinLevel * Modificator; }
            if ((Reward & 8) == 8) { reward.Anti = 300 * MinLevel * Modificator; }
            reward.Experience = 3 + MinLevel * Modificator / 20;
            ushort scienceid = 0;
            if ((Reward & 16) == 16) scienceid = 1;
            reward.Science = new ushort[] { scienceid };
            return reward;
        }
    }*/
    class FinalMissionParam
    {
        public byte[] Asteroids;
        public byte FieldSize;
        static SortedSet<byte> SmallPortalZone = new SortedSet<byte>(new byte[] { 0, 1, 2, 3, 8, 9, 10, 11, 15, 16, 17, 18, 21, 22, 23, 24, 28, 29, 30, 31, 36, 37, 38, 39 });
        static SortedSet<byte> MediumPortalZone = new SortedSet<byte>(new byte[] { 0, 1, 2, 3, 12, 13, 14, 15, 24, 25, 26, 36, 47, 57, 58, 59, 68, 69, 70, 71, 80, 81, 82, 83 });
        static SortedSet<byte> LargePortalZone = new SortedSet<byte>(new byte[] { 0, 1, 2, 3, 16, 17, 18, 19, 32, 33, 34, 48, 95, 109, 110, 111, 124, 125, 126, 127, 140, 141, 142, 143 });
        static Random rnd = new Random();
        /// <summary> метод создаёт артефакты </summary>
        /// <param name="fieldsize">размер поля боя</param>
        /// <param name="final">здесь ставится массив астероидов, которые будут обязательны</param>
        /// <param name="Explicit">здесь ставится массив точек, где точно не будут астероиды</param>
        /// <param name="asteroids">Количество астероидов. Если 0 - то только из массива final</param>
        public void SetAsteroids(byte fieldsize, byte[] final, byte[] Explicit, byte asteroids)
        {
            FieldSize = fieldsize;
            if (asteroids == 0) { Asteroids = final; return; }
            List<byte> list = new List<byte>(final);
            List<byte> total = new List<byte>();
            int max = fieldsize == 0 ? 40 : fieldsize == 1 ? 84 : 144;
            SortedSet<byte> PortalZone = fieldsize == 0 ? SmallPortalZone : fieldsize == 1 ? MediumPortalZone : LargePortalZone;
            SortedSet<byte> ExplicitSet = new SortedSet<byte>(Explicit);
            foreach (byte b in final)
                ExplicitSet.Add(b);
            for (byte i = 0; i < max; i++)
            {
                if (PortalZone.Contains(i)) continue;
                if (ExplicitSet.Contains(i)) continue;
                total.Add(i);

            }
            for (int i = 0; i < asteroids; i++)
            {
                int pos = rnd.Next(total.Count);
                byte b = total[pos];
                total.RemoveAt(pos);
                list.Add(b);
            }
            Asteroids = list.ToArray();
        }
    }
}
