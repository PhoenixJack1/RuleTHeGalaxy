using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    class GSMarket
    {
        public long Metal;
        public int MetalBase = 10;
        public int MetalStep = 1000000;

        public long Chips;
        public int ChipsBase = 10;
        public int ChipsStep = 1000000;

        public long Anti;
        public int AntiBase = 10;
        public int AntiStep = 1000000;

        public byte[] Code;
        public GSMarket()
        {
            Calculate();
        }
        public double BuyMetal, SellMetal, BuyChips, SellChips, BuyAnti, SellAnti;
        double GetMetalPrice()
        {
            double a = (double)Metal / (double)MetalStep;
            double result = MetalBase - MetalBase * a / 100;
            if (result < 1) result = 1;
            return result;
        }
        double GetChipsPrice()
        {
            double a = (double)Chips / (double)ChipsStep;
            double result = ChipsBase - ChipsBase * a / 100;
            if (result < 1) result = 1;
            return result;
        }
        double GetAntiPrice()
        {
            double a = (double)Anti / (double)AntiStep;
            double result = AntiBase - AntiBase * a / 100;
            if (result < 1) result = 1;
            return result;
        }
        public void ChangeMetal(int value)
        {
            Metal += value;
            Calculate();
        }
        public void ChangeChips(int value)
        {
            Chips += value;
            Calculate();
        }
        public void ChangeAnti(int value)
        {
            Anti += value;
            Calculate();
        }
        public void Calculate()
        {
            double MetalPrice = GetMetalPrice();
            double ChipsPrice = GetChipsPrice();
            double AntiPrice = GetAntiPrice();
            BuyMetal = Math.Round(MetalPrice * 1.05, 2);
            SellMetal = Math.Round(MetalPrice / 1.05, 2);
            BuyChips = Math.Round(ChipsPrice * 1.05, 2);
            SellChips = Math.Round(ChipsPrice / 1.05, 2);
            BuyAnti = Math.Round(AntiPrice * 1.05, 2);
            SellAnti = Math.Round(AntiPrice / 1.05, 2);
            List<byte> array = new List<byte>();
            array.AddRange(BitConverter.GetBytes(MetalPrice));
            array.AddRange(BitConverter.GetBytes(ChipsPrice));
            array.AddRange(BitConverter.GetBytes(AntiPrice));
            Code = array.ToArray();
        }

    }
}
