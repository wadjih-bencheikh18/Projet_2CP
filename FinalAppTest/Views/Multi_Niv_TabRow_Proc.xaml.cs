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
    /// Interaction logic for Multi_Niv_TabRow_Proc.xaml
    /// </summary>
    public partial class Multi_Niv_TabRow_Proc : UserControl
    {
        public Multi_Niv_TabRow_Proc()
        {
            InitializeComponent();
        }
        private TextBox id;
        private TextBox tempsArriv;
        private TextBox duree;
        private TextBox prio;
        private TextBox niveau;
        private TextBlock Ajouter;
        private StackPanel Table;
        public Multi_Niv_TabRow_Proc(TextBox id, TextBox tempsArriv, TextBox duree, TextBox prio, TextBox niveau, StackPanel Table, TextBlock Ajouter)
        {
            InitializeComponent();
            this.id = id;
            this.tempsArriv = tempsArriv;
            this.duree = duree;
            this.Table = Table;
            this.prio = prio;
            this.niveau = niveau;
            this.Ajouter = Ajouter;
        }
        private void modifier_Button_Click(object sender, RoutedEventArgs e)
        {
            id.Text = idTest.Text;
            tempsArriv.Text = tempsArrTest.Text;
            duree.Text = dureeTest.Text;
            prio.Text = prioTest.Text;
            niveau.Text = niveauTest.Text;
            Mult_Niv_Tab.modifier = true;
            Mult_Niv_Tab.proModifier = this;
            Ajouter.Text = "Modifier";
        }

        private void suprimer_Button_Click(object sender, RoutedEventArgs e)
        {
            Mult_Niv_Tab.ListPro.RemoveAll(p => p.id.ToString().Equals(this.idTest.Text) && p.tempsArriv.ToString().Equals(this.tempsArrTest.Text));
            Table.Children.Remove(this);
        }
    }
}
