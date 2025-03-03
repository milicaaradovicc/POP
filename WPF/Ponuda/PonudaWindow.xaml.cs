using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private ICollectionView _carCollectionView;

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
            _carCollectionView = CollectionViewSource.GetDefaultView(cars);
            CarsDataGrid.ItemsSource = cars;
        }
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_carCollectionView != null)
            {
                string searchText = SearchTextBox.Text.ToLower(); 
                _carCollectionView.Filter = (item) =>
                {
                    var car = item as Automobil;
                    if (car != null)
                    {
                        return car.Marka.Naziv.ToLower().Contains(searchText) ||
                               car.Model.NazivModela.ToLower().Contains(searchText) ||
                               car.Godiste.ToString().Contains(searchText);
                    }
                    return false;
                };

                if (_carCollectionView.IsEmpty)
                {
                    NoResultsText.Visibility = Visibility.Visible;
                }
                else
                {
                    NoResultsText.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchTextBox.Text == "Pretraga...")
            {
                SearchTextBox.Text = "";
            }
        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SearchTextBox.Text))
            {
                SearchTextBox.Text = "Pretraga...";
            }
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

