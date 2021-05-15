using CoreProject.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace CoreProject.Managers
{
    public class CourseManagers : DBBase
    {
        /// <summary>
        ///所有課程歷史紀錄
        /// </summary>
        /// <param name="Course_ID">課程ID</param>
        /// <param name="C_Name">課程名稱</param>
        /// <param name="StartDate">開課時間</param>
        /// <param name="EndDate">結束時間</param>
        /// <param name="Place_Name">教室</param>
        /// <param name="Price1">最小價格</param>
        /// <param name="Price2">最大價格</param>
        /// <param name="ddlTeacher">教師</param>
        /// <param name="ddlCourseStatus">課程狀態</param>
        /// <returns></returns>
        public DataTable SearchAllCourse(string Course_ID, string C_Name, string StartDate, string EndDate, string Place_Name, string Price1, string Price2, string ddlTeacher,string ddlCourseStatus)
        {

            string cmd = @"SELECT * 
								FROM Course
								INNER JOIN Teacher 
								ON Course.Teacher_ID=Teacher.Teacher_ID 
								INNER JOIN Place 
								ON Course.Place_ID=Place.Place_ID 
								 WHERE ";

            List<SqlParameter> parameters = new List<SqlParameter>();


            if (!string.IsNullOrEmpty(Course_ID))
            {
                cmd += "Course.Course_ID LIKE @Course_ID AND ";
                parameters.Add(new SqlParameter("@Course_ID", $"%{Course_ID}%"));
            }
            if (!string.IsNullOrEmpty(C_Name))
            {
                cmd += "Course.C_Name LIKE @C_Name AND ";
                parameters.Add(new SqlParameter("@C_Name", $"%{C_Name}%"));
            }
            //教師id
            if (!string.IsNullOrEmpty(ddlTeacher))
            {
                cmd += "Teacher.Teacher_ID = @Teacher_ID AND ";
                parameters.Add(new SqlParameter("@Teacher_ID", ddlTeacher));
            }

            if (!string.IsNullOrEmpty(StartDate) && !string.IsNullOrEmpty(EndDate))
            {
                DateTime tempDate1 = DateTime.Parse(StartDate);
                DateTime tempDate2 = DateTime.Parse(EndDate);

                if (tempDate1 > tempDate2)
                {
                    DateTime temp = tempDate1;
                    tempDate1 = tempDate2;
                    tempDate2 = temp;
                }
                cmd += "Course.StartDate >= @StartDate AND ";
                parameters.Add(new SqlParameter("@StartDate", tempDate1));
                cmd += "Course.EndDate <= @EndDate AND ";
                parameters.Add(new SqlParameter("@EndDate", tempDate2));
            }
            else if (!string.IsNullOrEmpty(StartDate))
            {
                cmd += "Course.StartDate >= @StartDate AND ";
                parameters.Add(new SqlParameter("@StartDate", StartDate));
            }
            else if (!string.IsNullOrEmpty(EndDate))
            {
                cmd += "Course.EndDate <= @EndDate AND ";
                parameters.Add(new SqlParameter("@EndDate", EndDate));
            }

            if (!string.IsNullOrEmpty(Place_Name))
            {
                cmd += "Place.Place_Name LIKE @Place_Name AND ";
                parameters.Add(new SqlParameter("@Place_Name", $"%{Place_Name}%"));
            }

            if (!string.IsNullOrEmpty(Price1) && !string.IsNullOrEmpty(Price2))
            {

                int tempPrice1 = int.Parse(Price1);
                int tempPrice2 = int.Parse(Price2);

                if (tempPrice1 > tempPrice2)
                {
                    int temp = tempPrice1;
                    tempPrice1 = tempPrice2;
                    tempPrice2 = temp;
                }
                cmd += "Course.Price >= @Price1 AND ";
                parameters.Add(new SqlParameter("@Price1", tempPrice1));
                cmd += "Course.Price <= @Price2 AND ";
                parameters.Add(new SqlParameter("@Price2", tempPrice2));

            }
            else if (!string.IsNullOrEmpty(Price1))
            {
                cmd += "Course.Price >= @Price1 AND ";
                parameters.Add(new SqlParameter("@Price1", Price1));

            }
            else if (!string.IsNullOrEmpty(Price2))
            {
                cmd += "Course.Price <= @Price2 AND ";
                parameters.Add(new SqlParameter("@Price2", Price2));
            }
            if (ddlCourseStatus == "2" )
            {
                cmd += "Course.d_date IS NOT NULL AND ";
                parameters.Add(new SqlParameter("d_date", "2"));
            }
            else if(ddlCourseStatus =="1")
            {
                cmd += "Course.d_date IS NULL AND ";
                parameters.Add(new SqlParameter("d_date", "1"));
            }
            else
            {
                //cmd += "";
                //parameters.Add(new SqlParameter("", "0"));
            }
            if (cmd.EndsWith(" WHERE "))
                cmd = cmd.Remove(cmd.Length - 7, 7);
            else
                cmd = cmd.Remove(cmd.Length - 5, 5);
            return GetDataTable(cmd, parameters);
        }


    }
}
