<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StudentSummary.aspx.cs" Inherits="TutorGroupParticipation.StudentSummary" %>

<!DOCTYPE html>

<html lang="en" xml:lang="en" xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <title>Student Summary</title>

    <meta charset="UTF-8"/>
    <meta name="google" content="notranslate"/>
    <meta http-equiv="Content-Language" content="en"/>

    <style>
        table {
            border-collapse: collapse;
        }

        td {
            vertical-align: top;
        }

        .StudentInfo td {
            padding: 10px;
        }

        .ActivitiesList td {
            vertical-align: middle;
            padding: 5px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table>
                <tr>
                    <asp:FormView ID="StudentInfoFrm" runat="server" >
                        <EmptyDataTemplate>
                            <span style="color: red">No information found for this student.</span>
                        </EmptyDataTemplate>
                        <ItemTemplate>
                            <table class="StudentInfo" style="width:1200px">
                                <tr>
                                    <td>
                                        <asp:Image ID="StudentImg" runat="server" ImageUrl='<%# "SynergyImages/" + Eval("StudentId") + ".jpg" %>' />
                                    </td>
                                    <td style="width: 300px">
                                        <strong>ID: </strong>
                                        <asp:Label ID="IDLbl" runat="server" Text='<%# Eval("StudentId") %>' />
                                        <br />
                                        <strong>Name: </strong>
                                        <asp:Label ID="NameLbl" runat="server" Text='<%# Eval("Name") %>' />
                                        <br />
                                        <strong>Year Level: </strong>
                                        <asp:Label ID="YearLevelLbl" runat="server" Text='<%# Eval("YearLevel") %>' />
                                        <br />
                                        <strong>Email: </strong>
                                        <asp:Label ID="EmailLbl" runat="server" Text='<%# Eval("Email") %>' />
                                        <br />
                                        <strong>Tutor Group: </strong>
                                        <asp:Label ID="TutorGroupLbl" runat="server" Text='<%# Eval("TutorGroup") %>' />
                                    </td>
                                    <td style="padding:0px 10px 0px 0px; vertical-align: middle">
                                        <table class="ActivitiesList" runat="server" >
                                            <tr>
                                                <td>
                                                    <b>Tutor Group Activities:</b></br>
                                                    <asp:Label ID="ActivitiesLbl" runat="server" Text='<%# Eval("Activities") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="border-top: solid 1px black; color: red; font-weight: bold">
                                                    You cannot make changes to your participation list at this point in time.<br />
                                                    If you need to make modifications, please speak to your tutor. 
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:FormView>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
