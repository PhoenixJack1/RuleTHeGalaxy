using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    class GameObjectName
    {
        static SortedList<int, ScienceName> BuildingsName = FillBuildingsNames();
        static SortedList<int, string[]> BuildingDescription = FillBuildingsDescription();
        static WeaponName[] WeaponNameRu = GetNamesRu();
        static WeaponName[] WeaponNameEn = GetNamesEn();
        static SortedList<EWeaponType, string[]> WeaponsDescription = FillWeaponsDescription();
        static SortedList<ShipGenerator2Types, string[]> ShipTypeDescription = FillShipTypeDescription();
        public static SortedList<int, string[]> GeneratorsName = FillGeneratorsNames();
        static SortedList<int, string[]> GeneratorsDescription = FillGeneratorsDescription();
        public static SortedList<int, string[]> ShieldsName = FillShieldsNames();
        static SortedList<int, string[]> ShieldsDescription = FillShieldsDescription();
        public static SortedList<int, string[]> ComputersName = FillComputersNames();
        public static SortedList<int, string[]> ComputersTargetName = FillComputerTargetNames();
        static SortedList<int, string[]> ComputersDescription = FillComputersDescription();
        public static SortedList<int, string[]> EnginesName = FillEnginesNames();
        static SortedList<int, string[]> EnginesDescription = FillEnginesDescription();
        static SortedList<int, string[]> EquipmentsName = FillEquipmentsNames();
        static SortedList<int, string[]> EquipmentsDescription = FillEquipmentsDescriptions();
        public static string GetNewLandsName(GameScience science)
        {
            return Links.Interface("NewLandName") + science.Level.ToString();
        }
        public static string GetnewLandDescription()
        {
            return Links.Interface("NewLandDescription");
        }
        public static string GetEquipmentName(Equipmentclass equip)
        {
            return equip.Name;
            //return string.Format("{0} {1}", EquipmentsName[equip.Type][Links.Lang], (equip.Level + 1).ToString());
        }
        public static string GetEngineName(Engineclass engine)
        {
            return string.Format("{0} {1}/{2}", EnginesName[engine.GetSizeDescription()][Links.Lang], engine.EnergyEvasion, engine.IrregularEvasion);
        }
        public static string GetComputerName(Computerclass computer)
        {
                int type = computer.GetType();
                string TargetModi = "";
                string SizeTypeModi = "";
                if (type == 0)
                {
                    if (computer.Size == ItemSize.Small) return string.Format("{0} {1}", ComputersName[0][Links.Lang], computer.Accuracy[0].ToString());
                    else if (computer.Size == ItemSize.Medium) return string.Format("{0} {1}", ComputersName[1][Links.Lang], computer.Accuracy[0].ToString());
                    else return string.Format("{0} {1}", ComputersName[2][Links.Lang], computer.Accuracy[0].ToString());
                }
                if (type == 1 || type == 5)
                    TargetModi = ComputersTargetName[0][Links.Lang];
                else if (type == 2 || type == 6)
                    TargetModi = ComputersTargetName[1][Links.Lang];
                else if (type == 3 || type == 7)
                    TargetModi = ComputersTargetName[2][Links.Lang];
                else
                    TargetModi = ComputersTargetName[3][Links.Lang];
                if (computer.Size == ItemSize.Small)
                {
                    if (type > 0 && type < 5)
                        SizeTypeModi = ComputersName[3][Links.Lang];
                    else
                        SizeTypeModi = ComputersName[6][Links.Lang];
                }
                else if (computer.Size == ItemSize.Medium)
                {
                    if (type > 0 && type < 5)
                        SizeTypeModi = ComputersName[4][Links.Lang];
                    else
                        SizeTypeModi = ComputersName[7][Links.Lang];
                }
                else
                {
                    if (type > 0 && type < 5)
                        SizeTypeModi = ComputersName[5][Links.Lang];
                    else
                        SizeTypeModi = ComputersName[8][Links.Lang];
                }
                return string.Format("{0} {1} {2}-{3}", TargetModi, SizeTypeModi, computer.GetAccuracy(), computer.GetDamage());

            
        }
        /*
        public static string GetComputerName(Computerclass computer)
        {
            return string.Format("{0} {1}/{2}", ComputersName[computer.GetSizeDescription()][Links.Lang], computer.EnergyAccuracy, computer.IrregularAccuracy);
        }
         */
        public static string GetShieldName(Shieldclass shield)
        {
            return string.Format("{0} {1}/{2}", ShieldsName[shield.GetSizeDescription()][Links.Lang], shield.Capacity, shield.Recharge);
        }
        public static string GetGeneratorName(Generatorclass generator)
        {
            return string.Format("{0} {1}/{2}", GeneratorsName[generator.GetSizeDescription()][Links.Lang], generator.Capacity, generator.Recharge);
        }
        public static string GetEquipmentDescription(Equipmentclass equip)
        {
            return EquipmentsDescription[equip.Type][Links.Lang];
        }
        static SortedList<int, string[]> FillEquipmentsDescriptions()
        {
            SortedList<int, string[]> list = new SortedList<int, string[]>();
            list.Add(0, new string[] { "Energy coprocessor", 
                "Энергетический сопроцессор увеличивает шанс попадания энергетическими видами вооружений." });
            list.Add(1, new string[] { "Ballistic coprocessor", 
                "Баллистический сопроцессор увеличивает шанс попадания физическими видами вооружений." });
            list.Add(2, new string[] { "Stochastic coprocessor", 
                "Вероятностный сопроцессор увеличивает шанс попадания аномальными видами вооружений." });
            list.Add(3, new string[] { "Mathematical coprocessor", 
                "Математический сопроцессор увеличивает шанс попадания кибернетическими видами вооружений." });
            list.Add(4, new string[] { "Universal coprocessor", 
                "Универсальный сопроцессор увеличивает шанс попадания всеми видами вооружений." });
            list.Add(5, new string[] { "Add-on armor module", 
                "Навесной броневой модуль увеличивает прочность корпуса корабля." });
            list.Add(6, new string[] { "Repairyng droid", 
                "Ремонтный дроид восстанавливает каждый ход небольшие повреждения корпуса." });
            list.Add(7, new string[] { "Resonator force module", 
                "Резонаторный силовой модуль увеличивает прочность силового щита." });
            list.Add(8, new string[] { "Force shield coil winding", 
                "Дополнительная силовая обмотка увеличивает регенерацию щита." });
            list.Add(9, new string[] { "Polymeric storage battery", 
                "Полимерный аккумулятор увеличивает максимальный запас энергии." });
            list.Add(10, new string[] { "Fuel elements", 
                "Топливные элементы увелчивают объем генерации энергии." });
            list.Add(11, new string[] { "Divergent trap kit", 
                "Комлект рассеивающих ловушек увеличивает уклонение от энергетического вооружения." });
            list.Add(12, new string[] { "Infrared trap kit", 
                "Комплект инфракрасных ловушек увеличивает уклонение от физического вооружения." });
            list.Add(13, new string[] { "Stochastic cloaking", 
                "Вероятностная маскировка увеличивает уклонение от аномальных видов вооружений." });
            list.Add(14, new string[] { "Noise jammer", 
                "Постановщик помех увеличивает уклонение от кибернетических видов вооружений." });
            list.Add(15, new string[] { "Reinforsment shunting nozzles", 
                "Усиленные маневровые дюзы увеличивают уклонение от всех видов вооружений." });
            list.Add(16, new string[] { "Reflecting elements", 
                "Отражательные элементы увеличивают сопротивляемость энергетическому урону." });
            list.Add(17, new string[] { "Composite armor", 
                "Композитная броня увеличивает сопротивляемость физическому урону." });
            list.Add(18, new string[] { "Stochastic screening", 
                "Вероятностное экранирование увеличивает сопротивляемость аномальному урону." });
            list.Add(19, new string[] { "Core screening", 
                "Экранированные кабели увеличивают сопротивляемость кибернетическому урону." });
            list.Add(20, new string[] { "Reinforcment construction", 
                "Усиленная конструкция увелчивает сопротивляемость любому урону." });
            list.Add(21, new string[] { "Condencer cell", 
                "Блок конденсаторов увеличивает урон энергетического вооружения." });
            list.Add(22, new string[] { "Graphene elements", 
                "Графеновые элементы увеличивает урон физического вооружения." });
            list.Add(23, new string[] { "Antimatter crystals", 
                "Кристаллы из антиматерии увеличивает урон аномального вооружения." });
            list.Add(24, new string[] { "Adaptive algoritms", 
                "Адаптивные алгоритмы увеличивают урон кибернетического вооружения." });
            list.Add(25, new string[] { "Weak points analyser", 
                "Анализатор уязвимых точек увеличивает урон от всех видов вооружений." });
            list.Add(30, new string[] { "Group add-on armor module", 
                "Групповой навесной броневой модуль увеличивает прочность корпуса для всех соседних кораблей." });
            list.Add(31, new string[] { "Group repairyng droid", 
                "Групповой ремонтный дроид восстанавливает часть повреждений для всех соседних кораблей." });
            list.Add(32, new string[] { "Group resonator force module", 
                "Групповой резонаторный силовой модуль увеличиват прочность силового щита для всех соседних кораблей." });
            list.Add(33, new string[] { "Group force shield coil winding", 
                "Групповая дополнительная силовая обмотка увеличивает регенерацию силового щита для всех соседних кораблей." });
            list.Add(34, new string[] { "Group polymeric storage battery", 
                "Групповой полимерный аккумулятор увеличивает запас энергии для всех соседних кораблей." });
            list.Add(35, new string[] { "Group fuel elements", 
                "Групповые топливные элементы увеличивают генерацию энергии для всех соседних кораблей." });
            list.Add(36, new string[] { "Group energy coprocessor", 
                "Групповой энергетический сопроцессор увеличивает точность энергетического вооружения для всех соседних кораблей." });
            list.Add(37, new string[] { "Group ballistic coprocessor", 
                "Групповой баллистический сопроцессор увеличивает точность физических атак для всех соседних кораблей." });
            list.Add(38, new string[] { "Group stochastic coprocessor", 
                "Групповой вероятностный сопроцессор увеличивает точность аномальных атак для всех соседних кораблей." });
            list.Add(39, new string[] { "Group mathematical coprocessor", 
                "Групповой математический сопроцессор увеличивает точность кибернетических атак для всех соседних кораблей." });
            list.Add(40, new string[] { "Group universal coprocessor", 
                "Групповой универсальный сопроцессор увеличивает точность любого вооружени для всех соседних кораблей." });
            list.Add(41, new string[] { "Group divergent trap kit", 
                "Групповой комлект рассеивающих ловушек увеличивает уклонение от энергетических атак для всех соседних кораблей." });
            list.Add(42, new string[] { "Group infrared trap kit", 
                "Групповой комплект инфракрасных ловушек увеличивает уклонение от физических атак для всех соседних кораблей." });
            list.Add(43, new string[] { "Group stochastic cloaking", 
                "Групповая вероятностная маскировка увеличивает уклонение от аномальных атак для всех соседних кораблей." });
            list.Add(44, new string[] { "Group noise jammer", 
                "Групповой постановщик помех увеличивает уклонение от кибернетических атак для всех соседних кораблей." });
            list.Add(45, new string[] { "Group reinforsment shunting nozzles", 
                "Групповые усиленные маневровые дюзы увеличивают уклонение от всех видов атак для всех соседних кораблей." });
            list.Add(46, new string[] { "Group reflecting elements", 
                "Групповые отражательные элементы увеличивают сопротивляемость энергетическому вооружению для всех соседних кораблей." });
            list.Add(47, new string[] { "Group composite armor", 
                "Групповая композитная броня увеличивает сопротивляемость физическим атакам для всех соседних кораблей." });
            list.Add(48, new string[] { "Group stochastic screening", 
                "Групповое вероятностное экранирование увеличивает сопротивляемость аномальным атакам для всех соседних кораблей." });
            list.Add(49, new string[] { "Group core screening", 
                "Групповые экранированные кабели увеличивают сопротивляемость кибернетическим атакам для всех соседних кораблей." });
            list.Add(50, new string[] { "Group reinforcment construction", 
                "Групповая усиленная конструкция увеличивает сопротивляемость всем видам атак для всех соседних кораблей." });
            list.Add(51, new string[] { "Group condencer cell", 
                "Групповой блок конденсаторов увеличивает урон от энергетических видов вооружений для всех соседних кораблей." });
            list.Add(52, new string[] { "Group graphene elements", 
                "Групповые графеновые элементы увеличивают урон от физических видов вооружений для всех соседних кораблей." });
            list.Add(53, new string[] { "Group antimatter crystals", 
                "Групповые кристаллы из антиматерии увеличивают урон от аномальных видов вооружений для всех соседних кораблей." });
            list.Add(54, new string[] { "Group adaptive algoritms", 
                "Групповые адаптивные алгоритмы увеличивают урон от кибернетических видов вооружений для всех соседних кораблей." });
            list.Add(55, new string[] { "Group weak points analyser", 
                "Групповой анализатор уязвимых точек увеличивает урон от всех видов вооруженний для всех соседних кораблей." });
            list.Add(57, new string[] { "Cargo module", 
                "Грузовой модуль позволяет перевозить грузы, в частности позволяют собирать ресуры с поля боя, месторождений и вражеских складов." });
            list.Add(58, new string[] { "Colony module", 
                "Колонизационный модуль содержит всё необходимое для начала колонизации планеты." });
            list.Add(59, new string[] { "Energical disturbance absorber", 
                "Поглотитель энергетических возмущений блокирует развитие критических эффектов от энергетических видов вооружений." });
            list.Add(60, new string[] { "Physical disturbance absorber", 
                "Поглотитель физичесих возумущений блокирует развитие критических эффектов от физических вилдов вооружений." });
            list.Add(61, new string[] { "Irregular disturbance absorber", 
                "Поглотитель аномальных возмущений блокирует развитие критических эффектов от аномальных видов вооружений." });
            list.Add(62, new string[] { "Cybernetic disturbance absorber", 
                "Поглотитель кибернетических возмущений блокирует развитие критических эффектов от кибернетических видов вооружений." });
            list.Add(63, new string[] { "Unversal disturbance absorber", 
                "Универсальный поглотитель возмущений блокирует развитие критических эффектов от всех видов вооружений." });
            list.Add(64, new string[] {"Group energical disturbance absorber",
            "Групповой поглотитель энергетических возмущений блокирует развитие критических эффектов от энергетических вооружений для всех соседних кораблей"});
            list.Add(65, new string[] {"Group physical disturbance absorber",
            "Групповой поглотитель физических возмущений блокирует развитие критических эффектов от физических вооружений для всех соседних кораблей"});
            list.Add(66, new string[] {"Group irregular disturbance absorber",
            "Групповой поглотитель аномальных возмущений блокирует развитие критических эффектов от аномальных вооружений для всех соседних кораблей"});
            list.Add(67, new string[] {"Group cybernetic disturbance absorber",
            "Групповой поглотитель кибернетических возмущений блокирует развитие критических эффектов от кибренетических вооружений для всех соседних кораблей"});
            list.Add(68, new string[] {"Group universal disturbance absorber",
            "Групповой универсальный поглотитель возмущений блокирует развитие критических эффектов от всех видов вооружений для всех соседних кораблей"});

            return list;
        }
        static SortedList<int, string[]> FillEquipmentsNames()
        {
            SortedList<int, string[]> list = new SortedList<int, string[]>();
            list.Add(0, new string[] { "Energy coprocessor", "Энергетический сопроцессор" });
            list.Add(1,new string[]{"Ballistic coprocessor","Баллистический сопроцессор"});
            list.Add(2,new string[]{"Stochastic coprocessor","Вероятностный сопроцессор"});
            list.Add(3,new string[]{"Mathematical coprocessor","Математический сопроцессор"});
            list.Add(4,new string[]{"Universal coprocessor","Универсальный сопроцессор"});
            list.Add(5,new string[]{"Add-on armor module","Навесной броневой модуль"});
            list.Add(6,new string[]{"Repairyng droid","Ремонтный дроид"});
            list.Add(7,new string[]{"Resonator force module","Резонаторный силовой модуль"});
            list.Add(8,new string[]{"Force shield coil winding","Дополнительная силовая обмотка"});
            list.Add(9,new string[]{"Polymeric storage battery","Полимерный аккумулятор"});
            list.Add(10,new string[]{"Fuel elements","Топливные элементы"});
            list.Add(11,new string[]{"Divergent trap kit","Комлект рассеивающих ловушек"});
            list.Add(12,new string[]{"Infrared trap kit","Комплект инфракрасных ловушек"});
            list.Add(13,new string[]{"Stochastic cloaking","Вероятностная маскировка"});
            list.Add(14,new string[]{"Noise jammer","Постановщик помех"});
            list.Add(15,new string[]{"Reinforsment shunting nozzles","Усиленные маневровые дюзы"});
            list.Add(16,new string[]{"Reflecting elements","Отражательные элементы"});
            list.Add(17,new string[]{"Composite armor","Композитная броня"});
            list.Add(18,new string[]{"Stochastic screening","Вероятностное экранирование"});
            list.Add(19,new string[]{"Core screening","Экранированные кабели"});
            list.Add(20,new string[]{"Reinforcment construction","Усиленная конструкция"});
            list.Add(21,new string[]{"Condencer cell","Блок конденсаторов"});
            list.Add(22,new string[]{"Graphene elements","Графеновые элементы"});
            list.Add(23,new string[]{"Antimatter crystals","Кристаллы из антиматерии"});
            list.Add(24,new string[]{"Adaptive algoritms","Адаптивные алгоритмы"});
            list.Add(25,new string[]{"Weak points analyser","Анализатор уязвимых точек"});
            list.Add(30, new string[] { "Group add-on armor module", "Групповой навесной броневой модуль" });
            list.Add(31, new string[] { "Group repairyng droid", "Групповой ремонтный дроид" });
            list.Add(32, new string[] { "Group resonator force module", "Групповой резонаторный силовой модуль" });
            list.Add(33, new string[] { "Group force shield coil winding", "Групповая дополнительная силовая обмотка" });
            list.Add(34, new string[] { "Group polymeric storage battery", "Групповой полимерный аккумулятор" });
            list.Add(35, new string[] { "Group fuel elements", "Групповые топливные элементы" });
            list.Add(36, new string[] { "Group energy coprocessor", "Групповой энергетический сопроцессор" });
            list.Add(37, new string[] { "Group ballistic coprocessor", "Групповой баллистический сопроцессор" });
            list.Add(38, new string[] { "Group stochastic coprocessor", "Групповой вероятностный сопроцессор" });
            list.Add(39, new string[] { "Group mathematical coprocessor", "Групповой математический сопроцессор" });
            list.Add(40, new string[] { "Group universal coprocessor", "Групповой универсальный сопроцессор" });
            list.Add(41, new string[] { "Group divergent trap kit", "Групповой комлект рассеивающих ловушек" });
            list.Add(42, new string[] { "Group infrared trap kit", "Групповой комплект инфракрасных ловушек" });
            list.Add(43, new string[] { "Group stochastic cloaking", "Групповая вероятностная маскировка" });
            list.Add(44, new string[] { "Group noise jammer", "Групповой постановщик помех" });
            list.Add(45, new string[] { "Group reinforsment shunting nozzles", "Групповые усиленные маневровые дюзы" });
            list.Add(46, new string[] { "Group reflecting elements", "Групповые отражательные элементы" });
            list.Add(47, new string[] { "Group composite armor", "Групповая композитная броня" });
            list.Add(48, new string[] { "Group stochastic screening", "Групповое вероятностное экранирование" });
            list.Add(49, new string[] { "Group core screening", "Групповые экранированные кабели" });
            list.Add(50, new string[] { "Group reinforcment construction", "Групповая усиленная конструкция" });
            list.Add(51, new string[] { "Group condencer cell", "Групповой блок конденсаторов" });
            list.Add(52, new string[] { "Group graphene elements", "Групповые графеновые элементы" });
            list.Add(53, new string[] { "Group antimatter crystals", "Групповые кристаллы из антиматерии" });
            list.Add(54, new string[] { "Group adaptive algoritms", "Групповые адаптивные алгоритмы" });
            list.Add(55, new string[] { "Group weak points analyser", "Групповой анализатор уязвимых точек" });
            list.Add(57, new string[] { "Cargo module", "Грузовой модуль" });
            list.Add(58, new string[] { "Colony module", "Колонизационный модуль" });
            list.Add(59, new string[] { "Energical disturbance absorber", "Поглотитель энергетических возмущений" });
            list.Add(60, new string[] { "Physical disturbance absorber", "Поглотитель физичесих возумущений" });
            list.Add(61, new string[] { "Irregular disturbance absorber", "Поглотитель аномальных возмущений" });
            list.Add(62, new string[] { "Cybernetic disturbance absorber", "Поглотитель кибернетических возмущений" });
            list.Add(63, new string[] { "Unversal disturbance absorber", "Универсальный поглотитель возмущений" });
            list.Add(64, new string[] { "Group energical disturbance absorber", "Групповой поглотитель энергетических возмущений" });
            list.Add(65, new string[] { "Group physical disturbance absorber", "Групповой поглотитель физических возмущений" });
            list.Add(66, new string[] { "Group irregular disturbance absorber", "Групповой поглотитель аномальных возмущений" });
            list.Add(67, new string[] { "Group cybernetic disturbance absorber", "Групповой поглотитель кибернетических возмущений" });
            list.Add(68, new string[] { "Group universal disturbance absorber", "Групповой универсальный кибернетических возмущений" });
            return list;
        }
        public static string GetEngineDescription(Engineclass engine)
        {
            return EnginesDescription[engine.GetSizeDescription()][Links.Lang];
        }
        static SortedList<int, string[]> FillEnginesDescription()
        {
            SortedList<int, string[]> list = new SortedList<int, string[]>();
            list.Add(5, new string[] { "Chemical engine", 
                "Химический двигатель устанавливается в малый слот. Обеспечивает уклонение от традиционных технологий. Расходует на перемещение 60% энергии за один хекс." });
            list.Add(0, new string[] { "Magnet engine", 
                "Магнитный двигатель устанавливается в малый слот. Обеспечивает небольшое уклонение от всех видов орудий. Расходует на перемещение 60% энергии за один хекс." });
            list.Add(15, new string[] { "Ion engine", 
                "Ионный двигатель устанавливается в средний слот. Обеспечивает среднее уклонение от традиционных технологий. Расходует 40% энергии при перемещении на один хекс." });
            list.Add(10, new string[] { "Phase engine", 
                "Фазовый двигатель устанавливается в средний слот. Обеспечивает среднее уклонение от всех видов урона. Расходует 40% энергии при перемещении на один хекс." });
            list.Add(25, new string[] { "Plasmic engine", 
                "Плазменный двигатель устанавливается в большой слот. Обеспечивает высокое уклонение от традиционных видов урона. Расходует 20% энергии при перемещении на один хекс." });
            list.Add(20, new string[] { "Antigravitation engine", 
                "Антигравитационный двигатель устанавливается в большой слот. Обеспечивает высокое уклонение от всех видов урона. Расходует 20% энергии при перемещении на один хекс." });
            return list;
        }
        static SortedList<int, string[]> FillEnginesNames()
        {
            SortedList<int, string[]> list = new SortedList<int, string[]>();
            list.Add(5, new string[] { "Chemical engine", "Химический двигатель" });
            list.Add(0, new string[] { "Magnet engine", "Магнитный двигатель" });
            list.Add(15, new string[] { "Ion engine", "Ионный двигатель" });
            list.Add(10, new string[] { "Phase engine", "Фазовый двигатель" });
            list.Add(25, new string[] { "Plasmic engine", "Плазменный двигатель" });
            list.Add(20, new string[] { "Antigravitation engine", "Антигравитационный двигатель" });
            return list;
        }
        static SortedList<int, string[]> FillComputerTargetNames()
        {
            SortedList<int, string[]> list = new SortedList<int, string[]>();
            list.Add(0, new string[] { "Energy", "Энергетический" });
            list.Add(1, new string[] { "Physic", "Физический" });
            list.Add(2, new string[] { "Irregular", "Аномальный" });
            list.Add(3, new string[] { "Cyber", "Кибернетический" });
            return list;
        }
        static SortedList<int, string[]> FillComputersNames()
        {
            SortedList<int, string[]> list = new SortedList<int, string[]>();
            list.Add(0, new string[] { "Aim computer Silverum", "Вычислитель Силверум" });
            list.Add(1, new string[] { "Aim computer Aurum", "Вычислитель Аурум" });
            list.Add(2, new string[] { "Aim computer Platinum", "Вычислитель Платинум" });
            list.Add(3, new string[] { "aim computer Helium", "вычислитель Гелиум" });
            list.Add(4, new string[] { "aim computer Lithium", "вычислитель Литиум" });
            list.Add(5, new string[] { "aim computer Uranium", "вычислитель Ураниум" });
            list.Add(6, new string[] { "aim computer Joktum", "вычислитель Йоктум" });
            list.Add(7, new string[] { "quantum aim computer", "квантовый вычислитель" });
            list.Add(8, new string[] { "aim computer with AI", "вычислитель с ИИ" });
            return list;
        }
         
        public static string GetComputerDescription(Computerclass computer)
        {
            if (Links.Lang == 0)
                return "Aim computer description";
            else
                return "Вычислитель очень полезное приспособление. За счёт достижений компьютерных технологий позволяет серьёъно усилить боевые возможности корабля в области прицеливаниия и нанесения к урона";
            //return ComputersDescription[computer.GetSizeDescription()][Links.Lang];
        }
        static SortedList<int, string[]> FillComputersDescription()
        {
            SortedList<int, string[]> list = new SortedList<int, string[]>();
            list.Add(5, new string[] { "Aim computer", 
                "Вычислитель - базовое прицельное приспособление. Требует для установки слот минимального размера. Обеспечивает прицеливание только традиционными технологиями. При выполнении прыжка на поле боя позволяет переместиться только рядом с порталом" });
            list.Add(0, new string[] { "Aim computer Krypton", 
                "Вычислитель Криптон устанавливается в малый слот. Обеспечивает равное прицеливание всеми типами технологий. Прыжок только в район портала." });
            list.Add(15, new string[] { "Aim computer Platinum", 
                "Вычислитель Платинум устанавливается в средний слот. Обеспечивает прицеливание преимущественно базовыми технологиями. Прыжок в радиусе двух клеток от портала." });
            list.Add(10, new string[] { "Aim computer Lithium", 
                "Вычислитель Литиум устанавливается в средний слот. Обеспечивает прицеливание всеми технологиями равномерно. Прыжок в радиусе двух клеток от портала." });
            list.Add(25, new string[] { "Aim computer with AI", 
                "Вычислитель с ИИ устанавливается только в большой слот. Обеспечивает максимальную точность базовыми технологиями. Прыжок в радиусе трех клеток от портала." });
            list.Add(20, new string[] { "Quantum aim computer", 
                "Квантовый вычислитель устанавливается в большой слот. Обеспечивает равномерно высокое прицеливание всеми технологиями. Прыжок в радиусе трех клеток от портала." });
            return list;
        }
        static SortedList<int, string[]> FillShieldsNames()
        {
            SortedList<int, string[]> list = new SortedList<int, string[]>();
            list.Add(5, new string[] { "Basic force shield", "Базовый силовой щит" });
            list.Add(0, new string[] { "Pointed force shield", "Точечный силовой щит" });
            list.Add(15, new string[] { "Standard force shield", "Стандартный силовой щит" });
            list.Add(10, new string[] { "Impulse force shield", "Импульсный силовой щит" });
            list.Add(25, new string[] { "Impruved force shield", "Улучшенный силовой щит" });
            list.Add(20, new string[] { "Tesla force shield", "Силовой щит Тесла" });
            return list;
        }
        public static string GetShieldDescription(Shieldclass shield)
        {
            return ShieldsDescription[shield.GetSizeDescription()][Links.Lang];
        }
        static SortedList<int, string[]> FillShieldsDescription()
        {
            SortedList<int, string[]> list = new SortedList<int, string[]>();
            list.Add(5, new string[] { "Basic force shield", 
                "Базовый силовой щит - устанавливается в самый маленький слот. Обеспечивает минимальную регенерацию щита и небольшую его прочность. Обеспечивает только лобовую защиту." });
            list.Add(0, new string[] { "Pointed force shield", 
                "Точечный силовой щит устанваливатся в малый слот. Обеспечивает среднюю регенирацию прочности щита и минимальную его прочность. Защита только лобовой области." });
            list.Add(15, new string[] { "Standard force shield", 
                "Стандартный силовой щит - устанавливается в средний слот. Обеспечивает серьъёзную прочность щита при небольшой регенерации. Блокирует урон со всей передней полусферы." });
            list.Add(10, new string[] { "Impulse force shield", 
                "Импульсный силовой щит - устанавливается в средний слот. Обеспечивает высокую регенерацию щита при невысокой прочности. Защита передей полусферы." });
            list.Add(25, new string[] { "Impruved force shield", 
                "Улучшенный силовой щит - требует самый большой слот. Обеспечивает максимальную прочность щита и приличную регенерацию. Защита практически со всех направлений." });
            list.Add(20, new string[] { "Tesla force shield", 
                "Силовой щит Тесла - устанавливается в большой слот. Обеспечивает максимальную регенерацию щита при высокой его прочности. Защита практически со всех направлений." });
            return list;
        }
        static SortedList<int, string[]> FillGeneratorsNames()
        {
            SortedList<int, string[]> list = new SortedList<int, string[]>();
            list.Add(5, new string[] { "Iridium power generator", "Иридиевый генератор энергии" });
            list.Add(0, new string[] { "Uranium power generator", "Урановый генератор энергии" });
            list.Add(15, new string[] { "Polimer power generator", "Полимерный генератор энергии" });
            list.Add(10, new string[] { "Torium power generator", "Ториевый генератор энергии" });
            list.Add(25, new string[] { "Dark matter power generator", "Генератор энергии на тёмной материи" });
            list.Add(20, new string[] { "Hydrogen power generator", "Термоядерный генератор энергии" });
            return list;
        }
        public static string GetGeneratorDescription(Generatorclass generator)
        {
            return GeneratorsDescription[generator.GetSizeDescription()][Links.Lang];
        }
        static SortedList<int, string[]> FillGeneratorsDescription()
        {
            SortedList<int, string[]> list = new SortedList<int, string[]>();
            list.Add(5, new string[] { "Iridium power generator", 
                "Иридиевый генератор энергии устанавливается в самый маленький слот и обеспечивает самые базовые портребности в энергии. Время, потребное на накопление энергии для прыжка на поле составляет три хода." });
            list.Add(0, new string[] { "Uranium power generator", 
                "Урановый генератор энергии устанавливается в малый слот. Обеспечивает приемлимую регенрацию энергии, хотя встроенные батареи самые маленькие на уровне. Задержка до прыжка составляет три хода." });
            list.Add(15, new string[] { "Polimer power generator", 
                "Полимерный генератор энергии устанавливается в средний слот и позволяет хранить большие запасы энергии, однако регенерация невелика. Задержка до прыжка составляет два хода." });
            list.Add(10, new string[] { "Torium power generator", 
                "Ториевый генератор энергии устанавливается в средний слот вырабатывает высокий объём энергии, батареи также не заставляют беспокоится. Задержка до прыжка составляет два хода." });
            list.Add(25, new string[] { "Dark matter power generator", 
                "Генератор энергии на тёмной материи требует большой слот и позволяет хранить максимальные запасы энергии и регенерация находится на высоком уровне. Задержка до прыжка составляет один ход" });
            list.Add(20, new string[] { "Hydrogen power generator", 
                "Термоядерный генератор энергии - самый лучший генератор в плане выработки энергии. Запасы также велики. Задержка до прыжка составляет один ход. Требует большой слот." });
            return list;
        }
        public static string GetShipTypeDescription(ShipTypeclass shiptype)
        {
            return ShipTypeDescription[shiptype.Type][Links.Lang];
        }
        static SortedList<ShipGenerator2Types, string[]> FillShipTypeDescription()
        {
            SortedList<ShipGenerator2Types, string[]> result = new SortedList<ShipGenerator2Types, string[]>();
            result.Add(ShipGenerator2Types.Scout, new string[]{"English description of scout",
                "Разведчик - быстрый, дешёвый и неплохо защищённый корабль, в умелых руках способен наносить разящие удары в самые неожиданные для противника места."});
            result.Add(ShipGenerator2Types.Corvett, new string[]{"English description of small support",
                "Корвет - это малый корабль поддержки. Он мало что может в одиночку, однако наличие большого грузового модуля позволяет оказывать большое влияние на другие корабли флота."});
            result.Add(ShipGenerator2Types.Cargo, new string[]{"English description of transport",
                "Транспортный корабль может нести много грузовых модулей при очень невысокой цене, кроме того он неплохо защищён и может принести пользу на разных участках боя."});
            result.Add(ShipGenerator2Types.Linkor, new string[]{"English description of medium tank",
                "Линкор - это ваша передовая линия обороны. Тяжёлые щиты, толстая броня и два орудия позволят переломить неудачно складывающийся бой."});
            result.Add(ShipGenerator2Types.Frigate, new string[]{"English description of medium support",
                "Преимущественная роль фригатов - это корабли поддержки. Однако он может не только усиливать соседей но и вести достаточно неплохой огонь с дальней дистанции."});
            result.Add(ShipGenerator2Types.Fighter, new string[]{"English description of fast damage ship",
                "Истребитель это два орудия и мощный движок. Гроза врагов любящих дальний бой. Однако заранее продумайте куда он сможет скрыться в случае если блицкриг неудался"});
            result.Add(ShipGenerator2Types.Dreadnought, new string[]{"English description of heavy damage ship",
                "Дредноут - это медленный атакующий корабль - хорошее соотношение цены, брони и огневой мощи. "});
            result.Add(ShipGenerator2Types.Devostator, new string[]{"English description of glass cannon",
                "Девостатор хорошо характеризует понятие \"Стеклянная пушка\". Максимальный урон и точность в ущерб защищённости. Но и цена высока, и тактические ошибки не прощает."});
            result.Add(ShipGenerator2Types.Warrior, new string[]{"English description of warrior",
                "Воин - универсальный корабль. Хорошая защищённость и мобильность, вкупе с очень высоким уроном позволяют вести бой на любом участке. Цена соответствует."});
            result.Add(ShipGenerator2Types.Cruiser, new string[]{"English description of heavy tank",
                "Крейсер - это тяжёлый танк - и этим всё сказано. Передний край обороны, прорыв вражеских позиций - это его стихия. Не теряйте понапрасну."});

            return result;
        }
        public static string GetWeaponDescription(Weaponclass weapon)
        {
            return WeaponsDescription[weapon.Type][Links.Lang];
        }
        static SortedList<EWeaponType, string[]> FillWeaponsDescription()
        {
            SortedList<EWeaponType, string[]> result = new SortedList<EWeaponType, string[]>();
            result.Add(EWeaponType.Laser, new string[]{"English description of Laser",
                "Ослепляет системы наведения противника и его меткость снижается на два раунда."});
            result.Add(EWeaponType.EMI, new string[]{"English description of EMI",
                "Восстанавливает энергию всех соседних дружественных кораблей."});
            result.Add(EWeaponType.Plasma, new string[]{"English description of Plasma",
                "Увеличивает наносимый урон в два раза. Не блокируется иммунитетом."});
            result.Add(EWeaponType.Solar, new string[]{"English description of Solar",
                "Уничтожает цель с одного выстрела."});
            result.Add(EWeaponType.Cannon, new string[]{"English description of Cannon",
                "Наносит пилоту контузию и корабль пропустит два раунда."});
            result.Add(EWeaponType.Gauss, new string[]{"English description of Gauss",
                "Наносит урон напрямую корпусу сквозь щит."});
            result.Add(EWeaponType.Missle, new string[]{"English description of Missle",
                "Осколками повреждает все соседние корабли противника. Не блокируется иммунитетом."});
            result.Add(EWeaponType.AntiMatter, new string[]{"English description of AntiMatter",
                "Оставляет частички антиматерии на корпусе корабля, что может привести к детонации снарядов ещё в стволе орудий."});
            result.Add(EWeaponType.Psi, new string[]{"English description of Psi",
                "Затуманивает разум пилота, что может привести к выстрелу по союзнику."});
            result.Add(EWeaponType.Dark, new string[]{"English description of Dark weapon",
                "Повреждает корпус и механизмы игнорируя броню корабля. Не блокируется иммунитетом."});
            result.Add(EWeaponType.Warp, new string[]{"English description of Warp weapon",
                "Перемещает корабль с поля боя назад на его базу."});
            result.Add(EWeaponType.Time, new string[]{"English description of Time weapon",
                "Оказывает уникальный темпоральный эффект на все системы корабей союзников, в результате чего они восстанавливаются от повреждений."});
            result.Add(EWeaponType.Slicing, new string[]{"English description of Slice weapon",
                "Позволяет повредить используемое программное обеспечение и понизить щит, броню и запасы энергии сразу на 20%."});
            result.Add(EWeaponType.Radiation, new string[]{"English description of Radiation weapon",
                "Накладывает радиоактивное заражение системам корабля что приводит к долговременному урону здоровья пилота и деградации систем корабля."});
            result.Add(EWeaponType.Drone, new string[]{"English description of Drone weapon",
                "Оставляет маяк на корпусе цели, увеличивая шанс попадания всеми союзниками."});
            result.Add(EWeaponType.Magnet, new string[]{"English descrition of Magnet weapon",
                "Наводит магнитные поля на врага и разворачивает его к себе самой незащищённой частью. Не блокируется иммунитетом."});
            return result;
        }
        public static string GetWeaponName(Weaponclass weapon)
        {
            WeaponName name;
            if (Links.Lang == 0)
                name = WeaponNameEn[(int)weapon.Type];
            else
                name = WeaponNameRu[(int)weapon.Type];
            return String.Format("{1} {0} {2:0.#}{3}", name.SizeName[(int)weapon.Size], name.Title, weapon.Damage * name.PowerModi, name.PowerName);
        }
        static WeaponName[] GetNamesRu()
        {
            WeaponName[] result = new WeaponName[16];
            result[0] = new WeaponName("Лазер", "кВт", "468нм", "567нм", "775нм", 1);
            result[1] = new WeaponName("ЭМИ", "ГГц", "0.1с", "0.3с", "0.8с", 1);
            result[2] = new WeaponName("Плазма", "кК", "Al", "Cu", "Ag", 1);
            result[3] = new WeaponName("Солнечная пушка", "Тлм", "G-класса", "А-класса", "O-класса", 1);
            result[4] = new WeaponName("Орудие", "мм", "осколочное", "бронебойное", "кумулятивное", 1);
            result[5] = new WeaponName("Гаусс пушка", "г", "Fe", "Tb", "Ho", 1);
            result[6] = new WeaponName("Ракета", "ктн", "разрывная", "кумулятивная", "с разделяющейся боеголовкой", 1);
            result[7] = new WeaponName("Генератор антиматерии", "мг", "водородный", "дейтериевый", "тритиевый", 1);
            result[8] = new WeaponName("Пси пушка", "кН", "альфа", "бета", "гамма", 1);
            result[9] = new WeaponName("Генератор тёмной энергии", "МтДж", "двойной", "тройной", "четверной", 1);
            result[10] = new WeaponName("Варп генератор", "Мтн", "3м", "5м", "8м", 1);
            result[11] = new WeaponName("Временная пушка", "сек", "15м", "25м", "40м", 1);
            result[12] = new WeaponName("Хакерская установка", "ГБ/сек", "2нм", "1нм", "квантовая", 1);
            result[13] = new WeaponName("Генератор радиации", "Гр", "200Бк", "300Бк", "500Бк", 1);
            result[14] = new WeaponName("БПЛА", "Mk", "легкосплавный", "титановый", "композитный", 1);
            result[15] = new WeaponName("Магнитная пушка", "Тл", "8МН", "24МН", "76МН", 1);
            return result;
        }
        static WeaponName[] GetNamesEn()
        {
            WeaponName[] result = new WeaponName[16];
            result[0] = new WeaponName("Laser", "kW", "468nm", "467nm", "775nm", 1);
            result[1] = new WeaponName("EMI", "GHz", "0,1s", "0,3s", "0.8s", 1);
            result[2] = new WeaponName("Plasma", "kK", "Al", "Cu", "Ag", 1);
            result[3] = new WeaponName("Solar gun", "Tlm", "G-class", "A-class", "O-class", 1);
            result[4] = new WeaponName("Cannon", "mm", "fragmentation", "armour-piercing", "cumulative", 1);
            result[5] = new WeaponName("Gauss weapon", "g", "Fe", "Tb", "Ho", 1);
            result[6] = new WeaponName("Missle", "ktn", "explosive", "cumulative", "with separable warhead", 1);
            result[7] = new WeaponName("Antimatter generator", "мг", "hydrogen", "deuterium", "tritium", 1);
            result[8] = new WeaponName("Psi gun", "kN", "alpha", "beta", "gamma", 1);
            result[9] = new WeaponName("Dark energy generator", "MdJ", "double", "triple", "quadruple", 1);
            result[10] = new WeaponName("Warp generator", "Mtn", "3m", "5m", "8m", 1);
            result[11] = new WeaponName("Time weapon", "sec", "15m", "25m", "40m", 1);
            result[12] = new WeaponName("Slice weapon", "GB/sec", "2nm", "1nm", "quantum", 1);
            result[13] = new WeaponName("Radiation generator", "Gr", "200Bk", "300Bk", "500Bk", 1);
            result[14] = new WeaponName("UAV", "Mk", "alloy", "titanic", "composite", 1);
            result[15] = new WeaponName("Magnet gun", "Tl", "8MN", "24MN", "76MN", 1);
            return result;
        }
        public static string GetWeaponGroupName(int pos, EWeaponType type)
        {
            string result="";
            switch (pos)
            {
                case 0: if (Links.Lang == 0) result = WeaponNameEn[(int)type].Title; else result = WeaponNameRu[(int)type].Title; break;
                case 1: if (Links.Lang == 0) result = WeaponNameEn[(int)type].SizeName[0]; else result = WeaponNameRu[(int)type].SizeName[0]; break;
                case 2: if (Links.Lang == 0) result = WeaponNameEn[(int)type].SizeName[1]; else result = WeaponNameRu[(int)type].SizeName[1]; break;
                case 3: if (Links.Lang == 0) result = WeaponNameEn[(int)type].SizeName[2]; else result = WeaponNameRu[(int)type].SizeName[2]; break;
            }
            return result.Substring(0, 1).ToUpper() + result.Substring(1);
        }
        static SortedList<int, string[]> FillBuildingsDescription()
        {
            SortedList<int, string[]> result=new SortedList<int,string[]>();
            result.Add(0, new string[]{"English description of Living disctrict",
                "Строительство жилых районов обязательно для любой развивающейся колонии, так как современные люди не могут продуктивно жить и трудится в землянках или чистом поле."});
            result.Add(1, new string[]{"English description of Bank",
                "Развитая банковская система является залогом вашего финансового здоровья, которое достагается уверенным приростом надёжного денег. Кроме того, размещение металлических счетов позволяет вам увеличивать прирост металла."});
            result.Add(2, new string[]{"English description of Mine",
                "В шахтах добывают металлы. Также вложения в ресурсную базу не могут не отразится самым положительным образом на ваших финансах."});
            result.Add(3, new string[]{"English description of Chip factory",
                "Фабрика микросхем производит микросхемы. Кроме того там также производят редкие запасные части для кораблей."});
            result.Add(4, new string[]{"English description of University",
                "На территории университета можно размещать микросхемы в больших количествах. Также мощные компьютеры, использующиеся в обучении, позволяют вам повысить математическую мощь вашей колонии."});
            result.Add(5, new string[]{"English description of Participle Acceletator",
                "Ускоритель частиц позволяет вырабатывать антиматерию, а также, как побочный продукт, вырабатывает редкие металлы."});
            result.Add(6, new string[]{"English description if Science lab",
                "Антиматерию хранят на складах научных лабораторий. Также учёные постоянно разрабатывают новые микросхемы."});
            result.Add(7, new string[]{"English description of Manufacture",
                "Заводы производят запасные части для кораблей. Также некоторую продукцию можно использовать и чисто с коммерческой целью."});
            result.Add(8, new string[]{"English description of Steel melting",
                "На территории сталеплавильного цеха можно хранить готовые к использованию металлы. Кроме того, вырабатываемые при плавке редкие элементы позволяют увеличить приход антиматерии."});
            result.Add(9, new string[]{"English description of Radar station",
                "Радарная станция ведёт прямое наблюдение за кораблями. Мощная аппаратура расположенная там позволяет решать сложные математические задачи."});
            result.Add(10, new string[]{"English description of Data Center",
                "Комплекс серверов дата центра прямо предназначен для моделирования поведения Варп поля. Кроме того с помощью вероятностного анализа можно помогать в производстве антиматерии."});
            return result;
        }
        public static string GetBuildingDescription(GSBuilding building)
        {
            return "Описание постройки " + building.Name;
            //return BuildingDescription[building.Type][Links.Lang];
        }
        static SortedList<int, ScienceName> FillBuildingsNames()
        {
            SortedList<int, ScienceName> result = new SortedList<int, ScienceName>();
            result.Add(0, new ScienceName(new string[] { "living district", "жилой район" }, 0));
            result.Add(1, new ScienceName(new string[] { "bank", "банк" }, 0));
            result.Add(2, new ScienceName(new string[] { "mine", "шахта" }, 1));
            result.Add(3, new ScienceName(new string[] { "chip factory", "фабрика микросхем" }, 1));
            result.Add(4, new ScienceName(new string[] { "university", "университет" }, 0));
            result.Add(5, new ScienceName(new string[] { "participle accelerator", "ускоритель частиц" }, 0));
            result.Add(6, new ScienceName(new string[] { "science lab", "научная лаборатория" }, 1));
            result.Add(7, new ScienceName(new string[] { "manufacure", "завод" }, 0));
            result.Add(8, new ScienceName(new string[] { "steel melting", "сталеплавильный цех" }, 0));
            result.Add(9, new ScienceName(new string[] { "radar station", "радарная станция" }, 1));
            result.Add(10, new ScienceName(new string[] { "data center", "вычислительный центр" }, 0));
            return result;
        }
        public static string GetBuildingsName(GSBuilding building)
        {
            return building.Name;
            /*
            string result = "";
            ScienceName name=BuildingsName[building.Type];
            if (building.IsRare)
            {
                if (Links.Lang == 0)
                    result += "federal ";
                else if (name.Sex == 0)
                    result += "федеральный ";
                else if (name.Sex == 1)
                    result += "федеральная ";
            }
            if (Links.Lang == 0)
                result += name.En;
            else
                result += name.Ru;
            result += " " + (building.Level + 1).ToString();
            result = result.Substring(0, 1).ToUpper() + result.Substring(1).ToLower();
            return result;
            */
        }
        class ScienceName
        {
            static SortedList<byte, ScienceName> result = FillScienceNames();
            public string En;
            public string Ru;
            public byte Sex;
            public ScienceName(string[] text, byte sex)
            { En = text[0]; Ru = text[1]; Sex = sex; }
           
            public static string GetName(bool IsRare, byte type)
            {
                ScienceName name = result[type];
                if (IsRare)
                {
                    if (Links.Lang == 0)
                        return string.Format("Rare {0}", name.En);
                    else
                    {
                        switch (name.Sex)
                        {
                            case 0: return string.Format("Редкий {0}", name.Ru);
                            case 1: return string.Format("Редкая {0}", name.Ru);
                            case 2: return string.Format("Редкое {0}", name.Ru);
                            default: return string.Format("Редкие {0}", name.Ru);
                        }
                    }
                }
                else
                {
                    if (Links.Lang == 0) return name.En;
                    else return name.Ru;
                }
            }
            
            static SortedList<byte,ScienceName> FillScienceNames()
            {
                SortedList<byte, ScienceName> result = new SortedList<byte, ScienceName>();
                result.Add(0, new ScienceName(new string[] { "Trade Sector", "Торговый сектор"},0));
                result.Add(1, new ScienceName(new string[] { "Banking Sector", "Банковский сектор" },0));
                result.Add(2, new ScienceName(new string[] { "Mining Sector", "Шахта" },1));
                result.Add(3, new ScienceName(new string[] { "Metal Recycle", "Сталеплавильный завод" },0));
                result.Add(4, new ScienceName(new string[] { "Chips Factory", "Фабрика микросхем" },0));
                result.Add(5, new ScienceName(new string[] { "University", "Университет" },0));
                result.Add(6, new ScienceName(new string[] { "Partiple Accelerator", "Ускоритель частиц" },0));
                result.Add(7, new ScienceName(new string[] { "Science Sector", "Научная лаборатория" },1));
                result.Add(8, new ScienceName(new string[] { "Repairing Bay", "Ремонтный цех" },0));
                result.Add(9, new ScienceName(new string[] { "Manufacture", "Машиностроительный завод" },0));
                result.Add(10, new ScienceName(new string[] { "Radar Station", "Радарная станция" },1));
                result.Add(11, new ScienceName(new string[] { "Data Center", "Дата центр" },0));
                result.Add(20, new ScienceName(new string[] { "Ship Type", "Тип корабля" },0));
                result.Add(21, new ScienceName(new string[] { "Energy Generator", "Генератор энергии" },0));
                result.Add(22, new ScienceName(new string[] { "Shield Generator", "Генератор щита" },0));
                result.Add(23, new ScienceName(new string[] { "Computer", "Вычислитель" },0));
                result.Add(24, new ScienceName(new string[] { "Engine", "Двигатель" },0));
                result.Add(30, new ScienceName(new string[] { "Laser Gun", "Лазерная пушка" },1));
                result.Add(31, new ScienceName(new string[] { "EMI", "Электормагнитный излучатель" },0));
                result.Add(32,new ScienceName( new string[] { "Plasma Weapon", "Плазменное оружие" },2));
                result.Add(33, new ScienceName(new string[] { "Solar Gun", "Солнечная пушка" },1));
                result.Add(34, new ScienceName(new string[] { "Cannon", "Орудийная установка" },1));
                result.Add(35, new ScienceName(new string[] { "Gauss", "Оружие гаусса" },2));
                result.Add(36, new ScienceName(new string[] { "Rocket missle", "Ракетная установка" },1));
                result.Add(37,new ScienceName( new string[] { "Anti Matter Gun", "Пушка антиматерии" },1));
                result.Add(38,new ScienceName( new string[] { "Psi Weapon", "Пси оружие" },2));
                result.Add(39, new ScienceName(new string[] { "Dark Energy Gun", "Генератор тёмной энергии" },0));
                result.Add(40, new ScienceName(new string[] { "Warp Generator", "Варп генератор" },0));
                result.Add(41,new ScienceName( new string[] { "Time Machine Weapon", "Оружие машины времени" },2));
                result.Add(42,new ScienceName( new string[] { "Slicing Weapon", "Хакерское оружие" },2));
                result.Add(43,new ScienceName( new string[] { "Radiation Gun", "Радиоактивная пушка" },1));
                result.Add(44,new ScienceName( new string[] { "UAV", "БПЛА" },0));
                result.Add(45,new ScienceName( new string[] { "Magnet Weapon", "Магнитное оружие" },2));
                result.Add(50, new ScienceName(new string[] { "Energy Battery", "Энергетические батареи" },3));
                result.Add(150,new ScienceName( new string[] { "Shield Capacity Increase", "Усилитель щита" },0));
                result.Add(51,new ScienceName( new string[] { "Armor Plate", "Броневые пластины" },3));
                result.Add(151,new ScienceName( new string[] { "Repair Module", "Ремонтный модуль" },0));
                result.Add(52,new ScienceName( new string[] { "Energy Damagy Amplyfier", "Усилитель энергетического урона" },0));
                result.Add(152, new ScienceName(new string[] { "Physic Damage Amplyfier", "Усилитель физического урона" },0));
                result.Add(53,new ScienceName( new string[] { "Anomaly Damage Aplyfier", "Усилитель аномального урона" },0));
                result.Add(153,new ScienceName( new string[] { "Cyber Damage Ampyfier", "Усилитель кибернетического урона" },0));
                result.Add(54, new ScienceName(new string[] { "All Weapon Damage Ampyfier", "Усилитель любого урона" },0));
                return (result);
            }
        }
        public static string GetScienceName(GameScience science)
        {
            switch (science.Type)
            {
                case Links.Science.EType.Building: return GameObjectName.GetBuildingsName(Links.Buildings[science.ID]); 
                case Links.Science.EType.ShipTypes: return Links.ShipTypes[science.ID].GetName(); 
                case Links.Science.EType.Weapon: return GameObjectName.GetWeaponName(Links.WeaponTypes[science.ID]); 
                case Links.Science.EType.Generator: return GameObjectName.GetGeneratorName(Links.GeneratorTypes[science.ID]); 
                case Links.Science.EType.Shield: return GameObjectName.GetShieldName(Links.ShieldTypes[science.ID]); 
                case Links.Science.EType.Computer: return GameObjectName.GetComputerName(Links.ComputerTypes[science.ID]); 
                case Links.Science.EType.Engine: return GameObjectName.GetEngineName(Links.EngineTypes[science.ID]); 
                case Links.Science.EType.Equipment: return GameObjectName.GetEquipmentName(Links.EquipmentTypes[science.ID]);
                case Links.Science.EType.Other: return GameObjectName.GetNewLandsName(science); 
                default: return"";
            }
        }
/*        public static string GetScienceName(GameScience science)
        {
            ushort id = (ushort)science.ID;
            byte Level = (byte)(id / 1000);
            byte type = (byte)((id - Level * 1000) / 10);
            byte position = (byte)(id - Level * 1000 - type * 10);
            if (position >= 5) { position -= 5; type += 100; }
            string shortName = ScienceName.GetName(science.IsRare, type);
            return string.Format("{0} {1}-{2}", shortName, Level, position);
            
        }
 */
        class WeaponName
        {
            public string Title;
            public string PowerName;
            public string[] SizeName;
            public double PowerModi;
            public WeaponName(string title, string powername, string small, string med, string large, double modi)
            {
                Title = title;
                PowerName = powername;
                SizeName = new string[] { small, med, large };
                PowerModi = modi;
            }
        }
    }

}
