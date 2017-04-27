using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DB
{
    static class DBOperate
    {
        static private string connstr = "Data Source=DESKTOP-FFV5E68\\SQLEXPRESS;Initial Catalog=AUDIOPLAYERDB;Integrated Security=true;";
        static private SqlConnection sqlconnection;
        static public bool InitDB()
        {
            try
            {
                sqlconnection = new SqlConnection(connstr);
                sqlconnection.Open();
                return true;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
                return false;
            }
        }

        static public SqlDataReader executeQuery(string qs)
        {
            try
            {
                SqlCommand command = new SqlCommand(qs, sqlconnection);
                SqlDataReader dr = command.ExecuteReader();
                return dr;
               
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
                return null;
            }

        }

        static public void executeSimpleQuery(string qs)
        {
            try
            {
                SqlCommand command = new SqlCommand(qs, sqlconnection);

            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }

        }


    }
}
