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
    public partial class LoadingPage : Page
    {
        DispatcherTimer timer = new DispatcherTimer();

        public LoadingPage(int t)
        {
            InitializeComponent();
            loading(t);
        }

        public LoadingPage()
        {
            InitializeComponent();
            loading(0);
        }

        private void timer_tick(object sender, EventArgs e)
        {
            timer.Stop();
            MAIN_GRID.Children.Remove(BackgroundLoad);
        }

        private void loading(int t)
        {
            timer.Tick += timer_tick;
            timer.Interval = new TimeSpan(0, 0, t);
            timer.Start();
        }

        private void SimulationButton_Click(object sender, MouseButtonEventArgs e)
        {
            MainWindow.main.Content = new InitPage();
        }

        private void ComparaisonButton_Click(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
