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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace FinalAppTest
{
    /// <summary>
    /// Interaction logic for LoadingWindow.xaml
    /// </summary>
    public partial class LoadingWindow : Window
    {
        public LoadingWindow()
        {
            InitializeComponent();
            loading();
        }

        DispatcherTimer timer = new DispatcherTimer();

        private void timer_tick(object sender, EventArgs e)
        {
            timer.Stop();
            Hide();
            MainWindow window = new MainWindow();
            window.ShowDialog();
            Close();
        }

        private void loading()
        {
            timer.Tick += timer_tick;
            timer.Interval = new TimeSpan(0, 0, 2);
            timer.Start();
        }
    }
}
