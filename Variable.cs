using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_project_Cafe
{
    public class Variable
    {
        public int product_price;      // 제품 가격
        public int sum_price;          // 총 구매 가격
        public int count;              // 리스트뷰 개수
        public int btClick;

        public string datetime;        // 구매시간
        public string stlm_number;     // 영수증 번호
        public string product_number;  // 제품번호
        public string payment_list;    // 리스트뷰에 담은 제품 목록
        public string place;           // 장소 선택
        public string payment_method;  // 결제방법
    }
}
