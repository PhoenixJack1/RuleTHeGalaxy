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
    class SchemasCanvas : Canvas
    {
        public static ApplyEnum Target;
        public ushort SelectID;
        public Canvas mainCanvas;
        Point weaponP1 = new Point(205, 5);
        Point weaponP2 = new Point(405, 5);
        Point weaponP3 = new Point(605, 5);
        Point ShipTypeP = new Point(5, 5);
        Point GenP = new Point(5, 225);
        Point ShieldP = new Point(205, 225);
        Point CompP = new Point(405, 225);
        Point EngP = new Point(605, 225);
        Point EP1 = new Point(5, 445);
        Point EP2 = new Point(205, 445);
        Point EP3 = new Point(405, 445);
        Point EP4 = new Point(605, 445);
        GameObjectImage weapon1, weapon2, weapon3;
        GameObjectImage shiptypepanel;
        GameObjectImage generatorpanel;
        GameObjectImage shieldpanel;
        GameObjectImage computerpanel;
        GameObjectImage enginepanel;
        GameObjectImage equippanel1, equippanel2, equippanel3, equippanel4;
        public Schema CurSchema;
        int SelectedSchema;
        public Viewbox box;
        public SchemasCanvas()
        {
            Width = 1280;
            Height = 600;
            box = new Viewbox();
            box.Width = 1230;
            box.Height = 520;
            box.HorizontalAlignment = HorizontalAlignment.Center;
            box.VerticalAlignment = VerticalAlignment.Center;
            box.Child = this;
            mainCanvas = new Canvas();
            Children.Add(mainCanvas);
            SelectedSchema = Int32.MaxValue;
        }
        public void Select()
        {
            bool needstop = Gets.GetTotalInfo("После открытия панели со схемами");
            SelectedSchema = Int32.MaxValue;
            if (GSGameInfo.PlayerSchemas.Count != 0 && !needstop)
            {
                WrapPanel panel = new WrapPanel();
                panel.Width = 1200;
                ScrollViewer viewer = new ScrollViewer();
                viewer.Content = panel;
                viewer.VerticalAlignment = VerticalAlignment.Center;
                viewer.Width = 1200;
                viewer.Height = 600;
                viewer.Background = Common.GetLinearBrush(Colors.Black, Colors.White, Colors.Black);
                List<UIElement> list = new List<UIElement>();
                list.Add(new SchemaInfoPanel(Int32.MaxValue));
                for (int i = 0; i < GSGameInfo.PlayerSchemas.Count; i++)
                {
                    ShipB ship = new ShipB(0, GSGameInfo.PlayerSchemas[i], 100, null, new byte[4], ShipSide.Attack, null, false, null, ShipBMode.Battle,255);
                    //ship.PanelB.VBX.Tag = i;
                    //list.Add(ship.PanelB.VBX);
                }
                foreach (UIElement p in list)
                {
                    FrameworkElement el = (FrameworkElement)p;
                    el.PreviewMouseDown += new MouseButtonEventHandler(el_PreviewMouseDown);
                    panel.Children.Add(p);
                }
                Links.Controller.PopUpCanvas.Place(viewer, false);
                return;
            }
            else
                CurSchema = new Schema();
            Refresh(false);

        }

        void el_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Links.Controller.PopUpCanvas.Remove();
            FrameworkElement el = (FrameworkElement)sender;
            int tag = (int)el.Tag;
            if (tag == Int32.MaxValue)
                CurSchema = new Schema();
            else CurSchema = GSGameInfo.PlayerSchemas[tag];
            SelectedSchema = tag;
            Refresh(false);

        }
        public void GenerateSchema()
        {
            int maxlevel = 5;
            int minlevel = 0;
            
            GSGameInfo.SciencesArray = new SortedSet<ushort>();
            SortedSet<EWeaponType> PirSet = new SortedSet<EWeaponType>();
            PirSet.Add(EWeaponType.Laser); PirSet.Add(EWeaponType.Plasma); PirSet.Add(EWeaponType.Cannon);
            PirSet.Add(EWeaponType.Gauss); PirSet.Add(EWeaponType.Missle); PirSet.Add(EWeaponType.Slicing);
            PirSet.Add(EWeaponType.Radiation);
            SortedSet<EWeaponType> AlienSet = new SortedSet<EWeaponType>();
            AlienSet.Add(EWeaponType.Solar); AlienSet.Add(EWeaponType.AntiMatter); AlienSet.Add(EWeaponType.Psi);
            AlienSet.Add(EWeaponType.Dark); AlienSet.Add(EWeaponType.Warp); AlienSet.Add(EWeaponType.Time);
            AlienSet.Add(EWeaponType.Radiation); AlienSet.Add(EWeaponType.Magnet);
            SortedSet<EWeaponType> GreenSet = new SortedSet<EWeaponType>();
            GreenSet.Add(EWeaponType.Laser); GreenSet.Add(EWeaponType.EMI); GreenSet.Add(EWeaponType.Solar);
            GreenSet.Add(EWeaponType.AntiMatter); GreenSet.Add(EWeaponType.Psi); GreenSet.Add(EWeaponType.Dark);
            GreenSet.Add(EWeaponType.Drone); GreenSet.Add(EWeaponType.Magnet);
            SortedSet<EWeaponType> TechSet = new SortedSet<EWeaponType>();
            TechSet.Add(EWeaponType.EMI); TechSet.Add(EWeaponType.Gauss); TechSet.Add(EWeaponType.AntiMatter);
            TechSet.Add(EWeaponType.Warp); TechSet.Add(EWeaponType.Time); TechSet.Add(EWeaponType.Slicing);
            TechSet.Add(EWeaponType.Drone);
            SortedSet<EWeaponType> MercSet = new SortedSet<EWeaponType>();
            MercSet.Add(EWeaponType.Plasma); MercSet.Add(EWeaponType.Solar); MercSet.Add(EWeaponType.Cannon);
            MercSet.Add(EWeaponType.Missle); MercSet.Add(EWeaponType.Dark); MercSet.Add(EWeaponType.Warp);
            MercSet.Add(EWeaponType.Slicing); MercSet.Add(EWeaponType.Magnet);
            SortedSet<EWeaponType> NiceWeaponSet = GreenSet;
            foreach (GameScience science in Links.Science.GameSciences.Values)
            {
                if (science.Level > maxlevel) continue;
                if (science.Level < minlevel) continue;
                if (science.Type == Links.Science.EType.Weapon && !NiceWeaponSet.Contains(Links.WeaponTypes[science.ID].Type)) continue;
                GSGameInfo.SciencesArray.Add(science.ID);
            }
            //Schema schema = SchemeGenerator.GetPirateSchema(2, out group);
            //Schema schema = SchemeGenerator.GetSpeedShipSchema();
            //Schema schema = SchemeGenerator.GetDefenderSchema();
            //Schema schema = SchemeGenerator.GetWarriorSchema();
            //if (schema == null) return;
            //CurSchema = schema;
            //Refresh(false);
        }

        void Refresh(bool NeedLoad)
        {
            if (NeedLoad)
                Gets.GetTotalInfo("После обновления панели со схемами");

            mainCanvas.Children.Clear();
            weapon1 = null; weapon2 = null; weapon3 = null;
            shiptypepanel = null;
            generatorpanel = null; shieldpanel = null;
            computerpanel = null; enginepanel = null;
            equippanel1 = null; equippanel2 = null;
            equippanel3 = null; equippanel4 = null;
            if (CurSchema.ShipType == null)
            {
                shiptypepanel = new GameObjectImage(Links.Interface("GOIShipType"), Brushes.Black, Brushes.LightGray, false);
                shiptypepanel.PreviewMouseDown += ShipTypeInfoPanel_PreviewMouseDown;
            }
            else
            {
                shiptypepanel = new GameObjectImage(GOImageType.Standard, CurSchema.ShipTypeID);
                shiptypepanel.PreviewMouseDown += ShipTypeInfoPanel_PreviewMouseDown;
                if (CurSchema.Generator == null)
                {
                    generatorpanel = new GameObjectImage(Links.Interface("GOIGenerator"), GameObjectImageParams.DarkGreen, GameObjectImageParams.LightGreen, false);
                    generatorpanel.SetSize(CurSchema.ShipType.GeneratorSize);
                    generatorpanel.Tag = CurSchema.ShipType.GeneratorSize;
                    generatorpanel.PreviewMouseDown += GeneratorInfoPanel_PreviewMouseDown;
                }
                else
                {
                    generatorpanel = new GameObjectImage(GOImageType.Standard, CurSchema.GeneratorID);
                    generatorpanel.Tag = CurSchema.ShipType.GeneratorSize;
                    generatorpanel.PreviewMouseDown += GeneratorInfoPanel_PreviewMouseDown;
                }
                if (CurSchema.Shield == null)
                {
                    shieldpanel = new GameObjectImage(Links.Interface("GOIShield"), GameObjectImageParams.DarkBlue, GameObjectImageParams.LightBlue, false);
                    shieldpanel.SetSize(CurSchema.ShipType.ShieldSize);
                    shieldpanel.Tag = CurSchema.ShipType.ShieldSize;
                    shieldpanel.PreviewMouseDown += ShieldInfoPanel_PreviewMouseDown;
                }
                else
                {
                    shieldpanel = new GameObjectImage(GOImageType.Standard, CurSchema.ShieldID);
                    shieldpanel.Tag = CurSchema.ShipType.ShieldSize;
                    shieldpanel.PreviewMouseDown += ShieldInfoPanel_PreviewMouseDown;
                }
                if (CurSchema.Computer == null)
                {
                    computerpanel = new GameObjectImage(Links.Interface("GOIComputer"), GameObjectImageParams.DarkGreen, GameObjectImageParams.LightGreen, false);
                    computerpanel.SetSize(CurSchema.ShipType.ComputerSize);
                    computerpanel.Tag = CurSchema.ShipType.ComputerSize;
                    computerpanel.PreviewMouseDown += ComputerInfoPanel_PreviewMouseDown;
                }
                else
                {
                    computerpanel = new GameObjectImage(GOImageType.Standard, CurSchema.ComputerID);
                    computerpanel.Tag = CurSchema.ShipType.ComputerSize;
                    computerpanel.PreviewMouseDown += ComputerInfoPanel_PreviewMouseDown;
                }
                if (CurSchema.Engine == null)
                {
                    enginepanel = new GameObjectImage(Links.Interface("GOIEngine"), GameObjectImageParams.DarkPurple, GameObjectImageParams.LightPurle, false);
                    enginepanel.SetSize(CurSchema.ShipType.EngineSize);
                    enginepanel.Tag = CurSchema.ShipType.EngineSize;
                    enginepanel.PreviewMouseDown += EngineInfoPanel_PreviewMouseDown;
                }
                else
                {
                    enginepanel = new GameObjectImage(GOImageType.Standard, CurSchema.EngineID);
                    enginepanel.Tag = CurSchema.ShipType.EngineSize;
                    enginepanel.PreviewMouseDown += EngineInfoPanel_PreviewMouseDown;
                }
                if (CurSchema.GetWeapon(0) != null)
                {
                    weapon1 = new GameObjectImage(GOImageType.Standard, CurSchema.GetWeapon(0).ID);
                    weapon1.Tag = new SelectItemHelper(ApplyEnum.Weapon1, CurSchema.ShipType.GetWeaponParams(0).Size, CurSchema.ShipType.GetWeaponParams(0).Group);
                    weapon1.PreviewMouseDown += WeaponInfoPanel_PreviewMouseDown;
                }
                else if (CurSchema.ShipType.GetWeaponParams(0) != null)
                {
                    weapon1 = new GameObjectImage(Links.Interface("GOIWeapon"), Brushes.Black, Brushes.LightGray, false);
                    weapon1.SetGroup(CurSchema.ShipType.GetWeaponParams(0).Group);
                    weapon1.SetSize(CurSchema.ShipType.GetWeaponParams(0).Size);
                    weapon1.Tag = new SelectItemHelper(ApplyEnum.Weapon1, CurSchema.ShipType.GetWeaponParams(0).Size, CurSchema.ShipType.GetWeaponParams(0).Group);
                    weapon1.PreviewMouseDown += WeaponInfoPanel_PreviewMouseDown;
                }
                if (CurSchema.GetWeapon(1) != null)
                {
                    weapon2 = new GameObjectImage(GOImageType.Standard, CurSchema.GetWeapon(1).ID);
                    weapon2.Tag = new SelectItemHelper(ApplyEnum.Weapon2, CurSchema.ShipType.GetWeaponParams(1).Size, CurSchema.ShipType.GetWeaponParams(1).Group);
                    weapon2.PreviewMouseDown += WeaponInfoPanel_PreviewMouseDown;
                }
                else if (CurSchema.ShipType.GetWeaponParams(1) != null)
                {
                    weapon2 = new GameObjectImage(Links.Interface("GOIWeapon"), Brushes.Black, Brushes.LightGray, false);
                    weapon2.SetGroup(CurSchema.ShipType.GetWeaponParams(1).Group);
                    weapon2.SetSize(CurSchema.ShipType.GetWeaponParams(1).Size);
                    weapon2.Tag = new SelectItemHelper(ApplyEnum.Weapon2, CurSchema.ShipType.GetWeaponParams(1).Size, CurSchema.ShipType.GetWeaponParams(1).Group);
                    weapon2.PreviewMouseDown += WeaponInfoPanel_PreviewMouseDown;
                }
                if (CurSchema.GetWeapon(2) != null)
                {
                    weapon3 = new GameObjectImage(GOImageType.Standard, CurSchema.GetWeapon(2).ID);
                    weapon3.Tag = new SelectItemHelper(ApplyEnum.Weapon3, CurSchema.ShipType.GetWeaponParams(2).Size, CurSchema.ShipType.GetWeaponParams(2).Group);
                    weapon3.PreviewMouseDown += WeaponInfoPanel_PreviewMouseDown;
                }
                else if (CurSchema.ShipType.GetWeaponParams(2) != null)
                {
                    weapon3 = new GameObjectImage(Links.Interface("GOIWeapon"), Brushes.Black, Brushes.LightGray, false);
                    weapon3.SetGroup(CurSchema.ShipType.GetWeaponParams(2).Group);
                    weapon3.SetSize(CurSchema.ShipType.GetWeaponParams(2).Size);
                    weapon3.Tag = new SelectItemHelper(ApplyEnum.Weapon3, CurSchema.ShipType.GetWeaponParams(2).Size, CurSchema.ShipType.GetWeaponParams(2).Group);
                    weapon3.PreviewMouseDown += WeaponInfoPanel_PreviewMouseDown;
                }

                if (CurSchema.GetEquipment(0) != null)
                {
                    equippanel1 = new GameObjectImage(GOImageType.Standard, CurSchema.Equipments[0].ID);
                    equippanel1.Tag = new SelectItemHelper(ApplyEnum.Equipment1, CurSchema.ShipType.EquipmentsSize[0], WeaponGroup.Any);
                    equippanel1.PreviewMouseDown += EquipmentInfoPanel_PreviewMouseDown;
                }
                else if (CurSchema.ShipType.EquipmentCapacity > 0)
                {
                    equippanel1 = new GameObjectImage(Links.Interface("GOIEquipment"), Brushes.Black, Brushes.Gray, true);
                    equippanel1.SetSize(CurSchema.ShipType.EquipmentsSize[0]);
                    equippanel1.Tag = new SelectItemHelper(ApplyEnum.Equipment1, CurSchema.ShipType.EquipmentsSize[0], WeaponGroup.Any);
                    equippanel1.PreviewMouseDown += EquipmentInfoPanel_PreviewMouseDown;
                }
                if (CurSchema.GetEquipment(1) != null)
                {
                    equippanel2 = new GameObjectImage(GOImageType.Standard, CurSchema.Equipments[1].ID);
                    equippanel2.Tag = new SelectItemHelper(ApplyEnum.Equipment2, CurSchema.ShipType.EquipmentsSize[1], WeaponGroup.Any);
                    equippanel2.PreviewMouseDown += EquipmentInfoPanel_PreviewMouseDown;
                }
                else if (CurSchema.ShipType.EquipmentCapacity > 1)
                {
                    equippanel2 = new GameObjectImage(Links.Interface("GOIEquipment"), Brushes.Black, Brushes.Gray, true);
                    equippanel2.SetSize(CurSchema.ShipType.EquipmentsSize[1]);
                    equippanel2.Tag = new SelectItemHelper(ApplyEnum.Equipment2, CurSchema.ShipType.EquipmentsSize[1], WeaponGroup.Any);
                    equippanel2.PreviewMouseDown += EquipmentInfoPanel_PreviewMouseDown;
                }
                if (CurSchema.GetEquipment(2) != null)
                {
                    equippanel3 = new GameObjectImage(GOImageType.Standard, CurSchema.Equipments[2].ID);
                    equippanel3.Tag = new SelectItemHelper(ApplyEnum.Equipment3, CurSchema.ShipType.EquipmentsSize[2], WeaponGroup.Any);
                    equippanel3.PreviewMouseDown += EquipmentInfoPanel_PreviewMouseDown;
                }
                else if (CurSchema.ShipType.EquipmentCapacity > 2)
                {
                    equippanel3 = new GameObjectImage(Links.Interface("GOIEquipment"), Brushes.Black, Brushes.Gray, true);
                    equippanel3.SetSize(CurSchema.ShipType.EquipmentsSize[2]);
                    equippanel3.Tag = new SelectItemHelper(ApplyEnum.Equipment3, CurSchema.ShipType.EquipmentsSize[2], WeaponGroup.Any);
                    equippanel3.PreviewMouseDown += EquipmentInfoPanel_PreviewMouseDown;
                }
                if (CurSchema.GetEquipment(3) != null)
                {
                    equippanel4 = new GameObjectImage(GOImageType.Standard, CurSchema.Equipments[3].ID);
                    equippanel4.Tag = new SelectItemHelper(ApplyEnum.Equipment4, CurSchema.ShipType.EquipmentsSize[3], WeaponGroup.Any);
                    equippanel4.PreviewMouseDown += EquipmentInfoPanel_PreviewMouseDown;
                }
                else if (CurSchema.ShipType.EquipmentCapacity > 3)
                {
                    equippanel4 = new GameObjectImage(Links.Interface("GOIEquipment"), Brushes.Black, Brushes.Gray, true);
                    equippanel4.SetSize(CurSchema.ShipType.EquipmentsSize[3]);
                    equippanel4.Tag = new SelectItemHelper(ApplyEnum.Equipment4, CurSchema.ShipType.EquipmentsSize[43], WeaponGroup.Any);
                    equippanel4.PreviewMouseDown += EquipmentInfoPanel_PreviewMouseDown;
                }

            }
            if (weapon1 != null)
            {
                mainCanvas.Children.Add(weapon1);
                Canvas.SetLeft(weapon1, weaponP1.X);
                Canvas.SetTop(weapon1, weaponP1.Y);
            }
            if (weapon2 != null)
            {
                mainCanvas.Children.Add(weapon2);
                Canvas.SetLeft(weapon2, weaponP2.X);
                Canvas.SetTop(weapon2, weaponP2.Y);
            }
            if (weapon3 != null)
            {
                mainCanvas.Children.Add(weapon3);
                Canvas.SetLeft(weapon3, weaponP3.X);
                Canvas.SetTop(weapon3, weaponP3.Y);
            }
            if (shiptypepanel != null)
            {
                mainCanvas.Children.Add(shiptypepanel);
                Canvas.SetLeft(shiptypepanel, ShipTypeP.X);
                Canvas.SetTop(shiptypepanel, ShipTypeP.Y);
            }
            if (generatorpanel != null)
            {
                mainCanvas.Children.Add(generatorpanel);
                Canvas.SetLeft(generatorpanel, GenP.X);
                Canvas.SetTop(generatorpanel, GenP.Y);
            }
            if (shieldpanel != null)
            {
                mainCanvas.Children.Add(shieldpanel);
                Canvas.SetLeft(shieldpanel, ShieldP.X);
                Canvas.SetTop(shieldpanel, ShieldP.Y);
            }
            if (computerpanel != null)
            {
                mainCanvas.Children.Add(computerpanel);
                Canvas.SetLeft(computerpanel, CompP.X);
                Canvas.SetTop(computerpanel, CompP.Y);
            }
            if (enginepanel != null)
            {
                mainCanvas.Children.Add(enginepanel);
                Canvas.SetLeft(enginepanel, EngP.X);
                Canvas.SetTop(enginepanel, EngP.Y);
            }
            if (equippanel1 != null)
            {
                mainCanvas.Children.Add(equippanel1);
                Canvas.SetLeft(equippanel1, EP1.X);
                Canvas.SetTop(equippanel1, EP1.Y);
            }
            if (equippanel2 != null)
            {
                mainCanvas.Children.Add(equippanel2);
                Canvas.SetLeft(equippanel2, EP2.X);
                Canvas.SetTop(equippanel2, EP2.Y);
            }
            if (equippanel3 != null)
            {
                mainCanvas.Children.Add(equippanel3);
                Canvas.SetLeft(equippanel3, EP3.X);
                Canvas.SetTop(equippanel3, EP3.Y);
            }
            if (equippanel4 != null)
            {
                mainCanvas.Children.Add(equippanel4);
                Canvas.SetLeft(equippanel4, EP4.X);
                Canvas.SetTop(equippanel4, EP4.Y);
            }
            ShipB ship;
            if (CurSchema.ShipType != null)
            {
                ship = new ShipB(0, CurSchema, 100, null, new byte[4], ShipSide.Attack, null, true, null, ShipBMode.Battle,255);
                //mainCanvas.Children.Add(ship.PanelB.VBX);
                //Canvas.SetLeft(ship.PanelB.VBX, 810);
                //Canvas.SetTop(ship.PanelB.VBX, 10);

                SchemaPricePanel schemaprice = new SchemaPricePanel(ship);
                mainCanvas.Children.Add(schemaprice);
                Canvas.SetLeft(schemaprice, 880);
                Canvas.SetTop(schemaprice, 320);
            }
            InterfaceButton ArmorButton = new InterfaceButton(200, 45, 7, 20);
            ArmorButton.SetText(Links.Interface("ArmorType") + Links.Armors[CurSchema.Armor].GetName());
            ArmorButton.PutToCanvas(mainCanvas, 830, 400);
            ArmorButton.PreviewMouseDown += ArmorButton_Click;

            InterfaceButton NameButton = new InterfaceButton(200, 45, 7, 20);
            NameButton.SetText(Links.Interface("SchemaName"));
            NameButton.PutToCanvas(mainCanvas, 830, 450);
            NameButton.PreviewMouseDown += NameButton_Click;

            InterfaceButton ConfirmButton = new InterfaceButton(200, 45, 7, 20);
            ConfirmButton.SetText(Links.Interface("Save"));
            ConfirmButton.PutToCanvas(mainCanvas, 1050, 425);
            ConfirmButton.PreviewMouseDown += ConfirmButton_Click;

            InterfaceButton DeleteButton = new InterfaceButton(200, 45, 7, 20);
            DeleteButton.SetText(Links.Interface("Delete"));
            DeleteButton.PutToCanvas(mainCanvas, 1050, 475);
            DeleteButton.PreviewMouseDown += DeleteButton_Click;

            InterfaceButton CreateShipButton = new InterfaceButton(200, 45, 7, 20);
            CreateShipButton.SetText(Links.Interface("CreateShip"));
            CreateShipButton.PutToCanvas(mainCanvas, 830, 500);
            CreateShipButton.PreviewMouseDown += CreateShipButton_Click;

            //HelpWindow.Place(HelpWindows.Schemas);
        }
        void ShipTypeInfoPanel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            List<ShipTypeclass> list = new List<ShipTypeclass>();
            foreach (ShipTypeclass s in Links.ShipTypes.Values)
                if (GSGameInfo.SciencesArray.Contains(s.ID))
                    list.Add(s);
            SelectItemCanvas canvas = new SelectItemCanvas(list.ToArray(), ShipTypeclass.GetShipTypeColumnNames(),
                new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 }, Links.Interface("SelectShipType"), ApplyEnum.ShipType);
            canvas.SetBaseSortParam(0, 0, SortVector.FromUp);
            canvas.SetBaseSortParam(1, 1, SortVector.FromUp);
            Links.Controller.PopUpCanvas.Place(canvas);
        }
        void GeneratorInfoPanel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            GameObjectImage image = (GameObjectImage)sender;
            ItemSize MaxSize = (ItemSize)image.Tag;
            List<Generatorclass> list = new List<Generatorclass>();
            foreach (Generatorclass s in Links.GeneratorTypes.Values)
                if (GSGameInfo.SciencesArray.Contains(s.ID) && s.Size <= MaxSize)
                    list.Add(s);
            SelectItemCanvas canvas = new SelectItemCanvas(list.ToArray(), Generatorclass.GetGeneratorTypeColumnNames(),
                new int[] { 0, 1, 2, 3, 4, 5, 6 }, Links.Interface("SelectGenerator"), ApplyEnum.Generator);
            canvas.SetBaseSortParam(2, 0, SortVector.FromUp);
            canvas.SetBaseSortParam(1, 1, SortVector.FromUp);
            Links.Controller.PopUpCanvas.Place(canvas);
        }
        void ShieldInfoPanel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            GameObjectImage image = (GameObjectImage)sender;
            ItemSize MaxSize = (ItemSize)image.Tag;
            List<Shieldclass> list = new List<Shieldclass>();
            foreach (Shieldclass s in Links.ShieldTypes.Values)
                if (GSGameInfo.SciencesArray.Contains(s.ID) && s.Size <= MaxSize)
                    list.Add(s);
            SelectItemCanvas canvas = new SelectItemCanvas(list.ToArray(), Shieldclass.GetShieldTypeColumnNames(),
                new int[] { 0, 1, 2, 3, 4, 5, 6, 7 }, Links.Interface("SelectShield"), ApplyEnum.Shield);
            canvas.SetBaseSortParam(3, 0, SortVector.FromUp);
            canvas.SetBaseSortParam(1, 1, SortVector.FromUp);
            Links.Controller.PopUpCanvas.Place(canvas);
        }
        void ComputerInfoPanel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            GameObjectImage image = (GameObjectImage)sender;
            ItemSize MaxSize = (ItemSize)image.Tag;
            List<Computerclass> list = new List<Computerclass>();
            foreach (Computerclass s in Links.ComputerTypes.Values)
                if (GSGameInfo.SciencesArray.Contains(s.ID) && s.Size <= MaxSize)
                    list.Add(s);
            SelectItemCanvas canvas = new SelectItemCanvas(list.ToArray(), Computerclass.GetComputerTypeColumnNames(),
                new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 }, Links.Interface("SelectComputer"), ApplyEnum.Computer);
            canvas.SetBaseSortParam(4, 0, SortVector.FromUp);
            canvas.SetBaseSortParam(0, 1, SortVector.FromUp);
            Links.Controller.PopUpCanvas.Place(canvas);
        }
        void EngineInfoPanel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            GameObjectImage image = (GameObjectImage)sender;
            ItemSize MaxSize = (ItemSize)image.Tag;
            List<Engineclass> list = new List<Engineclass>();
            foreach (Engineclass s in Links.EngineTypes.Values)
                if (GSGameInfo.SciencesArray.Contains(s.ID) && s.Size <= MaxSize)
                    list.Add(s);
            SelectItemCanvas canvas = new SelectItemCanvas(list.ToArray(), Engineclass.GetEngineTypeColumnNames(),
                new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, Links.Interface("SelectEngine"), ApplyEnum.Engine);
            canvas.SetBaseSortParam(5, 0, SortVector.FromUp);
            canvas.SetBaseSortParam(0, 1, SortVector.FromUp);
            Links.Controller.PopUpCanvas.Place(canvas);
        }
        void WeaponInfoPanel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            GameObjectImage image = (GameObjectImage)sender;
            SelectItemHelper helper = (SelectItemHelper)image.Tag;
            List<Weaponclass> list = new List<Weaponclass>();
            foreach (Weaponclass s in Links.WeaponTypes.Values)
                if (GSGameInfo.SciencesArray.Contains(s.ID) && s.CheckWeaponGroup(helper.Group) && s.Size <= helper.Size)
                    list.Add(s);
            SelectItemCanvas canvas = new SelectItemCanvas(list.ToArray(), Weaponclass.GetWeaponColumnNames(),
                new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, Links.Interface("SelectWeapon"), helper.Apply);
            canvas.SetBaseSortParam(0, 0, SortVector.FromDown);
            canvas.SetBaseSortParam(5, 1, SortVector.FromUp);
            canvas.SetBaseSortParam(1, 2, SortVector.FromUp);
            Links.Controller.PopUpCanvas.Place(canvas);
        }
        void EquipmentInfoPanel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            GameObjectImage image = (GameObjectImage)sender;
            SelectItemHelper helper = (SelectItemHelper)image.Tag;
            List<Equipmentclass> list = new List<Equipmentclass>();
            foreach (Equipmentclass s in Links.EquipmentTypes.Values)
                if (GSGameInfo.SciencesArray.Contains(s.ID) && s.Size <= helper.Size)
                    list.Add(s);
            SelectItemCanvas canvas = new SelectItemCanvas(list.ToArray(), Equipmentclass.GetEquipmentColumnNames(),
                new int[] { 0, 1, 2, 3, 4, 5, 6, 7 }, Links.Interface("SelectEquipment"), helper.Apply);
            canvas.SetBaseSortParam(0, 0, SortVector.FromDown);
            canvas.SetBaseSortParam(3, 1, SortVector.FromUp);
            canvas.SetBaseSortParam(1, 2, SortVector.FromUp);
            Links.Controller.PopUpCanvas.Place(canvas);
        }
        void ArmorButton_Click(object sender, RoutedEventArgs e)
        {
            ApplyEnum target = ApplyEnum.Armor;
            List<ShipArmor> list = Links.Armors.Values.ToList();
            SelectItemCanvas canvas = new SelectItemCanvas(list.ToArray(), ShipArmor.GetArmorColumnNames(),
                new int[] { 0, 1, 2, 3 }, Links.Interface("SelectArmorType"), target);
            Links.Controller.PopUpCanvas.Place(canvas);
        }

        void CreateShipButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CurSchema.Check())
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(Links.Interface("SchemaError")), true);
                return;
            }
            if (GSGameInfo.ShipsCap <= GSGameInfo.Ships.Count)
            {
                //Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(Links.Interface("HangarFull")), true);
                //return;
            }
            CurSchema.CalcPrice();
            if (!GSGameInfo.CheckPrice(CurSchema.Price))
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(Links.Interface("NoMoney")), true);
                return;
            }
            ShipCustomizePanel panel = new ShipCustomizePanel(CurSchema);
            Links.Controller.PopUpCanvas.Place(panel);
        }

        void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            List<Schema> list = new List<Schema>();
            if (SelectedSchema != Int32.MaxValue)
            {
                for (int i = 0; i < GSGameInfo.PlayerSchemas.Count; i++)
                    if (i != SelectedSchema)
                        list.Add(GSGameInfo.PlayerSchemas[i]);
                TryToSaveSchemas(list);
            }
            Select();
        }

        void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CurSchema.Check())
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(Links.Interface("SchemaError")), true);
                return;
            }
            List<Schema> list = new List<Schema>();
            if (SelectedSchema == Int32.MaxValue)
            {
                list.Add(CurSchema);
                for (int i = 0; i < GSGameInfo.PlayerSchemas.Count; i++)
                    list.Add(GSGameInfo.PlayerSchemas[i]);
            }
            else
            {
                for (int i = 0; i < GSGameInfo.PlayerSchemas.Count; i++)
                    if (i == SelectedSchema)
                        list.Add(CurSchema);
                    else
                        list.Add(GSGameInfo.PlayerSchemas[i]);
            }
            TryToSaveSchemas(list);
            Links.Controller.SchemasCanvas.Select();
        }
        bool TryToSaveSchemas(List<Schema> list)
        {
            List<byte> bytes = new List<byte>();
            foreach (Schema schema in list)
                bytes.AddRange(schema.GetCrypt());
            byte[] array = bytes.ToArray();
            return Events.AppendShipSchemas(array);

        }
        void NameButton_Click(object sender, RoutedEventArgs e)
        {
            SelectSchemaNameCanvas canvas = new SelectSchemaNameCanvas(CurSchema.CryptName);
            Links.Controller.PopUpCanvas.Place(canvas);
        }
        public void ApplyChanges()
        {
            switch (Target)
            {
                case ApplyEnum.ShipType: CurSchema.SetShipType(SelectID); break;
                case ApplyEnum.Weapon1: CurSchema.SetWeapon(SelectID, 0, true); break;
                case ApplyEnum.Weapon2: CurSchema.SetWeapon(SelectID, 1, true); break;
                case ApplyEnum.Weapon3: CurSchema.SetWeapon(SelectID, 2, true); break;
                case ApplyEnum.Generator: CurSchema.SetGenerator(SelectID); break;
                case ApplyEnum.Shield: CurSchema.SetShield(SelectID); break;
                case ApplyEnum.Computer: CurSchema.SetComputer(SelectID); break;
                case ApplyEnum.Engine: CurSchema.SetEngine(SelectID); break;
                case ApplyEnum.Equipment1: CurSchema.SetEquipment(SelectID, 0); break;
                case ApplyEnum.Equipment2: CurSchema.SetEquipment(SelectID, 1); break;
                case ApplyEnum.Equipment3: CurSchema.SetEquipment(SelectID, 2); break;
                case ApplyEnum.Equipment4: CurSchema.SetEquipment(SelectID, 3); break;
                case ApplyEnum.Name: CurSchema.SetName(SelectSchemaNameCanvas.CurName); break;
                case ApplyEnum.Armor: CurSchema.Armor = (ArmorType)SelectID; break;
            }
            Refresh(false);
        }

    }

    enum ApplyEnum { None, ShipType, Name, Generator, Shield, Computer, Engine, Weapon1, Weapon2, Weapon3, Equipment1, Equipment2, Equipment3, Equipment4, Armor }
    class SchemaInfoPanel : Border
    {
        public SchemaInfoPanel(int tag)
        {
            BorderBrush = Brushes.Black;
            BorderThickness = new Thickness(3);

            Width = 300;
            Height = 300;

            Background = Links.Brushes.AddImageBrush;
            Tag = tag;
        }
    }
    class SchemaPricePanel : Border
    {
        Canvas mainCanvas;
        public SchemaPricePanel(ShipB ship)
        {
            BorderBrush = Brushes.Black;
            BorderThickness = new Thickness(3);
            CornerRadius = new CornerRadius(20);
            Width = 300;
            Height = 70;

            Background = Brushes.Gray;

            mainCanvas = new Canvas();
            Child = mainCanvas;

            ship.Schema.CalcPrice();

            Common.PutRect(mainCanvas, 30, Links.Brushes.MoneyImageBrush, 5, 5, false);
            Common.PutLabel(mainCanvas, 35, 5, 18, ship.Schema.Price.Money.ToString());
            Common.PutRect(mainCanvas, 30, Links.Brushes.MetalImageBrush, 150, 5, false);
            Common.PutLabel(mainCanvas, 185, 5, 18, ship.Schema.Price.Metall.ToString());
            Common.PutRect(mainCanvas, 30, Links.Brushes.ChipsImageBrush, 5, 35, false);
            Common.PutLabel(mainCanvas, 35, 35, 18, ship.Schema.Price.Chips.ToString());
            Common.PutRect(mainCanvas, 30, Links.Brushes.AntiImageBrush, 155, 35, false);
            Common.PutLabel(mainCanvas, 185, 35, 18, ship.Schema.Price.Anti.ToString());
        }
    }
    class SelectItemHelper
    {
        public ApplyEnum Apply;
        public ItemSize Size;
        public WeaponGroup Group;
        public SelectItemHelper(ApplyEnum apply, ItemSize size, WeaponGroup group)
        {
            Apply = apply; Size = size; Group = group;
        }
    }
}

