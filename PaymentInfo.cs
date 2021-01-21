using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_project_Cafe
{
    class PaymentInfo
    {

        private string _ProductName;
        private string _ProductQuantity;
        private string _ProductPrice;

        public string ProductName { get { return _ProductName; } set { _ProductName = value; } }
        public string ProductQuantity { get { return _ProductQuantity; } set { _ProductQuantity = value; } }
        public string ProductPrice { get { return _ProductPrice; } set { _ProductPrice = value; } }

        private static List<PaymentInfo> instance;

        public static List<PaymentInfo> GetInstance()
        {

            if (instance == null)
                instance = new List<PaymentInfo>();

            return instance;
        }
    }
}
