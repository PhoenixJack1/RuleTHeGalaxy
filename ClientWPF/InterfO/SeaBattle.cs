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
    enum SeaBattleStatus { Not_In_Game, Waiting_Player, Playing, Server_Error } 
    class SeaBattle:Canvas
    {
        public SeaBattle()
        {
            Width = 1024;
            Height = 768;
            Background = Brushes.LightBlue;
            //SeaBattleStatus SBStatus = Gets.GetSeaBattleStatus();
            //switch (SBStatus)
            //{
            //    case SeaBattleStatus.Not_In_Game: Set_Not_In_Game_Window(); break;
            //    case SeaBattleStatus.Waiting_Player: //Окно должно переключится на индикатор ожидания и опрашивать сервер
            //        break;
            //}
            AddCloseButton();
        }
        #region Not_in_Start_Game
        void Set_Not_In_Game_Window()
        {
            Children.Clear();
            AddStartButton();
            AddCloseButton();
        }
        void Send_Request_For_Game_Start()
        {
            Events.SendRequestToStartSeaBattle();
        }
        #endregion
        #region Buttons
        void AddStartButton()
        {
            AddButton("Start Sea Battle", "start", 500, 400, new RoutedEventHandler(btn_Click));
        }
        void AddCloseButton()
        {
            AddButton("Close", "close", 900, 700, new RoutedEventHandler(btn_Click));
        }
        Button AddButton(string content, string tag, int left, int top, RoutedEventHandler btn_event)
        {
            Button btn = new Button();
            btn.Background = Brushes.Blue;
            btn.Content = content;
            btn.Tag = tag;
            Children.Add(btn);
            Canvas.SetLeft(btn, left);
            Canvas.SetTop(btn, top);
            btn.FontWeight = FontWeights.Bold;
            btn.FontSize = 20;
            btn.Foreground = Brushes.White;
            btn.Click += btn_event;
            return btn;
        }
        void btn_Click(object sender, RoutedEventArgs e)
        {
            string tag = (string)((Button)sender).Tag;
            switch (tag)
            {
                case "close": Links.Controller.PopUpCanvas.Remove(); break;
                case "start": Send_Request_For_Game_Start(); break;
            }
        }
        #endregion
    }
}
