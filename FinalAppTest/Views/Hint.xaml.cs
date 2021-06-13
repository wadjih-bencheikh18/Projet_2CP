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
    /// Interaction logic for Hint.xaml
    /// </summary>
    public partial class Hint : UserControl
    {
        public string TitleText { get; set; }
        public string DescriptionText { get; set; }
        public UserControl Page;
        public Viewbox hint;
        public Hint(string TitleText, string DescriptionText,UserControl Page,Viewbox hint)
        {
            InitializeComponent();
            this.TitleText = TitleText;
            this.DescriptionText = DescriptionText;
            this.Page = Page;
            this.hint = hint;
        }
        public Hint()
        {
            InitializeComponent();
        }
        public void Fin()
        {
            Suivant.Text = "FIN";
            Suivant.Margin = new Thickness(0, 0, 60, 5);
        }
        private void Next_Button(object sender, MouseButtonEventArgs e)
        {
            if(Page.GetType()==typeof(PAPS_Tab))
            {
                if(Suivant.Text=="FIN")
                {
                    ((PAPS_Tab)Page).FinHint();
                }
                else if(PAPS_Tab.NextHintCondition)
                {
                    PAPS_Tab.NbHint++;
                    hint.Child = null;
                    ((PAPS_Tab)Page).Hint();
                }
                else
                {
                    Storyboard sb = this.FindResource("Error") as Storyboard;
                    sb.Begin();
                }
            }
            else if (Page.GetType() == typeof(RR_Tab))
            {
                if (Suivant.Text == "FIN")
                {
                    ((RR_Tab)Page).FinHint();
                }
                else if (RR_Tab.NextHintCondition)
                {
                    RR_Tab.NbHint++;
                    hint.Child = null;
                    ((RR_Tab)Page).Hint();
                }
                else
                {
                    Storyboard sb = this.FindResource("Error") as Storyboard;
                    sb.Begin();
                }
            }
            else if (Page.GetType() == typeof(PARD_Tab))
            {
                if (Suivant.Text == "FIN")
                {
                    ((PARD_Tab)Page).FinHint();
                }
                else if (PARD_Tab.NextHintCondition)
                {
                    PARD_Tab.NbHint++;
                    hint.Child = null;
                    ((PARD_Tab)Page).Hint();
                }
                else
                {
                    Storyboard sb = this.FindResource("Error") as Storyboard;
                    sb.Begin();
                }
            }


        }
         private void Skip_Button(object sender, MouseButtonEventArgs e)
        {
            if (Page.GetType() == typeof(PAPS_Tab))
            {
              
                    ((PAPS_Tab)Page).FinHint();
               
            }
            else if (Page.GetType() == typeof(RR_Tab))
            {
                    ((RR_Tab)Page).FinHint();
                
            }
            else if (Page.GetType() == typeof(PARD_Tab))
            {
                
                    ((PARD_Tab)Page).FinHint();
               
            }
        }

        private void Previous_Button(object sender, MouseButtonEventArgs e)
        {
            if (Page.GetType() == typeof(PAPS_Tab))
            {
                if(PAPS_Tab.NbHint>0)
                {
                    PAPS_Tab.NbHint--;
                    hint.Child = null;
                    ((PAPS_Tab)Page).Hint();
                }
            }
            else if (Page.GetType() == typeof(RR_Tab))
            {
                if (RR_Tab.NbHint > 0)
                {
                    RR_Tab.NbHint--;
                    hint.Child = null;
                    ((RR_Tab)Page).Hint();
                }
            }
            else if (Page.GetType() == typeof(PARD_Tab))
            {
                if (PARD_Tab.NbHint > 0)
                {
                    PARD_Tab.NbHint--;
                    hint.Child = null;
                    ((PARD_Tab)Page).Hint();
                }
            }
        }
    }
}
