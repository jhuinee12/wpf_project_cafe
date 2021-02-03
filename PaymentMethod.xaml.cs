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
    /// PaymentMethod.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PaymentMethod : UserControl
    {
        DBSqlite dbs = new DBSqlite();
        Variable variable = new Variable();
        public PaymentMethod(Variable variable)
        {
            this.variable = variable;
            InitializeComponent();
        }
        private void btn_Card_Click(object sender, RoutedEventArgs e)
        {
            variable.payment_method = "card";
            LoadStlm();
            Stml stml = new Stml();
            this.Content = stml;
        }
        private void btn_Pay_Click(object sender, RoutedEventArgs e)
        {
            variable.payment_method = "cash";
            LoadStlm();
            Stml stml = new Stml();
            this.Content = stml;
        }

        public void LoadStlm()
        {
            try // 영수증 목록이 있으면
            {
                // 영수증 번호 생성 (마지막 영수증 번호 +1)
                variable.stlm_number = Int32.Parse(dbs.DataLoad("stlm", "order by stlm_number desc limit 1", "stlm_number")) + 1;
            }
            catch (Exception ex)     // 영수증 목록이 없으면
            {
                MessageBox.Show(ex.ToString());
                variable.stlm_number = 1;    // 영수증 번호 : 1
            }

            string datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); // 구매 시간

            MessageBox.Show("<" + variable.stlm_number + "> 영수증" + "\npayment_list : " + variable.payment_list + "\nprice : " + variable.sum_price);

            // 위에서 뽑아낸 값들을 stlm 테이블에 insert
            string query = "insert into stlm values ("
                + variable.stlm_number + ",\"\",\"" + variable.payment_list + "\"," + variable.sum_price + ", \""
                + variable.payment_method + "\",\"" + variable.place + "\",\"" + datetime + "\")";
            dbs.InsertColumn(query);
            dbs.StlmLoadData(variable.stlm_number.ToString());   // 현재 영수증 테이블 로드
        }
    }
}
