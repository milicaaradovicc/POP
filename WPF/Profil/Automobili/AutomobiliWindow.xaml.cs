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
using WPF.Ponuda;
using WPF.Profil.Zarada;
using static SR31_2023_POP2024.Repository.KorisnikRepository;

namespace WPF.Profil.Automobili
{
    /// <summary>
    /// Interaction logic for Automobili.xaml
    /// </summary>
    public partial class AutomobiliWindow : Window
    {
        private readonly ICarService _carService;
        private PoslovneInfoRepository _poslovneInfoRepository;


        public AutomobiliWindow()
        {
            InitializeComponent();
            _carService = new CarService();
            _poslovneInfoRepository = new PoslovneInfoRepository();
            LoadCars();
            CarsDataGrid.SelectionChanged += CarsDataGrid_SelectionChanged;
        }

        public Automobil? SelectedCar { get; set; }

        public void LoadCars()
        {
            var cars = _carService.GetCarsByLoggedUser();
            CarsDataGrid.ItemsSource = cars.Cast<Automobil>().Where(car => !car.Deleted).ToList();
        }

        private void CarsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedCar = (Automobil)CarsDataGrid.SelectedItem;
        }

        private void DetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedCar != null)
            {
                var detailsWindow = new DetailsWindow(SelectedCar);
                detailsWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Molimo izaberite automobil za prikaz detalja.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void AddCarButton_Click(object sender, RoutedEventArgs e)
        {
            AddCarWindow addCarWindow = new AddCarWindow(this);
            if (addCarWindow.ShowDialog() == true)
            {
                LoadCars();
            }
        }

        private void EditCarButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedCar != null)
            {
                var poslovneInfo = _poslovneInfoRepository.GetPoslovneInfo(SelectedCar.ID);

                if (poslovneInfo != null && poslovneInfo.Prodato)
                {
                    MessageBox.Show("Automobil nije dostupan za izmenu.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    var editWindow = new EditWindow(SelectedCar, _carService);
                    if (editWindow.ShowDialog() == true)
                    {
                        LoadCars();
                    }
                }
            }
            else
            {
                MessageBox.Show("Molimo izaberite automobil za izmenu.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void DeleteCarButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedCar != null)
            {
                var result = MessageBox.Show("Da li ste sigurni da želite da obrišete ovaj automobil?",
                                             "Potvrda brisanja",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _carService.DeleteCar(SelectedCar.ID);
                    LoadCars();
                }
            }
            else
            {
                MessageBox.Show("Molimo izaberite automobil za brisanje.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ExitMI_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            SessionManager.OdjavitiKorisnika();
            MessageBox.Show("Uspešno ste se odjavili!");


            var mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            if (mainWindow == null)
            {
                mainWindow = new MainWindow();
                mainWindow.Show();
            }
            else
            {
                mainWindow.Show();
                mainWindow.OsveziMeni();
            }

            this.Close();
        }
        private void PocetnaStranica_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            this.Close();
        }

        private void Ponuda_Click(object sender, RoutedEventArgs e)
        {
            PonudaWindow ponudaWindow = new PonudaWindow();
            ponudaWindow.Show();
        }

        private void PoslovneInfo_Click(object sender, RoutedEventArgs e)
        {
            var selectedCar = CarsDataGrid.SelectedItem as Automobil;

            if (selectedCar != null)
            {
                var nabavkaInfoWindow = new NabavkaInfo(selectedCar);
                nabavkaInfoWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Odaberite automobil za više informacija.","Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CarsDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var automobil = e.Row.Item as Automobil; 
            if (automobil != null)
            {
            
                var poslovneInfo = _poslovneInfoRepository.GetPoslovneInfo(automobil.ID);

                if (poslovneInfo != null && poslovneInfo.Prodato)
                {
                    e.Row.Background = new SolidColorBrush(Colors.Red);  
                    e.Row.Foreground = new SolidColorBrush(Colors.White); 
                }
                else
                {
                    e.Row.Background = new SolidColorBrush(Colors.LightGreen);  
                    e.Row.Foreground = new SolidColorBrush(Colors.Black);        
                }
            }
        }

        private void SellCarButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedCar = CarsDataGrid.SelectedItem as Automobil;

            if (selectedCar != null)
            {
                var poslovneInfo = _poslovneInfoRepository.GetPoslovneInfo(selectedCar.ID);

                if (poslovneInfo != null && poslovneInfo.Prodato)
                {
                    MessageBox.Show("Ovaj automobil je već prodat i nije dostupan za prodaju.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    var prodajaWindow = new ProdajaWindow(selectedCar);
                    prodajaWindow.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Selektujte automobil za prodaju.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Korisnici_Click(object sender, RoutedEventArgs e)
        {
            KorisniciWindow korisniciWindow = new KorisniciWindow();
            korisniciWindow.Show();

            this.Close();
        }

        private void Zarada_Click(object sender, RoutedEventArgs e)
        {
            SalaryWindow salaryWindow = new SalaryWindow();
            salaryWindow.Show();

            this.Close();
        }
        private void Profil_Click(object sender, RoutedEventArgs e)
        {

            ProfilWindow profilWindow = new ProfilWindow();
            profilWindow.Show();

            this.Close();
        }


    }
}
