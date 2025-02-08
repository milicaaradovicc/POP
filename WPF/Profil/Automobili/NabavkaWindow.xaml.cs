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
using SR31_2023_POP2024.Repository;
using static SR31_2023_POP2024.Repository.KorisnikRepository;
using SR31_2023_POP2024.Model;
namespace WPF.Profil.Automobili
{
    /// <summary>
    /// Interaction logic for NabavkaWindow.xaml
    /// </summary>
    public partial class NabavkaWindow : Window
    {
        //private Automobil _automobil;
        private PoslovneInfoRepository poslovneInfoRepository;
        private Automobil? _automobil;
        private AutomobiliWindow _automobiliWindow;


        public NabavkaWindow(Automobil automobil, AutomobiliWindow automobiliWindow)
        {
            InitializeComponent();
            _automobil = automobil;
            poslovneInfoRepository = new PoslovneInfoRepository();
            _automobiliWindow = automobiliWindow; 
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (DatumNabavkeDatePicker.SelectedDate == null || string.IsNullOrEmpty(CenaNabavkeTextBox.Text) || !decimal.TryParse(CenaNabavkeTextBox.Text, out decimal cenaNabavke)
                || string.IsNullOrEmpty(CenaProdajeTextBox.Text) || !decimal.TryParse(CenaProdajeTextBox.Text, out decimal cenaProdaje))
            {
                MessageBox.Show("Molimo vas da unesete validne podatke.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DateTime datumNabavke = DatumNabavkeDatePicker.SelectedDate.Value;
            if (datumNabavke > DateTime.Today)
            {
                MessageBox.Show("Nevalidan datum!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var ulogovaniKorisnik = SessionManager.GetTrenutniKorisnik();
            if (ulogovaniKorisnik == null)
            {
                MessageBox.Show("Nema ulogovanog korisnika.");
                return;
            }

            var poslovneInfo = new PoslovneInfo(
                cenaNabavke,
                DatumNabavkeDatePicker.SelectedDate.Value,
                false, 
                cenaProdaje, 
                null, 
                ulogovaniKorisnik, 
                null, 
                _automobil
            );

            if (_automobil == null)
            {
                MessageBox.Show("Automobil nije pravilno prosleđen.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            poslovneInfoRepository.AddPoslovneInfo(poslovneInfo);

            _automobiliWindow.LoadCars();

            MessageBox.Show("Poslovne informacije su uspešno dodate!");

            this.Close();
        }

    }
}

