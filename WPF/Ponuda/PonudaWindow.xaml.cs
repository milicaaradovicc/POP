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
using SR31_2023_POP2024.Service;
using WPF.Automobili;
using WPF.Profil.Automobili;

namespace WPF.Ponuda
{
    /// <summary>
    /// Interaction logic for PonudaWindow.xaml
    /// </summary>
    public partial class PonudaWindow : Window
    {
        private CarRepository _carRepository;
        public Automobil? SelectedCar { get; set; }

        public PonudaWindow()
        {
            InitializeComponent();
            _carRepository = new CarRepository(); 
            CarsDataGrid.SelectionChanged += CarsDataGrid_SelectionChanged;
            LoadCarData();
        }

        private void LoadCarData()
        {
            var cars = _carRepository.GetAllCars();

            CarsDataGrid.ItemsSource = cars;
        }

        private void OfferDetails_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedCar != null)
            {
                var offerDetails = new OfferDetails(SelectedCar);
                offerDetails.ShowDialog();
            }
            else
            {
                MessageBox.Show("Molimo izaberite automobil za prikaz detalja.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CarsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedCar = (Automobil)CarsDataGrid.SelectedItem;
        }

        private void BuyCar_Click(object sender, RoutedEventArgs e)
        {
            if (CarsDataGrid.SelectedItem is Automobil selectedCar)
            {
                ShoppingWindow shoppingWindow = new ShoppingWindow(selectedCar);
                shoppingWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Molimo vas da izaberete automobil za kupovinu.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void PocetnaStranica_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            this.Close();
        }

    }


}

