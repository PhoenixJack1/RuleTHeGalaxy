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
    class ClansCanvas:Canvas
    {
        public ClansCanvas()
        {
            Width = 400;
            Height = 520;
            HorizontalAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Center;
        }
        public void Select()
        {
            Children.Clear();
            Children.Add(new ClanBorder());
            
        }
    }
    class ClanBorder : Border
    {
        public ClanBorder()
        {
            //grid.Children.Add(this);
            //Grid.SetColumn(this, 2);
            BorderBrush = Brushes.White;
            BorderThickness = new Thickness(3);
            this.Width = 400;
            this.Height = 520;
            if (GSGameInfo.Clan.NoClan)
            {
                //нет клана
                StackPanel NoClanPanel = new StackPanel();
                NoClanPanel.Orientation = Orientation.Vertical;
                Child = NoClanPanel;
                InterfaceButton CreateClanBtn = new InterfaceButton(240, 100, 10, 26);
                CreateClanBtn.SetText(Links.Interface("CreateClanBtn"));
                NoClanPanel.Children.Add(CreateClanBtn);
                CreateClanBtn.Margin = new Thickness(0, 120, 0, 120);
                CreateClanBtn.PreviewMouseDown += CreateClanBtn_Click;

                InterfaceButton EnterInClanBtn = new InterfaceButton(240, 100, 10, 26);
                EnterInClanBtn.SetText(Links.Interface("EnterInClanBtn"));
                NoClanPanel.Children.Add(EnterInClanBtn);
                CreateClanBtn.Margin = new Thickness(0, 120, 0, 120);
                EnterInClanBtn.PreviewMouseDown += EnterInClanBtn_Click;
            }
            else
            {
                //есть клан
                ClanInfoCanvas claninfo = new ClanInfoCanvas();
                Child = claninfo;
            }
        }

        void EnterInClanBtn_Click(object sender, RoutedEventArgs e)
        {
            SelectClanBorder border = new SelectClanBorder();
            if (border.IsClosed == false)
                Links.Controller.PopUpCanvas.Place(border, false);
        }

        void CreateClanBtn_Click(object sender, RoutedEventArgs e)
        {
            Links.Controller.PopUpCanvas.Place(new CreateClanBorder(), false);
        }
    }
    class RequestsInfoCanvas : Border
    {
        Grid inngrid;
        Canvas incanvas;
        Label lastborder;
        InterfaceButton InviteButton;
        InterfaceButton DeclineButton;
        public RequestsInfoCanvas()
        {
            Width = 450;
            Height = 600;
            Background = Brushes.Black;
            BorderBrush = Brushes.White;
            BorderThickness = new Thickness(3);
            CornerRadius = new CornerRadius(10);
            incanvas = new Canvas();
            Child = incanvas;

            Label ClanName = new Label();
            ClanName.Content = Links.Interface("InviteRequest");
            ClanName.FontFamily = Links.Font;
            ClanName.FontSize = 30;
            ClanName.Foreground = Brushes.White;
            incanvas.Children.Add(ClanName);
            ClanName.HorizontalContentAlignment = HorizontalAlignment.Center;
            ClanName.Width = 400;
            Canvas.SetTop(ClanName, 10);
            Canvas.SetLeft(ClanName, 25);

            Grid grid = new Grid();
            grid.Width = 410;
            for (int i = 0; i < 9; i++)
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions[1].Width = new GridLength(3, GridUnitType.Star);
            incanvas.Children.Add(grid);
            Canvas.SetTop(grid, 70);

            Label Namelbl = new Label();
            Namelbl.Content = Links.Interface("Name");
            Namelbl.FontFamily = Links.Font;
            Namelbl.Foreground = Brushes.White;
            Namelbl.HorizontalAlignment = HorizontalAlignment.Center;
            Namelbl.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(Namelbl);
            Grid.SetColumn(Namelbl, 1);
            Namelbl.FontSize = 24;

            Label Poslbl = new Label();
            Poslbl.Content = "N";
            Poslbl.FontFamily = Links.Font;
            Poslbl.Foreground = Brushes.White;
            Poslbl.HorizontalAlignment = HorizontalAlignment.Center;
            Poslbl.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(Poslbl);
            Poslbl.FontSize = 24;

            Common.PutRect(grid, 0, 2, 30, Links.Brushes.LandIconBrush, Links.Interface("LandsCount"));
            Common.PutRect(grid, 0, 3, 30, Links.Brushes.PeopleImageBrush, Links.Interface("Population"));
            Common.PutRect(grid, 0, 4, 30, Links.Brushes.FleetBrush, Links.Interface("FleetsCount"));
            Common.PutRect(grid, 0, 5, 30, Links.Brushes.FleetDefenseBrush, Links.Interface("FleetsCountDefense"));
            Common.PutRect(grid, 0, 6, 30, Links.Brushes.TwoSwords, Links.Interface("BattlesCount"));
            Common.PutRect(grid, 0, 7, 30, Links.Brushes.ShipImageBrush, Links.Interface("ShipsCount"));
            Common.PutRect(grid, 0, 8, 30, Links.Brushes.SciencePict, Links.Interface("ScienceCount"));

            ScrollViewer viewer = new ScrollViewer();
            viewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            viewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            viewer.Height = 430;
            viewer.Width = 430;
            incanvas.Children.Add(viewer);
            Canvas.SetTop(viewer, 100);

            inngrid = new Grid();
            viewer.Content = inngrid;
            inngrid.Width = 410;
            inngrid.VerticalAlignment = VerticalAlignment.Top;
            for (int i = 0; i < 9; i++)
                inngrid.ColumnDefinitions.Add(new ColumnDefinition());
            inngrid.ColumnDefinitions[1].Width = new GridLength(3, GridUnitType.Star);

            InviteButton = new InterfaceButton(100, 40, 7, 20);
            InviteButton.SetText(Links.Interface("InvitePlayer"));
            InviteButton.PutToCanvas(incanvas, 120, 550);
            InviteButton.SetDisabled();
            InviteButton.PreviewMouseDown += InviteButton_Click;

            DeclineButton = new InterfaceButton(100, 40, 7, 20);
            DeclineButton.SetText(Links.Interface("DeclinePlayer"));
            DeclineButton.PutToCanvas(incanvas, 230, 550);
            DeclineButton.SetDisabled();
            DeclineButton.PreviewMouseDown += DeclineButton_Click;

            InterfaceButton CloseButton = new InterfaceButton(100, 40, 7, 20);
            CloseButton.PutToCanvas(incanvas, 10, 550);
            CloseButton.SetText(Links.Interface("Close"));
            CloseButton.PreviewMouseDown += CloseButton_Click;
            Fill();
        }

        void DeclineButton_Click(object sender, RoutedEventArgs e)
        {
            int playerid = (int)lastborder.Tag;
            string eventresult = Events.DeclinePlayer(playerid);
            if (eventresult != "")
            {
                Links.Controller.PopUpCanvas.Remove();
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(eventresult), true);
            }
            else
            {
                Refresh();
            }
        }

        void InviteButton_Click(object sender, RoutedEventArgs e)
        {
            int playerid = (int)lastborder.Tag;
            string eventresult = Events.InvitePlayer(playerid);
            if (eventresult != "")
            {
                Links.Controller.PopUpCanvas.Remove();
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(eventresult), true);
            }
            else
            {
                Refresh();
                Links.Controller.Nation.Select();
            }
        }

        void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Links.Controller.PopUpCanvas.Remove();
        }
        public void Refresh()
        {
            inngrid.Children.Clear();
            lastborder = null;
            InviteButton.SetDisabled();
            DeclineButton.SetDisabled();
            Fill();
        }
        public void Fill()
        {
            byte[] array = Events.GetRequestList();
            if (array[0] == 0)
            {
                int pos = -1;
                for (int i = 1; i < array.Length;)
                {
                    pos++;
                    int id = BitConverter.ToInt32(array, i); i += 4;
                    GSString name = new GSString(array, i); i += name.Array.Length;
                    int lands = BitConverter.ToInt32(array, i); i += 4;
                    double peoples = BitConverter.ToDouble(array, i); i += 8;
                    int fleets = BitConverter.ToInt32(array, i); i += 4;
                    int defense = BitConverter.ToInt32(array, i); i += 4;
                    int battles = BitConverter.ToInt32(array, i); i += 4;
                    int ships = BitConverter.ToInt32(array, i); i += 4;
                    int sciences = BitConverter.ToInt32(array, i); i += 4;
                    inngrid.RowDefinitions.Add(new RowDefinition());
                    inngrid.RowDefinitions[pos].Height = new GridLength(30);
                    PutLabel((pos + 1).ToString(), pos, 0);
                    Label namelbl = PutLabel(name.ToString(), pos, 1);
                    PutLabel(lands.ToString(), pos, 2);
                    PutLabel(Common.GetThreeSymbol(peoples * 1000000), pos, 3);
                    PutLabel(fleets.ToString(), pos, 4);
                    PutLabel(defense.ToString(), pos, 5);
                    PutLabel(battles.ToString(), pos, 6);
                    PutLabel(Common.GetThreeSymbol(ships), pos, 7);
                    PutLabel(Common.GetThreeSymbol(sciences), pos, 8);
                    namelbl.Tag = id;
                    namelbl.PreviewMouseDown += new MouseButtonEventHandler(namelbl_PreviewMouseDown);
                }
            }
            else
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(Common.GetStringFromByteArray(array, 1)), true);
            }
        }

        void namelbl_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Label lbl = (Label)sender;
            if (lastborder == null)
            {
                lbl.BorderBrush = Brushes.Red;
                lbl.BorderThickness = new Thickness(2);
                lastborder = lbl;
                InviteButton.SetEnabled();
                DeclineButton.SetEnabled();
            }
            else if (lastborder == lbl)
            {
                lbl.BorderBrush = null;
                lastborder = null;
                InviteButton.SetDisabled();
                DeclineButton.SetDisabled();
            }
            else
            {
                lastborder.BorderBrush = null;
                lbl.BorderBrush = Brushes.Red;
                lbl.BorderThickness = new Thickness(2);
                lastborder = lbl;
                InviteButton.SetEnabled();
                DeclineButton.SetEnabled();
            }
        }

        public Label PutLabel(string text, int row, int column)
        {
            Label lbl = new Label();
            lbl.FontFamily = Links.Font;
            lbl.Foreground = Brushes.White;
            lbl.HorizontalAlignment = HorizontalAlignment.Center;
            lbl.VerticalAlignment = VerticalAlignment.Center;
            lbl.FontSize = 16;
            lbl.Content = text;
            inngrid.Children.Add(lbl);
            Grid.SetRow(lbl, row);
            Grid.SetColumn(lbl, column);
            return lbl;
        }

    }
    class ClanInfoCanvas : Canvas
    {
        Grid inngrid;
        InterfaceButton RequestButton;
        InterfaceButton LeavePlayerButton;
        Label lastborder;
        public ClanInfoCanvas()
        {
            Width = 410;
            Height = 520;

            ClanEmblem emblem = new ClanEmblem(GSGameInfo.Clan.Image);
            Children.Add(emblem);

            Label ClanName = new Label();
            ClanName.Content = GSGameInfo.Clan.Name;
            ClanName.FontFamily = Links.Font;
            ClanName.FontSize = 30;
            ClanName.Foreground = Brushes.White;
            Children.Add(ClanName);
            ClanName.HorizontalContentAlignment = HorizontalAlignment.Center;
            ClanName.Width = 300;
            Canvas.SetTop(ClanName, 30);
            Canvas.SetLeft(ClanName, 100);

            Grid grid = new Grid();
            grid.ShowGridLines = true;
            grid.Width = 390;
            for (int i = 0; i < 10; i++)
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions[1].Width = new GridLength(3, GridUnitType.Star);
            Children.Add(grid);
            Canvas.SetTop(grid, 100);

            Label Namelbl = new Label();
            Namelbl.Content = Links.Interface("Name");
            Namelbl.FontFamily = Links.Font;
            Namelbl.Foreground = Brushes.White;
            Namelbl.HorizontalAlignment = HorizontalAlignment.Center;
            Namelbl.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(Namelbl);
            Grid.SetColumn(Namelbl, 1);
            Namelbl.FontSize = 24;

            Label Poslbl = new Label();
            Poslbl.Content = "N";
            Poslbl.FontFamily = Links.Font;
            Poslbl.Foreground = Brushes.White;
            Poslbl.HorizontalAlignment = HorizontalAlignment.Center;
            Poslbl.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(Poslbl);
            //Grid.SetColumn(Poslbl, 1);
            Poslbl.FontSize = 24;

            Common.PutRect(grid, 0, 2, 30, Links.Brushes.LandIconBrush, Links.Interface("LandsCount"));
            Common.PutRect(grid, 0, 3, 30, Links.Brushes.PeopleImageBrush, Links.Interface("Population"));
            Common.PutRect(grid, 0, 4, 30, Links.Brushes.FleetBrush, Links.Interface("FleetsCount"));
            Common.PutRect(grid, 0, 5, 30, Links.Brushes.FleetDefenseBrush, Links.Interface("FleetsCountDefense"));
            Common.PutRect(grid, 0, 6, 30, Links.Brushes.TwoSwords, Links.Interface("BattlesCount"));
            Common.PutRect(grid, 0, 7, 30, Links.Brushes.ShipImageBrush, Links.Interface("ShipsCount"));
            Common.PutRect(grid, 0, 8, 30, Links.Brushes.SciencePict, Links.Interface("ScienceCount"));
            Common.PutRect(grid, 0, 9, 30, Links.Brushes.CrownBrush, Links.Interface("Leadership"));

            ScrollViewer viewer = new ScrollViewer();
            viewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            viewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            viewer.Height = 320;
            viewer.Width = 410;
            Children.Add(viewer);
            Canvas.SetTop(viewer, 130);

            inngrid = new Grid();
            inngrid.ShowGridLines = true;
            viewer.Content = inngrid;
            inngrid.Width = 390;
            inngrid.VerticalAlignment = VerticalAlignment.Top;
            for (int i = 0; i < 10; i++)
                inngrid.ColumnDefinitions.Add(new ColumnDefinition());
            inngrid.ColumnDefinitions[1].Width = new GridLength(3, GridUnitType.Star);
            //inngrid.Background = Brushes.Red;
            //inngrid.ShowGridLines = true;

            InterfaceButton LeaveButton = new InterfaceButton(100, 40, 7, 20);
            LeaveButton.PutToCanvas(this, 20, 470);
            LeaveButton.SetText(Links.Interface("LeaveClan"));
            LeaveButton.PreviewMouseDown += LeaveButton_Click;

            RequestButton = new InterfaceButton(100, 40, 7, 20);
            RequestButton.SetText(Links.Interface("InviteRequest"));
            RequestButton.PutToCanvas(this, 130, 470);
            RequestButton.Visibility = Visibility.Hidden;
            RequestButton.PreviewMouseDown += RequestButton_Click;

            LeavePlayerButton = new InterfaceButton(100, 40, 7, 20);
            LeavePlayerButton.SetText(Links.Interface("LeavePlayer"));
            LeavePlayerButton.PutToCanvas(this, 240, 470);
            LeavePlayerButton.Visibility = Visibility.Hidden;
            LeavePlayerButton.SetDisabled();
            LeavePlayerButton.PreviewMouseDown += LeavePlayerButton_Click;
            Fill();
        }

        void LeavePlayerButton_Click(object sender, RoutedEventArgs e)
        {
            int playerid = (int)lastborder.Tag;
            string eventresult = Events.LeavePlayer(playerid);
            if (eventresult != "")
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(eventresult), true);
            else
                Links.Controller.Nation.Select();
        }

        void RequestButton_Click(object sender, RoutedEventArgs e)
        {
            RequestsInfoCanvas border = new RequestsInfoCanvas();
            Links.Controller.PopUpCanvas.Place(border, false);
        }

        void LeaveButton_Click(object sender, RoutedEventArgs e)
        {
            string leaveresult = Events.LeaveClanEvent();
            if (leaveresult != "")
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(leaveresult), true);
            }
            else
                Links.Controller.Nation.Select();
        }
        public void Fill()
        {
            byte[] array = Events.GetSelfClanInfo();
            if (array[0] == 0)
            {
                int leaderscount = BitConverter.ToInt32(array, 1);
                List<int> LeadersID = new List<int>();
                int c = 5;
                for (int i = 0; i < leaderscount; i++)
                {
                    LeadersID.Add(BitConverter.ToInt32(array, c));
                    c += 4;
                }
                int pos = -1;
                for (int i = c; i < array.Length;)
                {
                    pos++;
                    int id = BitConverter.ToInt32(array, i); i += 4;
                    GSString name = new GSString(array, i); i += name.Array.Length;
                    int lands = BitConverter.ToInt32(array, i); i += 4;
                    double peoples = BitConverter.ToDouble(array, i); i += 8;
                    int fleets = BitConverter.ToInt32(array, i); i += 4;
                    int defense = BitConverter.ToInt32(array, i); i += 4;
                    int battles = BitConverter.ToInt32(array, i); i += 4;
                    int ships = BitConverter.ToInt32(array, i); i += 4;
                    int sciences = BitConverter.ToInt32(array, i); i += 4;
                    inngrid.RowDefinitions.Add(new RowDefinition());
                    inngrid.RowDefinitions[pos].Height = new GridLength(30);
                    PutLabel((pos + 1).ToString(), pos, 0);
                    Label namelabel = PutLabel(name.ToString(), pos, 1);
                    namelabel.Tag = id;
                    PutLabel(lands.ToString(), pos, 2);
                    PutLabel(Common.GetThreeSymbol(peoples * 1000000), pos, 3);
                    PutLabel(fleets.ToString(), pos, 4);
                    PutLabel(defense.ToString(), pos, 5);
                    PutLabel(battles.ToString(), pos, 6);
                    PutLabel(Common.GetThreeSymbol(ships), pos, 7);
                    PutLabel(Common.GetThreeSymbol(sciences), pos, 8);
                    if (LeadersID.Contains(id)) PutLeaderRect(pos);
                    namelabel.PreviewMouseDown += new MouseButtonEventHandler(namelabel_PreviewMouseDown);
                    if (LeadersID.Contains(id) && GSGameInfo.PlayerName == name.ToString())
                        GSGameInfo.Clan.IsClanLeader = true;
                }
                if (GSGameInfo.Clan.IsClanLeader)
                {
                    RequestButton.Visibility = Visibility.Visible;
                    LeavePlayerButton.Visibility = Visibility.Visible;
                }
            }
            else
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(Common.GetStringFromByteArray(array, 1)), true);
            }
        }

        void namelabel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Label lbl = (Label)sender;
            if (lastborder == null)
            {
                lbl.BorderBrush = Brushes.Red;
                lbl.BorderThickness = new Thickness(2);
                lastborder = lbl;
                LeavePlayerButton.SetEnabled();
            }
            else if (lastborder == lbl)
            {
                lbl.BorderBrush = null;
                lastborder = null;
                LeavePlayerButton.SetDisabled();
            }
            else
            {
                lastborder.BorderBrush = null;
                lbl.BorderBrush = Brushes.Red;
                lbl.BorderThickness = new Thickness(2);
                lastborder = lbl;
                LeavePlayerButton.SetEnabled();
            }
        }

        public void PutLeaderRect(int row)
        {
            Common.PutRect(inngrid, row, 9, 30, Links.Brushes.VBrush, null);
        }
        public Label PutLabel(string text, int row, int column)
        {
            Label lbl = new Label();
            lbl.FontFamily = Links.Font;
            lbl.Foreground = Brushes.White;
            lbl.HorizontalAlignment = HorizontalAlignment.Center;
            lbl.VerticalAlignment = VerticalAlignment.Center;
            lbl.FontSize = 16;
            lbl.Content = text;
            inngrid.Children.Add(lbl);
            Grid.SetRow(lbl, row);
            Grid.SetColumn(lbl, column);
            return lbl;
        }

    }
    class SelectClanBorder : Border
    {
        Canvas MainCanvas;
        public int MaxClanID = 0;
        public int MinClanID = 0;
        public List<Border> CurBorders = new List<Border>();
        InterfaceButton LeftButton;
        InterfaceButton RightButton;
        InterfaceButton SendRequest;
        Border lastborder;
        public bool IsClosed = false;
        bool IsSelectMode = true;
        public SelectClanBorder()
        {
            Width = 805;
            Height = 580;
            Background = Brushes.Black;
            BorderBrush = Brushes.White;
            BorderThickness = new Thickness(3);
            CornerRadius = new CornerRadius(20);
            MainCanvas = new Canvas();
            Child = MainCanvas;

            Label TitleLabel = new Label();
            TitleLabel.Content = Links.Interface("SelectClanLbl");
            MainCanvas.Children.Add(TitleLabel);
            TitleLabel.Style = Links.TextStyle;
            TitleLabel.Foreground = Brushes.White;
            TitleLabel.Width = 800;
            TitleLabel.HorizontalContentAlignment = HorizontalAlignment.Center;

            InterfaceButton CloseButton = new InterfaceButton(100, 50, 7, 20);
            CloseButton.SetText(Links.Interface("Close"));
            CloseButton.PutToCanvas(MainCanvas, 410, 510);
            CloseButton.PreviewMouseDown += CloseButton_Click;

            FillTable(true);
            if (IsSelectMode)
            {

                LeftButton = new InterfaceButton(50, 50, 7, 20);
                LeftButton.SetImage(Links.Brushes.LeftArrowBrush);
                LeftButton.PutToCanvas(MainCanvas, 10, 510);
                LeftButton.PreviewMouseDown += LeftButton_Click;

                RightButton = new InterfaceButton(50, 50, 7, 20);
                RightButton.SetImage(Links.Brushes.RightArrowBrush);
                RightButton.PutToCanvas(MainCanvas, 740, 510);
                RightButton.PreviewMouseDown += RightButton_Click;

                SendRequest = new InterfaceButton(200, 50, 7, 20);
                SendRequest.SetText(Links.Interface("SendRequest"));
                SendRequest.PutToCanvas(MainCanvas, 200, 510);
                SendRequest.SetDisabled();
                SendRequest.PreviewMouseDown += SendRequest_Click;
            }
            else
            {
                InterfaceButton RemoveRequest = new InterfaceButton(200, 50, 7, 20);
                RemoveRequest.SetText(Links.Interface("RemoveRequest"));
                RemoveRequest.PutToCanvas(MainCanvas, 200, 510);
                RemoveRequest.PreviewMouseDown += RemoveRequest_Click;
            }
        }

        void RemoveRequest_Click(object sender, RoutedEventArgs e)
        {
            string eventresult = Events.RemoveInviteRequest();
            Links.Controller.PopUpCanvas.Remove();
            if (eventresult != "")
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(eventresult), true);
        }

        void SendRequest_Click(object sender, RoutedEventArgs e)
        {
            int clanid = (int)lastborder.Tag;
            string eventresult = Events.SendInviteRequest(clanid);
            if (eventresult == "")
            {
                Links.Controller.PopUpCanvas.Remove();
                Links.Controller.Nation.Select();
            }
            else
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(eventresult), true);
            }
        }

        void LeftButton_Click(object sender, RoutedEventArgs e)
        {
            FillTable(false);
        }

        void RightButton_Click(object sender, RoutedEventArgs e)
        {
            FillTable(true);
        }
        public void FillTable(bool isgrow)
        {
            lastborder = null;
            if (SendRequest != null)
                SendRequest.SetDisabled();
            foreach (Border border in CurBorders)
                MainCanvas.Children.Remove(border);
            CurBorders.Clear();
            byte[] array;
            if (isgrow)
                array = Events.GetClansList(MaxClanID);
            else
                array = Events.GetClansList(-MinClanID);
            MaxClanID = 0;
            MinClanID = Int32.MaxValue;
            if (array[0] != 0)
            {
                //Links.Controller.PopUpCanvas.Remove();
                string text = Common.GetStringFromByteArray(array, 1);
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(text), true);
                IsClosed = true;
                return;
            }

            if (array[1] == 1)
            {
                IsSelectMode = false;
                PutRequestClanInfo(array);
                return;
            }
            int pos = 0;
            for (int i = 2; i < array.Length; pos++)
            {
                int clanid = BitConverter.ToInt32(array, i); i += 4;
                if (clanid > MaxClanID) MaxClanID = clanid;
                if (clanid < MinClanID) MinClanID = clanid;
                GSString ClanName = new GSString(array, i); i += ClanName.Array.Length;
                long clanimage = BitConverter.ToInt64(array, i); i += 8;
                int players = BitConverter.ToInt32(array, i); i += 4;
                int lands = BitConverter.ToInt32(array, i); i += 4;

                PutBorder(pos, ClanName.ToString(), clanimage, players, lands, clanid);

            }
        }
        public void PutRequestClanInfo(byte[] array)
        {
            int i = 2;
            int clanid = BitConverter.ToInt32(array, i); i += 4;
            if (clanid > MaxClanID) MaxClanID = clanid;
            if (clanid < MinClanID) MinClanID = clanid;
            GSString ClanName = new GSString(array, i); i += ClanName.Array.Length;
            long clanimage = BitConverter.ToInt64(array, i); i += 8;
            int players = BitConverter.ToInt32(array, i); i += 4;
            int lands = BitConverter.ToInt32(array, i); i += 4;
            Border border = PutBorder(5, ClanName.ToString(), clanimage, players, lands, clanid);
            border.BorderBrush = Brushes.Red;
            lastborder = border;
            Grid.SetColumnSpan(border, 2);
            border.HorizontalAlignment = HorizontalAlignment.Center;
            border.Margin = new Thickness(100, 0, 0, 0);
        }
        public Border PutBorder(int pos, string ClanName, long ClanImage, int players, int lands, int clanid)
        {
            Border border = new Border();
            border.Tag = clanid;
            CurBorders.Add(border);
            border.Width = 200;
            border.Height = 150;
            MainCanvas.Children.Add(border);
            int column = pos % 4 * 200;
            int row = ((int)(pos / 4)) * 150 + 50;
            Canvas.SetLeft(border, column);
            Canvas.SetTop(border, row);
            border.BorderBrush = Brushes.White;
            border.BorderThickness = new Thickness(2);
            border.CornerRadius = new CornerRadius(5);
            Canvas canvas = new Canvas();
            border.Child = canvas;
            if (IsSelectMode)
                border.PreviewMouseDown += new MouseButtonEventHandler(border_PreviewMouseDown);

            ClanEmblem emblem = new ClanEmblem(ClanImage);
            canvas.Children.Add(emblem);
            Canvas.SetLeft(emblem, 65);
            Canvas.SetTop(emblem, 5);

            Label lbl = new Label();
            lbl.Content = ClanName;
            lbl.Width = 200;
            lbl.FontFamily = Links.Font;
            lbl.Foreground = Brushes.White;
            lbl.FontSize = 24;
            canvas.Children.Add(lbl);
            lbl.HorizontalContentAlignment = HorizontalAlignment.Center;
            Canvas.SetTop(lbl, 110);

            Rectangle crown = Common.GetRectangle(50, Links.Brushes.CrownBrush);
            canvas.Children.Add(crown);
            Canvas.SetLeft(crown, 5);
            Canvas.SetTop(crown, 20);
            crown.ToolTip = Links.Interface("PlayersCount");

            Label pllbl = new Label();
            pllbl.Content = players;
            pllbl.Width = 60;
            pllbl.FontFamily = Links.Font;
            pllbl.Foreground = Brushes.White;
            pllbl.FontSize = 24;
            canvas.Children.Add(pllbl);
            pllbl.HorizontalContentAlignment = HorizontalAlignment.Center;
            Canvas.SetTop(pllbl, 70);

            Rectangle landicon = Common.GetRectangle(50, Links.Brushes.LandIconBrush);
            canvas.Children.Add(landicon);
            Canvas.SetLeft(landicon, 140);
            Canvas.SetTop(landicon, 20);
            landicon.ToolTip = Links.Interface("LandsCount");

            Label landslbl = new Label();
            landslbl.Content = lands;
            landslbl.Width = 60;
            landslbl.FontFamily = Links.Font;
            landslbl.Foreground = Brushes.White;
            landslbl.FontSize = 24;
            canvas.Children.Add(landslbl);
            landslbl.HorizontalContentAlignment = HorizontalAlignment.Center;
            Canvas.SetTop(landslbl, 70);
            Canvas.SetLeft(landslbl, 135);

            return border;
        }

        void border_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Border border = (Border)sender;
            if (lastborder != null)
                if (lastborder == border)
                {
                    lastborder = null;
                    border.BorderBrush = Brushes.White;
                    SendRequest.SetDisabled();
                    return;
                }
                else
                    lastborder.BorderBrush = Brushes.White;
            border.BorderBrush = Brushes.Red;
            lastborder = border;
            SendRequest.SetEnabled();
        }

        void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Links.Controller.PopUpCanvas.Remove();
        }
    }

    class CreateClanBorder : Border
    {
        public static ScrolledPanelClanEmblem MainColor;
        public static ScrolledPanelClanEmblem LinesPanel;
        public static ScrolledPanelClanEmblem LineColor;
        public static ScrolledPanelClanEmblem CenterPanel;
        public static ScrolledPanelClanEmblem CenterColorPanel;
        public static ClanEmblem Emblem;
        public static GSTextBox ClanNameTB;
        public CreateClanBorder()
        {
            Width = 700;
            Height = 500;
            Background = Brushes.Black;
            BorderBrush = Brushes.White;
            BorderThickness = new Thickness(3);
            CornerRadius = new CornerRadius(20);

            Canvas MainCanvas = new Canvas();
            Child = MainCanvas;

            MainColor = new ScrolledPanelClanEmblem(GetRectangles1(ClanEmblem.RG), 7);
            MainCanvas.Children.Add(MainColor);
            Canvas.SetTop(MainColor, 30);

            LinesPanel = new ScrolledPanelClanEmblem(GetLinesPathes(ClanEmblem.Lines), 7);
            MainCanvas.Children.Add(LinesPanel);
            Canvas.SetLeft(LinesPanel, 70);
            Canvas.SetTop(LinesPanel, 30);

            LineColor = new ScrolledPanelClanEmblem(GetRectangles1(ClanEmblem.RG), 7);
            MainCanvas.Children.Add(LineColor);
            Canvas.SetLeft(LineColor, 140);
            Canvas.SetTop(LineColor, 30);

            CenterPanel = new ScrolledPanelClanEmblem(GetLinesPathes(ClanEmblem.Centers), 7);
            MainCanvas.Children.Add(CenterPanel);
            Canvas.SetLeft(CenterPanel, 210);
            Canvas.SetTop(CenterPanel, 30);

            CenterColorPanel = new ScrolledPanelClanEmblem(GetRectangles(ClanEmblem.CenterBrushes), 7);
            MainCanvas.Children.Add(CenterColorPanel);
            Canvas.SetLeft(CenterColorPanel, 280);
            Canvas.SetTop(CenterColorPanel, 30);

            Viewbox emblembox = new Viewbox();
            MainCanvas.Children.Add(emblembox);
            Canvas.SetLeft(emblembox, 400);
            Canvas.SetTop(emblembox, 100);
            emblembox.Width = 200;
            emblembox.Height = 200;
            Emblem = new ClanEmblem(0);
            emblembox.Child = Emblem;

            ClanNameTB = new GSTextBox();
            ClanNameTB.SetMaxLength(14);
            ClanNameTB.FontFamily = Links.Font;
            ClanNameTB.FontSize = 24;
            MainCanvas.Children.Add(ClanNameTB);
            Canvas.SetLeft(ClanNameTB, 400);
            Canvas.SetTop(ClanNameTB, 350);
            ClanNameTB.Width = 200;
            ClanNameTB.Height = 40;
            ClanNameTB.HorizontalContentAlignment = HorizontalAlignment.Center;

            InterfaceButton CloseButton = new InterfaceButton(100, 50, 7, 20);
            CloseButton.SetText(Links.Interface("Cancel"));
            CloseButton.PutToCanvas(MainCanvas, 510, 400);
            CloseButton.PreviewMouseDown += CloseButton_Click;

            InterfaceButton OkButton = new InterfaceButton(100, 50, 7, 20);
            OkButton.SetText(Links.Interface("Ok"));
            OkButton.PutToCanvas(MainCanvas, 390, 400);
            OkButton.PreviewMouseDown += OkButton_Click;
        }

        void OkButton_Click(object sender, RoutedEventArgs e)
        {
            string result = Events.CreateNewClan(Emblem.Image, new GSString(ClanNameTB.Text));
            Links.Controller.PopUpCanvas.Remove();
            if (result != "")
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(result), true);
            else
                Links.Controller.Nation.Select();
        }

        void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Links.Controller.PopUpCanvas.Remove();
        }
        Rectangle[] GetRectangles(Brush[] brushes)
        {
            Rectangle[] rects = new Rectangle[brushes.Length];
            for (int i = 0; i < brushes.Length; i++)
            {
                rects[i] = Common.GetRectangle(30, brushes[i]);
            }
            return rects;
        }
        Rectangle[] GetRectangles1(Color[] colors)
        {
            Rectangle[] rects = new Rectangle[colors.Length];
            for (int i = 0; i < colors.Length; i++)
            {
                rects[i] = Common.GetRectangle(30, new SolidColorBrush(colors[i]));
            }
            return rects;
        }
        Viewbox GetLinesPath(PathGeometry geom)
        {
            Path path = new Path(); path.Width = 70; path.Height = 100;
            path.Data = geom;
            path.Fill = Brushes.White;
            Viewbox vbx = new Viewbox();
            vbx.Child = path;
            return vbx;
        }
        Viewbox[] GetLinesPathes(PathGeometry[] array)
        {
            Viewbox[] result = new Viewbox[array.Length];
            for (int i = 0; i < array.Length; i++)
                result[i] = GetLinesPath(array[i]);
            return result;
        }
    }
    class GSTextBox : TextBox
    {
        string LastText = "";
        new int MaxLength = 1000;
        public GSTextBox()
        {
            TextChanged += new TextChangedEventHandler(GSTextBox_TextChanged);
        }
        public void SetMaxLength(int value)
        {
            MaxLength = value;
            if (LastText.Length > MaxLength)
            {
                LastText.Remove(MaxLength);
                Text = LastText;
            }
        }
        void GSTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (GSString.CheckString(Text) && Text.Length <= MaxLength)
                LastText = Text;
            else
                Text = LastText;
        }


    }
    public class ScrolledPanelClanEmblem : StackPanel
    {
        object[] Array;
        int Elements;
        public byte currentid = 0;

        public ScrolledPanelClanEmblem(object[] array, int elements)
        {
            VerticalAlignment = VerticalAlignment.Center;
            //Background = Brushes.DarkGray;
            Array = array;
            Elements = elements;
            Width = 60;
            Place();
            PreviewMouseWheel += new MouseWheelEventHandler(ScrolledPanel_PreviewMouseWheel);
        }

        void ScrolledPanel_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0) currentid = (byte)((currentid - 1 + Array.Length) % Array.Length);
            else currentid = (byte)((currentid + 1) % Array.Length);
            Place();
        }

        void bord_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Border border = (Border)sender;
            int tag = (int)border.Tag;
            currentid = (byte)tag;
            Place();
        }
        void Place()
        {
            Children.Clear();
            int delta = (int)Elements / 2;
            int first = Array.Length - delta + currentid;
            for (int i = first; i < first + Elements; i++)
            {
                Label lbl = new Label();
                int pos = i % Array.Length;
                //lbl.Foreground = Brushes.White;
                lbl.Content = Array[pos];
                lbl.HorizontalAlignment = HorizontalAlignment.Center;
                lbl.VerticalAlignment = VerticalAlignment.Center;
                lbl.Height = 60;
                if ((pos) == currentid) lbl.Background = Brushes.Red;
                //lbl.FontWeight = FontWeights.Bold;
                Border bord = new Border();
                bord.BorderBrush = Brushes.White;
                bord.BorderThickness = new Thickness(1);
                bord.Child = lbl;
                Children.Add(bord);

                bord.Tag = pos;
                bord.PreviewMouseDown += new MouseButtonEventHandler(bord_PreviewMouseDown);

                byte[] list = new byte[8];
                if (CreateClanBorder.MainColor != null) list[0] = CreateClanBorder.MainColor.currentid;
                if (CreateClanBorder.LinesPanel != null) list[1] = CreateClanBorder.LinesPanel.currentid;
                if (CreateClanBorder.LineColor != null) list[2] = CreateClanBorder.LineColor.currentid;
                if (CreateClanBorder.CenterPanel != null) list[3] = CreateClanBorder.CenterPanel.currentid;
                if (CreateClanBorder.CenterColorPanel != null) list[4] = CreateClanBorder.CenterColorPanel.currentid;
                if (CreateClanBorder.Emblem != null)
                    CreateClanBorder.Emblem.SetImage(list);
            }
        }
    }
}
