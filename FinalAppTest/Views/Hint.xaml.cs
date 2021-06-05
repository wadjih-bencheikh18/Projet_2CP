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

        private void Next_Button(object sender, MouseButtonEventArgs e)
        {
            if(Page.GetType()==typeof(PAPS_Tab))
            {
                PAPS_Tab.NbHint++;
                if(PAPS_Tab.NextHintCondition)
                {
                    hint.Child = null;
                    ((PAPS_Tab)Page).Hint_MouseLeftButtonDown(sender, e);
                }
            }
            
            
        }
         private void Skip_Button(object sender, MouseButtonEventArgs e)
        {

        }

        private void Previous_Button(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
