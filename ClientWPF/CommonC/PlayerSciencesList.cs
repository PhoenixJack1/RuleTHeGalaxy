using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public enum ScienceLearn { Low, Current, Max}
    public class PlayerSciencesList
    {
        public static SortedSet<ushort> GetSciencesArray(byte[] array)
        {
            SortedSet<ushort> SciencesArray = new SortedSet<ushort>();
            for (int i = 0; i < array.Length; i += 2)
            {
                ushort scienceID = BitConverter.ToUInt16(array, i);
                SciencesArray.Add(scienceID);
            }
            return SciencesArray;
        }
        public static SortedSet<ushort>[,] AllSciences;
        public SortedSet<ushort> SciencesArray;//Список всех исследований, доступый по индексу.
        GSPlayer Player;
        public byte[] ByteArray;
        public byte[] ByteArrayStep1;
        public SortedSet<ushort>[,] PlayerSciences;//первое - тип (обычный, редкий, все), второе - уровень, третье - собственно массив
        public byte MaxLevel = 0;
        public PlayerSciencesList(byte[] sciencesarray, GSPlayer player)
        {
            Player = player;
            SciencesArray = new SortedSet<ushort>();
            AnalyseTextSciences(sciencesarray, player);
            FillLevelAvailable();
            GetByteArray();
        }
        void AnalyseTextSciences(byte[] array, GSPlayer player)
        {
            if (array != null)
            {
                for (int i = 0; i < array.Length; i += 2)
                {
                    ushort ScienceID;
                    ScienceID = BitConverter.ToUInt16(array, i);
                    if (!ServerLinks.Science.GameSciences.ContainsKey(ScienceID))
                    { Console.WriteLine("Error while analyse sciences in Player with ID=" + Player.ID); continue; }
                    GameScience science = ServerLinks.Science.GameSciences[ScienceID];
                    if (science.Level > MaxLevel) MaxLevel = science.Level;
                    SciencesArray.Add(ScienceID);
                    //if (ServerLinks.Science.NewLands.ContainsKey(ScienceID))
                    //    player.MaxLandsCounts++;
                }
                //Console.WriteLine("Player ID=" + player.ID + " MaxLevel=" + MaxLevel);
            }
            foreach (ushort scienceID in ServerLinks.Science.BasicScienceList)
                if (!SciencesArray.Contains(scienceID))
                    SciencesArray.Add(scienceID);
            //foreach (GameScience science in ServerLinks.Science.GameSciences.Values)
            //    if (science.Level < 10)
            //        SciencesArray.Add(science.ID);
            //foreach (ushort scienceID in SciencesArray)
            //    SciencePrice.AddScience(scienceID);
        }
        void FillLevelAvailable()
        {
            PlayerSciences = new SortedSet<ushort>[3,51];
            for (int i = 0; i < 51; i++)
                for (int j = 0; j < 3; j++)
                    PlayerSciences[j, i] = new SortedSet<ushort>(AllSciences[j, i]);
            foreach (ushort ScienceID in SciencesArray)
            {
                GameScience science = ServerLinks.Science.GameSciences[ScienceID];
                if (science.NotLearned == true) continue;
                if (science.IsRare)
                    PlayerSciences[1, science.Level].Remove(ScienceID);
                else
                    PlayerSciences[0, science.Level].Remove(ScienceID);
                PlayerSciences[2, science.Level].Remove(ScienceID);
                if (science.Level > MaxLevel) MaxLevel = science.Level;
            }
        }
        public ushort SelectNewScience(ScienceLearn level)
        {
            /* При начальном уровне изучаются технологии из 10% - текущего уровня, 90% - низшего
             * При среднем уровне 70% - низшего, 20% - текущего, 10% - высшего
             * При максимальном уровне 50% - низшего, 20% текущего, 30% - высшего
             */
            ScienceLearn curlearn = ScienceLearn.Low;
            int rndval = ServerLinks.Science.ScienceRandom.Next(0, 100);
            if (level == ScienceLearn.Max)
            {
                if (rndval >= 70) curlearn = ScienceLearn.Max;
                else if (rndval >= 50) curlearn = ScienceLearn.Current;
                else curlearn = ScienceLearn.Low;
            }
            else if (level==ScienceLearn.Current)
            {
                if (rndval >= 90) curlearn = ScienceLearn.Max;
                else if (rndval >= 70) curlearn = ScienceLearn.Current;
                else curlearn = ScienceLearn.Low;
            }
            else
            {
                if (rndval >= 90) curlearn = ScienceLearn.Current;
                else curlearn = ScienceLearn.Low;
            }
            if (curlearn==ScienceLearn.Max)
            {
                if (MaxLevel == 50)
                    curlearn = ScienceLearn.Current;
                else
                {
                    byte nextlevel = (byte)(MaxLevel + 1);
                    int val = ServerLinks.Science.ScienceRandom.Next(PlayerSciences[2, nextlevel].Count);
                    return PlayerSciences[2, nextlevel].ElementAt(val);
                }
            }
            if (curlearn==ScienceLearn.Current)
            {
                if (PlayerSciences[2, MaxLevel].Count == 0)
                    curlearn = ScienceLearn.Low;
                else
                {
                    int val = ServerLinks.Science.ScienceRandom.Next(PlayerSciences[2, MaxLevel].Count);
                    return PlayerSciences[2, MaxLevel].ElementAt(val);
                }
            }
            List<ushort> Sciences = new List<ushort>();
            for (int i = 0; i < MaxLevel; i++)
                Sciences.AddRange(PlayerSciences[2, i]);
            if (Sciences.Count == 0)
                return 0;
            int cval = ServerLinks.Science.ScienceRandom.Next(Sciences.Count);
            return Sciences[cval];

        }
        public ushort LearnNewScienceFromBattle(bool? IsRare, byte minlevel)
        {
            throw new Exception();
        }

        public int SelectNewScienceToLearnRandom(int low, int mid, int max)
        {
            int sum = low + mid + max;
            int val = ServerLinks.BattleRandom.Next(sum);
            if (val < low) return SelectNewScience(ScienceLearn.Low);
            else if (val < low + mid) return SelectNewScience(ScienceLearn.Current);
            else return SelectNewScience(ScienceLearn.Max); 
        }
        //метод возвращает минимальный уровень исследовния в данной области
        //в котором число выученных исследований ниже базового уровня
        /* int GetMaxLevel(EScienceField field, ScienceFieldsclass arrayclass)
         {
             for (int i = 0; i < arrayclass.GetArray(field).Length; i++)
                 if (arrayclass.GetArray(field)[i].Count < ServerLinks.Science.MaxScinecesNeedsToBeKnownToLearnNextlevel)
                 {
                     int answer = i;
                     //if (!Player.Premium.Science && answer > ServerLinks.Parameters.PremiumMaxScienceLevel) answer = ServerLinks.Parameters.PremiumMaxScienceLevel;
                     return answer;
                 }
             return -1;
         }*/
        /* List<int> GetArrayWithScienceToLearn(EScienceField field, ScienceFieldsclass arraytotal, ScienceFieldsclass arrayplayer, int MaxLevel)
         {
             List<int> result = new List<int>();
             if (MaxLevel == -1)
                 MaxLevel = arraytotal.GetArray(field).Length;
             for (int i = 0; i < MaxLevel; i++)
             {
                 SortedSet<ushort> curtotal = arraytotal.GetArray(field)[i];
                 SortedSet<ushort> curplayer = arrayplayer.GetArray(field)[i];
                 if (curplayer.Count == curtotal.Count) continue;
                 foreach (ushort value in curtotal)
                     if (!curplayer.Contains(value))
                         result.Add(value);
             }
             return result;
         }*/
        /*  List<int> GetArrayWithOneLevelScienceToLearn(EScienceField field, ScienceFieldsclass arraytotal, ScienceFieldsclass arrayplayer, int CurLevel)
          {
              List<int> result = new List<int>();
              if (CurLevel == -1)
                  return result;
              SortedSet<ushort> curtotal = arraytotal.GetArray(field)[CurLevel];
              SortedSet<ushort> curplayer = arrayplayer.GetArray(field)[CurLevel];
              if (curtotal.Count == curplayer.Count) return result;
              foreach (ushort value in curtotal)
                  if (!curplayer.Contains(value))
                      result.Add(value);
              return result;

          }*/
        /* public int SelectNewScienceToLearn(EScienceField field)
         {
             //if (!SciencesArray.Contains(1900)) return 1900;
             //кинуть рандом, 0-5 - исследуем редкий скилл нижнего уровня, 6-10 - редкий скилл верхнего уровня, 
             //11-60 - обычный скилл нижнего уровня, 61-90 - обычный скилл верхнего уровня.
             int randomresult = ServerLinks.Science.ScienceRandom.Next(100);
             //Console.WriteLine(randomresult);
             int MaxLevelPlayer = GetMaxLevel(field, AllPlayer);
             List<int> array = new List<int>();
             if (randomresult < 11)
             {
                 if (randomresult < 6)
                     array = GetArrayWithScienceToLearn(field, Rare, RarePlayer, MaxLevelPlayer);
                 else
                     array = GetArrayWithOneLevelScienceToLearn(field, Rare, RarePlayer, MaxLevelPlayer);
                 if (array.Count == 0)
                     if (randomresult < 6)
                         array = GetArrayWithOneLevelScienceToLearn(field, Rare, RarePlayer, MaxLevelPlayer);
                     else
                         array = GetArrayWithScienceToLearn(field, Rare, RarePlayer, MaxLevelPlayer);
             }
             else if (randomresult < 91 || array.Count == 0)
             {
                 if (randomresult < 61)
                     array = GetArrayWithScienceToLearn(field, NotRare, NotRarePlayer, MaxLevelPlayer);
                 else
                     array = GetArrayWithOneLevelScienceToLearn(field, NotRare, NotRarePlayer, MaxLevelPlayer);
                 if (array.Count == 0)
                     if (randomresult < 61)
                         array = GetArrayWithOneLevelScienceToLearn(field, NotRare, NotRarePlayer, MaxLevelPlayer);
                     else
                         array = GetArrayWithScienceToLearn(field, NotRare, NotRarePlayer, MaxLevelPlayer);
             }
             else return -1; //ничего не изучено
             if (array.Count == 0)
             {
                 return -1; //ничего не изучено
             }
             int learnresult = ServerLinks.Science.ScienceRandom.Next(array.Count);
             return array.ElementAt(learnresult);
         }*/
        /*public int SelectNewScienceNotLearn()
        {
            EScienceField field = (EScienceField)ServerLinks.Science.ScienceRandom.Next(4);
            return SelectNewScienceToLearn(field);
        }
        public ushort LearnNewScienceFromBattle(bool? IsRare, byte minlevel)
        {
            List<ushort> list = new List<ushort>();
            if (IsRare == true)
                for (int i = minlevel; i <= MaxLevel; i++)
                    list.AddRange(Rare.All[i].ToArray());
            else if (IsRare == false)
                for (int i = minlevel; i <= MaxLevel; i++)
                    list.AddRange(NotRare.All[i].ToArray());
            else
                for (int i = minlevel; i <= MaxLevel; i++)
                    list.AddRange(All.All[i].ToArray());
            for (int i = 0; i < list.Count; i++)
                if (SciencesArray.Contains(list[i]))
                {
                    list.RemoveAt(i);
                    i--;
                }
            if (list.Count == 0) return 0;
            int random = ServerLinks.BattleRandom.Next(list.Count);
            ushort answer = list[random];
            PutScienceToArray(answer);
            return answer;
        }*/
        public void PutScienceToArray(ushort ScienceID)
        {
            if (ScienceID == 0) return;
            if (SciencesArray.Contains(ScienceID)) return;
            SciencesArray.Add(ScienceID);
            GameScience science = ServerLinks.Science.GameSciences[ScienceID];
            if (science.NotLearned) return;
            if (science.IsRare)
                PlayerSciences[1, science.Level].Remove(ScienceID);
            else
                PlayerSciences[0, science.Level].Remove(ScienceID);
            PlayerSciences[2, science.Level].Remove(ScienceID);
            if (science.Level > MaxLevel) MaxLevel = science.Level;
           
            GetByteArray();
        }
        public void GetByteArray()
        {
            List<byte> array = new List<byte>();
            foreach (ushort val in SciencesArray)
                array.AddRange(BitConverter.GetBytes((ushort)val));
            ByteArray = array.ToArray();
            List<byte> newarray = new List<byte>();
            newarray.Add(0); newarray.Add(4);
            newarray.AddRange(BitConverter.GetBytes(ByteArray.Length));
            newarray.AddRange(ByteArray);
            ByteArrayStep1 = newarray.ToArray();
        }

        
        public static void SciencePreparation()
        {
            ServerLinks.Science.ScienceRandom = new Random();
            AllSciences = new SortedSet<ushort>[3, 51];
            for (int i = 0; i < 51; i++)
                for (int j = 0; j < 3; j++)
                    AllSciences[j, i] = new SortedSet<ushort>();
            foreach (GameScience science in ServerLinks.Science.GameSciences.Values)
            {
                if (science.NotLearned == true) continue;
                if (science.IsRare)
                    AllSciences[1, science.Level].Add(science.ID);
                else
                    AllSciences[0, science.Level].Add(science.ID);
                AllSciences[2, science.Level].Add(science.ID);
            }
        }//SciencePreparation
        public bool CheckShipSchema(Schema schema)
        {
            int[] array = schema.GetPartsArray();
            for (int i = 0; i < 12; i++)
            {
                if (i < 5)
                    if (array[i] == -1 || !SciencesArray.Contains((ushort)array[i]))
                        return false;
                    else { }
                else
                    if (array[i] > 0 && !SciencesArray.Contains((ushort)array[i]))
                    return false;
            }
            return true;

        }

    }//class
}
