using CoreProject.Helpers;
using CoreProject.Managers;
using CoreProject.Models;
using CoreProject.ViewModels;
using Microsoft.VisualBasic;
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
                this.Label1.Text = "新增學生資料";
                this.region.Text = "確認新增";
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
            if (IsPostBack)
            {
                pwd.Attributes.Add("value", pwd.Text);
                newpwd.Attributes.Add("value", newpwd.Text);
                renewpwd.Attributes.Add("value", renewpwd.Text);
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
            var model = manager.GetStudentViewModel(id);
            var DBManagers = new DBAccountManager();
            if (model == null)
                Response.Redirect("~/Managers/ManagerStList.aspx");


            this.fname.Text = model.S_FirstName;
            this.lname.Text = model.S_LastName;
            this.idn.Text = model.Idn;
            this.gender.SelectedValue = model.gender.ToString();
            this.birthday.Text = model.Birthday.ToString();
            this.email.Text = model.Email;
            this.phone.Text = model.CellPhone;
            this.address.Text = model.Address;
            this.experience.SelectedValue = model.Experience.ToString();
            this.exyear.Text = model.ExYear.ToString();
            this.education.Text = model.Education.ToString();
            var schoolint = Convert.ToInt32(this.school.Text);
            this.school.SelectedValue = model.School_ID.ToString();
            this.psn.Text = model.PassNumber;
            string passpic = this.GetNewFileName(this.passpic);
            if (!string.IsNullOrEmpty(passpic))
                model.PassPic = passpic;


        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            var manager = new ManagerManagers();
            var DBmanager = new DBAccountManager();

            StudentAccountViewModel model = new StudentAccountViewModel() ;

            if (this.IsUpdateMode())
            {
                string qsID = Request.QueryString["Student_ID"];

                Guid temp;
                if (!Guid.TryParse(qsID, out temp))
                    return;

                manager.GetStudentViewModel(temp);
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
                        this.lbmsg.Visible = true;
                        return;
                    }
                }
            }
            else
            {
                if (string.IsNullOrEmpty(this.newpwd.Text))
                {
                    this.lbmsg.Text = "新密碼不可以為空";
                    this.lbmsg.Visible = true;

                    return;
                }

                if (DBmanager.GetAccount(this.idn.Text.Trim()) != null)
                {
                    this.lbmsg.Text = "帳號已重覆，請選擇其它帳號";
                    this.lbmsg.Visible = true;

                    return;
                }
                model.Idn = this.idn.Text.Trim();
                model.password = this.pwd.Text.Trim();
            }
            model.S_FirstName = this.fname.Text.Trim();
            model.S_LastName = this.lname.Text.Trim();
            model.Email = this.email.Text.Trim();
            model.CellPhone = this.phone.Text.Trim();

            model.gender = Convert.ToBoolean(this.gender.Text);
            model.Birthday = Convert.ToDateTime(this.birthday.Text);
            model.Email = this.email.Text.Trim();
            model.CellPhone = this.phone.Text.Trim();
            model.Address = this.address.Text.Trim();
            if (this.experience.SelectedItem.Text == "有")
            {
                if (this.exyear.SelectedItem.Text == "請選擇")
                {

                    model.ExYear = null;
                    this.lbmsg.Visible = true;
                    this.lbmsg.Text = "需選擇年數";
                    return;
                }
                else
                {
                    model.Experience = Convert.ToBoolean(this.experience.SelectedItem.Value);
                    model.ExYear = Convert.ToInt32(this.exyear.Text);

                }
            }
            else
            {
                model.Experience = Convert.ToBoolean(this.experience.SelectedItem.Value);
                model.ExYear = null;

            }

            if (this.education.SelectedItem.Text == "大學" || this.education.SelectedItem.Text == "研究所")
            {
                if (this.school.SelectedItem.Text == "請選擇")
                {

                    model.School_ID = null;
                    this.lbmsg.Visible = true;
                    this.lbmsg.Text = "需選擇學校";

                    return;
                }
                else
                {
                    model.Education = Convert.ToInt32(this.education.Text);
                    model.School_ID = Convert.ToInt32(this.school.Text);

                }
            }
            else
            {
                model.Education = Convert.ToInt32(this.education.Text);
                model.School_ID = null;
            }

            model.PassNumber = this.psn.Text.Trim();

            model.PassPic = this.GetNewFileName(this.passpic);


            if (this.IsUpdateMode())
            {
                model.e_empno = (Guid)Session["Acc_sum_ID"];
                manager.UpdataStudent(model);
            }
            else
            {
                try
                {
                    model.b_empno = (Guid)Session["Acc_sum_ID"];
                    manager.CreatStudent(model);
                }
                catch (Exception ex)
                {
                    this.lbmsg.Text = ex.ToString();
                    this.lbmsg.Visible = true;
                    return;
                }
            }
            this.lbmsg.Text = "新增成功";
            this.lbmsg.Visible = true;

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

        protected void experience_SelectedIndexChanged(object sender, EventArgs e)
        {
            StudentAccountViewModel model=new StudentAccountViewModel();

            if (this.experience.SelectedItem.Text == "有" || model.Experience !=false)
            {
                this.yearshow.Visible = true;
                this.exyear.Visible = true;

            }
            else
            {
                this.yearshow.Visible = false;
                this.exyear.Visible = false;

            }
        }
        protected void education_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.education.SelectedItem.Text == "大學" || this.education.SelectedItem.Text == "研究所")
            {
                this.schoolshow.Visible = true;
                this.school.Visible = true;

            }
            else
            {
                this.schoolshow.Visible = false;
                this.school.Visible = false;

            }
        }

    }
}