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
        //public SQLiteDataAdapter adapter;
        //public DataSet ds = new DataSet();
        //public DataTable dt = new DataTable();

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
                MessageBox.Show(rdr["Num"] + " " + rdr["Name"] + " " + rdr["type"] + " " + rdr["size"] + " " + rdr["price"]);
            }
            rdr.Close();
            cmd.Dispose();

            // 데이터그리드에 뿌리기 (실패)
            /*            adapter = new SQLiteDataAdapter(CommandText, con);
                        ds.Reset();
                        adapter.Fill(ds);
                        //dt = ds.Tables[0];
                        DbTestForm.tf.dgv_Test.DataContext = ds.Tables[0];
                        con.Close();*/
        }

        /* 회원 테이블 로드 */
        public void CustomerLoadData()
        {
            SetConnection();
            con.Open();
            string CommandText = "select * from Customer";
            cmd = new SQLiteCommand(CommandText, con);
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                MessageBox.Show(rdr["NickName"] + " " + rdr["Phone"]);
            }
            rdr.Close();
            cmd.Dispose();
        }

        /* 결제정보 테이블 로드 */
        public void StlmLoadData()
        {
            SetConnection();
            con.Open();
            string CommandText = "select * from Stlm";
            cmd = new SQLiteCommand(CommandText, con);
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                MessageBox.Show(rdr["Num"] + " " + rdr["Price"] + " " + rdr["How"] + " " + rdr["Date"]);
            }
            rdr.Close();
            cmd.Dispose();
        }
    }
}
