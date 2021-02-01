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
using System.Windows.Shapes;

namespace WPF_project_Cafe
{
    /// <summary>
    /// Sub_cafe.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Sub_cafe : Window
    {
        public Sub_cafe()
        {
            InitializeComponent();
            img();

        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {

        }
        public void img()
        {
            //sub_cafe_border.Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_sub_cafe\background_img3.jpg")));
            img_area.Background = GlobalVar.btn_select_img;
        }
    }
}
