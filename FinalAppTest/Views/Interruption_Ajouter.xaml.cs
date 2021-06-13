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
using Ordonnancement;

namespace FinalAppTest.Views
{
    /// <summary>
    /// Interaction logic for Interruption_Ajouter.xaml
    /// </summary>
    public partial class Interruption_Ajouter : UserControl
    {
        public UserControl processus;

        public Interruption_Ajouter(UserControl processus_parent)
        {
            InitializeComponent();
            processus = processus_parent;
        }

        private void ajouter_Button_Click(object sender, RoutedEventArgs e)
        {
            if (processus.GetType() == typeof(PAPS_TabRow))
            {
                bool valide = true;
                var bc = new BrushConverter();
                if (!Int32.TryParse(tempsArrTest.Text, out int tempsArriv) || tempsArriv <= 0 || tempsArriv >= Int32.Parse(((PAPS_TabRow)processus).dureeTest.Text))
                {
                    valide = false;
                    TArrRect.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
                }
                if (!Int32.TryParse(dureeTest.Text, out int duree) || duree <= 0)
                {
                    valide = false;
                    DureeRect.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
                }
                if (valide)
                {
                    if (PAPS_Tab.NbHint == 14) PAPS_Tab.HintSuivant();
                    Interruption inter = new Interruption(interType.Text, duree, tempsArriv);
                    PAPS_Tab.prog.listeProcessus.Find(p => p.id == int.Parse(((PAPS_TabRow)processus).idTest.Text)).Push(inter);
                    Interruption_TabRow row = new Interruption_TabRow((PAPS_TabRow)processus)
                    {
                        DataContext = inter
                    };
                    ((PAPS_TabRow)processus).parent.Items.RemoveAt(((PAPS_TabRow)processus).parent.Items.Count - 1);  // remove the ajouter_row
                    ((PAPS_TabRow)processus).parent.Items.Add(row);
                    ((PAPS_TabRow)processus).parent.Items.Add(new Interruption_Ajouter(((PAPS_TabRow)processus)));  // append ajouter_row
                }
            }
            else if (processus.GetType() == typeof(PCA_TabRow))
            {
                bool valide = true;
                var bc = new BrushConverter();
                if (!Int32.TryParse(tempsArrTest.Text, out int tempsArriv) || tempsArriv <= 0 || tempsArriv >= Int32.Parse(((PCA_TabRow)processus).dureeTest.Text))
                {
                    valide = false;
                    TArrRect.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
                }
                if (!Int32.TryParse(dureeTest.Text, out int duree) || duree <= 0)
                {
                    valide = false;
                    DureeRect.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
                }
                if (valide)
                {
                    Interruption inter = new Interruption(interType.Text, duree, tempsArriv);
                    PCA_Tab.prog.listeProcessus.Find(p => p.id == int.Parse(((PCA_TabRow)processus).idTest.Text)).Push(inter);
                    Interruption_TabRow row = new Interruption_TabRow((PCA_TabRow)processus)
                    {
                        DataContext = inter
                    };
                    ((PCA_TabRow)processus).parent.Items.RemoveAt(((PCA_TabRow)processus).parent.Items.Count - 1);  // remove the ajouter_row
                    ((PCA_TabRow)processus).parent.Items.Add(row);
                    ((PCA_TabRow)processus).parent.Items.Add(new Interruption_Ajouter(((PCA_TabRow)processus)));  // append ajouter_row*/
                }
            }
            else if (processus.GetType() == typeof(SlackTime_TabRow))
            {
                bool valide = true;
                var bc = new BrushConverter();
                if (!Int32.TryParse(tempsArrTest.Text, out int tempsArriv) || tempsArriv <= 0 || tempsArriv >= Int32.Parse(((SlackTime_TabRow)processus).dureeTest.Text))
                {
                    valide = false;
                    TArrRect.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
                }
                if (!Int32.TryParse(dureeTest.Text, out int duree) || duree <= 0)
                {
                    valide = false;
                    DureeRect.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
                }
                if (valide)
                {
                    Interruption inter = new Interruption(interType.Text, duree, tempsArriv);
                    SlackTime_Tab.prog.listeProcessus.Find(p => p.id == int.Parse(((SlackTime_TabRow)processus).idTest.Text)).Push(inter);
                    Interruption_TabRow row = new Interruption_TabRow((SlackTime_TabRow)processus)
                    {
                        DataContext = inter
                    };
                    ((SlackTime_TabRow)processus).parent.Items.RemoveAt(((SlackTime_TabRow)processus).parent.Items.Count - 1);  // remove the ajouter_row
                    ((SlackTime_TabRow)processus).parent.Items.Add(row);
                    ((SlackTime_TabRow)processus).parent.Items.Add(new Interruption_Ajouter(((SlackTime_TabRow)processus)));  // append ajouter_row*/
                }
            }
            else if (processus.GetType() == typeof(PARD_TabRow))
            {
                bool valide = true;
                var bc = new BrushConverter();
                if (!Int32.TryParse(tempsArrTest.Text, out int tempsArriv) || tempsArriv <= 0 || tempsArriv >= Int32.Parse(((PARD_TabRow)processus).dureeTest.Text))
                {
                    valide = false;
                    TArrRect.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
                }
                if (!Int32.TryParse(dureeTest.Text, out int duree) || duree <= 0)
                {
                    valide = false;
                    DureeRect.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
                }
                if (valide)
                {
                    if (PARD_Tab.NbHint == 15) PARD_Tab.HintSuivant();
                    Interruption inter = new Interruption(interType.Text, duree, tempsArriv);
                    PARD_Tab.prog.listeProcessus.Find(p => p.id == int.Parse(((PARD_TabRow)processus).idTest.Text)).Push(inter);
                    Interruption_TabRow row = new Interruption_TabRow((PARD_TabRow)processus)
                    {
                        DataContext = inter
                    };
                    ((PARD_TabRow)processus).parent.Items.RemoveAt(((PARD_TabRow)processus).parent.Items.Count - 1);  // remove the ajouter_row
                    ((PARD_TabRow)processus).parent.Items.Add(row);
                    ((PARD_TabRow)processus).parent.Items.Add(new Interruption_Ajouter(((PARD_TabRow)processus)));  // append ajouter_row*/
                }
            }
            else if (processus.GetType() == typeof(PLA_TabRow))
            {
                bool valide = true;
                var bc = new BrushConverter();
                if (!Int32.TryParse(tempsArrTest.Text, out int tempsArriv) || tempsArriv <= 0 || tempsArriv >= Int32.Parse(((PLA_TabRow)processus).dureeTest.Text))
                {
                    valide = false;
                    TArrRect.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
                }
                if (!Int32.TryParse(dureeTest.Text, out int duree) || duree <= 0)
                {
                    valide = false;
                    DureeRect.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
                }
                if (valide)
                {
                    Interruption inter = new Interruption(interType.Text, duree, tempsArriv);
                    PLA_Tab.prog.listeProcessus.Find(p => p.id == int.Parse(((PLA_TabRow)processus).idTest.Text)).Push(inter);
                    Interruption_TabRow row = new Interruption_TabRow((PLA_TabRow)processus)
                    {
                        DataContext = inter
                    };
                    ((PLA_TabRow)processus).parent.Items.RemoveAt(((PLA_TabRow)processus).parent.Items.Count - 1);  // remove the ajouter_row
                    ((PLA_TabRow)processus).parent.Items.Add(row);
                    ((PLA_TabRow)processus).parent.Items.Add(new Interruption_Ajouter(((PLA_TabRow)processus)));  // append ajouter_row
                }
            }
            else if (processus.GetType() == typeof(PCTR_TabRow))
            {
                bool valide = true;
                var bc = new BrushConverter();
                if (!Int32.TryParse(tempsArrTest.Text, out int tempsArriv) || tempsArriv <= 0 || tempsArriv >= Int32.Parse(((PCTR_TabRow)processus).dureeTest.Text))
                {
                    valide = false;
                    TArrRect.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
                }
                if (!Int32.TryParse(dureeTest.Text, out int duree) || duree <= 0)
                {
                    valide = false;
                    DureeRect.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
                }
                if (valide)
                {
                    Interruption inter = new Interruption(interType.Text, duree, tempsArriv);
                    PCTR_Tab.prog.listeProcessus.Find(p => p.id == int.Parse(((PCTR_TabRow)processus).idTest.Text)).Push(inter);
                    Interruption_TabRow row = new Interruption_TabRow((PCTR_TabRow)processus)
                    {
                        DataContext = inter
                    };
                    ((PCTR_TabRow)processus).parent.Items.RemoveAt(((PCTR_TabRow)processus).parent.Items.Count - 1);  // remove the ajouter_row
                    ((PCTR_TabRow)processus).parent.Items.Add(row);
                    ((PCTR_TabRow)processus).parent.Items.Add(new Interruption_Ajouter(((PCTR_TabRow)processus)));  // append ajouter_row
                }
            }
            else if (processus.GetType() == typeof(PAR_TabRow))
            {
                bool valide = true;
                var bc = new BrushConverter();
                if (!Int32.TryParse(tempsArrTest.Text, out int tempsArriv) || tempsArriv <= 0 || tempsArriv >= Int32.Parse(((PAR_TabRow)processus).dureeTest.Text))
                {
                    valide = false;
                    TArrRect.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
                }
                if (!Int32.TryParse(dureeTest.Text, out int duree) || duree <= 0)
                {
                    valide = false;
                    DureeRect.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
                }
                if (valide)
                {
                    Interruption inter = new Interruption(interType.Text, duree, tempsArriv);
                    PAR_Tab.prog.listeProcessus.Find(p => p.id == int.Parse(((PAR_TabRow)processus).idTest.Text)).Push(inter);
                    Interruption_TabRow row = new Interruption_TabRow((PAR_TabRow)processus)
                    {
                        DataContext = inter
                    };
                    ((PAR_TabRow)processus).parent.Items.RemoveAt(((PAR_TabRow)processus).parent.Items.Count - 1);  // remove the ajouter_row
                    ((PAR_TabRow)processus).parent.Items.Add(row);
                    ((PAR_TabRow)processus).parent.Items.Add(new Interruption_Ajouter(((PAR_TabRow)processus)));  // append ajouter_row
                }
            }
            else if (processus.GetType() == typeof(PSR_TabRow))
            {
                bool valide = true;
                var bc = new BrushConverter();
                if (!Int32.TryParse(tempsArrTest.Text, out int tempsArriv) || tempsArriv <= 0 || tempsArriv >= Int32.Parse(((PSR_TabRow)processus).dureeTest.Text))
                {
                    valide = false;
                    TArrRect.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
                }
                if (!Int32.TryParse(dureeTest.Text, out int duree) || duree <= 0)
                {
                    valide = false;
                    DureeRect.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
                }
                if (valide)
                {
                    Interruption inter = new Interruption(interType.Text, duree, tempsArriv);
                    PSR_Tab.prog.listeProcessus.Find(p => p.id == int.Parse(((PSR_TabRow)processus).idTest.Text)).Push(inter);
                    Interruption_TabRow row = new Interruption_TabRow((PSR_TabRow)processus)
                    {
                        DataContext = inter
                    };
                    ((PSR_TabRow)processus).parent.Items.RemoveAt(((PSR_TabRow)processus).parent.Items.Count - 1);  // remove the ajouter_row
                    ((PSR_TabRow)processus).parent.Items.Add(row);
                    ((PSR_TabRow)processus).parent.Items.Add(new Interruption_Ajouter(((PSR_TabRow)processus)));  // append ajouter_row
                }
            }
            else if (processus.GetType() == typeof(RR_TabRow))
            {
                bool valide = true;
                var bc = new BrushConverter();
                if (!Int32.TryParse(tempsArrTest.Text, out int tempsArriv) || tempsArriv <= 0 || tempsArriv >= Int32.Parse(((RR_TabRow)processus).dureeTest.Text))
                {
                    valide = false;
                    TArrRect.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
                }
                if (!Int32.TryParse(dureeTest.Text, out int duree) || duree <= 0)
                {
                    valide = false;
                    DureeRect.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
                }
                if (valide)
                {
                    if (RR_Tab.NbHint == 15) RR_Tab.HintSuivant();
                    Interruption inter = new Interruption(interType.Text, duree, tempsArriv);
                    RR_Tab.prog.listeProcessus.Find(p => p.id == int.Parse(((RR_TabRow)processus).idTest.Text)).Push(inter);
                    Interruption_TabRow row = new Interruption_TabRow((RR_TabRow)processus)
                    {
                        DataContext = inter
                    };
                    ((RR_TabRow)processus).parent.Items.RemoveAt(((RR_TabRow)processus).parent.Items.Count - 1);  // remove the ajouter_row
                    ((RR_TabRow)processus).parent.Items.Add(row);
                    ((RR_TabRow)processus).parent.Items.Add(new Interruption_Ajouter(((RR_TabRow)processus)));  // append ajouter_row
                }
            }
            else if (processus.GetType() == typeof(Comp_TabRow))
            {
                bool valide = true;
                var bc = new BrushConverter();
                if (!Int32.TryParse(tempsArrTest.Text, out int tempsArriv) || tempsArriv <= 0 || tempsArriv >= Int32.Parse(((Comp_TabRow)processus).dureeTest.Text))
                {
                    valide = false;
                    TArrRect.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
                }
                if (!Int32.TryParse(dureeTest.Text, out int duree) || duree <= 0)
                {
                    valide = false;
                    DureeRect.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
                }
                if (valide)
                {
                    Interruption inter = new Interruption(interType.Text, duree, tempsArriv);
                    Compar_Saisie.listeProc.Find(p => p.id == int.Parse(((Comp_TabRow)processus).idTest.Text)).Push(inter);
                    Interruption_TabRow row = new Interruption_TabRow((Comp_TabRow)processus)
                    {
                        DataContext = inter
                    };
                    ((Comp_TabRow)processus).parent.Items.RemoveAt(((Comp_TabRow)processus).parent.Items.Count - 1);  // remove the ajouter_row
                    ((Comp_TabRow)processus).parent.Items.Add(row);
                    ((Comp_TabRow)processus).parent.Items.Add(new Interruption_Ajouter(((Comp_TabRow)processus)));  // append ajouter_row*/
                }
            }
            else if (processus.GetType() == typeof(Multi_Niv_TabRow_Proc))
            {
                bool valide = true;
                var bc = new BrushConverter();
                if (!Int32.TryParse(tempsArrTest.Text, out int tempsArriv) || tempsArriv <= 0 || tempsArriv >= Int32.Parse(((Multi_Niv_TabRow_Proc)processus).dureeTest.Text))
                {
                    valide = false;
                    TArrRect.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
                }
                if (!Int32.TryParse(dureeTest.Text, out int duree) || duree <= 0)
                {
                    valide = false;
                    DureeRect.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
                }
                if (valide)
                {
                    if (Mult_Niv_Tab.NbHint == 23) Mult_Niv_Tab.HintSuivant();
                    Interruption inter = new Interruption(interType.Text, duree, tempsArriv);
                    Mult_Niv_Tab.ListPro.Find(p => p.id == int.Parse(((Multi_Niv_TabRow_Proc)processus).idTest.Text)).Push(inter);
                    Interruption_TabRow row = new Interruption_TabRow((Multi_Niv_TabRow_Proc)processus)
                    {
                        DataContext = inter
                    };
                    ((Multi_Niv_TabRow_Proc)processus).parent.Items.RemoveAt(((Multi_Niv_TabRow_Proc)processus).parent.Items.Count - 1);  // remove the ajouter_row
                    ((Multi_Niv_TabRow_Proc)processus).parent.Items.Add(row);
                    ((Multi_Niv_TabRow_Proc)processus).parent.Items.Add(new Interruption_Ajouter(((Multi_Niv_TabRow_Proc)processus)));  // append ajouter_row
                }
            }
            else if (processus.GetType() == typeof(Multi_Niv_R_TabRow_Proc))
            {
                bool valide = true;
                var bc = new BrushConverter();
                if (!Int32.TryParse(tempsArrTest.Text, out int tempsArriv) || tempsArriv <= 0 || tempsArriv >= Int32.Parse(((Multi_Niv_R_TabRow_Proc)processus).dureeTest.Text))
                {
                    valide = false;
                    tempsArrTest.Background = (Brush)bc.ConvertFrom("#FFEEBEBE");
                }
                if (!Int32.TryParse(dureeTest.Text, out int duree) || duree <= 0)
                {
                    valide = false;
                    dureeTest.Background = (Brush)bc.ConvertFrom("#FFEEBEBE");
                }
                if (valide)
                {
                    Interruption inter = new Interruption(interType.Text, duree, tempsArriv);
                    Mult_Niv_Recyclage_Tab.ListPro.Find(p => p.id == int.Parse(((Multi_Niv_R_TabRow_Proc)processus).idTest.Text)).Push(inter);
                    Interruption_TabRow row = new Interruption_TabRow((Multi_Niv_R_TabRow_Proc)processus)
                    {
                        DataContext = inter
                    };
                    ((Multi_Niv_R_TabRow_Proc)processus).parent.Items.RemoveAt(((Multi_Niv_R_TabRow_Proc)processus).parent.Items.Count - 1);  // remove the ajouter_row
                    ((Multi_Niv_R_TabRow_Proc)processus).parent.Items.Add(row);
                    ((Multi_Niv_R_TabRow_Proc)processus).parent.Items.Add(new Interruption_Ajouter(((Multi_Niv_R_TabRow_Proc)processus)));  // append ajouter_row
                }
            }
        }

        private void interruptionRow_Loaded(object sender, RoutedEventArgs e)
        {
            mainUC.Width = ((TreeViewItem)mainUC.Parent).ActualWidth-150;
        }
    }
}
