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


            Guid temp = Guid.NewGuid();

            string firstname = this.txtFirstname.Text;
            string lastname = this.txtLastname.Text;
            string department = this.txtDepartment.Text;
            string account = this.txtAccount.Text;
            string Password = this.txtPassword.Text;
            string Pwdcheck = this.txtPwdcheck.Text;
            DateTime datetime = DateTime.Now; // 取得現在時間
            string createtime = datetime.ToString("yyyy/MM/dd HH:mm:ss"); // 轉成字串
            int type = 1;

            SqlConnection conn = new SqlConnection("Data Source=localhost\\SQLExpress;Initial Catalog=Course_Selection_System_of_UBAY; Integrated Security=true");
            conn.Open();

            SqlCommand bb = new SqlCommand("Select * From Account_summary Where Account='" + txtAccount.Text + "'", conn);
            SqlDataReader ha = bb.ExecuteReader();

            if (string.IsNullOrEmpty(firstname) || string.IsNullOrEmpty(lastname) || string.IsNullOrEmpty(department) || string.IsNullOrEmpty(account) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Pwdcheck))
            {
                Response.Write("<script>alert('所有欄位皆為必填，不可為空!');</script>");
            }
            else if (Password != Pwdcheck)
            {
                Response.Write("<script>alert('確認密碼不一致，請重新輸入');</script>");
            }
            else if (ha.Read())
            {
                Response.Write("<script>alert('已有相同帳號，請換一個重新輸入');</script>");
            }
            else
            {
                ManagerManagers.InsertAdminTablel(temp.ToString(), firstname, lastname, department, account, Password, type, createtime);
            }


        }
    }
}
