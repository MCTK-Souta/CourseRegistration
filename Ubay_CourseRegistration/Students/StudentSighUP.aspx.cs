using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreProject.Managers;
using CoreProject.Models;

namespace Ubay_CourseRegistration.Students
{
    public partial class StudentSighUP : System.Web.UI.Page
    {
        private string[] _allowExts = { ".jpg", ".png", ".bmp", ".gif" };
        private string _saveFolder = "~/FileDownload/";
        protected void Page_Load(object sender, EventArgs e)
        {
            UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
        }

        protected void Button_StRegion(object sender, EventArgs e)
        {
            var Managers = new StudentManagers();
            StudentInfoModel stmodel = new StudentInfoModel();
            AccountModel acmodel = new AccountModel();

            if (this.fname.Text != string.Empty && this.lname.Text != string.Empty && this.idn.Text != string.Empty &&
                this.pwd.Text != string.Empty && this.repwd.Text != string.Empty && this.gender.Text != string.Empty &&
                this.birthday.Text != string.Empty && this.email.Text != string.Empty && this.phone.Text != string.Empty &&
                this.address.Text != string.Empty && this.experience.Text != string.Empty && this.exyear.Text != string.Empty &&
                this.education.Text != string.Empty)
            {
                StudentManagers managers = new StudentManagers();
                stmodel.S_FirstName = this.fname.Text.Trim();
                stmodel.S_LastName = this.lname.Text.Trim();
                bool idnc = Managers.Check(this.idn.Text);
                if (idnc == true)
                {
                    stmodel.Idn = this.idn.Text.Trim();

                }
                else
                {
                    this.lbmsg.Text = "身分證格式錯誤";
                    this.lbmsg.Visible = true;
                    return;
                }

                if (managers.GetAccount(this.idn.Text.Trim()) != null)
                {
                    stmodel.Idn = null;
                    this.lbmsg.Text = "帳號已重複註冊";
                    this.lbmsg.Visible = true;
                    return;
                }
                else
                {
                    acmodel.Account = this.idn.Text.Trim();
                }


                if (this.pwd.Text == this.repwd.Text)
                {
                    acmodel.password = this.pwd.Text.Trim();
                }
                else if(this.pwd.Text!=this.repwd.Text)
                {
                    this.lbmsg.Text = "密碼與確認密碼不一致";
                    return;
                }
                else
                {
                    this.lbmsg.Text = "密碼不可為空";
                    return;
                }
                stmodel.gender = this.gender.Text;
                stmodel.Birthday = Convert.ToDateTime(this.birthday.Text);
                stmodel.Email = this.email.Text.Trim();
                stmodel.CellPhone = this.phone.Text.Trim();
                stmodel.Address = this.address.Text.Trim();
                stmodel.Experience = this.experience.Text;
                stmodel.ExYear = this.exyear.Text;
                stmodel.Education = this.education.Text;

                stmodel.School_ID = this.school.Text;
                stmodel.PassNumber = this.psn.Text.Trim();

                stmodel.PassPic = this.GetNewFileName(this.passpic);
                stmodel.b_date = DateTime.Now;

                Managers.StudentSigh_UP(stmodel, acmodel);

                Response.Redirect("~/Login.aspx");
            }

            else
            {
                this.lbmsg.Text = "*為必填欄位";
                this.lbmsg.Visible = true;
            }


        }

        private string GetNewFileName(FileUpload fu)
        {
            if (!fu.HasFile)
                return string.Empty;


            var uFile = fu.PostedFile;
            var fileName = uFile.FileName;
            string fileExt = System.IO.Path.GetExtension(fileName);

            if (!_allowExts.Contains(fileExt.ToLower()))
                return string.Empty;


            string path = Server.MapPath(_saveFolder);
            string newFileName = Guid.NewGuid().ToString() + fileExt;
            string fullPath = System.IO.Path.Combine(path, newFileName);

            uFile.SaveAs(fullPath);
            return newFileName;
        }
    }
}