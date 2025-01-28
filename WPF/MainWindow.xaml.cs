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
using SR31_2023_POP2024.Model;
using SR31_2023_POP2024.Repository;
using WPF.Main;

namespace WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SalonRepository salonRepo = new SalonRepository();
            Salon salon = salonRepo.GetSalon();

            // Postavljanje podataka o salonu u TextBlock
            if (salon != null)
            {
                SalonImeTextBlock.Text =  salon.Ime;
                SalonAdresaTextBlock.Text = "Adresa: " + salon.Adresa;
            }
        }

        

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            // Otvara LoginWindow kada se klikne na "Login"
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show(); // Otvara Login prozor
        }

    }
}
