<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="TutorGroupParticipation.Error" %>

<!DOCTYPE html>

<html lang="en" xml:lang="en" xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <title>Error</title>

    <meta charset="UTF-8"/>
    <meta name="google" content="notranslate"/>
    <meta http-equiv="Content-Language" content="en"/>

</head>
<body>
    <p><b style="color: red">An Error Occurred.</b></p>
    <p>Please notify Data Management [ <a href="mailto:<%= System.Configuration.ConfigurationManager.AppSettings["dataManagementEmail"] %>"><%= System.Configuration.ConfigurationManager.AppSettings["dataManagementEmail"] %></a>].</p>
    <p><a href="~/Main.aspx">Return to the main page.</a></p>
</body>
</html>
