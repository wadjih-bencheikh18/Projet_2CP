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
    /// Interaction logic for Interruption_TabRow.xaml
    /// </summary>
    public partial class Interruption_TabRow : UserControl
    {
        public UserControl processus;

        public Interruption_TabRow(UserControl processus_parent)
        {
            InitializeComponent();
            processus = processus_parent;
        }

        private void supp_Button_Click(object sender, RoutedEventArgs e)
        {
            if (processus.GetType() == typeof(PAPS_TabRow))
            {
                if (PAPS_Tab.NbHint == 15) PAPS_Tab.HintSuivant();
                PAPS_Tab.prog.listeProcessus.Find(p => p.id == int.Parse(((PAPS_TabRow)processus).idTest.Text)).listeInterruptions.Remove((Interruption)this.DataContext);
                ((PAPS_TabRow)processus).parent.Items.Remove(this);
            }
            else if (processus.GetType() == typeof(PCA_TabRow))
            {
                PCA_Tab.prog.listeProcessus.Find(p => p.id == int.Parse(((PCA_TabRow)processus).idTest.Text)).listeInterruptions.Remove((Interruption)this.DataContext);
                ((PCA_TabRow)processus).parent.Items.Remove(this);
            }
            else if (processus.GetType() == typeof(SlackTime_TabRow))
            {
                SlackTime_Tab.prog.listeProcessus.Find(p => p.id == int.Parse(((SlackTime_TabRow)processus).idTest.Text)).listeInterruptions.Remove((Interruption)this.DataContext);
                ((SlackTime_TabRow)processus).parent.Items.Remove(this);
            }
            else if (processus.GetType() == typeof(PARD_TabRow))
            {
                PARD_Tab.prog.listeProcessus.Find(p => p.id == int.Parse(((PARD_TabRow)processus).idTest.Text)).listeInterruptions.Remove((Interruption)this.DataContext);
                ((PARD_TabRow)processus).parent.Items.Remove(this);
            }
            else if (processus.GetType() == typeof(PLA_TabRow))
            {
                PLA_Tab.prog.listeProcessus.Find(p => p.id == int.Parse(((PLA_TabRow)processus).idTest.Text)).listeInterruptions.Remove((Interruption)this.DataContext);
                ((PLA_TabRow)processus).parent.Items.Remove(this);
            }
            else if (processus.GetType() == typeof(PCTR_TabRow))
            {
                PCTR_Tab.prog.listeProcessus.Find(p => p.id == int.Parse(((PCTR_TabRow)processus).idTest.Text)).listeInterruptions.Remove((Interruption)this.DataContext);
                ((PCTR_TabRow)processus).parent.Items.Remove(this);
            }
            else if (processus.GetType() == typeof(PAR_TabRow))
            {
                PAR_Tab.prog.listeProcessus.Find(p => p.id == int.Parse(((PAR_TabRow)processus).idTest.Text)).listeInterruptions.Remove((Interruption)this.DataContext);
                ((PAR_TabRow)processus).parent.Items.Remove(this);
            }
            else if (processus.GetType() == typeof(PSR_TabRow))
            {
                PSR_Tab.prog.listeProcessus.Find(p => p.id == int.Parse(((PSR_TabRow)processus).idTest.Text)).listeInterruptions.Remove((Interruption)this.DataContext);
                ((PSR_TabRow)processus).parent.Items.Remove(this);
            }
            else if (processus.GetType() == typeof(RR_TabRow))
            {
                RR_Tab.prog.listeProcessus.Find(p => p.id == int.Parse(((RR_TabRow)processus).idTest.Text)).listeInterruptions.Remove((Interruption)this.DataContext);
                ((RR_TabRow)processus).parent.Items.Remove(this);
            }
            else if (processus.GetType() == typeof(Comp_TabRow))
            {
                Compar_Saisie.listeProc.Find(p => p.id == int.Parse(((Comp_TabRow)processus).idTest.Text)).listeInterruptions.Remove((Interruption)this.DataContext);
                ((Comp_TabRow)processus).parent.Items.Remove(this);
            }
            else if (processus.GetType() == typeof(Multi_Niv_TabRow_Proc))
            {
                Mult_Niv_Tab.ListPro.Find(p => p.id == int.Parse(((Multi_Niv_TabRow_Proc)processus).idTest.Text)).listeInterruptions.Remove((Interruption)this.DataContext);
                ((Multi_Niv_TabRow_Proc)processus).parent.Items.Remove(this);
            }
        }

        private void interruptionRow_Loaded(object sender, RoutedEventArgs e)
        {
            mainUC.Width = ((TreeViewItem)mainUC.Parent).ActualWidth - 150;
        }
    }
}
