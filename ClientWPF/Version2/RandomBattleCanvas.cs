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
    class RandomBattleCanvas : Canvas
    {
        static PathFigureCollectionConverter conv = new PathFigureCollectionConverter();
        static PathGeometry path1 = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M500,100 a500,500 0 0,1 400,150"));
        static PathGeometry path2 = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M900,250 a400,400 0 0,1 -400,600"));
        static PathGeometry path3 = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M500,850 a500,500 0 0,1 -450,-200"));
        static PathGeometry path4 = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M50,650 a500,500 0 0,1 0,-400"));
        static PathGeometry path5 = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M50,250 a600,500 0 0,1 450,-150"));
        static TimeSpan animtime = TimeSpan.FromSeconds(1.0);
        static DoubleAnimationUsingPath[] MoveAnims = CreateAnims();
        static DoubleAnimation ScaleUp = new DoubleAnimation(1, animtime);
        static DoubleAnimation ScaleDown = new DoubleAnimation(0.4, animtime);
        public Viewbox box;
        public static byte CurPos = 4;
        public static bool IsAnimated = false;
        byte TargetPos = 4;
        System.Windows.Threading.DispatcherTimer Timer;
        Canvas AsteroidCanvas;
        ScaleTransform AsteroidTransform;
        Rectangle AstroImage;
        ButtonV3 AstroButton;
        byte astro = 1;
        Canvas FieldSizeCanvas;
        ScaleTransform FieldSizeTransform;
        Rectangle FieldSizeImage;
        ButtonV3 FieldSizeButton;
        byte field = 1;
        Canvas ShipsCanvas;
        ScaleTransform ShipsTransform;
        ButtonV3 Ship1;
        ButtonV3 Ship2;
        ButtonV3 Ship3;
        byte ships = 1;
        Canvas DifCanvas;
        ScaleTransform DifTransform;
        byte dificulty = 1;
        Rectangle DifImage;
        ButtonV3 DifButton;
        TextBlock DifText;
        Canvas LevelCanvas;
        ScaleTransform LevelTransform;
        byte science = 1;
        Rectangle TechImage;
        public RandomBattleCanvas(byte lang)
        {
            Width = 1920; Height = 1080;
            CurPos = 4;
            Background = GetImage("MainBack");
            AddMoveLine("LeftSide", 0, -1080, 25);
            AddMoveLine("RightSide", 1895, 1080, 15);
            PutRectangle(1087, 121, 0, 0, "TopText", this);
            PutRectangle(626, 716, 300, 240, "Planet", this);
            CreateAsteroidCanvas();
            CreateFieldSizeCanvas();
            CreateShipsCanvas();
            CreateDifCanvas();
            CreateLevelCanvas();
            Select();
            ButtonV3 Begin = new ButtonV3(476, 194, GetImage("Start1"), GetImage("Start0"), 0, 880, this, 10);
            Begin.PreviewMouseDown += Begin_PreviewMouseDown;
            ButtonV3 Random = new ButtonV3(477, 194, GetImage("Random1"),GetImage("Random0"), 1300, 880, this, 10);
            Random.PreviewMouseDown += Random_PreviewMouseDown;
            ButtonV3 Back = new ButtonV3(218, 57, GetImage("Back1"),GetImage("Back0"), 1680, 15, this, 10);
            Back.PreviewMouseDown += Back_PreviewMouseDown;
            PutRectangle(472, 146, 300, 500, "PlanetInfo", this);
            Timer = new System.Windows.Threading.DispatcherTimer();
            Timer.Interval = TimeSpan.FromSeconds(1);
            Timer.Tick += Timer_Tick;
            RandomTimer = new System.Windows.Threading.DispatcherTimer();
            RandomTimer.Tick += RandomTimer_Tick;
        }

        private void Begin_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            List<byte> list = new List<byte>();
            list.Add(dificulty);
            list.Add(science);
            list.Add(ships);
            list.Add(field);
            list.Add(astro);
            Commands.Start_Test_Game(list.ToArray());
        }

        private void Back_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Links.Controller.LoginCanvas.Children.Remove(box);
        }

        private void RandomTimer_Tick(object sender, EventArgs e)
        {
            RandomTimer.Stop();
            randominterval += 0.1;
            if (randominterval >= 0.5)
            {
                IsAnimated = false; return;
            }
            Random rnd = new Random();
            astro = (byte)rnd.Next(0, 3);
            field = (byte)rnd.Next(0, 3);
            ships = (byte)rnd.Next(0, 3);
            dificulty = (byte)rnd.Next(0, 3);
            science = (byte)rnd.Next(0, 3);
            Select();
            RandomTimer.Interval = TimeSpan.FromSeconds(randominterval);
            RandomTimer.Start();
        }

        System.Windows.Threading.DispatcherTimer RandomTimer;
        double randominterval = 0.1;
        private void Random_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            IsAnimated = true;
            randominterval = 0.1;
            RandomTimer.Interval = TimeSpan.FromSeconds(randominterval);
            RandomTimer.Start();
            //RandomTimer_Tick(null, null);
        }

        void AddMoveLine(string name, int left, int target, int time)
        {
            Canvas canvas = new Canvas(); canvas.Height = 1080 * 2; canvas.Width = 25;
            Children.Add(canvas);
            Canvas.SetLeft(canvas, left);
            Rectangle rect1 = new Rectangle(); rect1.Width = 25; rect1.Height = 1080;
            canvas.Children.Add(rect1); rect1.Fill = GetImage(name);
            Rectangle rect2 = new Rectangle(); rect2.Width = 25; rect2.Height = 1080;
            canvas.Children.Add(rect2); rect2.Fill = rect1.Fill;
            Canvas.SetTop(rect2, -target);
            DoubleAnimation anim = new DoubleAnimation(0, target, TimeSpan.FromSeconds(time));
            anim.RepeatBehavior = RepeatBehavior.Forever;
            canvas.BeginAnimation(Canvas.TopProperty, anim);
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (TargetPos == CurPos)
            {
                IsAnimated = false;
                Timer.Stop();
                TargetPos = CurPos;
                return;
            }
            CurPos = (byte)((CurPos + 1) % 5);
            AsteroidCanvas.BeginAnimation(Canvas.LeftProperty, MoveAnims[((CurPos + 5 + 5) % 5) * 2]);
            AsteroidCanvas.BeginAnimation(Canvas.TopProperty, MoveAnims[((CurPos + 5 + 5) % 5) * 2 + 1]);
            FieldSizeCanvas.BeginAnimation(Canvas.LeftProperty, MoveAnims[((CurPos + 5 + 4) % 5) * 2]);
            FieldSizeCanvas.BeginAnimation(Canvas.TopProperty, MoveAnims[((CurPos + 5 + 4) % 5) * 2 + 1]);
            ShipsCanvas.BeginAnimation(Canvas.LeftProperty, MoveAnims[((CurPos + 5 + 3) % 5) * 2]);
            ShipsCanvas.BeginAnimation(Canvas.TopProperty, MoveAnims[((CurPos + 5 + 3) % 5) * 2 + 1]);
            DifCanvas.BeginAnimation(Canvas.LeftProperty, MoveAnims[((CurPos + 5 + 2) % 5) * 2]);
            DifCanvas.BeginAnimation(Canvas.TopProperty, MoveAnims[((CurPos + 5 + 2) % 5) * 2 + 1]);
            LevelCanvas.BeginAnimation(Canvas.LeftProperty, MoveAnims[((CurPos + 5 + 1) % 5) * 2]);
            LevelCanvas.BeginAnimation(Canvas.TopProperty, MoveAnims[((CurPos + 5 + 1) % 5) * 2 + 1]);
            switch (CurPos)
            {
                case 0:
                    AsteroidTransform.BeginAnimation(ScaleTransform.ScaleXProperty, ScaleUp);
                    AsteroidTransform.BeginAnimation(ScaleTransform.ScaleYProperty, ScaleUp);
                    LevelTransform.BeginAnimation(ScaleTransform.ScaleXProperty, ScaleDown);
                    LevelTransform.BeginAnimation(ScaleTransform.ScaleYProperty, ScaleDown);
                    break;
                case 1:
                    FieldSizeTransform.BeginAnimation(ScaleTransform.ScaleXProperty, ScaleUp);
                    FieldSizeTransform.BeginAnimation(ScaleTransform.ScaleYProperty, ScaleUp);
                    AsteroidTransform.BeginAnimation(ScaleTransform.ScaleXProperty, ScaleDown);
                    AsteroidTransform.BeginAnimation(ScaleTransform.ScaleYProperty, ScaleDown);
                    break;
                case 2:
                    ShipsTransform.BeginAnimation(ScaleTransform.ScaleXProperty, ScaleUp);
                    ShipsTransform.BeginAnimation(ScaleTransform.ScaleYProperty, ScaleUp);
                    FieldSizeTransform.BeginAnimation(ScaleTransform.ScaleXProperty, ScaleDown);
                    FieldSizeTransform.BeginAnimation(ScaleTransform.ScaleYProperty, ScaleDown);
                    break;
                case 3:
                    DifTransform.BeginAnimation(ScaleTransform.ScaleXProperty, ScaleUp);
                    DifTransform.BeginAnimation(ScaleTransform.ScaleYProperty, ScaleUp);
                    ShipsTransform.BeginAnimation(ScaleTransform.ScaleXProperty, ScaleDown);
                    ShipsTransform.BeginAnimation(ScaleTransform.ScaleYProperty, ScaleDown);
                    break;
                case 4:
                    LevelTransform.BeginAnimation(ScaleTransform.ScaleXProperty, ScaleUp);
                    LevelTransform.BeginAnimation(ScaleTransform.ScaleYProperty, ScaleUp);
                    DifTransform.BeginAnimation(ScaleTransform.ScaleXProperty, ScaleDown);
                    DifTransform.BeginAnimation(ScaleTransform.ScaleYProperty, ScaleDown);

                    break;
            }
        }

        void CreateAsteroidCanvas()
        {
            AsteroidCanvas = new Canvas();
            AsteroidCanvas.Width = 800; AsteroidCanvas.Height = 600;
            AsteroidCanvas.RenderTransform = AsteroidTransform = new ScaleTransform(0.35, 0.35);
            AsteroidCanvas.Tag = 0;
            //AsteroidCanvas.Background = Brushes.White;
            Canvas.SetLeft(AsteroidCanvas, 500); Canvas.SetTop(AsteroidCanvas, 100);
            Children.Add(AsteroidCanvas);
            PutRectangle(906, 508, -40, 50, "AstroBack", AsteroidCanvas);
            AstroImage = PutRectangle(823, 429, -10, 85, "AstroLow", AsteroidCanvas);
            AstroButton = new ButtonV3(380, 56, GetImage("Astro11"), GetImage("Astro10"), 350, 455, AsteroidCanvas, 0);
            AstroButton.PreviewMouseDown += AstroButton_PreviewMouseDown;
            AsteroidCanvas.PreviewMouseDown += Canvas_PreviewMouseDown;
        }
        void CreateFieldSizeCanvas()
        {
            FieldSizeCanvas = new Canvas();
            FieldSizeCanvas.Width = 800; FieldSizeCanvas.Height = 600;
            FieldSizeCanvas.RenderTransform = FieldSizeTransform = new ScaleTransform(0.35, 0.35);
            FieldSizeCanvas.Tag = 1;
            //FieldSizeCanvas.Background = Brushes.White;
            Canvas.SetLeft(FieldSizeCanvas, 50); Canvas.SetTop(FieldSizeCanvas, 250);
            Children.Add(FieldSizeCanvas);
            PutRectangle(880, 440, -40, 100, "FieldSizeBack", FieldSizeCanvas);
            FieldSizeImage = PutRectangle(586, 236, 101, 165, "FieldSizeSmall", FieldSizeCanvas);
            FieldSizeButton = new ButtonV3(405, 65, GetImage("FieldSize00"),GetImage("FieldSize01"), 70, 427, FieldSizeCanvas, 1);
            FieldSizeButton.PreviewMouseDown += FieldSizeButton_PreviewMouseDown;
            FieldSizeCanvas.PreviewMouseDown += Canvas_PreviewMouseDown;
        }
        void CreateShipsCanvas()
        {
            ShipsCanvas = new Canvas();
            ShipsCanvas.Width = 800; ShipsCanvas.Height = 600;
            ShipsCanvas.RenderTransform = ShipsTransform = new ScaleTransform(0.35, 0.35);
            ShipsCanvas.Tag = 2;
            //ShipsCanvas.Background = new SolidColorBrush(Color.FromArgb(100, 255, 255, 255));// ShipsCanvas.Opacity = 0.4;
            Canvas.SetLeft(ShipsCanvas, 60); Canvas.SetTop(ShipsCanvas, 650);
            Children.Add(ShipsCanvas);
            PutRectangle(1120, 486, 0, 50, "ShipInfo", ShipsCanvas);
            PutRectangle(358, 361, 60, 120, "Ship", ShipsCanvas);
            Ship1 = new ButtonV3(253, 104,GetImage("Ship01"), GetImage("Ship00"), 425, 135, ShipsCanvas, 2);
            Ship1.PreviewMouseDown += Ship_PreviewMouseDown; Ship1.Tag = 0;
            Ship2 = new ButtonV3(253, 104, GetImage("Ship11"), GetImage("Ship10"), 425, 245, ShipsCanvas, 2);
            Ship2.PreviewMouseDown += Ship_PreviewMouseDown; Ship2.Tag = 1;
            Ship3 = new ButtonV3(253, 104, GetImage("Ship21"), GetImage("Ship20"), 425, 360, ShipsCanvas, 2);
            Ship3.PreviewMouseDown += Ship_PreviewMouseDown; Ship3.Tag = 2;
            ShipsCanvas.PreviewMouseDown += Canvas_PreviewMouseDown;
        }

        private void Ship_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (RandomBattleCanvas.IsAnimated) return;
            if (CurPos != 2) return;
            ButtonV3 btn = (ButtonV3)sender;
            ships = (byte)(int)(btn.Tag);
            Select();
        }

        void CreateDifCanvas()
        {
            DifCanvas = new Canvas();
            DifCanvas.Width = 800; DifCanvas.Height = 600;
            DifCanvas.RenderTransform = DifTransform = new ScaleTransform(0.35, 0.35);
            DifCanvas.Tag = 3;
            //DifCanvas.Background = new SolidColorBrush(Color.FromArgb(100, 0, 255, 0));// DifCanvas.Opacity = 0.4;
            Canvas.SetLeft(DifCanvas, 500); Canvas.SetTop(DifCanvas, 850);
            Children.Add(DifCanvas);
            PutRectangle(710, 710, -80, -80, "DifRound", DifCanvas);
            DifImage = PutRectangle(490, 490, 40, 40, "DifMiddle", DifCanvas);
            DifButton = new ButtonV3(573, 193, GetImage("Dif10"), GetImage("Dif11"), 380, -30, DifCanvas, 3);
            PutRectangle(500, 325, 500, 150, "DifInfo", DifCanvas);
            DifText = new TextBlock(); DifText.Width = 400;
            DifCanvas.Children.Add(DifText); Canvas.SetLeft(DifText, 550); Canvas.SetTop(DifText, 180);
            DifText.FontSize = 24; DifText.TextWrapping = TextWrapping.Wrap; DifText.Foreground = new SolidColorBrush(Color.FromArgb(255, 100, 255, 255));
            DifText.Text = "На сложности колонизатор вы имеете небольшое преимущество в силах - уровне кораблей и их количестве";
            DifButton.PreviewMouseDown += DifButton_PreviewMouseDown;
            DifCanvas.PreviewMouseDown += Canvas_PreviewMouseDown;
        }

        private void DifButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (RandomBattleCanvas.IsAnimated) return;
            if (CurPos != 3) return;
            dificulty = (byte)((dificulty + 1) % 3);
            Select();
        }

        void CreateLevelCanvas()
        {
            LevelCanvas = new Canvas();
            LevelCanvas.Width = 800; LevelCanvas.Height = 600;
            LevelCanvas.RenderTransform = LevelTransform = new ScaleTransform(1, 1);
            LevelCanvas.Tag = 4;
            //LevelCanvas.Background = new SolidColorBrush(Color.FromArgb(100, 255, 0, 0));//LevelCanvas.Opacity = 0.4;
            Canvas.SetLeft(LevelCanvas, 900); Canvas.SetTop(LevelCanvas, 250);
            Children.Add(LevelCanvas);
            PutRectangle(510, 510, 20, 100, "DifRound", LevelCanvas);
            TechImage = PutRectangle(350, 320, 110, 200, "TechMedium", LevelCanvas);
            ButtonV3 TechButton = new ButtonV3(336, 178, GetImage("Tech0"),GetImage("Tech0"), 100, 0, LevelCanvas, 4);
            TechButton.PreviewMouseDown += TechButton_PreviewMouseDown;
            LevelCanvas.PreviewMouseDown += Canvas_PreviewMouseDown;
        }

        private void TechButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (RandomBattleCanvas.IsAnimated) return;
            if (CurPos != 4) return;
            science = (byte)((science + 1) % 3);
            Select();
        }

        private void Canvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (RandomBattleCanvas.IsAnimated) return;
            int tag = (int)((Canvas)sender).Tag;
            if (CurPos == tag) return;
            TargetPos = (byte)tag;
            IsAnimated = true;
            Timer.Start();
            Timer_Tick(null, null);
        }

        private void FieldSizeButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (RandomBattleCanvas.IsAnimated) return;
            if (CurPos != 1) return;
            field = (byte)((field + 1) % 3);
            Select();
        }

        private void AstroButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (RandomBattleCanvas.IsAnimated) return;
            if (CurPos != 0) return;
            astro = (byte)((astro + 1) % 3);
            Select();

        }

        void Select()
        {
            if (AstroImage != null)
            {
                if (astro == 0) AstroImage.Fill = GetImage("AstroNo");
                else if (astro == 1) AstroImage.Fill = GetImage("AstroLow");
                else AstroImage.Fill = GetImage("AstroMiddle");
            }
            if (AstroButton != null)
            {
                if (astro == 0) AstroButton.ChangeImage(GetImage("Astro01"), GetImage("Astro00"));
                else if (astro == 1) AstroButton.ChangeImage(GetImage("Astro11"),GetImage("Astro10"));
                else AstroButton.ChangeImage(GetImage("Astro21"), GetImage("Astro20"));
            }
            if (FieldSizeImage != null)
            {
                if (field == 0) FieldSizeImage.Fill = GetImage("FieldSizeSmall");
                else if (field == 1) FieldSizeImage.Fill = GetImage("FieldSizeMedium");
                else FieldSizeImage.Fill = GetImage("FieldSizeLarge");
            }
            if (DifImage != null)
            {
                if (dificulty == 0) DifImage.Fill = GetImage("DifLow");
                else if (dificulty == 1) DifImage.Fill = GetImage("DifMiddle");
                else DifImage.Fill =GetImage("DifHigh");
            }
            if (DifButton != null)
            {
                if (dificulty == 0) DifButton.ChangeImage(GetImage("Dif00"), GetImage("Dif01"));
                else if (dificulty == 1) DifButton.ChangeImage(GetImage("Dif10"),GetImage("Dif11"));
                else DifButton.ChangeImage(GetImage("Dif20"), GetImage("Dif21"));
            }
            if (DifText != null)
            {
                if (dificulty == 0) DifText.Text = "На уровне сложности колонизатор вы имеете небольшое преимущество в силах - уровне кораблей и их количестве";
                else if (dificulty == 1) DifText.Text = "На уровне сложности завоеватель вы сражатесь с равным по силам противником";
                else DifText.Text = "На этом уровне сложности вражеский флот имеет существенное преимущество в технологиях и кораблях, победить его - задача для настоящего адмирала";
            }
            if (ships == 0)
            {
                Ship1.Select(); Ship2.DeSelect(); Ship3.DeSelect();
            }
            else if (ships == 1)
            {
                Ship1.DeSelect(); Ship2.Select(); Ship3.DeSelect();
            }
            else
            {
                Ship1.DeSelect(); Ship2.DeSelect(); Ship3.Select();
            }
            if (TechImage != null)
            {
                if (science == 0) TechImage.Fill = GetImage("TechLow");
                else if (science == 1) TechImage.Fill = GetImage("TechMedium");
                else TechImage.Fill = GetImage("TechHigh");
            }
        }
        static ImageBrush GetImage(string text)
        {
            if (Links.LoadImageFromDLL)
                return new ImageBrush(new BitmapImage(new Uri(string.Format("pack://application:,,,/GraphicLibrary;component/Images/TestBattle/{0}.png", text))));
            else
                return new ImageBrush(new BitmapImage(new Uri(string.Format("Images/TestBattle/{0}.png", text), UriKind.Relative)));
        }
        static Rectangle PutRectangle(int width, int height, int left, int top, string text, Canvas canvas)
        {
            Rectangle rect = new Rectangle(); rect.Width = width; rect.Height = height;
            rect.Fill = GetImage(text);
            Canvas.SetLeft(rect, left); Canvas.SetTop(rect, top);
            if (canvas != null)
                canvas.Children.Add(rect);
            return rect;
        }
        static DoubleAnimationUsingPath[] CreateAnims()
        {
            DoubleAnimationUsingPath[] anims = new DoubleAnimationUsingPath[10];
            anims[0] = GetAnim(path1, PathAnimationSource.X);
            anims[1] = GetAnim(path1, PathAnimationSource.Y);
            anims[2] = GetAnim(path2, PathAnimationSource.X);
            anims[3] = GetAnim(path2, PathAnimationSource.Y);
            anims[4] = GetAnim(path3, PathAnimationSource.X);
            anims[5] = GetAnim(path3, PathAnimationSource.Y);
            anims[6] = GetAnim(path4, PathAnimationSource.X);
            anims[7] = GetAnim(path4, PathAnimationSource.Y);
            anims[8] = GetAnim(path5, PathAnimationSource.X);
            anims[9] = GetAnim(path5, PathAnimationSource.Y);
            return anims;
        }
        static DoubleAnimationUsingPath GetAnim(PathGeometry path, PathAnimationSource source)
        {
            DoubleAnimationUsingPath anim = new DoubleAnimationUsingPath();
            anim.PathGeometry = path;
            anim.Duration = animtime;
            anim.Source = source;
            return anim;
        }
    }
    class ButtonV3 : Canvas
    {
        ScaleTransform Scale;
        Rectangle Back;
        Rectangle Back1;
        byte Curpos;
        public ButtonV3(int width, int height, Brush b1, Brush b2, int left, int top, Canvas canvas, byte curpos)
        {
            Curpos = curpos;
            Back = new Rectangle(); Back.Width = width; Back.Height = height;
            Back1 = new Rectangle(); Back1.Width = width; Back1.Height = height;
            Children.Add(Back); Children.Add(Back1);
            Back1.Opacity = 0;
            Width = width; Height = height;
            RenderTransformOrigin = new Point(0.5, 0.5);
            RenderTransform = Scale = new ScaleTransform();
            Back.Fill = b2;
            Back1.Fill = b1;
            Canvas.SetLeft(this, left); Canvas.SetTop(this, top);
            if (canvas != null) canvas.Children.Add(this);
        }
        static TimeSpan time = TimeSpan.FromSeconds(1.0 / 3);
        static TimeSpan clicktime = TimeSpan.FromSeconds(1.0 / 10);
        public void ChangeImage(ImageBrush b1, ImageBrush b2)
        {
            Back.Fill = b1; Back1.Fill = b2;
        }
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            if (RandomBattleCanvas.IsAnimated) return;
            if (RandomBattleCanvas.CurPos != Curpos && Curpos != 10) return;
            if (Selected) return;
            DoubleAnimation anim = new DoubleAnimation(1, 0, time);
            DoubleAnimation anim1 = new DoubleAnimation(0, 1, time);
            Back.BeginAnimation(Rectangle.OpacityProperty, anim);
            Back1.BeginAnimation(Rectangle.OpacityProperty, anim1);

            //base.OnMouseEnter(e);
        }
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            if (RandomBattleCanvas.IsAnimated) return;
            if (RandomBattleCanvas.CurPos != Curpos && Curpos != 10) return;
            if (Selected) return;
            DoubleAnimation anim = new DoubleAnimation(1, 0, time);
            DoubleAnimation anim1 = new DoubleAnimation(0, 1, time);
            Back.BeginAnimation(Rectangle.OpacityProperty, anim1);
            Back1.BeginAnimation(Rectangle.OpacityProperty, anim);

            //base.OnMouseLeave(e);
        }
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (RandomBattleCanvas.IsAnimated) return;
            if (RandomBattleCanvas.CurPos != Curpos && Curpos != 10) return;
            DoubleAnimation anim = new DoubleAnimation(1, 0.95, clicktime);
            anim.AutoReverse = true;
            Scale.BeginAnimation(ScaleTransform.ScaleXProperty, anim);
            Scale.BeginAnimation(ScaleTransform.ScaleYProperty, anim);
        }
        bool Selected = false;
        public void Select()
        {
            Selected = true;
            DoubleAnimation anim = new DoubleAnimation(0, 1, TimeSpan.Zero);
            DoubleAnimation anim1 = new DoubleAnimation(1, 0, TimeSpan.Zero);
            Back.BeginAnimation(Rectangle.OpacityProperty, anim1);
            Back1.BeginAnimation(Rectangle.OpacityProperty, anim);
        }
        public void DeSelect()
        {
            Selected = false;
            DoubleAnimation anim = new DoubleAnimation(1, 0, TimeSpan.Zero);
            DoubleAnimation anim1 = new DoubleAnimation(0, 1, TimeSpan.Zero);
            Back.BeginAnimation(Rectangle.OpacityProperty, anim1);
            Back1.BeginAnimation(Rectangle.OpacityProperty, anim);
        }
    }
}
