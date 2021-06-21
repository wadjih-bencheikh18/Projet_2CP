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
using FinalAppTest.ViewModels;
using FinalAppTest.Views;

namespace FinalAppTest
{
    /// <summary>
    /// Interaction logic for DashBoard.xaml
    /// </summary>
    public partial class DashBoard : Page
    {
        public DashBoard()
        {
            InitializeComponent();
        }

        private void Home_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.main.Content = new WelcomePage();
        }

        private void Home_MouseEnter(object sender, MouseEventArgs e)
        {
            shadowHome.ShadowDepth = 2;
            shadowHome.BlurRadius = 7;
        }

        private void Home_MouseLeave(object sender, MouseEventArgs e)
        {
            shadowHome.ShadowDepth = 0;
            shadowHome.BlurRadius = 5;
        }

        private void Rectangle_MouseLeftButtonDown1(object sender, MouseButtonEventArgs e)
        {
            MainWindow.main.Content = new InitPage
            {
                DataContext = new PAPS_ViewModel()
            };
        }

        private void Rectangle_MouseEnter1(object sender, MouseEventArgs e)
        {
            Rectangle1.Opacity = 0.7;
            Shadow1.Opacity = 0.37;
        }

        private void Rectangle_MouseLeave1(object sender, MouseEventArgs e)
        {
            Rectangle1.Opacity = 0.5;
            Shadow1.Opacity = 0.17;
        }

        private void Rectangle_MouseLeftButtonDown2(object sender, MouseButtonEventArgs e)
        {
            MainWindow.main.Content = new InitPage
            {
                DataContext = new RR_ViewModel()
            };
        }

        private void Rectangle_MouseEnter2(object sender, MouseEventArgs e)
        {
            Rectangle2.Opacity = 0.7;
            Shadow2.Opacity = 0.37;
        }

        private void Rectangle_MouseLeave2(object sender, MouseEventArgs e)
        {
            Rectangle2.Opacity = 0.5;
            Shadow2.Opacity = 0.17;
        }

        private void Rectangle_MouseLeftButtonDown3(object sender, MouseButtonEventArgs e)
        {
            MainWindow.main.Content = new InitPage
            {
                DataContext = new PSR_ViewModel()
            };
        }

        private void Rectangle_MouseEnter3(object sender, MouseEventArgs e)
        {
            Rectangle3.Opacity = 0.7;
            Shadow3.Opacity = 0.37;
        }

        private void Rectangle_MouseLeave3(object sender, MouseEventArgs e)
        {
            Rectangle3.Opacity = 0.5;
            Shadow3.Opacity = 0.17;
        }

        private void Rectangle_MouseLeftButtonDown4(object sender, MouseButtonEventArgs e)
        {
            MainWindow.main.Content = new InitPage
            {
                DataContext = new PAR_ViewModel()
            };
        }

        private void Rectangle_MouseEnter4(object sender, MouseEventArgs e)
        {
            Rectangle4.Opacity = 0.7;
            Shadow4.Opacity = 0.37;
        }

        private void Rectangle_MouseLeave4(object sender, MouseEventArgs e)
        {
            Rectangle4.Opacity = 0.5;
            Shadow4.Opacity = 0.17;
        }

        private void Rectangle_MouseLeftButtonDown5(object sender, MouseButtonEventArgs e)
        {
            MainWindow.main.Content = new InitPage
            {
                DataContext = new PARD_ViewModel()
            };
        }

        private void Rectangle_MouseEnter5(object sender, MouseEventArgs e)
        {
            Rectangle5.Opacity = 0.7;
            Shadow5.Opacity = 0.37;
        }

        private void Rectangle_MouseLeave5(object sender, MouseEventArgs e)
        {
            Rectangle5.Opacity = 0.5;
            Shadow5.Opacity = 0.17;
        }

        private void Rectangle_MouseLeftButtonDown6(object sender, MouseButtonEventArgs e)
        {
            MainWindow.main.Content = new InitPage
            {
                DataContext = new PCA_ViewModel()
            };
        }

        private void Rectangle_MouseEnter6(object sender, MouseEventArgs e)
        {
            Rectangle6.Opacity = 0.7;
            Shadow6.Opacity = 0.37;
        }

        private void Rectangle_MouseLeave6(object sender, MouseEventArgs e)
        {
            Rectangle6.Opacity = 0.5;
            Shadow6.Opacity = 0.17;
        }

        private void Rectangle_MouseLeftButtonDown7(object sender, MouseButtonEventArgs e)
        {
            MainWindow.main.Content = new InitPage
            {
                DataContext = new PCTR_ViewModel()
            };
        }

        private void Rectangle_MouseEnter7(object sender, MouseEventArgs e)
        {
            Rectangle7.Opacity = 0.7;
            Shadow7.Opacity = 0.37;
        }

        private void Rectangle_MouseLeave7(object sender, MouseEventArgs e)
        {
            Rectangle7.Opacity = 0.5;
            Shadow7.Opacity = 0.17;
        }

        private void Rectangle_MouseLeftButtonDown8(object sender, MouseButtonEventArgs e)
        {
            MainWindow.main.Content = new InitPage
            {
                DataContext = new PLA_ViewModel()
            };
        }

        private void Rectangle_MouseEnter8(object sender, MouseEventArgs e)
        {
            Rectangle8.Opacity = 0.7;
            Shadow8.Opacity = 0.37;
        }

        private void Rectangle_MouseLeave8(object sender, MouseEventArgs e)
        {
            Rectangle8.Opacity = 0.5;
            Shadow8.Opacity = 0.17;
        }

        private void Rectangle_MouseLeftButtonDown9(object sender, MouseButtonEventArgs e)
        {
            MainWindow.main.Content = new InitPage
            {
                DataContext = new MultiNiveauViewModel()
            };
        }

        private void Rectangle_MouseEnter9(object sender, MouseEventArgs e)
        {
            Rectangle9.Opacity = 0.7;
            Shadow9.Opacity = 0.37;
        }

        private void Rectangle_MouseLeave9(object sender, MouseEventArgs e)
        {
            Rectangle9.Opacity = 0.5;
            Shadow9.Opacity = 0.17;
        }

        private void Rectangle_MouseLeftButtonDown10(object sender, MouseButtonEventArgs e)
        {
            MainWindow.main.Content = new InitPage
            {
                DataContext = new MultiNiveauRecyclageViewModel()
            };
        }

        private void Rectangle_MouseEnter10(object sender, MouseEventArgs e)
        {
            Rectangle10.Opacity = 0.7;
            Shadow10.Opacity = 0.37;
        }

        private void Rectangle_MouseLeave10(object sender, MouseEventArgs e)
        {
            Rectangle10.Opacity = 0.5;
            Shadow10.Opacity = 0.17;
        }

        private void Rectangle_MouseLeftButtonDown11(object sender, MouseButtonEventArgs e)
        {
            MainWindow.main.Content = new InitPage
            {
                DataContext = new SlackTime_ViewModel()
            };
        }

        private void Rectangle_MouseEnter11(object sender, MouseEventArgs e)
        {
            Rectangle11.Opacity = 0.7;
            Shadow11.Opacity = 0.37;
        }

        private void Rectangle_MouseLeave11(object sender, MouseEventArgs e)
        {
            Rectangle11.Opacity = 0.5;
            Shadow11.Opacity = 0.17;
        }

        private void Swipe(object sender, MouseEventArgs e)
        {
            if (Algos2.Visibility == Visibility.Hidden)
            {
                Algos1.Visibility = Visibility.Hidden;
                Algos2.Visibility = Visibility.Visible;
            }
            else
            {
                Algos2.Visibility = Visibility.Hidden;
                Algos1.Visibility = Visibility.Visible;
            }
        }

        /*private void Rectangle_MouseLeftButtonDown12(object sender, MouseButtonEventArgs e)
        {
            MainWindow.main.Content = new InitPage
            {
                DataContext = new PAPS_ViewModel()
            };
        }

        private void Rectangle_MouseEnter12(object sender, MouseEventArgs e)
        {
            Rectangle12.Opacity = 0.7;
            Shadow12.Opacity = 0.37;
        }

        private void Rectangle_MouseLeave12(object sender, MouseEventArgs e)
        {
            Rectangle12.Opacity = 0.5;
            Shadow12.Opacity = 0.17;
        }*/
    }
}
