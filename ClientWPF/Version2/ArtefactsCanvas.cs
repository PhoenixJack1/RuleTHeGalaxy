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

namespace Client
{
    class ArtefactsCanvas : Canvas
    {
        ArtefactFilter Filter;
        public Viewbox box;
        public ArtefactsCanvas()
        {
            box = new Viewbox();
            box.Width = 1230;
            box.Height = 520;
            box.HorizontalAlignment = HorizontalAlignment.Center;
            box.VerticalAlignment = VerticalAlignment.Center;
            box.Child = this;
            put();
            Width = 1280; Height = 600;
        }
        void put()
        {
            Children.Clear();

            Filter = new ArtefactFilter(new string[] { "Все", "Мирные", "Военные" });
            Children.Add(Filter);
            Canvas.SetLeft(Filter, (1280-Filter.Width)/2);
         
        }
        public void Select()
        {
            Gets.GetTotalInfo("После открытия панели артефактов");
            Refresh();
        }
        ArtefactList CurList;
        public void Refresh()
        {
            if (CurList != null) Children.Remove(CurList);
            if (Filter.curvalue==ArtefactFilterEnum.All)
                CurList = new ArtefactList(GSGameInfo.Artefacts);
            else if (Filter.curvalue==ArtefactFilterEnum.Peace)
            {
                SortedList<Artefact, ushort> elements = new SortedList<Artefact, ushort>();
                foreach (KeyValuePair<Artefact, ushort> pair in GSGameInfo.Artefacts)
                    if (pair.Key.Type == ArtefactType.Peace)
                        elements.Add(pair.Key, pair.Value);
                CurList = new ArtefactList(elements);
            }
            else
            {
                SortedList<Artefact, ushort> elements = new SortedList<Artefact, ushort>();
                foreach (KeyValuePair<Artefact, ushort> pair in GSGameInfo.Artefacts)
                    if (pair.Key.Type == ArtefactType.Battle)
                        elements.Add(pair.Key, pair.Value);
                CurList = new ArtefactList(elements);
            }
            Children.Add(CurList);
            Canvas.SetLeft(CurList, 140);
            Canvas.SetTop(CurList, 50);
        }
        class ArtefactList : Canvas
        {
            SortedList<int, Point> Points = GetPoints();
            List<ArtefactImage> Elements;
            SortedList<int, int> Show;
            TextBlock NameBlock;
            TextBlock DesctiptionBlock;
            GameButton SelectButton;
            Path Ramka;
            public ArtefactList(SortedList<Artefact, ushort> elements)
            {
                Width = 1000; Height = 300;
                Ramka = new Path(); Ramka.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                   "M450, 100 a45,45 0 0,1 -40,40 a10,25 0 0,0 -10,30 v110 a10,25 0 0,0 10,30 a45,45 0 0,1 40,40 a50,50 0 0,0 -70,-20 v-210 a50,50 0 0,0 70,-20" +
                   " M550, 100 a45,45 0 0,0 40,40 a10,25 0 0,1 10,30 v110 a10,25 0 0,1 -10,30 a45,45 0 0,0 -40,40 a50,50 0 0,1 70,-20 v-210 a50,50 0 0,1 -70,-20"));
                Children.Add(Ramka);
                Ramka.Stroke = Links.Brushes.SkyBlue;
                LinearGradientBrush ramkabrush = new LinearGradientBrush(); ramkabrush.StartPoint = new Point(0.5, 0);
                ramkabrush.EndPoint = new Point(0.5, 1);
                ramkabrush.GradientStops.Add(new GradientStop(Color.FromRgb(0, 100, 255), 0));
                ramkabrush.GradientStops.Add(new GradientStop(Colors.Transparent, 0.5));
                ramkabrush.GradientStops.Add(new GradientStop(Color.FromRgb(0, 100, 255), 1));
                Ramka.Fill = ramkabrush;
                Canvas.SetZIndex(Ramka, 20);
                NameBlock = Common.GetBlock(46, "", Brushes.White, 600);
                Children.Add(NameBlock);
                Canvas.SetLeft(NameBlock, 200);
                Canvas.SetTop(NameBlock, 50);
                DesctiptionBlock = Common.GetBlock(36, "", Brushes.White, 600);
                Children.Add(DesctiptionBlock);
                Canvas.SetLeft(DesctiptionBlock, 200);
                Canvas.SetTop(DesctiptionBlock, 350);
                SelectButton = new GameButton(300, 200, "Использовать", 36);
                SelectButton.PutToCanvas(this, 850, 300);
                SelectButton.PreviewMouseDown += SelectButton_PreviewMouseDown;
                List<ArtefactImage> list = new List<ArtefactImage>();
                if (elements.Count == 0)
                {
                    for (int i=0;i<7;i++)
                    {
                        ArtefactImage art = new ArtefactImage();
                        art.MouseMove += Rect_MouseMove;
                        list.Add(art);
                    }
                }
                else
                {
                    for (; list.Count < 7;)
                        foreach (KeyValuePair<Artefact, ushort> pair in elements)
                        {
                            ArtefactImage art = new ArtefactImage(pair);
                            art.MouseMove += Rect_MouseMove;
                            list.Add(art);
                        }
                }
                Elements = list;
                FillShow();
                Draw();
                
            }

            private void SelectButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
            {
                Elements[Show[0]].Use();
            }

            void FillShow()
            {
                Show = new SortedList<int, int>();
                Show.Add(0, 0); Show.Add(1, 1);
                Show.Add(2, 2); Show.Add(3, 3);
                Show.Add(-1, Elements.Count - 1);
                Show.Add(-2, Elements.Count - 2);
                Show.Add(-3, Elements.Count - 3);
            }
            void Draw()
            {
                foreach (KeyValuePair<int, int> pair in Show)
                {
                    ArtefactImage rect = Elements[pair.Value];
                    Children.Add(rect);
                    Canvas.SetLeft(rect, Points[pair.Key].X);
                    Canvas.SetTop(rect, Points[pair.Key].Y);
                    rect.Position = pair.Key;
                }
                Elements[Show[-3]].Opacity = 0;
                Elements[Show[3]].Opacity = 0;
                NameBlock.Text = Elements[Show[0]].GetName();
                DesctiptionBlock.Text = Elements[Show[0]].GetDescrition();
                SelectButton.SetText(Elements[Show[0]].GetButton());
            }
            static SortedList<int, Point> GetPoints()
            {
                SortedList<int, Point> result = new SortedList<int, Point>();
                result.Add(-3, new Point(-150, 50)); result.Add(-2, new Point(25, 105));
                result.Add(-1, new Point(200, 145)); result.Add(0, new Point(425, 150));
                result.Add(1, new Point(650, 145)); result.Add(2, new Point(825, 105));
                result.Add(3, new Point(1000, 50));
                return result;
            }

            private void Rect_MouseMove(object sender, MouseEventArgs e)
            {
                if (Moving == true) return;
                ArtefactImage rect = (ArtefactImage)sender;
                int val = rect.Position;
                if (val != 0) { TargetPos = val; MoveTo(); }
            }
            int TargetPos;
            TimeSpan ts = TimeSpan.FromSeconds(0.6);
            bool Moving = false;
            void MoveTo()
            {
                if (TargetPos > 0)
                {
                    Moving = true;
                    TargetPos--;
                    ArtefactImage elM3 = Elements[Show[-3]];
                    Children.Remove(elM3);
                    ArtefactImage elM2 = Elements[Show[-2]];
                    DoubleAnimation m2oa = new DoubleAnimation(1, 0, ts);
                    elM2.BeginAnimation(Canvas.OpacityProperty, m2oa);
                    AddMoveAnim(-2, -3, elM2);
                    ArtefactImage elM1 = Elements[Show[-1]];
                    AddMoveAnim(-1, -2, elM1);
                    ArtefactImage el0 = Elements[Show[0]];
                    AddMoveAnim(0, -1, el0);
                    ArtefactImage el1 = Elements[Show[1]];
                    AddMoveAnim(1, 0, el1);
                    ArtefactImage el2 = Elements[Show[2]];
                    AddMoveAnim(2, 1, el2);
                    ArtefactImage el3 = Elements[Show[3]];
                    AddMoveAnim(3, 2, el3);
                    DoubleAnimation m3oa = new DoubleAnimation(0, 1, ts);
                    m3oa.Completed += M3oa_Completed;
                    el3.BeginAnimation(Canvas.OpacityProperty, m3oa);
                    for (int i = -3; i < 4; i++)
                    {
                        Show[i] = (Show[i] + 1) % Elements.Count;
                        Elements[Show[i]].Position = i;
                    }

                    el3 = Elements[Show[3]];
                    Children.Add(el3);
                    DoubleAnimation anim = new DoubleAnimation(1, 0, TimeSpan.Zero);
                    el3.BeginAnimation(ArtefactImage.OpacityProperty, anim);
                    DoubleAnimationUsingKeyFrames ramkaleft = new DoubleAnimationUsingKeyFrames();
                    ramkaleft.Duration = ts;
                    ramkaleft.KeyFrames.Add(new LinearDoubleKeyFrame(160, KeyTime.FromPercent(0.25)));
                    ramkaleft.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(1.00)));
                    Ramka.BeginAnimation(Canvas.LeftProperty, ramkaleft);
                }
                else if (TargetPos < 0)
                {
                    Moving = true;
                    TargetPos++;
                    ArtefactImage el3 = Elements[Show[3]];
                    Children.Remove(el3);
                    ArtefactImage el2 = Elements[Show[2]];
                    DoubleAnimation m2oa = new DoubleAnimation(1, 0, ts);
                    el2.BeginAnimation(Canvas.OpacityProperty, m2oa);
                    AddMoveAnim(2, 3, el2);
                    ArtefactImage el1 = Elements[Show[1]];
                    AddMoveAnim(1, 2, el1);
                    ArtefactImage el0 = Elements[Show[0]];
                    AddMoveAnim(0, 1, el0);
                    ArtefactImage elM1 = Elements[Show[-1]];
                    AddMoveAnim(-1, 0, elM1);
                    ArtefactImage elM2 = Elements[Show[-2]];
                    AddMoveAnim(-2, -1, elM2);
                    ArtefactImage elM3 = Elements[Show[-3]];
                    AddMoveAnim(-3, -2, elM3);
                    DoubleAnimation m3oa = new DoubleAnimation(0, 1, ts);
                    m3oa.Completed += M3oa_Completed;
                    elM3.BeginAnimation(Canvas.OpacityProperty, m3oa);
                    for (int i = -3; i < 4; i++)
                    {
                        int el = Show[i] - 1; if (el == -1) el = Elements.Count - 1;
                        Show[i] = el;
                        Elements[Show[i]].Position = i;
                    }

                    elM3 = Elements[Show[-3]];
                    Children.Add(elM3);
                    DoubleAnimation anim = new DoubleAnimation(1, 0, TimeSpan.Zero);
                    elM3.BeginAnimation(Canvas.OpacityProperty, anim);
                    DoubleAnimationUsingKeyFrames ramkaright = new DoubleAnimationUsingKeyFrames();
                    ramkaright.Duration = ts;
                    ramkaright.KeyFrames.Add(new LinearDoubleKeyFrame(-160, KeyTime.FromPercent(0.25)));
                    ramkaright.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(1.00)));
                    Ramka.BeginAnimation(Canvas.LeftProperty, ramkaright);
                }
                else
                {
                    Moving = false;
                    NameBlock.Text = Elements[Show[0]].GetName();
                    DesctiptionBlock.Text = Elements[Show[0]].GetDescrition();
                    SelectButton.SetText(Elements[Show[0]].GetButton());
                }
            }

            private void M3oa_Completed(object sender, EventArgs e)
            {
                MoveTo();
            }

            void AddMoveAnim(int from, int to, ArtefactImage rect)
            {
                DoubleAnimation m2l = new DoubleAnimation(Points[from].X, Points[to].X, ts);
                rect.BeginAnimation(Canvas.LeftProperty, m2l);
                DoubleAnimation m2t = new DoubleAnimation(Points[from].Y, Points[to].Y, ts);
                rect.BeginAnimation(Canvas.TopProperty, m2t);
            }
            class ArtefactImage:Canvas
            {
                Artefact Artefact;
                ushort Count;
                public int Position;
                public ArtefactImage(KeyValuePair<Artefact, ushort> pair)
                {
                    Artefact = pair.Key; Count = pair.Value;
                    Width = 150; Height = 150; Background = Artefact.GetImage();
                    if (Count>1)
                    {
                        TextBlock CountBlock = Common.GetBlock(30, "X" + Count.ToString(), Brushes.White, 50);
                        Children.Add(CountBlock);
                        Canvas.SetLeft(CountBlock, 100);
                        Canvas.SetTop(CountBlock, 100);
                    }
                }
                public ArtefactImage()
                {
                    Border border;
                    Width = 150; Height = 150; Children.Add(border=Common.CreateBorder(150, 150, Brushes.White, 1, 0));
                    border.Background = Links.Brushes.Transparent;
                    border.CornerRadius = new CornerRadius(20);
                }
                public string GetName()
                {
                    if (Artefact != null)
                        return Artefact.GetName();
                    else
                        return "";
                }
                public string GetDescrition()
                {
                    if (Artefact != null)
                        return Artefact.GetDescription();
                    else
                        return "";
                }
                public string GetButton()
                {
                    if (Artefact == null)
                        return "-----------";
                    else if (Artefact.Type == ArtefactType.Peace)
                        return "Использовать";
                    else
                        return "Оснастить флот";
                }
                public void Use()
                {
                    if (Artefact == null)
                        return;
                    if (Artefact.Type==ArtefactType.Battle)
                    {
                        Links.Helper.ArtefactID = Artefact.ID;
                        Links.Controller.SelectPanel(GamePanels.FleetsCanvas, SelectModifiers.FleetForArtefact);
                        return;
                    }
                    switch (Artefact.Ability)
                    {
                        case ArtefactAbility.PResourceAdd: ArtefactUseResult result = new ArtefactUseResult(Events.UsePeaceArtefact(0, null).Message);
                            if (result.Result == true)
                            {
                                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(String.Format("Склады заполнены на {0} процентов", result.Value)));
                                Gets.GetResources();
                                Links.Controller.SelectPanel(GamePanels.Artefacts, SelectModifiers.None);
                            }
                            else
                                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(result.GetErrorText()));
                            break;
                        case ArtefactAbility.PPeopleClone: new SelectLandCanvas(1, "Выберите колонию, в которой применить Капсулу клонирования"); break;
                        case ArtefactAbility.PSectorRemove: new SelectLandCanvas(2, "Выберите колонию, в которой применить Эко-Бомбу"); break;
                        case ArtefactAbility.PsectorCreate: new SelectLandCanvas(3, "Выберите колонию, в которой применить Фазовый планировщик"); break;
                        case ArtefactAbility.PColonyCreate: new SelectLandCanvas(4, "Выберите аванпост, в котором применить Малый комплект создания рая"); break;
                    }
                    
                }
                
            }
        }
    }
    
    class SelectLandCanvas:Canvas
    {
        Artefact Art;
        public SelectLandCanvas(ushort artefactID, string text)
        {
            Art = Links.Artefacts[artefactID];
            Width = 1000; Height = 600;
            TextBlock Title = Common.GetBlock(30, text, Brushes.White, 1000);
            Children.Add(Title);
            WrapPanel panel = new WrapPanel();
            foreach (Land land in GSGameInfo.PlayerLands.Values)
            {
                if (land.CheckArtefactAvailable(Art) == false) continue;
                Border border = land.GetShortInfo(false, 300);
                border.PreviewMouseDown += Border_PreviewMouseDown;
                panel.Children.Add(border);
            }
            foreach (Avanpost avanpost in GSGameInfo.PlayerAvanposts.Values)
            {
                if (avanpost.CheckArtefactAvailable(Art) == false) continue;
                Border border = avanpost.GetShortInfo(false, 300);
                border.PreviewMouseDown += Border_PreviewMouseDown;
                panel.Children.Add(border);
            }
            if (panel.Children.Count==0)
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage("Нет подходящих объектов"));
                return;
            }
            if (panel.Children.Count == 1)
                panel.Width = 300;
            else if (panel.Children.Count == 2)
                panel.Width = 600;
            else panel.Width = 900;

            panel.Height = 300 + (Children.Count - 1) / 3 * 300;
            if (panel.Height > 450)
            {
                ScrollViewer viewer = new ScrollViewer(); viewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                viewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
                viewer.Content = panel;
                Children.Add(viewer); Canvas.SetTop(viewer, 150); Canvas.SetLeft(viewer, 50);
                Links.Controller.PopUpCanvas.Place(this,true);
            }
            else
            {
                Children.Add(panel); Canvas.SetTop(panel, 150); Canvas.SetLeft(panel, (1000 - panel.Width) / 2);
                Links.Controller.PopUpCanvas.Place(this,true);
            }
        }

        private void Border_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Border border = (Border)sender;
            switch (Art.Ability)
            {
                case ArtefactAbility.PPeopleClone: //Население +20
                    int landid = ((Land)border.Tag).ID;
                    ArtefactUseResult result =new ArtefactUseResult(Events.UsePeaceArtefact(1, BitConverter.GetBytes(landid)).Message);
                    if (result.Result == true)
                    {
                        Links.Controller.PopUpCanvas.Change(new SimpleInfoMessage(String.Format("Население увеличено на {0} миллионов человек", result.Value/10,0)),"Сообщение при использовании артефакта ID=1");
                        Links.Controller.SelectPanel(GamePanels.Artefacts, SelectModifiers.None);
                    }
                    else
                        Links.Controller.PopUpCanvas.Change(new SimpleInfoMessage(result.GetErrorText()),"Сообщение об ошибке при вызове артефакта ID=1");
                    break;
                case ArtefactAbility.PSectorRemove: //Очистить сектор
                    Links.Controller.PopUpCanvas.Remove();
                    int landid2 = ((Land)border.Tag).ID;
                    Colonies.CurrentPlanet = GSGameInfo.PlayerLands[landid2].Planet;
                    //Colonies.CurrentLand = GSGameInfo.PlayerLands[landid2];
                    Links.Helper.ArtefactOnSectorHandler = ClearSectorEvent;
                    Links.Controller.SelectPanel(GamePanels.Colonies, SelectModifiers.SectorForArtefact);
                    break;
                case ArtefactAbility.PsectorCreate: //Добавить сектор
                    int landid3 = ((Land)border.Tag).ID;
                    ArtefactUseResult result3 = new ArtefactUseResult(Events.UsePeaceArtefact(3, BitConverter.GetBytes(landid3)).Message);
                    if (result3.Result == true)
                    {
                        Links.Controller.PopUpCanvas.Change(new SimpleInfoMessage("Сектор добавлен"), "Сообщение при использовании артефакта ID=3");
                        GSGameInfo.PlayerLands[landid3].Planet.Locations++;
                        Gets.GetTotalInfo("После использования артефакта с ID=3");
                        Links.Controller.SelectPanel(GamePanels.Colonies, SelectModifiers.None);
                    }
                    else
                        Links.Controller.PopUpCanvas.Change(new SimpleInfoMessage(result3.GetErrorText()), "Сообщение об ошибке при вызове артефакта ID=3");
                    break;
                case ArtefactAbility.PColonyCreate: //Достроить авнапост Малый
                    int avanpostID = ((Avanpost)border.Tag).ID;
                    ArtefactUseResult result4 = new ArtefactUseResult(Events.UsePeaceArtefact(4, BitConverter.GetBytes(avanpostID)).Message);
                    if (result4.Result == true)
                    {
                        Links.Controller.PopUpCanvas.Change(new SimpleInfoMessage("Колония построена"), "Сообщение при использовании артефакта ID=4");
                        Gets.GetTotalInfo("После использования артефакта с ID=4");
                        Colonies.CurrentPlanet = GSGameInfo.PlayerLands[avanpostID].Planet;
                        //Colonies.CurrentLand = GSGameInfo.PlayerLands[avanpostID];
                        Links.Controller.SelectPanel(GamePanels.Colonies, SelectModifiers.None);
                    }
                    else
                        Links.Controller.PopUpCanvas.Change(new SimpleInfoMessage(result4.GetErrorText()), "Сообщение об ошибке при вызове артефакта ID=4");
                    break;
            }
        }
        public static void ClearSectorEvent(int landid, byte sectorpos)
        {
            LandSector sector = GSGameInfo.PlayerLands[landid].Locations[sectorpos];
            if (sector.Type==SectorTypes.Clear)
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage("Сектор уже чист"));
                return;
            }
            if (sector.Type==SectorTypes.Live)
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage("Нельзя очистить жилой сектор"));
                return;
            }
            if (sector.Type==SectorTypes.War && ((FleetBase)sector).Fleet!=null && ((FleetBase)sector).Fleet.Target!=null)
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage("Нельзя очистить военную базу, пока флот на задании"));
                return;
            }
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(landid));
            list.Add(sectorpos);
            ByteMessage eventresult = Events.UsePeaceArtefact(2, list.ToArray());
            ArtefactUseResult result = new ArtefactUseResult(eventresult.Message);
            if (result.Result == false)
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(result.GetErrorText()));
            else
            {
                Gets.GetTotalInfo("После использования артефакта с по очистке сектора");
                Links.Controller.SelectPanel(GamePanels.Colonies, SelectModifiers.None);
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage("Сектор очищен"));
            }
        }
    }
    enum ArtefactFilterEnum { All, Peace, War}
    class ArtefactFilter : Canvas
    {
        string[] Values;
        TextBlock block;
        public ArtefactFilterEnum curvalue = ArtefactFilterEnum.All;
        public ArtefactFilter(string[] list)
        {
            Values = list;
            Width = 300; Height = 50;
            Background = Brushes.Black;
            Path LeftArrow = new Path();
            LeftArrow.Width = 50; LeftArrow.Height = 50;
            LeftArrow.Stroke = Brushes.White;
            LeftArrow.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M25,0 a25,25 0 1,0 0.1,0 M25,1 a24,24 0 1,0 0.1,0 M7,25 l18,-18 a18,18 0 0,1 0,36z"));
            Children.Add(LeftArrow);
            LeftArrow.Fill = Common.GetRadialBrush(Colors.Green, 0.7, 0.3);
            LeftArrow.PreviewMouseDown += new MouseButtonEventHandler(LeftArrow_PreviewMouseDown);

            Path RightArrow = new Path();
            RightArrow.Width = 50; RightArrow.Height = 50;
            RightArrow.Stroke = Brushes.White;
            RightArrow.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M25,0 a25,25 0 1,0 0.1,0 M25,1 a24,24 0 1,0 0.1,0 M44,25 l-18,-18 a18,18 0 0,0 0,36z"));
            Children.Add(RightArrow);
            Canvas.SetLeft(RightArrow, 250);
            RightArrow.Fill = Common.GetRadialBrush(Colors.Green, 0.7, 0.3);
            RightArrow.PreviewMouseDown += new MouseButtonEventHandler(RightArrow_PreviewMouseDown);

            Path Body = new Path();
            Body.Stroke = Brushes.White;
            Body.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M25,0 h250 M25,50 h250"));
            Children.Add(Body);
            Margin = new Thickness(10);

            block = Common.GetBlock(24, Values[0]);
            Children.Add(block);
            block.Foreground = Brushes.White;
            block.TextAlignment = TextAlignment.Center;
            block.Width = 200;
            Canvas.SetLeft(block, 50);
            Canvas.SetTop(block, 10);
        }

        void RightArrow_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            curvalue++;
            if ((int)curvalue == 3) curvalue = ArtefactFilterEnum.All;
            else if ((int)curvalue == -1) curvalue = ArtefactFilterEnum.War;
            block.Text = Values[(int)curvalue];
            Links.Controller.ArtefactCanvas.Refresh();
        }

        void LeftArrow_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            curvalue--;
            if ((int)curvalue == 3) curvalue = ArtefactFilterEnum.All;
            else if ((int)curvalue == -1) curvalue = ArtefactFilterEnum.War;
            block.Text = Values[(int)curvalue];
            Links.Controller.ArtefactCanvas.Refresh();
        }
    }
}
