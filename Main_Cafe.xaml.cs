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
        public MainWindow()
        {

            InitializeComponent();

            Init();

            Menu_size();
            Menu_btn_add();
            sticker_add();

        }
        // Menu_select() 에서 Menu_count = 2 면  2*2 size
        public int Menu_count = 9;

        //언어 선택을 위한 국가 이미지 
        public void Init()
        {
            try
            {
                DBSqlite dbs = new DBSqlite();
                dbs.SetConnection();

                dbs.ProductLoadData();

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
                RowDefinition rd = new RowDefinition(); ;
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

        public void Menu_btn_add()
        {
           
            

            Button[] btn = new Button[9];

            string[] Name =
                           { "첫번째","두번째" , "3"
                           , "4","5","6" 
                           , "7", "8","9"
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
                    //btn[btn_num].Background = Brushes.White;
                    //btn[btn_num].BorderThickness = new Thickness(0, 0, 0, 0);
                    
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
                    //버튼을 추가한다 
                    Menu.Children.Add(btn[btn_num]);
                   
                    //버튼 이름을 바꿔주기 위함
                    btn_num = btn_num + 1;
               
            }

            btn[0].Background  = new ImageBrush(Kor.Source);
            btn[1].Background = new ImageBrush(Jpn.Source);

           
           
            
        }

        public void sticker_add()
        {
            //스티커 테스트
            TextBlock tb = new TextBlock();
            tb.Text = "New";
            Grid.SetColumn(tb, 0);
            Grid.SetRow(tb, 0);

            Menu.Children.Add(tb);
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
    }
}


