using CoreProject.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CoreProject.Managers
{
    public class StudentManagers
    {
        //學生註冊
        public void StudentSigh_UP(StudentInfoModel model,AccountModel acmodel)
        {
            Guid student_id =Guid.NewGuid();
            string connectionstring = "Data Source=localhost\\SQLExpress;Initial Catalog=Course_Selection_System_of_UBAY; Integrated Security=true";

            string queryString =
                $@"INSERT INTO Student
                    (Student_ID,S_FirstName,S_LastName,Birthday,idn,Email,Address,CellPhone,Education,School_ID,School_Name,
                        Experience,ExYear,gender,b_empno,b_date)
                    )
                VALUES
                    (@Student_ID,@S_FirstName,@S_LastName,@Birthday,@idn,@Email,@Address,@CellPhone,@Education,@School_ID,
                        @Experience,@ExYear,@gender,@b_empno,@b_date);

                INSERT INTO Account_summary
                    (Account,password,Type)
                VALUES
                    (@Account,@password,@Type);";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {

            new SqlParameter("@Student_ID", student_id),
            new SqlParameter("@S_FirstName", model.S_FirstName),
            new SqlParameter("@S_LastName", model.S_LastName),
            new SqlParameter("@Birthday", model.Birthday),
            new SqlParameter("@idn", model.Idn),
            new SqlParameter("@Email", model.Email),
            new SqlParameter("@Address", model.Address),
            new SqlParameter("@CellPhone", model.CellPhone),
            new SqlParameter("@Education", model.Education),
            new SqlParameter("@School_ID", model.School_ID),
            new SqlParameter("@Experience", model.Experience),
            new SqlParameter("@ExYear", model.ExYear),
            new SqlParameter("@gender", model.gender),
            new SqlParameter("@b_empno", model.b_empno),
            new SqlParameter("@b_date", model.b_date),

            new SqlParameter("@Account", model.Idn),
            new SqlParameter("@password", acmodel.password),
            new SqlParameter("@Type", "0")
        };

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                SqlCommand command = new SqlCommand(queryString, connection);




                try
                {
                    connection.Open();
                    int totalChangeRows = command.ExecuteNonQuery();
    HttpContext.Current.Response.Write("Total change" + totalChangeRows + "Rows");
                    connection.Close();
                }

                catch (Exception ex)
{
    HttpContext.Current.Response.Write(ex.Message);
}
            }
        }

        public static void tead()
{

}

    }
}
