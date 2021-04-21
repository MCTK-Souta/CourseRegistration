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

        }

        protected void Button_StRegion(object sender, EventArgs e)
        {
            var Managers = new StudentManagers();
            StudentInfoModel stmodel = new StudentInfoModel();
            AccountModel acmodel = new AccountModel();

            stmodel.S_FirstName = this.fname.Text.Trim();
            stmodel.S_LastName = this.lname.Text.Trim();
            stmodel.Idn= this.idn.Text.Trim();
            acmodel.Account = this.idn.Text.Trim();
            acmodel.password = this.pwd.Text.Trim();
            string repwd = this.repwd.Text.Trim();
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