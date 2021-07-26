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
    class SectorCreate:Canvas
    {
        SortedList<string, ImageBrush> Images;
        SortedList<SectorTypes, string> Names;

        List<Point> Points;
        SortedSet<SectorTypes> Builds;
        Land Land;
        int SectorID;
        Rectangle rect_create, rect_create_a;
        int ID = 0;
        Rectangle InfoRect;
        BrightText TitleBlock;
        Grid TitleGrid;
        TextBlock Text;
        ScrollViewer viewer;
        public SectorCreate(Land land, int sectorid)
        {
            Land = land; SectorID = sectorid;
            FillAll();
            
            foreach (LandSector sector in Land.Locations)
                if (sector.Type != SectorTypes.Clear && sector.Type != SectorTypes.War)
                    Builds.Remove(sector.Type);
            
            
            Width = 1280; Height = 1107;
            Children.Add(AddRect(1303, 1007, "fone",0,0));
            Children.Add(AddRect(1202, 484, "ui_colony", 61, -11));
            Children.Add(AddRect(1573, 231, "decor", -161, 815));
            Children.Add(AddRect(1455, 390, "elemeny", -101, 510));
            Rectangle X;
            Children.Add(X=AddRect(53, 53, "x", 1230, 10));
            Canvas create = new Canvas(); Children.Add(create);
            Canvas.SetLeft(create, 380); Canvas.SetTop(create, 955);
            create.Children.Add(rect_create = AddRect(492, 72, "button", 0, 0));
            create.Children.Add(rect_create_a = AddRect(492, 72, "button_on", 0, 0));
            rect_create_a.Opacity = 0;
            create.MouseEnter += Create_MouseEnter;
            create.MouseLeave += Create_MouseLeave;
            create.MouseDown += Create_MouseDown;
            X.PreviewMouseDown += X_PreviewMouseDown;
            BrightText br = new BrightText("Описание сектора", Colors.Orange, Colors.Transparent, 20);
            Children.Add(br);
            Canvas.SetLeft(br, 250); Canvas.SetTop(br, 10);
            TitleGrid = new Grid(); Children.Add(TitleGrid); Canvas.SetLeft(TitleGrid, 850); Canvas.SetTop(TitleGrid, 10); TitleGrid.Width = 280;
            TitleGrid.Height = 60;
            viewer = new ScrollViewer();
            viewer.Width = 680; viewer.Height = 400; Children.Add(viewer); Canvas.SetLeft(viewer, 70); Canvas.SetTop(viewer, 70);
            viewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden; viewer.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            Text = Common.GetBlock(30, "", Links.Brushes.SkyBlue, 680);
            Text.VerticalAlignment = VerticalAlignment.Top; Text.TextAlignment = TextAlignment.Justify;
            viewer.Content = Text;
            viewer.PreviewMouseWheel += Viewer_PreviewMouseWheel;
            Refresh();
        }

        private void Viewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            viewer.ScrollToVerticalOffset(viewer.VerticalOffset - e.Delta);
        }

        private void X_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Links.Controller.PopUpCanvas.Remove();
        }

        void Refresh()
        {
            for (int i = 1; i < 11; i++)
            {
                SectorTypes type = (SectorTypes)i;
                SectorButton b1 = new SectorButton(Names[type], Builds.Contains(type)==false, i, Images);
                Children.Add(b1);
                Canvas.SetLeft(b1, Points[i-1].X); Canvas.SetTop(b1, Points[i-1].Y);
                //Buttons.Add(b1);
            }
        }
        Rectangle TempRect;
        Viewbox TempTitle;
        public void ShowInfo(int id)
        {
            if (ID == id) return;
            ID = id;
            if (InfoRect != null)
            {
                TempRect = InfoRect;
                DoubleAnimation animHide = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.5));
                animHide.Completed += AnimHide_Completed;
                InfoRect.BeginAnimation(Rectangle.OpacityProperty, animHide);
                TempTitle = TitleBlock;
                TitleBlock.BeginAnimation(Viewbox.OpacityProperty, animHide);
            }
            InfoRect = new Rectangle(); InfoRect.Width = 454; InfoRect.Height = 457;
            Children.Add(InfoRect); Canvas.SetLeft(InfoRect, 763); Canvas.SetTop(InfoRect, 48);
            DoubleAnimation anim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5));
            InfoRect.BeginAnimation(Rectangle.OpacityProperty, anim);
            SectorTypes type = (SectorTypes)ID;
            InfoRect.Fill = Images[String.Format("active_{0}", Names[type])];
            TitleBlock = new BrightText(SectorImages.List[type].Title, Colors.SkyBlue, Colors.Transparent, 20);
            TitleBlock.Resize(0.5);
            TitleGrid.Children.Add(TitleBlock);
            TitleBlock.BeginAnimation(Viewbox.OpacityProperty, anim);
            SetText();
            DoubleAnimationUsingKeyFrames textanim = new DoubleAnimationUsingKeyFrames();
            textanim.Duration = TimeSpan.FromSeconds(0.5);
            textanim.KeyFrames.Add(new DiscreteDoubleKeyFrame(0, KeyTime.FromPercent(0)));
            textanim.KeyFrames.Add(new DiscreteDoubleKeyFrame(1, KeyTime.FromPercent(0.2)));
            textanim.KeyFrames.Add(new DiscreteDoubleKeyFrame(0, KeyTime.FromPercent(0.3)));
            textanim.KeyFrames.Add(new DiscreteDoubleKeyFrame(1, KeyTime.FromPercent(0.4)));
            textanim.KeyFrames.Add(new DiscreteDoubleKeyFrame(0, KeyTime.FromPercent(0.9)));
            textanim.KeyFrames.Add(new DiscreteDoubleKeyFrame(1, KeyTime.FromPercent(1)));
            Text.BeginAnimation(TextBlock.OpacityProperty, textanim);
            //CurText.BeginAnimation(TextBlock.OpacityProperty, textanim);
        }
        private void AnimHide_Completed(object sender, EventArgs e)
        {
            Children.Remove(TempRect);
            TitleGrid.Children.Remove(TempTitle);
        }
        private void Create_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ID == 0) return;
            Links.Controller.PopUpCanvas.Remove();
            SectorTypes s = (SectorTypes)ID;
            string eventresult = Events.BuildNewSector(Land, s, SectorID);
            if (eventresult == "")
            {
                Gets.GetTotalInfo("После эвента по строительству сектора");
                Colonies.InnerRefresh();
            }//Links.Controller.SelectPanel(GamePanels.Colonies, SelectModifiers.None);
        }

        private void Create_MouseLeave(object sender, MouseEventArgs e)
        {
            DoubleAnimation ShowAnim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.3));
            DoubleAnimation HideAnim = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.3));
            rect_create.BeginAnimation(Rectangle.OpacityProperty, ShowAnim);
            rect_create_a.BeginAnimation(Rectangle.OpacityProperty, HideAnim);
        }

        private void Create_MouseEnter(object sender, MouseEventArgs e)
        {
            DoubleAnimation ShowAnim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.3));
            DoubleAnimation HideAnim = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.3));
            rect_create.BeginAnimation(Rectangle.OpacityProperty, HideAnim);
            rect_create_a.BeginAnimation(Rectangle.OpacityProperty, ShowAnim);
        }
        Rectangle AddRect(int width, int height, string fill, int left, int top)
        {
            Rectangle rect = Common.GetRectangle(width, height, Images[fill]);
            Canvas.SetLeft(rect, left); Canvas.SetTop(rect, top);
            return rect;
        }
        void SetText()
        {
            Text.Inlines.Clear();
            SectorTypes type = (SectorTypes)ID;
            switch (type)
            {
                case SectorTypes.Live: Text.Inlines.Add(new Run("Жилой сектор - включает все необходимые материалы для выживания в любых климатических условиях, структура данной модификации рассчитана на долгосрочное использование ресурсов, благодаря чему сектор имеет все необходимое для воспитания следующих поколений колонистов."));
                    Text.Inlines.Add(new LineBreak());
                    Text.Inlines.Add(new Run("Несмотря на такой успех, модернизация куба проводилась более 50ти лет, самыми светлыми умами. После чего данный тип сектора получил название* Pandora*"));
                    Text.Inlines.Add(new LineBreak());
                    Text.Inlines.Add(new Run("Но, как и во всех экспериментах империи *зеленого альянса *, изначальное тестирование улучшений приводило к не мысленным последствиям, порой с летальным исходом среди населения, конечно же в таких случаях колонистов считали героями, и награждали посмертно. "));
                    Text.Inlines.Add(new LineBreak());
                    Text.Inlines.Add(new LineBreak());
                    Text.Inlines.Add(new Run("Выписка из документа №71 2176 год, планета *Сердце дьявола* "));
                    Text.Inlines.Add(new LineBreak());
                    Text.Inlines.Add(new Run("-Эта чертова махина продержалась около трех месяцев! температурные скачки расплавили обшивку, все нижние уровни расплавлены! "));
                    Text.Inlines.Add(new LineBreak());
                    Text.Inlines.Add(new Run("-Принято, эвакуируйте, население на спасательных капсулах! "));
                    Text.Inlines.Add(new LineBreak());
                    Text.Inlines.Add(new Run("-Но.... но все капсулы находились на нижних уровнях..... "));
                    Text.Inlines.Add(new LineBreak());
                    Text.Inlines.Add(new Run("-Так и запишем в докладе, надеюсь последующие эксперименты на планетах огненного типа будут успешнее.. вы там держитесь..."));
                    break;
                case SectorTypes.Money: Text.Inlines.Add(new Run("Впервые в истории запатентованная в 2223 году модификация принадлежала, Дейтону Дарли, известному пирату и конструктору, никто до сих пор не может понять, каким образом *Зеленый альянс* допустил подобную аферу, но каким-то чудесным образом, чертежи данного сектора попали в штаб инновационных технологий *Mileniuma* А в последующем были воспроизведены на орбите *Плутона* и стали первым крупным торговым сектором в галактике. Многие и по сей день подозревают что в данных секторах проходят не мысленные аферы и крутятся огромные суммы. "));
                    Text.Inlines.Add(new LineBreak());
                    Text.Inlines.Add(new Run("Инфраструктура колоний никогда не стоит на месте и постоянно развивается, независимо от направления движения. Одним из ключевых аспектов развития является рынок. Куб c установленной модификацией *Vavilon* является центром потока ресурсов, а также имеет многоступенчатую нелинейную структуру портов для малогабаритного и крупногабаритного торгового транспорта, что позволяет эффективнее выполнять торговые операции. Ныне же эту модификацию называют *торговым сектором* "));
                    break;
                case SectorTypes.Repair:
                    Text.Inlines.Add(new Run("Ведущий инженер и физик в 2139 годо предоставил миру первый прототип синтетической трехслойной, многослойной кварцевой стали. Которая была способна впитать в себя 2.114 *10^14Дж"));
                    Text.Inlines.Add(new LineBreak());
                    Text.Inlines.Add(new Run("Технологии не стоят на месте, в 2148 крупная компания по градостроительству именуемая *Milenium* представила на мировом рынке полностью автономный куб*FP-516*, сочетающий в себе все научные достижения квантовой энергетики и производства металлов. Размеры этого изобретения были настолько колоссальны, что могли вместить в себя пару городских кварталов."));
                    Text.Inlines.Add(new LineBreak());
                    Text.Inlines.Add(new Run("В дальнейшем, люди стали использовать куб * FP - 516 * В многочисленных проектах по колонизации и терраформированию планет, так как его применения были практически безграничными, а его работоспособность позволяла выполнять операции в самых суровых планетарных условиях, будь то вулканическая почва планеты, раскаляющаяся до 450 градусов по фаренгейту."));
                    Text.Inlines.Add(new LineBreak());
                    Text.Inlines.Add(new Run("В недалеком будущем, данные кубы стали называть просто * сектор * "));
                    break;
                case SectorTypes.Chips:
                    Text.Inlines.Add(new Run("H.I.T. имеет специальное IT подразделение - H.I.T. I.T. (Внезапно, правда?). \"Hit It\" занимается добычей поломанных микросхем с орбиты планет, или космоса. В основном это объекты в более-менее рабочем состоянии которые просто дрейфуют в космосе. Hit It либо перебирает микросхемы, либо сдаёт их на металл. Если попадаются действительно ценные образцы, их дорабатывают до нужд платформы, на которой находится отряд Hit It, и используют на деле."));
                    break;
                case SectorTypes.MetalCap:
                    Text.Inlines.Add(new Run("H.I.T.хранит металлы в специальных токсичных боксах.Так как любой металл находят не в чистом состоянии, его нужно очистить от примесей, и для этого выступают специально настроенные боксы, которые в течении определённого времени очищают металл от грязи, песка, камней и прочего ненужного материала.Таким образом металл просеивает сам себя.Быстро, дешево, ужасно токсично."));
                    break;
                case SectorTypes.Metall:
                    Text.Inlines.Add(new Run("Добыча металла -одна из самых главных веток промышленности в наше время. HeavyImperiumTechnologies, или же H.I.T., монополизировала этот бизнес в недавнем прошлом, и теперь управляет поставками и добычей по всему миру. H.I.T. - промышленная компания придумавшая инновационные, но очень загрязняющие атмосферу способы добычи металлов, видимо, у всего есть своя цена.Добыча металла - одна из самых главных веток промышленности в наше время. HeavyImperiumTechnologies, или же H.I.T., монополизировала этот бизнес в недавнем прошлом, и теперь управляет поставками и добычей по всему миру. H.I.T. - промышленная компания придумавшая инновационные, но очень загрязняющие атмосферу способы добычи металлов, видимо, у всего есть своя цена. "));
                    break;
                case SectorTypes.War:
                    Text.Inlines.Add(new Run("H.I.T. поддерживает милитаризм несмотря на множество забастовок внутри компании. Военная промышленность приносит очень много денег, и, наверное, является самой прибыльной из всех ответвлений. H.I.T. делают всё, от патронов до боеголовок, от электрокаров до космических танкеров. H.I.T. в этом профи, и все это знают"));
                    break;
                case SectorTypes.AntiCap:
                    Text.Inlines.Add(new Run("Антиматерия хранится в специальных капсулах на специализированных для этого складах, при любых признаках нестабильности капсулы, капсула либо на большой скорости выстреливает на орбиту где уже и детонирует, либо мгновенно доводится до очень минусовой температуры, что замедляет процесс разрушения капсулы и даёт время на попытки её сохранения. H.I.T. считает, что антиматерия — это один из самых важных, и трудно добываемых ресурсов. "));
                    break;
                case SectorTypes.Anti:
                    Text.Inlines.Add(new Run("Добыча антиматерии является весьма опасным делом, и поэтому H.I.T. проводят все эксперименты и работы по добыче в специальных, закрытых подземных корпусах. На поверхности корпуса выглядят как обычный аэродром, куда всех ученых посылают с грифом \"ОЧЕНЬ СЕКРЕТНО\". Антиматерия добывается только профессионалами, и только в маленьких количествах. Инцидент в корпусе по добычи может стоить жизни, например, целому острову. "));
                    break;
                case SectorTypes.ChipsCap:
                    Text.Inlines.Add(new Run("Для хранения микросхем Hit It используют специальные складские лаборатории, там микросхемы чистят, налаживают, и передают на хранение в специальные вакуумные отсеки до тех пор, пока микросхемы не понадобятся в использовании. В лабораториях поддерживается стерильность и постоянный, но не сильный минус градус. "));
                    break;
            }
        }
        void FillAll()
        {
            Names = new SortedList<SectorTypes, string>();
            Names.Add(SectorTypes.Live, "live");
            Names.Add(SectorTypes.Money, "money");
            Names.Add(SectorTypes.Metall, "metall");
            Names.Add(SectorTypes.MetalCap, "metallcap");
            Names.Add(SectorTypes.Chips, "chips");
            Names.Add(SectorTypes.ChipsCap, "chipscap");
            Names.Add(SectorTypes.Anti, "anti");
            Names.Add(SectorTypes.AntiCap, "anticap");
            Names.Add(SectorTypes.Repair, "repair");
            Names.Add(SectorTypes.War, "war");
            Images = new SortedList<string, ImageBrush>();
            Images.Add("fone", Gets.AddSectorCreate("fone"));
            Images.Add("ui_colony", Gets.AddSectorCreate("ui_colony"));
            Images.Add("target", Gets.AddSectorCreate("target_sector"));
            Images.Add("elemeny", Gets.AddSectorCreate("elemeny"));
            Images.Add("decor", Gets.AddSectorCreate("decor"));
            Images.Add("button", Gets.AddSectorCreate("button"));
            Images.Add("button_on", Gets.AddSectorCreate("button_on"));
            Images.Add("x", Gets.AddSectorCreate("x"));
            Builds = new SortedSet<SectorTypes>();
            for (int i=1; i<11;i++)
            {
                SectorTypes type = (SectorTypes)i;
                Builds.Add(type);
                string text = Names[type];
                Images.Add(String.Format("active_{0}",text), Gets.AddSectorCreate(String.Format("active_{0}", text)));
                Images.Add(String.Format("s_{0}", text), Gets.AddSectorCreate(String.Format("s_{0}", text)));
                Images.Add(String.Format("s_{0}_active", text), Gets.AddSectorCreate(String.Format("s_{0}_active", text)));
            }
            Points = new List<Point>() { new Point(42, 462), new Point(275, 462), new Point(505, 462), new Point(736, 462), new Point(966, 462),
            new Point(42,692), new Point(275, 692), new Point(505, 692), new Point(736, 692), new Point(966, 692)};
        }
        class SectorButton : Canvas
        {
            DoubleAnimation ShowAnim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.2));
            DoubleAnimation HideAnim = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.2));
            Rectangle TargetRect;
            bool IsBuilded;
            Path TargetPath;
            int ID;
            public SectorButton(string title, bool isbuilded, int id, SortedList<string, ImageBrush> images)
            {
                ID = id;
                IsBuilded = isbuilded;
                Width = 262; Height = 264;
                if (IsBuilded == false)
                {
                    Background = images[string.Format("s_{0}_active", title)];
                    TargetRect = new Rectangle(); TargetRect.Width = 325; TargetRect.Height = 306;
                    Children.Add(TargetRect); Canvas.SetLeft(TargetRect, -29); Canvas.SetTop(TargetRect, -33);
                    TargetRect.Fill = images["target"]; TargetRect.Opacity = 0;
                    PathFigureCollectionConverter conv = new PathFigureCollectionConverter();
                    TargetPath = new Path(); TargetPath.Fill = new SolidColorBrush(Color.FromArgb(1, 0, 0, 0));
                    TargetPath.Data = new PathGeometry((PathFigureCollection)(conv.ConvertFrom("M25,25 h215 v215 h-215z")));
                    Children.Add(TargetPath);
                    TargetPath.MouseEnter += SectorButton_MouseEnter;
                    TargetPath.MouseLeave += SectorButton_MouseLeave;
                    TargetPath.MouseDown += TargetPath_MouseDown;
                }
                else
                    Background = images[string.Format("s_{0}", title)];
            }

            private void TargetPath_MouseDown(object sender, MouseButtonEventArgs e)
            {
                SectorCreate canvas = (SectorCreate)Parent;
                canvas.ShowInfo(ID);
            }

            private void SectorButton_MouseLeave(object sender, MouseEventArgs e)
            {
                TargetRect.BeginAnimation(Rectangle.OpacityProperty, HideAnim);
            }

            private void SectorButton_MouseEnter(object sender, MouseEventArgs e)
            {
                TargetRect.BeginAnimation(Rectangle.OpacityProperty, ShowAnim);
            }
        }
    }
}
