﻿using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPF_project_Cafe
{
    class DBSqlite
    {
        public SQLiteConnection con;
        public SQLiteCommand cmd;
        public SQLiteDataReader rdr;
        public string[] payment_list;
        public string pdata;

        /* DB 연결 코드 */
        public void SetConnection()
        {
            string path = Environment.CurrentDirectory + @"\WPF_db\wpf.db;";
            con = new SQLiteConnection("Data Source=" + path + "version=3;");
        }

        /* 테이블 로드 */
        public void TableLoad(string query)
        {
            SetConnection();
            con.Open();
            cmd = new SQLiteCommand(query, con);
            rdr = cmd.ExecuteReader();
        }

        /* 테이블 로드 종료 */
        public void TableQuit()
        {
            rdr.Close();
            cmd.Dispose();
        }

        /* 컬럼 개수 구하기 */
        public int ColumnCount()
        {
            SetConnection();
            con.Open();
            cmd = new SQLiteCommand("select count(*) from product", con);
            int pCount =  Int32.Parse((String)cmd.ExecuteScalar());
            TableQuit();
            return pCount;
        }

        /* 컬럼 추가 */
        public void InsertColumn(string query)
        {
            SetConnection();
            con.Open();
            cmd = new SQLiteCommand(query, con);
            cmd.ExecuteNonQuery();
        }

        /* 테이블 내 데이터 가져오기 */
        public string DataLoad(string tableName, string where, string column)
        {
            pdata = "";
            string Query = "select * from " + tableName + " " + where;
            TableLoad(Query);
            while (rdr.Read())
            {
                pdata = (rdr[column] + "");
            }
            TableQuit();

            return pdata;
        }
        /* 상품 테이블 로드 */
        public void ProductLoadData()
        {
            GlobalVar.beverage_counter = 0;
            GlobalVar.dessert_counter = 0;
            TableLoad("select * from Product");

            while (rdr.Read())
            {
                //음료 읽어오기
                if ((string)rdr["beverage_dessert_etc"] == "beverage")
                {
                    GlobalVar.BEVERAGE_NAME[GlobalVar.beverage_counter] = (string)rdr["name"];
                    GlobalVar.BEVERAGE_STICKER[GlobalVar.beverage_counter] = (string)rdr["new_hot_none"];
                    GlobalVar.BEVERAGE_IMAGE[GlobalVar.beverage_counter] = (string)rdr["image"];

                    GlobalVar.beverage_counter++;
                }//디저트 읽어오기
                else if ((string)rdr["beverage_dessert_etc"] == "dessert")
                {
                    GlobalVar.DESSERT_NAME[GlobalVar.dessert_counter] = (string)rdr["name"];
                    GlobalVar.DESSERT_STICKER[GlobalVar.dessert_counter] = (string)rdr["new_hot_none"];
                    GlobalVar.DESSERT_IMAGE[GlobalVar.dessert_counter] = (string)rdr["image"];

                    GlobalVar.dessert_counter++;
                }
                //기타 등등 읽어오기
                else if ((string)rdr["beverage_dessert_etc"] == "etc")
                {
                    GlobalVar.ETC_NAME[GlobalVar.dessert_counter] = (string)rdr["name"];
                    GlobalVar.ETC_STICKER[GlobalVar.dessert_counter] = (string)rdr["new_hot_none"];
                    GlobalVar.ETC_IMAGE[GlobalVar.dessert_counter] = (string)rdr["image"];

                    GlobalVar.etc_counter++;
                }
                //// 메세지박스로 데이터 들어온거 테스트
                //MessageBox.Show(rdr["product_number"] + " " + rdr["name"] + " " + rdr["drink_snack_etc"] + " " + rdr["hot_ice_none"] + " " + rdr["size"] + " " + rdr["price"] + " " + rdr["image"] + " " + rdr["new_hot_none"]);
            }
            TableQuit();
        }

        /* 구매 제품명 출력 */
        public string PaymentListLoad (string pn)
        {
            pdata = "";
            string Query = "select * from product where product_number = \"" + pn + "\"";
            TableLoad(Query);
            while (rdr.Read())
            {
                pdata = (rdr["name"] + " / " + rdr["hot_ice_none"] + " / " + rdr["size"]);
            }
            TableQuit();

            return pdata;
        }

        /* 영수증 출력 */
        public void StlmLoadData(string stlm_number)
        {
            StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + @"\stlm_list\stlm_" + stlm_number + ".txt");
            int sum_price = 0;


            sw.WriteLine("영수증번호 : " + stlm_number);

            // 해당 영수증 불러오기
            string stlmQuery = "select * from stlm where stlm_number = " + stlm_number;
            TableLoad(stlmQuery);
            while (rdr.Read())
            {
                payment_list = (rdr["payment_list"] + "").Split('|');
                sw.WriteLine("결제방법 : " + rdr["card_cash"]);
                sw.WriteLine("결제일시 : " + rdr["datetime"]);
            }
            TableQuit();

            sw.WriteLine("***************************************************");

            // 총합 계산
            for (int i = 0; i < payment_list.Length; i++)
            {
                if (i % 2 == 0)
                {
                    int price = 0;
                    string productQuery = "select * from product where product_number = \"" + payment_list[i] + "\"";

                    TableLoad(productQuery);
                    while (rdr.Read())
                    {
                        price = Int32.Parse(rdr["price"] + "");
                        sw.WriteLine(rdr["product_number"] + " | " + rdr["name"] + " | " + payment_list[i + 1] + " | " + price * Int32.Parse(payment_list[i + 1])); // 테스트용
                    }
                    sum_price += price * Int32.Parse(payment_list[i + 1]);

                    TableQuit();
                }
            }
            sw.WriteLine("***************************************************");
            sw.WriteLine(stlm_number + "번 영수증의 구매 총합 : " + sum_price); // 테스트용
            sw.Close();
        }

        /* 상품명 출력 */
        public string StlmProductName(string stlm_number)
        {
            string product_name = "";

            for (int i = 0; i < payment_list.Length; i++)
            {
                if (i % 2 == 0)
                {
                    string productQuery = "select * from product where product_number = \"" + payment_list[i] + "\"";
                    TableLoad(productQuery);
                    while (rdr.Read())
                    {
                        if (rdr["beverage_dessert_etc"]+"" == "beverage")
                        {
                            product_name += ("(음료) " + rdr["name"] + "\n\n");
                        }
                        else if (rdr["beverage_dessert_etc"] + "" == "dessert")
                        {
                            product_name += ("(디저트) " + rdr["name"] + "\n\n");
                        }
                        else
                        {
                            product_name += ("(기타) " + rdr["name"] + "\n\n");
                        }
                    }
                    TableQuit();
                }
            }
            return product_name;
        }

        /* 수량 출력 */
        public string StlmProductQuantity(string stlm_number)
        {
            string product_quantity = "";

            for (int i = 0; i < payment_list.Length; i++)
            {
                if (i % 2 != 0)
                {
                    product_quantity += payment_list[i] + "\n\n";
                }
            }
            return product_quantity;
        }

        /* 상품별 가격 출력 */
        public string StlmProductPrice(string stlm_number)
        {
            string product_price = "";

            for (int i = 0; i < payment_list.Length; i++)
            {
                if (i % 2 == 0)
                {
                    int price = 0;
                    string productQuery = "select * from product where product_number = \"" + payment_list[i] + "\"";
                    TableLoad(productQuery);
                    while (rdr.Read())
                    {
                        price = Int32.Parse(rdr["price"] + "");
                    }
                    product_price += price * Int32.Parse(payment_list[i + 1]) + "\n\n";

                    TableQuit();
                }
            }
            return product_price;
        }
    }
}