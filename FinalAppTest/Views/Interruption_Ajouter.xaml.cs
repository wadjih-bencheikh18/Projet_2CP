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
            if (processus.GetType() == typeof(PLA_TabRow))
            {
                bool valide = true;
                var bc = new BrushConverter();
                if (!Int32.TryParse(tempsArrTest.Text, out int tempsArriv) || tempsArriv <= 0 || tempsArriv >= Int32.Parse(((PLA_TabRow)processus).dureeTest.Text))
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
            if (processus.GetType() == typeof(PCTR_TabRow))
            {
                bool valide = true;
                var bc = new BrushConverter();
                if (!Int32.TryParse(tempsArrTest.Text, out int tempsArriv) || tempsArriv <= 0 || tempsArriv >= Int32.Parse(((PCTR_TabRow)processus).dureeTest.Text))
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
            else if (processus.GetType() == typeof(PSP_TabRow))
            {
                bool valide = true;
                var bc = new BrushConverter();
                if (!Int32.TryParse(tempsArrTest.Text, out int tempsArriv) || tempsArriv <= 0 || tempsArriv >= Int32.Parse(((PSP_TabRow)processus).dureeTest.Text))
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
                    PSP_Tab.prog.listeProcessus.Find(p => p.id == int.Parse(((PSP_TabRow)processus).idTest.Text)).Push(inter);
                    Interruption_TabRow row = new Interruption_TabRow((PSP_TabRow)processus)
                    {
                        DataContext = inter
                    };
                    ((PSP_TabRow)processus).parent.Items.RemoveAt(((PSP_TabRow)processus).parent.Items.Count - 1);  // remove the ajouter_row
                    ((PSP_TabRow)processus).parent.Items.Add(row);
                    ((PSP_TabRow)processus).parent.Items.Add(new Interruption_Ajouter(((PSP_TabRow)processus)));  // append ajouter_row
                }
            }
            if (processus.GetType() == typeof(PSP_TabRow))
            {
                bool valide = true;
                var bc = new BrushConverter();
                if (!Int32.TryParse(tempsArrTest.Text, out int tempsArriv) || tempsArriv <= 0 || tempsArriv >= Int32.Parse(((PSP_TabRow)processus).dureeTest.Text))
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
                    PSP_Tab.prog.listeProcessus.Find(p => p.id == int.Parse(((PSP_TabRow)processus).idTest.Text)).Push(inter);
                    Interruption_TabRow row = new Interruption_TabRow((PSP_TabRow)processus)
                    {
                        DataContext = inter
                    };
                    ((PSP_TabRow)processus).parent.Items.RemoveAt(((PSP_TabRow)processus).parent.Items.Count - 1);  // remove the ajouter_row
                    ((PSP_TabRow)processus).parent.Items.Add(row);
                    ((PSP_TabRow)processus).parent.Items.Add(new Interruption_Ajouter(((PSP_TabRow)processus)));  // append ajouter_row
                }
            }
            if (processus.GetType() == typeof(PRIO_TabRow))
            {
                bool valide = true;
                var bc = new BrushConverter();
                if (!Int32.TryParse(tempsArrTest.Text, out int tempsArriv) || tempsArriv <= 0 || tempsArriv >= Int32.Parse(((PRIO_TabRow)processus).dureeTest.Text))
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
                    PRIO_Tab.prog.listeProcessus.Find(p => p.id == int.Parse(((PRIO_TabRow)processus).idTest.Text)).Push(inter);
                    Interruption_TabRow row = new Interruption_TabRow((PRIO_TabRow)processus)
                    {
                        DataContext = inter
                    };
                    ((PRIO_TabRow)processus).parent.Items.RemoveAt(((PRIO_TabRow)processus).parent.Items.Count - 1);  // remove the ajouter_row
                    ((PRIO_TabRow)processus).parent.Items.Add(row);
                    ((PRIO_TabRow)processus).parent.Items.Add(new Interruption_Ajouter(((PRIO_TabRow)processus)));  // append ajouter_row
                }
            }
            else if (processus.GetType() == typeof(RoundRobin_TabRow))
            {
                bool valide = true;
                var bc = new BrushConverter();
                if (!Int32.TryParse(tempsArrTest.Text, out int tempsArriv) || tempsArriv <= 0 || tempsArriv >= Int32.Parse(((RoundRobin_TabRow)processus).dureeTest.Text))
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
                    RoundRobin_Tab.prog.listeProcessus.Find(p => p.id == int.Parse(((RoundRobin_TabRow)processus).idTest.Text)).Push(inter);
                    Interruption_TabRow row = new Interruption_TabRow((RoundRobin_TabRow)processus)
                    {
                        DataContext = inter
                    };
                    ((RoundRobin_TabRow)processus).parent.Items.RemoveAt(((RoundRobin_TabRow)processus).parent.Items.Count - 1);  // remove the ajouter_row
                    ((RoundRobin_TabRow)processus).parent.Items.Add(row);
                    ((RoundRobin_TabRow)processus).parent.Items.Add(new Interruption_Ajouter(((RoundRobin_TabRow)processus)));  // append ajouter_row
                }
            }
            else  // Mult_Niv_TabRow
            {

            }
        }
    }
}
