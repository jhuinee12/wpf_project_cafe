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
        public static string Language = "Kor_name";        

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

        public static int product_price;       // 제품 가격
        public static int sum_price;           // 총 구매 가격
        public static int count;               // 리스트뷰 개수

        public static string datetime;         // 구매시간
        public static string stlm_number;      // 영수증 번호
        public static string product_number;   // 제품번호
        public static string payment_list;     // 리스트뷰에 담은 제품 목록
        public static string place;            // 장소 선택
        public static string payment_method;   // 결제방법

        public static int btClick;             // 버튼 클릭 여부
        public static string beverage_size;    // 음료 사이즈
        public static string beverage_type;    // 음료 상태(hot/ice)
        public static string beverage_Option;  // 음료 사이즈+상태 옵션
    }
}
