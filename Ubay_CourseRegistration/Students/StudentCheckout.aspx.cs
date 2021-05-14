using CoreProject.Managers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ubay_CourseRegistration.Students
{
    public partial class StudentCheckout : System.Web.UI.Page
    {
        string _id;
        StudentManagers _studentManagers = new StudentManagers();
        DataTable dt_cart;
        protected int TotalPrice = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            _id = Session["Acc_sum_ID"].ToString();
            //取得購物車列表
            dt_cart = _studentManagers.GetCart(_id);
            //計算總金額
            foreach (DataRow dr in dt_cart.Rows)
                TotalPrice += Convert.ToInt32(dr["Price"]);
        }

        protected void CheckOut(object sender, EventArgs e)
        {


            //比對的規則
            Regex regexCreditCard = new Regex(@"\d{4}-\d{4}-\d{4}-\d{4}");
            Regex regexMonthYear = new Regex(@"\d{2}/\d{2}");
            Regex regexCVN = new Regex(@"\d{3}");
            string cardNumber = $"{CreditCard1.Text}{CreditCard2.Text}{CreditCard3.Text}{CreditCard4.Text}";
            StudentManagers _studentManagers = new StudentManagers();


            if (regexCreditCard.Match($"{CreditCard1.Text}-{CreditCard2.Text}-{CreditCard3.Text}-{CreditCard4.Text}").Length < 1)
            {
                ShowAlert("信用卡卡號錯誤");
                return;
            }
            if (!_studentManagers.CheckCreditCardNo(cardNumber))
            {
                ShowAlert("信用卡格式錯誤");
                return;
            }
            if (regexMonthYear.Match($"{Month.Text}/{Year.Text}").Length < 1)
            {
                ShowAlert("月/年錯誤");
                return;
            }
            if (regexCVN.Match($"{CVN.Text}").Length < 1)
            {
                ShowAlert("安全碼錯誤");
                return;
            }
            //資料庫操作
            _studentManagers.studentCheckoutCourse(_id, dt_cart, DateTime.Now);
            _studentManagers.ClearCart(_id);
            //轉跳到StudentCourseRecord.aspx
            Response.Write($"<script>confirm('選課成功');location.href = 'StudentCourseRecord.aspx';</script>");
        }

        /// <summary>
        /// 顯示訊息
        /// </summary>
        /// <param name="Msg">訊息內容</param>
        void ShowAlert(string Msg)
        {
            Response.Write($"<script>alert('{Msg}')</script>");
        }
    }
}