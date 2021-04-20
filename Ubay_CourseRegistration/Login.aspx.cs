using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ubay_CourseRegistration;

namespace Ubay_CourseRegistration
{
    public partial class Login : System.Web.UI.Page
    {
            private string _goToUrl = "";
        private string _goTostUrl = "student/student_main.aspx";

        protected void Page_Load(object sender, EventArgs e)
        {
            _goToUrl = Request.RawUrl;

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


            if (isSuccess)
            {
                this.ltMessage.Text = "Success";
                this.PlaceHolder1.Visible = false;

                Response.Redirect(this._goTostUrl);

            }
            else
            {
                this.ltMessage.Text = "Fail";
                this.PlaceHolder1.Visible = true;
            }
        }

    }
}
