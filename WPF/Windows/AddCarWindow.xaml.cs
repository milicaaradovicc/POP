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

namespace WPF.Windows
{
    /// <summary>
    /// Interaction logic for AddCarWindow.xaml
    /// </summary>
    public partial class AddCarWindow : Window
    {
        private CarRepository carRepository;

        public AddCarWindow()
        {
            InitializeComponent();
            carRepository = new CarRepository();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var id = carRepository.GetNextCarId(); 

            
            var marka = new MarkaAutomobila(MarkaTextBox.Text, DrzavaTextBox.Text);
            var model = new ModelAutomobila(marka, ModelTextBox.Text);

            var automobil = new Automobil(
                id,
                marka,
                model,
                int.Parse(GodisteTextBox.Text),
                int.Parse(SnagaTextBox.Text),
                PogonTextBox.Text
            );

            carRepository.AddCar(automobil); 
            MessageBox.Show("Automobil je uspešno dodat!");

            this.Close(); 
        }

    }
}