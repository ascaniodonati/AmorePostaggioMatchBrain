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
using System.IO;
using Microsoft.Win32;

namespace AmorePostaggioMatchBrain
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OpenFileDialog ofd = new OpenFileDialog();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnImportCsv_Click(object sender, RoutedEventArgs e)
        {
            
            ofd.Filter = "File CSV | *.csv";
            ofd.Title = "Importa file CSV";

            if (ofd.ShowDialog() == true)
            {
                ApriCsv(ofd.FileName);
            }
        }

        private void ApriCsv(string path)
        {
            CSVUtility.Aggiorna(path);
            cmbNomi.ItemsSource = CSVUtility.ListaNomi();
            cmbNomi2.ItemsSource = CSVUtility.ListaNomi();
            dgrCsv.ItemsSource = CSVUtility.csvList;
        }

        private void btnUnisciDoppioni_Click(object sender, RoutedEventArgs e)
        {
            string doppioni = CSVUtility.Doppioni();

            if (doppioni != null)
            {
                MessageBox.Show($"Controlla questi nomi:\n\n{doppioni}");
            }
        }

        private void dgrCsv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbNomi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((string)cmbNomi.SelectedItem != "")
            {
                dgrAccoppiamenti.ItemsSource = CoppieUtility.VariMatch(CSVUtility.FindByName((string)cmbNomi.SelectedItem));
            }
        }

        private void dgrAccoppiamenti_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbNomi2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((string)cmbNomi2.SelectedItem != "")
            {
                txtResult.Text = CSVUtility.PiselloQuadrato((string)cmbNomi.SelectedItem) + CSVUtility.PiselloQuadrato((string)cmbNomi2.SelectedItem);
            }
        }

        private void btnRefreshCsv_Click(object sender, RoutedEventArgs e)
        {
            ApriCsv(ofd.FileName);
        }
    }
}
