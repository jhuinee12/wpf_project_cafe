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
        }

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
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
       

       
        
    }
}
