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
    class NewSchemasCanvas : Canvas
    {
        public static ApplyEnum Target;
        public ushort SelectID;
        public Schema CurSchema;
        public Viewbox box;
        public int SelectedSchema;
        enum Positions { Stage1, NewSchema, GetDamager, GetDefender, GetScout, GetSupport, LoadSchemas }
        Positions CurPosition;
        public NewSchemasCanvas()
        {
            Width = 1280;
            Height = 600;
            box = new Viewbox();
            box.Width = 1920;
            box.Height = 900;
            box.Margin = new Thickness(0, 80, 0, 0);
            Background = Links.Brushes.Interface.SchemaBack2;
            box.HorizontalAlignment = HorizontalAlignment.Center;
            box.VerticalAlignment = VerticalAlignment.Center;
            box.Child = this;
            CurPosition = Positions.Stage1;
        }
        public void Select()
        {
            CurPosition = Positions.Stage1;
            Refresh();
        }
        public void Refresh()
        {
            if (CurPosition == Positions.Stage1)
            {
                CreateStage1();
            }
            else
            {
                if (CurPosition != Positions.LoadSchemas)
                    SelectedSchema = Int32.MaxValue;
                CreateStage2();
                Canvas RefreshButton = GetRightButton(1107, 120, Links.Brushes.Interface.Refresh);
                RefreshButton.PreviewMouseDown += RefreshButton_PreviewMouseDown;

                Ellipse CloseEllipse = new Ellipse(); CloseEllipse.Width = 70; CloseEllipse.Height = 70;
                CloseEllipse.Fill = Brushes.Red; Children.Add(CloseEllipse); Canvas.SetLeft(CloseEllipse, 1057);
                Canvas.SetTop(CloseEllipse, 38); CloseEllipse.Opacity = 0.01;
                CloseEllipse.PreviewMouseDown += CloseEllipse_PreviewMouseDown;

                if (CurSchema.ShipType == null)
                {
                    AddShipClassSelect();
                }
                else
                {
                    Viewbox panelvbx = new Viewbox(); panelvbx.Width = 340; panelvbx.Height = 191;
                    ShipPanel2 panel = ShipPanel2.FromSchema(CurSchema);
                    panelvbx.Child = panel;
                    panel.UpdateLevel();
                    Children.Add(panelvbx); Canvas.SetLeft(panelvbx, 460); Canvas.SetTop(panelvbx, 205);
                    Canvas GeneratorCanvas;
                    if (CurSchema.Generator == null)
                        GeneratorCanvas = GetBigElementCanvas(Links.Interface("Generator"), 20, 285, 110, -1, null);
                    else
                        GeneratorCanvas = GetBigElementCanvas(GameObjectName.GetGeneratorName(CurSchema.Generator), 14, 285, 110, CurSchema.GeneratorID, Links.Brushes.GeneratorBrush);
                    GeneratorCanvas.Tag = CurSchema.ShipType.GeneratorSize;
                    GeneratorCanvas.PreviewMouseDown += GeneratorCanvas_PreviewMouseDown;
                    Canvas ShieldCanvas;
                    if (CurSchema.Shield == null)
                        ShieldCanvas = GetBigElementCanvas(Links.Interface("ShipShield"), 20, 285, 310, -1, null);
                    else
                        ShieldCanvas = GetBigElementCanvas(GameObjectName.GetShieldName(CurSchema.Shield), 14, 285, 310, CurSchema.ShieldID, Links.Brushes.ShieldBrush);
                    ShieldCanvas.Tag = CurSchema.ShipType.ShieldSize;
                    ShieldCanvas.PreviewMouseDown += ShieldCanvas_PreviewMouseDown;
                    Canvas ComputerCanvas;
                    if (CurSchema.Computer == null)
                        ComputerCanvas = GetBigElementCanvas(Links.Interface("Computer"), 20, 860, 110, -1, null);
                    else
                        ComputerCanvas = GetBigElementCanvas(GameObjectName.GetComputerName(CurSchema.Computer), 14, 860, 110, CurSchema.ComputerID, Links.Brushes.ComputerBrush);
                    ComputerCanvas.Tag = CurSchema.ShipType.ComputerSize;
                    ComputerCanvas.PreviewMouseDown += ComputerCanvas_PreviewMouseDown;
                    Canvas EngineCanvas;
                    if (CurSchema.Engine == null)
                        EngineCanvas = GetBigElementCanvas(Links.Interface("Engine"), 20, 860, 310, -1, null);
                    else
                        EngineCanvas = GetBigElementCanvas(GameObjectName.GetEngineName(CurSchema.Engine), 14, 860, 310, CurSchema.EngineID, Links.Brushes.EngineBrush);
                    EngineCanvas.Tag = CurSchema.ShipType.EngineSize;
                    EngineCanvas.PreviewMouseDown += EngineCanvas_PreviewMouseDown;
                    if (CurSchema.ShipType.WeaponCapacity > 1)
                    {
                        Canvas LeftWeaponCanvas;
                        if (CurSchema.GetWeapon(0) == null)
                            LeftWeaponCanvas = GetWeaponCanvas("", 20, 422, 50, -1);
                        else
                            LeftWeaponCanvas = GetWeaponCanvas(GameObjectName.GetWeaponName(CurSchema.GetWeapon(0)), 14, 422, 50, CurSchema.GetWeapon(0).ID);
                        LeftWeaponCanvas.Tag = new SelectItemHelper(ApplyEnum.Weapon1, CurSchema.ShipType.GetWeaponParams(0).Size, CurSchema.ShipType.GetWeaponParams(0).Group);
                        LeftWeaponCanvas.PreviewMouseDown += WeaponInfoPanel_PreviewMouseDown;
                    }
                    if (CurSchema.ShipType.WeaponCapacity == 1 || CurSchema.ShipType.WeaponCapacity == 3)
                    {
                        Canvas MiddleWeaponCanvas;
                        if (CurSchema.GetWeapon(1) == null)
                            MiddleWeaponCanvas = GetWeaponCanvas("", 20, 570, 30, -1);
                        else
                            MiddleWeaponCanvas = GetWeaponCanvas(GameObjectName.GetWeaponName(CurSchema.GetWeapon(1)), 14, 570, 30, CurSchema.GetWeapon(1).ID);
                        MiddleWeaponCanvas.Tag = new SelectItemHelper(ApplyEnum.Weapon2, CurSchema.ShipType.GetWeaponParams(1).Size, CurSchema.ShipType.GetWeaponParams(1).Group);
                        MiddleWeaponCanvas.PreviewMouseDown += WeaponInfoPanel_PreviewMouseDown;
                    }
                    if (CurSchema.ShipType.WeaponCapacity > 1)
                    {
                        Canvas RightWeaponCanvas;
                        if (CurSchema.GetWeapon(2) == null)
                            RightWeaponCanvas = GetWeaponCanvas("", 20, 715, 50, -1);
                        else
                            RightWeaponCanvas = GetWeaponCanvas(GameObjectName.GetWeaponName(CurSchema.GetWeapon(2)), 14, 715, 50, CurSchema.GetWeapon(2).ID);
                        RightWeaponCanvas.Tag = new SelectItemHelper(ApplyEnum.Weapon3, CurSchema.ShipType.GetWeaponParams(2).Size, CurSchema.ShipType.GetWeaponParams(2).Group);
                        RightWeaponCanvas.PreviewMouseDown += WeaponInfoPanel_PreviewMouseDown;
                    }
                    for (int i = 0; i < CurSchema.ShipType.EquipmentCapacity; i++)
                    {
                        Canvas EquipmentCanvas;
                        if (CurSchema.GetEquipment(i) == null)
                            EquipmentCanvas = GetEquipmentCanvas("", 20, 175 + i / 2 * 825, 130 + i % 2 * 175, -1);
                        else
                            EquipmentCanvas = GetEquipmentCanvas(GameObjectName.GetEquipmentName(CurSchema.GetEquipment(i)), 14, 175 + i / 2 * 825, 130 + i % 2 * 175, CurSchema.GetEquipment(i).ID);
                        EquipmentCanvas.Tag = new SelectItemHelper((ApplyEnum)(10 + i), CurSchema.ShipType.EquipmentsSize[i], WeaponGroup.Any);
                        EquipmentCanvas.PreviewMouseDown += EquipmentInfoPanel_PreviewMouseDown;
                    }
                    Canvas ArmorButton = GetRightButton(1107, 340, Links.Brushes.ArmorSelect);
                    ArmorButton.PreviewMouseDown += ArmorButton_Click;

                    Canvas SaveSchema = GetRightButton(1107, 230, Links.Brushes.Interface.AutoComputer);
                    SaveSchema.PreviewMouseDown += SaveSchema_PreviewMouseDown;

                    Canvas MakeShip = GetLeftButton(48, 120, Links.Brushes.ShipCreate);
                    MakeShip.PreviewMouseDown += MakeShip_PreviewMouseDown;

                    Canvas DeleteSchema = GetLeftButton(48, 230, Links.Brushes.EmptyImageBrush);
                    DeleteSchema.PreviewMouseDown += DeleteSchema_PreviewMouseDown;

                    PricePanel PricePanel = new PricePanel(300);
                    Children.Add(PricePanel); Canvas.SetLeft(PricePanel, 490); Canvas.SetTop(PricePanel, 460);
                    CurSchema.CalcPrice();
                    PricePanel.ChangePrice(CurSchema.Price);
                }
            }
        }

        private void CloseEllipse_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            CurPosition = Positions.Stage1;
            Refresh();
        }

        private void MakeShip_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!CurSchema.Check())
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(Links.Interface("SchemaError")), true);
                return;
            }
            if (GSGameInfo.ShipsCap <= GSGameInfo.Ships.Count)
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(Links.Interface("HangarFull")), true);
                return;
            }
            CurSchema.CalcPrice();
            if (!GSGameInfo.CheckPrice(CurSchema.Price))
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(Links.Interface("NoMoney")), true);
                return;
            }
            ShipCreateNew panel = new ShipCreateNew(CurSchema);
            //ShipCustomizePanel panel = new ShipCustomizePanel(CurSchema);
            Children.Clear();
            Children.Add(panel);
            //Canvas.SetLeft(panel, 90); Canvas.SetTop(panel, 50);
        }

        private void DeleteSchema_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            List<Schema> list = new List<Schema>();
            if (SelectedSchema != Int32.MaxValue)
            {
                for (int i = 0; i < GSGameInfo.PlayerSchemas.Count; i++)
                    if (i != SelectedSchema)
                        list.Add(GSGameInfo.PlayerSchemas[i]);
                if (TryToSaveSchemas(list))
                {
                    Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage("Delete Done"), true);
                    Gets.GetTotalInfo("После удаления схемы корабля");
                }
            }
            CurPosition = Positions.Stage1;
            Refresh();
        }

        private void SaveSchema_PreviewMouseDown(object sender, MouseButtonEventArgs e)
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
            if (TryToSaveSchemas(list))
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage("Save Done"), true);
                Gets.GetTotalInfo("После сохранения схемы корабля");
            }
        }
        bool TryToSaveSchemas(List<Schema> list)
        {
            List<byte> bytes = new List<byte>();
            foreach (Schema schema in list)
                bytes.AddRange(schema.GetCrypt());
            byte[] array = bytes.ToArray();
            return Events.AppendShipSchemas(array);

        }
        private void RefreshButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            switch (CurPosition)
            {
                case Positions.NewSchema: CurSchema = new Schema(); break;
                case Positions.GetDamager: CurSchema = SchemeGenerator.GetWarriorSchema(); break;
                case Positions.GetDefender: CurSchema = SchemeGenerator.GetDefenderSchema(); break;
                case Positions.GetScout: CurSchema = SchemeGenerator.GetSpeedShipSchema(); break;
                case Positions.GetSupport: CurSchema = SchemeGenerator.GetSupportSchema(); break;
                case Positions.LoadSchemas: Children.Clear(); Children.Add(new LoadSchemaWindow()); return;
                default: return;
            }
            Refresh();
        }
        void ArmorButton_Click(object sender, RoutedEventArgs e)
        {
            ApplyEnum target = ApplyEnum.Armor;
            List<ShipArmor> list = Links.Armors.Values.ToList();
            SelectItemCanvas canvas = new SelectItemCanvas(list.ToArray(), ShipArmor.GetArmorColumnNames(),
                new int[] { 0, 1, 2, 3 }, Links.Interface("SelectArmorType"), target);
            Links.Controller.PopUpCanvas.Place(canvas);
        }
        void EquipmentInfoPanel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Canvas c = (Canvas)sender;
            SelectItemHelper helper = (SelectItemHelper)c.Tag;
            SortedSet<WeaponGroup> WeaponGroups = new SortedSet<WeaponGroup>();
            for (int i = 0; i < 3; i++)
                if (CurSchema.Weapons.Length <= i || CurSchema.Weapons[i] == null) continue;
                else
                    WeaponGroups.Add(CurSchema.Weapons[i].Group);
            if (WeaponGroups.Count == 1)
                helper.Group = WeaponGroups.ElementAt(0);
            SelectItemForSchemaCanvas canvas = new SelectItemForSchemaCanvas(Links.Interface("SelectEquipment"), helper.Apply, helper);
            canvas.SetEquipment(helper.Size, helper.Group);
            Links.Controller.PopUpCanvas.Place(canvas);
        }
        void WeaponInfoPanel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Canvas c = (Canvas)sender;
            SelectItemHelper helper = (SelectItemHelper)c.Tag;
            SelectItemForSchemaCanvas canvas = new SelectItemForSchemaCanvas(Links.Interface("SelectWeapon"), helper.Apply, helper);
            canvas.SetWeapons(helper.Group, helper.Size);
            Links.Controller.PopUpCanvas.Place(canvas);
        }
        private void EngineCanvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Canvas c = (Canvas)sender;
            ItemSize MaxSize = (ItemSize)c.Tag;
            SelectItemHelper helper = new SelectItemHelper(ApplyEnum.Engine, MaxSize, WeaponGroup.Any);
            SelectItemForSchemaCanvas canvas = new SelectItemForSchemaCanvas(Links.Interface("SelectEngine"), helper.Apply, helper);
            canvas.SetEngine(MaxSize);
            Links.Controller.PopUpCanvas.Place(canvas);
        }
        private void ComputerCanvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Canvas c = (Canvas)sender;
            ItemSize MaxSize = (ItemSize)c.Tag;
            SelectItemHelper helper = new SelectItemHelper(ApplyEnum.Computer, MaxSize, WeaponGroup.Any);
            SelectItemForSchemaCanvas canvas = new SelectItemForSchemaCanvas(Links.Interface("SelectComputer"), helper.Apply, helper);
            canvas.SetComputer(MaxSize);
            Links.Controller.PopUpCanvas.Place(canvas);
        }
        private void ShieldCanvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Canvas c = (Canvas)sender;
            ItemSize MaxSize = (ItemSize)c.Tag;
            SelectItemHelper helper = new SelectItemHelper(ApplyEnum.Shield, MaxSize, WeaponGroup.Any);
            SelectItemForSchemaCanvas canvas = new SelectItemForSchemaCanvas(Links.Interface("SelectShield"), helper.Apply, helper);
            canvas.SetShield(MaxSize);
            Links.Controller.PopUpCanvas.Place(canvas);
        }
        private void GeneratorCanvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Canvas c = (Canvas)sender;
            ItemSize MaxSize = (ItemSize)c.Tag;
            SelectItemHelper helper = new SelectItemHelper(ApplyEnum.Generator, MaxSize, WeaponGroup.Any);
            SelectItemForSchemaCanvas canvas = new SelectItemForSchemaCanvas(Links.Interface("SelectGenerator"), helper.Apply, helper);
            canvas.SetGenerator(MaxSize);
            Links.Controller.PopUpCanvas.Place(canvas);
        }
        Canvas GetRightButton(int left, int top, Brush brush)
        {
            Canvas canvas = new Canvas();
            Children.Add(canvas); Canvas.SetLeft(canvas, left); Canvas.SetTop(canvas, top);
            Rectangle Back = new Rectangle(); Back.Width = 120; Back.Height = 110; Back.Fill = Links.Brushes.Interface.AddPanel;
            canvas.Children.Add(Back);
            Rectangle image = Common.GetRectangle(85, brush);
            canvas.Children.Add(image);
            Canvas.SetLeft(image, 3); Canvas.SetTop(image, 3);
            return canvas;
        }
        Canvas GetLeftButton(int left, int top, Brush brush)
        {
            Canvas canvas = new Canvas();
            Children.Add(canvas); Canvas.SetLeft(canvas, left); Canvas.SetTop(canvas, top);
            Rectangle Back = new Rectangle(); Back.Width = 120; Back.Height = 110; Back.Fill = Links.Brushes.Interface.AddPanel;
            canvas.Children.Add(Back);
            Back.RenderTransformOrigin = new Point(0.5, 0.5);
            Back.RenderTransform = new ScaleTransform(-1, 1);
            Rectangle image = Common.GetRectangle(85, brush);
            canvas.Children.Add(image);
            Canvas.SetLeft(image, 30); Canvas.SetTop(image, 3);
            return canvas;
        }
        Canvas GetEquipmentCanvas(string text, int textsize, int left, int top, int id)
        {
            Canvas canvas = new Canvas();
            Children.Add(canvas); Canvas.SetLeft(canvas, left); Canvas.SetTop(canvas, top);
            Label lbl = new Label(); lbl.Width = 125; lbl.Height = 65; lbl.VerticalContentAlignment = VerticalAlignment.Bottom;
            Canvas.SetLeft(lbl, -15);
            canvas.Children.Add(lbl);
            TextBlock block = Common.GetBlock(textsize, text, Brushes.White, 118);
            lbl.Content = block;
            Rectangle rect = Common.GetRectangle(100, Links.Brushes.Interface.SmallSquare); 
            canvas.Children.Add(rect); Canvas.SetTop(rect, 60);
            if (id > 0)
            {
                Equipmentclass equip = Links.EquipmentTypes[(ushort)id];
                Rectangle image = Pictogram.GetPict(equip, 70);
                canvas.Children.Add(image); Canvas.SetLeft(image, 15); Canvas.SetTop(image, 75);
                GameObjectImage img = new GameObjectImage(GOImageType.Standard, (ushort)id); img.Width = 300; img.Height = 450;
                image.ToolTip = img;
            }
            return canvas;
        }
        Canvas GetWeaponCanvas(string text, int textsize, int left, int top, int id)
        {
            Canvas canvas = new Canvas();
            Children.Add(canvas); Canvas.SetLeft(canvas, left); Canvas.SetTop(canvas, top);
            Label lbl = new Label(); lbl.Width = 145; lbl.Height = 85; lbl.VerticalContentAlignment = VerticalAlignment.Bottom;
            Canvas.SetLeft(lbl, -15);
            canvas.Children.Add(lbl);
            TextBlock block = Common.GetBlock(textsize, text, Brushes.White, 140);
            lbl.Content = block;
            Ellipse el = new Ellipse(); el.Width = 120; el.Height = 120; el.Fill = Links.Brushes.Interface.WeaponRound;
            canvas.Children.Add(el); Canvas.SetTop(el, 79);
            if (id > 0)
            {
                Weaponclass weapon = Links.WeaponTypes[(ushort)id];
                Rectangle image = Common.GetRectangle(70, Links.Brushes.WeaponsBrushes[weapon.Type]);
                canvas.Children.Add(image); Canvas.SetLeft(image, 22); Canvas.SetTop(image, 103);
                GameObjectImage img = new GameObjectImage(GOImageType.Standard, (ushort)id); img.Width = 300; img.Height = 450;
                image.ToolTip = img;
            }
            return canvas;
        }
        Canvas GetBigElementCanvas(string text, int textsize, int left, int top, int id, Brush brush)
        {
            Canvas canvas = new Canvas();
            Children.Add(canvas); Canvas.SetLeft(canvas, left); Canvas.SetTop(canvas, top);
            Label lbl = new Label(); lbl.Width = 125; lbl.Height = 85; lbl.VerticalContentAlignment = VerticalAlignment.Bottom;
            canvas.Children.Add(lbl);
            TextBlock block = Common.GetBlock(textsize, text, Brushes.White, 118);
            lbl.Content = block;
            Rectangle rect = Common.GetRectangle(140, Links.Brushes.Interface.BigSquare);
            canvas.Children.Add(rect); Canvas.SetLeft(rect, -5); Canvas.SetTop(rect, 70);
            if (id>0)
            {
                Rectangle image = Common.GetRectangle(100, brush);
                canvas.Children.Add(image); Canvas.SetLeft(image, 15); Canvas.SetTop(image, 90);
                GameObjectImage img = new GameObjectImage(GOImageType.Standard, (ushort)id); img.Width = 300; img.Height = 450;
                image.ToolTip = img;
            }
            return canvas;
        }
        void CreateStage2()
        {
            Children.Clear();
            Rectangle Back = new Rectangle(); Back.Width = 1000; Back.Height = 560;
            Back.Fill = Links.Brushes.Interface.SchemaGenerator;
            Children.Add(Back); Canvas.SetLeft(Back, 140); Canvas.SetTop(Back, 20);
        }
        public void AddShipClassSelect()
        {
            Path path = new Path(); path.Stroke = Brushes.SkyBlue; path.StrokeThickness = 3;
            path.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,174 h20 a 154,154 0 0,1 154,-154  v-20 M348,174 h-20 a154,154 0 0,1 -154,154 v20"));
            Children.Add(path); Canvas.SetLeft(path, 459); Canvas.SetTop(path, 140); path.RenderTransformOrigin = new Point(0.5, 0.5);
            RotateTransform rotate = new RotateTransform();
            path.RenderTransform = rotate;
            DoubleAnimation anim = new DoubleAnimation(0, 180, TimeSpan.FromSeconds(2));
            anim.RepeatBehavior = RepeatBehavior.Forever;
            anim.AccelerationRatio = 0.5; anim.DecelerationRatio = 0.5;
            rotate.BeginAnimation(RotateTransform.AngleProperty, anim);
            TextBlock text = Common.GetBlock(60, "Выберите модель", Brushes.White, 300);
            Children.Add(text); Canvas.SetLeft(text, 500); Canvas.SetTop(text, 230);
            Ellipse ellipse = new Ellipse();
            ellipse.Width = 348; ellipse.Height = 348; ellipse.Stroke = Brushes.SkyBlue;
            ellipse.StrokeThickness = 5; Children.Add(ellipse); Canvas.SetLeft(ellipse, 459);
            Canvas.SetTop(ellipse, 140);
            ellipse.Fill = new SolidColorBrush(Color.FromArgb(1, 0, 0, 0));
            ellipse.PreviewMouseDown += SelectShipModel;
        }

        private void SelectShipModel(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            SelectItemForSchemaCanvas canvas = new SelectItemForSchemaCanvas(Links.Interface("SelectShipType"), ApplyEnum.ShipType, null);
            canvas.SetShipType();
            Links.Controller.PopUpCanvas.Place(canvas);
            /*
            List<ShipTypeclass> list = new List<ShipTypeclass>();
            foreach (ShipTypeclass s in Links.ShipTypes.Values)
                if (GSGameInfo.SciencesList.SciencesArray.Contains(s.ID))
                    list.Add(s);
            SelectItemCanvas canvas = new SelectItemCanvas(list.ToArray(), ShipTypeclass.GetShipTypeColumnNames(),
                new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 }, Links.Interface("SelectShipType"), ApplyEnum.ShipType);
            canvas.SetBaseSortParam(0, 0, SortVector.FromUp);
            canvas.SetBaseSortParam(1, 1, SortVector.FromUp);
            Links.Controller.PopUpCanvas.Place(canvas);
            */
        }
        System.Windows.Threading.DispatcherTimer Timer;
        int pos = 0;
        int curpos = 0;
        ScrollViewer sv;
        TextBlock text;
        TextBlock Title;
        DateTime MoveEndTime;
        void CreateStage1()
        {
            Brush brush = new SolidColorBrush(Color.FromRgb(0, 168, 255));
            Children.Clear();
            Canvas canvas = new Canvas();
            Children.Add(canvas); Canvas.SetLeft(canvas, 340); Canvas.SetTop(canvas, 100);
            Path Lines = new Path();
            Lines.StrokeThickness = 2; Lines.Stroke = brush;
            Lines.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,25 h160 l20,35 h240 l20,-35 h160" +
                "M112, 30 h40 l17,30 M157,30 l17,30 M488,30 h-40 l-17,30 M443,30 l-17,30 M183,65 h234" +
                "M125, 20 h43 l10,-10 M166,15 l10,-10 h20 M475,20 h-43 l-10,-10 M434,15 l-10,-10 h-20" +
                "M0,375 h160 l20,-35 h240 l20,35 h160" +
                "M112, 370 h40 l17,-30 M157,370 l17,-30 M488,370 h-40 l-17,-30 M443,370 l-17,-30 M183,335 h234" +
                "M125, 380 h43 l10,10 M166,385 l10,10 h20 M475,380 h-43 l-10,10 M434,385 l-10,10 h-20" +
                "M190,75 a150,150 0 0,0 0,250 M185,75 a150,150 0 0,0 0,250 M410,75 a150,150 0 0,1 0,250 M415,75 a150,150 0 0,1 0,250"));
            canvas.Children.Add(Lines);
            Path Left = new Path(); Left.StrokeThickness = 2; Left.Stroke = brush; Left.Fill = Links.Brushes.Transparent;
            Left.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M0,50 l30,-50 v100z M22,15 v28 h-5 v15 h5 v28 M45,30 h-5 v45 h5"));
            canvas.Children.Add(Left); Canvas.SetLeft(Left, 50); Canvas.SetTop(Left, 135);
            Left.PreviewMouseDown += Left_PreviewMouseDown;

            Path Right = new Path(); Right.StrokeThickness = 2; Right.Stroke = brush; Right.Fill = Links.Brushes.Transparent;
            Right.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M0,50 l-30,-50 v100z M-22,15 v28 h5 v15 h-5 v28 M-45,30 h5 v45 h-5"));
            canvas.Children.Add(Right); Canvas.SetLeft(Right, 550); Canvas.SetTop(Right, 135);
            Right.PreviewMouseDown += Right_PreviewMouseDown;

            Path Round = new Path(); Round.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M200,75 a150,150 0 0,0 0,250 h30 a150,150 0 0,1 0,-250z M400,75 a150,150 0 0,1 0,250 h-30 a150,150 0 0,0 0,-250z"));
            RadialGradientBrush RoundBrush = new RadialGradientBrush(); RoundBrush.RadiusY = 0.65;
            RoundBrush.GradientStops.Add(new GradientStop(Color.FromRgb(00, 168, 255), 1));
            RoundBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0, 0, 168, 255), 0.9));
            Round.Fill = RoundBrush; canvas.Children.Add(Round);

            Canvas InnerC = new Canvas(); InnerC.Width = 300; InnerC.Height = 270;
            canvas.Children.Add(InnerC); Canvas.SetLeft(InnerC, 150); Canvas.SetTop(InnerC, 65);
            InnerC.Clip = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M150,0 a150,135 0 1,1 -0.1,0"));
            sv = new ScrollViewer(); sv.Width = 300; sv.Height = 250;
            InnerC.Children.Add(sv); Canvas.SetTop(sv, 10);
            sv.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
            sv.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
            StackPanel panel = new StackPanel(); panel.Orientation = Orientation.Horizontal;
            sv.Content = panel;
            panel.Children.Add(GetEll(Links.Brushes.Interface.ClearSchema, NewSchema_PreviewMouseDown));
            panel.Children.Add(GetEll(Links.Brushes.Interface.SpeedShip, Scout_PreviewMouseDown));
            panel.Children.Add(GetEll(Links.Brushes.Interface.DamageShip, DamagerSchema_PreviewMouseDown));
            panel.Children.Add(GetEll(Links.Brushes.Interface.TankShip, Defender_PreviewMouseDown));
            panel.Children.Add(GetEll(Links.Brushes.Interface.SupportShip, Support_PreviewMouseDown));
            //panel.Children.Add(GetEll(Links.Brushes.Interface.LoadSchema, LoadSchemas_PreviewMouseDown));
            Timer = new System.Windows.Threading.DispatcherTimer();
            Timer.Interval = TimeSpan.FromMilliseconds(30);
            Timer.Tick += Timer_Tick;
            
            Rectangle Helper = new Rectangle(); Helper.Width = 338; Helper.Height = 500;
            Children.Add(Helper); Canvas.SetLeft(Helper, 900); Canvas.SetTop(Helper, 100);
            Helper.Fill = Links.Brushes.Helper1;
            Border TextBorder = new Border(); TextBorder.BorderBrush = brush; TextBorder.BorderThickness = new Thickness(2);
            Children.Add(TextBorder); TextBorder.Width = 250; TextBorder.Height = 400;
            Canvas.SetLeft(TextBorder, 50); Canvas.SetTop(TextBorder, 100);
            text = Common.GetBlock(30, "");
            //text.VerticalAlignment = VerticalAlignment.Top;
            text.Foreground = Brushes.White;
            TextBorder.Child = text;
            
            sv.ScrollToHorizontalOffset(curpos);

            Border TitleBorder = new Border(); TitleBorder.Width = 240; TitleBorder.Height = 45;
            TitleBorder.Background = Links.Brushes.Transparent;
            canvas.Children.Add(TitleBorder); Canvas.SetLeft(TitleBorder, 180); Canvas.SetTop(TitleBorder, 10);
            Title = Common.GetBlock(30, "Пустая схема", Brushes.White, 240);
            TitleBorder.Child = Title;

            /*Rectangle Mirror = new Rectangle(); Mirror.Width = 240; Mirror.Height = 45;
            Mirror.Fill = new VisualBrush(TitleBorder);
            canvas.Children.Add(Mirror); Canvas.SetLeft(Mirror, 180); Canvas.SetTop(Mirror, 350);
            Mirror.RenderTransformOrigin = new Point(0.5, 0.5);
            Mirror.RenderTransform = new ScaleTransform(1, -1);
            Mirror.Opacity = 0.1;
            */
            Canvas LoadSchemas = new Canvas(); LoadSchemas.Background = brush;
            LoadSchemas.Width = 340; LoadSchemas.Height = 60;
            LoadSchemas.Clip = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,30 l18,-30 h233 l20,33 h-5 l-16,15 h-230 l-16,-15 h-5z"));
            canvas.Children.Add(LoadSchemas); Canvas.SetLeft(LoadSchemas, 165); Canvas.SetTop(LoadSchemas, 343);
            TextBlock LoadSchemasBlock = Common.GetBlock(30, Links.Interface("LoadSchemaT"), Brushes.White, 240);
            LoadSchemas.Children.Add(LoadSchemasBlock); Canvas.SetLeft(LoadSchemasBlock, 20); Canvas.SetTop(LoadSchemasBlock, 5);
            LoadSchemas.PreviewMouseDown += LoadSchemas_PreviewMouseDown;
            SetStartText();
        }

        private void Right_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            if (pos == 4) return;
            pos++;
            if (Timer.IsEnabled == false) { Timer.Start(); MoveEndTime = DateTime.Now; }
            SetStartText();
        }

        private void Left_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            if (pos == 0) return;
            pos--;
            if (Timer.IsEnabled==false) { Timer.Start(); MoveEndTime = DateTime.Now; }
            SetStartText();
        }
        void SetStartText()
        {
            switch (pos)
            {
                case 0: text.Text = Links.Interface("ClearSchema"); Title.Text = Links.Interface("ClearSchemaT"); break;
                case 1: text.Text = Links.Interface("SpeedShipSchema"); Title.Text = Links.Interface("SpeedShipSchemaT"); break;
                case 2: text.Text = Links.Interface("DamageShipSchema"); Title.Text = Links.Interface("DamageShipSchemaT"); break;
                case 3: text.Text = Links.Interface("TankShipSchema"); Title.Text = Links.Interface("TankShipSchemaT"); break;
                case 4: text.Text = Links.Interface("SupportShipSchema"); Title.Text = Links.Interface("SupportShipSchemaT"); break;
                //case 5: text.Text = Links.Interface("LoadSchema"); Title.Text = Links.Interface("LoadSchemaT"); break;
            }
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeSpan delta = DateTime.Now - MoveEndTime;
            MoveEndTime = DateTime.Now;
            int targetpos = pos * 300;
            if (curpos == targetpos) { Timer.Stop(); return; }
            else if (targetpos > curpos)
            { curpos += (int)delta.TotalMilliseconds; if (curpos > targetpos) curpos = targetpos; }
            else
            { curpos -= (int)delta.TotalMilliseconds; if (curpos < targetpos) curpos = targetpos; }
            sv.ScrollToHorizontalOffset(curpos);
        }

        Ellipse GetEll(Brush brush, MouseButtonEventHandler handler)
        {
            Ellipse el = new Ellipse();
            el.Width = 250; el.Height = 250; el.Margin = new Thickness(25, 0, 25, 0);
            el.Fill = brush;
            el.PreviewMouseDown += handler;
            return el;
        }
        void CreateStage12()
        {
            Children.Clear();
            Path NewSchema = PutPath("M0,160 l400,-100 v280 l-400,-100z");
            NewSchema.PreviewMouseDown += NewSchema_PreviewMouseDown;
            Path Scout = PutPath("M0,240 l400,100 v80 l-400,100z");
            Scout.PreviewMouseDown += Scout_PreviewMouseDown;
            Path DamagerSchema=PutPath("M0,520 l400,-100 h240 v180 h-640z");
            DamagerSchema.PreviewMouseDown += DamagerSchema_PreviewMouseDown;
            Path Defender = PutPath("M640,420 h240 l400,100 v80 h-640z");
            Defender.PreviewMouseDown += Defender_PreviewMouseDown;
            Path Support = PutPath("M880,420 l400,100 v-280 l-400,100z");
            Support.PreviewMouseDown += Support_PreviewMouseDown;
            Path LoadSchemas = PutPath("M880,340 l400,-100 v-80 l-400,-100z");
            LoadSchemas.PreviewMouseDown += LoadSchemas_PreviewMouseDown;
            PutText("Пустая схема",60, 300, 27, 157);
            PutText("Разведчик", 60, 300, 21, 325);
            PutText("Дамагер", 60, 300, 243, 486);
            PutText("Танк", 60, 300, 740, 486);
            PutText("Саппорт", 60, 300, 900, 325);
            PutText("Сохранённые схемы", 60, 300, 895, 139);
            
        }

        private void LoadSchemas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            CurPosition = Positions.LoadSchemas;
            Children.Clear();
            Children.Add(new LoadSchemaWindow());
        }

        private void Support_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            CurPosition = Positions.GetSupport;
            CurSchema = SchemeGenerator.GetSupportSchema();
            if (CurSchema == null)
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(Links.Interface("NoAutoSchema")), true);
                CurPosition = Positions.Stage1;
            }
            Refresh();
        }

        private void Defender_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            CurPosition = Positions.GetDefender;
            CurSchema = SchemeGenerator.GetDefenderSchema();
            if (CurSchema == null)
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(Links.Interface("NoAutoSchema")), true);
                CurPosition = Positions.Stage1;
            }
            Refresh();
        }

        private void Scout_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            CurPosition = Positions.GetScout;
            CurSchema = SchemeGenerator.GetSpeedShipSchema();
            if (CurSchema == null)
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(Links.Interface("NoAutoSchema")), true);
                CurPosition = Positions.Stage1;
            }
            Refresh();
        }

        private void DamagerSchema_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            CurPosition = Positions.GetDamager;
            CurSchema = SchemeGenerator.GetWarriorSchema();
            if (CurSchema == null)
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(Links.Interface("NoAutoSchema")), true);
                CurPosition = Positions.Stage1;
            }
                Refresh();
        }

        private void NewSchema_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            CurPosition = Positions.NewSchema;
            CurSchema = new Schema();
            Refresh();
        }
        
        Path PutPath(string text)
        {
            Path path = new Path();
            path.Stroke = Brushes.White;
            path.StrokeThickness = 5;
            path.Fill = Brushes.Gray;
            path.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(text));
            Children.Add(path);
            return path;
        }
        void PutText(string text, int size, int width, int left, int top)
        {
            TextBlock block = Common.GetBlock(size, text, Brushes.White, width);
            Children.Add(block);
            Canvas.SetLeft(block, left); Canvas.SetTop(block, top);
        }
        Rectangle PutRect(int width, int height, int left, int top, Brush brush)
        {
            Rectangle rect = Common.GetRectangle(width, brush);
            if (height != width) rect.Height = height;
            Children.Add(rect); Canvas.SetLeft(rect, left); Canvas.SetTop(rect, top);
            return rect;
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
            Refresh();
        }

    }
    public class LoadSchemaWindow:Canvas
    {
        SchemasCountFilter Filter;
        WrapPanel panel;
        public LoadSchemaWindow()
        {
            Width = 1200; Height = 500;
            Background = Brushes.Black;
            Canvas.SetLeft(this, 40);
            Canvas.SetTop(this, 50);
            Filter = new SchemasCountFilter(this);
            Children.Add(Filter);
            Canvas.SetLeft(Filter, 472);
            panel = new WrapPanel();
            panel.Width = 1200; panel.Height = 450;
            Children.Add(panel);
            Canvas.SetTop(panel, 50);
            Refresh();
        }
        public void Refresh()
        {
            panel.Children.Clear();
            for (int i = 0; i<GSGameInfo.PlayerSchemas.Count; i++)
            {
                if (i < (Filter.CurrentPage-1) * 6 || i > Filter.CurrentPage  * 6)
                    continue;
                Schema schema = GSGameInfo.PlayerSchemas[i];
                ShipPanel2 shippanel = ShipPanel2.GetShipPanel2(schema);
                shippanel.UpdateLevel();
                Viewbox vbx = new Viewbox(); vbx.Width = 400; vbx.Height = 225;
                vbx.Child = shippanel;
                panel.Children.Add(vbx);
                vbx.Tag = i;
                vbx.PreviewMouseDown += Vbx_PreviewMouseDown;
            }
        }

        private void Vbx_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                e.Handled = true;
                int tag = (int)((Viewbox)sender).Tag;
                Links.Controller.NewSchemasCanvas.SelectedSchema = tag;
                //Links.Controller.PopUpCanvas.Remove();
                Links.Controller.NewSchemasCanvas.CurSchema = GSGameInfo.PlayerSchemas[tag];
                Links.Controller.NewSchemasCanvas.Refresh();
            }
            else if (e.RightButton==MouseButtonState.Pressed)
            {
                e.Handled = true;
                Viewbox vbx = (Viewbox)sender;
                ShipPanel2 panel = (ShipPanel2)vbx.Child;
                Links.Controller.ShipPopUp.Place(panel.Ship.Schema);
            }
        }
        class SchemasCountFilter : Canvas
        {
            public int CurrentPage { get; private set; }
            Border Place;
            LoadSchemaWindow Window;
            public SchemasCountFilter(LoadSchemaWindow window)
            {
                Window = window;
                Rectangle rect = new Rectangle();
                rect.Width = 256; rect.Height = 60;
                rect.Fill = Links.Brushes.Interface.TextPanel;
                Children.Add(rect);
                Canvas.SetLeft(rect, 48);
                Width = 360; Height = 60;
                Background = Brushes.Black;

                Rectangle LeftArrow = Common.GetRectangle(50, Links.Brushes.Interface.LeftArrow);
                Children.Add(LeftArrow);

                LeftArrow.PreviewMouseDown += new MouseButtonEventHandler(LeftArrow_PreviewMouseDown);


                Rectangle RightArrow = Common.GetRectangle(50, Links.Brushes.Interface.RightArrow);
                Children.Add(RightArrow);
                Canvas.SetLeft(RightArrow, 302);
                RightArrow.PreviewMouseDown += new MouseButtonEventHandler(RightArrow_PreviewMouseDown);

                Place = new Border();
                Place.Width = 160;
                Place.Height = 45;
                Children.Add(Place);
                Canvas.SetLeft(Place, 100);
                Canvas.SetTop(Place, 7.5);

                CurrentPage = 1;
                Draw();
                
            }
            void Draw()
            {
                int pages = GSGameInfo.PlayerSchemas.Count / 6;
                if (pages * 6 < GSGameInfo.PlayerSchemas.Count) pages++;
                if (CurrentPage > pages) CurrentPage = 1;
                else if (CurrentPage < 1) CurrentPage = pages;
                Place.Child = Common.GetBlock(30, String.Format("{0}/{1}", CurrentPage, pages), Brushes.White, new Thickness());
            }
            void RightArrow_PreviewMouseDown(object sender, MouseButtonEventArgs e)
            {
                e.Handled = true;
                CurrentPage++;
                Draw();
                Window.Refresh();
                
            }
            void LeftArrow_PreviewMouseDown(object sender, MouseButtonEventArgs e)
            {
                e.Handled = true;
                CurrentPage--;
                Draw();
                Window.Refresh();
                
            }
        }
    }
    class SelectItemForSchemaCanvas : Canvas
    {
        class ItemHelper
        {
            public static SortedList<Elements, string> Names = GetNames();
            public Elements Element;
            public string Title;
            public SortedSet<ushort> IDs;
            public ISort TopItem;
            public bool SizePriority;
            public bool NeedButton;
            public List<ItemHelper> InnerItems;
            public ItemHelper(Elements element, ItemSize size, bool sizepriority, bool button)
            {
                NeedButton = button;
                Element = element;
                SizePriority = sizepriority;
                IDs = new SortedSet<ushort>();
                foreach (ushort scienceid in GSGameInfo.SciencesArray)
                {
                    GameScience science = Links.Science.GameSciences[scienceid];
                    if (science.Element != element) continue;
                    if (science.Size > size) continue;
                    IDs.Add(scienceid);
                }
                Title = Names[element];
                GetTopItem();
                if (NeedButton && IDs.Count <= 1)
                    NeedButton = false;
                
            }
            public void SetInnerItems(List<Elements> Elements, ItemSize size, bool sizepriority)
            {
                InnerItems = new List<ItemHelper>();
                foreach (Elements element in Elements)
                    InnerItems.Add(new ItemHelper(element, size, sizepriority, true));
                NeedButton = true;
            }
            public void GetTopItem()
            {
                switch (Element)
                {
                    case Elements.ShipScount:
                    case Elements.ShipCorvette:
                    case Elements.ShipTransport:
                    case Elements.ShipBattle:
                    case Elements.ShipFrigate:
                    case Elements.ShipFighter:
                    case Elements.ShipDreadnought:
                    case Elements.ShipDevostator:
                    case Elements.ShipWarrior:
                    case Elements.ShipCruiser: TopItemFromLevelShip(); break;
                    case Elements.Laser:
                    case Elements.Emi:
                    case Elements.Plasma:
                    case Elements.Solar:
                    case Elements.Cannon:
                    case Elements.Gauss:
                    case Elements.Missle:
                    case Elements.Anti:
                    case Elements.Psi:
                    case Elements.Dark:
                    case Elements.Warp:
                    case Elements.Time:
                    case Elements.Slice:
                    case Elements.Rad:
                    case Elements.Drone:
                    case Elements.Magnet: TopItemFromWeaponDamage(); break;
                    case Elements.GeneratorCap: TopItemFromGenerator(0); break;
                    case Elements.GeneratorRegen: TopItemFromGenerator(1); break;
                    case Elements.ShieldCap: TopItemFromShield(0); break;
                    case Elements.ShieldRegen: TopItemFromShield(1); break;
                    case Elements.ComputerBasic:TopItemFromComputer(1); break;
                    case Elements.ComputerAccEn: TopItemFromComputer(1); break;
                    case Elements.ComputerAccPh: TopItemFromComputer(1); break;
                    case Elements.ComputerAccIr: TopItemFromComputer(1); break;
                    case Elements.ComputerAccCy: TopItemFromComputer(1); break;
                    case Elements.ComputerDamEn:
                    case Elements.ComputerDamPh:
                    case Elements.ComputerDamIr:
                    case Elements.ComputerDamCy: TopItemFromComputer(2); break;
                    case Elements.EngineUniverse:
                    case Elements.EngineBasic: TopItemFromEngine(); break;
                    case Elements.Health: 
                    case Elements.HealthRestore:
                    case Elements.Shield:
                    case Elements.ShieldRestore: 
                    case Elements.Energy:
                    case Elements.EnergyRestore:
                    case Elements.AccuracyEnergy:
                    case Elements.AccuracyPhysic:
                    case Elements.AccuracyIrregular:
                    case Elements.AccuracyCyber:
                    case Elements.AccuracyTotal:
                    case Elements.EvasionEnergy:
                    case Elements.EvasionPhysic:
                    case Elements.EvasionIrregular:
                    case Elements.EvasionCyber:
                    case Elements.EvasionTotal:
                    case Elements.DamageEnergy:
                    case Elements.DamagePhysic:
                    case Elements.DamageIrregular:
                    case Elements.DamageCyber:
                    case Elements.DamageTotal:
                    case Elements.IgnoreEnergy:
                    case Elements.IgnorePhysic:
                    case Elements.IgnoreIrregular:
                    case Elements.IgnoreCyber:
                    case Elements.IgnoreTotal: TopItemFromEquip(); break;
                    case Elements.ImmuneEnergy:
                    case Elements.ImmunePhysic:
                    case Elements.ImmuneIrregular:
                    case Elements.ImmuneCyber:
                    case Elements.ImmuneTotal: TopItemFromConsume(); break;
                }
            }
            void TopItemFromConsume()
            {
                if (IDs.Count == 0) return;
                //int minconsume = 100;
                Equipmentclass TopEquip = Links.EquipmentTypes[IDs.ElementAt(0)];
                //ItemSize maxsize = TopEquip.Size;
                foreach (ushort id in IDs)
                {
                    Equipmentclass equip = Links.EquipmentTypes[id];
                    if (equip.Size > TopEquip.Size)
                        TopEquip = equip;
                    else if (equip.Size == TopEquip.Size && equip.Consume < TopEquip.Consume)
                        TopEquip = equip;
                    //{ minconsume = equip.Consume; TopEquip = equip; }
                    TopItem = (ISort)TopEquip;
                }
            }
            void TopItemFromEquip()
            {
                if (IDs.Count == 0) return;
                if (SizePriority == false)
                {
                    int maxvalue = 0;
                    Equipmentclass TopEquip = Links.EquipmentTypes[IDs.ElementAt(0)];
                    foreach (ushort id in IDs)
                    {
                        Equipmentclass equip = Links.EquipmentTypes[id];
                        if (equip.Value > maxvalue)
                        { maxvalue = equip.Value; TopEquip = equip; }
                        else if (equip.Value == maxvalue || equip.Size > TopEquip.Size)
                        { maxvalue = equip.Value; TopEquip = equip; }
                        TopItem = (ISort)TopEquip;
                    }
                }
                else
                {
                    int maxvalue = 0; ItemSize maxsize = ItemSize.Small;
                    Equipmentclass TopEquip = Links.EquipmentTypes[IDs.ElementAt(0)];
                    foreach (ushort id in IDs)
                    {
                        Equipmentclass equip = Links.EquipmentTypes[id];
                        if (equip.Size > maxsize)
                        { maxvalue = equip.Value; maxsize = equip.Size; TopEquip = equip; }
                        else if (equip.Size == maxsize && equip.Value > maxvalue)
                        { maxvalue = equip.Value; maxsize = equip.Size; TopEquip = equip; }
                    }
                    TopItem = (ISort)TopEquip;
                }
                
            }
            void TopItemFromEngine()
            {
                if (IDs.Count == 0) return;
                if (SizePriority == false)
                {
                    int maxvalue = 0;
                    Engineclass TopEngine = Links.EngineTypes[IDs.ElementAt(0)];
                    foreach (ushort id in IDs)
                    {
                        Engineclass engine = Links.EngineTypes[id];
                        if (engine.EnergyEvasion > maxvalue)
                        { maxvalue = engine.EnergyEvasion; TopEngine = engine; }
                        else if (engine.EnergyEvasion == maxvalue || engine.Size > TopEngine.Size)
                        { maxvalue = engine.EnergyEvasion; TopEngine = engine; }
                    }
                    TopItem = (ISort)TopEngine;
                }
                else
                {
                    int maxvalue = 0; ItemSize maxsize = ItemSize.Small;
                    Engineclass TopEngine = Links.EngineTypes[IDs.ElementAt(0)];
                    foreach (ushort id in IDs)
                    {
                        Engineclass engine = Links.EngineTypes[id];
                        if (engine.Size > maxsize)
                        { maxvalue = engine.EnergyEvasion; maxsize = engine.Size; TopEngine = engine; }
                        else if (engine.Size == maxsize && engine.EnergyEvasion > maxvalue)
                        { maxvalue = engine.EnergyEvasion; maxsize = engine.Size; TopEngine = engine;}
                    }
                    TopItem = (ISort)TopEngine;
                }
            }
            void TopItemFromComputer(int pos)
            {
                if (IDs.Count == 0) return;
                int maxvalue = 0;
                Computerclass TopComp = Links.ComputerTypes[IDs.ElementAt(0)];
                foreach (ushort id in IDs)
                {
                    Computerclass computer = Links.ComputerTypes[id];
                    if (computer.GetParam(pos)>maxvalue)
                    {
                        maxvalue = computer.GetParam(pos);
                        TopComp = computer;
                    }
                    else if (computer.GetParam(pos)==maxvalue || computer.Size>TopComp.Size)
                    {
                        maxvalue = computer.GetParam(pos);
                        TopComp = computer;
                    }
                }
                TopItem = TopComp;
            }
            void TopItemFromShield(int pos)
            {
                if (IDs.Count == 0) return;
                if (SizePriority == false)
                {
                    int maxvalue = 0;
                    Shieldclass TopShield = Links.ShieldTypes[IDs.ElementAt(0)];
                    foreach (ushort id in IDs)
                    {
                        Shieldclass shield = Links.ShieldTypes[id];
                        if (shield.GetParam(pos) > maxvalue)
                        {
                            maxvalue = shield.GetParam(pos);
                            TopShield = shield;
                        }
                        else if (shield.GetParam(pos) == maxvalue || shield.Size > TopShield.Size)
                        {
                            maxvalue = shield.GetParam(pos);
                            TopShield = shield;
                        }
                    }
                    TopItem = (ISort)TopShield;
                }
                else
                {
                    int maxvalue = 0; ItemSize maxsize = ItemSize.Small;
                    Shieldclass TopShield = Links.ShieldTypes[IDs.ElementAt(0)];
                    foreach (ushort id in IDs)
                    {
                        Shieldclass shield = Links.ShieldTypes[id];
                        if (shield.Size > maxsize)
                        {
                            maxvalue = shield.GetParam(pos);
                            maxsize = shield.Size;
                            TopShield = shield;
                        }
                        else if (shield.Size == maxsize && shield.GetParam(pos) > maxvalue)
                        {
                            maxvalue = shield.GetParam(pos);
                            maxsize = shield.Size;
                            TopShield = shield;
                        }
                    }
                    TopItem = (ISort)TopShield;
                }

            }
            void TopItemFromGenerator(int pos)
            {
                if (IDs.Count == 0) return;
                if (SizePriority == false)
                {
                    int maxvalue = 0;
                    Generatorclass TopGenerator = Links.GeneratorTypes[IDs.ElementAt(0)];
                    foreach (ushort id in IDs)
                    {
                        Generatorclass generator = Links.GeneratorTypes[id];
                        if (generator.GetParam(pos) > maxvalue)
                        {
                            maxvalue = generator.GetParam(pos);
                            TopGenerator = generator;
                        }
                        else if (generator.GetParam(pos) == maxvalue || generator.Size > TopGenerator.Size)
                        {
                            maxvalue = generator.GetParam(pos);
                            TopGenerator = generator;
                        }
                    }
                    TopItem = (ISort)TopGenerator;
                }
                else
                {
                    int maxvalue = 0; ItemSize maxsize = ItemSize.Small;
                    Generatorclass TopGenerator = Links.GeneratorTypes[IDs.ElementAt(0)];
                    foreach (ushort id in IDs)
                    {
                        Generatorclass generator = Links.GeneratorTypes[id];
                        if (generator.Size>maxsize)
                        {
                            maxvalue = generator.GetParam(pos);
                            maxsize = generator.Size;
                            TopGenerator = generator;
                        }
                        else if (generator.Size==maxsize && generator.GetParam(pos) > maxvalue)
                        {
                            maxvalue = generator.GetParam(pos);
                            maxsize = generator.Size;
                            TopGenerator = generator;
                        }
                    }
                    TopItem = (ISort)TopGenerator;
                }
                
            }
            void TopItemFromWeaponDamage()
            {
                if (IDs.Count == 0) return;
                int maxdamage = 0;
                Weaponclass TopWeapon = Links.WeaponTypes[IDs.ElementAt(0)];
                foreach (ushort id in IDs)
                {
                    Weaponclass weapon = Links.WeaponTypes[id];
                    if (weapon.Damage > maxdamage)
                    {
                        maxdamage = weapon.Damage;
                        TopWeapon = weapon;
                    }
                    else if (weapon.Damage == maxdamage && weapon.Size > TopWeapon.Size)
                    {
                        maxdamage = weapon.Damage;
                        TopWeapon = weapon;
                    }
                }
                TopItem = (ISort)TopWeapon;
            }
            void TopItemFromLevelShip()
            {
                if (IDs.Count == 0) return;
                int maxlevel = 0;
                ShipTypeclass TopType = Links.ShipTypes.Values[0];
                foreach (ushort id in IDs)
                {
                    ShipTypeclass shiptype = Links.ShipTypes[id];
                    if (shiptype.Level >= maxlevel) { maxlevel = shiptype.Level; TopType = shiptype; }
                }
                TopItem = (ISort)TopType;
            }
            static SortedList<Elements, string> GetNames()
            {
                SortedList<Elements, string> result = new SortedList<Elements, string>();
                result.Add(Elements.ShipScount, "Ship_Scout");
                result.Add(Elements.ShipCorvette, "Ship_Corvette");
                result.Add(Elements.ShipTransport, "Ship_Transport");
                result.Add(Elements.ShipBattle, "Ship_Battle");
                result.Add(Elements.ShipFrigate, "Ship_Frigate");
                result.Add(Elements.ShipFighter, "Ship_Fighter");
                result.Add(Elements.ShipDreadnought, "Ship_Dreadnought");
                result.Add(Elements.ShipDevostator, "Ship_Devostator");
                result.Add(Elements.ShipWarrior, "Ship_Warrior");
                result.Add(Elements.ShipCruiser, "Ship_Cruiser");
                result.Add(Elements.Laser, "Laser");
                result.Add(Elements.Emi, "EMI");
                result.Add(Elements.Plasma, "Plasma");
                result.Add(Elements.Solar, "Solar");
                result.Add(Elements.Cannon, "Cannon");
                result.Add(Elements.Gauss, "Gauss");
                result.Add(Elements.Missle, "Missle");
                result.Add(Elements.Anti, "Antimatter");
                result.Add(Elements.Psi, "Psi");
                result.Add(Elements.Dark, "DarkEnergy");
                result.Add(Elements.Warp, "Warp");
                result.Add(Elements.Time, "Time");
                result.Add(Elements.Slice, "Slice");
                result.Add(Elements.Rad, "Radiation");
                result.Add(Elements.Drone, "Drone");
                result.Add(Elements.Magnet, "Magnet");
                result.Add(Elements.GeneratorCap, "GeneratorCapacity");
                result.Add(Elements.GeneratorRegen, "GeneratorRecharge");
                result.Add(Elements.GeneratorCapSize, "LargeSizeGenerator");
                result.Add(Elements.GeneratorRegenSize, "LargeSizeGenerator");
                result.Add(Elements.ShieldCap, "ShieldCapacity");
                result.Add(Elements.ShieldRegen, "ShieldRecharge");
                result.Add(Elements.ShieldCapSize, "LargeSizeShield");
                result.Add(Elements.ShieldRegenSize, "LargeSizeShield");
                result.Add(Elements.ComputerBasic, "CompBasic");
                result.Add(Elements.ComputerAccEn, "CompAccEn");
                result.Add(Elements.ComputerAccPh, "CompAccPh");
                result.Add(Elements.ComputerAccIr, "CompAccIr");
                result.Add(Elements.ComputerAccCy, "CompAccCy");
                result.Add(Elements.ComputerDamEn, "CompDamEn");
                result.Add(Elements.ComputerDamPh, "CompDamPh");
                result.Add(Elements.ComputerDamIr, "CompDamIr");
                result.Add(Elements.ComputerDamCy, "CompDamCy");
                result.Add(Elements.EngineBasic, "EngineBasic");
                result.Add(Elements.EngineUniverse, "EngineUniverse");
                result.Add(Elements.EngineBasicSize, "LargeSizeEngine");
                result.Add(Elements.EngineUniverseSize, "LargeSizeEngine");
                result.Add(Elements.Health, "HealthEquip");
                result.Add(Elements.HealthRestore, "RegenHealthEquip");
                result.Add(Elements.Shield, "ShieldEquip");
                result.Add(Elements.ShieldRestore, "RegenShieldEquip");
                result.Add(Elements.Energy, "EnergyEquip");
                result.Add(Elements.EnergyRestore, "RegenEnergyEquip");
                result.Add(Elements.AccuracyEnergy, "AccuracyEquipEn");
                result.Add(Elements.AccuracyPhysic, "AccuracyEquipPh");
                result.Add(Elements.AccuracyIrregular, "AccuracyEquipIr");
                result.Add(Elements.AccuracyCyber, "AccuracyEquipCy");
                result.Add(Elements.AccuracyTotal, "AccuracyEquip");
                result.Add(Elements.EvasionEnergy, "EvasionEquipEn");
                result.Add(Elements.EvasionPhysic, "EvasionEquipPh");
                result.Add(Elements.EvasionIrregular, "EvasionEquipIr");
                result.Add(Elements.EvasionCyber, "EvasionEquipCy");
                result.Add(Elements.EvasionTotal, "EvasionEquip");
                result.Add(Elements.IgnoreEnergy, "IgnoreEquipEn");
                result.Add(Elements.IgnorePhysic, "IgnoreEquipPh");
                result.Add(Elements.IgnoreIrregular, "IgnoreEquipIr");
                result.Add(Elements.IgnoreCyber, "IgnoreEquipCy");
                result.Add(Elements.IgnoreTotal, "IgnoreEquip");
                result.Add(Elements.DamageEnergy, "DamageEquipEn");
                result.Add(Elements.DamagePhysic, "DamageEquipPh");
                result.Add(Elements.DamageIrregular, "DamageEquipIr");
                result.Add(Elements.DamageCyber, "DamageEquipCy");
                result.Add(Elements.DamageTotal, "DamageEquip");
                result.Add(Elements.ImmuneEnergy, "ImmuneEquipEn");
                result.Add(Elements.ImmunePhysic, "ImmuneEquipPh");
                result.Add(Elements.ImmuneIrregular, "ImmuneEquipIr");
                result.Add(Elements.ImmuneCyber, "ImmuneEquipCy");
                result.Add(Elements.ImmuneTotal, "ImmuneEquip");
                return result;
            }
        }
        TextBlock Title;
        WrapPanel panel;
        ApplyEnum Target;
        InterfaceButton MoveBack;
        SelectItemHelper Helper;
        public SelectItemForSchemaCanvas(string text, ApplyEnum target, SelectItemHelper helper)
        {
            Width = 1200; Height = 600;
            Target = target;
            Helper = helper;
            Background = new SolidColorBrush(Color.FromArgb(178, 0, 0, 0));

            Rectangle Back = new Rectangle(); Back.Width = 1200; Back.Height = 600;
            Children.Add(Back); Back.Stroke = Brushes.SkyBlue;
            Canvas.SetLeft(this, 40);
            Canvas.SetTop(this, 20);
            Title = Common.GetBlock(28, text, Brushes.White, 1100);
            Children.Add(Title); Canvas.SetLeft(Title, 50);
            ScrollViewer viewer = new ScrollViewer(); viewer.Width = 1180; viewer.Height = 500;
            Children.Add(viewer); Canvas.SetLeft(viewer, 10); Canvas.SetTop(viewer, 50);
            viewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            viewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            panel = new WrapPanel();
            viewer.Content = panel;

            InterfaceButton Close = new InterfaceButton(200, 40, 7, 24);
            Close.SetText(Links.Interface("Close"));
            Close.PutToCanvas(this, 950, 555);
            Close.PreviewMouseDown += Close_PreviewMouseDown;

            MoveBack = new InterfaceButton(200, 40, 7, 24);
            MoveBack.SetText(Links.Interface("MoveBack"));
            MoveBack.PutToCanvas(this, 50, 555);
            MoveBack.PreviewMouseDown += MoveBack_PreviewMouseDown;
            MoveBack.Visibility = Visibility.Hidden;

            InterfaceButton Grid = new InterfaceButton(200, 40, 7, 24);
            Grid.SetText(Links.Interface("ShowGrid"));
            Grid.PutToCanvas(this, 500, 555);
            Grid.PreviewMouseDown += Grid_PreviewMouseDown;
        }

        private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectItemCanvas canvas;
            switch (Target)
            {
                case ApplyEnum.ShipType:
                    List<ShipTypeclass> shiptypes = new List<ShipTypeclass>();
                    foreach (ShipTypeclass s in Links.ShipTypes.Values)
                        if (GSGameInfo.SciencesArray.Contains(s.ID))
                            shiptypes.Add(s);
                    canvas = new SelectItemCanvas(shiptypes.ToArray(), ShipTypeclass.GetShipTypeColumnNames(),
                        new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 }, Links.Interface("SelectShipType"), ApplyEnum.ShipType);
                    canvas.SetBaseSortParam(0, 0, SortVector.FromUp);
                    canvas.SetBaseSortParam(1, 1, SortVector.FromUp);
                    Links.Controller.PopUpCanvas.Place(canvas);
                    break;
                case ApplyEnum.Weapon1:
                case ApplyEnum.Weapon2:
                case ApplyEnum.Weapon3:
                    List<Weaponclass> weapons = new List<Weaponclass>();
                    foreach (Weaponclass s in Links.WeaponTypes.Values)
                        if (GSGameInfo.SciencesArray.Contains(s.ID) && s.CheckWeaponGroup(Helper.Group) && s.Size <= Helper.Size)
                            weapons.Add(s);
                    canvas = new SelectItemCanvas(weapons.ToArray(), Weaponclass.GetWeaponColumnNames(),
                        new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, Links.Interface("SelectWeapon"), Target);
                    canvas.SetBaseSortParam(0, 0, SortVector.FromDown);
                    canvas.SetBaseSortParam(5, 1, SortVector.FromUp);
                    canvas.SetBaseSortParam(1, 2, SortVector.FromUp);
                    Links.Controller.PopUpCanvas.Place(canvas);
                    break;
                case ApplyEnum.Generator:
                    ItemSize MaxSizeG = Helper.Size;
                    List<Generatorclass> generators = new List<Generatorclass>();
                    foreach (Generatorclass s in Links.GeneratorTypes.Values)
                        if (GSGameInfo.SciencesArray.Contains(s.ID) && s.Size <= MaxSizeG)
                            generators.Add(s);
                    canvas = new SelectItemCanvas(generators.ToArray(), Generatorclass.GetGeneratorTypeColumnNames(),
                        new int[] { 0, 1, 2, 3, 4, 5, 6 }, Links.Interface("SelectGenerator"), ApplyEnum.Generator);
                    canvas.SetBaseSortParam(2, 0, SortVector.FromUp);
                    canvas.SetBaseSortParam(1, 1, SortVector.FromUp);
                    Links.Controller.PopUpCanvas.Place(canvas);
                    break;
                case ApplyEnum.Shield:
                    ItemSize MaxSizeS = Helper.Size;
                    List<Shieldclass> shields = new List<Shieldclass>();
                    foreach (Shieldclass s in Links.ShieldTypes.Values)
                        if (GSGameInfo.SciencesArray.Contains(s.ID) && s.Size <= MaxSizeS)
                            shields.Add(s);
                    canvas = new SelectItemCanvas(shields.ToArray(), Shieldclass.GetShieldTypeColumnNames(),
                        new int[] { 0, 1, 2, 3, 4, 5, 6, 7 }, Links.Interface("SelectShield"), ApplyEnum.Shield);
                    canvas.SetBaseSortParam(3, 0, SortVector.FromUp);
                    canvas.SetBaseSortParam(1, 1, SortVector.FromUp);
                    Links.Controller.PopUpCanvas.Place(canvas);
                    break;
                case ApplyEnum.Computer:
                    ItemSize MaxSizeC = (ItemSize)Helper.Size;
                    List<Computerclass> comps = new List<Computerclass>();
                    foreach (Computerclass s in Links.ComputerTypes.Values)
                        if (GSGameInfo.SciencesArray.Contains(s.ID) && s.Size <= MaxSizeC)
                            comps.Add(s);
                    canvas = new SelectItemCanvas(comps.ToArray(), Computerclass.GetComputerTypeColumnNames(),
                        new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 }, Links.Interface("SelectComputer"), ApplyEnum.Computer);
                    canvas.SetBaseSortParam(4, 0, SortVector.FromUp);
                    canvas.SetBaseSortParam(0, 1, SortVector.FromUp);
                    Links.Controller.PopUpCanvas.Place(canvas);
                    break;
                case ApplyEnum.Engine:
                    ItemSize MaxSizeE = (ItemSize)Helper.Size;
                    List<Engineclass> enginies = new List<Engineclass>();
                    foreach (Engineclass s in Links.EngineTypes.Values)
                        if (GSGameInfo.SciencesArray.Contains(s.ID) && s.Size <= MaxSizeE)
                            enginies.Add(s);
                    canvas = new SelectItemCanvas(enginies.ToArray(), Engineclass.GetEngineTypeColumnNames(),
                        new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, Links.Interface("SelectEngine"), ApplyEnum.Engine);
                    canvas.SetBaseSortParam(5, 0, SortVector.FromUp);
                    canvas.SetBaseSortParam(0, 1, SortVector.FromUp);
                    Links.Controller.PopUpCanvas.Place(canvas);
                    break;
                case ApplyEnum.Equipment1:
                case ApplyEnum.Equipment2:
                case ApplyEnum.Equipment3:
                case ApplyEnum.Equipment4:
                    List<Equipmentclass> equips = new List<Equipmentclass>();
                    foreach (Equipmentclass s in Links.EquipmentTypes.Values)
                        if (GSGameInfo.SciencesArray.Contains(s.ID) && s.Size <= Helper.Size)
                            equips.Add(s);
                    canvas = new SelectItemCanvas(equips.ToArray(), Equipmentclass.GetEquipmentColumnNames(),
                        new int[] { 0, 1, 2, 3, 4, 5, 6, 7 }, Links.Interface("SelectEquipment"), Target);
                    canvas.SetBaseSortParam(0, 0, SortVector.FromDown);
                    canvas.SetBaseSortParam(3, 1, SortVector.FromUp);
                    canvas.SetBaseSortParam(1, 2, SortVector.FromUp);
                    Links.Controller.PopUpCanvas.Place(canvas);
                    break;
            }
        }

        private void MoveBack_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            panel.Children.Clear();
            PutStart();
            MoveBack.Visibility = Visibility.Hidden;
        }

        private void Close_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Links.Controller.PopUpCanvas.Remove();
        }

        Border GetInnerItemCanvas(ItemHelper helper)
        {
            Border border = new Border(); border.Width = 220; border.Height = 350;
            border.BorderBrush = Brushes.SkyBlue; border.BorderThickness = new Thickness(2);
            Canvas canvas = new Canvas(); border.Child = canvas;
            TextBlock Title = Common.GetBlock(20, Links.Interface(helper.Title), Brushes.White, 210);
            canvas.Children.Add(Title); Canvas.SetLeft(Title, 5); Canvas.SetTop(Title, 5);
            if (helper.TopItem != null)
            {
                GameObjectImage img = new GameObjectImage(GOImageType.Standard, (ushort)helper.TopItem.GetID());
                img.PreviewMouseDown += Img_PreviewMouseDown; img.Tag = (ushort)helper.TopItem.GetID();
                canvas.Children.Add(img);
                Canvas.SetLeft(img, 10); Canvas.SetTop(img, 50);
            }
            else
            {
                Rectangle rect = new Rectangle(); rect.Width = 200; rect.Height = 200;
                switch (helper.Element)
                {
                    case Elements.AccuracyTotal: rect.Fill = Pictogram.Accuracy; break;
                    case Elements.EvasionTotal: rect.Fill = Pictogram.Evasion; break;
                    case Elements.DamageTotal: rect.Fill = Pictogram.Damage; break;
                    case Elements.IgnoreTotal: rect.Fill = Pictogram.Ignore; break;
                    case Elements.ImmuneTotal: rect.Fill = Pictogram.Immune; break;
                }
                canvas.Children.Add(rect); Canvas.SetLeft(rect, 10); Canvas.SetTop(rect, 50);
            }
            if (helper.NeedButton)
            //if (helper.IDs.Count > 1 && helper.SizePriority==false)
            {
                InterfaceButton button = new InterfaceButton(200, 50, 7, 20);
                button.SetText(Links.Interface("MoreItems"));
                button.PutToCanvas(canvas, 10, 295);
                if (helper.InnerItems != null)
                {
                    button.Tag = helper.InnerItems;
                    button.PreviewMouseDown += ShowMiddleItems_PreviewMouseDown;
                }
                else
                {
                    button.Tag = helper;
                    button.PreviewMouseDown += ShowAllItems_PreviewMouseDown;
                }
            }
            return border;
        }
        Border GetInnerItemCanvasShort(ushort id)
        {
            Border border = new Border(); border.Width = 220; border.Height = 260;
            border.BorderBrush = Brushes.SkyBlue; border.BorderThickness = new Thickness(2);
            Canvas canvas = new Canvas(); border.Child = canvas;
            GameObjectImage img = new GameObjectImage(GOImageType.Standard, id);
            img.PreviewMouseDown += Img_PreviewMouseDown; img.Tag = id;
            canvas.Children.Add(img);
            Canvas.SetLeft(img, 10); Canvas.SetTop(img, 5);
            return border;
        }
        private void ShowMiddleItems_PreviewMouseDown (object sender, MouseButtonEventArgs e)
        {
            panel.Children.Clear();
            MoveBack.Visibility = Visibility.Visible;
            InterfaceButton button = (InterfaceButton)sender;
            List<ItemHelper> inner = (List<ItemHelper>)button.Tag;
            foreach (ItemHelper item in inner)
            {
                if (item.IDs.Count == 0) continue;
                Border border = GetInnerItemCanvas(item);
                panel.Children.Add(border);
            }
        }
        private void ShowAllItems_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            panel.Children.Clear();
            MoveBack.Visibility = Visibility.Visible;
            InterfaceButton button = (InterfaceButton)sender;
            ItemHelper helper = (ItemHelper)button.Tag;
            foreach (ushort id in helper.IDs)
                panel.Children.Add(GetInnerItemCanvasShort(id));
        }

        private void Img_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            GameObjectImage image = (GameObjectImage)sender;
            ushort id = (ushort)image.Tag;
            Links.Controller.NewSchemasCanvas.SelectID = id;
            NewSchemasCanvas.Target = Target;
            Links.Controller.NewSchemasCanvas.ApplyChanges();
            Links.Controller.PopUpCanvas.Remove();
        }

        List<ItemHelper> Helpers;
        public void PutStart()
        {
            foreach (ItemHelper helper in Helpers)
            {
                if (helper.IDs.Count == 0 && helper.InnerItems==null) continue;
                Border border = GetInnerItemCanvas(helper);
                panel.Children.Add(border);
            }
        }
        public void SetEquipment(ItemSize size, WeaponGroup group)
        {
            Helpers = new List<ItemHelper>();
            Helpers.Add(new ItemHelper(Elements.Health, size, size==ItemSize.Large, true));
            Helpers.Add(new ItemHelper(Elements.HealthRestore, size, size == ItemSize.Large, true));
            Helpers.Add(new ItemHelper(Elements.Shield, size, size == ItemSize.Large, true));
            Helpers.Add(new ItemHelper(Elements.ShieldRestore, size, size == ItemSize.Large, true));
            Helpers.Add(new ItemHelper(Elements.Energy, size, size == ItemSize.Large, true));
            Helpers.Add(new ItemHelper(Elements.EnergyRestore, size, size == ItemSize.Large, true));
            Helpers.Add(new ItemHelper(Elements.AccuracyTotal, size, size == ItemSize.Large, true));
            Helpers[6].SetInnerItems(new List<Elements>(new Elements[] { Elements.AccuracyEnergy, Elements.AccuracyPhysic,
            Elements.AccuracyIrregular, Elements.AccuracyCyber, Elements.AccuracyTotal}), size, size == ItemSize.Large);
            Helpers.Add(new ItemHelper(Elements.EvasionTotal, size, size == ItemSize.Large, true));
            Helpers[7].SetInnerItems(new List<Elements>(new Elements[] { Elements.EvasionEnergy, Elements.EvasionPhysic,
            Elements.EvasionIrregular, Elements.EvasionCyber, Elements.EvasionTotal}), size, size == ItemSize.Large);
            Helpers.Add(new ItemHelper(Elements.IgnoreTotal, size, size == ItemSize.Large, true));
            Helpers[8].SetInnerItems(new List<Elements>(new Elements[] { Elements.IgnoreEnergy, Elements.IgnorePhysic,
            Elements.IgnoreIrregular, Elements.IgnoreCyber, Elements.IgnoreTotal}), size, size == ItemSize.Large);
            Helpers.Add(new ItemHelper(Elements.DamageTotal, size, size == ItemSize.Large, true));
            Helpers[9].SetInnerItems(new List<Elements>(new Elements[] { Elements.DamageEnergy, Elements.DamagePhysic,
            Elements.DamageIrregular, Elements.DamageCyber, Elements.DamageTotal}), size, size == ItemSize.Large);
            Helpers.Add(new ItemHelper(Elements.ImmuneTotal, size, true, true));
            Helpers[10].SetInnerItems(new List<Elements>(new Elements[] { Elements.ImmuneEnergy, Elements.ImmunePhysic,
            Elements.ImmuneIrregular, Elements.ImmuneCyber, Elements.ImmuneTotal}), size, false);
            PutStart();
        }
        public void SetEngine(ItemSize size)
        {
            Helpers = new List<ItemHelper>();
            Helpers.Add(new ItemHelper(Elements.EngineBasic, size, false, true));
            Helpers.Add(new ItemHelper(Elements.EngineUniverse, size, false, true));
            if (Helpers[0].IDs.Count > 0 && Links.EngineTypes[(ushort)Helpers[0].TopItem.GetID()].Size < size)
            {
                Helpers.Add(new ItemHelper(Elements.EngineBasic, size, true, false));
                Helpers[2].Title = ItemHelper.Names[Elements.EngineBasicSize];
            }
            if (Helpers[1].IDs.Count > 0 && Links.EngineTypes[(ushort)Helpers[1].TopItem.GetID()].Size < size)
            {
                Helpers.Add(new ItemHelper(Elements.EngineUniverse, size, true, false));
                if (Helpers.Count == 4)
                    Helpers[3].Title = ItemHelper.Names[Elements.EngineUniverseSize];
                else
                    Helpers[2].Title = ItemHelper.Names[Elements.EngineUniverseSize];
            }
            PutStart();
        }
        public void SetComputer(ItemSize size)
        {
            Helpers = new List<ItemHelper>();
            Helpers.Add(new ItemHelper(Elements.ComputerBasic, size, false, true));
            Helpers.Add(new ItemHelper(Elements.ComputerAccEn, size, false, true));
            Helpers.Add(new ItemHelper(Elements.ComputerAccPh, size, false, true));
            Helpers.Add(new ItemHelper(Elements.ComputerAccIr, size, false, true));
            Helpers.Add(new ItemHelper(Elements.ComputerAccCy, size, false, true));
            Helpers.Add(new ItemHelper(Elements.ComputerDamEn, size, false, true));
            Helpers.Add(new ItemHelper(Elements.ComputerDamPh, size, false, true));
            Helpers.Add(new ItemHelper(Elements.ComputerDamIr, size, false, true));
            Helpers.Add(new ItemHelper(Elements.ComputerDamCy, size, false, true));
            PutStart();
        }
        public void SetShield(ItemSize size)
        {
            Helpers = new List<ItemHelper>();
            Helpers.Add(new ItemHelper(Elements.ShieldCap, size, false, true));
            Helpers.Add(new ItemHelper(Elements.ShieldRegen, size, false, true));
            if (Helpers[0].IDs.Count > 0 && Links.ShieldTypes[(ushort)Helpers[0].TopItem.GetID()].Size < size)
            {
                Helpers.Add(new ItemHelper(Elements.ShieldCap, size, true, false));
                Helpers[2].Title = ItemHelper.Names[Elements.ShieldCapSize];
            }
            if (Helpers[1].IDs.Count > 0 && Links.ShieldTypes[(ushort)Helpers[1].TopItem.GetID()].Size < size)
            {
                Helpers.Add(new ItemHelper(Elements.ShieldRegen, size, true, false));
                if (Helpers.Count == 4)
                    Helpers[3].Title = ItemHelper.Names[Elements.ShieldRegenSize];
                else
                    Helpers[2].Title = ItemHelper.Names[Elements.ShieldRegenSize];
            }
            PutStart();
        }
        public void SetGenerator(ItemSize size)
        {
            Helpers = new List<ItemHelper>();
            Helpers.Add(new ItemHelper(Elements.GeneratorCap, size, false, true));
            Helpers.Add(new ItemHelper(Elements.GeneratorRegen, size, false, true));
            if (Helpers[0].IDs.Count>0 && Links.GeneratorTypes[(ushort)Helpers[0].TopItem.GetID()].Size<size)
            {
                Helpers.Add(new ItemHelper(Elements.GeneratorCap, size, true, false));
                Helpers[2].Title = ItemHelper.Names[Elements.GeneratorCapSize];
            }
            if (Helpers[1].IDs.Count>0 && Links.GeneratorTypes[(ushort)Helpers[1].TopItem.GetID()].Size<size)
            {
                Helpers.Add(new ItemHelper(Elements.GeneratorRegen, size, true, false));
                if (Helpers.Count == 4)
                    Helpers[3].Title = ItemHelper.Names[Elements.GeneratorRegenSize];
                else
                    Helpers[2].Title = ItemHelper.Names[Elements.GeneratorRegenSize];
            }
            PutStart();
        }
        public void SetWeapons(WeaponGroup group, ItemSize size)
        {
            Helpers = new List<ItemHelper>();
            if (group == WeaponGroup.Energy || group == WeaponGroup.Any)
            {
                Helpers.Add(new ItemHelper(Elements.Laser, size, false, true));
                Helpers.Add(new ItemHelper(Elements.Emi, size, false, true));
                Helpers.Add(new ItemHelper(Elements.Plasma, size, false, true));
                Helpers.Add(new ItemHelper(Elements.Solar, size, false, true));
            }
            if (group == WeaponGroup.Physic || group==WeaponGroup.Any)
            {
                Helpers.Add(new ItemHelper(Elements.Cannon, size, false, true));
                Helpers.Add(new ItemHelper(Elements.Gauss, size, false, true));
                Helpers.Add(new ItemHelper(Elements.Missle, size, false, true));
                Helpers.Add(new ItemHelper(Elements.Anti, size, false, true));
            }
            if (group==WeaponGroup.Irregular || group==WeaponGroup.Any)
            {
                Helpers.Add(new ItemHelper(Elements.Psi, size, false, true));
                Helpers.Add(new ItemHelper(Elements.Dark, size, false, true));
                Helpers.Add(new ItemHelper(Elements.Warp, size, false, true));
                Helpers.Add(new ItemHelper(Elements.Time, size, false, true));
            }
            if (group==WeaponGroup.Cyber || group==WeaponGroup.Any)
            {
                Helpers.Add(new ItemHelper(Elements.Slice, size, false, true));
                Helpers.Add(new ItemHelper(Elements.Rad, size, false, true));
                Helpers.Add(new ItemHelper(Elements.Drone, size, false, true));
                Helpers.Add(new ItemHelper(Elements.Magnet, size, false, true));
            }
            PutStart();
        }
        public void SetShipType()
        {
            //Необходимо разбить все схемы кораблей не 10 групп и выделить в каждой топовый вариант схемы.
            //Если в группе нет членов - то группы нет.
            //Затем необходимо каждый топовый вариант расположить в собственном окне.
            //Если группа не из одного элемента, то необходимо расположить рядом кнопку с открыванием всех вариантов
            Helpers = new List<ItemHelper>();
            Helpers.Add(new ItemHelper(Elements.ShipCruiser, ItemSize.Any, false, true));
            Helpers.Add(new ItemHelper(Elements.ShipWarrior, ItemSize.Any, false, true));
            Helpers.Add(new ItemHelper(Elements.ShipDevostator, ItemSize.Any, false, true));
            Helpers.Add(new ItemHelper(Elements.ShipDreadnought, ItemSize.Any, false, true));
            Helpers.Add(new ItemHelper(Elements.ShipFighter, ItemSize.Any, false, true));
            Helpers.Add(new ItemHelper(Elements.ShipCorvette, ItemSize.Any, false, true));
            Helpers.Add(new ItemHelper(Elements.ShipBattle, ItemSize.Any, false, true));
            Helpers.Add(new ItemHelper(Elements.ShipScount,ItemSize.Any, false, true));
            Helpers.Add(new ItemHelper(Elements.ShipFrigate, ItemSize.Any, false, true));
            Helpers.Add(new ItemHelper(Elements.ShipTransport, ItemSize.Any, false, true));

            PutStart();
        }
        
    }
}
