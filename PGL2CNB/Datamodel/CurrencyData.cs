using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGL2CNB.Datamodel
{
    public class CurrencyData
    {
        private string country;
        public string Country
        {
            get { return country; }
            set
            {
                if (country == value)
                    return;
                country = value;
            }
        }

        private string code;
        public string Code
        {
            get { return code; }
            set
            {
                if (code == value)
                    return;
                code = value;
            }
        }

        private string currencyName;
        public string CurrencyName
        {
            get { return currencyName; }
            set
            {
                if (currencyName == value)
                    return;
                currencyName = value;
            }
        }

        private int count;
        public int Count
        {
            get { return count; }
            set
            {
                if (count == value)
                    return;
                count = value;
            }
        }

        private double rate;
        public double Rate
        {
            get { return rate; }
            set
            {
                if (rate == value)
                    return;
                rate = value;
            }
        }
    }
}
