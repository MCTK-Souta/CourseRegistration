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
    public partial class Ad_Region : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.IsUpdateMode())
            {
                Guid temp;
                Guid.TryParse(Request.QueryString["Manager_ID"], out temp);
                this.LoadAccount(temp);
                this.LB1.Text = "修改管理人資料";
                this.Regis.Text = "確認修改";
            }
            else
            {
                this.Label2.Visible = false;
                this.Label3.Visible = false;
                this.Label4.Visible = false;
            }
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

            Guid Creator;
            Creator = (Guid)Session["Acc_sum_ID"];
            SqlConnection conn = new SqlConnection("Data Source=localhost\\SQLExpress;Initial Catalog=Course_Selection_System_of_UBAY; Integrated Security=true");
            if (this.IsUpdateMode())
            {
                Guid editor;
                editor = (Guid)Session["Acc_sum_ID"];

                var manager = new ManagerManagers();
                var model = manager.GetAccountViewModel(editor);


                conn.Open();
                var Managers = new ManagerManagers();

                SqlCommand passwordcheck = new SqlCommand("Select * From Account_summary Where password = '" + txtPassword.Text + "'", conn);
                SqlDataReader pwdchk = passwordcheck.ExecuteReader();

                this.WarningMsg.Text = "";
                if (string.IsNullOrEmpty(this.txtFirstname.Text))
                {
                    this.WarningMsg.Text = "姓氏欄位不可為空!";
                    return;
                }
                else if (string.IsNullOrEmpty(this.txtLastname.Text))
                {
                    this.WarningMsg.Text = "名字欄位不可為空!";
                    return;
                }
                else if (string.IsNullOrEmpty(this.txtDepartment.Text))
                {
                    this.WarningMsg.Text = "單位欄位不可為空!";
                    return;
                }
                else if (string.IsNullOrEmpty(this.txtAccount.Text))
                {
                    this.WarningMsg.Text = "帳號不可為空!";
                    return;
                }
                else if (model.Account != this.txtAccount.Text) //如果帳號欄位裡的值，並不是此登入者帳號原先的帳號，進入判斷式(判斷使用者是否欲更改帳號，若是，進入內層判斷式)
                {
                    if (Managers.GetAccount(this.txtAccount.Text.Trim()) != null) //判斷欲更改的新帳號是否已有相同帳號
                    {
                        this.WarningMsg.Text = "已有相同帳號，請重新輸入";
                        return;
                    }
                }
                else if (!string.IsNullOrEmpty(this.txtPwdcheck.Text) ||
                    !string.IsNullOrEmpty(this.txtPassword.Text)) //如果舊密碼、新密碼、確認新密碼三個欄位中，任一欄位不是空值(代表使用者想改密碼)，進入判斷式
                {
                    if (!pwdchk.Read())
                    {
                        this.WarningMsg.Text = "舊密碼輸入錯誤，請重新輸入";
                        return;
                    }
                    else if (acmodel.password != asmodel.Pwdcheck)
                    {
                        this.WarningMsg.Text = "新密碼確認不一致，請重新輸入";
                        return;
                    }
                }
                else if (string.IsNullOrEmpty(this.oldPassword.Text) &&
                    string.IsNullOrEmpty(this.txtPwdcheck.Text) &&
                    string.IsNullOrEmpty(this.txtPassword.Text)) //如果舊密碼、新密碼、確認新密碼三個欄位，所有欄位皆為空值(代表使用者不想改密碼)，將使用者的舊密碼代入欄位裡
                {
                    acmodel.password = model.password;
                }
                ManagerManagers.UpdateAdminTablel(acmodel, asmodel, createtime, editor);
            }
            else
            {
                conn.Open();

                SqlCommand bb = new SqlCommand("Select * From Account_summary Where Account='" + txtAccount.Text + "'", conn);
                SqlDataReader ha = bb.ExecuteReader();

                if (string.IsNullOrEmpty(asmodel.firstname) || string.IsNullOrEmpty(asmodel.lastname) ||
                    string.IsNullOrEmpty(asmodel.department) || string.IsNullOrEmpty(acmodel.Account) ||
                    string.IsNullOrEmpty(acmodel.password) || string.IsNullOrEmpty(asmodel.Pwdcheck))
                {
                    this.WarningMsg.Text = "所有欄位皆為必填，不可為空!";
                }
                else if (acmodel.password != asmodel.Pwdcheck)
                {
                    this.WarningMsg.Text = "確認密碼不一致，請重新輸入";
                }
                else if (ha.Read())
                {
                    this.WarningMsg.Text = "已有相同帳號，請重新輸入";
                }
                else
                {
                    ManagerManagers.InsertAdminTablel(acmodel, asmodel, createtime, Creator);
                    this.WarningMsg.Text = "新增成功";
                }
            }
        }

        private void LoadAccount(Guid updater)
        {

            if (string.IsNullOrEmpty(Request.QueryString["Manager_ID"]))
            {
                updater = (Guid)Session["Acc_sum_ID"];
            }

            var manager = new ManagerManagers();
            var model = manager.GetAccountViewModel(updater);

            if (model == null)
                Response.Redirect("~/SystemAdmin/MemberList.aspx");

            this.txtFirstname.Text = model.firstname;
            this.txtLastname.Text = model.lastname;
            this.txtDepartment.Text = model.department;
            this.txtAccount.Text = model.Account;

        }

        private bool IsUpdateMode()
        {
            string qsID = Request.QueryString["Manager_ID"];

            Guid temp;
            if (Guid.TryParse(qsID, out temp))
                return true;

            return false;
        }

        protected void Turnback_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Managers/ManagerSearch.aspx");
        }
    }
}
