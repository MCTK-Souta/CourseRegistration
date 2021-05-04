using CoreProject.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ubay_CourseRegistration.Managers
{
    public partial class ManagerInfo : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            Guid temp;
            Guid.TryParse(Request.QueryString["ID"], out temp);

            //this.txtAccount.Enabled = false;
            //this.txtAccount.BackColor = System.Drawing.Color.DarkGray;
            this.LoadAccount(temp);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void UpdateAdmin_Click(object sender, EventArgs e)
        {
            Account_summaryModel asmodel = new Account_summaryModel();
            AccountModel acmodel = new AccountModel();
            asmodel.firstname = this.txtFirstname.Text;
            asmodel.lastname = this.txtLastname.Text;
            asmodel.department = this.txtDepartment.Text;
            acmodel.Account = this.txtAccount.Text;
            string oldpassword = this.txtPassword.Text;
            acmodel.password = this.txtNewPassword.Text;
            asmodel.Pwdcheck = this.txtPwdcheck.Text;
            asmodel.datetime = DateTime.Now; // 取得現在時間
            string updatetime = asmodel.datetime.ToString("yyyy/MM/dd HH:mm:ss"); // 轉成字串
            acmodel.Type = true;

            Guid editor;
            editor = (Guid)Session["Acc_sum_ID"];

            SqlConnection conn = new SqlConnection("Data Source=localhost\\SQLExpress;Initial Catalog=Course_Selection_System_of_UBAY; Integrated Security=true");
            conn.Open();
            var Managers = new DBAccountManager();

            SqlCommand passwordcheck = new SqlCommand("Select * From Account_summary Where password = '" + txtPassword.Text + "'", conn);
            SqlDataReader pwdchk = passwordcheck.ExecuteReader();

            if (string.IsNullOrEmpty(asmodel.firstname) || string.IsNullOrEmpty(asmodel.lastname) ||
                string.IsNullOrEmpty(asmodel.department) || string.IsNullOrEmpty(acmodel.Account) ||
                string.IsNullOrEmpty(acmodel.password) || string.IsNullOrEmpty(asmodel.Pwdcheck) ||
                string.IsNullOrEmpty(this.txtPassword.Text))
            {
                this.WarningMsg.Text = "所有欄位皆為必填，不可為空!";
            }
            else if (Managers.GetAccount(this.txtAccount.Text.Trim()) != null)
            {
                this.WarningMsg.Text = "已有相同帳號，請重新輸入";
            }
            else if (!pwdchk.Read())
            {
                this.WarningMsg.Text = "舊密碼輸入錯誤，請重新輸入";
            }
            else if (acmodel.password != asmodel.Pwdcheck)
            {
                this.WarningMsg.Text = "新密碼確認不一致，請重新輸入";
            }
            else
            {
                ManagerManagers.UpdateAdminTablel(acmodel, asmodel, updatetime, editor);
            }
        }


        private void LoadAccount(Guid updater)
        {
            updater = (Guid)Session["Acc_sum_ID"];

            var manager = new ManagerManagers();
            var model = manager.GetAccountViewModel(updater);

            if (model == null)
                Response.Redirect("~/SystemAdmin/MemberList.aspx");

            this.txtFirstname.Text = model.firstname;
            this.txtLastname.Text = model.lastname;
            this.txtDepartment.Text = model.department;
            this.txtAccount.Text = model.Account;

        }
    }
}