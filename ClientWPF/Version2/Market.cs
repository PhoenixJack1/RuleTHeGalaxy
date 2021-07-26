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
    class Market:Canvas
    {
        public Viewbox box;
        Grid ResourceGrid;
        Label BuyMetal, SellMetal, BuyChips, SellChips, BuyAnti, SellAnti;//, BuyZip, SellZip;
        public enum ResButtons { V0, V000, V1, V2, V3, V4, V5, V6, V7, V8, V9, All, Ok, Clear, BackSpace };
        public enum ResEllipses { BuyMetal, SellMetal, BuyChips, SellChips, BuyAnti, SellAnti };
        public InterfaceButton ChangeButton;
        Border Pilot1, Pilot2, Pilot3, Pilot4;
        long ChangeValue;
        TextBox ValueBox;
        Ellipse CurrentResEllipse;
        double[] Prices;
        //List<GSPilot> PilotsList;
        public Market()
        {
            box = new Viewbox();
            box.Width = 1230; box.Height = 520;
            box.HorizontalAlignment = HorizontalAlignment.Center;
            box.VerticalAlignment = VerticalAlignment.Center;
            box.Child = this;
            Width = 1280; Height = 600;
            put();
        }

        public void ButtonPressed(Key key)
        {
            switch (key)
            {
                case Key.D1: case Key.NumPad1: ResButtonClickResult(ResButtons.V1, "2"); break;
                case Key.D2: case Key.NumPad2: ResButtonClickResult(ResButtons.V2, "2"); break;
                case Key.D3: case Key.NumPad3: ResButtonClickResult(ResButtons.V3, "2"); break;
                case Key.D4: case Key.NumPad4: ResButtonClickResult(ResButtons.V4, "2"); break;
                case Key.D5: case Key.NumPad5: ResButtonClickResult(ResButtons.V5, "2"); break;
                case Key.D6: case Key.NumPad6: ResButtonClickResult(ResButtons.V6, "2"); break;
                case Key.D7: case Key.NumPad7: ResButtonClickResult(ResButtons.V7, "2"); break;
                case Key.D8: case Key.NumPad8: ResButtonClickResult(ResButtons.V8, "2"); break;
                case Key.D9: case Key.NumPad9: ResButtonClickResult(ResButtons.V9, "2"); break;
                case Key.D0: case Key.NumPad0: ResButtonClickResult(ResButtons.V0, "2"); break;
                case Key.Back: ResButtonClickResult(ResButtons.BackSpace, "2"); break;
                case Key.Enter: ResButtonClickResult(ResButtons.Ok, "2"); break;

            }
        }

        public void Select()
        {
            Refresh();
        }
        public void Refresh()
        {
            Gets.GetTotalInfo("После открытия панели рынка");
            double[] prices = Gets.GetMarketPrices();
            Prices = prices;
            BuyMetal.Content = Math.Round(prices[0] * 1.05, 2).ToString("#.00");
            SellMetal.Content = Math.Round(prices[0] / 1.05, 2).ToString("#.00");
            BuyChips.Content = Math.Round(prices[1] * 1.05, 2).ToString("#.00");
            SellChips.Content = Math.Round(prices[1] / 1.05, 2).ToString("#.00");
            BuyAnti.Content = Math.Round(prices[2] * 1.05, 2).ToString("#.00");
            SellAnti.Content = Math.Round(prices[2] / 1.05, 2).ToString("#.00");
            //BuyZip.Content = Math.Round(prices[3] * 1.05, 2).ToString("#.00");
            //SellZip.Content = Math.Round(prices[3] / 1.05, 2).ToString("#.00");


            if (GSGameInfo.AcademyPilotsList.Count > 0)
                Pilot1.Child = new PilotsImage(GSGameInfo.AcademyPilotsList[0], PilotsListMode.Clear, null);
            else Pilot1.Child = null;
            if (GSGameInfo.AcademyPilotsList.Count > 1)
                Pilot2.Child = new PilotsImage(GSGameInfo.AcademyPilotsList[1], PilotsListMode.Clear, null);
            else Pilot2.Child = null;
            if (GSGameInfo.AcademyPilotsList.Count > 2)
                Pilot3.Child = new PilotsImage(GSGameInfo.AcademyPilotsList[2], PilotsListMode.Clear, null);
            else Pilot3.Child = null;
            if (GSGameInfo.AcademyPilotsList.Count > 3)
                Pilot4.Child = new PilotsImage(GSGameInfo.AcademyPilotsList[3], PilotsListMode.Clear, null);
            else Pilot4.Child = null;

            //HelpWindow.Place(HelpWindows.Market);
        }
        public void put()
        {
            Background = Brushes.Black;

            Border ResourceBorder = new Border();
            Children.Add(ResourceBorder);
            ResourceBorder.BorderThickness = new Thickness(2);
            ResourceBorder.BorderBrush = Brushes.Black;
            ResourceBorder.Width = 606;
            ResourceBorder.Height = 600;
            ResourceBorder.Margin = new Thickness(2);

            ResourceGrid = new Grid();
            ResourceBorder.Child = ResourceGrid;
            //ResourceGrid.ShowGridLines = true;
            ResourceGrid.ColumnDefinitions.Add(new ColumnDefinition());
            ResourceGrid.ColumnDefinitions.Add(new ColumnDefinition());
            ResourceGrid.ColumnDefinitions.Add(new ColumnDefinition());
            ResourceGrid.ColumnDefinitions.Add(new ColumnDefinition());
            ResourceGrid.ColumnDefinitions.Add(new ColumnDefinition());


            ResourceGrid.RowDefinitions.Add(new RowDefinition());
            ResourceGrid.RowDefinitions.Add(new RowDefinition());
            ResourceGrid.RowDefinitions.Add(new RowDefinition());
            ResourceGrid.RowDefinitions.Add(new RowDefinition());
            ResourceGrid.RowDefinitions.Add(new RowDefinition());
            ResourceGrid.RowDefinitions.Add(new RowDefinition());
            ResourceGrid.RowDefinitions.Add(new RowDefinition());
            ResourceGrid.RowDefinitions.Add(new RowDefinition());
            ResourceGrid.RowDefinitions.Add(new RowDefinition());
            ResourceGrid.RowDefinitions.Add(new RowDefinition());

            Label ResourceTitleLabel = GetResLabel(Links.Interface("ResourceMarket"), 0, 0);
            ResourceTitleLabel.VerticalAlignment = VerticalAlignment.Top;
            Grid.SetColumnSpan(ResourceTitleLabel, 5);

            Label BuyResLabel = GetResLabel(Links.Interface("Buy"), 0, 1);
            BuyResLabel.VerticalAlignment = VerticalAlignment.Bottom;

            Label SellResLabel = GetResLabel(Links.Interface("Sell"), 0, 3);
            SellResLabel.VerticalAlignment = VerticalAlignment.Bottom;


            Rectangle MetalRectangle = GetSpecRectangle(Links.Brushes.MetalImageBrush, 1, 2);
            ResourceGrid.Children.Add(MetalRectangle);
            BuyMetal = GetResLabel("BuyMetal", 1, 1);
            SellMetal = GetResLabel("SellMetal", 1, 3);

            Rectangle ChipsRectangle = GetSpecRectangle(Links.Brushes.ChipsImageBrush, 2, 2);
            ResourceGrid.Children.Add(ChipsRectangle);
            BuyChips = GetResLabel("BuyChips", 2, 1);
            SellChips = GetResLabel("SellChips", 2, 3);

            Rectangle AntiRectangle = GetSpecRectangle(Links.Brushes.AntiImageBrush, 3, 2);
            ResourceGrid.Children.Add(AntiRectangle);
            BuyAnti = GetResLabel("BuyAnti", 3, 1);
            SellAnti = GetResLabel("SellAnti", 3, 3);

            GetResButton("0", 9, 1, ResButtons.V0);
            GetResButton("000", 9, 2, ResButtons.V000);
            GetResButton("1", 8, 1, ResButtons.V1);
            GetResButton("2", 8, 2, ResButtons.V2);
            GetResButton("3", 8, 3, ResButtons.V3);
            GetResButton("4", 7, 1, ResButtons.V4);
            GetResButton("5", 7, 2, ResButtons.V5);
            GetResButton("6", 7, 3, ResButtons.V6);
            GetResButton("7", 6, 1, ResButtons.V7);
            GetResButton("8", 6, 2, ResButtons.V8);
            GetResButton("9", 6, 3, ResButtons.V9);
            GetResButton(Links.Interface("All"), 9, 3, ResButtons.All);
            ChangeButton = GetResButton(Links.Interface("Change"), 9, 4, ResButtons.Ok);
            GetResButton(Links.Interface("Clear"), 5, 0, ResButtons.Clear);
            GetResButton("<-", 5, 4, ResButtons.BackSpace);

            ValueBox = new TextBox();
            ResourceGrid.Children.Add(ValueBox);
            Grid.SetRow(ValueBox, 5);
            Grid.SetColumn(ValueBox, 1);
            Grid.SetColumnSpan(ValueBox, 3);
            ValueBox.Style = Links.ButtonStyle;
            ValueBox.FontSize = 30;
            ValueBox.Foreground = Brushes.DeepPink;
            ValueBox.HorizontalContentAlignment = HorizontalAlignment.Center;
            ValueBox.VerticalContentAlignment = VerticalAlignment.Center;
            ValueBox.IsEnabled = false;

            GetResEllipse(1, 0, ResEllipses.BuyMetal);
            GetResEllipse(1, 4, ResEllipses.SellMetal);
            GetResEllipse(2, 0, ResEllipses.BuyChips);
            GetResEllipse(2, 4, ResEllipses.SellChips);
            GetResEllipse(3, 0, ResEllipses.BuyAnti);
            GetResEllipse(3, 4, ResEllipses.SellAnti);


            Border PilotBorder = new Border();
            Children.Add(PilotBorder);
            PilotBorder.BorderThickness = new Thickness(2);
            PilotBorder.BorderBrush = Brushes.Black;
            PilotBorder.Width = 656;
            PilotBorder.Height = 600;
            Canvas.SetLeft(PilotBorder, 610);
            PilotBorder.Margin = new Thickness(2);

            Grid PilotsGrid = new Grid();
            PilotBorder.Child = PilotsGrid;
            PilotsGrid.ColumnDefinitions.Add(new ColumnDefinition());
            PilotsGrid.ColumnDefinitions.Add(new ColumnDefinition());
            PilotsGrid.ShowGridLines = true;
            PilotsGrid.RowDefinitions.Add(new RowDefinition()); PilotsGrid.RowDefinitions[0].Height = new GridLength(50);
            PilotsGrid.RowDefinitions.Add(new RowDefinition());
            PilotsGrid.RowDefinitions.Add(new RowDefinition()); PilotsGrid.RowDefinitions[2].Height = new GridLength(50);
            PilotsGrid.RowDefinitions.Add(new RowDefinition());
            PilotsGrid.RowDefinitions.Add(new RowDefinition()); PilotsGrid.RowDefinitions[4].Height = new GridLength(50);
            PilotsGrid.RowDefinitions.Add(new RowDefinition()); PilotsGrid.RowDefinitions[5].Height = new GridLength(50);

            Pilot1 = GetPilotsBorder(1, 0, PilotsGrid);
            Pilot2 = GetPilotsBorder(1, 1, PilotsGrid);
            Pilot3 = GetPilotsBorder(3, 0, PilotsGrid);
            Pilot4 = GetPilotsBorder(3, 1, PilotsGrid);

            Label PilotTitleLabel = new Label();
            PilotsGrid.Children.Add(PilotTitleLabel);
            PilotTitleLabel.Content = Links.Interface("Academy");
            PilotTitleLabel.HorizontalAlignment = HorizontalAlignment.Center;
            PilotTitleLabel.VerticalAlignment = VerticalAlignment.Center;
            PilotTitleLabel.Style = Links.TextStyle;
            Grid.SetColumnSpan(PilotTitleLabel, 2);
            /*
            Button ChangePilotsBtn = new Button();
            PilotsGrid.Children.Add(ChangePilotsBtn);
            ChangePilotsBtn.Content = Links.Interface("NewRelease");
            Grid.SetRow(ChangePilotsBtn, 5);
            Grid.SetColumnSpan(ChangePilotsBtn, 2);
            ChangePilotsBtn.HorizontalAlignment = HorizontalAlignment.Center;
            ChangePilotsBtn.Margin = new Thickness(5);
            ChangePilotsBtn.Style = Links.ButtonStyle;
            ChangePilotsBtn.Click += new RoutedEventHandler(ChangePilotsBtn_Click);
            */
            AddPilotsButton(PilotsGrid, 5, 0, "NewRelease", ChangePilotsBtn_Click, 0);
            AddPilotsButton(PilotsGrid, 2, 0, "Hire", HireBtn_Click, 0);
            AddPilotsButton(PilotsGrid, 2, 1, "Hire", HireBtn_Click, 1);
            AddPilotsButton(PilotsGrid, 4, 0, "Hire", HireBtn_Click, 2);
            AddPilotsButton(PilotsGrid, 4, 1, "Hire", HireBtn_Click, 3);
            AddPilotsButton(PilotsGrid, 5, 1, "Barracks", TavernBtn_Click, 0);
        }
        void TavernBtn_Click(object sender, RoutedEventArgs e)
        {
            PilotsListPanel panel = new PilotsListPanel(PilotsListMode.Academy);
            Links.Controller.PopUpCanvas.Place(panel, false);
        }
        void HireBtn_Click(object sender, RoutedEventArgs e)
        {
            InterfaceButton btn = (InterfaceButton)sender;
            byte tag = (byte)btn.Tag;
            string hireresult = Events.HirePilot(tag);
            string answer = "";
            if (hireresult == "")
                answer = String.Format(Links.Interface("HireResult"), GSGameInfo.AcademyPilotsList[tag].GetName(),
                    GSGameInfo.AcademyPilotsList[tag].GetNick(), GSGameInfo.AcademyPilotsList[tag].GetSurname());
            else answer = hireresult;
            Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(answer), true);
            Refresh();
        }
        void AddPilotsButton(Grid grid, int row, int column, string textPos, MouseButtonEventHandler click, byte tag)
        {
            InterfaceButton btn = new InterfaceButton(120, 50, 7, 20);
            grid.Children.Add(btn);
            btn.SetText(Links.Interface(textPos));
            Grid.SetRow(btn, row);
            Grid.SetColumn(btn, column);
            btn.HorizontalAlignment = HorizontalAlignment.Center;
            btn.PreviewMouseDown += click;
            btn.Tag = tag;
        }
        void ChangePilotsBtn_Click(object sender, RoutedEventArgs e)
        {
            if (GSGameInfo.Money < 10000)
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(String.Format(Links.Interface("MoneyNeed"), 10000)), true);
            else
            {
                string result = Events.ChangePilotsRelease();
                if (result == "")
                    Refresh();
                else Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(result), true);
            }
        }
        Border GetPilotsBorder(int row, int column, Grid parent)
        {
            Border result = new Border();
            parent.Children.Add(result);
            Grid.SetRow(result, row);
            Grid.SetColumn(result, column);
            result.Width = 200;
            result.Height = 200;
            result.BorderBrush = Brushes.Black;
            result.BorderThickness = new Thickness(2);
            return result;
        }
        void GetResEllipse(int row, int column, ResEllipses mark)
        {
            Ellipse el1 = new Ellipse();
            el1.Width = 50;
            el1.Height = 50;
            el1.Stroke = Brushes.Black;
            el1.StrokeThickness = 5;
            ResourceGrid.Children.Add(el1);
            Grid.SetRow(el1, row);
            Grid.SetColumn(el1, column);
            el1.Tag = mark;
            el1.Fill = Brushes.White;
            el1.PreviewMouseDown += new MouseButtonEventHandler(el1_PreviewMouseDown);
        }

        void el1_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Ellipse el = (Ellipse)sender;
            ResEllipses mark = (ResEllipses)el.Tag;
            if (CurrentResEllipse == null)
            {
                CurrentResEllipse = el;
                el.Fill = Links.Brushes.AddImageBrush;
            }
            else if (CurrentResEllipse != el)
            {
                CurrentResEllipse.Fill = Brushes.White;
                CurrentResEllipse = el;
                el.Fill = Links.Brushes.AddImageBrush;
            }
            else
            {
                CurrentResEllipse = null;
                el.Fill = Brushes.White;
            }
        }
        InterfaceButton GetResButton(string text, int row, int column, ResButtons mark)
        {
            InterfaceButton btn = new InterfaceButton(100, 50, 7, 20);
            ResourceGrid.Children.Add(btn);
            Grid.SetRow(btn, row);
            Grid.SetColumn(btn, column);
            btn.Tag = mark;
            btn.SetText(text);
            btn.PreviewMouseDown += btn_Click;
            return btn;
        }
        public void ResButtonClickResult(ResButtons mark, string reason)
        {
            switch (mark)
            {
                case ResButtons.V0:
                    ChangeValue *= 10; break;
                case ResButtons.V1:
                    ChangeValue = ChangeValue * 10 + 1; break;
                case ResButtons.V2:
                    ChangeValue = ChangeValue * 10 + 2; break;
                case ResButtons.V3:
                    ChangeValue = ChangeValue * 10 + 3; break;
                case ResButtons.V4:
                    ChangeValue = ChangeValue * 10 + 4; break;
                case ResButtons.V5:
                    ChangeValue = ChangeValue * 10 + 5; break;
                case ResButtons.V6:
                    ChangeValue = ChangeValue * 10 + 6; break;
                case ResButtons.V7:
                    ChangeValue = ChangeValue * 10 + 7; break;
                case ResButtons.V8:
                    ChangeValue = ChangeValue * 10 + 8; break;
                case ResButtons.V9:
                    ChangeValue = ChangeValue * 10 + 9; break;
                case ResButtons.V000:
                    ChangeValue = ChangeValue * 1000; break;
                case ResButtons.Clear:
                    ChangeValue = 0; break;
                case ResButtons.BackSpace:
                    ChangeValue = ChangeValue / 10; break;
                case ResButtons.Ok:
                    if (CurrentResEllipse != null) TryExchenge(); break;
                case ResButtons.All: ChangeValue = CalculateAllValue(); break;

            }
            if (ChangeValue > 1000000) ChangeValue = 1000000;
            ValueBox.Text = ChangeValue.ToString("# ### ###.#");
            ChangeButton.Focus();
        }
        int CalculateAllValue()
        {
            if (CurrentResEllipse == null) return 0;
            ResEllipses mark = (ResEllipses)CurrentResEllipse.Tag;
            switch (mark)
            {
                case ResEllipses.BuyMetal:
                    int TotalMetal = (int)((double)GSGameInfo.Money / Math.Round(Prices[0] * 1.05, 2));
                    int MetalCapacity = GSGameInfo.Capacity.Metal - GSGameInfo.Metals;
                    return TotalMetal > MetalCapacity ? MetalCapacity : TotalMetal;
                case ResEllipses.BuyChips:
                    int TotalChips = (int)((double)GSGameInfo.Money / Math.Round(Prices[1] * 1.05, 2));
                    int ChipsCapacity = GSGameInfo.Capacity.Chips - GSGameInfo.Chips;
                    return TotalChips > ChipsCapacity ? ChipsCapacity : TotalChips;
                case ResEllipses.BuyAnti:
                    int TotalAnti = (int)((double)GSGameInfo.Money / Math.Round(Prices[2] * 1.05, 2));
                    int AntiCapacity = GSGameInfo.Capacity.Anti - GSGameInfo.Anti;
                    return TotalAnti > AntiCapacity ? AntiCapacity : TotalAnti;
                case ResEllipses.SellMetal:
                    return GSGameInfo.Metals;
                case ResEllipses.SellChips:
                    return GSGameInfo.Chips;
                case ResEllipses.SellAnti:
                    return GSGameInfo.Anti;
                default: return 0;
            }
        }
        void TryExchenge()
        {
            ResEllipses mark = (ResEllipses)CurrentResEllipse.Tag;
            long price;
            if (ChangeValue == 0) return;
            string text = "";
            bool Sucsess = false;
            switch (mark)
            {
                case ResEllipses.BuyMetal:
                    price = (long)(ChangeValue * Math.Round(Prices[0] * 1.05, 2));
                    if (GSGameInfo.Money < price)
                        text = string.Format(Links.Interface("MoneyNeed"), price.ToString("# ### ###"));
                    else
                    {
                        string result = Events.ChangeResources(0, (int)ChangeValue);
                        if (result == "") { text = String.Format(Links.Interface("MoneyToMetal"), price, ChangeValue); Sucsess = true; }
                        else text = result;
                    }
                    break;
                case ResEllipses.SellMetal:
                    if (GSGameInfo.Metals < ChangeValue)
                        text = String.Format(Links.Interface("MetalNeed"), ChangeValue.ToString("# ### ###"));
                    else
                    {
                        string result = Events.ChangeResources(1, (int)ChangeValue);
                        if (result == "") { text = String.Format(Links.Interface("MoneyRecieve"), (long)(ChangeValue * Prices[0] / 1.05)); Sucsess = true; }

                    }
                    break;
                case ResEllipses.BuyChips:
                    price = (long)(ChangeValue * Math.Round(Prices[1] * 1.05, 2));
                    if (GSGameInfo.Money < price)
                        text = string.Format(Links.Interface("MoneyNeed"), price.ToString("# ### ###"));
                    else
                    {
                        string result = Events.ChangeResources(2, (int)ChangeValue);
                        if (result == "") { text = String.Format(Links.Interface("MoneyToChips"), price, ChangeValue); Sucsess = true; }
                    }
                    break;
                case ResEllipses.SellChips:
                    if (GSGameInfo.Chips < ChangeValue)
                        text = String.Format(Links.Interface("ChipsNeed"), ChangeValue.ToString("# ### ###"));
                    else
                    {
                        string result = Events.ChangeResources(3, (int)ChangeValue);
                        if (result == "") { text = String.Format(Links.Interface("MoneyRecieve"), (long)(ChangeValue * Prices[1] / 1.05)); Sucsess = true; }
                        else text = result;
                    }
                    break;
                case ResEllipses.BuyAnti:
                    price = (long)(ChangeValue * Math.Round(Prices[2] * 1.05, 2)); ;
                    if (GSGameInfo.Money < price)
                        text = string.Format(Links.Interface("MoneyNeed"), price.ToString("# ### ###"));
                    else
                    {
                        string result = Events.ChangeResources(4, (int)ChangeValue);
                        if (result == "") { text = String.Format(Links.Interface("MoneyToAnti"), price, ChangeValue); Sucsess = true; }
                    }
                    break;
                case ResEllipses.SellAnti:
                    if (GSGameInfo.Anti < ChangeValue)
                        text = String.Format(Links.Interface("AntiNeed"), ChangeValue.ToString("# ### ###"));
                    else
                    {
                        string result = Events.ChangeResources(5, (int)ChangeValue);
                        if (result == "") { text = String.Format(Links.Interface("MoneyRecieve"), (long)(ChangeValue * Prices[2] / 1.05)); Sucsess = true; }
                        else text = result;
                    }
                    break;

            }
            if (Sucsess)
            {
                System.Threading.Tasks.TaskFactory fact = new System.Threading.Tasks.TaskFactory();
                fact.StartNew(GetResources);
                //Gets.GetResources();

                //if (GSGameInfo.Quest == Quest_Position.Q17_Make_Trade)
                //    Gets.GetTotalInfo("После);
            }
            Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(text), true);

        }
        public void GetResources()
        {
            Links.Controller.MainCanvas.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)Gets.GetResources);
        }
        void btn_Click(object sender, RoutedEventArgs e)
        {
            InterfaceButton btn = (InterfaceButton)sender;
            ResButtons mark = (ResButtons)btn.Tag;
            ResButtonClickResult(mark, "1");
        }
        Label GetResLabel(string text, int row, int column)
        {
            Label result = new Label();
            result.Style = Links.TextStyle;
            result.Content = text;
            result.VerticalAlignment = VerticalAlignment.Center;
            result.HorizontalAlignment = HorizontalAlignment.Center;
            ResourceGrid.Children.Add(result);
            Grid.SetRow(result, row);
            Grid.SetColumn(result, column);
            result.Foreground = Brushes.White;
            return result;
        }
        Rectangle GetSpecRectangle(Brush brush, int row, int column)
        {
            Rectangle result = new Rectangle();
            result.Width = 50;
            result.Height = 50;
            result.HorizontalAlignment = HorizontalAlignment.Center;
            result.VerticalAlignment = VerticalAlignment.Center;
            result.Fill = brush;
            Grid.SetRow(result, row);
            Grid.SetColumn(result, column);
            return result;
        }
    }
}
