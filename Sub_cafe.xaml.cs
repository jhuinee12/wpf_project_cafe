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
        MainWindow mw = new MainWindow();

        Variable variable = new Variable();

        DBSqlite DB = new DBSqlite();

        public Sub_cafe(Variable variable)
        {
            this.variable = variable;
            InitializeComponent();
            Load();
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            variable.btClick = 0;
            this.Close();
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            if (variable.beverage_size != "" && variable.beverage_type != "")
            {
                variable.btClick = 1;
                this.Close();
            }
            else
            {
                MessageBox.Show("음료 타입이 선택되지 않았습니다.");
            }
        }
        public void Load()
        {
            sub_cafe_border.Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_sub_cafe\background_img3.jpg")));
            menu_img.Background = GlobalVar.btn_select_img;
            menu_sticker_img.Background = GlobalVar.btn_select_sticker_img;
            explain.Background = GlobalVar.btn_select_explain_img;

            // 음료이면 옵션 버튼 보이기
            if (variable.product_number.StartsWith("B"))
            {
                btn_short.Visibility = Visibility.Visible;
                btn_tall.Visibility = Visibility.Visible;
                btn_grande.Visibility = Visibility.Visible;
                btn_venti.Visibility = Visibility.Visible;
                btn_hot.Visibility = Visibility.Visible;
                btn_ice.Visibility = Visibility.Visible;

                variable.beverage_size = "";
                variable.beverage_type = "";
            }
            // 음료가 아니면 옵션 버튼 감추기
            else
            {
                btn_short.Visibility = Visibility.Hidden;
                btn_tall.Visibility = Visibility.Hidden;
                btn_grande.Visibility = Visibility.Hidden;
                btn_venti.Visibility = Visibility.Hidden;
                btn_hot.Visibility = Visibility.Hidden;
                btn_ice.Visibility = Visibility.Hidden;

                variable.beverage_size = "none";
                variable.beverage_type = "none";
                variable.product_price = 0;
            }
        }

        private void btn_short_Click(object sender, RoutedEventArgs e)
        {
            variable.beverage_size = "short";
            variable.product_price = 0;
            btn_short.Background = yClick();
            btn_tall.Background = gClick();
            btn_grande.Background = gClick();
            btn_venti.Background = gClick();
        }

        private void btn_tall_Click(object sender, RoutedEventArgs e)
        {
            variable.beverage_size = "tall";
            variable.product_price = 500;
            btn_short.Background = gClick();
            btn_tall.Background = yClick();
            btn_grande.Background = gClick();
            btn_venti.Background = gClick();
        }

        private void btn_grande_Click(object sender, RoutedEventArgs e)
        {
            variable.beverage_size = "grande";
            variable.product_price = 1000;
            btn_short.Background = gClick();
            btn_tall.Background = gClick();
            btn_grande.Background = yClick();
            btn_venti.Background = gClick();
        }

        private void btn_venti_Click(object sender, RoutedEventArgs e)
        {
            variable.beverage_size = "venti";
            variable.product_price = 1500;
            btn_short.Background = gClick();
            btn_tall.Background = gClick();
            btn_grande.Background = gClick();
            btn_venti.Background = yClick();
        }
        private void btn_hot_Click(object sender, RoutedEventArgs e)
        {
            variable.beverage_type = "hot";
            btn_hot.Background = yClick();
            btn_ice.Background = gClick();
        }

        private void btn_ice_Click(object sender, RoutedEventArgs e)
        {
            variable.beverage_type = "hot";
            btn_hot.Background = gClick();
            btn_ice.Background = yClick();
        }


        //델리게이트로 끌고 와야하는데 리턴방식이 solid방식이라 끌어오지못함..그래서 메서드 추가시킴
        private SolidColorBrush yClick()
        {
            Color ycolor = Color.FromRgb(255, 255, 0);
            SolidColorBrush ybrush = new SolidColorBrush(ycolor);
            return ybrush;
        }

        private SolidColorBrush gClick()
        {
            Color gcolor = Color.FromRgb(112, 112, 112);
            SolidColorBrush gbrush = new SolidColorBrush(gcolor);
            return gbrush;
        }

       
    }
}
