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
using Microsoft.Data.SqlClient;
using SR31_2023_POP2024;
using SR31_2023_POP2024.Model;
using SR31_2023_POP2024.Service;

namespace WPF.Automobili
{
    /// <summary>
    /// Interaction logic for EditWindowxaml.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        private readonly ICarService _carService;  
        private Automobil selectedCar;
        private static string connectionString = "Data Source=localhost;Initial Catalog=POP;Integrated Security=True;Trust Server Certificate=True";


        public EditWindow(Automobil car, ICarService carService)  
        {
            InitializeComponent();
            selectedCar = car;
            _carService = carService;
            this.DataContext = selectedCar;
            LoadPogoni();
        }

        private void LoadPogoni()
        {
            var pogoni = new List<string>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT Naziv FROM Pogon";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var pogon = reader.GetString(0);
                            pogoni.Add(pogon.Replace("_", " "));  
                        }
                    }
                }
            }

            PogonComboBox.ItemsSource = pogoni;  

            var selectedPogonString = selectedCar.Pogon.ToString().Replace("_", " ");
            PogonComboBox.SelectedItem = pogoni.FirstOrDefault(p => p == selectedPogonString);
        }
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            var errorMessage = "Molimo unesite: ";
            bool valid = true;

            if (string.IsNullOrEmpty(MarkaTextBox.Text))
            {
                errorMessage += "Marku, ";
                valid = false;
            }

            if (string.IsNullOrEmpty(DrzavaTextBox.Text))
            {
                errorMessage += "Državu nastanka, ";
                valid = false;
            }

            if (string.IsNullOrEmpty(ModelTextBox.Text))
            {
                errorMessage += "Model, ";
                valid = false;
            }

            if (string.IsNullOrEmpty(GodisteTextBox.Text) || !int.TryParse(GodisteTextBox.Text, out int godina) || godina <= 0)
            {
                errorMessage += "Validno Godište (broj), ";
                valid = false;
            }

            if (string.IsNullOrEmpty(SnagaTextBox.Text) || !int.TryParse(SnagaTextBox.Text, out int snaga) || snaga <= 0)
            {
                errorMessage += "Validnu Snagu (broj), ";
                valid = false;
            }

            if (PogonComboBox.SelectedItem == null)
            {
                errorMessage += "Pogon, ";
                valid = false;
            }

            if (!valid)
            {
                errorMessage = errorMessage.TrimEnd(',', ' ') + ".";
                MessageBox.Show(errorMessage, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var selectedPogonString = PogonComboBox.SelectedItem as string;
            if (selectedPogonString == null)
            {
                MessageBox.Show("Molimo vas da izaberete pogon.");
                return;
            }

            var formattedPogonString = selectedPogonString.Replace(" ", "_");

         
           
            _carService.EditCar(selectedCar.ID.ToString(), selectedCar);
            MessageBox.Show("Uspešno ste izmenili automobil!");

          //  var tabela = Application.Current.MainWindow as Tabela;
         //   tabela?.LoadCars();

            this.Close();
        }
    }
}