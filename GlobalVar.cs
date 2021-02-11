using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPF_project_Cafe
{
    class GlobalVar
    {
        public static string[] BEVERAGE_NAME = new string[10000];
        public static string[] BEVERAGE_STICKER = new string[10000];
        public static string[] BEVERAGE_IMAGE = new string[10000];
        public static string[] BEVERAGE_EXPLAIN = new string[10000];

        public static string[] DESSERT_NAME = new string[10000];
        public static string[] DESSERT_STICKER = new string[10000];
        public static string[] DESSERT_IMAGE = new string[10000];
        public static string[] DESSERT_EXPLAIN = new string[10000];

        public static string[] ETC_NAME = new string[10000];
        public static string[] ETC_STICKER = new string[10000];
        public static string[] ETC_IMAGE = new string[10000];
        public static string[] ETC_EXPLAIN = new string[10000];

        //public static int btn_select_num;
        public static Brush btn_select_img;
        public static Brush btn_select_sticker_img;
        public static Brush btn_select_explain_img;
        
        public static int Total_page;
      

        public static int Beverage_Total_page;
        public static int Dessert_Total_page;
        public static int Etc_Total_page;

        public static int counter = 0;

        public static int beverage_counter = 0;
        public static int dessert_counter = 0;
        public static int etc_counter = 0;
    
    }
}
