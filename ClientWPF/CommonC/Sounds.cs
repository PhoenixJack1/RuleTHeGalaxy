using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;
using System.IO;
using System.Threading.Tasks;

namespace Client
{
    class MySound
    {
        public NAudio.Wave.WaveOut Wave;
        public string Path;
        static System.Threading.Tasks.TaskFactory factory = new TaskFactory();
        public MySound(string path)
        {
            if (Links.SoundVolume == 0f) return;
            Path = path;
            factory.StartNew(PlayA);
        }
        void PlayA()
        {
            System.Windows.Resources.StreamResourceInfo res =
                System.Windows.Application.GetResourceStream(
                    new Uri(String.Format("pack://application:,,,/SoundLibrary;component/Sounds/{0}", Path)));
            if (Path.Substring(Path.Length - 4) == ".mp3")
            {
                //using (var ms = System.IO.File.OpenRead(Path))
                using (var rdr = new NAudio.Wave.Mp3FileReader(res.Stream))
                using (var wavStream = NAudio.Wave.WaveFormatConversionStream.CreatePcmStream(rdr))
                using (var baStream = new NAudio.Wave.BlockAlignReductionStream(wavStream))
                using (Wave = new NAudio.Wave.WaveOut(NAudio.Wave.WaveCallbackInfo.FunctionCallback()))
                {
                    Wave.Init(baStream);
                    Wave.Volume = Links.SoundVolume;
                    Wave.Play();
                    while (Wave.PlaybackState == NAudio.Wave.PlaybackState.Playing)
                    {
                        System.Threading.Thread.Sleep(1000);
                    }
                }
            }
            else
            {
                using (var rdr = new NAudio.Wave.WaveFileReader(res.Stream)) //new NAudio.Wave.AudioFileReader(Path))
                using (Wave = new NAudio.Wave.WaveOut(NAudio.Wave.WaveCallbackInfo.FunctionCallback()))
                {
                    Wave.Init(rdr);
                    Wave.Volume = Links.SoundVolume;
                    Wave.Play();
                    while (Wave.PlaybackState == NAudio.Wave.PlaybackState.Playing)
                    {
                        System.Threading.Thread.Sleep(1000);
                    }
                }
            }
        }
    }
    enum MusicType { Strategy, Battle }
    class MyMusic
    {
        public NAudio.Wave.WaveOut Wave;
        public string Path;
        public bool IsPlaying = false;
        public float Volume;
        static System.Threading.Tasks.TaskFactory factory = new TaskFactory();
        public bool forcestop = false;
        public MyMusic(string path)
        {
            Path = path;
            Volume = Links.MusicVolume;
            Play();
        }
        void Play()
        {
            IsPlaying = true;
            factory.StartNew(PlayA);
        }
        void PlayA()
        {
            System.Windows.Resources.StreamResourceInfo res =
                System.Windows.Application.GetResourceStream(
                    new Uri(String.Format("pack://application:,,,/SoundLibrary;component/Music/{0}", Path)));
            using (var ms = res.Stream)//System.IO.File.OpenRead(Path))
            using (var rdr = new NAudio.Wave.Mp3FileReader(ms))
            using (var wavStream = NAudio.Wave.WaveFormatConversionStream.CreatePcmStream(rdr))
            using (var baStream = new NAudio.Wave.BlockAlignReductionStream(wavStream))
            using (Wave = new NAudio.Wave.WaveOut(NAudio.Wave.WaveCallbackInfo.FunctionCallback()))
            {
                Wave.Init(baStream);
                Wave.Volume = Volume;
                Wave.Play();
                while (Wave.PlaybackState == NAudio.Wave.PlaybackState.Playing)
                {
                    System.Threading.Thread.Sleep(1000);
                }
                IsPlaying = false;
                if (forcestop == false) Music.Play();
            }
        }



    }
    class Music
    {
        public static MyMusic CurMusic;
        public static MusicType CurType = MusicType.Strategy;
        static string[] StrategyMusic = new string[] { "Global/Zastavka.mp3" };
        static int Strategypos = 0;
        static string[] BattleMusic = new string[] { "Battle/BattleTheme1.mp3","Battle/BattleTheme2.mp3","Battle/BattleTheme3.mp3" };
        static int Battlepos = DateTime.Now.Millisecond % BattleMusic.Length;
        public static bool IsChanging = false;
        public static void Play()
        {
            //запуск музыки
            //если громкость нулевая - то ничего не делаем
            if (Links.MusicVolume == 0f) return;
            //медленно запускаем музыку в соответствии с типом
            if (CurType == MusicType.Strategy)
                PlayStrategy();
            else
                PlayBattle("Start");
        }
        static int pos = 0;
        static void PlayBattle(string reason)
        {
            string music = BattleMusic[Battlepos];
            Battlepos++; Battlepos = Battlepos % BattleMusic.Length;
            CurMusic = new MyMusic(music);
            pos++;
            //Links.Controller.mainWindow.Title = pos.ToString();
        }
        static void PlayStrategy()
        {
            string music = StrategyMusic[Strategypos];
            Strategypos++; Strategypos = Strategypos % StrategyMusic.Length;
            CurMusic = new MyMusic(music);
            pos++;
            //Links.Controller.mainWindow.Title = pos.ToString();
        }
        public static void ChangeVolume(float volume)
        {
            if (Links.MusicVolume == volume) return;
            if (volume == 0f)
            {
                Links.MusicVolume = volume;
                if (IsChanging)
                    CurMusic.IsPlaying = false;
                else
                {
                    CurMusic.Volume = volume;
                    CurMusic.Wave.Stop();
                }
            }
            else if (Links.MusicVolume != 0f)
            {
                Links.MusicVolume = volume;
                CurMusic.Volume = volume;
                CurMusic.Wave.Volume = volume;
            }
            else
            {
                Links.MusicVolume = volume;
                Play();
            }
        }
        public static void ChangeType(MusicType type)
        {
            if (type == CurType) return;
            //Надо медленно остановить текущую и запустить новую
            IsChanging = true;
            CurType = type;
            if (Links.MusicVolume == 0f) return;
            SlowChange();

        }
        static void SlowChange()
        {
            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.3);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private static void Timer_Tick(object sender, EventArgs e)
        {
            if (CurMusic.IsPlaying == false)
            {
                ((System.Windows.Threading.DispatcherTimer)sender).Stop();
                if (Links.MusicVolume != 0)
                {
                    if (CurType == MusicType.Strategy)
                        PlayStrategy();
                    else
                        PlayBattle("First"+pos);
                }
                IsChanging = false;
            }
            else if (CurMusic.Volume >= 0.1f)
            {
                CurMusic.Volume -= 0.1f;
                CurMusic.Wave.Volume = CurMusic.Volume;
            }
            else
            {
                CurMusic.forcestop = true;
                CurMusic.Wave.Stop();
                CurMusic.IsPlaying = false;
                ((System.Windows.Threading.DispatcherTimer)sender).Stop();
                if (CurType == MusicType.Strategy)
                    PlayStrategy();
                else
                    PlayBattle("Second"+pos);
                IsChanging = false;
            }

        }


    }
}
