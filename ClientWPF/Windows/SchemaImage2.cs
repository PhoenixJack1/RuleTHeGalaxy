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
    class SchemaImage2 : Border
    {
        Grid grid;
        Label NameLabel;
        Label HealthLabel;
        Label ShieldLabel;
        Label GeneratorLabel;
        Label OnShieldLabel;
        Label OnHealthLabel;
        StackPanel WeaponPanel;
        StackPanel EquipmentPanel;
        public Schema schema;
        Rectangle SelectRectangle;
        public SchemaImage2(Schema schema)
        {
            this.schema = schema;
            BorderThickness = new Thickness(4);
            BorderBrush = Brushes.Gold;
            Width = 220;
            Height = 200;

            SelectRectangle = new Rectangle();
            SelectRectangle.Width = 20; SelectRectangle.Height = 20;
            SelectRectangle.Fill = new ImageBrush(Links.PicsList["S001_Select"]);
            SelectRectangle.HorizontalAlignment = HorizontalAlignment.Left;
            SelectRectangle.VerticalAlignment = VerticalAlignment.Top;

            grid = new Grid();
            Child = grid;

            if (!schema.IsReal)
            {
                Background = new ImageBrush(Links.PicsList["S002_New"]);
                return;
            }
            grid.RowDefinitions.Add(new RowDefinition()); grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition()); grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.ShowGridLines = true;
            Background = Brushes.White;
            NameLabel = new Label();
            NameLabel.Background = Brushes.Black;
            NameLabel.Style = Links.SmallTextStyleWhite;

            StackPanel TopPanel = new StackPanel();
            grid.Children.Add(TopPanel);
            Grid.SetRow(TopPanel, 1);
            TopPanel.Orientation = Orientation.Horizontal;
            TopPanel.HorizontalAlignment = HorizontalAlignment.Center;

            ShieldLabel = new Label();
            ShieldLabel.Background = Brushes.Blue;
            ShieldLabel.Style = Links.SmallTextStyleWhite;
            TopPanel.Children.Add(ShieldLabel);

            GeneratorLabel = new Label();
            GeneratorLabel.Background = Brushes.Green;
            GeneratorLabel.Style = Links.SmallTextStyleWhite;
            TopPanel.Children.Add(GeneratorLabel);

            HealthLabel = new Label();
            HealthLabel.Background = Brushes.Red;
            HealthLabel.Style = Links.SmallTextStyleWhite;
            TopPanel.Children.Add(HealthLabel);

            WeaponPanel = new StackPanel();
            grid.Children.Add(WeaponPanel);
            Grid.SetRow(WeaponPanel, 2);
            WeaponPanel.Orientation = Orientation.Horizontal;
            WeaponPanel.HorizontalAlignment = HorizontalAlignment.Center;

            OnShieldLabel = new Label();
            OnShieldLabel.Background = Brushes.Blue;
            OnShieldLabel.Style = Links.SmallTextStyleWhite;

            OnHealthLabel = new Label();
            OnHealthLabel.Background = Brushes.Red;
            OnHealthLabel.Style = Links.SmallTextStyleWhite;

            grid.Children.Add(NameLabel);
            if (schema.GetShipType() != null)
            {
                NameLabel.Content = GetName(schema.GetShipType().Name, schema.GetName());
            }
            HealthLabel.Content = "HP:" + schema.ShipParams.BasicHealth;
            ShieldLabel.Content = "SC:" + schema.ShipParams.BasicShieldCapacity;
            GeneratorLabel.Content = "EC:" + schema.ShipParams.BasicEnergyCapacity;

            OnShieldLabel.Content = "SD:" + schema.ShipParams.BasicShieldDamage;
            OnHealthLabel.Content = "HD:" + schema.ShipParams.BasicHealthDamage;

            WeaponPanel.Children.Add(OnShieldLabel);
            for (int i = 0; i < schema.GetShipType().WeaponCapacity; i++)
            {
                Weaponclass weapon = schema.GetWeapon(i);
                if (weapon != null) WeaponPanel.Children.Add(GetWeaponRectangle(weapon.Type));
            }
            WeaponPanel.Children.Add(OnHealthLabel);

            EquipmentPanel = new StackPanel();
            EquipmentPanel.Orientation = Orientation.Horizontal;
            EquipmentPanel.HorizontalAlignment = HorizontalAlignment.Center;
            grid.Children.Add(EquipmentPanel);
            Grid.SetRow(EquipmentPanel, 3);
            for (int i = 0; i < schema.GetShipType().EquipmentCapacity; i++)
            {
                Equipmentclass equipment = schema.GetEquipment(i);
                if (equipment != null) EquipmentPanel.Children.Add(GetEquipmentRectangle(equipment.Type));
            }

            StackPanel PricePanel = schema.Price.GetStackPanel();
            grid.Children.Add(PricePanel);
            Grid.SetRow(PricePanel, 4);


        }
        public void Select()
        {
            if (!grid.Children.Contains(SelectRectangle)) grid.Children.Add(SelectRectangle);
        }
        public void DeSelect()
        {
            if (grid.Children.Contains(SelectRectangle)) grid.Children.Remove(SelectRectangle);
        }
        public static Rectangle GetWeaponRectangle(EWeaponType type)
        {
            Rectangle rect = new Rectangle();
            rect.Width = 25; rect.Height = 25;
            rect.Fill = Links.Brushes.WeaponBrushes[type];
            return rect;
        }
        public static Rectangle GetEquipmentRectangle(EquipmentType type)
        {
            Rectangle rect = new Rectangle();
            rect.Width = 25; rect.Height = 25;
            rect.Fill = Links.Brushes.EquipmentBrushes[type];
            rect.VerticalAlignment = VerticalAlignment.Center;
            return rect;
        }
        static SortedList<int, string> BaseRoman;
        static SortedList<int, char> Volumes;
        static string[] NameList;
        static string GetName(string typename, int name)
        {
            byte[] array = BitConverter.GetBytes(name);
            string result = typename;
            if (array[0] != 0 && array[0] < NameList.Length)
                result += " " + NameList[array[0]];
            if (array[1] != 0 && array[1] < Volumes.Count)
                result += " " + Volumes[array[1]].ToString();
            if (array[2] != 0)
                result += "-" + GetRoman(array[2]);
            if (array[3] != 0)
                result += "-" + array[3].ToString();
            return result;
        }
        public static void Prepare()
        {
            BaseRoman = new SortedList<int, string>();
            BaseRoman.Add(1, "I");
            BaseRoman.Add(4, "IV");
            BaseRoman.Add(5, "V");
            BaseRoman.Add(9, "IX");
            BaseRoman.Add(10, "X");
            BaseRoman.Add(40, "XL");
            BaseRoman.Add(50, "L");
            BaseRoman.Add(90, "XC");
            BaseRoman.Add(100, "C");
            NameList = new string[] { "", "Mk", "Type", "Var", "Ver", "Vol", "Group", "Param", "Obj" };
            Volumes = new SortedList<int, char>();
            for (int i = 1; i < 27; i++)
                Volumes.Add(i, (char)(i + 64));
            for (int i = 27; i < 53; i++)
                Volumes.Add(i, (char)(i + 70));
            for (int i = 53; i < 85; i++)
                Volumes.Add(i, (char)(i + 987));
            Volumes.Add(86, 'Ё');
            for (int i = 87; i < 119; i++)
                Volumes.Add(i, (char)(i + 985));
            Volumes.Add(120, 'ё');

        }
        static string GetRoman(byte k)
        {
            StringBuilder result = new StringBuilder();
            var neednumbers =
                from z in BaseRoman.Keys
                where z <= k
                orderby z descending
                select z;
            foreach (byte cur in neednumbers)
            {
                while ((k / cur) >= 1)
                {
                    k -= cur;
                    result.Append(BaseRoman[cur]);
                }
            }
            return result.ToString();
        }
    }
}
