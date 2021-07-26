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
    class Colony_Info : Viewbox
    {
        public SortedList<string, ImageBrush> Images;
        public Canvas MainCanvas;
        Land Land;
        Avanpost Avanpost;
        Rectangle Info_Act;
        BrightText InfoText;
        Color ForwardBlue = Color.FromArgb(255, 66, 255, 255);
        Color BackBlue = Color.FromArgb(255, 044, 58, 255);
        Color ForwardRed = Color.FromArgb(255, 255, 170, 085);
        Color BackRed = Color.FromArgb(255, 255, 051, 044);
        int BlurRad = 30;
        public Colony_Info(GSPlanet planet)
        {
            FillAll();
            Width = 340; Height = 413; Canvas.SetZIndex(this, 255);
            Land = planet.GetLand();
            Avanpost = planet.GetAvanpost();
            MainCanvas = new Canvas(); MainCanvas.Width = 510; MainCanvas.Height = 617; Child = MainCanvas;
            MainCanvas.Children.Add(AddRect(458, 617, "flame", 0, 0, 0));
            MainCanvas.Children.Add(AddRect(227, 579, "ui", 295, 49, 0));
            BrightText Main_Info, Title;
            if (Land != null)
            {
                MainCanvas.Children.Add(AddRect(361, 143, "icon_col", 21, 142, 0));
                Main_Info = new BrightText((Land.Peoples - Land.BuildingsCount).ToString(), ForwardBlue, BackBlue, Colors.Transparent, BlurRad);
                Title = new BrightText(Land.Name.ToString(), ForwardBlue,BackBlue, Colors.Transparent, BlurRad);
            }
            else
            {
                MainCanvas.Children.Add(AddRect(361, 143, "icon_avan", 21, 142, 0));
                Main_Info = new BrightText(String.Format("{0}%",Avanpost.GetBuildPercent().ToString()), ForwardBlue, BackBlue, Colors.Transparent, BlurRad);
                Title = new BrightText(Avanpost.Name.ToString(), ForwardBlue, BackBlue, Colors.Transparent, BlurRad);
            }
            Main_Info.Resize(1.5);
            MainCanvas.Children.Add(Main_Info); Canvas.SetLeft(Main_Info, 180); Canvas.SetTop(Main_Info, 170);
            Title.Resize(0.8);
            Grid Title_Grid = new Grid(); Title_Grid.Width = 363; MainCanvas.Children.Add(Title_Grid);
            Canvas.SetTop(Title_Grid, 60); Canvas.SetLeft(Title_Grid, 20); Title.HorizontalAlignment = HorizontalAlignment.Center; Title_Grid.Children.Add(Title);
            if ((Land != null && Land.Pillage == false && Land.Conquer == false && Land.RiotIndicator == 0)||
                (Avanpost!=null&& Avanpost.Pillage==false && Avanpost.Conquer==false && Avanpost.RiotIndicator==0))
            {
                MainCanvas.Children.Add(AddRect(360, 155, "info", 22, 270, 0));
                BrightText no_treat_text = new BrightText("Угроз нет", ForwardBlue, BackBlue, Colors.Transparent, BlurRad);
                MainCanvas.Children.Add(no_treat_text); Canvas.SetLeft(no_treat_text, 120); Canvas.SetTop(no_treat_text, 315);
            }
            else
            {
                if ((Land!=null && Land.Pillage==true)||(Avanpost!=null && Avanpost.Pillage==true))
                {
                    MainCanvas.Children.Add(AddRect(130, 141, "pillage", 27, 285, 0));
                    BrightText Pillage = new BrightText("Грабёж", ForwardRed, BackRed, Colors.Transparent, BlurRad);
                    Pillage.Resize(0.6);
                    MainCanvas.Children.Add(Pillage); Canvas.SetLeft(Pillage, 45); Canvas.SetTop(Pillage, 265);
                }
                if ((Land!=null&& Land.Conquer==true )|| (Avanpost!=null && Avanpost.Conquer==true))
                {
                    MainCanvas.Children.Add(AddRect(131, 141, "conquare", 139, 285, 0));
                    BrightText Conquare = new BrightText("Захват", ForwardRed, BackRed, Colors.Transparent, BlurRad);
                    Conquare.Resize(0.6);
                    MainCanvas.Children.Add(Conquare); Canvas.SetLeft(Conquare, 170); Canvas.SetTop(Conquare, 265);
                }
                if ((Land != null && Land.RiotIndicator != 0) || (Avanpost != null && Avanpost.RiotIndicator != 0))
                {
                    MainCanvas.Children.Add(AddRect(131, 153, "loyality", 251, 270, 0));
                    BrightText Loyality = new BrightText("Лояльность", ForwardBlue, BackBlue, Colors.Transparent, BlurRad);
                    Loyality.Resize(0.6);
                    MainCanvas.Children.Add(Loyality); Canvas.SetLeft(Loyality, 255); Canvas.SetTop(Loyality, 265);
                    int loyalval = Land == null ? 100 - Avanpost.RiotIndicator : 100 - Land.RiotIndicator;
                    BrightText LoyalVal = new BrightText(loyalval.ToString(), ForwardBlue, BackBlue, Colors.Transparent, BlurRad);
                    LoyalVal.Resize(0.8);
                    MainCanvas.Children.Add(LoyalVal); Canvas.SetLeft(LoyalVal, 290); Canvas.SetTop(LoyalVal, 325);
                }
                
            }
            Canvas Info_Canvas = new Canvas(); Info_Canvas.Width = 363; Info_Canvas.Height = 159;
            Canvas.SetLeft(Info_Canvas, 22); Canvas.SetTop(Info_Canvas, 400);
            MainCanvas.Children.Add(Info_Canvas);
            Info_Canvas.Children.Add(AddRect(360, 155, "info", 0, 0, 0));
            Info_Canvas.Children.Add(Info_Act = AddRect(363, 159, "info_act", 0, 0, 0));
            Info_Act.Opacity = 0;
            if (Land != null)
                InfoText = new BrightText("Общая информация", ForwardBlue, BackBlue, Colors.Transparent, BlurRad);
            else
                InfoText = new BrightText("Строительство аванпоста", ForwardBlue, BackBlue, Colors.Transparent, BlurRad);
            Grid Info_Grid = new Grid(); Info_Grid.Width = 363; Info_Canvas.Children.Add(Info_Grid);
            Canvas.SetTop(Info_Grid, 60);
            Info_Grid.Children.Add(InfoText);
            InfoText.Resize(0.7);
            Info_Grid.HorizontalAlignment = HorizontalAlignment.Center;
            Info_Canvas.MouseEnter += Info_Canvas_MouseEnter;
            Info_Canvas.MouseLeave += Info_Canvas_MouseLeave;
            Info_Canvas.PreviewMouseDown += Info_Canvas_PreviewMouseDown;
        }

        private void Info_Canvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Land != null)
                Links.Controller.PopUpCanvas.Place(new LandInfo(Land));
            else
                Links.Controller.PopUpCanvas.Place(new AvanpostPanel(Avanpost));
        }

        private void Info_Canvas_MouseLeave(object sender, MouseEventArgs e)
        {
            DoubleAnimation anim = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.3));
            Info_Act.BeginAnimation(Rectangle.OpacityProperty, anim);
            InfoText.ChangeColor(Colors.SkyBlue, TimeSpan.FromSeconds(0.3));
        }

        private void Info_Canvas_MouseEnter(object sender, MouseEventArgs e)
        {
            DoubleAnimation anim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.3));
            Info_Act.BeginAnimation(Rectangle.OpacityProperty, anim);
            InfoText.ChangeColor(Colors.Orange, TimeSpan.FromSeconds(0.3));
        }

        Rectangle AddRect(int width, int height, string fill, int left, int top, int z)
        {
            Rectangle rect = Common.GetRectangle(width, height, Images[fill]);
            Canvas.SetLeft(rect, left); Canvas.SetTop(rect, top); Canvas.SetZIndex(rect, z);
            return rect;
        }
        void FillAll()
        {
            Images = new SortedList<string, ImageBrush>();
            Images.Add("flame", Gets.AddPicColonyShort("flame"));
            Images.Add("ui", Gets.AddPicColonyShort("ui"));
            Images.Add("icon_col", Gets.AddPicColonyShort("icon_human_colony"));
            Images.Add("icon_avan", Gets.AddPicColonyShort("icon_avanpost"));
            Images.Add("pillage", Gets.AddPicColonyShort("colony_attack"));
            Images.Add("conquare", Gets.AddPicColonyShort("colony_def"));
            Images.Add("loyality", Gets.AddPicColonyShort("loyality"));
            Images.Add("info", Gets.AddPicColonyShort("info"));
            Images.Add("info_act", Gets.AddPicColonyShort("info_active"));
        }
    }
}
