using Microsoft.Win32;
using PGL2CNB.Viewmodels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PGL2CNB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private VMMainWindow _vm;

        public MainWindow()
        {
            _vm = new VMMainWindow();
            InitializeComponent();
            this.DataContext = _vm;
        }

        private void BtnSetPath_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "HTML file (*.html)|*.html|HTM file (*.htm)|*.htm";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (saveFileDialog.ShowDialog() == true)
            {
                _vm.FilePath = saveFileDialog.FileName;
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            _vm.Clear();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            _vm.CreateFile();
        }
    }
}
