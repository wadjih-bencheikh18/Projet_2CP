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

namespace Video2
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
        public class Processus
        {
            public string id{ get; set; }
            public string duree { get; set; }
            public string tempsArriv { get; set; }
            public string tempsFin { get; set; }
            public string tempsService { get; set; }
            public string Priorite { get; set; }
            
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
