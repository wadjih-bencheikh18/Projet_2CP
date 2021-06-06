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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FinalAppTest.Views
{
    /// <summary>
    /// Logique d'interaction pour APropos.xaml
    /// </summary>
    public partial class APropos : Page
    {
        public int view = 0;

        public APropos()
        {
            InitializeComponent();
        }

        private void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.main.Content = new WelcomePage();
        }

        private void Ellipse_MouseEnter(object sender, MouseEventArgs e)
        {
            shadowHome.ShadowDepth = 2;
            shadowHome.BlurRadius = 7;
        }

        private void Ellipse_MouseLeave(object sender, MouseEventArgs e)
        {
            shadowHome.ShadowDepth = 0;
            shadowHome.BlurRadius = 5;
        }

        private async void Suiv_Click(object sender, MouseButtonEventArgs e)
        {
            view++;
            if (view == 0)
            {
                page1.Visibility = Visibility.Visible;
                page2.Visibility = Visibility.Hidden;
                page3.Visibility = Visibility.Hidden;
                Suiv.Visibility = Visibility.Visible;
                Prec.Visibility = Visibility.Hidden;
            }
            else if (view == 1)
            {

                Suiv.Visibility = Visibility.Hidden;
                Prec.Visibility = Visibility.Hidden;
                SuivLeftAnime(page1);
                page2.Visibility = Visibility.Visible;
                SuivRightAnime(page2);
                await Task.Delay(1500);
                Suiv.Visibility = Visibility.Visible;
                Prec.Visibility = Visibility.Visible;
                page1.Visibility = Visibility.Hidden;
                page3.Visibility = Visibility.Hidden;
            }
            else if (view == 2)
            {
                Suiv.Visibility = Visibility.Hidden;
                Prec.Visibility = Visibility.Hidden;
                SuivLeftAnime(page2);
                page3.Visibility = Visibility.Visible;
                SuivRightAnime(page3);
                await Task.Delay(1500);
                Prec.Visibility = Visibility.Visible;
                page1.Visibility = Visibility.Hidden;
                page2.Visibility = Visibility.Hidden;
            }
        }

        private  void SuivLeftAnime(Grid page)
        {
            var sb1 = new Storyboard();
            var ta1 = new ThicknessAnimation();
            ta1.BeginTime = new TimeSpan(0);
            if(page==page1)
                ta1.SetValue(Storyboard.TargetNameProperty, "page1");
            else if (page == page2)
                ta1.SetValue(Storyboard.TargetNameProperty, "page2");
            else if (page == page3)
                ta1.SetValue(Storyboard.TargetNameProperty, "page3");
            Storyboard.SetTargetProperty(ta1, new PropertyPath(MarginProperty));
            ta1.From = new Thickness(0, 0, 0, 0);
            ta1.To = new Thickness(-page.ActualWidth, 0, page.ActualWidth, 0);
            ta1.Duration = new Duration(TimeSpan.FromSeconds(1.5));
            sb1.Children.Add(ta1);
            sb1.Begin(page);
        }
        private void PrecLeftAnime(Grid page)
        {
            var sb1 = new Storyboard();
            var ta1 = new ThicknessAnimation();
            ta1.BeginTime = new TimeSpan(0);
            if (page == page1)
                ta1.SetValue(Storyboard.TargetNameProperty, "page1");
            else if (page == page2)
                ta1.SetValue(Storyboard.TargetNameProperty, "page2");
            else if (page == page3)
                ta1.SetValue(Storyboard.TargetNameProperty, "page3");
            Storyboard.SetTargetProperty(ta1, new PropertyPath(MarginProperty));
            ta1.From = new Thickness(-page.ActualWidth, 0, page.ActualWidth, 0);
            ta1.To = new Thickness(0, 0, 0, 0);
            ta1.Duration = new Duration(TimeSpan.FromSeconds(1.5));
            sb1.Children.Add(ta1);
            sb1.Begin(page);
        }
        private void SuivRightAnime(Grid page)
        {
            var sb1 = new Storyboard();
            var ta1 = new ThicknessAnimation();
            ta1.BeginTime = new TimeSpan(0); 
            if (page == page1)
                ta1.SetValue(Storyboard.TargetNameProperty, "page1");
            else if (page == page2)
                ta1.SetValue(Storyboard.TargetNameProperty, "page2");
            else if (page == page3)
                ta1.SetValue(Storyboard.TargetNameProperty, "page3");
            Storyboard.SetTargetProperty(ta1, new PropertyPath(MarginProperty));
            ta1.From = new Thickness(page.ActualWidth, 0, -page.ActualWidth, 0);
            ta1.To = new Thickness(0, 0, 0, 0);
            ta1.Duration = new Duration(TimeSpan.FromSeconds(1.5));

            sb1.Children.Add(ta1);
            sb1.Begin(page);
        }
        private void PrecRightAnime(Grid page)
        {
            var sb1 = new Storyboard();
            var ta1 = new ThicknessAnimation();
            ta1.BeginTime = new TimeSpan(0);
            if (page == page1)
                ta1.SetValue(Storyboard.TargetNameProperty, "page1");
            else if (page == page2)
                ta1.SetValue(Storyboard.TargetNameProperty, "page2");
            else if (page == page3)
                ta1.SetValue(Storyboard.TargetNameProperty, "page3");
            Storyboard.SetTargetProperty(ta1, new PropertyPath(MarginProperty));
            ta1.From = new Thickness(0, 0, 0, 0); 
            ta1.To = new Thickness(page.ActualWidth, 0, -page.ActualWidth, 0);
            ta1.Duration = new Duration(TimeSpan.FromSeconds(1.5));

            sb1.Children.Add(ta1);
            sb1.Begin(page);
        }
        private async void Prec_Click(object sender, MouseButtonEventArgs e)
        {
            view--;
            if (view == 0)
            {
                Suiv.Visibility = Visibility.Hidden;
                Prec.Visibility = Visibility.Hidden;
                PrecLeftAnime(page1);
                page1.Visibility = Visibility.Visible;
                PrecRightAnime(page2);
                await Task.Delay(1500);
                Suiv.Visibility = Visibility.Visible;
                page2.Visibility = Visibility.Hidden;
                page3.Visibility = Visibility.Hidden;
            }
            else if (view == 1)
            {

                Suiv.Visibility = Visibility.Hidden;
                Prec.Visibility = Visibility.Hidden;
                PrecLeftAnime(page2);
                page2.Visibility = Visibility.Visible;
                PrecRightAnime(page3);
                await Task.Delay(1500);
                Suiv.Visibility = Visibility.Visible;
                Prec.Visibility = Visibility.Visible;
                page1.Visibility = Visibility.Hidden;
                page3.Visibility = Visibility.Hidden;
            }
            else if (view == 2)
            {
                Suiv.Visibility = Visibility.Hidden;
                Prec.Visibility = Visibility.Visible;
                SuivLeftAnime(page2);
                page3.Visibility = Visibility.Visible;
                SuivRightAnime(page3);
                await Task.Delay(1500);
                page1.Visibility = Visibility.Hidden;
                page2.Visibility = Visibility.Hidden;
            }
        }
    }
}
