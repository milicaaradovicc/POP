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
using SR31_2023_POP2024;
using SR31_2023_POP2024.Repository;

namespace WPF.Profil
{
    /// <summary>
    /// Interaction logic for KorisnikAdd.xaml
    /// </summary>
    public partial class KorisnikAdd : Window
    {
        private KorisnikRepository korisnikRepository;

        private static string connectionString = "Data Source=localhost;Initial Catalog=POP;Integrated Security=True;Trust Server Certificate=True";

        public event Action? OnUserAdded;
        public KorisnikAdd()
        {
            InitializeComponent();
            korisnikRepository = new KorisnikRepository();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var errorMessage = "Molimo unesite: ";
            bool valid = true;

            if (string.IsNullOrEmpty(ImeTextBox.Text))
            {
                errorMessage += "Ime, ";
                valid = false;
            }

            if (string.IsNullOrEmpty(PrezimeTextBox.Text))
            {
                errorMessage += "Prezime, ";
                valid = false;
            }

            if (string.IsNullOrEmpty(KImeTextBox.Text))
            {
                errorMessage += "Korisnicko ime, ";
                valid = false;
            }

            if (string.IsNullOrEmpty(LozinkaTextBox.Text))
            {
                errorMessage += "Lozinku, ";
                valid = false;
            }

            if (string.IsNullOrEmpty(JMBGTextBox.Text) || !long.TryParse(JMBGTextBox.Text, out long jmbg) || JMBGTextBox.Text.Length != 13)
            {
                errorMessage += "JMBG mora biti broj od tačno 13 cifara, ";
                valid = false;
            }
            else if (korisnikRepository.JMBGExists(JMBGTextBox.Text))
            {
                errorMessage += "JMBG već postoji, ";
                valid = false;
            }


            if (!valid)
            {
                errorMessage = errorMessage.TrimEnd(',', ' ') + ".";
                MessageBox.Show(errorMessage, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var korisnik = new Korisnik(
            ImeTextBox.Text,
            PrezimeTextBox.Text,
            KImeTextBox.Text,
            LozinkaTextBox.Text,
            JMBGTextBox.Text,
            0 
            );

            korisnikRepository.AddKorisnik(korisnik);
            MessageBox.Show("Korisnik je uspešno dodat!");

            OnUserAdded?.Invoke();

            this.Close();

        }

    }
}
