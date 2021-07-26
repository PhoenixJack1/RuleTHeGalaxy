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
using System.Windows.Media.Animation;

enum ENotice
{
    ColonizeBattleStart, //начало битвы за колонизацию                                      ВЫПОЛНЕНО 2
    ColonizeBattleWin, //Победа в битве за колонизацию                                      ВЫПОЛНЕНО
    ColonizeBattleLose, //Поражение в битве за колонизацию                                  ВЫПОЛНЕНО

    PillageAttackStart, //Начало битвы за грабёж в роли атаки                               ВЫПОЛНЕНО  
    PillageAttackWinPillage, //Победа в битве за грабёж и ограбление                        ВЫПОЛНЕНО
    PillageAttackWinNext, //Победа в битве за грабёж в роли атаки и следующая битва         ВЫПОЛНЕНО
    PillageAttackWinReturn, //Победа в битве за грабёж в роли атаки и возвращение           ВЫПОЛНЕНО
    PillageAttackLose, //Поражение в битве за грабёж в роли атаки                           ВЫПОЛНЕНО
    PillageNoDefenseWin, //Успешное ограбление колонии без защиты                           ВЫПОЛНЕНО
    PillageNoDefenseLose, //Ограблние вашей колонии                                         ВЫПОЛНЕНО
    DefenseSelfStart, //Начало битвы в роли защиты собственной колонии                      ВЫПОЛНЕНО
    DefenseSelfWin, //Победа в битве в роли защиты собственной колонии                      ВЫПОЛНЕНО
    DefenseSelfLose, //Поражение в битве в роли защиты собственной колонии                  ВЫПОЛНЕНО
    DefenseClanStart, //Начало битвы в роли защиты колонии клана                            ВЫПОЛНЕНО
    DefenseClanWin, //Победа в битве в роли защиты колонии клана                            ВЫПОЛНЕНО
    DefenseClanLose, //Поражение в битве в роли защиты колонии клана                        ВЫПОЛНЕНО
    DefenseByClanStart, //Начало битвы за защиту своей колонии флотом клана                 ВЫПОЛНЕНО
    DefenseByClanWin, //Победа в битве за защиту своей колонии флотом клана                 ВЫПОЛНЕНО
    DefenseByClanLose, //Поражение в битве за защиту своей колонии флотом клана             ВЫПОЛНЕНО

    FreeColonyStart, //Начало колонизации новой колонии
    FreeColonySucsess, //Успешая колонизация колонии
    FreeColonyFail, //Неудачная колонизация колонии

    ColonyAttackStart, //Начало битвы за колонизацию в роли атаки                               ВЫПОЛНЕНО ТЕСТ
    ColonyAttackWinReturn, //Победа в битве но невозможность продолжения                        ВЫПОЛНЕНО ТЕСТ
    ColonyAttackWinColony, //Победа в битве за колонизацию в роли атаки и колонизация           В ПРОЦЕССЕ
    ColonyAttackWinFail, //Победа в битве за колонизацию в роли атаки но неудачная колонизация  В ПРОЦЕССЕ
    ColonyAttackWinLastFail, //Победа в битве за колонизацию но невозможность начать фин битву  В ПРОЦЕССЕ
    ColonyAttackWinLast, //Победа в битве за колонизацию в роли атаки и начало финальной битвы  В ПРОЦЕССЕ
    ColonyAttackWinNext, //Победа в битве за колонизацию в роли атаки и повторная битва         В ПРОЦЕССЕ
    ColonyAttackWinUprise, //Победа в битве за колонизацию в роли атаки и восстание             В ПРОЦЕССЕ
    ColonyAttackWinNoUprise, //Победа в битве за колонизацию в роли атаки но восстание уже есть В ПРОЦЕССЕ
    ColonyAttackLose, //Поражение в битве за колонизацию в роли атаки                           В ПРОЦЕССЕ

    ColonyLastAttackFailStart, //Невозможность начать финальную битву                           В ПРОЦЕССЕ
    ColonyLastAttackStart, //Начало финальной битвы в роли атаки                                В ПРОЦЕССЕ
    ColonyLastAttackWinColony, //Победа в финальной битве и колонизация колонии                 В ПРОЦЕССЕ
    ColonyLastAttackWinFail, //Победа в финальной битве но неудачная колонизация                В ПРОЦЕССЕ
    ColonyLastAttackWinUprise, //Победа в финальной битве и восстание                           В ПРОЦЕССЕ
    ColonyLastAttackWinNoUprise, //Победа в финальной битве но неудачное восстание              В ПРОЦЕССЕ
    ColonyLastAttackLose, //Поражение в финальной битве                                         В ПРОЦЕССЕ

    ColonyNoDefenseColonyWin, //Колонизация колонии без защиты                                  В ПРОЦЕССЕ
    ColonyNoDefenseFail, //Неудачная колонизация без защиты                                     В ПРОЦЕССЕ
    ColonyNoDefenseColonyLose, //Потеря колонии                                                 В ПРОЦЕССЕ
    ColonyNoDefenseUpriseWin, //Организация восстания в колонии без защиты                      В ПРОЦЕССЕ
    ColonyNoDefenseUpriseLose, //Восстание на вашей колонии                                     В ПРОЦЕССЕ
    ColonyNoDefenseNoUprise, //В колонии уже запущено восстание                                 В ПРОЦЕССЕ

    ColonyNewLose, //Потеряна строящаяся колония
    ColonyNewDestroyed, //Разрушена строящаяся колония

    LastDefenseStart, //Флот вступил в битву на последнюю защиту колонии                        В ПРОЦЕССЕ
    LastDefenseWin, //Флот выиграл в битве на последнюю защиту колонии                          В ПРОЦЕССЕ
    LastDefenseLoseColony, //Флот проиграл в последней битве и колония потеряна                 В ПРОЦЕССЕ
    LastDefenseLoseNoColony, //Флот проиграл в последней битве но колонизация не удалась        В ПРОЦЕССЕ
    LastDefenseLoseUprise, //Флот проиграл в последней битве и начато восстание                 В ПРОЦЕССЕ
    LastDefenseLoseNoUprise, //Флот проиграл в последней битве но восстание уже запущено        В ПРОЦЕССЕ

    PirateBattleStart, //Начало битвы с пиратами
    PirateBattleWin, //Победа в битве с пиратами
    PirateBattleLose, //Поражение в битве с пиратами

    GreenTeamBattleStart,
    GreenTeamBattleWin,
    GreenTeamBattleLose,

    AlienBattleStart,
    AlienBattleWin,
    AlienBattleLose,

    TechnoTeamBattleStart,
    TechnoTeamBattleWin,
    TechnoTeamBattleLose,

    MercBattleStart,
    MercBattleWin,
    MercBattleLose,

    ResourcesBattleStart, //Начало битвы за ресурсы
    ResourceBattleWin, //Победа в битве за ресурсы
    ResourceBattleLose, //Поражение в битве за ресурсы
    TargetColonyOwnerChanged, //у цели (колонии) сменился хозяин

    LearnNewScience,

    AddPremiumCurrency,
    ActivatePremium,
    AddPremiumQuickStart,
    AddPremiumOrion,

    LoseLandByEnemy,

    ColonyNewLandWin,
    ColonyNewLandLose,

    StoryLineStart,
    StoryLineWin,
    StoryLineLose,
    

    RecieceReward,
    RecieveMissionReward
}
namespace Client
{
    class GameNotice
    {
        static byte[] array;
        static int i;
        static TextBlock tb;
        static DateTime dt;
        public static List<TextBlock> GetNoticePanels(byte[] Array)
        {
            List<TextBlock> list = new List<TextBlock>();
            array = Array;
            //array = Gets.GetNoticeList();
            if (array == null) return null;
            for (i = 0; i < array.Length;)
            {

                dt = GetTime(BitConverter.ToInt64(array, i)); i += 8;
                ENotice type = (ENotice)array[i]; i++;
                //grid = new Grid();
                //PutElement(GetTime(BitConverter.ToInt64(array, i)), 100, 0);
                //i+=8;
                //ENotice type = (ENotice)array[i]; i++;

                switch (type)
                {
                    case ENotice.ColonizeBattleStart: GetColonizeBattleStart(); break; //+
                    case ENotice.ColonizeBattleWin: FillColonizeBattleWin(); break;//+
                    case ENotice.ColonizeBattleLose: FillColonizeBattleLose(); break;//+
                    case ENotice.DefenseSelfStart: FillDefenseSelfStart(); break;//+
                    case ENotice.DefenseSelfWin: FillDefenseSelfWin(); break;//+
                    case ENotice.DefenseSelfLose: FillDefenseSelfLose(); break;//+
                    case ENotice.DefenseClanStart: FillDefenseClanStart(); break;//+
                    case ENotice.DefenseClanWin: FillDefenseClanWin(); break;//-
                    case ENotice.DefenseClanLose: FillDefenseClanLose(); break;//+
                    case ENotice.DefenseByClanStart: FillDefenseByClanStart(); break;//-
                    case ENotice.DefenseByClanWin: FillDefenseByClanWin(); break;//-
                    case ENotice.DefenseByClanLose: FillDefenseByClanLose(); break;
                    case ENotice.PillageAttackStart: FillPillageAttackStart(); break;
                    case ENotice.PillageAttackWinPillage: FillPillageAttackWinPillage(); break;
                    case ENotice.PillageAttackWinNext: FillPillageAttackWinNext(); break;
                    case ENotice.PillageAttackWinReturn: FillPillageAttackWinReturn(); break;
                    case ENotice.PillageAttackLose: FillPillageAttackLose(); break;
                    case ENotice.PillageNoDefenseWin: FillPillageNoDefenseWin(); break;
                    case ENotice.PillageNoDefenseLose: FillPilageNoDefenseLose(); break;//+
                    case ENotice.ColonyAttackStart: FillColonyAttackStart(); break;
                    case ENotice.ColonyAttackWinReturn: FillColonyAttackWinReturn(); break;
                    case ENotice.ColonyAttackWinColony: FillColonyAttackWinColony(); break;
                    case ENotice.ColonyAttackWinFail: FillColonyAttackWinFail(); break;
                    case ENotice.ColonyAttackWinLastFail: FillColonyAttackWinLastFail(); break;
                    case ENotice.ColonyAttackWinLast: FillColonyAttackWinLast(); break;
                    case ENotice.ColonyAttackWinNext: FillColonyAttackWinNext(); break;
                    case ENotice.ColonyAttackWinUprise: FillColonyAttackWinUprise(); break;
                    case ENotice.ColonyAttackWinNoUprise: FillColonyAttackWinNoUprise(); break;
                    case ENotice.ColonyAttackLose: FillColonyAttackLose(); break;
                    case ENotice.ColonyLastAttackFailStart: FillColonyLastAttackFailStart(); break;
                    case ENotice.ColonyLastAttackStart: FillColonyLastAttackStart(); break;
                    case ENotice.ColonyLastAttackWinColony: FillColonyLastAttackWinColony(); break;
                    case ENotice.ColonyLastAttackWinFail: FillColonyLastAttackFail(); break;
                    case ENotice.ColonyLastAttackWinUprise: FillColonyLastAttackWinUprise(); break;
                    case ENotice.ColonyLastAttackWinNoUprise: FillColonyLastAttackWinNoUprise(); break;
                    case ENotice.ColonyLastAttackLose: FillColonyLastAttackLose(); break;
                    case ENotice.ColonyNoDefenseColonyWin: FillColonyNoDefenseColonyWin(); break;
                    case ENotice.ColonyNoDefenseFail: FillColonyNoDefenseFail(); break;
                    case ENotice.ColonyNoDefenseColonyLose: FillColonyNoDefenseColonyLose(); break;
                    case ENotice.ColonyNoDefenseUpriseWin: FillColonyNoDefenseUpriseWin(); break;
                    case ENotice.ColonyNoDefenseUpriseLose: FillColonyNoDefenseUpriseLose(); break;
                    case ENotice.ColonyNoDefenseNoUprise: FillColonyNoDefenseNoUprise(); break;
                    case ENotice.ColonyNewLose: FillColonyNewLose(); break;
                    case ENotice.ColonyNewDestroyed: FillColonyNewDestroyed(); break;
                    case ENotice.LastDefenseStart: FillLastDefenseStart(); break;
                    case ENotice.LastDefenseWin: FillLastDefenseWin(); break;
                    case ENotice.LastDefenseLoseColony: FillLastDefenseLoseColony(); break;
                    case ENotice.LastDefenseLoseNoColony: FillLastDefenseLoseNoColony(); break;
                    case ENotice.LastDefenseLoseUprise: FillLastDefenseLoseUprise(); break;
                    case ENotice.LastDefenseLoseNoUprise: FillLastDefenseLoseNoUprise(); break;
                    case ENotice.PirateBattleStart: FillPirateBattleStart(); break;
                    case ENotice.PirateBattleWin: FillPirateBattleWin(); break;
                    case ENotice.PirateBattleLose: FillPirateBattleLose(); break;
                    case ENotice.GreenTeamBattleStart: FillGreenTeamBattleStart(); break;
                    case ENotice.GreenTeamBattleWin: FillGreenTeamBattleWin(); break;
                    case ENotice.GreenTeamBattleLose: FillGreenTeamBattleLose(); break;
                    case ENotice.AlienBattleStart: FillAlienBattleStart(); break;
                    case ENotice.AlienBattleWin: FillAlienBattleWin(); break;
                    case ENotice.AlienBattleLose: FillAlienBattleLose(); break;
                    case ENotice.TechnoTeamBattleStart: FillTechTeamBattleStart(); break;
                    case ENotice.TechnoTeamBattleWin: FilltechTeamBattleWin(); break;
                    case ENotice.TechnoTeamBattleLose: FillTechTeamBattleLose(); break;
                    case ENotice.MercBattleStart: FillMercBattleStart(); break;
                    case ENotice.MercBattleWin: FillMercBattleWin(); break;
                    case ENotice.MercBattleLose: FillMercBattleLose(); break;
                    case ENotice.TargetColonyOwnerChanged: FillTargetColonyOwnerChanged(); break;
                    case ENotice.LearnNewScience: FillLearnNewScience(); break;
                    case ENotice.AddPremiumCurrency: FillAddPremiumCurrency(); break;
                    case ENotice.ActivatePremium: FillActivatePremium(); break;
                    case ENotice.AddPremiumOrion: FillOrionPremium(); break;
                    case ENotice.AddPremiumQuickStart: FillQuickStartPremium(); break;
                    case ENotice.LoseLandByEnemy: FillLoseLandByEnemy(); break;
                    case ENotice.ColonyNewLandWin: FillColonyNewLandWin(); break;
                    case ENotice.ColonyNewLandLose: FillColonyNewLandLose(); break;
                    case ENotice.StoryLineStart: FillStoryLinekStart(); break;
                    case ENotice.StoryLineWin: FillStoryLinekWin(); break;
                    case ENotice.StoryLineLose: FillStoryLineLose(); break;

                    case ENotice.RecieceReward: FillRecieveReward(); break;
                    case ENotice.RecieveMissionReward: FillRecieveMission2Reward(); break;
                }
                list.Add(tb);


            }
            return list;
        }
        #region Colonize

        public static void GetColonizeBattleStart()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Battle);
            AddFleetEmblem();
            AddBattle();
            tb.Inlines.Add(new Run(Links.Interface("NotColonizeBattleStart")));
            AddPlanet();
        }
        static void tb_EnterInBattleMouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;
            long battleid = (long)tb.Tag;
            Links.Controller.IntBoya.Place(battleid);
        }

        static void AddBattle()
        {
            long battleid = BitConverter.ToInt64(array, i); i += 8;
            tb.Tag = battleid;
            tb.PreviewMouseDown += new MouseButtonEventHandler(tb_EnterInBattleMouseDown);
        }
        static void AddFleetEmblem()
        {
            int fleetimage = BitConverter.ToInt32(array, i); i += 4;
            FleetEmblem emblem = new FleetEmblem(fleetimage, 20);
            tb.Inlines.Add(new InlineUIContainer(emblem));
        }
        static void AddSelfColony()
        {
            int landid = BitConverter.ToInt32(array, i); i += 4;
            Rectangle rect = Common.GetRectangle(20, Links.Brushes.LandIconBrush);
            ShortLandInfo info = new ShortLandInfo(landid, null);
            rect.ToolTip = info.Border;
            tb.Inlines.Add(new InlineUIContainer(rect));
        }
        static void AddPlanet()
        {
            int planetid = BitConverter.ToInt32(array, i); i += 4;
            GSPlanet planet = Links.Planets[planetid];
            Rectangle rect = Common.GetRectangle(20, Links.Brushes.PlanetsBrushes[planet.ImageType]);
            PlanetPanelInfo info = new PlanetPanelInfo(planet, true);
            rect.ToolTip = info;
            tb.Inlines.Add(new InlineUIContainer(rect));
        }
        static void AddTargetColony()
        {
            Rectangle rect = Common.GetRectangle(20, Links.Brushes.LandIconBrush);
            ShortLandInfo info = new ShortLandInfo(array, ref i, false, null);
            rect.ToolTip = info.Border;
            tb.Inlines.Add(new InlineUIContainer(rect));
        }
        static void PutExperience()
        {
            int experience = BitConverter.ToInt32(array, i); i += 4;
            if (experience == 0) return;
            tb.Inlines.Add(new LineBreak());
            tb.Inlines.Add(new Bold(new Run(Links.Interface("NotExp"))));
            Rectangle exprect = Common.GetRectangle(20, Links.Brushes.ExperienceBrush);
            tb.Inlines.Add(new InlineUIContainer(exprect));
            tb.Inlines.Add(new Bold(new Run(" " + experience.ToString() + " ")));
        }
        static void PutReward(Reward2 reward)
        {
            tb.Inlines.Add(new LineBreak());
            //tb.Inlines.Add(new Bold(new Run(Links.Interface("NotReward"))));
            if (reward.Money != 0)
            {
                Rectangle moneyrect = Common.GetRectangle(20, Links.Brushes.MoneyImageBrush);
                tb.Inlines.Add(new InlineUIContainer(moneyrect));
                tb.Inlines.Add(new Bold(new Run(" " + reward.Money.ToString() + " ")));
            }
            if (reward.Metall != 0)
            {
                Rectangle metallrect = Common.GetRectangle(20, Links.Brushes.MetalImageBrush);
                tb.Inlines.Add(new InlineUIContainer(metallrect));
                tb.Inlines.Add(new Bold(new Run(" " + reward.Metall.ToString() + " ")));
            }
            if (reward.Chips != 0)
            {
                Rectangle chipsrect = Common.GetRectangle(20, Links.Brushes.ChipsImageBrush);
                tb.Inlines.Add(new InlineUIContainer(chipsrect));
                tb.Inlines.Add(new Bold(new Run(" " + reward.Chips.ToString() + " ")));
            }
            if (reward.Anti != 0)
            {
                Rectangle antirect = Common.GetRectangle(20, Links.Brushes.AntiImageBrush);
                tb.Inlines.Add(new InlineUIContainer(antirect));
                tb.Inlines.Add(new Bold(new Run(" " + reward.Anti.ToString() + " ")));
            }
            if (reward.Experience != 0)
            {
                Rectangle exprect = Common.GetRectangle(20, Links.Brushes.ExperienceBrush);
                tb.Inlines.Add(new InlineUIContainer(exprect));
                tb.Inlines.Add(new Bold(new Run(" " + reward.Experience.ToString() + " ")));
            }
            if (reward.Sciences!=null && reward.Sciences.Length != 0)
            {
                foreach (ushort scienceid in reward.Sciences)
                {
                    GameScience science = Links.Science.GameSciences[scienceid];
                    tb.Inlines.Add(new LineBreak());
                    tb.Inlines.Add(new Bold(new Run("Получена технология: ")));
                    tb.Inlines.Add(new Bold(new Run(GameObjectName.GetScienceName(science))));
                }
            }
            if (reward.Artefacts!=null && reward.Artefacts.Length!=0)
            {
                foreach (ushort artid in reward.Artefacts)
                {
                    Artefact art = Links.Artefacts[artid];
                    tb.Inlines.Add(new LineBreak());
                    tb.Inlines.Add(new Bold(new Run("Получен артефакт: ")));
                    tb.Inlines.Add(new Bold(new Run(art.GetName())));
                }
            }
            if (reward.BonusShip!=null)
            {
                tb.Inlines.Add(new LineBreak());
                tb.Inlines.Add(new Bold(new Run("Получен корабль: ")));
                ShipB ship = reward.BonusShip.GetShipB();
                tb.Inlines.Add(new Bold(new Run(ship.GetName())));
            }
            if (reward.Pilot!= null)
            {
                tb.Inlines.Add(new LineBreak());
                tb.Inlines.Add(new Bold(new Run("Присоединился пилот: ")));
                Rectangle rating = PilotsImage.GetRatingImage(reward.Pilot.Specialization, reward.Pilot.Talent, reward.Pilot.Rang, 20);
                rating.ToolTip= new PilotsImage(reward.Pilot, PilotsListMode.Ship, null);
                tb.Inlines.Add(new InlineUIContainer(rating));
            }
            if (reward.AvanpostID!=-1)
            {
                GSPlanet planet = Links.Planets[reward.AvanpostID];
                Avanpost avanpost = null;
                foreach (Avanpost avan in GSGameInfo.PlayerAvanposts.Values)
                    if (avan.PlanetID == reward.AvanpostID)
                        avanpost = avan;
                if (avanpost!= null)
                {
                    tb.Inlines.Add(new LineBreak());
                    tb.Inlines.Add(new Bold(new Run(Links.Interface("NotNewAvanpost"))));
                    tb.Inlines.Add(new Bold(new Run(avanpost.Name.ToString())));
                }
            }
            if (reward.LandID!=-1)
            {
                GSPlanet planet = Links.Planets[reward.LandID];
                Land land = null;
                foreach (Land l in GSGameInfo.PlayerLands.Values)
                    if (l.PlanetID == reward.LandID)
                        land = l;
                if (land != null)
                {
                    tb.Inlines.Add(new LineBreak());
                    tb.Inlines.Add(new Bold(new Run(Links.Interface("NotNewLand"))));
                    tb.Inlines.Add(new Bold(new Run(land.Name.ToString())));
                }
            }
        }
        static void PutReward()
        {
            Reward2 reward = new Reward2(array, ref i);
            tb.Inlines.Add(new LineBreak());
            tb.Inlines.Add(new Bold(new Run(Links.Interface("NotReward"))));
            if (reward.Money != 0)
            {
                Rectangle moneyrect = Common.GetRectangle(20, Links.Brushes.MoneyImageBrush);
                tb.Inlines.Add(new InlineUIContainer(moneyrect));
                tb.Inlines.Add(new Bold(new Run(" " + reward.Money.ToString() + " ")));
            }
            if (reward.Metall != 0)
            {
                Rectangle metallrect = Common.GetRectangle(20, Links.Brushes.MetalImageBrush);
                tb.Inlines.Add(new InlineUIContainer(metallrect));
                tb.Inlines.Add(new Bold(new Run(" " + reward.Metall.ToString() + " ")));
            }
            if (reward.Chips != 0)
            {
                Rectangle chipsrect = Common.GetRectangle(20, Links.Brushes.ChipsImageBrush);
                tb.Inlines.Add(new InlineUIContainer(chipsrect));
                tb.Inlines.Add(new Bold(new Run(" " + reward.Chips.ToString() + " ")));
            }
            if (reward.Anti != 0)
            {
                Rectangle antirect = Common.GetRectangle(20, Links.Brushes.AntiImageBrush);
                tb.Inlines.Add(new InlineUIContainer(antirect));
                tb.Inlines.Add(new Bold(new Run(" " + reward.Anti.ToString() + " ")));
            }
            if (reward.Experience != 0)
            {
                Rectangle exprect = Common.GetRectangle(20, Links.Brushes.ExperienceBrush);
                tb.Inlines.Add(new InlineUIContainer(exprect));
                tb.Inlines.Add(new Bold(new Run(" " + reward.Experience.ToString() + " ")));
            }
            if (reward.Sciences.Length != 0)
            {
                foreach (ushort scienceid in reward.Sciences)
                {
                    if (scienceid == 0) continue;
                    GameScience science = Links.Science.GameSciences[scienceid];
                    tb.Inlines.Add(new LineBreak());
                    tb.Inlines.Add(new Bold(new Run(GameObjectName.GetScienceName(science))));
                }
            }
        }
        public static void FillColonizeBattleWin()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Good);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotColonizeBattleWin")));
            AddPlanet();
        }
        public static void FillColonizeBattleLose()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Bad);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotColonizeBattleLose")));
            AddPlanet();
        }
        #endregion
        #region Defense
        public static void FillDefenseSelfStart()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Battle);
            AddFleetEmblem();
            AddBattle();
            tb.Inlines.Add(new Run(Links.Interface("NotDefenseSelfStart")));
            AddSelfColony();
        }
        public static void FillDefenseSelfWin()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Good);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotDefenseSelfWin")));
            AddSelfColony();
            PutReward();
        }
        public static void FillDefenseSelfLose()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Bad);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotDefenseSelfLose")));
            AddSelfColony();
        }
        public static void FillDefenseClanStart()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Battle);
            AddFleetEmblem();
            AddBattle();
            tb.Inlines.Add(new Run(Links.Interface("NotDefenseClanStart")));
            AddTargetColony();
        }
        public static void FillDefenseClanWin()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Good);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotDefenseClanWin")));
            AddTargetColony();
            PutReward();
        }
        public static void FillDefenseClanLose()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Bad);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotDefenseClanLose")));
            AddTargetColony();
        }
        public static void FillDefenseByClanStart()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Battle);
            tb.Inlines.Add(new Run(Links.Interface("NotDefenseByClanStart")));
            AddBattle();
            AddSelfColony();
        }
        public static void FillDefenseByClanWin()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Good);
            tb.Inlines.Add(new Run(Links.Interface("NotDefenseByClanWin")));
            AddSelfColony();
        }
        public static void FillDefenseByClanLose()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Bad);
            tb.Inlines.Add(new Run(Links.Interface("NotDefenseByClanLose")));
            AddSelfColony();
        }
        #endregion
        #region Pillage
        public static void FillPillageAttackStart()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Battle);
            AddFleetEmblem();
            AddBattle();
            tb.Inlines.Add(new Run(Links.Interface("NotPillageAttackStart")));
            AddTargetColony();
        }
        public static void FillPillageAttackWinPillage()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Good);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotPillageAttackWinPillage")));
            AddTargetColony();
            PutReward();
        }
        public static void FillPillageAttackWinNext()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Battle);
            AddFleetEmblem();
            AddBattle();
            tb.Inlines.Add(new Run(Links.Interface("NotPillageAttackWinNext")));
            AddTargetColony();
            PutExperience();
        }
        public static void FillPillageAttackWinReturn()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Good);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotPillageAttackWinReturn")));
            AddTargetColony();
            PutExperience();
        }
        public static void FillPillageAttackLose()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Bad);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotPillageAttackLose")));
            AddTargetColony();
        }
        public static void FillPillageNoDefenseWin()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Good);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotPillageNoDefenseWin")));
            AddTargetColony();
            PutReward();
        }
        public static void FillPilageNoDefenseLose()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Bad);
            AddSelfColony();
            tb.Inlines.Add(new Run(Links.Interface("NotPillageNoDefenseLose")));
            PutReward();
        }
        #endregion
        #region ColonizeLandFree
        public static void FillColonyNewLandWin()
        {
            tb = GetBasicTextBlock(NoticeEmotion.ImportantWin);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotColonyNewLandWin")));
            AddSelfColony();
        }
        public static void FillColonyNewLandLose()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Bad);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotColonyNewLandLose")));
            AddPlanet();
        }
        #endregion
        #region ColonizeEnemy
        public static void FillColonyAttackStart()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Battle);
            AddFleetEmblem();
            AddBattle();
            tb.Inlines.Add(new Run(Links.Interface("NotColonyAttackStart")));
            AddTargetColony();
        }
        public static void FillColonyAttackWinReturn()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Good);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotColonyAttackWinReturn")));
            AddTargetColony();
            PutExperience();
        }
        public static void FillColonyAttackWinColony()
        {
            tb = GetBasicTextBlock(NoticeEmotion.ImportantWin);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotColonyAttackWinColony")));
            AddSelfColony();
            PutReward();
        }
        public static void FillColonyAttackWinFail()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Good);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotColonyAttackWinFail")));
            AddTargetColony();
            PutExperience();
        }
        public static void FillColonyAttackWinLastFail()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Good);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotColonyAttackWinLastFail")));
            AddTargetColony();
            PutExperience();
        }
        public static void FillColonyAttackWinLast()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Battle);
            AddFleetEmblem();
            AddBattle();
            tb.Inlines.Add(new Run(Links.Interface("NotColonyAttackWinLast")));
            AddTargetColony();
            PutExperience();
        }
        public static void FillColonyAttackWinNext()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Battle);
            AddFleetEmblem();
            AddBattle();
            tb.Inlines.Add(new Run(Links.Interface("NotColonyAttackWinNext")));
            AddTargetColony();
            PutExperience();
        }
        public static void FillColonyAttackWinUprise()
        {
            tb = GetBasicTextBlock(NoticeEmotion.ImportantWin);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotColonyAttackWinUprise")));
            AddTargetColony();
            PutExperience();
        }
        public static void FillColonyAttackWinNoUprise()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Good);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotColonyAttackWinNoUprise")));
            AddTargetColony();
            PutExperience();
        }
        public static void FillColonyAttackLose()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Bad);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotColonyAttackLose")));
            AddTargetColony();
        }
        public static void FillColonyLastAttackFailStart()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Neutral);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotColonyLastAttackFailStart")));
            AddTargetColony();
        }
        public static void FillColonyLastAttackStart()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Battle);
            AddFleetEmblem();
            AddBattle();
            tb.Inlines.Add(new Run(Links.Interface("NotColonyLastAttackStart")));
            AddTargetColony();
        }
        public static void FillColonyLastAttackWinColony()
        {
            tb = GetBasicTextBlock(NoticeEmotion.ImportantWin);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotColonyLastAttackWinColony")));
            AddSelfColony();
            PutReward();
        }
        public static void FillColonyLastAttackFail()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Good);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotColonyLastAttackFail")));
            AddTargetColony();
            PutExperience();
        }
        public static void FillColonyLastAttackWinUprise()
        {
            tb = GetBasicTextBlock(NoticeEmotion.ImportantWin);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotColonyLastAttackWinUprise")));
            AddTargetColony();
            PutExperience();
        }
        public static void FillColonyLastAttackWinNoUprise()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Good);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotColonyLastAttackWinNoUprise")));
            AddTargetColony();
            PutExperience();
        }
        public static void FillColonyLastAttackLose()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Bad);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotColonyLastAttackLose")));
            AddTargetColony();
        }
        public static void FillColonyNoDefenseColonyWin()
        {
            tb = GetBasicTextBlock(NoticeEmotion.ImportantWin);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotColonyNoDefenseColonyWin")));
            AddSelfColony();
            PutReward();
        }
        public static void FillColonyNoDefenseFail()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Neutral);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotColonyNoDefenseFail")));
            AddTargetColony();
        }
        public static void FillColonyNoDefenseColonyLose()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Bad);
            AddTargetColony();
            tb.Inlines.Add(new Run(Links.Interface("NotColonyNoDefenseColonyLose")));
            PutReward();
        }
        public static void FillColonyNoDefenseUpriseWin()
        {
            tb = GetBasicTextBlock(NoticeEmotion.ImportantWin);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotColonyNoDefenseUpriseWin")));
            AddTargetColony();
        }
        public static void FillColonyNoDefenseUpriseLose()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Bad);
            AddSelfColony();
            tb.Inlines.Add(new Run(Links.Interface("NotColonyNoDefenseUpriseLose")));
        }
        public static void FillColonyNoDefenseNoUprise()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Neutral);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotColonyNoDefenseNoUprise")));
            AddTargetColony();

        }
        public static void FillColonyNewLose()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Bad);
            AddTargetColony();
            tb.Inlines.Add(new Run(Links.Interface("NotColonyNewLose")));
        }
        public static void FillColonyNewDestroyed()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Good);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotColonyNewDestroyed")));
            AddTargetColony();
        }
        public static void FillLastDefenseStart()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Battle);
            AddFleetEmblem();
            AddBattle();
            tb.Inlines.Add(new Run(Links.Interface("NotLastDefenseStart")));
            AddSelfColony();
        }
        public static void FillLastDefenseWin()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Good);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotLastDefenseWin")));
            PutReward();
        }
        public static void FillLastDefenseLoseColony()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Bad);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotLastDefenseLoseColony")));
            AddTargetColony();
            PutReward();
        }
        public static void FillLastDefenseLoseNoColony()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Bad);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotLastDefenseLoseNoColony")));
            AddSelfColony();
        }
        public static void FillLastDefenseLoseUprise()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Bad);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotLastDefenseLoseUprise")));
            AddSelfColony();
        }
        public static void FillLastDefenseLoseNoUprise()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Bad);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotLastDefenseLoseNoUprise")));
            AddSelfColony();
        }
        #endregion
        #region Resource
        public static void FillPirateBattleStart()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Battle);
            AddFleetEmblem();
            AddBattle();
            tb.Inlines.Add(new Run(Links.Interface("NotPirateBattleStart")));
        }
        public static void FillPirateBattleWin()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Good);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotPirateBattleWin")));
            PutReward();
        }
        public static void FillPirateBattleLose()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Bad);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotPirateBattleLose")));
        }
        public static void FillGreenTeamBattleStart()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Battle);
            AddFleetEmblem();
            AddBattle();
            tb.Inlines.Add(new Run(Links.Interface("NotGreenTeamBattleStart")));
        }
        public static void FillGreenTeamBattleWin()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Good);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotGreenTeamBattleWin")));
            PutReward();
        }
        public static void FillGreenTeamBattleLose()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Bad);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotGreenTeamBattleLose")));
        }
        public static void FillAlienBattleStart()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Battle);
            AddFleetEmblem();
            AddBattle();
            tb.Inlines.Add(new Run(Links.Interface("NotAlienBattleStart")));
        }
        public static void FillAlienBattleWin()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Good);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotAlienBattleWin")));
            PutReward();
        }
        public static void FillAlienBattleLose()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Bad);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotAlienBattleLose")));
        }
        public static void FillTechTeamBattleStart()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Battle);
            AddFleetEmblem();
            AddBattle();
            tb.Inlines.Add(new Run(Links.Interface("NotTechTeamBattleStart")));
        }
        public static void FilltechTeamBattleWin()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Good);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotTechTeamBattleWin")));
            PutReward();
        }
        public static void FillTechTeamBattleLose()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Bad);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotTechTeamBattleLose")));
        }
        public static void FillMercBattleStart()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Battle);
            AddFleetEmblem();
            AddBattle();
            tb.Inlines.Add(new Run(Links.Interface("NotMercBattleStart")));
        }
        public static void FillMercBattleWin()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Good);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotMercBattleWin")));
            PutReward();
        }
        public static void FillMercBattleLose()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Bad);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotMercBattleLose")));
        }
        #endregion
        #region StoryLine
        public static void FillStoryLinekStart()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Battle);
            AddFleetEmblem();
            AddBattle();
            byte storyID = array[i]; i++;
            StoryLine2 story = StoryLine2.StoryLines[storyID];
            tb.Inlines.Add(new Run(Links.Interface("NotStoryLineStart")));
            tb.Inlines.Add(new Run(story.Title));
        }
        public static void FillStoryLinekWin()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Good);
            AddFleetEmblem();
            byte StoryID = array[i]; i++;
            StoryLine2 story = StoryLine2.StoryLines[StoryID];
            tb.Inlines.Add(new Run(Links.Interface("NotStoryLineWin")));
            tb.Inlines.Add(new Run(story.Title));
            Reward2 reward = new Reward2(array, ref i);
            PutReward(reward);
        }
        public static void FillStoryLineLose()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Bad);
            AddFleetEmblem();
            byte StoryID = array[i]; i++;
            StoryLine2 story = StoryLine2.StoryLines[StoryID];
            tb.Inlines.Add(new Run(Links.Interface("NotStoryLineLose")));
            tb.Inlines.Add(new Run(story.Title));
        }
        
        #endregion
        #region Others
        public static void FillTargetColonyOwnerChanged()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Neutral);
            AddFleetEmblem();
            tb.Inlines.Add(new Run(Links.Interface("NotTargetColonyOwnerChanged")));
            AddTargetColony();
        }
        public static void FillLearnNewScience()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Science);
            ushort scienceid = BitConverter.ToUInt16(array, i); i += 2;
            tb.Inlines.Add(new Run(Links.Interface("NotLearnNewScience")));
            GameScience science = Links.Science.GameSciences[scienceid];
            //tb.Inlines.Add(new LineBreak());
            tb.Inlines.Add(new Bold(new Run(GameObjectName.GetScienceName(science))));
        }
        public static void FillAddPremiumCurrency()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Premium);
            int value = BitConverter.ToInt32(array, i); i += 4;
            tb.Inlines.Add(new Run(Links.Interface("NotAddPremiumCurrency")));
            tb.Inlines.Add(new InlineUIContainer(new Premium_Coin(50, 50, value)));
        }
        public static void FillActivatePremium()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Premium);
            int days = BitConverter.ToInt32(array, i); i += 4;
            tb.Inlines.Add(new Run(string.Format(Links.Interface("NotActivatePremium"), days)));
        }
        public static void FillQuickStartPremium()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Premium);
            int sciences = BitConverter.ToInt32(array, i); i += 4;
            tb.Inlines.Add(new Bold(new Run(string.Format(Links.Interface("NotQuickStartPremium"), sciences))));

        }
        public static void FillOrionPremium()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Premium);
            AddSelfColony();
            tb.Inlines.Add(new Run(Links.Interface("NotOrionPremium")));
        }
        public static void FillLoseLandByEnemy()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Bad);
            tb.Inlines.Add(new Run(Links.Interface("NotLoseLandByEnemy")));
            AddTargetColony();
        }
        public static void FillRecieveReward()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Science);
            Reward2 reward = new Reward2(array, ref i);
            tb.Inlines.Add(new Run(Links.Interface("NotRecieveReward")));
            PutReward(reward);
        }
        public static void FillRecieveMission2Reward()
        {
            tb = GetBasicTextBlock(NoticeEmotion.Science);
            Mission2Type type =(Mission2Type)array[i]; i++;
            Reward2 reward = new Reward2(array, ref i);
            switch (type)
            {
                case Mission2Type.Metheorit: tb.Inlines.Add(new Run(Links.Interface("NotRecieveRewardMeteorit"))); break;
                case Mission2Type.OreBelt: tb.Inlines.Add(new Run(Links.Interface("NotRecieveRewardOreBelt"))); break;
                case Mission2Type.Anomaly: tb.Inlines.Add(new Run(Links.Interface("NotRecieveRewardAnomaly"))); break;
            }
            PutReward(reward);
        }
        #endregion
        enum NoticeEmotion{Good, Bad, Neutral, ImportantWin, Battle, Science, Premium}
        static LinearGradientBrush BadMessageBrush = Common.GetLinearBrush(Color.FromRgb(255, 100, 100),Colors.White,Color.FromRgb(255,100,100));
        static LinearGradientBrush GoodMessageBrush = Common.GetLinearBrush(Color.FromRgb(100, 255, 100), Colors.White, Color.FromRgb(100, 255, 100));
        static LinearGradientBrush NeutralMessageBrush = Common.GetLinearBrush(Colors.Silver, Colors.White, Colors.Silver);
        static LinearGradientBrush ImportantMessageBrush = Common.GetLinearBrush(Colors.Gold, Colors.White, Colors.Gold);
        static LinearGradientBrush ScienceMessageBrush = Common.GetLinearBrush(Colors.SkyBlue, Colors.White, Colors.SkyBlue);
        static LinearGradientBrush PremiumMessageBrush = Common.GetLinearBrush(Colors.Gold, Colors.SkyBlue, Colors.Gold);
        static LinearGradientBrush BattleMessageBrush = GetBattleMessageBrush();
        static TextBlock GetBasicTextBlock(NoticeEmotion emotion)
        {
            TextBlock tb = new TextBlock();
            tb.FontFamily = Links.Font;
            tb.FontSize = 20;
            tb.TextWrapping = TextWrapping.Wrap;
            AddTime(tb);
            switch (emotion)
            {
                case NoticeEmotion.Good: tb.Background = GoodMessageBrush; 
                    tb.Inlines.Add(new Bold(new Run(Links.Interface("NotVictory"))));
                    tb.Inlines.Add(new LineBreak());
                    break;
                case NoticeEmotion.Bad: tb.Background = BadMessageBrush; 
                    tb.Inlines.Add(new Bold(new Run(Links.Interface("NotLose"))));
                    tb.Inlines.Add(new LineBreak());
                    break;
                case NoticeEmotion.ImportantWin: tb.Background = ImportantMessageBrush; 
                    tb.Inlines.Add(new Bold(new Run(Links.Interface("NotVictory"))));
                    tb.Inlines.Add(new LineBreak());
                    break;
                case NoticeEmotion.Neutral: tb.Background = NeutralMessageBrush; break;
                case NoticeEmotion.Battle: tb.Background = BattleMessageBrush; 
                    tb.Inlines.Add(new Bold(new Run(Links.Interface("NotBattle"))));
                    tb.Inlines.Add(new LineBreak());
                    break;
                case NoticeEmotion.Science: tb.Background = ScienceMessageBrush;
                    tb.Inlines.Add(new Bold(new Run(Links.Interface("NotScience"))));
                    tb.Inlines.Add(new LineBreak());
                    break;
                case NoticeEmotion.Premium: tb.Background = PremiumMessageBrush;
                    tb.Inlines.Add(new Bold(new Run(Links.Interface("NotPremium"))));
                    tb.Inlines.Add(new LineBreak());
                    break;
            }
            

            return tb;
        }
       public static LinearGradientBrush GetBattleMessageBrush()
        {
            LinearGradientBrush brush = new LinearGradientBrush();
            GradientStop gr1 = new GradientStop(Colors.Black, 0);
            GradientStop gr2 = new GradientStop(Colors.Black, 1);
            brush.GradientStops.Add(gr1);
            brush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
            brush.GradientStops.Add(gr2);
            ColorAnimation anim = new ColorAnimation(Colors.Black, Colors.White, TimeSpan.FromSeconds(1));
            anim.AutoReverse = true;
            anim.RepeatBehavior = RepeatBehavior.Forever;
            gr1.BeginAnimation(GradientStop.ColorProperty, anim);
            gr2.BeginAnimation(GradientStop.ColorProperty,anim);
            return brush;
        }
       public static void AddTime(TextBlock tb)
       {
           if (dt.Date != DateTime.Now.Date)
               tb.Inlines.Add(new Bold(new Run(string.Format(" {0}/{1}/{2} {3}:{4:00}:{5:00} ", Links.Lang == 0 ? dt.Month : dt.Day, Links.Lang == 0 ? dt.Day : dt.Month, dt.Year, dt.Hour, dt.Minute, dt.Second))));
           else
               tb.Inlines.Add(new Bold(new Run(string.Format(" {0}:{1:00}:{2:00} ", dt.Hour, dt.Minute, dt.Second))));
       }
       
        public static DateTime GetTime(long ticks)
        {
            DateTime currtime = new DateTime(ticks);
            DateTime time = (currtime - (GSGameInfo.ServerTime - GSGameInfo.UpdateTime));
            return time;
        }
      
    }
}
