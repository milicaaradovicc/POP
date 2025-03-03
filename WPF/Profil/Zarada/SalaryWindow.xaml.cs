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
using System.Windows.Shapes;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot;
using SR31_2023_POP2024;
using SR31_2023_POP2024.Repository;
using WPF.Ponuda;
using WPF.Profil.Automobili;
using static SR31_2023_POP2024.Repository.KorisnikRepository;

namespace WPF.Profil.Zarada
{
    /// <summary>
    /// Interaction logic for SalaryWindow.xaml
    /// </summary>
    public partial class SalaryWindow : Window
    {
        private PoslovneInfoRepository _poslovneInfoRepository;
        private PoslovneInfo _selectedPoslovneInfo;

        public SalaryWindow()
        {
            InitializeComponent();
            _poslovneInfoRepository = new PoslovneInfoRepository();
            LoadSalary();
            SalaryDataGrid.SelectionChanged += SalaryDataGrid_SelectionChanged;
            Korisnik ulogovaniKorisnik = SessionManager.GetTrenutniKorisnik();

            if (ulogovaniKorisnik != null)
            {
                Dispatcher.Invoke(() =>
                {
                    TotalEarningsText.Text =  ulogovaniKorisnik.Zarada.ToString("0.00") + "€";
                });
            }
            else
            {
                MessageBox.Show("Nema ulogovanog korisnika.");
            }
        }

        private void SalaryDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedPoslovneInfo = (PoslovneInfo)SalaryDataGrid.SelectedItem;
        }
        public void LoadSalary()
        {
            var poslovneInfo = _poslovneInfoRepository.GetAllPoslovneInfo();
            SalaryDataGrid.ItemsSource = poslovneInfo.Cast<PoslovneInfo>();
        }

        private void Account_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedPoslovneInfo != null)
            {
                AccountWindow accountWindow = new AccountWindow(_selectedPoslovneInfo);
                accountWindow.Show();
            }
            else
            {
                MessageBox.Show("Molimo izaberite stavku iz tabele.");
            }
        }

        private void PocetnaStranica_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            this.Close();
        }

        private void Ponuda_Click(object sender, RoutedEventArgs e)
        {
            PonudaWindow ponudaWindow = new PonudaWindow();
            ponudaWindow.Show();
        }

        private void Automobili_Click(object sender, RoutedEventArgs e)
        {
            AutomobiliWindow automobiliWindow = new AutomobiliWindow();
            automobiliWindow.Show();

            this.Close();
        }

        private void Korisnici_Click(object sender, RoutedEventArgs e)
        {
            KorisniciWindow korisniciWindow = new KorisniciWindow();
            korisniciWindow.Show();

            this.Close();
        }
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            SessionManager.OdjavitiKorisnika();
            MessageBox.Show("Uspešno ste se odjavili!");


            var mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            if (mainWindow == null)
            {
                mainWindow = new MainWindow();
                mainWindow.Show();
            }
            else
            {
                mainWindow.Show();
                mainWindow.OsveziMeni();
            }

            this.Close();
        }
        private void Profil_Click(object sender, RoutedEventArgs e)
        {

            ProfilWindow profilWindow = new ProfilWindow();
            profilWindow.Show();

            this.Close();
        }

    }

}
