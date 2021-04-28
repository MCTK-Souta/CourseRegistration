<%@ Page Title="" Language="C#" MasterPageFile="~/Managers/ManagerMaster.Master" AutoEventWireup="true" CodeBehind="ManagerStList.aspx.cs" Inherits="Ubay_CourseRegistration.Managers.ManagerStList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
            <h1 style="margin: 0,auto; left: 85%; position: relative;">學生資料維護</h1>
        <a href="MemberDetail.aspx">新增</a>

    <div>
        進階搜尋：
        <p> 
            Name: <asp:TextBox runat="server" ID="txtName"></asp:TextBox> 
            Level: 
            <asp:RadioButtonList runat="server" ID="rdblLevel" RepeatDirection="Horizontal" RepeatLayout="Flow">
                <asp:ListItem Text="All" Value=""></asp:ListItem>
                <asp:ListItem Text="Normal" Value="0"></asp:ListItem>
                <asp:ListItem Text="Admin" Value="1"></asp:ListItem>
                <asp:ListItem Text="Employee" Value="2"></asp:ListItem>
                <asp:ListItem Text="Supervisor" Value="3"></asp:ListItem>
            </asp:RadioButtonList>
            <%--<asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />--%>
        </p>
    </div>

<%--    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" OnRowCommand="GridView1_RowCommand">
        <Columns>
            <asp:TemplateField HeaderText="Account">
                <ItemTemplate>
                    <a href="MemberDetail.aspx?ID=<%# Eval("ID") %>">
                    <%# Eval("Account") %>
                    </a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Name" HeaderText="Name" />
            <asp:BoundField DataField="Title" HeaderText="Title" />


            <asp:TemplateField HeaderText="Act">
                <ItemTemplate>
                <%--<asp:Button runat="server" ID="btnDelete" Text="Del" CommandName="DeleteItem" 
                CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure?');" />--%>
<%--                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>--%>

    <asp:Repeater runat="server" ID="repPaging">
        <ItemTemplate>
            <a href="<%# Eval("Link") %>" title="<%# Eval("Title") %>">Page-<%# Eval("Name") %></a>
        </ItemTemplate>
    </asp:Repeater>
    <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
</div>

</asp:Content>
