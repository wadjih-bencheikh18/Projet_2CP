using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Video2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public class Processus
        {
            public string id{ get; set; }
            public string duree { get; set; }
            public string tempsArriv { get; set; }
            public string Priorite { get; set; }
        }
        
        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            bool random = (bool) this.RandomizeCheckBox.IsChecked;
            int NbProcessus;
            bool correct = int.TryParse(this.ProcessusNumTextBox.Text, out NbProcessus);
            if (correct && NbProcessus > 0)
            {
                this.MainStackPanel.Children.RemoveRange(0, this.MainStackPanel.Children.Count - 1);  // vider la fenétre
            }
            else
            {
                if (correct)
                {
                    MessageBox.Show("Nombre des processus incorrect");
                }
                else
                {
                    MessageBox.Show("Input invalid");
                }
                this.ProcessusNumTextBox.Text = "";
                return;
            }
            Random r = new Random();
            for (int i = 0; i < NbProcessus; i++)
            {
                Processus pro = new Processus();
                pro.id = (i + 1).ToString();
                pro.duree = (random ? r.Next(1, 10) : 1).ToString();
                pro.tempsArriv = (random ? r.Next(1, 10) : 0).ToString();
                pro.Priorite = (random ? r.Next(1, 10) : 1).ToString();
                this.InitTab.Items.Add(pro);
            }
        }

        private void MyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.MyComboBox.SelectedItem.ToString().EndsWith("Round Robin"))
            {
                this.NumberEntry.Visibility = Visibility.Visible;
                this.MyTextBlock.Text = "Quantum:";
            }
            else if (this.MyComboBox.SelectedItem.ToString().EndsWith("Multi Niveaux"))
            {
                this.NumberEntry.Visibility = Visibility.Visible;
                this.MyTextBlock.Text = "Nombre des Niveaux:";
            }
            else if (this.NumberEntry != null)
            {
                this.NumberEntry.Visibility = Visibility.Hidden;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MyComboBox_SelectionChanged(this.MyComboBox, null);
        }
    }
}
