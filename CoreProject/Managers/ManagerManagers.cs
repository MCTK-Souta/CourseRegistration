using CoreProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Ubay_CourseRegistration.Utility
{
    public class ManagerManagers
    {
        public static void InsertAdminTablel(AccountModel acmodel, Account_summaryModel asmodel, string createtime)
        {
            string connectionstring = "Data Source=localhost\\SQLExpress;Initial Catalog=Course_Selection_System_of_UBAY; Integrated Security=true";

            string queryString =
                $@"

                INSERT INTO Account_summary
                    (Acc_sum_ID,Account, password, Type)
                VALUES
                    (@GUID,@Account, @Password, @Type);
                INSERT INTO Manager
                    (Manager_ID,Manager_FirstName,Manager_LastName,Department,Account,b_date,b_empno)
                VALUES
                    (@GUID,@Firstname,@Lastname,@Department,@Account,@createtime,@GUID);
                ";



            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                command.Parameters.AddWithValue("@GUID", acmodel.Acc_sum_ID);
                command.Parameters.AddWithValue("@Firstname", asmodel.firstname);
                command.Parameters.AddWithValue("@Lastname", asmodel.lastname);
                command.Parameters.AddWithValue("@Department", asmodel.department);
                command.Parameters.AddWithValue("@Account", acmodel.Account);
                command.Parameters.AddWithValue("@Password", acmodel.password);
                command.Parameters.AddWithValue("@Pwdcheck", asmodel.Pwdcheck);
                command.Parameters.AddWithValue("@Type", acmodel.Type);
                command.Parameters.AddWithValue("@createtime", createtime);

                //List<SqlParameter> parameters = new List<SqlParameter>()
                //{
                //    new SqlParameter("@GUID", acmodel.Acc_sum_ID),
                //    new SqlParameter("@Firstname", asmodel.firstname),
                //    new SqlParameter("@Lastname", asmodel.lastname),
                //    new SqlParameter("@Department", asmodel.department),
                //    new SqlParameter("@Account", acmodel.Account),
                //    new SqlParameter("@Password", acmodel.Password),
                //    new SqlParameter("@Pwdcheck", asmodel.Pwdcheck),
                //    new SqlParameter("@Pwdcheck", acmodel.Type),
                //    new SqlParameter("@createtime", createtime)
                //};



                try
                {
                    connection.Open();
                    int totalChangeRows = command.ExecuteNonQuery();
                    //HttpContext.Current.Response.Write("Total change" + totalChangeRows + "Rows");
                    HttpContext.Current.Response.Write("<script>alert('新增成功!');</script>");
                }

                catch (Exception ex)
                {
                    HttpContext.Current.Response.Write(ex.Message);
                }
            }
        } //新增管理人(勿更改)

        public static DataTable ReadTestTable1DT(string Account)

        {
            string connectionstring =
                "Data Source=localhost\\SQLExpress;Initial Catalog=Course_Selection_System_of_UBAY; Integrated Security=true";

            string queryString =
                $@" SELECT * FROM Account_summary
                    WHERE Account = @Account";
            //ORDER BY ID DESC;";

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@Account", Account);


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
                    HttpContext.Current.Response.Write(ex.Message);
                    return null;
                }

            }

        } //查詢

        public static DataTable ReadTestTable1on1(string ID)
        {
            string connectionstring =
                "Data Source=localhost\\SQLExpress;Initial Catalog=CSharpLession; Integrated Security=true";

            string queryString =
                $@" SELECT * FROM TestTable1;
                WHERE ID = @id
                ORDER BY ID DESC; ";

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@ID", 3);


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
                    HttpContext.Current.Response.Write(ex.Message);
                    return null;
                }

            }

        } //查詢

        public static void DeleteTestTablel(string id)
        {
            string connectionstring = "Data Source=localhost\\SQLExpress;Initial Catalog=CSharpLession; Integrated Security=true";

            string queryString =
                $@"DELETE FROM TestTable1 WHERE ID=@ID";

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@ID", id);

                try
                {
                    connection.Open();
                    int totalChangeRows = command.ExecuteNonQuery();
                    HttpContext.Current.Response.Write("Total change" + totalChangeRows + "Rows");
                }

                catch (Exception ex)
                {
                    HttpContext.Current.Response.Write(ex.Message);
                }
            }
        } //刪除

        public static void UpdateTestTablel(string id, string name, string birthday, string numbercol)
        {
            string connectionstring = "Data Source=localhost\\SQLExpress;Initial Catalog=CSharpLession; Integrated Security=true";

            string queryString =
                $@"UPDATE  TestTable1 SET
                    ID=@ID,Name=@Name,Birthday=@Birthday,NumberCol=@NumberCol
                WHERE
                    ID=@ID;";

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@ID", id);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Birthday", birthday);
                command.Parameters.AddWithValue("@NumberCol", numbercol);

                try
                {
                    connection.Open();
                    int totalChangeRows = command.ExecuteNonQuery();
                    HttpContext.Current.Response.Write("Total change" + totalChangeRows + "Rows");
                }

                catch (Exception ex)
                {
                    HttpContext.Current.Response.Write(ex.Message);
                }
            }
        } //修改


        public static DataTable BuildDataTablel()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Birthday", typeof(DateTime));
            dt.Columns.Add("NumberCol", typeof(int));
            dt.Columns["Birthday"].AllowDBNull = true;

            DateTime baseDate = new DateTime(2011, 1, 1);
            for (var i = 0; i < 50; i++)
            {
                DataRow dr = dt.NewRow();
                dr["ID"] = i;
                dr["Name"] = "UserName" + i;
                dr["Birthday"] = baseDate.AddDays(i);
                //dr["NumberCol"] = "NuberCol"+(i);
                dt.Rows.Add(dr);
            }

            return dt;

        } //迴圈建立多筆資料
    }
}