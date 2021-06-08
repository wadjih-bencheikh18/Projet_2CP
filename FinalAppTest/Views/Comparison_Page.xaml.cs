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
    /// Logique d'interaction pour Comparison_Page.xaml
    /// </summary>
    public partial class Comparison_Page : Page
    {
        private List<Grid> mainGrids, algo3Grids;
        private List<TextBlock> algo1, algo2, algo3;
        private List<Rectangle> algo1Rects, algo2Rects, algo3Rects;
        private List<String> rectColors;
        private int nbPros = 2;
        private int pageIndex = 0;
       /* List<int> caca = new List<int>()
        {
            Algo1Tser,
        };*/
        public Comparison_Page()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            mainGrids = new List<Grid>() { AlgoGrid,TatGrid,TCPUGrid,TSerGrid,TRepGrid,TFinGrid,FamiGrid };
            algo1 = new List<TextBlock>() { Algo1Name, Algo1Def, Algo1Tat, Algo1TCPU, Algo1TSer, Algo1TRep, Algo1TFin, Algo1Fami };
            algo2 = new List<TextBlock>() { Algo2Name, Algo2Def, Algo2Tat, Algo2TCPU, Algo2TSer, Algo2TRep, Algo2TFin, Algo2Fami };
            algo3 = new List<TextBlock>() { Algo3Name, Algo3Def, Algo3Tat, Algo3TCPU, Algo3TSer, Algo3TRep, Algo3TFin, Algo3Fami };
            algo3Grids = new List<Grid>() { Algo3TatGrid, Algo3TCPUGrid, Algo3TSerGrid, Algo3TRepGrid, Algo3TFinGrid , Algo3FamiGrid};
            algo1Rects = new List<Rectangle>() { Algo1TatRect, Algo1TCPURect, Algo1TSerRect, Algo1TRepRect, Algo1TFinRect, Algo1FamiRect };
            algo2Rects = new List<Rectangle>() { Algo2TatRect, Algo2TCPURect, Algo2TSerRect, Algo2TRepRect, Algo2TFinRect, Algo2FamiRect };
            algo3Rects = new List<Rectangle>() { Algo3TatRect, Algo3TCPURect, Algo3TSerRect, Algo3TRepRect, Algo3TFinRect, Algo3FamiRect };
            rectColors = new List<String>() { "#FF90F47B", "#FFFFB162", "#FFFF9898" };

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
            }
        }



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