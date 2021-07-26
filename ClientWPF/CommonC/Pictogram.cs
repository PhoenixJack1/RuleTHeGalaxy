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
    enum Picts { Health, Shield, Energy, Ignore, Damage, Increase, Decrease, Restore, Accuracy, Evasion, Immune, HealthDamage, ShieldDamage, CritDamage, Cargo, Colony, Speed }
    enum Target { None, Energy, Physic, Irregular, Cyber, Total, Health, Shield, Generator, White }
    class Pictogram
    {
        public static Brush HealthDamage;
        public static Brush ShieldDamage;
        public static Brush CriticalChance;
        public static Brush HealthBrush;
        public static Brush RestoreHealth;
        public static Brush ShieldBrush;
        public static Brush RestoreShield;
        public static Brush EnergyBrush;
        public static Brush RestoreEnergy;
        public static Brush Accuracy;
        public static Brush AccuracyEnergy;
        public static Brush AccuracyPhysic;
        public static Brush AccuracyIrregular;
        public static Brush AccuracyCyber;
        public static Brush Evasion;
        public static Brush EvasionEnergy;
        public static Brush EvasionPhysic;
        public static Brush EvasionIrregular;
        public static Brush EvasionCyber;
        public static Brush Ignore;
        public static Brush IgnoreEnergy;
        public static Brush IgnorePhysic;
        public static Brush IgnoreIrregular;
        public static Brush IgnoreCyber;
        public static Brush Damage;
        public static Brush DamageEnergy;
        public static Brush DamagePhysic;
        public static Brush DamageIrregular;
        public static Brush DamageCyber;

        public static Brush HealthBrushGroup;
        public static Brush RestoreHealthGroup;
        public static Brush ShieldBrushGroup;
        public static Brush RestoreShieldGroup;
        public static Brush EnergyBrushGroup;
        public static Brush RestoreEnergyGroup;
        public static Brush AccuracyGroup;
        public static Brush AccuracyEnergyGroup;
        public static Brush AccuracyPhysicGroup;
        public static Brush AccuracyIrregularGroup;
        public static Brush AccuracyCyberGroup;
        public static Brush EvasionGroup;
        public static Brush EvasionEnergyGroup;
        public static Brush EvasionPhysicGroup;
        public static Brush EvasionIrregularGroup;
        public static Brush EvasionCyberGroup;
        public static Brush IgnoreGroup;
        public static Brush IgnoreEnergyGroup;
        public static Brush IgnorePhysicGroup;
        public static Brush IgnoreIrregularGroup;
        public static Brush IgnoreCyberGroup;
        public static Brush DamageGroup;
        public static Brush DamageEnergyGroup;
        public static Brush DamagePhysicGroup;
        public static Brush DamageIrregularGroup;
        public static Brush DamageCyberGroup;

        public static Brush Immune;
        public static Brush ImmuneEnergy;
        public static Brush ImmunePhysic;
        public static Brush ImmuneIrregular;
        public static Brush ImmuneCyber;
        public static Brush ImmuneEnergyGroup;
        public static Brush ImmunePhysicGroup;
        public static Brush ImmuneIrregularGroup;
        public static Brush ImmuneCyberGroup;

        public static Brush FleetSpeedBrush;
        public static Brush IncreaseBrush;
        public static Brush DecreaseBrush;

        static PathFigureCollectionConverter conv = new PathFigureCollectionConverter();
        static ImageBrush GetPictBrush(string name)
        {
            if (Links.LoadImageFromDLL)
                return new ImageBrush(new BitmapImage(new Uri(String.Format("pack://application:,,,/GraphicLibrary;component/Images/Picts/{0}.png", name))));
            else
                return new ImageBrush(new BitmapImage(new Uri(String.Format("Images/Picts/{0}.png", name), UriKind.Relative)));
            //return new ImageBrush(new BitmapImage(new Uri(String.Format("Images//Picts//{0}.png", name), UriKind.Relative)));
        }
        public static void CreateBrushes2()
        {
            HealthDamage = GetPictBrush("P004_HealthDamage");
            ShieldDamage = GetPictBrush("P005_ShieldDamage");
            CriticalChance = new VisualBrush(GetPict(Picts.CritDamage, Target.None, false, null));
            HealthBrush = GetPictBrush("P001_Health");
            RestoreHealth = new VisualBrush(GetPict(Picts.Restore, Target.Health, false, null));
            ShieldBrush = GetPictBrush("P002_Shield");
            RestoreShield = new VisualBrush(GetPict(Picts.Restore, Target.Shield, false, null));
            EnergyBrush = GetPictBrush("P003_Energy");
            RestoreEnergy = new VisualBrush(GetPict(Picts.Restore, Target.Generator, false, null));
            Accuracy = new VisualBrush(GetPict(Picts.Accuracy, Target.Total, false, null));
            AccuracyEnergy = new VisualBrush(GetPict(Picts.Accuracy, Target.Energy, false, null));
            AccuracyPhysic = new VisualBrush(GetPict(Picts.Accuracy, Target.Physic, false, null));
            AccuracyIrregular = new VisualBrush(GetPict(Picts.Accuracy, Target.Irregular, false, null));
            AccuracyCyber = new VisualBrush(GetPict(Picts.Accuracy, Target.Cyber, false, null));
            Evasion = new VisualBrush(GetPict(Picts.Evasion, Target.Total, false, null));
            EvasionEnergy = GetPictBrush("P010_EvasionEn");
            EvasionPhysic = GetPictBrush("P011_EvasionPh");
            EvasionIrregular = GetPictBrush("P012_EvasionIr");
            EvasionCyber = GetPictBrush("P013_EvasionCy");
            Ignore = new VisualBrush(GetPict(Picts.Ignore, Target.Total, false, null));
            IgnoreEnergy = GetPictBrush("P006_AbsorbEn");
            IgnorePhysic = GetPictBrush("P007_AbsorbPh");
            IgnoreIrregular = GetPictBrush("P008_AbsorbIr");
            IgnoreCyber = GetPictBrush("P009_AbsorbCy");
            Damage = new VisualBrush(GetPict(Picts.Damage, Target.Total, false, null));
            DamageEnergy = GetPictBrush("P014_DamageEn");
            DamagePhysic = GetPictBrush("P015_DamagePh");
            DamageIrregular = GetPictBrush("P016_DamageIr");
            DamageCyber = GetPictBrush("P017_DamageCy");

            HealthBrushGroup = new VisualBrush(GetPict(Picts.Health, Target.None, true, null));
            RestoreHealthGroup = new VisualBrush(GetPict(Picts.Restore, Target.Health, true, null));
            ShieldBrushGroup = new VisualBrush(GetPict(Picts.Shield, Target.None, true, null));
            RestoreShieldGroup = new VisualBrush(GetPict(Picts.Restore, Target.Shield, true, null));
            EnergyBrushGroup = new VisualBrush(GetPict(Picts.Energy, Target.None, true, null));
            RestoreEnergyGroup = new VisualBrush(GetPict(Picts.Restore, Target.Generator, true, null));
            AccuracyGroup = new VisualBrush(GetPict(Picts.Accuracy, Target.Total, true, null));
            AccuracyEnergyGroup = new VisualBrush(GetPict(Picts.Accuracy, Target.Energy, true, null));
            AccuracyPhysicGroup = new VisualBrush(GetPict(Picts.Accuracy, Target.Physic, true, null));
            AccuracyIrregularGroup = new VisualBrush(GetPict(Picts.Accuracy, Target.Irregular, true, null));
            AccuracyCyberGroup = new VisualBrush(GetPict(Picts.Accuracy, Target.Cyber, true, null));
            EvasionGroup = new VisualBrush(GetPict(Picts.Evasion, Target.Total, true, null));
            EvasionEnergyGroup = new VisualBrush(GetPict(Picts.Evasion, Target.Energy, true, null));
            EvasionPhysicGroup = new VisualBrush(GetPict(Picts.Evasion, Target.Physic, true, null));
            EvasionIrregularGroup = new VisualBrush(GetPict(Picts.Evasion, Target.Irregular, true, null));
            EvasionCyberGroup = new VisualBrush(GetPict(Picts.Evasion, Target.Cyber, true, null));
            IgnoreGroup = new VisualBrush(GetPict(Picts.Ignore, Target.Total, true, null));
            IgnoreEnergyGroup = new VisualBrush(GetPict(Picts.Ignore, Target.Energy, true, null));
            IgnorePhysicGroup = new VisualBrush(GetPict(Picts.Ignore, Target.Cyber, true, null));
            IgnoreIrregularGroup = new VisualBrush(GetPict(Picts.Ignore, Target.Irregular, true, null));
            IgnoreCyberGroup = new VisualBrush(GetPict(Picts.Ignore, Target.Cyber, true, null));
            DamageGroup = new VisualBrush(GetPict(Picts.Damage, Target.Total, true, null));
            DamageEnergyGroup = new VisualBrush(GetPict(Picts.Damage, Target.Energy, true, null));
            DamagePhysicGroup = new VisualBrush(GetPict(Picts.Damage, Target.Physic, true, null));
            DamageIrregularGroup = new VisualBrush(GetPict(Picts.Damage, Target.Irregular, true, null));
            DamageCyberGroup = new VisualBrush(GetPict(Picts.Damage, Target.Cyber, true, null));

            Immune = new VisualBrush(GetPict(Picts.Immune, Target.Total, false, null));
            ImmuneEnergy = new VisualBrush(GetPict(Picts.Immune, Target.Energy, false, null));
            ImmunePhysic = new VisualBrush(GetPict(Picts.Immune, Target.Physic, false, null));
            ImmuneIrregular = new VisualBrush(GetPict(Picts.Immune, Target.Irregular, false, null));
            ImmuneCyber = new VisualBrush(GetPict(Picts.Immune, Target.Cyber, false, null));
            ImmuneEnergyGroup = new VisualBrush(GetPict(Picts.Immune, Target.Energy, true, null));
            ImmunePhysicGroup = new VisualBrush(GetPict(Picts.Immune, Target.Physic, true, null));
            ImmuneIrregularGroup = new VisualBrush(GetPict(Picts.Immune, Target.Irregular, true, null));
            ImmuneCyberGroup = new VisualBrush(GetPict(Picts.Immune, Target.Cyber, true, null));

            FleetSpeedBrush = new VisualBrush(GetPict(Picts.Speed, Target.None, false, null));
            IncreaseBrush = new VisualBrush(GetPict(Picts.Increase, Target.None, false, null));
            DecreaseBrush = new VisualBrush(GetPict(Picts.Decrease, Target.None, false, null));
        }
        public static void CreateBrushes()
        {
            HealthDamage = new VisualBrush(GetPict(Picts.HealthDamage, Target.None, false, null));
            ShieldDamage = new VisualBrush(GetPict(Picts.ShieldDamage, Target.None, false, null));
            CriticalChance = new VisualBrush(GetPict(Picts.CritDamage, Target.None, false, null));
            HealthBrush = new VisualBrush(GetPict(Picts.Health, Target.None, false, null));
            RestoreHealth = new VisualBrush(GetPict(Picts.Restore, Target.Health, false, null));
            ShieldBrush =new VisualBrush(GetPict(Picts.Shield, Target.None, false, null));
            RestoreShield = new VisualBrush(GetPict(Picts.Restore, Target.Shield, false, null));
            EnergyBrush = new VisualBrush(GetPict(Picts.Energy, Target.None, false, null));
            RestoreEnergy = new VisualBrush(GetPict(Picts.Restore, Target.Generator, false, null));
            Accuracy = new VisualBrush(GetPict(Picts.Accuracy, Target.Total, false, null));
            AccuracyEnergy = new VisualBrush(GetPict(Picts.Accuracy, Target.Energy, false, null));
            AccuracyPhysic = new VisualBrush(GetPict(Picts.Accuracy, Target.Physic, false, null));
            AccuracyIrregular = new VisualBrush(GetPict(Picts.Accuracy, Target.Irregular, false, null));
            AccuracyCyber = new VisualBrush(GetPict(Picts.Accuracy, Target.Cyber, false, null));
            Evasion = new VisualBrush(GetPict(Picts.Evasion, Target.Total, false, null));
            EvasionEnergy = new VisualBrush(GetPict(Picts.Evasion, Target.Energy, false, null));
            EvasionPhysic = new VisualBrush(GetPict(Picts.Evasion, Target.Physic, false, null));
            EvasionIrregular = new VisualBrush(GetPict(Picts.Evasion, Target.Irregular, false, null));
            EvasionCyber = new VisualBrush(GetPict(Picts.Evasion, Target.Cyber, false, null));
            Ignore = new VisualBrush(GetPict(Picts.Ignore, Target.Total, false, null));
            IgnoreEnergy = new VisualBrush(GetPict(Picts.Ignore, Target.Energy, false, null));
            IgnorePhysic = new VisualBrush(GetPict(Picts.Ignore, Target.Physic, false, null));
            IgnoreIrregular = new VisualBrush(GetPict(Picts.Ignore, Target.Irregular, false, null));
            IgnoreCyber = new VisualBrush(GetPict(Picts.Ignore, Target.Cyber, false, null));
            Damage = new VisualBrush(GetPict(Picts.Damage, Target.Total, false, null));
            DamageEnergy = new VisualBrush(GetPict(Picts.Damage, Target.Energy, false, null));
            DamagePhysic = new VisualBrush(GetPict(Picts.Damage, Target.Physic, false, null));
            DamageIrregular = new VisualBrush(GetPict(Picts.Damage, Target.Irregular, false, null));
            DamageCyber = new VisualBrush(GetPict(Picts.Damage, Target.Cyber, false, null));

            HealthBrushGroup = new VisualBrush(GetPict(Picts.Health, Target.None, true, null));
            RestoreHealthGroup = new VisualBrush(GetPict(Picts.Restore, Target.Health, true, null));
            ShieldBrushGroup = new VisualBrush(GetPict(Picts.Shield, Target.None, true, null));
            RestoreShieldGroup = new VisualBrush(GetPict(Picts.Restore, Target.Shield, true, null));
            EnergyBrushGroup = new VisualBrush(GetPict(Picts.Energy, Target.None, true, null));
            RestoreEnergyGroup = new VisualBrush(GetPict(Picts.Restore, Target.Generator, true, null));
            AccuracyGroup = new VisualBrush(GetPict(Picts.Accuracy, Target.Total, true, null));
            AccuracyEnergyGroup = new VisualBrush(GetPict(Picts.Accuracy, Target.Energy, true, null));
            AccuracyPhysicGroup = new VisualBrush(GetPict(Picts.Accuracy, Target.Physic, true, null));
            AccuracyIrregularGroup = new VisualBrush(GetPict(Picts.Accuracy, Target.Irregular, true, null));
            AccuracyCyberGroup = new VisualBrush(GetPict(Picts.Accuracy, Target.Cyber, true, null));
            EvasionGroup = new VisualBrush(GetPict(Picts.Evasion, Target.Total, true, null));
            EvasionEnergyGroup = new VisualBrush(GetPict(Picts.Evasion, Target.Energy, true, null));
            EvasionPhysicGroup = new VisualBrush(GetPict(Picts.Evasion, Target.Physic, true, null));
            EvasionIrregularGroup = new VisualBrush(GetPict(Picts.Evasion, Target.Irregular, true, null));
            EvasionCyberGroup = new VisualBrush(GetPict(Picts.Evasion, Target.Cyber, true, null));
            IgnoreGroup = new VisualBrush(GetPict(Picts.Ignore, Target.Total, true, null));
            IgnoreEnergyGroup = new VisualBrush(GetPict(Picts.Ignore, Target.Energy, true, null));
            IgnorePhysicGroup = new VisualBrush(GetPict(Picts.Ignore, Target.Cyber, true, null));
            IgnoreIrregularGroup = new VisualBrush(GetPict(Picts.Ignore, Target.Irregular, true, null));
            IgnoreCyberGroup = new VisualBrush(GetPict(Picts.Ignore, Target.Cyber, true, null));
            DamageGroup = new VisualBrush(GetPict(Picts.Damage, Target.Total, true, null));
            DamageEnergyGroup = new VisualBrush(GetPict(Picts.Damage, Target.Energy, true, null));
            DamagePhysicGroup = new VisualBrush(GetPict(Picts.Damage, Target.Physic, true, null));
            DamageIrregularGroup = new VisualBrush(GetPict(Picts.Damage, Target.Irregular, true, null));
            DamageCyberGroup = new VisualBrush(GetPict(Picts.Damage, Target.Cyber, true, null));

            Immune = new VisualBrush(GetPict(Picts.Immune, Target.Total, false, null));
            ImmuneEnergy = new VisualBrush(GetPict(Picts.Immune, Target.Energy, false, null));
            ImmunePhysic = new VisualBrush(GetPict(Picts.Immune, Target.Physic, false, null));
            ImmuneIrregular = new VisualBrush(GetPict(Picts.Immune, Target.Irregular, false, null));
            ImmuneCyber = new VisualBrush(GetPict(Picts.Immune, Target.Cyber, false, null));
            ImmuneEnergyGroup = new VisualBrush(GetPict(Picts.Immune, Target.Energy, true, null));
            ImmunePhysicGroup = new VisualBrush(GetPict(Picts.Immune, Target.Physic, true, null));
            ImmuneIrregularGroup = new VisualBrush(GetPict(Picts.Immune, Target.Irregular, true, null));
            ImmuneCyberGroup = new VisualBrush(GetPict(Picts.Immune, Target.Cyber, true, null));

            FleetSpeedBrush = new VisualBrush(GetPict(Picts.Speed, Target.None, false, null));
            IncreaseBrush = new VisualBrush(GetPict(Picts.Increase, Target.None, false, null));
            DecreaseBrush = new VisualBrush(GetPict(Picts.Decrease, Target.None, false, null));
        }
        static UIElement GetPict(Picts pict, Target target, bool isGroup, string tip, int size)
        {
            //FrameworkElement element = Common.GetRectangle(size, Brushes.Red);
            FrameworkElement  element = (FrameworkElement)GetPict(pict, target, isGroup, tip);
            if (size == 30) return element;
            element.RenderTransformOrigin = new Point(0.5, 0.5);
            ScaleTransform transform = new ScaleTransform(size / 30.0, size / 30.0);
            element.RenderTransform = transform;
            return element;
        }
        static UIElement GetPict(Picts pict, Target target, bool isGroup, string tip)
        {
            Color color = Colors.Black;
            switch (target)
            {
                case Target.Energy: color = Colors.Blue; break;
                case Target.Physic: color = Colors.Red; break;
                case Target.Irregular: color = Colors.Purple; break;
                case Target.Cyber: color = Colors.Green; break;
                case Target.Total: color = Colors.Gold; break;
                case Target.Health: color = Colors.Red; break;
                case Target.Shield: color = Colors.Blue; break;
                case Target.Generator: color = Colors.Green; break;
                case Target.White: color = Colors.LightGray; break;
            }
            UIElement result = null;
            switch (pict)
            {
                case Picts.Health: result = GetHealtPict(); break;
                case Picts.Energy: result = GetEnergyPict(); break;
                case Picts.Shield: result = GetShieldPict(); break;
                case Picts.Ignore: result = GetIgnorePict(color); break;
                case Picts.Damage: result = GetDamagePict(color); break;
                case Picts.Increase: result = GetIncreasePict(); break;
                case Picts.Decrease: result = GetDecreasePict(); break;
                case Picts.Restore: result = GetRestorePict(color); break;
                case Picts.Accuracy: result = GetAccuracyPict(color); break;
                case Picts.Evasion: result = GetEvasionPict(color); break;
                case Picts.Immune: result = GetImmunePict(color); break;
                case Picts.HealthDamage: result = GetHealthDamage(Colors.Yellow); break;
                case Picts.ShieldDamage: result = GetShieldDamage(Colors.Yellow); break;
                case Picts.CritDamage: result = GetCritPict(); break;
                case Picts.Cargo: result = GetCargoPict(); break;
                case Picts.Colony: result = GetColonyPict(); break;
                case Picts.Speed: result = GetSpeedPict(); break;
            }
            
            if (isGroup)
            {
                Canvas group = GetGroupCanvas();
                group.HorizontalAlignment = HorizontalAlignment.Center;
                group.VerticalAlignment = VerticalAlignment.Center;
                group.Children.Add(result);
                group.ToolTip = tip;
                ScaleTransform transform = new ScaleTransform(0.7, 0.7);
                result.RenderTransform = transform;
                Canvas.SetLeft(result, 4.5);
                Canvas.SetTop(result, 4.5);
                return group;
            }
            else
            {
                FrameworkElement el = (FrameworkElement)result;
                el.ToolTip = tip;
                return result;
            }

        }
        public static Rectangle GetPict(Equipmentclass equip, int size)
        {
            switch (equip.Type)
            {
                case 0: return Common.GetRectangle(size,AccuracyEnergy,  Links.Interface("EnAcc"));
                case 1: return Common.GetRectangle(size,AccuracyPhysic, Links.Interface("PhAcc"));
                case 2: return Common.GetRectangle(size,AccuracyIrregular, Links.Interface("IrAcc"));
                case 3: return Common.GetRectangle(size,AccuracyCyber, Links.Interface("CyAcc"));
                case 4: return Common.GetRectangle(size,Accuracy, Links.Interface("AlAcc"));
                case 5: return Common.GetRectangle(size, HealthBrush, Links.Interface("Hull"));
                case 6: return Common.GetRectangle(size,RestoreHealth, Links.Interface("HullRestore"));
                case 7: return Common.GetRectangle(size,ShieldBrush, Links.Interface("Shield"));
                case 8: return Common.GetRectangle(size,RestoreShield, Links.Interface("ShieldRestore"));
                case 9: return Common.GetRectangle(size,EnergyBrush, Links.Interface("Energy"));
                case 10: return Common.GetRectangle(size,RestoreEnergy, Links.Interface("EnergyRestore"));
                case 11: return Common.GetRectangle(size,EvasionEnergy, Links.Interface("EnEva"));
                case 12: return Common.GetRectangle(size,EvasionPhysic, Links.Interface("PhEva"));
                case 13: return Common.GetRectangle(size,EvasionIrregular, Links.Interface("IrEva"));
                case 14: return Common.GetRectangle(size,EvasionCyber, Links.Interface("CyEva"));
                case 15: return Common.GetRectangle(size,Evasion, Links.Interface("AlEva"));
                case 16: return Common.GetRectangle(size,IgnoreEnergy, Links.Interface("EnIgn"));
                case 17: return Common.GetRectangle(size,IgnorePhysic, Links.Interface("PhIgn"));
                case 18: return Common.GetRectangle(size,IgnoreIrregular, Links.Interface("IrIgn"));
                case 19: return Common.GetRectangle(size,IgnoreCyber, Links.Interface("CyIgn"));
                case 20: return Common.GetRectangle(size,Ignore, Links.Interface("AlIgn"));
                case 21: return Common.GetRectangle(size,DamageEnergy, Links.Interface("EnDam"));
                case 22: return Common.GetRectangle(size,DamagePhysic, Links.Interface("PhDam"));
                case 23: return Common.GetRectangle(size,DamageIrregular, Links.Interface("IrDam"));
                case 24: return Common.GetRectangle(size,DamageCyber, Links.Interface("CyDam"));
                case 25: return Common.GetRectangle(size,Damage, Links.Interface("AlDam"));
                case 30: return Common.GetRectangle(size,HealthBrushGroup, Links.Interface("HullGroup"));
                case 31: return Common.GetRectangle(size,RestoreHealthGroup, Links.Interface("HullRestoreGroup"));
                case 32: return Common.GetRectangle(size,ShieldBrushGroup, Links.Interface("ShieldGroup"));
                case 33: return Common.GetRectangle(size,RestoreShieldGroup, Links.Interface("ShieldRestoreGroup"));
                case 34: return Common.GetRectangle(size,EnergyBrushGroup, Links.Interface("EnergyGroup"));
                case 35: return Common.GetRectangle(size,RestoreEnergyGroup, Links.Interface("EnergyRestoreGroup"));
                case 36: return Common.GetRectangle(size,AccuracyEnergyGroup, Links.Interface("EnAccGr"));
                case 37: return Common.GetRectangle(size,AccuracyPhysicGroup, Links.Interface("PhAccGr"));
                case 38: return Common.GetRectangle(size,AccuracyIrregularGroup, Links.Interface("IrAccGr"));
                case 39: return Common.GetRectangle(size,AccuracyCyberGroup, Links.Interface("CyAccGr"));
                case 40: return Common.GetRectangle(size,AccuracyGroup, Links.Interface("AlAccGr"));
                case 41: return Common.GetRectangle(size,EvasionEnergyGroup, Links.Interface("EnEvaGr"));
                case 42: return Common.GetRectangle(size,EvasionPhysicGroup, Links.Interface("PhEvaGr"));
                case 43: return Common.GetRectangle(size,EvasionIrregularGroup, Links.Interface("IrEvaGr"));
                case 44: return Common.GetRectangle(size,EvasionCyberGroup, Links.Interface("CyEvaGr"));
                case 45: return Common.GetRectangle(size,EvasionGroup, Links.Interface("AlEvaGr"));
                case 46: return Common.GetRectangle(size,IgnoreEnergyGroup, Links.Interface("EnIgnGr"));
                case 47: return Common.GetRectangle(size,IgnorePhysicGroup, Links.Interface("PhIgnGr"));
                case 48: return Common.GetRectangle(size,IgnoreIrregularGroup, Links.Interface("IrIgnGr"));
                case 49: return Common.GetRectangle(size,IgnoreCyberGroup, Links.Interface("CyIgnGr"));
                case 50: return Common.GetRectangle(size,IgnoreGroup, Links.Interface("AlIgnGr"));
                case 51: return Common.GetRectangle(size,DamageEnergyGroup, Links.Interface("EnDamGr"));
                case 52: return Common.GetRectangle(size,DamagePhysicGroup, Links.Interface("PhDamGr"));
                case 53: return Common.GetRectangle(size,DamageIrregularGroup, Links.Interface("IrDamGr"));
                case 54: return Common.GetRectangle(size,DamageCyberGroup, Links.Interface("CyDamGr"));
                case 55: return Common.GetRectangle(size,DamageGroup, Links.Interface("AlDamGr"));
                case 57: return Common.GetRectangle(size,CargoBrush, Links.Interface("Cargo"));
                case 58: return Common.GetRectangle(size,ColonyBrush, Links.Interface("Colony"));
                case 59: return Common.GetRectangle(size,ImmuneEnergy, Links.Interface("EnImm"));
                case 60: return Common.GetRectangle(size,ImmunePhysic, Links.Interface("PhImm"));
                case 61: return Common.GetRectangle(size,ImmuneIrregular, Links.Interface("IrImm"));
                case 62: return Common.GetRectangle(size,ImmuneCyber, Links.Interface("CyImm"));
                case 63: return Common.GetRectangle(size,Immune, Links.Interface("AlImm"));
                case 64: return Common.GetRectangle(size, ImmuneEnergyGroup, Links.Interface("EnImmGr"));
                case 65: return Common.GetRectangle(size, ImmunePhysicGroup, Links.Interface("PhImmGr"));
                case 66: return Common.GetRectangle(size, ImmuneIrregularGroup, Links.Interface("IrImmGr"));
                case 67: return Common.GetRectangle(size, ImmuneCyberGroup, Links.Interface("CyImmGr"));
            }
            return null;
        }
        /*
        public static UIElement GetPict(Equipmentclass equip)
        {
            switch (equip.Type)
            {
                case 0: return GetPict(Picts.Accuracy, Target.Energy, false, Links.Interface("EnAcc"));
                case 1: return GetPict(Picts.Accuracy, Target.Physic, false, Links.Interface("PhAcc"));
                case 2: return GetPict(Picts.Accuracy, Target.Irregular, false, Links.Interface("IrAcc"));
                case 3: return GetPict(Picts.Accuracy, Target.Cyber, false, Links.Interface("CyAcc"));
                case 4: return GetPict(Picts.Accuracy, Target.Total, false, Links.Interface("AlAcc"));
                case 5: return GetPict(Picts.Health, Target.None, false, Links.Interface("Hull"));
                case 6: return GetPict(Picts.Restore, Target.Health, false, Links.Interface("HullRestore"));
                case 7: return GetPict(Picts.Shield, Target.None, false, Links.Interface("Shield"));
                case 8: return GetPict(Picts.Restore, Target.Shield, false, Links.Interface("ShieldRestore"));
                case 9: return GetPict(Picts.Energy, Target.None, false, Links.Interface("Energy"));
                case 10: return GetPict(Picts.Restore, Target.Generator, false, Links.Interface("EnergyRestore"));
                case 11: return GetPict(Picts.Evasion, Target.Energy, false, Links.Interface("EnEva"));
                case 12: return GetPict(Picts.Evasion, Target.Physic, false, Links.Interface("PhEva"));
                case 13: return GetPict(Picts.Evasion, Target.Irregular, false, Links.Interface("IrEva"));
                case 14: return GetPict(Picts.Evasion, Target.Cyber, false, Links.Interface("CyEva"));
                case 15: return GetPict(Picts.Evasion, Target.Total, false, Links.Interface("AlEva"));
                case 16: return GetPict(Picts.Ignore, Target.Energy, false, Links.Interface("EnIgn"));
                case 17: return GetPict(Picts.Ignore, Target.Physic, false, Links.Interface("PhIgn"));
                case 18: return GetPict(Picts.Ignore, Target.Irregular, false, Links.Interface("IrIgn"));
                case 19: return GetPict(Picts.Ignore, Target.Cyber, false, Links.Interface("CyIgn"));
                case 20: return GetPict(Picts.Ignore, Target.Total, false, Links.Interface("AlIgn"));
                case 21: return GetPict(Picts.Damage, Target.Energy, false, Links.Interface("EnDam"));
                case 22: return GetPict(Picts.Damage, Target.Physic, false, Links.Interface("PhDam"));
                case 23: return GetPict(Picts.Damage, Target.Irregular, false, Links.Interface("IrDam"));
                case 24: return GetPict(Picts.Damage, Target.Cyber, false, Links.Interface("CyDam"));
                case 25: return GetPict(Picts.Damage, Target.Total, false, Links.Interface("AlDam"));
                case 30: return GetPict(Picts.Health, Target.None, true, Links.Interface("HullGroup"));
                case 31: return GetPict(Picts.Restore, Target.Health, true, Links.Interface("HullRestoreGroup"));
                case 32: return GetPict(Picts.Shield, Target.None, true, Links.Interface("ShieldGroup"));
                case 33: return GetPict(Picts.Restore, Target.Shield, true, Links.Interface("ShieldRestoreGroup"));
                case 34: return GetPict(Picts.Energy, Target.None, true, Links.Interface("EnergyGroup"));
                case 35: return GetPict(Picts.Restore, Target.Generator, true, Links.Interface("EnergyRestoreGroup"));
                case 36: return GetPict(Picts.Accuracy, Target.Energy, true, Links.Interface("EnAccGr"));
                case 37: return GetPict(Picts.Accuracy, Target.Physic, true, Links.Interface("PhAccGr"));
                case 38: return GetPict(Picts.Accuracy, Target.Irregular, true, Links.Interface("IrAccGr"));
                case 39: return GetPict(Picts.Accuracy, Target.Cyber, true, Links.Interface("CyAccGr"));
                case 40: return GetPict(Picts.Accuracy, Target.Total, true, Links.Interface("AlAccGr"));
                case 41: return GetPict(Picts.Evasion, Target.Energy, true, Links.Interface("EnEvaGr"));
                case 42: return GetPict(Picts.Evasion, Target.Physic, true, Links.Interface("PhEvaGr"));
                case 43: return GetPict(Picts.Evasion, Target.Irregular, true, Links.Interface("IrEvaGr"));
                case 44: return GetPict(Picts.Evasion, Target.Cyber, true, Links.Interface("CyEvaGr"));
                case 45: return GetPict(Picts.Evasion, Target.Total, true, Links.Interface("AlEvaGr"));
                case 46: return GetPict(Picts.Ignore, Target.Energy, true, Links.Interface("EnIgnGr"));
                case 47: return GetPict(Picts.Ignore, Target.Physic, true, Links.Interface("PhIgnGr"));
                case 48: return GetPict(Picts.Ignore, Target.Irregular, true, Links.Interface("IrIgnGr"));
                case 49: return GetPict(Picts.Ignore, Target.Cyber, true, Links.Interface("CyIgnGr"));
                case 50: return GetPict(Picts.Ignore, Target.Total, true, Links.Interface("AlIgnGr"));
                case 51: return GetPict(Picts.Damage, Target.Energy, true, Links.Interface("EnDamGr"));
                case 52: return GetPict(Picts.Damage, Target.Physic, true, Links.Interface("PhDamGr"));
                case 53: return GetPict(Picts.Damage, Target.Irregular, true, Links.Interface("IrDamGr"));
                case 54: return GetPict(Picts.Damage, Target.Cyber, true, Links.Interface("CyDamGr"));
                case 55: return GetPict(Picts.Damage, Target.Total, true, Links.Interface("AlDamGr"));
                case 57: return GetPict(Picts.Cargo, Target.None, false, Links.Interface("Cargo"));
                case 58: return GetPict(Picts.Colony, Target.None, false, Links.Interface("Colony"));
                case 59: return GetPict(Picts.Immune, Target.Energy, false, Links.Interface("EnImm"));
                case 60: return GetPict(Picts.Immune, Target.Physic, false, Links.Interface("PhImm"));
                case 61: return GetPict(Picts.Immune, Target.Irregular, false, Links.Interface("IrImm"));
                case 62: return GetPict(Picts.Immune, Target.Cyber, false, Links.Interface("CyImm"));
                case 63: return GetPict(Picts.Immune, Target.Total, false, Links.Interface("AlImm"));
            }
            return null;
        }*/
        static Canvas GetShieldDamage(Color color)
        {
            Canvas result = new Canvas(); result.Width = 30; result.Height = 30;
            result.HorizontalAlignment = HorizontalAlignment.Center;
            result.VerticalAlignment = VerticalAlignment.Center;
            UIElement Shield = GetShieldPict();
            result.Children.Add(Shield);
            UIElement Damage = GetDamagePict(color);
            result.Children.Add(Damage);
            return result;
        }
        static Canvas GetHealthDamage(Color color)
        {
            Canvas result = new Canvas(); result.Width = 30; result.Height = 30;
            result.HorizontalAlignment = HorizontalAlignment.Center;
            result.VerticalAlignment = VerticalAlignment.Center;
            UIElement Health = GetHealtPict();
            result.Children.Add(Health);
            UIElement Damage = GetDamagePict(color);
            result.Children.Add(Damage);
            return result;
        }
        static Canvas GetGroupCanvas()
        {
            Canvas result = new Canvas(); result.Width = 30; result.Height = 30;
            result.HorizontalAlignment = HorizontalAlignment.Left;
            Path arroy = new Path(); arroy.StrokeLineJoin = PenLineJoin.Round; arroy.StrokeThickness = 1; arroy.Stroke = Brushes.Red;
            GeometryGroup group = new GeometryGroup();
            group.Children.Add(GetPathGeom("M15,0 l3,2 h-1 v2 h-4 v-2 h-1z", new RotateTransform(0, 15, 15)));
            group.Children.Add(GetPathGeom("M15,0 l3,2 h-1 v2 h-4 v-2 h-1z", new RotateTransform(45, 15, 15)));
            group.Children.Add(GetPathGeom("M15,0 l3,2 h-1 v2 h-4 v-2 h-1z", new RotateTransform(90, 15, 15)));
            group.Children.Add(GetPathGeom("M15,0 l3,2 h-1 v2 h-4 v-2 h-1z", new RotateTransform(135, 15, 15)));
            group.Children.Add(GetPathGeom("M15,0 l3,2 h-1 v2 h-4 v-2 h-1z", new RotateTransform(180, 15, 15)));
            group.Children.Add(GetPathGeom("M15,0 l3,2 h-1 v2 h-4 v-2 h-1z", new RotateTransform(-45, 15, 15)));
            group.Children.Add(GetPathGeom("M15,0 l3,2 h-1 v2 h-4 v-2 h-1z", new RotateTransform(-90, 15, 15)));
            group.Children.Add(GetPathGeom("M15,0 l3,2 h-1 v2 h-4 v-2 h-1z", new RotateTransform(-135, 15, 15)));
            arroy.Data = group;
            arroy.Fill = GetRadBrush(new Point(0.7, 0.3), 1, 1, Colors.Yellow, 0.6, Colors.White, 0);
            result.Children.Add(arroy);
            return result;
        }
        static Brush SpeedBrush = GetSpeedBrush();
        static Brush GetSpeedBrush()
        {
            PathGeometry geom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M15,0 l15,15 l-3,3 l-12,-12 l-12,12 l-3,-3z M7,18 h5 v5 h-5z M7,25 h5 v7h-5z M18,18 h5 v5 h-5z M18,25 h5 v7h-5z"));
            GeometryDrawing draw = new GeometryDrawing(Common.GetLinearBrush(Colors.Blue, Colors.SkyBlue, Colors.Blue),
                new Pen(Brushes.Black, 1), geom);
            return new DrawingBrush(draw);
        }
        static UIElement GetSpeedPict()
        {
            Canvas res = new Canvas(); res.Width = 30; res.Height = 30;
            res.HorizontalAlignment = HorizontalAlignment.Center;
            res.VerticalAlignment = VerticalAlignment.Center;
            res.Background = SpeedBrush;
            return res;
        }
        public static Brush CargoBrush = GetCargoBrush();
        static Brush GetCargoBrush()
        {
            DrawingGroup group = new DrawingGroup();
            LinearGradientBrush metalbrush = new LinearGradientBrush();
            metalbrush.GradientStops.Add(new GradientStop(Colors.Black, 1));
            metalbrush.GradientStops.Add(new GradientStop(Colors.LightGray, 0.5));
            metalbrush.GradientStops.Add(new GradientStop(Colors.Black, 0));
            group.Children.Add(new GeometryDrawing(null, new Pen(metalbrush, 4), new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M28,10 l-10,-8 h-16 v26 h16 l10,-8"))));
            group.Children.Add(new GeometryDrawing(GetRadialBrush(Colors.Red, 0.8, 0.3), new Pen(metalbrush, 1),
                new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M4.5,8.5 a3,3 0 0,1 3,-3 h8 a3,3 0 0,1 0,6 h-8 a3,3 0 0,1 -3,-3"))));
            group.Children.Add(new GeometryDrawing(GetRadialBrush(Colors.Blue, 0.8, 0.3), new Pen(metalbrush, 1),
                new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M4.5,15.5 a3,3 0 0,1 3,-3 h8 a3,3 0 0,1 0,6 h-8 a3,3 0 0,1 -3,-3"))));
            group.Children.Add(new GeometryDrawing(GetRadialBrush(Colors.Green, 0.8, 0.3), new Pen(metalbrush, 1),
                new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M4.5,22.5 a3,3 0 0,1 3,-3 h8 a3,3 0 0,1 0,6 h-8 a3,3 0 0,1 -3,-3"))));
            return new DrawingBrush(group);
        }
        static Brush GetRadialBrush(Color color, double x, double y)
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientOrigin = new Point(x, y);
            brush.GradientStops.Add(new GradientStop(color, 1));
            brush.GradientStops.Add(new GradientStop(Colors.White, 0));
            return brush;
        }
        static UIElement GetCargoPict()
        {
            Canvas res = new Canvas(); res.Width = 30; res.Height = 30;
            res.HorizontalAlignment = HorizontalAlignment.Center;
            res.VerticalAlignment = VerticalAlignment.Center;
            res.Background = CargoBrush;
            return res;
        }
        public static Brush ColonyBrush = GetColonyBrush();
        static Brush GetColonyBrush()
        {
            DrawingGroup group = new DrawingGroup();
            LinearGradientBrush metalbrush = new LinearGradientBrush();
            metalbrush.GradientStops.Add(new GradientStop(Colors.Black, 1));
            metalbrush.GradientStops.Add(new GradientStop(Colors.LightGray, 0.5));
            metalbrush.GradientStops.Add(new GradientStop(Colors.Black, 0));
            group.Children.Add(new GeometryDrawing(null, new Pen(metalbrush, 4), new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M28,10 l-10,-8 h-16 v26 h16 l10,-8"))));
            group.Children.Add(new GeometryDrawing(null, new Pen(GetRadialBrush(Colors.Red, 0.8, 0.3), 1),
                new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M8,26 a18,10 0 0,0 6,-10 a18,10 0 0,1 4,-11"))));
            group.Children.Add(new GeometryDrawing(null, new Pen(GetRadialBrush(Colors.Red, 0.8, 0.3), 1),
    new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M4,20 a18,10 0 0,1 10,-4 a18,10 0 0,0 9,-7"))));
            group.Children.Add(new GeometryDrawing(null, new Pen(GetRadialBrush(Color.FromArgb(150,255,0,255), 0.8, 0.3), 1),
                new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M6,18 L10,25 M5.7,18.5 a0.5,0.5 0 1,1 0.0.01 M9.5,25 a0.5,0.5 0 1,1 0.0.01"+
                      "M13,10 L18,14.5 M12.5,10 a0.5,0.5 0 1,1 0.0.01 M17.5,14.5 a0.5,0.5 0 1,1 0.0.01"))));
            group.Children.Add(new GeometryDrawing(null, new Pen(GetRadialBrush(Color.FromArgb(150,0,255,0), 0.8, 0.3), 1),
                new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M9,17 L12.5,23 M8.5,17 a0.5,0.5 0 1,1 0.0.01 M12,23 a0.5,0.5 0 1,1 0.0.01"+
                      "M14.5,7 L21,12.5 M14.5,7.5 a0.5,0.5 0 1,1 0.0.01 M20.5,12.3 a0.5,0.5 0 1,1 0.0.01"))));
            group.Children.Add(new GeometryDrawing(null, new Pen(GetRadialBrush(Color.FromArgb(150,0,0,255), 0.8, 0.3), 1),
                new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M11.5,16 L14,20.5 M11.3,16.5 a0.5,0.5 0 1,1 0.0.01 M13.5,20.5 a0.5,0.5 0 1,1 0.0.01"+
                      "M17,5.5 L23,10.5 M16.5,5.5 a0.5,0.5 0 1,1 0.0.01 M22,10.3 a0.5,0.5 0 1,1 0.0.01"))));
            return new DrawingBrush(group);
        }
        static UIElement GetColonyPict()
        {
            Canvas res = new Canvas(); res.Width = 30; res.Height = 30;
            res.HorizontalAlignment = HorizontalAlignment.Center;
            res.VerticalAlignment = VerticalAlignment.Center;
            res.Background = ColonyBrush;
            return res;
        }
        static UIElement GetImmunePict(Color color)
        {
            Canvas res = new Canvas(); res.Width = 30; res.Height = 30;
            res.HorizontalAlignment = HorizontalAlignment.Center;
            res.VerticalAlignment = VerticalAlignment.Center;
            Path round = new Path();
            res.Children.Add(round);
            round.Data = new EllipseGeometry(new Point(15, 15), 15, 15);
            round.Fill = GetRadBrush(new Point(0.8, 0.2), 1, 1, color, 0.5, Colors.White, 0);
            Path eyes = new Path(); eyes.Stroke = Brushes.Black; eyes.StrokeThickness = 0.2;
            eyes.StrokeLineJoin = PenLineJoin.Round; res.Children.Add(eyes);
            eyes.Fill = Brushes.White; Canvas.SetZIndex(eyes, 1);
            eyes.Data = GetPathGeom("m5,10 c3,8 7,5 10,2 c5,6 8,3 10,-2 a11,3 360 0,1 -20,0");
            Path pupils = new Path(); pupils.Fill = Brushes.Black; res.Children.Add(pupils);
            Canvas.SetZIndex(pupils, 2);
            GeometryGroup pupilsgroup = new GeometryGroup();
            pupilsgroup.Children.Add(GetPathGeom("m9,11.6 a1,1 360 0,0 3,0.2"));
            pupilsgroup.Children.Add(GetPathGeom("m17.6,11.8 a1,1 360 0,0 3,-0.2"));
            pupils.Data = pupilsgroup;
            Path eyebrown = new Path(); eyebrown.StrokeLineJoin = PenLineJoin.Round; eyebrown.StrokeStartLineCap = PenLineCap.Round;
            eyebrown.StrokeEndLineCap = PenLineCap.Round; res.Children.Add(eyebrown);
            eyebrown.Data = GetPathGeom("m4,10.2 a5,1 360 0,1 2,0 a 10,4 360 0,0 9,1.5 a10,4 360 0,0 9,-1.5 a5,1 360 0,1 2,0");
            eyebrown.Stroke = GetRadBrush(new Point(0.5, 0.5), 1, 1, color, 0.6, Colors.White, 0);
            //res.HorizontalAlignment = HorizontalAlignment.Left;
            return res;
        }
        static UIElement GetEvasionPict(Color color)
        {
            Path res = new Path(); res.Stroke = Brushes.Black; res.StrokeThickness = 0.2; res.StrokeLineJoin = PenLineJoin.Round;
            res.HorizontalAlignment = HorizontalAlignment.Center;
            res.VerticalAlignment = VerticalAlignment.Center;
            GeometryGroup group = new GeometryGroup();
            CombinedGeometry circle = new CombinedGeometry();
            circle.GeometryCombineMode = GeometryCombineMode.Exclude;
            circle.Geometry1 = new EllipseGeometry(new Point(15, 15), 13, 13);
            circle.Geometry2 = new EllipseGeometry(new Point(15, 15), 10, 10);
            group.Children.Add(circle);
            group.Children.Add(new RectangleGeometry(new Rect(0, 14, 15, 2), 0, 0, new RotateTransform(45, 15, 15)));
            group.Children.Add(new RectangleGeometry(new Rect(15, 14, 15, 2), 0, 0, new RotateTransform(45, 15, 15)));
            group.Children.Add(new RectangleGeometry(new Rect(14, 0, 2, 15), 0, 0, new RotateTransform(45, 15, 15)));
            group.Children.Add(new RectangleGeometry(new Rect(14, 15, 2, 15), 0, 0, new RotateTransform(45, 15, 15)));

            res.Data = group;
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.GradientStops.Add(new GradientStop(color, 0.7));
            brush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
            brush.GradientStops.Add(new GradientStop(color, 0.3));
            brush.StartPoint = new Point(0.3, 0); brush.EndPoint = new Point(0.7, 1);
            res.Fill = brush;
            //res.Fill=GetRadBrush(new Point(0.7,0.4),1,1,color,0.4,Colors.White,0);
            //res.RenderTransform = new RotateTransform(45, 15, 15);
            return res;

        }
        static UIElement GetAccuracyPict(Color color)
        {
            Path res = new Path(); res.Stroke = Brushes.Black; res.StrokeThickness = 0.2; res.StrokeLineJoin = PenLineJoin.Round;
            res.HorizontalAlignment = HorizontalAlignment.Center;
            res.VerticalAlignment = VerticalAlignment.Center;
            GeometryGroup group = new GeometryGroup();
            CombinedGeometry circle = new CombinedGeometry();
            circle.GeometryCombineMode = GeometryCombineMode.Exclude;
            circle.Geometry1 = new EllipseGeometry(new Point(15, 15), 13, 13);
            circle.Geometry2 = new EllipseGeometry(new Point(15, 15), 10, 10);
            group.Children.Add(circle);
            group.Children.Add(new RectangleGeometry(new Rect(0, 14, 11, 2)));
            group.Children.Add(new RectangleGeometry(new Rect(19, 14, 11, 2)));
            group.Children.Add(new RectangleGeometry(new Rect(14, 0, 2, 11)));
            group.Children.Add(new RectangleGeometry(new Rect(14, 19, 2, 11)));
            res.Data = group;
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.GradientStops.Add(new GradientStop(color, 0.7));
            brush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
            brush.GradientStops.Add(new GradientStop(color, 0.3));
            res.Fill = brush;
            //res.Fill=GetRadBrush(new Point(0.7,0.4),1,1,color,0.4,Colors.White,0);
            return res;
        }
        static UIElement GetRestorePict(Color color)
        {
            Path res = new Path(); res.Stroke = Brushes.Black; res.StrokeThickness = 0.2; res.StrokeLineJoin = PenLineJoin.Round;
            res.HorizontalAlignment = HorizontalAlignment.Center;
            res.VerticalAlignment = VerticalAlignment.Center;
            GeometryGroup group = new GeometryGroup();
            CombinedGeometry circle = new CombinedGeometry();
            circle.GeometryCombineMode = GeometryCombineMode.Exclude;
            circle.Geometry1 = new EllipseGeometry(new Point(15, 15), 15, 15);
            circle.Geometry2 = new EllipseGeometry(new Point(15, 15), 12, 12);
            group.Children.Add(circle);
            group.Children.Add(GetPathGeom("m13,13 a3,5 360 1,1 4,0 a5,3 360 1,1 0,4 a3,5 360 1,1 -4,0 a5,3 360 1,1 0,-4"));
            res.Data = group;
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.GradientStops.Add(new GradientStop(color, 0.7));
            brush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
            brush.GradientStops.Add(new GradientStop(color, 0.3));
            res.Fill = brush;
            //res.Fill=GetRadBrush(new Point(0.6,0.2),1,1,color, 0.4, Colors.White,0);
            return res;
        }
        static UIElement GetDecreasePict()
        {
            Path res = new Path(); res.Stroke = Brushes.Black; res.StrokeThickness = 0.2; res.StrokeLineJoin = PenLineJoin.Round;
            res.HorizontalAlignment = HorizontalAlignment.Center;
            res.VerticalAlignment = VerticalAlignment.Center;
            GeometryGroup group = new GeometryGroup();
            group.Children.Add(new RectangleGeometry(new Rect(5, 0, 10, 4)));
            group.Children.Add(new RectangleGeometry(new Rect(5, 6, 10, 7)));
            group.Children.Add(GetPathGeom("M10,27 l8,-5 a2,1 360 0,0 -3,-2  v-5 h-10 v5 a2,1 360 0,0 -3,2 z"));
            res.Data = group;
            res.Fill = GetRadBrush(new Point(0.6, 0.3), 1, 1, Colors.SkyBlue, 0.3, Colors.White, 0);
            return res;
        }
        static UIElement GetIncreasePict()
        {
            Path res = new Path(); res.Stroke = Brushes.Black; res.StrokeThickness = 0.2; res.StrokeLineJoin = PenLineJoin.Round;
            res.HorizontalAlignment = HorizontalAlignment.Center;
            res.VerticalAlignment = VerticalAlignment.Center;
            GeometryGroup group = new GeometryGroup();
            group.Children.Add(new RectangleGeometry(new Rect(5, 23, 10, 4)));
            group.Children.Add(new RectangleGeometry(new Rect(5, 14, 10, 7)));
            group.Children.Add(GetPathGeom("M10,0 l8,5 a2,1 360 0,1 -3,2  v5 h-10 v-5 a2,1 360 0,1 -3,-2 z"));
            res.Data = group;
            res.Fill = GetRadBrush(new Point(0.6, 0.3), 1, 1, Colors.Red, 0.3, Colors.White, 0);
            return res;
        }
        static UIElement GetDamagePict(Color color)
        {
            Path res = new Path(); res.Stroke = Brushes.Black; res.StrokeThickness = 0.2; res.StrokeLineJoin = PenLineJoin.Round;
            res.HorizontalAlignment = HorizontalAlignment.Center;
            res.VerticalAlignment = VerticalAlignment.Center;
            res.Data = GetPathGeom("m15,0 l2,11 l5,-5 l-4,7 l7,-3 l-6,5 l8,4 l-9,-1 l3,5 l-5,-4 l-2,10 l-2,-11 l-5,3 l4,-5 l-7,-1 l8,-2 l-6,-8 l8,6z");
            RadialGradientBrush brush = GetRadBrush(new Point(0.6, 0.4), 0, 0, color, 0.8, Colors.White, 0.1);
            //brush.GradientStops.Add(new GradientStop(Colors.White,0));
            res.Fill = brush;
            return res;
        }
        static UIElement GetIgnorePict(Color color)
        {
            Path res = new Path(); res.Stroke = Brushes.Black; res.StrokeThickness = 0.2; res.StrokeLineJoin = PenLineJoin.Round;
            res.HorizontalAlignment = HorizontalAlignment.Center;
            res.VerticalAlignment = VerticalAlignment.Center;
            GeometryGroup group = new GeometryGroup();
            group.Children.Add(GetPathGeom("m15,0 a15,15 180 0,1 0,30 a7.5,7.5 180 0,0 0,-15 a7.5,7.5 180 0,1 0,-15"));
            group.Children.Add(new EllipseGeometry(new Point(15, 7), 3, 3));
            group.Children.Add(GetPathGeom("m15,0 a15,15 180 0,0 0,30 a7.5,7.5 180 0,0 0,-15 a7.5,7.5 180 0,1 0,-15" +
            "a15,15 180 0,0 0,30 a7.5 7.5 180 0,0 0,-15 a7.5,7.5 180 0,1 0,-15"));
            group.Children.Add(new EllipseGeometry(new Point(15, 22), 3, 3));
            res.Data = group;
            res.Fill = GetRadBrush(new Point(0.8, 0.2), 0, 0, color, 1, Colors.White, 0);
            return res;
        }
        static UIElement GetShieldPict()
        {
            Path res = new Path(); res.Stroke = Brushes.Black; res.StrokeThickness = 0.2;
            res.HorizontalAlignment = HorizontalAlignment.Center;
            res.VerticalAlignment = VerticalAlignment.Center;
            res.Data = GetPathGeom("m2,5 a14,8 180 0,1 25,0 a20,20 360 0,1 -12.5,25 a20,20 360 0,1 -12.5,-25");
            res.StrokeLineJoin = PenLineJoin.Round;
            res.Fill = GetRadBrush(new Point(0.85, 0.15), 0, 0, Colors.Blue, 1, Colors.SkyBlue, 0.2, Colors.White, 0);
            return res;
        }
        static UIElement GetEnergyPict()
        {
            Path res = new Path(); res.Stroke = Brushes.Black; res.StrokeThickness = 0.2;
            GeometryGroup group = new GeometryGroup();
            res.HorizontalAlignment = HorizontalAlignment.Center;
            res.VerticalAlignment = VerticalAlignment.Center;
            group.Children.Add(GetPathGeom("M15,0 h10 l-7,11 h7 l-16,16 l7,-13 h-5 z"));
            group.Children.Add(GetPathGeom("M11,2 a15,15 360 0,0 -5,24 a1,1 180 1,1 -2.5,0 a15,16 360 0,1 7,-26 a1,1 360 0,1 0,2.2"));
            group.Children.Add(GetPathGeom("M28,2 a15,15 360 0,1 -12,24 a1,1 360 0,0 1,2 a15,16.3 360 0,0 11.7,-28 a1,1 360 0,0 -0.5,2"));
            res.Data = group;
            //res.Fill = GetRadBrush(new Point(0.7, 0.2), 1, 1, Colors.Green, 0.6, Colors.White, 0);

            LinearGradientBrush brush = new LinearGradientBrush();
            brush.GradientStops.Add(new GradientStop(Colors.Green, 1));
            brush.GradientStops.Add(new GradientStop(Colors.Yellow, 0.3));
            brush.GradientStops.Add(new GradientStop(Colors.Green, 0));
            res.Fill = brush;

            return res;
        }

        static UIElement GetHealtPict()
        {
            Path res = new Path();
            res.HorizontalAlignment = HorizontalAlignment.Center;
            res.VerticalAlignment = VerticalAlignment.Center;
            res.Stroke = Brushes.Black;
            res.StrokeThickness = 0.2;
            res.Data = GetPathGeom("M0,8 a5.1,5 180,0,1 15,0 a5.1,5 180 0,1 15,0 a20,20 180 0,1 -15,20 a20,20 180 0,1 -15,-20");
            res.Fill = GetRadBrush(new Point(0.7, 0.3), 1, 1, Colors.Red, 0.5, Colors.White, 0);
            return res;
        }
        static UIElement GetCritPict()
        {
            Path flame = new Path();
            flame.HorizontalAlignment = HorizontalAlignment.Center;
            flame.VerticalAlignment = VerticalAlignment.Center;
            flame.Stroke = Brushes.Black;
            flame.StrokeThickness = 0.2;
            flame.Data = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M2,18 a10,10 0 0,0 25,0 a15,12 0 0,1 0,-14 a12,12 0 0,0 -8,15"+
                  "a15,15 0 0,1 0,-18 a15,15 0 0,0 -8,18 a15,15 0 0,0 -9,-14 a10,10 0 0,1 0,13"));
            RadialGradientBrush flamebrush = new RadialGradientBrush();
            flamebrush.GradientOrigin = new Point(0.5, 0.97);
            flamebrush.RadiusX = 0.6; flamebrush.RadiusY = 0.5;
            flamebrush.GradientStops.Add(new GradientStop(Colors.Red, 1));
            flamebrush.GradientStops.Add(new GradientStop(Colors.Orange, 0.5));
            flamebrush.GradientStops.Add(new GradientStop(Colors.Yellow, 0));
            flame.Fill = flamebrush;
            return flame;
        }
        static PathGeometry GetPathGeom(string text, Transform transfrom)
        {
            PathGeometry geom = GetPathGeom(text);
            geom.Transform = transfrom;
            return geom;
        }
        static PathGeometry GetPathGeom(string text)
        {
            return new PathGeometry((PathFigureCollection)conv.ConvertFrom(text));
        }
        static RadialGradientBrush GetRadBrush(Point origin, double radx, double rady, Color color1, double offset1, Color color2, double offset2, Color color3, double offset3)
        {
            RadialGradientBrush brush = GetRadBrush(origin, radx, rady, color1, offset1, color2, offset2);
            brush.GradientStops.Add(new GradientStop(color3, offset3));
            return brush;
        }
        static RadialGradientBrush GetRadBrush(Point origin, double radx, double rady, Color color1, double offset1, Color color2, double offset2)
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientOrigin = origin;
            if (radx != 0)
                brush.RadiusX = radx;
            if (rady != 0)
                brush.RadiusY = rady;
            brush.GradientStops.Add(new GradientStop(color1, offset1));
            brush.GradientStops.Add(new GradientStop(color2, offset2));
            return brush;
        }
        
        public static DrawingBrush ShipBrush()
        {
            PathGeometry body = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M130,70 a25,25 0 0,1 40,0 a140,230 0 0,1 70,150 a20,20 0 0,1 -20,20" +
                  "a200,200 0 0,0 -140,0 a20,20 0 0,1 -20,-20 a140,230 0 0,1 70,-150"+
                  "M140,62 v-10 h-6 v-32 h32 v32 h-6 v10 a25,25 0 0,0 -20,0"+
                  "M90,123 v-15 h6 v-32 h-32 v32 h6 v65 a140,230 0 0,1 20,-51"+
                  " M210,123 v-15 h-6 v-32 h32 v32 h-6 v65 a140,230 0 0,0 -20.-51 M0,0 h1 M300,260 h-1"));
            RadialGradientBrush bodybrush = new RadialGradientBrush();
            bodybrush.GradientOrigin = new Point(0.6, 0.4);
            bodybrush.GradientStops.Add(new GradientStop(Colors.Gray, 1));
            bodybrush.GradientStops.Add(new GradientStop(Colors.White, 0));
            Pen bodypen = new Pen(Brushes.Black, 5);
            bodypen.LineJoin = PenLineJoin.Round;
            GeometryDrawing bodydraw = new GeometryDrawing(bodybrush, bodypen, body);

            PathGeometry cabin = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M110,110 a50,50 0 0,1 80,0 a100,100 0 0,1 20,40 a150,150 0 0,1 -120,0 a100,100 0 0,1 20,-40"));
            RadialGradientBrush cabinbrush = new RadialGradientBrush();
            cabinbrush.GradientOrigin = new Point(0.8, 0.2);
            cabinbrush.GradientStops.Add(new GradientStop(Colors.Blue, 1));
            cabinbrush.GradientStops.Add(new GradientStop(Colors.White, 0));
            GeometryDrawing cabindraw = new GeometryDrawing(cabinbrush, bodypen, cabin);
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(bodydraw); group.Children.Add(cabindraw);
            return new DrawingBrush(group);
        }
         
        public static DrawingBrush FleetBrush()
        {
            PathGeometry body1 = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M130,270 a25,25 0 0,1 40,0 a140,230 0 0,1 70,150 a20,20 0 0,1 -20,20" +
                  "a200,200 0 0,0 -140,0 a20,20 0 0,1 -20,-20 a140,230 0 0,1 70,-150" +
                  "M140,262 v-10 h-6 v-32 h32 v32 h-6 v10 a25,25 0 0,0 -20,0" +
                  "M90,323 v-15 h6 v-32 h-32 v32 h6 v65 a140,230 0 0,1 20,-51" +
                  "M210,323 v-15 h-6 v-32 h32 v32 h-6 v65 a140,230 0 0,0 -20.-51 "));
            RadialGradientBrush bodybrush = new RadialGradientBrush();
            bodybrush.GradientOrigin = new Point(0.6, 0.4);
            bodybrush.GradientStops.Add(new GradientStop(Colors.Gray, 1));
            bodybrush.GradientStops.Add(new GradientStop(Colors.White, 0));
            Pen bodypen = new Pen(Brushes.Black, 5);
            bodypen.LineJoin = PenLineJoin.Round;
            GeometryDrawing bodydraw1 = new GeometryDrawing(bodybrush, bodypen, body1);

            PathGeometry cabin1 = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M110,310 a50,50 0 0,1 80,0 a100,100 0 0,1 20,40 a150,150 0 0,1 -120,0 a100,100 0 0,1 20,-40"));
            RadialGradientBrush cabinbrush = new RadialGradientBrush();
            cabinbrush.GradientOrigin = new Point(0.8, 0.2);
            cabinbrush.GradientStops.Add(new GradientStop(Colors.Blue, 1));
            cabinbrush.GradientStops.Add(new GradientStop(Colors.White, 0));
            GeometryDrawing cabindraw = new GeometryDrawing(cabinbrush, bodypen, cabin1);
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(bodydraw1); group.Children.Add(cabindraw);

            PathGeometry body2 = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M270,60 v50 h60 v-50 a70,100 0 0,1 0,200 h-60 a70,100 0 0,1 0,-200" +
                "M290,110 v-40 h-6 v-32 h32 v32 h-6 v40z" +
                "M250,134 v-10 h6 v-32 h-32 v32 h6 v24 a70,100 0 0,1 20,-14" +
                "M350,134 v-10 h-6 v-32 h32 v32 h-6 v24 a70,100 0 0,0 -20.-14"));
            PathGeometry cabin2 = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M220,160 a100,100 0 0,1 160,0 v60 a100,100 0 0,1 -160,0z"));
            group.Children.Add(new GeometryDrawing(bodybrush, bodypen, body2));
            group.Children.Add(new GeometryDrawing(cabinbrush, bodypen, cabin2));

            PathGeometry body3 = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M420,280 a30,30 0 0,1 60,0 v40 a35,35 0 0,1 70,0 v80" +
               "a35,35 0 0,1 -70,0 a40,40 0 0,1-60,0 a35,35 0 0,1 -70,0 v-80 a35,35 0 0,1 70,0z" +
               "M440,252 v-10 h-6 v-32 h32 v32 h-6 v10 a30,30 0 0,0 -20,0" +
               " M395,306.5 v-25 h6 v-32 h-32 v32 h6 v25 a35,35 0 0,1 20,0" +
               "M505,306.5 v-25 h-6 v-32 h32 v32 h-6 v25 a35,35 0 0,0 -20,-0"));
            PathGeometry cabin3 = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M360,310 a35,35 0 0,1 50,0 v100 a35,35 0 0,1 -50,0z M490,310 a35,35 0 0,1 50,0 v100 a35,35 0 0,1 -50,0z"));
            group.Children.Add(new GeometryDrawing(bodybrush, bodypen, body3));
            group.Children.Add(new GeometryDrawing(cabinbrush, bodypen, cabin3));


            return new DrawingBrush(group);
        }
        public static DrawingBrush ShipTypeBrush(int guns)
        {
            PathGeometry body = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M110,100 a40,40 0 0,1 80,0 v50 l50,50 v30 l-50,-20"
              + "l20,50 h-120 l20,-50 l-50,20 v-30 l50,-50z"));
            LinearGradientBrush bodybrush = new LinearGradientBrush();
            bodybrush.GradientStops.Add(new GradientStop(Colors.Gray, 1));
            bodybrush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
            bodybrush.GradientStops.Add(new GradientStop(Colors.Gray, 0));
            Pen pen = new Pen(Brushes.Black, 2);
            pen.LineJoin = PenLineJoin.Round;
            GeometryDrawing bodydraw = new GeometryDrawing(bodybrush, pen, body);
            PathGeometry gun;
            switch (guns)
            {
                case 1: gun = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M140,50 v11.2 a40,40 0 0,1 20,0 v-11.2 h6 v-32 h-32 v32z")); break;
                case 2: gun = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M70,160 v30 l20,-20 v-10 h6 v-32 h-32 v32z M230,160 v30 l-20,-20 v-10 h-6 v-32 h32 v32z")); break;
                default: gun = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M140,50 v11.2 a40,40 0 0,1 20,0 v-11.2 h6 v-32 h-32 v32z M70,160 v30 l20,-20 v-10 h6 v-32 h-32 v32z M230,160 v30 l-20,-20 v-10 h-6 v-32 h32 v32z")); break;
            }
            GeometryDrawing gundraw = new GeometryDrawing(Brushes.Red, pen, gun);
            PathGeometry cabin = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M120,110 a30,30 0 0,1 60,0 v10 h-60z")); 
            RadialGradientBrush cabinbrush = new RadialGradientBrush();
            cabinbrush.GradientOrigin = new Point(0.7, 0.4);
            cabinbrush.GradientStops.Add(new GradientStop(Colors.Blue, 1));
            cabinbrush.GradientStops.Add(new GradientStop(Colors.White, 0));
            GeometryDrawing cabindraw = new GeometryDrawing(cabinbrush, null, cabin);
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(bodydraw);
            group.Children.Add(gundraw);
            group.Children.Add(cabindraw);
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetComputerBrush()
        {
            DrawingGroup group = new DrawingGroup();
            PathFigureCollectionConverter conv = new PathFigureCollectionConverter();
            PathGeometry ramka = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M15,15 h98 v98 h-98z M5,10 a5,5 0 0,1 5,-5 h108 a5,5 0 0,1 5,5 v108"
                      + "a 5,5 0 0,1 -5,5 h-108 a5,5 0 0,1 -5,-5z"));
            LinearGradientBrush ramkabrush = new LinearGradientBrush();
            ramkabrush.GradientStops.Add(new GradientStop(Colors.Gray, 1));
            ramkabrush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
            ramkabrush.GradientStops.Add(new GradientStop(Colors.Gray, 0));
            group.Children.Add(new GeometryDrawing(ramkabrush, new Pen(Brushes.Black, 1), ramka));
            PathGeometry ekran = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M15,15 h98 v98 h-98z"));
            group.Children.Add(new GeometryDrawing(Brushes.Black, new Pen(Brushes.Black, 1), ekran));
            PathGeometry body = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M36,50 v5 h6 v-5 a7,10 0 0,1 0,20 h-6 a7,10 0 0,1 0,-20" +
                "M38,55 v-4 h-0.6 v-3.2 h3.2 v3.2 h-0.6 v4z" +
                "M34.2,50.3 v-1 h0.6 v-3.2 h-3.2 v3.2 h0.6 v2.4 a7,10 0 0,1 2,-1.4" +
                "M44,50.3 v-1 h-0.6 v-3.2 h3.2 v3.2 h-0.6 v2.4 a7,10 0 0,0 -2.-1.4"));
            RadialGradientBrush bodybrush = new RadialGradientBrush();
            bodybrush.GradientOrigin = new Point(0.8, 0.3);
            bodybrush.GradientStops.Add(new GradientStop(Colors.Gray, 1));
            bodybrush.GradientStops.Add(new GradientStop(Colors.White, 0));
            group.Children.Add(new GeometryDrawing(bodybrush, new Pen(Brushes.White, 0.5), body));
            RadialGradientBrush cabinbrush = new RadialGradientBrush();
            cabinbrush.GradientOrigin = new Point(0.8, 0.3);
            cabinbrush.GradientStops.Add(new GradientStop(Colors.Blue, 1));
            cabinbrush.GradientStops.Add(new GradientStop(Colors.White, 0));
            PathGeometry cabin = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M31,60 a10,10 0 0,1 16,0 v6 a10,10 0 0,1 -16,0z"));
            group.Children.Add(new GeometryDrawing(cabinbrush, new Pen(Brushes.Black, 0.2), cabin));
            PathGeometry aim = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M20,60 a20,20 0 1,1 0,0.1 M23,60 a17,17 0 1,1 0,0.1 " +
                      "M16,58 h20 v4 h-20z M64,58 h-20 v4 h20z M38,36 v20 h4 v-20z M38,84 v-20 h4 v20z"));
            group.Children.Add(new GeometryDrawing(new SolidColorBrush(Color.FromArgb(144, 255, 0, 0)), new Pen(Brushes.White, 0.4), aim));
            PathGeometry nulllabel = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M0,4 a2,4 0 1,1 0,0.1"));
            PathGeometry onelabel = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M0,4 l2,-3 v8"));
            LinearGradientBrush textbrush = new LinearGradientBrush();
            textbrush.GradientStops.Add(new GradientStop(Colors.Green, 1));
            textbrush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
            textbrush.GradientStops.Add(new GradientStop(Colors.Green, 0));
            Pen textpen = new Pen(textbrush, 1.2);
            Point[] Nulls = new Point[] { new Point(80, 20), new Point(85, 20), new Point(90,20), new Point(95,20), new Point(100,20), new Point(105,20), 
            new Point (80,30), new Point(85,30), new Point(95,30), new Point(100,30), new Point(105,30),
            new Point (80,40), new Point(85,40), new Point(100,40), new Point(105,40),
            new Point(80,50), new Point(105,50),
            new Point (80,60), new Point (105,60),
           new Point (80,70), new Point(85,70), new Point(100,70), new Point(105,70),
            new Point (80,80), new Point(85,80), new Point(90,80), new Point(100,80), new Point(105,80),};
            Point[] Ones = new Point[] {
                new Point(90, 30), 
                new Point(90, 40), new Point(95,40),
            new Point (85,50), new Point(90,50), new Point(95,50), new Point (100,50),
            new Point(85,60), new Point(90,60), new Point(95,60), new Point(100,60),
             new Point(90, 70), new Point(95,70),
            new Point(95, 80), };
            foreach (Point pt in Nulls)
            {
                PathFigure nullfig = new PathFigure();
                nullfig.StartPoint = new Point(pt.X, pt.Y + 4); ;
                nullfig.Segments.Add(new ArcSegment(new Point(pt.X, pt.Y + 4.1), new Size(2, 4), 0, true, SweepDirection.Clockwise, true));
                PathGeometry nullgeom = new PathGeometry(new PathFigure[] { nullfig });
                group.Children.Add(new GeometryDrawing(null, textpen, nullgeom));
            }
            foreach (Point pt in Ones)
            {
                PathFigure onefig = new PathFigure();
                onefig.StartPoint = new Point(pt.X, pt.Y + 4);
                onefig.Segments.Add(new LineSegment(new Point(pt.X + 2, pt.Y + 1), true));
                onefig.Segments.Add(new LineSegment(new Point(pt.X + 2, pt.Y + 8), true));
                PathGeometry onegeom = new PathGeometry(new PathFigure[] { onefig });
                group.Children.Add(new GeometryDrawing(null, textpen, onegeom));
            }
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetEngineBrush()
        {
            DrawingGroup group = new DrawingGroup();
            PathFigureCollectionConverter conv = new PathFigureCollectionConverter();
            PathGeometry flame = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M20,44 a44,84 0 0,0 88,0"));
            RadialGradientBrush flamebrush = new RadialGradientBrush();
            flamebrush.GradientOrigin = new Point(0.5, 0);
            flamebrush.GradientStops.Add(new GradientStop(Color.FromArgb(0, 255, 0, 0), 1));
            flamebrush.GradientStops.Add(new GradientStop(Color.FromArgb(96, 255, 0, 0), 0.7));
            flamebrush.GradientStops.Add(new GradientStop(Color.FromArgb(128, 255, 153, 0), 0.5));
            flamebrush.GradientStops.Add(new GradientStop(Colors.White, 0));
            group.Children.Add(new GeometryDrawing(flamebrush, null, flame));
            PathGeometry body = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M30,0 v34 a34,10 0 0,0 68,0 v-34z M30,34 l-15,30 a49,12 0 0,0 98,0 l-15,-30 a34,10 0 0,1 -68,0" +
                " M41,41 l-10,32 M52,43 l-5,32 M64,44 l0,32 M76,43 l5,32 M87,41 l10,32"));
            RadialGradientBrush bodybrush = new RadialGradientBrush();
            bodybrush.GradientOrigin = new Point(0.7, 0.3);
            bodybrush.GradientStops.Add(new GradientStop(Colors.Gray, 1));
            bodybrush.GradientStops.Add(new GradientStop(Colors.White, 0));
            group.Children.Add(new GeometryDrawing(bodybrush, new Pen(Brushes.Black, 1), body));
            RadialGradientBrush blackmetal = new RadialGradientBrush();
            blackmetal.GradientOrigin = new Point(0.8, 0.2);
            blackmetal.GradientStops.Add(new GradientStop(Colors.Black, 1));
            blackmetal.GradientStops.Add(new GradientStop(Colors.White, 0));
            Pen blackpen = new Pen(Brushes.Black, 1);
            blackpen.LineJoin = PenLineJoin.Round;
            group.Children.Add(new GeometryDrawing(blackmetal, blackpen, new PathGeometry((PathFigureCollection)conv.ConvertFrom("M30,34 a34,10 0 0,0 11,7.5 l-10,31.5 a 49,12 0 0,1 -16,-10z"))));
            group.Children.Add(new GeometryDrawing(blackmetal, blackpen, new PathGeometry((PathFigureCollection)conv.ConvertFrom("M52,43.5 a34,10 0 0,0 12,0.5 l0,31.5 a 49,12 0 0,1 -16.5,-0.5z"))));
            group.Children.Add(new GeometryDrawing(blackmetal, blackpen, new PathGeometry((PathFigureCollection)conv.ConvertFrom("M76,43.5 a34,10 0 0,0 11,-2 l10,31.5 a 49,12 0 0,1 -16,2z"))));
            group.Children.Add(new GeometryDrawing(blackmetal, blackpen, new PathGeometry((PathFigureCollection)conv.ConvertFrom("M30,34 a34,10 0 0,0 68,0 v-3 a34,10 0 0,1 -68,0z"))));
            group.Children.Add(new GeometryDrawing(blackmetal, blackpen, new PathGeometry((PathFigureCollection)conv.ConvertFrom("M30,24 a34,10 0 0,0 68,0 v-3 a34,10 0 0,1 -68,0z"))));
            group.Children.Add(new GeometryDrawing(blackmetal, blackpen, new PathGeometry((PathFigureCollection)conv.ConvertFrom("M30,14 a34,10 0 0,0 68,0 v-3 a34,10 0 0,1 -68,0z"))));
            group.Children.Add(new GeometryDrawing(blackmetal, blackpen, new PathGeometry((PathFigureCollection)conv.ConvertFrom("M30,4 a34,10 0 0,0 68,0 v-3 a34,10 0 0,1 -68,0z"))));

            return new DrawingBrush(group);
        }
        public static DrawingBrush GetShieldBrush()
        {
            DrawingGroup group = new DrawingGroup();
            PathFigureCollectionConverter conv = new PathFigureCollectionConverter();
            RadialGradientBrush shieldbrush = new RadialGradientBrush();
            shieldbrush.GradientOrigin = new Point(0.75, 0.25);
            shieldbrush.GradientStops.Add(new GradientStop(Colors.SkyBlue, 1));
            shieldbrush.GradientStops.Add(new GradientStop(Colors.White, 0.3));
            group.Children.Add(new GeometryDrawing(shieldbrush, null, new PathGeometry((PathFigureCollection)conv.ConvertFrom("M64,10 l20,54 l-20,54 l-20,-54z"))));
            group.Children.Add(new GeometryDrawing(shieldbrush, null, new PathGeometry((PathFigureCollection)conv.ConvertFrom("M64,0 l0,10 l-20,54 l-44,0 a64,64 0 0,1 64,-64"))));
            group.Children.Add(new GeometryDrawing(shieldbrush, null, new PathGeometry((PathFigureCollection)conv.ConvertFrom("M64,0 l0,10 l20,54 l44,0 a64,64 0 0,0 -64,-64"))));
            group.Children.Add(new GeometryDrawing(shieldbrush, null, new PathGeometry((PathFigureCollection)conv.ConvertFrom("M64,128 l0,-10 l20,-54 l44,0 a64,64 0 0,1 -64,64"))));
            group.Children.Add(new GeometryDrawing(shieldbrush, null, new PathGeometry((PathFigureCollection)conv.ConvertFrom("M64,128 l0,-10 l-20,-54 l-44,0 a64,64 0 0,0 64,64"))));
            RadialGradientBrush crystalbrush = new RadialGradientBrush();
            crystalbrush.GradientOrigin = new Point(0.7, 0.3);
            crystalbrush.GradientStops.Add(new GradientStop(Colors.Blue, 1));
            crystalbrush.GradientStops.Add(new GradientStop(Colors.SkyBlue, 0.4));
            crystalbrush.GradientStops.Add(new GradientStop(Colors.White, 0.1));
            group.Children.Add(new GeometryDrawing(crystalbrush, new Pen(Brushes.Black, 1), new PathGeometry((PathFigureCollection)conv.ConvertFrom("M64,30 l15,30 l-15,30 l-15,-30z "))));
            RadialGradientBrush metalbrush = new RadialGradientBrush();
            metalbrush.GradientOrigin = new Point(0.7, 0.6);
            metalbrush.GradientStops.Add(new GradientStop(Colors.Gray, 1));
            metalbrush.GradientStops.Add(new GradientStop(Colors.White, 0));
            PathGeometry metal = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M40,128 v-23 a35,15 5 0,1 -35,-20 a59,15 0 0,0 118,0" +
                      "a35,15 -5 0,1 -35,20 v23z M60,100 v-12 l-5,-14 l9,7 l9,-7 l-5,14 v12z "));
            Pen metalpen = new Pen(Brushes.Black, 1);
            metalpen.LineJoin = PenLineJoin.Round;
            group.Children.Add(new GeometryDrawing(metalbrush, metalpen, metal));
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetGeneratorBrush()
        {
            DrawingGroup group = new DrawingGroup();
            PathFigureCollectionConverter conv = new PathFigureCollectionConverter();
            RadialGradientBrush flashbrush = new RadialGradientBrush();
            flashbrush.GradientStops.Add(new GradientStop(Colors.Red, 1));
            flashbrush.GradientStops.Add(new GradientStop(Colors.Yellow, 0.7));
            flashbrush.GradientStops.Add(new GradientStop(Colors.White, 0.8));
            group.Children.Add(new GeometryDrawing(flashbrush, null, new PathGeometry((PathFigureCollection)conv.ConvertFrom("M5,64 a59,59 0 1,1 0 0.1"))));
            LinearGradientBrush beambrush = new LinearGradientBrush();
            for (double i = 0; i <= 1.01; i += 0.2)
                beambrush.GradientStops.Add(new GradientStop(Colors.Red, i));
            for (double i = 0.1; i <= 1.01; i += 2)
                beambrush.GradientStops.Add(new GradientStop(Colors.White, i));
            Pen beampen = new Pen(beambrush, 4);
            beampen.StartLineCap = PenLineCap.Round;
            beampen.EndLineCap = PenLineCap.Round;

            PathGeometry beams = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M49,64 l-10,-5 l-20,10 l-13,-7 M64,50 l5,-10 l-10,-20 l7,-13" +
                      "M79,64 l10,5 l20,-10 l13,7 M64,79 l-5,10 l10,20 l-7,13" +
                          "M53,53 l-15,-5 l5,-20 l-20,-5 M74,53 l5,-15 l20,5 l5,-20" +
                  "M74,74 l15,5 l-5,20 l20,5 M53,74 l-5,15 l-20,-5 l-5,20"));
            group.Children.Add(new GeometryDrawing(null, beampen, beams));

            RadialGradientBrush centerbrush = new RadialGradientBrush();
            centerbrush.GradientOrigin = new Point(0.8, 0.3);
            centerbrush.GradientStops.Add(new GradientStop(Colors.Gold, 1));
            centerbrush.GradientStops.Add(new GradientStop(Colors.LightGreen, 0.2));
            centerbrush.GradientStops.Add(new GradientStop(Colors.White, 0));
            group.Children.Add(new GeometryDrawing(centerbrush, new Pen(Brushes.Black, 0.2), new PathGeometry((PathFigureCollection)conv.ConvertFrom("M40,64 a24,24 0 1,1 0,0.1"))));

            LinearGradientBrush bodybrush = new LinearGradientBrush();
            bodybrush.GradientStops.Add(new GradientStop(Colors.Black, 1));
            bodybrush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
            bodybrush.GradientStops.Add(new GradientStop(Colors.Black, 0));

            Pen bodypen = new Pen(Brushes.Black, 1);
            bodypen.LineJoin = PenLineJoin.Round;

            PathGeometry body = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M0,64 a64,64 0 1,1 0,0.1 M5,64 a59,59 0 1,1 0,0.1" +
                      "M60,123 v-35 a24,24 0 0,1 -16,-10 a30,30 0 0,0 40,0 a24,24 0 0,1 -16,10 v35z" +
                      "M60,5 v35 a24,24 0 0,0 -16,10 a30,30 0 0,1 40,0 a24,24 0 0,0 -16,-10 v-35z"));
            group.Children.Add(new GeometryDrawing(bodybrush, bodypen, body));

            return new DrawingBrush(group);
        }
        public static DrawingBrush GetEnergyWeaponBrush(byte opacity)
        {
            DrawingGroup group = new DrawingGroup();
            LinearGradientBrush backbrush = new LinearGradientBrush();
            backbrush.GradientStops.Add(new GradientStop(Color.FromArgb(opacity, 0, 0, 255), 1));
            backbrush.GradientStops.Add(new GradientStop(Color.FromArgb(opacity, 255, 255, 255), 0.5));
            backbrush.GradientStops.Add(new GradientStop(Color.FromArgb(opacity, 0, 0, 255), 0));
            group.Children.Add(new GeometryDrawing(backbrush,null,new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,64 a64,64 0 1,1 0 0.1"))));
            Pen pen = new Pen(new SolidColorBrush(Color.FromArgb(opacity, 0, 0, 0)), 5);
            pen.StartLineCap = PenLineCap.Round;
            pen.EndLineCap = PenLineCap.Round;
            group.Children.Add(new GeometryDrawing(null, pen,
                new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M64,29 a20,23 0 0,1 0,40 a15,23 0 0,0 0,40"))));
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetPhysicWeaponBrush(byte opacity)
        {
            DrawingGroup group = new DrawingGroup();
            LinearGradientBrush backbrush = new LinearGradientBrush();
            backbrush.GradientStops.Add(new GradientStop(Color.FromArgb(opacity, 255, 0, 0), 1));
            backbrush.GradientStops.Add(new GradientStop(Color.FromArgb(opacity, 255, 255, 255), 0.5));
            backbrush.GradientStops.Add(new GradientStop(Color.FromArgb(opacity, 255, 0, 0), 0));
            group.Children.Add(new GeometryDrawing(backbrush, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,64 a64,64 0 1,1 0 0.1"))));
            Pen pen = new Pen(new SolidColorBrush(Color.FromArgb(opacity, 0, 0, 0)), 5);
            pen.StartLineCap = PenLineCap.Round;
            pen.EndLineCap = PenLineCap.Round;
            group.Children.Add(new GeometryDrawing(pen.Brush, pen,
                new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M64,39 l10,20 v40 h-20v-40z"))));
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetIrregularWeaponBrush(byte opacity)
        {
            DrawingGroup group = new DrawingGroup();
            LinearGradientBrush backbrush = new LinearGradientBrush();
            backbrush.GradientStops.Add(new GradientStop(Color.FromArgb(opacity, 255, 0, 255), 1));
            backbrush.GradientStops.Add(new GradientStop(Color.FromArgb(opacity, 255, 255, 255), 0.5));
            backbrush.GradientStops.Add(new GradientStop(Color.FromArgb(opacity, 255, 0, 255), 0));
            group.Children.Add(new GeometryDrawing(backbrush, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,64 a64,64 0 1,1 0 0.1"))));
            Pen pen = new Pen(new SolidColorBrush(Color.FromArgb(opacity, 0, 0, 0)), 5);
            pen.StartLineCap = PenLineCap.Round;
            pen.EndLineCap = PenLineCap.Round;
            group.Children.Add(new GeometryDrawing(null, pen,
                new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M60,64 a8,8 0 0,1 16,0 a16,16 0 0,1 -32,0 a24,24 0 0,1 48,0"+
                      "a32,32 0 0,1 -64,0 a40,40 0 0,1 80,0 a40,40 0 0,1 -80,0"))));
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetCyberWeaponBrush(byte opacity)
        {
            DrawingGroup group = new DrawingGroup();
            LinearGradientBrush backbrush = new LinearGradientBrush();
            backbrush.GradientStops.Add(new GradientStop(Color.FromArgb(opacity, 0, 255, 0), 1));
            backbrush.GradientStops.Add(new GradientStop(Color.FromArgb(opacity, 255, 255, 255), 0.5));
            backbrush.GradientStops.Add(new GradientStop(Color.FromArgb(opacity, 0, 255, 0), 0));
            group.Children.Add(new GeometryDrawing(backbrush, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,64 a64,64 0 1,1 0 0.1"))));
            Pen pen = new Pen(new SolidColorBrush(Color.FromArgb(opacity, 0, 0, 0)), 5);
            pen.StartLineCap = PenLineCap.Round;
            pen.EndLineCap = PenLineCap.Round;
            group.Children.Add(new GeometryDrawing(null, pen,
                new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M20,40 h10 M40,40 v10 M50,40 l10,10 M70,40 h10 M90,40 v10 M100,40 l10,10"+
                      "M30,60 l-10,10 M40,60 h10 M60,60 l10,10 M80,60 h10 M100,60 v10 M110,60 v10"+
                      "M20,80 v10 M30,80 l10,10 M60,80 l-10,10 M 70,80 v10 M80,80 l10,10 M100,80 h10"))));
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetAllWeaponBrush(byte opacity)
        {
            DrawingGroup group = new DrawingGroup();
            LinearGradientBrush backbrush = new LinearGradientBrush();
            backbrush.GradientStops.Add(new GradientStop(Color.FromArgb(opacity, 255, 255, 0), 1));
            backbrush.GradientStops.Add(new GradientStop(Color.FromArgb(opacity, 255, 255, 255), 0.5));
            backbrush.GradientStops.Add(new GradientStop(Color.FromArgb(opacity, 255, 255, 0), 0));
            group.Children.Add(new GeometryDrawing(backbrush, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,64 a64,64 0 1,1 0 0.1"))));
            Pen pen = new Pen(new SolidColorBrush(Color.FromArgb(opacity, 0, 0, 0)), 5);
            pen.StartLineCap = PenLineCap.Round;
            pen.EndLineCap = PenLineCap.Round;
            group.Children.Add(new GeometryDrawing(null, pen,
                new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M24,64 a40,40 0 1,1 0.0.1"))));
            return new DrawingBrush(group);
        }
        public static SortedList<WeaponGroup, DrawingBrush> GetWeaponGroupBrushes(byte opacity)
        {
            SortedList<WeaponGroup, DrawingBrush> list = new SortedList<WeaponGroup, DrawingBrush>();
            list.Add(WeaponGroup.Energy, GetEnergyWeaponBrush(opacity));
            list.Add(WeaponGroup.Physic, GetPhysicWeaponBrush(opacity));
            list.Add(WeaponGroup.Irregular, GetIrregularWeaponBrush(opacity));
            list.Add(WeaponGroup.Cyber, GetCyberWeaponBrush(opacity));
            list.Add(WeaponGroup.Any, GetAllWeaponBrush(opacity));
            return list;
        }
        public static DrawingBrush GetEquipmentBrush()
        {
            DrawingGroup group = new DrawingGroup();
            PathFigureCollectionConverter conv = new PathFigureCollectionConverter();
            RadialGradientBrush radstreel = new RadialGradientBrush();
            radstreel.GradientOrigin = new Point(0.8, 0.3);
            radstreel.GradientStops.Add(new GradientStop(Colors.Gray, 1));
            radstreel.GradientStops.Add(new GradientStop(Colors.White, 0));
            Pen maxpen = new Pen(Brushes.Black, 1);
            maxpen.LineJoin = PenLineJoin.Round;
            Pen pen = new Pen(Brushes.Black, 0.5);
            pen.LineJoin = PenLineJoin.Round;
            group.Children.Add(new GeometryDrawing(radstreel, maxpen, new PathGeometry((PathFigureCollection)conv.ConvertFrom("M0,65 h50 v40 h-50z"))));
            group.Children.Add(new GeometryDrawing(radstreel, maxpen, new PathGeometry((PathFigureCollection)conv.ConvertFrom("M50,65 h50 v40 h-50z"))));
            group.Children.Add(new GeometryDrawing(radstreel, maxpen, new PathGeometry((PathFigureCollection)conv.ConvertFrom("M0,25 h50 v40 h-50z"))));
            group.Children.Add(new GeometryDrawing(radstreel, maxpen, new PathGeometry((PathFigureCollection)conv.ConvertFrom("M50,25 h50 v40 h-50z"))));
            group.Children.Add(new GeometryDrawing(radstreel, maxpen, new PathGeometry((PathFigureCollection)conv.ConvertFrom("M0,25 l20,-10 h50 l-20,10z"))));
            group.Children.Add(new GeometryDrawing(radstreel, maxpen, new PathGeometry((PathFigureCollection)conv.ConvertFrom("M50,25 l20,-10 h50 l-20,10z"))));
            group.Children.Add(new GeometryDrawing(radstreel, maxpen, new PathGeometry((PathFigureCollection)conv.ConvertFrom("M100,25 l20,-10 v40 l-20,10z"))));
            group.Children.Add(new GeometryDrawing(radstreel, maxpen, new PathGeometry((PathFigureCollection)conv.ConvertFrom("M100,65 l20,-10 v40 l-20,10z"))));
            RadialGradientBrush healthbrush = new RadialGradientBrush();
            healthbrush.GradientOrigin = new Point(0.8, 0.3);
            healthbrush.GradientStops.Add(new GradientStop(Colors.Red, 1));
            healthbrush.GradientStops.Add(new GradientStop(Colors.White, 0));
            group.Children.Add(new GeometryDrawing(healthbrush, pen, new PathGeometry((PathFigureCollection)conv.ConvertFrom(
                "M5,40 a10,10 0 0,1 20,0 a10,10 0 0,1 20,0 a20,23 30 0,1 -20,20 a20,23 -30 0,1 -20,-20"))));
            RadialGradientBrush shieldbrush = new RadialGradientBrush();
            shieldbrush.GradientOrigin = new Point(0.9, 0.2);
            shieldbrush.GradientStops.Add(new GradientStop(Colors.Blue, 1));
            shieldbrush.GradientStops.Add(new GradientStop(Colors.White, 0));
            group.Children.Add(new GeometryDrawing(shieldbrush, pen, new PathGeometry((PathFigureCollection)conv.ConvertFrom(
               "M55,35 a25,25 0 0,1 35,0 a25,25 0 0,1 -17.5,25 a25,25 0 0,1 -17.5,-25"))));
            LinearGradientBrush powerbrush = new LinearGradientBrush();
            powerbrush.GradientStops.Add(new GradientStop(Colors.Green, 1));
            powerbrush.GradientStops.Add(new GradientStop(Colors.Yellow, 0.5));
            powerbrush.GradientStops.Add(new GradientStop(Colors.Green, 0));
            group.Children.Add(new GeometryDrawing(powerbrush, pen, new PathGeometry((PathFigureCollection)conv.ConvertFrom(
               "M71,72 h10 l-7,11 h7 l-16,16 l7,-13 h-5 z" +
                  "M68,72 a15,15 360 0,0 -5,24 a1,1 180 1,1 -2.5,0 a15,16 360 0,1 7,-26 a1,1 360 0,1 0,2.2" +
                  "M85,72 a15,15 360 0,1 -12,24 a1,1 360 0,0 1,2 a15,16.3 360 0,0 11.7,-28 a1,1 360 0,0 -0.5,2"))));
            RadialGradientBrush damagebrush = new RadialGradientBrush();
            damagebrush.GradientStops.Add(new GradientStop(Colors.Yellow, 0));
            damagebrush.GradientStops.Add(new GradientStop(Colors.Orange, 0.5));
            damagebrush.GradientStops.Add(new GradientStop(Colors.Red, 1));
            group.Children.Add(new GeometryDrawing(damagebrush, pen, new PathGeometry((PathFigureCollection)conv.ConvertFrom(
               "M25,70 l2,11 l5,-5 l-4,7 l7,-3 l-6,5 l8,4 l-9,-1 l3,5 l-5,-4 l-2,10 l-2,-11 l-5,3 l4,-5 l-7,-1 l8,-2 l-6,-8 l8,6z"))));
            group.Children.Add(new GeometryDrawing(null, pen, new PathGeometry((PathFigureCollection)conv.ConvertFrom(
               "M0,0 h0.1 M127,127 v-0.1"))));
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetSmallItemBrush()
        {
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(new GeometryDrawing(Brushes.Green, null,
                new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,20 a20,20 0 0,1 20,-20 h88 a20,20 0 0,1 20,20 v88 a20,20 0 0,1 -20,20 h-88 a20,20 0 0,1 -20,-20z"))));
            group.Children.Add(new GeometryDrawing(null, new Pen(Brushes.White, 20),
                new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M40,100.5 a20,20 0 0,0 20,-40 a20,20 0 0,1 20,-40"))));
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetMediumItemBrush()
        {
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(new GeometryDrawing(Brushes.Blue, null,
                new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,20 a20,20 0 0,1 20,-20 h88 a20,20 0 0,1 20,20 v88 a20,20 0 0,1 -20,20 h-88 a20,20 0 0,1 -20,-20z"))));
            group.Children.Add(new GeometryDrawing(null, new Pen(Brushes.White, 20),
                new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M20,113 v-65 a20,20 0 0,1 40,0 v65 M60,48 a20,20 0 0,1 40,0 v65"))));
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetLargeItemBrush()
        {
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(new GeometryDrawing(Brushes.Red, null,
                new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,20 a20,20 0 0,1 20,-20 h88 a20,20 0 0,1 20,20 v88 a20,20 0 0,1 -20,20 h-88 a20,20 0 0,1 -20,-20z"))));
            group.Children.Add(new GeometryDrawing(null, new Pen(Brushes.White, 20),
                new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M45,5 v80 a20,20 0 0,0 20,20 h30"))));
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetAddItemBrush()
        {
            return new DrawingBrush(new GeometryDrawing(null, new Pen(Brushes.Red, 20),
                new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                    "M64,0 v30 M0,64 h30 M64,128 v-30 M128,64 h-30 M40,64 h48 M64,40 v48 M10,40 v-30 h30 M88,10 h30 v30 M118,88 v30 h-30 M10,88 v30 h30"))));
        }
        public static DrawingBrush GetAntiResBrush()
        {
            DrawingGroup group = new DrawingGroup();
            RadialGradientBrush brush1 = Common.GetRadialBrush(Colors.Red, 0.8, 0.1);
            RadialGradientBrush brush2 = Common.GetRadialBrush(Colors.Red, 0.4, 0.1);
            RadialGradientBrush brush3 = Common.GetRadialBrush(Colors.Red, 0.6, 0.7);

            group.Children.Add(new GeometryDrawing(brush1, null,
                new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M44,46 a33,47 10 1,1 49,48 a20,20 0 0,1 -21,-10 a7,15 22 1,0 -2,-40 a35,30 0 0,0 -26,4"))));
            group.Children.Add(new GeometryDrawing(brush2, null,
                new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M84.5,43 a30,30 0 0,1 -2,23 a11,5 -10 1,0 -33,23 a40,40 0 0,0 16,18 a45,33 -20 1,1 20,-63"))));
            group.Children.Add(new GeometryDrawing(brush3, null,
                new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M63.2,66 a22,13 48 1,0 37,17 a50,50 0 0,0 8,-25 a43,33 35 1,1 -65,15 a30,30 0 0,1 20,-7"))));
            group.Children.Add(new GeometryDrawing(null, new Pen(Brushes.Black,0.01),
                new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,0 h0.01 M128,128 h-0.01"))));
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetRepairPictBrush()
        {
            DrawingGroup group = new DrawingGroup();
            LinearGradientBrush arm1 = Common.GetLinearBrush(Colors.Red, Color.FromArgb(255,255,150,150), Colors.Red);
            LinearGradientBrush arm2 = Common.GetLinearBrush(Colors.Red, Color.FromArgb(255,255,150,150), Colors.Red, new Point(1, 0), new Point(0, 1));
            LinearGradientBrush molotok = Common.GetLinearBrush(Colors.Black, Colors.LightGray, Colors.Black);
            LinearGradientBrush otvertka = Common.GetLinearBrush(Colors.Black, Colors.LightGray, Colors.Black, new Point(1, 0), new Point(0, 1));
            Pen pen = new Pen(Brushes.Black, 1);
            pen.LineJoin = PenLineJoin.Round;

            group.Children.Add(new GeometryDrawing(molotok, pen,
                new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M90,80 l-10,-5 l-10,-10 l5,-10 l-5,-5 a10,10 0 0,1 14,-3 l16,-14 l-23,-23 l-16,14"+ 
              "a10,10 0 0,1 -3,14 a40,40 0 0,0 -35,25 a50,55 0 0,0 -6,60 a2,2 0 0,0 4,0 a40,35 0 0,1 20,-40 l17,0 l7,7 l5,10z"))));
            group.Children.Add(new GeometryDrawing(arm1, pen,
                new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M150,180 a15,20 45 0,0 20,-20 l-80,-80 l-20,20 l80,80 M85,85 l82,82 M80,90 l83,83 M75,95 l82,82"))));
            group.Children.Add(new GeometryDrawing(otvertka, pen,
                new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M110,80 l10,0 l50,-50 l1,1 l5,-5 l8,-5 l5,5 l-5,8 l-5,5 l1,1 l-50,50 l0,10z M170,30 l10,10 M120,80 l10,10"))));
            group.Children.Add(new GeometryDrawing(arm2, pen,
                new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M50,180 a20,15 45 0,1 -20,-20 l80,-80 l20,20 l-80,80 M115,85 l-82,82 M120,90 l-83,83 M125,95 l-82,82"))));
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetSciencePictBrush()
        {
            DrawingGroup group = new DrawingGroup();
            Pen pen = new Pen(Brushes.Black, 1);
            pen.LineJoin = PenLineJoin.Round;
            LinearGradientBrush brush=Common.GetLinearBrush(Colors.SkyBlue,Colors.White,Colors.SkyBlue);
            brush.GradientStops[0].Offset=0.8;
            brush.GradientStops[2].Offset=-0.3;

            group.Children.Add(new GeometryDrawing(brush, pen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M17,19 a2,2 0 0,0 0,4 v24 l-16,41 a3,3 0 0,0 2,4 h36 a3,3 0 0,0 2,-4 l-16,-41 v-24 a2,2 0 0,0 0,-4z M17,23 h8"))));
            group.Children.Add(new GeometryDrawing(Common.GetLinearBrush(Colors.Red, Colors.White, Colors.Red), null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M12.3,59.8 l-10.8,28 a3,3 0 0,0 2,4 h35.2 a3,3 0 0,0 2,-4 l-11,-28z"))));

            group.Children.Add(new GeometryDrawing(brush, pen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
               "M58,17 a2,2 0 0,0 0,4 v65 a5,6 0 0,0 10,0 v-65 a2,2 0 0,0 0,-4z M58,21 h10"))));
            group.Children.Add(new GeometryDrawing(Common.GetLinearBrush(Colors.Green, Colors.White, Colors.Green), null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M58.5,50 v35 a4.5,6.5 0 0,0 9,0 v-35z"))));

            group.Children.Add(new GeometryDrawing(brush, pen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
               "M94,38 a11.5,3 0 0,0 23,0 a11.5,3 0 0,0 -23,0 a11.5,3 0 0,0 23,0 M94,38 a20,20 0 0,1 -5,20 a20,20 0 0,0 10,34 a20,20 0 0,0 13,0 a20,20 0 0,0 10,-34 a20,20 0 0,1 -5,-20"))));
            group.Children.Add(new GeometryDrawing(Common.GetLinearBrush(Colors.Purple, Colors.White, Colors.Purple), null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M89.5,58.5 a21,20 0 0,0 10,33 a20,20 0 0,0 12,0 a22,20 0 0,0 10,-33 a20,10 0 0,1 -32,0"))));

            group.Children.Add(new GeometryDrawing(null, pen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,0 h0.1 M128,128 h-0.1"))));
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetRarePictBrush()
        {
            DrawingGroup group = new DrawingGroup();
            LinearGradientBrush brush1 = Common.GetLinearBrush(Colors.Red, Colors.White, Colors.Red);
            LinearGradientBrush brush2 = Common.GetLinearBrush(Colors.Red, Colors.White, Colors.Red);
            brush2.GradientStops[1].Offset = 0.6;

            group.Children.Add(new GeometryDrawing(brush2, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M27,103 v-77 l15,29z M78,87 l32,-32 l1,44z M111,99 l10,-43 l0,32z M70,17 a32,9 39 1,1 0,0.1 M42,55 l46,-14 l-9,47z"))));

            group.Children.Add(new GeometryDrawing(brush1, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M38,16 l25,-2 l-36,12z M42,55 l30,-30 l-45,1z M42,55 l36,32 l-51,16z"))));

            group.Children.Add(new GeometryDrawing(brush1, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M27,26 l36,-12 l8,0 a32,9 39 0,0 2,11z M42,55 l31,-30 a32,9 39 0,0 16,16z M79,86 l9,-45 a32,9 39 0,0 22,14z M79,87 l32,12 l-84,4z M111,99 l-1,-44 a32,9 39 0,0 11,0z"))));

            group.Children.Add(new GeometryDrawing(null, new Pen(Brushes.Black, 0.1), new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,0 h0.1 M128,128 h-0.1"))));
            return new DrawingBrush(group);

         }
        /*
        public static DrawingBrush GetDefenseLandPictBrush()
        {
            DrawingGroup group = new DrawingGroup();
            LinearGradientBrush backbrush = new LinearGradientBrush();
            backbrush.GradientStops.Add(new GradientStop(Colors.DarkGray, 0.8));
            backbrush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
            backbrush.GradientStops.Add(new GradientStop(Colors.DarkGray, 0.2));
            Pen pen = new Pen(Brushes.Black, 2);
            PathGeometry round = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M50,0 a50,50 0 1,1 -0.1,0"));
            group.Children.Add(new GeometryDrawing(backbrush, pen, round));
            PathGeometry planet = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M10,80 a50,40 0 0,1 80,0"));
            group.Children.Add(new GeometryDrawing(null, pen, planet));
            PathGeometry land = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M10,80 a50,40 0 0,1 40,-16 a15,14 0 0,1 0,25 a10,6 0 0,0 10,9 a50,50 0 0,1 -50,-17"));
            group.Children.Add(new GeometryDrawing(Brushes.Black, pen, land));
            PathGeometry shield = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M30,15 h40 a20,35 20 0,1 -20,45 a20,35 -20 0,1 -20,-45"));
            group.Children.Add(new GeometryDrawing(Brushes.Black, pen, shield));
            return new DrawingBrush(group);
        }
      */
      /*
        public static DrawingBrush GetColonizationPictBrush()
        {
            DrawingGroup group = new DrawingGroup();
            LinearGradientBrush backbrush = new LinearGradientBrush();
            backbrush.GradientStops.Add(new GradientStop(Colors.DarkGray, 0.8));
            backbrush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
            backbrush.GradientStops.Add(new GradientStop(Colors.DarkGray, 0.2));
            Pen pen = new Pen(Brushes.Black, 2);
            PathGeometry round = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M50,0 a50,50 0 1,1 -0.1,0"));
            group.Children.Add(new GeometryDrawing(backbrush, pen, round));
            PathGeometry planet = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M10,80 a50,40 0 0,1 80,0"));
            group.Children.Add(new GeometryDrawing(null, pen, planet));
            PathGeometry land = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M10,80 a50,40 0 0,1 40,-16 a15,14 0 0,1 0,25 a10,6 0 0,0 10,9 a50,50 0 0,1 -50,-17"));
            group.Children.Add(new GeometryDrawing(Brushes.Black, pen, land));
            PathGeometry flag = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M30,68 v-60 l50,20 l-50,20"));
            group.Children.Add(new GeometryDrawing(Brushes.Black, new Pen(Brushes.Black, 4), flag));
            return new DrawingBrush(group);
        }
       */
        public static DrawingBrush GetAttackEnemyBrush()
        {
            DrawingGroup group = new DrawingGroup();
            LinearGradientBrush backbrush = new LinearGradientBrush();
            backbrush.GradientStops.Add(new GradientStop(Colors.DarkGray, 0.8));
            backbrush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
            backbrush.GradientStops.Add(new GradientStop(Colors.DarkGray, 0.2));
            Pen pen = new Pen(Brushes.Black, 2);
            PathGeometry round = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M50,0 a50,50 0 1,1 -0.1,0"));
            group.Children.Add(new GeometryDrawing(backbrush, pen, round));
            PathGeometry mask = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M26,12 l12,16 a16,14 0 0,1 24,0 l12,-16 v50 a20,35 0 0,1 -15,28"+
                  "a14,20 0 0,0 6,-20 h-11 l-4,4 l-4,-4 h-11 a14,20 0 0,0 6,20 a20,35 0 0,1 -15,-28z"+
                  "M34,50 h2 l10,5 v2 l-6,3 h-4 l-4,-4z M66,50 h-2 l-10,5 v2 l6,3 h4 l4,-4z"));
            group.Children.Add(new GeometryDrawing(Brushes.Black, pen, mask));
            return new DrawingBrush(group);
        }
        /*
        public static DrawingBrush GetResourceMissionBrush()
        {
            DrawingGroup group = new DrawingGroup();
            LinearGradientBrush backbrush = new LinearGradientBrush();
            backbrush.GradientStops.Add(new GradientStop(Colors.DarkGray, 0.8));
            backbrush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
            backbrush.GradientStops.Add(new GradientStop(Colors.DarkGray, 0.2));
            Pen pen = new Pen(Brushes.Black, 2);
            PathGeometry round = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M50,0 a50,50 0 1,1 -0.1,0"));
            group.Children.Add(new GeometryDrawing(backbrush, pen, round));
            PathGeometry cup = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M30,20 h40 a30,40 0 0,1 -10,45 v10 h5 v5 h5 v10 h-40 v-10"+
                      "h5 v-5 h5 v-10 a30,40 0 0,1 -10,-45"));
            group.Children.Add(new GeometryDrawing(Brushes.Black, pen, cup));
            PathGeometry hands = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M72,30 h20 a25,25 0 0,1 -28,30 M28,30 h-20 a25,25 0 0,0 28,30"));
            group.Children.Add(new GeometryDrawing(null, new Pen(Brushes.Black,3), hands));

            return new DrawingBrush(group);
        }
        */
        /*
        public static DrawingBrush GetFreeFleetBrush()
        {
            DrawingGroup group = new DrawingGroup();
            LinearGradientBrush backbrush = new LinearGradientBrush();
            backbrush.GradientStops.Add(new GradientStop(Colors.DarkGray, 0.8));
            backbrush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
            backbrush.GradientStops.Add(new GradientStop(Colors.DarkGray, 0.2));
            Pen pen = new Pen(Brushes.Black, 2);
            PathGeometry round = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M50,0 a50,50 0 1,1 -0.1,0"));
            group.Children.Add(new GeometryDrawing(backbrush, pen, round));
            PathGeometry arrows = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M50,10 l10,15 a5,5 0 0,0 -5,5 v15 h15 a5,5 0 0,0 5,-5 l15,10 l-15,10" +
                      "a5,5 0 0,0 -5,-5 h-15 v15 a5,5 0 0,0 5,5 l-10,15 l-10,-15 a 5,5 0 0,0 5,-5 v-15 h-15" +
                      "a5,5 0 0,0 -5,5 l-15,-10 l15,-10 a5,5 0 0,0 5,5 h15 v-15 a5,5 0 0,0 -5,-5z"));
            group.Children.Add(new GeometryDrawing(Brushes.Black, null, arrows));
           
            return new DrawingBrush(group);
        }
        */
        /*
        public static DrawingBrush GetPillageEnemyBrush()
        {
            DrawingGroup group = new DrawingGroup();
            LinearGradientBrush backbrush = new LinearGradientBrush();
            backbrush.GradientStops.Add(new GradientStop(Colors.DarkGray, 0.8));
            backbrush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
            backbrush.GradientStops.Add(new GradientStop(Colors.DarkGray, 0.2));
            Pen pen = new Pen(Brushes.Black, 2);
            PathGeometry round = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M50,0 a50,50 0 1,1 -0.1,0"));
            group.Children.Add(new GeometryDrawing(backbrush, pen, round));
            PathGeometry planet = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M10,80 a50,40 0 0,1 80,0"));
            group.Children.Add(new GeometryDrawing(null, pen, planet));
            PathGeometry land = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M10,80 a50,40 0 0,1 40,-16 a15,14 0 0,1 0,25 a10,6 0 0,0 10,9 a50,50 0 0,1 -50,-17"));
            group.Children.Add(new GeometryDrawing(Brushes.Black, pen, land));
            PathGeometry mask = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M22,20 a52,37 0 0,1 56,0 a19,21 0 0,1 -14,35 l-14,-6 l-14,6 a19,21 0 0,1 -14,-35" +
                      "M28,33 a11,11 0 0,1 14,1 a11,14 0 0,1 -14,-1 M72,33 a11,11 0 0,0 -14,1 a11,14 0 0,0 14,-1"));
            group.Children.Add(new GeometryDrawing(Brushes.Black, null, mask));
            return new DrawingBrush(group);
        }
        */
        /*
        public static DrawingBrush GetFleetConquerBrush()
        {
            DrawingGroup group = new DrawingGroup();
            LinearGradientBrush backbrush = new LinearGradientBrush();
            backbrush.GradientStops.Add(new GradientStop(Colors.DarkGray, 0.8));
            backbrush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
            backbrush.GradientStops.Add(new GradientStop(Colors.DarkGray, 0.2));
            Pen pen = new Pen(Brushes.Black, 2);
            PathGeometry round = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M50,0 a50,50 0 1,1 -0.1,0"));
            group.Children.Add(new GeometryDrawing(backbrush, pen, round));
            PathGeometry planet = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M10,80 a50,40 0 0,1 80,0"));
            group.Children.Add(new GeometryDrawing(null, pen, planet));
            PathGeometry land = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M10,80 a50,40 0 0,1 40,-16 a15,14 0 0,1 0,25 a10,6 0 0,0 10,9 a50,50 0 0,1 -50,-17"));
            group.Children.Add(new GeometryDrawing(Brushes.Black, pen, land));
            PathGeometry fire = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M40,10 a15,15 0 0,1 5,20 a4,4 0 1,0 7,2 a7,9 0 0,1 8,-10 a5,9 0 0,0 0,15" +
                      "a20,15 0 0,1 -10,20 a5,5 0 1,0 -6,-10 a4,4 0 0,1 -4,-4 a10,10 0 0,1 2,-10 a30,20 0 0,0 -1,24 a70,30 0 0,1 -5,-32 a15,15 0 0,0 4,-15"));
            group.Children.Add(new GeometryDrawing(Brushes.Black, null, fire));
            return new DrawingBrush(group);
        }
        */
        /*
        public static DrawingBrush GetLastDefenderBrush()
        {
            DrawingGroup group = new DrawingGroup();
            LinearGradientBrush backbrush = new LinearGradientBrush();
            backbrush.GradientStops.Add(new GradientStop(Colors.DarkGray, 0.8));
            backbrush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
            backbrush.GradientStops.Add(new GradientStop(Colors.DarkGray, 0.2));
            Pen pen = new Pen(Brushes.Black, 2);
            PathGeometry round = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M50,0 a50,50 0 1,1 -0.1,0"));
            group.Children.Add(new GeometryDrawing(backbrush, pen, round));
            PathGeometry planet = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M10,80 a50,40 0 0,1 80,0"));
            group.Children.Add(new GeometryDrawing(null, pen, planet));
            PathGeometry land = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M10,80 a50,40 0 0,1 40,-16 a15,14 0 0,1 0,25 a10,6 0 0,0 10,9 a50,50 0 0,1 -50,-17"));
            group.Children.Add(new GeometryDrawing(Brushes.Black, pen, land));
            PathGeometry crown = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M26,20 l5,10 a4,4 0 1,0 7,0 l4,-15 l4,15 a4,4 0 1,0 7,0" +
                      "l4,-15 l4,15 a4,4 0 1,0 7,0 l5,-10 l-3,30 h-41 l-3,-30 M30,53 h39 a2,2 0 0,1 0,4 h-39 a2,2 0 0,1 0,-4"));
            group.Children.Add(new GeometryDrawing(Brushes.Black, null, crown));
            return new DrawingBrush(group);
        }
        */
        public static DrawingBrush GetGoldCrownBrush()
        {
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.GradientStops.Add(new GradientStop(Colors.Gold, 0.8));
            brush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
            brush.GradientStops.Add(new GradientStop(Colors.Gold, 0.2));
            Pen pen = new Pen();
            pen.Brush = Brushes.Black;
            pen.LineJoin = PenLineJoin.Round;
            return new DrawingBrush(new GeometryDrawing(brush, pen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M5,40 l20,88 h78 l20,-88 a5,5 0 1,0 -6,0 l-35,50 l-16,-80 a5,5 0 1,0 -5,0 l-15,80 l-35,-50 a5,5 0 1,0 -6,0"))));
        }
        /*
        public static DrawingBrush GetLandIconBrush()
        {
            Pen pen = new Pen();
            pen.Brush = Brushes.Black;
            pen.LineJoin = PenLineJoin.Round;
            return new DrawingBrush(new GeometryDrawing(Brushes.LightGreen, pen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M10,0 l-10,40 h30z M90,0 l10,40 h-30z M12,0 h37 l-17,40z M88,0 h-37 l17,40z M50,2 l16,38 h-32z"+
                  "M0,42 l13,58 l13,-58z M100,42 l-13,58 l-13,-58z M28,42 h10 l-5,58 h-18z M72,42 h-10 l5,58 h18z M40,42 h9 v58 h-14z M60,42 h-9 v58 h14z"))));
        }
        */
        public static DrawingBrush GetLeftArrowBrush()
        {
            PathGeometry triang = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M2,15 l26,-12 v24z"));
            PathGeometry back = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,0 h30 v30 h-30z"));
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(new GeometryDrawing(Brushes.Black, null, back));
            group.Children.Add(new GeometryDrawing(Brushes.White, null, triang));
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetRightArrowBrush()
        {
            PathGeometry triang = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M28,15 l-26,-12 v24z"));
            PathGeometry back = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,0 h30 v30 h-30z"));
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(new GeometryDrawing(Brushes.Black, null, back));
            group.Children.Add(new GeometryDrawing(Brushes.White, null, triang));
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetVBrush()
        {
            RadialGradientBrush brush = Common.GetRadialBrush(Colors.Green, 0.8, 0.3);
            return new DrawingBrush(new GeometryDrawing(brush, new Pen(Brushes.Black,1), new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M5,40 l30,60 l60,-90 l-60,50z"))));
        }
        public static DrawingBrush GetCloudBrush()
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientStops.Add(new GradientStop(Color.FromArgb(100, 180, 100, 100), 0.3));
            brush.GradientStops.Add(new GradientStop(Color.FromArgb(100, 200, 200, 200), 1));
            PathGeometry geom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M20,40 a15,15 0 1,1 20,-20 a15,15 0 0,1 40,20 a15,20 0 1,1 -10,30 a15,15 0 1,1 -30,10 a20,20 0 1,1 -20,-40"));
            GeometryDrawing draw = new GeometryDrawing(brush, null, geom);
            return new DrawingBrush(draw);
        }
        public static DrawingBrush GetRiotBrush()
        {
            RadialGradientBrush backward = new RadialGradientBrush();
            backward.GradientOrigin = new Point(0.5, 1);
            backward.GradientStops.Add(new GradientStop(Colors.White, 0));
            backward.GradientStops.Add(new GradientStop(Colors.Yellow, 0.1));
            backward.GradientStops.Add(new GradientStop(Colors.Orange, 0.3));
            backward.GradientStops.Add(new GradientStop(Colors.Red, 1));
            Pen pen = new Pen();
            pen.Brush = Brushes.Black;
            pen.Thickness = 2;
            pen.LineJoin = PenLineJoin.Round;
            return new DrawingBrush(new GeometryDrawing(backward, pen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M38,5 a50,50 0 0,1 35,52 a50,50 0 0,0 0,-35 a50,43 0 0,1 14,57"+
                  "a50,50 0 0,1 -75,0 a30,30 0 0,1 0,-35 a30,30 0 0,0 0,-25 a50,50 0 0,1 27,37 a50,50 0 0,0 -1,-51"))));
      
        }
        public static DrawingBrush GetRiotPrepareBrush()
        {
            LinearGradientBrush forward = new LinearGradientBrush();
            forward.GradientStops.Add(new GradientStop(Colors.Green, 0.2));
            forward.GradientStops.Add(new GradientStop(Colors.White, 0.5));
            forward.GradientStops.Add(new GradientStop(Colors.Green, 0.8));
            RadialGradientBrush backward = new RadialGradientBrush();
            backward.GradientOrigin = new Point(0.5, 1);
            backward.GradientStops.Add(new GradientStop(Colors.White, 0));
            backward.GradientStops.Add(new GradientStop(Colors.Yellow, 0.1));
            backward.GradientStops.Add(new GradientStop(Colors.Orange, 0.3));
            backward.GradientStops.Add(new GradientStop(Colors.Red, 1));
            Pen pen = new Pen();
            pen.Brush = Brushes.Black;
            pen.Thickness = 2;
            pen.LineJoin = PenLineJoin.Round;
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(new GeometryDrawing(backward, pen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M38,5 a50,50 0 0,1 35,52 a50,50 0 0,0 0,-35 a50,43 0 0,1 14,57" +
                  "a50,50 0 0,1 -75,0 a30,30 0 0,1 0,-35 a30,30 0 0,0 0,-25 a50,50 0 0,1 27,37 a50,50 0 0,0 -1,-51"))));
            group.Children.Add(new GeometryDrawing(forward, pen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M10,24 a48,48 0 1,0 80,0 a30,30 0 0,1 -24,45 l-10,-60 l-3,10 l-3,-15 l-3,15 l-3,-10 l-10,60 a30,30 0 0,1 -24,-45"))));
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetAlienBrush()
        {
            DrawingGroup group = new DrawingGroup();
            Pen pen = new Pen(Brushes.Black, 1);
            group.Children.Add(new GeometryDrawing(Brushes.Violet, pen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M41,3 a40,37.5 0 0,1 0,75 a40,37.5 0 0,1 0,-75"))));
            group.Children.Add(new GeometryDrawing(Brushes.Black, pen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M27,23 a20,33 0 0,1 27,0 a25,20 90 0,1 -13.5,35 a25,20 90 0,1 -13.5,-35 M27,35 a14,10 80 0,1 12,11 a14,10 80 0,1 -12,-11 M54,35 a10,14 80 0,1 -12,11 a10,14 80 0,1 12,-11"))));
            group.Children.Add(new GeometryDrawing(Brushes.Black, pen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M41,6 a32,30 0 0,1 0,66 a32,30 0 0,1 0,-66 M37,10 a29,28 0 0,1 34,27 l3,3 l-1,6 l-4,1 a29,28 0 0,1 -46,13 l-4,0 l-4,-4 l0,-4 a29,28 0 0,1 13,-39 l1,-3 l5,-1.5z"))));
            group.Children.Add(new GeometryDrawing(null, pen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,0 h0.1 M82,0 h-0.1 M0,82 h0.1 M82,82 h-0.1"))));
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetPirateBrush()
        {
            DrawingGroup group = new DrawingGroup();
            Pen pen = new Pen(Brushes.Black, 0.5);
            pen.LineJoin = PenLineJoin.Round;
            group.Children.Add(new GeometryDrawing(Brushes.Red, pen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M2,5 a5,5 0 0,1 5,1 l55,48 a5,5 0 0,1 2,5 a5,5 0 0,1 -5,-2 l-55,-48 a5,5 0 0,1 -2,-4" +
                  "M62,5 a5,5 0 0,0 -5,1 l-55,48 a5,5 0 0,0 -2,5 a5,5 0 0,0 5,-2 l55,-48 a5,5 0 0,0 2,-4"))));
            group.Children.Add(new GeometryDrawing(Brushes.DarkGray, pen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M18,31 l5,-5 l3,0 l2,3 l-1,-6 l2,-2 l3,4 l-2,-7 l0,-3 l3,-2 l3,-1 l-4,0 l0,-3 l1,-2" +
                  "a20,20 0 0,1 15,15 a10,20 0 0,1 -4,22 a20,20 0 0,1 -12,18 a20,20 0 0,1 -12,-18 a10,20 0 0,1 -2,-13"))));
            group.Children.Add(new GeometryDrawing(Brushes.Red, pen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M20,29 a20,20 0 0,0 9,6 l-5,2 a10,10 0 0,1 -5,-7z" +
                  "M44,28 a20,20 0 0,1 -10,7 l5,2 a10,10 0 0,0 5,-9"))));
            group.Children.Add(new GeometryDrawing(null, pen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,0 h0.1 M65,0 h-0.1 M0,65 h0.1 M65,65 h-0.1"))));
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetGreenTeamBrush()
        {
            DrawingGroup group = new DrawingGroup();
            Pen pen = new Pen(Brushes.Black, 0.5);
            pen.LineJoin = PenLineJoin.Round;
            group.Children.Add(new GeometryDrawing(new SolidColorBrush(Color.FromRgb(155, 32, 0)), pen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M27,27 a60,37 0 0,0 0,46 a60,30 0 0,1 0,-46 M55,27 a60,37 0 0,1 0,46 a60,30 0 0,0 0,-46"))));
            group.Children.Add(new GeometryDrawing(Brushes.Green, pen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M41,5 a50,55 0 0,0 15,30 a21,22 0 1,1 -30,0 a50,55 0 0,0 15,-30"))));
            group.Children.Add(new GeometryDrawing(Brushes.White, pen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M41,32 a20,27 0 0,0 7,15 a11,13 0 1,1 -14,0 a20,27 0 0,0 7,-15"))));
            group.Children.Add(new GeometryDrawing(Brushes.Blue, pen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M41,47 a10,15 0 0,0 4,9 a6.5,7 0 1,1 -8,0 a10,15 0 0,0 4,-9"))));
            group.Children.Add(new GeometryDrawing(null, pen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,0 h0.1 M80,0 h-0.1 M0,80 h0.1 M80,80 h-0.1"))));
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetTechTeamBrush()
        {
            DrawingGroup group = new DrawingGroup();
            Pen pen = new Pen(Brushes.Black, 0.5);
            pen.LineJoin = PenLineJoin.Round;
            group.Children.Add(new GeometryDrawing(Brushes.Navy, pen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M45,15 a30,30 0 1,0 0.1,0"))));
            group.Children.Add(new GeometryDrawing(null, new Pen(Brushes.Gold, 2), new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M46,75 v-41 a4,4 0 0,0 -4,-4 h-6 a4,4 0 1,0 0,0.1 M42,75 v-32 a4,4 0 0,0 -4,-4 h-23" +
                  "M38,75 v-24 a4,4 0 0,0 -4,-4 h-6 a4,4 0 1, 0 0,0.1 M50,75 v-21 a4,4 0 0,1 4,-4 h6 a4,4 0 1,1 0,0.1 M50,15 v15 a4,4 0 0,0 4,4 h5 a4,4 0 1,1 0,0.1"))));
            group.Children.Add(new GeometryDrawing(Brushes.Gold, pen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M45,5 a40,40 0 1,0 0.1,0 M45,15 a30,30 0 1,0 0.1,0"))));
            group.Children.Add(new GeometryDrawing(null, pen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,0 h0.1 M90,0 h-0.1 M0,90 h0.1 M90,90 h-0.1"))));
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetMercTeamBrush()
        {
            DrawingGroup group = new DrawingGroup();
            Pen pen = new Pen(Brushes.Black, 0.5);
            pen.LineJoin = PenLineJoin.Round;
            group.Children.Add(new GeometryDrawing(Brushes.Gold, pen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M14,7 a25,25 0 0,0 20,21 a3,3 0 0,1 0,5 a9,8 0 1,0 12,0 a3,3 0 0,1 0,-5 a25,25 0 0,0 20,-21 a35,28 0 1,1 -52,0"))));
            group.Children.Add(new GeometryDrawing(new SolidColorBrush(Color.FromRgb(155, 32, 0)), pen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M38,18 a2,3 0 0,1 4,0 h-1 a3,10 0 0,0 0,6 a3,10 0 0,1 0,6 a2,2 0 0,1 0,4 h-2 a2,2 0 0,1 0,-4 a3,10 0 0,1 0,-6 a3,10 0 0,0 0,-6 h-1"))));
            group.Children.Add(new GeometryDrawing(Brushes.Silver, pen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M41,34 v40 l-2,2 v-42"))));
            group.Children.Add(new GeometryDrawing(Brushes.Red, pen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M43,64 l5,-9 l-2,9 l3,9z M50,63 l10,-10 l-6,10 l6,10z M59,62 l8,-12 l-4,11 l6,10z M68,60 l5,-14 l-1,12 l6,8z" +
                       "M37,64 l-5,-9 l2,9 l-3,9z M30,63 l-10,-10 l6,10 l-6,10z M21,62 l-8,-12 l4,11 l-6,10z M12,60 l-5,-14 l1,12 l-6,8z"))));
            group.Children.Add(new GeometryDrawing(null, pen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,0 h0.1 M80,0 h-0.1 M0,80 h0.1 M80,80 h-0.1"))));
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetEmptyBrush()
        {
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(new GeometryDrawing(Brushes.Red, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M50,0 a50,50 0 1,1 -0.1,0 M50,10 a40,40 0 1,1 -0.1,0"))));
            group.Children.Add(new GeometryDrawing(Brushes.Red, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M20,15 l65,65 l-6,6 l-65,-65z"))));
            group.Children.Add(new GeometryDrawing(Brushes.Red, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M80,15 l-65,65 l6,6 l65,-65z"))));
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetShipModelBrush()
        {
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(new GeometryDrawing(Brushes.Black, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M20,30 a20,20 0 0,1 20,-20 h210 a20,20 0 0,1 20,20 v210 a20,20 0 0,1 -20,20 h-210 a20,20 0 0,1 -20,-20z"))));
            Pen dash = new Pen(Brushes.LightGreen, 2);
            dash.DashStyle = DashStyles.Dash;
            dash.LineJoin = PenLineJoin.Round;
            group.Children.Add(new GeometryDrawing(null, dash, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M180,220 v-15 h-10 l10,-15 M150,237 a100,100 0 0,1 50,13 l-20,-30"))));
            group.Children.Add(new GeometryDrawing(null, dash, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M210,108 v25 a20,20 0 0,1 0,40 l-30,17 v-10 l10,-10 v-62 h-6 v-32 h32 v32z"))));
            group.Children.Add(new GeometryDrawing(null, dash, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M150,130 l30,20 v40 h-30"))));
            group.Children.Add(new GeometryDrawing(null, dash, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M150,60 l30,80 l10,-7.5 M210,117.5 l10,-7.5 l10,-60 a100,140 0 0,1 12,130  M150,60 v176"))));
            group.Children.Add(new GeometryDrawing(null, dash, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M180,220 l20,-10 l10,30 a100,140 0 0,0 32,-60 l-15,-15"))));
            Pen solid = new Pen(Brushes.White, 3);
            solid.LineJoin = PenLineJoin.Round;
            group.Children.Add(new GeometryDrawing(null, solid, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M70,50 a100,140 0 0,0 20,190 l10,-30 l20,10 l-20,30 a100,100 0 0,1 50,-13 M150,60 l-30,80 l-40,-30 l-10,-60"))));
            group.Children.Add(new GeometryDrawing(null, solid, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
               "M90,108 v9.5 l20,15 v-24.5 h6v-32h-32v32z M150,125 h-20 l20,-55 M85,125 l30,25 l-40,50z M150,170 h-25 v-10 l25,-20"))));
            Pen arrows = new Pen(Brushes.White, 2);
            arrows.LineJoin = PenLineJoin.Round;
            group.Children.Add(new GeometryDrawing(null, arrows, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
               "M70,50 h-40 M100,250 h-70 M35,50 v200 M30,60 l5,-10 l5,10 M30,240 l5,10 l5,-10 M52,130 v-100 M248,130 v-100 M52,35 h195 M62,30 l-10,5 l10,5 M237,30 l10,5 l-10,5"))));
            return new DrawingBrush(group);
        }
        public static Rectangle GetStoryLineRectangle()
        {
            Rectangle rect = new Rectangle();
            rect.Width = 1; rect.Height = 1;
            PathGeometry geom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M0,0 l7,18 h18 l-14,10 l9,22 l-20,-15 l-20,15 l9,-22 l-14,-10 h18z"));
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.GradientStops.Add(new GradientStop(Colors.Orange, 0.7));
            brush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
            brush.GradientStops.Add(new GradientStop(Colors.Orange, 0.3));
            rect.Fill = new DrawingBrush(new GeometryDrawing(brush, new Pen(Brushes.Orange, 0.01), geom));
            return rect;
        }
        public static DrawingBrush GetShipMoveBrush()
        {
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(new GeometryDrawing(Brushes.SkyBlue, new Pen(Brushes.Black, 2), new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M10,0 h20 l10,17.3 l-10,17.3 h-20 l-10,-17.3z"))));
            group.Children.Add(new GeometryDrawing(Brushes.Green, new Pen(Brushes.Black, 2), new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M40,17.3 h20 l10,17.3 l-10,17.3 h-20 l-10,-17.3z"))));
            group.Children.Add(new GeometryDrawing(Brushes.SkyBlue, new Pen(Brushes.White, 3), new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                            "M20,17.3 l30,17.3 l-3,-6 M50,34.6 l-6,0"))));
            return new DrawingBrush(group);
        }
    }
}
