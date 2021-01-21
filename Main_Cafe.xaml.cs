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

        Dictionary<string, int> payment_list = new Dictionary<string, int>();

        public string product_number = "1";
        public string product_name = "";
        public int product_quantity = 1;
        public int product_price = 0;

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
                dbs.StlmLoadData("1",0); // StlmLoadData 불러오기

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
                    Grid.SetColumnSpan(btn[btn_num], 3 );
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
        public void sticker_add(int set_sticker_col, int set_sticker_row, int set_sticker_col2, int set_sticker_row2, int set_sticker_col3,int set_sticker_row3)
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


        /* 결제선(리스트뷰) */
        public void LoadListView()
        {
            payment_list.Add(product_number, 1);

            dbs.PaymentListLoad(product_number);
            product_name = dbs.pdata;
            dbs.DataLoad("Product", "where product_number = " + product_number, "price");

            product_price = Int32.Parse(dbs.pdata);

            PaymentInfo.GetInstance().Add(new PaymentInfo()
            {
                ProductName = product_name,
                ProductQuantity = product_quantity.ToString(),
                ProductPrice = (product_quantity * product_price).ToString()
            });

            paymentListView.ItemsSource = PaymentInfo.GetInstance();
        }

        private static PaymentInfo ListView_GetItem(RoutedEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while (!(dep is System.Windows.Controls.ListViewItem))
            {
                try
                {
                    dep = VisualTreeHelper.GetParent(dep);
                }
                catch
                {
                    return null;
                }
            }

            ListViewItem item = (ListViewItem)dep;
            PaymentInfo content = (PaymentInfo)item.Content;

            return content;
        }

        // 수량  -1
        private void BtnMinus_Click(object sender, RoutedEventArgs e)
        {
            product_quantity--;

            PaymentInfo pi = PaymentInfo.GetInstance().ElementAt(0);
            pi.ProductQuantity = (product_quantity).ToString();
            pi.ProductPrice = (product_quantity * product_price).ToString();
            paymentListView.Items.Refresh();
        }

        // 수량 +1
        private void BtnPlus_Click(object sender, RoutedEventArgs e)
        {
            product_quantity++;

            PaymentInfo pi = PaymentInfo.GetInstance().ElementAt(0);
            pi.ProductQuantity = product_quantity.ToString();
            pi.ProductPrice = (product_quantity * product_price).ToString();
            paymentListView.Items.Refresh();
        }

        // 구매 목록 삭제 버튼
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            // 버튼 위치에서 수정하고 싶어요...
            // paymentListView.Items.Remove(paymentListView.SelectedItem);
        }
    }

}


