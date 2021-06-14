using FinalAppTest.ViewModels;
using FinalAppTest.Views;
using Ordonnancement;
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

namespace FinalAppTest
{
    /// <summary>
    /// Interaction logic for InitPage.xaml
    /// </summary>
    public partial class InitPage : Page
    {
        public static Rectangle grey;
        public static DockPanel navigation;

        public InitPage()
        {
            InitializeComponent();
            grey = Grey;
            navigation = Navigation;
        }

        private void PAPS_Button_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new PAPS_ViewModel();
        }

        private void PCA_Button_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new PCA_ViewModel();
        }

        private void PLA_Button_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new PLA_ViewModel();
        }

        private void PCTR_Button_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new PCTR_ViewModel();
        }

        private void PAR_Button_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new PAR_ViewModel();
        }

        private void PSR_Button_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new PSR_ViewModel();
        }

        private void RRButton_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new RR_ViewModel();
        }
        private void PARDButton_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new PARD_ViewModel();
        }
        private void SlackTime_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new SlackTime_ViewModel();
        }
        private void MultiNivButton_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new MultiNiveauViewModel();
        }
        private void MultiNivRecyclageButton_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new MultiNiveauRecyclageViewModel();
        }

        public void Image1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Algos1.Visibility = Visibility.Hidden;
            Algos2.Visibility = Visibility.Visible;
        }

        public void Image2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Algos1.Visibility = Visibility.Visible;
            Algos2.Visibility = Visibility.Hidden;
        }
    }
}
