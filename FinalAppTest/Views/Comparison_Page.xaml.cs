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
    /// Logique d'interaction pour Comparison_Page.xaml
    /// </summary>
    public partial class Comparison_Page : Page
    {
        private int pageIndex = 0;
        public Comparison_Page()
        {
            InitializeComponent();
        }

        private void Swipe(object sender, MouseButtonEventArgs e)
        {
            if (pageIndex == 0)
            {
                Page1.Visibility = Visibility.Hidden;
                Page2.Visibility = Visibility.Visible;
            }
            else
            {
                Page2.Visibility = Visibility.Hidden;
                Page1.Visibility = Visibility.Visible;
            }
            pageIndex = (pageIndex + 1) % 2;
        }

        private void RightHover(object sender, MouseEventArgs e)
        {
            RightEffect.ShadowDepth = 10;
        }
        private void RightLeave(object sender, MouseEventArgs e)
        {
            RightEffect.ShadowDepth = 6;
        }
        private void LeftHover(object sender, MouseEventArgs e)
        {
            LeftEffect.ShadowDepth = 10;
        }
        private void LeftLeave(object sender, MouseEventArgs e)
        {
            LeftEffect.ShadowDepth = 6;
        }
    }
}