using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace WPF.Profil.Zarada
{
    /// <summary>
    /// Interaction logic for AccountWindow.xaml
    /// </summary>
    public partial class AccountWindow : Window
    {
        public ObservableCollection<PoslovneInfo> PoslovneInfoDetails { get; set; }

        public AccountWindow(PoslovneInfo poslovneInfo)
        {
            InitializeComponent();
            PoslovneInfoDetails = new ObservableCollection<PoslovneInfo>
        {
            poslovneInfo 
        };
            DataContext = this; 
        }
    }
}

