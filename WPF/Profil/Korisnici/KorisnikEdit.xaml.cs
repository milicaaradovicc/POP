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

namespace WPF.Profil
{
    /// <summary>
    /// Interaction logic for KorisnikEdit.xaml
    /// </summary>
    public partial class KorisnikEdit : Window
    {
        private readonly KorisnikRepository _korisnikRepository;
        private Korisnik _korisnik;

        public KorisnikEdit(Korisnik korisnik, KorisnikRepository korisnikRepository)
        {
            InitializeComponent();
            _korisnikRepository = korisnikRepository;
            _korisnik = korisnik;
            this.DataContext = _korisnik;
        }
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
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
            else if (_korisnikRepository.JMBGExists(JMBGTextBox.Text) && JMBGTextBox.Text != _korisnik.JMBG)
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

            _korisnik.Ime = ImeTextBox.Text;
            _korisnik.Prezime = PrezimeTextBox.Text;
            _korisnik.KorisnickoIme = KImeTextBox.Text;
            _korisnik.Lozinka = LozinkaTextBox.Text;
            _korisnik.JMBG = JMBGTextBox.Text;

            _korisnikRepository.EditKorisnik(_korisnik);  

            MessageBox.Show("Podaci su uspešno izmenjeni.");
            this.Close();
        }
    }
}
