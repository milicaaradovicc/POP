using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace WPF.Ponuda
{
    /// <summary>
    /// Interaction logic for ShoppingWindow.xaml
    /// </summary>
    public partial class ShoppingWindow : Window
    {
        private Automobil _selectedCar;
        private readonly PoslovneInfoRepository _poslovneInfoRepository = new PoslovneInfoRepository();
        private readonly KupacRepository _kupacRepository = new KupacRepository();
        private readonly KorisnikRepository _korisnikRepository = new KorisnikRepository();

        public ShoppingWindow(Automobil selectedCar)
        {
            InitializeComponent();
            _selectedCar = selectedCar ?? throw new ArgumentNullException(nameof(selectedCar));
            PrikaziPodatke(_selectedCar.ID);
        }

        private void PrikaziPodatke(int automobilId)
        {
            var poslovneInfo = _poslovneInfoRepository.GetPoslovneInfo(automobilId);
            if (poslovneInfo != null)
            {
                CenaProdajeTextBlock.Text = poslovneInfo.CenaProdaje.ToString("N2") + " €";
                DatumProdajeTextBlock.Text = DateTime.Now.ToString("dd.MM.yyyy");
            }
            else
            {
                CenaProdajeTextBlock.Text = "N/A";
                DatumProdajeTextBlock.Text = "N/A";
            }
        }

        private void KupiAutomobil_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ImeKupcaTextBox.Text) ||
                string.IsNullOrWhiteSpace(PrezimeKupcaTextBox.Text) ||
                string.IsNullOrWhiteSpace(BrojLicneKarteTextBox.Text))
            {
                MessageBox.Show("Molimo vas popunite sve podatke.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!Regex.IsMatch(BrojLicneKarteTextBox.Text, @"^\d{9}$"))
            {
                MessageBox.Show("Broj lične karte mora imati tačno 9 cifara.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Kupac kupac = _kupacRepository.GetKupac(BrojLicneKarteTextBox.Text);
            if (kupac == null)
            {
                kupac = new Kupac(ImeKupcaTextBox.Text, PrezimeKupcaTextBox.Text, BrojLicneKarteTextBox.Text);
                _kupacRepository.AddKupac(kupac);
            }
            else if (kupac.Ime != ImeKupcaTextBox.Text || kupac.Prezime != PrezimeKupcaTextBox.Text)
            {
                MessageBox.Show("Uneto ime i prezime ne odgovaraju podacima za ovaj broj lične karte!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var poslovneInfo = _poslovneInfoRepository.GetPoslovneInfo(_selectedCar.ID);
            if (poslovneInfo == null)
            {
                MessageBox.Show("Nema podataka o nabavci za ovaj automobil.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            decimal cenaProdaje = poslovneInfo.CenaProdaje;
            decimal nabavnaCena = poslovneInfo.CenaNabavke;
            decimal profit = cenaProdaje - nabavnaCena;
            decimal zaradaProdavca = Math.Max(profit * 0.25m, 150m);

            var datumProdaje = DateTime.Now;

            var novePoslovneInfo = new PoslovneInfo(
                nabavnaCena,
                poslovneInfo.DatumNabavke,
                true, 
                cenaProdaje,
                datumProdaje,
                poslovneInfo.Prodavac,
                kupac,
                _selectedCar
            );

            try
            {
                _poslovneInfoRepository.UpdatePoslovneInfo(novePoslovneInfo);

                if (poslovneInfo.Prodavac != null)
                {
                    _korisnikRepository.UpdateEarnings(poslovneInfo.Prodavac.JMBG, zaradaProdavca);
                }

                MessageBox.Show("Uspešno ste kupili automobil!", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Došlo je do greške: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
