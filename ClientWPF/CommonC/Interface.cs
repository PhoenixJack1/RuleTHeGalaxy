using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    class Interface
    {
        public static void FillInterface()
        {
            Links.InterfaceText = new SortedList<string, string[]>();
            AddText("GameName", "Rule The Galaxy", "Управляй галактикой");//0
            AddText("Global", "Nation", "Государство");
            AddText("Galaxy", "Galaxy", "Галактика");//1
            AddText("Sector", "Sector", "Сектор");//2
            AddText("System", "System", "Система");//3
            AddText("Land", "Land", "Колония");//4
            AddText("Science", "Science", "Наука");//5
            AddText("Market", "Market", "Рынок");//6
            AddText("Clan", "Alliance", "Альянс");
            AddText("Fleet", "Fleet", "Флот"); //7
            AddText("Ships", "Ships", "Корабли");
            AddText("Schemas", "Schemas", "Чертежи");//8
            AddText("Chat", "Chat", "Чат"); //9
            AddText("Ok", "Ok", "Ок"); //10
            AddText("Cancel", "Cancel", "Отмена"); //11
            AddText("Close", "Close", "Закрыть");
            AddText("ResourceMarket", "Resource Market", "Обменник ресурсов");//12
            AddText("Buy", "Buy", "Покупка"); //13
            AddText("Sell", "Sell", "Продажа"); //14
            AddText("All", "All", "Все");//15
            AddText("No", "No", "Нет");
            AddText("Exit", "Exit", "Выход");
            AddText("ParametersB", "Parameters", "Параметры");
            AddText("Basic", "Basic", "Базовые");
            AddText("Improve", "Improve", "Улучшенные");
            AddText("Expert", "Expert", "Экспертные");
            AddText("Change", "Exchange", "Обменять"); //16
            AddText("Clear", "Clear", "Очистить"); //17
            AddText("Send", "Send", "Отправить");
            AddText("Academy", "Academy", "Академия");//18
            AddText("MoneyNeed", "You need {0} credits", "Вам нужно {0} кредитов");//19
            AddText("MoneyToMetal", "You changed {0} money for {1} units of metal", "Вы обменяли {0} кредитов на {1} единиц металлов");//20
            AddText("MetalNeed", "You need {0} units of metal", "Вам нужно {0} единиц металлов");//21
            AddText("MoneyRecieve", "You recieve {0} credits", "Вы получили {0} кредитов"); //22
            AddText("MoneyToChips", "You changed {0} credits for {1} unit of chip", "Вы обменяли {0} кредитов на {1} единиц микросхем");//23
            AddText("ChipsNeed", "You need {0} units of chip", "Вам нужно {0} единиц микросхем");//24
            AddText("MoneyToAnti", "You changed {0} credits for {1} unit of antimaterial", "Вы обменяли {0} кредитов на {1} единиц антиматерии");//25
            AddText("AntiNeed", "You need {0} units of antimaterial", "Вам нужно {0} единиц антиматерии");//26
            AddText("MoneyToZip", "You changed {0} credits for {1} zip parts", "Вы обменяли {0} кредитов на {1} запасных частей");//27
            AddText("ZipNeed", "You need {0} zip parts", "Вам нужно {0} запасных частей");//28
            AddText("Russia", "Russia", "Россия"); //29
            AddText("Japan", "Japan", "Япония");//30
            AddText("American", "USA", "США");
            AddText("Man", "Man", "Мужской");//31
            AddText("Female", "Female", "Женский");//32
            AddText("Age", "Age", "Возраст");//33
            AddText("NewRelease", "New Release", "Новый выпуск"); //34
            AddText("Hull", "Hull value", "Прочность корпуса");//35
            AddText("Hull2", "Hull", "Корпус");
            AddText("HullRestore", "Hull restore", "Восстановление корпуса");//36
            AddText("HullGroup", "Hull value increase for group", "Увеличение прочности корпуса для группы");
            AddText("HullRestoreGroup", "Group hull restore", "Восстановление корпуса для группы"); //37
            AddText("Shield", "Shield force", "Сила щита");//38
            AddText("ShieldGroup", "Shield force increase for group", "Увеличение силы щита для группы");
            AddText("ShieldRestore", "Shield restore", "Восстановление шита");//39
            AddText("ShieldRestoreGroup", "Group shield restore", "Восстановление щита для группы");//40
            AddText("Energy", "Energy capacity", "Запас энергии генератора");//41
            AddText("Energy2", "Energy", "Энергия");
            AddText("EnergyGroup", "Energy capacity increase for group", "Увеличение запаса энергии для группы");
            AddText("EnergyRestore", "Energy generation", "Генерация энергии");//42
            AddText("EnergyRestoreGroup", "Group energy generation", "Генерация энергии для группы"); //43
            AddText("EnergyConsume", "Energy consume", "Расход энергии");
            AddText("EnAcc", "Energy weapon accuracy", "Точность энергетического оружия");//44
            AddText("PhAcc", "Physic weapon accuracy", "Точность физического оружия");//45
            AddText("IrAcc", "Irregular weapon accuracy", "Точность аномального оружия");//46
            AddText("CyAcc", "Cyber weapon accuracy", "Точность кибернетического оружия");//47
            AddText("AlAcc", "All weapon accuracy", "Точность любого оружия");//48
            AddText("round", "round", "ход");
            AddText("Move", "Move", "Движение");
            AddText("WeaponGroup", "Target group", "Область действия");
            AddText("EnAccGr", "Energy weapon accuracy for group", "Точность энергетического оружия для группы");
            AddText("PhAccGr", "Physic weapon accuracy for group", "Точность физического оружия для группы");
            AddText("IrAccGr", "Irregular weapon accuracy for group", "Точность аномального оружия для группы");
            AddText("CyAccGr", "Cyber weapon accuracy for group", "Точность кибернетического оружия для группы");
            AddText("AlAccGr", "All weapon accuracy for group", "Точность любого оружия для группы");
            AddText("EnEva", "Energy weapon evasion", "Уклонение от энергетического оружия");
            AddText("PhEva", "Physic weapon evasion", "Уклонение от физического оружия");
            AddText("IrEva", "Irregular weapon evasion", "Уклонение от аномального оружия");
            AddText("CyEva", "Cyber weapon evasion", "Уклонение от кибернетического оружия");
            AddText("AlEva", "All weapon evasion", "Уклонение от любого оружия");
            AddText("EnEvaGr", "Energy weapon evasion for group", "Уклонение от энергетического оружия для группы");
            AddText("PhEvaGr", "Physic weapon evasion for group", "Уклонение от физического оружия для группы");
            AddText("IrEvaGr", "Irregular weapon evasion for group", "Уклонение от аномального оружия для группы");
            AddText("CyEvaGr", "Cyber weapon evasion for group", "Уклонение от кибернетического оружия для группы");
            AddText("AlEvaGr", "All weapon evasion for group", "Уклонение от любого оружия для группы");
            AddText("EnDam", "Energy weapon damage increase", "Усиление урона от энергетического оружия");
            AddText("PhDam", "Physic weapon damage increase", "Усиление урона от физического оружия");
            AddText("IrDam", "Irregular weapon damage increase", "Усиление урона от аномального оружия");
            AddText("CyDam", "Cyber weapon damage increase", "Усиление урона от кибернетического оружия");
            AddText("AlDam", "All weapon damage increase", "Усиление урона от любого оружия");
            AddText("EnDamGr", "Energy weapon damage increase for group", "Усиление урона от энергетического оружия для группы");
            AddText("PhDamGr", "Physic weapon damage increase for group", "Усиление урона от физического оружия для группы");
            AddText("IrDamGr", "Irregular weapon damage increase for group", "Усиление урона от аномального оружия для группы");
            AddText("CyDamGr", "Cyber weapon damage increase for group", "Усиление урона от кибернетического оружия для группы");
            AddText("AlDamGr", "All weapon damage increase for group", "Усиление урона от любого оружия для группы");
            AddText("EnIgn", "Energy weapon damage absorption", "Поглощение урона от энергетического оружия");
            AddText("PhIgn", "Physic weapon damage absorption", "Поглощение урона от физического оружия");
            AddText("IrIgn", "Irregular weapon damage absorption", "Поглощение урона от аномального оружия");
            AddText("CyIgn", "Cyber weapon damage absorption", "Поглощение урона от кибернетического оружия");
            AddText("AlIgn", "All weapon damage absorption", "Поглощение урона от любого оружия");
            AddText("EnIgnGr", "Energy weapon damage absorption for group", "Поглощение урона от энергетического оружия для группы");
            AddText("PhIgnGr", "Physic weapon damage absorption for group", "Поглощение урона от физического оружия для группы");
            AddText("IrIgnGr", "Irregular weapon damage absorption for group", "Поглощение урона от аномального оружия для группы");
            AddText("CyIgnGr", "Cyber weapon damage absorption for group", "Поглощение урона от кибернетического оружия для группы");
            AddText("AlIgnGr", "All weapon damage absorption for group", "Поглощение урона от любого оружия для группы");
            AddText("EnImm", "Energy weapon effects immune", "Иммунитет к эффектам от энергетического оружия");
            AddText("PhImm", "Physic weapon effects immune", "Иммунитет к эффектам от физического оружия");
            AddText("IrImm", "Irregular weapon effects immune", "Иммунитет к эффектам от аномального оружия");
            AddText("CyImm", "Cyber weapon effects immune", "Иммунитет к эффектам от кибернетического оружия");
            AddText("AlImm", "All weapon effects immune", "Иммунитет к эффектам от любого оружия");
            AddText("EnImmGr", "Energy weapon effects immune for group", "Иммунитет к эффектам от энергетического оружия для группы");
            AddText("PhImmGr", "Physic weapon effects immune for group", "Иммунитет к эффектам от физического оружия для группы");
            AddText("IrImmGr", "Irregular weapon effects immune for group", "Иммунитет к эффектам от аномального оружия для группы");
            AddText("CyImmGr", "Cyber weapon effects immune for group", "Иммунитет к эффектам от кибернетического оружия для группы");
            AddText("Cargo", "Cargo value", "Объем грузового отделения");
            AddText("Colony", "Colony size", "Размер колониального отделения");
            AddText("Hire", "Hire", "Нанять");
            AddText("HireResult", "Pilot {0} \"{1}\" {2} has hired", "Пилот {0} \"{1}\" {2} нанят");
            AddText("Level", "Level", "Уровень");
            AddText("Barracks", "Barracks", "Казармы");
            AddText("AllPilots", "Show all pilots", "Показывать всех пилотов");
            AddText("OnlyFreePilots", "Show free pilots only", "Показывать только свободных пилотов");
            AddText("OnlyBusyPilots", "Show busy pilots only", "Показывать только пилотов в кораблях");
            AddText("Dismiss", "Fire", "Уволить");
            AddText("Educate", "Educate", "Обучить");
            AddText("NotAvailable", "Weapon not available", "Оружие не предусмотрено");
            AddText("NotPlaced", "Weapon not placed", "Оружие не установлено");
            AddText("Laser", "Laser gun", "Лазерная установка");
            AddText("EMI", "Generator of electromagnetic pulse", "Генератор электромагнитного импульса");
            AddText("Plasma", "Plasma gun", "Плазменное орудие");
            AddText("Solar", "Solar gun", "Солнечная пушка");
            AddText("Cannon", "Large-caliber gun", "Крупнокалиберная пушка");
            AddText("Gauss", "Gauss gun", "Установка гаусса");
            AddText("Missle", "Rocket launcher", "Ракетная установка");
            AddText("Antimatter", "Antimatter generator", "Генератор антиматерии");
            AddText("DarkEnergy", "Dark energy generator", "Генератор тёмной энергии");
            AddText("Warp", "Black hole generator", "Генератор чёрных дыр");
            AddText("Psi", "Psi weapon", "Пси оружие");
            AddText("Time", "Time paradox generator", "Генератор временных парадоксов");
            AddText("Slice", "Slice system", "Система компьютерного проникновения");
            AddText("Radiation", "Radioactive influence generator", "Генератор радиоактивного излучения");
            AddText("Drone", "Unmanned strike unit", "Беспилотные ударные аппараты");
            AddText("Magnet", "Magnet gun", "Магнитная пушка");
            AddText("ShieldDamage", "Damage to shield", "Урон по щиту");
            AddText("HealthDamage", "Damage to hull", "Урон по корпусу");
            AddText("EnergySpent", "Energy consumtion per shot", "Расход энергии на выстрел");
            AddText("NoPilot", "No pilot", "Нет пилота");
            AddText("NoFleet", "No fleet", "Нет флота");
            AddText("NewFleet", "New Fleet", "Создать флот");
            AddText("FleetEmblem", "Select fleet emblem", "Выберите эмблему флота");
            AddText("FleetCreated", "Fleet was created", "Флот создан");
            AddText("ReformFleet", "Change Ships", "Поменять корабли");
            AddText("ChangeFleetBehavior", "Change Behavior", "Сменить поведение");
            AddText("DeleteFleet", "Delete Fleet", "Расформировать флот");
            AddText("SendFleetToEnemy", "Stop Invasion", "Остановить вторжение");
            AddText("SendFleetToRecon", "Send Fleet to Recognize", "Послать флот на разведку");
            AddText("SendFleetForRes", "Send Fleet for Resources", "Послать флот за ресурсами");
            AddText("SendFleetToFight", "Send Fleet to Conquer", "Послать флот на захват");
            AddText("SendFleetToDef", "Send Fleet to Defence", "Послать флот на защиту");
            AddText("SelectShips", "Select ships for Fleet", "Выберите корабли для флота");
            AddText("NoAvailableShips", "You don`t available ship", "У вас нет подходящих кораблей");
            AddText("PutPilot", "Put pilot", "Посадить пилота");
            AddText("RemovePilot", "Remove Pilot", "Высадить пилота");
            AddText("NoFreePilots", "No Free Pilots", "Нет свободных пилотов");
            AddText("SendFleetToTest", "Send Fleet To Test Battle", "Послать флот на тестовую битву");
            AddText("AverageShipHealth", "Average Ship Health", "Среднее здоровье кораблей");
            AddText("FleetBehavior", "Current health when ship leave battle", "Текущее здоровье корабля, при котором он покидает бой");
            AddText("FleetInScout", "Patrol", "Патрулирует");
            AddText("FleetInBattle", "Fleet in battle", "Флот в бою");
            AddText("FleetBusy", "Fleet is busy", "Флот занят");
            AddText("ShipBusy", "Ship is busy", "Корабль занят");
            AddText("MinFleetsShips", "Too few ships in the fleet", "Слишком мало кораблей во флоте");
            AddText("MaxLandShips", "Too much ships in land", "Слишком много кораблей в колонии");
            AddText("MaxFleetShips", "Too much ships in fleet", "Слишком много кораблей фо флоте");
            AddText("FleetInDefense", "Fleet in Defence", "Флот в охранении");
            AddText("EnterInBattle", "Enter in the battle", "Посмотреть бой");
            AddText("BattleNotFinded", "Sorry, battle was not finded", "Извините, бой не найден");
            AddText("FleetNotReady", "Sorry, your fleet is not ready", "Извините, ваш флот не готов");
            AddText("LaserCrit", "Crit: Blinding", "Крит: Ослепление");
            AddText("EMICrit", "Crit: Discharge", "Крит: Разрядка");
            AddText("PlasmaCrit", "Crit: Damage X2", "Крит: Урон Х2");
            AddText("SolarCrit", "Crit: Vaporization", "Крит: Испарение");
            AddText("CannonCrit", "Crit: Contusion", "Крит: Контузия");
            AddText("GaussCrit", "Crit: Penetration", "Крит: Проникновение");
            AddText("MissleCrit", "Crit: Splinters", "Крит: Осколки");
            AddText("AntiCrit", "Crit: Annihilation", "Крит: Аннигиляция");
            AddText("PsiCrit", "Crit: Disturbance", "Крит: Помрачение");
            AddText("DarkCrit", "Crit: Overload", "Крит: Перегрузка");
            AddText("WarpCrit", "Crit: Portal", "Крит: Портал");
            AddText("TimeCrit", "Crit: Paradox", "Крит: Парадокс");
            AddText("SliceCrit", "Crit: Block", "Крит: Блокировка");
            AddText("DronCrit", "Crit: Sabotage", "Крит: Саботаж");
            AddText("RadCrit", "Crit: Radiation", "Крит: Радиация");
            AddText("MagnetCrit", "Crit: Reverse", "Крит: Разворот");
            AddText("BuildBuilding", "Build", "Построить");
            AddText("mln", "b.", "млн.");
            AddText("NoEnviroment", "The planet is not suitable for colonization", "Планета не подходит для колонизации");
            AddText("UnknownPlanet", "Information about the planet is not available. Send a fleet to patrol the system to obtain information.", "Информация о планете не доступна. Отправьте флот на патрулирование системы, чтобы получить информацию.");
            AddText("BuildingDestroy", "Destroy", "Разрушить");
            AddText("BuildingUpgrade", "Upgrade", "Улучшить");
            AddText("NoMoney", "Not enough money", "Недостаточно денег");
            AddText("NoMetal", "Not enough metal", "Недостаточно металлов");
            AddText("NoChip", "Not enough chips", "Недостаточно микросхем");
            AddText("NoAnti", "Not enough antimatter", "Недостаточно антиматерии");
            AddText("NoZip", "Not enough zip parts", "Недостаточно запасных частей");
            AddText("ReplaceSector", "Replace", "Переместить");
            AddText("Small", "Small size", "Малый размер");
            AddText("Medium", "Medium size", "Средний размер");
            AddText("Large", "Large size", "Крупный размер");
            AddText("CritChance", "Critical hit chance", "Шанс критического удара");
            AddText("Generator", "Energy generator", "Генератор энергии");
            AddText("ShipShield", "Shield generator", "Генератор силового щита");
            AddText("Engine", "Ship engine", "Двигатель корабля");
            AddText("Computer", "Ship computer", "Компьютер корабля");
            AddText("EnergyWeapon", "Energy weapon", "Энергетическое оружие");
            AddText("PhysicWeapon", "Physical weapon", "Физическое оружие");
            AddText("IrregularWeapon", "Irregular weapon", "Аномальное оружие");
            AddText("CyberWeapon", "Cybernetic weapon", "Кибернетическое оружие");
            AddText("AllWeapon", "All group weapon", "Оружие любой группы");
            AddText("Equipment", "Equipment", "Дополнительное оборудование");
            AddText("ShipTypeNotSelected", "Ship type not selected", "Тип корабля не выбран");
            AddText("SelectShipType", "Select ship type", "Выберите тип корабля");
            AddText("SelectWeapon", "Select weapon", "Выберите вооружение");
            AddText("SelectGenerator", "Select power generator", "Выберите генератор энергии");
            AddText("SelectShield", "Select force shield generator", "Выберите генератор силового щита");
            AddText("SelectComputer", "Select ship aim computer", "Выберите корабельный вычислитель");
            AddText("SelectEngine", "Select ship engine", "Выберите корабельный двигатель");
            AddText("SelectEquipment", "Select addition equipment", "Выберите дополнительное оборудование");
            AddText("Title", "Title", "Наименование");
            AddText("Type", "Type", "Тип");
            AddText("Size", "Size", "Размер");
            AddText("Accuracy", "Accuracy", "Точность");
            AddText("AccuracyBonus", "Accuracy bonus", "Бонус к точности");
            AddText("DamageBonus", "Damage bonus", "Бонус к урону");
            AddText("Effect", "Effect", "Эффект");
            AddText("SchemaName", "Schema name", "Имя схемы");
            AddText("Save", "Save", "Сохранить");
            AddText("SchemaError", "Schema is incorrect", "Схема неверна");
            AddText("Delete", "Dalete", "Удалить");
            AddText("CreateShip", "Build ship", "Построить корабль");
            AddText("SelectShipModel", "Select model for ship", "Выберите модель корабля");
            AddText("HangarFull", "Hangars are full", "Нет места в ангарах");
            AddText("WarpHealth", "Warp armor", "Прочность варпа");
            AddText("FleetsSpeed", "Fleet`s speed", "Скорость флота");
            AddText("FleetInfo", "Select fleet`s parameters", "Настройка параметров флота");
            AddText("FleetCommands", "Fleet's commands", "Управление флотом");
            AddText("Buildings", "Buildings", "Постройки");
            AddText("ShipTypes", "Space ship models", "Модели космических кораблей");
            AddText("Generators", "Ship's power generators", "Корабельные генераторы энергии");
            AddText("Shields", "Ship's force shield", "Корабельные установки силового щита");
            AddText("Computers", "Ship's aim computers", "Корабельные вычислители");
            AddText("Engines", "Ship's engines", "Корабельные двигатели");
            AddText("Weapons", "Weapons", "Вооружение");
            AddText("Equipments", "Equipments", "Дополнительное оборудование");
            AddText("Others", "Other technology", "Прочие технологии");
            AddText("LevelSciences", "Level {0} scieneces", "Технологии {0} уровня");
            AddText("EnergySciences", "Energy sciences", "Энергетические науки");
            AddText("PhysicSciences", "Physical sciences", "Физические науки");
            AddText("IrregularSciences", "Irregular sciences", "Аномальные науки");
            AddText("CyberneticSciences", "Cybernetic sciences", "Кибернетические науки");
            AddText("Enemy", "Infection", "Заражение");
            AddText("NoEnemy", "There is no Infection", "Здесь нет заражения");
            AddText("Reward", "Reward", "Награда");
            AddText("Experience", "Experience", "Опыт");
            AddText("NoRepair", "Ship repaired", "Корабль отремонтирован");
            AddText("Repair", "Repair", "Ремонт");
            AddText("Pilot", "Pilot", "Пилот");
            AddText("Min", "min", "мин");
            AddText("NoHomeLand", "No home colony", "Нет колонии приписки");
            AddText("FleetInMission", "Fleet in mission", "Флот на задании");
            AddText("Planets", "Planets", "Планеты");
            AddText("Lands", "Lands", "Колоний");
            AddText("MaxPopulation", "Max Popul.:", "Макс. насел.:");
            AddText("Unlimited", "Unlim.", "Беск.");
            AddText("Atmosphere", "Atmos-phere:", "Атмос-фера:");
            AddText("ColonizeLand", "Colonize \nNew Land", "Колонизировать \nновую колонию");
            AddText("NoColonize", "You have maximum lands", "Достигнут предел лимита колоний");
            AddText("NoColonyPower", "Not enougth colony power", "Недостаточно колонизационных мощностей");
            AddText("BadPlanet", "You can't make new lands on this planet", "На этой планете невозможно основывать новые колонии");
            AddText("BadPlanetPillage", "This planet can not be pillaged", "Эта планету нельзя грабить");
            AddText("DefenseLandSend", "Send to Defense", "Отправить на защиту");
            AddText("DefenseLandRemove", "Remove from Defense", "Снять с защиты");
            //AddText("DefensePlayerStatus", "Fleet in colony defense group", "Флот на защите колоний");
            AddText("PillageLandSend", "Send to Pillage", "Отправить на грабёж");
            AddText("ChangeFleetParams", "Change behavior", "Изменить поведение");
            AddText("NoPillageFreeColony", "No one to pillage", "Некого грабить");
            AddText("NoPillageSelfColony", "You can't to pillage self colony's", "Нельзя грабить собственные колонии");
            AddText("NoPillageClanColony", "You can't to pillage Alliance's colony's", "Нельза грабить колонии альянса");
            AddText("NoPillageConstructedColony", "You can't to pillage builded colony's", "Нельзя грабить строящиеся колонии");
            AddText("NoCaptureFreeColony", "No one to capture", "Некого захватывать");
            AddText("NoCaptureSelfColony", "You can't to capture self colony's", "Нельзя захватывать собственные колонии");
            AddText("NoCaptureClanColony", "You can't to capture Alliance's colony's", "Нельза захватывать колонии альянса");
            AddText("NoCapturePower", "Not enought colony power", "Недостаточно колонизационных мощностей");
            AddText("FreeLand", "Land is free", "Место свободно");
            AddText("ArmorType0", "Balanced", "Сбалансированная");
            AddText("ArmorType1", "Reflection", "Отражающая");
            AddText("ArmorType2", "Composite", "Композитная");
            AddText("ArmorType3", "AntiIrregular", "Антианомальная");
            AddText("ArmorType4", "AntiCybernetical", "Антикибернетическая");
            AddText("ArmorType5", "Traditional", "Традиционная");
            AddText("ArmorType6", "Screening", "Экранированная");
            AddText("ArmorType7", "Heavy", "Тяжёлая");
            AddText("ArmorType", "Armor: ", "Броня: ");
            AddText("SelectArmorType", "Select armor type for ship", "Выберите тип брони корабля");
            AddText("CreateClanBtn", "Create a New Alliance", "Создать новый альянс");
            AddText("EnterInClanBtn", "Find an Alliance", "Вступить альянс");
            AddText("SelectClanLbl", "Send request to Alliance", "Отправить заявку в альянс");
            AddText("ClanName", "Alliance's\nName", "Название\nАльянса");
            AddText("ClanEmblem", "Emblem", "Эмблема");
            AddText("ClanPlayersCount", "Nations", "Число\nгосударств");
            AddText("ClanLandsCount", "Lands", "Число\nколоний");
            AddText("PlayersCount", "Governors count", "Число государств");
            AddText("LandsCount", "Lands count", "Число колоний");
            AddText("Population", "Population", "Население");
            AddText("FleetsCount", "Fleets count", "Число флотов");
            AddText("FleetsCountDefense", "Fleets count in defense", "Число флотов в защите");
            AddText("BattlesCount", "Battles count", "Число боёв");
            AddText("ShipsCount", "Ships count", "Число кораблей");
            AddText("ScienceCount", "Sciences count", "Число исследований");
            AddText("Leadership", "Leadership", "Лидерство");
            AddText("Name", "Name", "Имя");
            AddText("LeaveClan", "Leave", "Покинуть");
            AddText("SendRequest", "Send request", "Отправить заявку");
            AddText("RemoveRequest", "Remove request", "Отозвать заявку");
            AddText("InviteRequest", "Requests", "Заявки");
            AddText("InvitePlayer", "Invite", "Принять");
            AddText("DeclinePlayer", "Decline", "Отклонить");
            AddText("LeavePlayer", "Out", "Выгнать");
            AddText("NotYourColony", "Now this is not your colony", "Сейчас это не ваша колония");
            AddText("NewLandName", "Expansion colony ", "Расширение колоний ");
            AddText("NewLandDescription", "This science expand your ability to control colonys by one", "Это исследование позволяет вам управлять ещё одной новой колонией");
            AddText("FleetFree", "Fleet is free", "Флот свободен");
            AddText("FleetEnemy", "Attack Invasion", "Атака вторжения");
            AddText("FleetPilage", "Pillage", "Грабёж");
            AddText("FleetConquer", "Capture colonies", "Захват колоний");
            AddText("FleetResources", "Resource collection", "Сбор ресурсов");
            AddText("Riot", "Riot", "Восстание");
            AddText("RiotText", "In your colony is a riot! You can lose this colony", "В вашей колонии восстание! Вы можете потерять эту колонию");
            AddText("RiotPrepare", "Civil disorders", "Народные волнения");
            AddText("RiotPrepareText", "In your colony is a civil disorders! Prepare to defend you cololny", "В вашей колонии народные волнения! Приготовьтесь к защите колонии");
            AddText("GenInfo", "General information", "Общая информация");
            AddText("DestroyShip", "Destroy\nship", "Разобрать\nкорабль");
            AddText("NotRemind", "Don't remind", "Не напоминать");
            AddText("LowAcc", "low", "низ");
            AddText("MiddleAcc", "mid", "средн");
            AddText("HighAcc", "high", "выс");
            AddText("MaxAcc", "max", "макс");
            AddText("Buy_Premium", "Buy premium", "Купить премиум");
            AddText("Premium_Forever", "Full accsess", "Полный доступ");
            AddText("Premium_Left", "Left: {0} days", "Осталось: {0} дней");
            AddText("Help", "Help", "Помощь");
            AddText("Finish", "Finish", "Завершить");
            AddText("AllFleets", "All fleets", "Все флоты");
            AddText("NoClan", "No alliance", "Нет альянса");
            AddText("NewBasePrice", "New fleet's base price", "Стоимость строительства базы");
            AddText("GOIBuilding", "Building", "Строение");
            AddText("GOIShipType", "Model", "Модель");
            AddText("GOIGenerator", "Generator", "Генератор");
            AddText("GOIShield", "Force Shield", "Силовой щит");
            AddText("GOIComputer", "Aim computer", "Вычислитель");
            AddText("GOIEngine", "Engine", "Двигатель");
            AddText("GOIWeapon", "Weapon", "Вооружение");
            AddText("GOIEquipment", "Equipment", "Оборудование");
            AddText("GOINewLand", "Expand", "Расширение");
            AddText("NoItem", "Not installed", "Не установлено");
            AddText("TooFar", "Destination spot is too far", "Точка назначения слишком далеко");
            //AddText("NoPilot", "No pilot", "Нет пилота");
            //AddText("NoFleet", "No fleet", "Нет флота");
            AddText("NeedRepair", "Need to repair", "Нужен ремонт");
            AddText("BonusActive", "Acitve\n", "Активировано\n");
            AddText("PremiumTitle", "Premium account", "Премиум аккунт");
            AddText("PremiumLeft", "Before returning, left {0} days", "До возвращения осталось {0} дней");
            AddText("PremiumFalse", " - Not active", " - Не активирован");
            AddText("PremiumBonusNoDelete", "Accaunt protect\n-Your account will not delete after 30 days passive", "Защита аккаунта\n- Нет удаления аккаунта при отсутствии активности более 30 дней");
            AddText("PremiumBonusFullScience", "Science freedom\n- You can learn new science after 10 level without premium", "Научная свобода\n- Технологии не ограничены 10м уровнем и после снятия премиума");
            AddText("PremiumBonusFullMoney", "Finance freedom\n- You can collect more than 1 million money without premium", "Финансовая свобода\n- Деньги не ограничены 1 миллионом и после снятия премиума");
            AddText("PremiumBonusQuickStart", "Quick start\n- You will recieve all sciences from 1 to 5 level and 3 millions of money", "Быстрый старт\n- Открытие всех технологий с первого по пятый уровень и получение 3 миллионов кредитов");
            AddText("PremiumBonusOrion", "Edem\n- You will recieve a colony on Edem - planet, where no fights", "Эдем\n- Получение колонии на эдеме - планете, на которой запрещены боевые действия");
            AddText("PremiumBonusInfo", "Premium bonuses - Activating when you bought a premium currency", "Премиум бонусы - Активируются при приобретении премиумной валюты");
            AddText("Premium_30", "Premium\n30 days", "Премиум\n30 дней");
            AddText("Premium_90", "Premium\n90 days", "Премиум\n90 дней");
            AddText("Premium_180", "Premium\n180 days", "Премиум\n180 дней");
            AddText("Premium_365", "Premium\n365 days", "Премиум\n365 дней");
            AddText("Premium_Full", "Premium\nForever", "Премиум\nБессрочно");
            AddText("PremiumInfo1", "Premium-accaunt allow you to use all the possibilities of the game without limitations.", "Премиум-аккаунт позволяет воспользоваться всем возможностями игры без ограничений.");
            AddText("PremiumInfo2", "Without premium on the game run next limitations:", "Без премиума на игру действуют следующие ограничения:");
            AddText("PremiumInfo3", "- You can't hold more than two colonies;", "- Максимально можно владеть не более чем двумя колониями;");
            AddText("PremiumInfo4", "- You can't rule more than three fleets;", "- Максимально можно управлять не более чем тремя флотами;");
            AddText("PremiumInfo5", "- You can`t build more than 15 ships;", "- Максимально можно построить не более 15 кораблей;");
            AddText("PremiumInfo6", "- You can't reveal science more than 10 level;", "- Максимальный уровень технологий - 10ый;");
            AddText("PremiumInfo7", "- You can't have more than 1 million of money;", "- Максимальное количество денег на счету - 1 миллион;");
            AddText("PremiumInfo8", "- Your account will be delete after 30 days of passive.", "- При отсутствии активности более 30 дней аккаунт удаляется.");
            AddText("PremiumInfo9", "But you will recieave bonuses when you reciece premium currency.", "Но, при получении премиум валюты активируются бонусы.");
            AddText("PremiumBuyButton", "Buy premium currency", "Купить премиум валюту");

            AddText("Build_Federal", "Federal", "Федеральные");
            AddText("Build_Typical", "Local", "Местные");
            AddText("Build_Live_Sectors", "Live sectors", "Жилые сектора");
            AddText("Build_Banks", "Banks", "Банки");
            AddText("Build_Mines", "Mines", "Шахты");
            AddText("Build_Chip_Factories", "Chip factories", "Фабрики микросхем");
            AddText("Build_Universiteties", "Universiteties", "Университеты");
            AddText("Build_Part_Accelerators", "Participle accelerators", "Ускорители частиц");
            AddText("Build_Science_Lab", "Science laboratories", "Научные лаборатории");
            AddText("Build_Manufactures", "Manufactures", "Заводы");
            AddText("Build_Meltings", "Steel meltings", "Сталеплавильные цеха");
            AddText("Build_Radar", "Radar stations", "Радарные станции");
            AddText("Build_Data_Centers", "Data centers", "Дата центры");

            AddText("Ship_Scout", "Scout", "Разведчик");
            AddText("Ship_Corvette", "Corvette", "Корвет");
            AddText("Ship_Transport", "Transport", "Транспорт");
            AddText("Ship_Battle", "Battleship", "Линкор");
            AddText("Ship_Frigate", "Frigate", "Фригат");
            AddText("Ship_Fighter", "Fighter", "Истребитель");
            AddText("Ship_Dreadnought", "Dreadnought", "Дредноут");
            AddText("Ship_Devostator", "Devostator", "Девостатор");
            AddText("Ship_Warrior", "Warrior", "Воитель");
            AddText("Ship_Cruiser", "Battlecruiser", "Крейсер");

            AddText("Item_Small", "Small", "Малые");
            AddText("Item_Medium", "Medium", "Средние");
            AddText("Item_Large", "Large", "Большие");
            AddText("Item_Group", "Group", "Групповые");
            AddText("Item_Energy", " energy", " энергетические");
            AddText("Item_Physic", " physic", " физические");
            AddText("Item_Irregular", " irregular", " аномальные");
            AddText("Item_Cyber", " cyber", " кибернетические");
            AddText("Item_Total", " universal", " универсальные");
            AddText("Item_Energy2", "Energy", "Энергетические");
            AddText("Item_Physic2", "Physic", "Физические");
            AddText("Item_Irregular2", "Irregular", "Аномальные");
            AddText("Item_Cyber2", "Cyber", "Кибернетические");
            AddText("Item_Total2", "Universal", "Универсальные");
            AddText("Item_Generators", " generators", " генераторы");
            AddText("Item_Shields", " shields", " щиты");
            AddText("Item_Computers", " aim's computers", " вычислители");
            AddText("Item_Engines", " engines", " двигатели");
            AddText("Item_Basic", "Basic Parameters", "Базовые параметры");
            AddText("Item_Regen", "Regeneration", "Регенерация");
            AddText("Item_Accuracy", "Accuracy", "Точность");
            AddText("Item_Evasion", "Evasion", "Уклонение");
            AddText("Item_Absorb", "Reduce damage", "Снижение повреждений");
            AddText("Item_Damage", "Increase damage", "Повышение урона");
            AddText("Item_Immune", "Critical immunity", "Иммунитет к критическим эффектам");
            AddText("Item_NotBattle", "Not battle", "Не боевые");
            AddText("Item_Cargo", "Cargo", "Грузовые");
            AddText("Item_Colony", "Colony", "Колонизационные");
            AddText("Item_NewLand", "Nation expand", "Расширение госудраства");
            AddText("BattleEnd", "Battle end", "Конец боя");
            AddText("ChangeLandNameTitle", "Change colony name", "Сменить наименование колонии");
            AddText("ScienceLevel", "Sort by level", "Сортировка по уровню");
            AddText("ScienceType", "Sort by type", "Сортировка по типу");
            AddText("NoCommands", "Сommands are not available", "Команды не доступны");
            AddText("NoRule", "Change settings not available", "Изменение параметров недоступно");
            AddText("MoreItems", "Show More Items", "Показать другие варианты");
            AddText("MoveBack", "Back", "Назад");
            AddText("ShowGrid", "Show in grid", "Показать в таблице");
            AddText("GeneratorCapacity", "Capacity generator", "Ёмкостный генератор");
            AddText("GeneratorRecharge", "Recharge generator", "Высокоэнергетический генератор");
            AddText("LargeSizeGenerator", "Big size generator", "Генератор большого размера");
            AddText("ShieldCapacity", "Capacity shield generator", "Ёмкостный генератор щита");
            AddText("ShieldRecharge", "Recharge shield generator", "Самовостанавливающийся генератор щита");
            AddText("LargeSizeShield", "Big size shield generator", "Генератор щита большого размера");
            AddText("CompBasic", "Basic aim computer", "Базовый вычислитель");
            AddText("CompAccEn", "Energy aim computer with high accuracy", "Энергетический вычислитель высокой точности");
            AddText("CompAccPh", "Physic aim computer with high accuracy", "Физический вычислитель высокой точности");
            AddText("CompAccIr", "Irregular aim computer with high accuracy", "Аномальный вычислитель высокой точности");
            AddText("CompAccCy", "Cybernetic aim computer with high accuracy", "Кибернетический вычислитель высокой точности");
            AddText("CompDamEn", "Energy aim computer of weak points", "Энергетический вычислитель слабых точек");
            AddText("CompDamPh", "Physic aim computer of weak points", "Физический вычислитель слабых точек");
            AddText("CompDamIr", "Irregular aim computer of weak points", "Аномальный вычислитель слабых точек");
            AddText("CompDamCy", "Cybernetic aim computer of weak points", "Кибернетический вычислитель слабых точек");
            AddText("EngineUniverse", "Universe Engine", "Универсальный двигатель");
            AddText("EngineBasic", "Basic Engine", "Базовый двигатель");
            AddText("LargeSizeEngine", "Big size engine", "Двигатель большого размера");
            AddText("HealthEquip", "Hull value equipment", "Навесные модули корпуса");
            AddText("RegenHealthEquip", "Hull restore equipment", "Модули ремонта корпуса");
            AddText("ShieldEquip", "Shield power equipment", "Модули прочности щита");
            AddText("RegenShieldEquip", "Shield regeneration equipment", "Модули регенерации щита");
            AddText("EnergyEquip", "Power generator capacity equipment", "Модули увеличения ёмкости генератора энергии");
            AddText("RegenEnergyEquip", "Power generator recharge equipment", "Модули увеличения генерации энергии");
            AddText("AccuracyEquipEn", "Accuracy equipment of energy weapon", "Модификаторы меткости энергетического оружия");
            AddText("AccuracyEquipPh", "Accuracy equipment of physic weapon", "Модификаторы меткости физического оружия");
            AddText("AccuracyEquipIr", "Accuracy equipment of irregular weapon", "Модификаторы меткости аномального оружия");
            AddText("AccuracyEquipCy", "Accuracy equipment of cyber weapon", "Модификаторы меткости кибернетического оружия");
            AddText("AccuracyEquip", "Accuracy equipment", "Модификаторы меткости");
            AddText("EvasionEquipEn", "Evasion equipment from energy weapon", "Модули уклонения от энергетического оружия");
            AddText("EvasionEquipPh", "Evasion equipment from physic weapon", "Модули уклонения от физического оружия");
            AddText("EvasionEquipIr", "Evasion equipment from irregular weapon", "Модули уклонения от аномального оружия");
            AddText("EvasionEquipCy", "Evasion equipment from cyber weapon", "Модули уклонения от кибернетического оружия");
            AddText("EvasionEquip", "Evasion equipment", "Модули уклонения");
            AddText("DamageEquipEn", "Damage equipment of energy weapon", "Модули урона энергетического оружия");
            AddText("DamageEquipPh", "Damage equipment of physic weapon", "Модули урона физического оружия");
            AddText("DamageEquipIr", "Damage equipment of irregular weapon", "Модули урона аномального оружия");
            AddText("DamageEquipCy", "Damage equipment of cyber weapon", "Модули урона кибернетического оружия");
            AddText("DamageEquip", "Damage equipment", "Модули урона");
            AddText("IgnoreEquipEn", "Absorption equipment of energy weapon", "Модули поглощения урона от энергетического оружия");
            AddText("IgnoreEquipPh", "Absorption equipment of physic weapon", "Модули поглощения урона от физического оружия");
            AddText("IgnoreEquipIr", "Absorption equipment of irregular weapon", "Модули поглощения урона от аномального оружия");
            AddText("IgnoreEquipCy", "Absorption equipment of cyber weapon", "Модули поглощения урона от кибернетического оружия");
            AddText("IgnoreEquip", "Absorption equipment", "Модули поглощения урона");
            AddText("ImmuneEquipEn", "Immune equipment from energy weapon", "Модули иммунитета от энергетического оружия");
            AddText("ImmuneEquipPh", "Immune equipment from physic weapon", "Модули иммунитета от физического оружия");
            AddText("ImmuneEquipIr", "Immune equipment from irregular weapon", "Модули иммунитета от аномального оружия");
            AddText("ImmuneEquipCy", "Immune equipment from cyber weapon", "Модули иммунитета от кибернетического оружия");
            AddText("ImmuneEquip", "Immune equipment", "Модули иммунитета");
            AddText("ClearSchema", "English Text", "Создание пустой схемы, все элементы которой можно выбрать самому");
            AddText("SpeedShipSchema", "English Text", "Автоматическое создание модели корабля из доступных элементов, максимально приспособленного к быстрым атакам");
            AddText("DamageShipSchema", "English Text", "Создание модели корабля, максимально заточенного на нанесение урона");
            AddText("TankShipSchema", "English Text", "Создание модели корабля - защитника, основная роль которого в защите других специализированных кораблей");
            AddText("SupportShipSchema", "English Text", "Создание модели корабля - поддержки, который усиливает другие корабли");
            AddText("LoadSchema", "English Text", "Загрузка ранее созданных и сохранённых схем");
            AddText("ClearSchemaT", "Clear Schema", "Пустая схема");
            AddText("SpeedShipSchemaT", "Scout ship", "Корабль разведчик");
            AddText("DamageShipSchemaT", "Fighter ship", "Дамагер");
            AddText("TankShipSchemaT", "Defender ship", "Корабль защитник");
            AddText("SupportShipSchemaT", "Support ship", "Корабль поддержки");
            AddText("LoadSchemaT", "Load Schemas", "Загрузка схем");
            AddText("Parameters", "Game Settings", "Игровые настройки");
            AddText("Audio", "Audio", "Звук");
            AddText("Music", "Music volume", "Громкость музыки");
            AddText("Sound", "Sound volume", "Громкость звуков");
            AddText("RandomTitle", "Science", "Технология");
            AddText("RandomText", "Random science", "Случайная технология");
            AddText("StartBattle", "Start battle", "Начать бой");
            AddText("SelectFleet", "Select fleet", "Выбрать флот");
            AddText("PirateBaseText", "Pirate base text", "Конкуренты слили нам информацию о нахождении пиратской базы. Однако добраться до её сокровищ - не такая простая задача. Дело в том что база – не стационарная. По боевой мощи она соответствует тяжёлому кораблю. Плюс охрана. Но разве вам не интересно, что же там насобирали господа пираты?");
            AddText("ConvoyDefenseText", "Convoy defense text", "Правитель, Вас просят помочь в защите конвоя от атаки налётчиков. Будьте осторожны, противники хорошо подготовились к атаке. Вам необходимо уничтожить все атакующие корабли и не позволить уничтожить транспорт.");
            AddText("ConvoyDestroyText", "Convoy destroy text", "Правитель, Вам поступил заказ на тайную операцию. Необходимо не допустить доставку груза до места назначения. Постарайтесь уничтожить транспортный корабль. Но будьте осторожны, защитникам будут постоянно поступать подкрепления.");
            AddText("OreBeltReidText","Ore belt raid text", "Ваши разведчики обнаружили богатый минералами участок в поясе астероидов. Отправляйте туда корабли для сбора ресурсов. Но будьте осторожны, вряд ли этот участок остался незамеченным другими.");
            AddText("MeteoritRaidText", "meteorit raid text", "Наши обсерватории заметили крупный и богатый ресурсами астероид. Вы можете отправить свои корабли на захват астероида. Не забудьте только должным образом вооружить их, бесхозный астероид – лакомый кусочек.");
            AddText("LongRangeRaidText","Long range raid text", "Наши разведчики выкрали информацию о перспективном участке в далёком космосе, где велика вероятность найти что-нибудь интересное. Мы могли бы снарядить экспедицию, но надо торопиться, мы не единственные знаем эту информацию.");
            AddText("CompetitionText", "Competition text", "Правитель, нам пришло приглашение на участие в турнире. Необходимо выставить команду, численностью до пяти кораблей. Если вы победите – награда может быть весьма достойная.");
            AddText("BigCompetitionText", "Big competition text", "Наши войска добились разрешения на участие в большом турнире. Формат турнира – 10 на 10 кораблей. Соберите ваши лучшие корабли, соперники тоже не лаптем щи хлебают. Однако ради приза стоит побороться. ");
            AddText("AlienBoundText", "Alien bound text", "Мы обнаружили небольшую базу чужих в нашей галактике. Стремительная атака ваших войск могла бы немного помочь в войне. Тем более что на их складе возможно поживиться ресурсами.");
            AddText("AlienBaseText", "Alien base text", "Наш генеральный штаб разработал план операции по атаке укреплённой базы чужих. Но будьте готовы к жаркой битве, разведка сообщает о наличии тяжёлых кораблей в районе операции.");
            AddText("ScienceExpeditionText", "Science expedition text", "От научного сообщества галактики нам поступило предложение принять участие в научной экспедиции. Насколько я знаю этих ребят – пустыми словами они разбрасываться не привыкли. Так что там будет всё – и битвы и награда. Моя рекомендация однозначная – надо участвовать.");
            AddText("ArtifactSearchText", "Artifact search text", "Информация с пометкой молния! Учёные вскрыли тайник древних! Однако не всё так просто. Его охраняют несколько тяжёлых кораблей и группа прикрытия. Однозначно – это будет тяжёлая битва. Но награда! Правда не знаю что Вам рекомендовать, нужно тщательно всё спланировать.");
            AddText("PirateShipyardText", "Pirate shipyard text", "Наши союзники предлагают нам атаковать верфь пиратов. Взамен мы сможем присвоить себе всё что сможем там найти. Но это не простой налёт – на верфи куча кораблей и постоянно будут поступать из ремонта новые. Тут нужна стремительная операция, долгой осадой мы ничего не добьёмся.");
            AddText("BATTLE!!!", "BATTLE!!!", "СРАЖЕНИЕ!!!");
            AddText("Manual", "Manual", "Ручное");
            AddText("Auto", "Auto", "Автоматическое");
            AddText("NoAutoSchema", "Failed to assemble a scheme, no fitting parts", "Не получилось собрать схему, нет подходящих частей");
            AddText("BuildedColony", "Under construction", "Строится");
            AddText("PirateSchemaName", "Pirate's ", " пиратов");
            AddText("GreenTeamSchemaName", "Green Alliance's ", " зелёного альянса");
            AddText("TechsSchemaName", "Technocrate's ", " технократов");
            AddText("MercsSchemaName", "Mercenaries ", " наёмников");
            AddText("AliensSchemaName", "Invasion's ", " вторжения");
            AddText("Autopilot", "Autopilot", "Автопилот");

            AddText("Notice", "Notices", "Уведомления");
            AddText("SelfNotice", "Self notices", "Личные уведомления");
            AddText("ClanNotice", "Alliance notices", "Уведомления альянса");
            AddText("NotColonizeBattleStart", " has enter to battle with colony defenders", " вступил в бой с защитой колонии");
            AddText("NotColonizeBattleWin", " won the battle with colony defenders", " победил в битве с защитой колонии");
            AddText("NotColonizeBattleLose", "Your fleet lose the battle with colony defenders", " проиграл в битве с защитой колонии");
            
            AddText("NotStoryLineStart", " started to do: ", " начал выполнять задание: ");
            AddText("NotStoryLineWin", " has completed: ", " выполнил задание: ");
            AddText("NotStoryLineLose", " could not do: ", " не смог выполнить задание: ");
            AddText("NotRecieveReward", "You recieve the Reward: ", "Вы получили награду: ");
            AddText("NotNewAvanpost", "New avanpost has based: ", "Основан новый аванпост: ");
            AddText("NotNewLand", "New land has based: ", "Основана новая колония: ");
            AddText("NotRecieveRewardMeteorit", "NotRecieveRewardMeteorit", "При разработке метеорита получены ресурсы:");
            AddText("NotRecieveRewardOreBelt", "NotRecieveRewardOreBelt", "Во время рейда в пояс астероидов получены следующие ресурсы:");
            AddText("NotRecieveRewardAnomaly", "NotRecieveRewardAnomaly", "Адсорбция излучения аномалии позволила получить антиматерию:");

            AddText("NotDefenseSelfStart", " begin to defense your colony ", " вступил в битву на защиту колонии ");
            AddText("NotDefenseSelfWin", " won the battle in defending your colony ", " победил в битве по защите вашей колонии ");
            AddText("NotDefenseSelfLose", " lose the battle to defending your colony ", " проиграл в битве по защите вашей колонии ");
            AddText("NotDefenseClanStart", " begin to defense alliance member colony ", " вступил в битву на защиту колонии альянса ");
            AddText("NotDefenseClanWin", " won the battle to defense alliance member colony ", " победил в битве по защите колонии альянса ");
            AddText("NotDefenseClanLose", " lose the battle to defense alliance member colony ", " проиграл в битве по защите колонии альянса ");
            AddText("NotDefenseByClanStart", "Alliance fleet is defending your colony ", "Флот альянса вступил на защиту вашей колонии ");
            AddText("NotDefenseByClanWin", "Alliance fleet has win the battle for defending your colony ", "Флот альянса победил в битве по защите вашей колонии ");
            AddText("NotDefenseByClanLose", "Alliance fleet lose in battle for defense your colony ", "Флот альянса проиграл в битве по защите вашей колонии ");

            AddText("NotPillageAttackStart", " meets with defense in pillage mission ", " встретился с сопротивлением при попытке грабежа ");
            AddText("NotPillageAttackWinPillage", " won the battle and has pillaged enemy colony ", " победил в битве и ограбил вражескую колонию ");
            AddText("NotPillageAttackWinNext", " won the battle and start fight with a next defender ", " победил в битве и вступил в новую битву со следующим защитником ");
            AddText("NotPillageAttackWinReturn", " won the battle but can`t fight more for pillage ", " победил в битве но не смог продолжить грабёж ");
            AddText("NotPillageAttackLose", " lose in battle for pillage ", " потерпел поражение в битве при попытке грабежа ");
            AddText("NotPillageNoDefenseWin", " pillaged enemy colony ", " ограбил вражескую колонию " );
            AddText("NotPillageNoDefenseLose", " was pillaged by enemy ", " была ограблена врагом ");

            AddText("NotColonyAttackStart", " meet with defense in conquest mission ", " встретился с сопротивление при попытке колонизации ");
            AddText("NotColonyAttacWinReturn", " won the battle but can't to continue conquest mission ", " победил в битве но не смог продолжить колонизационную миссию ");
            AddText("NotColonyAttackWinColony", " won the battle and has conquered a colony ", " победил в битве и захватил колонию ");
            AddText("NotColonyAttackWinFail", " won the battle but fail to conquer a colony ", " победил в битве но не смог захватить колонию ");
            AddText("NotColonyAttackWinLastFail", " won the battle but fail to start last battle ", " победил в битве но не смог вступить в финальный бой ");
            AddText("NotColonyAttackWinLast", " won the battle and start a new battle with last defender ", " победил в битве и вступил в бой с последним защитником колонии ");
            AddText("NotColonyAttacWinkNext", " won the battle and start fight with a next defender ", " победил в битве и вступил в новую битву со следующим защитником ");
            AddText("NotColonyAttackWinUprise", " won the battle and make an uprise on a colony ", " победил в битве и запустил восстание в колонии ");
            AddText("NotColonyAttackWinNoUprise", " won the battle but can't make a new uprise ", " победил в битве но не смог развязать новое восстание ");
            AddText("NotColonyAttackLose", " lose in battle for conquest mission ", " проиграл бой защитнику колонии ");

            AddText("NotColonyLastAttackFailStart", " fail to make a new uprise ", " не смог развязать новое восстание в колонии ");
            AddText("NotColonyLastAttackStart", " start a battle with last defender ", " начал бой с последним защитником ");
            AddText("NotColonyLastAttackWinColony", " defeat a last defender and has conquered a colony ", " победил последнего защитника и захватил колонию ");
            AddText("NotColonyLastattackWinFail", " defeat a last defender but can't to conquer a colony ", " победил последнего защитника но не смог захватить колонию ");
            AddText("NotColonyLastAttackWinUprise", " defeat a last defender and make an uprise on a colony ", " победил последнего защитника и развязал восстание в колонии ");
            AddText("NotColonyLastAttackWinNoUprise", " defeat a last defender but can't make an uprise ", " победил последнего защитника но не смог развязать восстание в колонии ");
            AddText("NotColonyLastAttackLose", " lose in battle with last defender ", " проиграл в битве с последним защитником ");

            AddText("NotColonyNoDefenseColonyWin", " has conquered a colony ", " захватил колонию ");
            AddText("NotColonyNoDefenseFail", " fail to conquer a colony ", " не смог захватить колонию ");
            AddText("NotColonyNoDefenseColonyLose", " was conquered ", " захвачена ");
            AddText("NotColonyNoDefenseUpriseWin", " begin an uprise ", " развязал восстание ");
            AddText("NotColonyNoDefenseUpriseLose", " was begin an uprise ", " было развязано восстание ");
            AddText("NotColonyNoDefenseNoUprise", " can't to begin an uprise ", " не смог развязать восстание ");

            AddText("NotColonyNewLose", " destroyed", " разрушена");
            AddText("NotColonyNewDestroyed", " has destroyed a building colony ", " разрушил строящуюся колонию ");

            AddText("NotLastDefenseStart", " start a battle as Last Defender of your colony ", " вступил в бой как последний защитник вашей колонии ");
            AddText("NotLastDefenseWin", " won the battle as Last Defender of your colony ", " победил в битве как последний защитник вашей колонии ");
            AddText("NotLastDefenseLoseColony", " lose in battle as Last Defender and lost a colony ", " проиграл в битве как последний защитник и потерял колонию ");
            AddText("NotLastDefenseLoseNoColony", " lose in battle as Last Defender but enemy can't to conquer your colony ", " проиграл в битве как последний защитник, но враг не смог колонизировать вашу колонию ");
            AddText("NotLastDefenseLoseUprise", " lose in battle as Last Defender and enemy begin an uprise ", " проиграл в битве как последний защитник и враг развязал восстание на вашей колонии ");
            AddText("NotLastDefenseLoseNoUprise", " lose in battle as Last Defender but enemy can't begin an uprise ", " проиграл в битве как последний защитник, но враг не смог развязать восстание ");

            AddText("NotPirateBattleStart", " has enter to battle with Pirate's fleet", " вступил в бой с флотом пиратов");
            AddText("NotPirateBattleWin", " won the battle with Pirate's fleet", " победил в битве с флотом пиратов");
            AddText("NotPirateBattleLose", "Your fleet lose the battle with Pirate's fleet", "Ваш флот проиграл в битве с флотом пиратов");

            AddText("NotGreenTeamBattleStart", " has enter to battle with Green Ally's fleet", " вступил в бой с флотом Зелёного альянса");
            AddText("NotGreenTeamBattleWin", " won the battle with Green Ally's fleet", " победил в битве с флотом Зелёного альянса");
            AddText("NotGreenTeamBattleLose", "Your fleet lose the battle with Green Ally's fleet", "Ваш флот проиграл в битве с флотом Зелёного альянса");

            AddText("NotAlienBattleStart", " has enter to battle with Alien's fleet", " вступил в бой с флотом пришельцев");
            AddText("NotAlienBattleWin", " won the battle with Alien's fleet", " победил в битве с флотом пришельцев");
            AddText("NotAlienBattleLose", "Your fleet lose the battle with Alien's fleet", "Ваш флот проиграл в битве с флотом пришельцев");

            AddText("NotTechTeamBattleStart", " has enter to battle with Tech Bratherhood's fleet", " вступил в бой с флотом Технобратства");
            AddText("NotTechTeamBattleWin", " won the battle with Tech Bratherhood's fleet", " победил в битве с флотом Технобратства");
            AddText("NotTechTeamBattleLose", "Your fleet lose the battle with Tech Bratherhood's fleet", "Ваш флот проиграл в битве с флотом Технобратства");

            AddText("NotMercBattleStart", " has enter to battle with Mercenaries's fleet", " вступил в бой с флотом Наёмников");
            AddText("NotMercBattleWin", " won the battle with Mercenaries's fleet", " победил в битве с флотом Наёмников");
            AddText("NotMercBattleLose", "Your fleet lose the battle with Mercenaries's fleet", "Ваш флот проиграл в битве с флотом Наёмников");

            AddText("NotTargetColonyOwnerChanged", "Target colony has changed owner", "У колонии-цели сменился владелец");
            AddText("NotLearnNewScience", "You have learn a new science: ", "Вы изучили новую технологию: ");
            AddText("NotAddPremiumCurrency", "You have recieved a premium currency: ","Вы получили премиум валюту: ");
            AddText("NotActivatePremium", "Premium-accaunt activated on {0} days", "Премиум-аккаунт активирован на {0} дней");
            AddText("NotQuickStartPremium", "Quick start activated, {0} sciences learned, 3 millions credites recieved", "Быстрый старт активирован, {0} исследований изучено, 3 миллиона кредитов получено");
            AddText("NotOrionPremium", "You have recieved a land on Edem", "Вы получили колонию на Эдеме");

            AddText("NotLoseLandByEnemy", "Invasion's fleet has conquer your colony", "Флот вторжения захватил вашу колонию");

            AddText("NotColonyNewLandWin", " has created a new colony ", " создал новую колонию ");
            AddText("NotColonyNewLandLose", " can't create a new colony on a planet ", " не смог создать новую колонию на планете ");

            AddText("YourFleet", " Your fleet ", " Ваш флот ");
            AddText("NotBattle", " BATTLE!!!", " СРАЖЕНИЕ!!!");
            AddText("NotVictory", "VICTORY!!!", "ПОБЕДА!!!");
            AddText("NotReward", "REWARD: ", "ДОБЫЧА: ");
            AddText("NotLose", "LOSE!!!", "ПОРАЖЕНИЕ!!!");
            AddText("NotExp", "EXPERIENCE: ", "ОПЫТ: ");
            AddText("NotScience", "CONGRATULATIONS!!!", "ПОЗДРАВЛЯЕМ!!!");
            AddText("NotPremium", "PREMIUM", "ПРЕМИУМ");

            AddText("FleetScout", "Patrol", "Патрулирование");
            AddText("FleetReturn", "Return fleet", "Вернуть флот");
            AddText("Mission", "Make mission", "Выполнить задание");
            AddText("Enemy's", "Enemy's:", "Враги:");
            AddText("MisShips", "Ships:", "Корабли: ");
            AddText("MisLevel", "Tech level:", "Уровень: ");
            AddText("MisReward", "Reward:", "Добыча:");

            AddText("MisPirateBase", "Attack a pirate base", "Нападение на пиратскую базу");
            AddText("MisConvoyDestroy", "Attack a convoy", "Нападение на конвой");
            AddText("MisConvoyDefense", "Defense a convoy", "Сопровождение конвоя");
            AddText("MisOreBeltRaid", "Raid in ф asteroid belt", "Рейд в пояс астероидов");
            AddText("MisMetheoritRaid", "Collect roaming meteorites", "Сбор блуждающих астероидов");
            AddText("MisLongRangeRaid", "Raid in deep space", "Рейд в дальний космос");
            AddText("MisCompetition", "Competition", "Участие в турнире");
            AddText("MisAnomalySearch", "Anomaly exploration", "Исследование аномалий");
            AddText("MisAlienBounds", "Raid in alien`s bounds", "Рейд по границам пришельцев");
            AddText("MisAlienBases", "Raid in alien's centrals", "Рейд по базам пришельцев");
            AddText("MisScienceRaid", "Scientific expedition assist", "Участие в научной экспедиции");
            AddText("MisArtefactRaid", "An ancient artefact search", "Поиск атрефактов древних");
            AddText("MisPirateShipyard", "Attack a pirate shipyard", "Нападение на пиратскую верфь");

            AddText("ShowLearn", "Show education?", "Показать обучение?");
            AddText("Next", "Next", "Далее");
            AddText("Back", "Back", "Назад");
            AddText("ShowMore", "Show more", "Показать больше");
            AddText("DamageType", "Damage type:", "Тип урона:");
            AddText("CritChanceShort", "Crit. hit chance", "Шанс крит. удара");
            AddText("EnergyDamage", "Energy", "Энергетический");
            AddText("PhysicDamage", "Physical", "Физический");
            AddText("IrregularDamage", "Irregular", "Аномальный");
            AddText("CyberDamage", "Cybernetic", "Кибернетический");

            AddText("BattleLesson1.0", "1. Main elements of battle field:", "1. Основные элементы поля боя:");
            AddText("BattleLesson1.1", "1.1 The main field of the battle field is a space from 8x5, 12x7 or 16x9 hexes.", "1.1 Основное поле боя, представляет собой поле размером 8х5, 12х7 или 16х9 хексов. ");
            AddText("BattleLesson1.2", "1.2 Gates is an additional field from 5x1 hexes. On gates are located ships that are ready for teleportation on the battle field. You can’t shoot in ships, located in the gate.",
                "1.2 Гейты, представляют собой дополнительные поля размером 5х1 хекс. На них располагаются корабли, готовые к телепортации на поле боя. По кораблям, расположенным в гейте, вести огонь нельзя.");
            AddText("BattleLesson1.3", "1.3 Portals are objects, located in opposite corners of the battle field. Every fleet has it’s own portal. If the portal will be destroyed, no one ship can’t enter in the battle field.",
                "1.3 Порталы, представляющие собой объекты в противоположных концах поля боя. Каждому флоту принадлежит свой портал. При его уничтожении ни один корабль соответствующего флота не сможет более попасть на поле боя.");
            AddText("BattleLesson1.4", "1.4 The attacking fleet, its gate and the portal are located in the left side of the battle field. The defending fleet is located in the right side of the battle field. Mostly, you will be rule by the attacking fleet.",
                "1.4 Атакующие корабли, их гейт и портал расположены в левой части поля боя, защищающиеся в правой. В основном вы будете управлять атакующим флотом.");
            AddText("BattleLesson1.5", "1.5 The Time indicator. It is show time that left for the end of the round and the time for making commands.",
                "1.5 Индикатор оставшегося времени. Показывает время, оставшееся до окончания периода отдачи команд.");
            AddText("BattleLesson1.6", "1.6 The confirm button. It is used for confirm your commands to your ships.",
                "1.6 Кнопка подтверждения хода. Она предназначена для подтверждения своего хода.");
            AddText("BattleLesson1.7", "1.7 Reset button. It is used for reset your commands.",
                "1.7 Кнопка сброса хода. Предназначена для сбрасывания отданных команд.");
            AddText("BattleLesson1.8", "1.8 The button of the animation speed. You can increase the animation speed in 2 or 4 times.",
                "1.8 Кнопка ускорения анимации. Позволяет менять скорость анимации в бою в 2 и в 4 раза.");
            AddText("BattleLesson1.9", "1.9 Additional shoot buttons. It is allow you to correct ship shooting more precisely.",
                "1.9 Дополнительные кнопки стрельбы. Позволяют более точно управлять режимом огня кораблей.");
            AddText("BattleLesson2.0", "2. Battles in the game is proceeding by next way:",
                "2. Сражение в игре происходит следующим образом:");
            AddText("BattleLesson2.1", "2.1 In the initial time all ships are located on their own bases. On the field are located only portals and asteroids.",
                "2.1 В начальный момент времени все корабли расположены на собственных базах, на поле боя расположены только порталы и астероиды.");
            AddText("BattleLesson2.2", "2.2 In the beginning of every round one ship from every fleet is placed in the gate, where they are preparing to the teleportation to the field. This process is continue during one to three rounds (it is depends from equipment of the ship). This time, that left for ship ready for teleportation, is imaging by the text “1”, “2” or “3”.",
                "2.2 Затем каждый раунд по одному кораблю от каждого флота попадают в гейт, где они готовятся к телепортации на поле боя. Этот период занимает от одного до трёх раундов (в зависимости от установленного оборудования). Время оставшееся до готовности корабля к прыжку отображается надписью 1, 2 или 3.");
            AddText("BattleLesson2.3", "2.3 When the ship will be ready for teleportation to the field the text on ship will change to “GO”. After that you can rule by your ship.",
                "2.3 Когда корабль будет готов к прыжку – надпись на нём сменится на «GO». С этого момента корабль попадает под ваше управление.");
            AddText("BattleLesson2.4", "2.4 Selecting of the ship is doing by left mouse button click. After that on the battle field is light by purple color that hexes, in which selection ship can make teleportation. The teleportation range also is depends from equipment of the ship. You can give your first command to enter your ship in the battle field by clicking left mouse button on a purple hex.",
                "2.4 Выделение корабля производится кликом по нему левой кнопки мышки. В этот момент на поле боя фиолетовым цветом подсвечиваются те хексы, на которые корабль может телепортироваться. Начальная дальность прыжка также зависит от установленного на корабле оборудования. Кликом левой кнопки мышки по выделенному хексу вы можете отдать кораблю команду на прыжок. ");
            AddText("BattleLesson2.5", "2.5 In that time you only give a command, but teleportation will be done only after you click on the confirm button. But not hurry to do this. You also can give and another commands.",
                "2.5 В этот момент вы только отдаёте команду, сам прыжок будет выполнен только после того, как вы подтвердите команду соответствующей кнопкой. Однако не спешите это делать, вы также можете отдать и другие команды.");
            AddText("BattleLesson2.6", "2.6 To the ship, that located on the battle field, is available next commands: the moving, the shooting and the living the battle field. All this commands are making by left mouse button click. The moving – is by clicking on free, lighted by green color, hex. The shooting – is by clicking on a enemy ship. The living the battle field – is by clicking on your portal.",
                "2.6 Кораблю, расположенному на поле боя, доступны следующие команды: Перемещение, стрельба и покидание поля боя. Все эти команды также отдаются левой кнопкой мыши. Перемещение - кликом по свободному, подсвеченному зелёным светом, хексу. Стрельба – кликом по вражескому кораблю. Покидание поля боя – по своему порталу.");
            AddText("BattleLesson2.7", "2.7 In case, if your ship is ready for the shooting – you can see the text “Ready” on the ship. ",
               "2.7 В случае если корабль готов к стрельбе – то на нём отображается индикатор “Ready”. Если по различным причинам корабль не может вести стрельбу, но может перемещаться – то отображается информация “Move”. Если же он не может ни стрелять ни перемещаться – то отображается информация “No Energy”.");
            AddText("BattleLesson2.8", "2.8 After selecting your ship and moving the pointer to the enemy ship you can see a hit chance in percent. The hit chance depends from many factors: weapon type, distance, additional equipment, any barriers between ships, buffs and debuffs. If a target was blocked by asteroid you will see the text “block”.",
               "2.8 При выборе своего корабля левой кнопкой мышки и наведении указателя на вражеский корабль отображается шанс попадания вооружением в процентах. Шанс попадания зависит от многих факторов:  от вооружения, от расстояния, от установленного дополнительно оборудования на вашем и вражеском корабле, от наличия других преград между кораблями, от временных эффектов. Если цель заблокирована астероидом – то отображается информация “block”.");
            AddText("BattleLesson2.9", "2.9 The ship can shoot from 1, 2 or 3 guns. One gun can shoot only one time for one round. When you make click by left mouse button on a target you give one command to shoot from one gun. If your ship have 2 or 3 guns you need 2 or 3 click for shooting by all guns.",
                "2.9 Для стрельбы кораблю может быть доступно от 1 до 3 различных орудий. Одно орудие за один раунд может совершить один выстрел. При однократном клике левой кнопкой мышки по цели даётся команда на стрельбу из одного орудия. Если орудия у корабля 2 или 3 то для стрельбы всеми орудиями потребуется 2 или 3 нажатия.");
            AddText("BattleLesson2.10", "2.10 In the simple click by left mouse button you give command to shoot from one gun, that available for shoot. If you want to make shoot by specific gun (left, right or middle) you can use by special buttons in the bottom of interface. The four button of this panel allows you to send commands to all your ships to shoot by all guns on selected target.",
                "2.10 При простом клике левой кнопкой мышки выдаётся команда на стрельбу из того орудия, которое доступно. Если вы хотите отдать команду на стрельбу каким-то конкретным орудием (левым, правым или центральным), то для этого внизу доступны специальные кнопки. Четвёртая кнопка позволяет отдать команды на стрельбу по выбранной цели всеми своими кораблями одновременно.");
            AddText("BattleLesson2.11", "2.11 All your ships spent energy on shooting and moving. Energy is indicated by green line. In the start of every round energy is restored on the value that depends from ship equipment.",
                "2.11 На стрельбу и перемещение корабля расходуется энергия. Количество энергии, доступной кораблю, отображается зелёным индикатором.  В начале каждого раунда запас энергии восстанавливается на значение, зависящее от установленного оборудования. ");
            AddText("BattleLesson2.12", "2.12 Every ship has a force shield and an armored hull for his protect from damage. The Force shield doesn’t protect the ship from all sides, but can absorb more damage and can quickly recover. If the hull value of the ship will be equal to zero – the ship is destroyed. The force shield strength and the hull value are indicated by blue and red line. ",
                "2.12 Для защиты от урона у корабля есть силовой щит и бронированный корпус. Силовой щит защищает корабль не со всех сторон, но, как правило, может принять больше повреждений и восстанавливается быстрее. Если прочность корпуса корабля станет равной нулю – корабль разрушается. Прочность силового щита и корпуса отображается синим и красным индикатором.");
            AddText("BattleLesson3.0", "3.0 Weapons: All ships can be equipped with weapons based on different principles. They differ in the type of damage, different damage to force shield and hull, accuracy, energy consumption and critical effect.",
                "3.0 Вооружение: Корабли могут быть оснащены орудиями, основанными на различных принципах. Они отличаются типом урона, различным повреждением щита и брони, точности, расходом энергии и эффектом при критическом срабатывании.");
            AddText("BattleLesson3.1", "3.1 All weapons are divided into 4 groups according to the type of damage – Energy, Physical, Irregular, and Cybernetic. For protection from each type of damage the ship has similarly allocated the armoring, which reduces the damage and evasion, which allows avoiding damage completely.",
               "3.1 Все вооружения разделены на 4 группы по типу урона – Энергетическая, Физическая, Аномальная и Кибернетическая. Для защиты от каждого типа урона у корабля аналогично выделяется бронирование, которое снижает повреждение и уклонение, которое позволяет избежать урона полностью. ");
            AddText("BattleLesson3.2", "3.2 Laser weapon is the first energy weapons, to conquer mankind. Its action is based on direct energy conversion in a focused light emission of high intensity. In critical hit the laser beam blinds the aiming of an enemy ship, that significantly reduces its accuracy on two rounds.",
                "3.2 Лазерное оружие является первым энергетическим вооружением, покорившееся человечеству. Его действие основано на прямом преобразовании энергии в узконаправленное световое излучение высокой интенсивности. При критическом эффекте луч лазера ослепляет прицельные приспособления вражеского корабля, что на два хода существенно снижает его точность.");
            AddText("BattleLesson3.3", "3.3 The damaging effects of an electromagnetic impulse due to occurrence of induced voltages and currents in on-Board systems of the target, which leads to their damage. In critical effect of electromagnetic impulse additionally charging the energy storage at all nearby friendly ships.",
                "3.3 Поражающее воздействие электромагнитного импульса обусловлено возникновением наведённых напряжений и токов в бортовых системах цели, что приводит к их повреждению. При критическом эффекте электромагнитный импульс дополнительно подзаряжает накопители энергии на всех соседних дружественных кораблях.");
            AddText("BattleLesson3.4", "3.4 The plasma is heated to ultra-high temperature ionized gas, which causes huge damage to any objects. Critical effect doubles damage dealt.",
                "3.4 Плазма – это раскалённый до сверхвысоких температур ионизированный газ, который наносит огромные повреждения любым объектам. Критический эффект позволяет удвоить наносимые повреждения. ");
            AddText("BattleLesson3.5", "3.5 The solar gun is the latest advancement of energy technologies. Energy nuclear interaction is concentrated on the example of the sun is a terrible weapon. The critical hit of this radiation can destroy a ship with one hit.",
                "3.5 Солнечная пушка – это последнее достижение энергетических технологий. Энергия термоядерного взаимодействия сконцентрированная по примеру солнца является грозным оружием. Критический эффект этого излучения позволяет уничтожить корабль с одного попадания. ");
            AddText("BattleLesson3.6", "3.6 The cannon is the oldest of technologies, but its effectiveness is quite decent. Strikes a metal core with a critical effect can contusion the pilot and to deprive the command ship for two turns.",
                "3.6 Ствольное орудие является древнейшей из применяемых технологий, однако его эффективность вполне достойная. Удары металлических сердечников при критическом эффекте могут контузить пилота и лишить корабль управления на два хода.");
            AddText("BattleLesson3.7", "3.7 The idea of Gauss weapon known for a long time, but only recently managed to achieve the efficiency of this process allows you to apply it in actual combat conditions. In critical hit, high velocity of the projectile allows it to overcome the force shield and cause damage directly to the hull.",
                "3.7 Идеи орудия Гаусса известны достаточно давно, но только в недавнее время удалось достигнуть КПД этого процесса, позволяющего применять его в реальных боевых условиях. При критическом эффекте сверхвысокая скорость снаряда позволяет ему преодолеть силовой щит и нанести урон сразу по корпусу.");
            AddText("BattleLesson3.8", "3.8 Rocket launcher is very powerful weapon of physics applications. Guided missiles allow you to reliably hit enemies at a great distance. In critical hit sheared warhead is also striking all the neighboring ships.",
                "3.8 Ракетная установка очень мощное вооружение физических технологий. Управляемые снаряды позволяют надёжно поражать противников, находящихся на большом расстоянии. При критическом эффекте разделяемая боеголовка также поражает и все соседние корабли.");
            AddText("BattleLesson3.9", "3.9 The shells of antimatter is based on the effect of disintegration, in which stands out just a huge amount of energy. When critical effect of particle of antimatter linger around the surface of the ship and interact with the shells, causing them to suicide bombing.",
                "3.9 Снаряды антиматерии основаны на эффекте дезинтеграции, при которой выделяется просто громадное количество энергии. При критическом эффекте частицы антиматерии задерживаются  около поверхности корабля и вступают во взаимодействие со снарядами, вызывая их самоподрыв. ");
            AddText("BattleLesson3.10", "3.10 The development of the psi weapons mankind has engaged in since the dawn of its existence. But only recently managed to make in this field a qualitative leap. When critical effect of psi weapon leaves in the subconscious of pilot violations that could cause him to shoot at their ships.",
                "3.10 Разработкой пси оружия человечество занималось с самой зари своего существования. Но только в недавнее время удалось совершить в этой области качественный прорыв. При критическом эффекте пси оружие оставляет в подсознании пилота нарушения, что может заставить его выстрелить по своим кораблям.");
            AddText("BattleLesson3.11", "3.11 Dark energy is truly inexhaustible fount of possibilities. But we just started making the first tentative steps in its development. The generator of dark energy is one of them. When critical effect of the irregular power of this weapon allows you to deal damage to the hull ignoring his armoring.",
                "3.11 Тёмная энергия – это поистине неисчерпаемый кладезь возможностей. Но мы только начали делать первые робкие шаги по её освоению. Генератор тёмной энергии – один из них. При критическом эффекте аномальная мощь этого оружия позволяет наносить повреждения корпусу корабля игнорируя его бронирование.");
            AddText("BattleLesson3.12", "3.12 The black hole generator is another development of Irregular technology. Ultra-small shells having a monstrous mass, are a very effective weapon. The critical effect of a black hole could create a wormhole and throw out the ship from the battle back to base.",
                "3.12 Генератор чёрных дыр – ещё одна разработка Аномальных технологий. Сверхмалые снаряды, обладающие при этом чудовищной массой, являются очень эффективным оружием. При критическом эффекте чёрная дыра может создать кротовую нору и выкинуть корабль из боя назад на базу.");
            AddText("BattleLesson3.13", "3.13 The time paradox generator – the most mysterious weapon available to mankind. Only very few scientists understand how it works. The critical effect of this weapon allows you to throw the time back and restoring the damage received by the hull of allies.",
                "3.13 Генератор временных парадоксов – самое загадочное оружие доступное человечеству. Только считанные единицы понимают как оно работает. Критический эффект этого оружия позволяет отбрасывать время назад, восстанавливая повреждения полученные корпусом союзников.");
            AddText("BattleLesson3.14", "3.14 The computer penetration system is a beacon of modern weapons. Remotely hacking computer systems of the ships you can achieve amazing results. The critical effect you can to sabotage the ship's systems and reduce their effectiveness by 20%.",
                "3.14 Система компьютерного проникновения является маяком современного оружия. Удалённо взламывая компьютерные системы кораблей можно достигнуть поразительных результатов. При критическом эффекте можно саботировать системы корабля и разом снизить их эффективность на 20%.");
            AddText("BattleLesson3.15", "3.15 The generator of radioactive radiation acts on the target by highly penetrating radiation, which is equally dangerous for the electronics, causing its degradation, and for the people. Critical effect this radiation causes radioactive contamination of the ship, long inflicting significant damage.",
                "3.15 Генератор радиоактивного излучения воздействует на цель высоко проникающим излучением, которое одинаково опасно и для электроники, вызывая его деградацию, так и для людей. При критическом эффекте это излучение вызывает радиоактивное заражение корабля, длительно нанося ему существенный урон.");
            AddText("BattleLesson3.16", "3.16 Unmanned impact spacecraft was taken on Board quite a long time, but now the development of artificial intelligence has allowed this weapon to gain a firm place in the Arsenal. Ultra-small robotic ships are extremely dangerous and omnipresent. At the critical effect they can sabotage the force shield of the ship, just disabling it.",
                "3.16 Беспилотные ударные аппараты были взяты на вооружение достаточно давно, но только сейчас разработка искусственного интеллекта позволила этому оружию занять прочное место в арсенале. Сверхмалые роботизированные корабли чрезвычайно опасны и вездесущи. При критическом эффекте они могут саботировать силовой щит корабля, просто отключив его.");
            AddText("BattleLesson3.17", "development of scientists. Making the object a magnetic field of enormous power can cause him great damage. In addition, with a critical effect of the vehicle can be simply deployed his most helpless part of it and quickly destroy it.",
                "3.17 Магнитное орудие – это новейшая разработка учёных. Наводя на объект магнитное поле огромной мощности можно наносить ему большие повреждения. Кроме  того, при критическом эффекте корабль можно попросту развернуть к себе его самой беззащитной частью и быстро его уничтожить.");
            AddText("BattleLesson4.0", "4.0 Group effects: On some ships can be fitted with equipment which propagates its effect to all nearby ally ships.The use of such ships in the right place can significantly affect the outcome of the battle.",
                "4.0 Групповые эффекты: На некоторых кораблях может быть установлено оборудование, которое распространяет свой эффект на все соседние дружеские корабли.Применение таких кораблей в нужном месте может существенно повлиять на исход боя.");
            AddText("BattleLesson4.1", "4.1 Group effects: .",
                "4.1 Групповые эффекты: .");
            AddText("BattleLesson5.0", "5.0 Information: When you hover the mouse over the ship in pop - up window displays brief information about the ship.",
                "5.0 Информация: При наведении указателя мышки на корабль во всплывающем окне отображается краткая информация о корабле.");
            AddText("BattleLesson5.1", "5.1 It includes a maximum strength of hull, force shield, capacity of energy generator and their regeneration.",
                "5.1 Она включает максимальную прочность корпуса, силового щита и запас энергии генератора и их регенерацию.");
            AddText("BattleLesson5.2", "5.2 Energy spent on a moving ship on one hex and its evasion from different damage types.",
               "5.2 Расход энергии генератора на перемещение на один хекс и уклонение от различных типов урона.");
            AddText("BattleLesson5.3", "5.3 The presence of immunity to critical effect and armoring from different damage types.",
               "5.3 Наличие иммунитетов к критическим эффектам и брони против различных типов урона.");
            AddText("BattleLesson5.4", "5.4 The cumulative damage of all equipped weapons on force shield and hull.",
               "5.4 Суммарный урон всех установленных орудий по силовому щиту и корпусу корабля.");
            AddText("BattleLesson6.0", "6.0 You also can see the panel with a detailed battle ship parameters by clicking on right mouse button.",
               "6.0 При нажатии правой кнопкой мыши по кораблю открывается панель с подробной информацией о боевых характеристиках корабля.");
            AddText("BattleLesson6.1", "6.1 In this panel you can see a numeric information about ship hull, force shield and generator energy.",
                "6.1 Здесь можно увидеть численные значения прочности корпуса, щита, энергии генератора.");
            AddText("BattleLesson6.2", "6.2 The hull armoring.",
                "6.2 Бронирование корпуса.");
            AddText("BattleLesson6.3", "6.3 The evasion from different damage type.",
                "6.3 Уклонение от различных типов урона.");
            AddText("BattleLesson6.4", "6.4 The energy spent on a moving, the required number of rounds to prepare for teleportation and the starting jump distance.",
                "6.4 Затраты энергии на перемещение корабля, необходимое число раундов на подготовку к телепортации  и начальную дальность прыжка. ");
            AddText("BattleLesson6.5", "6.5 Also, here you can see a detailed information about every weapon, including a damage to hull and shield, an energy spent for shooting and critical hit chance.",
                "6.5 Также здесь доступна подробная информация по каждому орудию, включающую урон по корпусу и щиту, затраты энергии на выстрел, точность и шанс срабатывания критического эффекта.");
            AddText("BattleLesson6.6", "6.6 On the both sides of the panel can be placed information about group bonuses and immunity.",
                "6.6 По сторонам панели может быть размещена информация о наличии групповых бонусов и иммунитетов.");
            /*
            AddText("Help_First_Title", "", "Приветствие");
            AddText("Help_First_1", "", 
                "Добро пожаловать в игру Управляй Галактикой. На календаре 2316 год. В недавнем времени технологии совершили огромный скачок, открыв телепортацию в пределах галактики и колонизацию планет. Вы - руководитель небольшого государства на территории Земли. Государство сильно ограничено территорией, но перед вами вся галактика и народ жаждет новых открытий, территорий и завоеваний.");
            AddText("Help_First_2", "",
                "Вверху экрана кнопки позволяют перемещаться по основным окнам управления вашего государства:");
            AddText("Help_First_3", "",
                "В Меню Государство - сосредоточена общая информация о государстве в целом и уведомления о текущих событиях. Кроме того в этом меню находится информация о вашем альянсе, когда вы в него вступите или создадите свой.");
            AddText("Help_First_4", "",
                "В меню Галактика  и Сектор можно увидеть звёзды, текущие события в галактике и перечень миссий, доступных вашим вооружённым силам. Хотя звёзд в галактике неисчислимое количество - не на всех из них есть потенциально подходящие для колонизации планеты. В центре карты в центральном секторе находится Солнце и солнечная система, где расположена наша родина Земля.");
            AddText("Help_First_5", "",
                "При выборе звезды открывается меню Система, в котором отображается текущая солнечная система.");
            AddText("Help_First_6", "",
                "В меню Колония - информация о ваших колониях, населению и постройках.");
            AddText("Help_First_7", "",
                "В меню Флот - информация о боевых флотах кораблей. Оттуда их можно отправлять на различные миссии или менять их поведение.");
            AddText("Help_First_8", "",
                "В меню Корабли - информация о кораблях. Там можно их ремонтировать, распределять по флотам, менять пилотов.");
            AddText("Help_First_9", "",
                "В меню Чертежи - можно компоновать из различных элементов чертеж корабля, а потом по ним строить корабли.");
            AddText("Help_First_10", "",
                "В меню Наука можно проводить исследования, выделяя финансы и ресурсы на различные направления, а также можно смотреть уже изученные технологии.");
            AddText("Help_First_11", "",
                "В меню Рынок можно менять одни ресурсы на другие, а также нанимать новых пилотов для кораблей.");
            AddText("Help_First_12", "",
                "Вверху экрана указана информация о наличии и максимальном количестве ресурсов, а также о наличии и максимальном количестве кораблей.");
            AddText("Help_First_13", "",
                "Всего есть 4 вида ресурса.");
            AddText("Help_First_14", "",
                " Первый ресурс - деньги. Это самый важный ресурс. Все операции требуют денег. Кроме того их не надо хранить, их количество может быть очень велико. Кроме того, на рынке все ресурсы меняются только через деньги.");
            AddText("Help_First_15", "",
                " Второй ресурс - металлы. Металлы очень распространены, но под их хранение надо выделять достаточно большие территории.");
            AddText("Help_First_16", "",
                " Третий ресурс - микросхемы. В наш век это очень важный ресурс для производства высокотехнологичного оборудования.");
            AddText("Help_First_17", "",
                "Четвёртый ресурс - антиматерия. Это очень редкий ресурс и используется в больших количествах при производстве редкого оборудования");
            AddText("Help_First_18", "",
                "Кроме того есть два ресурса, которые нельзя накапливать или транспортировать, они строго привязаны к возможностям колоний. Это вычислительная мощность колонии и ремонтный потенциал.");
            AddText("Help_First_19", "",
                " Вычислительная мощность увеличивает возможности флота. От этого показателя зависит количество кораблей во флоте, скорость его возвращения из боя, а также он влияет на некоторые возможности в бою.");
            AddText("Help_First_20", "",
                " Ремонтный потенциал показывает возможность и скорость ремонта кораблей, пострадавших в бою.");
               
            AddText("Help_Schemas_Title", "Schemas", "Чертежи");
            AddText("Help_Schemas_1","",
                "В этом меню можно скомпоновать чертёж корабля. Корабль состоит из нескольких обязательных модулей, и несколько необязательных. Модули могут отличаться по размеру, что влияет не только на численное значение характеристики,  но и также на получаемые дополнительные эффекты. Из совокупности характеристик всех модулей, а также бонусов дающихся пилотом, складывается боевая эффективность корабля. Корабль имеет следующие характеристики:");
            AddText("Help_Schemas_2", "",
                " Прочность корпуса - Текущая(1), максимальная(2), и регенерация(3). Это главная характеристика корабля. Как только текущая прочность становится равной нулю - корабль разрушается. Каждый ход здоровье корабля увеличивается на величину регенерации (если она есть). Однако в бою на корабль могут быть наложены долговременные эффекты, которые каждый ход снижают прочность корабля.");
            AddText("Help_Schemas_3", "",
               " Броня - параметр прямо влияющий на прочность. При получении урона - он снижается на величину (процентное) брони корабля. Броня не может превышать 90%. Кроме того, нажатием кнопки \"Броня\" можно поменять тип брони, увеличив сопротивляемость одним типам урона, и снизив остальным.");
            AddText("Help_Schemas_4", "",
                " Щит - Очень важная характеристика.  Он, обычно, выше прочности, регенерируется каждый раунд. Кроме того сбитые щиты автоматически восстанавливаются после боя и не требуют затрат на ремонт. Также щит не защищает от урона со всех сторон. Он может защищать как небольшую область перед кораблём так и защищать 5/6 корабля.");
            AddText("Help_Schemas_5", "",
                " Энергия - Параметр прямо влияющий на эффективность корабля. Практически все модули постоянно потребляют энергию. Кроме того, энергия расходуется в больших количествах при стрельбе и передвижении по полю боя.");
            AddText("Help_Schemas_6", "",
                " Уклонение - позволяет избегать урона. Чем выше уклонение, тем сложнее противнику попасть по кораблю. Также шанс попадания зависит от того, двигался ли корабль в данный раунд, от расстояния до цели, от меткости корабля, от общей точности оружия и от наличия препятствий.");
            AddText("Help_Schemas_7", "",
                " Урон по щиту, урон по броне - разные типы вооружения имеют разный урон по щиту корабля и по его броне.");
            AddText("Help_Schemas_8", "",
                " Точность - важный парметр. Показывает шанс на попадание данным оружием без учёта обстоятельств цели.");

            AddText("Help_Market_Title", "Market", "Рынок");
            AddText("Help_Market_1", "",
                "В этом меню можно получить доступ к обменнику ресурсов и к академии пилотов. Обменник позволяет выполнять продажу и покупку металлов, микросхем и антиматерии. Курс обмена является общим для всех государств и зависит от мирового спроса и предложения ресурсов. Максимальное количество ресурсов к обмену за одну операцию – 1 миллион.");
            AddText("Help_Market_2", "",
                "В академии пилотов можно нанимать новых пилотов для ваших боевых кораблей. Корабль не может сражаться без пилота. Пилоты, в зависимости от их успехах в учёбе, имеют разную боевую эффективность, которая выражается прибавкой к боевым характеристикам корабля. Всего может быть от 1 до 5 бонусов. Количество, эффективность и разнообразие бонусов зависит от характеристик пилота. Это специализация, ранг, талант и уровень. ");
            AddText("Help_Market_3", "",
                "Специализация характеризует область, в которой сконцентрированы навыки пилота. Соответственно она может быть энергетическая, физическая, аномальная кибернетическая и универсальная. Специализация отображается соответствующим цветом в эмблеме пилота. Количество бонусных навыков зависит от ранга пилота. Ранг отображается в эмблеме количеством объектов. При отображении одного объекта с полосами – у пилота открывается пятый навык при прокачке до 10 уровня.  ");
            AddText("Help_Market_4", "",
                "Талант влияет на эффективность бонусов. При низком навыке объекты на эмблеме представлены прямоугольниками, при среднем – галочками, при высоком – звёздами, при наивысшем – коронами. Уровень пилота также влияет на эффективность навыков. Уровень пилота растёт при получении им боевого опыта. Уровень на эмблеме никак не отражается. ");
            AddText("Help_Market_5", "",
                "Одновременно можно смотреть только четыре дела пилотов.  Получение новых дел пилотов требует затрат – 10 000 кредитов.  Найм же пилотов не требует денег, вы можете нанять всех, но вот место в казарме ограниченно.  В казарме вы можете посмотреть на пилотов или уволить тех кто не нужен. Пилот, приписанный к кораблю живёт не в казарме а в ангаре.");

            AddText("Help_Chat_Title", "Chat", "Чат");
            AddText("Help_Chat_1", "", 
                "В этом окне чата можно общаться с другими игроками. Сообщения бывают трех видов: общие, клановые и личные. Общие сообщения пишутся бех префиксов, клановые с префиксом /c и отображаются зелёным цветом. Личные сообщения пишутся через префикс /имя_игрока, или щелчком мыши по имени в чате. Отображаются розовым цветом");

            //AddText("NotEnemyAttackStart", " has enter in battle with Invasion`s fleet", " вступил в бой с флотом вторжения");
            */
        }
        static void AddText(string pos, string En, string Ru)
        {
            Links.InterfaceText.Add(pos, new string[] { En, Ru });
        }
        public static string GetDate(DateTime time)
        {
            if (Links.Lang == 0)
                return String.Format("{0} {1} {2}", Months[0, time.Month - 1], time.Day, time.Year);
            else
                return String.Format("{0} {1} {2}", time.Day, Months[1, time.Month - 1], time.Year);
        }
        static string[,] Months = GetMonths();
        static string[,] GetMonths()
        {
            string[, ] list = new string[2,12];
            list[0, 0] = "January"; list[1, 0] = "Января";
            list[0, 1] = "February"; list[1, 1] = "Февраля";
            list[0, 2] = "March"; list[1, 2] = "Марта";
            list[0, 3] = "April"; list[1, 3] = "Апреля";
            list[0, 4] = "May"; list[1, 4] = "Мая";
            list[0, 5] = "June"; list[1, 5] = "Июня";
            list[0, 6] = "July"; list[1, 6] = "Июля";
            list[0, 7] = "August"; list[1, 7] = "Августа";
            list[0, 8] = "September"; list[1, 8] = "Сентября";
            list[0, 9] = "October"; list[1, 9] = "Октября";
            list[0, 10] = "November"; list[1, 10] = "Ноября";
            list[0, 11] = "December"; list[1, 11] = "Декабря";
            return list;

        }
    }
}
