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

namespace FinalAppTest.Views
{
    /// <summary>
    /// Logique d'interaction pour APropos.xaml
    /// </summary>
    public partial class APropos : Page
    {
        public APropos()
        {
            InitializeComponent();
        }

        private void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.main.Content = new WelcomePage();
        }

        private void Ellipse_MouseEnter(object sender, MouseEventArgs e)
        {
            shadowHome.ShadowDepth = 2;
            shadowHome.BlurRadius = 7;
        }

        private void Ellipse_MouseLeave(object sender, MouseEventArgs e)
        {
            shadowHome.ShadowDepth = 0;
            shadowHome.BlurRadius = 5;
        }

        private void go1to2_Click(object sender, MouseButtonEventArgs e)
        {
            page1.Visibility = Visibility.Hidden;
            page2.Visibility = Visibility.Visible;
        }

        private void go2to1_Click(object sender, MouseButtonEventArgs e)
        {
            page2.Visibility = Visibility.Hidden;
            page1.Visibility = Visibility.Visible;
        }

        private void go2to3_Click(object sender, MouseButtonEventArgs e)
        {
            page2.Visibility = Visibility.Hidden;
            page3.Visibility = Visibility.Visible;
        }

        private void go3to2_Click(object sender, MouseButtonEventArgs e)
        {
            page3.Visibility = Visibility.Hidden;
            page2.Visibility = Visibility.Visible;
        }
    }
}
