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
        public int sum_price = 0;

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

        /* 영수증 출력 */
        public void StlmLoadData(string stlm_number)
        {
            string stlmQuery = "select * from stlm where stlm_number = "+stlm_number;
            TableLoad(stlmQuery);
            while (rdr.Read())
            {
                payment_list = (rdr["payment_list"] + "").Split('|');

                for (int i = 0; i < payment_list.Length; i++) 
                {

                    if (i % 2 == 0)
                    {
                        MessageBox.Show("상품번호:"+payment_list[i]);

                        int price = 0;
                        string productQuery = "select * from product where product_number = " + payment_list[i];
                        TableLoad(productQuery); 
                        while (rdr.Read())
                        {
                            price = Int32.Parse(rdr["price"] + "");
                        }

                        sum_price += price * Int32.Parse(payment_list[i + 1]);
                        MessageBox.Show("총합 : " + sum_price);

                        TableQuit();
                    }
                }
            }
            TableQuit();
        }
    }
}