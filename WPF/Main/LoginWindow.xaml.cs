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
using SR31_2023_POP2024.Repository;
using SR31_2023_POP2024;
using static SR31_2023_POP2024.Repository.KorisnikRepository;

namespace WPF.Main
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string korisnickoIme = KorisnickoImeTextBox.Text;
            string lozinka = LozinkaPasswordBox.Password;

            KorisnikRepository korisnikRepo = new KorisnikRepository();
            Korisnik korisnik = korisnikRepo.GetKorisnikByUsername(korisnickoIme); 

            if (korisnik == null)
            {
                MessageBox.Show("Korisnik nije pronađen. Proverite korisničko ime.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (korisnik.Lozinka != lozinka)
            {
                MessageBox.Show("Pogrešna lozinka. Proverite lozinku.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("Uspešno ste se prijavili!");

                SessionManager.PrijaviKorisnika(korisnik);

                MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                mainWindow?.InitializeMenu();

                this.Close();
            }
        }
    }

}
