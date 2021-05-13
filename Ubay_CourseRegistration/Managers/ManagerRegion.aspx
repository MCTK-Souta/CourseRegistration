<%@ Page Title="" Language="C#" MasterPageFile="~/Managers/ManagerMaster.Master" AutoEventWireup="true" CodeBehind="ManagerRegion.aspx.cs" Inherits="Ubay_CourseRegistration.Managers.Ad_Region" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <br />
        <asp:Label ID="LB1" runat="server" Text="新增管理人資料" Style="margin: 0,auto; left: 45%; position: relative;" Font-Size="20pt"></asp:Label><br />
        <br />
        <div style="margin: 0,auto; left: 160%; position: relative;">
            <div>姓氏：<asp:TextBox runat="server" ID="txtFirstname"></asp:TextBox></div>
            <br />
            <div>名字：<asp:TextBox runat="server" ID="txtLastname"></asp:TextBox></div>
            <br />
            <div>單位：<asp:TextBox runat="server" ID="txtDepartment"></asp:TextBox></div>
            <br />
            <div>帳號：<asp:TextBox runat="server" ID="txtAccount"></asp:TextBox></div>
            <br />
            <div>
                <asp:Label ID="Label2" runat="server" Text="Label" Visible="true">舊密碼：
                <asp:TextBox runat="server" ID="oldPassword">
                </asp:TextBox>(若不更改密碼則無需填寫)
                </asp:Label>
            </div>
            <br />
            <div>密碼：
                <asp:TextBox runat="server" TextMode="Password" ID="txtPassword">
                </asp:TextBox>
                <asp:Label ID="Label3" runat="server" Text="(若不更改密碼則無需填寫)" Visible="true">
                </asp:Label>
            </div>
            <br />
            <div style="margin-left: -65px;">
                再次確認密碼：
                <asp:TextBox runat="server" TextMode="Password" ID="txtPwdcheck">
                </asp:TextBox>
                <asp:Label ID="Label4" runat="server" Text="(若不更改密碼則無需填寫)" Visible="true">
                </asp:Label>
            </div>
            <br />
            <br />
            <asp:Button ID="Regis" runat="server" Text="確認註冊" Style="margin: 0,auto; left: 25%; position: relative;" OnClick="CreateAdmin_Click" />
            <asp:Button ID="Turnbackbtn" runat="server" Text="返回" OnClick="Turnback_Click" Style="margin: 0,auto; left: 25%;position: relative;"/>
            <br />
            <br />
            <asp:Label ID="WarningMsg" runat="server" Style="color: red; font-size: 20px; font-weight: bolder;"></asp:Label>
            <br />
            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
            <br />

        </div>
    </div>

</asp:Content>
