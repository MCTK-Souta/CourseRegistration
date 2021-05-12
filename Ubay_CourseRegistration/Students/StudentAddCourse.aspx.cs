﻿using CoreProject.Managers;
using CoreProject.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ubay_CourseRegistration.Students
{
    public partial class StudentAddCourse : System.Web.UI.Page
    {
        StudentManagers _studentManagers = new StudentManagers();
        //使用PagedDataSource物件實現課程資料repeater分頁
        readonly PagedDataSource _pgsource = new PagedDataSource();
        static DateTime datetime = DateTime.Now;
        //課程資料repeater數字首尾頁索引 
        int _firstIndex, _lastIndex;
        //設定課程資料repeater項目數量
        private int _pageSize = 10;
        //用來裝目前登入學生帳號
        string _ID;




        protected void Page_Load(object sender, EventArgs e)
        {
            
            _ID = Session["Acc_sum_ID"].ToString();
            if (Page.IsPostBack) return;

            //查詢教師的下拉選單內容方法
            _studentManagers.ReadTeacherTable(ref ddlTeacher);

            BindDataIntoRepeater();

            var _post = Request.QueryString["datetime"];

            if (_post != null)
                datetime = DateTime.Parse(_post);
            monthOnCalendar.Text = $"{datetime.ToString("yyyy/MM")}月課程紀錄";
            CreateCalendar();
            dt_cart = new DataTable();
            dt_cart.Columns.Add("Course_ID", typeof(string));
            dt_cart.Columns.Add("C_Name", typeof(string));
            dt_cart.Columns.Add("Price", typeof(int));
            _studentManagers.ClearCart(_ID);
        }

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
            var dtt = _studentManagers.StudentAddCourse(_ID);
            _pgsource.DataSource = dtt.DefaultView;
            _pgsource.AllowPaging = true;
            // 要在Repeater顯示的項目數 
            _pgsource.PageSize = _pageSize;
            _pgsource.CurrentPageIndex = CurrentPage;
            //維持顯示 Total pages
            ViewState["TotalPages"] = _pgsource.PageCount;
            // 顯示現在頁數之於總頁數  Example: "Page 1 of 10"
            lblpage.Text = "Page " + (CurrentPage + 1) + " of " + _pgsource.PageCount;
            //First, Last, Previous, Next 按鈕的使用控制
            lbPrevious.Enabled = !_pgsource.IsFirstPage;
            lbNext.Enabled = !_pgsource.IsLastPage;
            lbFirst.Enabled = !_pgsource.IsFirstPage;
            lbLast.Enabled = !_pgsource.IsLastPage;
            dt_courses = dtt;
            // Bind資料進Repeater
            rptResult.DataSource = _pgsource;
            rptResult.DataBind();

            //呼叫Repeater分頁
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
            lnkPage.BackColor = Color.FromName("#F75C2F");
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

            Response.Redirect($"StudentAddCourse.aspx?datetime={datetime.ToString("yyyy/MM/dd")}");

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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            searchCouser();
        }

        //查詢新增課程
        void searchCouser()
        {
            dt_courses = _studentManagers.SearchCouserAdd(
             _ID,
            txtCourseID.Text,
            txtCourseName.Text,
            txtStartDate1.Text,
            txtStartDate2.Text,
            txtPlace.Text,
            TxtPrice1.Text,
            TxtPrice2.Text,
            ddlTeacher.SelectedValue
              );
            rptResult.DataSource = dt_courses;
            rptResult.DataBind();
        }




        #region 新增的項目
        static DataTable dt_courses = new DataTable();
        static DataTable dt_cart = new DataTable();
        protected void ShowRemark(object sender, CommandEventArgs e)
        {
            DataRow dr = GetCurrentCourse(e.CommandArgument.ToString())[0];
            //Remarks.Text = (string)dr["Remarks"];
            //改三元運算讓如果簡介是NULL也可以有通知
            Remarks.Text = string.IsNullOrEmpty(dr["Remarks"].ToString())
            ? "此課程暫無簡介"
            : (string)dr["Remarks"];
        }
        protected void AddCourseCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox ckbox = (CheckBox)sender;
            DataRow result = GetCurrentCourse(ckbox.Text)[0];
           
            if (ckbox.Checked )
            {
                DataRow dr = dt_cart.NewRow();
                dr["Course_ID"] = result["Course_ID"];
                dr["C_Name"] = result["C_Name"];
                dr["Price"] = result["Price"];
                int results = dt_cart.AsEnumerable().Where(d =>
                {
                    return d["Course_ID"].ToString() == result["Course_ID"].ToString();
                }).ToList().Count;
                if (results == 0)
                    dt_cart.Rows.Add(dr);
            }
            else
            {
                DataRow[] rows = new DataRow[dt_cart.Rows.Count];
                dt_cart.Rows.CopyTo(rows, 0);
                foreach (DataRow row in rows)
                    if (row["Course_ID"].ToString() == ckbox.Text)
                        dt_cart.Rows.Remove(row);
            }
            RepeaterCart.DataSource = dt_cart;
            RepeaterCart.DataBind();
        }
        protected List<DataRow> GetCurrentCourse(string Course_ID)
        {
            List<DataRow> results = dt_courses.AsEnumerable().Where(d =>
            {
                return d["Course_ID"].ToString() == Course_ID;
            }).ToList();
            return results;
        }
        protected int TotalPrice()
        {
            int totalprice = 0;
            foreach (DataRow dr in dt_cart.Rows)
                totalprice += (int)dr["Price"];
            return totalprice;
        }
        protected void btnCheckout_Click(object sender, EventArgs e)
        {

            if (!_studentManagers.AddCart(_ID, dt_cart, "Cart"))
                return;
            Response.Redirect("~/Students/StudentCheckout.aspx");



        }
        #endregion


        protected void CreateCalendar()//int InYear, int InMonth)
        {
            DataTable dt_course = _studentManagers.StudentAddCourse(_ID);
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
                List<StudentCourseTimeModel> _tempClassList = new List<StudentCourseTimeModel>();
                //[1,2,3]

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
                Calendar.Items[datetime.Day + ii - 1].BackColor = Color.LightPink;
        }


    }
}