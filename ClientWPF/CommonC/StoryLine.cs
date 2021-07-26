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
using System.Text.RegularExpressions;

namespace Client
{
    public enum StoryType { Text, SpaceBattle, PlanetBattle }
    public class StoryLine2 : FleetTargetI, ServerBattleSide2
    {
        public static SortedList<int, StoryLine2> StoryLines = GetStoryes();
        public static SortedList<byte, string> StoryShipNames = GetStoryShipNames();
        public static SortedList<byte, StoryRarePilot> StoryPilotsInfo = GetStoryPilotsInfo();
        public static SortedList<byte, HelperInfo> HelpersInfo = GetHelpersInfo();
        public static SortedList<byte, Brush> StoryAsteroids = GetStoryAsteroids();
        public int ID;
        public string Title;
        public StoryType StoryType;
        public byte Person;
        public string Text;
        public Reward2 Reward;
        public int LocationID = -1;
        public BattleFieldParam BattleFieldParam;
        public SideBattleParam Side2BattleParam;
        public int[] RemoveQuestBlocks;
        public StoryLine2(int id, string crypt)
        {
            ID = id;
            Title = Common.GetTaggedValue(crypt, "Title");
            string text = Common.GetTaggedValue(crypt, "Type");
            TextPosition[] pos = TextPosition.GetPositions(text);
            switch (pos[0].Title)
            {
                case "TEXT": StoryType = StoryType.Text; break;
                case "SPACE": StoryType = StoryType.SpaceBattle; LocationID = pos[0].IntValues[0]; break;
                case "PLANET": StoryType = StoryType.PlanetBattle; LocationID = pos[0].IntValues[0]; break;
                default: StoryType = StoryType.Text; break;
            }
            text = Common.GetTaggedValue(crypt, "Helper");
            Person = Byte.Parse(text);
            Text = Common.GetTaggedValue(crypt, "Text");
            text = Common.GetTaggedValue(crypt, "RemoveBlock");
            if (text!= null)
            {
                pos = TextPosition.GetPositions(text);
                RemoveQuestBlocks = pos[0].IntValues;
            }
            text = Common.GetTaggedValue(crypt, "Battle");
            if (text != "")
            {
                BattleFieldParam = new BattleFieldParam(text);
            }
            text = Common.GetTaggedValue(crypt, "SideD");
            if (text != "")
            {
                Side2BattleParam = new SideBattleParam(text);
            }
            List<ShipBattleParam> shiplist = new List<ShipBattleParam>();
            for (int i = 0; ; i++)
            {
                string regex = "ShipD" + i.ToString();
                text = Common.GetTaggedValue(crypt, regex);
                if (text == "")
                    break;
                shiplist.Add(new ShipBattleParam(text));
            }
            if (Side2BattleParam != null) Side2BattleParam.Ships = shiplist.ToArray();
            text = Common.GetTaggedValue(crypt, "Reward");
            if (text != "")
            {
                pos = TextPosition.GetPositions(text);
                Reward = new Reward2();
                foreach (TextPosition p in pos)
                {
                    switch (p.Title)
                    {
                        case "MONEY": Reward.Money = p.IntValues[0]; break;
                        case "METALL": Reward.Metall = p.IntValues[0]; break;
                        case "CHIPS": Reward.Chips = p.IntValues[0]; break;
                        case "ANTI": Reward.Anti = p.IntValues[0]; break;
                        case "EXP": Reward.Experience = p.IntValues[0]; break;
                        case "ART":
                            Reward.Artefacts = new ushort[p.IntValues.Length];
                            for (int i = 0; i < p.IntValues.Length; i++) Reward.Artefacts[i] = (ushort)p.IntValues[i]; break;
                        case "TECH":
                            Reward.Sciences = new ushort[p.IntValues.Length];
                            for (int i = 0; i < p.IntValues.Length; i++) Reward.Sciences[i] = (ushort)p.IntValues[i]; break;
                        case "AVANPOST": Reward.AvanpostID = p.IntValues[0]; break;
                        case "LAND": Reward.LandID = p.IntValues[0]; break;
                    }
                }
                Reward.CreateArray();
            }
            text = Common.GetTaggedValue(crypt, "BonusShip");
            if (text != "")
            {
                pos = TextPosition.GetPositions(text);
                Schema schema = new Schema();
                GSPilot pilot = null;
                int model = 0;
                foreach (TextPosition p in pos)
                {
                    switch (p.Title)
                    {
                        case "IMG": model = BitConverter.ToInt32(p.GetBytes(), 0); break;
                        case "NAME": schema.SetName(BitConverter.ToInt32(p.GetBytes(), 0)); break;
                        case "ST": schema.SetShipType((ushort)p.IntValues[0]); break;
                        case "GEN": schema.SetGenerator((ushort)p.IntValues[0]); break;
                        case "SH": schema.SetShield((ushort)p.IntValues[0]); break;
                        case "ENG": schema.SetEngine((ushort)p.IntValues[0]); break;
                        case "COMP": schema.SetComputer((ushort)p.IntValues[0]); break;
                        case "W1": schema.SetWeapon((ushort)p.IntValues[0], 0); break;
                        case "W2": schema.SetWeapon((ushort)p.IntValues[0], 1); break;
                        case "W3": schema.SetWeapon((ushort)p.IntValues[0], 2); break;
                        case "E1": schema.SetEquipment((ushort)p.IntValues[0], 0); break;
                        case "E2": schema.SetEquipment((ushort)p.IntValues[0], 1); break;
                        case "E3": schema.SetEquipment((ushort)p.IntValues[0], 2); break;
                        case "E4": schema.SetEquipment((ushort)p.IntValues[0], 3); break;
                        case "ARM": schema.Armor = (ArmorType)p.IntValues[0]; break;
                        case "PIL": pilot = new GSPilot(p.GetBytes(), 0); break;
                    }
                }
                if (Reward == null) Reward = new Reward2();
                Reward.BonusShip = new GSShip(0, schema, 100, model); Reward.BonusShip.Pilot = pilot;
                Reward.CreateArray();
            }
            text = Common.GetTaggedValue(crypt, "Pilot");
            if (text != "")
            {
                pos = TextPosition.GetPositions(text);
                GSPilot pilot = null;
                foreach (TextPosition p in pos)
                {
                    switch (p.Title)
                    {
                        case "PIL": pilot = new GSPilot(p.GetBytes(), 0); break;
                    }
                }
                if (Reward == null) Reward = new Reward2();
                Reward.Pilot = pilot;
                Reward.CreateArray();
            }

        }
        public static SortedList<int, StoryLine2> GetStoryes()
        {
            SortedList<int, StoryLine2> result = new SortedList<int, StoryLine2>();
            string[] files = System.IO.File.ReadAllLines("GameData/StoryLine/Ru/MissionList_Rus.txt", Encoding.GetEncoding(1251));
            byte id = 0;
            foreach (String line in files)
            {
                result.Add(id, new StoryLine2(id, System.IO.File.ReadAllText(String.Format("GameData/StoryLine/Ru/{0}", line), Encoding.GetEncoding(1251))));
                id++;
            }
            //result.Add(0, new StoryLine2(0, System.IO.File.ReadAllText("GameData/StoryLine/Ru/New1.txt", Encoding.GetEncoding(1251))));
            return result;
        }
        public GSStar GetStar()
        {
            if (StoryType == StoryType.Text) return null;
            else if (StoryType == StoryType.SpaceBattle) return Links.Stars[LocationID];
            else return Links.Planets[LocationID].Star;
        }
        public static SortedList<byte, string> GetStoryShipNames()
        {
            SortedList<byte, string> result = new SortedList<byte, string>();
            string file = System.IO.File.ReadAllText("GameData/StoryLine/Ru/StoryShipNames_Rus.txt", Encoding.GetEncoding(1251));
            string[] split = file.Split(';');
            foreach (string s in split)
            {
                string[] inn = s.Split('/');
                result.Add(Byte.Parse(inn[0]), inn[1]);
            }
            return result;
        }
        public static SortedList<byte, StoryRarePilot> GetStoryPilotsInfo()
        {
            SortedList<byte, StoryRarePilot> result = new SortedList<byte, StoryRarePilot>();
            string[] files = System.IO.File.ReadAllLines("GameData/StoryLine/Ru/StoryPilots_Rus.txt", Encoding.GetEncoding(1251));
            foreach (string s in files)
            {
                string[] inn = s.Split('/');
                string name = inn[1];
                string nick = inn[2];
                string family = inn[3];
                byte sex = (byte)(inn[4] == "M" ? 0 : 1);
                byte country = 0;
                switch (inn[5])
                {
                    case "Rus": country = 0; break;
                    case "Jap": country = 1; break;
                    case "Amer": country = 2; break;
                    case "Comp": country = 100; break;
                }
                string city = inn[6];
                int age = Int32.Parse(inn[7]);
                result.Add(Byte.Parse(inn[0]), new StoryRarePilot(name, nick, family, sex, country, city, age));
            }
            return result;
        }
        public static SortedList<byte, Brush> GetStoryAsteroids()
        {
            SortedList<byte, Brush> result = new SortedList<byte, Brush>();
            string[] files = System.IO.File.ReadAllLines("GameData/StoryLine/Ru/Asteroids_Rus.txt", Encoding.GetEncoding(1251));
            foreach (String line in files)
            {
                string[] inn = line.Split('/');
                byte id = Byte.Parse(inn[0]);
                result.Add(id, GetStoryImage(inn[1]));
            }
            return result;
        }
        public static SortedList<byte, HelperInfo> GetHelpersInfo()
        {
            SortedList<byte, HelperInfo> result = new SortedList<byte, HelperInfo>();
            string[] files = System.IO.File.ReadAllLines("GameData/StoryLine/Ru/Helpers_Rus.txt", Encoding.GetEncoding(1251));
            foreach (String line in files)
            {
                string[] inn = line.Split('/');
                byte id = Byte.Parse(inn[0]);
                result.Add(id, new HelperInfo(inn[1], GetStoryImage(inn[2])));
            }
            return result;
        }
        public static ImageBrush GetStoryImage(string file)
        {
            return new ImageBrush(new BitmapImage(new Uri(String.Format("GameData/StoryLine/Ru/Images/{0}", file), UriKind.Relative)));
        }
       
        public void PutPanel()
        {
            StoryLinePanel panel = new StoryLinePanel(this);
            Links.Controller.PopUpCanvas.Place(panel);
        }

        
        public static RadialGradientBrush GetStoryFlashBrush()
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            GradientStop gr1, gr2, gr3;
            brush.GradientStops.Add(gr1 = new GradientStop(Colors.Transparent, -0.2));
            brush.GradientStops.Add(gr2 = new GradientStop(Colors.Yellow, -0.1));
            brush.GradientStops.Add(gr3 = new GradientStop(Colors.Transparent, 0));
            DoubleAnimation anim = new DoubleAnimation();
            anim.Duration = TimeSpan.FromSeconds(1);
            anim.By = 1.2;
            anim.AccelerationRatio = 1;
            anim.RepeatBehavior = RepeatBehavior.Forever;
            gr1.BeginAnimation(GradientStop.OffsetProperty, anim);
            gr2.BeginAnimation(GradientStop.OffsetProperty, anim);
            gr3.BeginAnimation(GradientStop.OffsetProperty, anim);
            return brush;
        }
        public FleetTargetE GetTargetType()
        {
            return FleetTargetE.Story;
        }
        public byte[] GetTargetArray()
        {
            List<byte> list = new List<byte>();
            list.Add((byte)FleetTargetE.Story);
            list.AddRange(BitConverter.GetBytes(ID));
            return list.ToArray();
        }
        public BattleFieldGroup GetBattleField()
        {
            return BattleFieldParam.GetBattleField();
        }
        public Restriction GetRestriction()
        {
            return Restriction.None;
        }
        public SideBattleParam GetSide2BattleParams(GSPlayer player, Restriction restriction)
        {
            return Side2BattleParam;
        }
        public SideBattleParam GetSide1BattleParams(GSPlayer player, Restriction restriction)
        {
            return null;
        }
    }
    public class HelperInfo
    {
        public string Name;
        public ImageBrush Brush;
        public HelperInfo(string name, ImageBrush brush)
        {
            Name = name; Brush = brush;
        }
    }
    public class StoryRarePilot
    {
        public string Name;
        public string Surname;
        public string Nick;
        public string City;
        public int Age;
        public byte Sex;
        public byte Country;
        public StoryRarePilot(string name, string nick, string family, byte sex, byte country, string city, int age)
        {
            Name = name; Nick = nick; Surname = family; City = city; Age = age; Sex = sex; Country = country;
        }
    }
    class FleetMissionPanel : Canvas
    {
        public static bool PanelIsCreated = false;
        public static FleetMissionPanel CurrentPanel = null;
        protected TextBlock PersonName;
        protected Border PersonBorder;
        protected ScrollViewer rewardviewer;
        protected Border textborder;
        protected TextBlock TitleBlock;
        protected Border rewardborder;
        public FleetMissionPanel()
        {
            PanelIsCreated = true;
            CurrentPanel = this;
            Width = 1200;
            Height = 700;
            Background = new SolidColorBrush(Color.FromArgb(50, 0, 0, 0));
            PersonName = Common.GetBlock(30, "", Brushes.White, 300);
            Children.Add(PersonName); Canvas.SetLeft(PersonName, 930); Canvas.SetTop(PersonName, 150);
            PersonBorder = Common.CreateBorder(300, 450, Links.Brushes.SkyBlue, 2, 0);
            Children.Add(PersonBorder); Canvas.SetLeft(PersonBorder, 930); Canvas.SetTop(PersonBorder, 200);
           
            textborder = new Border();
            textborder.Width = 700;
            textborder.Height = 200;
            textborder.Background = Brushes.Black;
            textborder.BorderBrush = Brushes.SkyBlue;
            textborder.BorderThickness = new Thickness(1);
            Children.Add(textborder);
            Canvas.SetLeft(textborder, 200);
            Canvas.SetTop(textborder, 200);

            TitleBlock = Common.GetBlock(30, "", Brushes.White, new Thickness());
            TitleBlock.Width = 700;
            Children.Add(TitleBlock);
            Canvas.SetTop(TitleBlock, 150);
            Canvas.SetLeft(TitleBlock, 200);
            TitleBlock.TextAlignment = TextAlignment.Center;

            TextBlock RewardBlock = Common.GetBlock(26, Links.Interface("Reward"), Brushes.White, new Thickness());
            RewardBlock.Width = 700;
            Children.Add(RewardBlock);
            Canvas.SetTop(RewardBlock, 410);
            Canvas.SetLeft(RewardBlock, 200);
            RewardBlock.TextAlignment = TextAlignment.Center;

            rewardborder = new Border();
            rewardborder.Width = 700;
            rewardborder.Height = 150;
            rewardborder.Background = Brushes.Black;
            rewardborder.BorderBrush = Brushes.SkyBlue;
            rewardborder.BorderThickness = new Thickness(1);
            Children.Add(rewardborder);
            Canvas.SetLeft(rewardborder, 200);
            Canvas.SetTop(rewardborder, 445);

            InterfaceButton CloseButton = new InterfaceButton(200, 70, 7, 26);
            CloseButton.SetText(Links.Interface("Close"));
            CloseButton.PutToCanvas(this, 550, 600);
            CloseButton.PreviewMouseDown += CloseButton_PreviewMouseDown;
        }
        private void CloseButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Links.Controller.PopUpCanvas.Remove();
            PanelIsCreated = false;
        }
        protected void PutImage(ImageBrush Image)
        {
            ImageBrush personImage = Image;
            double width = personImage.ImageSource.Width; double height = personImage.ImageSource.Height;
            Rectangle personrect = new Rectangle(); personrect.Fill = personImage;
            PersonBorder.Child = personrect;
            if (height / 448 > width / 298)
            {
                personrect.Width = width / height * 448;
                personrect.Height = 448;
                personrect.Margin = new Thickness((298 - personrect.Width) / 2, 0, 0, 0);
            }
            else
            {
                personrect.Width = 298;
                personrect.Height = height / width * 298;
                personrect.Margin = new Thickness(0, (448 - personrect.Height), 0, 0);
            }
        }
    }
    class StoryLinePanel : FleetMissionPanel
    {
        public static StoryLine2 CurrentStory;
        public StoryLinePanel(StoryLine2 story)
        {
            CurrentStory = story;

            PersonName.Text = StoryLine2.HelpersInfo[CurrentStory.Person].Name;
            PutImage(StoryLine2.HelpersInfo[CurrentStory.Person].Brush);


            FleetMissionTextBlock block = new FleetMissionTextBlock(CurrentStory.Text, 26, 690);
            textborder.Child = block;
            block.Start();

            TitleBlock.Text = CurrentStory.Title;

            PlaceRewardAllInfo();
            InterfaceButton StartButton = new InterfaceButton(200, 70, 7, 26);
            if (CurrentStory.StoryType == StoryType.Text)
            {
                StartButton.SetText("Далее");
                StartButton.PreviewMouseDown += FinishTextMission;
            }
            else
            {
                StartButton.SetText(Links.Interface("SelectFleet"));
                StartButton.PreviewMouseDown += SelectFleetForStory;
            }
            StartButton.PutToCanvas(this, 250, 600);
        }
        public void ChangeStory(StoryLine2 story)
        {
            if (story.Person != CurrentStory.Person)
            {
                PersonName.Text = StoryLine2.HelpersInfo[story.Person].Name;
                ImageBrush personImage = StoryLine2.HelpersInfo[story.Person].Brush;
                double width = personImage.ImageSource.Width; double height = personImage.ImageSource.Height;
                Rectangle personrect = new Rectangle(); personrect.Fill = personImage;
                PersonBorder.Child = personrect;
                if (height / 448 > width / 298)
                {
                    personrect.Width = width / height * 448;
                    personrect.Height = 448;
                    personrect.Margin = new Thickness((298 - personrect.Width) / 2, 0, 0, 0);
                }
                else
                {
                    personrect.Width = 298;
                    personrect.Height = height / width * 298;
                    personrect.Margin = new Thickness(0, (448 - personrect.Height), 0, 0);
                }
            }
            FleetMissionTextBlock block = new FleetMissionTextBlock(CurrentStory.Text, 26, 690);
            textborder.Child = block;
            block.Start();
            TitleBlock.Text = story.Title;
            CurrentStory = story;
            PlaceRewardAllInfo();
        }
        void PlaceRewardAllInfo()
        {
            if (CurrentStory.Reward != null)
            {
                Canvas RewardCanvas = new Canvas();
                //RewardCanvas.Width = 695;
                RewardCanvas.Height = 145;
                //RewardCanvas.ClipToBounds = true;
                rewardborder.Child = RewardCanvas;
                int rewardpos = 0;
                if (CurrentStory.Reward.BonusShip != null)
                {
                    Viewbox box = new Viewbox(); box.Width = 130; box.Height = 110;
                    ShipB shipimage = new ShipB(0, CurrentStory.Reward.BonusShip.Schema, 100, CurrentStory.Reward.BonusShip.Pilot, BitConverter.GetBytes(CurrentStory.Reward.BonusShip.Model), ShipSide.Attack, null, false, null, ShipBMode.Info, 255);
                    box.Child = shipimage.HexShip;
                    RewardCanvas.Children.Add(box); Canvas.SetLeft(box, rewardpos); Canvas.SetTop(box, 20);
                    rewardpos += 130;
                }
                if (CurrentStory.Reward.Money != 0) { PutRewardInfo(Links.Brushes.MoneyImageBrush, rewardpos, CurrentStory.Reward.Money, RewardCanvas); rewardpos += 110; }
                if (CurrentStory.Reward.Metall != 0) { PutRewardInfo(Links.Brushes.MetalImageBrush, rewardpos, CurrentStory.Reward.Metall, RewardCanvas); rewardpos += 110; }
                if (CurrentStory.Reward.Chips != 0) { PutRewardInfo(Links.Brushes.ChipsImageBrush, rewardpos, CurrentStory.Reward.Chips, RewardCanvas); rewardpos += 110; }
                if (CurrentStory.Reward.Anti != 0) { PutRewardInfo(Links.Brushes.AntiImageBrush, rewardpos, CurrentStory.Reward.Anti, RewardCanvas); rewardpos += 110; }
                if (CurrentStory.Reward.Experience != 0) { PutRewardInfo(Links.Brushes.ExperienceBrush, rewardpos, CurrentStory.Reward.Experience, RewardCanvas); rewardpos += 110; }
                if (CurrentStory.Reward.Pilot != null)
                {
                    PutRewardInfo(new VisualBrush(new PilotsImage(CurrentStory.Reward.Pilot, PilotsListMode.Clear, null)), rewardpos, 0, RewardCanvas); rewardpos += 110;
                }
                if (CurrentStory.Reward.Artefacts != null && CurrentStory.Reward.Artefacts.Length > 0)
                {
                    foreach (ushort artid in CurrentStory.Reward.Artefacts)
                    {
                        Artefact art = Links.Artefacts[artid];
                        PutRewardInfo(art.GetImage(), rewardpos, art.GetName(), RewardCanvas); rewardpos += 110;
                    }
                }
                if (CurrentStory.Reward.Sciences != null && CurrentStory.Reward.Sciences.Length != 0)
                    foreach (ushort scienceid in CurrentStory.Reward.Sciences)
                    {
                        GameObjectImage image = new GameObjectImage(GOImageType.Standard, scienceid);
                        image.Width = 130; image.Height = 143;
                        RewardCanvas.Children.Add(image);
                        Canvas.SetLeft(image, rewardpos);
                        //Canvas.SetTop(image, 450);
                        rewardpos += 130;

                    }
                if (CurrentStory.Reward.AvanpostID != -1)
                {
                    PutRewardPlanet(CurrentStory.Reward.AvanpostID, rewardpos, "Аванпост", RewardCanvas); rewardpos += 150;
                }
                if (CurrentStory.Reward.LandID != -1)
                {
                    PutRewardPlanet(CurrentStory.Reward.LandID, rewardpos, "Колония", RewardCanvas); rewardpos += 150;
                }
                RewardCanvas.Width = rewardpos;
                if (rewardpos > 700)
                {
                    Canvas MovinCanvas = new Canvas(); MovinCanvas.Width = 695; MovinCanvas.Height = 145;
                    rewardborder.Child = MovinCanvas;
                    Path pathleft = new Path(); pathleft.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,70 l40,-70 v140 z"));
                    pathleft.Fill = Links.Brushes.SkyBlue;
                    MovinCanvas.Children.Add(pathleft); pathleft.Tag = -200; pathleft.PreviewMouseDown += RewardCanvasMove;
                    Canvas.SetTop(pathleft, 2.5);
                    Path pathright = new Path(); pathright.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,0 l40,70 l-40,70z"));
                    pathright.Fill = Links.Brushes.SkyBlue;
                    MovinCanvas.Children.Add(pathright); pathright.Tag = 200; pathright.PreviewMouseDown += RewardCanvasMove;
                    Canvas.SetLeft(pathright, 655); Canvas.SetTop(pathright, 2.5);
                    rewardviewer = new ScrollViewer();
                    rewardviewer.Width = 615; rewardviewer.Height = 145;
                    rewardviewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
                    rewardviewer.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
                    rewardviewer.Content = RewardCanvas;
                    Canvas.SetLeft(rewardviewer, 40);
                    MovinCanvas.Children.Add(rewardviewer);
                }
            }
            else rewardborder.Child = null;
        }
        public void PutRewardInfo(Brush brush, int left, int value, Canvas canvas)
        {
            Rectangle rect = Common.GetRectangle(100, brush);
            canvas.Children.Add(rect);
            Canvas.SetLeft(rect, left);
            //Canvas.SetTop(rect, 450);
            TextBlock block = Common.GetBlock(20, value.ToString("### ### ### ###"), Brushes.White, new Thickness(0));
            canvas.Children.Add(block); block.Width = 100; block.TextAlignment = TextAlignment.Center;
            Canvas.SetLeft(block, left);
            Canvas.SetTop(block, 110);
        }
        public void PutRewardInfo(Brush brush, int left, string text, Canvas canvas)
        {
            Rectangle rect = Common.GetRectangle(100, brush);
            canvas.Children.Add(rect);
            Canvas.SetLeft(rect, left);
            //Canvas.SetTop(rect, 450);
            TextBlock block = Common.GetBlock(16, text, Brushes.White, new Thickness(0));
            canvas.Children.Add(block); block.Width = 100; block.TextAlignment = TextAlignment.Center;
            Canvas.SetLeft(block, left);
            Canvas.SetTop(block, 110);
        }
        public void PutRewardPlanet(int PlanetID, int left, string text, Canvas canvas)
        {
            GSPlanet planet = Links.Planets[PlanetID];
            PlanetPanelInfo info = new PlanetPanelInfo(planet, true);
            canvas.Children.Add(info);
            info.RenderTransform = new ScaleTransform(0.7, 0.7);
            Canvas.SetLeft(info, left);// Canvas.SetTop(info, 450);
            TextBlock block = Common.GetBlock(20, text, Brushes.White, new Thickness(0));
            canvas.Children.Add(block); block.Width = 100; block.TextAlignment = TextAlignment.Center;
            Canvas.SetLeft(block, left + 20);
            Canvas.SetTop(block, 110);
        }
        private void RewardCanvasMove(object sender, MouseButtonEventArgs e)
        {
            Path path = (Path)sender;
            int delta = (int)path.Tag;
            rewardviewer.ScrollToHorizontalOffset(rewardviewer.HorizontalOffset + delta);
        }
        private void FinishTextMission(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            //Links.Controller.PopUpCanvas.Remove();
            StartBattleEventResult result = Events.SendFleetToStoryLine(null);
            if (result.Result == false)
            {
                PanelIsCreated = false;
                Links.Controller.PopUpCanvas.Change(new SimpleInfoMessage(result.ErrorText), "Отображение ошибки при обработке сюжетной миссии");
                return;
            }
            else
            {
                Gets.GetResources();
                Gets.GetTotalInfo("После окончания текстовой миссии");
                //Links.Controller.PopUpCanvas.Remove();
                NoticeBorder.CheckNotice();
            }
        }
        private void SelectFleetForStory(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Links.Controller.PopUpCanvas.Remove();
            Links.Controller.SelectPanel(GamePanels.FleetsCanvas, SelectModifiers.FleetForStory);
        }
        private void CloseButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Links.Controller.PopUpCanvas.Remove();
            PanelIsCreated = false;
        }
    }
    
    class FleetMissionTextBlock : TextBlock
    {
        System.Windows.Threading.DispatcherTimer timer;
        int pos = 0;
        string CurText;
        DateTime StartTime;
        public FleetMissionTextBlock(string text, int fontsize, int width)
        {
            FontFamily = Links.Font;
            Foreground = Brushes.White;
            TextWrapping = TextWrapping.Wrap;
            FontSize = fontsize;
            Width = width;
            CurText = text;
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.04);
            timer.Tick += Timer_Tick;
            PreviewMouseDown += StoryLineBlock_PreviewMouseDown;
        }
        public void Start()
        {
            StartTime = DateTime.Now;
            timer.Start();
        }
        private void StoryLineBlock_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            needstop = true;
        }
        bool needstop = false;
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (needstop) { Text = CurText; timer.Stop(); return; }
            Text = CurText.Substring(0, pos);
            TimeSpan spent = DateTime.Now - StartTime;
            pos = (int)(spent.TotalMilliseconds / 40);
            if (pos > CurText.Length)
            {
                Text = CurText; timer.Stop();
            }
        }
    }
    class StoryReward
    {
        public int Money;
        public int Metall;
        public int Chips;
        public int Anti;
        public int Exp;
        public ushort[] Techs;
        public int BonusShip;
        public StoryReward()
        {

        }
        public StoryReward(int money, int metall, int chips, int anti, int exp, ushort[] techs, int bonusship)
        {
            Money = money; Metall = metall; Chips = chips; Anti = anti; Exp = exp; Techs = techs; BonusShip = bonusship;
        }
    }
    
}
