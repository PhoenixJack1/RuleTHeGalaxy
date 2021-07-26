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
    class SchemaCombineGrid:Grid
    {
        enum ButtonsEnum { ShipType, ShipName, Generator, Shield, Engine, Computer, Weapon1, Weapon2, Weapon3, Equipment1, Equipment2, Equipment3, Equipment4 };
        enum ItemsE {Param, Damage, Accuracy, Evasion, Price, Health, Shield, ShieldRe, Gen, GenRe, HealthD, ShieldD, ActiveSpent, 
            AccEn, AccPh, AccIr, AccCy, EvaEn, EvaPh, EvaIr, EvaCy, Money, Metal, Chips, Anti };
        public Schema schema;
        SortedList<ButtonsEnum, Button> Buttons;
        SortedList<ItemsE, TreeViewItem> TItems;
        Border ShipTypeBorder;
        Border ShipParamBorder;
        Border WeaponBorder;
        Border EquipmentBorder;
       
        
        public SchemaCombineGrid(Schema schema)
        {
            this.schema = schema;
            Create();
            Refresh();
        }
        public void Refresh()
        {
            ShipParamBorder.Visibility = Visibility.Hidden;
            WeaponBorder.Visibility = Visibility.Hidden;
            EquipmentBorder.Visibility = Visibility.Hidden;
            for (int i = 6; i < 13; i++)
                Buttons[(ButtonsEnum)i].Visibility = Visibility.Collapsed;
            foreach (TreeViewItem item in TItems.Values)
                item.Visibility = Visibility.Collapsed;
            ShipTypeclass shiptype = schema.GetShipType();
            if (shiptype == null) return;
            Buttons[ButtonsEnum.ShipType].Content = shiptype.Name;
            if (schema.GetName() != 0)

                Buttons[ButtonsEnum.ShipName].Content = shiptype.Name + " " + SelectSchemaNameCanvas.GetNameResult(schema.GetName());
            else
                Buttons[ButtonsEnum.ShipName].Content = "Select Name";
            
            ShipParamBorder.Visibility = Visibility.Visible;
            if (schema.GetGenerator() != null)
                Buttons[ButtonsEnum.Generator].Content = schema.GetGenerator().Name;
            else
                Buttons[ButtonsEnum.Generator].Content = "Select Generator Size " + ((Run)Common.GetItemSizeInline(shiptype.GeneratorSize)).Text;
            if (schema.GetShield() != null)
                Buttons[ButtonsEnum.Shield].Content = schema.GetShield().Name;
            else
                Buttons[ButtonsEnum.Shield].Content = "Select Shield Size " + ((Run)Common.GetItemSizeInline(shiptype.ShieldSize)).Text;
            if (schema.GetComputer() != null)
                Buttons[ButtonsEnum.Computer].Content = schema.GetComputer().Name;
            else
                Buttons[ButtonsEnum.Computer].Content = "Select Computer Size " + ((Run)Common.GetItemSizeInline(shiptype.ComputerSize)).Text;
            if (schema.GetEngine() != null)
                Buttons[ButtonsEnum.Engine].Content = schema.GetEngine().Name;
            else
                Buttons[ButtonsEnum.Engine].Content = "Select Engine Size " + ((Run)Common.GetItemSizeInline(shiptype.EngineSize)).Text;

            WeaponBorder.Visibility = Visibility.Visible;
            for (int i=0;i<3;i++)
                if (shiptype.WeaponCapacity > i)
                {
                    Buttons[(ButtonsEnum)(i+6)].Visibility = Visibility.Visible;
                    if (schema.GetWeapon(i) != null)
                        Buttons[(ButtonsEnum)(i+6)].Content = schema.GetWeapon(i).Name;
                    else
                        Buttons[(ButtonsEnum)(6+i)].Content = "Select " + Common.GetNumber(i) + " Weapon " + shiptype.WeaponsParam[i].Group + " " + shiptype.WeaponsParam[i].Size;
                }
            EquipmentBorder.Visibility = Visibility.Visible;
            for (int i=0;i<4;i++)
                if (shiptype.EquipmentCapacity > i)
                {
                    Buttons[(ButtonsEnum)(9+i)].Visibility = Visibility.Visible;
                    if (schema.GetEquipment(i) != null)
                        Buttons[(ButtonsEnum)(9+i)].Content = schema.GetEquipment(i).Name;
                    else
                        Buttons[(ButtonsEnum)(9+i)].Content = "Select " + Common.GetNumber(i) + " Equipment " + shiptype.EquipmentsSize[i];
                }
            TItems[ItemsE.Param].Visibility = Visibility.Visible;
            TItems[ItemsE.Damage].Visibility = Visibility.Visible;
            TItems[ItemsE.Accuracy].Visibility = Visibility.Visible;
            TItems[ItemsE.Evasion].Visibility = Visibility.Visible;
            TItems[ItemsE.Price].Visibility = Visibility.Visible;
            schema.Calculate();
            PutItemValue(schema.ShipParams.BasicHealth, ItemsE.Health, "Health");
            PutItemValue(schema.ShipParams.BasicShieldCapacity, ItemsE.Shield, "Shield");
            PutItemValue(schema.ShipParams.BasicShieldRecharge, ItemsE.ShieldRe, "Shield Rechatge");
            PutItemValue(schema.ShipParams.BasicEnergyCapacity, ItemsE.Gen, "Energy");
            PutItemValue(schema.ShipParams.BasicEnergyRecharge, ItemsE.GenRe, "Energy Recharge");
            PutItemValue(schema.ShipParams.BasicShieldDamage, ItemsE.ShieldD, "Damage To Shield");
            PutItemValue(schema.ShipParams.BasicHealthDamage, ItemsE.HealthD, "Damage To Health");
            PutItemValue(schema.ShipParams.BasicEnergyActiveSpend, ItemsE.ActiveSpent, "Energy Spent In Turn");
            PutItemValue(schema.ShipParams.BasicEnergyAccuracy, ItemsE.AccEn, "Energy Accuracy");
            PutItemValue(schema.ShipParams.BasicPhysicAccuracy, ItemsE.AccPh, "Physic Accuracy");
            PutItemValue(schema.ShipParams.BasicIrregularAccuracy, ItemsE.AccIr, "Irregular Accuracy");
            PutItemValue(schema.ShipParams.BasicCyberAccuracy, ItemsE.AccCy, "Cyber Accuracy");
            PutItemValue(schema.ShipParams.BasicEnergyEvasion, ItemsE.EvaEn, "Energy Evasion");
            PutItemValue(schema.ShipParams.BasicPhysicEvasion, ItemsE.EvaPh , "Physic Evasion");
            PutItemValue(schema.ShipParams.BasicIrregularEvasion, ItemsE.EvaIr, "Irregular Evasion");
            PutItemValue(schema.ShipParams.BasicCyberEvasion, ItemsE.EvaCy, "Cyber Evasion");
            PutItemValue(schema.Price.Money, ItemsE.Money, "MoneY");
            PutItemValue(schema.Price.Metall, ItemsE.Metal, "Metal");
            PutItemValue(schema.Price.Chips, ItemsE.Chips, "Chips");
            PutItemValue(schema.Price.Anti, ItemsE.Anti, "Anti");
        }
        void PutItemValue(int value, ItemsE item, string text)
        {
            if (value != 0)
            {
                string skip = "\t\t\t";
                if (text.Length > 7 ) skip = "\t\t"; if (text.Length > 14) skip = "\t";
                TItems[item].Visibility = Visibility.Visible;
                TItems[item].Header = text + ": "+skip+ + value;
            }
        }
        public void Create()
        {
            ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinitions.Add(new ColumnDefinition());
            ScrollViewer textviewer = new ScrollViewer();
            Children.Add(textviewer);
            Grid.SetColumn(textviewer, 1);
            textviewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            textviewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;

            ScrollViewer buttonviewer = new ScrollViewer();
            Children.Add(buttonviewer);
            buttonviewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            buttonviewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;

            StackPanel ButtonPanel = new StackPanel();
            buttonviewer.Content = ButtonPanel;

            ShipTypeBorder = new Border();
            ShipTypeBorder.BorderBrush = Brushes.Red;
            ShipTypeBorder.BorderThickness = new Thickness(2);
            ShipTypeBorder.Margin = new Thickness(5);
            ButtonPanel.Children.Add(ShipTypeBorder);

            StackPanel ShipTypePanel = new StackPanel();
            ShipTypeBorder.Child = ShipTypePanel;

            Buttons = new SortedList<ButtonsEnum, Button>();

            Buttons.Add(ButtonsEnum.ShipType,PutButton("Ship Type", ShipTypePanel, 0, 0));
            Buttons[ButtonsEnum.ShipType].Click += new RoutedEventHandler(ShipTypeButton_Click);
            Buttons.Add(ButtonsEnum.ShipName, PutButton("Schema Name", ShipTypePanel, 0, 1));
            Buttons[ButtonsEnum.ShipName].Click += new RoutedEventHandler(ShipNameButton_Click);

            ShipParamBorder = new Border();
            ShipParamBorder.BorderThickness = new Thickness(2);
            ShipParamBorder.BorderBrush = Brushes.Blue;
            ShipParamBorder.Margin = new Thickness(5);
            ButtonPanel.Children.Add(ShipParamBorder);

            StackPanel ShipParamPanel = new StackPanel();
            ShipParamBorder.Child = ShipParamPanel;

            Buttons.Add(ButtonsEnum.Generator, PutButton("Select Generator", ShipParamPanel, 0, 0));
            Buttons[ButtonsEnum.Generator].Click += new RoutedEventHandler(GeneratorButton_Click);
            Buttons.Add(ButtonsEnum.Shield, PutButton("Select Shield", ShipParamPanel, 0, 1));
            Buttons[ButtonsEnum.Shield].Click += new RoutedEventHandler(ShieldButton_Click);
            Buttons.Add(ButtonsEnum.Computer, PutButton("Select Computer", ShipParamPanel, 1, 0));
            Buttons[ButtonsEnum.Computer].Click += new RoutedEventHandler(ComputerButton_Click);
            Buttons.Add(ButtonsEnum.Engine, PutButton("Select Engine", ShipParamPanel, 1, 1));
            Buttons[ButtonsEnum.Engine].Click += new RoutedEventHandler(EngineButton_Click);

            WeaponBorder = new Border();
            WeaponBorder.BorderThickness = new Thickness(2);
            WeaponBorder.BorderBrush = Brushes.Gold;
            WeaponBorder.Margin = new Thickness(5);
            ButtonPanel.Children.Add(WeaponBorder);

            StackPanel WeaponPanel = new StackPanel();
            WeaponBorder.Child = WeaponPanel;
            //WeaponButtons = new Button[3];
            for (int i = 0; i < 3; i++)
            {
                int column = i % 2;
                int row = (i - column) / 2;
                Buttons.Add((ButtonsEnum)(i + 6), PutButton("Select " + Common.GetNumber(i) + " weapon", WeaponPanel, row, column));
                Buttons[(ButtonsEnum)(i+6)].Tag = i;
                Buttons[(ButtonsEnum)(i+6)].Click += new RoutedEventHandler(WeaponButton_Click);
            }
            EquipmentBorder = new Border();
            EquipmentBorder.BorderThickness = new Thickness(2);
            EquipmentBorder.BorderBrush = Brushes.Gold;
            EquipmentBorder.Margin = new Thickness(5);
            ButtonPanel.Children.Add(EquipmentBorder);

            StackPanel EquipmentPanel = new StackPanel();
            EquipmentBorder.Child = EquipmentPanel;
            //EquipmentButtons = new Button[4];
            for (int i = 0; i < 4; i++)
            {
                int column = i % 2;
                int row = (i - column) / 2;
                Buttons.Add((ButtonsEnum)(i+9),  PutButton("Select " + Common.GetNumber(i) + " equipment", EquipmentPanel, row, column));
                Buttons[(ButtonsEnum)(i+9)].Tag = i;
                Buttons[(ButtonsEnum)(i+9)].Click += new RoutedEventHandler(EquipmentButton_Click);
            }

            TItems = new SortedList<ItemsE, TreeViewItem>();
            TreeView ParamsTreeView = new TreeView();
            Children.Add(ParamsTreeView);
            Grid.SetColumn(ParamsTreeView, 1);
            ParamsTreeView.Background = Brushes.Black;
            ParamsTreeView.FontWeight = FontWeights.Bold;

            TItems.Add(ItemsE.Param, new TreeViewItem());
            TItems[ItemsE.Param].Header = "Базовые параметры";
            TItems[ItemsE.Param].Foreground = Brushes.White;
            TItems[ItemsE.Param].IsExpanded = true;

            TItems.Add(ItemsE.Health, PutItem("Health:", ItemsE.Param));
            TItems.Add(ItemsE.Shield, PutItem("Shield:", ItemsE.Param));
            TItems.Add(ItemsE.ShieldRe, PutItem("Shield Recharge:", ItemsE.Param));
            TItems.Add(ItemsE.Gen, PutItem("Energy:", ItemsE.Param));
            TItems.Add(ItemsE.GenRe, PutItem("Energy Recharge:", ItemsE.Param));

            TItems.Add(ItemsE.Damage, new TreeViewItem());
            TItems[ItemsE.Damage].Header = "Параметры урона";
            TItems[ItemsE.Damage].Foreground = Brushes.White;
            TItems[ItemsE.Damage].IsExpanded = true;

            TItems.Add(ItemsE.ShieldD, PutItem("Damage to Shield:", ItemsE.Damage));
            TItems.Add(ItemsE.HealthD, PutItem("Damage To Health:", ItemsE.Damage));
            TItems.Add(ItemsE.ActiveSpent, PutItem("Energy Spent:", ItemsE.Damage));

            TItems.Add(ItemsE.Accuracy, new TreeViewItem());
            TItems[ItemsE.Accuracy].Header = "Точность";
            TItems[ItemsE.Accuracy].Foreground = Brushes.White;
            TItems[ItemsE.Accuracy].IsExpanded = true;

            TItems.Add(ItemsE.AccEn, PutItem("Energy Accuracy:", ItemsE.Accuracy));
            TItems.Add(ItemsE.AccPh, PutItem("Physic Accuracy:", ItemsE.Accuracy));
            TItems.Add(ItemsE.AccIr, PutItem("Irregular Accuracy:", ItemsE.Accuracy));
            TItems.Add(ItemsE.AccCy, PutItem("Cyber Accuracy:", ItemsE.Accuracy));

            TItems.Add(ItemsE.Evasion, new TreeViewItem());
            TItems[ItemsE.Evasion].Header = "Уклонение";
            TItems[ItemsE.Evasion].Foreground = Brushes.White;
            TItems[ItemsE.Evasion].IsExpanded = true;

            TItems.Add(ItemsE.EvaEn, PutItem("Energy Evasion:", ItemsE.Evasion));
            TItems.Add(ItemsE.EvaPh, PutItem("Physic Evasion:", ItemsE.Evasion));
            TItems.Add(ItemsE.EvaIr, PutItem("Irregular Evasion:", ItemsE.Evasion));
            TItems.Add(ItemsE.EvaCy, PutItem("Cyber Evasion:", ItemsE.Evasion));

            TItems.Add(ItemsE.Price, new TreeViewItem());
            TItems[ItemsE.Price].Header = "Цена";
            TItems[ItemsE.Price].Foreground = Brushes.White;
            TItems[ItemsE.Price].IsExpanded = true;
            
            TItems.Add(ItemsE.Money,PutItem("Money:",ItemsE.Price));
            TItems.Add(ItemsE.Metal, PutItem("Metals:", ItemsE.Price));
            TItems.Add(ItemsE.Chips, PutItem("Chips:", ItemsE.Price));
            TItems.Add(ItemsE.Anti, PutItem("Anti:", ItemsE.Price));

            ParamsTreeView.Items.Add(TItems[ItemsE.Param]);
            ParamsTreeView.Items.Add(TItems[ItemsE.Damage]);
            ParamsTreeView.Items.Add(TItems[ItemsE.Accuracy]);
            ParamsTreeView.Items.Add(TItems[ItemsE.Evasion]);
            ParamsTreeView.Items.Add(TItems[ItemsE.Price]);
        }

        void EquipmentButton_Click(object sender, RoutedEventArgs e)
        {
            ShipTypeclass shiptype = schema.GetShipType();
            Button btn = (Button)sender;
            int tag = (int)btn.Tag;
            List<Equipmentclass> list = new List<Equipmentclass>();
            foreach (Equipmentclass equipment in Links.EquipmentTypes.Values)
                if (GSGameInfo.SciencesList.SciencesArray.ContainsKey((ushort)equipment.ID) && shiptype.EquipmentsSize[tag] >= equipment.Size)
                    list.Add(equipment);
            ApplyEnum apply;
            switch (tag)
            {
                case 0: apply = ApplyEnum.Equipment1; break;
                case 1: apply = ApplyEnum.Equipment2; break;
                case 2: apply = ApplyEnum.Equipment3; break;
                default: apply = ApplyEnum.Equipment4; break;
            }
            SelectItemCanvas canvas = new SelectItemCanvas(list.ToArray(), Equipmentclass.GetEquipmentColumnNames(), new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 }, "Выберите оборудование", apply);
            Links.Controller.PopUpCanvas.Place(canvas, false);
        }
        void WeaponButton_Click(object sender, RoutedEventArgs e)
        {
            ShipTypeclass shiptype = schema.GetShipType();
            Button btn = (Button)sender;
            int tag = (int)btn.Tag;
            List<Weaponclass> list = new List<Weaponclass>();
            foreach (Weaponclass weapon in Links.WeaponTypes.Values)
                if (GSGameInfo.SciencesList.SciencesArray.ContainsKey((ushort)weapon.ID) && 
                    shiptype.WeaponsParam[tag].Size >= weapon.Size && 
                    weapon.CheckWeaponGroup(shiptype.WeaponsParam[tag].Group))
                    list.Add(weapon);
            ApplyEnum apply;
            switch (tag) { case 0: apply = ApplyEnum.Weapon1; break; case 1: apply = ApplyEnum.Weapon2; break; default: apply = ApplyEnum.Weapon3; break; }
            SelectItemCanvas canvas = new SelectItemCanvas(list.ToArray(), Weaponclass.GetWeaponColumnNames(), new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 }, "Выберите оружие", apply);
            Links.Controller.PopUpCanvas.Place(canvas, false);
        }
        void EngineButton_Click(object sender, RoutedEventArgs e)
        {
            ShipTypeclass shiptype = schema.GetShipType();
            List<Engineclass> list = new List<Engineclass>();
            foreach (Engineclass engine in Links.EngineTypes.Values)
                if (GSGameInfo.SciencesList.SciencesArray.ContainsKey((ushort)engine.ID) && shiptype.EngineSize >= engine.Size)
                    list.Add(engine);
            SelectItemCanvas canvas = new SelectItemCanvas(list.ToArray(), Engineclass.GetEngineTypeColumnNames(), new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, "Выберите двигатель", ApplyEnum.Engine);
            Links.Controller.PopUpCanvas.Place(canvas, false);
        }

        void ComputerButton_Click(object sender, RoutedEventArgs e)
        {
            ShipTypeclass shiptype = schema.GetShipType();
            List<Computerclass> list = new List<Computerclass>();
            foreach (Computerclass computer in Links.ComputerTypes.Values)
                if (GSGameInfo.SciencesList.SciencesArray.ContainsKey((ushort)computer.ID) && shiptype.ComputerSize >= computer.Size)
                    list.Add(computer);
            SelectItemCanvas canvas = new SelectItemCanvas(list.ToArray(), Computerclass.GetComputerTypeColumnNames(), new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, "Выберите компьютер", ApplyEnum.Computer);
            Links.Controller.PopUpCanvas.Place(canvas, false);
        }

        void ShieldButton_Click(object sender, RoutedEventArgs e)
        {
            ShipTypeclass shiptype = schema.GetShipType();
            List<Shieldclass> list = new List<Shieldclass>();
            foreach (Shieldclass shield in Links.ShieldTypes.Values)
                if (GSGameInfo.SciencesList.SciencesArray.ContainsKey((ushort)shield.ID) && shiptype.ShieldSize >= shield.Size)
                    list.Add(shield);
            SelectItemCanvas canvas = new SelectItemCanvas(list.ToArray(), Shieldclass.GetShieldTypeColumnNames(), new int[] { 0, 1, 2, 3, 4, 5, 6,7 }, "Выберите щит", ApplyEnum.Shield);
            Links.Controller.PopUpCanvas.Place(canvas, false);
        }

        void GeneratorButton_Click(object sender, RoutedEventArgs e)
        {
            ShipTypeclass shiptype=schema.GetShipType();
            List<Generatorclass> list = new List<Generatorclass>();
            foreach (Generatorclass generator in Links.GeneratorTypes.Values)
                if (GSGameInfo.SciencesList.SciencesArray.ContainsKey((ushort)generator.ID) && shiptype.GeneratorSize >= generator.Size)
                    list.Add(generator);
            SelectItemCanvas canvas = new SelectItemCanvas(list.ToArray(), Generatorclass.GetGeneratorTypeColumnNames(), new int[] { 0, 1, 2, 3, 4, 5, 6 }, "Выберите генератор", ApplyEnum.Generator);
            Links.Controller.PopUpCanvas.Place(canvas, false);
        }

        void ShipNameButton_Click(object sender, RoutedEventArgs e)
        {
            SelectSchemaNameCanvas canvas = new SelectSchemaNameCanvas();
            Links.Controller.PopUpCanvas.Place(canvas, false);
        }

        void ShipTypeButton_Click(object sender, RoutedEventArgs e)
        {
            List<ShipTypeclass> list = new List<ShipTypeclass>();
            foreach (ShipTypeclass shiptype in Links.ShipTypes.Values)
                if (GSGameInfo.SciencesList.SciencesArray.ContainsKey((ushort)shiptype.ID))
                    list.Add(shiptype);

            SelectItemCanvas canvas = new SelectItemCanvas(list.ToArray(), ShipTypeclass.GetShipTypeColumnNames(), new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, "Выберите модель корабля", ApplyEnum.ShipType);


            Links.Controller.PopUpCanvas.Place(canvas, false);
        }
        Button PutButton(string text, StackPanel stack, int row, int column)
        {
            Button button = new Button();
            button.Style = Links.ButtonStyle;
            stack.Children.Add(button);
            button.Content = text;
            return button;
        }
        TreeViewItem PutItem(string text, ItemsE parent)
        {
            TreeViewItem item = new TreeViewItem();
            TItems[parent].Items.Add(item);
            item.Foreground = Brushes.White;
            item.Header = text;
            return item;

        }
    }
}
