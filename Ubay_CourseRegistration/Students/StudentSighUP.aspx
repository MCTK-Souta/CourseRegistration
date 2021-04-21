<%@ Page Title="" Language="C#" MasterPageFile="~/Students/StudentSubMaster.Master" AutoEventWireup="true" CodeBehind="StudentSighUP.aspx.cs" Inherits="Ubay_CourseRegistration.Students.StudentSighUP" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <h1 style="margin: 0,auto; left: 45%; position: relative;">註冊帳戶</h1>
        <div style="margin: 0,auto; left: 40%; position: relative;">
            <div>
                姓氏：<asp:TextBox runat="server" ID="fname"></asp:TextBox><br />
            </div>
            <br />
            <div>
                名字：<asp:TextBox runat="server" ID="lname"></asp:TextBox><br />
            </div>
            <br />
            <div>
                帳號：<asp:TextBox runat="server" placeholder="請輸入身分證字號" ID="idn"></asp:TextBox><br />
            </div>
            <br />
            <div>
                密碼：<asp:TextBox runat="server" TextMode="Password" ID="pwd"></asp:TextBox><br />
            </div>
            <br />
            <div style="margin-left: -65px;">
                再次確認密碼：<asp:TextBox runat="server" TextMode="Password" ID="repwd"></asp:TextBox><br />
            </div>
            <br />
            <div>
                性別：<asp:DropDownList ID="gender" runat="server">
                    <asp:ListItem Text="男" Value="0"></asp:ListItem>
                    <asp:ListItem Text="女" Value="1"></asp:ListItem>
                </asp:DropDownList>

            </div>
            <br />
            <div>
                生日：<asp:TextBox runat="server" TextMode="Date" ID="birthday"></asp:TextBox><br />
            </div>
            <br />
            <div style="margin-left: -10px;">
                E-mail：<asp:TextBox runat="server" TextMode="Email" ID="email"></asp:TextBox><br />
            </div>
            <br />
            <div>
                手機：<asp:TextBox runat="server" TextMode="Number" ID="phone"></asp:TextBox><br />
                <asp:RegularExpressionValidator ID="revPhone" runat="server"
                    ErrorMessage="Not a valid phone" ControlToValidate="phone"
                    ValidationExpression="^[01]?[- .]?(\([2-9]\d{2}\)|[2-9]\d{2})[- .]?\d{3}[- .]?\d{4}$">
                </asp:RegularExpressionValidator>
            </div>
            <br />
            <div>
                地址：<asp:TextBox runat="server" ID="address"></asp:TextBox><br />
            </div>
            <br />

            <div style="margin-left: -65px;">
                有無程式經驗：
                <asp:RadioButtonList ID="experience" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    <asp:ListItem Text="無" Value="0"></asp:ListItem>
                    <asp:ListItem Text="有" Value="1"></asp:ListItem>
                </asp:RadioButtonList>
                <asp:DropDownList ID="exyear" runat="server" RepeatLayout="Flow">
                    <asp:ListItem Text="1年以下" Value="0"></asp:ListItem>
                    <asp:ListItem Text="1年" Value="1"></asp:ListItem>
                    <asp:ListItem Text="2年" Value="2"></asp:ListItem>
                    <asp:ListItem Text="3年(含)以上" Value="3"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <br />

            <div style="margin-left: -33px;">
                最高學歷：
                <asp:DropDownList ID="education" runat="server">
                    <asp:ListItem Text="國小" Value="0"></asp:ListItem>
                    <asp:ListItem Text="國中" Value="1"></asp:ListItem>
                    <asp:ListItem Text="高中" Value="2"></asp:ListItem>
                    <asp:ListItem Text="大學" Value="3"></asp:ListItem>
                    <asp:ListItem Text="研究所" Value="4"></asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="school" runat="server">
                    <asp:ListItem>大學以上時需選擇學校</asp:ListItem>
                    <asp:ListItem Text="高雄第一科技大學" Value="1"></asp:ListItem>
                </asp:DropDownList><br />
                <%--                <asp:DropDownList ID="departments" runat="server" Visible="false">
                    <asp:ListItem>系別</asp:ListItem>
                </asp:DropDownList><br />--%>
                <br />

            </div>
            <div style="margin-left: -33px;">護照號碼：<asp:TextBox ID="psn" runat="server" TextMode="Number"></asp:TextBox></div>
            <br />
            <div style="margin-left: -65px;">上傳護照照片：<asp:FileUpload ID="passpic" runat="server" /></div>
            <br />
            <br />
            <asp:Button ID="Button1" runat="server" Text="確認註冊" Style="margin: 0,auto; left: 5%; position: relative;" OnClick="Button_StRegion" /><br />
            <br />
            <br />
        </div>
    </div>
</asp:Content>
