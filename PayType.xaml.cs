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
    /// PayType.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PayType : Window
    {
        Variable variable = new Variable();
        DBSqlite dbs = new DBSqlite();

        public PayType(Variable variable)
        {
            this.variable = variable;
            InitializeComponent();
        }

        public SolidColorBrush yClick()
        {
            Color ycolor = Color.FromRgb(255, 255, 0);
            SolidColorBrush ybrush = new SolidColorBrush(ycolor);
            return ybrush;
        }

        public SolidColorBrush gClick()
        {
            Color gcolor = Color.FromRgb(112, 112, 112);
            SolidColorBrush gbrush = new SolidColorBrush(gcolor);
            return gbrush;
        }

        private void btn_Store_Click(object sender, RoutedEventArgs e)
        {
            variable.place = "here";
            btn_Store.Background = yClick();
            btn_TakeOut.Background = gClick();
        }
        private void btn_TakeOut_Click(object sender, RoutedEventArgs e)
        {
            variable.place = "takeout";
            btn_TakeOut.Background = yClick();
            btn_Store.Background = gClick();
        }

        private void btn_Card_Click(object sender, RoutedEventArgs e)
        {
            if (variable.place == null)
            {
                MessageBox.Show("식사 장소를 선택해주세요");
            }
            else if (variable.place == "here")
            {
                variable.payment_method = "card";
                btn_Card.Background = yClick();
                btn_Pay.Background = gClick();

                if (MessageBox.Show("매장식사 / 카드결제\n" + "결제를 진행하시겠습니까?", "확인", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    LoadStlm();
                }
            }
            else if (variable.place == "takeout")
            {
                variable.payment_method = "card";
                btn_Card.Background = yClick();
                btn_Pay.Background = gClick();

                if (MessageBox.Show("포장 / 카드결제\n" + "결제를 진행하시겠습니까?", "확인", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    LoadStlm();
                }
            }
        }
        private void btn_Pay_Click(object sender, RoutedEventArgs e)
        {
            if (variable.place == null)
            {
                MessageBox.Show("식사 장소를 선택해주세요");
            }
            else if (variable.place == "here")
            {
                variable.payment_method = "cash";
                btn_Pay.Background = yClick();
                btn_Card.Background = gClick();

                if (MessageBox.Show("매장식사 / 현금결제\n" + "결제를 진행하시겠습니까?", "확인", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    LoadStlm();
                }
            }
            else if (variable.place == "takeout")
            {
                variable.payment_method = "cash";
                btn_Pay.Background = yClick();
                btn_Card.Background = gClick();

                if (MessageBox.Show("포장 / 현금결제\n" + "결제를 진행하시겠습니까?", "확인", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    LoadStlm();
                }
            }
        }

        public void LoadStlm()
        {
            variable.stlm_number = DateTime.Now.ToString("yyyyMMddHHmmss");     // 영수증 번호 생성
            variable.datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");   // 현재시간 생성

            // 위에서 뽑아낸 값들을 stlm 테이블에 insert
            string query = "insert into stlm values ("
                + variable.stlm_number + ",\"" + variable.payment_list + "\",\"" + String.Format("{0:#,0}", variable.sum_price) + "\",\""
                + variable.payment_method + "\",\"" + variable.place + "\",\"" + variable.datetime + "\")";
            dbs.InsertColumn(query);

            StlmLoad stl = new StlmLoad(variable);
            this.Close();
            stl.ShowDialog();
        }
    }
}
