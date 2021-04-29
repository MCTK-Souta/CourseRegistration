using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ubay_CourseRegistration.Managers
{
    public partial class ManagerMaster : System.Web.UI.MasterPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            int Type = -1;
            if (Session["Type"] != null)
            {
                Type = (int)Session["Type"];
            }



            if (Type != 1)
            {
                Response.Redirect("~/Login.aspx");
            }

            if (!LoginHelper.HasLogined())
            {
                Response.Redirect("~/Login.aspx");
            }

        }
    }
}