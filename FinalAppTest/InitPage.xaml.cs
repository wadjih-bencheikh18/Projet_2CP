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

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (ContentViewer.Content == null) return;  // no algo selected
            Ordonnancement.Ordonnancement prog = new PAPS();
            if (ContentViewer.Content.GetType() == typeof(PAPS_ViewModel))  // PAPS
            {
                if (PAPS_Tab.prog.listeProcessus.Count == 0)
                {
                    var bc = new BrushConverter();
                    //StartButton.BorderBrush = (Brush)bc.ConvertFrom("#FFF52C2C");
                    StartButton.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
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
                    //StartButton. = (Brush)bc.ConvertFrom("#FFF52C2C");
                    StartButton.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
                }
                else
                {
                    MainWindow.main.Content = new SimulationPage(PCA_Tab.prog);
                }
            }
            else if (ContentViewer.Content.GetType() == typeof(PLA_ViewModel))  // PLA
            {
                if (PLA_Tab.prog.listeProcessus.Count == 0)
                {
                    var bc = new BrushConverter();
                    StartButton.Stroke = (Brush)bc.ConvertFrom("#FFF52C2C");
                    StartButton.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
                }
                else
                {
                    MainWindow.main.Content = new SimulationPage(PLA_Tab.prog);
                }
            }
            else if (ContentViewer.Content.GetType() == typeof(PCTR_ViewModel))  // PCTR
            {
                if (PCTR_Tab.prog.listeProcessus.Count == 0)
                {
                    var bc = new BrushConverter();
                    StartButton.Stroke = (Brush)bc.ConvertFrom("#FFF52C2C");
                    StartButton.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
                }
                else
                {
                    MainWindow.main.Content = new SimulationPage(PCTR_Tab.prog);
                }
            }
            else if (ContentViewer.Content.GetType() == typeof(PAR_ViewModel))  // PAR
            {
                if (PAR_Tab.prog.listeProcessus.Count == 0)
                {
                    var bc = new BrushConverter();
                    //StartButton.BorderBrush = (Brush)bc.ConvertFrom("#FFF52C2C");
                    StartButton.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
                }
                else
                {
                    MainWindow.main.Content = new SimulationPage(PAR_Tab.prog);
                }
            }
            else if (ContentViewer.Content.GetType() == typeof(RR_ViewModel))  // RoundRobin
            {
                if (RR_Tab.prog.listeProcessus.Count == 0 || RR_Tab.prog.quantum <= 0)
                {
                    var bc = new BrushConverter();
                    //StartButton.BorderBrush = (Brush)bc.ConvertFrom("#FFF52C2C");
                    StartButton.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
                }
                else
                {
                    MainWindow.main.Content = new SimulationPage(RR_Tab.prog);
                }
            }
            else if (ContentViewer.Content.GetType() == typeof(SlackTime_ViewModel))  // PAR dynamique
            {
                if (SlackTime_Tab.prog.listeProcessus.Count == 0)
                {
                    var bc = new BrushConverter();
                    //StartButton.BorderBrush = (Brush)bc.ConvertFrom("#FFF52C2C");
                    StartButton.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
                }
                else
                {
                    MainWindow.main.Content = new SimulationPage(SlackTime_Tab.prog);
                }
            }
            else if (ContentViewer.Content.GetType() == typeof(PARD_ViewModel))  // PAR dynamique
            {
                if (PARD_Tab.prog.listeProcessus.Count == 0)
                {
                    var bc = new BrushConverter();
                    //StartButton.BorderBrush = (Brush)bc.ConvertFrom("#FFF52C2C");
                    StartButton.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
                }
                else
                {
                    MainWindow.main.Content = new SimulationPage(PARD_Tab.prog);
                }
            }
            else if (ContentViewer.Content.GetType() == typeof(PSR_ViewModel))  // priorité sans réquisition
            {
                if (PSR_Tab.prog.listeProcessus.Count == 0)
                {
                    var bc = new BrushConverter();
                    StartButton.Stroke = (Brush)bc.ConvertFrom("#FFF52C2C");
                    StartButton.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
                }
                else
                {
                    MainWindow.main.Content = new SimulationPage(PSR_Tab.prog);
                }
            }
            else if (ContentViewer.Content.GetType() == typeof(MultiNiveauViewModel))  // MultiNiveaux
            {
                if (Mult_Niv_Tab.ListPro.Count==0 || Mult_Niv_Tab.indiceniv==0)
                {
                    var bc = new BrushConverter();
                    //StartButton.BorderBrush = (Brush)bc.ConvertFrom("#FFF52C2C");
                    StartButton.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
                }
                else
                {
                    Mult_Niv_Tab.prog = new MultiNiveau(Mult_Niv_Tab.indiceniv, Mult_Niv_Tab.niveaux);
                    foreach (ProcessusNiveau pro in Mult_Niv_Tab.ListPro)
                    {
                        Mult_Niv_Tab.prog.Push(pro);
                    }
                    MainWindow.main.Content = new SimulationPage_MultiLvl(Mult_Niv_Tab.prog,0);
                }
              
            }
            else if (ContentViewer.Content.GetType() == typeof(MultiNiveauRecyclageViewModel))  // MultiNiveaux
            {
                if (Mult_Niv_Recyclage_Tab.ListPro.Count==0 || Mult_Niv_Recyclage_Tab.indiceniv==0)
                {
                    var bc = new BrushConverter();
                    //StartButton.BorderBrush = (Brush)bc.ConvertFrom("#FFF52C2C");
                    StartButton.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
                }
                else
                {
                    Mult_Niv_Recyclage_Tab.prog = new MultiNiveauRecyclage(Mult_Niv_Recyclage_Tab.indiceniv, Mult_Niv_Recyclage_Tab.niveaux);
                    foreach (ProcessusNiveau pro in Mult_Niv_Recyclage_Tab.ListPro)
                    {
                        Mult_Niv_Recyclage_Tab.prog.Push(pro);
                    }
                    MainWindow.main.Content = new SimulationPage_MultiLvl(Mult_Niv_Recyclage_Tab.prog,1);
                }
              
            }
        }

        private void ReturnButton_Click(object sender, MouseButtonEventArgs e)
        {
            MainWindow.main.Content = new WelcomePage();
        }
    }
}
