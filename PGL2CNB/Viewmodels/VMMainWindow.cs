using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PGL2CNB.Datamodel;
using PGL2CNB.Helpers;
using System.Windows;

namespace PGL2CNB.Viewmodels
{
    public class VMMainWindow : BaseViewmodel
    {
        private ObservableCollection<Currency> colCurrency;
        public ObservableCollection<Currency> ColCurrency
        {
            get { return colCurrency; }
            set
            {
                if (colCurrency == value)
                    return;
                colCurrency = value;
                OnPropertyChanged();
            }
        }

        private bool dataLoaded = false;
        public bool DataLoaded
        {
            get { return dataLoaded; }
            set
            {
                if (dataLoaded == value)
                    return;
                dataLoaded = value;
                OnPropertyChanged();
            }
        }

        private DateTime fromDate = DateTime.Today;
        public DateTime FromDate
        {
            get { return fromDate; }
            set
            {
                if (fromDate == value)
                    return;
                fromDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime toDate = DateTime.Today;
        public DateTime ToDate
        {
            get { return toDate; }
            set
            {
                if (toDate == value)
                    return;
                toDate = value;
                if (ToDate < FromDate)
                    FromDate = ToDate;
                OnPropertyChanged();
            }
        }

        private string filePath;
        public string FilePath
        {
            get { return filePath; }
            set
            {
                if (filePath == value)
                    return;
                filePath = value;
                OnPropertyChanged("FilePath");
            }
        }

        public void Clear()
        {
            ToDate = DateTime.Today;
            FromDate = DateTime.Today;
            FilePath = string.Empty;
            foreach (var curr in ColCurrency) {
                curr.IsChecked = false;
                curr.Clear();
            }
        }

        public void CreateFile()
        {
            var selectedCurrencies = ColCurrency.Where(a => a.IsChecked).ToDictionary(a => a.Code);
            try
            {
                if(selectedCurrencies.Count==0)
                {
                    MessageBox.Show("Vyberte měny, které chcete exportovat.", "Varování", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                else if (string.IsNullOrEmpty(FilePath))
                {
                    foreach (var curr in selectedCurrencies.Values)
                        curr.Clear();
                    MessageBox.Show("Není zadaná cesta pro export souboru. Zadejte prvně cestu.", "Varování", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var processedData = CNBApiHelper.ProcessData(FromDate, ToDate, selectedCurrencies);
                if (ColCurrency.Count > 1)
                {
                    CurrencyHelper.CalculateCurrencyCorrelations(processedData, selectedCurrencies);
                }
                FileHelper.SaveDataToHTMLFile(FilePath, processedData, selectedCurrencies, FromDate, ToDate);
                foreach (var curr in selectedCurrencies.Values)
                    curr.Clear();
                MessageBox.Show("Soubor byl vytvořen a uložen zde: " + FilePath + ".", "Informace", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Při ukládání došlo k chybě: " + ex.Message + ".", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                foreach (var curr in selectedCurrencies.Values)
                    curr.Clear();
            }
        }

        public void GetData()
        {
            try
            {
                ColCurrency = CNBApiHelper.GetAvailableCurrencies();
                DataLoaded = true;
            }
            catch (Exception ex)
            {
                DataLoaded = false;
                MessageBox.Show("Při načítání dat došlo k chybě. Zkontrolujte připojení k internetu a zkuste to znovu.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Windows.Application.Current.Shutdown();
            }
        }

        public VMMainWindow()
        {
            GetData();
        }

    }
}
