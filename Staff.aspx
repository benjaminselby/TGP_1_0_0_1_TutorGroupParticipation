<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Staff.aspx.cs" Inherits="Staff.StaffForm" %>

<!DOCTYPE html>

<html lang="en" xml:lang="en" xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <title>Staff Summary</title>

    <meta charset="UTF-8"/>
    <meta name="google" content="notranslate"/>
    <meta http-equiv="Content-Language" content="en"/>

    <style type="text/css">
        .auto-style1 {
            height: 12px;
        }
        table.studentListTbl {
            border-collapse: collapse;
        }
        table.studentListTbl td {
            border: 2px solid slategray;
            padding: 5px;
        }
    </style>
</head>

<body>
    <form id="UserForm" runat="server">
        <table style="width: 100%">
            <colgroup>
                <col span="1" style="text-align: left; width: 100px" />
                <col span="1" style="text-align: left; width: 300px" />
                <col span="1" style="text-align: right" />
            </colgroup>
            <tr>
                <td>
                    <strong>Current user: </strong>
                </td>
                <td>
                    <asp:Label ID="currentUserNameLbl" runat="server" Text="" />
                </td>
                <td>                    
                </td>
            </tr>
            <tr>
                <td>
                    <strong>Class: </strong>
                </td>
                <td>
                    <asp:DropDownList ID="ClassListDdl" runat="server"
                        OnSelectedIndexChanged="ClassListDdl_SelectedIndexChanged" AutoPostBack="True"
                        Width="200px" />
                </td>
                <td style="float:right;">       
                    <!-- This button should only be visible when the user has authority to modify the activity lists. -->
                    <asp:Button ID="ModifyActivitiesBtn" runat="server" OnClick="ModifyActivitiesBtn_Click" 
                        Text="Modify Available Activities" Visible="false"/>
                </td>
            </tr>
            <tr>
                <td></td>
                <td></td>
                <td></td>
            </tr>
        </table>
        <table class="studentListTbl" style="width: 100%;">
            <colgroup>
                <col span="1" style="width: 150px" />
                <col span="1" style="width: 300px"/>
                <col span="1" style=""/>
            </colgroup>
            <asp:ListView ID="StudentList" runat="server"
                    DataKeyNames = "StudentId">

                <EmptyDataTemplate>
                    <span style="color: red">No students found for this class.</span>
                </EmptyDataTemplate>

                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:Image ID="StudentImg" runat="server" ImageUrl='<%# "SynergyImages/" + Eval("StudentId") + ".jpg" %>' />
                            <br />
                        </td>
                        <td>
                            <strong>ID: </strong>
                            <asp:Label ID="IDLbl" runat="server" Text='<%# Eval("StudentId") %>' />
                            <br />
                            <strong>Name: </strong>
                            <asp:Label ID="NameLbl" runat="server" Text='<%# Bind("Name") %>' />
                            <br />
                            <strong>Year Level: </strong>
                            <asp:Label ID="YearLevelLbl" runat="server" Text='<%# Bind("YearLevel") %>' />
                            <br />
                            <strong>Email: </strong>
                            <asp:Label ID="EmailLbl" runat="server" Text='<%# Bind("Email") %>' />
                            <br />
                            <strong>Tutor Group: </strong>
                            <asp:Label ID="TutorGroupLbl" runat="server" Text='<%# Bind("TutorGroup") %>' />
                            <br />
                        </td>
                        <td>
                            <asp:Label ID="ActivitiesLbl" runat="server" Text='<%# Bind("Activities") %>' />
                            <br />
                            <asp:Button ID="EditStudentBtn" Text="Edit" OnClick="StudentListEditBtn_Click" runat="server" CommandArgument='<%# Eval("StudentId") %>' />
                        </td>
                    </tr>
                </ItemTemplate>

            </asp:ListView>

        </table>
    </form>
</body>
</html>
