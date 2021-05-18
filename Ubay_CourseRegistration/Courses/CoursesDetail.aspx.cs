using CoreProject.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ubay_CourseRegistration.Courses
{
    public partial class CoursesDetail : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    CourseManagers course = new CourseManagers();
            //    course.ReadTeacherTable(ref tcList);
            //}
            //if (this.IsUpdateMode())
            //{
            //    Guid temp;
            //    Guid.TryParse(Request.QueryString["Student_ID"], out temp);

            //    this.Price.Enabled = false;
            //    this.Price.BackColor = System.Drawing.Color.DarkGray;
            //    this.LoadAccount(temp);
            //}
            //else
            //{
            //    this.passview.Visible = false;
            //    this.pwd.BackColor = System.Drawing.Color.DarkGray;
            //    this.Label1.Text = "新增學生資料";
            //    this.region.Text = "確認新增";
            //}

        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        private bool IsUpdateMode()
        {
            string qsID = Request.QueryString["Student_ID"];

            Guid temp;
            if (Guid.TryParse(qsID, out temp))
                return true;

            return false;
        }





    }
}