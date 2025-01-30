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
using Microsoft.Data.SqlClient;
using SR31_2023_POP2024.Model;
using static SR31_2023_POP2024.Repository.KorisnikRepository;
using WPF.Profil.Automobili;

namespace WPF.Automobili
{
    /// <summary>
    /// Interaction logic for AddCarWindow.xaml
    /// </summary>
    public partial class AddCarWindow : Window
    {
        private CarRepository carRepository;

        private static string connectionString = "Data Source=localhost;Initial Catalog=POP;Integrated Security=True;Trust Server Certificate=True";


        public AddCarWindow()
        {
            InitializeComponent();
            carRepository = new CarRepository();
            LoadPogoni();

        }
        private void NextButton_Click(object sender, RoutedEventArgs e)
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

            var marka = new MarkaAutomobila(MarkaTextBox.Text, DrzavaTextBox.Text);
            var model = new ModelAutomobila(marka, ModelTextBox.Text);

            var selectedPogonString = PogonComboBox.SelectedItem as string;
            if (selectedPogonString == null)
            {
                MessageBox.Show("Molimo vas da izaberete pogon.");
                return;
            }

            var formattedPogonString = selectedPogonString.Replace(" ", "_");

            Pogon selectedPogon;
            if (!Enum.TryParse(formattedPogonString, out selectedPogon))
            {
                MessageBox.Show("Došlo je do greške prilikom odabira pogona.");
                return;
            }

            Korisnik ulogovaniKorisnik = SessionManager.GetTrenutniKorisnik();
            if (ulogovaniKorisnik == null)
            {
                MessageBox.Show("Nema ulogovanog korisnika.");
                return;
            }
            
                // Kreiraj automobil
                var automobil = new Automobil(
                    0,
                    marka,
                    model,
                    int.Parse(GodisteTextBox.Text),
                    int.Parse(SnagaTextBox.Text),
                    selectedPogon,
                    false
                );

                carRepository.AddCar(automobil);

               NabavkaWindow nabavkaWindow = new NabavkaWindow(automobil); 
            nabavkaWindow.Show();

                this.Close();
            
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
        }




    }
}