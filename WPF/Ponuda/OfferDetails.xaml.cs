using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace WPF.Ponuda
{
    /// <summary>
    /// Interaction logic for OfferDetails.xaml
    /// </summary>
    public partial class OfferDetails : Window
    {
        private readonly PoslovneInfoRepository _poslovneInfoRepository;
        private readonly Automobil _automobil;

        public OfferDetails(Automobil automobil)
        {
            InitializeComponent();
            _poslovneInfoRepository = new PoslovneInfoRepository();
            _automobil = automobil;

            LoadCarDetails();

            LoadBusinessDetails();
        }

        private void LoadCarDetails()
        {
            MarkaTextBlock.Text = _automobil.Marka.Naziv;
            ModelTextBlock.Text = _automobil.Model.NazivModela;
            GodisteTextBlock.Text = _automobil.Godiste.ToString();
            SnagaTextBlock.Text = _automobil.Snaga.ToString();
            PogonTextBlock.Text = _automobil.PogonDisplay.ToString();
        }

        private void LoadBusinessDetails()
        {
            var poslovneInfo = _poslovneInfoRepository.GetPoslovneInfo(_automobil.ID);

            if (poslovneInfo != null)
            {
                ProdavacTextBlock.Text = $"{poslovneInfo.Prodavac.Ime} {poslovneInfo.Prodavac.Prezime}";
                CenaProdajeTextBlock.Text = poslovneInfo.CenaProdaje.ToString("C", CultureInfo.GetCultureInfo("de-DE"));
            }
        }
    }
}