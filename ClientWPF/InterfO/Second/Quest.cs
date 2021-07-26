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
using System.Text.RegularExpressions;

namespace Client
{
    /*
    public class QuestButton:InterfaceButton
    {
        public QuestButton():base(50,30,5,14)
        {
            SetText("Обучение");
            PreviewMouseDown += QuestButton_Click;
        }

        
        public void QuestButton_Click(object sender, MouseButtonEventArgs e)
        {
            switch (GSGameInfo.Quest)
            {
                case Quest_Position.Q01_Basic_Info: Q01_Basic_Info(); break;
                case Quest_Position.Q02_Build_Bank: Q02_Build_Bank(); break;
                case Quest_Position.Q03_Build_Mine: Q03_Build_Mine(); break;
                case Quest_Position.Q04_Build_Chips: Q04_Build_Chips(); break;
                case Quest_Position.Q05_Build_Anti: Q05_Build_Anti(); break;
                case Quest_Position.Q06_Scheme_Info: Q06_Scheme_Info(); break;
                case Quest_Position.Q07_Create_Scheme: Q07_Create_Scheme(); break;
                case Quest_Position.Q08_Build_Radar: Q08_Build_Radar(); break;
                case Quest_Position.Q09_Build_Ships: Q09_Build_Ships(); break;
                case Quest_Position.Q10_Hire_Pilots: Q10_Hire_Pilots(); break;
                case Quest_Position.Q11_Create_Fleet: Q11_Create_Fleet(); break;
                case Quest_Position.Q12_Move_Ships_to_Fleet: Q12_Move_Ship_To_Fleet(); break;
                case Quest_Position.Q13_Send_To_Mission: Q13_Send_To_Mission(); break;
                case Quest_Position.Q14_Battle_End: Q14_Battle_End(); break;
                case Quest_Position.Q15_Build_Factory: Q15_Build_Factory(); break;
                case Quest_Position.Q16_Learn_Science: Q16_Learn_Science(); break;
                case Quest_Position.Q17_Make_Trade: Q17_Make_Trade(); break;
                case Quest_Position.Q18_Conquer_Info: Q18_Conquer_Info(); break;
                case Quest_Position.Q19_Alliance_Info: Q19_Alliance_Info(); break;
                case Quest_Position.Q20_Chat_Info: Q20_Chat_Info(); break;
                case Quest_Position.Q21_All_Done: SetAllDonePanel(); break;
            }
        }
        public void SetAllDonePanel()
        {
            Border border = new Border();
            border.Width = 1000;
            border.Height = 650;
            border.BorderBrush = Brushes.White;
            border.BorderThickness = new Thickness(3);
            border.CornerRadius = new CornerRadius(20);
            border.Background = Brushes.Black;
            WrapPanel panel = new WrapPanel();
            panel.Orientation = Orientation.Vertical;
            panel.Margin = new Thickness(5);
            border.Child = panel;
            SortedList<int, string> CurQuests;
            if (Links.Lang == 0) CurQuests = Links.Quest_En; else CurQuests = Links.Quest_Ru;
            for (int i=1;i<21;i++)
            {
                InterfaceButton btn = new InterfaceButton(490, 50, 10, 16);
                btn.Margin = new Thickness(2);
                btn.Tag = i;
                btn.PreviewMouseDown += ShowQuest_Click;

                string text = QuestInfo.GetTitle(CurQuests[i]);
                btn.SetText( String.Format("{0}) {1}", i, text));
                panel.Children.Add(btn);
            }
            InterfaceButton CloseButton = new InterfaceButton(500, 50, 10, 16);
            //CloseButton.Width = 200; CloseButton.Height = 50;
            //CloseButton.FontFamily = Links.Font; CloseButton.FontSize = 20;
            //CloseButton.FontWeight = FontWeights.Bold;
            //CloseButton.Background = Brushes.Black;
            //CloseButton.Foreground = Brushes.White;
            CloseButton.SetText(Links.Interface("Close"));
            panel.Children.Add(CloseButton);
            CloseButton.PreviewMouseDown += CloseButton_Click;

            Links.Controller.PopUpCanvas.Place(border, false);
        }

        private void ShowQuest_Click(object sender, MouseButtonEventArgs e)
        {
            InterfaceButton btn = (InterfaceButton)sender;
            int tag = (int)btn.Tag;
            QuestWindow window = new QuestWindow(false);
            QuestInfo info;
            if (Links.Lang == 0)
                info = new QuestInfo(Links.Quest_En[tag]);
            else
                info = new QuestInfo(Links.Quest_Ru[tag]);
            window.SetTitle(info.Title);
            window.SetMainInfo(info.Info);
            Links.Controller.PopUpCanvas.Place(window, false);
        }

        private void CloseButton_Click(object sender, MouseButtonEventArgs e)
        {
            Links.Controller.PopUpCanvas.Remove();
        }

        public void Q20_Chat_Info()
        {
            QuestWindow window = new QuestWindow(true);
            QuestInfo info;
            if (Links.Lang == 0)
                info = new QuestInfo(Links.Quest_En[20]);
            else
                info = new QuestInfo(Links.Quest_Ru[20]);
            window.SetTitle(info.Title);
            window.SetMainInfo(info.Info);
            window.SetTarget(info.Target);
            window.SetReward(10000, 0, 0, 0);
            Links.Controller.PopUpCanvas.Place(window, false);
        }
        public void Q19_Alliance_Info()
        {
            QuestWindow window = new QuestWindow(true);
            QuestInfo info;
            if (Links.Lang == 0)
                info = new QuestInfo(Links.Quest_En[19]);
            else
                info = new QuestInfo(Links.Quest_Ru[19]);
            window.SetTitle(info.Title);
            window.SetMainInfo(info.Info);
            window.SetTarget(info.Target); window.SetReward(10000, 0, 0, 0);
            Links.Controller.PopUpCanvas.Place(window, false);
        }
        public void Q18_Conquer_Info()
        {
            QuestWindow window = new QuestWindow(true);
            QuestInfo info;
            if (Links.Lang == 0)
                info = new QuestInfo(Links.Quest_En[18]);
            else
                info = new QuestInfo(Links.Quest_Ru[18]);
            window.SetTitle(info.Title);
            window.SetMainInfo(info.Info);
            window.SetTarget(info.Target); window.SetReward(10000, 0, 0, 0);
            Links.Controller.PopUpCanvas.Place(window, false);
        }
        public void Q17_Make_Trade()
        {
            QuestWindow window = new QuestWindow(false);
            QuestInfo info;
            if (Links.Lang == 0)
                info = new QuestInfo(Links.Quest_En[17]);
            else
                info = new QuestInfo(Links.Quest_Ru[17]);
            window.SetTitle(info.Title);
            window.SetMainInfo(info.Info);
            window.SetTarget(info.Target); window.SetReward(10000, 0, 0, 0);
            Links.Controller.PopUpCanvas.Place(window, false);
        }
        public void Q16_Learn_Science()
        {
            QuestWindow window = new QuestWindow(false);
            QuestInfo info;
            if (Links.Lang == 0)
                info = new QuestInfo(Links.Quest_En[16]);
            else
                info = new QuestInfo(Links.Quest_Ru[16]);
            window.SetTitle(info.Title);
            window.SetMainInfo(info.Info);
            window.SetTarget(info.Target); window.SetReward(10000, 0, 0, 0);
            Links.Controller.PopUpCanvas.Place(window, false);
        }
        public void Q15_Build_Factory()
        {
            QuestWindow window = new QuestWindow(false);
            QuestInfo info;
            if (Links.Lang == 0)
                info = new QuestInfo(Links.Quest_En[15]);
            else
                info = new QuestInfo(Links.Quest_Ru[15]);
            window.SetTitle(info.Title);
            window.SetMainInfo(info.Info);
            window.SetTarget(info.Target); window.SetReward(1000, 100, 100, 100);
            Links.Controller.PopUpCanvas.Place(window, false);
        }
        public void Q14_Battle_End()
        {
            QuestWindow window = new QuestWindow(false);
            QuestInfo info;
            if (Links.Lang == 0)
                info = new QuestInfo(Links.Quest_En[14]);
            else
                info = new QuestInfo(Links.Quest_Ru[14]);
            window.SetTitle(info.Title);
            window.SetMainInfo(info.Info);
            window.SetTarget(info.Target); window.SetReward(3500, 300, 300, 0);
            Links.Controller.PopUpCanvas.Place(window, false);
        }
        public void Q13_Send_To_Mission()
        {
            QuestWindow window = new QuestWindow(false);
            QuestInfo info;
            if (Links.Lang == 0)
                info = new QuestInfo(Links.Quest_En[13]);
            else
                info = new QuestInfo(Links.Quest_Ru[13]);
            window.SetTitle(info.Title);
            window.SetMainInfo(info.Info);
            window.SetTarget(info.Target); window.SetReward(10000, 0, 0, 0);
            Links.Controller.PopUpCanvas.Place(window, false);
        }
        public void Q12_Move_Ship_To_Fleet()
        {
            QuestWindow window = new QuestWindow(false);
            QuestInfo info;
            if (Links.Lang == 0)
                info = new QuestInfo(Links.Quest_En[12]);
            else
                info = new QuestInfo(Links.Quest_Ru[12]);
            window.SetTitle(info.Title);
            window.SetMainInfo(info.Info);
            window.SetTarget(info.Target); window.SetReward(10000, 0, 0, 0);
            Links.Controller.PopUpCanvas.Place(window, false);
        }
        public void Q11_Create_Fleet()
        {
            QuestWindow window = new QuestWindow(false);
            QuestInfo info;
            if (Links.Lang == 0)
                info = new QuestInfo(Links.Quest_En[11]);
            else
                info = new QuestInfo(Links.Quest_Ru[11]);
            window.SetTitle(info.Title);
            window.SetMainInfo(info.Info);
            window.SetTarget(info.Target); window.SetReward(10000, 0, 0, 0);
            Links.Controller.PopUpCanvas.Place(window, false);
        }
        public void Q10_Hire_Pilots()
        {
            QuestWindow window = new QuestWindow(false);
            QuestInfo info;
            if (Links.Lang == 0)
                info = new QuestInfo(Links.Quest_En[10]);
            else
                info = new QuestInfo(Links.Quest_Ru[10]);
            window.SetTitle(info.Title);
            window.SetMainInfo(info.Info);
            window.SetTarget(info.Target); window.SetReward(10000, 0, 0, 0);
            Links.Controller.PopUpCanvas.Place(window, false);
        }
        public void Q09_Build_Ships()
        {
            QuestWindow window = new QuestWindow(false);
            QuestInfo info;
            if (Links.Lang == 0)
                info = new QuestInfo(Links.Quest_En[9]);
            else
                info = new QuestInfo(Links.Quest_Ru[9]);
            window.SetTitle(info.Title);
            window.SetMainInfo(info.Info);
            window.SetTarget(info.Target); window.SetReward(20000, 0, 0, 0);
            Links.Controller.PopUpCanvas.Place(window, false);
        }
        public void Q08_Build_Radar()
        {
            QuestWindow window = new QuestWindow(false);
            QuestInfo info;
            if (Links.Lang == 0)
                info = new QuestInfo(Links.Quest_En[8]);
            else
                info = new QuestInfo(Links.Quest_Ru[8]);
            window.SetTitle(info.Title);
            window.SetMainInfo(info.Info);
            window.SetTarget(info.Target); window.SetReward(20000, 1000, 1000, 1000);
            Links.Controller.PopUpCanvas.Place(window, false);
        }
        public void Q07_Create_Scheme()
        {
            QuestWindow window = new QuestWindow(false);
            QuestInfo info;
            if (Links.Lang == 0)
                info = new QuestInfo(Links.Quest_En[7]);
            else
                info = new QuestInfo(Links.Quest_Ru[7]);
            window.SetTitle(info.Title);
            window.SetMainInfo(info.Info);
            window.SetTarget(info.Target);
            window.SetReward(6000, 400, 800, 0);

            Links.Controller.PopUpCanvas.Place(window, false);
        }
        public void Q06_Scheme_Info()
        {
            QuestWindow window = new QuestWindow(true);
            QuestInfo info;
            if (Links.Lang == 0)
                info = new QuestInfo(Links.Quest_En[6]);
            else
                info = new QuestInfo(Links.Quest_Ru[6]);
            window.SetTitle(info.Title);
            window.SetMainInfo(info.Info);
            window.SetTarget(info.Target);
            window.SetReward(10000, 0, 0, 0);
            Links.Controller.PopUpCanvas.Place(window, false);
        }
        public void Q05_Build_Anti()
        {
            QuestWindow window = new QuestWindow(false);
            QuestInfo info;
            if (Links.Lang == 0)
                info = new QuestInfo(Links.Quest_En[5]);
            else
                info = new QuestInfo(Links.Quest_Ru[5]);
            window.SetTitle(info.Title);
            window.SetMainInfo(info.Info);
            window.SetTarget(info.Target);
            window.SetReward(10000, 0, 0, 0);
            Links.Controller.PopUpCanvas.Place(window, false);
        }
        public void Q04_Build_Chips()
        {
            QuestWindow window = new QuestWindow(false);
            QuestInfo info;
            if (Links.Lang == 0)
                info = new QuestInfo(Links.Quest_En[4]);
            else
                info = new QuestInfo(Links.Quest_Ru[4]);
            window.SetTitle(info.Title);
            window.SetMainInfo(info.Info);
            window.SetTarget(info.Target);
            window.SetReward(2500, 400, 600, 0);
            Links.Controller.PopUpCanvas.Place(window, false);
        }
        public void Q03_Build_Mine()
        {
            QuestWindow window = new QuestWindow(false);
            QuestInfo info;
            if (Links.Lang == 0)
                info = new QuestInfo(Links.Quest_En[3]);
            else
                info = new QuestInfo(Links.Quest_Ru[3]);
            window.SetTitle(info.Title);
            window.SetMainInfo(info.Info);
            window.SetTarget(info.Target);
            window.SetReward(3000, 300, 100, 0);
            Links.Controller.PopUpCanvas.Place(window, false);
        }
        public void Q02_Build_Bank()
        {
            QuestWindow window = new QuestWindow(false);
            QuestInfo info;
            if (Links.Lang == 0)
                info = new QuestInfo(Links.Quest_En[2]);
            else
                info = new QuestInfo(Links.Quest_Ru[2]);
            window.SetTitle(info.Title);
            window.SetMainInfo(info.Info);
            window.SetTarget(info.Target);
            window.SetReward(2000, 100, 200, 0);
            Links.Controller.PopUpCanvas.Place(window, false);
        }
        public void Q01_Basic_Info()
        {
            QuestWindow window = new QuestWindow(true);
            QuestInfo info;
            if (Links.Lang == 0)
                info = new QuestInfo(Links.Quest_En[1]);
            else
                info = new QuestInfo(Links.Quest_Ru[1]);
            window.SetTitle(info.Title);
            window.SetMainInfo(info.Info);
            window.SetTarget(info.Target);
            window.SetReward(3500, 200, 100, 0);
            Links.Controller.PopUpCanvas.Place(window, false);
        }
        
        
        public void SetQuest()
        {
            if (GSGameInfo.Quest==Quest_Position.Q21_All_Done)
            {
                Background = Brushes.Black;     
            }
            else
            {
                ColorAnimation anim = new ColorAnimation(Colors.Black, Colors.White, TimeSpan.FromSeconds(2));
                anim.RepeatBehavior = RepeatBehavior.Forever;
                anim.AutoReverse = true;
                Brush black = new SolidColorBrush(Colors.Black);
                black.BeginAnimation(SolidColorBrush.ColorProperty, anim);
                Background = black;
            }
        }
    }
    public class QuestWindow:Border
    {
        TextBlock Title;
        TextBlock RewardBlock;
        TextBlock TargetBlock;
        TextBlock MainInfoBlock;
        public QuestWindow(bool NeedFinishButton)
        {
            Width = 1000;
            Height = 600;
            BorderBrush = Brushes.White;
            BorderThickness = new Thickness(3);
            CornerRadius = new CornerRadius(20);
            Background = Brushes.Black;

            Canvas canvas = new Canvas();
            Child = canvas;

            Title = new TextBlock();
            Title.FontFamily = Links.Font;
            Title.FontSize = 24;
            Title.Foreground = Brushes.White;
            Title.Width = 800;
            canvas.Children.Add(Title);
            Canvas.SetLeft(Title, 100);
            Canvas.SetTop(Title, 5);
            Title.FontWeight = FontWeights.Bold;
            Title.TextAlignment = TextAlignment.Center;

            InterfaceButton CloseButton = new InterfaceButton(200, 50, 10, 20);
            CloseButton.PutToCanvas(canvas, 200, 540);
            CloseButton.SetText(Links.Interface("Close"));
            CloseButton.PreviewMouseDown += CloseButton_Click;

            if (NeedFinishButton)
            {
                InterfaceButton FinishButton = new InterfaceButton(200,50,10,20);
                FinishButton.PutToCanvas(canvas, 600, 540);
                FinishButton.SetText(Links.Interface("Finish"));
                FinishButton.PreviewMouseDown += FinishButton_Click;
            }
            Border RewardBorder = new Border();
            RewardBorder.Width = 900;
            RewardBorder.Height = 60;
            RewardBorder.BorderBrush = Brushes.White;
            RewardBorder.BorderThickness = new Thickness(2);
            RewardBorder.CornerRadius = new CornerRadius(10);
            canvas.Children.Add(RewardBorder);
            Canvas.SetLeft(RewardBorder, 50);
            Canvas.SetTop(RewardBorder, 475);

            RewardBlock = new TextBlock();
            RewardBlock.FontFamily = Links.Font;
            RewardBlock.FontSize = 26;
            RewardBlock.Foreground = Brushes.White;
            RewardBlock.FontWeight = FontWeights.Bold;
            RewardBlock.TextAlignment = TextAlignment.Center;
            RewardBorder.Child = RewardBlock;

            Border TargetBorder = new Border();
            TargetBorder.Width = 900;
            TargetBorder.Height = 60;
            TargetBorder.BorderBrush = Brushes.White;
            TargetBorder.BorderThickness = new Thickness(2);
            TargetBorder.CornerRadius = new CornerRadius(10);
            canvas.Children.Add(TargetBorder);
            Canvas.SetLeft(TargetBorder, 50);
            Canvas.SetTop(TargetBorder, 410);

            TargetBlock = new TextBlock();
            TargetBlock.FontFamily = Links.Font;
            TargetBlock.FontSize = 24;
            TargetBlock.Foreground = Brushes.White;
            TargetBlock.FontWeight = FontWeights.Bold;
            TargetBlock.TextAlignment = TextAlignment.Center;
            TargetBlock.VerticalAlignment = VerticalAlignment.Center;
            TargetBorder.Child = TargetBlock;

            Border MainInfoBorder = new Border();
            MainInfoBorder.Width = 900;
            MainInfoBorder.Height = 350;
            MainInfoBorder.BorderBrush = Brushes.White;
            MainInfoBorder.BorderThickness = new Thickness(2);
            MainInfoBorder.CornerRadius = new CornerRadius(10);
            canvas.Children.Add(MainInfoBorder);
            Canvas.SetLeft(MainInfoBorder, 50);
            Canvas.SetTop(MainInfoBorder, 50);

            ScrollViewer MainInfoViewer = new ScrollViewer();
            MainInfoViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            MainInfoViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            MainInfoBorder.Child = MainInfoViewer;

            MainInfoBlock = new TextBlock();
            MainInfoBlock.FontFamily = Links.Font;
            MainInfoBlock.FontSize = 24;
            MainInfoBlock.Foreground = Brushes.White;
            MainInfoBlock.TextAlignment = TextAlignment.Justify;
            MainInfoBlock.Margin = new Thickness(7);
            MainInfoBlock.TextWrapping = TextWrapping.Wrap;
            MainInfoViewer.Content = MainInfoBlock;
        }

        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            Links.Controller.PopUpCanvas.Remove();
            bool eventresult = Events.SimpleFinishQuest();
            if (eventresult)
            {
                Gets.GetTotalInfo();
                Gets.GetResources();
                //Links.Controller.mainWindow.QuestButton.QuestButton_Click(null, null);
            }
            else
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage("Error"), true);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Links.Controller.PopUpCanvas.Remove();
        }

        public void SetTitle(Inline[] collection)
        {
            foreach (Inline inl in collection)
            Title.Inlines.Add(inl);
        }
        public void SetReward(int money, int metall, int chips, int anti)
        {
            RewardBlock.Inlines.Add(new Run(Links.Interface("Reward") + ":"));
            if (money > 0)
            {
                RewardBlock.Inlines.Add(new InlineUIContainer(Common.GetRectangle(30, Links.Brushes.MoneyImageBrush)));
                RewardBlock.Inlines.Add(new Run(" " + money.ToString() + " "));
            }
            if (metall > 0)
            {
                RewardBlock.Inlines.Add(new InlineUIContainer(Common.GetRectangle(30, Links.Brushes.MetalImageBrush)));
                RewardBlock.Inlines.Add(new Run(" " + metall.ToString() + " "));
            }
            if (chips > 0)
            {
                RewardBlock.Inlines.Add(new InlineUIContainer(Common.GetRectangle(30, Links.Brushes.ChipsImageBrush)));
                RewardBlock.Inlines.Add(new Run(" " + chips.ToString() + " "));
            }
            if (chips > 0)
            {
                RewardBlock.Inlines.Add(new InlineUIContainer(Common.GetRectangle(30, Links.Brushes.AntiImageBrush)));
                RewardBlock.Inlines.Add(new Run(" " + anti.ToString() + " "));
            }
        }
        public void SetTarget(Inline[] collection)
        {
            foreach (Inline inl in collection)
                TargetBlock.Inlines.Add(inl);
        }
        public void SetMainInfo(Inline[] collection)
        {
            foreach (Inline inl in collection)
                MainInfoBlock.Inlines.Add(inl);
        }

    }
    class TextElement
    {
        public enum TextType { Run, BoldUnderline, Break, Italic, ItalicBold, ItalicUnderline,
            Money, Metall, Chips, Anti, MathPower, RepairPower,
        BuildSize, NoPilot, 
        LiveSector, Bank, Mine, SteelMelt, ChipFact, Univer, PartAcc, ScienceLab, Factory, Radar, DataCenter,
        PanelHealth, PanelShield, PanelEnergy, PanelArmor, PanelEvasion, PanelMove,
        WeaponGroups, ImmuneGroups, ItemSizes, PilotsRatingsGroup,
        Cargo, Colony, ShieldDamage, HealthDamage, Crit, EnergyAccuracy, PhysicAccuracy, EnergyConsume,
        EnergyGenerator, ShieldGenerator, AimComputer, Engine, Equipment,
        ShipStatusTime, ShipStatusRange, RepairIndicator,
        Laser, Missle}
        string Text;
        TextType Types;
        public TextElement(string text, TextType types)
        {
            Text = text;
            Types = types;
        }
        public TextElement(TextType types)
        {
            Types = types;
        }
        public Inline GetInline()
        {
            switch (Types)
                {
                    case TextType.Run: return new Run(Text);
                    case TextType.BoldUnderline:  return new Bold(new Underline(new Run(Text)));
                    case TextType.Break: return new LineBreak();
                    case TextType.Italic: return new Italic(new Run(Text));
                    case TextType.ItalicBold: return new Bold(new Italic(new Run(Text)));
                    case TextType.ItalicUnderline: return new Underline(new Italic(new Run(Text)));
                    case TextType.Money: return new InlineUIContainer(Common.GetRectangle(30, Links.Brushes.MoneyImageBrush));
                    case TextType.Metall: return new InlineUIContainer(Common.GetRectangle(30, Links.Brushes.MetalImageBrush));
                    case TextType.Chips: return new InlineUIContainer(Common.GetRectangle(30, Links.Brushes.ChipsImageBrush));
                    case TextType.Anti: return new InlineUIContainer(Common.GetRectangle(30, Links.Brushes.AntiImageBrush));
                    case TextType.MathPower: return new InlineUIContainer(Common.GetRectangle(30, Links.Brushes.MathPowerBrush));
                    case TextType.RepairPower: return new InlineUIContainer(Common.GetRectangle(30, Links.Brushes.ZipImageBrush));
                    case TextType.BuildSize: return new InlineUIContainer(Common.GetRectangle(30, Links.Brushes.BuildingSizeBrush));
                    case TextType.LiveSector: return new InlineUIContainer(Common.GetRectangle(30, Links.Brushes.TradeSector));
                    case TextType.Bank: return new InlineUIContainer(Common.GetRectangle(30, Links.Brushes.Bank));
                    case TextType.Mine: return new InlineUIContainer(Common.GetRectangle(30, Links.Brushes.Mine));
                    case TextType.SteelMelt: return new InlineUIContainer(Common.GetRectangle(30, Links.Brushes.RepairingBay));
                    case TextType.ChipFact: return new InlineUIContainer(Common.GetRectangle(30, Links.Brushes.ChipsFactory));
                    case TextType.Univer: return new InlineUIContainer(Common.GetRectangle(30, Links.Brushes.University));
                    case TextType.PartAcc: return new InlineUIContainer(Common.GetRectangle(30, Links.Brushes.ParticipleAccelerator));
                    case TextType.ScienceLab: return new InlineUIContainer(Common.GetRectangle(30, Links.Brushes.ScienceSector));
                    case TextType.Factory: return new InlineUIContainer(Common.GetRectangle(30, Links.Brushes.Manufacture));
                    case TextType.Radar: return new InlineUIContainer(Common.GetRectangle(30, Links.Brushes.Radar));
                    case TextType.DataCenter: return new InlineUIContainer(Common.GetRectangle(30, Links.Brushes.DataCenter));
                    case TextType.PanelHealth: return new InlineUIContainer(GetPanelHeal());
                    case TextType.PanelShield: return new InlineUIContainer(GetPanelShield());
                    case TextType.PanelEnergy: return new InlineUIContainer(GetPanelEnergy());
                    case TextType.PanelArmor:return new InlineUIContainer(GetPanelAbsorp());
                    case TextType.PanelEvasion: return new InlineUIContainer(GetPanelEvasion());
                    case TextType.PanelMove: return new InlineUIContainer(new Location(100, 1));
                    case TextType.WeaponGroups: return new InlineUIContainer(GetWeaponGroupsPanel());
                    case TextType.ImmuneGroups: return new InlineUIContainer(GetImmuneGroupsPanel());
                    case TextType.Cargo: return new InlineUIContainer(Common.GetRectangle(30, Pictogram.CargoBrush));
                    case TextType.Colony: return new InlineUIContainer(Common.GetRectangle(30, Pictogram.ColonyBrush));
                    case TextType.Laser: return new InlineUIContainer(WeapCanvas.GetCanvas(EWeaponType.Laser,30,false));
                    case TextType.Missle: return new InlineUIContainer(WeapCanvas.GetCanvas(EWeaponType.Missle, 30, false));
                    case TextType.HealthDamage: return new InlineUIContainer(Common.GetRectangle(30, Pictogram.HealthDamage));
                    case TextType.ShieldDamage: return new InlineUIContainer(Common.GetRectangle(30, Pictogram.ShieldDamage));
                    case TextType.EnergyConsume: return new InlineUIContainer(Common.GetRectangle(30, Pictogram.EnergyBrush));
                    case TextType.Crit: return new InlineUIContainer(Common.GetRectangle(30, Pictogram.CriticalChance));
                    case TextType.EnergyAccuracy: return new InlineUIContainer(Common.GetRectangle(30, Pictogram.AccuracyEnergy));
                    case TextType.PhysicAccuracy: return new InlineUIContainer(Common.GetRectangle(30, Pictogram.AccuracyPhysic));
                    case TextType.ItemSizes: return new InlineUIContainer(GetItemSizesPanel());
                    case TextType.EnergyGenerator: return new InlineUIContainer(Common.GetRectangle(30, Links.Brushes.GeneratorBrush));
                    case TextType.ShieldGenerator: return new InlineUIContainer(Common.GetRectangle(30, Links.Brushes.ShieldBrush));
                    case TextType.AimComputer: return new InlineUIContainer(Common.GetRectangle(30, Links.Brushes.ComputerBrush));
                    case TextType.Engine: return new InlineUIContainer(Common.GetRectangle(30, Links.Brushes.EngineBrush));
                    case TextType.Equipment: return new InlineUIContainer(Common.GetRectangle(30, Links.Brushes.EquipmentBrush));
                    case TextType.PilotsRatingsGroup: return new InlineUIContainer(GetPilotRatingsGroup());
                    case TextType.NoPilot: return new InlineUIContainer(Common.GetRectangle(30, Links.Brushes.EmptyImageBrush));
                    case TextType.ShipStatusTime: return new InlineUIContainer(new HexShipStatus(EHexShipStasus.Port, 3, 0));
                    case TextType.ShipStatusRange: return new InlineUIContainer(new HexShipStatus(EHexShipStasus.Gate, 2, 0));
                    case TextType.RepairIndicator: return new InlineUIContainer(ShipInfoPanel2.GetRepairIndicator(60, 4));
                default:return null;
                }
            }
        public static Inline[] GetFormattedText(string text, ref int i)
        {
            TextElement[] elements = GetElements(text, ref i);
            List<Inline> list = new List<Inline>();
            foreach (TextElement element in elements)
                list.Add(element.GetInline());
            return list.ToArray();
        }
        static Regex firsttag=new Regex("<[A-Z]{1,5}>");
        static Regex lastRreg = new Regex("</R>");
        static Regex lastBUreg = new Regex("</BU>");
        static Regex italictreg = new Regex("</I>");
        static Regex italboldreg = new Regex("</IB>");
        static Regex italunderreg = new Regex("</IU>");
        static TextElement[] GetElements(string text, ref int i)
        {
            Match m1;
            string s;
            List<TextElement> list = new List<TextElement>();
            for (; i < text.Length;)
            {
                Match m = firsttag.Match(text, i);
                if (m.Success == false) break;
                i = m.Index + m.Length;
                switch (m.Value)
                {
                    case "<END>":
                        return list.ToArray();
                    case "<R>":
                        m1 = lastRreg.Match(text, i);
                        s = text.Substring(i, m1.Index - i);
                        list.Add(new TextElement(s, TextType.Run ));
                        i = m1.Index + m1.Length;
                        break;
                    case "<BU>":
                        m1 = lastBUreg.Match(text, i);
                        s = text.Substring(i, m1.Index - i);
                        list.Add(new TextElement(s, TextType.BoldUnderline));
                        i = m1.Index + m1.Length;
                        break;
                    case "<I>":
                        m1 = italictreg.Match(text, i);
                        s = text.Substring(i, m1.Index - i);
                        list.Add(new TextElement(s, TextType.Italic));
                        i = m1.Index + m1.Length;
                        break;
                    case "<IB>":
                        m1 = italboldreg.Match(text, i);
                        s = text.Substring(i, m1.Index - i);
                        list.Add(new TextElement(s, TextType.ItalicBold));
                        i = m1.Index + m1.Length;
                        break;
                    case "<IU>":
                        m1 = italunderreg.Match(text, i);
                        s = text.Substring(i, m1.Index - i);
                        list.Add(new TextElement(s, TextType.ItalicUnderline));
                        i = m1.Index + m1.Length;
                        break;
                    case "<BR>": list.Add(new TextElement(TextType.Break)); break;
                    case "<MONEY>": list.Add(new TextElement(TextType.Money)); break;
                    case "<MET>": list.Add(new TextElement(TextType.Metall)); break;
                    case "<CHIPS>": list.Add(new TextElement(TextType.Chips)); break;
                    case "<ANTI>": list.Add(new TextElement(TextType.Anti)); break;
                    case "<MPOWE>": list.Add(new TextElement(TextType.MathPower)); break;
                    case "<REPOW>": list.Add(new TextElement(TextType.RepairPower)); break;
                    case "<BSIZE>": list.Add(new TextElement(TextType.BuildSize)); break;
                    case "<LIVS>": list.Add(new TextElement(TextType.LiveSector)); break;
                    case "<BANK>": list.Add(new TextElement(TextType.Bank)); break;
                    case "<MINE>": list.Add(new TextElement(TextType.Mine)); break;
                    case "<STMEL>": list.Add(new TextElement(TextType.SteelMelt)); break;
                    case "<CHFAC>": list.Add(new TextElement(TextType.ChipFact)); break;
                    case "<UNIV>": list.Add(new TextElement(TextType.Univer)); break;
                    case "<PARAC>": list.Add(new TextElement(TextType.PartAcc)); break;
                    case "<SCLAB>": list.Add(new TextElement(TextType.ScienceLab)); break;
                    case "<FACT>": list.Add(new TextElement(TextType.Factory)); break;
                    case "<RADAR>": list.Add(new TextElement(TextType.Radar)); break;
                    case "<DATAC>": list.Add(new TextElement(TextType.DataCenter)); break;
                    case "<PHEAL>": list.Add(new TextElement(TextType.PanelHealth)); break;
                    case "<PSHIE>": list.Add(new TextElement(TextType.PanelShield)); break;
                    case "<PENER>": list.Add(new TextElement(TextType.PanelEnergy)); break;
                    case "<PARMO>": list.Add(new TextElement(TextType.PanelArmor)); break;
                    case "<PEVAS>": list.Add(new TextElement(TextType.PanelEvasion)); break;
                    case "<PMOVE>": list.Add(new TextElement(TextType.PanelMove)); break;
                    case "<WEAPG>": list.Add(new TextElement(TextType.WeaponGroups)); break;
                    case "<IMMUG>": list.Add(new TextElement(TextType.ImmuneGroups)); break;
                    case "<CARGO>": list.Add(new TextElement(TextType.Cargo)); break;
                    case "<COLON>": list.Add(new TextElement(TextType.Colony)); break;
                    case "<WLASE>": list.Add(new TextElement(TextType.Laser)); break;
                    case "<WMISL>": list.Add(new TextElement(TextType.Missle)); break;
                    case "<HEDAM>": list.Add(new TextElement(TextType.HealthDamage)); break;
                    case "<SHDAM>": list.Add(new TextElement(TextType.ShieldDamage)); break;
                    case "<ENCON>": list.Add(new TextElement(TextType.EnergyConsume)); break;
                    case "<CRIT>": list.Add(new TextElement(TextType.Crit)); break;
                    case "<ENACC>": list.Add(new TextElement(TextType.EnergyAccuracy)); break;
                    case "<PHACC>": list.Add(new TextElement(TextType.PhysicAccuracy)); break;
                    case "<ISIZE>": list.Add(new TextElement(TextType.ItemSizes)); break;
                    case "<EGEN>": list.Add(new TextElement(TextType.EnergyGenerator)); break;
                    case "<SGEN>": list.Add(new TextElement(TextType.ShieldGenerator)); break;
                    case "<AIMC>": list.Add(new TextElement(TextType.AimComputer)); break;
                    case "<ENGIN>": list.Add(new TextElement(TextType.Engine)); break;
                    case "<EQUIP>": list.Add(new TextElement(TextType.Equipment)); break;
                    case "<PIRAG>": list.Add(new TextElement(TextType.PilotsRatingsGroup)); break;
                    case "<NOPIL>": list.Add(new TextElement(TextType.NoPilot)); break;
                    case "<SSTTE>": list.Add(new TextElement(TextType.ShipStatusTime)); break;
                    case "<SSROE>": list.Add(new TextElement(TextType.ShipStatusRange)); break;
                    case "<REPIN>":  list.Add(new TextElement(TextType.RepairIndicator)); break;
                }
            }
            return list.ToArray();
        }
        static Canvas GetPanelHeal()
        {
            Canvas canvas = new Canvas(); canvas.Width = 55; canvas.Height = 50;
            canvas.Background = Brushes.Silver;
            canvas.Children.Add(new HealthCanvas(300, 200, 50));
            canvas.Clip = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M0,12 a12.5,10 0 0,1 25.0,0 a12.5,10 0 0,1 25.0,0 a35,35 0 0,1 -25,38 a35,35 0 0,1 -25,-38"));
            canvas.ClipToBounds = true;
            return canvas;
        }
        static Canvas GetPanelShield()
        {
            Canvas canvas = new Canvas(); canvas.Width = 55; canvas.Height = 50;
            canvas.Background = Brushes.Silver;
            canvas.Children.Add(new ShieldCanvas(400, 300, 200, 30));
            canvas.Clip = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M0,12 a25,12 0 0,1 50,0 a30,50 0 0,1 -25,38 a30,50 0 0,1 -25,-38"));
            canvas.ClipToBounds = true;
            return canvas;
        }
        static Canvas GetPanelEnergy()
        {
            Canvas canvas = new Canvas(); canvas.Width = 55; canvas.Height = 50;
            canvas.Background = Brushes.Silver;
            canvas.Children.Add(new EnergyCanvas(500, 400, 300));
            Canvas.SetLeft(canvas.Children[0], 10);
            Canvas.SetTop(canvas.Children[0], 10);
            canvas.Clip = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M20,10 h25 l-10,15 h10 l-10,15 h10 l-25,10 l-25,-10 h10 l10,-15 h-10 l10,-15z"));
            canvas.ClipToBounds = true;
            return canvas;
        }
        static Canvas GetPanelAbsorp()
        {
            Canvas canvas = new Canvas(); canvas.Width = 55; canvas.Height = 50;
            canvas.Background = Brushes.Silver;
            canvas.Children.Add(new AbsorpCanvas(10, 20, 30, 40));
            canvas.Clip = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M24,1 a25,25 0 1,0 0.1,0"));
            canvas.ClipToBounds = true;
            return canvas;
        }
        static Canvas GetPanelEvasion()
        {
            Canvas canvas = new Canvas(); canvas.Width = 55; canvas.Height = 50;
            canvas.Background = Brushes.Silver;
            canvas.Children.Add(new EvasionCanvas (15, 25, 35, 45));
            canvas.Clip = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M27.5,-3 a28,28 0 1,0 0.1,0"));
            Canvas.SetLeft(canvas.Children[0], 2.5);
            canvas.ClipToBounds = true;
            return canvas;
        }
        static StackPanel GetWeaponGroupsPanel()
        {
            StackPanel panel = new StackPanel();
            panel.Orientation = Orientation.Horizontal;
            panel.Children.Add(Common.GetRectangle(30, Links.Brushes.WeaponGroupBrush[WeaponGroup.Energy]));
            panel.Children.Add(Common.GetRectangle(30, Links.Brushes.WeaponGroupBrush[WeaponGroup.Physic]));
            panel.Children.Add(Common.GetRectangle(30, Links.Brushes.WeaponGroupBrush[WeaponGroup.Irregular]));
            panel.Children.Add(Common.GetRectangle(30, Links.Brushes.WeaponGroupBrush[WeaponGroup.Cyber]));
            return panel;
        }
        static StackPanel GetImmuneGroupsPanel()
        {
            StackPanel panel = new StackPanel();
            panel.Orientation = Orientation.Horizontal;
            panel.Children.Add(Common.GetRectangle(30, Pictogram.ImmuneEnergy));
            panel.Children.Add(Common.GetRectangle(30, Pictogram.ImmunePhysic));
            panel.Children.Add(Common.GetRectangle(30, Pictogram.ImmuneIrregular));
            panel.Children.Add(Common.GetRectangle(30, Pictogram.ImmuneCyber));
            return panel;
        }
        static StackPanel GetItemSizesPanel()
        {
            StackPanel panel = new StackPanel();
            panel.Orientation = Orientation.Horizontal;
            panel.Children.Add(Common.GetRectangle(30, Links.Brushes.SmallImageBrush));
            panel.Children.Add(Common.GetRectangle(30, Links.Brushes.MediumImageBrush));
            panel.Children.Add(Common.GetRectangle(30, Links.Brushes.LargeImageBrush));
            return panel;
        }
        static StackPanel GetPilotRatingsGroup()
        {
            StackPanel panel = new StackPanel();
            panel.Orientation = Orientation.Horizontal;
            panel.Children.Add(PilotsImage.GetRatingImage(0, 0, 0, 40));
            ((Rectangle)panel.Children[0]).Margin = new Thickness(0, 0, 5, 0);
            panel.Children.Add(PilotsImage.GetRatingImage(1, 1, 1, 40));
            ((Rectangle)panel.Children[1]).Margin = new Thickness(0, 0, 5, 0);
            panel.Children.Add(PilotsImage.GetRatingImage(2, 2, 2, 40));
            ((Rectangle)panel.Children[2]).Margin = new Thickness(0, 0, 5, 0);
            panel.Children.Add(PilotsImage.GetRatingImage(3, 3, 3, 40));
            ((Rectangle)panel.Children[3]).Margin = new Thickness(0, 0, 5, 0);
            panel.Children.Add(PilotsImage.GetRatingImage(4, 0, 4, 40));
            ((Rectangle)panel.Children[4]).Margin = new Thickness(0, 0, 5, 0);
            return panel;
        }
        
    }
    class QuestInfo
    {
        public Inline[] Title;
        public Inline[] Info;
        public Inline[] Target;
        public QuestInfo(string text)
        {
            Match m1;
            int i = 0;
            List<TextElement> list = new List<TextElement>();
            Regex titlereg = new Regex("<TITLE>");
            Regex inforeg = new Regex("<INFO>");
            Regex missionreg = new Regex("<TARGET>");
            m1 = titlereg.Match(text, 0);
            if (m1.Success == false)
                throw new Exception();
            i = m1.Index + m1.Length;
            Title = TextElement.GetFormattedText(text, ref i);
            m1 = inforeg.Match(text, i);
            if (m1.Success == false)
                throw new Exception();
            i = m1.Index + m1.Length;
            Info = TextElement.GetFormattedText(text, ref i);
            m1 = missionreg.Match(text, i);
            if (m1.Success == false)
                throw new Exception();
            i = m1.Index + m1.Length;
            Target = TextElement.GetFormattedText(text, ref i);

        }
        public static string GetTitle(string text)
        {
            Regex titlereg = new Regex("<TITLE>");
            Match m1 = titlereg.Match(text, 0);
            Regex endreg = new Regex("<END>");
            Match m2 = endreg.Match(text, 0);
            return text.Substring(m1.Index + m1.Length+6, m2.Index - m1.Index - m1.Length-13);
        }
    }
    */
}
