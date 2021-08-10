<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ActivitiesEdit.aspx.cs" Inherits="TutorGroupParticipation.ActivitiesEditForm" %>

<!DOCTYPE html>

<html lang="en" xml:lang="en" xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title>Edit Activities For Year Level</title>

    <meta charset="UTF-8"/>
    <meta name="google" content="notranslate"/>
    <meta http-equiv="Content-Language" content="en"/>

</head>

<body>
    <form id="ActivitiesFrm" runat="server">
        <table style="width: 100%">
            <colgroup>
                <col span="1" style="width: 100px; vertical-align: top" />
                <col span="1" style="text-align: left; width: 400px; vertical-align: top" />
                <col span="1" style="text-align: left; vertical-align: top" />
            </colgroup>
            <tr>
                <td style="text-align:right">
                    <strong>Year Level: </strong>
                </td>
                <td>
                    <asp:DropDownList ID="YearLevelDdl" runat="server" OnSelectedIndexChanged="YearLevelDdl_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                </td>
                <td>
                </td>
            </tr>
            <tr style="line-height:5px">
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td style="vertical-align:top; text-align: right">
                    <asp:Button ID="backToMainPageBtn" Text="< Back to Main Page" runat="server" OnClick="backToMainPageBtn_Click"/>
                </td>
                <td>
                    <asp:GridView ID="ActivitiesGrd" runat="server"
                        AutoGenerateColumns="False" DataKeyNames="Seq"
                        OnRowCancelingEdit="ActivitiesGrd_RowCancelingEdit"
                        OnRowEditing="ActivitiesGrd_RowEditing"
                        OnRowUpdating="ActivitiesGrd_RowUpdating"
                        OnRowDeleting="ActivitiesGrd_RowDeleting">
                        <EmptyDataTemplate>
                            <p style="font-weight: bold; color: #CC0000;">
                                No activities found for this year level. 
                            </p>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText="Activity">
                                <ItemTemplate>
                                    <asp:Label ID="activityLbl" runat="server" Text='<%#Eval("Activity") %>' Width="300"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="activityTbx" runat="server" Text='<%#Eval("Activity") %>' Width="300"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <div style="white-space:nowrap">
                                        <asp:Button ID="editBtn" runat="server" Text="Edit" CommandName="Edit" />
                                        <asp:Button ID="deleteBtn" runat="server" Text="Delete" CommandName="Delete"
                                            OnClientClick='<%# Eval("Activity", "return confirm(\"Are you sure you want to delete the activity [{0}]?\");") %>'/>
                                    </div>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div style="white-space:nowrap">
                                        <asp:Button ID="saveBtn" runat="server" Text="Save" CommandName="Update" />
                                        <asp:Button ID="cancelBtn" runat="server" Text="Cancel" CommandName="Cancel" />
                                    </div>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
                <td style="text-align: left; vertical-align: top">
                    <b>Add new activity to this year level</b>
                    <br />
                    <asp:TextBox ID="NewActivityName" runat="server" Width="300px"></asp:TextBox>
                    <br />
                    <asp:Button ID="AddNewActivityBtn" runat="server" Text="Add" OnClick="AddNewActivityBtn_Click" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
