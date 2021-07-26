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

namespace Client
{
    enum DialogResult{Ok, Cancel};
    class ErrorMessage:Grid
    {
        DialogResult result;
        public static void Show(string Caption, string Text, bool OnlyOk)
        {
            ErrorMessage mess = new ErrorMessage(Caption, Text, OnlyOk);
            Links.Controller.PopUpCanvas.Place(mess, false);

        }
        public ErrorMessage(string Caption, string Text, bool OnlyOk)
        {
            Width = 400;
            Height = 300;
            RowDefinitions.Add(new RowDefinition());
            RowDefinitions.Add(new RowDefinition());
            RowDefinitions.Add(new RowDefinition());
            ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinitions.Add(new ColumnDefinition());
            Background = Brushes.Black;

            Label Captionlbl = new Label();
            Captionlbl.Style = Links.SmallTextStyleWhite;
            Captionlbl.Content = Caption;
            Children.Add(Captionlbl);
            Grid.SetColumnSpan(Captionlbl, 3);

            Label TextLabel = new Label();
            TextLabel.Style = Links.SmallTextStyleWhite;
            TextLabel.FontSize = 30;
            TextLabel.Content = Text;
            Children.Add(TextLabel);
            Grid.SetRow(TextLabel, 1);
            Grid.SetColumnSpan(TextLabel, 3);

  
            Button OkButton = new Button();
            Children.Add(OkButton);
            OkButton.Style = Links.ButtonStyle;
            OkButton.Content = "Ok";
            Grid.SetRow(OkButton, 2);
            Grid.SetColumn(OkButton, 1);
            OkButton.Click += new RoutedEventHandler(OkButton_Click);

            if (!OnlyOk)
            {
                Grid.SetColumn(OkButton, 0);

                Button CancelButton = new Button();
                Children.Add(CancelButton);
                CancelButton.Style = Links.ButtonStyle;
                CancelButton.Content = "Cancel";
                Grid.SetRow(CancelButton, 2);
                Grid.SetColumn(CancelButton, 2);
            }

            

        }

        void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Links.Controller.PopUpCanvas.Remove();
        }
    }
}
