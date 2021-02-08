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
        Variable variable = new Variable();
        DBSqlite dbs = new DBSqlite();
        public StlmLoad(Variable variable)
        {
            this.variable = variable;
            InitializeComponent();

            string dir = Environment.CurrentDirectory + @"\stlm_list\";

            if (Exists(dir))
            {
                dbs.StlmLoadData(variable.stlm_number);
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
            String paytype = dbs.DataLoad("stlm", "where stlm_number = " + variable.stlm_number, "card_cash");

            txt_sum_price.Text = variable.sum_price + "원";
            txt_date.Text = dbs.DataLoad("stlm", "where stlm_number = " + variable.stlm_number, "datetime");
            txt_stlm_number.Text = variable.stlm_number.ToString();
            txt_payType.Text = (paytype == "card") ? "카드결제" : "현금결제";

            txt_product_name.Text = dbs.StlmProductName(variable.stlm_number.ToString());
            txt_product_quantity.Text = dbs.StlmProductQuantity(variable.stlm_number.ToString());
            txt_product_price.Text = dbs.StlmProductPrice(variable.stlm_number.ToString());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // variable 리셋
            variable.product_price = 0;
            variable.sum_price = 0;
            variable.datetime = "";
            variable.stlm_number = "";
            variable.product_number = "";
            variable.payment_list = "";
            variable.place = "";
            variable.payment_method = "";

            PaymentInfo.GetInstance().Clear();

            MainWindow mw = new MainWindow();
            this.Close();
            mw.ShowDialog();
        }
    }
}
