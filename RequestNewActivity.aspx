<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RequestNewActivity.aspx.cs" Inherits="TutorGroupParticipation.RequestNewActivity" %>

<!DOCTYPE html>

<html lang="en" xml:lang="en" xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <title>Request New Activity</title>

    <meta charset="UTF-8"/>
    <meta name="google" content="notranslate"/>
    <meta http-equiv="Content-Language" content="en"/>

</head>

<body>
    <form id="form1" runat="server">
        <div>
            <h3>Send a Message Requesting New Activity to be Created</h3>
            <p>
            This will send an email to your Year Level Manager asking them to add a new activity to the available list. <br />
            Please <u>clearly</u> list the activities which you would like added. 
            </p>
            <table>
                <colgroup>
                    <col span="1" style="width: 60px" />
                    <col span="1" style="width: 10px" />
                    <col span="1"  />
                </colgroup>
                <tr>
                    <td style="text-align:right">
                        <label style="font-weight: bold">To:</label>
                    </td>
                    <td></td>
                    <td>
                        <label id="recipientNameLbl" runat="server"></label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:right">
                        <label style="font-weight: bold">From:</label>
                    </td>
                    <td></td>
                    <td>
                        <label id="senderNameLbl" runat="server"></label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:right;vertical-align:top">
                        <label style="font-weight: bold">Message:</label>
                    </td>
                    <td></td>
                    <td>
                        <asp:TextBox id="MessageTbx" runat="server" Height="200px" Width="500px"
                            TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td></td>
                    <td  style="text-align:right">
                        <asp:Button ID="sendBtn" Text="Send Message" runat="server" OnClick="sendBtn_Click" />
                        <div style="width: 10px; display: inline-block"></div>
                        <asp:Button ID="cancelBtn" Text="Cancel" runat="server" OnClick="cancelBtn_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
