using CoreProject.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ubay_CourseRegistration;

namespace Ubay_CourseRegistration
{
    public partial class Login : System.Web.UI.Page
    {
        private string _goToManager = "Managers/ManagerMainPage.aspx";
        private string _goToStudent = "Students/StudentMainPage.aspx";

        private const string _sessionKey = "IsLogined";
        private const string _sessionKey_Account = "Account";
        protected void Page_Load(object sender, EventArgs e)
        {


            if (LoginHelper.HasLogined())
            {
                this.PlaceHolder1.Visible = false;
            }

        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            string acc = this.txtAccount.Text;
            string pwd = this.txtPassword.Text;


            bool isSuccess = LoginHelper.TryLogin(acc, pwd);

            
            SqlConnection conn = new SqlConnection("Data Source=localhost\\SQLExpress;Initial Catalog=Course_Selection_System_of_UBAY; Integrated Security=true");
            conn.Open();
            SqlCommand Typecheck = new SqlCommand("Select * From Account_summary Where Type=1 AND Account='" + txtAccount.Text + "'", conn);
            SqlDataReader Typechk = Typecheck.ExecuteReader();
            
            if (isSuccess)
            {
                this.ltMessage.Text = "Success";
                this.PlaceHolder1.Visible = false;

                //將帳號存入Session
                Session["Account"] = txtAccount.Text;
                Session["IsLogined"] = true;
                Session["Acc_sum_ID"] = DBAccountManager.GetUserAccount(txtAccount.Text).Rows[0]["Acc_sum_ID"];

                if (Typechk.Read())
                {
                    Response.Redirect(this._goToManager);
                }
                else
                {
                    Response.Redirect(this._goToStudent);
                }


            }
            else
            {
                this.ltMessage.Text = "帳號或密碼錯誤，請重新輸入!";
                this.PlaceHolder1.Visible = true;
            }
        }

    }
}
