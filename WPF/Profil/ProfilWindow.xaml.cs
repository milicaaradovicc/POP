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
using SR31_2023_POP2024;
using WPF.Ponuda;
using WPF.Profil.Automobili;
using WPF.Profil.Zarada;
using static SR31_2023_POP2024.Repository.KorisnikRepository;

namespace WPF.Profil
{
    /// <summary>
    /// Interaction logic for ProfilWindow.xaml
    /// </summary>
    public partial class ProfilWindow : Window
    {
        public ProfilWindow()
        {
            InitializeComponent();

            Korisnik ulogovaniKorisnik = SessionManager.GetTrenutniKorisnik();

            if (ulogovaniKorisnik != null)
            {
                ImeTextBlock.Text = "Ime: " + ulogovaniKorisnik.Ime;
                PrezimeTextBlock.Text = "Prezime: " + ulogovaniKorisnik.Prezime;
                KorisnickoImeTextBlock.Text = "Korisnicko ime: " + ulogovaniKorisnik.KorisnickoIme;
                JMBGTextBlock.Text = "JMBG: " + ulogovaniKorisnik.JMBG;
            }
            else
            {
                MessageBox.Show("Nema ulogovanog korisnika.");
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            SessionManager.OdjavitiKorisnika();
            MessageBox.Show("Uspešno ste se odjavili!");

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            this.Close(); 
        }

        private void Korisnici_Click(object sender, RoutedEventArgs e)
        {
            KorisniciWindow korisniciWindow = new KorisniciWindow();
            korisniciWindow.Show();

            this.Close();
        }

        private void Automobili_Click(object sender, RoutedEventArgs e)
        {
            AutomobiliWindow automobiliWindow = new AutomobiliWindow();
            automobiliWindow.Show();

            this.Close();
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

        private void Zarada_Click(object sender, RoutedEventArgs e)
        {
            SalaryWindow salaryWindow = new SalaryWindow();
            salaryWindow.Show();

            this.Close();
        }

    }

}
