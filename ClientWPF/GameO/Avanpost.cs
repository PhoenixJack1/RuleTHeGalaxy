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
    public class Avanpost
    {
        public int ID; //ID территории
        public GSString Name; //Название территории
        public int PlanetID; //ID планеты, на которой расположена территория
        public GSPlanet Planet; //ссылка на планету, на которой расположена территория
        public LandAdd NeedResources;
        public LandAdd HaveResources;
        public bool Pillage;
        public bool Conquer;
        public byte RiotIndicator;
        public Avanpost(int id, int planetid, GSString name, LandAdd need, LandAdd have, bool pillage, bool conquer, byte riotindicator)
        {
            ID = id; Name = name; PlanetID = planetid; Planet = Links.Planets[PlanetID];
            //LandLevel = Planet.LandLevel; LandSize = Planet.LandSize;
            NeedResources = need;
            HaveResources = have;
            Pillage = pillage; Conquer = conquer; RiotIndicator = riotindicator;
        }
        public static Avanpost GetLand(byte[] array, ref int s)
        {
            int id = BitConverter.ToInt32(array, s);
            int planetid = BitConverter.ToInt32(array, s + 4);
            int playerid = BitConverter.ToInt32(array, s + 8);
            s += 12;
            GSString name = new GSString(array, s);
            s += name.Array.Length;
            LandAdd need = new LandAdd();
            need.Money = BitConverter.ToInt32(array, s); s += 4;
            need.Metall = BitConverter.ToInt32(array, s); s += 4;
            need.Chips = BitConverter.ToInt32(array, s); s += 4;
            need.Anti = BitConverter.ToInt32(array, s); s += 4;
            LandAdd have = new LandAdd();
            have.Money = BitConverter.ToInt32(array, s); s += 4;
            have.Metall = BitConverter.ToInt32(array, s); s += 4;
            have.Chips = BitConverter.ToInt32(array, s); s += 4;
            have.Anti = BitConverter.ToInt32(array, s); s += 4;
            bool Pillage = array[s] > 0; s++;
            bool Conquer = array[s] > 0; s++;
            byte RiotIndicator = array[s]; s++;
            Avanpost land = new Avanpost(id, planetid, name, need, have, Pillage, Conquer, RiotIndicator);
            
            return land;
        }
        /// <summary> Метод создаёт панель с краткой информацией об авванпосте </summary>
        /// <param name="needevents">показывает, нужен ли эвент при клике с открытием аванпоста</param>
        public Border GetShortInfo(bool needevents, int width)
        {
            LandChanger.PlanetInfo info = new LandChanger.PlanetInfo(0, Planet, ID);
            info.Width = 300; info.Height = 300;
            info.Show();
            Border border = new Border();
            border.Width = width; border.Height = 300;
            border.Child = info;
            /*
            Border border = new Border(); border.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 168, 255));
            border.Width = 300; border.Height = 200;
            border.BorderThickness = new Thickness(2); border.CornerRadius = new CornerRadius(20);
            border.Background = Brushes.Black;

            Canvas canvas = new Canvas();
            canvas.Width = 300; canvas.Height = 200;
            border.Child = canvas;

            TextBlock Title = Common.GetBlock(30, Name.ToString(), Brushes.White, 300);
            canvas.Children.Add(Title); Canvas.SetTop(Title, 5);

            Canvas AddCanvas = GetAddResourceCanvas(new Brush[] { Links.Brushes.MoneyImageBrush, Links.Brushes.MetalImageBrush, Links.Brushes.ChipsImageBrush, Links.Brushes.AntiImageBrush },
                new Brush[] { Brushes.Green, Brushes.Gray, Brushes.Blue, Brushes.Red },
                new int[] { NeedResources.Money, NeedResources.Metall, NeedResources.Chips, NeedResources.Anti }, 
                new int[] { HaveResources.Money, HaveResources.Metall, HaveResources.Chips, HaveResources.Anti});
            canvas.Children.Add(AddCanvas);
            Canvas.SetLeft(AddCanvas, 10); Canvas.SetTop(AddCanvas, 40);
            double percent = (double)HaveResources.Money / NeedResources.Money + (double)HaveResources.Metall / NeedResources.Metall +
                (double)HaveResources.Chips / NeedResources.Chips + (double)HaveResources.Anti / NeedResources.Anti;
            percent = Math.Round(percent / 4 * 100,0);
            TextBlock percentblock = Common.GetBlock(55, percent.ToString()+"%", Brushes.White, 90);
            canvas.Children.Add(percentblock);  Canvas.SetLeft(percentblock, 210); Canvas.SetTop(percentblock, 70);
    */        
    if (needevents)
                border.PreviewMouseDown += Border_PreviewMouseDown;
            border.Tag = this;
            return border;

        }
        public int GetBuildPercent()
        {
            int percent = (HaveResources.Money / 10 + HaveResources.Metall + HaveResources.Chips + HaveResources.Anti) * 100 /
                        (NeedResources.Money / 10 + NeedResources.Metall + NeedResources.Chips + NeedResources.Anti);
            return percent;
        }
        private void Border_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Colonies.CurrentPlanet = Planet;
            //Colonies.CurrentLand = null;
            //Colonies.CurrentAvanpost = this;
            Links.Controller.SelectPanel(GamePanels.Colonies, SelectModifiers.None);
        }
        public bool CheckArtefactAvailable(Artefact art)
        {
            switch (art.Ability)
            {
                case ArtefactAbility.PColonyCreate: if (Planet.MaxPopulation <= art.Param1) return true; else return false;
                default: return false;
            }
        }
        Canvas GetAddResourceCanvas(Brush[] brushes, Brush[] Colors, int[] need, int[] have)
        {
            Canvas canvas = new Canvas();
            canvas.Width = 200; canvas.Height = 150;
            Rectangle rect = new Rectangle(); rect.Width = 200; rect.Height = 150; rect.Stroke = Brushes.White; rect.StrokeThickness = 2;
            canvas.Children.Add(rect);
            double max = need[0] / 10;
            for (int i = 1; i < 4; i++)
                if (need[i] > max) max = need[i];
            for (int i = 0; i < brushes.Length; i++)
            {
                Rectangle cub1 = new Rectangle(); cub1.Height = 30; cub1.Width = need[i] * 150.0 / max / (i == 0 ? 10 : 1);
                if (cub1.Width > 148) cub1.Width = 148; else if (cub1.Width < 3) cub1.Width = 3;
                cub1.Stroke = Colors[i]; canvas.Children.Add(cub1); Canvas.SetLeft(cub1, 35); Canvas.SetTop(cub1, i*35+10);
                Rectangle cub2 = new Rectangle(); cub2.Height = 30; cub2.Width = have[i] * 150.0 / max / (i == 0 ? 10 : 1);
                cub2.Fill = Colors[i]; canvas.Children.Add(cub2); Canvas.SetLeft(cub2, 35); Canvas.SetTop(cub2, i*35+10);
                Rectangle img = Common.GetRectangle(20, brushes[i]); canvas.Children.Add(img); Canvas.SetLeft(img, 5); Canvas.SetTop(img, i*35+15);
            }
            return canvas;
        }
    }
    class NPCLand
    {

    }
}
