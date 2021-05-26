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

        private void PSP_Button_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new PSP_ViewModel();
        }

        private void PRIO_Button_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new PRIO_ViewModel();
        }

        private void RoundRobinButton_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new RoundRobinViewModel();
        }
        private void PSPDynamique_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new PSPDynamique_ViewModel();
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
            else if (ContentViewer.Content.GetType() == typeof(PSP_ViewModel))  // PSP
            {
                if (PSP_Tab.prog.listeProcessus.Count == 0)
                {
                    var bc = new BrushConverter();
                    //StartButton.BorderBrush = (Brush)bc.ConvertFrom("#FFF52C2C");
                    StartButton.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
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
                    //StartButton.BorderBrush = (Brush)bc.ConvertFrom("#FFF52C2C");
                    StartButton.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
                }
                else
                {
                    MainWindow.main.Content = new SimulationPage(RoundRobin_Tab.prog);
                }
            }
            else if (ContentViewer.Content.GetType() == typeof(PSPDynamique_ViewModel))  // PSP dynamique
            {
                if (PSPDynamique_Tab.prog.listeProcessus.Count == 0)
                {
                    var bc = new BrushConverter();
                    //StartButton.BorderBrush = (Brush)bc.ConvertFrom("#FFF52C2C");
                    StartButton.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
                }
                else
                {
                    MainWindow.main.Content = new SimulationPage(PSPDynamique_Tab.prog);
                }
            }
            else if (ContentViewer.Content.GetType() == typeof(PRIO_ViewModel))  // priorité sans réquisition
            {
                if (PRIO_Tab.prog.listeProcessus.Count == 0)
                {
                    var bc = new BrushConverter();
                    StartButton.Stroke = (Brush)bc.ConvertFrom("#FFF52C2C");
                    StartButton.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
                }
                else
                {
                    MainWindow.main.Content = new SimulationPage(PRIO_Tab.prog);
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
    }
}
