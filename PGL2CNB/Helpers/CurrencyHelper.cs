using PGL2CNB.Datamodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGL2CNB.Helpers
{
    public static class CurrencyHelper
    {
        private static void CalculateCurrencyCorrelationsForTwoCurrencies(Dictionary<DateTime, Dictionary<string, CurrencyData>> currData, Currency curr1, Currency curr2)
        {
            double x = 0d, y = 0d;
            double xy = 0d, xx = 0d, yy = 0d;
            double corrCoef = 0d;
            foreach (var day in currData.Values)
            {
                x = day[curr1.Code].Rate;
                y = day[curr2.Code].Rate;
                xy += (x - curr1.Average) * (y - curr2.Average);
                xx += Math.Pow(x - curr1.Average,2);
                yy += Math.Pow(y - curr2.Average,2);
            }
            corrCoef = xy / Math.Sqrt(xx * yy);
            curr1.CurrencyCorrelation.Add(curr2.Code, corrCoef);
            curr2.CurrencyCorrelation.Add(curr1.Code, corrCoef);
        }

        public static void CalculateCurrencyCorrelations(Dictionary<DateTime, Dictionary<string, CurrencyData>> currData, Dictionary<string, Currency> selectedCurrencies)
        {
            for(int i = 0; i < selectedCurrencies.Count-1; i++)
            {
                for (int j = i+1; j < selectedCurrencies.Count; j++)
                {
                    CalculateCurrencyCorrelationsForTwoCurrencies(currData, selectedCurrencies.ElementAt(i).Value, selectedCurrencies.ElementAt(j).Value);
                }
            }
        }
    }
}
