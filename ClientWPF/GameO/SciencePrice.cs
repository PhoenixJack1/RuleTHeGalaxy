using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    class SciencePrice
    {
        public static ItemPrice GetSciencePrice(GSPlayer player, ScienceLearn level)
        {
            int maxlevel = player.Sciences.MaxLevel;
            int sciences = player.Sciences.PlayerSciences.Length;
            return GetSciencePrice(maxlevel, sciences, level);
        }
        public static ItemPrice GetSciencePrice(int maxlevel, int sciences, ScienceLearn level)
        {
            double levelconst = 1; double countconst = 1;
            double moneyconst = 0.25; double metallconst = 0.25; double chipsconst = 0.25; double anticonst = 0.25;
            switch (level)
            {
                case ScienceLearn.Low: levelconst = 4.6; countconst = 150; moneyconst = 0.25; metallconst = 0.4; chipsconst = 0.2; anticonst = 0.15; break;
                case ScienceLearn.Current: levelconst = 4.8; countconst = 125; moneyconst = 0.2; metallconst = 0.3; chipsconst = 0.25; anticonst = 0.25; break;
                case ScienceLearn.Max: levelconst = 5.0; countconst = 100; moneyconst = 0.15; metallconst = 0.2; chipsconst = 0.3; anticonst = 0.35; break;
            }
            double sum = Math.Pow(maxlevel, levelconst) * (1 + sciences / countconst);
            double money = Math.Round(sum * moneyconst, 0);
            double metall = Math.Round(sum * metallconst / 10, 0);
            double chips = Math.Round(sum * chipsconst / 10, 0);
            double anti = Math.Round(sum * anticonst / 10, 0);
            return new ItemPrice((int)money, (int)metall, (int)chips, (int)anti);
        }
        
    }
}
