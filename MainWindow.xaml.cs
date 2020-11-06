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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AirMite
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            grappeB1.Visibility = Visibility.Collapsed;
            grappeB2.Visibility = Visibility.Collapsed;
            grappeB3.Visibility = Visibility.Collapsed;
            grappeB4.Visibility = Visibility.Collapsed;
            grappeB5.Visibility = Visibility.Collapsed;
            grappeB6.Visibility = Visibility.Collapsed;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Hides "play" menu
            grappeA1.Visibility = Visibility.Collapsed;
            grappeA2.Visibility = Visibility.Collapsed;
            grappeA3.Visibility = Visibility.Collapsed;

            grappeB1.Visibility = Visibility.Visible;
            grappeB2.Visibility = Visibility.Visible;
            grappeB3.Visibility = Visibility.Visible;
            grappeB4.Visibility = Visibility.Visible;
            grappeB5.Visibility = Visibility.Visible;
            grappeB6.Visibility = Visibility.Visible;
        }
    }
}
