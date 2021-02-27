using System;
using System.Collections.Generic;
using System.IO;
using static System.IO.Directory;
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
    /// StlmLoad.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class StlmLoad : Window
    {
     
        DBSqlite dbs = new DBSqlite();
        public StlmLoad()
        {
            
            InitializeComponent();

            btn_home.Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_btn\home.png")));
            btn_home.Style = FindResource("Btn_Style") as Style;

            string dir = Environment.CurrentDirectory + @"\stlm_list\";

            if (Exists(dir))
            {
                dbs.StlmLoadData(GlobalVar.stlm_number);
            }
            else
            {
                CreateDirectory(dir);
            }

            Stml();
        }

        public void Stml()
        {
            // 카드/현금 결제방법 구분
            String paytype = dbs.DataLoad("stlm", "where stlm_number = " + GlobalVar.stlm_number, "card_cash");

            txt_sum_price.Text = dbs.DataLoad("stlm", "where stlm_number = " + GlobalVar.stlm_number, "sum_price") + "원";
            txt_date.Text = dbs.DataLoad("stlm", "where stlm_number = " + GlobalVar.stlm_number, "datetime");
            txt_stlm_number.Text = GlobalVar.stlm_number.ToString();
            txt_payType.Text = (paytype == "card") ? "카드결제" : "현금결제";

            txt_product_name.Text = dbs.StlmProductName(GlobalVar.stlm_number.ToString());
            txt_product_quantity.Text = dbs.StlmProductQuantity(GlobalVar.stlm_number.ToString());
            txt_product_price.Text = dbs.StlmProductPrice(GlobalVar.stlm_number.ToString());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // variable 리셋
            GlobalVar.product_price = 0;
            GlobalVar.sum_price = 0;
            GlobalVar.datetime = "";
            GlobalVar.stlm_number = "";
            GlobalVar.product_number = "";
            GlobalVar.payment_list = "";
            GlobalVar.place = "";
            GlobalVar.payment_method = "";

            PaymentInfo.GetInstance().Clear();

            MainWindow mw = new MainWindow();
            this.Close();
            mw.ShowDialog();
        }
    }
}
