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
using System.Text.RegularExpressions;

namespace WPF.Profil.Automobili
{
    /// <summary>
    /// Interaction logic for ProdajaWindow.xaml
    /// </summary>
    public partial class ProdajaWindow : Window
    {
        private Automobil _selectedCar;
        private readonly PoslovneInfoRepository _poslovneInfoRepository = new PoslovneInfoRepository();
        private readonly KupacRepository _kupacRepository = new KupacRepository();

        public ProdajaWindow(Automobil selectedCar)
        {
            InitializeComponent();
            _selectedCar = selectedCar ?? throw new ArgumentNullException(nameof(selectedCar));
            PrikaziPoslovneInfo(_selectedCar.ID);
            DatumProdajePicker.SelectedDate = DateTime.Now;
        }

        private void PrikaziPoslovneInfo(int automobilId)
        {
            var poslovneInfo = _poslovneInfoRepository.GetPoslovneInfo(automobilId);

            if (poslovneInfo != null)
            {
                CenaNabavkeTextBlock.Text = poslovneInfo.CenaNabavke.ToString("N2") + " €";
                DatumNabavkeTextBlock.Text = poslovneInfo.DatumNabavke.ToString("dd.MM.yyyy");
                CenaProdajeTextBox.Text = ((int)poslovneInfo.CenaProdaje).ToString();

            }
            else
            {
                CenaNabavkeTextBlock.Text = "N/A";
                DatumNabavkeTextBlock.Text = "N/A";
                CenaProdajeTextBox.Text = string.Empty;
            }
        }

        private void ConfirmSaleButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CenaProdajeTextBox.Text) ||
                DatumProdajePicker.SelectedDate == null ||
                string.IsNullOrWhiteSpace(ImeKupcaTextBox.Text) ||
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

            var kupacRepository = new KupacRepository();
            Kupac? kupac = kupacRepository.GetKupac(BrojLicneKarteTextBox.Text);

            if (kupac == null) 
            {
                kupac = new Kupac(ImeKupcaTextBox.Text, PrezimeKupcaTextBox.Text, BrojLicneKarteTextBox.Text);
                int kupacId = kupacRepository.AddKupac(kupac);
            }
            else 
            {
                if (kupac.Ime != ImeKupcaTextBox.Text || kupac.Prezime != PrezimeKupcaTextBox.Text)
                {
                    MessageBox.Show("Uneto ime i prezime ne odgovaraju podacima za ovaj broj lične karte!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            if (!decimal.TryParse(CenaProdajeTextBox.Text, out decimal cenaProdaje))
            {
                MessageBox.Show("Neispravan unos za cenu prodaje.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var datumProdaje = DatumProdajePicker.SelectedDate.Value;

            var postojecePoslovneInfo = _poslovneInfoRepository.GetPoslovneInfo(_selectedCar.ID);

            if (postojecePoslovneInfo == null)
            {
                MessageBox.Show("Nema podataka o nabavci za ovaj automobil.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            decimal nabavnaCena = postojecePoslovneInfo.CenaNabavke;
            decimal profit = cenaProdaje - nabavnaCena;
            decimal zaradaProdavca = Math.Max(profit * 0.25m, 150m);

            var poslovneInfo = new PoslovneInfo(
                nabavnaCena,
                postojecePoslovneInfo.DatumNabavke,
                true, 
                cenaProdaje,
                datumProdaje,
                SessionManager.GetTrenutniKorisnik(),
                kupac,
                _selectedCar
            );

            try
            {
                _poslovneInfoRepository.UpdatePoslovneInfo(poslovneInfo);
                var automobiliWindow = Application.Current.Windows.OfType<AutomobiliWindow>().FirstOrDefault();
                if (automobiliWindow != null)
                {
                    automobiliWindow.LoadCars();  
                }

                var prodavac = SessionManager.GetTrenutniKorisnik();
                if (prodavac != null)
                {
                    var korisnikRepo = new KorisnikRepository();
                    korisnikRepo.UpdateEarnings(prodavac.JMBG, zaradaProdavca);
                }

                MessageBox.Show($"Automobil je uspešno prodat. Vaša zarada: {zaradaProdavca:N2} €", "Uspešno", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Došlo je do greške: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}