<%@ Page Title="" Language="C#" MasterPageFile="~/Students/StudentMaster.Master" AutoEventWireup="true" CodeBehind="StudentInfo.aspx.cs" Inherits="Ubay_CourseRegistration.Students.StudentInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <h1 style="margin: 0,auto; left: 85%; position: relative;">學生資料維護</h1>
        <div style="margin: 0,auto; left: 80%; position: relative;">
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
                性別：<asp:RadioButton ID="RadioButton1" runat="server" GroupName="sex" />男<asp:RadioButton ID="RadioButton2" runat="server" GroupName="sex" />女<br />
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
                手機：<asp:TextBox runat="server" TextMode="Phone" ID="phone"></asp:TextBox><br />
            </div>
            <br />
            <div>
                地址：<asp:TextBox runat="server" ID="address"></asp:TextBox><br />
            </div>
            <br />

            <div style="margin-left: -65px;">
                有無程式經驗：
                <asp:RadioButton ID="RadioButton3" runat="server" GroupName="exp" />
                無
                <asp:RadioButton ID="RadioButton4" runat="server" GroupName="exp" />
                有
            </div>
            <br />

            <div style="margin-left: -33px;">
                最高學歷：
                <asp:RadioButton ID="RadioButton5" runat="server" GroupName="sexp" />
                國小
                <asp:RadioButton ID="RadioButton6" runat="server" GroupName="sexp" />
                國中
                <asp:RadioButton ID="RadioButton7" runat="server" GroupName="sexp" />
                高中
                <asp:RadioButton ID="RadioButton8" runat="server" GroupName="sexp" />
                大學
                <asp:RadioButton ID="RadioButton9" runat="server" GroupName="sexp" />
                研究所
                    <asp:DropDownList ID="DropDownList1" runat="server">
                        <asp:ListItem>大專院校時需選擇</asp:ListItem>
                    </asp:DropDownList><br />
                <br />

            </div>
            <div style="margin-left: -33px;">護照號碼：<asp:TextBox runat="server" TextMode="Phone"></asp:TextBox></div>
            <br />
            <div style="margin-left: -65px;">上傳護照照片：<asp:FileUpload ID="FileUpload1" runat="server" /></div>
            <br />
            <br />
            <asp:Button ID="Button1" runat="server" Text="確認更改" Style="margin: 0,auto; left: 5%; position: relative;" />
            <br />
            <br />
            <br />
        </div>
    </div>
</asp:Content>
