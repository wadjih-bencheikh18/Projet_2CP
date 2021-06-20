using Ordonnancement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using WpfCharts;

namespace FinalAppTest.Views
{
    /// <summary>
    /// Logique d'interaction pour Comparison_Page.xaml
    /// </summary>
    public partial class Comparison_Page : Page
    {
        private Ordonnancement.Ordonnancement prog1, prog2, prog3;
        private readonly Random random = new Random(1234);
        private List<Grid> mainGrids, algo3Grids;
        private List<TextBlock> algo1, algo2, algo3;
        private List<Rectangle> algo1Rects, algo2Rects, algo3Rects;
        private List<Brush> rectColors;
        private int nbPros = 2;
        private int pageIndex = 0;
        List<int> Algos;

        int quantum;
        int tempsMAJ;
        /* List<int> caca = new List<int>()
         {
             Algo1Tser,
         };*/
        public Comparison_Page()
        {
            InitializeComponent();
        }

        public Comparison_Page(List<int> Algos, int quantum, int tempsMAJ)
        {
            InitializeComponent();
            this.Algos = Algos;
            
            this.quantum = quantum;
            this.tempsMAJ = tempsMAJ;
            nbPros = Algos.Count;
            prog1 = CreateProg(Algos[0]);
            prog2 = CreateProg(Algos[1]);
            if (nbPros > 2) prog3 = CreateProg(Algos[2]);

            foreach (Processus processus in Compar_Saisie.listeProc)
            {
                prog1.Push(new Processus(processus));
                prog2.Push(new Processus(processus));
                if (nbPros > 2) prog3.Push(new Processus(processus));
            }
            prog1.Executer();
            prog1.CalculeResultats();
            prog1.Famine(60);
            prog2.Executer();
            prog2.CalculeResultats();
            prog2.Famine(40);
            if (nbPros > 2)
            {
                prog3.Executer();
                prog3.CalculeResultats();
                prog3.Famine(60);
            }
            
            Lines = new ObservableCollection<ChartLine>();
        }
        private double Crit(Ordonnancement.Ordonnancement prog, int i)
        {
            switch (i)
            {
                case 0: return prog.tempsAtt;
                case 1: return prog.pourcentageRepos;
                case 2: return prog.tempsService;
                case 3: return prog.tempsReponse;
                case 4: return prog.tempsFin;
                case 5: return prog.nbFamine;
            }
            return 0;
        }
        private void ApplyColor(int i)
        {
            if (nbPros > 2)
            {
                if (Crit(prog1,i) > Crit(prog2, i) && Crit(prog2, i) > Crit(prog3, i))
                {
                    algo1Rects[i].Fill = rectColors[2];
                    algo2Rects[i].Fill = rectColors[1];
                    algo3Rects[i].Fill = rectColors[0];
                }
                else if (Crit(prog1, i) > Crit(prog3, i) && Crit(prog3, i) > Crit(prog2, i))
                {
                    algo1Rects[i].Fill = rectColors[2];
                    algo2Rects[i].Fill = rectColors[0];
                    algo3Rects[i].Fill = rectColors[1];
                }
                else if (Crit(prog2, i) > Crit(prog1, i) && Crit(prog1, i) > prog3.tempsAtt)
                {
                    algo1Rects[i].Fill = rectColors[1];
                    algo2Rects[i].Fill = rectColors[2];
                    algo3Rects[i].Fill = rectColors[0];
                }
                else if (Crit(prog2, i) > Crit(prog3, i)  && Crit(prog3, i) > Crit(prog1, i))
                {
                    algo1Rects[i].Fill = rectColors[0];
                    algo2Rects[i].Fill = rectColors[2];
                    algo3Rects[i].Fill = rectColors[1];
                }
                else if (Crit(prog3, i) > Crit(prog1, i) && Crit(prog1, i) > Crit(prog2, i))
                {
                    algo1Rects[i].Fill = rectColors[1];
                    algo2Rects[i].Fill = rectColors[0];
                    algo3Rects[i].Fill = rectColors[2];
                }
                else
                {
                    algo1Rects[i].Fill = rectColors[0];
                    algo2Rects[i].Fill = rectColors[1];
                    algo3Rects[i].Fill = rectColors[2];
                }
            }
            else
            {
                if (Crit(prog1, i) > Crit(prog2, i))
                {
                    algo1Rects[i].Fill = rectColors[2];
                    algo2Rects[i].Fill = rectColors[0];
                }
                else
                {
                    algo1Rects[i].Fill = rectColors[0];
                    algo2Rects[i].Fill = rectColors[2];
                }
            }
        }
        private void ColorFix()
        {
            for (int i = 0; i < 6; i++)
                ApplyColor(i);
        }
        private double CalculateMax()
        {
            double max = 0;
            if (CalculateMaxProg(prog1)>max) max = CalculateMaxProg(prog1);
            if (CalculateMaxProg(prog2)>max) max = CalculateMaxProg(prog2);
            if(nbPros == 3) if (CalculateMaxProg(prog3) > max) max = CalculateMaxProg(prog3);
            return max;
        }
        private double CalculateMaxProg(Ordonnancement.Ordonnancement prog)
        {
            double max = 0;
            if (prog.tempsAtt > max) max = prog.tempsAtt;
            if (prog.tempsService > max) max = prog.tempsService;
            if (prog.tempsRepos > max) max = prog.tempsRepos;
            if (prog.tempsReponse > max) max = prog.tempsReponse;
            if (prog.tempsFin > max) max = prog.tempsFin;
            return max;
        }
        private void FillAlgo(Ordonnancement.Ordonnancement prog, int Algo, List<TextBlock> Texts,Color color)
        {
            prog.CalculeResultats();
            prog.Famine(60);
            Texts[0].Text = NameProg(Algo);
            Texts[1].Text = DescProg(Algo);
            Texts[2].Text = prog.tempsAtt.ToString();
            Texts[3].Text = (prog.pourcentageRepos*100).ToString()+"%";
            Texts[4].Text = prog.tempsService.ToString();
            Texts[5].Text = prog.tempsReponse.ToString();
            Texts[6].Text = prog.tempsFin.ToString();
            Texts[7].Text = prog.nbFamine.ToString();
            Texts[8].Text = NameProg(Algo);
            Color FillColor = color;
            FillColor.A = 50;
            var pts = new List<double>();
            pts.Add(prog.tempsAtt);
            pts.Add(prog.tempsService);
            pts.Add(prog.tempsReponse);
            pts.Add(prog.tempsFin);
            pts.Add(prog.tempsRepos);
            ChartLine line = new ChartLine
                {
                    LineColor = color,
                    FillColor = FillColor,
                    LineThickness = 2,
                    PointDataSource = pts,
                    Name = "Chart " + (Lines.Count + 1)
                };
            Lines.Add(line);
        }
        private Ordonnancement.Ordonnancement CreateProg(int Algo)
        {
            switch (Algo)
            {
                case 0:
                    return new PAPS();
                case 1:
                    return new PCA();
                case 2:
                    return new PLA();
                case 3:
                    return new PCTR();
                case 4:
                    return new PAR();
                case 5:
                    return new PSR();
                case 6:
                    return new SlackTime();
                case 7:
                    return new RR(quantum);
                case 8:
                    return new PARD(quantum);
                default:
                    return new PAPS();
            }
        }
        private string NameProg(int Algo)
        {
            switch (Algo)
            {
                case 0:
                    return "PAPS";
                case 1:
                    return "PCA";
                case 2:
                    return "PLA";
                case 3:
                    return "PCTR";
                case 4:
                    return "PAR";
                case 5:
                    return "PSR";
                case 6:
                    return "SlackTime";
                case 7:
                    return "RR";
                case 8:
                    return "PARD";
                default:
                    return "PAPS";
            }
        }
        private string DescProg(int Algo)
        {
            switch (Algo)
            {
                case 0:
                    return "PAPS";
                case 1:
                    return "PCA";
                case 2:
                    return "PLA";
                case 3:
                    return "PCTR";
                case 4:
                    return "PAR";
                case 5:
                    return "PSR";
                case 6:
                    return "SlackTime";
                case 7:
                    return "RR";
                case 8:
                    return "PARD";
                default:
                    return "PAPS";
            }
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            DataContext = this;

            Axes = new[] { "Temps d'attante", "Temps de service", "Temps de reponse", "Temps de fin", "Temps de Repos" };

            
            var bc = new BrushConverter();
            mainGrids = new List<Grid>() { AlgoGrid, TatGrid, TCPUGrid, TSerGrid, TRepGrid, TFinGrid, FamiGrid };
            algo1 = new List<TextBlock>() { Algo1Name, Algo1Def, Algo1Tat, Algo1TCPU, Algo1TSer, Algo1TRep, Algo1TFin, Algo1Fami,nameAlgo1 };
            algo2 = new List<TextBlock>() { Algo2Name, Algo2Def, Algo2Tat, Algo2TCPU, Algo2TSer, Algo2TRep, Algo2TFin, Algo2Fami, nameAlgo2 };
            algo3 = new List<TextBlock>() { Algo3Name, Algo3Def, Algo3Tat, Algo3TCPU, Algo3TSer, Algo3TRep, Algo3TFin, Algo3Fami, nameAlgo3};
            algo3Grids = new List<Grid>() { Algo3TatGrid, Algo3TCPUGrid, Algo3TSerGrid, Algo3TRepGrid, Algo3TFinGrid, Algo3FamiGrid };
            algo1Rects = new List<Rectangle>() { Algo1TatRect, Algo1TCPURect, Algo1TSerRect, Algo1TRepRect, Algo1TFinRect, Algo1FamiRect };
            algo2Rects = new List<Rectangle>() { Algo2TatRect, Algo2TCPURect, Algo2TSerRect, Algo2TRepRect, Algo2TFinRect, Algo2FamiRect };
            algo3Rects = new List<Rectangle>() { Algo3TatRect, Algo3TCPURect, Algo3TSerRect, Algo3TRepRect, Algo3TFinRect, Algo3FamiRect };
            rectColors = new List<Brush>() { (Brush)bc.ConvertFrom("#FF90F47B"), (Brush)bc.ConvertFrom("#FFFFB162"), (Brush)bc.ConvertFrom("#FFFF9898")  };

            if (nbPros < 3)
            {
                foreach (Grid grid in mainGrids)
                {
                    grid.ColumnDefinitions.RemoveAt(1);
                }

                Algo3Panel.Visibility = Visibility.Collapsed;

                foreach (Grid grid in algo3Grids)
                {
                    grid.Visibility = Visibility.Collapsed;
                }
                algo3Chart.Visibility = Visibility.Hidden;
                nameAlgo3.Visibility = Visibility.Hidden;
            }
            Maximum = CalculateMax();
            FillAlgo(prog1, Algos[0], algo1, Colors.Green);
            FillAlgo(prog2, Algos[1], algo2, Colors.Red);
            if (Algos.Count == 3)
            {
                nbPros = 3;
                FillAlgo(prog3, Algos[2], algo3, Colors.Blue);
            }
            Maximum = Math.Round(CalculateMax());
            ColorFix();
        }


        public List<double> GenerateRandomDataSet(int nmbrOfPoints)
        {
            var pts = new List<double>(nmbrOfPoints);
            for (var i = 0; i < nmbrOfPoints; i++)
            {
                pts.Add(random.NextDouble());
            }
            return pts;
        }
        public string[] Axes { get; set; }
        public double Maximum { get; set; }
        public ObservableCollection<ChartLine> Lines { get; set; }

        private void Swipe(object sender, MouseButtonEventArgs e)
        {

            if (pageIndex == 0)
            {
                Page1.Visibility = Visibility.Collapsed;
                Page2.Visibility = Visibility.Visible;
            }
            else
            {
                Page2.Visibility = Visibility.Collapsed;
                Page1.Visibility = Visibility.Visible;
            }
            pageIndex = (pageIndex + 1) % 2;
        }

        private void Exit(object sender, MouseEventArgs e)
        {
            MainWindow.main.Content = new Compar_Saisie();
        }



    }
}