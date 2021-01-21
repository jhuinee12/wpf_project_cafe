using System;
using System.Collections.Generic;
using System.Data.SQLite;
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

/*        public void ExecuteQuery(string query)
        {
            SetConnection();
            con.Open();
            cmd = con.CreateCommand();
            cmd.CommandText = query;
            con.Close();
        }*/

        /* 테이블 로드 */
        public void TableLoad(string query)
        {
            SetConnection();
            con.Open();
            cmd = new SQLiteCommand(query, con);
            rdr = cmd.ExecuteReader();
        }

        /* 테이블 종료 */
        public void TableQuit()
        {
            rdr.Close();
            cmd.Dispose();
        }

        /* 테이블 내 데이터 가져오기 */
        public void DataLoad(string tableName, string where, string column)
        {
            string Query = "select * from " + tableName + " " + where;
            TableLoad(Query);
            while (rdr.Read())
            {
                pdata = (rdr[column] + "");
            }
            TableQuit();
        }

        /* 구매 제품명 출력 */
        public void PaymentListLoad (string pn)
        {
            string Query = "select * from product where product_number = " + pn;
            TableLoad(Query);
            while (rdr.Read())
            {
                pdata = (rdr["name"] + " / " + rdr["hot_ice_none"] + " / " + rdr["size"]);
            }
            TableQuit();
        }

        /* 영수증 출력 */
        public void StlmLoadData(string stlm_number, int sum_price)
        {
            // 해당 영수증 불러오기
            string stlmQuery = "select * from stlm where stlm_number = "+stlm_number;
            TableLoad(stlmQuery);
            while (rdr.Read())
            {
                payment_list = (rdr["payment_list"] + "").Split('|');
            }
            TableQuit();

            // 총합 계산
            for (int i = 0; i < payment_list.Length; i++)
            {
                if (i % 2 == 0)
                {
                    string product_name = "";
                    MessageBox.Show("상품번호:" + payment_list[i] + " 구매수량:" + payment_list[i+1]); // 테스트용

                    int price = 0;
                    string productQuery = "select * from product where product_number = " + payment_list[i];
                    TableLoad(productQuery);
                    while (rdr.Read())
                    {
                        price = Int32.Parse(rdr["price"] + "");
                        product_name = (rdr["name"] + " " + rdr["hot_ice_none"] + " " + rdr["size"]);
                        MessageBox.Show("<" + product_name + ">" + "의 구매 총합 : " + price * Int32.Parse(payment_list[i + 1])); // 테스트용
                    }
                    sum_price += price * Int32.Parse(payment_list[i + 1]);

                    TableQuit();
                }
            }
            MessageBox.Show(stlm_number + "번 영수증의 구매 총합 : " + sum_price); // 테스트용
        }
    }
}