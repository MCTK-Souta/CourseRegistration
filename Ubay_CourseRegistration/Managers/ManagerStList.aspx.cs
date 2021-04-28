using CoreProject.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ubay_CourseRegistration.Managers
{
    public partial class ManagerStList : System.Web.UI.Page
    {
        //const int _pageSize = 10;


        //internal class PagingLink
        //{
        //    public string Name { get; set; }
        //    public string Link { get; set; }
        //    public string Title { get; set; }
        //}



        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!this.IsPostBack)
        //    {
        //        this.LoadGridView();
        //        this.RestoreParameters();
        //    }
        //}

        //private void RestoreParameters()
        //{
        //    string name = Request.QueryString["name"];
        //    string levelText = Request.QueryString["level"];

        //    if (!string.IsNullOrEmpty(name))
        //        this.txtName.Text = name;

        //    if (!string.IsNullOrEmpty(levelText))
        //        this.rdblLevel.SelectedValue = levelText;
        //}

        //private string GetQueryString(bool includePage, int? pageIndex)
        //{
        //    //----- Get Query string parameters -----
        //    string page = Request.QueryString["Page"];
        //    string name = Request.QueryString["name"];
        //    string levelText = Request.QueryString["level"];
        //    //----- Get Query string parameters -----


        //    List<string> conditions = new List<string>();

        //    if (!string.IsNullOrEmpty(page) && includePage)
        //        conditions.Add("Page=" + page);

        //    if (!string.IsNullOrEmpty(name))
        //        conditions.Add("Name=" + name);

        //    if (!string.IsNullOrEmpty(levelText))
        //        conditions.Add("Level=" + levelText);

        //    if (pageIndex.HasValue)
        //        conditions.Add("Page=" + pageIndex.Value);

        //    string retText =
        //        (conditions.Count > 0)
        //            ? "?" + string.Join("&", conditions)
        //            : string.Empty;

        //    return retText;
        //}


        //private void LoadGridView()
        //{
        //    //----- Get Query string parameters -----
        //    string page = Request.QueryString["Page"];
        //    int pIndex = 0;
        //    if (string.IsNullOrEmpty(page))
        //        pIndex = 1;
        //    else
        //    {
        //        int.TryParse(page, out pIndex);

        //        if (pIndex <= 0)
        //            pIndex = 1;
        //    }

        //    string name = Request.QueryString["name"];
        //    string levelText = Request.QueryString["level"];

        //    int? level = null;
        //    if (!string.IsNullOrEmpty(levelText))
        //    {
        //        int temp;
        //        if (int.TryParse(levelText, out temp))
        //            level = temp;
        //    }
        //    //----- Get Query string parameters -----


        //    int totalSize = 0;

        //    var manager = new AccountManager();
        //    var list = manager.GetAccountViewModels(name, level, out totalSize, pIndex, _pageSize);
        //    int pages = PagingHelper.CalculatePages(totalSize, _pageSize);

        //    List<PagingLink> pagingList = new List<PagingLink>();
        //    for (var i = 1; i <= pages; i++)
        //    {
        //        pagingList.Add(new PagingLink()
        //        {
        //            Link = $"MamberList.aspx{this.GetQueryString(false, i)}",
        //            Name = $"{i}",
        //            Title = $"前往第 {i} 頁"
        //        });
        //    }

        //    this.repPaging.DataSource = pagingList;
        //    this.repPaging.DataBind();

        //    this.GridView1.DataSource = list;
        //    this.GridView1.DataBind();
        //}

        //protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    string cmdName = e.CommandName;
        //    string arg = e.CommandArgument.ToString();

        //    if (cmdName == "DeleteItem")
        //    {
        //        Guid id;
        //        if (Guid.TryParse(arg, out id))
        //        {
        //            var manager = new StudentManagers();
        //            manager.DeleteAccountViewModel(id);

        //            this.LoadGridView();
        //            this.lblMsg.Text = "已刪除。";
        //        }
        //    }
        //}

        //protected void btnSearch_Click(object sender, EventArgs e)
        //{
        //    string name = this.txtName.Text;
        //    string level = this.rdblLevel.Text;


        //    string template = "?Page=1";

        //    if (!string.IsNullOrEmpty(name))
        //        template += "&name=" + name;

        //    if (!string.IsNullOrEmpty(level))
        //        template += "&level=" + level;

        //    Response.Redirect("MamberList.aspx" + template);
        //}
    }
}