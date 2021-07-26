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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client
{
    
    enum HelpWindows { First, Schemas, Market, Chat }
    class HelpWindow:Border
    {
        HelpWindows Pos;
        TextBlock Title;
        TextBlock Block;
        /*
        public static void Place(HelpWindows pos)
        {
            if (GSGameInfo.HelpArray[(int)pos] > 0) return;
            new HelpWindow(pos);
        }
        */
        /*
        public static void Place()
        {
            
            if (Links.Controller.CurrentGrid == Links.Controller.schemaspanel)
            { Place(HelpWindows.Schemas); return; }
            else if (Links.Controller.CurrentGrid==Links.Controller.galaxypanel)
            { Place(HelpWindows.First); return; }
            else if (Links.Controller.CurrentGrid==Links.Controller.marketpanel)
            { Place(HelpWindows.Market); return; }
            else if (Links.Controller.CurrentGrid==Links.Controller.chatpanel)
            { Place(HelpWindows.Chat); return; }
   
        }
        */
        public HelpWindow(HelpWindows pos)
        {
            Pos = pos;
            Width = 1200;
            Height = 660;
            Background = Brushes.Silver;
            BorderBrush = Brushes.White;
            BorderThickness = new Thickness(5);
            CornerRadius = new CornerRadius(20);
            Canvas MainCanvas = new Canvas();
            Child = MainCanvas;
            Button CloseButton = new Button();
            CloseButton.Width = 200; CloseButton.Height = 50;
            CloseButton.Content = Links.Interface("Close");
            CloseButton.FontFamily = Links.Font;
            CloseButton.Background = Brushes.Black;
            CloseButton.Foreground = Brushes.White;
            CloseButton.FontSize = 24;
            CloseButton.FontWeight = FontWeights.Bold;
            Canvas.SetLeft(CloseButton, 900);
            Canvas.SetTop(CloseButton, 600);
            MainCanvas.Children.Add(CloseButton);
            CloseButton.Click += new RoutedEventHandler(CloseButton_Click);

            Button DontRemind = new Button();
            DontRemind.Width = 200; DontRemind.Height = 50;
            DontRemind.Content = Links.Interface("NotRemind");
            DontRemind.FontFamily = Links.Font;
            DontRemind.Background = Brushes.Black;
            DontRemind.Foreground = Brushes.White;
            DontRemind.FontSize = 24;
            DontRemind.FontWeight = FontWeights.Bold;
            Canvas.SetLeft(DontRemind, 300);
            Canvas.SetTop(DontRemind, 600);
            MainCanvas.Children.Add(DontRemind);
            DontRemind.Click += new RoutedEventHandler(DontRemind_Click);

            Title = new TextBlock(); Title.Width = 1100;
            Title.FontFamily = Links.Font;
            Title.FontSize = 30; Title.TextAlignment = TextAlignment.Center;
            MainCanvas.Children.Add(Title); Canvas.SetTop(Title, 5);
            Canvas.SetLeft(Title, 50);

            ScrollViewer textviewer = new ScrollViewer(); textviewer.Width = 1100; textviewer.Height = 500;
            textviewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            textviewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            MainCanvas.Children.Add(textviewer);
            Canvas.SetLeft(textviewer, 50); Canvas.SetTop(textviewer, 50);
            Block = new TextBlock(); Block.FontFamily = Links.Font; Block.Width = 1080;
            Block.FontSize = 24; 
            //MainCanvas.Children.Add(Block); 
            textviewer.Content = Block;
            Block.TextWrapping = TextWrapping.Wrap;
            //Canvas.SetLeft(Block, 50); Canvas.SetTop(Block, 50);

            Fill();

            Links.Controller.PopUpCanvas.Place(this);
        }

        void DontRemind_Click(object sender, RoutedEventArgs e)
        {
            GSGameInfo.HelpArray[(int)Pos] = 255;
            string eventresult = Events.UpdateHelps();
            if (eventresult == "") Links.Controller.PopUpCanvas.Remove();
            else Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(eventresult), true);
        }

        void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Links.Controller.PopUpCanvas.Remove();
        }
        void Fill()
        {
            switch (Pos)
            {
                case HelpWindows.First: Fill_First(); break;
                case HelpWindows.Schemas: Fill_Schemas(); break;
                case HelpWindows.Market: Fill_Market(); break;
                case HelpWindows.Chat: Fill_Chat(); break;
            }
        }
        void Fill_First()
        {
            Title.Text = Links.Interface("Help_First_Title");
            Block.Inlines.Add(new Run(Links.Interface("Help_First_1"))); Block.Inlines.Add(new LineBreak());
            Block.Inlines.Add(new Run(Links.Interface("Help_First_2"))); Block.Inlines.Add(new LineBreak());
            Block.Inlines.Add(new Run(Links.Interface("Help_First_3"))); Block.Inlines.Add(new LineBreak());
            Block.Inlines.Add(new Run(Links.Interface("Help_First_4"))); Block.Inlines.Add(new LineBreak());
            Block.Inlines.Add(new Run(Links.Interface("Help_First_5"))); Block.Inlines.Add(new LineBreak());
            Block.Inlines.Add(new Run(Links.Interface("Help_First_6"))); Block.Inlines.Add(new LineBreak());
            Block.Inlines.Add(new Run(Links.Interface("Help_First_7"))); Block.Inlines.Add(new LineBreak());
            Block.Inlines.Add(new Run(Links.Interface("Help_First_8"))); Block.Inlines.Add(new LineBreak());
            Block.Inlines.Add(new Run(Links.Interface("Help_First_9"))); Block.Inlines.Add(new LineBreak());
            Block.Inlines.Add(new Run(Links.Interface("Help_First_10"))); Block.Inlines.Add(new LineBreak());
            Block.Inlines.Add(new Run(Links.Interface("Help_First_11"))); Block.Inlines.Add(new LineBreak());
            Block.Inlines.Add(new Run(Links.Interface("Help_First_12"))); Block.Inlines.Add(new LineBreak());
            Block.Inlines.Add(new Run(Links.Interface("Help_First_13"))); Block.Inlines.Add(new LineBreak());
            Rectangle moneyrect = Common.GetRectangle(30, Links.Brushes.MoneyImageBrush);
            Block.Inlines.Add(new InlineUIContainer(moneyrect));
            Block.Inlines.Add(new Run(Links.Interface("Help_First_14"))); Block.Inlines.Add(new LineBreak());
            Block.Inlines.Add(new InlineUIContainer(Common.GetRectangle(30, Links.Brushes.MetalImageBrush)));
            Block.Inlines.Add(new Run(Links.Interface("Help_First_15"))); Block.Inlines.Add(new LineBreak());
            Block.Inlines.Add(new InlineUIContainer(Common.GetRectangle(30, Links.Brushes.ChipsImageBrush)));
            Block.Inlines.Add(new Run(Links.Interface("Help_First_16"))); Block.Inlines.Add(new LineBreak());
            Block.Inlines.Add(new InlineUIContainer(Common.GetRectangle(30, Links.Brushes.AntiImageBrush)));
            Block.Inlines.Add(new Run(Links.Interface("Help_First_17"))); Block.Inlines.Add(new LineBreak());
            Block.Inlines.Add(new Run(Links.Interface("Help_First_18"))); Block.Inlines.Add(new LineBreak());
            Block.Inlines.Add(new InlineUIContainer(Common.GetRectangle(30, Links.Brushes.MathPowerBrush)));
            Block.Inlines.Add(new Run(Links.Interface("Help_First_19"))); Block.Inlines.Add(new LineBreak());
            Block.Inlines.Add(new InlineUIContainer(Common.GetRectangle(30, Links.Brushes.ZipImageBrush)));
            Block.Inlines.Add(new Run(Links.Interface("Help_First_20"))); Block.Inlines.Add(new LineBreak());
        }
        void Fill_Schemas()
        {
            Title.Text = Links.Interface("Help_Schemas_Title");
            Block.Inlines.Add(new Run(Links.Interface("Help_Schemas_1"))); Block.Inlines.Add(new LineBreak());
            HealthCanvas healthcanvas = new HealthCanvas(); healthcanvas.Width = 50; healthcanvas.Height = 50;
            healthcanvas.Capacity.Content = "(1)";
            healthcanvas.Value.Content = "(2)";
            healthcanvas.Restore.Content = "(3)";
            Block.Inlines.Add(new InlineUIContainer(healthcanvas)); //Block.Inlines.Add(new LineBreak());
            UIElement health = Common.GetRectangle(30, Pictogram.HealthBrush);
            Block.Inlines.Add(new InlineUIContainer(health));
            Block.Inlines.Add(new Run(Links.Interface("Help_Schemas_2"))); Block.Inlines.Add(new LineBreak());
            AbsorpCanvas absorpcanvas = new AbsorpCanvas(); absorpcanvas.Width = 50; absorpcanvas.Height = 50;
            absorpcanvas.Energy.Content = "50";
            absorpcanvas.Physic.Content = "20";
            absorpcanvas.Irregular.Content = "15";
            absorpcanvas.Cyber.Content = "35";
            Block.Inlines.Add(new InlineUIContainer(absorpcanvas));
            Block.Inlines.Add(new InlineUIContainer(Common.GetRectangle(30, Pictogram.Ignore)));
            Block.Inlines.Add(new Run(Links.Interface("Help_Schemas_3"))); Block.Inlines.Add(new LineBreak());
            ShieldCanvas shieldcanvas = new ShieldCanvas(); shieldcanvas.Width = 50; shieldcanvas.Height = 50;
            shieldcanvas.Capacity.Content = "500";
            shieldcanvas.Value.Content = "200";
            shieldcanvas.Restore.Content = "+150";
            shieldcanvas.SetShieldType(150);
            Block.Inlines.Add(new InlineUIContainer(shieldcanvas));
            Block.Inlines.Add(new InlineUIContainer(Common.GetRectangle(30, Pictogram.ShieldBrush)));
            Block.Inlines.Add(new Run(Links.Interface("Help_Schemas_4"))); Block.Inlines.Add(new LineBreak());
            EnergyCanvas energycanvas = new EnergyCanvas(); energycanvas.Width = 50; energycanvas.Height = 50;
            energycanvas.Margin = new Thickness(10, 0, 0, 0);
            energycanvas.Capacity.Content = "300";
            energycanvas.Value.Content = "250";
            energycanvas.Restore.Content = "+200";
            Block.Inlines.Add(new InlineUIContainer(energycanvas));
            Block.Inlines.Add(new InlineUIContainer(Common.GetRectangle(30, Pictogram.EnergyBrush)));
            Block.Inlines.Add(new Run(Links.Interface("Help_Schemas_5"))); Block.Inlines.Add(new LineBreak());
            EvasionCanvas evasioncanvas = new EvasionCanvas(); evasioncanvas.Width = 50; evasioncanvas.Height = 50;
            evasioncanvas.Energy.Content = "200";
            evasioncanvas.Physic.Content = "100";
            evasioncanvas.Irregular.Content = "50";
            evasioncanvas.Cyber.Content = "150";
            Block.Inlines.Add(new InlineUIContainer(evasioncanvas));
            Block.Inlines.Add(new InlineUIContainer(Common.GetRectangle(30, Pictogram.Evasion)));
            Block.Inlines.Add(new Run(Links.Interface("Help_Schemas_6"))); Block.Inlines.Add(new LineBreak());
            Block.Inlines.Add(new InlineUIContainer(Common.GetRectangle(30, Pictogram.ShieldDamage)));
            Block.Inlines.Add(new InlineUIContainer(Common.GetRectangle(30, Pictogram.HealthDamage)));
            Block.Inlines.Add(new Run(Links.Interface("Help_Schemas_7"))); Block.Inlines.Add(new LineBreak());
            Block.Inlines.Add(new InlineUIContainer(Common.GetRectangle(30, Pictogram.Accuracy)));
            Block.Inlines.Add(new Run(Links.Interface("Help_Schemas_8"))); Block.Inlines.Add(new LineBreak());
        }
        void Fill_Market()
        {
            Title.Text = Links.Interface("Help_Market_Title");
            Block.Inlines.Add(new Run(Links.Interface("Help_Market_1"))); Block.Inlines.Add(new LineBreak());
            Block.Inlines.Add(new Run(Links.Interface("Help_Market_2"))); Block.Inlines.Add(new LineBreak());
            Block.Inlines.Add(new Run(Links.Interface("Help_Market_3"))); Block.Inlines.Add(new LineBreak());
            Block.Inlines.Add(new Run(Links.Interface("Help_Market_4"))); Block.Inlines.Add(new LineBreak());
            Block.Inlines.Add(new Run(Links.Interface("Help_Market_5"))); Block.Inlines.Add(new LineBreak());
        }
        void Fill_Chat()
        {
            Title.Text = Links.Interface("Help_Chat_Title");
            Block.Inlines.Add(new Run(Links.Interface("Help_Chat_1"))); Block.Inlines.Add(new LineBreak());
        }
    }
    
}
