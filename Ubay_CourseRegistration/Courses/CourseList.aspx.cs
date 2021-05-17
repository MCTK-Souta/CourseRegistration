using CoreProject.Managers;
using CoreProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ubay_CourseRegistration.Managers;

namespace Ubay_CourseRegistration.Courses
{
    public partial class CourseList : System.Web.UI.Page
    {

        StudentManagers _studentManagers = new StudentManagers();

        CourseManagers _courseManagers = new CourseManagers();

        //使用PagedDataSource物件實現課程資料repeater分頁
        readonly PagedDataSource _pgsource = new PagedDataSource();
        static DateTime datetime = DateTime.Now;
        //課程資料repeater數字首尾頁索引 
        int _firstIndex, _lastIndex;
        //設定課程資料repeater項目數量
        private int _pageSize = 10;



        protected void Page_Load(object sender, EventArgs e)
        {


            if (Page.IsPostBack) return;

            //查詢教師的下拉選單內容方法
            _studentManagers.ReadTeacherTable(ref ddlTeacher);


            BindDataIntoRepeater();


            var _post = Request.QueryString["datetime"];
            if (_post != null)
                datetime = DateTime.Parse(_post);
            monthOnCalendar.Text = $"{datetime.ToString("yyyy/MM")}月課程紀錄";
            CreateCalendar();
        }

        //用來記錄課程資料repeater當前頁
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

        private void BindDataIntoRepeater()
        {

            var dtt = _courseManagers.SearchAllCourse(txtCourseID.Text, txtCourseName.Text, txtStartDate1.Text, txtStartDate2.Text, txtPlace.Text, TxtPrice1.Text, TxtPrice2.Text, ddlTeacher.SelectedValue,ddlCourseStatus.SelectedValue);
            _pgsource.DataSource = dtt.DefaultView;
            //啟用分頁
            _pgsource.AllowPaging = true;
            //要在Repeater顯示的項目數 
            _pgsource.PageSize = _pageSize;
            //當前頁索引
            _pgsource.CurrentPageIndex = CurrentPage;
            //維持顯示 Total pages
            ViewState["TotalPages"] = _pgsource.PageCount;
            // 顯示現在頁數之於總頁數  Example: "Page 1 of 10"
            lblpage.Text = "Page " + (CurrentPage + 1) + " of " + _pgsource.PageCount;
            //課程資料repeater的First, Last, Previous, Next 按鈕的使用控制
            lbPrevious.Enabled = !_pgsource.IsFirstPage;
            lbNext.Enabled = !_pgsource.IsLastPage;
            lbFirst.Enabled = !_pgsource.IsFirstPage;
            lbLast.Enabled = !_pgsource.IsLastPage;

            // Bind資料進Repeater
            rptResult.DataSource = _pgsource;
            rptResult.DataBind();

            HandlePaging();
        }

        #region 課程Repeater下方按鈕功能
        //處理課程Repeater分頁頁碼
        private void HandlePaging()
        {
            var dtt = new DataTable();
            dtt.Columns.Add("PageIndex");
            dtt.Columns.Add("PageText");

            //設定頁數頁碼
            _firstIndex = CurrentPage - 5;
            if (CurrentPage > 5)
                _lastIndex = CurrentPage + 5;
            else
                _lastIndex = 10;

            // 檢查最後一頁是否大於總頁數，然後將其設為總頁數
            if (_lastIndex > Convert.ToInt32(ViewState["TotalPages"]))
            {
                _lastIndex = Convert.ToInt32(ViewState["TotalPages"]);
                _firstIndex = _lastIndex - 10;
            }
            //如果第一頁索引小於0時 將他設回0
            if (_firstIndex < 0)
                _firstIndex = 0;

            //根據前面的first 和 last page索引，建立頁碼
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
            lnkPage.BackColor = Color.FromName("#F75C2F");//設定當前分頁頁碼顏色
        }

        #endregion


        //搜尋課程button
        public void btnSearch_Click(object sender, EventArgs e)
        {
            rptResult.DataSource = _courseManagers.SearchAllCourse(
                            txtCourseID.Text,
                            txtCourseName.Text,
                            txtStartDate1.Text,
                            txtStartDate2.Text,
                            txtPlace.Text,
                            TxtPrice1.Text,
                            TxtPrice2.Text,
                            ddlTeacher.SelectedValue,
                            ddlCourseStatus.SelectedValue
                            ); ;
            BindDataIntoRepeater();
            CreateCalendar();
            rptResult.DataBind();

        }

        //月曆的上、下一月功能
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

            monthOnCalendar.Text = $"{datetime.ToString("yyyy/MM")}月課程紀錄";

            CreateCalendar();
        }


        //建立月曆表格內容 int InYear, int InMonth
        protected void CreateCalendar()
        {

            DataTable dt_course = _courseManagers.SearchAllCourse(txtCourseID.Text, txtCourseName.Text, txtStartDate1.Text, txtStartDate2.Text, txtPlace.Text, TxtPrice1.Text, TxtPrice2.Text, ddlTeacher.SelectedValue,ddlCourseStatus.SelectedValue);
            DataTable dt_calendar = new DataTable();

            dt_calendar.Columns.Add(new DataColumn("Date"));
            dt_calendar.Columns.Add(new DataColumn("Course"));
            dt_calendar.Columns.Add(new DataColumn("Place"));
            dt_calendar.Columns.Add(new DataColumn("StartTime"));


            int j = (int)datetime.AddDays(-datetime.Day + 1).DayOfWeek;
            //填滿空格
            for (int i = 0; i < j; i++)
                dt_calendar.Rows.Add("");

            //產生該月的日期列表
            for (int i = 1; i <= DateTime.DaysInMonth(datetime.Year, datetime.Month); i++)
            {
                DataRow dr = dt_calendar.NewRow();
                dr[0] = i.ToString();
                List<StudentCourseTimeModel> _tempClassList = new List<StudentCourseTimeModel>();


                foreach (DataRow r in dt_course.Rows)
                {
                    Regex regex = new Regex(@"\d{2}:\d{2}");
                    StudentCourseTimeModel _tempclass = new StudentCourseTimeModel((DateTime)r["StartDate"], (DateTime)r["EndDate"], $"{r["C_Name"]} {r["Place_Name"]} {regex.Match(r["StartTime"].ToString())}");
                    if (!_tempClassList.Contains(_tempclass))
                        _tempClassList.Add(_tempclass);
                }
                string _tmpstr = string.Empty;

                foreach (StudentCourseTimeModel tempclass in _tempClassList)
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
                Calendar.Items[datetime.Day + j - 1].BackColor = Color.LightPink;
        }
    }
}