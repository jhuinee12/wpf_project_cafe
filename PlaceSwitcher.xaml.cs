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

namespace WPF_project_Cafe
{
    /// <summary>
    /// PlaceSwitcher.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PlaceSwitcher : UserControl
    {
        Variable variable = new Variable();
        public PlaceSwitcher(Variable variable)
        {
            this.variable = variable;
            InitializeComponent();
        }
        private void btn_Store_Click(object sender, RoutedEventArgs e)
        {
            variable.place = "here";
            PaymentMethod pm = new PaymentMethod(variable);
            this.Content = pm;
        }
        private void btn_TakeOut_Click(object sender, RoutedEventArgs e)
        {
            variable.place = "takeout";
            PaymentMethod pm = new PaymentMethod(variable);
            this.Content = pm;
        }
    }
}
