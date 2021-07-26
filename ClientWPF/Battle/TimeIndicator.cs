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
    class TimeIndicator : Canvas
    {
        DateTime StartTime;
        DateTime EndTime;
        public Battle Battle;
        System.Windows.Threading.DispatcherTimer Timer = new System.Windows.Threading.DispatcherTimer();
        bool PauseMode = false;
        public bool IsHaveTurns = false;
        TextBlock IndicatorText;
        public TimeIndicator()
        {
            //Rectangle Indicator = new Rectangle();
            Width = 90; Height = 60; Background = new SolidColorBrush(Color.FromArgb(153, 0, 255, 255));
            Canvas.SetLeft(this, 578);
            Canvas.SetTop(this, 55);

            IndicatorText = Common.GetBlock(60, "60");
            IndicatorText.Foreground = Brushes.White; IndicatorText.Width = 90; Children.Add(IndicatorText);
            //Canvas.SetLeft(IndicatorText, 578); 
            Canvas.SetTop(IndicatorText, -3);

            //FontSize = fontSize;
            //FontFamily = Links.Font;
            Timer.Interval = TimeSpan.FromSeconds(0.2);
            Timer.Tick += new EventHandler(Timer_Tick);
            //Foreground = Brushes.Green;
            //FontSize = 20;
            //Width = 80;
            //HorizontalAlignment = HorizontalAlignment.Center;
            //HorizontalContentAlignment = HorizontalAlignment.Center;
            SetValue(TimeSpan.FromSeconds(0));
        }
        public void Continue()
        {
            PauseMode = false;
            if (!Timer.IsEnabled) Timer_End();
        }
        public void Pause()
        {
            PauseMode = true;
        }
        void Timer_End() //таймер закончился
        {
            Timer.Stop(); //таймер останавливаем
            //SetValue(TimeSpan.FromSeconds(0));
            int moveslength = Battle.Moves == null ? 0 : Battle.Moves.Length; //число ходов уже имеющихся в бою
            Gets.ReadTurns(Battle); //считываем все ходы боя
            if (Battle.Moves[Battle.Moves.Length - 1].Array[0] == 50 && IntBoya.IsActive) //если последний ход - пятидесятый
            {
                if (Battle.IsFinished == true) return;
                Battle.IsFinished = true;
                if (BattleController.AutoWork == false) //и контроллер не работает
                    BattleController.StartWorking("Пятидесятый ход"); //запускаем контроллер
                IndicatorText.Text = "End"; //в индикатор пишем что всё
                IntBoya.SetAutoMode(); //запускаем автоматический режим
            }
            else if (moveslength == Battle.Moves.Length) //если число ходов не изменилось
            {
                Time(5); //то устанавливаем индикатор на 5 сек
            }
            else //если число ходов изменилось
            {
                //BattleCanvas.TurnsLabel.Content = String.Format("{0}/{1}/{2}", Battle.CurrentTurn, Battle.CurrentRound, Battle.Moves.Length);
                //BattleFieldCanvas.ConfirmStop.Color = Colors.Red;
                //BattleFieldCanvas.BTNConfirm.DeSelect(); //сбрасываем кнопку подтверждения
                short time = Gets.GetTurnEndTime(Battle); //считываем время до окончания хода
                if (BattleController.AutoWork == false && IntBoya.IsActive)  //если контроллер не работает
                    BattleController.StartWorking("Конец таймера"); //запускаем контроллер
                //DebugWindow.tb1.Text = "Пуск";
                if (time > 0) Time(time); //устанавливаем таймер на получившееся время, если он больше нуля
                else Time(5); //в противном случае устанавливаем на 5 сек
            }
        }
        void Timer_Tick(object sender, EventArgs e)
        {
            TimeSpan LeftTime = EndTime - DateTime.Now;
            if ((int)LeftTime.TotalSeconds <= 0)
            {
                SetValue(TimeSpan.FromSeconds(0));
                if (!PauseMode) Timer_End();
                return;
            }
            SetValue(LeftTime);
        }
        void SetValue(TimeSpan span)
        {
            if (IntBoya.CurMode == BattleMode.Mode1)
            {
                int seconds = span.Hours * 3600 + span.Minutes * 60 + span.Seconds;
                IndicatorText.Text = seconds.ToString();
            }
            else
            {
                IndicatorText.Text = "";
            }
                //Content = String.Format("{0:00}:{1:00}:{2:00}", span.Hours, span.Minutes, span.Seconds);
        }
        public void Time(double time)
        {
            StartTime = DateTime.Now;
            EndTime = StartTime + TimeSpan.FromSeconds(time);
                SetValue(TimeSpan.FromSeconds(time));
                if (!Timer.IsEnabled)
                    Timer.Start();

        }
        public void Stop()
        {
            Timer.Stop();
        }
    }
    /*
    class TimeIndicator : Label
    {
        DateTime StartTime;
        DateTime EndTime;
        public Battle Battle;
        System.Windows.Threading.DispatcherTimer Timer = new System.Windows.Threading.DispatcherTimer();
        bool PauseMode = false;
        public bool IsHaveTurns = false;
        public TimeIndicator(double fontSize)
        {
            FontSize = fontSize;
            FontFamily = Links.Font;
            Timer.Interval = TimeSpan.FromSeconds(0.2);
            Timer.Tick += new EventHandler(Timer_Tick);
            Foreground = Brushes.Green;
            FontSize = 20;
            Width = 80;
            HorizontalAlignment = HorizontalAlignment.Center;
            HorizontalContentAlignment = HorizontalAlignment.Center;
            SetValue(TimeSpan.FromSeconds(0));
        }
        public void Continue()
        {
            PauseMode = false;
            if (!Timer.IsEnabled) Timer_End();
        }
        public void Pause()
        {
            PauseMode = true;
        }
        void Timer_End() //таймер закончился
        {
            Timer.Stop(); //таймер останавливаем
            //SetValue(TimeSpan.FromSeconds(0));
            int moveslength = Battle.Moves == null ? 0 : Battle.Moves.Length; //число ходов уже имеющихся в бою
            Gets.ReadTurns(Battle); //считываем все ходы боя
            if (Battle.Moves[Battle.Moves.Length - 1].Command == 50 && BattleCanvas.IsActive) //если последний ход - пятидесятый
            {
                if (BattleController.AutoWork == false) //и контроллер не работает
                    BattleController.StartWorking("Пятидесятый ход"); //запускаем контроллер
                Content = Links.Interface("BattleEnd"); //в индикатор пишем что всё
                BattleCanvas.SetAutoMode(); //запускаем автоматический режим
            }
            else if (moveslength == Battle.Moves.Length) //если число ходов не изменилось
            {
                Time(5); //то устанавливаем индикатор на 5 сек
            }
            else //если число ходов изменилось
            {
                //BattleCanvas.TurnsLabel.Content = String.Format("{0}/{1}/{2}", Battle.CurrentTurn, Battle.CurrentRound, Battle.Moves.Length);
                BattleCanvas.Conflbl.Background = BattleCanvas.GetWhiteConfirmBrush(); //сбрасываем кнопку подтверждения
                short time = Gets.GetTurnEndTime(Battle); //считываем время до окончания хода
                if (BattleController.AutoWork == false && BattleCanvas.IsActive)  //если контроллер не работает
                    BattleController.StartWorking("Конец таймера"); //запускаем контроллер
                if (time > 0) Time(time); //устанавливаем таймер на получившееся время, если он больше нуля
                else Time(5); //в противном случае устанавливаем на 5 сек
            }
         }
        void Timer_Tick(object sender, EventArgs e)
        {
            TimeSpan LeftTime = EndTime - DateTime.Now;
            if ((int)LeftTime.TotalSeconds <= 0) 
            {
                SetValue(TimeSpan.FromSeconds(0));
                if (!PauseMode) Timer_End();
                return; 
            }
            SetValue(LeftTime);
        }
        void SetValue(TimeSpan span)
        {
            int seconds = span.Hours * 3600 + span.Minutes * 60 + span.Seconds;
            Content = seconds;
            //Content = String.Format("{0:00}:{1:00}:{2:00}", span.Hours, span.Minutes, span.Seconds);
        }
        public void Time(double time)
        {
            StartTime = DateTime.Now;
            EndTime = StartTime + TimeSpan.FromSeconds(time);
            SetValue(TimeSpan.FromSeconds(time));
            if (!Timer.IsEnabled)
                Timer.Start();
        }
    }
    */
}
