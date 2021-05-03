using CoreProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using CoreProject.Helpers;
using CoreProject.ViewModels;


namespace Ubay_CourseRegistration.Managers
{
    public class ManagerManagers : DBBase
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
                    HttpContext.Current.Response.Write("<script>alert('新增成功!');</script>");
                }

                catch (Exception ex)
                {
                    HttpContext.Current.Response.Write(ex.Message);
                }
            }
        } //新增管理人(勿更改)

        public static AccountModel GetAccount(string Account)

        {
            string connectionstring =
                "Data Source=localhost\\SQLExpress;Initial Catalog=Course_Selection_System_of_UBAY; Integrated Security=true";

            string queryString =
                $@" SELECT * FROM Account_summary
                    WHERE Account = @Account";

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@Account", Account);


                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();


                    AccountModel model = null;

                    while (reader.Read())
                    {
                        model = new AccountModel();
                        model.Acc_sum_ID = (Guid)reader["Acc_sum_ID"];
                        model.Account = (string)reader["Account"]; 
                        model.password = (string)reader["password"];
                        model.Type = (bool)reader["Type"];
                    }
                    reader.Close();
                    return model;
                }

                catch (Exception ex)
                {
                    HttpContext.Current.Response.Write(ex.Message);
                    return null;
                }

            }

        }


        public List<StudentAccountViewModel> GetStudentViewModels(
      string name, string Idn, out int totalSize, int currentPage = 1, int pageSize = 10)
        {
            //----- Process filter conditions -----
            List<string> conditions = new List<string>();

            if (!string.IsNullOrEmpty(name))
                conditions.Add(" Student.S_FirstName+Student.S_LastName LIKE '%' + @name + '%'");

            if (!string.IsNullOrEmpty(Idn))
                conditions.Add(" Idn = @Idn");

            string filterConditions =
                (conditions.Count > 0)
                    ? (" WHERE " + string.Join(" AND ", conditions))
                    : string.Empty;
            //----- Process filter conditions -----


            string query =
                $@"         
					SELECT TOP {10} * FROM
                    (
                        SELECT 
                            ROW_NUMBER() OVER(ORDER BY Student.Idn) AS RowNumber,
                            Student.Student_ID,
                            Student.S_FirstName+Student.S_LastName AS 姓名,
                            Student.gender AS 性別,
                            Student.Idn AS 身份證字號,
							Student.CellPhone AS 手機,
                            Student.Address AS 地址
                        FROM Student
                        JOIN Account_summary
                        ON Student.Student_ID = Account_summary.Acc_sum_ID
                        {filterConditions}
                    ) AS TempT
                    WHERE RowNumber > {pageSize * (currentPage - 1)}
                    ORDER BY 身份證字號
                    ";

            string countQuery =
                $@" SELECT 
                        COUNT(Student.Idn) 
                    FROM Student
                    JOIN Account_summary
                    ON  Student.Student_ID = Account_summary.Acc_sum_ID
                    {filterConditions}
                ";

            List<SqlParameter> dbParameters = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(name))
                dbParameters.Add(new SqlParameter("@name", name));

            if (!string.IsNullOrEmpty(Idn))
                dbParameters.Add(new SqlParameter("@Idn", Idn));


            var dt = this.GetDataTable(query, dbParameters);

            List<StudentAccountViewModel> list = new List<StudentAccountViewModel>();

            foreach (DataRow dr in dt.Rows)
            {
                StudentAccountViewModel model = new StudentAccountViewModel();
                model.Student_ID = (Guid)dr["Student_ID"];
                model.S_FirstName = (string)dr["姓名"];
                model.gender = (bool)dr["性別"];
                model.Idn = (string)dr["身份證字號"];
                model.CellPhone = (string)dr["手機"];
                model.Address = (string)dr["地址"];


                list.Add(model);
            }


            // 算總數並回傳
            int? totalSize2 = this.GetScale(countQuery, dbParameters) as int?;
            totalSize = (totalSize2.HasValue) ? totalSize2.Value : 0;

            return list;
        }

        public void DeleteStudentViewModel(Guid id)
        {
            string dbCommandText =
                $@" DELETE AccountInfos WHERE ID = @id;
                    DELETE Accounts WHERE ID = @id;
                ";

            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@id", id),
            };

            this.ExecuteNonQuery(dbCommandText, parameters);
        }

        public string GetgenderName(bool gender)
        {
            switch (gender)
            {
                case false:
                    return "男";
                case true:
                    return "女";
                default:
                    return "";
            }
        }

        public StudentAccountViewModel GetAccountViewModel(Guid id)
        {
            string connectionString = GetConnectionString();
            string queryString =
                $@" SELECT 
                    Student.Student_ID,
                    Student.S_FirstName,
                    Student.S_LastName,
                    Student.Birthday,
                    Student.idn,
                    Student.Email,
                    Student.Address,
                    Student.CellPhone,
                    Student.Education,
                    Student.School_ID,
                    Student.Experience,
                    Student.ExYear,
                    Student.gender,
                    Student.PassNumber,
                    Student.PassPic,
                    Account_summary.password
                    FROM Student
                    JOIN Account_summary
                        ON Student.Student_ID = Account_summary.Acc_sum_ID
                    WHERE Student.Student_ID = @id
                ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@id", id);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    StudentAccountViewModel model = null;

                    while (reader.Read())
                    {
                        model = new StudentAccountViewModel();
                        model.Student_ID = (Guid)reader["Student_ID"];
                        model.S_FirstName = (string)reader["S_FirstName"];
                        model.S_LastName = (string)reader["S_LastName"];
                        model.Birthday = (DateTime)reader["Birthday"];
                        model.Idn = (string)reader["Idn"];
                        model.Email = (string)reader["Email"];
                        model.Address = (string)reader["Address"];
                        model.CellPhone = (string)reader["CellPhone"];
                        model.Education = (string)reader["Education"];
                        model.School_ID = (string)reader["School_ID"];
                        model.Experience = (string)reader["Experience"];
                        model.ExYear = (string)reader["ExYear"];
                        model.gender = (bool)reader["gender"];
                        model.PassNumber = (string)reader["PassNumber"];
                        model.PassPic = (string)reader["PassPic"];

                        model.password = (string)reader["password"];
                    }

                    reader.Close();

                    return model;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public void CreatStudent(StudentAccountViewModel model)
        {

            Guid student_id = Guid.NewGuid();


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
            new SqlParameter("@b_empno", model.b_empno),
            new SqlParameter("@b_date", model.b_date),

            new SqlParameter("@Acc_sum_ID", student_id),
            new SqlParameter("@Account", model.Idn),
            new SqlParameter("@password", model.password),
            new SqlParameter("@Type", false)
            };

            this.ExecuteNonQuery(queryString, parameters);

        }

        public void UpdataStudent(StudentAccountViewModel model)
        {

            Guid student_id = Guid.NewGuid();


            string queryString =
                $@" UPDATE Account_summary
                    SET 
                        Account = @Account, 
                        password = @password, 
                    WHERE
                        Acc_sum_ID = @Acc_sum_ID;

                 UPDATE Student
                    SET 
                        S_FirstName = @S_FirstName, 
                        S_LastName = @S_LastName, 
                        Birthday = @Birthday, 
                        idn = @idn, 
                        Email = @Email, 
                        Address = @Address, 
                        CellPhone = @CellPhone, 
                        Education = @Education, 
                        School_ID = @School_ID, 
                        Experience = @Experience, 
                        ExYear = @ExYear, 
                        gender = @gender, 
                        PassNumber = @PassNumber, 
                        PassPic = @PassPic, 
                        b_empno = @b_empno, 
                        b_date = @b_date, 
                    WHERE
                        Student_ID = @Student_ID;
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
            new SqlParameter("@b_empno", model.b_empno),
            new SqlParameter("@b_date", model.b_date),

            new SqlParameter("@Acc_sum_ID", student_id),
            new SqlParameter("@Account", model.Idn),
            new SqlParameter("@password", model.password),
            new SqlParameter("@Type", false)
            };

            this.ExecuteNonQuery(queryString, parameters);

        }
    }
}