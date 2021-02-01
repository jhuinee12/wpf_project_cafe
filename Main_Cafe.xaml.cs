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
        DBSqlite DB = new DBSqlite(); // db 생성자

        public string[] beverage_name;
        public string[] beverage_sticker;
        public string[] beverage_image;
        public beverage[] Beverage;

        public string MenuBar = "beverage";

        public int Page = 0;
        public int j;

        Button[] btn = new Button[9];
        Label[] sticker = new Label[9];

        public int Previous_counter;

        public string product_number = "";
        public int product_price = 0;
        public int stlm_number;

        public string[] dessert_Name =
                     { "초콜릿 크로캉 롱 슈" ,"초콜릿 쉬폰","초콜릿 크렘린"
                           , "클래식 카토 초콜릿","쿠키 앤 크림","생크림 피칸 타르트"
                           , "생크림 소프트 쉬폰", "딸기 케이크","딸기 요거트"
                       };
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

        // Menu_select() 에서 Menu_count = 2 면  2*2 size
        public int Menu_count = 9;

        public MainWindow()
        {
            //DB 데이터(음료이름,스티커여부) 불러오기
            DB.ProductLoadData();

            //데이터(음료이름,스티커여부)를 옮겨주고 옮긴 음료이름을 이미지와 매칭시킴
            hand_over_data();

            InitializeComponent();

            //국가 이미지 설정
            Init();

            //동적 그리드 나누기
            Menu_size();

            //동적 버튼과스티커 생성하기
            Menu_btn_add();



            LoadListView();
        }
        public void hand_over_data()
        {
            Beverage = new beverage[GlobalVar.beverage_counter];
            beverage_name = new string[GlobalVar.beverage_counter];
            beverage_sticker = new string[GlobalVar.beverage_counter];
            beverage_image = new string[GlobalVar.beverage_counter];

            //DB 데이터들을 옮겨준다
            for (int i = 0; i < GlobalVar.beverage_counter; i++)
            {
                beverage_name[i] = GlobalVar.BEVERAGE_NAME[i];
                beverage_sticker[i] = GlobalVar.BEVERAGE_STICKER[i];
                beverage_image[i] = GlobalVar.BEVERAGE_IMAGE[i];


                Beverage[i].Name = beverage_name[i];
                Beverage[i].sticker = beverage_sticker[i];
                Beverage[i].img = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + beverage_image[i])));


            }
            //총 페이지 구하기
            if (GlobalVar.beverage_counter / 9 >= 0 && GlobalVar.beverage_counter % 9 == 0)
            {
                GlobalVar.Beverage_Total_page = GlobalVar.beverage_counter / 9;
            }
            else
            {
                GlobalVar.Beverage_Total_page = GlobalVar.beverage_counter / 9 + 1;
            }


        }


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


            int sticker_num = 0;
            int btn_num = 0;



            int Colum_set = 0;
            int Row_set = 0;


            for (int i = 0; i < 9; i++)
            {
                if (GlobalVar.beverage_counter - 9 * Page > btn_num && GlobalVar.beverage_counter - 9 * Page > sticker_num)
                {

                    //버튼과 스티커를 생성
                    btn[btn_num] = new Button();
                    sticker[sticker_num] = new Label();

                    //버튼 음료(이미지,이름)
                    if (MenuBar == "beverage")
                    {
                        btn[i].Background = Beverage[i + 9 * Page].img;
                        btn[i].Content = Beverage[i + 9 * Page].Name;

                    }

                    // btn[GlobalVar.btn_num].Background = Brushes.White;
                    // btn[GlobalVar.btn_num].BorderThickness = new Thickness(0, 0, 0, 0);

                    //스티커 종류 선택
                    if (Beverage[i + 9 * Page].sticker == "new")
                    {
                        sticker[sticker_num].Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_sticker\new.png")));
                    }
                    else if (Beverage[i + 9 * Page].sticker == "hot")
                    {
                        sticker[sticker_num].Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_sticker\hot.png")));
                    }


                    //버튼의 영역을 3*3으로
                    Grid.SetColumnSpan(btn[btn_num], 3);
                    Grid.SetRowSpan(btn[btn_num], 3);


                    //버튼의 col/row값을 설정
                    Grid.SetColumn(btn[btn_num], Colum_set);
                    Grid.SetRow(btn[btn_num], Row_set);
                    //스티커의 col/row값을 설정
                    Grid.SetColumn(sticker[sticker_num], Colum_set);
                    Grid.SetRow(sticker[sticker_num], Row_set);


                    //버튼의 스타일을 재정의
                    btn[btn_num].FontWeight = FontWeights.Bold;
                    btn[btn_num].Style = FindResource("Button_Style") as Style;


                    //버튼을 추가한다 
                    Menu.Children.Add(btn[btn_num]);
                    Menu.Children.Add(sticker[sticker_num]);
                    //다음 컬럼으로 이동
                    Colum_set = Colum_set + 3;

                    ////끝 컬럼 쪽이라면 로우를 이동시키고 컬럼도 다시 초기화
                    if (Colum_set > 6)
                    {
                        Colum_set = 0;
                        Row_set = Row_set + 3;
                        if (Row_set > 6)
                        {
                            Row_set = 0;

                        }
                    }


                    //다음 버튼 
                    btn_num++;
                    sticker_num++;




                }

            }
            #region           //이벤트 버튼 처리
            if (MenuBar == "beverage")
            {
                if ((Page == 0 && Page == GlobalVar.Beverage_Total_page - 1 && GlobalVar.beverage_counter / 9 == 1 && GlobalVar.beverage_counter % 9 == 0) || //첫페이지자 마지막페이지이고 상품개수가 9개
                    (Page == GlobalVar.Beverage_Total_page - 1 && GlobalVar.beverage_counter / 9 > 0 && GlobalVar.beverage_counter % 9 == 0) ||//다음페이지가 있으면 현재 페이지의 상품개수는 9개
                    (Page >= 0 && Page < GlobalVar.Beverage_Total_page - 1))//마지막 페이지 인데 상품개수가 9개
                {
                    btn[0].Click += new RoutedEventHandler(OpenSubCafe_0);
                    btn[1].Click += new RoutedEventHandler(OpenSubCafe_1);
                    btn[2].Click += new RoutedEventHandler(OpenSubCafe_2);
                    btn[3].Click += new RoutedEventHandler(OpenSubCafe_3);
                    btn[4].Click += new RoutedEventHandler(OpenSubCafe_4);
                    btn[5].Click += new RoutedEventHandler(OpenSubCafe_5);
                    btn[6].Click += new RoutedEventHandler(OpenSubCafe_6);
                    btn[7].Click += new RoutedEventHandler(OpenSubCafe_7);
                    btn[8].Click += new RoutedEventHandler(OpenSubCafe_8);
                }
                else if (Page >= 0 && Page == GlobalVar.Beverage_Total_page - 1 && GlobalVar.beverage_counter / 9 >= 0 && GlobalVar.beverage_counter % 9 < 9 && GlobalVar.beverage_counter % 9 > 0)//(첫페이지자)마지막페이지이고 상품개수가 9개미만
                {
                    if (GlobalVar.beverage_counter % 9 == 1)//page 1개에 상품 1개
                    {
                        btn[0].Click += new RoutedEventHandler(OpenSubCafe_0);
                    }
                    if (GlobalVar.beverage_counter % 9 == 2)//상품 2개
                    {
                        btn[0].Click += new RoutedEventHandler(OpenSubCafe_0);
                        btn[1].Click += new RoutedEventHandler(OpenSubCafe_1);
                    }
                    if (GlobalVar.beverage_counter % 9 == 3)//상품 3개
                    {
                        btn[0].Click += new RoutedEventHandler(OpenSubCafe_0);
                        btn[1].Click += new RoutedEventHandler(OpenSubCafe_1);
                        btn[2].Click += new RoutedEventHandler(OpenSubCafe_2);
                    }
                    if (GlobalVar.beverage_counter % 9 == 4)//상품 4개
                    {
                        btn[0].Click += new RoutedEventHandler(OpenSubCafe_0);
                        btn[1].Click += new RoutedEventHandler(OpenSubCafe_1);
                        btn[2].Click += new RoutedEventHandler(OpenSubCafe_2);
                        btn[3].Click += new RoutedEventHandler(OpenSubCafe_3);
                    }
                    if (GlobalVar.beverage_counter % 9 == 5)//상품 5개
                    {
                        btn[0].Click += new RoutedEventHandler(OpenSubCafe_0);
                        btn[1].Click += new RoutedEventHandler(OpenSubCafe_1);
                        btn[2].Click += new RoutedEventHandler(OpenSubCafe_2);
                        btn[3].Click += new RoutedEventHandler(OpenSubCafe_3);
                        btn[4].Click += new RoutedEventHandler(OpenSubCafe_4);
                    }
                    if (GlobalVar.beverage_counter % 9 == 6)//상품 6개
                    {
                        btn[0].Click += new RoutedEventHandler(OpenSubCafe_0);
                        btn[1].Click += new RoutedEventHandler(OpenSubCafe_1);
                        btn[2].Click += new RoutedEventHandler(OpenSubCafe_2);
                        btn[3].Click += new RoutedEventHandler(OpenSubCafe_3);
                        btn[4].Click += new RoutedEventHandler(OpenSubCafe_4);
                        btn[5].Click += new RoutedEventHandler(OpenSubCafe_5);
                    }
                    if (GlobalVar.beverage_counter % 9 == 7)//상품 7개
                    {
                        btn[0].Click += new RoutedEventHandler(OpenSubCafe_0);
                        btn[1].Click += new RoutedEventHandler(OpenSubCafe_1);
                        btn[2].Click += new RoutedEventHandler(OpenSubCafe_2);
                        btn[3].Click += new RoutedEventHandler(OpenSubCafe_3);
                        btn[4].Click += new RoutedEventHandler(OpenSubCafe_4);
                        btn[5].Click += new RoutedEventHandler(OpenSubCafe_5);
                        btn[6].Click += new RoutedEventHandler(OpenSubCafe_6);
                    }
                    if (GlobalVar.beverage_counter % 9 == 8)//상품 8개
                    {
                        btn[0].Click += new RoutedEventHandler(OpenSubCafe_0);
                        btn[1].Click += new RoutedEventHandler(OpenSubCafe_1);
                        btn[2].Click += new RoutedEventHandler(OpenSubCafe_2);
                        btn[3].Click += new RoutedEventHandler(OpenSubCafe_3);
                        btn[4].Click += new RoutedEventHandler(OpenSubCafe_4);
                        btn[5].Click += new RoutedEventHandler(OpenSubCafe_5);
                        btn[6].Click += new RoutedEventHandler(OpenSubCafe_6);
                        btn[7].Click += new RoutedEventHandler(OpenSubCafe_7);
                    }
                }
                #endregion
                //btn[i].Click += new RoutedEventHandler(OpenSubCafe);
            }

        }
        #region //버튼 이벤트    
        private void OpenSubCafe_0(object sender, EventArgs e)
        {
            GlobalVar.btn_select_img = btn[0].Background;

            Sub_cafe sub_cafe = new Sub_cafe();
            sub_cafe.ShowDialog();
        }
        private void OpenSubCafe_1(object sender, EventArgs e)
        {
            GlobalVar.btn_select_img = btn[1].Background;

            Sub_cafe sub_cafe = new Sub_cafe();
            sub_cafe.ShowDialog();
        }
        private void OpenSubCafe_2(object sender, EventArgs e)
        {
            GlobalVar.btn_select_img = btn[2].Background;

            Sub_cafe sub_cafe = new Sub_cafe();
            sub_cafe.ShowDialog();
        }
        private void OpenSubCafe_3(object sender, EventArgs e)
        {
            GlobalVar.btn_select_img = btn[3].Background;

            Sub_cafe sub_cafe = new Sub_cafe();
            sub_cafe.ShowDialog();
        }
        private void OpenSubCafe_4(object sender, EventArgs e)
        {
            GlobalVar.btn_select_img = btn[4].Background;


            Sub_cafe sub_cafe = new Sub_cafe();
            sub_cafe.ShowDialog();
        }
        private void OpenSubCafe_5(object sender, EventArgs e)
        {
            GlobalVar.btn_select_img = btn[5].Background;

            Sub_cafe sub_cafe = new Sub_cafe();
            sub_cafe.ShowDialog();
        }
        private void OpenSubCafe_6(object sender, EventArgs e)
        {
            GlobalVar.btn_select_img = btn[6].Background;

            Sub_cafe sub_cafe = new Sub_cafe();
            sub_cafe.ShowDialog();
        }
        private void OpenSubCafe_7(object sender, EventArgs e)
        {
            GlobalVar.btn_select_img = btn[7].Background;

            Sub_cafe sub_cafe = new Sub_cafe();
            sub_cafe.ShowDialog();
        }
        private void OpenSubCafe_8(object sender, EventArgs e)
        {
            GlobalVar.btn_select_img = btn[8].Background;

            Sub_cafe sub_cafe = new Sub_cafe();
            sub_cafe.ShowDialog();
        }
        #endregion

        private void btn_Next_Click(object sender, RoutedEventArgs e)
        {   //현재 버튼 개수는 9개고 다음 버튼이 7개 일때 나머지 2개가 끝에 남아있음을 방지
            if (Page < GlobalVar.Beverage_Total_page - 1)
            {
                for (int i = 0; i < 9; i++)
                {

                    btn[i].Visibility = Visibility.Hidden;
                    btn[i].IsEnabled = false;
                    sticker[i].Visibility = Visibility.Hidden;
                    sticker[i].IsEnabled = false;

                }

                Page++;

                Menu_btn_add();
            }
            // MessageBox.Show("다음");                     
        }


        private void btn_Previous_Click(object sender, RoutedEventArgs e)
        {

            //MessageBox.Show("이전");

            //현재 버튼이 7개 이고 이전 버튼 개수는 9개일때 인덱스 오버 방지
            if (Page == GlobalVar.Beverage_Total_page - 1)
            {
                Previous_counter = GlobalVar.beverage_counter - 9 * Page;
            }
            else
            {
                Previous_counter = 9;
            }

            if (Page > 0)
            {
                for (int i = 0; i < Previous_counter; i++)
                {   //이전으로 돌아갈시 현재 버튼이 남는것을 방지
                    btn[i].Visibility = Visibility.Hidden;
                    btn[i].IsEnabled = false;
                    sticker[i].Visibility = Visibility.Hidden;
                    sticker[i].IsEnabled = false;
                }

                Page--;
                Menu_btn_add();
            }
        }

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
                ProductName = DB.PaymentListLoad(product_number),
                ProductQuantity = 1,
                ProductPrice = Int32.Parse(DB.DataLoad("Product", "where product_number = \"" + product_number + "\"", "price"))
            });

            product_number = "B02HS";
            PaymentInfo.GetInstance().Add(new PaymentInfo()
            {
                ProductNumber = product_number,
                ProductName = DB.PaymentListLoad(product_number),
                ProductQuantity = 1,
                ProductPrice = Int32.Parse(DB.DataLoad("Product", "where product_number = \"" + product_number + "\"", "price"))
            });

            product_number = "B03IT";
            PaymentInfo.GetInstance().Add(new PaymentInfo()
            {
                ProductNumber = product_number,
                ProductName = DB.PaymentListLoad(product_number),
                ProductQuantity = 1,
                ProductPrice = Int32.Parse(DB.DataLoad("Product", "where product_number = \"" + product_number + "\"", "price"))
            });

            paymentListView.ItemsSource = PaymentInfo.GetInstance();
        }

        // 수량  -1 버튼
        private void BtnMinus_Click(object sender, RoutedEventArgs e)
        {
            Button selectBtn = sender as Button;
            // 현재 선택된 리스트 행 인덱스 찾기
            PaymentInfo pi = (PaymentInfo)selectBtn.DataContext;

            if (pi.ProductQuantity > 0)
            {
                pi.ProductQuantity--;   // 현재 리스트의 ProductQuantity--
                product_price = Int32.Parse(DB.DataLoad("Product", "where product_number = \"" + pi.ProductNumber + "\"", "price"));
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
            Button selectBtn = sender as Button;

            // 현재 선택된 리스트 행 인덱스 찾기
            PaymentInfo pi = (PaymentInfo)selectBtn.DataContext;

            pi.ProductQuantity++;   // 현재 리스트의 ProductQuantity++
            product_price = Int32.Parse(DB.DataLoad("Product", "where product_number = \"" + pi.ProductNumber + "\"", "price"));
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
                stlm_number = Int32.Parse(DB.DataLoad("stlm", "order by stlm_number desc limit 1", "stlm_number")) + 1;
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
            DB.InsertColumn(query);

            DB.StlmLoadData(stlm_number.ToString());   // 현재 영수증 테이블 로드
        }
    }

}