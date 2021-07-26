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

namespace Client
{
    class SelectedGrid : Grid
    {
        protected string gridName;
        public Path Button;
        public new bool IsEnabled = true;
        public SelectedGrid(string Name, Path button)
        {
            this.gridName = Name;
            Label lbl = new Label();
            lbl.Content = Name;
            lbl.FontSize = 50;
            Children.Add(lbl);
            Button = button;
        }
        public virtual void Select()
        {
            if (Links.Controller.CurrentGrid == null)
            {
                Links.Controller.mainWindow.LowBorder.Child = this;
                Links.Controller.CurrentGrid = this;
                if (IsEnabled) Button.Fill = Links.Brushes.SelectedMainBrush; else Button.Fill = Links.Brushes.DisabledMainBrush;
            }
            else
            {
                if (Links.Controller.CurrentGrid == this) return;
                Links.Controller.CurrentGrid.DeSelect();
                Links.Controller.mainWindow.LowBorder.Child = this;
                Links.Controller.CurrentGrid = this;
                if (IsEnabled) Button.Fill = Links.Brushes.SelectedMainBrush; else Button.Fill = Links.Brushes.DisabledMainBrush;
            }




            //MessageBox.Show(gridName + "Selected");
        }
        public virtual void DeSelect()
        {

            Links.Controller.LastGrid = this;
            if (IsEnabled) Button.Fill = Links.Brushes.NotSelectedMainBrush; else Button.Fill = Links.Brushes.DisabledMainBrush;
            //if (Links.Controller.galaxypanel.Popup.IsOpen)
           // {
           //         Links.Controller.galaxypanel.Popup.IsOpen = false;
           //         Links.Controller.galaxypanel.PopUpElement.ToolTip = "";
           // }
           // else if (Links.Controller.sectorpanel.Popup.IsOpen)
           // {
           //     Links.Controller.sectorpanel.Popup.IsOpen = false;
           //     Links.Controller.sectorpanel.PopUpElement.ToolTip = "";
           // }
            //MessageBox.Show(gridName + "Closed");
        }
        public virtual void Enable()
        {
            IsEnabled = true;
            if (Links.Controller.CurrentGrid == this) Button.Fill = Links.Brushes.SelectedMainBrush;
            else Button.Fill = Links.Brushes.NotSelectedMainBrush;
        }
        public virtual void Disable()
        {
            IsEnabled = false;
            Button.Fill = Links.Brushes.DisabledMainBrush;

        }
    }
    class BlankPanel:SelectedGrid
    {
        public SelectedGrid PreviousGrid;
        public BlankPanel(string Name):base(Name, new Path())
        {
            Children.Clear();
        }
        public override void Select()
        {
            PreviousGrid = Links.Controller.CurrentGrid;
            base.Select();
            Links.Controller.MainTimer.Stop();
        }
        public override void DeSelect()
        {
            base.DeSelect();
            //PreviousGrid.Select();
            Gets.GetResources();
            Links.Controller.MainTimer.Start();
        }
    }
    class ChatPanel:SelectedGrid
    {
        System.Timers.Timer timer;
        ScrollViewer Scroll;
        TextBlock FD;
        TextBox Chat;
        Button SendButton;
        public ChatPanel(string Name):base(Name, Links.Controller.mainWindow.Buttons[10])
        {
            Children.RemoveAt(0);
            RowDefinitions.Add(new RowDefinition()); RowDefinitions[0].Height = new GridLength(15, GridUnitType.Star); //окно для чата
            RowDefinitions.Add(new RowDefinition()); //окно для ввода сообщения
            ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinitions.Add(new ColumnDefinition()); ColumnDefinitions[1].Width = new GridLength(30);
            Border TopBorder = new Border(); TopBorder.BorderBrush = Brushes.Red; TopBorder.BorderThickness = new Thickness(2);
            Children.Add(TopBorder); Grid.SetColumnSpan(TopBorder, 2);
            Scroll = new ScrollViewer(); TopBorder.Child = Scroll; Scroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            
            FD = new TextBlock(); Scroll.Content = FD; FD.FontSize = 18;
            FD.MouseDown += new MouseButtonEventHandler(TB_MouseDown);           
            FD.PreviewKeyDown += new KeyEventHandler(TB_KeyDown);
            FD.Foreground = Brushes.White;
            Chat = new TextBox(); Children.Add(Chat); Grid.SetRow(Chat, 1); Chat.KeyDown += new KeyEventHandler(Chat_KeyDown);

            SendButton = new Button(); Children.Add(SendButton); Grid.SetRow(SendButton, 1); Grid.SetColumn(SendButton, 1);
            SendButton.Content = Links.Interface("Send") ; SendButton.Background = Brushes.Green; SendButton.Click += new RoutedEventHandler(SendButton_Click);
            timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
        }

        void Chat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SendMessage();
        }
        public void AddChatMessages(SortedList<uint, ChatMessage> messages)
        {
            FD.Inlines.Clear();
            //while (FD.Inlines.Count > 50)
            //    FD.Inlines.Remove(FD.Inlines.FirstInline);
            foreach (ChatMessage message in messages.Values)
            {

                Span span = new Span();
                Hyperlink link;
                switch (message.Type)
                {
                    case EChatMessageType.ForAll:
                        link = new Hyperlink(new Run(message.Creator));
                        link.Tag = message.Creator;
                        link.Click += new RoutedEventHandler(link_Click);
                        span.Inlines.Add(link);
                        span.Inlines.Add(new Run(": "));
                        span.Inlines.Add(new Run(message.Text));

                        break;
                    case EChatMessageType.Private:
                        if (message.Creator == GSGameInfo.PlayerName)
                        {
                            link = new Hyperlink(new Run(message.Target));
                            span.Inlines.Add(new Run("To "));
                            link.Tag = message.Target;
                        }
                        else
                        {
                            link = new Hyperlink(new Run(message.Creator));
                            span.Inlines.Add(new Run("From "));
                            link.Tag = message.Creator;
                        }

                        link.Click += new RoutedEventHandler(link_Click);
                        span.Inlines.Add(link);
                        span.Inlines.Add(new Run(": "));
                        Run PrivateText = new Run(message.Text);
                        PrivateText.Foreground = Brushes.Violet;
                        span.Inlines.Add(PrivateText);
                        break;
                    case EChatMessageType.Clan:
                        link = new Hyperlink(new Run(message.Creator));
                        link.Tag = message.Creator;
                        link.Click += new RoutedEventHandler(link_Click);
                        span.Inlines.Add(link);
                        span.Inlines.Add(new Run(": "));
                        Run ClanText = new Run(message.Text);
                        ClanText.Foreground = Brushes.Green;
                        span.Inlines.Add(ClanText);
                        break;
                    case EChatMessageType.System:
                        Run SystemText = new Run(message.Text);
                        SystemText.Foreground = Brushes.Gray;
                        span.Inlines.Add(SystemText);
                        break;
                }
                span.Inlines.Add(new LineBreak());
                FD.Inlines.Add(span);
            }
            Scroll.ScrollToEnd();
        }

        void link_Click(object sender, RoutedEventArgs e)
        {
            AnalysChatLine analys = new AnalysChatLine(Chat.Text);
            string target = (string)(((Hyperlink)sender).Tag);
            Chat.Text = "/" + target + " " + analys.Text;
        }
        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                (System.Threading.ThreadStart)Events.AppendChatMessages);
        }
        public override void Select()
        {
            base.Select();
            timer.Start();
            //HelpWindow.Place(HelpWindows.Chat);
        }
        public override void DeSelect()
        {
            base.DeSelect();
            timer.Stop();
        }
        void SendButton_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }
        void SendMessage()
        {
            AnalysChatLine analys = new AnalysChatLine(Chat.Text);
            if (analys.Type == EAnalysChatLine.Empty || analys.Type == EAnalysChatLine.ForClanEmpty || analys.Type == EAnalysChatLine.PrivateEmpty) return;
            if (analys.Type == EAnalysChatLine.ForClan)
            {
                Chat.Text = "/c ";
                Chat.Select(3, 0);
                ChatMessage message = new ChatMessage(EChatMessageType.Clan, "", "", analys.Text,0);
                Events.SendChatMessage(message);
            }
            else if (analys.Type == EAnalysChatLine.Private)
            {
                Chat.Text = "/" + analys.Target+" ";
                Chat.Select(Chat.Text.Length, 0);
                ChatMessage message = new ChatMessage(EChatMessageType.Private, "", analys.Target, analys.Text,0);
                Events.SendChatMessage(message);
            }
            else
            {
                Chat.Text = "";
                ChatMessage message=new ChatMessage(EChatMessageType.ForAll,"","",analys.Text,0);
                Events.SendChatMessage(message);
            }
        }
        public enum EAnalysChatLine { Empty, ForClanEmpty, PrivateEmpty, ForClan, Private, ForAll }
        class AnalysChatLine
        {
            public string Text { get; private set; }
            public string Target { get; private set; }
            public EAnalysChatLine Type { get; private set; }
            public AnalysChatLine(string ChatText)
            {
                Target = ""; Text = "";
                if (ChatText.Length == 0) { Type = EAnalysChatLine.Empty; return; } //сообщение пустое
                if (ChatText[0] == '/') //сообщение с целью
                {
                    if (ChatText.Length == 1) { Type = EAnalysChatLine.Empty; return; } //слеш есть, но сообщение пустое
                    string shortline = ChatText.Substring(0, 2);
                    if ((shortline == "/a" || shortline == "/A" || shortline == "/а" || shortline == "/А") && (ChatText.Length == 2 || ChatText[2] == ' ')) //сообщение клану
                    {
                        string chatText = "";
                        if (ChatText.Length > 3) chatText = ChatText.Substring(3);
                        if (chatText.Length == 0) { Type = EAnalysChatLine.ForClanEmpty; return; }
                        else { Type = EAnalysChatLine.ForClan; Text = chatText; return; }
                    }
                    else
                    {
                        string target = ChatText.Split(' ')[0];
                        string text = "";
                        if (ChatText != target) text = ChatText.Substring(target.Length + 1);
                        if (text.Length == 0) { Type = EAnalysChatLine.PrivateEmpty; Target = target.Substring(1); return; }
                        else { Type = EAnalysChatLine.Private; Target = target.Substring(1); Text = text; return; }
                    }
                }
                else { Type = EAnalysChatLine.ForAll; Text = ChatText; return; }
            }
        }
        void TB_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            Chat.Focus();
            Chat.Select(Chat.Text.Length, 0);
        }

        void TB_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Chat.Focus();   
            Chat.Select(Chat.Text.Length,0);
        }
    }
}
