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
        public InitPage()
        {
            InitializeComponent();
        }

        private void PAPS_Button_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new PAPS_ViewModel();
        }

        private void PCA_Button_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new PCA_ViewModel();
        }

        private void PSP_Button_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new PSP_ViewModel();
        }

        private void RoundRobinButton_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new RoundRobinViewModel();
        }

        private void MultiNivButton_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new MultiNiveauViewModel();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (ContentViewer.Content == null) return;  // no algo selected
            Ordonnancement.Ordonnancement prog = new PAPS();
            if (ContentViewer.Content.GetType() == typeof(PAPS_ViewModel))  // PAPS
            {
                if (PAPS_Tab.prog.listeProcessus.Count == 0)
                {
                    var bc = new BrushConverter();
                    StartButton.BorderBrush = (Brush)bc.ConvertFrom("#FFF52C2C");
                    StartButton.Background = (Brush)bc.ConvertFrom("#FFEEBEBE");
                }
                else
                {
                   
                    MainWindow.main.Content = new SimulationPage(PAPS_Tab.prog);
                }
            }
            else if (ContentViewer.Content.GetType() == typeof(PCA_ViewModel))  // PCA
            {
                if (PCA_Tab.prog.listeProcessus.Count == 0)
                {
                    var bc = new BrushConverter();
                    StartButton.BorderBrush = (Brush)bc.ConvertFrom("#FFF52C2C");
                    StartButton.Background = (Brush)bc.ConvertFrom("#FFEEBEBE");
                }
                else
                {
                    MainWindow.main.Content = new SimulationPage(PCA_Tab.prog);
                }
            }
            else if (ContentViewer.Content.GetType() == typeof(PSP_ViewModel))  // PSP
            {
                if (PSP_Tab.prog.listeProcessus.Count == 0)
                {
                    var bc = new BrushConverter();
                    StartButton.BorderBrush = (Brush)bc.ConvertFrom("#FFF52C2C");
                    StartButton.Background = (Brush)bc.ConvertFrom("#FFEEBEBE");
                }
                else
                {
                    MainWindow.main.Content = new SimulationPage(PSP_Tab.prog);
                }
            }
            else if (ContentViewer.Content.GetType() == typeof(RoundRobinViewModel))  // RoundRobin
            {
                if (RoundRobin_Tab.prog.listeProcessus.Count == 0 || RoundRobin_Tab.prog.quantum <= 0)
                {
                    var bc = new BrushConverter();
                    StartButton.BorderBrush = (Brush)bc.ConvertFrom("#FFF52C2C");
                    StartButton.Background = (Brush)bc.ConvertFrom("#FFEEBEBE");
                }
                else
                {
                    MainWindow.main.Content = new SimulationPage(RoundRobin_Tab.prog);
                }
            }
            /*else if (ContentViewer.Content.GetType() == typeof(MultiNiveauViewModel))  // MultiNiveaux
            {
                if (PAPS_Tab.prog.listeProcessus.Count == 0)
                {
                    var bc = new BrushConverter();
                    StartButton.BorderBrush = (Brush)bc.ConvertFrom("#FFF52C2C");
                    StartButton.Background = (Brush)bc.ConvertFrom("#FFEEBEBE");
                }
                else
                {
                    Main.Content = new SimulationPage(PAPS_Tab.prog);
                }
            }*/
        }
    }
}
