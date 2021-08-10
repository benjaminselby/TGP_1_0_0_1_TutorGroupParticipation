<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="Main.Main" %>

<!DOCTYPE html>

<html lang="en" xml:lang="en" xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <title>Main Page</title>

    <meta charset="UTF-8"/>
    <meta name="google" content="notranslate"/>
    <meta http-equiv="Content-Language" content="en"/>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <!-- This page will only render if the current user is not authorised. -->
            Error - you are not currently supported as a user of this system. Please contact <a href="mailto:<%= ConfigurationManager.AppSettings["dataManagementEmail"] %>">Data Management</a>
        </div>
    </form>
</body>
</html>
