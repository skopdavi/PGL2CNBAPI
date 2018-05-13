using PGL2CNB.Datamodel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGL2CNB.Helpers
{
    public static class FileHelper
    {
        private static StringBuilder sb;
        private static void CreateCurrencyTable(Dictionary<DateTime, Dictionary<string, CurrencyData>> currData, Dictionary<string, Currency> selectedCurrencies, DateTime from, DateTime to)
        {
            DateTime day = from;

            sb.AppendLine(string.Format(@"<h1>Přehled vybraných kurzů měn za období {0} - {1}</h1>", from.ToString("dd. MM. yyyy"), to.ToString("dd. MM. yyyy")));
            sb.AppendLine(@"<table>");

            sb.AppendLine(@"<tr>");
            sb.AppendLine(string.Format(@"<th>{0}</th>", "Země"));
            sb.AppendLine(string.Format(@"<th>{0}</th>", "Měna"));
            sb.AppendLine(string.Format(@"<th>{0}</th>", "Kód"));
            sb.AppendLine(string.Format(@"<th>{0}</th>", "Minimum"));
            sb.AppendLine(string.Format(@"<th>{0}</th>", "Maximum"));
            sb.AppendLine(string.Format(@"<th>{0}</th>", "Medián"));
            sb.AppendLine(string.Format(@"<th>{0}</th>", "Průměr"));

            while (day <= to)
            {
                sb.AppendLine(string.Format(@"<th>{0}</th>", day.ToString("dd. MM. yyyy")));
                day = day.AddDays(1);
            }
            sb.AppendLine(@"</tr>");


            foreach (var curr in selectedCurrencies.Values)
            {
                sb.AppendLine(@"<tr>");

                sb.AppendLine(string.Format(@"<td>{0}</td>", curr.Country));
                sb.AppendLine(string.Format(@"<td>{0}</td>", curr.CurrencyName));
                sb.AppendLine(string.Format(@"<td>{0}</td>", curr.Code));

                sb.AppendLine(string.Format(@"<td>{0}</td>", curr.Rates.Min()));
                sb.AppendLine(string.Format(@"<td>{0}</td>", curr.Rates.Max()));
                
                int pos = ((int)curr.Rates.Count / 2);
                if (pos > 0)
                    sb.AppendLine(string.Format(@"<td>{0}</td>", curr.Rates.OrderBy(a => a).ElementAt(pos)));
                else
                    sb.AppendLine(string.Format(@"<td>{0}</td>", 0));
                sb.AppendLine(string.Format(@"<td>{0}</td>", curr.Rates.Average()));
                day = from;
                while (day <= to)
                {
                    sb.AppendLine(string.Format(@"<td>{0}</td>", currData[day][curr.Code].Rate));
                    day = day.AddDays(1);
                }

                sb.AppendLine(@"</tr>");
            }
            sb.AppendLine(@"</table>");
        }

        private static void CreateCorrelationTable(Dictionary<string, Currency> selectedCurrencies)
        {
            sb.AppendLine(string.Format(@"<h1>Přehled korelačních koeficientů pro vybrané měny a dané období</h1>"));
            sb.AppendLine(@"<table>");
            Dictionary<string, int> posTable = new Dictionary<string, int>();
            int i = 0;
            sb.AppendLine(@"<tr>");
            sb.AppendLine(string.Format(@"<th>{0}</th>", "Kód"));
            foreach (var curr in selectedCurrencies)
            {
                posTable.Add(curr.Value.Code, i++);
                sb.AppendLine(string.Format(@"<th>{0}</th>", curr.Value.Code));
            }
            sb.AppendLine(@"</tr>");
            foreach (var curr in selectedCurrencies)
            {
                sb.AppendLine(@"<tr>");
                sb.AppendLine(string.Format(@"<td>{0}</td>", curr.Value.Code));
                foreach (var curr2 in selectedCurrencies)
                {
                    
                    if (curr.Key == curr2.Key)
                        sb.AppendLine(string.Format(@"<td>{0}</td>", "x"));
                    else
                        sb.AppendLine(string.Format(@"<td>{0}</td>", curr.Value.CurrencyCorrelation[curr2.Value.Code]));
                }
                sb.AppendLine(@"</tr>");
            }

            sb.AppendLine(@"</table>");
        }

        public static void SaveDataToHTMLFile(string path, Dictionary<DateTime, Dictionary<string, CurrencyData>> currData, Dictionary<string, Currency> selectedCurrencies, DateTime from, DateTime to)
        {
            sb = new StringBuilder();

            sb.AppendLine(@"<!DOCTYPE html>
                            <html>
                            <head>
                            <meta charset=""UTF-8"">
                            <style>
                            table, th, td {
                                border: 1px solid black;
                                border-collapse: collapse;
                            }
                            th, td {
                                padding: 5px;
                                text-align: left;
                            }
                            </style>
                            </head>
                            <body>");

            CreateCurrencyTable(currData, selectedCurrencies, from, to);
            CreateCorrelationTable(selectedCurrencies);


            sb.AppendLine(@"</body>
                            </html>");

            using (StreamWriter fs = new StreamWriter(path))
            {
                fs.WriteLine(sb.ToString());

            }

        }
    }
}
