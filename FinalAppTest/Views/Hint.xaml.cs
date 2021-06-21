using FinalAppTest.Comparaison;
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
        public Page Comp_page;
        public Viewbox hint;
        public Hint(string TitleText, string DescriptionText,UserControl Page,Viewbox hint)
        {
            InitializeComponent();
            this.TitleText = TitleText;
            this.DescriptionText = DescriptionText;
            this.Page = Page;
            this.hint = hint;
        }
        public Hint(string TitleText, string DescriptionText, Page Page, Viewbox hint)
        {
            InitializeComponent();
            this.TitleText = TitleText;
            this.DescriptionText = DescriptionText;
            this.Comp_page = Page;
            this.Page = null;
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
            if (Page == null)  // Comp page
            {
                if (Suivant.Text == "FIN")
                {
                    ((Compar_Saisie)Comp_page).FinHint();
                }
                else if (Compar_Saisie.NextHintCondition)
                {
                    Compar_Saisie.NbHint++;
                    hint.Child = null;
                    ((Compar_Saisie)Comp_page).Hint();
                }
                else
                {
                    Storyboard sb = this.FindResource("Error") as Storyboard;
                    sb.Begin();
                }
            }
            else if(Page.GetType()==typeof(PAPS_Tab))
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
            else if (Page.GetType() == typeof(Mult_Niv_Tab))
            {
                if (Suivant.Text == "FIN")
                {
                    ((Mult_Niv_Tab)Page).FinHint();
                }
                else if (Mult_Niv_Tab.NextHintCondition)
                {
                    Mult_Niv_Tab.NbHint++;
                    hint.Child = null;
                    ((Mult_Niv_Tab)Page).Hint();
                }
                else
                {
                    Storyboard sb = this.FindResource("Error") as Storyboard;
                    sb.Begin();
                }
            }
            else if (Page.GetType() == typeof(Mult_Niv_Recyclage_Tab))
            {
                if (Suivant.Text == "FIN")
                {
                    ((Mult_Niv_Recyclage_Tab)Page).FinHint();
                }
                else if (Mult_Niv_Recyclage_Tab.NextHintCondition)
                {
                    Mult_Niv_Recyclage_Tab.NbHint++;
                    hint.Child = null;
                    ((Mult_Niv_Recyclage_Tab)Page).Hint();
                }
                else
                {
                    Storyboard sb = this.FindResource("Error") as Storyboard;
                    sb.Begin();
                }
            }
        
            else if (Page.GetType() == typeof(PSR_Tab))
            {
                if (Suivant.Text == "FIN")
                {
                    ((PSR_Tab)Page).FinHint();
                }
                else if (PSR_Tab.NextHintCondition)
                {
                    PSR_Tab.NbHint++;
                    hint.Child = null;
                    ((PSR_Tab)Page).Hint();
                }
                else
                {
                    Storyboard sb = this.FindResource("Error") as Storyboard;
                    sb.Begin();
                }
            }
            else if (Page.GetType() == typeof(PAR_Tab))
            {
                if (Suivant.Text == "FIN")
                {
                    ((PAR_Tab)Page).FinHint();
                }
                else if (PAR_Tab.NextHintCondition)
                {
                    PAR_Tab.NbHint++;
                    hint.Child = null;
                    ((PAR_Tab)Page).Hint();
                }
                else
                {
                    Storyboard sb = this.FindResource("Error") as Storyboard;
                    sb.Begin();
                }
            }
            else if (Page.GetType() == typeof(SlackTime_Tab))
            {
                if (Suivant.Text == "FIN")
                {
                    ((SlackTime_Tab)Page).FinHint();
                }
                else if (SlackTime_Tab.NextHintCondition)
                {
                    SlackTime_Tab.NbHint++;
                    hint.Child = null;
                    ((SlackTime_Tab)Page).Hint();
                }
                else
                {
                    Storyboard sb = this.FindResource("Error") as Storyboard;
                    sb.Begin();
                }
            }
            else if (Page.GetType() == typeof(PCA_Tab))
            {
                if (Suivant.Text == "FIN")
                {
                    ((PCA_Tab)Page).FinHint();
                }
                else if (PCA_Tab.NextHintCondition)
                {
                    PCA_Tab.NbHint++;
                    hint.Child = null;
                    ((PCA_Tab)Page).Hint();
                }
                else
                {
                    Storyboard sb = this.FindResource("Error") as Storyboard;
                    sb.Begin();
                }
            }
            else if (Page.GetType() == typeof(PCTR_Tab))
            {
                if (Suivant.Text == "FIN")
                {
                    ((PCTR_Tab)Page).FinHint();
                }
                else if (PCTR_Tab.NextHintCondition)
                {
                    PCTR_Tab.NbHint++;
                    hint.Child = null;
                    ((PCTR_Tab)Page).Hint();
                }
                else
                {
                    Storyboard sb = this.FindResource("Error") as Storyboard;
                    sb.Begin();
                }
            }
            else if (Page.GetType() == typeof(PLA_Tab))
            {
                if (Suivant.Text == "FIN")
                {
                    ((PLA_Tab)Page).FinHint();
                }
                else if (PLA_Tab.NextHintCondition)
                {
                    PLA_Tab.NbHint++;
                    hint.Child = null;
                    ((PLA_Tab)Page).Hint();
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
            if (Page == null)  // Comp page
            {
                ((Compar_Saisie)Comp_page).FinHint();
            }
            else if (Page.GetType() == typeof(PAPS_Tab))
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
            else if (Page.GetType() == typeof(PSR_Tab))
            {
               
                    ((PSR_Tab)Page).FinHint();
               
            }
            else if (Page.GetType() == typeof(PAR_Tab))
            {
                
                    ((PAR_Tab)Page).FinHint();
               
            }
            else if (Page.GetType() == typeof(SlackTime_Tab))
            {
                
                    ((SlackTime_Tab)Page).FinHint();
                
            }
            else if (Page.GetType() == typeof(PCA_Tab))
            {

                ((PCA_Tab)Page).FinHint();

            }
            else if (Page.GetType() == typeof(PCTR_Tab))
            {

                ((PCTR_Tab)Page).FinHint();

            }
            else if (Page.GetType() == typeof(PLA_Tab))
            {

                ((PLA_Tab)Page).FinHint();

            }
            else if (Page.GetType() == typeof(Mult_Niv_Tab))
            {

                ((Mult_Niv_Tab)Page).FinHint();

            }
            else if (Page.GetType() == typeof(Mult_Niv_Recyclage_Tab))
            {

                ((Mult_Niv_Recyclage_Tab)Page).FinHint();

            }
        }

        private void Previous_Button(object sender, MouseButtonEventArgs e)
        {
            if (Page == null)
            {
                if (Compar_Saisie.NbHint > 0)
                {
                    Compar_Saisie.NbHint++;
                    hint.Child = null;
                    ((Compar_Saisie)Comp_page).Hint();
                }
            }
            else if (Page.GetType() == typeof(PAPS_Tab))
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
            else if (Page.GetType() == typeof(PSR_Tab))
            {
                
                if (PSR_Tab.NbHint>0)
                {
                    PSR_Tab.NbHint--;
                    hint.Child = null;
                    ((PSR_Tab)Page).Hint();
                }
                
            }
            else if (Page.GetType() == typeof(PAR_Tab))
            {
                
                if (PAR_Tab.NbHint>0)
                {
                    PAR_Tab.NbHint--;
                    hint.Child = null;
                    ((PAR_Tab)Page).Hint();
                }
                
            }
            else if (Page.GetType() == typeof(SlackTime_Tab))
            {
                if (SlackTime_Tab.NbHint>0)
                {
                    SlackTime_Tab.NbHint++;
                    hint.Child = null;
                    ((SlackTime_Tab)Page).Hint();
                }
                
            }
            else if (Page.GetType() == typeof(PCA_Tab))
            {
                if (PCA_Tab.NbHint > 0)
                {
                    PCA_Tab.NbHint++;
                    hint.Child = null;
                    ((PCA_Tab)Page).Hint();
                }

            }
            else if (Page.GetType() == typeof(PCTR_Tab))
            {
                if (PCTR_Tab.NbHint > 0)
                {
                    PCTR_Tab.NbHint++;
                    hint.Child = null;
                    ((PCTR_Tab)Page).Hint();
                }

            }
            else if (Page.GetType() == typeof(PLA_Tab))
            {
                if (PLA_Tab.NbHint > 0)
                {
                    PLA_Tab.NbHint++;
                    hint.Child = null;
                    ((PLA_Tab)Page).Hint();
                }

            }
            else if (Page.GetType() == typeof(Mult_Niv_Tab))
            {
                if (Mult_Niv_Tab.NbHint > 0)
                {
                    Mult_Niv_Tab.NbHint++;
                    hint.Child = null;
                    ((Mult_Niv_Tab)Page).Hint();
                }

            }
            else if (Page.GetType() == typeof(Mult_Niv_Recyclage_Tab))
            {
                if (Mult_Niv_Recyclage_Tab.NbHint > 0)
                {
                    Mult_Niv_Recyclage_Tab.NbHint++;
                    hint.Child = null;
                    ((Mult_Niv_Recyclage_Tab)Page).Hint();
                }

            }
        }
    }
}
