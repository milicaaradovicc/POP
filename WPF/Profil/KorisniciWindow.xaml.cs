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
using static SR31_2023_POP2024.Repository.KorisnikRepository;

namespace WPF.Profil
{
    /// <summary>
    /// Interaction logic for KorisniciWindow.xaml
    /// </summary>
    public partial class KorisniciWindow : Window
    {
        private KorisnikRepository korisnikRepository = new KorisnikRepository();

        public Korisnik? SelectedUser { get; set; }

        public KorisniciWindow()
        {
            InitializeComponent();
            UcitajKorisnike();
            UsersDataGrid.SelectionChanged += UserDataGrid_SelectionChanged;

        }

        public void UcitajKorisnike()
        {
            List<Korisnik> korisnici = korisnikRepository.GetAllKorisnici();
            UsersDataGrid.ItemsSource = korisnici;
        }

        private void UserDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedUser = (Korisnik)UsersDataGrid.SelectedItem;
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            SessionManager.OdjavitiKorisnika();
            MessageBox.Show("Uspešno ste se odjavili!");

            var mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            if (mainWindow != null)
            {
                mainWindow.OsveziMeni();
            }

            this.Close(); 
        }

        private void KorisnikDetails_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedUser != null)
            {
                var detailsWindow = new KorisnikDetails(SelectedUser);
                detailsWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Molimo izaberite korisnika za prikaz detalja.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void KorisnikAdd_Click(object sender, RoutedEventArgs e)
        {
            KorisnikAdd addKorisnik = new KorisnikAdd();
            addKorisnik.OnUserAdded += UcitajKorisnike;  
            addKorisnik.ShowDialog();

        }

        private void KorisnikDelete_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedUser != null)
            {
                var result = MessageBox.Show("Da li ste sigurni da želite da obrišete ovog korisnika?",
                                             "Potvrda brisanja",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    korisnikRepository.DeleteKorisnik(SelectedUser.JMBG);
                    UcitajKorisnike();  
                    MessageBox.Show("Korisnik je uspešno obrisan.");
                }
            }
            else
            {
                MessageBox.Show("Molimo izaberite korisnika za brisanje.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
 }