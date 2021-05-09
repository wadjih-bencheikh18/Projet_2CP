using FinalAppTest.ViewModels;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void PAPS_Button_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new PAPS_ViewModel();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new SimulationPage();
        }
    }
}
