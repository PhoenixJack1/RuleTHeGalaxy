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
using System.Windows.Media.Animation;


namespace Client
{
    /*
    class Premium
    {
        public bool HasOrion;
        public bool HasQuickStart;
        public bool HasFullPay;
        public bool FullScience;
        public bool AllMoney;
        public bool NoDelete;
        public int CurrentMoney;
        public int PremiumDays;
        public int TotalPayedMoney;
        public bool IsPremium;
        public bool Money = false;
        public bool Science = false;
        public byte[] Array;
        public Premium(byte[] array)
        {
            int i = 0;
            HasOrion = BitConverter.ToBoolean(array, i); i++;
            HasQuickStart = BitConverter.ToBoolean(array, i); i++;
            HasFullPay = BitConverter.ToBoolean(array, i); i++;
            FullScience = BitConverter.ToBoolean(array, i); i++;
            AllMoney = BitConverter.ToBoolean(array, i); i++;
            NoDelete = BitConverter.ToBoolean(array, i); i++;
            IsPremium = BitConverter.ToBoolean(array, i); i++;
            CurrentMoney = BitConverter.ToInt32(array, i); i += 4;
            PremiumDays = BitConverter.ToInt32(array, i); i += 4;
            TotalPayedMoney = BitConverter.ToInt32(array, i); i += 4;
        }      
    }
    class PremiumWindow : Border
    {

        public PremiumWindow(Premium premium)
        {
            BorderBrush = Brushes.White;
            BorderThickness = new Thickness(3);
            CornerRadius = new CornerRadius(20);
            Background = Brushes.Black;
            Width = 1015; Height = 600;
            Canvas mainCanvas = new Canvas();
            Child = mainCanvas;
            Premium_Coin Current = new Premium_Coin(200, 200, premium.CurrentMoney);
            mainCanvas.Children.Add(Current);
            Canvas.SetLeft(Current, 10);
            Canvas.SetTop(Current, 10);

            Premium_Coin_Silver Total = new Premium_Coin_Silver(100, 100, premium.TotalPayedMoney);
            mainCanvas.Children.Add(Total);
            Canvas.SetLeft(Total, 60);
            Canvas.SetTop(Total, 250);

            TextBlock TitleBlock = new TextBlock();
            TitleBlock.FontFamily = Links.Font;
            TitleBlock.FontSize = 50;
            TitleBlock.Width = 800;
            TitleBlock.Foreground = Brushes.White;
            TitleBlock.FontWeight = FontWeights.Bold;
            TitleBlock.TextAlignment = TextAlignment.Center;
            mainCanvas.Children.Add(TitleBlock);
            Canvas.SetLeft(TitleBlock, 200);
            Canvas.SetTop(TitleBlock, 10);

            PremiumButton Days_30 = new PremiumButton(Links.Interface("Premium_30"), 300,0);
            mainCanvas.Children.Add(Days_30);
            Canvas.SetLeft(Days_30, 220); Canvas.SetTop(Days_30, 100);

            PremiumButton Days_90 = new PremiumButton(Links.Interface("Premium_90"), 800,1);
            mainCanvas.Children.Add(Days_90);
            Canvas.SetLeft(Days_90, 380); Canvas.SetTop(Days_90, 100);

            PremiumButton Days_180 = new PremiumButton(Links.Interface("Premium_180"), 1500,2);
            mainCanvas.Children.Add(Days_180);
            Canvas.SetLeft(Days_180, 220); Canvas.SetTop(Days_180, 180);

            PremiumButton Days_365 = new PremiumButton(Links.Interface("Premium_365"), 2500,3);
            mainCanvas.Children.Add(Days_365);
            Canvas.SetLeft(Days_365, 380); Canvas.SetTop(Days_365, 180);

            PremiumButton Days_Full = new PremiumButton(Links.Interface("Premium_Full"), 5000,4);
            mainCanvas.Children.Add(Days_Full);
            Canvas.SetLeft(Days_Full, 300); Canvas.SetTop(Days_Full, 260);

            //TitleBlock.Text = "Premium account - no premium";
            TitleBlock.Text = Links.Interface("PremiumTitle");
            if (premium.IsPremium)
                TitleBlock.Text += String.Format(Links.Interface("PremiumLeft"), premium.PremiumDays);
            else
                TitleBlock.Text += Links.Interface("PremiumFalse");

            Border InfoBorder = new Border();
            InfoBorder.BorderBrush = Brushes.White; InfoBorder.BorderThickness = new Thickness(2);
            InfoBorder.CornerRadius = new CornerRadius(20);
            InfoBorder.Width = 460; InfoBorder.Height = 260;
            mainCanvas.Children.Add(InfoBorder);
            Canvas.SetLeft(InfoBorder, 540); Canvas.SetTop(InfoBorder, 80);

            TextBlock InfoBlock = new TextBlock();
            InfoBlock.FontFamily = Links.Font;
            InfoBlock.FontSize = 20;
            InfoBlock.Foreground = Brushes.White;
            InfoBlock.TextWrapping = TextWrapping.Wrap;
            InfoBlock.Margin = new Thickness(5);
            InfoBorder.Child = InfoBlock;
            InfoBlock.Text = String.Format("{0}\n{1}\n{2}\n{3}\n{4}\n{5}\n{6}\n{7}\n{8}",
               Links.Interface("PremiumInfo1"), Links.Interface("PremiumInfo2"),
               Links.Interface("PremiumInfo3"), Links.Interface("PremiumInfo4"),
               Links.Interface("PremiumInfo5"), Links.Interface("PremiumInfo6"),
               Links.Interface("PremiumInfo7"), Links.Interface("PremiumInfo8"),
               Links.Interface("PremiumInfo9"));

            TextBlock BonusBlock = new TextBlock();
            BonusBlock.FontFamily = Links.Font;
            BonusBlock.FontSize = 30;
            BonusBlock.Width = 800;
            BonusBlock.Foreground = Brushes.White;
            BonusBlock.FontWeight = FontWeights.Bold;
            BonusBlock.TextAlignment = TextAlignment.Center;
            mainCanvas.Children.Add(BonusBlock);
            Canvas.SetLeft(BonusBlock, 150);
            Canvas.SetTop(BonusBlock, 340);
            BonusBlock.Text = Links.Interface("PremiumBonusInfo");

            PremiumBonus NoDeleteBonus = new PremiumBonus(premium.NoDelete, 300, Links.Interface("PremiumBonusNoDelete"));
            mainCanvas.Children.Add(NoDeleteBonus);
            Canvas.SetLeft(NoDeleteBonus, 10);
            Canvas.SetTop(NoDeleteBonus, 380);

            PremiumBonus FullScienceBonus = new PremiumBonus(premium.FullScience, 800, Links.Interface("PremiumBonusFullScience"));
            mainCanvas.Children.Add(FullScienceBonus);
            Canvas.SetLeft(FullScienceBonus, 210);
            Canvas.SetTop(FullScienceBonus, 380);

            PremiumBonus AllMoneyBonus = new PremiumBonus(premium.AllMoney, 1500, Links.Interface("PremiumBonusFullMoney"));
            mainCanvas.Children.Add(AllMoneyBonus);
            Canvas.SetLeft(AllMoneyBonus, 410);
            Canvas.SetTop(AllMoneyBonus, 380);

            PremiumBonus QuickStartBonus = new PremiumBonus(premium.HasQuickStart, 2500, Links.Interface("PremiumBonusQuickStart"));
            mainCanvas.Children.Add(QuickStartBonus);
            Canvas.SetLeft(QuickStartBonus, 610);
            Canvas.SetTop(QuickStartBonus, 380);

            PremiumBonus OrionBonus = new PremiumBonus(premium.HasOrion, 5000, Links.Interface("PremiumBonusOrion"));
            mainCanvas.Children.Add(OrionBonus);
            Canvas.SetLeft(OrionBonus, 810);
            Canvas.SetTop(OrionBonus, 380);

            Button BuyButton = new Button();
            BuyButton.Width = 300; BuyButton.Height = 40;
            BuyButton.Background = Brushes.Black; BuyButton.Foreground = Brushes.White;
            BuyButton.FontWeight = FontWeights.Bold;  BuyButton.FontFamily = Links.Font;
            BuyButton.Content = Links.Interface("PremiumBuyButton"); BuyButton.FontSize = 30;
            mainCanvas.Children.Add(BuyButton);
            Canvas.SetLeft(BuyButton, 100); Canvas.SetTop(BuyButton, 550);


            Button CloseButton = new Button();
            CloseButton.Width = 200; CloseButton.Height = 40;
            CloseButton.Background = Brushes.Black; CloseButton.Foreground = Brushes.White;
            CloseButton.FontWeight = FontWeights.Bold; CloseButton.FontFamily = Links.Font;
            CloseButton.Content = Links.Interface("Close"); CloseButton.FontSize = 30;
            mainCanvas.Children.Add(CloseButton);
            Canvas.SetLeft(CloseButton, 500); Canvas.SetTop(CloseButton, 550);
            CloseButton.Click += new RoutedEventHandler(CloseButton_Click);
        }

        void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Links.Controller.PopUpCanvas.Remove();
        }
        public static void CreateBrushes()
        {

        }
        class PremiumBonus : Border
        {
            static LinearGradientBrush RedBrush = GetRedBrush();
            static LinearGradientBrush GreenBrush = GetGreenBrush();
            public PremiumBonus(bool isActive, int value, string text)
            {
                Width = 190;
                Height = 160;
                BorderBrush = Brushes.White;
                BorderThickness = new Thickness(2);
                CornerRadius = new CornerRadius(10);
                if (isActive)
                    Background = GreenBrush;
                else
                    Background = RedBrush;
                Border innerborder = new Border();
                innerborder.Background = Brushes.Black;
                innerborder.Width = 180;
                innerborder.Height = 150;
                innerborder.BorderBrush = Brushes.White;
                innerborder.BorderThickness = new Thickness(2);
                innerborder.CornerRadius = new CornerRadius(10);
                Child = innerborder;

                StackPanel Stack = new StackPanel();
                Stack.Orientation = Orientation.Horizontal;
                innerborder.Child = Stack;

                TextBlock block = new TextBlock();
                block.Width = 115;
                block.Margin = new Thickness(5);
                block.FontFamily = Links.Font;
                block.FontSize = 16;
                block.Foreground = Brushes.White;
                block.TextAlignment = TextAlignment.Center;
                Stack.Children.Add(block);
                if (isActive) block.Text = Links.Interface("BonusActive"); else block.Text = "";
                block.Text += text;
                block.TextWrapping = TextWrapping.Wrap;

                Premium_Coin_Silver coin = new Premium_Coin_Silver(50, 50, value);
                Stack.Children.Add(coin);
            }
            static LinearGradientBrush GetRedBrush()
            {
                LinearGradientBrush brush = new LinearGradientBrush();
                brush.GradientStops.Add(new GradientStop(Colors.Red, 0.2));
                brush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
                brush.GradientStops.Add(new GradientStop(Colors.Red, 0.8));
                return brush;
            }
            static LinearGradientBrush GetGreenBrush()
            {
                LinearGradientBrush brush = new LinearGradientBrush();
                brush.GradientStops.Add(new GradientStop(Colors.Green, 0.2));
                brush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
                brush.GradientStops.Add(new GradientStop(Colors.Green, 0.8));
                return brush;
            }
        }
        /*
        class PremiumButton:Button
        {
            byte Period;
            public PremiumButton(string text, int value, byte period)
            {
                Width = 150; Height = 70; Background = Brushes.Black;
                StackPanel panel = new StackPanel(); panel.Orientation = Orientation.Horizontal;
                Content = panel;
                TextBlock block = new TextBlock(); block.Foreground = Brushes.White; block.FontSize = 20; block.Margin = new Thickness(0, 5, 5, 0);
                block.TextWrapping = TextWrapping.Wrap; block.Width = 70; block.TextAlignment = TextAlignment.Center;
                block.Text = text; panel.Children.Add(block); block.FontFamily = Links.Font;
                Premium_Coin coin = new Premium_Coin(60, 60, value);
                panel.Children.Add(coin);
                Period = period;
                Click += PremiumButton_Click;
            }
            /*
            private void PremiumButton_Click(object sender, RoutedEventArgs e)
            {
                Links.Controller.PopUpCanvas.Remove();
                string eventresult = Events.ActivatePremium(Period);
                if (eventresult == "")
                {
                    Gets.GetTotalInfo();
                    Links.Controller.PopUpCanvas.Place(new PremiumWindow(GSGameInfo.Premium), false);
                }
                else
                {
                    Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(eventresult), true);
                }
            
            }
            //public static void Place
        }
        
    }*/
}
