<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FacebookAuthorized.aspx.cs" Inherits="ExternalServicesPOC.Web.FacebookAuthorized" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Facebook Authorization</title>
    <script type="text/javascript">

        function setAuthorizationToken(authToken) {
            window.opener.setAuthorizationToken(authToken);
            window.close();
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label runat="server" ID="ErrorMessage" />
    </div>
    </form>
</body>
</html>
