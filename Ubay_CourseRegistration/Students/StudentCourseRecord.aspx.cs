using CoreProject.Managers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ubay_CourseRegistration.Students
{
    public partial class StudentCourseRecord : System.Web.UI.Page
    {
        StudentManagers _studentManagers = new StudentManagers();
        readonly PagedDataSource _pgsource = new PagedDataSource();
        static DateTime datetime = DateTime.Now;
        int _firstIndex, _lastIndex;
        string _ID ;
        public string _month { get; set; } = "";
        private int _pageSize = 10;


        private int CurrentPage
        {
            get
            {
                if (ViewState["CurrentPage"] == null)
                {
                    return 0;
                }
                return ((int)ViewState["CurrentPage"]);
            }
            set
            {
                ViewState["CurrentPage"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            _ID = Session["Acc_sum_ID"].ToString();
            if (Page.IsPostBack) return;
            BindDataIntoRepeater();

            var _post = Request.QueryString["datetime"];
            //var asdflkhjalsd = Request.QueryString["coursetime"];
            //var sadfas = Request.QueryString["course12time"];
            //var asdf = Request.QueryString["eee"];
            //var afdsf = Request.QueryString["ffff"];
            if (_post != null)
                datetime = DateTime.Parse(_post);
            TEST.Text = $"{datetime.ToString("yyyy/MM")}月課程紀錄";
            CreateCalendar();
        }

        //連接報名紀錄資料表，帶入報名學生及報名課程資料 (此段功能已移至StudentManager  待測試)
        public DataTable GetDataTable()
        {
            string connectionString = "Data Source=localhost\\SQLExpress;Initial Catalog=Course_Selection_System_of_UBAY; Integrated Security=true";

            string queryString =
             $@" SELECT * 
                FROM Registration_record
                    INNER JOIN Student
                    ON Registration_record.Student_ID=Student.Student_ID
                INNER JOIN Course 
                ON Registration_record.Course_ID=Course.Course_ID
		            INNER JOIN Teacher
		            ON Teacher.Teacher_ID=Course.Teacher_ID
		            INNER JOIN Place
		            ON Place.Place_ID=Course.Place_ID
                WHERE Registration_record.Student_ID ='F9B00778-B226-4F73-B525-FB923E4DB80F';";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand command = new SqlCommand(queryString, connection);

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
                    throw;
                }
            }
        }

        private void BindDataIntoRepeater()
        {
            var dtt = _studentManagers.GetStudentCourseRecord(_ID);
            _pgsource.DataSource = dtt.DefaultView;
            _pgsource.AllowPaging = true;
            // Number of items to be displayed in the Repeater
            _pgsource.PageSize = _pageSize;
            _pgsource.CurrentPageIndex = CurrentPage;
            // Keep the Total pages in View State
            ViewState["TotalPages"] = _pgsource.PageCount;
            // Example: "Page 1 of 10"
            lblpage.Text = "Page " + (CurrentPage + 1) + " of " + _pgsource.PageCount;
            // Enable First, Last, Previous, Next buttons
            lbPrevious.Enabled = !_pgsource.IsFirstPage;
            lbNext.Enabled = !_pgsource.IsLastPage;
            lbFirst.Enabled = !_pgsource.IsFirstPage;
            lbLast.Enabled = !_pgsource.IsLastPage;

            // Bind data into repeater
            rptResult.DataSource = _pgsource;
            rptResult.DataBind();

            // Call the function to do paging
            HandlePaging();
        }

        private void HandlePaging()
        {
            var dtt = new DataTable();
            dtt.Columns.Add("PageIndex"); //Start from 0
            dtt.Columns.Add("PageText"); //Start from 1

            _firstIndex = CurrentPage - 5;
            if (CurrentPage > 5)
                _lastIndex = CurrentPage + 5;
            else
                _lastIndex = 10;

            // Check last page is greater than total page then reduced it to total no. of page is last index
            if (_lastIndex > Convert.ToInt32(ViewState["TotalPages"]))
            {
                _lastIndex = Convert.ToInt32(ViewState["TotalPages"]);
                _firstIndex = _lastIndex - 10;
            }

            if (_firstIndex < 0)
                _firstIndex = 0;

            // Now creating page number based on above first and last page index
            for (var i = _firstIndex; i < _lastIndex; i++)
            {
                var dr = dtt.NewRow();
                dr[0] = i;
                dr[1] = i + 1;
                dtt.Rows.Add(dr);
            }

            rptPaging.DataSource = dtt;
            rptPaging.DataBind();
        }

        protected void rptPaging_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (!e.CommandName.Equals("newPage")) return;
            CurrentPage = Convert.ToInt32(e.CommandArgument.ToString());
            BindDataIntoRepeater();
        }

        protected void lbFirst_Click1(object sender, EventArgs e)
        {
            CurrentPage = 0;
            BindDataIntoRepeater();
        }

        protected void lbPrevious_Click1(object sender, EventArgs e)
        {
            CurrentPage -= 1;
            BindDataIntoRepeater();
        }

        protected void lbNext_Click1(object sender, EventArgs e)
        {
            CurrentPage += 1;
            BindDataIntoRepeater();
        }

        protected void lbLast_Click1(object sender, EventArgs e)
        {
            CurrentPage = (Convert.ToInt32(ViewState["TotalPages"]) - 1);
            BindDataIntoRepeater();
        }
        protected void rptPaging_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            var lnkPage = (LinkButton)e.Item.FindControl("lbPaging");
            if (lnkPage.CommandArgument != CurrentPage.ToString()) return;
            lnkPage.Enabled = false;
            lnkPage.BackColor = Color.FromName("#F75C2F");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string course_id = this.txtCourseID.Text;
            string courseName = this.txtCourseName.Text;
            string teacher_id = this.ddlTeacher.Text;
            string StartDate = this.txtStartDate1.Text;
            string StarDate2 = this.txtStartDate2.Text;
            string place_id = this.txtPlace.Text;
            string minPrice = this.TxtPrice1.Text;
            string maxPrice = this.TxtPrice2.Text;

            string template = "?Page=1";

            if (!string.IsNullOrEmpty(course_id))
                template += "&course_id=" + course_id;

            if (!string.IsNullOrEmpty(courseName))
                template += "&courseName=" + courseName;

            if (!string.IsNullOrEmpty(teacher_id))
                template += "&teacher_id=" + teacher_id;

            if (!string.IsNullOrEmpty(place_id))
                template += "&place_id=" + place_id;

            //Response.Redirect("StudentCourseRecord.aspx" + template);

            BindDataIntoRepeater();

        }

        protected void NextMonth_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).CommandName)
            {
                case "Next":
                    datetime = datetime.AddMonths(1);
                    break;
                case "Previous":
                    datetime = datetime.AddMonths(-1);
                    break;
            }
            
            Response.Redirect($"StudentCourseRecord.aspx?datetime={datetime.ToString("yyyy/MM/dd")}");

            CreateCalendar();
        }



        protected void Calendar_UpdateCommand(object source, DataListCommandEventArgs e)
        {
            DataTable dt_calendar = new DataTable();
            //DataTable dt_course = new DataTable();
            dt_calendar.Columns.Add(new DataColumn("Date"));
            dt_calendar.Columns.Add(new DataColumn("Course"));
            dt_calendar.Columns.Add(new DataColumn("Place"));
            dt_calendar.Columns.Add(new DataColumn("StartTime"));
            int ii = (int)DateTime.Now.AddDays(-DateTime.Now.Day + 1).DayOfWeek;
            //填滿空格
            for (int i = 0; i < ii; i++)
                dt_calendar.Rows.Add("");

            //產生該月的日期列表
            for (int i = 1; i <= DateTime.DaysInMonth(datetime.Year, datetime.Month); i++)
            {
                DataRow dr = dt_calendar.NewRow();
                dr[0] = i.ToString();
                dr[1] = "";
                dr[2] = "";
                dr[3] = "";

                dt_calendar.Rows.Add(dr);
            }

            //資料綁定
            Calendar.DataSource = dt_calendar;
            Calendar.DataBind();

            //設定當天顏色
            Calendar.Items[DateTime.Now.Day + ii - 1].BackColor = Color.LightPink;
        }

        protected void CreateCalendar()//int InYear, int InMonth)
        {
            DataTable dt_course = _studentManagers.GetStudentCourseRecord(_ID);
            DataTable dt_calendar = new DataTable();

            dt_calendar.Columns.Add(new DataColumn("Date"));
            dt_calendar.Columns.Add(new DataColumn("Course"));
            dt_calendar.Columns.Add(new DataColumn("Place"));
            dt_calendar.Columns.Add(new DataColumn("StartTime"));
            

            int ii = (int)datetime.AddDays(-datetime.Day + 1).DayOfWeek;
            //填滿空格
            for (int i = 0; i < ii; i++)
                dt_calendar.Rows.Add("");

            //產生該月的日期列表
            for (int i = 1; i <= DateTime.DaysInMonth(datetime.Year, datetime.Month); i++)
            {
                DataRow dr = dt_calendar.NewRow();
                dr[0] = i.ToString();
                List<TempClass> _tempClassList = new List<TempClass>();
                //[1,2,3]

                foreach (DataRow r in dt_course.Rows)
                {
                    //if (DateTime.Parse(r["StartDate"].ToString()) = _currentDay)
                    //{
                    //dr[1] = r["C_Name"].ToString();
                    //dr[2] = r["Place_Name"].ToString();
                    //dr[3] = r["StartTime"].ToString();
                    //}
                    TempClass _tempclass = new TempClass((DateTime)r["StartDate"], (DateTime)r["EndDate"], $"{r["C_Name"]} {r["Place_Name"]} {r["StartTime"]}");
                    if (!_tempClassList.Contains(_tempclass))
                        _tempClassList.Add(_tempclass);
                }
                string _tmpstr = string.Empty;

                foreach (TempClass tempclass in _tempClassList)
                {
                    if (tempclass.Check(DateTime.Parse($"{datetime.Year}/{datetime.Month}/{i}")))
                    {
                        _tmpstr += $"{tempclass.ClassName}<br>";
                    }
                }
                dr[1] = _tmpstr;
                dt_calendar.Rows.Add(dr);
            }

            //資料綁定
            Calendar.DataSource = dt_calendar;
            Calendar.DataBind();

            //設定當天顏色
            if (datetime.ToString("yyyy/MM") == DateTime.Now.ToString("yyyy/MM"))
                Calendar.Items[datetime.Day + ii - 1].BackColor = Color.LightPink;
        }
    }

    class TempClass
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public string ClassName { get; set; }

        public TempClass() { }
        public TempClass(DateTime startdate, DateTime enddate, string classname)
        {
            StartDate = startdate;
            EndDate = enddate;
            ClassName = classname;
            DayOfWeek = startdate.DayOfWeek;
        }

        public bool Check(DateTime date)
        {
            if (date.DayOfWeek != DayOfWeek)
                return false;
            if (date >= StartDate && date <= EndDate)
                return true;
            return false;
        }
    }

}