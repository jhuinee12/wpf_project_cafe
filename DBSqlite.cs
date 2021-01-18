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
        public int stlmSerialNum;
        //public SQLiteDataAdapter adapter;
        //public DataSet ds = new DataSet();
        //public DataTable dt = new DataTable();
        //체크인테스트 sgpark 
        //commit test cgh

        public void SetConnection()
        {
            string path = Environment.CurrentDirectory + @"\WPF_db\wpf.db;";
            con = new SQLiteConnection("Data Source=" + path + "version=3;");
        }

        public void ExecuteQuery(string query)
        {
            SetConnection();
            con.Open();
            cmd = con.CreateCommand();
            cmd.CommandText = query;
            con.Close();
        }

        /* 상품 테이블 로드 */
        public void ProductLoadData()
        {
            SetConnection();
            con.Open();
            string CommandText = "select * from Product";
            cmd = new SQLiteCommand(CommandText, con);
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                // 메세지박스로 데이터 들어온거 테스트
                MessageBox.Show(rdr["serial_number"] + " " + rdr["name"] + " " + rdr["hot_ice_none"] + " " + rdr["size"] + " " + rdr["price"]);
            }
            rdr.Close();
            cmd.Dispose();
        }

        /* 회원 테이블 로드 */
        public void CustomerLoadData()
        {
            SetConnection();
            con.Open();

            string selectText = "select * from Customer";
            cmd = new SQLiteCommand(selectText, con);
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                MessageBox.Show(rdr["serial_number"] + " " + rdr["nickname"] + " " + rdr["phone_number"]);
            }

            rdr.Close();
            cmd.Dispose();
        }

        /* 영수증 출력 */
        public void StlmLoadData()
        {
            SetConnection();
            con.Open();
            string CommandText = "select * from Stlm";
            cmd = new SQLiteCommand(CommandText, con);
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                MessageBox.Show(rdr["serial_number"] + " " + rdr["customer_nickname"] + " " + rdr["sum_price"]
                    + " " + rdr["card_cash"] + " " + rdr["datetime"]);
            }
            rdr.Close();
            cmd.Dispose();
        }
    }
}
