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
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        DBSqlite dbs = new DBSqlite(); // db 생성자

        // sticker_mode = new/hot 이라면  new/hot 스키터 
        public string sticker_mode = "nomal";
        public string beverage_name = "nothing";

        public string product_number = "";
        public int product_price = 0;
        public int stlm_number;

        //음료 경로
        public ImageBrush amelicano = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_beverage\amelicano.jpg")));
        public ImageBrush apple_mint_tea = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_beverage\apple_mint_tea.jpg")));
        public ImageBrush banana_milkshake = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_beverage\banana_milkshake.jpg")));
        public ImageBrush cafe_mocha = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_beverage\cafe_mocha.jpg")));
        public ImageBrush cappuccino = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_beverage\cappuccino.jpg")));
        public ImageBrush chamomile = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_beverage\chamomile.jpg")));
        public ImageBrush cold_brew = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_beverage\cold_brew.jpg")));
        public ImageBrush espresso = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_beverage\espresso.jpg")));
        public ImageBrush french_earl_grey = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_beverage\french_earl_grey.jpg")));


        public MainWindow()
        {
            InitializeComponent();

            Init();
            //동적 그리드 나누기
            Menu_size();
            //동적 버튼 생성하기
            Menu_btn_add();
            LoadListView();

            //스티커 생성 sticker_mode = new 또는 hot 이라면  new 또는 hot 스티커 
            //스티커 한개 생성하려면  sticker_add(a,b),  컬럼 a 로우b 의 위치에 생성됨
            //한번에 스티커 2개  생성하려면  sticker_add(a,b,c,d),  컬럼 a 로우b 의 위치에 1개 생성 c,d위치에 1개 생성
            //최대 한번에 3개 까지 생성 
            //sticker_add(0,1,2,2,2,1,1);

            sticker_mode = "hot";
            sticker_add(0, 1);
        }

        // Menu_select() 에서 Menu_count = 2 면  2*2 size
        public int Menu_count = 9;

        //언어 선택을 위한 국가 이미지 
        public void Init()
        {
            try
            {
                //국기 이미지
                Kor.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_country\korea.jpg"));
                Eng.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_country\amelica.jpg"));
                Chn.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_country\china.jpg"));
                Jpn.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_country\japan.jpg"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        //Menu 동적 그리드 나누기
        public void Menu_size()
        {
            //  Menu.ShowGridLines = true;


            for (int i = 0; i < Menu_count; i++)
            {
                RowDefinition rd = new RowDefinition();
                rd.Height = new GridLength(1, GridUnitType.Star);
                Menu.RowDefinitions.Add(rd);
            }

            for (int i = 0; i < Menu_count; i++)
            {
                ColumnDefinition cd = new ColumnDefinition();
                cd.Width = new GridLength(1, GridUnitType.Star);
                Menu.ColumnDefinitions.Add(cd);
            }

        }
        //Menu 버튼생성
        public void Menu_btn_add()
        {
            Button[] btn = new Button[9];

            string[] Name =
                           { "아메리카노","애플민트차" ,"바나나 밀크쉐이크"
                           , "카페 모카","카푸치노","카모마일"
                           , "콜드브루", "에스프레소","프렌치 얼그레이"
                           };
            //버튼
            int btn_num = 0;
            int col_cnt = 0;
            int row_cnt = 0;
            int[] Colum_set = { 0, 3, 6 };
            int[] Row_set = { 0, 3, 6 };

            for (int i = 0; i < Menu_count; i++)
            {

                btn[btn_num] = new Button();

                btn[btn_num].Content = Name[btn_num];
                // btn[btn_num].Background = Brushes.White;
                // btn[btn_num].BorderThickness = new Thickness(0, 0, 0, 0);

                //버튼의 영역을 3*3으로
                Grid.SetColumnSpan(btn[btn_num], 3);
                Grid.SetRowSpan(btn[btn_num], 3);

                //버튼의 col/row값을 설정
                Grid.SetColumn(btn[btn_num], Colum_set[col_cnt]);
                Grid.SetRow(btn[btn_num], Row_set[row_cnt]);

                //다음 컬럼으로 이동
                col_cnt++;

                //끝 컬럼 쪽이라면 로우를 이동시키고 컬럼도 다시 초기화
                if (col_cnt > 2)
                {
                    col_cnt = 0;
                    row_cnt++;

                    if (row_cnt > 2)
                    {
                        row_cnt = 0;
                    }
                }

                //버튼의 스타일을 재정의
                btn[btn_num].FontWeight = FontWeights.Bold;
                btn[btn_num].Style = FindResource("Button_Style") as Style;

                //버튼을 추가한다 
                Menu.Children.Add(btn[btn_num]);
                //btn[btn_num].Style = FindResource("test123") as Style;
                //버튼 이름을 바꿔주기 위함
                btn_num = btn_num + 1;

            }

            //버튼 이미지            
            btn[0].Background = amelicano;
            btn[1].Background = apple_mint_tea;
            btn[2].Background = banana_milkshake;
            btn[3].Background = cafe_mocha;
            btn[4].Background = cappuccino;
            btn[5].Background = chamomile;
            btn[6].Background = cold_brew;
            btn[7].Background = espresso;
            btn[8].Background = french_earl_grey;
            //btn[2].Template = FindResource("test123") as ControlTemplate;
        }
        #region 스티커 생성 , 한번에 최대 3개의 스티커까지 생성가능     
        public void sticker_add(int set_sticker_col, int set_sticker_row)
        {


            //int sticker_num=0;
            int[] Colum_set = { 0, 3, 6 };
            int[] Row_set = { 0, 3, 6 };
            int col_cnt = 0;
            int row_cnt = 0;

            Label[] sticker = new Label[9];

            for (int sticker_num = 0; sticker_num < Menu_count; sticker_num++)
            {
                //스티커 테스트
                sticker[sticker_num] = new Label();

                if (sticker_mode == "new")
                {
                    sticker[sticker_num].Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_sticker\new.png")));
                }
                else if (sticker_mode == "hot")
                {
                    sticker[sticker_num].Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_sticker\hot.png")));
                }

                Grid.SetColumn(sticker[sticker_num], Colum_set[set_sticker_col]);
                Grid.SetRow(sticker[sticker_num], Row_set[set_sticker_row]);


                Menu.Children.Add(sticker[sticker_num]);
                ////전체 스티커 씌우기
                //col_cnt++;
                //if (col_cnt > 2)
                //{
                //    col_cnt = 0;
                //    row_cnt++;

                //    if (row_cnt > 2)
                //    {
                //        row_cnt = 0;
                //    }
                //}
            }


        }
        public void sticker_add(int set_sticker_col, int set_sticker_row, int set_sticker_col2, int set_sticker_row2)
        {
            sticker_add(set_sticker_col, set_sticker_row);

            int[] Colum_set = { 0, 3, 6 };
            int[] Row_set = { 0, 3, 6 };
            Label[] sticker = new Label[9];

            for (int sticker_num = 0; sticker_num < Menu_count; sticker_num++)
            {

                sticker[sticker_num] = new Label();
                if (sticker_mode == "new")
                {
                    sticker[sticker_num].Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_sticker\new.jpg")));
                }
                else if (sticker_mode == "hot")
                {
                    sticker[sticker_num].Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_sticker\hot.jpg")));
                }
                Grid.SetColumn(sticker[sticker_num], Colum_set[set_sticker_col2]);
                Grid.SetRow(sticker[sticker_num], Row_set[set_sticker_row2]);

                Menu.Children.Add(sticker[sticker_num]);
            }
        }
        public void sticker_add(int set_sticker_col, int set_sticker_row, int set_sticker_col2, int set_sticker_row2, int set_sticker_col3, int set_sticker_row3)
        {
            sticker_add(set_sticker_col, set_sticker_row, set_sticker_col2, set_sticker_row2);

            int[] Colum_set = { 0, 3, 6 };
            int[] Row_set = { 0, 3, 6 };
            Label[] sticker = new Label[9];

            for (int sticker_num = 0; sticker_num < Menu_count; sticker_num++)
            {

                sticker[sticker_num] = new Label();

                sticker[sticker_num].Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_sticker\new.jpg")));

                Grid.SetColumn(sticker[sticker_num], Colum_set[set_sticker_col3]);
                Grid.SetRow(sticker[sticker_num], Row_set[set_sticker_row3]);

                Menu.Children.Add(sticker[sticker_num]);
            }
        }
        #endregion

        private void btn_beverage_Click(object sender, RoutedEventArgs e)
        {
            //선택된 섹션은 흰색으로 선택되지 않는 섹션은 회색으로 하여 표현  
            btn_beverage.Background = Brushes.White;
            btn_dessert.Background = Brushes.LightGray;
            btn_etc.Background = Brushes.LightGray;
        }

        private void btn_dessert_Click(object sender, RoutedEventArgs e)
        {
            //선택된 섹션은 흰색으로 선택되지 않는 섹션은 회색으로 하여 표현  
            btn_beverage.Background = Brushes.LightGray;
            btn_dessert.Background = Brushes.White;
            btn_etc.Background = Brushes.LightGray;

        }

        private void btn_etc_Click(object sender, RoutedEventArgs e)
        {
            //선택된 섹션은 흰색으로 선택되지 않는 섹션은 회색으로 하여 표현  
            btn_beverage.Background = Brushes.LightGray;
            btn_dessert.Background = Brushes.LightGray;
            btn_etc.Background = Brushes.White;
        }

        private void btn_kor_Click(object sender, RoutedEventArgs e)
        {
            //선택된 섹션은 흰색으로 선택되지 않는 섹션은 회색으로 하여 표현  
            btn_kor.Background = Brushes.White;
            btn_eng.Background = Brushes.LightGray;
            btn_chn.Background = Brushes.LightGray;
            btn_jpn.Background = Brushes.LightGray;
        }

        private void btn_eng_Click(object sender, RoutedEventArgs e)
        {
            //선택된 섹션은 흰색으로 선택되지 않는 섹션은 회색으로 하여 표현  
            btn_kor.Background = Brushes.LightGray;
            btn_eng.Background = Brushes.White;
            btn_chn.Background = Brushes.LightGray;
            btn_jpn.Background = Brushes.LightGray;
        }

        private void btn_chn_Click(object sender, RoutedEventArgs e)
        {
            //선택된 섹션은 흰색으로 선택되지 않는 섹션은 회색으로 하여 표현  
            btn_kor.Background = Brushes.LightGray;
            btn_eng.Background = Brushes.LightGray;
            btn_chn.Background = Brushes.White;
            btn_jpn.Background = Brushes.LightGray;
        }

        private void btn_jpn_Click(object sender, RoutedEventArgs e)
        {
            //선택된 섹션은 흰색으로 선택되지 않는 섹션은 회색으로 하여 표현  
            btn_kor.Background = Brushes.LightGray;
            btn_eng.Background = Brushes.LightGray;
            btn_chn.Background = Brushes.LightGray;
            btn_jpn.Background = Brushes.White;
        }


        /* 결제선(리스트뷰) 출력하기 > 메뉴 버튼 클릭 시 해당 product_number를 끌어와서 넣도록 변경할 것 */
        public void LoadListView()
        {
            product_number = "B01HT";
            PaymentInfo.GetInstance().Add(new PaymentInfo()
            {
                ProductNumber = product_number,
                ProductName = dbs.PaymentListLoad(product_number),
                ProductQuantity = 1,
                ProductPrice = Int32.Parse(dbs.DataLoad("Product", "where product_number = \"" + product_number + "\"", "price"))
            });

            product_number = "B02HS";
            PaymentInfo.GetInstance().Add(new PaymentInfo()
            {
                ProductNumber = product_number,
                ProductName = dbs.PaymentListLoad(product_number),
                ProductQuantity = 1,
                ProductPrice = Int32.Parse(dbs.DataLoad("Product", "where product_number = \"" + product_number + "\"", "price"))
            });

            product_number = "B03IT";
            PaymentInfo.GetInstance().Add(new PaymentInfo()
            {
                ProductNumber = product_number,
                ProductName = dbs.PaymentListLoad(product_number),
                ProductQuantity = 1,
                ProductPrice = Int32.Parse(dbs.DataLoad("Product", "where product_number = \"" + product_number + "\"", "price"))
            });

            paymentListView.ItemsSource = PaymentInfo.GetInstance();
        }

        // 수량  -1 버튼
        private void BtnMinus_Click(object sender, RoutedEventArgs e)
        {
            // 현재 선택된 리스트 행 인덱스 찾기
            PaymentInfo pi = PaymentInfo.GetInstance().ElementAt(paymentListView.SelectedIndex);

            if (pi.ProductQuantity > 0)
            {
                pi.ProductQuantity--;   // 현재 리스트의 ProductQuantity--
                product_price = Int32.Parse(dbs.DataLoad("Product", "where product_number = \"" + pi.ProductNumber + "\"", "price"));
                pi.ProductPrice = product_price * pi.ProductQuantity;
                paymentListView.Items.Refresh();
            }
            else
            {
                MessageBox.Show("개수를 1개 이상 입력해주세요.");
            }
        }

        // 수량 +1 버튼
        private void BtnPlus_Click(object sender, RoutedEventArgs e)
        {
            // 현재 선택된 리스트 행 인덱스 찾기
            PaymentInfo pi = PaymentInfo.GetInstance().ElementAt(paymentListView.SelectedIndex);
            pi.ProductQuantity++;   // 현재 리스트의 ProductQuantity++
            product_price = Int32.Parse(dbs.DataLoad("Product", "where product_number = \"" + pi.ProductNumber + "\"", "price"));
            pi.ProductPrice = product_price * pi.ProductQuantity;
            paymentListView.Items.Refresh();
        }

        // 구매 목록 삭제 버튼
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            // 삭제 구현 못함
        }

        // 결제하기 버튼
        private void BtnPay_Click(object sender, RoutedEventArgs e)
        {
            try // 영수증 목록이 있으면
            {
                // 영수증 번호 생성 (마지막 영수증 번호 +1)
                stlm_number = Int32.Parse(dbs.DataLoad("stlm", "order by stlm_number desc limit 1", "stlm_number")) + 1;
            }
            catch (Exception ex)     // 영수증 목록이 없으면
            {
                stlm_number = 1;    // 영수증 번호 : 1
            }

            int count = paymentListView.Items.Count;                        // 현재 리스트뷰의 행 개수
            int sum_price = 0;                                              // 총합
            string payment_list = "";                                       // 구매 목록
            string datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); // 구매 시간

            // 현재 리스트뷰에 있는 행들을 payment_list에 한줄로 넣기
            for (int i = 0; i < count; i++)
            {
                PaymentInfo pi = PaymentInfo.GetInstance().ElementAt(i);

                if (pi.ProductQuantity > 0) // 상품 개수가 0개보다 많은 경우만 계산
                {
                    if (i != count - 1)     // 마지막 행이 아니면 수량 다음 "|" 입력
                    {
                        payment_list += pi.ProductNumber + "|";
                        payment_list += pi.ProductQuantity + "|";
                        sum_price += pi.ProductPrice;
                    }
                    else // 마지막 행이 아니면 수량 다음 "|" 입력X
                    {
                        payment_list += pi.ProductNumber + "|";
                        payment_list += pi.ProductQuantity;
                        sum_price += pi.ProductPrice;
                    }
                }
            }

            MessageBox.Show("<" + stlm_number + "> 영수증" + "\npayment_list : " + payment_list + "\nprice : " + sum_price);

            // 위에서 뽑아낸 값들을 stlm 테이블에 insert
            string query = "insert into stlm(stlm_number, payment_list, sum_price, datetime) values ("
                + stlm_number + ",\"" + payment_list + "\"," + sum_price + ", \"" + datetime + "\")";
            dbs.InsertColumn(query);
            dbs.StlmLoadData(stlm_number.ToString());   // 현재 영수증 테이블 로드
        }
    }

}