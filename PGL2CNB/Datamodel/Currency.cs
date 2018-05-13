using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PGL2CNB.Datamodel
{
    public class Currency : INotifyPropertyChanged
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
                OnPropertyChanged();
            }
        }

        private List<double> rates = new List<double>();
        public List<double> Rates
        {
            get { return rates; }
            set
            {
                if (rates == value)
                    return;
                rates = value;
                OnPropertyChanged();
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
                OnPropertyChanged();
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
                OnPropertyChanged();
            }
        }

        private bool isChecked;
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                if (isChecked == value)
                    return;
                isChecked = value;
                OnPropertyChanged();
            }
        }

        private double sum = 0;
        public double Sum
        {
            get { return sum; }
            set
            {
                if (sum == value)
                    return;
                sum = value;
            }
        }

        private int count = 0;
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

        public double Average
        {
            get { return Sum / Count; }
        }

        private Dictionary<string, double> currencyCorrelation = new Dictionary<string, double>();
        public Dictionary<string, double> CurrencyCorrelation
        {
            get { return currencyCorrelation; }
            set
            {
                if (currencyCorrelation == value)
                    return;
                currencyCorrelation = value;
            }
        }

        public void AddRate(double tempDec)
        {
            Sum += tempDec;
            Count++;
            rates.Add(tempDec);
        }

        public void Clear()
        {
            currencyCorrelation = new Dictionary<string, double>();
            rates = new List<double>();
            sum = 0;
            count = 0;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName]string caller = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(caller));
            }
        }
    }
}
