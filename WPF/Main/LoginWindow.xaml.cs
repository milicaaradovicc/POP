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
            Korisnik korisnik = korisnikRepo.GetKorisnikByUsernameAndPassword(korisnickoIme, lozinka);

            if (korisnik != null)
            {
                MessageBox.Show("Uspešno ste se prijavili!");
                // Prebaci na sledeći prozor
            }
            else
            {
                MessageBox.Show("Pogrešno korisničko ime ili lozinka.");
            }
        }
    }
}