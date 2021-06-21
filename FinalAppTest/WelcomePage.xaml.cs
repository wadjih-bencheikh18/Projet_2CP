using FinalAppTest.Views;
using FinalAppTest.Comparaison;
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
using System.Windows.Threading;

namespace FinalAppTest
{
    /// <summary>
    /// Interaction logic for LoadingPage.xaml
    /// </summary>
    public partial class WelcomePage : Page
    {
        public WelcomePage()
        {
            InitializeComponent();
        }

        private void AproposButton_Click(object sender, MouseButtonEventArgs e)
        {
            MainWindow.main.Content = new APropos();
        }

        private void SimulationButton_Click(object sender, MouseButtonEventArgs e)
        {
            //MainWindow.main.Content = new InitPage();
            MainWindow.main.Content = new DashBoard();
        }

        private void ComparaisonButton_Click(object sender, MouseButtonEventArgs e)
        {
            MainWindow.main.Content = new Compar_Saisie();
        }

        private void Rectangle1_MouseEnter(object sender, MouseEventArgs e)
        {
            Rectangle1.Fill = (Brush)new BrushConverter().ConvertFrom("#F16022");
            Text1.Foreground = (Brush)new BrushConverter().ConvertFrom("#F16022");
        }

        private void Rectangle1_MouseLeave(object sender, MouseEventArgs e)
        {
            Rectangle1.Fill = (Brush)new BrushConverter().ConvertFrom("#000000");
            Text1.Foreground = (Brush)new BrushConverter().ConvertFrom("#000000");
        }

        private void Rectangle2_MouseEnter(object sender, MouseEventArgs e)
        {
            Rectangle2.Fill = (Brush)new BrushConverter().ConvertFrom("#F16022");
            Text2.Foreground = (Brush)new BrushConverter().ConvertFrom("#F16022");
        }

        private void Rectangle2_MouseLeave(object sender, MouseEventArgs e)
        {
            Rectangle2.Fill = (Brush)new BrushConverter().ConvertFrom("#000000");
            Text2.Foreground = (Brush)new BrushConverter().ConvertFrom("#000000");
        }

        private void Rectangle3_MouseEnter(object sender, MouseEventArgs e)
        {
            Rectangle3.Fill = (Brush)new BrushConverter().ConvertFrom("#F16022");
            Text3.Foreground = (Brush)new BrushConverter().ConvertFrom("#F16022");
        }

        private void Rectangle3_MouseLeave(object sender, MouseEventArgs e)
        {
            Rectangle3.Fill = (Brush)new BrushConverter().ConvertFrom("#000000");
            Text3.Foreground = (Brush)new BrushConverter().ConvertFrom("#000000");
        }

        private void Rectangle4_MouseEnter(object sender, MouseEventArgs e)
        {
            Shadow1.Opacity = 0.67;
            Shadow1.ShadowDepth = 4;
            Shadow1.BlurRadius = 10;
        }

        private void Rectangle4_MouseLeave(object sender, MouseEventArgs e)
        {
            Shadow1.Opacity = 0.37;
            Shadow1.ShadowDepth = 2;
            Shadow1.BlurRadius = 4;
        }

        private void Rectangle5_MouseEnter(object sender, MouseEventArgs e)
        {
            Shadow2.Opacity = 0.67;
            Shadow2.ShadowDepth = 4;
            Shadow2.BlurRadius = 10;
        }

        private void Rectangle5_MouseLeave(object sender, MouseEventArgs e)
        {
            Shadow2.Opacity = 0.37;
            Shadow2.ShadowDepth = 2;
            Shadow2.BlurRadius = 4;
        }
    }
}
