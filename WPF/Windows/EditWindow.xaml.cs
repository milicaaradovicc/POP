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
using SR31_2023_POP2024.Service;

namespace WPF.Windows
{
    /// <summary>
    /// Interaction logic for EditWindowxaml.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        private readonly CarService _carService;  
        private Automobil selectedCar;  

        // Konstruktor
        public EditWindow(Automobil car, CarService carService)
        {
            InitializeComponent();
            selectedCar = car;  
            _carService = carService; 
            this.DataContext = selectedCar;  
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            _carService.EditCar(selectedCar.ID.ToString(), selectedCar); 
            MessageBox.Show("Uspešno ste izmenili automobil!");  
            this.Close();  
        }
    }
}