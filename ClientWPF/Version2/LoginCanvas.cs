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
    public enum Languages { En, Ru }
    public enum GlobalWindows { Login, Main}
    class LoginCanvas : Canvas
    {
        enum ELoginMenu { Start, Login, Sign}
        //bool IsLogin = true;
        //Rectangle TitleRect;
        public static Languages CurrentLanguage = (Languages)Links.SaveData.Lang;
        //public static SortedList<string, string[]> LoginTexts;
        /*bool ValidEmail;
        bool ValidName;
        bool ValidPassword;
        bool ConfirmPassword;*/

        /*Canvas StartMenuCanvas;
        Rectangle StartMenuMirror;

        Canvas LoginTextCanvas;
        Rectangle LoginTextMirror;
        Label LoginEmailLbl, LoginPassLbl;
        TextBox LoginEmailBox;
        PasswordBox LoginPasswordBox;
        CheckBox LoginBox;
        Label BoxText;

        Canvas SignTextCanvas;
        Rectangle SignTextMirror;
        Label SignEmailLbl, SignNameLbl, SignPassLbl, SignConfirmLbl;
        TextBox SignEmailBox, SignNameBox;
        PasswordBox SignPass1Box, SignPass2Box;

        InterfaceButton MainBtn, ChangeBtn, LangBtn, CloseBtn;
        InterfaceButton SingleButton, MultiButton, RandomButton;*/
        ELoginMenu LoginMenu = ELoginMenu.Start;
        //static LinearGradientBrush br = GetButtonBrush();
        ButtonV2 single, online, random, lang, exit;
        public LoginCanvas()
        {
            //Common.CreateLoginBrushes();
            //TextPrepare();
            Width = 1294;
            Height = 758;
            Rectangle MainBack = new Rectangle(); MainBack.Width = 1294; MainBack.Height = 758;
            Children.Add(MainBack);
            MainBack.Fill = new ImageBrush(Links.LoginImage);
            ScaleTransform MainScale = new ScaleTransform();
            MainBack.RenderTransformOrigin = new Point(0.5, 0.5);
            MainBack.RenderTransform = MainScale;
            DoubleAnimation MainScaleAnim = new DoubleAnimation(1, 1.1, TimeSpan.FromSeconds(15));
            MainScaleAnim.RepeatBehavior = RepeatBehavior.Forever;
            MainScaleAnim.AutoReverse = true;
            MainScaleAnim.AccelerationRatio = 0.5; MainScaleAnim.DecelerationRatio = 0.5;
            MainScale.BeginAnimation(ScaleTransform.ScaleXProperty, MainScaleAnim);
            MainScale.BeginAnimation(ScaleTransform.ScaleYProperty, MainScaleAnim);
            Rectangle Pers = new Rectangle(); Pers.Width = 509;
            Pers.Height = 736.5; Children.Add(Pers);
            Pers.Fill = new ImageBrush(Links.PersImage);
            Canvas.SetTop(Pers, 21.5);

            Rectangle logobeta = new Rectangle(); logobeta.Width = 310; logobeta.Height = 155;
            Children.Add(logobeta); logobeta.Fill = Links.Brushes.Login.LogoBeta;
            Canvas.SetLeft(logobeta, 763); Canvas.SetTop(logobeta, 140);

            Rectangle logogame = new Rectangle(); logogame.Width = 775; logogame.Height = 230;
            Children.Add(logogame); logogame.Fill = Links.Brushes.Login.LogoGame;
            Canvas.SetLeft(logogame, 520); Canvas.SetTop(logogame, 0);

            single = new ButtonV2(290, 100, Links.Brushes.Login.SingleE0, Links.Brushes.Login.SingleE1,
                Links.Brushes.Login.SingleR0, Links.Brushes.Login.SingleR1);
            Children.Add(single); Canvas.SetLeft(single, 970); Canvas.SetTop(single, 240);
            single.PreviewMouseDown += Single_PreviewMouseDown;

            online = new ButtonV2(287, 100, Links.Brushes.Login.OnlineE0, Links.Brushes.Login.OnlineE1,
                Links.Brushes.Login.OnlineR0, Links.Brushes.Login.OnlineR1);
            Children.Add(online); Canvas.SetLeft(online, 977); Canvas.SetTop(online, 333);
            online.PreviewMouseDown += Online_PreviewMouseDown;

            random = new ButtonV2(290, 97, Links.Brushes.Login.RandomE0, Links.Brushes.Login.RandomE1,
                Links.Brushes.Login.RandomR0, Links.Brushes.Login.RandomR1);
            Children.Add(random); Canvas.SetLeft(random, 975); Canvas.SetTop(random, 427);
            random.PreviewMouseDown += TestBtn_PreviewMouseDown;

            exit = new ButtonV2(215, 175, Links.Brushes.Login.ExitE0, Links.Brushes.Login.ExitE1,
                Links.Brushes.Login.ExitR0, Links.Brushes.Login.ExitR1);
            Children.Add(exit); Canvas.SetLeft(exit, 1055); Canvas.SetTop(exit, 520);
            exit.PreviewMouseDown += Exit_PreviewMouseDown;

            lang = new ButtonV2(92, 92, Links.Brushes.Login.Ru0, Links.Brushes.Login.Ru1,
                Links.Brushes.Login.En0, Links.Brushes.Login.En1);
            Children.Add(lang); Canvas.SetLeft(lang, 1072); Canvas.SetTop(lang, 652);
            lang.PreviewMouseDown += Lang_PreviewMouseDown;

            ButtonV2 vk = new ButtonV2(35, 35, Links.Brushes.Login.VK1, Links.Brushes.Login.VK0,
                null, null);
            Children.Add(vk); Canvas.SetLeft(vk, 15); Canvas.SetTop(vk, 15);
            vk.PreviewMouseDown += Vk_PreviewMouseDown;

            Rectangle logostudio = new Rectangle(); logostudio.Width = 250; logostudio.Height = 147;
            Children.Add(logostudio); logostudio.Fill = Links.Brushes.Login.LogoStudio;
            Canvas.SetLeft(logostudio, -28); Canvas.SetTop(logostudio, 645);

            Rectangle Parameters = new Rectangle(); Parameters.Width = 200; Parameters.Height = 50;
            Children.Add(Parameters); Parameters.Fill = Brushes.Red;
            Canvas.SetLeft(Parameters, 300); Canvas.SetTop(Parameters, 700);
            Parameters.PreviewMouseDown += Parameters_PreviewMouseDown;
            SetLanguage();
        }

        private void Parameters_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Children.Add(new Parameters(GlobalWindows.Login));
        }

        private void Online_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Not available in beta version");
        }

        private void Vk_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("www.vk.com/rulethegalaxy");
        }

        private void Single_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            SingleGameStartCanvas2 canvas = new SingleGameStartCanvas2();
            Children.Add(canvas);
            Canvas.SetZIndex(canvas, 255);
        }

        private void Exit_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Commands.Stop_Game();
        }

        private void Lang_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            byte curlang = (byte)CurrentLanguage;
            curlang++;
            curlang = (byte)(curlang % 2);
            CurrentLanguage = (Languages)curlang;
            SetLanguage();
        }
        void SetLanguage()
        {
            byte curlang = (byte)CurrentLanguage;
            single.SetLang(curlang); online.SetLang(curlang);
            random.SetLang(curlang); exit.SetLang(curlang);
            lang.SetLang(curlang);
        }
        /*
        void SelectMenu()
        {
            switch (LoginMenu)
            {
                case ELoginMenu.Start:
                    StartMenuCanvas.Visibility = Visibility.Visible;
                    StartMenuMirror.Visibility = Visibility.Visible;
                    LoginTextCanvas.Visibility = Visibility.Hidden;
                    LoginTextMirror.Visibility = Visibility.Hidden;
                    SignTextCanvas.Visibility = Visibility.Hidden;
                    SignTextMirror.Visibility = Visibility.Hidden;
                    MainBtn.Visibility = Visibility.Hidden;
                    ChangeBtn.Visibility = Visibility.Hidden;
                    break;
                case ELoginMenu.Login:
                    StartMenuCanvas.Visibility = Visibility.Hidden;
                    StartMenuMirror.Visibility = Visibility.Hidden;
                    LoginTextCanvas.Visibility = Visibility.Visible;
                    LoginTextMirror.Visibility = Visibility.Visible;
                    SignTextCanvas.Visibility = Visibility.Hidden;
                    SignTextMirror.Visibility = Visibility.Hidden;
                    MainBtn.Visibility = Visibility.Visible;
                    ChangeBtn.Visibility = Visibility.Visible;
                    EmailBox_LostFocus(null, null);
                    PasswordBox_LostFocus(null, null);
                    break;
                case ELoginMenu.Sign:
                    StartMenuCanvas.Visibility = Visibility.Hidden;
                    StartMenuMirror.Visibility = Visibility.Hidden;
                    LoginTextCanvas.Visibility = Visibility.Hidden;
                    LoginTextMirror.Visibility = Visibility.Hidden;
                    SignTextCanvas.Visibility = Visibility.Visible;
                    SignTextMirror.Visibility = Visibility.Visible;
                    MainBtn.Visibility = Visibility.Visible;
                    ChangeBtn.Visibility = Visibility.Visible;
                    NameBox_LostFocus(null, null);
                    PasswordConfirmBox_LostFocus(null, null);
                    EmailBox_LostFocus(null, null);
                    PasswordBox_LostFocus(null, null);
                    break;
            }


        }
        static LinearGradientBrush GetButtonBrush()
        {
            LinearGradientBrush br = new LinearGradientBrush();
            br.StartPoint = new Point(0.5, 1);
            br.EndPoint = new Point(0.5, 0);
            br.GradientStops.Add(new GradientStop(Color.FromArgb(150, 0, 0, 0), 0));
            br.GradientStops.Add(new GradientStop(Color.FromArgb(200, 0, 0, 0), 0.5));
            br.GradientStops.Add(new GradientStop(Color.FromArgb(150, 0, 0, 0), 1));
            return br;
        } 
        void CreateMultiplayerButtons()
        {
            MainBtn = new InterfaceButton(250, 105, 7, 40);
            MainBtn.PutToCanvas(this, 800, 555);

            MainBtn.Background = br;
            MainBtn.PreviewMouseDown += MainBtn_PreviewMouseDown;

            CloseBtn = new InterfaceButton(150, 90, 7, 30);
            CloseBtn.PutToCanvas(this, 1055, 665);
            CloseBtn.Background = br;
            CloseBtn.PreviewMouseDown += CloseBtn_PreviewMouseDown;

            ChangeBtn = new InterfaceButton(150, 105, 7, 24);
            ChangeBtn.PutToCanvas(this, 1055,555);
            ChangeBtn.Background = br;

            ChangeBtn.PreviewMouseDown += ChangeBtn_PreviewMouseDown;
            LangBtn = new InterfaceButton(150, 90, 7, 30);
            LangBtn.PutToCanvas(this, 800, 665);
            LangBtn.SetText("Ru");
            LangBtn.Background = br;
            LangBtn.PreviewMouseDown += LangBtn_PreviewMouseDown;
            /*
            TestBtn = new InterfaceButton(200, 90, 7, 24);
            TestBtn.PutToCanvas(this, 500, 665);
            TestBtn.Background = br;
            TestBtn.SetText("Тестовый бой");
            TestBtn.PreviewMouseDown += TestBtn_PreviewMouseDown;
            */
        //}
        /*
        void CreateSignCanvas()
        {
            SignTextCanvas = new Canvas();
            SignTextCanvas.Background = new SolidColorBrush(Color.FromArgb(150, 0, 0, 0));
            SignTextCanvas.Width = 400; SignTextCanvas.Height = 330;
            Children.Add(SignTextCanvas); SignTextCanvas.Visibility = Visibility.Hidden;
            Canvas.SetLeft(SignTextCanvas, 800);
            Canvas.SetTop(SignTextCanvas, 50);
            Rectangle SignBorder = new Rectangle();
            SignBorder.Width = 400; SignBorder.Height = 330;
            SignBorder.Stroke = Brushes.SkyBlue;
            SignTextCanvas.Children.Add(SignBorder);


            SignTextMirror = new Rectangle(); SignTextMirror.Visibility = Visibility.Hidden;
            SignTextMirror.Width = 400; SignTextMirror.Height = 200;
            SignTextMirror.Fill = new VisualBrush(SignTextCanvas);
            ScaleTransform SignScale = new ScaleTransform(1, -0.8);
            SignTextMirror.RenderTransformOrigin = new Point(1, 0.5);
            //rect.RenderTransform = scale;
            Children.Add(SignTextMirror);
            Canvas.SetLeft(SignTextMirror, 742);
            Canvas.SetTop(SignTextMirror, 360);
            LinearGradientBrush Signoppmask = new LinearGradientBrush();
            Signoppmask.GradientStops.Add(new GradientStop(Colors.Transparent, 0));
            Signoppmask.GradientStops.Add(new GradientStop(Color.FromArgb(100, 0, 0, 0), 1));
            Signoppmask.StartPoint = new Point(0, 0); Signoppmask.EndPoint = new Point(0, 1);
            SignTextMirror.OpacityMask = Signoppmask;
            SkewTransform SignSkew = new SkewTransform(30, 0);
            TransformGroup SignGroup = new TransformGroup();
            SignGroup.Children.Add(SignSkew);
            SignGroup.Children.Add(SignScale);
            SignTextMirror.RenderTransform = SignGroup;

            DoubleAnimationUsingKeyFrames anim = new DoubleAnimationUsingKeyFrames();
            anim.KeyFrames.Add(new DiscreteDoubleKeyFrame(1, KeyTime.FromPercent(0)));
            anim.KeyFrames.Add(new DiscreteDoubleKeyFrame(0, KeyTime.FromPercent(0.50)));
            anim.KeyFrames.Add(new DiscreteDoubleKeyFrame(1, KeyTime.FromPercent(0.52)));
            anim.KeyFrames.Add(new DiscreteDoubleKeyFrame(0.3, KeyTime.FromPercent(0.525)));
            anim.KeyFrames.Add(new DiscreteDoubleKeyFrame(1, KeyTime.FromPercent(0.53)));
            anim.KeyFrames.Add(new DiscreteDoubleKeyFrame(0.7, KeyTime.FromPercent(0.535)));
            anim.KeyFrames.Add(new DiscreteDoubleKeyFrame(1, KeyTime.FromPercent(0.54)));
            anim.RepeatBehavior = RepeatBehavior.Forever;
            anim.Duration = TimeSpan.FromSeconds(9);
            SignTextCanvas.BeginAnimation(Canvas.OpacityProperty, anim);

            SignEmailLbl = PutLabelToImage(100, 10, SignTextCanvas, 26);
            SignEmailBox = PutBoxToImage(0, 50, SignTextCanvas, EmailBox_LostFocus); SignEmailBox.TabIndex = 0; SignEmailBox.IsEnabled = false;
            SignNameLbl = PutLabelToImage(100, 90, SignTextCanvas, 26);
            SignNameBox = PutBoxToImage(0, 130, SignTextCanvas, NameBox_LostFocus); SignNameBox.TabIndex = 1; SignNameBox.IsEnabled = false;

            SignPassLbl = PutLabelToImage(100, 170, SignTextCanvas, 26);
            SignPass1Box = PutPasswordBoxToImage(0, 210, SignTextCanvas, PasswordBox_LostFocus); SignPass1Box.TabIndex = 2; SignPass1Box.IsEnabled = false;
            SignConfirmLbl = PutLabelToImage(100, 250, SignTextCanvas, 26);
            SignPass2Box = PutPasswordBoxToImage(0, 290, SignTextCanvas, PasswordConfirmBox_LostFocus); SignPass2Box.TabIndex = 3; SignPass2Box.IsEnabled = false;


            Rectangle rect = new Rectangle(); rect.Fill = new SolidColorBrush(Color.FromArgb(50, 255, 0, 0));
            rect.Width = 400; rect.Height = 330; SignTextCanvas.Children.Add(rect);
            TextBlock BetaText = Common.GetBlock(40, "Not available in beta", Brushes.Red, 500);
            SignTextCanvas.Children.Add(BetaText);
            RotateTransform betarotate = new RotateTransform(30);
            BetaText.RenderTransform = betarotate;
        }
        void CreateLoginCanvas()
        {
            LoginTextCanvas = new Canvas();
            LoginTextCanvas.Background = new SolidColorBrush(Color.FromArgb(150, 0, 0, 0));
            LoginTextCanvas.Width = 400; LoginTextCanvas.Height = 280;
            Children.Add(LoginTextCanvas);
            Canvas.SetLeft(LoginTextCanvas, 800);
            Canvas.SetTop(LoginTextCanvas, 100);
            Rectangle LoginBorder = new Rectangle();
            LoginBorder.Width = 400; LoginBorder.Height = 280;
            LoginBorder.Stroke = Brushes.SkyBlue;
            LoginTextCanvas.Children.Add(LoginBorder);


            LoginTextMirror = new Rectangle();
            LoginTextMirror.Width = 400; LoginTextMirror.Height = 200;
            VisualBrush vb = new VisualBrush(LoginTextCanvas);
            LoginTextMirror.Fill = vb;
            ScaleTransform LoginScale = new ScaleTransform(1, -1);
            LoginTextMirror.RenderTransformOrigin = new Point(1, 0.5);
            //rect.RenderTransform = scale;
            Children.Add(LoginTextMirror);
            Canvas.SetLeft(LoginTextMirror, 742);
            Canvas.SetTop(LoginTextMirror, 380);
            LinearGradientBrush Loginoppmask = new LinearGradientBrush();
            Loginoppmask.GradientStops.Add(new GradientStop(Colors.Transparent, 0));
            Loginoppmask.GradientStops.Add(new GradientStop(Color.FromArgb(50, 0, 0, 0), 1));
            Loginoppmask.StartPoint = new Point(0, 0); Loginoppmask.EndPoint = new Point(0, 1);
            LoginTextMirror.OpacityMask = Loginoppmask;
            SkewTransform LoginSkew = new SkewTransform(30, 0);
            TransformGroup LoginGroup = new TransformGroup();
            LoginGroup.Children.Add(LoginSkew);
            LoginGroup.Children.Add(LoginScale);
            LoginTextMirror.RenderTransform = LoginGroup;

            DoubleAnimation anim1 = new DoubleAnimation(25, -25, TimeSpan.FromSeconds(9));
            anim1.RepeatBehavior = RepeatBehavior.Forever;
            DoubleAnimation anim2 = new DoubleAnimation(24.9, -25.1, TimeSpan.FromSeconds(9));
            anim2.RepeatBehavior = RepeatBehavior.Forever;
            DoubleAnimation anim3 = new DoubleAnimation(24.8, -25.2, TimeSpan.FromSeconds(9));
            anim3.RepeatBehavior = RepeatBehavior.Forever;
            LinearGradientBrush animbrush = new LinearGradientBrush();
            animbrush.StartPoint = new Point(0.5, 1); animbrush.EndPoint = new Point(0.5, 0);
            GradientStop gr1 = new GradientStop(Colors.White, 10);
            GradientStop gr2 = new GradientStop(Colors.Transparent, 9.5);
            GradientStop gr3 = new GradientStop(Colors.White, 9);
            animbrush.GradientStops.Add(gr1);
            animbrush.GradientStops.Add(gr2);
            animbrush.GradientStops.Add(gr3);
            gr1.BeginAnimation(GradientStop.OffsetProperty, anim1);
            gr2.BeginAnimation(GradientStop.OffsetProperty, anim2);
            gr3.BeginAnimation(GradientStop.OffsetProperty, anim3);
            LoginTextCanvas.OpacityMask = animbrush;

            LoginEmailLbl = PutLabelToImage(100, 0, LoginTextCanvas, 40);
            LoginEmailBox = PutBoxToImage(0, 70, LoginTextCanvas, EmailBox_LostFocus); LoginEmailBox.TabIndex = 0;
            if (Links.SaveData.SavePass) LoginEmailBox.Text = Links.SaveData.Login;
            LoginPassLbl = PutLabelToImage(100, 120, LoginTextCanvas, 40);
            LoginPasswordBox = PutPasswordBoxToImage(0, 190, LoginTextCanvas, PasswordBox_LostFocus); LoginPasswordBox.TabIndex = 1;
            if (Links.SaveData.SavePass) LoginPasswordBox.Password = Links.SaveData.Password;
            LoginBox = new CheckBox(); LoginBox.Width = 20; LoginBox.Height = 20; LoginBox.IsChecked = Links.SaveData.SavePass;
            LoginTextCanvas.Children.Add(LoginBox); Canvas.SetLeft(LoginBox, 20); Canvas.SetTop(LoginBox, 250); LoginBox.TabIndex = 2;
            BoxText = PutLabelToImage(40, 240, LoginTextCanvas, 22);
        }
        void CreateStartMenu()
        {
            StartMenuCanvas = new Canvas();
            StartMenuCanvas.Background = new SolidColorBrush(Color.FromArgb(150, 0, 0, 0));
            StartMenuCanvas.Width = 400; LoginTextCanvas.Height = 280;
            Children.Add(StartMenuCanvas);
            Canvas.SetLeft(StartMenuCanvas, 800);
            Canvas.SetTop(StartMenuCanvas, 100);
            Rectangle StartMenuBorder = new Rectangle();
            StartMenuBorder.Width = 400; StartMenuBorder.Height = 280;
            StartMenuBorder.Stroke = Brushes.SkyBlue;
            StartMenuCanvas.Children.Add(StartMenuBorder);


            StartMenuMirror = new Rectangle();
            StartMenuMirror.Width = 400; StartMenuMirror.Height = 200;
            VisualBrush vb = new VisualBrush(StartMenuCanvas);
            StartMenuMirror.Fill = vb;
            ScaleTransform StartMenuScale = new ScaleTransform(1, -1);
            StartMenuMirror.RenderTransformOrigin = new Point(1, 0.5);
            Children.Add(StartMenuMirror);
            Canvas.SetLeft(StartMenuMirror, 742);
            Canvas.SetTop(StartMenuMirror, 380);
            LinearGradientBrush StartMenuoppmask = new LinearGradientBrush();
            StartMenuoppmask.GradientStops.Add(new GradientStop(Colors.Transparent, 0));
            StartMenuoppmask.GradientStops.Add(new GradientStop(Color.FromArgb(50, 0, 0, 0), 1));
            StartMenuoppmask.StartPoint = new Point(0, 0); StartMenuoppmask.EndPoint = new Point(0, 1);
            StartMenuMirror.OpacityMask = StartMenuoppmask;
            SkewTransform StartMenuSkew = new SkewTransform(30, 0);
            TransformGroup StartMenuGroup = new TransformGroup();
            StartMenuGroup.Children.Add(StartMenuSkew);
            StartMenuGroup.Children.Add(StartMenuScale);
            LoginTextMirror.RenderTransform = StartMenuGroup;

            SingleButton = new InterfaceButton(400, 90, 7, 40);
            SingleButton.PutToCanvas(StartMenuCanvas, 0, 0);
            SingleButton.Background = br;
            SingleButton.PreviewMouseDown += SingleButton_PreviewMouseDown;

            MultiButton = new InterfaceButton(400, 90, 7, 40);
            MultiButton.PutToCanvas(StartMenuCanvas, 0, 95);
            MultiButton.Background = br;
            MultiButton.PreviewMouseDown += MultiButton_PreviewMouseDown;

            RandomButton = new InterfaceButton(400, 90, 7, 40);
            RandomButton.PutToCanvas(StartMenuCanvas, 0, 190);
            RandomButton.Background = br;
            RandomButton.PreviewMouseDown+= TestBtn_PreviewMouseDown;
            //MainBtn.PreviewMouseDown += MainBtn_PreviewMouseDown;

            /*DoubleAnimation anim1 = new DoubleAnimation(25, -25, TimeSpan.FromSeconds(9));
            anim1.RepeatBehavior = RepeatBehavior.Forever;
            DoubleAnimation anim2 = new DoubleAnimation(24.9, -25.1, TimeSpan.FromSeconds(9));
            anim2.RepeatBehavior = RepeatBehavior.Forever;
            DoubleAnimation anim3 = new DoubleAnimation(24.8, -25.2, TimeSpan.FromSeconds(9));
            anim3.RepeatBehavior = RepeatBehavior.Forever;
            LinearGradientBrush animbrush = new LinearGradientBrush();
            animbrush.StartPoint = new Point(0.5, 1); animbrush.EndPoint = new Point(0.5, 0);
            GradientStop gr1 = new GradientStop(Colors.White, 10);
            GradientStop gr2 = new GradientStop(Colors.Transparent, 9.5);
            GradientStop gr3 = new GradientStop(Colors.White, 9);
            animbrush.GradientStops.Add(gr1);
            animbrush.GradientStops.Add(gr2);
            animbrush.GradientStops.Add(gr3);
            gr1.BeginAnimation(GradientStop.OffsetProperty, anim1);
            gr2.BeginAnimation(GradientStop.OffsetProperty, anim2);
            gr3.BeginAnimation(GradientStop.OffsetProperty, anim3);
            LoginTextCanvas.OpacityMask = animbrush;
            */
            /*
            LoginEmailLbl = PutLabelToImage(100, 0, LoginTextCanvas, 40);
            LoginEmailBox = PutBoxToImage(0, 70, LoginTextCanvas, EmailBox_LostFocus); LoginEmailBox.TabIndex = 0;
            if (Links.SaveData.SavePass) LoginEmailBox.Text = Links.SaveData.Login;
            LoginPassLbl = PutLabelToImage(100, 120, LoginTextCanvas, 40);
            LoginPasswordBox = PutPasswordBoxToImage(0, 190, LoginTextCanvas, PasswordBox_LostFocus); LoginPasswordBox.TabIndex = 1;
            if (Links.SaveData.SavePass) LoginPasswordBox.Password = Links.SaveData.Password;
            LoginBox = new CheckBox(); LoginBox.Width = 20; LoginBox.Height = 20; LoginBox.IsChecked = Links.SaveData.SavePass;
            LoginTextCanvas.Children.Add(LoginBox); Canvas.SetLeft(LoginBox, 20); Canvas.SetTop(LoginBox, 250); LoginBox.TabIndex = 2;
            BoxText = PutLabelToImage(40, 240, LoginTextCanvas, 22);
    */
        //}

        private void SingleButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Commands.Load_Single_Game();// Start_Single_Game();
        }

        private void MultiButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            LoginMenu = ELoginMenu.Login;
            //SelectMenu();
            //SetLanguage();
        }

        private void TestBtn_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            RandomBattleCanvas canvas = new RandomBattleCanvas((byte)CurrentLanguage);
            Viewbox box = new Viewbox();
            canvas.box = box;
            box.Width = 1294; box.Height = 758;
            box.Child = canvas;
            box.Stretch = Stretch.Fill;
            //TestBattleParamSelectCanvas canvas = new TestBattleParamSelectCanvas(CurrentLanguage);
            Children.Add(box);//.box);
            Canvas.SetZIndex(box, 255);
        }

        /*public void MainBtn_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (LoginMenu==ELoginMenu.Login)
            {
                EmailBox_LostFocus(null, null);
                PasswordBox_LostFocus(null, null);
                if (ValidEmail && ValidPassword) Commands.Login(LoginEmailBox.Text, LoginPasswordBox.Password, LoginBox.IsChecked==true);
            }
            else if (LoginMenu==ELoginMenu.Sign)
            {
                EmailBox_LostFocus(null, null);
                NameBox_LostFocus(null, null);
                PasswordBox_LostFocus(null, null);
                PasswordConfirmBox_LostFocus(null, null);
                if (ValidEmail && ValidName && ValidPassword && ConfirmPassword)
                {
                    bool createresult = Commands.Create_Account(SignEmailBox.Text, SignPass1Box.Password, SignNameBox.Text);
                    if (createresult)
                        Commands.Login(SignEmailBox.Text, SignPass1Box.Password, LoginBox.IsChecked == true);
                }
            }
        }*/
        /*
        private void EmailBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (LoginMenu==ELoginMenu.Login)
            {
                if (LoginEmailBox.Text=="") { LoginEmailBox.Background = null;  ValidEmail = false; return; }
                if (Common.IsValidEmail(LoginEmailBox.Text)) ValidEmail = true; else ValidEmail = false;
                if (ValidEmail) LoginEmailBox.Background = Const.LightGreenBrush;
                else LoginEmailBox.Background = Const.LightRedBrush;
            }
            else if (LoginMenu==ELoginMenu.Sign)
            {
                if (Common.IsValidEmail(SignEmailBox.Text)) ValidEmail = true; else ValidEmail = false;
                if (ValidEmail) SignEmailBox.Background = Const.LightGreenBrush;
                else SignEmailBox.Background = Const.LightRedBrush;
                if (SignEmailBox.Text == "") { SignEmailBox.Background = null; ValidEmail = false; return; }
            }
        }
        private void NameBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (SignNameBox.Text == "") { ValidName = false;  SignNameBox.Background = null;  return; }
            ValidName = new GSString(SignNameBox.Text).CheckName();
            if (ValidName) SignNameBox.Background = Const.LightGreenBrush;
            else SignNameBox.Background = Const.LightRedBrush;
        }
        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (LoginMenu==ELoginMenu.Login)
            {
                if (LoginPasswordBox.Password == "") { ValidPassword = false;  LoginPasswordBox.Background = null;  return; }
                string password = LoginPasswordBox.Password;
                ValidPassword = new GSString(password).CheckPassword();
                if (ValidPassword) LoginPasswordBox.Background = Const.LightGreenBrush;
                else LoginPasswordBox.Background = Const.LightRedBrush;
            }
            else if (LoginMenu==ELoginMenu.Sign)
            {
                if (SignPass1Box.Password == "") { ValidPassword = false; SignPass1Box.Background = null; return; }
                string password = SignPass1Box.Password;
                ValidPassword = new GSString(password).CheckPassword();
                if (ValidPassword) SignPass1Box.Background = Const.LightGreenBrush;
                else SignPass1Box.Background = Const.LightRedBrush;
                if (SignPass2Box.Password.Length != 0) PasswordConfirmBox_LostFocus(null, null);
            }
        }
        private void PasswordConfirmBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (SignPass2Box.Password=="") { SignPass2Box.Background = null; ConfirmPassword = false; return; }
            if (SignPass1Box.Password == SignPass2Box.Password) ConfirmPassword = true;
            else ConfirmPassword = false;
            if (ConfirmPassword) SignPass2Box.Background = Const.LightGreenBrush;
            else SignPass2Box.Background = Const.LightRedBrush;
        }*/
        private void ChangeBtn_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (LoginMenu == ELoginMenu.Login)
                LoginMenu = ELoginMenu.Sign;
            else
                LoginMenu = ELoginMenu.Login;
            //SelectMenu();
            //SetLanguage();
            /*
            if (IsLogin)
            {
                LoginTextCanvas.Visibility = Visibility.Hidden;
                LoginTextMirror.Visibility = Visibility.Hidden;
                SignTextCanvas.Visibility = Visibility.Visible;
                SignTextMirror.Visibility = Visibility.Visible;
                IsLogin = false;
                NameBox_LostFocus(null, null);
                PasswordConfirmBox_LostFocus(null, null);
            }
            else
            {
                LoginTextCanvas.Visibility = Visibility.Visible;
                LoginTextMirror.Visibility = Visibility.Visible;
                SignTextCanvas.Visibility = Visibility.Hidden;
                SignTextMirror.Visibility = Visibility.Hidden;
                IsLogin = true;
            }
            SetLanguage();
            EmailBox_LostFocus(null, null);
            PasswordBox_LostFocus(null, null);
            */
        }

        private void LangBtn_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (CurrentLanguage == Languages.Ru)
                CurrentLanguage = Languages.En;
            else
                CurrentLanguage = Languages.Ru;
            //SetLanguage();
        }

        private void CloseBtn_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (LoginMenu == ELoginMenu.Start)
            {
                Links.Controller.mainWindow.Close();
                Links.Controller.loadWindow.Close();
            }
            else
            {
                LoginMenu = ELoginMenu.Start;
                //SelectMenu();
            }
        }

        public Label PutLabelToImage(int left, int top, Canvas canvas, int size)
        {
            Label lbl = new Label();
            canvas.Children.Add(lbl);
            Canvas.SetLeft(lbl, left);
            Canvas.SetTop(lbl, top);
            lbl.FontFamily = Links.Font;
            lbl.FontSize = size;
            lbl.FontWeight = FontWeights.Bold;
            lbl.Foreground = Brushes.White;
            return lbl;
        }
        public TextBox PutBoxToImage(int left, int top, Canvas canvas, RoutedEventHandler eventhandler)
        {
            TextBox text = new TextBox();
            canvas.Children.Add(text);
            Canvas.SetLeft(text, left);
            Canvas.SetTop(text, top);
            text.Width = 400;
            text.Height = 40;
            text.FontFamily = Links.Font;
            text.FontSize = 30;
            text.Background = Brushes.Transparent;
            text.FontWeight = FontWeights.Bold;
            text.Foreground = new SolidColorBrush(Color.FromArgb(255, 32, 255, 144));
            text.TextAlignment = TextAlignment.Center;
            if (eventhandler != null)
                text.LostFocus += eventhandler;
            return text;
        }
        public PasswordBox PutPasswordBoxToImage(int left, int top, Canvas canvas, RoutedEventHandler eventhandler)
        {
            PasswordBox tb = new PasswordBox();
            tb.Width = 400;
            canvas.Children.Add(tb);
            Canvas.SetLeft(tb, left);
            Canvas.SetTop(tb, top);
            tb.Height = 40;
            tb.FontSize = 30;
            tb.Background = Brushes.Transparent;
            tb.FontWeight = FontWeights.Bold;
            tb.Foreground = new SolidColorBrush(Color.FromArgb(255, 32, 255, 144));
            tb.HorizontalContentAlignment = HorizontalAlignment.Center;
            if (eventhandler != null)
                tb.LostFocus += eventhandler;
            return tb;

        }
        /*
        void TextPrepare()
        {
            LoginTexts = new SortedList<string, string[]>();
            LoginTexts.Add("LblEmail", Common.GetLangStr("Enter E-mail", "Введите Е-майл")); //0
            LoginTexts.Add("LblPass", Common.GetLangStr("Enter Password", "Введите пароль")); //1
            LoginTexts.Add("BtnLogin", Common.GetLangStr("Login", "Войти")); //2
            LoginTexts.Add("BtnExit", Common.GetLangStr("Exit", "Выход")); //3
            LoginTexts.Add("BtnSign", Common.GetLangStr("Sign", "Регистрация")); //4
            LoginTexts.Add("LblConfirm", Common.GetLangStr("Confirm Password", "Подтвердите пароль"));//5
            LoginTexts.Add("LblName", Common.GetLangStr("Enter Name", "Введите имя")); //6
            LoginTexts.Add("LblPassCreate", Common.GetLangStr("Create Password", "Создайте пароль")); //7
            LoginTexts.Add("BtnLang", Common.GetLangStr("En", "Ru"));
            LoginTexts.Add("LoginBox", Common.GetLangStr("Save Password", "Сохранить пароль"));
            LoginTexts.Add("TestBtn", Common.GetLangStr("Random Battle", "Случайный бой"));
            LoginTexts.Add("SingleBtn", Common.GetLangStr("Single player", "Одиночная игра"));
            LoginTexts.Add("MultiBtn", Common.GetLangStr("Online game", "Онлайн игра"));
        }
        void SetLanguage()
        {
            if (CurrentLanguage==Languages.En)
            {
                LoginEmailLbl.Content = LoginTexts["LblEmail"][0];
                LoginPassLbl.Content = LoginTexts["LblPass"][0];
                BoxText.Content = LoginTexts["LoginBox"][0];

                SignEmailLbl.Content = LoginTexts["LblEmail"][0];
                SignNameLbl.Content = LoginTexts["LblName"][0];
                SignPassLbl.Content = LoginTexts["LblPassCreate"][0];
                SignConfirmLbl.Content = LoginTexts["LblConfirm"][0];

                if (LoginMenu==ELoginMenu.Login)
                {
                    MainBtn.SetText(LoginTexts["BtnLogin"][0]);
                    ChangeBtn.SetText(LoginTexts["BtnSign"][0]);
                }
                else if (LoginMenu==ELoginMenu.Sign)
                {
                    MainBtn.SetText(LoginTexts["BtnSign"][0]);
                    ChangeBtn.SetText(LoginTexts["BtnLogin"][0]);
                }
                LangBtn.SetText(LoginTexts["BtnLang"][0]);
                CloseBtn.SetText(LoginTexts["BtnExit"][0]);
                //TestBtn.SetText(LoginTexts["TestBtn"][0]);
                //TitleRect.Fill = Links.EnTitleBrush;
                SingleButton.SetText(LoginTexts["SingleBtn"][0]);
                MultiButton.SetText(LoginTexts["MultiBtn"][0]);
                RandomButton.SetText(LoginTexts["TestBtn"][0]);
            }
            else
            {
                LoginEmailLbl.Content = LoginTexts["LblEmail"][1];
                LoginPassLbl.Content = LoginTexts["LblPass"][1];
                BoxText.Content = LoginTexts["LoginBox"][1];

                SignEmailLbl.Content = LoginTexts["LblEmail"][1];
                SignNameLbl.Content = LoginTexts["LblName"][1];
                SignPassLbl.Content = LoginTexts["LblPassCreate"][1];
                SignConfirmLbl.Content = LoginTexts["LblConfirm"][1];

                if (LoginMenu == ELoginMenu.Login)
                {
                    MainBtn.SetText(LoginTexts["BtnLogin"][1]);
                    ChangeBtn.SetText(LoginTexts["BtnSign"][1]);
                }
                else if(LoginMenu == ELoginMenu.Sign)
                {
                    MainBtn.SetText(LoginTexts["BtnSign"][1]);
                    ChangeBtn.SetText(LoginTexts["BtnLogin"][1]);
                }
                LangBtn.SetText(LoginTexts["BtnLang"][1]);
                CloseBtn.SetText(LoginTexts["BtnExit"][1]);
                //TestBtn.SetText(LoginTexts["TestBtn"][1]);
                SingleButton.SetText(LoginTexts["SingleBtn"][1]);
                MultiButton.SetText(LoginTexts["MultiBtn"][1]);
                RandomButton.SetText(LoginTexts["TestBtn"][1]);
                //TitleRect.Fill = Links.RuTitleBrush;
            }
        }
        */
    }
    class SingleGameStartCanvas2 : Viewbox
    {
        public SingleGameStartCanvas2()
        {
            Width = 1294; Height = 758; Stretch = Stretch.Fill;
            Canvas canvas = new Canvas();
            Child = canvas;
            canvas.Width = 1920; canvas.Height = 1080; canvas.Background = Gets.GetBrushInt("kosmos3");
            BTN btn1 = new BTN();
            canvas.Children.Add(btn1); Canvas.SetLeft(btn1, 1532); Canvas.SetTop(btn1, 593);

            BTN btn2 = new BTN();
            canvas.Children.Add(btn2); Canvas.SetLeft(btn2, 1532); Canvas.SetTop(btn2, 675);
            btn2.PreviewMouseDown += NewGame_PreviewMouseDown;

            BTN btn3 = new BTN();
            canvas.Children.Add(btn3); Canvas.SetLeft(btn3, 1532); Canvas.SetTop(btn3, 754);

            BTN btn4 = new BTN();
            canvas.Children.Add(btn4); Canvas.SetLeft(btn4, 1532); Canvas.SetTop(btn4, 835);
            btn4.PreviewMouseDown += Close_PreviewMouseDown;
        }

        private void NewGame_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Commands.Start_Single_Game();
        }

        private void Close_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Links.Controller.LoginCanvas.Children.Remove(this);
        }
        class BTN:Canvas
        {
            Rectangle rect1;
            public BTN()
            {
                rect1 = Common.GetRectangle(380, 150, Gets.GetBrushInt("kosmos31"));
                Children.Add(rect1);
                rect1.Opacity = 0;
                Rectangle rect2 = Common.GetRectangle(310, 50, Links.Brushes.Transparent);
                Children.Add(rect2);
                Canvas.SetLeft(rect2, 40);Canvas.SetTop(rect2, 38);
                rect2.MouseEnter += Rect2_MouseEnter;
                rect2.MouseLeave += Rect2_MouseLeave;
            }

            private void Rect2_MouseLeave(object sender, MouseEventArgs e)
            {
                rect1.BeginAnimation(Rectangle.OpacityProperty, null);
            }

            private void Rect2_MouseEnter(object sender, MouseEventArgs e)
            {
                DoubleAnimation anim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.2));
                rect1.BeginAnimation(Rectangle.OpacityProperty, anim);
            }
        }
    }

    class SingleGameStartCanvas:Canvas
    {
        public Viewbox box;
        public SingleGameStartCanvas(Languages lang)
        {
            box = new Viewbox(); box.Width = 1294; box.Height = 758;
            box.Child = this;
            Width = 1294; Height = 758; Background = Brushes.Black;

            GameButton NewGame = new GameButton(400, 200, "", 50);
            NewGame.PutToCanvas(this, 447, 100);
            NewGame.PreviewMouseDown += NewGame_PreviewMouseDown;

            GameButton LoadGame = new GameButton(400, 200, "", 50);
            LoadGame.PutToCanvas(this, 447, 325);
            LoadGame.PreviewMouseDown += LoadGame_PreviewMouseDown;
                        
            GameButton Close = new GameButton(400, 200, "", 50);
            //InterfaceButton Close = new InterfaceButton(120, 40, 7, 20);
            Close.PutToCanvas(this, 447, 550);
            Close.PreviewMouseDown += Close_PreviewMouseDown;

            switch (lang)
            {
                case Languages.En:
                    NewGame.SetText("New Game");
                    LoadGame.SetText("Load Game");
                    Close.SetText("Close");
                    break;
                case Languages.Ru:
                    NewGame.SetText("Новая игра");
                    LoadGame.SetText("Загрузить");
                    Close.SetText("Закрыть");
                    break;
            }

        }

        private void LoadGame_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Commands.Load_Single_Game();
            }
            catch (Exception)
            {
                MessageBox.Show("Load game error");
                Commands.Stop_Game();

            }
        }

        private void NewGame_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Commands.Start_Single_Game();
        }

        private void Close_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Links.Controller.LoginCanvas.Children.Remove(box);
        }
    }
    class TestBattleParamSelectCanvas:Canvas
    {
        List<SelectedButton> Hardness;
        List<SelectedButton> Levels;
        List<SelectedButton> Ships;
        List<SelectedButton> Size;
        List<SelectedButton> Asteroids;
        public Viewbox box;
        public TestBattleParamSelectCanvas(Languages lang)
        {
            box = new Viewbox(); box.Width = 1294; box.Height = 758;
            box.Child = this;
            Width = 560; Height = 300; Background = Brushes.Black;
            GameButton Start = new GameButton(120, 40, "", 20);
            //InterfaceButton Start = new InterfaceButton(120, 40, 7, 20);
            Start.PutToCanvas(this, 50, 255);
            Start.PreviewMouseDown += Start_PreviewMouseDown;

            GameButton Random = new GameButton(120, 40, "", 20);
            //InterfaceButton Random = new InterfaceButton(120, 40, 7, 20);
            Random.PutToCanvas(this, 230, 255);
            Random.PreviewMouseDown += Random_PreviewMouseDown;
            Random.RenderTransformOrigin = new Point(0.5, 0.5);

            GameButton Close = new GameButton(120, 40, "", 20);
            //InterfaceButton Close = new InterfaceButton(120, 40, 7, 20);
            Close.PutToCanvas(this, 400, 255);
            Close.PreviewMouseDown += Close_PreviewMouseDown;
            if (lang == Languages.En)
            {
                Hardness = AddBorder(10, "Dificulty", "Easy", "Middle", "Hard");
                Levels = AddBorder(120, "Science level", "Beginner", "Middle", "High");
                Ships = AddBorder(230, "Ships count", "Small", "Middle", "Many");
                Size = AddBorder(340, "Battle field size", "Small", "Middle", "Large");
                Asteroids = AddBorder(450, "Asteroids count", "No", "Small", "Middle");
                Start.SetText("Start");
                Random.SetText("Random");
                Close.SetText("Close");
            }
            else
            {
                Hardness = AddBorder(10, "Сложность", "Низкая", "Средняя", "Тяжёлая");
                Levels = AddBorder(120, "Уровень технологий", "Начальный", "Средний", "Высокий");
                Ships = AddBorder(230, "Количество кораблей", "Мало", "Средне", "Много");
                Size = AddBorder(340, "Размер поля боя", "Малое", "Среднее", "Большое");
                Asteroids = AddBorder(450, "Количество астероидов", "Нет", "Мало", "Средне");
                Start.SetText("Начать");
                Random.SetText("Случайно");
                Close.SetText("Закрыть");
            }
            
        }

        private void Start_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            List<byte> list = new List<byte>();
            for (byte i = 0; i < 3; i++)
                if (Hardness[i].IsSelected) list.Add(i);
            for (byte i = 0; i < 3; i++)
                if (Levels[i].IsSelected) list.Add(i);
            for (byte i = 0; i < 3; i++)
                if (Ships[i].IsSelected) list.Add(i);
            for (byte i = 0; i < 3; i++)
                if (Size[i].IsSelected) list.Add(i);
            for (byte i = 0; i < 3; i++)
                if (Asteroids[i].IsSelected) list.Add(i);
            Commands.Start_Test_Game(list.ToArray());
        }


        private void Close_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Links.Controller.LoginCanvas.Children.Remove(box);
        }

        private void Random_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Random rnd = new Random();
            int a = rnd.Next(3);
            Hardness[a].SelectedButton_PreviewMouseDown(null, null);
            int b = rnd.Next(3);
            Levels[a].SelectedButton_PreviewMouseDown(null, null);
            int c = rnd.Next(3);
            Ships[c].SelectedButton_PreviewMouseDown(null, null);
            int d = rnd.Next(3);
            Size[d].SelectedButton_PreviewMouseDown(null, null);
            int f = rnd.Next(3);
            Asteroids[f].SelectedButton_PreviewMouseDown(null, null);
            ScaleTransform Scale = new ScaleTransform(0.8, 0.8);
            GameButton btn = (GameButton)sender;
            btn.RenderTransform = Scale;
            DoubleAnimation anim = new DoubleAnimation(0.9, 1, TimeSpan.FromSeconds(0.05));
            Scale.BeginAnimation(ScaleTransform.ScaleXProperty, anim);
            Scale.BeginAnimation(ScaleTransform.ScaleYProperty, anim);
        }

        List<SelectedButton> AddBorder(int left, string text0, string text1, string text2, string text3)
        {
            Border border = new Border(); border.Width = 100; border.Height = 220;
            Children.Add(border); Canvas.SetLeft(border, left); Canvas.SetTop(border, 30);
            border.BorderThickness = new Thickness(1);
            border.BorderBrush = new SolidColorBrush((Color.FromRgb(0, 128, 255)));
            StackPanel panel = new StackPanel();
            panel.Orientation = Orientation.Vertical;
            border.Child = panel;
            TextBlock text = new TextBlock();
            text.Foreground = Brushes.White;
            text.TextAlignment = TextAlignment.Center;
            text.Height = 35;
            text.TextWrapping = TextWrapping.Wrap;
            text.Text = text0;
            panel.Children.Add(text);
            List<SelectedButton> list = new List<SelectedButton>();
            SelectedButton btn1 = new SelectedButton(text1);
            panel.Children.Add(btn1);
            list.Add(btn1);
            SelectedButton btn2 = new SelectedButton(text2);
            panel.Children.Add(btn2);
            list.Add(btn2);
            SelectedButton btn3 = new SelectedButton(text3);
            panel.Children.Add(btn3);
            list.Add(btn3);
            btn1.Buttons = list; btn2.Buttons = list; btn3.Buttons = list;
            btn2.Select();
            return list;
        }
    }
    class SelectedButton:Canvas
    {
        public bool IsSelected;
        ScaleTransform Scale;
        public List<SelectedButton> Buttons;
        static ImageBrush brush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/resources/BtnAdd.png")));
        public SelectedButton(string text)
        {
            Width = 80; Height = 40; Margin = new Thickness(10);
            Background = brush;
            RenderTransformOrigin = new Point(0.5, 0.5);
            Scale = new ScaleTransform();
            RenderTransform = Scale;
            TextBlock block = Common.GetBlock(16, text, Brushes.White, 80);
            Children.Add(block); Canvas.SetTop(block, 10);
            PreviewMouseDown += SelectedButton_PreviewMouseDown;
        }

        public void SelectedButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
           if (IsSelected)
            {
                DeSelect();
                for (int i=0;i<Buttons.Count;i++)
                {
                    if (Buttons[i] == this)
                        continue;
                    else
                    {
                        Buttons[i].Select();
                        return;
                    }
                }
            }
           else
            {
                Select();
                for (int i=0; i<Buttons.Count;i++)
                {
                    if (Buttons[i] == this) continue;
                    else
                        Buttons[i].DeSelect();
                }
            }
        }
        public void Select()
        {
            Scale.ScaleX = 0.8; Scale.ScaleY = 0.8; IsSelected = true;
        }
        public void DeSelect()
        {
            Scale.ScaleX = 1.0; Scale.ScaleY = 1.0; IsSelected = false;
        }
    }
    class ButtonV2 : Canvas
    {
        Brush EnB1, EnB2, RuB1, RuB2;
        ScaleTransform Scale;
        byte Lang = 0;
        Rectangle Back;
        Rectangle Back1;
        public ButtonV2(int width, int height, Brush enB1, Brush enB2, Brush ruB1, Brush ruB2)
        {
            Back = new Rectangle(); Back.Width = width; Back.Height = height;
            Back1 = new Rectangle(); Back1.Width = width; Back1.Height = height;
            Children.Add(Back); Children.Add(Back1);
            Back1.Opacity = 0;
            Width = width; Height = height;
            EnB1 = enB1; EnB2 = enB2; RuB1 = ruB1; RuB2 = ruB2;
            RenderTransformOrigin = new Point(0.5, 0.5);
            RenderTransform = Scale = new ScaleTransform();
            Back.Fill = EnB1;
            Back1.Fill = EnB2;
        }
        public void SetLang(byte lang)
        {
            Lang = lang;
            if (Lang == 0)
            { Back.Fill = EnB1; Back1.Fill = EnB2; }
            else
            { Back.Fill = RuB1; Back1.Fill = RuB2; }
        }
        static TimeSpan time = TimeSpan.FromSeconds(1.0 / 3);
        static TimeSpan clicktime = TimeSpan.FromSeconds(1.0 / 10);
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            DoubleAnimation anim = new DoubleAnimation(1, 0, time);
            DoubleAnimation anim1 = new DoubleAnimation(0, 1, time);
            Back.BeginAnimation(Rectangle.OpacityProperty, anim);
            Back1.BeginAnimation(Rectangle.OpacityProperty, anim1);

            //base.OnMouseEnter(e);
        }
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            DoubleAnimation anim = new DoubleAnimation(1, 0, time);
            DoubleAnimation anim1 = new DoubleAnimation(0, 1, time);
            Back.BeginAnimation(Rectangle.OpacityProperty, anim1);
            Back1.BeginAnimation(Rectangle.OpacityProperty, anim);

            //base.OnMouseLeave(e);
        }
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            DoubleAnimation anim = new DoubleAnimation(1, 0.95, clicktime);
            anim.AutoReverse = true;
            Scale.BeginAnimation(ScaleTransform.ScaleXProperty, anim);
            Scale.BeginAnimation(ScaleTransform.ScaleYProperty, anim);
        }
    }
}
