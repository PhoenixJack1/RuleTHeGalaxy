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
using System.Windows.Media.Effects;
using System.Windows.Media.Animation;
using System.Windows.Threading;


namespace Client
{
    class Diod
    {
        PathFigureCollectionConverter conv = new PathFigureCollectionConverter();
        static Random rnd = new Random();
        static Canvas canvas;
        Path path;
        DoubleAnimation anim;
        int dimension;
        //static DispatcherTimer timer=new DispatcherTimer();
        public static void Start(Canvas canvas)
        {
            //timer.Stop();
            Diod.canvas = canvas;
            //timer.Interval = TimeSpan.FromSeconds(5);
            //timer.Tick += new EventHandler(timer_Tick);
            //timer.Start();
            //timer_Tick(null, null);
        }
        public static void Stop()
        {
            //timer.Stop();
        }
        static void timer_Tick(object sender, EventArgs e)
        {
            new Diod();
            new Diod();
            new Diod();
            new Diod();
            new Diod();
            new Diod();
            new Diod();
            new Diod();
            new Diod();
        }
        public Diod()
        {
            path = new Path();
            path.Stroke = Brushes.White;
            path.StrokeThickness = 3;
            path.StrokeLineJoin = PenLineJoin.Round;
            int type = rnd.Next(9);
            string data;
            switch (type)
            {
                case 0: data = "M0,0 a20,20 0 1,1 0,0.1 h25 l-10,-10 v20 l10,-10 v10 v-20 v10 h15"; break;
                case 1: data = "M0,0 h10 l5,10 l10,-20 l10,20 l10,-20 l10,20 l10,-20 l10,20 l10,-20 l5,10 h10"; break;
                case 2: data = "M0,0 h20 v-10 v20 M25,-11 v20 v-9 h20"; break;
                case 3: data = "M0,0 v10 h20 a10,10 0 0,0 20,0 a10,10 0 0,0 20,0 a10,10 0 0,0 20,0 h20 v-10 M0,30 h100"; break;
                case 4: data = "M0,0 h20 v20 M 15,20 h30 M40,20 v-20 h20 M40,45 v-23 l-4,10 l4,-10 l4,10"; break;
                case 5: data = "M0,0 v20 h60 M10,30 v10 M20,30 v10 M30,30 v10 M40,30 v10 M50,30 v10 M10,15 v10"; break;
                case 6: data = "M0,0 h20 a20,20 0 1,1 0,0.1 M60,0 h20 M27,-13 l27,27 M27,13 l27,-27"; break;
                case 7: data = "M5,5 h30 v30 h-30z M5,7 h-5 M5,12 h-5 M5,17 h-5 M5,22 h-5 M5,27 h-5 M5,32 h-5" +
              "M35,7 h5 M35,12 h5 M35,17 h5 M35,22 h5 M35,27 h5 M35,32 h5" +
              "M7,5 v-5 M12,5 v-5 M17,5 v-5 M22,5 v-5 M27,5 v-5 M32,5 v-5" +
              "M7,35 v5 M12,35 v5 M17,35 v5 M22,35 v5 M27,35 v5 M32,35 v5"; break;
                default: data = "M0,0 h20 v7 h30 v-14 h-30 v7 M50,0 h20"; break;
            }
            path.Data = new PathGeometry((PathFigureCollection)conv.ConvertFrom(data));
            canvas.Children.Add(path);
            int top = rnd.Next(50, 600);
            int left = rnd.Next(50, 1000);
            dimension = rnd.Next(4);
            Canvas.SetLeft(path, left);
            Canvas.SetTop(path, top);
            int time = rnd.Next(5, 10);
            switch (dimension)
            {
                case 0: anim = new DoubleAnimation(top, -100, TimeSpan.FromSeconds(time)); break;
                case 1: anim = new DoubleAnimation(top, 820, TimeSpan.FromSeconds(time)); break;
                case 2: anim = new DoubleAnimation(left, -100, TimeSpan.FromSeconds(time)); break;
                default: anim = new DoubleAnimation(left, 1380, TimeSpan.FromSeconds(time)); break;
            }
            anim.Completed += new EventHandler(anim_Completed);
            DoubleAnimation anim1 = new DoubleAnimation(0, 0.4, TimeSpan.FromSeconds(2));
            ElasticEase el = new ElasticEase();
            el.Oscillations = 3;

            anim1.EasingFunction = el;
            anim1.Completed += new EventHandler(anim1_Completed);
            path.BeginAnimation(Path.OpacityProperty, anim1);
        }

        void anim1_Completed(object sender, EventArgs e)
        {
            if (dimension == 0 || dimension == 1)
                path.BeginAnimation(Canvas.TopProperty, anim);
            else
                path.BeginAnimation(Canvas.LeftProperty, anim);
        }

        void anim_Completed(object sender, EventArgs e)
        {
            canvas.Children.Remove(path);
        }
    }
}
