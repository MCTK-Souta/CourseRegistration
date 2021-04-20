<%@ Page Title="" Language="C#" MasterPageFile="~/Students/StudentMaster.Master" AutoEventWireup="true" CodeBehind="StudentCourseRecord.aspx.cs" Inherits="Ubay_CourseRegistration.Students.StudentCourseRecord" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <style type="text/css">
        .auto-style1 {
            width: 80%;
        }
        table {
            border-collapse: collapse;
        }

        table, td, th {
            border: 1px solid #A0674B;
        }

        th {
            background-color: #ECB88A;
            color: white;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <div role ="dialog" class="el_dialog" style ="margin-top: 15vh;width:1160px;">
        <div class="el_dialog_header">
            <%--歷史報名紀錄分頁標題--%> 
            <h1 style="margin: 0,auto; left: 60%; position: relative; width: 100%;">歷史報名紀錄</h1>
        </div>

        <div class="el_dialog_body">
            <div>
            <%--搜尋框--%>

            <input type="search" class="search_key" accesskey="S" name="key" id="Key" value="搜尋" autocomplete="off" style="margin:0,auto;left:20%;position:relative;">
            <br />

            <%--顯示學生所有歷史課程--%>
            <div style="margin:0,auto;left:20%;position:relative;">
                            <div>
                <asp:Repeater ID="rptResult" runat="server">
                    <HeaderTemplate>
                        <table class="auto-style1">
                            <tr>
                                <th>課程ID</th>
                                <th>課程名稱</th>
                                <th>教師</th>
                                <th>開課時間</th>
                                <th>結訓時間</th>
                                <th>教室ID</th>
                                <th>價格</th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td><%#Eval("Course_ID") %></td>
                            <td><%#Eval("C_Name") %></td>
                            <td><%#Eval("Teacher_ID") %></td>
                            <td><%#Eval("StartDate") %></td>
                            <td><%#Eval("EndDate") %></td>
                            <td><%#Eval("Place_ID") %></td>
                            <td><%#Eval("Price") %></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </div>

            <div style="margin-top: 20px;">
                <table style="margin:0,auto;left:20%;position:relative; width: 600px;">
                    <tr>
                        <td>
                            <asp:LinkButton ID="lbFirst" runat="server" OnClick="lbFirst_Click1" ForeColor="#A0674B">First</asp:LinkButton >
                        </td>
                        <td>
                            <asp:LinkButton ID="lbPrevious" runat="server" OnClick="lbPrevious_Click1" ForeColor="#A0674B">Previous</asp:LinkButton>
                        </td>
                        <td>
                            <asp:DataList ID="rptPaging" runat="server"
                                OnItemCommand="rptPaging_ItemCommand"
                                OnItemDataBound="rptPaging_ItemDataBound" RepeatDirection="Horizontal" ForeColor="#A0674B">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbPaging" runat="server"
                                        CommandArgument='<%# Eval("PageIndex") %>' CommandName="newPage"
                                        Text='<%# Eval("PageText") %> ' Width="20px" ForeColor="#A0674B"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:DataList>
                        </td>
                        <td>
                            <asp:LinkButton ID="lbNext" runat="server" OnClick="lbNext_Click1" ForeColor="#A0674B">Next</asp:LinkButton>
                        </td>
                        <td>
                            <asp:LinkButton ID="lbLast" runat="server" OnClick="lbLast_Click1" ForeColor="#A0674B">Last</asp:LinkButton>
                        </td>
                        <td>
                            <asp:Label ID="lblpage" runat="server" Text="" ForeColor="#A0674B"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>       
        </div>

            
        </div>
            <%--顯示登入的學生已選過的課程紀錄月曆--%>
            <div>
                <div id='calendar' style="left:20%;position:relative;width:900px;"></div>
            </div>
        </div>
    </div> 
</asp:Content>
