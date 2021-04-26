using CoreProject.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ubay_CourseRegistration.Utility;

namespace Ubay_CourseRegistration.Managers
{
    public partial class Ad_Region : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void CreateAdmin_Click(object sender, EventArgs e)
        {
            Account_summaryModel asmodel = new Account_summaryModel();
            AccountModel acmodel = new AccountModel();
            acmodel.Acc_sum_ID = Guid.NewGuid();
            asmodel.firstname = this.txtFirstname.Text;
            asmodel.lastname = this.txtLastname.Text;
            asmodel.department = this.txtDepartment.Text;
            acmodel.Account = this.txtAccount.Text;
            acmodel.password = this.txtPassword.Text;
            asmodel.Pwdcheck = this.txtPwdcheck.Text;
            asmodel.datetime = DateTime.Now; // 取得現在時間
            string createtime = asmodel.datetime.ToString("yyyy/MM/dd HH:mm:ss"); // 轉成字串
            acmodel.Type = true;

            SqlConnection conn = new SqlConnection("Data Source=localhost\\SQLExpress;Initial Catalog=Course_Selection_System_of_UBAY; Integrated Security=true");
            conn.Open();

            SqlCommand bb = new SqlCommand("Select * From Account_summary Where Account='" + txtAccount.Text + "'", conn);
            SqlDataReader ha = bb.ExecuteReader();

            if (string.IsNullOrEmpty(asmodel.firstname) || string.IsNullOrEmpty(asmodel.lastname) ||
                string.IsNullOrEmpty(asmodel.department) || string.IsNullOrEmpty(acmodel.Account) ||
                string.IsNullOrEmpty(acmodel.password) || string.IsNullOrEmpty(asmodel.Pwdcheck))
            {
                Response.Write("<script>alert('所有欄位皆為必填，不可為空!');</script>");
            }
            else if (acmodel.password != asmodel.Pwdcheck)
            {
                Response.Write("<script>alert('確認密碼不一致，請重新輸入');</script>");
            }
            else if (ha.Read())
            {
                Response.Write("<script>alert('已有相同帳號，請換一個重新輸入');</script>");
            }
            else
            {
                ManagerManagers.InsertAdminTablel(acmodel, asmodel, createtime);
            }
        }
    }
}
