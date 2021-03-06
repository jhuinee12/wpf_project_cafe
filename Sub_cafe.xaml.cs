﻿using System;
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
        DBSqlite DB = new DBSqlite();

        public Sub_cafe()
        {
          
            InitializeComponent();
            Load();
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            GlobalVar.btClick = 0;
            this.Close();
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            bool check = true;
            if (GlobalVar.beverage_size != "" && GlobalVar.beverage_type != "")
            {

                if (GlobalVar.product_number.StartsWith("B")) // 음료이면 옵션 상태 추가
                {
                    GlobalVar.beverage_Option = "size : " + GlobalVar.beverage_size + ", type : " + GlobalVar.beverage_type;
                }
                else
                {
                    GlobalVar.beverage_Option = "";
                }

                if (PaymentInfo.GetInstance().Count != 0)
                {
                    for (int i = 0; i < PaymentInfo.GetInstance().Count; i++)
                    {
                        if (GlobalVar.product_number.Equals(PaymentInfo.GetInstance().ElementAt(i).ProductNumber)
                            && GlobalVar.beverage_Option.Equals(PaymentInfo.GetInstance().ElementAt(i).ProductOption))
                        {
                            MessageBox.Show("이미 선택된 상품입니다.");
                            check = false;
                            break;
                        }
                    }
                }
                
                if (check)
                {
                    GlobalVar.btClick = 1;
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("음료 타입이 선택되지 않았습니다.");
            }
        }
        public void Load()
        {

            greenbord.Background= new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_sub_cafe\greenbord.jpg")));
            sub_cafe_border.Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_sub_cafe\background_img3.jpg")));
            menu_img.Background = GlobalVar.btn_select_img;
            menu_sticker_img.Background = GlobalVar.btn_select_sticker_img;
            if (GlobalVar.btn_select_explain_img == null)
            {
                explain_no.Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_explain\NoReady.jpg")));
            }
            else if (GlobalVar.btn_select_explain_img != null)
            {
                explain.Background = GlobalVar.btn_select_explain_img;
            }
               
            SELECT_NAME.Text = GlobalVar.select_name;

            // 음료이면 옵션 버튼 보이기
            if (GlobalVar.product_number.StartsWith("B"))
            {
                btn_short.Visibility = Visibility.Visible;
                btn_tall.Visibility = Visibility.Visible;
                btn_grande.Visibility = Visibility.Visible;
                btn_venti.Visibility = Visibility.Visible;
                btn_hot.Visibility = Visibility.Visible;
                btn_ice.Visibility = Visibility.Visible;

                GlobalVar.beverage_size = "";
                GlobalVar.beverage_type = "";
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

                GlobalVar.beverage_size = "none";
                GlobalVar.beverage_type = "none";
                GlobalVar.product_price = 0;
            }
        }

        private void btn_short_Click(object sender, RoutedEventArgs e)
        {
            GlobalVar.beverage_size = "short";
            GlobalVar.product_price = 0;
            btn_short.Background = yClick();
            btn_tall.Background = gClick();
            btn_grande.Background = gClick();
            btn_venti.Background = gClick();
        }

        private void btn_tall_Click(object sender, RoutedEventArgs e)
        {
            GlobalVar.beverage_size = "tall";
            GlobalVar.product_price = 500;
            btn_short.Background = gClick();
            btn_tall.Background = yClick();
            btn_grande.Background = gClick();
            btn_venti.Background = gClick();
        }

        private void btn_grande_Click(object sender, RoutedEventArgs e)
        {
            GlobalVar.beverage_size = "grande";
            GlobalVar.product_price = 1000;
            btn_short.Background = gClick();
            btn_tall.Background = gClick();
            btn_grande.Background = yClick();
            btn_venti.Background = gClick();
        }

        private void btn_venti_Click(object sender, RoutedEventArgs e)
        {
            GlobalVar.beverage_size = "venti";
            GlobalVar.product_price = 1500;
            btn_short.Background = gClick();
            btn_tall.Background = gClick();
            btn_grande.Background = gClick();
            btn_venti.Background = yClick();
        }
        private void btn_hot_Click(object sender, RoutedEventArgs e)
        {
            GlobalVar.beverage_type = "hot";
            btn_hot.Background = yClick();
            btn_ice.Background = gClick();
        }

        private void btn_ice_Click(object sender, RoutedEventArgs e)
        {
            GlobalVar.beverage_type = "ice";
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
