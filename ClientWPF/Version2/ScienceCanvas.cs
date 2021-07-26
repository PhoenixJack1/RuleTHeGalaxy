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
   /* class ScienceListBorder:Border
    {
        ScienceFilter Filter;
        StackPanel ScienceListPanel;
        SortedSet<double> OpenedItems;
        TreeViewItem Focuseditem;
        System.Windows.Threading.DispatcherTimer FocusedTimes;// = new System.Windows.Threading.DispatcherTimer();
        public GameScience FocusedScience;
        SortedList<ScienceGroup, SortedSet<ushort>> ScienceGroups;
        public ScienceListBorder()
        {
            //BorderBrush = Brushes.Black;
            //BorderThickness = new Thickness(2);
            Margin = new Thickness(2);
            Height = 606;
            Width = 500;

            StackPanel ScienceListTopPanel = new StackPanel();
            ScienceListTopPanel.Orientation = Orientation.Vertical;
            Child = ScienceListTopPanel;

            Filter = new ScienceFilter(new string[] { Links.Interface("ScienceLevel"), Links.Interface("ScienceType") });
            ScienceListTopPanel.Children.Add(Filter);

            ScrollViewer ScienceListViewer = new ScrollViewer();
            ScienceListViewer.PreviewMouseWheel += ScienceListViewer_PreviewMouseWheel;
            ScienceListViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            ScienceListViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            ScienceListViewer.Width = 490;
            ScienceListViewer.Height = 530;
            ScienceListTopPanel.Children.Add(ScienceListViewer);

            ScienceListPanel = new StackPanel();
            ScienceListPanel.Orientation = Orientation.Vertical;
            ScienceListViewer.Content = ScienceListPanel;

            OpenedItems = new SortedSet<double>();
            FocusedTimes = new System.Windows.Threading.DispatcherTimer();
            FocusedTimes.Tick += FocusedTimes_Tick;
        }
        void FocusedTimes_Tick(object sender, EventArgs e)
        {
            Focuseditem.Focus();
            FocusedTimes.Stop();
        }
        private void ScienceListViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer viewer = (ScrollViewer)sender;
            if (e.Delta > 0)
                viewer.PageUp();
            else
                viewer.PageDown();
        }
        public void Draw()
        {
            if (Filter.curvalue == 0)
                PutBasicParameters();
            else
                PutBasicParametersGroups();
        }
        public void PutBasicParameters()
        {
            ScienceListPanel.Children.Clear();
            TreeView ScienceListTree = new TreeView();
            ScienceListTree.Background = Brushes.Black;
            ScienceListPanel.Children.Add(ScienceListTree);
            for (int i = 0; i <= ScienceAnalys.MaxLevelAvailable; i++)
            {
                TreeViewItem item = new TreeViewItem();
                item.Header = string.Format(Links.Interface("LevelSciences"), (i + 1).ToString());
                item.Style = Links.TextStyle;
                item.Foreground = Brushes.White;
                ScienceListTree.Items.Add(item);
                item.Tag = (double)i;
                if (OpenedItems.Contains((double)item.Tag)) item.IsExpanded = true;
                item.Expanded += new RoutedEventHandler(item_Expanded);
                item.Collapsed += new RoutedEventHandler(item_Expanded);
                PutTypeItem(Links.Interface("Buildings"), item, Links.Science.EType.Building, i);
                PutTypeItem(Links.Interface("ShipTypes"), item, Links.Science.EType.ShipTypes, i);
                PutTypeItem(Links.Interface("Generators"), item, Links.Science.EType.Generator, i);
                PutTypeItem(Links.Interface("Shields"), item, Links.Science.EType.Shield, i);
                PutTypeItem(Links.Interface("Computers"), item, Links.Science.EType.Computer, i);
                PutTypeItem(Links.Interface("Engines"), item, Links.Science.EType.Engine, i);
                PutTypeItem(Links.Interface("Weapons"), item, Links.Science.EType.Weapon, i);
                PutTypeItem(Links.Interface("Equipments"), item, Links.Science.EType.Equipment, i);
                PutTypeItem(Links.Interface("Others"), item, Links.Science.EType.Other, i);
            }
          
            if (Focuseditem != null)
            {
                //Focuseditem.Background = Brushes.LightBlue;
                //Focuseditem.IsSelected = true;
                //Focuseditem.Focus();
                FocusedTimes.Interval = TimeSpan.FromMilliseconds(100);
                //FocusedTimes.Tick += new EventHandler(FocusedTimes_Tick);
                FocusedTimes.Start();
            }
        }
        void item_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)sender;
            if (item.IsExpanded)
                OpenedItems.Add((double)item.Tag);
            else
                OpenedItems.Remove((double)item.Tag);
        }
        void PutTypeItem(string Name, TreeViewItem parent, Links.Science.EType type, int level)
        {
            TreeViewItem item = new TreeViewItem();
            item.Background = Brushes.Black;
            item.Foreground = Brushes.White;
            parent.Items.Add(item);
            item.Header = Name;
            item.Tag = (double)level + ((double)type) / 10 + 0.01;
            item.Expanded += new RoutedEventHandler(item_Expanded);
            item.Collapsed += new RoutedEventHandler(item_Expanded);
            if (OpenedItems.Contains((double)item.Tag)) item.IsExpanded = true;
            foreach (ushort value in GSGameInfo.SciencesArray)
            {
                GameScience science = Links.Science.GameSciences[value];
                if (science.Level == level && science.Type == type)
                {
                    TreeViewItem inneritem = new TreeViewItem();
                    item.Items.Add(inneritem);
                    StackPanel panel = new StackPanel();
                    panel.Orientation = Orientation.Horizontal;
                    if (science.IsRare) panel.Children.Add(Common.GetRectangle(30, Links.Brushes.RareImageBrush));
                    Label NameLabel = new Label();
                    NameLabel.Style = Links.TextStyle;
                    NameLabel.Foreground = Brushes.White;
                    panel.Children.Add(NameLabel);
                    switch (science.Type)
                    {
                        case Links.Science.EType.Building: NameLabel.Content = GameObjectName.GetBuildingsName(Links.Buildings[science.ID]); break;
                        case Links.Science.EType.ShipTypes: NameLabel.Content = Links.ShipTypes[science.ID].GetName(); break;
                        case Links.Science.EType.Weapon: NameLabel.Content = GameObjectName.GetWeaponName(Links.WeaponTypes[science.ID]); break;
                        case Links.Science.EType.Generator: NameLabel.Content = GameObjectName.GetGeneratorName(Links.GeneratorTypes[science.ID]); break;
                        case Links.Science.EType.Shield: NameLabel.Content = GameObjectName.GetShieldName(Links.ShieldTypes[science.ID]); break;
                        case Links.Science.EType.Computer: NameLabel.Content = GameObjectName.GetComputerName(Links.ComputerTypes[science.ID]); break;
                        case Links.Science.EType.Engine: NameLabel.Content = GameObjectName.GetEngineName(Links.EngineTypes[science.ID]); break;
                        case Links.Science.EType.Equipment: NameLabel.Content = GameObjectName.GetEquipmentName(Links.EquipmentTypes[science.ID]); break;
                        case Links.Science.EType.Other: NameLabel.Content = GameObjectName.GetNewLandsName(science); break;
                    }
                    //NameLabel.Content = science.Name;
                    NameLabel.VerticalAlignment = VerticalAlignment.Center;
                    inneritem.Header = panel;
                    inneritem.Tag = science;
                    if (FocusedScience != null && FocusedScience == science)
                    {
                        parent.IsExpanded = true;
                        item.IsExpanded = true;
                        Focuseditem = inneritem;
                    }
                    inneritem.PreviewMouseDown += new MouseButtonEventHandler(inneritem_PreviewMouseDown);
                }
            }
        }
        void inneritem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)sender;
            GameScience science = (GameScience)item.Tag;
            if (Links.Controller.Science2Canvas._ElementsCanvas.IsRightBorder == false)
            {
                Links.Helper.Science = science;
                Links.Controller.Science2Canvas._ElementsCanvas.PutRightBorder(false);
                Links.Controller.Science2Canvas._ElementsCanvas.IsRightBorder = true;
            }
            else
                Links.Controller.Science2Canvas._ElementsCanvas.LearnNewScinece(science, false);
            //InfoBorder.Child = GetScienceInfoElement(science); //vbx;
            Focuseditem = item;
            FocusedScience = science;
        }
        public void PutBasicParametersGroups()
        {
            FillGroups();
            ScienceListPanel.Children.Clear();

            TreeView item = new TreeView();
            item.FontSize = 24;
            item.FontFamily = Links.Font;
            item.FontWeight = FontWeights.Bold;
            item.Background = Brushes.Black;
            ScienceListPanel.Children.Add(item);
            TreeViewItem buildings = PutTypeItemGroup(Links.Interface("Buildings"), item, ScienceGroup.None);
            TreeViewItem common_buildings = PutTypeItemGroup("Общие постройки", buildings, ScienceGroup.None);
            PutTypeItemGroup("Жилые дома", common_buildings, ScienceGroup.BuildLive);
            PutTypeItemGroup("Ремонтные цеха", common_buildings, ScienceGroup.BuildRepair);
            TreeViewItem res_buildings = PutTypeItemGroup("Ресурсные постройки", buildings, ScienceGroup.None);
            PutTypeItemGroup("Банки", res_buildings, ScienceGroup.BuildBank);
            PutTypeItemGroup("Шахты", res_buildings, ScienceGroup.BuildMetal);
            PutTypeItemGroup("Фабрики микросхем", res_buildings, ScienceGroup.BuildChip);
            PutTypeItemGroup("Ускорители частиц", res_buildings, ScienceGroup.BuildAnti);
            TreeViewItem cap_buildings = PutTypeItemGroup("Складские постройки", buildings, ScienceGroup.None);
            PutTypeItemGroup("Склады металла", cap_buildings, ScienceGroup.BuildMetalCap);
            PutTypeItemGroup("Склады микросхем", cap_buildings, ScienceGroup.BuildChipCap);
            PutTypeItemGroup("Склады антиматерии", cap_buildings, ScienceGroup.BuildAntiCap);
            TreeViewItem war_buildings = PutTypeItemGroup("Военные постройки", buildings, ScienceGroup.None);
            PutTypeItemGroup("Радары", war_buildings, ScienceGroup.BuildRadar);
            PutTypeItemGroup("Дата центры", war_buildings, ScienceGroup.BuildData);
            PutTypeItemGroup("Учебные центры", war_buildings, ScienceGroup.BuildEducation);

            TreeViewItem shiptypes = PutTypeItemGroup(Links.Interface("ShipTypes"), item, ScienceGroup.None);
            PutTypeItemGroup(Links.Interface("Ship_Scout"), shiptypes, ScienceGroup.Ship_Scout);
            PutTypeItemGroup(Links.Interface("Ship_Corvette"), shiptypes, ScienceGroup.Ship_Corvette);
            PutTypeItemGroup(Links.Interface("Ship_Transport"), shiptypes, ScienceGroup.Ship_Transp);
            PutTypeItemGroup(Links.Interface("Ship_Battle"), shiptypes, ScienceGroup.Ship_Battle);
            PutTypeItemGroup(Links.Interface("Ship_Frigate"), shiptypes, ScienceGroup.Ship_Frigate);
            PutTypeItemGroup(Links.Interface("Ship_Fighter"), shiptypes, ScienceGroup.Ship_Fighter);
            PutTypeItemGroup(Links.Interface("Ship_Dreadnought"), shiptypes, ScienceGroup.Ship_Dreadnought);
            PutTypeItemGroup(Links.Interface("Ship_Devostator"), shiptypes, ScienceGroup.Ship_Devostator);
            PutTypeItemGroup(Links.Interface("Ship_Warrior"), shiptypes, ScienceGroup.Ship_Warrior);
            PutTypeItemGroup(Links.Interface("Ship_Cruiser"), shiptypes, ScienceGroup.Ship_Cruiser);

            TreeViewItem generators = PutTypeItemGroup(Links.Interface("Generators"), item, ScienceGroup.None);
            TreeViewItem[] generators_sizes = PutTypeItems(generators, Links.Interface("Item_Generators"));
            PutTypeItemGroup(GameObjectName.GeneratorsName[5][Links.Lang], generators_sizes[0], ScienceGroup.Generator_S_S);
            PutTypeItemGroup(GameObjectName.GeneratorsName[0][Links.Lang], generators_sizes[0], ScienceGroup.Generator_R_S);
            PutTypeItemGroup(GameObjectName.GeneratorsName[15][Links.Lang], generators_sizes[1], ScienceGroup.Generator_S_M);
            PutTypeItemGroup(GameObjectName.GeneratorsName[10][Links.Lang], generators_sizes[1], ScienceGroup.Generator_R_M);
            PutTypeItemGroup(GameObjectName.GeneratorsName[25][Links.Lang], generators_sizes[2], ScienceGroup.Generator_S_L);
            PutTypeItemGroup(GameObjectName.GeneratorsName[20][Links.Lang], generators_sizes[2], ScienceGroup.Generator_R_L);

            TreeViewItem shields = PutTypeItemGroup(Links.Interface("Shields"), item, ScienceGroup.None);
            TreeViewItem[] shields_sizes = PutTypeItems(shields, Links.Interface("Item_Shields"));
            PutTypeItemGroup(GameObjectName.ShieldsName[5][Links.Lang], shields_sizes[0], ScienceGroup.Shield_S_S);
            PutTypeItemGroup(GameObjectName.ShieldsName[0][Links.Lang], shields_sizes[0], ScienceGroup.Shield_R_S);
            PutTypeItemGroup(GameObjectName.ShieldsName[15][Links.Lang], shields_sizes[1], ScienceGroup.Shield_S_M);
            PutTypeItemGroup(GameObjectName.ShieldsName[10][Links.Lang], shields_sizes[1], ScienceGroup.Shield_R_M);
            PutTypeItemGroup(GameObjectName.ShieldsName[25][Links.Lang], shields_sizes[2], ScienceGroup.Shield_S_L);
            PutTypeItemGroup(GameObjectName.ShieldsName[20][Links.Lang], shields_sizes[2], ScienceGroup.Shield_R_L);

            TreeViewItem computers = PutTypeItemGroup(Links.Interface("Computers"), item, ScienceGroup.None);
            TreeViewItem[] comp_sizes = PutTypeItems(computers, Links.Interface("Item_Computers"));
            TreeViewItem[] comp_smalls = PutFiveTypeItems(comp_sizes[0], Links.Interface("Item_Small"), Links.Interface("Item_Computers"));
            PutTypeItemGroup(GameObjectName.ComputersTargetName[0][Links.Lang] + " " + GameObjectName.ComputersName[3][Links.Lang], comp_smalls[0], ScienceGroup.Comp_I_E_S);
            PutTypeItemGroup(GameObjectName.ComputersTargetName[0][Links.Lang] + " " + GameObjectName.ComputersName[6][Links.Lang], comp_smalls[0], ScienceGroup.Comp_E_E_S);
            PutTypeItemGroup(GameObjectName.ComputersTargetName[1][Links.Lang] + " " + GameObjectName.ComputersName[3][Links.Lang], comp_smalls[1], ScienceGroup.Comp_I_P_S);
            PutTypeItemGroup(GameObjectName.ComputersTargetName[1][Links.Lang] + " " + GameObjectName.ComputersName[6][Links.Lang], comp_smalls[1], ScienceGroup.Comp_E_P_S);
            PutTypeItemGroup(GameObjectName.ComputersTargetName[2][Links.Lang] + " " + GameObjectName.ComputersName[3][Links.Lang], comp_smalls[2], ScienceGroup.Comp_I_I_S);
            PutTypeItemGroup(GameObjectName.ComputersTargetName[2][Links.Lang] + " " + GameObjectName.ComputersName[6][Links.Lang], comp_smalls[2], ScienceGroup.Comp_E_I_S);
            PutTypeItemGroup(GameObjectName.ComputersTargetName[3][Links.Lang] + " " + GameObjectName.ComputersName[3][Links.Lang], comp_smalls[3], ScienceGroup.Comp_I_C_S);
            PutTypeItemGroup(GameObjectName.ComputersTargetName[3][Links.Lang] + " " + GameObjectName.ComputersName[6][Links.Lang], comp_smalls[3], ScienceGroup.Comp_E_C_S);
            PutTypeItemGroup(GameObjectName.ComputersName[0][Links.Lang], comp_smalls[4], ScienceGroup.Comp_B_S);
            TreeViewItem[] comp_med = PutFiveTypeItems(comp_sizes[1], Links.Interface("Item_Medium"), Links.Interface("Item_Computers"));
            PutTypeItemGroup(GameObjectName.ComputersTargetName[0][Links.Lang] + " " + GameObjectName.ComputersName[4][Links.Lang], comp_med[0], ScienceGroup.Comp_I_E_M);
            PutTypeItemGroup(GameObjectName.ComputersTargetName[0][Links.Lang] + " " + GameObjectName.ComputersName[7][Links.Lang], comp_med[0], ScienceGroup.Comp_E_E_M);
            PutTypeItemGroup(GameObjectName.ComputersTargetName[1][Links.Lang] + " " + GameObjectName.ComputersName[4][Links.Lang], comp_med[1], ScienceGroup.Comp_I_P_M);
            PutTypeItemGroup(GameObjectName.ComputersTargetName[1][Links.Lang] + " " + GameObjectName.ComputersName[7][Links.Lang], comp_med[1], ScienceGroup.Comp_E_P_M);
            PutTypeItemGroup(GameObjectName.ComputersTargetName[2][Links.Lang] + " " + GameObjectName.ComputersName[4][Links.Lang], comp_med[2], ScienceGroup.Comp_I_I_M);
            PutTypeItemGroup(GameObjectName.ComputersTargetName[2][Links.Lang] + " " + GameObjectName.ComputersName[7][Links.Lang], comp_med[2], ScienceGroup.Comp_E_I_M);
            PutTypeItemGroup(GameObjectName.ComputersTargetName[3][Links.Lang] + " " + GameObjectName.ComputersName[4][Links.Lang], comp_med[3], ScienceGroup.Comp_I_C_M);
            PutTypeItemGroup(GameObjectName.ComputersTargetName[3][Links.Lang] + " " + GameObjectName.ComputersName[7][Links.Lang], comp_med[3], ScienceGroup.Comp_E_C_M);
            PutTypeItemGroup(GameObjectName.ComputersName[1][Links.Lang], comp_med[4], ScienceGroup.Comp_B_M);
            TreeViewItem[] comp_large = PutFiveTypeItems(comp_sizes[2], Links.Interface("Item_Large"), Links.Interface("Item_Computers"));
            PutTypeItemGroup(GameObjectName.ComputersTargetName[0][Links.Lang] + " " + GameObjectName.ComputersName[5][Links.Lang], comp_large[0], ScienceGroup.Comp_I_E_L);
            PutTypeItemGroup(GameObjectName.ComputersTargetName[0][Links.Lang] + " " + GameObjectName.ComputersName[8][Links.Lang], comp_large[0], ScienceGroup.Comp_E_E_L);
            PutTypeItemGroup(GameObjectName.ComputersTargetName[1][Links.Lang] + " " + GameObjectName.ComputersName[5][Links.Lang], comp_large[1], ScienceGroup.Comp_I_P_L);
            PutTypeItemGroup(GameObjectName.ComputersTargetName[1][Links.Lang] + " " + GameObjectName.ComputersName[8][Links.Lang], comp_large[1], ScienceGroup.Comp_E_P_L);
            PutTypeItemGroup(GameObjectName.ComputersTargetName[2][Links.Lang] + " " + GameObjectName.ComputersName[5][Links.Lang], comp_large[2], ScienceGroup.Comp_I_I_L);
            PutTypeItemGroup(GameObjectName.ComputersTargetName[2][Links.Lang] + " " + GameObjectName.ComputersName[8][Links.Lang], comp_large[2], ScienceGroup.Comp_E_I_L);
            PutTypeItemGroup(GameObjectName.ComputersTargetName[3][Links.Lang] + " " + GameObjectName.ComputersName[5][Links.Lang], comp_large[3], ScienceGroup.Comp_I_C_L);
            PutTypeItemGroup(GameObjectName.ComputersTargetName[3][Links.Lang] + " " + GameObjectName.ComputersName[8][Links.Lang], comp_large[3], ScienceGroup.Comp_E_C_L);
            PutTypeItemGroup(GameObjectName.ComputersName[2][Links.Lang], comp_large[4], ScienceGroup.Comp_B_L);

            TreeViewItem engines = PutTypeItemGroup(Links.Interface("Engines"), item, ScienceGroup.None);
            TreeViewItem[] engines_sizes = PutTypeItems(engines, Links.Interface("Item_Engines"));
            PutTypeItemGroup(GameObjectName.EnginesName[5][Links.Lang], engines_sizes[0], ScienceGroup.Engine_B_S);
            PutTypeItemGroup(GameObjectName.EnginesName[0][Links.Lang], engines_sizes[0], ScienceGroup.Engine_U_S);
            PutTypeItemGroup(GameObjectName.EnginesName[15][Links.Lang], engines_sizes[1], ScienceGroup.Engine_B_M);
            PutTypeItemGroup(GameObjectName.EnginesName[10][Links.Lang], engines_sizes[1], ScienceGroup.Engine_U_M);
            PutTypeItemGroup(GameObjectName.EnginesName[25][Links.Lang], engines_sizes[2], ScienceGroup.Engine_B_L);
            PutTypeItemGroup(GameObjectName.EnginesName[20][Links.Lang], engines_sizes[2], ScienceGroup.Engine_U_L);

            TreeViewItem weapons = PutTypeItemGroup(Links.Interface("Weapons"), item, ScienceGroup.None);
            TreeViewItem energy_weapon = PutTypeItemGroup(Links.Interface("EnergyWeapon"), weapons, ScienceGroup.None);
            TreeViewItem laser = PutTypeItemGroup(GameObjectName.GetWeaponGroupName(0, EWeaponType.Laser), energy_weapon, ScienceGroup.None);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(1, EWeaponType.Laser), laser, ScienceGroup.Laser_S);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(2, EWeaponType.Laser), laser, ScienceGroup.Laser_M);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(3, EWeaponType.Laser), laser, ScienceGroup.Laser_L);
            TreeViewItem emi = PutTypeItemGroup(GameObjectName.GetWeaponGroupName(0, EWeaponType.EMI), energy_weapon, ScienceGroup.None);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(1, EWeaponType.EMI), emi, ScienceGroup.EMI_S);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(2, EWeaponType.EMI), emi, ScienceGroup.EMI_M);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(3, EWeaponType.EMI), emi, ScienceGroup.EMI_L);
            TreeViewItem plasma = PutTypeItemGroup(GameObjectName.GetWeaponGroupName(0, EWeaponType.Plasma), energy_weapon, ScienceGroup.None);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(1, EWeaponType.Plasma), plasma, ScienceGroup.Plasma_S);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(2, EWeaponType.Plasma), plasma, ScienceGroup.Plasma_M);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(3, EWeaponType.Plasma), plasma, ScienceGroup.Plasma_L);
            TreeViewItem solar = PutTypeItemGroup(GameObjectName.GetWeaponGroupName(0, EWeaponType.Solar), energy_weapon, ScienceGroup.None);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(1, EWeaponType.Solar), solar, ScienceGroup.Solar_S);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(2, EWeaponType.Solar), solar, ScienceGroup.Solar_M);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(3, EWeaponType.Solar), solar, ScienceGroup.Solar_L);
            TreeViewItem physic_weapon = PutTypeItemGroup(Links.Interface("PhysicWeapon"), weapons, ScienceGroup.None);
            TreeViewItem cannon = PutTypeItemGroup(GameObjectName.GetWeaponGroupName(0, EWeaponType.Cannon), physic_weapon, ScienceGroup.None);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(1, EWeaponType.Cannon), cannon, ScienceGroup.Cannon_S);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(2, EWeaponType.Cannon), cannon, ScienceGroup.Cannon_M);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(3, EWeaponType.Cannon), cannon, ScienceGroup.Cannon_L);
            TreeViewItem gauss = PutTypeItemGroup(GameObjectName.GetWeaponGroupName(0, EWeaponType.Gauss), physic_weapon, ScienceGroup.None);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(1, EWeaponType.Gauss), gauss, ScienceGroup.Gauss_S);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(2, EWeaponType.Gauss), gauss, ScienceGroup.Gauss_M);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(3, EWeaponType.Gauss), gauss, ScienceGroup.Gauss_L);
            TreeViewItem missle = PutTypeItemGroup(GameObjectName.GetWeaponGroupName(0, EWeaponType.Missle), physic_weapon, ScienceGroup.None);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(1, EWeaponType.Missle), missle, ScienceGroup.Missle_S);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(2, EWeaponType.Missle), missle, ScienceGroup.Missle_M);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(3, EWeaponType.Missle), missle, ScienceGroup.Missle_L);
            TreeViewItem anti = PutTypeItemGroup(GameObjectName.GetWeaponGroupName(0, EWeaponType.AntiMatter), physic_weapon, ScienceGroup.None);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(1, EWeaponType.AntiMatter), anti, ScienceGroup.Anti_S);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(2, EWeaponType.AntiMatter), anti, ScienceGroup.Anti_M);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(3, EWeaponType.AntiMatter), anti, ScienceGroup.Anti_L);
            TreeViewItem irr_weapon = PutTypeItemGroup(Links.Interface("IrregularWeapon"), weapons, ScienceGroup.None);
            TreeViewItem psi = PutTypeItemGroup(GameObjectName.GetWeaponGroupName(0, EWeaponType.Psi), irr_weapon, ScienceGroup.None);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(1, EWeaponType.Psi), psi, ScienceGroup.Psi_S);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(2, EWeaponType.Psi), psi, ScienceGroup.Psi_M);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(3, EWeaponType.Psi), psi, ScienceGroup.Psi_L);
            TreeViewItem dark = PutTypeItemGroup(GameObjectName.GetWeaponGroupName(0, EWeaponType.Dark), irr_weapon, ScienceGroup.None);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(1, EWeaponType.Dark), dark, ScienceGroup.Dark_S);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(2, EWeaponType.Dark), dark, ScienceGroup.Dark_M);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(3, EWeaponType.Dark), dark, ScienceGroup.Dark_L);
            TreeViewItem warp = PutTypeItemGroup(GameObjectName.GetWeaponGroupName(0, EWeaponType.Warp), irr_weapon, ScienceGroup.None);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(1, EWeaponType.Warp), warp, ScienceGroup.Warp_S);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(2, EWeaponType.Warp), warp, ScienceGroup.Warp_M);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(3, EWeaponType.Warp), warp, ScienceGroup.Warp_L);
            TreeViewItem time = PutTypeItemGroup(GameObjectName.GetWeaponGroupName(0, EWeaponType.Time), irr_weapon, ScienceGroup.None);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(1, EWeaponType.Time), time, ScienceGroup.Time_S);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(2, EWeaponType.Time), time, ScienceGroup.Time_M);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(3, EWeaponType.Time), time, ScienceGroup.Time_L);
            TreeViewItem cyber_weapon = PutTypeItemGroup(Links.Interface("CyberWeapon"), weapons, ScienceGroup.None);
            TreeViewItem slice = PutTypeItemGroup(GameObjectName.GetWeaponGroupName(0, EWeaponType.Slicing), cyber_weapon, ScienceGroup.None);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(1, EWeaponType.Slicing), slice, ScienceGroup.Slice_S);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(2, EWeaponType.Slicing), slice, ScienceGroup.Slice_M);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(3, EWeaponType.Slicing), slice, ScienceGroup.Slice_L);
            TreeViewItem rad = PutTypeItemGroup(GameObjectName.GetWeaponGroupName(0, EWeaponType.Radiation), cyber_weapon, ScienceGroup.None);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(1, EWeaponType.Radiation), rad, ScienceGroup.Rad_S);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(2, EWeaponType.Radiation), rad, ScienceGroup.Rad_M);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(3, EWeaponType.Radiation), rad, ScienceGroup.Rad_L);
            TreeViewItem drone = PutTypeItemGroup(GameObjectName.GetWeaponGroupName(0, EWeaponType.Drone), cyber_weapon, ScienceGroup.None);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(1, EWeaponType.Drone), drone, ScienceGroup.Drone_S);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(2, EWeaponType.Drone), drone, ScienceGroup.Drone_M);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(3, EWeaponType.Drone), drone, ScienceGroup.Drone_L);
            TreeViewItem magnet = PutTypeItemGroup(GameObjectName.GetWeaponGroupName(0, EWeaponType.Magnet), cyber_weapon, ScienceGroup.None);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(1, EWeaponType.Magnet), magnet, ScienceGroup.Magnet_S);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(2, EWeaponType.Magnet), magnet, ScienceGroup.Magnet_M);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(3, EWeaponType.Magnet), magnet, ScienceGroup.Magnet_L);

            TreeViewItem equip = PutTypeItemGroup(Links.Interface("Equipments"), item, ScienceGroup.None);
            TreeViewItem basic_equip = PutTypeItemGroup(Links.Interface("Item_Basic"), equip, ScienceGroup.None);
            PutEquipItems(basic_equip, Links.Interface("Hull"), ScienceGroup.Health_S, ScienceGroup.Health_M, ScienceGroup.Health_L);
            PutEquipItems(basic_equip, Links.Interface("Shield"), ScienceGroup.Shield_S, ScienceGroup.Shield_M, ScienceGroup.Shield_L);
            PutEquipItems(basic_equip, Links.Interface("Energy"), ScienceGroup.Energy_S, ScienceGroup.Energy_M, ScienceGroup.Energy_L);
            TreeViewItem regen_equip = PutTypeItemGroup(Links.Interface("Item_Regen"), equip, ScienceGroup.None);
            PutEquipItems(regen_equip, Links.Interface("HullRestore"), ScienceGroup.Reg_H_S, ScienceGroup.Reg_H_M, ScienceGroup.Reg_H_L);
            PutEquipItems(regen_equip, Links.Interface("ShieldRestore"), ScienceGroup.Reg_S_S, ScienceGroup.Reg_S_M, ScienceGroup.Reg_S_L);
            PutEquipItems(regen_equip, Links.Interface("EnergyRestore"), ScienceGroup.Reg_E_S, ScienceGroup.Reg_E_M, ScienceGroup.Reg_E_L);
            PutFiveItemTotal(equip, Links.Interface("Item_Accuracy"), new ScienceGroup[] { ScienceGroup.Acc_E_S, ScienceGroup.Acc_E_M, ScienceGroup.Acc_E_L,
            ScienceGroup.Acc_P_S,ScienceGroup.Acc_P_M,ScienceGroup.Acc_P_L,ScienceGroup.Acc_I_S,ScienceGroup.Acc_I_M,ScienceGroup.Acc_I_L,
            ScienceGroup.Acc_C_S,ScienceGroup.Acc_C_M, ScienceGroup.Acc_C_L, ScienceGroup.Acc_A_S,ScienceGroup.Acc_A_M,ScienceGroup.Acc_A_L});
            PutFiveItemTotal(equip, Links.Interface("Item_Evasion"), new ScienceGroup[] {ScienceGroup.Eva_E_S, ScienceGroup.Eva_E_M, ScienceGroup.Eva_E_L,
            ScienceGroup.Eva_P_S, ScienceGroup.Eva_P_M, ScienceGroup.Eva_P_L, ScienceGroup.Eva_I_S, ScienceGroup.Eva_I_M, ScienceGroup.Eva_I_L,
            ScienceGroup.Eva_C_S, ScienceGroup.Eva_C_M, ScienceGroup.Eva_C_L, ScienceGroup.Eva_A_S, ScienceGroup.Eva_A_M, ScienceGroup.Eva_A_L});
            PutFiveItemTotal(equip, Links.Interface("Item_Absorb"), new ScienceGroup[] {ScienceGroup.Ign_E_S, ScienceGroup.Ign_E_M, ScienceGroup.Ign_E_L,
                ScienceGroup.Ign_P_S, ScienceGroup.Ign_P_M, ScienceGroup.Ign_P_L, ScienceGroup.Ign_I_S, ScienceGroup.Ign_I_M, ScienceGroup.Ign_I_L,
                ScienceGroup.Ign_C_S,ScienceGroup.Ign_C_M, ScienceGroup.Ign_C_L, ScienceGroup.Ign_A_S, ScienceGroup.Ign_A_M, ScienceGroup.Ign_A_L });
            PutFiveItemTotal(equip, Links.Interface("Item_Damage"), new ScienceGroup[]{ ScienceGroup.Dam_E_S, ScienceGroup.Dam_E_M, ScienceGroup.Dam_E_L,
            ScienceGroup.Dam_P_S, ScienceGroup.Dam_P_M, ScienceGroup.Dam_P_L, ScienceGroup.Dam_I_S, ScienceGroup.Dam_I_M, ScienceGroup.Dam_I_L,
            ScienceGroup.Dam_C_S, ScienceGroup.Dam_C_M, ScienceGroup.Dam_C_L, ScienceGroup.Dam_A_S, ScienceGroup.Dam_A_S, ScienceGroup.Dam_A_L});
            PutFiveItemTotal2(equip, Links.Interface("Item_Immune"), new ScienceGroup[] {ScienceGroup.Imm_E_S, ScienceGroup.Imm_E_M, ScienceGroup.Imm_P_S,
                ScienceGroup.Imm_P_M, ScienceGroup.Imm_I_S, ScienceGroup.Imm_I_M, ScienceGroup.Imm_C_S, ScienceGroup.Imm_C_M, ScienceGroup.Imm_A_S, ScienceGroup.Imm_A_M });
            TreeViewItem nb_equip = PutTypeItemGroup(Links.Interface("Item_NotBattle"), equip, ScienceGroup.None);
            TreeViewItem cargo = PutTypeItemGroup(Links.Interface("Item_Cargo"), nb_equip, ScienceGroup.None);
            PutTypeItemGroup(Links.Interface("Item_Small"), cargo, ScienceGroup.Cargo_S);
            PutTypeItemGroup(Links.Interface("Item_Medium"), cargo, ScienceGroup.Cargo_M);
            TreeViewItem colony = PutTypeItemGroup(Links.Interface("Item_Colony"), nb_equip, ScienceGroup.None);
            PutTypeItemGroup(Links.Interface("Item_Small"), colony, ScienceGroup.Colony_S);
            PutTypeItemGroup(Links.Interface("Item_Medium"), colony, ScienceGroup.Colony_M);
            TreeViewItem others = PutTypeItemGroup(Links.Interface("Others"), item, ScienceGroup.None);
            PutTypeItemGroup(Links.Interface("Item_NewLand"), others, ScienceGroup.NewLands);
            //PutTypeItem(Links.Interface("Equipments"), item, Links.Science.EType.Equipment, i);
            //PutTypeItem(Links.Interface("Others"), item, Links.Science.EType.Other, i);
            if (Focuseditem != null)
            {
                ItemsControl par = Focuseditem;
                for (;;)
                {
                    par = (ItemsControl)par.Parent;
                    if (par != item)
                    {
                        TreeViewItem tvi = (TreeViewItem)par;
                        tvi.IsExpanded = true;
                        par = tvi;
                    }
                    else
                    {
                        break;
                    }

                }
                //Focuseditem.Background = Brushes.LightBlue;
                //Focuseditem.IsSelected = true;
                //Focuseditem.Focus();
                FocusedTimes.Interval = TimeSpan.FromMilliseconds(100);
                FocusedTimes.Start();
            }
        }
        void FillGroups()
        {
            ScienceGroups = new SortedList<ScienceGroup, SortedSet<ushort>>();
            for (int i = 0; i < 226; i++)
                ScienceGroups.Add((ScienceGroup)i, new SortedSet<ushort>());
            foreach (ushort id in GSGameInfo.SciencesArray)
            {
                GameScience science = Links.Science.GameSciences[id];
                switch (science.Type)
                {
                    case Links.Science.EType.Building: ScienceGroups[GetBuildingGroup(science)].Add(id); break;
                    case Links.Science.EType.ShipTypes: ScienceGroups[GetShipTypeGroup(science)].Add(id); break;
                    case Links.Science.EType.Weapon: ScienceGroups[GetWeaponGroup(science)].Add(id); break;
                    case Links.Science.EType.Generator: ScienceGroups[GetGeneratorGroup(science)].Add(id); break;
                    case Links.Science.EType.Shield: ScienceGroups[GetShieldTypeGroup(science)].Add(id); break;
                    case Links.Science.EType.Computer: ScienceGroups[GetComputerGroup(science)].Add(id); break;
                    case Links.Science.EType.Engine: ScienceGroups[GetEngineGroup(science)].Add(id); break;
                    case Links.Science.EType.Equipment: ScienceGroups[GetEquipmentGroup(science)].Add(id); break;
                    case Links.Science.EType.Other: ScienceGroups[ScienceGroup.NewLands].Add(id); break;
                }
            }
        }
        TreeViewItem PutTypeItemGroup(string Name, ItemsControl parrent, ScienceGroup group)
        {
            TreeViewItem item = new TreeViewItem();
            item.Background = Brushes.Black;
            item.Foreground = Brushes.White;
            parrent.Items.Add(item);
            item.Header = Name;
            if (group == ScienceGroup.None) return item;
            foreach (ushort id in ScienceGroups[group])
            {
                TreeViewItem inneritem = new TreeViewItem();
                item.Items.Add(inneritem);
                StackPanel panel = new StackPanel();
                panel.Orientation = Orientation.Horizontal;
                GameScience science = Links.Science.GameSciences[id];
                if (science.IsRare) panel.Children.Add(Common.GetRectangle(30, Links.Brushes.RareImageBrush));
                Label NameLabel = new Label();
                NameLabel.Style = Links.TextStyle;
                NameLabel.Foreground = Brushes.White;
                panel.Children.Add(NameLabel);
                switch (science.Type)
                {
                    case Links.Science.EType.Building: NameLabel.Content = GameObjectName.GetBuildingsName(Links.Buildings[science.ID]); break;
                    case Links.Science.EType.ShipTypes: NameLabel.Content = Links.ShipTypes[science.ID].GetName(); break;
                    case Links.Science.EType.Weapon: NameLabel.Content = GameObjectName.GetWeaponName(Links.WeaponTypes[science.ID]); break;
                    case Links.Science.EType.Generator: NameLabel.Content = GameObjectName.GetGeneratorName(Links.GeneratorTypes[science.ID]); break;
                    case Links.Science.EType.Shield: NameLabel.Content = GameObjectName.GetShieldName(Links.ShieldTypes[science.ID]); break;
                    case Links.Science.EType.Computer: NameLabel.Content = GameObjectName.GetComputerName(Links.ComputerTypes[science.ID]); break;
                    case Links.Science.EType.Engine: NameLabel.Content = GameObjectName.GetEngineName(Links.EngineTypes[science.ID]); break;
                    case Links.Science.EType.Equipment: NameLabel.Content = GameObjectName.GetEquipmentName(Links.EquipmentTypes[science.ID]); break;
                    case Links.Science.EType.Other: NameLabel.Content = GameObjectName.GetNewLandsName(science); break;
                }
                //NameLabel.Content = science.Name;
                NameLabel.VerticalAlignment = VerticalAlignment.Center;
                inneritem.Header = panel;
                inneritem.Tag = science;
                TreeViewItem par = (TreeViewItem)parrent;
                if (FocusedScience != null && FocusedScience == science)
                {
                    //par.IsExpanded = true;
                    //item.IsExpanded = true;
                    Focuseditem = inneritem;
                }
                inneritem.PreviewMouseDown += new MouseButtonEventHandler(inneritem_PreviewMouseDown);
            }
            return item;
        }
        TreeViewItem[] PutTypeItems(ItemsControl parrent, string name)
        {
            TreeViewItem[] array = new TreeViewItem[3];
            for (int i = 0; i < 3; i++)
            {
                TreeViewItem item = new TreeViewItem();
                item.Background = Brushes.Black;
                item.Foreground = Brushes.White;
                parrent.Items.Add(item);
                array[i] = item;
            }
            array[0].Header = Links.Interface("Item_Small") + name;
            array[1].Header = Links.Interface("Item_Medium") + name;
            array[2].Header = Links.Interface("Item_Large") + name;
            return array;
        }
        TreeViewItem[] PutFiveTypeItems(TreeViewItem parrent, string size, string name)
        {
            TreeViewItem[] array = new TreeViewItem[5];
            for (int i = 0; i < 5; i++)
            {
                TreeViewItem item = new TreeViewItem();
                item.Background = Brushes.Black;
                item.Foreground = Brushes.White;
                parrent.Items.Add(item);
                array[i] = item;
            }
            array[0].Header = size + Links.Interface("Item_Energy") + name;
            array[1].Header = size + Links.Interface("Item_Physic") + name;
            array[2].Header = size + Links.Interface("Item_Irregular") + name;
            array[3].Header = size + Links.Interface("Item_Cyber") + name;
            array[4].Header = size + Links.Interface("Item_Total") + name;
            return array;
        }
        void PutEquipItems(ItemsControl parrent, string Name, ScienceGroup sg1, ScienceGroup sg2, ScienceGroup sg3)
        {
            TreeViewItem top = PutTypeItemGroup(Name, parrent, ScienceGroup.None);
            PutTypeItemGroup(Links.Interface("Item_Small"), top, sg1);
            PutTypeItemGroup(Links.Interface("Item_Medium"), top, sg2);
            PutTypeItemGroup(Links.Interface("Item_Group"), top, sg3);
        }
        void PutEquipItems(ItemsControl parrent, string Name, ScienceGroup sg1, ScienceGroup sg2)
        {
            TreeViewItem top = PutTypeItemGroup(Name, parrent, ScienceGroup.None);
            PutTypeItemGroup(Links.Interface("Item_Small"), top, sg1);
            PutTypeItemGroup(Links.Interface("Item_Medium"), top, sg2);
        }
        void PutFiveItemTotal(TreeViewItem parrent, string name, ScienceGroup[] groups)
        {
            TreeViewItem top = PutTypeItemGroup(name, parrent, ScienceGroup.None);
            PutEquipItems(top, Links.Interface("Item_Energy2"), groups[0], groups[1], groups[2]);
            PutEquipItems(top, Links.Interface("Item_Physic2"), groups[3], groups[4], groups[5]);
            PutEquipItems(top, Links.Interface("Item_Irregular2"), groups[6], groups[7], groups[8]);
            PutEquipItems(top, Links.Interface("Item_Cyber2"), groups[9], groups[10], groups[11]);
            PutEquipItems(top, Links.Interface("Item_Total2"), groups[12], groups[13], groups[14]);
        }
        void PutFiveItemTotal2(TreeViewItem parrent, string name, ScienceGroup[] groups)
        {
            TreeViewItem top = PutTypeItemGroup(name, parrent, ScienceGroup.None);
            PutEquipItems(top, Links.Interface("Item_Energy2"), groups[0], groups[1]);
            PutEquipItems(top, Links.Interface("Item_Physic2"), groups[2], groups[3]);
            PutEquipItems(top, Links.Interface("Item_Irregular2"), groups[4], groups[5]);
            PutEquipItems(top, Links.Interface("Item_Cyber2"), groups[6], groups[7]);
            PutEquipItems(top, Links.Interface("Item_Total2"), groups[8], groups[9]);
        }
        ScienceGroup GetBuildingGroup(GameScience science)
        {
            GSBuilding building = Links.Buildings[science.ID];
            switch (building.Sector)
            {
                case SectorTypes.Live: return ScienceGroup.BuildLive;
                case SectorTypes.Money: return ScienceGroup.BuildBank;
                case SectorTypes.Metall: return ScienceGroup.BuildMetal;
                case SectorTypes.MetalCap: return ScienceGroup.BuildMetalCap;
                case SectorTypes.Chips: return ScienceGroup.BuildChip;
                case SectorTypes.ChipsCap: return ScienceGroup.BuildChipCap;
                case SectorTypes.Anti: return ScienceGroup.BuildAnti;
                case SectorTypes.AntiCap: return ScienceGroup.BuildAntiCap;
                case SectorTypes.Repair: return ScienceGroup.BuildRepair;
                case SectorTypes.War: if (building.Type == BuildingType.Radar) return ScienceGroup.BuildRadar;
                    else if (building.Type == BuildingType.DataCenter) return ScienceGroup.BuildData;
                    else return ScienceGroup.BuildEducation;
                default: return ScienceGroup.Error;
            }
        }
        ScienceGroup GetShipTypeGroup(GameScience science)
        {
            ShipTypeclass type = Links.ShipTypes[science.ID];
            switch (type.Type)
            {
                case ShipGenerator2Types.Scout: return ScienceGroup.Ship_Scout;
                case ShipGenerator2Types.Corvett: return ScienceGroup.Ship_Corvette;
                case ShipGenerator2Types.Cargo: return ScienceGroup.Ship_Transp;
                case ShipGenerator2Types.Linkor: return ScienceGroup.Ship_Battle;
                case ShipGenerator2Types.Frigate: return ScienceGroup.Ship_Frigate;
                case ShipGenerator2Types.Fighter: return ScienceGroup.Ship_Fighter;
                case ShipGenerator2Types.Dreadnought: return ScienceGroup.Ship_Dreadnought;
                case ShipGenerator2Types.Devostator: return ScienceGroup.Ship_Devostator;
                case ShipGenerator2Types.Warrior: return ScienceGroup.Ship_Warrior;
                case ShipGenerator2Types.Cruiser: return ScienceGroup.Ship_Cruiser;
                default: return ScienceGroup.Error;
            }
        }
        ScienceGroup GetWeaponGroup(GameScience science)
        {
            Weaponclass weapon = Links.WeaponTypes[science.ID];
            switch (weapon.Type)
            {
                case EWeaponType.Laser: if (weapon.Size == ItemSize.Small) return ScienceGroup.Laser_S; else if (weapon.Size == ItemSize.Medium) return ScienceGroup.Laser_M; else return ScienceGroup.Laser_L;
                case EWeaponType.EMI: if (weapon.Size == ItemSize.Small) return ScienceGroup.EMI_S; else if (weapon.Size == ItemSize.Medium) return ScienceGroup.EMI_M; else return ScienceGroup.EMI_L;
                case EWeaponType.Plasma: if (weapon.Size == ItemSize.Small) return ScienceGroup.Plasma_S; else if (weapon.Size == ItemSize.Medium) return ScienceGroup.Plasma_M; else return ScienceGroup.Plasma_L;
                case EWeaponType.Solar: if (weapon.Size == ItemSize.Small) return ScienceGroup.Solar_S; else if (weapon.Size == ItemSize.Medium) return ScienceGroup.Solar_M; else return ScienceGroup.Solar_L;
                case EWeaponType.Cannon: if (weapon.Size == ItemSize.Small) return ScienceGroup.Cannon_S; else if (weapon.Size == ItemSize.Medium) return ScienceGroup.Cannon_M; else return ScienceGroup.Cannon_L;
                case EWeaponType.Gauss: if (weapon.Size == ItemSize.Small) return ScienceGroup.Gauss_S; else if (weapon.Size == ItemSize.Medium) return ScienceGroup.Gauss_M; else return ScienceGroup.Gauss_L;
                case EWeaponType.Missle: if (weapon.Size == ItemSize.Small) return ScienceGroup.Missle_S; else if (weapon.Size == ItemSize.Medium) return ScienceGroup.Missle_M; else return ScienceGroup.Missle_L;
                case EWeaponType.AntiMatter: if (weapon.Size == ItemSize.Small) return ScienceGroup.Anti_S; else if (weapon.Size == ItemSize.Medium) return ScienceGroup.Anti_M; else return ScienceGroup.Anti_L;
                case EWeaponType.Psi: if (weapon.Size == ItemSize.Small) return ScienceGroup.Psi_S; else if (weapon.Size == ItemSize.Medium) return ScienceGroup.Psi_M; else return ScienceGroup.Psi_L;
                case EWeaponType.Dark: if (weapon.Size == ItemSize.Small) return ScienceGroup.Dark_S; else if (weapon.Size == ItemSize.Medium) return ScienceGroup.Dark_M; else return ScienceGroup.Dark_L;
                case EWeaponType.Warp: if (weapon.Size == ItemSize.Small) return ScienceGroup.Warp_S; else if (weapon.Size == ItemSize.Medium) return ScienceGroup.Warp_M; else return ScienceGroup.Warp_L;
                case EWeaponType.Time: if (weapon.Size == ItemSize.Small) return ScienceGroup.Time_S; else if (weapon.Size == ItemSize.Medium) return ScienceGroup.Time_M; else return ScienceGroup.Time_M;
                case EWeaponType.Slicing: if (weapon.Size == ItemSize.Small) return ScienceGroup.Slice_S; else if (weapon.Size == ItemSize.Medium) return ScienceGroup.Slice_M; else return ScienceGroup.Slice_L;
                case EWeaponType.Radiation: if (weapon.Size == ItemSize.Small) return ScienceGroup.Rad_S; else if (weapon.Size == ItemSize.Medium) return ScienceGroup.Rad_M; else return ScienceGroup.Rad_L;
                case EWeaponType.Drone: if (weapon.Size == ItemSize.Small) return ScienceGroup.Drone_S; else if (weapon.Size == ItemSize.Medium) return ScienceGroup.Drone_M; else return ScienceGroup.Drone_L;
                case EWeaponType.Magnet: if (weapon.Size == ItemSize.Small) return ScienceGroup.Magnet_S; else if (weapon.Size == ItemSize.Medium) return ScienceGroup.Magnet_M; else return ScienceGroup.Magnet_L;
                default: return ScienceGroup.Error;
            }
        }
        ScienceGroup GetGeneratorGroup(GameScience science)
        {
            Generatorclass generator = Links.GeneratorTypes[science.ID];
            switch (generator.GetSizeDescription())
            {
                case 0: return ScienceGroup.Generator_R_S;
                case 5: return ScienceGroup.Generator_S_S;
                case 10: return ScienceGroup.Generator_R_M;
                case 15: return ScienceGroup.Generator_S_M;
                case 20: return ScienceGroup.Generator_R_L;
                case 25: return ScienceGroup.Generator_S_L;
                default: return ScienceGroup.Error;
            }
        }
        ScienceGroup GetShieldTypeGroup(GameScience science)
        {
            Shieldclass shield = Links.ShieldTypes[science.ID];
            switch (shield.GetSizeDescription())
            {
                case 0: return ScienceGroup.Shield_R_S;
                case 5: return ScienceGroup.Shield_S_S;
                case 10: return ScienceGroup.Shield_R_M;
                case 15: return ScienceGroup.Shield_S_M;
                case 20: return ScienceGroup.Shield_R_L;
                case 25: return ScienceGroup.Shield_S_L;
                default: return ScienceGroup.Error;
            }
        }
        ScienceGroup GetComputerGroup(GameScience science)
        {
            Computerclass comp = Links.ComputerTypes[science.ID];
            switch (comp.GetType())
            {
                case 0: if (comp.Size == ItemSize.Small) return ScienceGroup.Comp_B_S; else if (comp.Size == ItemSize.Medium) return ScienceGroup.Comp_B_M; else return ScienceGroup.Comp_B_L;
                case 1: if (comp.Size == ItemSize.Small) return ScienceGroup.Comp_I_E_S; else if (comp.Size == ItemSize.Medium) return ScienceGroup.Comp_I_E_M; else return ScienceGroup.Comp_I_E_L;
                case 2: if (comp.Size == ItemSize.Small) return ScienceGroup.Comp_I_P_S; else if (comp.Size == ItemSize.Medium) return ScienceGroup.Comp_I_P_M; else return ScienceGroup.Comp_I_P_L;
                case 3: if (comp.Size == ItemSize.Small) return ScienceGroup.Comp_I_I_S; else if (comp.Size == ItemSize.Medium) return ScienceGroup.Comp_I_I_M; else return ScienceGroup.Comp_I_I_L;
                case 4: if (comp.Size == ItemSize.Small) return ScienceGroup.Comp_I_C_S; else if (comp.Size == ItemSize.Medium) return ScienceGroup.Comp_I_C_M; else return ScienceGroup.Comp_I_C_L;
                case 5: if (comp.Size == ItemSize.Small) return ScienceGroup.Comp_E_E_S; else if (comp.Size == ItemSize.Medium) return ScienceGroup.Comp_E_E_M; else return ScienceGroup.Comp_E_E_L;
                case 6: if (comp.Size == ItemSize.Small) return ScienceGroup.Comp_E_P_S; else if (comp.Size == ItemSize.Medium) return ScienceGroup.Comp_E_P_M; else return ScienceGroup.Comp_E_P_L;
                case 7: if (comp.Size == ItemSize.Small) return ScienceGroup.Comp_E_I_S; else if (comp.Size == ItemSize.Medium) return ScienceGroup.Comp_E_I_M; else return ScienceGroup.Comp_E_I_L;
                case 8: if (comp.Size == ItemSize.Small) return ScienceGroup.Comp_E_C_S; else if (comp.Size == ItemSize.Medium) return ScienceGroup.Comp_E_C_M; else return ScienceGroup.Comp_E_C_L;
                default: return ScienceGroup.Error;
            }
        }
        ScienceGroup GetEquipmentGroup(GameScience science)
        {
            Equipmentclass equip = Links.EquipmentTypes[science.ID];
            switch (equip.Type)
            {
                case 0: if (equip.Size == ItemSize.Small) return ScienceGroup.Acc_E_S; else return ScienceGroup.Acc_E_M;
                case 1: if (equip.Size == ItemSize.Small) return ScienceGroup.Acc_P_S; else return ScienceGroup.Acc_P_M;
                case 2: if (equip.Size == ItemSize.Small) return ScienceGroup.Acc_I_S; else return ScienceGroup.Acc_I_M;
                case 3: if (equip.Size == ItemSize.Small) return ScienceGroup.Acc_C_S; else return ScienceGroup.Acc_C_M;
                case 4: if (equip.Size == ItemSize.Small) return ScienceGroup.Acc_A_S; else return ScienceGroup.Acc_A_M;
                case 5: if (equip.Size == ItemSize.Small) return ScienceGroup.Health_S; else return ScienceGroup.Health_M;
                case 6: if (equip.Size == ItemSize.Small) return ScienceGroup.Reg_H_S; else return ScienceGroup.Reg_H_M;
                case 7: if (equip.Size == ItemSize.Small) return ScienceGroup.Shield_S; else return ScienceGroup.Shield_M;
                case 8: if (equip.Size == ItemSize.Small) return ScienceGroup.Reg_S_S; else return ScienceGroup.Reg_S_M;
                case 9: if (equip.Size == ItemSize.Small) return ScienceGroup.Energy_S; else return ScienceGroup.Energy_M;
                case 10: if (equip.Size == ItemSize.Small) return ScienceGroup.Reg_E_S; else return ScienceGroup.Reg_E_M;
                case 11: if (equip.Size == ItemSize.Small) return ScienceGroup.Eva_E_S; else return ScienceGroup.Eva_E_M;
                case 12: if (equip.Size == ItemSize.Small) return ScienceGroup.Eva_P_S; else return ScienceGroup.Eva_P_M;
                case 13: if (equip.Size == ItemSize.Small) return ScienceGroup.Eva_I_S; else return ScienceGroup.Eva_I_M;
                case 14: if (equip.Size == ItemSize.Small) return ScienceGroup.Eva_C_S; else return ScienceGroup.Eva_C_M;
                case 15: if (equip.Size == ItemSize.Small) return ScienceGroup.Eva_A_S; else return ScienceGroup.Eva_A_M;
                case 16: if (equip.Size == ItemSize.Small) return ScienceGroup.Ign_E_S; else return ScienceGroup.Ign_E_M;
                case 17: if (equip.Size == ItemSize.Small) return ScienceGroup.Ign_P_S; else return ScienceGroup.Ign_P_M;
                case 18: if (equip.Size == ItemSize.Small) return ScienceGroup.Ign_I_S; else return ScienceGroup.Ign_I_M;
                case 19: if (equip.Size == ItemSize.Small) return ScienceGroup.Ign_C_S; else return ScienceGroup.Ign_C_M;
                case 20: if (equip.Size == ItemSize.Small) return ScienceGroup.Ign_A_S; else return ScienceGroup.Ign_A_M;
                case 21: if (equip.Size == ItemSize.Small) return ScienceGroup.Dam_E_S; else return ScienceGroup.Dam_E_M;
                case 22: if (equip.Size == ItemSize.Small) return ScienceGroup.Dam_P_S; else return ScienceGroup.Dam_P_M;
                case 23: if (equip.Size == ItemSize.Small) return ScienceGroup.Dam_I_S; else return ScienceGroup.Dam_I_M;
                case 24: if (equip.Size == ItemSize.Small) return ScienceGroup.Dam_C_S; else return ScienceGroup.Dam_C_M;
                case 25: if (equip.Size == ItemSize.Small) return ScienceGroup.Dam_A_S; else return ScienceGroup.Dam_A_M;
                case 30: return ScienceGroup.Health_L;
                case 31: return ScienceGroup.Reg_H_L;
                case 32: return ScienceGroup.Shield_L;
                case 33: return ScienceGroup.Reg_S_L;
                case 34: return ScienceGroup.Energy_L;
                case 35: return ScienceGroup.Reg_E_L;
                case 36: return ScienceGroup.Acc_E_L;
                case 37: return ScienceGroup.Acc_P_L;
                case 38: return ScienceGroup.Acc_I_L;
                case 39: return ScienceGroup.Acc_C_L;
                case 40: return ScienceGroup.Acc_A_L;
                case 41: return ScienceGroup.Eva_E_L;
                case 42: return ScienceGroup.Eva_P_L;
                case 43: return ScienceGroup.Eva_I_L;
                case 44: return ScienceGroup.Eva_C_L;
                case 45: return ScienceGroup.Eva_A_L;
                case 46: return ScienceGroup.Ign_E_L;
                case 47: return ScienceGroup.Ign_P_L;
                case 48: return ScienceGroup.Ign_I_L;
                case 49: return ScienceGroup.Ign_C_L;
                case 50: return ScienceGroup.Ign_A_L;
                case 51: return ScienceGroup.Dam_E_L;
                case 52: return ScienceGroup.Dam_P_L;
                case 53: return ScienceGroup.Dam_I_L;
                case 54: return ScienceGroup.Dam_C_L;
                case 55: return ScienceGroup.Dam_A_L;
                case 57: if (equip.Size == ItemSize.Small) return ScienceGroup.Cargo_S; else if (equip.Size == ItemSize.Medium) return ScienceGroup.Cargo_M; else return ScienceGroup.Cargo_L;
                case 58: if (equip.Size == ItemSize.Small) return ScienceGroup.Colony_S; else if (equip.Size == ItemSize.Medium) return ScienceGroup.Colony_M; else return ScienceGroup.Colony_L;
                case 59: if (equip.Size == ItemSize.Small) return ScienceGroup.Imm_E_S; else if (equip.Size == ItemSize.Medium) return ScienceGroup.Imm_E_M; else return ScienceGroup.Imm_E_L;
                case 60: if (equip.Size == ItemSize.Small) return ScienceGroup.Imm_P_S; else if (equip.Size == ItemSize.Medium) return ScienceGroup.Imm_P_M; else return ScienceGroup.Imm_P_L;
                case 61: if (equip.Size == ItemSize.Small) return ScienceGroup.Imm_I_S; else if (equip.Size == ItemSize.Medium) return ScienceGroup.Imm_I_M; else return ScienceGroup.Imm_I_L;
                case 62: if (equip.Size == ItemSize.Small) return ScienceGroup.Imm_C_S; else if (equip.Size == ItemSize.Medium) return ScienceGroup.Imm_C_M; else return ScienceGroup.Imm_C_L;
                case 63: if (equip.Size == ItemSize.Small) return ScienceGroup.Imm_A_S; else if (equip.Size == ItemSize.Medium) return ScienceGroup.Imm_A_M; else return ScienceGroup.Imm_A_L;
                default: return ScienceGroup.Error;
            }
        }
        ScienceGroup GetEngineGroup(GameScience science)
        {
            Engineclass engine = Links.EngineTypes[science.ID];
            switch (engine.GetSizeDescription())
            {
                case 0: return ScienceGroup.Engine_U_S;
                case 5: return ScienceGroup.Engine_B_S;
                case 10: return ScienceGroup.Engine_U_M;
                case 15: return ScienceGroup.Engine_B_M;
                case 20: return ScienceGroup.Engine_U_L;
                case 25: return ScienceGroup.Engine_B_L;
                default: return ScienceGroup.Error;
            }
        }
    }
    class ScienceCanvas:Canvas
    {
        StackPanel ScienceListPanel;
        SortedSet<double> OpenedItems;
        LearnScienceButton Energy;
        LearnScienceButton Physic;
        LearnScienceButton Irregular;
        LearnScienceButton Cybernetic;
        ScienceFilter Filter;
        public GameScience FocusedScience;
        TreeViewItem Focuseditem;
        Border InfoBorder;
        SortedList<ScienceGroup, SortedSet<ushort>> ScienceGroups;
        Viewbox box;
        public ScienceCanvas()
        {
            box = new Viewbox();
            box.Width = 1230;
            box.Height = 520;
            box.HorizontalAlignment = HorizontalAlignment.Center;
            box.VerticalAlignment = VerticalAlignment.Center;
            box.Child = this;
            Width = 1280;
            Height = 600;
            put();
        }
        void put()
        {
            

            Border ScienceListBorder = new Border(); //первая колонка
            ScienceListBorder.BorderBrush = Brushes.Black;
            ScienceListBorder.BorderThickness = new Thickness(2);
            ScienceListBorder.Margin = new Thickness(2);
            Children.Add(ScienceListBorder);
            ScienceListBorder.Height = 606;
            ScienceListBorder.Width = 400;

            StackPanel ScienceListTopPanel = new StackPanel();
            ScienceListTopPanel.Orientation = Orientation.Vertical;
            ScienceListBorder.Child = ScienceListTopPanel;

            Filter = new ScienceFilter(new string[] { Links.Interface("ScienceLevel"), Links.Interface("ScienceType") });
            ScienceListTopPanel.Children.Add(Filter);

            ScrollViewer ScienceListViewer = new ScrollViewer();
            ScienceListViewer.PreviewMouseWheel += ScienceListViewer_PreviewMouseWheel;
            ScienceListViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            ScienceListViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            ScienceListViewer.Width = 390;
            ScienceListViewer.Height = 530;
            ScienceListTopPanel.Children.Add(ScienceListViewer);

            ScienceListPanel = new StackPanel();
            ScienceListPanel.Orientation = Orientation.Vertical;
            ScienceListViewer.Content = ScienceListPanel;

            OpenedItems = new SortedSet<double>();

            InfoBorder = new Border(); //вторая колонка
            InfoBorder.BorderBrush = Brushes.Black;
            InfoBorder.BorderThickness = new Thickness(2);
            InfoBorder.Margin = new Thickness(2);
            Children.Add(InfoBorder);
            InfoBorder.Height = 606;
            Canvas.SetLeft(InfoBorder, 425);

            Border ButtonBorder = new Border(); //третья колонка
            ButtonBorder.BorderBrush = Brushes.Black;
            ButtonBorder.BorderThickness = new Thickness(2);
            ButtonBorder.Margin = new Thickness(2);
            Children.Add(ButtonBorder);
            ButtonBorder.Height = 606;
            Canvas.SetLeft(ButtonBorder, 900);

            StackPanel ButtonPanel = new StackPanel();
            ButtonPanel.Orientation = Orientation.Vertical;
            ButtonBorder.Child = ButtonPanel;

            Energy = new LearnScienceButton(EScienceField.Energy);
            ButtonPanel.Children.Add(Energy);
            Physic = new LearnScienceButton(EScienceField.Physic);
            ButtonPanel.Children.Add(Physic);
            Irregular = new LearnScienceButton(EScienceField.Irregular);
            ButtonPanel.Children.Add(Irregular);
            Cybernetic = new LearnScienceButton(EScienceField.Cybernetic);
            ButtonPanel.Children.Add(Cybernetic);
            FocusedTimes = new System.Windows.Threading.DispatcherTimer();
            FocusedTimes.Tick += new EventHandler(FocusedTimes_Tick);
        }

        private void ScienceListViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer viewer = (ScrollViewer)sender;
            if (e.Delta > 0)
                viewer.PageUp();
            else
                viewer.PageDown();
        }



        public void Select()
        {
            Links.Controller.MainCanvas.LowBorder.Child = box;
            Refresh();
        }

        public void Refresh()
        {
            Gets.GetTotalInfo("После открытия панели науки 2");
            ScienceAnalys.Fill2();
            Gets.GetSciencePriceBases();
            Focuseditem = null;
            Draw();
        }
        public void Draw()
        {
            if (Filter.curvalue == 0)
                PutBasicParameters();
            else
                PutBasicParametersGroups();
        }
        public void PutBasicParametersGroups()
        {
            FillGroups();
            ScienceListPanel.Children.Clear();

            TreeView item = new TreeView();
            item.FontSize = 24;
            item.FontFamily = Links.Font;
            item.FontWeight = FontWeights.Bold;
            item.Background = Brushes.Black;
            ScienceListPanel.Children.Add(item);
            TreeViewItem buildings = PutTypeItemGroup(Links.Interface("Buildings"), item, ScienceGroup.None);
            TreeViewItem common_buildings = PutTypeItemGroup("Общие постройки", buildings, ScienceGroup.None);
            PutTypeItemGroup("Жилые дома", common_buildings, ScienceGroup.BuildLive);
            PutTypeItemGroup("Ремонтные цеха", common_buildings, ScienceGroup.BuildRepair);
            TreeViewItem res_buildings = PutTypeItemGroup("Ресурсные постройки", buildings, ScienceGroup.None);
            PutTypeItemGroup("Банки", res_buildings, ScienceGroup.BuildBank);
            PutTypeItemGroup("Шахты", res_buildings, ScienceGroup.BuildMetal);
            PutTypeItemGroup("Фабрики микросхем", res_buildings, ScienceGroup.BuildChip);
            PutTypeItemGroup("Ускорители частиц", res_buildings, ScienceGroup.BuildAnti);
            TreeViewItem cap_buildings = PutTypeItemGroup("Складские постройки", buildings, ScienceGroup.None);
            PutTypeItemGroup("Склады металла", cap_buildings, ScienceGroup.BuildMetalCap);
            PutTypeItemGroup("Склады микросхем", cap_buildings, ScienceGroup.BuildChipCap);
            PutTypeItemGroup("Склады антиматерии", cap_buildings, ScienceGroup.BuildAntiCap);
            TreeViewItem war_buildings = PutTypeItemGroup("Военные постройки", buildings, ScienceGroup.None);
            PutTypeItemGroup("Радары", war_buildings, ScienceGroup.BuildRadar);
            PutTypeItemGroup("Дата центры", war_buildings, ScienceGroup.BuildData);
            PutTypeItemGroup("Учебные центры", war_buildings, ScienceGroup.BuildEducation);
            
            TreeViewItem shiptypes = PutTypeItemGroup(Links.Interface("ShipTypes"), item, ScienceGroup.None);
            PutTypeItemGroup(Links.Interface("Ship_Scout"), shiptypes, ScienceGroup.Ship_Scout);
            PutTypeItemGroup(Links.Interface("Ship_Corvette"), shiptypes, ScienceGroup.Ship_Corvette);
            PutTypeItemGroup(Links.Interface("Ship_Transport"), shiptypes, ScienceGroup.Ship_Transp);
            PutTypeItemGroup(Links.Interface("Ship_Battle"), shiptypes, ScienceGroup.Ship_Battle);
            PutTypeItemGroup(Links.Interface("Ship_Frigate"), shiptypes, ScienceGroup.Ship_Frigate);
            PutTypeItemGroup(Links.Interface("Ship_Fighter"), shiptypes, ScienceGroup.Ship_Fighter);
            PutTypeItemGroup(Links.Interface("Ship_Dreadnought"), shiptypes, ScienceGroup.Ship_Dreadnought);
            PutTypeItemGroup(Links.Interface("Ship_Devostator"), shiptypes, ScienceGroup.Ship_Devostator);
            PutTypeItemGroup(Links.Interface("Ship_Warrior"), shiptypes, ScienceGroup.Ship_Warrior);
            PutTypeItemGroup(Links.Interface("Ship_Cruiser"), shiptypes, ScienceGroup.Ship_Cruiser);

            TreeViewItem generators = PutTypeItemGroup(Links.Interface("Generators"), item, ScienceGroup.None);
            TreeViewItem[] generators_sizes = PutTypeItems(generators, Links.Interface("Item_Generators"));
            PutTypeItemGroup(GameObjectName.GeneratorsName[5][Links.Lang], generators_sizes[0], ScienceGroup.Generator_S_S);
            PutTypeItemGroup(GameObjectName.GeneratorsName[0][Links.Lang], generators_sizes[0], ScienceGroup.Generator_R_S);
            PutTypeItemGroup(GameObjectName.GeneratorsName[15][Links.Lang], generators_sizes[1], ScienceGroup.Generator_S_M);
            PutTypeItemGroup(GameObjectName.GeneratorsName[10][Links.Lang], generators_sizes[1], ScienceGroup.Generator_R_M);
            PutTypeItemGroup(GameObjectName.GeneratorsName[25][Links.Lang], generators_sizes[2], ScienceGroup.Generator_S_L);
            PutTypeItemGroup(GameObjectName.GeneratorsName[20][Links.Lang], generators_sizes[2], ScienceGroup.Generator_R_L);

            TreeViewItem shields = PutTypeItemGroup(Links.Interface("Shields"), item, ScienceGroup.None);
            TreeViewItem[] shields_sizes = PutTypeItems(shields, Links.Interface("Item_Shields"));
            PutTypeItemGroup(GameObjectName.ShieldsName[5][Links.Lang], shields_sizes[0], ScienceGroup.Shield_S_S);
            PutTypeItemGroup(GameObjectName.ShieldsName[0][Links.Lang], shields_sizes[0], ScienceGroup.Shield_R_S);
            PutTypeItemGroup(GameObjectName.ShieldsName[15][Links.Lang], shields_sizes[1], ScienceGroup.Shield_S_M);
            PutTypeItemGroup(GameObjectName.ShieldsName[10][Links.Lang], shields_sizes[1], ScienceGroup.Shield_R_M);
            PutTypeItemGroup(GameObjectName.ShieldsName[25][Links.Lang], shields_sizes[2], ScienceGroup.Shield_S_L);
            PutTypeItemGroup(GameObjectName.ShieldsName[20][Links.Lang], shields_sizes[2], ScienceGroup.Shield_R_L);

            TreeViewItem computers = PutTypeItemGroup(Links.Interface("Computers"), item, ScienceGroup.None);
            TreeViewItem[] comp_sizes = PutTypeItems(computers, Links.Interface("Item_Computers"));
            TreeViewItem[] comp_smalls = PutFiveTypeItems(comp_sizes[0], Links.Interface("Item_Small"), Links.Interface("Item_Computers"));
            PutTypeItemGroup(GameObjectName.ComputersTargetName[0][Links.Lang] + " " + GameObjectName.ComputersName[3][Links.Lang], comp_smalls[0], ScienceGroup.Comp_I_E_S);
            PutTypeItemGroup(GameObjectName.ComputersTargetName[0][Links.Lang] + " " + GameObjectName.ComputersName[6][Links.Lang], comp_smalls[0], ScienceGroup.Comp_E_E_S);
            PutTypeItemGroup(GameObjectName.ComputersTargetName[1][Links.Lang] + " " + GameObjectName.ComputersName[3][Links.Lang], comp_smalls[1], ScienceGroup.Comp_I_P_S);
            PutTypeItemGroup(GameObjectName.ComputersTargetName[1][Links.Lang] + " " + GameObjectName.ComputersName[6][Links.Lang], comp_smalls[1], ScienceGroup.Comp_E_P_S);
            PutTypeItemGroup(GameObjectName.ComputersTargetName[2][Links.Lang] + " " + GameObjectName.ComputersName[3][Links.Lang], comp_smalls[2], ScienceGroup.Comp_I_I_S);
            PutTypeItemGroup(GameObjectName.ComputersTargetName[2][Links.Lang] + " " + GameObjectName.ComputersName[6][Links.Lang], comp_smalls[2], ScienceGroup.Comp_E_I_S);
            PutTypeItemGroup(GameObjectName.ComputersTargetName[3][Links.Lang] + " " + GameObjectName.ComputersName[3][Links.Lang], comp_smalls[3], ScienceGroup.Comp_I_C_S);
            PutTypeItemGroup(GameObjectName.ComputersTargetName[3][Links.Lang] + " " + GameObjectName.ComputersName[6][Links.Lang], comp_smalls[3], ScienceGroup.Comp_E_C_S);
            PutTypeItemGroup(GameObjectName.ComputersName[0][Links.Lang], comp_smalls[4], ScienceGroup.Comp_B_S);
            TreeViewItem[] comp_med = PutFiveTypeItems(comp_sizes[1], Links.Interface("Item_Medium"), Links.Interface("Item_Computers"));
            PutTypeItemGroup(GameObjectName.ComputersTargetName[0][Links.Lang] + " " + GameObjectName.ComputersName[4][Links.Lang], comp_med[0], ScienceGroup.Comp_I_E_M);
            PutTypeItemGroup(GameObjectName.ComputersTargetName[0][Links.Lang] + " " + GameObjectName.ComputersName[7][Links.Lang], comp_med[0], ScienceGroup.Comp_E_E_M);
            PutTypeItemGroup(GameObjectName.ComputersTargetName[1][Links.Lang] + " " + GameObjectName.ComputersName[4][Links.Lang], comp_med[1], ScienceGroup.Comp_I_P_M);
            PutTypeItemGroup(GameObjectName.ComputersTargetName[1][Links.Lang] + " " + GameObjectName.ComputersName[7][Links.Lang], comp_med[1], ScienceGroup.Comp_E_P_M);
            PutTypeItemGroup(GameObjectName.ComputersTargetName[2][Links.Lang] + " " + GameObjectName.ComputersName[4][Links.Lang], comp_med[2], ScienceGroup.Comp_I_I_M);
            PutTypeItemGroup(GameObjectName.ComputersTargetName[2][Links.Lang] + " " + GameObjectName.ComputersName[7][Links.Lang], comp_med[2], ScienceGroup.Comp_E_I_M);
            PutTypeItemGroup(GameObjectName.ComputersTargetName[3][Links.Lang] + " " + GameObjectName.ComputersName[4][Links.Lang], comp_med[3], ScienceGroup.Comp_I_C_M);
            PutTypeItemGroup(GameObjectName.ComputersTargetName[3][Links.Lang] + " " + GameObjectName.ComputersName[7][Links.Lang], comp_med[3], ScienceGroup.Comp_E_C_M);
            PutTypeItemGroup(GameObjectName.ComputersName[1][Links.Lang], comp_med[4], ScienceGroup.Comp_B_M);
            TreeViewItem[] comp_large = PutFiveTypeItems(comp_sizes[2], Links.Interface("Item_Large"), Links.Interface("Item_Computers"));
            PutTypeItemGroup(GameObjectName.ComputersTargetName[0][Links.Lang] + " " + GameObjectName.ComputersName[5][Links.Lang], comp_large[0], ScienceGroup.Comp_I_E_L);
            PutTypeItemGroup(GameObjectName.ComputersTargetName[0][Links.Lang] + " " + GameObjectName.ComputersName[8][Links.Lang], comp_large[0], ScienceGroup.Comp_E_E_L);
            PutTypeItemGroup(GameObjectName.ComputersTargetName[1][Links.Lang] + " " + GameObjectName.ComputersName[5][Links.Lang], comp_large[1], ScienceGroup.Comp_I_P_L);
            PutTypeItemGroup(GameObjectName.ComputersTargetName[1][Links.Lang] + " " + GameObjectName.ComputersName[8][Links.Lang], comp_large[1], ScienceGroup.Comp_E_P_L);
            PutTypeItemGroup(GameObjectName.ComputersTargetName[2][Links.Lang] + " " + GameObjectName.ComputersName[5][Links.Lang], comp_large[2], ScienceGroup.Comp_I_I_L);
            PutTypeItemGroup(GameObjectName.ComputersTargetName[2][Links.Lang] + " " + GameObjectName.ComputersName[8][Links.Lang], comp_large[2], ScienceGroup.Comp_E_I_L);
            PutTypeItemGroup(GameObjectName.ComputersTargetName[3][Links.Lang] + " " + GameObjectName.ComputersName[5][Links.Lang], comp_large[3], ScienceGroup.Comp_I_C_L);
            PutTypeItemGroup(GameObjectName.ComputersTargetName[3][Links.Lang] + " " + GameObjectName.ComputersName[8][Links.Lang], comp_large[3], ScienceGroup.Comp_E_C_L);
            PutTypeItemGroup(GameObjectName.ComputersName[2][Links.Lang], comp_large[4], ScienceGroup.Comp_B_L);

            TreeViewItem engines = PutTypeItemGroup(Links.Interface("Engines"), item, ScienceGroup.None);
            TreeViewItem[] engines_sizes = PutTypeItems(engines, Links.Interface("Item_Engines"));
            PutTypeItemGroup(GameObjectName.EnginesName[5][Links.Lang], engines_sizes[0], ScienceGroup.Engine_B_S);
            PutTypeItemGroup(GameObjectName.EnginesName[0][Links.Lang], engines_sizes[0], ScienceGroup.Engine_U_S);
            PutTypeItemGroup(GameObjectName.EnginesName[15][Links.Lang], engines_sizes[1], ScienceGroup.Engine_B_M);
            PutTypeItemGroup(GameObjectName.EnginesName[10][Links.Lang], engines_sizes[1], ScienceGroup.Engine_U_M);
            PutTypeItemGroup(GameObjectName.EnginesName[25][Links.Lang], engines_sizes[2], ScienceGroup.Engine_B_L);
            PutTypeItemGroup(GameObjectName.EnginesName[20][Links.Lang], engines_sizes[2], ScienceGroup.Engine_U_L);

            TreeViewItem weapons = PutTypeItemGroup(Links.Interface("Weapons"), item, ScienceGroup.None);
            TreeViewItem energy_weapon = PutTypeItemGroup(Links.Interface("EnergyWeapon"), weapons, ScienceGroup.None);
            TreeViewItem laser = PutTypeItemGroup(GameObjectName.GetWeaponGroupName(0, EWeaponType.Laser), energy_weapon, ScienceGroup.None);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(1, EWeaponType.Laser), laser, ScienceGroup.Laser_S);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(2, EWeaponType.Laser), laser, ScienceGroup.Laser_M);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(3, EWeaponType.Laser), laser, ScienceGroup.Laser_L);
            TreeViewItem emi = PutTypeItemGroup(GameObjectName.GetWeaponGroupName(0, EWeaponType.EMI), energy_weapon, ScienceGroup.None);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(1, EWeaponType.EMI), emi, ScienceGroup.EMI_S);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(2, EWeaponType.EMI), emi, ScienceGroup.EMI_M);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(3, EWeaponType.EMI), emi, ScienceGroup.EMI_L);
            TreeViewItem plasma = PutTypeItemGroup(GameObjectName.GetWeaponGroupName(0, EWeaponType.Plasma), energy_weapon, ScienceGroup.None);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(1, EWeaponType.Plasma), plasma, ScienceGroup.Plasma_S);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(2, EWeaponType.Plasma), plasma, ScienceGroup.Plasma_M);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(3, EWeaponType.Plasma), plasma, ScienceGroup.Plasma_L);
            TreeViewItem solar = PutTypeItemGroup(GameObjectName.GetWeaponGroupName(0, EWeaponType.Solar), energy_weapon, ScienceGroup.None);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(1, EWeaponType.Solar), solar, ScienceGroup.Solar_S);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(2, EWeaponType.Solar), solar, ScienceGroup.Solar_M);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(3, EWeaponType.Solar), solar, ScienceGroup.Solar_L);
            TreeViewItem physic_weapon = PutTypeItemGroup(Links.Interface("PhysicWeapon"), weapons, ScienceGroup.None);
            TreeViewItem cannon = PutTypeItemGroup(GameObjectName.GetWeaponGroupName(0, EWeaponType.Cannon), physic_weapon, ScienceGroup.None);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(1, EWeaponType.Cannon), cannon, ScienceGroup.Cannon_S);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(2, EWeaponType.Cannon), cannon, ScienceGroup.Cannon_M);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(3, EWeaponType.Cannon), cannon, ScienceGroup.Cannon_L);
            TreeViewItem gauss = PutTypeItemGroup(GameObjectName.GetWeaponGroupName(0, EWeaponType.Gauss), physic_weapon, ScienceGroup.None);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(1, EWeaponType.Gauss), gauss, ScienceGroup.Gauss_S);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(2, EWeaponType.Gauss), gauss, ScienceGroup.Gauss_M);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(3, EWeaponType.Gauss), gauss, ScienceGroup.Gauss_L);
            TreeViewItem missle = PutTypeItemGroup(GameObjectName.GetWeaponGroupName(0, EWeaponType.Missle), physic_weapon, ScienceGroup.None);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(1, EWeaponType.Missle), missle, ScienceGroup.Missle_S);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(2, EWeaponType.Missle), missle, ScienceGroup.Missle_M);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(3, EWeaponType.Missle), missle, ScienceGroup.Missle_L);
            TreeViewItem anti = PutTypeItemGroup(GameObjectName.GetWeaponGroupName(0, EWeaponType.AntiMatter), physic_weapon, ScienceGroup.None);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(1, EWeaponType.AntiMatter), anti, ScienceGroup.Anti_S);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(2, EWeaponType.AntiMatter), anti, ScienceGroup.Anti_M);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(3, EWeaponType.AntiMatter), anti, ScienceGroup.Anti_L);
            TreeViewItem irr_weapon = PutTypeItemGroup(Links.Interface("IrregularWeapon"), weapons, ScienceGroup.None);
            TreeViewItem psi = PutTypeItemGroup(GameObjectName.GetWeaponGroupName(0, EWeaponType.Psi), irr_weapon, ScienceGroup.None);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(1, EWeaponType.Psi), psi, ScienceGroup.Psi_S);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(2, EWeaponType.Psi), psi, ScienceGroup.Psi_M);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(3, EWeaponType.Psi), psi, ScienceGroup.Psi_L);
            TreeViewItem dark = PutTypeItemGroup(GameObjectName.GetWeaponGroupName(0, EWeaponType.Dark), irr_weapon, ScienceGroup.None);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(1, EWeaponType.Dark), dark, ScienceGroup.Dark_S);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(2, EWeaponType.Dark), dark, ScienceGroup.Dark_M);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(3, EWeaponType.Dark), dark, ScienceGroup.Dark_L);
            TreeViewItem warp = PutTypeItemGroup(GameObjectName.GetWeaponGroupName(0, EWeaponType.Warp), irr_weapon, ScienceGroup.None);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(1, EWeaponType.Warp), warp, ScienceGroup.Warp_S);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(2, EWeaponType.Warp), warp, ScienceGroup.Warp_M);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(3, EWeaponType.Warp), warp, ScienceGroup.Warp_L);
            TreeViewItem time = PutTypeItemGroup(GameObjectName.GetWeaponGroupName(0, EWeaponType.Time), irr_weapon, ScienceGroup.None);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(1, EWeaponType.Time), time, ScienceGroup.Time_S);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(2, EWeaponType.Time), time, ScienceGroup.Time_M);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(3, EWeaponType.Time), time, ScienceGroup.Time_L);
            TreeViewItem cyber_weapon = PutTypeItemGroup(Links.Interface("CyberWeapon"), weapons, ScienceGroup.None);
            TreeViewItem slice = PutTypeItemGroup(GameObjectName.GetWeaponGroupName(0, EWeaponType.Slicing), cyber_weapon, ScienceGroup.None);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(1, EWeaponType.Slicing), slice, ScienceGroup.Slice_S);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(2, EWeaponType.Slicing), slice, ScienceGroup.Slice_M);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(3, EWeaponType.Slicing), slice, ScienceGroup.Slice_L);
            TreeViewItem rad = PutTypeItemGroup(GameObjectName.GetWeaponGroupName(0, EWeaponType.Radiation), cyber_weapon, ScienceGroup.None);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(1, EWeaponType.Radiation), rad, ScienceGroup.Rad_S);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(2, EWeaponType.Radiation), rad, ScienceGroup.Rad_M);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(3, EWeaponType.Radiation), rad, ScienceGroup.Rad_L);
            TreeViewItem drone = PutTypeItemGroup(GameObjectName.GetWeaponGroupName(0, EWeaponType.Drone), cyber_weapon, ScienceGroup.None);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(1, EWeaponType.Drone), drone, ScienceGroup.Drone_S);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(2, EWeaponType.Drone), drone, ScienceGroup.Drone_M);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(3, EWeaponType.Drone), drone, ScienceGroup.Drone_L);
            TreeViewItem magnet = PutTypeItemGroup(GameObjectName.GetWeaponGroupName(0, EWeaponType.Magnet), cyber_weapon, ScienceGroup.None);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(1, EWeaponType.Magnet), magnet, ScienceGroup.Magnet_S);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(2, EWeaponType.Magnet), magnet, ScienceGroup.Magnet_M);
            PutTypeItemGroup(GameObjectName.GetWeaponGroupName(3, EWeaponType.Magnet), magnet, ScienceGroup.Magnet_L);

            TreeViewItem equip = PutTypeItemGroup(Links.Interface("Equipments"), item, ScienceGroup.None);
            TreeViewItem basic_equip = PutTypeItemGroup(Links.Interface("Item_Basic"), equip, ScienceGroup.None);
            PutEquipItems(basic_equip, Links.Interface("Hull"), ScienceGroup.Health_S, ScienceGroup.Health_M, ScienceGroup.Health_L);
            PutEquipItems(basic_equip, Links.Interface("Shield"), ScienceGroup.Shield_S, ScienceGroup.Shield_M, ScienceGroup.Shield_L);
            PutEquipItems(basic_equip, Links.Interface("Energy"), ScienceGroup.Energy_S, ScienceGroup.Energy_M, ScienceGroup.Energy_L);
            TreeViewItem regen_equip = PutTypeItemGroup(Links.Interface("Item_Regen"), equip, ScienceGroup.None);
            PutEquipItems(regen_equip, Links.Interface("HullRestore"), ScienceGroup.Reg_H_S, ScienceGroup.Reg_H_M, ScienceGroup.Reg_H_L);
            PutEquipItems(regen_equip, Links.Interface("ShieldRestore"), ScienceGroup.Reg_S_S, ScienceGroup.Reg_S_M, ScienceGroup.Reg_S_L);
            PutEquipItems(regen_equip, Links.Interface("EnergyRestore"), ScienceGroup.Reg_E_S, ScienceGroup.Reg_E_M, ScienceGroup.Reg_E_L);
            PutFiveItemTotal(equip, Links.Interface("Item_Accuracy"), new ScienceGroup[] { ScienceGroup.Acc_E_S, ScienceGroup.Acc_E_M, ScienceGroup.Acc_E_L,
            ScienceGroup.Acc_P_S,ScienceGroup.Acc_P_M,ScienceGroup.Acc_P_L,ScienceGroup.Acc_I_S,ScienceGroup.Acc_I_M,ScienceGroup.Acc_I_L,
            ScienceGroup.Acc_C_S,ScienceGroup.Acc_C_M, ScienceGroup.Acc_C_L, ScienceGroup.Acc_A_S,ScienceGroup.Acc_A_M,ScienceGroup.Acc_A_L});
            PutFiveItemTotal(equip, Links.Interface("Item_Evasion"), new ScienceGroup[] {ScienceGroup.Eva_E_S, ScienceGroup.Eva_E_M, ScienceGroup.Eva_E_L,
            ScienceGroup.Eva_P_S, ScienceGroup.Eva_P_M, ScienceGroup.Eva_P_L, ScienceGroup.Eva_I_S, ScienceGroup.Eva_I_M, ScienceGroup.Eva_I_L,
            ScienceGroup.Eva_C_S, ScienceGroup.Eva_C_M, ScienceGroup.Eva_C_L, ScienceGroup.Eva_A_S, ScienceGroup.Eva_A_M, ScienceGroup.Eva_A_L});
            PutFiveItemTotal(equip, Links.Interface("Item_Absorb"), new ScienceGroup[] {ScienceGroup.Ign_E_S, ScienceGroup.Ign_E_M, ScienceGroup.Ign_E_L,
                ScienceGroup.Ign_P_S, ScienceGroup.Ign_P_M, ScienceGroup.Ign_P_L, ScienceGroup.Ign_I_S, ScienceGroup.Ign_I_M, ScienceGroup.Ign_I_L,
                ScienceGroup.Ign_C_S,ScienceGroup.Ign_C_M, ScienceGroup.Ign_C_L, ScienceGroup.Ign_A_S, ScienceGroup.Ign_A_M, ScienceGroup.Ign_A_L });
            PutFiveItemTotal(equip, Links.Interface("Item_Damage"), new ScienceGroup[]{ ScienceGroup.Dam_E_S, ScienceGroup.Dam_E_M, ScienceGroup.Dam_E_L,
            ScienceGroup.Dam_P_S, ScienceGroup.Dam_P_M, ScienceGroup.Dam_P_L, ScienceGroup.Dam_I_S, ScienceGroup.Dam_I_M, ScienceGroup.Dam_I_L,
            ScienceGroup.Dam_C_S, ScienceGroup.Dam_C_M, ScienceGroup.Dam_C_L, ScienceGroup.Dam_A_S, ScienceGroup.Dam_A_S, ScienceGroup.Dam_A_L});
            PutFiveItemTotal2(equip, Links.Interface("Item_Immune"), new ScienceGroup[] {ScienceGroup.Imm_E_S, ScienceGroup.Imm_E_M, ScienceGroup.Imm_P_S,
                ScienceGroup.Imm_P_M, ScienceGroup.Imm_I_S, ScienceGroup.Imm_I_M, ScienceGroup.Imm_C_S, ScienceGroup.Imm_C_M, ScienceGroup.Imm_A_S, ScienceGroup.Imm_A_M });
            TreeViewItem nb_equip = PutTypeItemGroup(Links.Interface("Item_NotBattle"), equip, ScienceGroup.None);
            TreeViewItem cargo = PutTypeItemGroup(Links.Interface("Item_Cargo"), nb_equip, ScienceGroup.None);
            PutTypeItemGroup(Links.Interface("Item_Small"), cargo, ScienceGroup.Cargo_S);
            PutTypeItemGroup(Links.Interface("Item_Medium"), cargo, ScienceGroup.Cargo_M);
            TreeViewItem colony = PutTypeItemGroup(Links.Interface("Item_Colony"), nb_equip, ScienceGroup.None);
            PutTypeItemGroup(Links.Interface("Item_Small"), colony, ScienceGroup.Colony_S);
            PutTypeItemGroup(Links.Interface("Item_Medium"), colony, ScienceGroup.Colony_M);
            TreeViewItem others = PutTypeItemGroup(Links.Interface("Others"), item, ScienceGroup.None);
            PutTypeItemGroup(Links.Interface("Item_NewLand"), others, ScienceGroup.NewLands);
            //PutTypeItem(Links.Interface("Equipments"), item, Links.Science.EType.Equipment, i);
            //PutTypeItem(Links.Interface("Others"), item, Links.Science.EType.Other, i);
            if (Focuseditem != null)
            {
                ItemsControl par = Focuseditem;
                for (;;)
                {
                    par = (ItemsControl)par.Parent;
                    if (par != item)
                    {
                        TreeViewItem tvi = (TreeViewItem)par;
                        tvi.IsExpanded = true;
                        par = tvi;
                    }
                    else
                    {
                        break;
                    }

                }
                //Focuseditem.Background = Brushes.LightBlue;
                //Focuseditem.IsSelected = true;
                //Focuseditem.Focus();
                FocusedTimes.Interval = TimeSpan.FromMilliseconds(100);
                FocusedTimes.Start();
            }
        }
        public void PutBasicParameters()
        {
            ScienceListPanel.Children.Clear();
            TreeView ScienceListTree = new TreeView();
            ScienceListTree.Background = Brushes.Black;
            ScienceListPanel.Children.Add(ScienceListTree);
            for (int i = 0; i <= ScienceAnalys.MaxLevelAvailable; i++)
            {
                TreeViewItem item = new TreeViewItem();
                item.Header = string.Format(Links.Interface("LevelSciences"), (i + 1).ToString());
                item.Style = Links.TextStyle;
                item.Foreground = Brushes.White;
                ScienceListTree.Items.Add(item);
                item.Tag = (double)i;
                if (OpenedItems.Contains((double)item.Tag)) item.IsExpanded = true;
                item.Expanded += new RoutedEventHandler(item_Expanded);
                item.Collapsed += new RoutedEventHandler(item_Expanded);
                PutTypeItem(Links.Interface("Buildings"), item, Links.Science.EType.Building, i);
                PutTypeItem(Links.Interface("ShipTypes"), item, Links.Science.EType.ShipTypes, i);
                PutTypeItem(Links.Interface("Generators"), item, Links.Science.EType.Generator, i);
                PutTypeItem(Links.Interface("Shields"), item, Links.Science.EType.Shield, i);
                PutTypeItem(Links.Interface("Computers"), item, Links.Science.EType.Computer, i);
                PutTypeItem(Links.Interface("Engines"), item, Links.Science.EType.Engine, i);
                PutTypeItem(Links.Interface("Weapons"), item, Links.Science.EType.Weapon, i);
                PutTypeItem(Links.Interface("Equipments"), item, Links.Science.EType.Equipment, i);
                PutTypeItem(Links.Interface("Others"), item, Links.Science.EType.Other, i);
            }
            Energy.Total.Content = ScienceAnalys.ScienceInFields[8].ToString() + "/" + ScienceAnalys.ScienceInFields[0].ToString();
            Energy.Rare.Content = ScienceAnalys.ScienceInFields[12].ToString() + "/" + ScienceAnalys.ScienceInFields[4].ToString();
            Energy.SetPrices(SciencePrice.GetSciencePrice(EScienceField.Energy));
            Physic.Total.Content = ScienceAnalys.ScienceInFields[9].ToString() + "/" + ScienceAnalys.ScienceInFields[1].ToString();
            Physic.Rare.Content = ScienceAnalys.ScienceInFields[13].ToString() + "/" + ScienceAnalys.ScienceInFields[5].ToString();
            Physic.SetPrices(SciencePrice.GetSciencePrice(EScienceField.Physic));
            Irregular.Total.Content = ScienceAnalys.ScienceInFields[10].ToString() + "/" + ScienceAnalys.ScienceInFields[2].ToString();
            Irregular.Rare.Content = ScienceAnalys.ScienceInFields[14].ToString() + "/" + ScienceAnalys.ScienceInFields[6].ToString();
            Irregular.SetPrices(SciencePrice.GetSciencePrice(EScienceField.Irregular));
            Cybernetic.Total.Content = ScienceAnalys.ScienceInFields[11].ToString() + "/" + ScienceAnalys.ScienceInFields[3].ToString();
            Cybernetic.Rare.Content = ScienceAnalys.ScienceInFields[15].ToString() + "/" + ScienceAnalys.ScienceInFields[7].ToString();
            Cybernetic.SetPrices(SciencePrice.GetSciencePrice(EScienceField.Cybernetic));

            if (Focuseditem != null)
            {
                //Focuseditem.Background = Brushes.LightBlue;
                //Focuseditem.IsSelected = true;
                //Focuseditem.Focus();
                FocusedTimes.Interval = TimeSpan.FromMilliseconds(100);
                //FocusedTimes.Tick += new EventHandler(FocusedTimes_Tick);
                FocusedTimes.Start();
            }
        }
        void FocusedTimes_Tick(object sender, EventArgs e)
        {
            Focuseditem.Focus();
            FocusedTimes.Stop();
        }
        System.Windows.Threading.DispatcherTimer FocusedTimes;// = new System.Windows.Threading.DispatcherTimer();
        void PutTypeItem(string Name, TreeViewItem parent, Links.Science.EType type, int level)
        {
            TreeViewItem item = new TreeViewItem();
            item.Background = Brushes.Black;
            item.Foreground = Brushes.White;
            parent.Items.Add(item);
            item.Header = Name;
            item.Tag = (double)level + ((double)type) / 10 + 0.01;
            item.Expanded += new RoutedEventHandler(item_Expanded);
            item.Collapsed += new RoutedEventHandler(item_Expanded);
            if (OpenedItems.Contains((double)item.Tag)) item.IsExpanded = true;
            foreach (ushort value in GSGameInfo.SciencesArray)
            {
                GameScience science = Links.Science.GameSciences[value];
                if (science.Level == level && science.Type == type)
                {
                    TreeViewItem inneritem = new TreeViewItem();
                    item.Items.Add(inneritem);
                    StackPanel panel = new StackPanel();
                    panel.Orientation = Orientation.Horizontal;
                    if (science.IsRare) panel.Children.Add(Common.GetRectangle(30, Links.Brushes.RareImageBrush));
                    Label NameLabel = new Label();
                    NameLabel.Style = Links.TextStyle;
                    NameLabel.Foreground = Brushes.White;
                    panel.Children.Add(NameLabel);
                    switch (science.Type)
                    {
                        case Links.Science.EType.Building: NameLabel.Content = GameObjectName.GetBuildingsName(Links.Buildings[science.ID]); break;
                        case Links.Science.EType.ShipTypes: NameLabel.Content = Links.ShipTypes[science.ID].GetName(); break;
                        case Links.Science.EType.Weapon: NameLabel.Content = GameObjectName.GetWeaponName(Links.WeaponTypes[science.ID]); break;
                        case Links.Science.EType.Generator: NameLabel.Content = GameObjectName.GetGeneratorName(Links.GeneratorTypes[science.ID]); break;
                        case Links.Science.EType.Shield: NameLabel.Content = GameObjectName.GetShieldName(Links.ShieldTypes[science.ID]); break;
                        case Links.Science.EType.Computer: NameLabel.Content = GameObjectName.GetComputerName(Links.ComputerTypes[science.ID]); break;
                        case Links.Science.EType.Engine: NameLabel.Content = GameObjectName.GetEngineName(Links.EngineTypes[science.ID]); break;
                        case Links.Science.EType.Equipment: NameLabel.Content = GameObjectName.GetEquipmentName(Links.EquipmentTypes[science.ID]); break;
                        case Links.Science.EType.Other: NameLabel.Content = GameObjectName.GetNewLandsName(science); break;
                    }
                    //NameLabel.Content = science.Name;
                    NameLabel.VerticalAlignment = VerticalAlignment.Center;
                    inneritem.Header = panel;
                    inneritem.Tag = science;
                    if (FocusedScience != null && FocusedScience == science)
                    {
                        parent.IsExpanded = true;
                        item.IsExpanded = true;
                        Focuseditem = inneritem;
                    }
                    inneritem.PreviewMouseDown += new MouseButtonEventHandler(inneritem_PreviewMouseDown);
                }
            }
        }
        TreeViewItem[] PutTypeItems(ItemsControl parrent, string name)
        {
            TreeViewItem[] array = new TreeViewItem[3];
            for (int i = 0; i < 3; i++)
            {
                TreeViewItem item = new TreeViewItem();
                item.Background = Brushes.Black;
                item.Foreground = Brushes.White;
                parrent.Items.Add(item);
                array[i] = item;
            }
            array[0].Header = Links.Interface("Item_Small") + name;
            array[1].Header = Links.Interface("Item_Medium") + name;
            array[2].Header = Links.Interface("Item_Large") + name;
            return array;
        }

        void PutEquipItems(ItemsControl parrent, string Name, ScienceGroup sg1, ScienceGroup sg2, ScienceGroup sg3)
        {
            TreeViewItem top = PutTypeItemGroup(Name, parrent, ScienceGroup.None);
            PutTypeItemGroup(Links.Interface("Item_Small"), top, sg1);
            PutTypeItemGroup(Links.Interface("Item_Medium"), top, sg2);
            PutTypeItemGroup(Links.Interface("Item_Group"), top, sg3);
        }
        void PutEquipItems(ItemsControl parrent, string Name, ScienceGroup sg1, ScienceGroup sg2)
        {
            TreeViewItem top = PutTypeItemGroup(Name, parrent, ScienceGroup.None);
            PutTypeItemGroup(Links.Interface("Item_Small"), top, sg1);
            PutTypeItemGroup(Links.Interface("Item_Medium"), top, sg2);
        }
        TreeViewItem[] PutFiveTypeItems(TreeViewItem parrent, string size, string name)
        {
            TreeViewItem[] array = new TreeViewItem[5];
            for (int i = 0; i < 5; i++)
            {
                TreeViewItem item = new TreeViewItem();
                item.Background = Brushes.Black;
                item.Foreground = Brushes.White;
                parrent.Items.Add(item);
                array[i] = item;
            }
            array[0].Header = size + Links.Interface("Item_Energy") + name;
            array[1].Header = size + Links.Interface("Item_Physic") + name;
            array[2].Header = size + Links.Interface("Item_Irregular") + name;
            array[3].Header = size + Links.Interface("Item_Cyber") + name;
            array[4].Header = size + Links.Interface("Item_Total") + name;
            return array;
        }
        void PutFiveItemTotal(TreeViewItem parrent, string name, ScienceGroup[] groups)
        {
            TreeViewItem top = PutTypeItemGroup(name, parrent, ScienceGroup.None);
            PutEquipItems(top, Links.Interface("Item_Energy2"), groups[0], groups[1], groups[2]);
            PutEquipItems(top, Links.Interface("Item_Physic2"), groups[3], groups[4], groups[5]);
            PutEquipItems(top, Links.Interface("Item_Irregular2"), groups[6], groups[7], groups[8]);
            PutEquipItems(top, Links.Interface("Item_Cyber2"), groups[9], groups[10], groups[11]);
            PutEquipItems(top, Links.Interface("Item_Total2"), groups[12], groups[13], groups[14]);
        }
        void PutFiveItemTotal2(TreeViewItem parrent, string name, ScienceGroup[] groups)
        {
            TreeViewItem top = PutTypeItemGroup(name, parrent, ScienceGroup.None);
            PutEquipItems(top, Links.Interface("Item_Energy2"), groups[0], groups[1]);
            PutEquipItems(top, Links.Interface("Item_Physic2"), groups[2], groups[3]);
            PutEquipItems(top, Links.Interface("Item_Irregular2"), groups[4], groups[5]);
            PutEquipItems(top, Links.Interface("Item_Cyber2"), groups[6], groups[7]);
            PutEquipItems(top, Links.Interface("Item_Total2"), groups[8], groups[9]);
        }
        TreeViewItem PutTypeItemGroup(string Name, ItemsControl parrent, ScienceGroup group)
        {
            TreeViewItem item = new TreeViewItem();
            item.Background = Brushes.Black;
            item.Foreground = Brushes.White;
            parrent.Items.Add(item);
            item.Header = Name;
            if (group == ScienceGroup.None) return item;
            foreach (ushort id in ScienceGroups[group])
            {
                TreeViewItem inneritem = new TreeViewItem();
                item.Items.Add(inneritem);
                StackPanel panel = new StackPanel();
                panel.Orientation = Orientation.Horizontal;
                GameScience science = Links.Science.GameSciences[id];
                if (science.IsRare) panel.Children.Add(Common.GetRectangle(30, Links.Brushes.RareImageBrush));
                Label NameLabel = new Label();
                NameLabel.Style = Links.TextStyle;
                NameLabel.Foreground = Brushes.White;
                panel.Children.Add(NameLabel);
                switch (science.Type)
                {
                    case Links.Science.EType.Building: NameLabel.Content = GameObjectName.GetBuildingsName(Links.Buildings[science.ID]); break;
                    case Links.Science.EType.ShipTypes: NameLabel.Content = Links.ShipTypes[science.ID].GetName(); break;
                    case Links.Science.EType.Weapon: NameLabel.Content = GameObjectName.GetWeaponName(Links.WeaponTypes[science.ID]); break;
                    case Links.Science.EType.Generator: NameLabel.Content = GameObjectName.GetGeneratorName(Links.GeneratorTypes[science.ID]); break;
                    case Links.Science.EType.Shield: NameLabel.Content = GameObjectName.GetShieldName(Links.ShieldTypes[science.ID]); break;
                    case Links.Science.EType.Computer: NameLabel.Content = GameObjectName.GetComputerName(Links.ComputerTypes[science.ID]); break;
                    case Links.Science.EType.Engine: NameLabel.Content = GameObjectName.GetEngineName(Links.EngineTypes[science.ID]); break;
                    case Links.Science.EType.Equipment: NameLabel.Content = GameObjectName.GetEquipmentName(Links.EquipmentTypes[science.ID]); break;
                    case Links.Science.EType.Other: NameLabel.Content = GameObjectName.GetNewLandsName(science); break;
                }
                //NameLabel.Content = science.Name;
                NameLabel.VerticalAlignment = VerticalAlignment.Center;
                inneritem.Header = panel;
                inneritem.Tag = science;
                TreeViewItem par = (TreeViewItem)parrent;
                if (FocusedScience != null && FocusedScience == science)
                {
                    //par.IsExpanded = true;
                    //item.IsExpanded = true;
                    Focuseditem = inneritem;
                }
                inneritem.PreviewMouseDown += new MouseButtonEventHandler(inneritem_PreviewMouseDown);
            }
            return item;
        }
        public void LearnNewScience(GameScience science)
        {
            OpenedItems.Clear();
            FocusedScience = science;
            Refresh();
            //ScienceInfoCanvas canvas = new ScienceInfoCanvas(science);
            UIElement element = GetScienceInfoElement(science);
            InfoBorder.Child = element; //  canvas;
            DoubleAnimation oppanim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(2));
            element.BeginAnimation(Viewbox.OpacityProperty, oppanim);
            //canvas.Show();
        }
        void inneritem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)sender;
            GameScience science = (GameScience)item.Tag;
            InfoBorder.Child = GetScienceInfoElement(science); //vbx;
            Focuseditem = item;
            FocusedScience = science;
        }
        public UIElement GetScienceInfoElement(GameScience science)
        {
            Viewbox vbx = new Viewbox();
            vbx.Child = new GameObjectImage(GOImageType.Standard, science.ID);
           
            return vbx;
        }

        void item_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)sender;
            if (item.IsExpanded)
                OpenedItems.Add((double)item.Tag);
            else
                OpenedItems.Remove((double)item.Tag);
        }
        void FillGroups()
        {
            ScienceGroups = new SortedList<ScienceGroup, SortedSet<ushort>>();
            for (int i = 0; i < 226; i++)
                ScienceGroups.Add((ScienceGroup)i, new SortedSet<ushort>());
            foreach (ushort id in GSGameInfo.SciencesArray)
            {
                GameScience science = Links.Science.GameSciences[id];
                switch (science.Type)
                {
                    case Links.Science.EType.Building: ScienceGroups[GetBuildingGroup(science)].Add(id); break;
                    case Links.Science.EType.ShipTypes: ScienceGroups[GetShipTypeGroup(science)].Add(id); break;
                    case Links.Science.EType.Weapon: ScienceGroups[GetWeaponGroup(science)].Add(id); break;
                    case Links.Science.EType.Generator: ScienceGroups[GetGeneratorGroup(science)].Add(id); break;
                    case Links.Science.EType.Shield: ScienceGroups[GetShieldTypeGroup(science)].Add(id); break;
                    case Links.Science.EType.Computer: ScienceGroups[GetComputerGroup(science)].Add(id); break;
                    case Links.Science.EType.Engine: ScienceGroups[GetEngineGroup(science)].Add(id); break;
                    case Links.Science.EType.Equipment: ScienceGroups[GetEquipmentGroup(science)].Add(id); break;
                    case Links.Science.EType.Other: ScienceGroups[ScienceGroup.NewLands].Add(id); break;
                }
            }
        }
        ScienceGroup GetBuildingGroup(GameScience science)
        {
            GSBuilding building = Links.Buildings[science.ID];
            switch (building.Sector)
            {
                case SectorTypes.Live: return ScienceGroup.BuildLive;
                case SectorTypes.Money: return ScienceGroup.BuildBank;
                case SectorTypes.Metall: return ScienceGroup.BuildMetal;
                case SectorTypes.MetalCap: return ScienceGroup.BuildMetalCap;
                case SectorTypes.Chips: return ScienceGroup.BuildChip;
                case SectorTypes.ChipsCap: return ScienceGroup.BuildChipCap;
                case SectorTypes.Anti: return ScienceGroup.BuildAnti;
                case SectorTypes.AntiCap: return ScienceGroup.BuildAntiCap;
                case SectorTypes.Repair: return ScienceGroup.BuildRepair;
                case SectorTypes.War:
                    if (building.Type == BuildingType.Radar) return ScienceGroup.BuildRadar;
                    else if (building.Type == BuildingType.DataCenter) return ScienceGroup.BuildData;
                    else return ScienceGroup.BuildEducation;
                default: return ScienceGroup.Error;
            }
        }
        ScienceGroup GetShipTypeGroup(GameScience science)
        {
            ShipTypeclass type = Links.ShipTypes[science.ID];
            switch (type.Type)
            {
                case ShipGenerator2Types.Scout: return ScienceGroup.Ship_Scout;
                case ShipGenerator2Types.Corvett: return ScienceGroup.Ship_Corvette;
                case ShipGenerator2Types.Cargo: return ScienceGroup.Ship_Transp;
                case ShipGenerator2Types.Linkor: return ScienceGroup.Ship_Battle;
                case ShipGenerator2Types.Frigate: return ScienceGroup.Ship_Frigate;
                case ShipGenerator2Types.Fighter: return ScienceGroup.Ship_Fighter;
                case ShipGenerator2Types.Dreadnought: return ScienceGroup.Ship_Dreadnought;
                case ShipGenerator2Types.Devostator: return ScienceGroup.Ship_Devostator;
                case ShipGenerator2Types.Warrior: return ScienceGroup.Ship_Warrior;
                case ShipGenerator2Types.Cruiser: return ScienceGroup.Ship_Cruiser;
                default: return ScienceGroup.Error;
            }
        }
        ScienceGroup GetWeaponGroup(GameScience science)
        {
            Weaponclass weapon = Links.WeaponTypes[science.ID];
            switch (weapon.Type)
            {
                case EWeaponType.Laser: if (weapon.Size == ItemSize.Small) return ScienceGroup.Laser_S; else if (weapon.Size == ItemSize.Medium) return ScienceGroup.Laser_M; else return ScienceGroup.Laser_L;
                case EWeaponType.EMI: if (weapon.Size == ItemSize.Small) return ScienceGroup.EMI_S; else if (weapon.Size == ItemSize.Medium) return ScienceGroup.EMI_M; else return ScienceGroup.EMI_L;
                case EWeaponType.Plasma: if (weapon.Size == ItemSize.Small) return ScienceGroup.Plasma_S; else if (weapon.Size == ItemSize.Medium) return ScienceGroup.Plasma_M; else return ScienceGroup.Plasma_L;
                case EWeaponType.Solar: if (weapon.Size == ItemSize.Small) return ScienceGroup.Solar_S; else if (weapon.Size == ItemSize.Medium) return ScienceGroup.Solar_M; else return ScienceGroup.Solar_L;
                case EWeaponType.Cannon: if (weapon.Size == ItemSize.Small) return ScienceGroup.Cannon_S; else if (weapon.Size == ItemSize.Medium) return ScienceGroup.Cannon_M; else return ScienceGroup.Cannon_L;
                case EWeaponType.Gauss: if (weapon.Size == ItemSize.Small) return ScienceGroup.Gauss_S; else if (weapon.Size == ItemSize.Medium) return ScienceGroup.Gauss_M; else return ScienceGroup.Gauss_L;
                case EWeaponType.Missle: if (weapon.Size == ItemSize.Small) return ScienceGroup.Missle_S; else if (weapon.Size == ItemSize.Medium) return ScienceGroup.Missle_M; else return ScienceGroup.Missle_L;
                case EWeaponType.AntiMatter: if (weapon.Size == ItemSize.Small) return ScienceGroup.Anti_S; else if (weapon.Size == ItemSize.Medium) return ScienceGroup.Anti_M; else return ScienceGroup.Anti_L;
                case EWeaponType.Psi: if (weapon.Size == ItemSize.Small) return ScienceGroup.Psi_S; else if (weapon.Size == ItemSize.Medium) return ScienceGroup.Psi_M; else return ScienceGroup.Psi_L;
                case EWeaponType.Dark: if (weapon.Size == ItemSize.Small) return ScienceGroup.Dark_S; else if (weapon.Size == ItemSize.Medium) return ScienceGroup.Dark_M; else return ScienceGroup.Dark_L;
                case EWeaponType.Warp: if (weapon.Size == ItemSize.Small) return ScienceGroup.Warp_S; else if (weapon.Size == ItemSize.Medium) return ScienceGroup.Warp_M; else return ScienceGroup.Warp_L;
                case EWeaponType.Time: if (weapon.Size == ItemSize.Small) return ScienceGroup.Time_S; else if (weapon.Size == ItemSize.Medium) return ScienceGroup.Time_M; else return ScienceGroup.Time_M;
                case EWeaponType.Slicing: if (weapon.Size == ItemSize.Small) return ScienceGroup.Slice_S; else if (weapon.Size == ItemSize.Medium) return ScienceGroup.Slice_M; else return ScienceGroup.Slice_L;
                case EWeaponType.Radiation: if (weapon.Size == ItemSize.Small) return ScienceGroup.Rad_S; else if (weapon.Size == ItemSize.Medium) return ScienceGroup.Rad_M; else return ScienceGroup.Rad_L;
                case EWeaponType.Drone: if (weapon.Size == ItemSize.Small) return ScienceGroup.Drone_S; else if (weapon.Size == ItemSize.Medium) return ScienceGroup.Drone_M; else return ScienceGroup.Drone_L;
                case EWeaponType.Magnet: if (weapon.Size == ItemSize.Small) return ScienceGroup.Magnet_S; else if (weapon.Size == ItemSize.Medium) return ScienceGroup.Magnet_M; else return ScienceGroup.Magnet_L;
                default: return ScienceGroup.Error;
            }
        }
        ScienceGroup GetGeneratorGroup(GameScience science)
        {
            Generatorclass generator = Links.GeneratorTypes[science.ID];
            switch (generator.GetSizeDescription())
            {
                case 0: return ScienceGroup.Generator_R_S;
                case 5: return ScienceGroup.Generator_S_S;
                case 10: return ScienceGroup.Generator_R_M;
                case 15: return ScienceGroup.Generator_S_M;
                case 20: return ScienceGroup.Generator_R_L;
                case 25: return ScienceGroup.Generator_S_L;
                default: return ScienceGroup.Error;
            }
        }
        ScienceGroup GetShieldTypeGroup(GameScience science)
        {
            Shieldclass shield = Links.ShieldTypes[science.ID];
            switch (shield.GetSizeDescription())
            {
                case 0: return ScienceGroup.Shield_R_S;
                case 5: return ScienceGroup.Shield_S_S;
                case 10: return ScienceGroup.Shield_R_M;
                case 15: return ScienceGroup.Shield_S_M;
                case 20: return ScienceGroup.Shield_R_L;
                case 25: return ScienceGroup.Shield_S_L;
                default: return ScienceGroup.Error;
            }
        }
        ScienceGroup GetComputerGroup(GameScience science)
        {
            Computerclass comp = Links.ComputerTypes[science.ID];
            switch (comp.GetType())
            {
                case 0: if (comp.Size == ItemSize.Small) return ScienceGroup.Comp_B_S; else if (comp.Size == ItemSize.Medium) return ScienceGroup.Comp_B_M; else return ScienceGroup.Comp_B_L;
                case 1: if (comp.Size == ItemSize.Small) return ScienceGroup.Comp_I_E_S; else if (comp.Size == ItemSize.Medium) return ScienceGroup.Comp_I_E_M; else return ScienceGroup.Comp_I_E_L;
                case 2: if (comp.Size == ItemSize.Small) return ScienceGroup.Comp_I_P_S; else if (comp.Size == ItemSize.Medium) return ScienceGroup.Comp_I_P_M; else return ScienceGroup.Comp_I_P_L;
                case 3: if (comp.Size == ItemSize.Small) return ScienceGroup.Comp_I_I_S; else if (comp.Size == ItemSize.Medium) return ScienceGroup.Comp_I_I_M; else return ScienceGroup.Comp_I_I_L;
                case 4: if (comp.Size == ItemSize.Small) return ScienceGroup.Comp_I_C_S; else if (comp.Size == ItemSize.Medium) return ScienceGroup.Comp_I_C_M; else return ScienceGroup.Comp_I_C_L;
                case 5: if (comp.Size == ItemSize.Small) return ScienceGroup.Comp_E_E_S; else if (comp.Size == ItemSize.Medium) return ScienceGroup.Comp_E_E_M; else return ScienceGroup.Comp_E_E_L;
                case 6: if (comp.Size == ItemSize.Small) return ScienceGroup.Comp_E_P_S; else if (comp.Size == ItemSize.Medium) return ScienceGroup.Comp_E_P_M; else return ScienceGroup.Comp_E_P_L;
                case 7: if (comp.Size == ItemSize.Small) return ScienceGroup.Comp_E_I_S; else if (comp.Size == ItemSize.Medium) return ScienceGroup.Comp_E_I_M; else return ScienceGroup.Comp_E_I_L;
                case 8: if (comp.Size == ItemSize.Small) return ScienceGroup.Comp_E_C_S; else if (comp.Size == ItemSize.Medium) return ScienceGroup.Comp_E_C_M; else return ScienceGroup.Comp_E_C_L;
                default: return ScienceGroup.Error;
            }
        }
        ScienceGroup GetEquipmentGroup(GameScience science)
        {
            Equipmentclass equip = Links.EquipmentTypes[science.ID];
            switch (equip.Type)
            {
                case 0: if (equip.Size == ItemSize.Small) return ScienceGroup.Acc_E_S; else return ScienceGroup.Acc_E_M;
                case 1: if (equip.Size == ItemSize.Small) return ScienceGroup.Acc_P_S; else return ScienceGroup.Acc_P_M;
                case 2: if (equip.Size == ItemSize.Small) return ScienceGroup.Acc_I_S; else return ScienceGroup.Acc_I_M;
                case 3: if (equip.Size == ItemSize.Small) return ScienceGroup.Acc_C_S; else return ScienceGroup.Acc_C_M;
                case 4: if (equip.Size == ItemSize.Small) return ScienceGroup.Acc_A_S; else return ScienceGroup.Acc_A_M;
                case 5: if (equip.Size == ItemSize.Small) return ScienceGroup.Health_S; else return ScienceGroup.Health_M;
                case 6: if (equip.Size == ItemSize.Small) return ScienceGroup.Reg_H_S; else return ScienceGroup.Reg_H_M;
                case 7: if (equip.Size == ItemSize.Small) return ScienceGroup.Shield_S; else return ScienceGroup.Shield_M;
                case 8: if (equip.Size == ItemSize.Small) return ScienceGroup.Reg_S_S; else return ScienceGroup.Reg_S_M;
                case 9: if (equip.Size == ItemSize.Small) return ScienceGroup.Energy_S; else return ScienceGroup.Energy_M;
                case 10: if (equip.Size == ItemSize.Small) return ScienceGroup.Reg_E_S; else return ScienceGroup.Reg_E_M;
                case 11: if (equip.Size == ItemSize.Small) return ScienceGroup.Eva_E_S; else return ScienceGroup.Eva_E_M;
                case 12: if (equip.Size == ItemSize.Small) return ScienceGroup.Eva_P_S; else return ScienceGroup.Eva_P_M;
                case 13: if (equip.Size == ItemSize.Small) return ScienceGroup.Eva_I_S; else return ScienceGroup.Eva_I_M;
                case 14: if (equip.Size == ItemSize.Small) return ScienceGroup.Eva_C_S; else return ScienceGroup.Eva_C_M;
                case 15: if (equip.Size == ItemSize.Small) return ScienceGroup.Eva_A_S; else return ScienceGroup.Eva_A_M;
                case 16: if (equip.Size == ItemSize.Small) return ScienceGroup.Ign_E_S; else return ScienceGroup.Ign_E_M;
                case 17: if (equip.Size == ItemSize.Small) return ScienceGroup.Ign_P_S; else return ScienceGroup.Ign_P_M;
                case 18: if (equip.Size == ItemSize.Small) return ScienceGroup.Ign_I_S; else return ScienceGroup.Ign_I_M;
                case 19: if (equip.Size == ItemSize.Small) return ScienceGroup.Ign_C_S; else return ScienceGroup.Ign_C_M;
                case 20: if (equip.Size == ItemSize.Small) return ScienceGroup.Ign_A_S; else return ScienceGroup.Ign_A_M;
                case 21: if (equip.Size == ItemSize.Small) return ScienceGroup.Dam_E_S; else return ScienceGroup.Dam_E_M;
                case 22: if (equip.Size == ItemSize.Small) return ScienceGroup.Dam_P_S; else return ScienceGroup.Dam_P_M;
                case 23: if (equip.Size == ItemSize.Small) return ScienceGroup.Dam_I_S; else return ScienceGroup.Dam_I_M;
                case 24: if (equip.Size == ItemSize.Small) return ScienceGroup.Dam_C_S; else return ScienceGroup.Dam_C_M;
                case 25: if (equip.Size == ItemSize.Small) return ScienceGroup.Dam_A_S; else return ScienceGroup.Dam_A_M;
                case 30: return ScienceGroup.Health_L;
                case 31: return ScienceGroup.Reg_H_L;
                case 32: return ScienceGroup.Shield_L;
                case 33: return ScienceGroup.Reg_S_L;
                case 34: return ScienceGroup.Energy_L;
                case 35: return ScienceGroup.Reg_E_L;
                case 36: return ScienceGroup.Acc_E_L;
                case 37: return ScienceGroup.Acc_P_L;
                case 38: return ScienceGroup.Acc_I_L;
                case 39: return ScienceGroup.Acc_C_L;
                case 40: return ScienceGroup.Acc_A_L;
                case 41: return ScienceGroup.Eva_E_L;
                case 42: return ScienceGroup.Eva_P_L;
                case 43: return ScienceGroup.Eva_I_L;
                case 44: return ScienceGroup.Eva_C_L;
                case 45: return ScienceGroup.Eva_A_L;
                case 46: return ScienceGroup.Ign_E_L;
                case 47: return ScienceGroup.Ign_P_L;
                case 48: return ScienceGroup.Ign_I_L;
                case 49: return ScienceGroup.Ign_C_L;
                case 50: return ScienceGroup.Ign_A_L;
                case 51: return ScienceGroup.Dam_E_L;
                case 52: return ScienceGroup.Dam_P_L;
                case 53: return ScienceGroup.Dam_I_L;
                case 54: return ScienceGroup.Dam_C_L;
                case 55: return ScienceGroup.Dam_A_L;
                case 57: if (equip.Size == ItemSize.Small) return ScienceGroup.Cargo_S; else if (equip.Size == ItemSize.Medium) return ScienceGroup.Cargo_M; else return ScienceGroup.Cargo_L;
                case 58: if (equip.Size == ItemSize.Small) return ScienceGroup.Colony_S; else if (equip.Size == ItemSize.Medium) return ScienceGroup.Colony_M; else return ScienceGroup.Colony_L;
                case 59: if (equip.Size == ItemSize.Small) return ScienceGroup.Imm_E_S; else if (equip.Size == ItemSize.Medium) return ScienceGroup.Imm_E_M; else return ScienceGroup.Imm_E_L;
                case 60: if (equip.Size == ItemSize.Small) return ScienceGroup.Imm_P_S; else if (equip.Size == ItemSize.Medium) return ScienceGroup.Imm_P_M; else return ScienceGroup.Imm_P_L;
                case 61: if (equip.Size == ItemSize.Small) return ScienceGroup.Imm_I_S; else if (equip.Size == ItemSize.Medium) return ScienceGroup.Imm_I_M; else return ScienceGroup.Imm_I_L;
                case 62: if (equip.Size == ItemSize.Small) return ScienceGroup.Imm_C_S; else if (equip.Size == ItemSize.Medium) return ScienceGroup.Imm_C_M; else return ScienceGroup.Imm_C_L;
                case 63: if (equip.Size == ItemSize.Small) return ScienceGroup.Imm_A_S; else if (equip.Size == ItemSize.Medium) return ScienceGroup.Imm_A_M; else return ScienceGroup.Imm_A_L;
                default: return ScienceGroup.Error;
            }
        }
        ScienceGroup GetEngineGroup(GameScience science)
        {
            Engineclass engine = Links.EngineTypes[science.ID];
            switch (engine.GetSizeDescription())
            {
                case 0: return ScienceGroup.Engine_U_S;
                case 5: return ScienceGroup.Engine_B_S;
                case 10: return ScienceGroup.Engine_U_M;
                case 15: return ScienceGroup.Engine_B_M;
                case 20: return ScienceGroup.Engine_U_L;
                case 25: return ScienceGroup.Engine_B_L;
                default: return ScienceGroup.Error;
            }
        }
        class LearnScienceButton : Border
        {
            Label Money;
            Label Metals;
            Label Chips;
            Label Anti;
            public Label Total;
            public Label Rare;
            EScienceField Field;
            public LearnScienceButton(EScienceField field)
            {
                Field = field;
                BorderBrush = Brushes.White;
                BorderThickness = new Thickness(2);
                CornerRadius = new CornerRadius(10);
                Height = 146;
                Margin = new Thickness(2);
                Background = new SolidColorBrush(Color.FromArgb(1, 0, 0, 0));

                Grid grid = new Grid();
                Child = grid;
                grid.RowDefinitions.Add(new RowDefinition());
                grid.RowDefinitions[0].Height = new GridLength(2, GridUnitType.Star);
                grid.RowDefinitions.Add(new RowDefinition());
                grid.RowDefinitions.Add(new RowDefinition());
                grid.RowDefinitions.Add(new RowDefinition());
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                TextBlock NameLabel = Common.GetBlock(21, "", Brushes.White, new Thickness());
                NameLabel.TextWrapping = TextWrapping.Wrap;
                //Label NameLabel = new Label();
                //NameLabel.Style = Links.TextStyle;
                //NameLabel.Foreground = Brushes.White;
                switch (field)
                {
                    case EScienceField.Energy: NameLabel.Text = Links.Interface("EnergySciences"); break;
                    case EScienceField.Physic: NameLabel.Text = Links.Interface("PhysicSciences"); break;
                    case EScienceField.Irregular: NameLabel.Text = Links.Interface("IrregularSciences"); break;
                    case EScienceField.Cybernetic: NameLabel.Text = Links.Interface("CyberneticSciences"); break;
                }
                Rectangle emblem = Common.GetRectangle(50, Links.Brushes.WeaponGroupBrush[(WeaponGroup)(byte)field]);
                grid.Children.Add(emblem);
                Grid.SetColumn(emblem, 1);
                //NameLabel.Content = field.ToString();
                //NameLabel.Margin = new Thickness(0, -3, 0, -3);
                //Grid.SetColumnSpan(NameLabel, 2);
                grid.Children.Add(NameLabel);

                Money = AddStackPanel(grid, 2, 0, Links.Brushes.MoneyImageBrush);
                Metals = AddStackPanel(grid, 2, 1, Links.Brushes.MetalImageBrush);
                Chips = AddStackPanel(grid, 3, 0, Links.Brushes.ChipsImageBrush);
                Anti = AddStackPanel(grid, 3, 1, Links.Brushes.AntiImageBrush);
                Total = AddStackPanel(grid, 1, 0, Links.Brushes.SciencePict);
                Rare = AddStackPanel(grid, 1, 1, Links.Brushes.RareImageBrush);

                this.PreviewMouseDown += new MouseButtonEventHandler(LearnScienceButton_PreviewMouseDown);

                ToolTip = new ScienceListByField(field);
            }

            void LearnScienceButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
            {
                Events.LearnScience(Field);
            }
            Label AddStackPanel(Grid grid, int row, int column, Brush brush)
            {
                StackPanel panel = new StackPanel();
                panel.Orientation = Orientation.Horizontal;
                grid.Children.Add(panel);
                Grid.SetRow(panel, row);
                Grid.SetColumn(panel, column);
                panel.HorizontalAlignment = HorizontalAlignment.Left;

                Rectangle rect = Common.GetRectangle(30, brush);
                panel.Children.Add(rect);
                Label lbl = new Label();
                lbl.Style = Links.TextStyle;
                lbl.FontSize = 18;
                lbl.Foreground = Brushes.White;
                panel.Children.Add(lbl);
                return lbl;
            }
            public void SetPrices(ItemPrice price)
            {
                Money.Content = price.Money.ToString("### ### ###");
                Metals.Content = price.Metall.ToString("### ### ###");
                Chips.Content = price.Chips.ToString("### ### ###");
                Anti.Content = price.Anti.ToString("### ### ###");
            }
        }

    }
    class ScienceListByField : Border
    {
        TextBlock InfoBlock;
        public ScienceListByField(EScienceField field)
        {
            Width = 400;
            BorderBrush = Brushes.White;
            BorderThickness = new Thickness(3);
            CornerRadius = new CornerRadius(20);
            Background = Brushes.Black;
            Canvas mainCanvas = new Canvas();
            Child = mainCanvas;
            Rectangle emblem = Common.GetRectangle(50, Links.Brushes.WeaponGroupBrush[(WeaponGroup)(byte)field]);
            mainCanvas.Children.Add(emblem);
            Canvas.SetLeft(emblem, 340);
            Canvas.SetTop(emblem, 10);

            TextBlock NameLabel = Common.GetBlock(21, "", Brushes.White, new Thickness());
            NameLabel.TextWrapping = TextWrapping.Wrap;
            NameLabel.Width = 330;
            switch (field)
            {
                case EScienceField.Energy: NameLabel.Text = Links.Interface("EnergySciences"); Height = 500; break;
                case EScienceField.Physic: NameLabel.Text = Links.Interface("PhysicSciences"); Height = 430; break;
                case EScienceField.Irregular: NameLabel.Text = Links.Interface("IrregularSciences"); Height = 480; break;
                case EScienceField.Cybernetic: NameLabel.Text = Links.Interface("CyberneticSciences"); Height = 570; break;
            }
            if (Links.Lang == 0) Height = Height - 80;
            mainCanvas.Children.Add(NameLabel);
            Canvas.SetLeft(NameLabel, 10);
            Canvas.SetTop(NameLabel, 20);

            InfoBlock = Common.GetBlock(18, "", Brushes.White, new Thickness());
            InfoBlock.Width = 380;
            InfoBlock.FontWeight = FontWeights.Normal;
            InfoBlock.TextAlignment = TextAlignment.Left;
            mainCanvas.Children.Add(InfoBlock);
            Canvas.SetLeft(InfoBlock, 10);
            Canvas.SetTop(InfoBlock, 70);
            switch (field)
            {
                case EScienceField.Energy:
                    AddInfo(0, String.Format("{0}, {1}, {2}, {3}", Links.Interface("Build_Live_Sectors"), Links.Interface("Build_Banks"),
                        Links.Interface("Build_Mines"), Links.Interface("Build_Manufactures")));
                    AddInfo(1, String.Format("{0}, {1}, {2}", Links.Interface("Ship_Scout"), Links.Interface("Ship_Fighter"), Links.Interface("Ship_Warrior")));
                    AddInfo(2, String.Format("{0}", Links.Interface("All")));
                    AddInfo(3, String.Format("{0}", Links.Interface("Basic")));
                    AddInfo(4, String.Format("{0}, {1}", Links.Interface("Item_Energy2"), Links.Interface("Item_Irregular2")));
                    AddInfo(5, String.Format("{0}", Links.Interface("Basic")));
                    AddInfo(6, String.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7},",
                        Links.Interface("Laser"), Links.Interface("EMI"), Links.Interface("Plasma"), Links.Interface("Solar"),
                        Links.Interface("Psi"), Links.Interface("DarkEnergy"), Links.Interface("Warp"), Links.Interface("Time")));
                    AddInfo(7, String.Format("{0}, {1}, {2}, {3}, {4} {5} {6}, {7} {8} {9} {10}, {11} {12} {13}, {14} {15} {16} {17}, {18} {19} {20}, {21}",
                        Links.Interface("HullRestore"), Links.Interface("Shield"), Links.Interface("Energy"), Links.Interface("EnergyRestore"),
                        Links.Interface("Item_Accuracy"), Links.Interface("Item_Energy"), Links.Interface("Item_Irregular"),
                        Links.Interface("Item_Evasion"), Links.Interface("Item_Energy"), Links.Interface("Item_Irregular"), Links.Interface("Item_Total"),
                        Links.Interface("Item_Absorb"), Links.Interface("Item_Energy"), Links.Interface("Item_Irregular"),
                        Links.Interface("Item_Damage"), Links.Interface("Item_Energy"), Links.Interface("Item_Irregular"), Links.Interface("Item_Total"),
                        Links.Interface("Item_Immune"), Links.Interface("Item_Energy"), Links.Interface("Item_Irregular"),
                        Links.Interface("Item_Cargo")));
                    AddInfo(8, Links.Interface("Item_NewLand"));
                    break;

                case EScienceField.Physic:
                    AddInfo(0, String.Format("{0}", Links.Interface("All")));
                    AddInfo(1, String.Format("{0}", Links.Interface("All")));
                    AddInfo(2, String.Format("{0}", Links.Interface("No")));
                    AddInfo(3, String.Format("{0}", Links.Interface("No")));
                    AddInfo(4, String.Format("{0}, {1}", Links.Interface("Item_Physic2"), Links.Interface("Item_Cyber2")));
                    AddInfo(5, String.Format("{0}", Links.Interface("No")));
                    AddInfo(6, String.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7},",
                        Links.Interface("Cannon"), Links.Interface("Gauss"), Links.Interface("Missle"), Links.Interface("Antimatter"),
                        Links.Interface("Slice"), Links.Interface("Radiation"), Links.Interface("Drone"), Links.Interface("Magnet")));
                    AddInfo(7, String.Format("{0}, {1} {2} {3}, {4} {5} {6}, {7} {8} {9} {10}, {11} {12} {13}, {14} {15} {16}",
                        Links.Interface("Hull"),
                        Links.Interface("Item_Accuracy"), Links.Interface("Item_Physic"), Links.Interface("Item_Cyber"),
                        Links.Interface("Item_Evasion"), Links.Interface("Item_Physic"), Links.Interface("Item_Cyber"),
                        Links.Interface("Item_Absorb"), Links.Interface("Item_Physic"), Links.Interface("Item_Cyber"), Links.Interface("Item_Total"),
                        Links.Interface("Item_Damage"), Links.Interface("Item_Physic"), Links.Interface("Item_Cyber"),
                        Links.Interface("Item_Immune"), Links.Interface("Item_Physic"), Links.Interface("Item_Cyber"), Links.Interface("Item_Total")));
                    break;
                case EScienceField.Irregular:
                    AddInfo(0, String.Format("{0}, {1}, {2}", Links.Interface("Build_Part_Accelerators"), Links.Interface("Build_Science_Lab"),
                        Links.Interface("Build_Meltings")));
                    AddInfo(1, String.Format("{0}, {1}, {2}", Links.Interface("Ship_Corvette"), Links.Interface("Ship_Frigate"),
                        Links.Interface("Ship_Dreadnought")));
                    AddInfo(2, String.Format("{0}", Links.Interface("Improve")));
                    AddInfo(3, String.Format("{0}", Links.Interface("Improve")));
                    AddInfo(4, String.Format("{0}, {1}, {2}", Links.Interface("Basic"), Links.Interface("Item_Irregular2"), Links.Interface("Item_Physic2")));
                    AddInfo(5, String.Format("{0}", Links.Interface("All")));
                    AddInfo(6, String.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7},",
                        Links.Interface("Psi"), Links.Interface("DarkEnergy"), Links.Interface("Warp"), Links.Interface("Time"),
                        Links.Interface("Cannon"), Links.Interface("Gauss"), Links.Interface("Missle"), Links.Interface("Antimatter")));
                    AddInfo(7, String.Format("{0}, {1}, {2}, {3} {4} {5} {6}, {7} {8} {9} {10}, {11} {12} {13} {14}, {15} {16} {17} {18}, {19} {20} {21}, {22}, {23}",
                        Links.Interface("HullRestore"), Links.Interface("ShieldRestore"), Links.Interface("EnergyRestore"),
                        Links.Interface("Item_Accuracy"), Links.Interface("Item_Irregular"), Links.Interface("Item_Physic"), Links.Interface("Item_Total"),
                        Links.Interface("Item_Evasion"), Links.Interface("Item_Irregular"), Links.Interface("Item_Physic"), Links.Interface("Item_Total"),
                        Links.Interface("Item_Absorb"), Links.Interface("Item_Irregular"), Links.Interface("Item_Physic"), Links.Interface("Item_Total"),
                        Links.Interface("Item_Damage"), Links.Interface("Item_Irregular"), Links.Interface("Item_Physic"), Links.Interface("Item_Total"),
                        Links.Interface("Item_Immune"), Links.Interface("Item_Irregular"), Links.Interface("Item_Physic"),
                        Links.Interface("Item_Cargo"), Links.Interface("Item_Colony")));
                    break;
                case EScienceField.Cybernetic:
                    AddInfo(0, String.Format("{0}, {1}, {2}, {3}", Links.Interface("Build_Chip_Factories"), Links.Interface("Build_Universiteties"),
                        Links.Interface("Build_Radar"), Links.Interface("Build_Data_Centers")));
                    AddInfo(1, String.Format("{0}, {1}, {2}, {3}", Links.Interface("Ship_Transport"), Links.Interface("Ship_Battle"),
                        Links.Interface("Ship_Devostator"), Links.Interface("Ship_Cruiser")));
                    AddInfo(2, String.Format("{0}", Links.Interface("Basic")));
                    AddInfo(3, String.Format("{0}", Links.Interface("All")));
                    AddInfo(4, String.Format("{0}, {1}, {2}", Links.Interface("Basic"), Links.Interface("Item_Cyber2"), Links.Interface("Item_Energy2")));
                    AddInfo(5, String.Format("{0}", Links.Interface("Improve")));
                    AddInfo(6, String.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7},",
                        Links.Interface("Slice"), Links.Interface("Radiation"), Links.Interface("Drone"), Links.Interface("Magnet"),
                       Links.Interface("Laser"), Links.Interface("EMI"), Links.Interface("Plasma"), Links.Interface("Solar")));
                    AddInfo(7, String.Format("{0}, {1}, {2}, {3}, {4} {5} {6} {7}, {8} {9} {10}, {11} {12} {13}, {14} {15} {16}, {17} {18} {19} {20}, {21}",
                       Links.Interface("Hull"), Links.Interface("Shield"), Links.Interface("ShieldRestore"), Links.Interface("Energy"),
                       Links.Interface("Item_Accuracy"), Links.Interface("Item_Cyber"), Links.Interface("Item_Energy"), Links.Interface("Item_Total"),
                       Links.Interface("Item_Evasion"), Links.Interface("Item_Cyber"), Links.Interface("Item_Energy"),
                       Links.Interface("Item_Absorb"), Links.Interface("Item_Cyber"), Links.Interface("Item_Energy"),
                       Links.Interface("Item_Damage"), Links.Interface("Item_Cyber"), Links.Interface("Item_Energy"),
                       Links.Interface("Item_Immune"), Links.Interface("Item_Cyber"), Links.Interface("Item_Energy"), Links.Interface("Item_Total"),
                       Links.Interface("Item_Colony")));
                    AddInfo(8, Links.Interface("Item_NewLand"));
                    break;
            }
        }
        void AddInfo(byte type, string text)
        {
            switch (type)
            {
                case 0: InfoBlock.Inlines.Add(new Bold(new Run(Links.Interface("Buildings") + ": "))); break;
                case 1: InfoBlock.Inlines.Add(new Bold(new Run(Links.Interface("ShipTypes") + ": "))); break;
                case 2: InfoBlock.Inlines.Add(new Bold(new Run(Links.Interface("Generators") + ": "))); break;
                case 3: InfoBlock.Inlines.Add(new Bold(new Run(Links.Interface("Shields") + ": "))); break;
                case 4: InfoBlock.Inlines.Add(new Bold(new Run(Links.Interface("Computers") + ": "))); break;
                case 5: InfoBlock.Inlines.Add(new Bold(new Run(Links.Interface("Engines") + ": "))); break;
                case 6: InfoBlock.Inlines.Add(new Bold(new Run(Links.Interface("Weapons") + ": "))); break;
                case 7: InfoBlock.Inlines.Add(new Bold(new Run(Links.Interface("Equipments") + ": "))); break;
                case 8: InfoBlock.Inlines.Add(new Bold(new Run(Links.Interface("Others") + ": "))); break;
            }
            InfoBlock.Inlines.Add(text);
            InfoBlock.Inlines.Add(new LineBreak());
        }
    }
    class NewLandInfoPanel : Border
    {
        Canvas mainCanvas;
        void Create()
        {
            Height = 135;
            Width = 300;
            BorderBrush = Brushes.Black;
            BorderThickness = new Thickness(3);
        }
        public NewLandInfoPanel(GameScience science)
        {
            Create();
            mainCanvas = new Canvas();
            Child = mainCanvas;

            Common.PutRect(mainCanvas, 64, Links.Brushes.LandIconBrush, 5, 5, false);

            Border nameborder = Common.PutBorder(220, 60, Brushes.Black, 2, 70, 5, mainCanvas);
            Common.PutBlock(16, nameborder, GameObjectName.GetNewLandsName(science));

            LinearGradientBrush backbrush = new LinearGradientBrush();
            backbrush.GradientStops.Add(new GradientStop(Colors.LightGreen, 1));
            backbrush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
            backbrush.GradientStops.Add(new GradientStop(Colors.Gold, 0));
            mainCanvas.Background = backbrush;
            Border DescriptionBorder = Common.PutBorder(280, 145, Brushes.Black, 2, 5, 100, mainCanvas);
            Common.PutBlock(18, DescriptionBorder, GameObjectName.GetnewLandDescription());
            Height = 285;
        }
    }
    enum ScienceGroup
    {
        BuildLive, BuildBank, 
        BuildMetal, BuildMetalCap,
        BuildChip, BuildChipCap,
        BuildAnti, BuildAntiCap,
        BuildRepair,
        BuildRadar, BuildData, BuildEducation,
        Ship_Scout, Ship_Corvette, Ship_Transp, Ship_Battle, Ship_Frigate, Ship_Fighter, Ship_Dreadnought, Ship_Devostator, Ship_Warrior, Ship_Cruiser,
        Comp_B_S, Comp_B_M, Comp_B_L,
        Comp_I_E_S, Comp_I_E_M, Comp_I_E_L,
        Comp_I_P_S, Comp_I_P_M, Comp_I_P_L,
        Comp_I_I_S, Comp_I_I_M, Comp_I_I_L,
        Comp_I_C_S, Comp_I_C_M, Comp_I_C_L,
        Comp_E_E_S, Comp_E_E_M, Comp_E_E_L,
        Comp_E_P_S, Comp_E_P_M, Comp_E_P_L,
        Comp_E_I_S, Comp_E_I_M, Comp_E_I_L,
        Comp_E_C_S, Comp_E_C_M, Comp_E_C_L,
        Engine_B_S, Engine_B_M, Engine_B_L,
        Engine_U_S, Engine_U_M, Engine_U_L,
        Generator_S_S, Generator_S_M, Generator_S_L,
        Generator_R_S, Generator_R_M, Generator_R_L,
        Shield_S_S, Shield_S_M, Shield_S_L,
        Shield_R_S, Shield_R_M, Shield_R_L,
        Laser_S, Laser_M, Laser_L,
        EMI_S, EMI_M, EMI_L,
        Plasma_S, Plasma_M, Plasma_L,
        Solar_S, Solar_M, Solar_L,
        Cannon_S, Cannon_M, Cannon_L,
        Gauss_S, Gauss_M, Gauss_L,
        Missle_S, Missle_M, Missle_L,
        Anti_S, Anti_M, Anti_L,
        Psi_S, Psi_M, Psi_L,
        Dark_S, Dark_M, Dark_L,
        Warp_S, Warp_M, Warp_L,
        Time_S, Time_M, Time_L,
        Slice_S, Slice_M, Slice_L,
        Rad_S, Rad_M, Rad_L,
        Drone_S, Drone_M, Drone_L,
        Magnet_S, Magnet_M, Magnet_L,
        Acc_E_S, Acc_E_M, Acc_E_L,
        Acc_P_S, Acc_P_M, Acc_P_L,
        Acc_I_S, Acc_I_M, Acc_I_L,
        Acc_C_S, Acc_C_M, Acc_C_L,
        Acc_A_S, Acc_A_M, Acc_A_L,
        Health_S, Health_M, Health_L,
        Reg_H_S, Reg_H_M, Reg_H_L,
        Shield_S, Shield_M, Shield_L,
        Reg_S_S, Reg_S_M, Reg_S_L,
        Energy_S, Energy_M, Energy_L,
        Reg_E_S, Reg_E_M, Reg_E_L,
        Eva_E_S, Eva_E_M, Eva_E_L,
        Eva_P_S, Eva_P_M, Eva_P_L,
        Eva_I_S, Eva_I_M, Eva_I_L,
        Eva_C_S, Eva_C_M, Eva_C_L,
        Eva_A_S, Eva_A_M, Eva_A_L,
        Ign_E_S, Ign_E_M, Ign_E_L,
        Ign_P_S, Ign_P_M, Ign_P_L,
        Ign_I_S, Ign_I_M, Ign_I_L,
        Ign_C_S, Ign_C_M, Ign_C_L,
        Ign_A_S, Ign_A_M, Ign_A_L,
        Dam_E_S, Dam_E_M, Dam_E_L,
        Dam_P_S, Dam_P_M, Dam_P_L,
        Dam_I_S, Dam_I_M, Dam_I_L,
        Dam_C_S, Dam_C_M, Dam_C_L,
        Dam_A_S, Dam_A_M, Dam_A_L,
        Imm_E_S, Imm_E_M, Imm_E_L,
        Imm_P_S, Imm_P_M, Imm_P_L,
        Imm_I_S, Imm_I_M, Imm_I_L,
        Imm_C_S, Imm_C_M, Imm_C_L,
        Imm_A_S, Imm_A_M, Imm_A_L,
        Cargo_S, Cargo_M, Cargo_L,
        Colony_S, Colony_M, Colony_L,
        NewLands,
        Error,
        None
    }
    class ScienceFilter : Canvas
    {
        string[] Values;
        TextBlock block;
        public byte curvalue = 0;
        public ScienceFilter(string[] list)
        {
            Values = list;
            Width = 300; Height = 50;
            Background = Brushes.Black;
            Path LeftArrow = new Path();
            LeftArrow.Width = 50; LeftArrow.Height = 50;
            LeftArrow.Stroke = Brushes.White;
            LeftArrow.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M25,0 a25,25 0 1,0 0.1,0 M25,1 a24,24 0 1,0 0.1,0 M7,25 l18,-18 a18,18 0 0,1 0,36z"));
            Children.Add(LeftArrow);
            LeftArrow.Fill = Common.GetRadialBrush(Colors.Green, 0.7, 0.3);
            LeftArrow.PreviewMouseDown += new MouseButtonEventHandler(LeftArrow_PreviewMouseDown);

            Path RightArrow = new Path();
            RightArrow.Width = 50; RightArrow.Height = 50;
            RightArrow.Stroke = Brushes.White;
            RightArrow.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M25,0 a25,25 0 1,0 0.1,0 M25,1 a24,24 0 1,0 0.1,0 M44,25 l-18,-18 a18,18 0 0,0 0,36z"));
            Children.Add(RightArrow);
            Canvas.SetLeft(RightArrow, 250);
            RightArrow.Fill = Common.GetRadialBrush(Colors.Green, 0.7, 0.3);
            RightArrow.PreviewMouseDown += new MouseButtonEventHandler(RightArrow_PreviewMouseDown);

            Path Body = new Path();
            Body.Stroke = Brushes.White;
            Body.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M25,0 h250 M25,50 h250"));
            Children.Add(Body);
            Margin = new Thickness(10);

            block = Common.GetBlock(24, Values[0]);
            Children.Add(block);
            block.Foreground = Brushes.White;
            block.TextAlignment = TextAlignment.Center;
            block.Width = 200;
            Canvas.SetLeft(block, 50);
            Canvas.SetTop(block, 10);
        }

        void RightArrow_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            curvalue++;
            if (curvalue == 255) curvalue = (byte)(Values.Length - 1);
            else if (curvalue == Values.Length) curvalue = 0;
            block.Text = Values[curvalue];
            Links.Controller.Science2Canvas.ScienceBorder.Draw();
        }

        void LeftArrow_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            curvalue--;
            if (curvalue == 255) curvalue = (byte)(Values.Length - 1);
            else if (curvalue == Values.Length) curvalue = 0;
            block.Text = Values[curvalue];
            Links.Controller.Science2Canvas.ScienceBorder.Draw();
        }
    }*/
}
