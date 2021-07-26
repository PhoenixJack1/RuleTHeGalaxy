using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public class ServerPremium
    {
        public bool HasOrion = false;
        public bool HasQuickStart = false;
        public bool HasFullPay = false;
        public bool FullScience = false;
        public bool NoDelete = false;
        public bool AllMoney = false;
        public int CurrentMoney = 500;
        public int PremiumDays = 0;
        public int TotalPayedMoney = 500;
        public bool IsPremium = false;
        public bool Money = false;
        public bool Science = false;
        public byte[] Array;
        GSAccount Account;
        public ServerPremium(GSAccount account)
        {
            Account = account;
            //CreateArray();
            AddMoney(0);
        }
        //бит 0 - включение быстрого старта
        //бит 1 - включение Ориона
        public byte AddMoney(int value)
        {
            byte answer = 0;
            CurrentMoney += value;
            TotalPayedMoney += value;
            if (NoDelete == false && TotalPayedMoney >= ServerLinks.Parameters.PremiumNoDelete)
                NoDelete = true;
            if (FullScience == false && TotalPayedMoney >= ServerLinks.Parameters.PremiumFullScience)
                FullScience = true;
            if (AllMoney == false && TotalPayedMoney >= ServerLinks.Parameters.PremiumNoMoneyCapacity)
                AllMoney = true;
            if (HasQuickStart == false && TotalPayedMoney >= ServerLinks.Parameters.PremiumQuickStartPrice)
            {
                HasQuickStart = true;
                answer = (byte)(answer | 1);
            }
            if (HasOrion == false && TotalPayedMoney >= ServerLinks.Parameters.PremiumOrionPrice)
            {
                HasOrion = true;
                answer = (byte)(answer | 2);
            }
            CheckOptions();
            CreateArray();

            return answer;
        }
        public ServerPremium(byte[] array, GSAccount account)
        {
            Account = account;
            Array = array;
            int i = 6;
            HasOrion = BitConverter.ToBoolean(array, i); i++;
            HasQuickStart = BitConverter.ToBoolean(array, i); i++;
            HasFullPay = BitConverter.ToBoolean(array, i); i += 1;
            FullScience = BitConverter.ToBoolean(array, i); i += 1;
            AllMoney = BitConverter.ToBoolean(array, i); i += 1;
            NoDelete = BitConverter.ToBoolean(array, i); i += 2;
            CurrentMoney = BitConverter.ToInt32(array, i); i += 4;
            PremiumDays = BitConverter.ToInt32(array, i); i += 4;
            TotalPayedMoney = BitConverter.ToInt32(array, i); i += 4;
            if (HasFullPay) IsPremium = true;
            else if (PremiumDays > 0) IsPremium = true;
            else IsPremium = false;
            AddMoney(0);
            //CreateArray();
        }
        public void CheckOptions()
        {
            if (PremiumDays > 0) IsPremium = true;
            else IsPremium = false;
            if (IsPremium || AllMoney) Money = true; else Money = false;
            if (IsPremium || FullScience) Science = true; else Science = false;
        }
        public void NextDay()
        {
            if (HasFullPay) return;
            if (PremiumDays > 0)
            {
                PremiumDays--;
                CheckOptions();
                CreateArray();
            }
        }
        public bool SetFullPay()
        {
            if (HasFullPay || CurrentMoney < ServerLinks.Parameters.PremiumFullPayPrice) return false;
            CurrentMoney -= ServerLinks.Parameters.PremiumFullPayPrice;
            HasFullPay = true;
            PremiumDays = 9999;
            CheckOptions();
            CreateArray();
            return true;
        }
        public bool SetOneMonthPremium()
        {
            if (HasFullPay || CurrentMoney < ServerLinks.Parameters.PremiumOneMonthPrice) return false;
            CurrentMoney -= ServerLinks.Parameters.PremiumOneMonthPrice;
            PremiumDays += 30;
            CheckOptions();
            CreateArray();
            return true;
        }
        public bool SetThreeMonthPremium()
        {
            if (HasFullPay || CurrentMoney < ServerLinks.Parameters.PremiumThreeMonthPrice) return false;
            CurrentMoney -= ServerLinks.Parameters.PremiumThreeMonthPrice;
            PremiumDays += 90;
            CheckOptions();
            CreateArray();
            return true;
        }
        public bool SetSixMonthPremium()
        {
            if (HasFullPay || CurrentMoney < ServerLinks.Parameters.PremiumSixMonthPrice) return false;
            CurrentMoney -= ServerLinks.Parameters.PremiumSixMonthPrice;
            PremiumDays += 180;
            CheckOptions();
            CreateArray();
            return true;
        }
        public bool SetOneYearPremium()
        {
            if (HasFullPay || CurrentMoney < ServerLinks.Parameters.PremiumOneYearPrice) return false;
            CurrentMoney -= ServerLinks.Parameters.PremiumOneYearPrice;
            PremiumDays += 365;
            CheckOptions();
            CreateArray();
            return true;
        }
        public bool CheckUpdateTime()
        {
            if (ServerLinks.GameMode == EGameMode.Single) return true;
            if (IsPremium)
                if (DateTime.Now < Account.LastEnter + ServerLinks.Parameters.PremiumUpdateTime)
                    return true;
                else return false;
            else
                if (DateTime.Now < Account.LastEnter + ServerLinks.Parameters.NotPremiumUpdateTime)
                return true;
            else return false;
        }
        public void CreateArray()
        {
            List<byte> list = new List<byte>();
            list.Add(10);
            list.Add(0);
            list.AddRange(BitConverter.GetBytes((int)19));
            list.AddRange(BitConverter.GetBytes(HasOrion));
            list.AddRange(BitConverter.GetBytes(HasQuickStart));
            list.AddRange(BitConverter.GetBytes(HasFullPay));
            list.AddRange(BitConverter.GetBytes(FullScience));
            list.AddRange(BitConverter.GetBytes(AllMoney));
            list.AddRange(BitConverter.GetBytes(NoDelete));
            list.AddRange(BitConverter.GetBytes(IsPremium));
            list.AddRange(BitConverter.GetBytes(CurrentMoney));
            list.AddRange(BitConverter.GetBytes(PremiumDays));
            list.AddRange(BitConverter.GetBytes(TotalPayedMoney));
            Array = list.ToArray();
        }
    }
}
