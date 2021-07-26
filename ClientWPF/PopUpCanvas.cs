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
    enum PopUpMode { Grow, Show}
   
    class PopUpCanvas:Canvas
    {
        static PopUpMode CurMode = PopUpMode.Show;
        public PopUpCanvas()
        {
            Width = 1920;
            Height = 1080;
            Canvas.SetZIndex(this, 200);
            Background = new SolidColorBrush(Color.FromArgb(180, 0,0,0));
        }

        void PopUpCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Remove();

        }
        FrameworkElement ChangeElement;
        TimeSpan showtime = TimeSpan.FromSeconds(0.5);
        public void Change(FrameworkElement element, string reason)
        {
            ChangeElement = element;
            UIElement last = Children[0];
            if (CurMode == PopUpMode.Grow)
            {
                last.RenderTransformOrigin = new Point(0.5, 0.5);
                ScaleTransform scale = new ScaleTransform(1, 0);
                last.RenderTransform = scale;
                DoubleAnimation anim = new DoubleAnimation(1, 0, showtime);
                scale.BeginAnimation(ScaleTransform.ScaleYProperty, anim);
                anim.Completed += Anim_CompletedChange;
                scale.BeginAnimation(ScaleTransform.ScaleXProperty, anim);
            }
            else if (CurMode==PopUpMode.Show)
            {
                DoubleAnimation hide = new DoubleAnimation(1, 0, showtime);
                hide.Completed += Anim_CompletedChange;
                last.BeginAnimation(UIElement.OpacityProperty, hide);
            }
        }
        private void Anim_CompletedChange(object sender, EventArgs e)
        {
            Children.Clear();
            Place(ChangeElement);
        }
        public void Place()
        {
            if (!Links.Controller.MainCanvas.Children.Contains(this))
                Links.Controller.MainCanvas.Children.Add(this);
        }
        public void Place(FrameworkElement element)
        {
            Links.Controller.MainCanvas.StopAutoChange();
            Place();
            RemoveMouseClickClose();
            if (Double.IsNaN(element.Width) || Double.IsNaN(element.Height))
            {
                Grid grid = new Grid();
                grid.Width = 1920;
                grid.Height = 1080;
                Children.Add(grid);
                grid.Children.Add(element);
                element.VerticalAlignment = VerticalAlignment.Center;
                element.HorizontalAlignment = HorizontalAlignment.Center;
                element = grid;
            }
            else
            {
                Children.Add(element);
                Canvas.SetLeft(element, (Width - element.Width) / 2);
                Canvas.SetTop(element, (Height - element.Height) / 2);
            }
            
            if (CurMode == PopUpMode.Grow)
            {
                element.RenderTransformOrigin = new Point(0.5, 0.5);
                ScaleTransform scale = new ScaleTransform(1, 0);
                element.RenderTransform = scale;
                DoubleAnimation anim = new DoubleAnimation(0, 1, showtime);
                scale.BeginAnimation(ScaleTransform.ScaleYProperty, anim);
                scale.BeginAnimation(ScaleTransform.ScaleXProperty, anim);
            }
            else if (CurMode==PopUpMode.Show)
            {
                DoubleAnimation showanim = new DoubleAnimation(0, 1, showtime);
                element.BeginAnimation(FrameworkElement.OpacityProperty, showanim);
            }
        }
        public void RemoveElement(FrameworkElement element)
        {
            Children.Remove(element);
            if (Children.Count == 0) Remove();
        }
        bool MouseClickCloseAdded = false;
        void AddMouseClickClose()
        {
            if (MouseClickCloseAdded)
                return;
            else
            {
                MouseDown+=new MouseButtonEventHandler(PopUpCanvas_MouseDown);
                MouseClickCloseAdded = true; 
            }
        }
        void RemoveMouseClickClose()
        {
            if (!MouseClickCloseAdded)
                return;
            else
            {
                MouseDown -= new MouseButtonEventHandler(PopUpCanvas_MouseDown);
                MouseClickCloseAdded = false;
            }
        }
        public void Place(FrameworkElement element, bool isCloseOnClick)
        {
            Place(element);

            if (isCloseOnClick)
                AddMouseClickClose();
            else
                RemoveMouseClickClose();

        }
        void PopUpCanvas_Click(object sender, RoutedEventArgs e)
        {
            Remove();
        }
        
        public void Remove()
        {
            if (Children.Count == 0) return;

            UIElement element = Children[0];
            if (CurMode == PopUpMode.Grow)
            {
                element.RenderTransformOrigin = new Point(0.5, 0.5);
                ScaleTransform scale = new ScaleTransform(1, 0);
                element.RenderTransform = scale;
                DoubleAnimation anim = new DoubleAnimation(1, 0, showtime);
                scale.BeginAnimation(ScaleTransform.ScaleYProperty, anim);
                anim.Completed += Anim_Completed;
                scale.BeginAnimation(ScaleTransform.ScaleXProperty, anim);
            }
            else if (CurMode == PopUpMode.Show)
            {
                DoubleAnimation hideanim = new DoubleAnimation(1, 0, showtime);
                hideanim.Completed += Anim_Completed;
                element.BeginAnimation(FrameworkElement.OpacityProperty, hideanim);
            }
        }

        private void Anim_Completed(object sender, EventArgs e)
        {
            Children.Clear();
            if (Links.Controller.MainCanvas.Children.Contains(this))
                Links.Controller.MainCanvas.Children.Remove(this);
        }
    }
    class PopUpButton : Button, PopUpElementInterface
    {
        public Button GetEndButton()
        {
            return this;
        }
    }
    interface PopUpElementInterface
    {
        Button GetEndButton();
    }
    class BattleInProcessMessage : Grid
    {
        public BattleInProcessMessage()
        {
            Width = 910;
            Height = 405;
            RowDefinitions.Add(new RowDefinition()); RowDefinitions.Add(new RowDefinition());
            RowDefinitions[1].Height = new GridLength(50);
            ColumnDefinitions.Add(new ColumnDefinition()); ColumnDefinitions.Add(new ColumnDefinition());
            Background = Links.Brushes.Interface.RamkaMedium;
            Canvas Close = Common.GetOkCanvas();
            Children.Add(Close);
            Grid.SetRow(Close, 1);
            Close.PreviewMouseDown += ForceEndTurn_PreviewMouseDown;
            TextBlock lbl = Common.GetBlock(30, "Некоторые ваши флоты находятся в бою. Если вы завершите ход, то все бои завершаться в автоматическом режиме. Переключить все бои в автоиатический режим?",
                Brushes.White, 500);
            
            Children.Add(lbl);
            Grid.SetColumnSpan(lbl, 2);
            lbl.HorizontalAlignment = HorizontalAlignment.Center;
            lbl.VerticalAlignment = VerticalAlignment.Center;
            Canvas cancel = Common.GetCloseCanvas(true);
            Children.Add(cancel);
            Grid.SetRow(cancel, 1); Grid.SetColumn(cancel, 1);
        }

        private void ForceEndTurn_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Links.Controller.PopUpCanvas.Remove();
            Links.Controller.MainCanvas.EndTurn(true);
            Links.Controller.MainCanvas.StartEndTurn();
        }
    }
    class SimpleConfirmMessage:Grid
    {
        public Canvas Ok;
        public SimpleConfirmMessage(List<Inline> list)
        {
            Width = 910;
            Height = 405;
            RowDefinitions.Add(new RowDefinition()); RowDefinitions.Add(new RowDefinition());
            ColumnDefinitions.Add(new ColumnDefinition()); ColumnDefinitions.Add(new ColumnDefinition());
            RowDefinitions[1].Height = new GridLength(50);
            Background = Links.Brushes.Interface.RamkaMedium;
            Ok = Common.GetOkCanvas(); Children.Add(Ok);
            Grid.SetRow(Ok, 1);
            Canvas Close = Common.GetCloseCanvas(true);
            Children.Add(Close);
            Grid.SetRow(Close, 1);
            Grid.SetColumn(Close, 1);
            TextBlock block = Common.GetBlock(30, "", Brushes.White, 700);
            block.Inlines.AddRange(list.ToArray());

            Children.Add(block); Grid.SetColumnSpan(block, 2);
        }
    }
    class SimpleInfoMessage : Grid
    {
        public SimpleInfoMessage(string Message)
        {
            Width = 910;
            Height = 405;
            RowDefinitions.Add(new RowDefinition()); RowDefinitions.Add(new RowDefinition());
            RowDefinitions[1].Height = new GridLength(50);
            Background = Links.Brushes.Interface.RamkaMedium;
            Canvas Close = Common.GetOkCanvas();
            Children.Add(Close);
            Grid.SetRow(Close, 1);
            Close.PreviewMouseDown += Close_PreviewMouseDown;
            Label lbl = new Label();
            lbl.Style = Links.TextStyle;
            lbl.Content = Message;
            lbl.FontSize = 40;
            Children.Add(lbl);
            lbl.HorizontalAlignment = HorizontalAlignment.Center;
            lbl.VerticalAlignment = VerticalAlignment.Center;
            lbl.Foreground = Brushes.White;
        }

        private void Close_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Links.Controller.PopUpCanvas.Remove();
        }
    }
    class BattlePopUp : Canvas
    {
        public BattlePopUp()
        {
            Width = 1280;
            Height = 720;
            Canvas.SetZIndex(this, 200);
            Background = new SolidColorBrush(Color.FromArgb(100, 100, 100, 100));
        }

        void PopUpCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Remove();
        }

        public void Place()
        {
            if (!Links.Controller.IntBoya.Children.Contains(this))
                Links.Controller.IntBoya.Children.Add(this);
        }
        public void Place(FrameworkElement element)
        {
            Place();
            RemoveMouseClickClose();
            Children.Add(element);
            Canvas.SetLeft(element, (Width - element.Width) / 2);
            Canvas.SetTop(element, (Height - element.Height) / 2);

        }
        public void RemoveElement(FrameworkElement element)
        {
            Children.Remove(element);
            if (Children.Count == 0) Remove();
        }
        bool MouseClickCloseAdded = false;
        void AddMouseClickClose()
        {
            if (MouseClickCloseAdded)
                return;
            else
            {
                MouseDown += new MouseButtonEventHandler(PopUpCanvas_MouseDown);
                MouseClickCloseAdded = true;
            }
        }
        void RemoveMouseClickClose()
        {
            if (!MouseClickCloseAdded)
                return;
            else
            {
                MouseDown -= new MouseButtonEventHandler(PopUpCanvas_MouseDown);
                MouseClickCloseAdded = false;
            }
        }
        public void Place(FrameworkElement element, bool isCloseOnClick)
        {
            Place(element);

            if (isCloseOnClick)
                AddMouseClickClose();
            else
                RemoveMouseClickClose();

        }
        void PopUpCanvas_Click(object sender, RoutedEventArgs e)
        {
            Remove();
        }

        public void Remove()
        {
            Children.Clear();
            if (Links.Controller.IntBoya.Children.Contains(this))
                Links.Controller.IntBoya.Children.Remove(this);
        }
    }
    class MediumInfo : Canvas
    {
        public MediumInfo(string text, MouseButtonEventHandler OkHandler)
        {
            Width = 910; Height = 405; Background = Links.Brushes.Interface.RamkaMedium;
            TextBlock textblock = Common.GetBlock(36, text, Brushes.White, 800);
            Children.Add(textblock);
            Canvas.SetLeft(textblock, 55); Canvas.SetTop(textblock, 80);
            GameButton OkButton = new GameButton(200, 80, Links.Interface("Ok"), 30);
            Children.Add(OkButton); Canvas.SetLeft(OkButton, 130); Canvas.SetTop(OkButton, 290);
            if (OkHandler!=null)
                OkButton.PreviewMouseDown += OkHandler;

            GameButton CancelButton = new GameButton(200, 80, Links.Interface("Cancel"), 30);
            Children.Add(CancelButton); Canvas.SetLeft(CancelButton, 580); Canvas.SetTop(CancelButton, 290);
            CancelButton.PreviewMouseDown += CancelButton_PreviewMouseDown;
        }

        private void CancelButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Links.Controller.BattlePopUp.Remove();
        }
    }
    class GameButton : Border
    {
        TextBlock block;
        public GameButton(int width, int height, string text, int textsize)
        {
            Width = width; Height = height;
            Background = Links.Brushes.Interface.ButtonMedium;
            block = Common.GetBlock(textsize, text, Brushes.White, width - 20);
            Child = block;
            RenderTransformOrigin = new Point(0.5, 0.5);
            PreviewMouseDown += GameButton_PreviewMouseDown;
        }
        public void SetText(string text)
        {
            block.Text = text;
        }
        private void GameButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ScaleTransform scale = new ScaleTransform(0.8, 0.8);
            RenderTransform = scale;
            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            System.Windows.Threading.DispatcherTimer timer = (System.Windows.Threading.DispatcherTimer)sender;
            timer.Stop();
            RenderTransform = null;
        }
        public void PutToCanvas(Canvas canvas, int left, int top)
        {
            canvas.Children.Add(this);
            Canvas.SetLeft(this, left);
            Canvas.SetTop(this, top);
        }
        public void AddEvent(MouseButtonEventHandler handler)
        {
            PreviewMouseDown += handler;
            if (Events == null) Events = new List<MouseButtonEventHandler>();
            Events.Add(handler);
        }
        List<MouseButtonEventHandler> Events;
        public void ClearSubscribers()
        {
            if (Events == null) return;
           foreach (MouseButtonEventHandler handler in Events)
            {
                PreviewMouseDown -= handler;
            }
            Events = null;
        }
    }
}
