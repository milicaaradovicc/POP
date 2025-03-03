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

namespace WPF.Profil.Automobili
{
    /// <summary>
    /// Interaction logic for NabavkaInfo.xaml
    /// </summary>
    public partial class NabavkaInfo : Window
    {
        private PoslovneInfoRepository poslovneInfoRepository;
        public NabavkaInfo(Automobil selectedCar)
        {
            InitializeComponent();
            poslovneInfoRepository = new PoslovneInfoRepository();

            LoadPoslovneInfo(selectedCar);
        }

        private void LoadPoslovneInfo(Automobil selectedCar)
        {
          
            var poslovneInfo = poslovneInfoRepository.GetPoslovneInfo(selectedCar.ID);

            if (poslovneInfo != null)
            {
                CenaNabavkeTextBlock.Text = $"{poslovneInfo.CenaNabavke:N2} €";
                DatumNabavkeTextBlock.Text = poslovneInfo.DatumNabavke.ToString("dd/MM/yyyy");
                CenaProdajeTextBlock.Text = $"{poslovneInfo.CenaProdaje:N2} €";
            }
        }
    }
}