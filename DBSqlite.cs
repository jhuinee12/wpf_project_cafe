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
                MessageBox.Show(rdr["sum_price"]+"");
            }
            TableQuit();
        }
    }
}