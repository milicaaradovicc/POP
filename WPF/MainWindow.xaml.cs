using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SR31_2023_POP2024;
using SR31_2023_POP2024.Service;
using WPF.Windows;

namespace WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
  
    public partial class MainWindow : Window
    {
        private readonly ICarService _carService;

        public MainWindow()
        {
            InitializeComponent();
            _carService = new CarService();
            var cars = _carService.GetAllCars();
            CarsDataGrid.ItemsSource = cars.Where(car => !car.Deleted).ToList();
            CarsDataGrid.SelectionChanged += CarsDataGrid_SelectionChanged;

        }

        public Automobil SelectedCar { get; set; }

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
            AddCarWindow addCarWindow = new AddCarWindow();
            addCarWindow.ShowDialog(); 

            var cars = _carService.GetAllCars();
            CarsDataGrid.ItemsSource = cars.Where(car => !car.Deleted).ToList();
        }

        private void EditCarButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedCar != null)
            {
                var carService = new CarService(); 

                var editWindow = new EditWindow(SelectedCar, carService); 
                editWindow.ShowDialog();  

                var cars = carService.GetAllCars();  
                CarsDataGrid.ItemsSource = cars.Where(car => !car.Deleted).ToList();  
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

                    var cars = _carService.GetAllCars();
                    CarsDataGrid.ItemsSource = cars.Where(car => !car.Deleted).ToList();
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

       
    }

}
