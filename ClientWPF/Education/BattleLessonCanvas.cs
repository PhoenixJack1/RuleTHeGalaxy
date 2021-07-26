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
using System.Windows.Threading;

namespace Client
{
    class BattleLesson1Canvas : Canvas
    {
        enum ECurGroup { Basic, MainParts1, Controls2, Weapons3, Groups4, ShortInfo5, FullInfo6 }
        Path Selection;
        Canvas Back;
        TextBlock Block;
        int CurSubLesson = 1;
        ECurGroup CurGroup = ECurGroup.Basic;
        GameButton btn1, btn2, btn3;
        Rectangle Ground;
        Canvas ShowWeapon;
        public BattleLesson1Canvas()
        {
            Width = 1280; Height = 720;
            Ground = new Rectangle();
            Ground.Width = 1280; Ground.Height = 720;
            Children.Add(Ground);
            BitmapImage back = new BitmapImage(new Uri("pack://application:,,,/EducationLibrary;component/Education/Battle/Image1.jpg"));
            Ground.Fill = new ImageBrush(back);
            Background = Brushes.Black;
            Selection = new Path(); Selection.Stroke = Brushes.Red; Selection.StrokeThickness = 10;
            //Selection.Fill = new SolidColorBrush(Color.FromArgb(100, 255, 0, 0));
            Children.Add(Selection); Canvas.SetZIndex(Selection, 20);
            Back = new Canvas(); Back.Width = 400; Back.Height = 300;
            Back.Background = Links.Brushes.Interface.RamkaMedium;
            Children.Add(Back); Canvas.SetZIndex(Back, 20);
            Block = Common.GetBlock(20, "", Brushes.White, 300);
            Back.Children.Add(Block); Canvas.SetZIndex(Block, 20);
            btn1 = new GameButton(120, 70, Links.Interface("Next"), 20);
            Back.Children.Add(btn1); Canvas.SetZIndex(btn1, 20);
            btn1.PreviewMouseDown += Btn1_PreviewMouseDown;

            btn2 = new GameButton(120, 70, Links.Interface("Back"), 20);
            Back.Children.Add(btn2); Canvas.SetZIndex(btn2, 20);
            btn2.PreviewMouseDown += Btn2_PreviewMouseDown;


            btn3 = new GameButton(120, 70, Links.Interface("ShowMore"), 20);
            Back.Children.Add(btn3); Canvas.SetZIndex(btn3, 20);
            btn3.Visibility = Visibility.Hidden;
            btn3.PreviewMouseDown += Btn3_PreviewMouseDown;

            Canvas.SetLeft(Block, 50);
            Canvas.SetLeft(btn2, 50);
            Canvas.SetLeft(btn3, 190);
            LastPosition = new Position(450, 0, 500, 300);
            ShootAnim.IsRealBattle = false;
            SetLesson();
            //MoveTo(450, 150, 400, 300);
        }

        private void Btn3_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            new MySound("Interface/Next.wav");
        }

        private void Btn2_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (changes) return;
            new MySound("Interface/Prev.wav");
            if (CurSubLesson <= 1) return;
            CurSubLesson--;
            if (CurGroup == ECurGroup.MainParts1 && CurSubLesson == 100) { CurGroup = ECurGroup.Basic; CurSubLesson = 1; }
            if (CurGroup == ECurGroup.Controls2 && CurSubLesson == 200) { CurGroup = ECurGroup.Basic; CurSubLesson = 2; }
            if (CurGroup == ECurGroup.Weapons3 && CurSubLesson == 300) { CurGroup = ECurGroup.Basic; CurSubLesson = 3; }
            if (CurGroup == ECurGroup.Groups4 && CurSubLesson == 400) { CurGroup = ECurGroup.Basic; CurSubLesson = 4; }
            if (CurGroup == ECurGroup.ShortInfo5 && CurSubLesson == 500) { CurGroup = ECurGroup.Basic; CurSubLesson = 5; }
            if (CurGroup == ECurGroup.FullInfo6 && CurSubLesson == 600) { CurGroup = ECurGroup.Basic; CurSubLesson = 6; }
            SetLesson();
            btn1.SetText(Links.Interface("Next"));
        }

        private void Btn1_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (changes) return;
            new MySound("Interface/Next.wav");
            if (CurSubLesson == 6 || CurSubLesson == 606)
            {
                ShootAnim.IsRealBattle = true;
                Links.Controller.BattlePopUp.Remove();
                return;
            }
            CurSubLesson++;
            if (CurGroup == ECurGroup.MainParts1 && CurSubLesson == 110) { CurGroup = ECurGroup.Basic; CurSubLesson = 2; }
            if (CurGroup == ECurGroup.Controls2 && CurSubLesson == 213) { CurGroup = ECurGroup.Basic; CurSubLesson = 3; }
            if (CurGroup == ECurGroup.Weapons3 && CurSubLesson == 318) { CurGroup = ECurGroup.Basic; CurSubLesson = 4; }
            if (CurGroup == ECurGroup.Groups4 && CurSubLesson == 402) { CurGroup = ECurGroup.Basic; CurSubLesson = 5; }
            if (CurGroup == ECurGroup.ShortInfo5 && CurSubLesson == 505) { CurGroup = ECurGroup.Basic; CurSubLesson = 6; }
            if (CurGroup == ECurGroup.FullInfo6 && CurSubLesson == 607) { CurGroup = ECurGroup.Basic; CurSubLesson = 6; }
            SetLesson();
            if (CurSubLesson == 6 || CurSubLesson == 606)
                btn1.SetText(Links.Interface("Close"));
            else
                btn1.SetText(Links.Interface("Next"));
        }
        void SetLesson()
        {
            switch (CurSubLesson)
            {
                case 1:
                    ChangeBack("Image1.jpg");
                    Selection.Data = null;
                    ChangeBlockText(Links.Interface("BattleLesson1.0"), "");
                    btn3.Visibility = Visibility.Visible;
                    btn3.ClearSubscribers();
                    btn3.AddEvent(ShowLesson1More);
                    MoveTo(450, 150, 500, 300);
                    break;
                case 101:
                    ChangeBlockText(Links.Interface("BattleLesson1.1"), "M240,70 h67 l33,53 h66 l33,-54 h66 l33,53 h67 l33,-53 h67 l33,53 h67" +
                "l33,-53 h66 l33,54 h66 l33,54 l-33,54 l33,54 l-33,54 l33,54 l-33,54 l33,54 l-33,54 l33,54 l-33,54" +
                "h-67 l-33,-54 h-67 l-33,57 h-67 l-33,-54 h-67 l-33,53 h-66 l-33,-54 h-66 l-33,53 h-66 l-33,-54 h-66 l-33,-57" +
                "l33,-54 l-33,-54 l33,-54 l-33,-54 l33,-54 l-33,-54 l33,-54 l-33,-54 z");
                    btn3.Visibility = Visibility.Hidden;
                    MoveTo(450, 150, 400, 300);
                    break;
                case 102:
                    ChangeBlockText(Links.Interface("BattleLesson1.2"), "M100,70 h67 l33,54 l-33,54 l33,54 l-33,54 l33,54 l-33,54 l33,54 l-33,54 l33,54 l-33,54" +
               " h-65 l-33,-54 l33,-54 l-33,-54 l33,-54 l-33,-54 l33,-54 l-33,-54 l33,-54 l-33,-54 z" +
               "M1070,123 h67 l33,54 l-33,54 l33,54 l-33,54 l33,54 l-33,54 l33,54 l-33,54 l33,54 l-33,54" +
               "h-65 l-33,-54 l33,-54 l-33,-54 l33,-54 l-33,-54 l33,-54 l-33,-54 l33,-54 l-33,-54 z");
                   break;
                case 103:
                   ChangeBlockText(Links.Interface("BattleLesson1.3"), "M240,70 h67 l33,54 l-33,54 h-66 l-33,-54 z M935,553 h67 l33,54 l-33,54 h-66 l-33,-54 z");
                    break;
                case 104:
                    ChangeBlockText(Links.Interface("BattleLesson1.4"), "M100,70 h210 l33,54 l-33,54 h-100 v430 h-140 v-538 z");
                    break;
                case 105:
                    ChangeBlockText(Links.Interface("BattleLesson1.5"), "M590,50 h100 v75 h-100z");
                    break;
                case 106:
                    ChangeBlockText(Links.Interface("BattleLesson1.6"), "M1150,590 h130 v130 h-130z");
                    break;
                case 107:
                    ChangeBlockText(Links.Interface("BattleLesson1.7"), "M1070,650 h85 v75 h-85z");
                    break;
                case 108:
                    ChangeBlockText(Links.Interface("BattleLesson1.8"), "M1210,85 h85 v75 h-85z");
                    break;
                case 109:
                    ChangeBack("Image1.jpg");
                     ChangeBlockText(Links.Interface("BattleLesson1.9"), "M100,655 h330 v75 h-330z");
                    break;
                case 2:
                    ChangeBack("Image1.jpg");
                    ChangeBlockText(Links.Interface("BattleLesson2.0"), "");
                    MoveTo(450, 150, 500, 300);
                    btn3.Visibility = Visibility.Visible;
                    btn3.ClearSubscribers();
                    btn3.AddEvent(ShowLesson2More);
                    break;
                case 201:
                    ChangeBack("Image2.jpg");
                    ChangeBlockText(Links.Interface("BattleLesson2.1"), "");
                    btn3.Visibility = Visibility.Hidden;
                    MoveTo(450, 150, 400, 300);
                    break;
                case 202:
                    MoveTo(450, 150, 400, 400);
                    ChangeBack("Image3.jpg");
                    ChangeBlockText(Links.Interface("BattleLesson2.2"), "M60,165 h150 v240 h-150z");
                    break;
                case 203:
                    MoveTo(450, 150, 400, 300);
                    ChangeBlockText(Links.Interface("BattleLesson2.3"), "M60,55 h150 v135 h-150z");
                    break;
                case 204:
                    ChangeBack("Image3.jpg");
                    ChangeBlockText(Links.Interface("BattleLesson2.4"), "M340,125 h67 l33,54 l-33,54 h-67 l-33,54 h-67 l-33,-54 l33,-54 h67z");
                    MoveTo(450, 150, 400, 400);
                    break;
                case 205:
                    MoveTo(650, 150, 400, 300);
                    ChangeBack("Image4.jpg");
                    ChangeBlockText(Links.Interface("BattleLesson2.5"), "M105,65 h67 l275,110 h67 l33,54 l-33,54 h-67 l-275,-110 h-67 l-33,-54z M1160,585 h120 v130 h-120z");
                    break;
                case 206:
                    MoveTo(550, 400, 500, 300);
                    ChangeBack("Image5.jpg");
                    ChangeBlockText(Links.Interface("BattleLesson2.6"), "M340,120 h67  l33,-54 h67 l33,54 l-33,54  l33,54 h67 l33,54 l-33,54" +
                        "h-67 l-33,54 l33,54 l-33,54 h-67 l-33,-54 h-67 l-33,54 h-67 l-33,-54 l33,-54 l-33,-54 l33,-54 l-33,-54 l33,-54 h67z" +
                        "M240,70 h67 l33,54 l-33,54 h-67 l-33,-54z M800,280 h150 v120 h-150z");
                    break;
                case 207:
                    MoveTo(550, 150, 400, 400);
                    ChangeBack("Image5.jpg");
                    ChangeBlockText(Links.Interface("BattleLesson2.7"), "M300,225 h150 v120 h-150z");
                    break;
                case 208:
                    MoveTo(650, 150, 500, 400);
                    ChangeBack("Image6.jpg");
                    ChangeBlockText(Links.Interface("BattleLesson2.8"), "M10,10 h200 v180 h-200z M380,150 h200 v180 h-200z");
                    break;
                case 209:
                    MoveTo(250, 350, 400, 350);
                    ChangeBack("Image7.jpg");
                    ChangeBlockText(Links.Interface("BattleLesson2.9"), "M1030,100 h160 v140 h-160z M695,210 h160 v140 h-160z M1030,320 h160 v140 h-160z");
                    break;
                case 210:
                    MoveTo(350, 200, 500, 300);
                    ChangeBack("Image8.jpg");
                    ChangeBlockText(Links.Interface("BattleLesson2.10"), "M290,50 h460 v100 h-460z M790,50 h120 v100 h-120z");
                    break;
                case 211:
                    MoveTo(550, 150, 500, 300);
                    ChangeBack("Image9.jpg");
                    ChangeBlockText(Links.Interface("BattleLesson2.11"), "M180,180 a50,80 30 1,1 -0.1,0");
                    break;
                case 212:
                    MoveTo(550, 150, 500, 300);
                    ChangeBack("Image9.jpg");
                    ChangeBlockText(Links.Interface("BattleLesson2.12"), "M280,150 a50,80 -80 1,1 -0.1,0 M220,310 a50,80 -80 1,1 -0.1,0");
                    btn3.Visibility = Visibility.Hidden;
                    break;
                case 3:
                    MoveTo(550, 150, 500, 300);
                    ChangeBack("Image9.jpg");
                    ChangeBlockText(Links.Interface("BattleLesson3.0"), "M300,190 a50,80 -80 1,1 -0.1,0 M260,290 a50,80 -80 1,1 -0.1,0");
                    btn3.Visibility = Visibility.Visible;
                    btn3.ClearSubscribers();
                    btn3.AddEvent(ShowLesson3More);
                    if (ShowWeapon != null) Children.Remove(ShowWeapon); 
                    break;
                case 301:
                    MoveTo(440, 160, 400, 400);
                    ChangeBack("");
                    ChangeBlockText(Links.Interface("BattleLesson3.1"), "");
                    btn3.Visibility = Visibility.Hidden;
                    if (ShowWeapon != null)
                    {
                        if (ShowWeapon.GetType() == typeof(ShowWeaponCanvas))
                        {
                            ((ShowWeaponCanvas)ShowWeapon).Close();
                            Children.Remove(((ShowWeaponCanvas)ShowWeapon).VBX);
                        }
                        else
                            Children.Remove(ShowWeapon);
                        
                    }
                    ShowWeapon = ShowWeaponCanvas.GetBasicWeaponInfoCanvas();
                    Children.Add(ShowWeapon);
                    break;
                case 302:
                    MoveTo(750, 150, 400, 400);
                    ChangeBlockText(Links.Interface("BattleLesson3.2"), "");
                    if (ShowWeapon.GetType() == typeof(ShowWeaponCanvas))
                    {
                        ((ShowWeaponCanvas)ShowWeapon).Close();
                        Children.Remove(((ShowWeaponCanvas)ShowWeapon).VBX);
                    }
                    else
                        Children.Remove(ShowWeapon);
                    ShowWeapon = new ShowWeaponCanvas(EWeaponType.Laser);
                    Children.Add(((ShowWeaponCanvas)ShowWeapon).VBX);
                    break;
                case 303:
                    ChangeBlockText(Links.Interface("BattleLesson3.3"), "");
                    ((ShowWeaponCanvas)ShowWeapon).Close();
                    Children.Remove(((ShowWeaponCanvas)ShowWeapon).VBX);
                    ShowWeapon = new ShowWeaponCanvas(EWeaponType.EMI);
                    Children.Add(((ShowWeaponCanvas)ShowWeapon).VBX);
                    break;
                case 304:
                    ChangeBlockText(Links.Interface("BattleLesson3.4"), "");
                    ((ShowWeaponCanvas)ShowWeapon).Close();
                    Children.Remove(((ShowWeaponCanvas)ShowWeapon).VBX);
                    ShowWeapon = new ShowWeaponCanvas(EWeaponType.Plasma);
                    Children.Add(((ShowWeaponCanvas)ShowWeapon).VBX);
                    break;
                case 305:
                    ChangeBlockText(Links.Interface("BattleLesson3.5"), "");
                    ((ShowWeaponCanvas)ShowWeapon).Close();
                    Children.Remove(((ShowWeaponCanvas)ShowWeapon).VBX);
                    ShowWeapon = new ShowWeaponCanvas(EWeaponType.Solar);
                    Children.Add(((ShowWeaponCanvas)ShowWeapon).VBX);
                    break;
                case 306:
                    ChangeBlockText(Links.Interface("BattleLesson3.6"), "");
                    ((ShowWeaponCanvas)ShowWeapon).Close();
                    Children.Remove(((ShowWeaponCanvas)ShowWeapon).VBX);
                    ShowWeapon = new ShowWeaponCanvas(EWeaponType.Cannon);
                    Children.Add(((ShowWeaponCanvas)ShowWeapon).VBX);
                    break;
                case 307:
                    ChangeBlockText(Links.Interface("BattleLesson3.7"), "");
                    ((ShowWeaponCanvas)ShowWeapon).Close();
                    Children.Remove(((ShowWeaponCanvas)ShowWeapon).VBX);
                    ShowWeapon = new ShowWeaponCanvas(EWeaponType.Gauss);
                    Children.Add(((ShowWeaponCanvas)ShowWeapon).VBX);
                    break;
                case 308:
                    ChangeBlockText(Links.Interface("BattleLesson3.8"), "");
                    ((ShowWeaponCanvas)ShowWeapon).Close();
                    Children.Remove(((ShowWeaponCanvas)ShowWeapon).VBX);
                    ShowWeapon = new ShowWeaponCanvas(EWeaponType.Missle);
                    Children.Add(((ShowWeaponCanvas)ShowWeapon).VBX);
                    break;
                case 309:
                    ChangeBlockText(Links.Interface("BattleLesson3.9"), "");
                    ((ShowWeaponCanvas)ShowWeapon).Close();
                    Children.Remove(((ShowWeaponCanvas)ShowWeapon).VBX);
                    ShowWeapon = new ShowWeaponCanvas(EWeaponType.AntiMatter);
                    Children.Add(((ShowWeaponCanvas)ShowWeapon).VBX);
                    break;
                case 310:
                    ChangeBlockText(Links.Interface("BattleLesson3.10"), "");
                    ((ShowWeaponCanvas)ShowWeapon).Close();
                    Children.Remove(((ShowWeaponCanvas)ShowWeapon).VBX);
                    ShowWeapon = new ShowWeaponCanvas(EWeaponType.Psi);
                    Children.Add(((ShowWeaponCanvas)ShowWeapon).VBX);
                    break;
                case 311:
                    ChangeBlockText(Links.Interface("BattleLesson3.11"), "");
                    ((ShowWeaponCanvas)ShowWeapon).Close();
                    Children.Remove(((ShowWeaponCanvas)ShowWeapon).VBX);
                    ShowWeapon = new ShowWeaponCanvas(EWeaponType.Dark);
                    Children.Add(((ShowWeaponCanvas)ShowWeapon).VBX);
                    break;
                case 312:
                    ChangeBlockText(Links.Interface("BattleLesson3.12"), "");
                    ((ShowWeaponCanvas)ShowWeapon).Close();
                    Children.Remove(((ShowWeaponCanvas)ShowWeapon).VBX);
                    ShowWeapon = new ShowWeaponCanvas(EWeaponType.Warp);
                    Children.Add(((ShowWeaponCanvas)ShowWeapon).VBX);
                    break;
                case 313:
                    ChangeBlockText(Links.Interface("BattleLesson3.13"), "");
                    ((ShowWeaponCanvas)ShowWeapon).Close();
                    Children.Remove(((ShowWeaponCanvas)ShowWeapon).VBX);
                    ShowWeapon = new ShowWeaponCanvas(EWeaponType.Time);
                    Children.Add(((ShowWeaponCanvas)ShowWeapon).VBX);
                    break;
                case 314:
                    ChangeBlockText(Links.Interface("BattleLesson3.14"), "");
                    ((ShowWeaponCanvas)ShowWeapon).Close();
                    Children.Remove(((ShowWeaponCanvas)ShowWeapon).VBX);
                    ShowWeapon = new ShowWeaponCanvas(EWeaponType.Slicing);
                    Children.Add(((ShowWeaponCanvas)ShowWeapon).VBX);
                    break;
                case 315:
                    ChangeBlockText(Links.Interface("BattleLesson3.15"), "");
                    ((ShowWeaponCanvas)ShowWeapon).Close();
                    Children.Remove(((ShowWeaponCanvas)ShowWeapon).VBX);
                    ShowWeapon = new ShowWeaponCanvas(EWeaponType.Radiation);
                    Children.Add(((ShowWeaponCanvas)ShowWeapon).VBX);
                    break;
                case 316:
                    ChangeBlockText(Links.Interface("BattleLesson3.16"), "");
                    ((ShowWeaponCanvas)ShowWeapon).Close();
                    Children.Remove(((ShowWeaponCanvas)ShowWeapon).VBX);
                    ShowWeapon = new ShowWeaponCanvas(EWeaponType.Drone);
                    Children.Add(((ShowWeaponCanvas)ShowWeapon).VBX);
                    break;
                case 317:
                    ChangeBlockText(Links.Interface("BattleLesson3.17"), "");
                    ((ShowWeaponCanvas)ShowWeapon).Close();
                    Children.Remove(((ShowWeaponCanvas)ShowWeapon).VBX);
                    ShowWeapon = new ShowWeaponCanvas(EWeaponType.Magnet);
                    Children.Add(((ShowWeaponCanvas)ShowWeapon).VBX);
                    break;
                case 4:
                    MoveTo(550, 150, 500, 300);
                    ChangeBack("Image10.jpg");
                    ChangeBlockText(Links.Interface("BattleLesson4.0"), "M230,90 a150,150 0 1,1 -0.1,0");
                    btn3.Visibility = Visibility.Visible;
                    btn3.ClearSubscribers();
                    btn3.AddEvent(ShowLesson4More);
                    if (ShowWeapon != null)
                    {
                        if (ShowWeapon.GetType() == typeof(ShowWeaponCanvas))
                        {
                            ((ShowWeaponCanvas)ShowWeapon).Close();
                            Children.Remove(((ShowWeaponCanvas)ShowWeapon).VBX);
                        }
                    }
                    break;
                case 401:
                    MoveTo(550, 150, 400, 300);
                    ChangeBack("Image13.jpg");
                    ChangeBlockText(Links.Interface("BattleLesson4.1"), "");
                    btn3.Visibility = Visibility.Hidden;
                    break;
                case 5:
                    MoveTo(550, 150, 500, 300);
                    ChangeBack("Image11.jpg");
                    ChangeBlockText(Links.Interface("BattleLesson5.0"), "");
                    btn3.Visibility = Visibility.Visible;
                    btn3.ClearSubscribers();
                    btn3.AddEvent(ShowLesson5More);
                    break;
                case 501:
                    MoveTo(550, 150, 400, 300);
                    ChangeBlockText(Links.Interface("BattleLesson5.1"), "M230,220 h260 l40,100 h-260z");
                    btn3.Visibility = Visibility.Hidden;
                    break;
                case 502:
                    ChangeBlockText(Links.Interface("BattleLesson5.2"), "M70,130 h180 v150 h-180z");
                    break;
                case 503:
                    ChangeBlockText(Links.Interface("BattleLesson5.3"), "M10,220 h70 v110 h-70z M90,260 h140 v90 h-140z");
                    break;
                case 504:
                    ChangeBlockText(Links.Interface("BattleLesson5.4"), "M110,340 h100 v80 h-100z");
                    break;
                case 6:
                    MoveTo(750, 300, 500, 300);
                    if (Links.Lang == 0)
                        ChangeBack("Image12En.jpg");
                    else
                        ChangeBack("Image12Ru.jpg");
                    ChangeBlockText(Links.Interface("BattleLesson6.0"), "");
                    btn3.Visibility = Visibility.Visible;
                    btn3.ClearSubscribers();
                    btn3.AddEvent(ShowLesson6More);
                    break;
                case 601:
                    MoveTo(750, 300, 400, 300);
                    ChangeBlockText(Links.Interface("BattleLesson6.1"), "M50,160 h180 v260 h-180z");
                    btn3.Visibility = Visibility.Hidden;
                    break;
                case 602:
                    ChangeBlockText(Links.Interface("BattleLesson6.2"), "M480,180 h100 v180 h-100z");
                    break;
                case 603:
                    ChangeBlockText(Links.Interface("BattleLesson6.3"), "M590,180 h100 v180 h-100z");
                    break;
                case 604:
                    ChangeBlockText(Links.Interface("BattleLesson6.4"), "M490,360 h210 v70 h-210z");
                    break;
                case 605:
                    ChangeBlockText(Links.Interface("BattleLesson6.5"), "M120,440 h170 v65 h-170z M450,440 h170 v65 h-170z");
                    break;
                case 606:
                    ChangeBlockText(Links.Interface("BattleLesson6.6"), "M720,170 h100 v80 h-100z");
                    break;
                default:
                    Selection.Data = null;
                    Block.Text = null;
                    break;
            }

        }
        string changeimage;
        Brush changebrush;
        void ChangeBack(string newimage)
        {
            if (newimage == changeimage) return;
            changeimage = newimage;
            if (newimage != "")
                changebrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/EducationLibrary;component/Education/Battle/" + changeimage)));
            else
                changebrush = Brushes.Black;
            DoubleAnimation anim = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.5));
            anim.Completed += GroundHideCompleted;
            Ground.BeginAnimation(TextBlock.OpacityProperty, anim);
        }

        private void GroundHideCompleted(object sender, EventArgs e)
        {
            Ground.Fill = changebrush;
            DoubleAnimation anim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5));
            Ground.BeginAnimation(TextBlock.OpacityProperty, anim);
        }
        string changetext, changeselection;
        void ChangeBlockText(string newtext, string newselection)
        {
            changetext = newtext;
            changeselection = newselection;
            DoubleAnimation anim = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.5));
            anim.Completed += TextHideCompleted;
            Block.BeginAnimation(TextBlock.OpacityProperty, anim);
            Selection.BeginAnimation(Path.OpacityProperty, anim);
        }

        private void TextHideCompleted(object sender, EventArgs e)
        {
            Block.Text = changetext;
            Selection.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(changeselection));
            Block.Width = LastPosition.blockwidth;
            DoubleAnimation anim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5));
            Block.BeginAnimation(TextBlock.OpacityProperty, anim);
            Selection.BeginAnimation(Path.OpacityProperty, anim);
        }

        private void ShowLesson1More(object sender, MouseButtonEventArgs e)
        {
            if (changes) return;
            CurGroup = ECurGroup.MainParts1;
            CurSubLesson = 101;
            SetLesson();
        }
        private void ShowLesson2More(object sender, MouseButtonEventArgs e)
        {
            if (changes) return;
            CurGroup = ECurGroup.Controls2;
            CurSubLesson = 201;
            SetLesson();
        }
        private void ShowLesson3More(object sender, MouseButtonEventArgs e)
        {
            if (changes) return;
            CurGroup = ECurGroup.Weapons3;
            CurSubLesson = 301;
            SetLesson();
        }
        private void ShowLesson4More(object sender, MouseButtonEventArgs e)
        {
            if (changes) return;
            MessageBox.Show("Under construction");
            /*CurGroup = ECurGroup.Groups4;
            CurSubLesson = 401;
            SetLesson();*/
        }
        private void ShowLesson5More(object sender, MouseButtonEventArgs e)
        {
            if (changes) return;
            CurGroup = ECurGroup.ShortInfo5;
            CurSubLesson = 501;
            SetLesson();
        }
        private void ShowLesson6More(object sender, MouseButtonEventArgs e)
        {
            if (changes) return;
            CurGroup = ECurGroup.FullInfo6;
            CurSubLesson = 601;
            SetLesson();
            btn1.SetText(Links.Interface("Next"));
        }
        Position LastPosition;
        bool changes = false;
        TimeSpan time = TimeSpan.FromSeconds(1);
        void MoveTo(int left, int top, int width, int height)
        {
            Position NewPos = new Position(left, top, width, height);
            if (LastPosition != null && LastPosition.parameter == NewPos.parameter) return;
            changes = true;
            DoubleAnimation anim1 = new DoubleAnimation(LastPosition.backleft, NewPos.backleft, time);
            anim1.Completed += Anim1_Completed;
            Back.BeginAnimation(Canvas.LeftProperty, anim1);
            DoubleAnimation anim2 = new DoubleAnimation(LastPosition.backtop, NewPos.backtop, time);
            Back.BeginAnimation(Canvas.TopProperty, anim2);
            DoubleAnimation anim3 = new DoubleAnimation(LastPosition.backwidth, NewPos.backwidth, time);
            Back.BeginAnimation(Canvas.WidthProperty, anim3);
            DoubleAnimation anim4 = new DoubleAnimation(LastPosition.backheight, NewPos.backheight, time);
            Back.BeginAnimation(Canvas.HeightProperty, anim4);
            Canvas.SetTop(Block, 30 + (height - 300) * 0.1);
            DoubleAnimation anim5 = new DoubleAnimation(LastPosition.btn1left, NewPos.btn1left, time);
            btn1.BeginAnimation(Canvas.LeftProperty, anim5);
            DoubleAnimation anim6 = new DoubleAnimation(LastPosition.btntop, NewPos.btntop, time);
            btn1.BeginAnimation(Canvas.TopProperty, anim6);
            btn2.BeginAnimation(Canvas.TopProperty, anim6);
            btn3.BeginAnimation(Canvas.TopProperty, anim6);
            LastPosition = NewPos;
        }

        private void Anim1_Completed(object sender, EventArgs e)
        {
            changes = false;
        }

        class Position
        {
            public long parameter;
            public int backleft, backtop, backwidth, backheight;
            public int blockwidth, blocktop;
            public int btn1left, btntop;
            public Position(int left, int top, int width, int height)
            {
                parameter = left + top * 1000 + width * 1000000 + height * 1000000000;
                backleft = left; backtop = top; backwidth = width; backheight = height;
                blockwidth = width - 100; blocktop = (int)(30 + (height - 300) * 0.1);
                btn1left = width - 170; btntop = (int)(height - 100 - (height - 300) * 0.1);
            }

        }
    }
    class ShowWeaponCanvas:Canvas
    {
        Schema schema;
        public Viewbox VBX;
        public ShipB shipb1;
        public ShipB shipb2;
        public EWeaponType WeaponType;
        DispatcherTimer Timer;
        int weapon = 0;
        public ShowWeaponCanvas(EWeaponType type)
        {
            if (FireTimer != null) FireTimer.Stop();
            WeaponType = type;
            Width = 1200;
            Height = 1200;
            Background = Brushes.Black;
            Hex hex1 = new Hex(0, 0, 0);
            hex1.CreatePath();
            Children.Add(hex1.canvas);
            Canvas.SetLeft(hex1.canvas, 50);
            Hex hex2 = new Hex(0, 0, 0);
            hex2.CreatePath();
            Children.Add(hex2.canvas);
            Canvas.SetLeft(hex2.canvas, 50); Canvas.SetTop(hex2.canvas, 940);
            schema = new Schema();
            schema.SetShipType(48235);
            schema.SetShield(49261);
            schema.SetGenerator(50257);
            schema.SetEngine(50277);
            schema.SetComputer(50280);
            VBX = new Viewbox(); VBX.Width = 720; VBX.Height = 720;
            VBX.Child = this;
            SetWeapon(type);
            shipb1 = new ShipB(100, schema, 100, GSPilot.GetBasicPilot(), new byte[] { 253, 1, 1, 1 }, ShipSide.Attack, null, false, null, ShipBMode.Battle, 255);
            Children.Add(shipb1.HexShip);
            Canvas.SetLeft(shipb1.HexShip, 50);
            Canvas.SetTop(shipb1.HexShip, 0);
            shipb1.SetAngle(180, true);
            shipb1.HexShip.Hex = hex1;

            shipb2 = new ShipB(101, schema, 100, GSPilot.GetBasicPilot(), new byte[] { 253, 1, 1, 1 }, ShipSide.Defense, null, false,null, ShipBMode.Battle, 255);
            Children.Add(shipb2.HexShip);
            Canvas.SetLeft(shipb2.HexShip, 50);
            Canvas.SetTop(shipb2.HexShip, 940);
            shipb2.HexShip.Hex = hex2;

            switch (type)
            {
                case EWeaponType.Laser: AddWeaponParams(Links.Interface("Laser"), type, Links.Interface("EnergyDamage"), Brushes.Blue, 6, 4, 7, 5, 7); break;
                case EWeaponType.EMI:AddWeaponParams(Links.Interface("EMI"), type, Links.Interface("EnergyDamage"), Brushes.Blue, 9, 1, 7, 6, 9); break;
                case EWeaponType.Plasma: AddWeaponParams(Links.Interface("Plasma"), type, Links.Interface("EnergyDamage"), Brushes.Blue, 9, 9, 3, 1, 5); break;
                case EWeaponType.Solar: AddWeaponParams(Links.Interface("Solar"), type, Links.Interface("EnergyDamage"), Brushes.Blue, 6, 6, 5, 2, 1); break;
                case EWeaponType.Cannon: AddWeaponParams(Links.Interface("Cannon"), type, Links.Interface("PhysicDamage"), Brushes.Red, 2, 6, 5, 9, 5); break;
                case EWeaponType.Gauss: AddWeaponParams(Links.Interface("Gauss"), type, Links.Interface("PhysicDamage"), Brushes.Red, 5, 5, 7, 4, 5); break;
                case EWeaponType.Missle: AddWeaponParams(Links.Interface("Missle"), type, Links.Interface("PhysicDamage"), Brushes.Red, 1, 6, 9, 4, 7); break;
                case EWeaponType.AntiMatter: AddWeaponParams(Links.Interface("Antimatter"), type, Links.Interface("PhysicDamage"), Brushes.Red, 6, 7, 3, 7, 7); break;
                case EWeaponType.Psi: AddWeaponParams(Links.Interface("Psi"), type, Links.Interface("IrregularDamage"), Brushes.Purple, 4, 3, 7, 6, 7); break;
                case EWeaponType.Dark: AddWeaponParams(Links.Interface("DarkEnergy"), type, Links.Interface("IrregularDamage"), Brushes.Purple, 7, 6, 5, 3, 9); break;
                case EWeaponType.Warp: AddWeaponParams(Links.Interface("Warp"), type, Links.Interface("IrregularDamage"), Brushes.Purple, 5, 9, 3, 3, 3); break;
                case EWeaponType.Time: AddWeaponParams(Links.Interface("Time"), type, Links.Interface("IrregularDamage"), Brushes.Purple, 3, 5, 9, 3, 7); break;
                case EWeaponType.Slicing: AddWeaponParams(Links.Interface("Slice"), type, Links.Interface("CyberDamage"), Brushes.Green, 3, 3, 5, 7, 5); break;
                case EWeaponType.Radiation: AddWeaponParams(Links.Interface("Radiation"), type, Links.Interface("CyberDamage"), Brushes.Green, 4, 3, 5, 2, 7); break;
                case EWeaponType.Drone: AddWeaponParams(Links.Interface("Drone"), type, Links.Interface("CyberDamage"), Brushes.Green, 4, 5, 9, 3, 5); break;
                case EWeaponType.Magnet: AddWeaponParams(Links.Interface("Magnet"), type, Links.Interface("CyberDamage"), Brushes.Green, 6, 4, 7, 6, 9); break;
            }
            
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromSeconds(2);
            Timer.Tick += Timer_Tick;
            Timer.Start();
        }
        public void Close()
        {
            Timer.Stop();
        }
        void AddWeaponParams(string title, EWeaponType type, string damtype, Brush damcolor, int shield, int hull, int acc, int spent, int crit)
        {
            AddTitle(title, 550, 100);
            AddPict(Links.Brushes.WeaponsBrushes[type]);
            AddText(Links.Interface("DamageType"), damtype, damcolor, 550, 200);
            PutInfoData(Pictogram.ShieldBrush, Brushes.Blue, shield, 550, 300, Links.Interface("ShieldDamage"));
            PutInfoData(Pictogram.HealthDamage, Brushes.Red, hull, 550, 460, Links.Interface("HealthDamage"));
            PutInfoData(Pictogram.AccuracyEnergy, Brushes.Gold, acc, 550, 620, Links.Interface("Accuracy"));
            PutInfoData(Pictogram.EnergyBrush, Brushes.Green, spent, 550, 780, Links.Interface("EnergyConsume"));
            PutInfoData(Pictogram.CriticalChance, Brushes.Orange, crit, 550, 940, Links.Interface("CritChanceShort"));

        }
        void AddTitle(string text, int left, int top)
        {
            TextBlock tb = Common.GetBlock(90, text, Brushes.White, 1500);
            tb.TextAlignment = TextAlignment.Left;
            Children.Add(tb);
            Canvas.SetLeft(tb, left); Canvas.SetTop(tb, top);
        }
        void AddText(string text1, string text2, Brush brush, int left, int top)
        {
            TextBlock tb = Common.GetBlock(50, text1, Brushes.White, 250);
            Children.Add(tb);
            Canvas.SetLeft(tb, left); Canvas.SetTop(tb, top);
            TextBlock tb2 = Common.GetBlock(50, text2, brush, 300);
            tb2.TextAlignment = TextAlignment.Left;
            Children.Add(tb2);
            Canvas.SetLeft(tb2, left+250); Canvas.SetTop(tb2, top);
        }
        void AddPict(Brush brush)
        {
            Rectangle pict = Common.GetRectangle(150, brush);
            pict.Stroke = Brushes.White; pict.StrokeThickness = 3;
            Children.Add(pict); Canvas.SetLeft(pict, 400); Canvas.SetTop(pict, 100);
        }
        void PutInfoData(Brush image, Brush color, int val, int left, int top, string title)
        {
            Rectangle border = new Rectangle(); border.Width = 500; border.Height = 200;
            border.Fill = Links.Brushes.Interface.RamkaWeapon;
            Children.Add(border); Canvas.SetLeft(border, left); Canvas.SetTop(border, top);
            Rectangle rect = Common.GetRectangle(65,image);
            Children.Add(rect); Canvas.SetLeft(rect, left+30); Canvas.SetTop(rect, top+75);
            string text = "";
            for (int i = 0; i < val; i++)
                text += String.Format("M{0},{1} v50", left + 120 + i * 40, top + 105);
            Path lines = new Path(); lines.Stroke = color; lines.StrokeThickness = 20;
            lines.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(text));
            Children.Add(lines);
            TextBlock tb = Common.GetBlock(50, title, Brushes.White, 435);
            Children.Add(tb);
            Canvas.SetLeft(tb, left+50); Canvas.SetTop(tb, top+35);
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            ShootAnim anim = null;
            weapon++; weapon = weapon % 3;
            Point startpoint = new Point();
            Gun CurGun = Gun.Left;
            switch (weapon)
            {
                case 0: startpoint = shipb2.HexShip.GetLeftGunPoint(); CurGun = Gun.Left; break;
                case 1: startpoint = shipb2.HexShip.GetMiddleGunPoint(); CurGun = Gun.Top; break;
                case 2: startpoint = shipb2.HexShip.GetRightGunPoint(); CurGun = Gun.Right; break;
            }
            Point target = shipb1.HexShip.GetShipCenterPoint();
            switch (WeaponType)
            {
                case EWeaponType.Laser: anim = new LaserAnim(startpoint, target, false); break;
                case EWeaponType.EMI: anim=new EMIAnim(startpoint, target, false); break;
                case EWeaponType.Plasma: anim = new PlasmaAnim(startpoint, target, false, true, true); break;
                case EWeaponType.Solar: anim = new SolarAnim(startpoint, target, false); break;
                case EWeaponType.Cannon: anim = new CannonAnim(startpoint, target, false, true); break;
                case EWeaponType.Gauss: anim = new GaussAnim(startpoint, target, false); break;
                case EWeaponType.Missle: anim=new MissleAnim(startpoint, target, false, true); break;
                case EWeaponType.AntiMatter: anim=new AntiAnim(startpoint, target, false); break;
                case EWeaponType.Psi: anim=new PsiAnim(startpoint, target, false); break;
                case EWeaponType.Dark: anim=new DarkAnim(startpoint, target, false); break;
                case EWeaponType.Warp: anim=new WarpAnim(startpoint, target, false); break;
                case EWeaponType.Time: anim=new TimeAnim(startpoint, target, false); break;
                case EWeaponType.Slicing: anim=new SliceAnim(startpoint, target, false); break;
                case EWeaponType.Radiation: anim=new RadAnim(startpoint, target, false); break;
                case EWeaponType.Drone: anim=new DroneAnim(startpoint, target, false, true, CurGun); break;
                case EWeaponType.Magnet: anim=new MagnetAnim(startpoint, target, false); break;
            }
            Children.Add(anim.Canvas);
        }

        public void SetWeapon (EWeaponType weapon)
        {
            ushort weaponid = 0;
            switch (weapon)
            {
                case EWeaponType.Laser: weaponid = 46301; break;
                case EWeaponType.EMI: weaponid = 46311; break;
                case EWeaponType.Plasma: weaponid = 47321; break;
                case EWeaponType.Solar: weaponid = 46331; break;
                case EWeaponType.Cannon: weaponid = 46341; break;
                case EWeaponType.Gauss: weaponid = 46351; break;
                case EWeaponType.Missle: weaponid = 47361; break;
                case EWeaponType.AntiMatter: weaponid = 46371; break;
                case EWeaponType.Psi: weaponid = 48381; break;
                case EWeaponType.Dark: weaponid = 48391; break;
                case EWeaponType.Warp: weaponid = 49401; break;
                case EWeaponType.Time: weaponid = 46411; break;
                case EWeaponType.Slicing: weaponid = 48421; break;
                case EWeaponType.Radiation: weaponid = 48431; break;
                case EWeaponType.Drone: weaponid = 49441; break;
                case EWeaponType.Magnet: weaponid = 49451; break;
            }
            for (int i = 0; i < 3; i++)
                schema.SetWeapon(weaponid, i, false);
        }
        DispatcherTimer FireTimer;
        TimeSpan WaveDelay;
        int Shoots;
        double Angle;
        public void ShieldFlashStarted(TimeSpan flashdelay, int shoots, TimeSpan Wavedelay, double angle)
        {
            if (FireTimer != null) FireTimer.Stop();
            WaveDelay = Wavedelay;
            Shoots = shoots;
            Angle = angle;
            if (flashdelay.TotalMilliseconds > 0)
            {
                FireTimer = new DispatcherTimer();
                FireTimer.Interval = flashdelay;
                FireTimer.Tick += new EventHandler(FireTimerShieldFlash_Tick);
                FireTimer.Start();
            }
            else
            {
                FireTimerShieldFlash_Tick(null, null);
            }
        }
        void FireTimerShieldFlash_Tick(object sender, EventArgs e)
        {
            if (sender != null) FireTimer.Stop();
            shipb1.HexShip.ShieldFlash(Angle, Shoots, WaveDelay);

        }
        public static Canvas GetBasicWeaponInfoCanvas()
        {
            Canvas canvas = new Canvas();
            canvas.Width = 1280; canvas.Height = 720;
            Rectangle rectE = new Rectangle(); rectE.Stroke = Brushes.Blue; rectE.StrokeThickness = 5;
            rectE.Fill = new SolidColorBrush(Color.FromArgb(101, 144, 144, 255));
            canvas.Children.Add(rectE); rectE.Width = 638; rectE.Height = 358;

            Rectangle rectP = new Rectangle(); rectP.Stroke = Brushes.Red; rectP.StrokeThickness = 5;
            rectP.Fill = new SolidColorBrush(Color.FromArgb(101, 255, 144, 144));
            canvas.Children.Add(rectP); rectP.Width = 638; rectP.Height = 358; Canvas.SetLeft(rectP, 642);

            Rectangle rectI = new Rectangle(); rectI.Stroke = Brushes.Purple; rectI.StrokeThickness = 5;
            rectI.Fill = new SolidColorBrush(Color.FromArgb(101, 255, 144, 255));
            canvas.Children.Add(rectI); rectI.Width = 638; rectI.Height = 358; Canvas.SetTop(rectI, 362);

            Rectangle rectC = new Rectangle(); rectC.Stroke = Brushes.Green; rectC.StrokeThickness = 5;
            rectC.Fill = new SolidColorBrush(Color.FromArgb(101, 144, 255, 144));
            canvas.Children.Add(rectC); rectC.Width = 638; rectC.Height = 358; Canvas.SetLeft(rectC, 642); Canvas.SetTop(rectC, 362);

            TextBlock textE = Common.GetBlock(50, Links.Interface("EnergyWeapon"), Brushes.White, 500);
            canvas.Children.Add(textE); Canvas.SetLeft(textE, 70); Canvas.SetTop(textE, 70);

            TextBlock textP = Common.GetBlock(50, Links.Interface("PhysicWeapon"), Brushes.White, 500);
            canvas.Children.Add(textP); Canvas.SetLeft(textP, 710); Canvas.SetTop(textP, 70);

            TextBlock textI = Common.GetBlock(50, Links.Interface("IrregularWeapon"), Brushes.White, 500);
            canvas.Children.Add(textI); Canvas.SetLeft(textI, 70); Canvas.SetTop(textI, 600);

            TextBlock textC = Common.GetBlock(50, Links.Interface("CyberWeapon"), Brushes.White, 500);
            canvas.Children.Add(textC); Canvas.SetLeft(textC, 710); Canvas.SetTop(textC, 600);

            Point[] WPoints = new Point[] { new Point(70, 150), new Point(160, 150), new Point(250,150), new Point(340, 150),
                new Point(850, 150), new Point(940,150), new Point(1030,150), new Point(1120, 150),
                new Point(70, 450), new Point(160,450), new Point(250, 450), new Point(340, 450),
            new Point(850,450), new Point(940,450), new Point(1030, 450), new Point(1120, 450)};
            for (int i=0;i<WPoints.Length;i++)
            {
                Rectangle wr = Common.GetRectangle(80, Links.Brushes.WeaponsBrushes[(EWeaponType)i]);
                canvas.Children.Add(wr); Canvas.SetLeft(wr, WPoints[i].X); Canvas.SetTop(wr, WPoints[i].Y);
            }

            return canvas;
        }
        
    }
}
