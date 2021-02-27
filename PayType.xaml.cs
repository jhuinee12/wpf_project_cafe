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
       
        DBSqlite dbs = new DBSqlite();

        public PayType()
        { 
            InitializeComponent();
            btn_Store.Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_btn\store.png")));
            btn_TakeOut.Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_btn\takeout.png")));
            btn_Card.Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_btn\card.png")));
            btn_Cash.Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_btn\pay.png")));
            btn_Back.Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_btn\home.png")));
            btn_Pay.Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_btn\payment.png")));
            btn_Store.Style = FindResource("Btn_Style") as Style;
            btn_TakeOut.Style = FindResource("Btn_Style") as Style;
            btn_Card.Style = FindResource("Btn_Style") as Style;
            btn_Cash.Style = FindResource("Btn_Style") as Style;
            btn_Back.Style = FindResource("Btn_Style") as Style;
            btn_Pay.Style = FindResource("Btn_Style") as Style;
        }

        private void btn_Store_Click(object sender, RoutedEventArgs e)
        {
            GlobalVar.place = "here";
            btn_Store.Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_btn\store_click.png")));
            btn_TakeOut.Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_btn\takeout.png")));
        }
        private void btn_TakeOut_Click(object sender, RoutedEventArgs e)
        {
            GlobalVar.place = "takeout";
            btn_Store.Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_btn\store.png")));
            btn_TakeOut.Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_btn\takeout_click.png")));
        }

        private void btn_Card_Click(object sender, RoutedEventArgs e)
        {
            GlobalVar.payment_method = "card";
            btn_Card.Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_btn\card_click.png")));
            btn_Cash.Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_btn\pay.png")));
        }
        private void btn_Cash_Click(object sender, RoutedEventArgs e)
        {
            GlobalVar.payment_method = "cash";
            btn_Card.Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_btn\card.png")));
            btn_Cash.Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_btn\pay_click.png")));
        }

        private void btn_Pay_Click(object sender, RoutedEventArgs e)
        {

            if (GlobalVar.place == null)
            {
                MessageBox.Show("식사 장소를 선택해주세요");
            }
            else if (GlobalVar.payment_method == null)
            {
                MessageBox.Show("결제 방법을 선택해주세요");
            }
            else
            {
                string pm = GlobalVar.payment_method == "card" ? "카드결제" : "현금결제";
                string pp = GlobalVar.place == "here" ? "매장식사" : "포장";
                if (MessageBox.Show(pp + " / " + pm + "\n결제를 진행하시겠습니까?", "확인", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    LoadStlm();
                }
            }
        }
        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            GlobalVar.payment_list = "";
            GlobalVar.sum_price = 0;
            MainWindow mw = new MainWindow();
            this.Close();
            mw.ShowDialog();
        }

        public void LoadStlm()
        {
            GlobalVar.stlm_number = DateTime.Now.ToString("yyyyMMddHHmmss");     // 영수증 번호 생성
            GlobalVar.datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");   // 현재시간 생성

            // 위에서 뽑아낸 값들을 stlm 테이블에 insert
            string query = "insert into stlm values ("
                + GlobalVar.stlm_number + ",\"" + GlobalVar.payment_list + "\",\"" + String.Format("{0:#,0}", GlobalVar.sum_price) + "\",\""
                + GlobalVar.payment_method + "\",\"" + GlobalVar.place + "\",\"" + GlobalVar.datetime + "\")";
            dbs.InsertColumn(query);

            StlmLoad stl = new StlmLoad();
            this.Close();
            stl.ShowDialog();
        }
    }
}
