using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MANUUFinance
{
    class AdminLogin
    {
        public bool administratorLogin(int userId)
        {
            bool admin = false;
            string cs = ConfigurationManager.ConnectionStrings["FinanceConnectionString"].ConnectionString;
            //Instantiate SQL Connection
            SqlConnection objSqlConnection = new SqlConnection(cs);
            //Prepare Update String
            objSqlConnection.Open();
            SqlCommand myCommand = new SqlCommand("SELECT Username FROM [Grievance].[dbo].[User] where UserID = '" + userId + "'", objSqlConnection);
            SqlDataReader objDataReader = myCommand.ExecuteReader();

            while (objDataReader.Read())
            {
                if (objDataReader["Username"].Equals("super1"))
                {
                    admin = true;
                }
                if (objDataReader["Username"].Equals("super2"))
                {
                    admin = true;
                }
            }
           
            objSqlConnection.Close();
            return admin;
        }
    }
}
