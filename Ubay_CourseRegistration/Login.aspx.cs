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

        protected void Page_Load(object sender, EventArgs e)
        {
            //_goToStudent = Request.RawUrl;

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
            SqlConnection con = new SqlConnection("Data Source=localhost\\SQLExpress;Initial Catalog=Course_Selection_System_of_UBAY; Integrated Security=true");
            con.Open();

            SqlCommand Typecheck = new SqlCommand("Select * From Account_summary Where Type=1 AND Account='" + txtAccount.Text + "'", conn);
            SqlCommand Typcheck = new SqlCommand("Select * From Account_summary Where Type=2 AND Account='" + txtAccount.Text + "'", con);
            SqlDataReader Typechk = Typecheck.ExecuteReader();
            SqlDataReader Typchk = Typcheck.ExecuteReader();

            
            if (isSuccess)
            {
                this.ltMessage.Text = "Success";
                this.PlaceHolder1.Visible = false;

                if (Typechk.Read())
                {
                    Response.Redirect(this._goToManager);
                }
                if (Typchk.Read())
                {
                    Response.Redirect(this._goToStudent);
                }


            }
            else
            {
                this.ltMessage.Text = "Fail";
                this.PlaceHolder1.Visible = true;
            }
        }

    }
}
