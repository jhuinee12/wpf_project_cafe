using System;
using System.Collections.Generic;
using System.Globalization;
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
        public string[] beverage_explain;

        public string[] dessert_name;
        public string[] dessert_sticker;
        public string[] dessert_image;
        public string[] dessert_explain;

        public string[] etc_name;
        public string[] etc_sticker;
        public string[] etc_image;
        public string[] etc_explain;

        public beverage[] Beverage;
        public dessert[] Dessert;
        public etc[] Etc;

        public string MenuBar = "beverage";

        // sticker_mode = new/hot 이라면  new/hot 스키터 
        //public string sticker_mode = "nomal";
      
        public int Page = 0;
        public int ListViewPage = 1;
        public string[] dessert_number;    // 음료번호 배열

        Button[] btn = new Button[9];
        Label[] sticker = new Label[9];
        
        public int Previous_counter;
        
        public int cnt;//다국어선택에서 사용

        public List<PaymentInfo> DataList = new List<PaymentInfo>();

        public MainWindow()
        {
           
            //DB 데이터(음료이름,스티커여부) 불러오기
            DB.ProductLoadData();

            //데이터(음료,디저트,기타등등 이름,스티커여부)를 옮겨주고 옮긴 이름을 이미지와 매칭시킴
            hand_over_data();

            InitializeComponent();
            
            //페이지 전환버튼 스타일 변경
            page_move_btn_style();
            
            //국가 이미지 설정
            Init();

            //동적 그리드 나누기
            Menu_size();

            //동적 버튼과스티커 생성하기
            Menu_btn_add();

            ListView();
        }
       
        public void hand_over_data()
        {
            Beverage = new beverage[GlobalVar.beverage_counter];
            beverage_name = new string[GlobalVar.beverage_counter];
            beverage_sticker = new string[GlobalVar.beverage_counter];
            beverage_image = new string[GlobalVar.beverage_counter];
            beverage_explain = new string[GlobalVar.beverage_counter];

            Dessert = new dessert[GlobalVar.dessert_counter];
            dessert_name = new string[GlobalVar.dessert_counter];
            dessert_sticker = new string[GlobalVar.dessert_counter];
            dessert_image = new string[GlobalVar.dessert_counter];
            dessert_explain = new string[GlobalVar.dessert_counter];

            Etc = new etc[GlobalVar.etc_counter];
            etc_name = new string[GlobalVar.etc_counter];
            etc_sticker = new string[GlobalVar.etc_counter];
            etc_image = new string[GlobalVar.etc_counter];
            etc_explain = new string[GlobalVar.etc_counter];

            //DB 음료 데이터들을 옮겨준다
            for (int i = 0; i < GlobalVar.beverage_counter; i++)
            {
                beverage_name[i] = GlobalVar.BEVERAGE_NAME[i];
                beverage_sticker[i] = GlobalVar.BEVERAGE_STICKER[i];
                beverage_image[i] = GlobalVar.BEVERAGE_IMAGE[i];
                beverage_explain[i] = GlobalVar.BEVERAGE_EXPLAIN[i];

                Beverage[i].Name = beverage_name[i];
                Beverage[i].sticker = beverage_sticker[i];
                Beverage[i].img = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + beverage_image[i])));
                Beverage[i].explain = beverage_explain[i];



            }
            //DB 디저트 데이터들을 옮겨준다
            for (int i = 0; i < GlobalVar.dessert_counter; i++)
            {
                dessert_name[i] = GlobalVar.DESSERT_NAME[i];
                dessert_sticker[i] = GlobalVar.DESSERT_STICKER[i];
                dessert_image[i] = GlobalVar.DESSERT_IMAGE[i];
                dessert_explain[i] = GlobalVar.DESSERT_EXPLAIN[i];

                Dessert[i].Name = dessert_name[i];
                Dessert[i].sticker = dessert_sticker[i];
                Dessert[i].img = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + dessert_image[i])));
                Dessert[i].explain = dessert_explain[i];
            }
            //DB 기타 데이터들을 옮겨준다
            for (int i = 0; i < GlobalVar.etc_counter; i++)
            {
                etc_name[i] = GlobalVar.ETC_NAME[i];
                etc_sticker[i] = GlobalVar.ETC_STICKER[i];
                etc_image[i] = GlobalVar.ETC_IMAGE[i];
                etc_explain[i] = GlobalVar.ETC_EXPLAIN[i];

                Etc[i].Name = etc_name[i];
                Etc[i].sticker = etc_sticker[i];
                Etc[i].img = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + etc_image[i])));
                Etc[i].explain = etc_explain[i];
            }

            //음료 총 페이지 구하기
            if (GlobalVar.beverage_counter / 9 >= 0 && GlobalVar.beverage_counter % 9 == 0)
            {
                GlobalVar.Beverage_Total_page = GlobalVar.beverage_counter / 9;
            }
            else
            {
                GlobalVar.Beverage_Total_page = GlobalVar.beverage_counter / 9 + 1;
            }
            //디저트 총 페이지 구하기
            if (GlobalVar.dessert_counter / 9 >= 0 && GlobalVar.dessert_counter % 9 == 0)
            {
                GlobalVar.Dessert_Total_page = GlobalVar.dessert_counter / 9;
            }
            else
            {
                GlobalVar.Dessert_Total_page = GlobalVar.dessert_counter / 9 + 1;
            }
            //기타등등 총 페이지 구하기
            if (GlobalVar.etc_counter / 9 >= 0 && GlobalVar.etc_counter % 9 == 0)
            {
                GlobalVar.Etc_Total_page = GlobalVar.etc_counter / 9;
            }
            else
            {
                GlobalVar.Etc_Total_page = GlobalVar.etc_counter / 9 + 1;
            }
        }

        // Menu_select() 에서 Menu_count = 2 면  2*2 size
        public int Menu_count = 9;

        //언어 선택을 위한 국가 이미지 
        public void Init()
        {
            try
            {
                paymentListView.Items.Refresh();    // 리스트뷰 새로고침

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
            
            if (MenuBar == "beverage")
            {
                GlobalVar.counter = GlobalVar.beverage_counter;
            }
            else if (MenuBar == "dessert")
            {
                GlobalVar.counter = GlobalVar.dessert_counter;
            }
            else if (MenuBar == "etc")
            {
                GlobalVar.counter = GlobalVar.etc_counter;
            }

            for (int i = 0; i < 9; i++)
            {
               

                if (GlobalVar.counter - 9 * Page > btn_num && GlobalVar.counter - 9 * Page > sticker_num)
                {
                    //버튼과 스티커를 생성
                    btn[btn_num] = new Button();
                    sticker[sticker_num] = new Label();

                    //버튼 음료(이미지,이름)
                    if (MenuBar == "beverage")
                    {
                        btn[i].Background = Beverage[i + 9 * Page].img;
                        btn[i].Content = Beverage[i + 9 * Page].Name;

                        //스티커 종류 선택
                        if (Beverage[i + 9 * Page].sticker == "new")
                        {
                            sticker[sticker_num].Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_sticker\new.png")));
                        }
                        else if (Beverage[i + 9 * Page].sticker == "hot")
                        {
                            sticker[sticker_num].Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_sticker\hot.png")));
                        }

                        GlobalVar.Total_page = GlobalVar.Beverage_Total_page;

                    } //버튼 디저트(이미지,이름)
                    else if (MenuBar == "dessert")
                    {
                        
                        btn[i].Background = Dessert[i + 9 * Page].img;
                        btn[i].Content = Dessert[i + 9 * Page].Name;
                        
                       
                        //스티커 종류 선택
                        if (Dessert[i + 9 * Page].sticker == "new")
                        {
                            sticker[sticker_num].Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_sticker\new.png")));
                        }
                        else if (Dessert[i + 9 * Page].sticker == "hot")
                        {
                            sticker[sticker_num].Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_sticker\hot.png")));
                        }

                        GlobalVar.Total_page = GlobalVar.Dessert_Total_page;
                    } //버튼 기타(이미지,이름)
                    else if (MenuBar == "etc")
                    {
                        btn[i].Background = Etc[i + 9 * Page].img;
                        btn[i].Content = Etc[i + 9 * Page].Name;

                        //스티커 종류 선택
                        if (Etc[i + 9 * Page].sticker == "new")
                        {
                            sticker[sticker_num].Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_sticker\new.png")));
                        }
                        else if (Etc[i + 9 * Page].sticker == "hot")
                        {
                            sticker[sticker_num].Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_sticker\hot.png")));
                        }

                        GlobalVar.Total_page = GlobalVar.Etc_Total_page;
                    }

                    // btn[GlobalVar.btn_num].Background = Brushes.White;
                    // btn[GlobalVar.btn_num].BorderThickness = new Thickness(0, 0, 0, 0);

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
                    //이벤트 버튼 처리
                    btn[i].Click += new RoutedEventHandler(OpenSubCafe_Click);
                    
                }
            }
        }
  
    
#region //버튼 이벤트    
        private void OpenSubCafe_Click(object sender, EventArgs e)
        {          
            Button SelectBtn = sender as Button;
            
            GlobalVar.btn_select_img = SelectBtn.Background;
            GlobalVar.btn_select_explain_img = null;

            // 클릭한 버튼의 제품번호
            if (GlobalVar.Language == "Kor_name")
            {
                GlobalVar.product_number = DB.DataLoad("product", "where Kor_name = \'" + SelectBtn.Content + "\'", "product_number");
            }
            else if (GlobalVar.Language == "Eng_name")
            {
                GlobalVar.product_number = DB.DataLoad("product", "where Eng_name = \'" + SelectBtn.Content + "\'", "product_number");
            }


            if (MenuBar == "beverage")
            {
                GlobalVar.counter = GlobalVar.beverage_counter;
               
                for (int i = 0; i < GlobalVar.counter; i++)
                {
                    
                    if ((string)SelectBtn.Content == beverage_name[i])
                    {
                        //해당 이름 뿌려주기
                        GlobalVar.select_name = beverage_name[i];

                        //해당 버튼의 설명이 비어있으면 이미지가 안나옴
                        if (Beverage[i].explain != "none")
                        {
                            GlobalVar.btn_select_explain_img = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + Beverage[i].explain)));
                        }
                        else if (Beverage[i].explain == "none")
                        {
                            GlobalVar.btn_select_explain_img = null;
                        }

                        //해당 버튼에 있는 스티커 다시 뿌려주기      
                        if (Beverage[i].sticker == "new")
                        {
                            GlobalVar.btn_select_sticker_img = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_sticker\new.png")));
                        }
                        else if (Beverage[i].sticker == "hot")
                        {
                            GlobalVar.btn_select_sticker_img = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_sticker\hot.png")));
                        }
                        else
                        {
                            GlobalVar.btn_select_sticker_img = null;
                        }

                    }
                }
            }
            else if (MenuBar == "dessert")
            {
                GlobalVar.counter = GlobalVar.dessert_counter;
               
                for (int i = 0; i < GlobalVar.counter; i++)
                {
                   
                    if ((string)SelectBtn.Content == dessert_name[i])
                    {
                        //해당 이름 뿌려주기
                        GlobalVar.select_name = dessert_name[i];

                        //해당 버튼의 설명이 비어있으면 이미지가 안나옴
                        if (Dessert[i].explain != "none")
                        {
                            GlobalVar.btn_select_explain_img = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + Dessert[i].explain)));
                        }                      
                        else if (Dessert[i].explain == "none")
                        {
                            GlobalVar.btn_select_explain_img = null;
                        }
                        //해당 버튼에 있는 스티커 다시 뿌려주기      
                        if (Dessert[i].sticker == "new")
                        {
                            GlobalVar.btn_select_sticker_img = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_sticker\new.png")));
                        }
                        else if (Dessert[i].sticker == "hot")
                        {
                            GlobalVar.btn_select_sticker_img = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_sticker\hot.png")));
                        }
                        else
                        {
                            GlobalVar.btn_select_sticker_img = null;
                        }

                    }
                }
            }
            else if (MenuBar == "etc")
            {
                GlobalVar.counter = GlobalVar.etc_counter;

                for (int i = 0; i < GlobalVar.counter; i++)
                {

                    if ((string)SelectBtn.Content == etc_name[i])
                    {
                        //해당 이름 뿌려주기
                        GlobalVar.select_name = etc_name[i];

                        //해당 버튼의 설명이 비어있으면 이미지가 안나옴
                        if (Etc[i].explain != "none")
                        {
                            GlobalVar.btn_select_explain_img = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + Etc[i].explain)));
                        }
                        else if (Etc[i].explain == "none")
                        {
                            GlobalVar.btn_select_explain_img = null;
                        }
                        //해당 버튼에 있는 스티커 다시 뿌려주기   
                        if (Etc[i].sticker == "new")
                        {
                            GlobalVar.btn_select_sticker_img = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_sticker\new.png")));
                        }
                        else if (Etc[i].sticker == "hot")
                        {
                            GlobalVar.btn_select_sticker_img = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_sticker\hot.png")));
                        }
                        else
                        {
                            GlobalVar.btn_select_sticker_img = null;
                        }

                    }
                }
            }

  

            Sub_cafe sub_cafe = new Sub_cafe();
            sub_cafe.ShowDialog();

            

            if (GlobalVar.btClick == 1)
            {
                LoadListView(GlobalVar.product_number);
            }
        }

        #endregion

#region //페이지 전환 버튼
        //페이지 전환버튼 스타일 변경
        public void page_move_btn_style()
        {
            btn_Previous.Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_btn\Previous.png")));
            btn_Next.Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_btn\Next.png")));
            btn_listView_Previous.Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_btn\Previous.png")));
            btn_listView_Next.Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_btn\Next.png")));
            btn_Pay.Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_btn\payment.png")));
            //listbtn_Style 재사용
            btn_Previous.Style = FindResource("listBtn_Style") as Style;
            btn_Next.Style = FindResource("listBtn_Style") as Style;
            btn_listView_Previous.Style = FindResource("listBtn_Style") as Style;
            btn_listView_Next.Style = FindResource("listBtn_Style") as Style;
            btn_Pay.Style = FindResource("listBtn_Style") as Style;
        }
        private void btn_Next_Click(object sender, RoutedEventArgs e)
        {
            //현재 버튼 개수는 9개고 다음 버튼이 7개 일때 나머지 2개가 끝에 남아있음을 방지
            if (Page < GlobalVar.Total_page - 1)
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
        }

        private void btn_Previous_Click(object sender, RoutedEventArgs e)
        {
            //현재 버튼이 7개 이고 이전 버튼 개수는 9개일때 인덱스 오버 방지
            if (Page == GlobalVar.Total_page - 1)
            {
                Previous_counter = GlobalVar.counter - 9 * Page;
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
#endregion

#region //상단 메뉴바 이동버튼
        private void btn_beverage_Click(object sender, RoutedEventArgs e)
        {
            //선택된 섹션은 흰색으로 선택되지 않는 섹션은 회색으로 하여 표현  
            btn_beverage.Background = Brushes.White;
            btn_dessert.Background = Brushes.LightGray;
            btn_etc.Background = Brushes.LightGray;

            if (MenuBar == "beverage")
            {
                GlobalVar.counter = GlobalVar.beverage_counter;
            }
            else if (MenuBar == "dessert")
            {
                GlobalVar.counter = GlobalVar.dessert_counter;
            }
            else if (MenuBar == "etc")
            {
                GlobalVar.counter = GlobalVar.etc_counter;
            }

            //현재 버튼이 7개 이고 이전 버튼 개수는 9개일때 인덱스 오버 방지
            if (Page == GlobalVar.Total_page - 1)
            {
                Previous_counter = GlobalVar.counter - 9 * Page;
            }
            else
            {
                Previous_counter = 9;
            }
            //메뉴바 이동시 이전 버튼이 남는것을 방지
            for (int i = 0; i < Previous_counter; i++)
            {
                btn[i].Visibility = Visibility.Hidden;
                btn[i].IsEnabled = false;
                sticker[i].Visibility = Visibility.Hidden;
                sticker[i].IsEnabled = false;
            }
            
           
            //메뉴바 선택
            MenuBar = "beverage";

            //MenuBar에 해당 되는 버튼 생성
            Menu_btn_add();
        }

        private void btn_dessert_Click(object sender, RoutedEventArgs e)
        {
            //선택된 섹션은 흰색으로 선택되지 않는 섹션은 회색으로 하여 표현  
            btn_beverage.Background = Brushes.LightGray;
            btn_dessert.Background = Brushes.White;
            btn_etc.Background = Brushes.LightGray; ;

            //메뉴바 이동시 이전 버튼이 남는것을 방지(인덱스 오버 처리해야함)
            if (MenuBar == "beverage")
            {
                GlobalVar.counter = GlobalVar.beverage_counter;
            }
            else if (MenuBar == "dessert")
            {
                GlobalVar.counter = GlobalVar.dessert_counter;
            }
            else if (MenuBar == "etc")
            {
                GlobalVar.counter = GlobalVar.etc_counter;
            }

            //현재 버튼이 7개 이고 이전 버튼 개수는 9개일때 인덱스 오버 방지
            if (Page == GlobalVar.Total_page - 1)
            {
                Previous_counter = GlobalVar.counter - 9 * Page;
            }
            else
            {
                Previous_counter = 9;
            }

            for (int i = 0; i < Previous_counter; i++)
            {
                btn[i].Visibility = Visibility.Hidden;
                btn[i].IsEnabled = false;
                sticker[i].Visibility = Visibility.Hidden;
                sticker[i].IsEnabled = false;
            }
          
            //메뉴바 선택
            MenuBar = "dessert";

            Page = 0;
            //MenuBar에 해당 되는 버튼 생성
            Menu_btn_add();
        }

        private void btn_etc_Click(object sender, RoutedEventArgs e)
        {
            //선택된 섹션은 흰색으로 선택되지 않는 섹션은 회색으로 하여 표현  
            btn_beverage.Background = Brushes.LightGray;
            btn_dessert.Background = Brushes.LightGray;
            btn_etc.Background = Brushes.White;
            //메뉴바 이동시 이전 버튼이 남는것을 방지(인덱스 오버 처리해야함)
            if (MenuBar == "beverage")
            {
                GlobalVar.counter = GlobalVar.beverage_counter;
            }
            else if (MenuBar == "dessert")
            {
                GlobalVar.counter = GlobalVar.dessert_counter;
            }
            else if (MenuBar == "etc")
            {
                GlobalVar.counter = GlobalVar.etc_counter;
            }

            //현재 버튼이 7개 이고 이전 버튼 개수는 9개일때 인덱스 오버 방지
            if (Page == GlobalVar.Total_page - 1)
            {
                Previous_counter = GlobalVar.counter - 9 * Page;
            }
            else
            {
                Previous_counter = 9;
            }
            for (int i = 0; i < Previous_counter; i++)
            {
                btn[i].Visibility = Visibility.Hidden;
                btn[i].IsEnabled = false;
                sticker[i].Visibility = Visibility.Hidden;
                sticker[i].IsEnabled = false;
            }
            
          
            
            //메뉴바 선택
            MenuBar = "etc";

            Page = 0;
            //MenuBar에 해당 되는 버튼 생성
            Menu_btn_add();
        }
#endregion

        private void btn_kor_Click(object sender, RoutedEventArgs e)
        {
            GlobalVar.Language = "Kor_name";
            
            //이전 언어로 된것들을 안보이게
            if (GlobalVar.counter > 9)
            {
                cnt = 9;
            }
            for (int i = 0; i < cnt; i++)
            {
                btn[i].Content = null;
                btn[i].Visibility = Visibility.Hidden;
                btn[i].IsEnabled = false;
                sticker[i].Visibility = Visibility.Hidden;
                sticker[i].IsEnabled = false;
            }
            
            //DB 데이터(음료이름,스티커여부) 불러오기
            DB.ProductLoadData();
           
            //데이터(음료,디저트,기타등등 이름,스티커여부)를 옮겨주고 옮긴 이름을 이미지와 매칭시킴
            hand_over_data();

            //동적 버튼과스티커 생성하기
            Menu_btn_add();

            //선택된 섹션은 흰색으로 선택되지 않는 섹션은 회색으로 하여 표현  
            btn_kor.Background = Brushes.White;
            btn_eng.Background = Brushes.LightGray;
            btn_chn.Background = Brushes.LightGray;
            btn_jpn.Background = Brushes.LightGray;
            //다국어선택시 메뉴바 이름설정
            btn_beverage.Content = "음료";
            btn_dessert.Content = "디저트";
            btn_etc.Content = "기타";
        }

        private void btn_eng_Click(object sender, RoutedEventArgs e)
        {
            GlobalVar.Language = "Eng_name";
           
            //이전 언어로 된것들을 안보이게
            if (GlobalVar.counter > 9)
            {
                cnt = 9;
            }
            for (int i = 0; i < cnt; i++)
            {
                btn[i].Content = null;
                btn[i].Visibility = Visibility.Hidden;
                btn[i].IsEnabled = false;
                sticker[i].Visibility = Visibility.Hidden;
                sticker[i].IsEnabled = false;
            }

            //DB 데이터(음료이름,스티커여부) 불러오기
            DB.ProductLoadData();
            
            //데이터(음료,디저트,기타등등 이름,스티커여부)를 옮겨주고 옮긴 이름을 이미지와 매칭시킴
            hand_over_data();

            //동적 버튼과스티커 생성하기
            Menu_btn_add();

            //선택된 섹션은 흰색으로 선택되지 않는 섹션은 회색으로 하여 표현  
            btn_kor.Background = Brushes.LightGray;
            btn_eng.Background = Brushes.White;
            btn_chn.Background = Brushes.LightGray;
            btn_jpn.Background = Brushes.LightGray;
            //다국어선택시 메뉴바 이름설정
            btn_beverage.Content = "Beverage";
            btn_dessert.Content = "Dessert";
            btn_etc.Content = "Etc";
        }

        private void btn_chn_Click(object sender, RoutedEventArgs e)
        {
            GlobalVar.Language = "Chn_name";
            //선택된 섹션은 흰색으로 선택되지 않는 섹션은 회색으로 하여 표현  
            btn_kor.Background = Brushes.LightGray;
            btn_eng.Background = Brushes.LightGray;
            btn_chn.Background = Brushes.White;
            btn_jpn.Background = Brushes.LightGray;
        }

        private void btn_jpn_Click(object sender, RoutedEventArgs e)
        {
            GlobalVar.Language = "Jpn_name";
            //선택된 섹션은 흰색으로 선택되지 않는 섹션은 회색으로 하여 표현  
            btn_kor.Background = Brushes.LightGray;
            btn_eng.Background = Brushes.LightGray;
            btn_chn.Background = Brushes.LightGray;
            btn_jpn.Background = Brushes.White;
        }

        public void ListView()
        {
            if (PaymentInfo.GetInstance() != null)
            {
                int totalPage = 0;

                if (PaymentInfo.GetInstance().Count % 5 != 0)
                {
                    totalPage = PaymentInfo.GetInstance().Count / 5 + 1;
                }
                else
                {
                    totalPage = PaymentInfo.GetInstance().Count / 5;
                }

                DataList.Clear();
                if (5 * ListViewPage < PaymentInfo.GetInstance().Count)
                {
                    for (int i = 5 * ListViewPage - 5; i < 5 * ListViewPage; i++)
                    {
                        DataList.Add(PaymentInfo.GetInstance().ElementAt(i));
                    }
                }
                else
                {
                    for (int i = 5 * ListViewPage - 5; i < PaymentInfo.GetInstance().Count; i++)
                    {
                        DataList.Add(PaymentInfo.GetInstance().ElementAt(i));
                    }
                }

                paymentListView.ItemsSource = DataList;
            }
            paymentListView.Items.Refresh();
        }
        public void LoadListView(string pn)
        {
            int totalPage = 0;

            GlobalVar.product_number = pn;
            GlobalVar.product_price += int.Parse(DB.DataLoad("Product", "where product_number = \"" + GlobalVar.product_number + "\"", "price"));

            PaymentInfo.GetInstance().Add(new PaymentInfo()
            {
                ProductNumber = GlobalVar.product_number,
                ProductName = DB.PaymentListLoad(GlobalVar.product_number),
                ProductQuantity = 1,
                ProductPrice = String.Format("{0:#,0}", GlobalVar.product_price),
                ProductOption = GlobalVar.beverage_Option
            });

            if (PaymentInfo.GetInstance().Count % 5 != 0)
            {
                totalPage = PaymentInfo.GetInstance().Count / 5 + 1;
                if (PaymentInfo.GetInstance().Count!=1 && PaymentInfo.GetInstance().Count % 5 == 1)
                    ListViewPage++;
            }
            else
            {
                totalPage = PaymentInfo.GetInstance().Count / 5;
            }

            DataList.Clear();
            if (5 * ListViewPage < PaymentInfo.GetInstance().Count)
            {
                for (int i = 5 * ListViewPage - 5; i < 5 * ListViewPage; i++)
                {
                    DataList.Add(PaymentInfo.GetInstance().ElementAt(i));
                }
            }
            else
            {
                for (int i = 5 * ListViewPage - 5; i < PaymentInfo.GetInstance().Count; i++)
                {
                    DataList.Add(PaymentInfo.GetInstance().ElementAt(i));
                }
            }

            paymentListView.ItemsSource = DataList;
            paymentListView.Items.Refresh();
        }

#region       // 리스트뷰 페이지 previous
        private void btn_listView_Previous_Click(object sender, RoutedEventArgs e)
        {
            DataList.Clear();
            int totalPage = 0;

            if (PaymentInfo.GetInstance().Count % 5 != 0)
            {
                totalPage = PaymentInfo.GetInstance().Count / 5 + 1;
            }
            else
            {
                totalPage = PaymentInfo.GetInstance().Count / 5;
            }

            if (ListViewPage > 1)
            {
                ListViewPage--;
                DataList.Clear();
                if (5 * ListViewPage < PaymentInfo.GetInstance().Count)
                {
                    for (int i = 5 * ListViewPage - 5; i < 5 * ListViewPage; i++)
                    {
                        DataList.Add(PaymentInfo.GetInstance().ElementAt(i));
                    }
                }
                else
                {
                    for (int i = 5 * ListViewPage - 5; i < PaymentInfo.GetInstance().Count; i++)
                    {
                        DataList.Add(PaymentInfo.GetInstance().ElementAt(i));
                    }
                }

                paymentListView.ItemsSource = DataList;
                paymentListView.Items.Refresh();
            }
        }
        #endregion


#region       // 리스트뷰 페이지 next
        private void btn_listView_Next_Click(object sender, RoutedEventArgs e)
        {
            int totalPage = 0;

            if (PaymentInfo.GetInstance().Count % 5 != 0)
            {
                totalPage = PaymentInfo.GetInstance().Count / 5 + 1;
            }
            else
            {
                totalPage = PaymentInfo.GetInstance().Count / 5;
            }

            if (ListViewPage < totalPage)
            {
                ListViewPage++;
                DataList.Clear();
                if (5*ListViewPage < PaymentInfo.GetInstance().Count)
                {
                    for (int i = 5 * ListViewPage - 5; i < 5 * ListViewPage; i++)
                    {
                        DataList.Add(PaymentInfo.GetInstance().ElementAt(i));
                    }
                }
                else
                {
                    for (int i = 5 * ListViewPage - 5; i < PaymentInfo.GetInstance().Count; i++)
                    {
                        DataList.Add(PaymentInfo.GetInstance().ElementAt(i));
                    }
                }

                paymentListView.ItemsSource = DataList;
                paymentListView.Items.Refresh();
            }
        }
        #endregion

        #region     // 리스트뷰 버튼 클릭 이벤트       
        // 수량  -1 버튼
        private void BtnMinus_Click(object sender, RoutedEventArgs e)
        {
            Button selectBtn = sender as Button;
            // 현재 선택된 리스트 행 인덱스 찾기
            PaymentInfo pi = (PaymentInfo)selectBtn.DataContext;

            if (pi.ProductQuantity > 1)
            {
                pi.ProductQuantity--;   // 현재 리스트의 ProductQuantity--
                pi.ProductPrice = String.Format("{0:#,0}", int.Parse(pi.ProductPrice, NumberStyles.AllowThousands) /(pi.ProductQuantity+1)*pi.ProductQuantity);
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
            pi.ProductPrice = String.Format("{0:#,0}", int.Parse(pi.ProductPrice, NumberStyles.AllowThousands) / (pi.ProductQuantity - 1) * pi.ProductQuantity);
            paymentListView.Items.Refresh();
        }

        // 구매 목록 삭제 버튼
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            Button selectBtn = sender as Button;
            PaymentInfo pi = (PaymentInfo)selectBtn.DataContext;

            for (int i=0; i< PaymentInfo.GetInstance().Count; i++)
            {
                PaymentInfo pie = PaymentInfo.GetInstance().ElementAt(i);
                if (pi.ProductNumber == pie.ProductNumber && pi.ProductOption == pie.ProductOption)
                {
                    PaymentInfo.GetInstance().RemoveAt(i);
                }
            }
            ListView();
        }
        #endregion

#region       // 결제하기 버튼
        private void BtnPay_Click(object sender, RoutedEventArgs e)
        {
            GlobalVar.count = PaymentInfo.GetInstance().Count;   // 현재 리스트뷰의 행 개수

            if (GlobalVar.count == 0)
            {
                MessageBox.Show("구매 메뉴를 선택해주세요");
            }
            else
            {
                // 현재 리스트뷰에 있는 행들을 payment_list에 한줄로 넣기
                for (int i = 0; i < GlobalVar.count; i++)
                {
                    PaymentInfo pi = PaymentInfo.GetInstance().ElementAt(i);

                    if (pi.ProductQuantity > 0) // 상품 개수가 0개보다 많은 경우만 계산
                    {
                        if (i != GlobalVar.count - 1)     // 마지막 행이 아니면 수량 다음 "|" 입력
                        {
                            GlobalVar.payment_list += pi.ProductNumber + "|";
                            GlobalVar.payment_list += pi.ProductQuantity + "|";
                            GlobalVar.payment_list += pi.ProductPrice + "|";
                            GlobalVar.payment_list += pi.ProductOption + "|";
                            GlobalVar.sum_price += int.Parse(pi.ProductPrice, NumberStyles.AllowThousands);
                        }
                        else // 마지막 행이 아니면 수량 다음 "|" 입력X
                        {
                            GlobalVar.payment_list += pi.ProductNumber + "|";
                            GlobalVar.payment_list += pi.ProductQuantity + "|";
                            GlobalVar.payment_list += pi.ProductPrice + "|";
                            GlobalVar.payment_list += pi.ProductOption;
                            GlobalVar.sum_price += int.Parse(pi.ProductPrice, NumberStyles.AllowThousands);
                        }
                    }
                }

                PayType pt = new PayType();
                this.Close();
                pt.ShowDialog();
            }
        }
        #endregion


        #region // 리스트뷰 컬럼 사이즈 조정
        private void paymentListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListView listView = sender as ListView;
            GridView gView = listView.View as GridView;

            var workingWidth = listView.ActualWidth - SystemParameters.VerticalScrollBarWidth;
            var col1 = 0.40;
            var col2 = 0.25;
            var col3 = 0.25;
            var col4 = 0.13;

            gView.Columns[0].Width = workingWidth * col1;
            gView.Columns[1].Width = workingWidth * col2;
            gView.Columns[2].Width = workingWidth * col3;
            gView.Columns[3].Width = workingWidth * col4;
        }
        #endregion
    }

}