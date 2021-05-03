using CoreProject.Helpers;
using CoreProject.Managers;
using CoreProject.Models;
using CoreProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ubay_CourseRegistration.Managers
{

    public partial class ManagerStDetail : System.Web.UI.Page
    {

        private string[] _allowExts = { ".jpg", ".png", ".bmp", ".gif" };
        private string _saveFolder = "~/FileDownload/";

        protected void Page_Init(object sender, EventArgs e)
        {
            if (this.IsUpdateMode())
            {
                Guid temp;
                Guid.TryParse(Request.QueryString["Student_ID"], out temp);

                this.idn.Enabled = false;
                this.idn.BackColor = System.Drawing.Color.DarkGray;
                this.LoadAccount(temp);
            }
            else
            {
                this.pwd.Enabled = false;
                this.pwd.BackColor = System.Drawing.Color.DarkGray;
            }

        }

        private bool IsUpdateMode()
        {
            string qsID = Request.QueryString["Student_ID"];

            Guid temp;
            if (Guid.TryParse(qsID, out temp))
                return true;

            return false;
        }

        private void LoadAccount(Guid id)
        {
            var manager = new ManagerManagers();
            var model = manager.GetAccountViewModel(id);
            var DBManagers = new DBAccountManager();
            if (model == null)
                Response.Redirect("~/Managers/ManagerStList.aspx");


            this.fname.Text = model.S_FirstName;
            this.lname.Text = model.S_LastName;
            this.birthday.Text = model.Birthday.ToString();
            bool idnc = DBManagers.Check(this.idn.Text);
            if (idnc == true)
            {
                model.Idn = this.idn.Text.Trim();

            }
            else
            {
                this.lbmsg.Text = "身分證格式錯誤";
                this.lbmsg.Visible = true;
                return;
            }

            if (DBManagers.GetAccount(this.idn.Text.Trim()) != null)
            {
                model.Idn = null;
                this.lbmsg.Text = "帳號已重複註冊";
                this.lbmsg.Visible = true;
                return;
            }
            else
            {
                model.Account = this.idn.Text.Trim();
            }
            this.email.Text = model.Email;
            this.address.Text = model.Address;
            this.phone.Text = model.CellPhone;
            this.education.Text = model.Education;
            this.school.Text = model.School_ID;
            this.experience.Text = model.Experience;
            this.exyear.Text = model.ExYear;
            this.gender.Text = model.gender.ToString();
            this.psn.Text = model.PassNumber;
            //this.passpic. = model.PassPic;
            string passpic = this.GetNewFileName(this.passpic);
            if (!string.IsNullOrEmpty(passpic))
                model.PassPic = passpic;
            this.pwd.Text = model.password;


        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            var manager = new ManagerManagers();
            var DBmanager = new DBAccountManager();

            StudentAccountViewModel model = null;

            if (this.IsUpdateMode())
            {
                string qsID = Request.QueryString["Student_ID"];

                Guid temp;
                if (!Guid.TryParse(qsID, out temp))
                    return;

                manager.GetAccountViewModel(temp);
            }
            else
            {
                model = new StudentAccountViewModel();
            }


            if (this.IsUpdateMode())
            {
                if (!string.IsNullOrEmpty(this.newpwd.Text) &&
                !string.IsNullOrEmpty(this.renewpwd.Text))
                {
                    if (model.password == this.renewpwd.Text)
                    {
                        model.password = this.renewpwd.Text.Trim();
                    }
                    else
                    {
                        this.lbmsg.Text = "密碼和原密碼不一致";
                        return;
                    }
                }
            }
            else
            {
                if (string.IsNullOrEmpty(this.pwd.Text))
                {
                    this.lbmsg.Text = "密碼不可以為空";
                    return;
                }

                if (DBmanager.GetAccount(this.idn.Text.Trim()) != null)
                {
                    this.lbmsg.Text = "帳號已重覆，請選擇其它帳號";
                    return;
                }

                model.Account = this.idn.Text.Trim();
                model.password = this.newpwd.Text.Trim();
            }

            model.S_FirstName = this.fname.Text.Trim();
            model.S_LastName = this.lname.Text.Trim();
            model.Email = this.email.Text.Trim();
            model.CellPhone = this.phone.Text.Trim();



            if (this.IsUpdateMode())
                manager.UpdataStudent(model);
            else
            {
                try
                {
                    manager.CreatStudent(model);
                }
                catch (Exception ex)
                {
                    this.lbmsg.Text = ex.ToString();
                    return;
                }
            }

            this.lbmsg.Text = "新增成功";
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