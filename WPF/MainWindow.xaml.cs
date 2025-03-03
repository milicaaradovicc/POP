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
using SR31_2023_POP2024.Model;
using SR31_2023_POP2024.Repository;
using WPF.Main;
using WPF.Ponuda;
using WPF.Profil;
using static SR31_2023_POP2024.Repository.KorisnikRepository;

namespace WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SalonRepository salonRepo = new SalonRepository();
            Salon salon = salonRepo.GetSalon();

            if (salon != null)
            {
                SalonImeTextBlock.Text = salon.Ime;
                SalonAdresaTextBlock.Text = "Adresa: " + salon.Adresa;
            }


            InitializeMenu();
        }

        public void InitializeMenu()
        {
            if (SessionManager.JePrijavljen())
            {
                LoginMenuItem.Header = "Logout";
            }
            else
            {
                LoginMenuItem.Header = "Login";
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (SessionManager.JePrijavljen())
            {
                SessionManager.OdjavitiKorisnika();
                MessageBox.Show("Uspešno ste se odjavili!");
                LoginMenuItem.Header = "Login"; 
            }
            else
            {
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
            }
        }

        private void Ponuda_Click(object sender, RoutedEventArgs e)
        {
            PonudaWindow ponudaWindow = new PonudaWindow();
            ponudaWindow.Show();

            this.Close();
        }

        private void Profil_Click(object sender, RoutedEventArgs e)
        {
            if (!SessionManager.JePrijavljen())
            {
                MessageBox.Show("Morate biti prijavljeni da biste pristupili profilu.", "Obaveštenje",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                return; 
            }

            ProfilWindow profilWindow = new ProfilWindow();
            profilWindow.Show();

            this.Close();
        }

        public void OsveziMeni()
        {
            if (SessionManager.JePrijavljen())
            {
                LoginMenuItem.Header = "Logout";
            }
            else
            {
                LoginMenuItem.Header = "Login";
            }
        }

    }
}
