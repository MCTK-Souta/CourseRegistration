using CoreProject.Models;
using CoreProject.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Text.RegularExpressions;
using System.Data;

namespace CoreProject.Managers
{
    public class StudentManagers : DBBase
    {
        //public bool GetAccountForRegion(string Account)
        //{
        //    SqlConnection conn = new SqlConnection("Data Source=localhost\\SQLExpress;Initial Catalog=Course_Selection_System_of_UBAY; Integrated Security=true");
        //    conn.Open();

        //    SqlCommand bb = new SqlCommand("Select * From Account_summary Where Account='" + Account + "'", conn);
        //    SqlDataReader ha = bb.ExecuteReader();

        //    return ha.Read();

        //}


        private bool HasAccount(string account)
        {
            return false;
        }

        /// <summary>
        /// 學生註冊 學生Model,帳戶Model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="acmodel"></param>
        public void StudentSigh_UP(StudentInfoModel model, AccountModel acmodel)
        {
            if (this.HasAccount(acmodel.Account))
            {
                throw new Exception($"Account [{acmodel.Account}] has been created.");
            }

            Guid student_id = Guid.NewGuid();
            Guid b_empno = student_id;
            string queryString =
                $@" INSERT INTO Account_summary
                    (Acc_sum_ID,Account,password,Type)
                VALUES
                    (@Acc_sum_ID,@Account,@password,@Type);


                INSERT INTO Student
                    (Student_ID,S_FirstName,S_LastName,Birthday,idn,Email,Address,CellPhone,Education,School_ID,
                        Experience,ExYear,gender,PassNumber,PassPic,b_empno,b_date)
                    
                VALUES
                    (@Student_ID,@S_FirstName,@S_LastName,@Birthday,@idn,@Email,@Address,@CellPhone,@Education,@School_ID,
                        @Experience,@ExYear,@gender,@PassNumber,@PassPic,@b_empno,@b_date);

";

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
            new SqlParameter("@PassNumber",model.PassNumber),
            new SqlParameter("@PassPic",model.PassPic),
            new SqlParameter("@b_empno", b_empno),
            new SqlParameter("@b_date", model.b_date),

            new SqlParameter("@Acc_sum_ID", student_id),
            new SqlParameter("@Account", model.Idn),
            new SqlParameter("@password", acmodel.password),
            new SqlParameter("@Type", false)
            };

            this.ExecuteNonQuery(queryString, parameters);

        }



        //public DataTable GetStudentCourse(string ID)
        //{
        //    string cmd = @"SELECT * FROM Registration_record WHERE Student_ID = @ID;";
        //    List<SqlParameter> parameters = new List<SqlParameter>()
        //    {
        //        new SqlParameter("@ID",ID)
        //    };
        //    return this.GetDataTable(cmd, parameters);
        //}

        //抓取學生歷史報名紀錄
        public DataTable GetStudentCourseRecord(string ID)
        {
            string cmd = @" SELECT * 
                FROM Registration_record
                    INNER JOIN Student
                    ON Registration_record.Student_ID=Student.Student_ID
                INNER JOIN Course 
                ON Registration_record.Course_ID=Course.Course_ID
		            INNER JOIN Teacher
		            ON Teacher.Teacher_ID=Course.Teacher_ID
		            INNER JOIN Place
		            ON Place.Place_ID=Course.Place_ID
                WHERE Registration_record.Student_ID =@ID;";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@ID",ID)
            };
            return this.GetDataTable(cmd, parameters);
        }


    }
}
