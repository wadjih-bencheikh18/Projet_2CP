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




namespace ResultTable
{
    class Processus
    {
        //donnés
        public int id { get; } //ID du processus
        public int tempsArriv { get; } //temps d'arrivé
        public int duree { get; } //temps d'execution du processus (burst time)
        public int prio { get; } //priorite du processus
        //à remplir
        public int etat; // 0:bloqué  1:prêt  2:en cours
        public int tempsFin;
        public int tempsAtt;
        public int tempsService;
        public int tempsRestant;
        public Processus(int id, int tempsArriv, int duree, int prio)  //constructeur pour l'algorithme de priorité
        {
            this.id = id;
            this.tempsArriv = tempsArriv;
            this.duree = duree;
            this.prio = prio;
            tempsRestant = duree;
        }
        public Processus(int id, int tempsArriv, int duree) //constructeur pour les autres algorithmes
        {
            this.id = id;
            this.tempsArriv = tempsArriv;
            this.duree = duree;
            prio = 0;
            tempsRestant = duree;
        }
        public virtual void Affichage() //affiche les caracteristiques d'un processus
        {
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.Write("ID : " + id);
            Console.Write("\ttemps d'arrivé : " + tempsArriv);
            Console.Write("\tduree : " + duree);
            Console.Write("\tpriorité : " + prio);
            Console.Write("\ttemps d'attente : " + tempsAtt);
            Console.Write("\ttemps de fin :  " + tempsFin);
            Console.Write("\ttemps de service  : " + tempsService);
            //Console.Write("\ttemps restant : " + tempsRestant);
        }
    }
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Processus Pro;
            int Nb = 3;
            Random r1, r2, r3,r4,r5,r6,r7;
            List<Processus> P = new List<Processus>();
            for (int i = 0; i < Nb; i++)
            {
                r1 = new Random();
                r2 = new Random();
                r3 = new Random();
                r4 = new Random();
                r5 = new Random();
                r6 = new Random();

                RowDefinition rowdef = new RowDefinition();
                rowdef.Height = new GridLength(30);
                Grid1.RowDefinitions.Insert(i,rowdef);
                
                DataGridCell cell = new DataGridCell();
                if (i % 2 == 0) { cell.Background = new SolidColorBrush(Colors.LightBlue); }
                Grid.SetColumnSpan(cell, 7);
                Grid.SetRow(cell, i+1);
                Grid.SetColumn(cell, 0);
                cell.BorderBrush = new SolidColorBrush(Colors.Black);
                Grid1.Children.Add(cell);


                TextBlock id = new TextBlock();
                id.Text = i.ToString();
                id.Name = "id" + id.Text.ToString();
                id.FontFamily = new FontFamily("Lexend");
                id.HorizontalAlignment = HorizontalAlignment.Center;
                id.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetRow(id, i + 1);
                Grid.SetColumn(id, 0);
                Grid1.Children.Add(id);

                TextBlock tAr = new TextBlock();
                tAr.Text = r1.Next(0,30).ToString() + "s";
                id.Name = "Tar" + tAr.Text.ToString();
                tAr.FontFamily = new FontFamily("Lexend");
                tAr.HorizontalAlignment = HorizontalAlignment.Center;
                tAr.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetRow(tAr, i + 1);
                Grid.SetColumn(tAr, 1);
                Grid1.Children.Add(tAr);

                TextBlock dur = new TextBlock();
                dur.Text = r2.Next(4, 80).ToString() + "s";
                dur.FontFamily = new FontFamily("Lexend");
                dur.HorizontalAlignment = HorizontalAlignment.Center;
                dur.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetRow(dur, i + 1);
                Grid.SetColumn(dur, 2);
                Grid1.Children.Add(dur);

                TextBlock prio = new TextBlock();
                prio.Text = r3.Next(2, 8).ToString(); 
                prio.FontFamily = new FontFamily("Lexend");
                prio.HorizontalAlignment = HorizontalAlignment.Center;
                prio.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetRow(prio, i + 1);
                Grid.SetColumn(prio, 3);
                Grid1.Children.Add(prio);

                TextBlock tAtt = new TextBlock();
                tAtt.Text = r4.Next(0, 50).ToString() + "s"; ;
                tAtt.FontFamily = new FontFamily("Lexend");
                tAtt.HorizontalAlignment = HorizontalAlignment.Center;
                tAtt.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetRow(tAtt, i + 1);
                Grid.SetColumn(tAtt, 4);
                Grid1.Children.Add(tAtt);

                TextBlock tFin = new TextBlock();
                tFin.Text = r5.Next(10, 200).ToString() + "s"; ;
                tFin.FontFamily = new FontFamily("Lexend");
                tFin.HorizontalAlignment = HorizontalAlignment.Center;
                tFin.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetRow(tFin, i + 1);
                Grid.SetColumn(tFin, 5);
                Grid1.Children.Add(tFin);

                TextBlock tSer = new TextBlock();
                tSer.Text = r6.Next(4, 60).ToString() + "s"; ;
                tSer.FontFamily = new FontFamily("Lexend");
                tSer.HorizontalAlignment = HorizontalAlignment.Center;
                tSer.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetRow(tSer, i + 1);
                Grid.SetColumn(tSer, 7);
                Grid1.Children.Add(tSer);

                

                Pro = new Processus(i, r1.Next(0, 15), r2.Next(1, 15), r3.Next(1, 5));
                P.Add(Pro);

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            


        }
    }
}
