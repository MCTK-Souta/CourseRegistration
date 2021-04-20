using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Ubay_CourseRegistration
{
    public class DataBaseExecutor
    {

  //新增
        public static DataTable ReadTestTable1DT()
        {
            string connectionstring =
                "Data Source=localhost\\SQLExpress;Initial Catalog=CSharpLession; Integrated Security=true";

            string queryString =
                $@" SELECT * FROM TestTable1";
                    //WHERE NumberCol = @NumberCol 
                    //ORDER BY ID DESC;";
            
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                //command.Parameters.AddWithValue("@NumberCol", numbercol);


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
