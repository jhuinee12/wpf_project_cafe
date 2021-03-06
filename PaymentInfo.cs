﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_project_Cafe
{
    public class PaymentInfo
    {

        public string _ProductNumber;
        public string _ProductName;
        public int _ProductQuantity;
        public string _ProductPrice;
        public string _ProductOption;

        public string ProductNumber { get { return _ProductNumber; } set { _ProductNumber = value; } }
        public string ProductName { get { return _ProductName; } set { _ProductName = value; } }
        public int ProductQuantity { get { return _ProductQuantity; } set { _ProductQuantity = value; } }
        public string ProductPrice { get { return _ProductPrice; } set { _ProductPrice = value; } }
        public string ProductOption { get { return _ProductOption; } set { _ProductOption = value; } }

        public static List<PaymentInfo> instance;

        public static List<PaymentInfo> GetInstance()
        {

            if (instance == null)
                instance = new List<PaymentInfo>();

            return instance;
        }
    }
}
