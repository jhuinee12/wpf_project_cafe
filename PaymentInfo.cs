using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_project_Cafe
{
    class PaymentInfo
    {

        private string _ProductNumber;
        private string _ProductName;
        private int _ProductQuantity;
        private int _ProductPrice;

        public string ProductNumber { get { return _ProductNumber; } set { _ProductNumber = value; } }
        public string ProductName { get { return _ProductName; } set { _ProductName = value; } }
        public int ProductQuantity { get { return _ProductQuantity; } set { _ProductQuantity = value; } }
        public int ProductPrice { get { return _ProductPrice; } set { _ProductPrice = value; } }

        private static List<PaymentInfo> instance;

        public static List<PaymentInfo> GetInstance()
        {

            if (instance == null)
                instance = new List<PaymentInfo>();

            return instance;
        }
    }
}
