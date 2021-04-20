using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace Ubay_CourseRegistration
{
    public class DBAccountManager
    {
        public static DataTable GetUserAccount(string account)
        {
            string connectionstring =
                "Data Source=localhost\\SQLExpress;Initial Catalog=CSharpLession; Integrated Security=true";

            string queryString =
                $@" SELECT Account_summary.Account,Account_summary.password,Manager.Account,Manager.Manager_FirstName,Manager.Manager_LastName
                    FROM Account_summary inner join Manager 
                    on Account_summary.Account=Manager.Account
					WHERE Account_summary.Account=@account;";

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@account", account);


                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    reader.Close();
                    return dt;
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }

            }


        }
    }
}