<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditStudent.aspx.cs" Inherits="TutorGroupParticipation.EditStudent" %>

<!DOCTYPE html>

<html lang="en" xml:lang="en" xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <title>Edit Student Activities</title>

    <meta charset="UTF-8"/>
    <meta name="google" content="notranslate"/>
    <meta http-equiv="Content-Language" content="en"/>

    <style type="text/css">
        .auto-style1 {
            width: 393px;
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <div>
            <table>
                <tr>
                    <td style="vertical-align: top">
                        <asp:FormView ID="StudentInfoFrm" runat="server" >
                            <EmptyDataTemplate>
                                <span style="color: red">No students found for this class.</span>
                            </EmptyDataTemplate>
                            <ItemTemplate>
                                <asp:Image ID="StudentImg" runat="server" ImageUrl='<%# "SynergyImages/" + Eval("StudentId") + ".jpg" %>' />
                                <br />
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
                            </ItemTemplate>
                        </asp:FormView>
                        <br />
                        <asp:Button ID="SaveParticipationBtn" runat="server" Text="Save and Exit" OnClick="SaveBtn_Click" Width="140px" />
                        <br />
                        <br />
                        <asp:Button ID="Cancel" runat="server" Text="Cancel" OnClick="CancelBtn_Click" Width="140px" />
                        <br />
                        <br />
                        <br />
                        <br />
                        <div id="RequestNewActivityDiv" runat="server" visible="false">
                        <!-- Keep the text & button relating to new activity requests invisible unless the user is a student. -->
                        If you do not find the activity you have participated in here,<br />
                        you may request a new activity to be added.                            
                        </div>
                        <br />
                        <asp:Button ID="RequestNewActivityBtn" runat="server" 
                            Text="Request New Activity" OnClick="RequestNewActivityBtn_Click" 
                            visible="false"/>
                    </td>
                    <td style="vertical-align: top">
                        <div runat="server" >
                            <asp:CheckBoxList ID="ParticipationCbxLst" runat="server" AutoPostBack="False"
                                RepeatLayout="table" RepeatColumns="4" RepeatDirection="vertical">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
