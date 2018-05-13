using PGL2CNB.Datamodel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PGL2CNB.Helpers
{
    public static class CNBApiHelper
    {
        private static List<string> GetDayData(DateTime? day = null)
        {
            List<string> ret = new List<string>();
            WebClient client = new WebClient();
            string url = @"http://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt";
            if (day.HasValue)
                url = @"http://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt?date=" + day.Value.ToString("dd.MM.yyyy");

            using (Stream data = client.OpenRead(url))
            {
                using (StreamReader reader = new StreamReader(data))
                {
                    while (!reader.EndOfStream)
                    {
                        ret.Add(reader.ReadLine());
                    }
                }
            }
            return ret;
        }

        public static ObservableCollection<Currency> GetAvailableCurrencies()
        {
            ObservableCollection<Currency> ret = new ObservableCollection<Currency>();
            var data = GetDayData();
            data.RemoveRange(0, 2);
            foreach (var lineString in data)
            {
                Currency curr = new Currency();
                var line = lineString.Split('|');//země|měna|množství|kód|kurz
                curr.Code = line[3];
                curr.Country = line[0];
                curr.CurrencyName = line[1];
                ret.Add(curr);
            }
            return ret;
        }


        private static Dictionary<string, CurrencyData> ProcessDayData(List<string> data, Dictionary<string, Currency> selectedCurrencies)
        {
            Dictionary<string, CurrencyData> ret = new Dictionary<string, CurrencyData>();
            data.RemoveRange(0, 2);
            foreach (var lineString in data)
            {
                var line = lineString.Split('|');//země|měna|množství|kód|kurz
                if (selectedCurrencies.ContainsKey(line[3]))
                {
                    try
                    {
                        var currentCurr = selectedCurrencies[line[3]];
                        double tempDec = 0;
                        int tempInt = 0;
                        int.TryParse(line[2], out tempInt);
                        double.TryParse(line[4].Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out tempDec);
                        var curr = new CurrencyData() { Country = line[0], CurrencyName = line[1], Count = tempInt, Code = line[3], Rate = tempDec };
                        currentCurr.AddRate(tempDec);
                        ret.Add(line[3], curr);
                    }
                    catch
                    {
                        //DO NOTHING
                    }
                }
            }
            return ret;
        }

        public static Dictionary<DateTime, Dictionary<string, CurrencyData>> ProcessData(DateTime from, DateTime to, Dictionary<string, Currency> selectedCurrencies)
        {
            Dictionary<DateTime, Dictionary<string, CurrencyData>> ret = new Dictionary<DateTime, Dictionary<string, CurrencyData>>();
            var day = from;
            while (day <= to)
            {
                var dayData = GetDayData(day);
                ret.Add(day, ProcessDayData(dayData, selectedCurrencies));
                day = day.AddDays(1);
            }
            return ret;
        }
    }
}
