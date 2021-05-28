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
            else if (processus.GetType() == typeof(PSPDynamique_TabRow))
            {
                PSPDynamique_Tab.prog.listeProcessus.Find(p => p.id == int.Parse(((PSPDynamique_TabRow)processus).idTest.Text)).listeInterruptions.Remove((Interruption)this.DataContext);
                ((PSPDynamique_TabRow)processus).parent.Items.Remove(this);
            }
            if (processus.GetType() == typeof(PLA_TabRow))
            {
                PLA_Tab.prog.listeProcessus.Find(p => p.id == int.Parse(((PLA_TabRow)processus).idTest.Text)).listeInterruptions.Remove((Interruption)this.DataContext);
                ((PLA_TabRow)processus).parent.Items.Remove(this);
            }
            if (processus.GetType() == typeof(PCTR_TabRow))
            {
                PCTR_Tab.prog.listeProcessus.Find(p => p.id == int.Parse(((PCTR_TabRow)processus).idTest.Text)).listeInterruptions.Remove((Interruption)this.DataContext);
                ((PCTR_TabRow)processus).parent.Items.Remove(this);
            }
            else if (processus.GetType() == typeof(PSP_TabRow))
            {
                PSP_Tab.prog.listeProcessus.Find(p => p.id == int.Parse(((PSP_TabRow)processus).idTest.Text)).listeInterruptions.Remove((Interruption)this.DataContext);
                ((PSP_TabRow)processus).parent.Items.Remove(this);
            }
            else if (processus.GetType() == typeof(PSP_TabRow))
            {
                PSP_Tab.prog.listeProcessus.Find(p => p.id == int.Parse(((PSP_TabRow)processus).idTest.Text)).listeInterruptions.Remove((Interruption)this.DataContext);
                ((PSP_TabRow)processus).parent.Items.Remove(this);
            }
            else if (processus.GetType() == typeof(PRIO_TabRow))
            {
                PRIO_Tab.prog.listeProcessus.Find(p => p.id == int.Parse(((PRIO_TabRow)processus).idTest.Text)).listeInterruptions.Remove((Interruption)this.DataContext);
                ((PRIO_TabRow)processus).parent.Items.Remove(this);
            }
            else if (processus.GetType() == typeof(RoundRobin_TabRow))
            {
                RoundRobin_Tab.prog.listeProcessus.Find(p => p.id == int.Parse(((RoundRobin_TabRow)processus).idTest.Text)).listeInterruptions.Remove((Interruption)this.DataContext);
                ((RoundRobin_TabRow)processus).parent.Items.Remove(this);
            }
            else
            {
                //Mult_Niv_Tab.prog.listeProcessus.Find(p => p.id == int.Parse(((Mult_Niv_TabRow)processus).idTest.Text)).listeInterruptions.Remove((Interruption)this.DataContext);
                //processus.parent.Items.Remove(this);
            }
        }
    }
}
