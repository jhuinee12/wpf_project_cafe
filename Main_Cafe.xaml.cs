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
        public static Variable variable = new Variable();
        DBSqlite DB = new DBSqlite(); // db 생성자

        public string[] beverage_name;
        public string[] beverage_sticker;
        public string[] beverage_image;

        public string[] dessert_name;
        public string[] dessert_sticker;
        public string[] dessert_image;

        public string[] etc_name;
        public string[] etc_sticker;
        public string[] etc_image;

        public beverage[] Beverage;
        public dessert[] Dessert;
        public etc[] Etc;

        public string MenuBar = "beverage";

        // sticker_mode = new/hot 이라면  new/hot 스키터 
        //public string sticker_mode = "nomal";
      
        public int Page = 0;
        public int j;
        public int beveragePageCount = 0;  // 배열 위치 count
        public int dessertPageCount = 0;  // 배열 위치 count

        public string[] beverage_number;    // 음료번호 배열
        public string[] dessert_number;    // 음료번호 배열

        Button[] btn = new Button[9];
        Label[] sticker = new Label[9];
        
        public int Previous_counter;

        public MainWindow()
        {
            //DB 데이터(음료이름,스티커여부) 불러오기
            DB.ProductLoadData();

            //데이터(음료,디저트,기타등등 이름,스티커여부)를 옮겨주고 옮긴 이름을 이미지와 매칭시킴
            hand_over_data();

            InitializeComponent();

            //국가 이미지 설정
            Init();

            //동적 그리드 나누기
            Menu_size();

            //동적 버튼과스티커 생성하기
            Menu_btn_add();
        }
        public void hand_over_data()
        {
            Beverage = new beverage[GlobalVar.beverage_counter];
            beverage_name = new string[GlobalVar.beverage_counter];
            beverage_sticker = new string[GlobalVar.beverage_counter];
            beverage_image = new string[GlobalVar.beverage_counter];

            Dessert = new dessert[GlobalVar.dessert_counter];
            dessert_name = new string[GlobalVar.dessert_counter];
            dessert_sticker = new string[GlobalVar.dessert_counter];
            dessert_image = new string[GlobalVar.dessert_counter];

            Etc = new etc[GlobalVar.etc_counter];
            etc_name = new string[GlobalVar.etc_counter];
            etc_sticker = new string[GlobalVar.etc_counter];
            etc_image = new string[GlobalVar.etc_counter];

            //DB 음료 데이터들을 옮겨준다
            for (int i = 0; i < GlobalVar.beverage_counter; i++)
            {
                beverage_name[i] = GlobalVar.BEVERAGE_NAME[i];
                beverage_sticker[i] = GlobalVar.BEVERAGE_STICKER[i];
                beverage_image[i] = GlobalVar.BEVERAGE_IMAGE[i];

                Beverage[i].Name = beverage_name[i];
                Beverage[i].sticker = beverage_sticker[i];
                Beverage[i].img = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + beverage_image[i])));
            }
            //DB 디저트 데이터들을 옮겨준다
            for (int i = 0; i < GlobalVar.dessert_counter; i++)
            {
                dessert_name[i] = GlobalVar.DESSERT_NAME[i];
                dessert_sticker[i] = GlobalVar.DESSERT_STICKER[i];
                dessert_image[i] = GlobalVar.DESSERT_IMAGE[i];

                Dessert[i].Name = dessert_name[i];
                Dessert[i].sticker = dessert_sticker[i];
                Dessert[i].img = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + dessert_image[i])));
            }
            //DB 기타 데이터들을 옮겨준다
            for (int i = 0; i < GlobalVar.etc_counter; i++)
            {
                etc_name[i] = GlobalVar.ETC_NAME[i];
                etc_sticker[i] = GlobalVar.ETC_STICKER[i];
                etc_image[i] = GlobalVar.ETC_IMAGE[i];

                Etc[i].Name = etc_name[i];
                Etc[i].sticker = etc_sticker[i];
                Etc[i].img = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + etc_image[i])));
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
                       
            //pn = new string[DB.ColumnCount()];
            beverage_number = new string[16];
            for (int i = 0; i < beverage_number.Length; i++)
            {
                //beverage_number[i] = DB.DataLoad("product", "where beverage_dessert_etc = \"beverage\" limit " + (i + 1), "product_number");
                beverage_number[i] = DB.DataLoad("product", "where beverage_dessert_etc = \"beverage\" limit " + (i + 1), "product_number");
            }
            dessert_number = new string[16];
            for (int i = 0; i < dessert_number.Length; i++)
            {
                //beverage_number[i] = DB.DataLoad("product", "where beverage_dessert_etc = \"beverage\" limit " + (i + 1), "product_number");
                dessert_number[i] = DB.DataLoad("product", "where beverage_dessert_etc = \"dessert\" limit " + (i + 1), "product_number");
            }
        }
  
    
#region //버튼 이벤트    
        private void OpenSubCafe_Click(object sender, EventArgs e)
        {          
            Button SelectBtn = sender as Button;
            
            GlobalVar.btn_select_img = SelectBtn.Background;
            //해당 버튼에 있는 스티커 다시 뿌려주기        
            for (int i = 0; i < GlobalVar.counter; i++)
            {
                if (MenuBar == "beverage")
                {
                    GlobalVar.counter = GlobalVar.beverage_counter;
                    if ((string)SelectBtn.Content == beverage_name[i])
                    {
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
                else if (MenuBar == "dessert")
                {
                    GlobalVar.counter = GlobalVar.dessert_counter;
                    if ((string)SelectBtn.Content == dessert_name[i])
                    {
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
                else if (MenuBar == "etc")
                {
                    GlobalVar.counter = GlobalVar.etc_counter;
                    if ((string)SelectBtn.Content == etc_name[i])
                    {
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
          

            Sub_cafe sub_cafe = new Sub_cafe(variable);
            sub_cafe.ShowDialog();

            if (variable.btClick == 1)
            {
                LoadListView(beverage_number[beveragePageCount + 0]);
            }
        }
       


            //private void OpenSubCafe_0(object sender, EventArgs e)
            //{
            //    GlobalVar.btn_select_img = btn[0].Background;
            //    GlobalVar.btn_select_sticker_img = sticker[0].Background;

            //    Sub_cafe sub_cafe = new Sub_cafe(variable);
            //    sub_cafe.ShowDialog();

            //    if (variable.btClick == 1)
            //    {
            //        LoadListView(beverage_number[beveragePageCount + 0]);
            //    }
            //}
            //private void OpenSubCafe_1(object sender, EventArgs e)
            //{
            //    GlobalVar.btn_select_img = btn[1].Background;
            //    GlobalVar.btn_select_sticker_img = sticker[1].Background;

            //    Sub_cafe sub_cafe = new Sub_cafe(variable);
            //    sub_cafe.ShowDialog();

            //    if (variable.btClick == 1)
            //    {
            //        LoadListView(beverage_number[beveragePageCount + 1]);
            //    }
            //}
            //private void OpenSubCafe_2(object sender, EventArgs e)
            //{
            //    GlobalVar.btn_select_img = btn[2].Background;
            //    GlobalVar.btn_select_sticker_img = sticker[2].Background;

            //    Sub_cafe sub_cafe = new Sub_cafe(variable);
            //    sub_cafe.ShowDialog();

            //    if (variable.btClick == 1)
            //    {
            //        LoadListView(beverage_number[beveragePageCount + 2]);
            //    }
            //}
            //private void OpenSubCafe_3(object sender, EventArgs e)
            //{
            //    GlobalVar.btn_select_img = btn[3].Background;
            //    GlobalVar.btn_select_sticker_img = sticker[3].Background;

            //    Sub_cafe sub_cafe = new Sub_cafe(variable);
            //    sub_cafe.ShowDialog();

            //    if (variable.btClick == 1)
            //    {
            //        LoadListView(beverage_number[beveragePageCount + 3]);
            //    }
            //}
            //private void OpenSubCafe_4(object sender, EventArgs e)
            //{
            //    GlobalVar.btn_select_img = btn[4].Background;
            //    GlobalVar.btn_select_sticker_img = sticker[4].Background;

            //    Sub_cafe sub_cafe = new Sub_cafe(variable);
            //    sub_cafe.ShowDialog();

            //    if (variable.btClick == 1)
            //    {
            //        LoadListView(beverage_number[beveragePageCount + 4]);
            //    }
            //}
            //private void OpenSubCafe_5(object sender, EventArgs e)
            //{
            //    GlobalVar.btn_select_img = btn[5].Background;
            //    GlobalVar.btn_select_sticker_img = sticker[5].Background;

            //    Sub_cafe sub_cafe = new Sub_cafe(variable);
            //    sub_cafe.ShowDialog();

            //    if (variable.btClick == 1)
            //    {
            //        LoadListView(beverage_number[beveragePageCount + 5]);
            //    }
            //}
            //private void OpenSubCafe_6(object sender, EventArgs e)
            //{
            //    GlobalVar.btn_select_img = btn[6].Background;
            //    GlobalVar.btn_select_sticker_img = sticker[6].Background;

            //    Sub_cafe sub_cafe = new Sub_cafe(variable);
            //    sub_cafe.ShowDialog();

            //    if (variable.btClick == 1)
            //    {
            //        LoadListView(beverage_number[beveragePageCount + 6]);
            //    }
            //}
            //private void OpenSubCafe_7(object sender, EventArgs e)
            //{
            //    GlobalVar.btn_select_img = btn[7].Background;
            //    GlobalVar.btn_select_sticker_img = sticker[7].Background;

            //    Sub_cafe sub_cafe = new Sub_cafe(variable);
            //    sub_cafe.ShowDialog();

            //    if (variable.btClick == 1)
            //    {
            //        LoadListView(beverage_number[beveragePageCount + 7]);
            //    }
            //}
            //private void OpenSubCafe_8(object sender, EventArgs e)
            //{
            //    GlobalVar.btn_select_img = btn[8].Background;
            //    GlobalVar.btn_select_sticker_img = sticker[8].Background;

            //    Sub_cafe sub_cafe = new Sub_cafe(variable);
            //    sub_cafe.ShowDialog();

            //    if (variable.btClick == 1)
            //    {
            //        LoadListView(beverage_number[beveragePageCount + 8]);
            //    }
            //}
            #endregion

            #region //페이지 전환 버튼
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
        public void LoadListView(string pn)
        {
            variable.product_number = pn;
            PaymentInfo.GetInstance().Add(new PaymentInfo()
            {
                ProductNumber = variable.product_number,
                ProductName = DB.PaymentListLoad(variable.product_number),
                ProductQuantity = 1,
                ProductPrice = Int32.Parse(DB.DataLoad("Product", "where product_number = \"" + variable.product_number + "\"", "price"))
            });

            paymentListView.ItemsSource = PaymentInfo.GetInstance();
            paymentListView.Items.Refresh();
        }

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
                variable.product_price = Int32.Parse(DB.DataLoad("Product", "where product_number = \"" + pi.ProductNumber + "\"", "price"));
                pi.ProductPrice = variable.product_price * pi.ProductQuantity;
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
            variable.product_price = Int32.Parse(DB.DataLoad("Product", "where product_number = \"" + pi.ProductNumber + "\"", "price"));
            pi.ProductPrice = variable.product_price * pi.ProductQuantity;
            paymentListView.Items.Refresh();
        }

        // 구매 목록 삭제 버튼
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            Button selectBtn = sender as Button;
            PaymentInfo pi = (PaymentInfo)selectBtn.DataContext;

            for (int i=0; i< paymentListView.Items.Count; i++)
            {
                PaymentInfo pie = PaymentInfo.GetInstance().ElementAt(i);
                if (pi.ProductNumber == pie.ProductNumber)
                {
                    PaymentInfo.GetInstance().RemoveAt(i);
                }
            }
            paymentListView.Items.Refresh();
        }
        #endregion

#region       // 결제하기 버튼
        private void BtnPay_Click(object sender, RoutedEventArgs e)
        {
            variable.count = paymentListView.Items.Count;   // 현재 리스트뷰의 행 개수

            if (variable.count == 0)
            {
                MessageBox.Show("구매 메뉴를 선택해주세요");
            }
            else
            {
                // 현재 리스트뷰에 있는 행들을 payment_list에 한줄로 넣기
                for (int i = 0; i < variable.count; i++)
                {
                    PaymentInfo pi = PaymentInfo.GetInstance().ElementAt(i);

                    if (pi.ProductQuantity > 0) // 상품 개수가 0개보다 많은 경우만 계산
                    {
                        if (i != variable.count - 1)     // 마지막 행이 아니면 수량 다음 "|" 입력
                        {
                            variable.payment_list += pi.ProductNumber + "|";
                            variable.payment_list += pi.ProductQuantity + "|";
                            variable.sum_price += pi.ProductPrice;
                        }
                        else // 마지막 행이 아니면 수량 다음 "|" 입력X
                        {
                            variable.payment_list += pi.ProductNumber + "|";
                            variable.payment_list += pi.ProductQuantity;
                            variable.sum_price += pi.ProductPrice;
                        }
                    }
                }

                PayType pt = new PayType(variable);
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
            var col4 = 0.10;

            gView.Columns[0].Width = workingWidth * col1;
            gView.Columns[1].Width = workingWidth * col2;
            gView.Columns[2].Width = workingWidth * col3;
            gView.Columns[3].Width = workingWidth * col4;
        }
        #endregion
    }

}