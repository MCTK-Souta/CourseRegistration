using System;
using System.Linq;
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
            if (IsPostBack)
            {
                pwd.Attributes.Add("value", pwd.Text);
                repwd.Attributes.Add("value", repwd.Text);

            }
            if(!IsPostBack)
            { 
            StudentManagers stmanagers = new StudentManagers();
            stmanagers.GetSchoolList(ref school);
            }

        }

        protected void Button_StRegion(object sender, EventArgs e)
        {
            var Managers = new DBAccountManager(); 
            StudentInfoModel stmodel = new StudentInfoModel();
            AccountModel acmodel = new AccountModel();
            var StManagers = new StudentManagers();
            if (this.fname.Text != string.Empty &&
                this.lname.Text != string.Empty &&
                this.idn.Text != string.Empty &&
                this.pwd.Text != string.Empty &&
                this.repwd.Text != string.Empty &&
                this.gender.Text != string.Empty &&
                this.birthday.Text != string.Empty &&
                this.email.Text != string.Empty &&
                this.phone.Text != string.Empty &&
                this.address.Text != string.Empty &&
                this.experience.Text != string.Empty &&
                this.education.Text != string.Empty)
            {
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

                if (Managers.GetAccount(this.idn.Text.Trim()) != null)
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
                stmodel.gender = Convert.ToBoolean(this.gender.Text);
                stmodel.Birthday = Convert.ToDateTime(this.birthday.Text);
                stmodel.Email = this.email.Text.Trim();
                stmodel.CellPhone = this.phone.Text.Trim();
                stmodel.Address = this.address.Text.Trim();
                if(this.experience.SelectedItem.Text=="有")
                {
                    if(this.exyear.SelectedItem.Text== "請選擇")
                    {
                        stmodel.ExYear = null;
                        this.lbmsg.Visible = true;
                        this.lbmsg.Text = "需選擇年數";
                        return;
                    }
                    else
                    {
                        stmodel.Experience = Convert.ToInt32( this.experience.Text);
                        stmodel.ExYear = Convert.ToInt32(this.exyear.Text);

                    }
                }
                else
                {
                    stmodel.Experience = Convert.ToInt32(this.experience.Text);
                    stmodel.ExYear = Convert.ToInt32(this.exyear.Text);

                }

                if(this.education.SelectedItem.Text=="大學"||this.education.SelectedItem.Text=="研究所")
                {
                    if(this.school.SelectedItem.Text== "請選擇")
                    {
                        stmodel.Education = null;
                        stmodel.School_ID = null;
                        this.lbmsg.Visible = true;
                        this.lbmsg.Text = "需選擇學校";

                        return;
                    }
                    else
                    {
                        stmodel.Education = this.education.Text;
                        stmodel.School_ID = Convert.ToInt32( this.school.Text);

                    }
                }
                else
                {
                    stmodel.Education = this.education.Text;
                    stmodel.School_ID = Convert.ToInt32(this.school.Text);
                }

                stmodel.PassNumber = this.psn.Text.Trim();
                if (this.GetNewFileName(this.passpic) == "檔案類型錯誤")
                {
                    stmodel.PassPic = null;
                    this.lbmsg.Text = "檔案僅接受.jpg, .png, .bmp, .gif";
                    this.lbmsg.Visible = true;
                    return;
                }
                {
                    stmodel.PassPic = this.GetNewFileName(this.passpic);
                }

                stmodel.b_date = DateTime.Now;

                StManagers.StudentSigh_UP(stmodel, acmodel);
                Response.Write
                ("<script>alert('註冊成功，返回登入頁面');location.href='/Login.aspx'; </script>");
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
            {

                return "檔案類型錯誤";

            }


            string path = Server.MapPath(_saveFolder);
            string newFileName = Guid.NewGuid().ToString() + fileExt;
            string fullPath = System.IO.Path.Combine(path, newFileName);

            uFile.SaveAs(fullPath);
            return newFileName;
        }

        protected void experience_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.experience.SelectedItem.Text == "有")
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