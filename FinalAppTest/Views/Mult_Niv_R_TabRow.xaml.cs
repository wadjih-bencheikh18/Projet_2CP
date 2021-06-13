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
    /// Interaction logic for Mult_Niv_TabRow.xaml
    /// </summary>
    public partial class Mult_Niv_R_TabRow : UserControl
    {
        public Mult_Niv_R_TabRow()
        {
            InitializeComponent();
        }

        private TextBox nivId;
        private ComboBox algoSelect;
        private TextBox quantum;
        private TextBlock Ajouter;
        private StackPanel Table;

        public Mult_Niv_R_TabRow(TextBox nivId, ComboBox algoSelect, TextBox quantum, StackPanel Table, TextBlock Ajouter)
        {
            InitializeComponent();
            this.nivId = nivId;
            this.algoSelect = algoSelect;
            this.quantum = quantum;
            this.Table = Table;
            this.Ajouter = Ajouter;
        }
        private void modifier_Button_Click(object sender, RoutedEventArgs e)
        {
            //if (Mult_Niv_Recyclage_Tab.NbHint == 7) Mult_Niv_Recyclage_Tab.HintSuivant();
            nivId.Text = nivtest.Text;
            algoSelect.Text = type.Text;
            quantum.Text = quantumTest.Text;
            Mult_Niv_Recyclage_Tab.modifier = true;
            Mult_Niv_Recyclage_Tab.proModifier = this;
            if (Mult_Niv_Recyclage_Tab.ThisPage != null) Mult_Niv_Recyclage_Tab.ThisPage.ajouterButton.Visibility = Visibility.Visible;
            Ajouter.Text = "Modifier";
            if (Mult_Niv_Recyclage_Tab.NbHint == 7) Mult_Niv_Recyclage_Tab.HintSuivant();
        }
    }
}
