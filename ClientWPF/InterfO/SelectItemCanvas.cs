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
    class SelectItemCanvas : Canvas
    {
        Label nameLabel;
        ISort[] Basearray;
        ISort[] array;
        string[] ColumnNames;
        int[] ColumnPos;
        NameLabelGrid[] Titles;
        Grid grid;
        List<UIElement> ElementToClear;
        ScrollViewer viewer;
        ApplyEnum Target;
        public SelectItemCanvas(ISort[] array, string[] columnNames, int[] columnPos, string title, ApplyEnum target)
        {
            Target = target;
            ElementToClear = new List<UIElement>();
            ((SchemaPanel)Links.Controller.schemaspanel).SelectID = -1;
            Basearray = array;
            this.array = array;
            ColumnNames = columnNames;
            ColumnPos = columnPos;
            //Background = Brushes.LightGray;
            Width = 600;
            Height = 600;
            nameLabel = new Label();
            nameLabel.Content = title;
            nameLabel.Background = Brushes.Black;
            nameLabel.FontSize = 20;
            nameLabel.FontWeight = FontWeights.Bold;
            nameLabel.Foreground = Brushes.White;
            Children.Add(nameLabel);
            Size s = nameLabel.DesiredSize;
            Canvas.SetLeft(nameLabel, 300 - title.Length * nameLabel.FontSize / 4);

            viewer = new ScrollViewer();
            Children.Add(viewer);
            viewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            viewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            viewer.Width = 600;
            viewer.Height = 400;
            Canvas.SetTop(viewer, 50);

            grid = new Grid();

            //grid.Width = 600;
            viewer.Content = grid;
            grid.Background = Brushes.Black;
            Button SelectButton = new Button();
            Children.Add(SelectButton);
            SelectButton.Content = "Confirm";
            SelectButton.Style = Links.ButtonStyle;
            SelectButton.FontSize = 20;
            Canvas.SetTop(SelectButton, 450);
            Canvas.SetLeft(SelectButton, 200);
            SelectButton.Click += new RoutedEventHandler(SelectButton_Click);

            Button CloseButton = new Button();
            CloseButton.Style = Links.ButtonStyle;
            CloseButton.FontSize = 20;
            CloseButton.Content = "Close";
            Canvas.SetTop(CloseButton, 450);
            Canvas.SetLeft(CloseButton, 300);
            Children.Add(CloseButton);
            CloseButton.Click += new RoutedEventHandler(CloseButton_Click);



            //grid.Background = Brushes.Gray;
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            Label ElementNamelabel = new Label();
            ElementNamelabel.Content = "Название";
            ElementNamelabel.FontWeight = FontWeights.Bold;
            ElementNamelabel.Foreground = Brushes.White;
            grid.Children.Add(ElementNamelabel);
            Titles = new NameLabelGrid[ColumnNames.Length];
            for (int i = 0; i < ColumnNames.Length; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.ColumnDefinitions[i + 1].Width = new GridLength(70);
                Titles[i] = new NameLabelGrid(ColumnNames[i], ColumnPos[i]);
                grid.Children.Add(Titles[i]);
                Grid.SetColumn(Titles[i], i + 1);
                Border bord = new Border();
                bord.BorderBrush = Brushes.White;
                bord.BorderThickness = new Thickness(2);
                Grid.SetColumn(bord, i + 1);
                grid.Children.Add(bord);
            }
            grid.RowDefinitions.Add(new RowDefinition()); grid.RowDefinitions[0].Height = new GridLength(50, GridUnitType.Pixel);

            for (int i = 0; i < array.Length; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition()); grid.RowDefinitions[i + 1].Height = new GridLength(30);

            }
            grid.MouseDown += new MouseButtonEventHandler(grid_MouseDown);
            grid.MouseWheel += new MouseWheelEventHandler(grid_MouseWheel);
            Fill();
        }

        void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Links.Controller.PopUpCanvas.Remove();
        }

        void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            ((SchemaPanel)Links.Controller.schemaspanel).SelectID = SelectedTag;
            ((SchemaPanel)Links.Controller.schemaspanel).Target = Target;
            ((SchemaPanel)Links.Controller.schemaspanel).ApplyChanges();
            Links.Controller.PopUpCanvas.Remove();
        }

        void grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            viewer.ScrollToHorizontalOffset(viewer.HorizontalOffset - e.Delta);
        }

        void grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            bool CheckError = false;
            SortedList<int, int> Primes = new SortedList<int, int>();
            for (int i = 0; i < Titles.Length; i++)
            {
                if (Titles[i].Vector != SortVector.None && Titles[i].Prime == 4)
                    CheckError = true;
                if (Titles[i].Prime != 4 && Titles[i].Vector == SortVector.None)
                    CheckError = true;
                int prime = Titles[i].Prime;
                if (prime != 4)
                {
                    if (Primes.ContainsKey(prime)) CheckError = true;
                    else
                        Primes.Add(prime, prime);
                }
            }
            if (Primes.Count > 0 && Primes.ElementAt(0).Key != 0) CheckError = true;
            for (int i = 1; i < Primes.Count; i++)
                if (Primes.ElementAt(i - 1).Key + 1 != Primes.ElementAt(i).Key) CheckError = true;

            if (!CheckError) grid.Background = new SolidColorBrush(Color.FromArgb(255, 0, 20, 0));
            else grid.Background = new SolidColorBrush(Color.FromArgb(255, 20, 0, 0));
            if (CheckError == false)
            {
                int z = 0;
                for (int i = 0; i < Titles.Length; i++)
                {
                    if (Titles[i].Vector != SortVector.None)
                        z++;
                }
                SortParam[] SortParams = new SortParam[z];
                for (int i = 0; i < Titles.Length; i++)
                {
                    if (Titles[i].Vector != SortVector.None)
                    {
                        SortParam param = new SortParam(Titles[i].ColumnPos, Titles[i].Vector == SortVector.FromUp);
                        SortParams[Titles[i].Prime] = param;
                    }
                }
                if (SortParams.Length == 0) return;
                bool ParamsChanges = false;
                if (LastSortParams == null)
                    ParamsChanges = true;
                else
                    if (LastSortParams.Length != SortParams.Length)
                        ParamsChanges = true;
                    else
                        for (int i = 0; i < SortParams.Length; i++)
                            if (SortParams[i].FromMax != LastSortParams[i].FromMax || SortParams[i].Param != LastSortParams[i].Param)
                                ParamsChanges = true;
                if (ParamsChanges)
                {
                    array = Sort4(array, SortParams);
                    foreach (UIElement element in ElementToClear)
                        grid.Children.Remove(element);
                    Fill();
                    LastSortParams = SortParams;
                }


            }
        }
        SortParam[] LastSortParams;
        void Fill()
        {
            ElementToClear = new List<UIElement>();

            for (int i = 0; i < array.Length; i++)
            {

                Border border = GetElementName(array[i].GetName(), array[i].GetID());
                grid.Children.Add(border);
                Grid.SetRow(border, i + 1);
                ElementToClear.Add(border);
                for (int j = 0; j < ColumnPos.Length; j++)
                {
                    Border bord1 = new Border();
                    bord1.BorderThickness = new Thickness(2);
                    bord1.BorderBrush = Brushes.White;
                    grid.Children.Add(bord1);
                    Grid.SetRow(bord1, i + 1);
                    Grid.SetColumn(bord1, j + 1);
                    bord1.Tag = array[i].GetID();
                    UIElement element = array[i].GetElement(j);
                    if (element == null)
                    {
                        Label lbl = new Label();
                        lbl.Content = array[i].GetParam(j);
                        lbl.FontWeight = FontWeights.Bold;
                        lbl.Foreground = Brushes.White;
                        lbl.HorizontalAlignment = HorizontalAlignment.Center;
                        bord1.Child = lbl;
                    }
                    else
                    {
                        bord1.Child = element;
                    }
                    ElementToClear.Add(bord1);
                }
            }
            SelectRow();
        }



        Border GetElementName(string text, int id)
        {
            Border border = new Border();
            border.BorderThickness = new Thickness(2);
            border.BorderBrush = Brushes.White;
            Label lbl = new Label();
            lbl.Content = text;
            border.Child = lbl;
            lbl.Background = Brushes.Red;
            lbl.Foreground = Brushes.White;
            lbl.FontWeight = FontWeights.Bold;
            lbl.MouseDown += new MouseButtonEventHandler(lbl_MouseDown);
            lbl.Tag = id;
            return border;
        }
        int SelectedTag = -1;
        void SelectRow()
        {
            if (SelectedTag == -1) return;
            foreach (UIElement element in ElementToClear)
            {
                if (element.GetType() == typeof(Border))
                {
                    Border bord = (Border)element;
                    if (bord.Tag != null && (int)bord.Tag == SelectedTag)
                    {
                        bord.Background = Brushes.White;
                        if (bord.Child.GetType() == typeof(Label))
                        {
                            Label label = (Label)bord.Child;
                            label.Foreground = Brushes.Black;
                        }
                    }
                    else
                    {
                        bord.Background = null;
                        if (bord.Child.GetType() == typeof(Label))
                        {
                            Label label = (Label)bord.Child;
                            label.Foreground = Brushes.White;
                        }
                    }

                }
            }
        }
        void lbl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int id = (int)((Label)sender).Tag;
            SelectedTag = id;
            SelectRow();
        }
        class SortParam
        {
            public int Param;
            public bool FromMax;
            public SortParam(int param, bool fromMax)
            {
                Param = param; FromMax = fromMax;
            }
        }
        static ISort[] Sort4(ISort[] array, SortParam[] SortParams)
        {
            List<ISort> result = new List<ISort>();
            if (SortParams.Length > 1)
            {
                SortParam[] ShortSortParams = new SortParam[SortParams.Length - 1];
                for (int i = 0; i < SortParams.Length - 1; i++)
                    ShortSortParams[i] = SortParams[i];
                ISort[] MiddleResult = Sort4(array, ShortSortParams);
                List<ISort> temp = new List<ISort>();
                for (int i = 0; i < MiddleResult.Length; )
                {
                    int[] IntParams = new int[ShortSortParams.Length];
                    for (int j = 0; j < IntParams.Length; j++)
                        IntParams[j] = MiddleResult[i].GetParam(ShortSortParams[j].Param);
                    temp.Add(MiddleResult[i]);
                    if (i + 1 < MiddleResult.Length) i++;
                    else
                    {
                        ISort[] result2 = Sort(temp.ToArray(), SortParams[SortParams.Length - 1].Param, SortParams[SortParams.Length - 1].FromMax);
                        result.AddRange(result2);
                        break;
                    }
                    bool ItIsAOurElement = true;
                    for (int j = 0; j < IntParams.Length; j++)
                        if (IntParams[j] == MiddleResult[i].GetParam(SortParams[j].Param)) continue;
                        else ItIsAOurElement = false;
                    if (ItIsAOurElement) continue;
                    else
                    {
                        ISort[] result3 = Sort(temp.ToArray(), SortParams[SortParams.Length - 1].Param, SortParams[SortParams.Length - 1].FromMax);
                        result.AddRange(result3);
                        temp = new List<ISort>();
                        continue;
                    }
                }
            }
            else
            {
                result.AddRange(Sort(array, SortParams[0].Param, SortParams[0].FromMax));
            }
            return result.ToArray();
        }
        static ISort[] Sort(ISort[] array, int param, bool FromMax)
        {

            int FirstValue, cur_pos;
            ISort temp;
            for (int i = 0; i < array.Length; i++)
            {
                FirstValue = array[i].GetParam(param);
                cur_pos = i;
                for (int j = i + 1; j < array.Length; j++)
                {
                    if (FromMax)
                    {
                        if (FirstValue < array[j].GetParam(param))
                        {
                            FirstValue = array[j].GetParam(param);
                            cur_pos = j;
                        }
                    }
                    else
                        if (FirstValue > array[j].GetParam(param))
                        {
                            FirstValue = array[j].GetParam(param);
                            cur_pos = j;
                        }
                }
                temp = array[i];
                array[i] = array[cur_pos];
                array[cur_pos] = temp;

            }
            return array;
        }

    }
    interface ISort
    {
        int GetParam(int pos);
        string GetName();
        int GetID();
        UIElement GetElement(int pos);
    }
    
    enum SortVector { None, FromUp, FromDown }

    class NameLabelGrid : Grid
    {
        Polyline TopTriangle, LowTriangle;
        Label[] Primelbl = new Label[4];
        public SortVector Vector;
        public int Prime = 4;
        public int ColumnPos;
        public NameLabelGrid(string Text, int columnPos)
        {
            ColumnPos = columnPos;
            Width = 60;
            Height = 50;

            RowDefinitions.Add(new RowDefinition()); RowDefinitions.Add(new RowDefinition()); RowDefinitions.Add(new RowDefinition());
            ColumnDefinitions.Add(new ColumnDefinition()); ColumnDefinitions.Add(new ColumnDefinition()); ColumnDefinitions.Add(new ColumnDefinition());
            Viewbox vbx = new Viewbox();
            Label titlelabel = new Label();
            titlelabel.Content = Text;
            titlelabel.Foreground = Brushes.White;
            Children.Add(vbx);
            vbx.Margin = new Thickness(-2);
            Grid.SetColumnSpan(vbx, 3);
            vbx.Child = titlelabel;
            TopTriangle = GetTriangle(1, 0, true, 0);
            Children.Add(TopTriangle);
            TopTriangle.MouseDown += new MouseButtonEventHandler(VectorEllipse_MouseDown);
            LowTriangle = GetTriangle(2, 0, false, 1);
            Children.Add(LowTriangle);
            LowTriangle.MouseDown += new MouseButtonEventHandler(VectorEllipse_MouseDown);
            Primelbl[0] = GetLabel(1, 1, 10, 0);
            Children.Add(Primelbl[0]);
            Primelbl[0].MouseDown += new MouseButtonEventHandler(PrimeEllipse_MouseDown);
            Primelbl[1] = GetLabel(1, 2, 10, 1);
            Children.Add(Primelbl[1]);
            Primelbl[1].MouseDown += new MouseButtonEventHandler(PrimeEllipse_MouseDown);
            Primelbl[2] = GetLabel(2, 1, 10, 2);
            Children.Add(Primelbl[2]);
            Primelbl[2].MouseDown += new MouseButtonEventHandler(PrimeEllipse_MouseDown);
            Primelbl[3] = GetLabel(2, 2, 10, 3);
            Children.Add(Primelbl[3]);
            Primelbl[3].MouseDown += new MouseButtonEventHandler(PrimeEllipse_MouseDown);
        }

        void PrimeEllipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Label el = (Label)sender;
            int tag = (int)el.Tag;
            if (Prime == tag)
                Prime = 4;
            else
                Prime = tag;
            PrimeChanged();
        }
        void PrimeChanged()
        {
            for (int i = 0; i < 4; i++)
            {
                if (Prime == i)
                    Primelbl[i].Foreground = Brushes.Red;
                else
                    Primelbl[i].Foreground = Brushes.White;
            }
        }
        void VectorEllipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Polyline pol = (Polyline)sender;
            if (pol == TopTriangle)
                switch (Vector)
                {
                    case SortVector.None: Vector = SortVector.FromDown; break;
                    case SortVector.FromDown: Vector = SortVector.None; break;
                    case SortVector.FromUp: Vector = SortVector.FromDown; break;
                }
            else
            {
                switch (Vector)
                {
                    case SortVector.None: Vector = SortVector.FromUp; break;
                    case SortVector.FromUp: Vector = SortVector.None; break;
                    case SortVector.FromDown: Vector = SortVector.FromUp; break;
                }
            }
            VectorChanged();

        }
        public void VectorChanged()
        {
            switch (Vector)
            {
                case SortVector.None:
                    TopTriangle.Fill = Brushes.LightGray;
                    LowTriangle.Fill = Brushes.LightGray;
                    break;
                case SortVector.FromUp:
                    TopTriangle.Fill = Brushes.LightGray;
                    LowTriangle.Fill = Brushes.Green;
                    break;
                case SortVector.FromDown:
                    TopTriangle.Fill = Brushes.Green;
                    LowTriangle.Fill = Brushes.LightGray;
                    break;
            }
        }
        Polyline GetTriangle(int row, int column, bool isTop, int tag)
        {
            Polyline pol = new Polyline();
            if (isTop)
            {
                pol.Points.Add(new Point(0, 10));
                pol.Points.Add(new Point(5, 0));
                pol.Points.Add(new Point(10, 10));
            }
            else
            {
                pol.Points.Add(new Point(0, 0));
                pol.Points.Add(new Point(5, 10));
                pol.Points.Add(new Point(10, 0));
            }
            Grid.SetRow(pol, row);
            Grid.SetColumn(pol, column);

            pol.StrokeThickness = 1;
            pol.Stroke = Brushes.Black;
            pol.Fill = Brushes.LightGray;
            pol.Tag = tag;
            return pol;
        }
        Label GetLabel(int row, int column, int size, int tag)
        {
            Label lbl = new Label();
            Grid.SetRow(lbl, row);
            Grid.SetColumn(lbl, column);
            lbl.HorizontalAlignment = HorizontalAlignment.Center;
            lbl.VerticalAlignment = VerticalAlignment.Center;
            lbl.Content = tag + 1;
            lbl.Tag = tag;
            lbl.FontWeight = FontWeights.Bold;
            lbl.Foreground = Brushes.White;
            lbl.Margin = new Thickness(-4);
            return lbl;
        }
    }
}
