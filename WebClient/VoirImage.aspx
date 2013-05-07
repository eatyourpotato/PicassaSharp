<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VoirImage.aspx.cs" Inherits="WebClient.VoirImage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <style type="text/css">
        input { margin:12px 10px 10px 10px;
        }
        </style>
    <title>Client Leger</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="height: 191px">   
        <p><asp:Label ID="resultat" runat="server"></asp:Label></p>
          <p>
              <asp:Label ID="Label2" runat="server" Text="userName : "></asp:Label>
              <asp:TextBox ID="userName" runat="server" Height="22px"></asp:TextBox>
              <asp:Label ID="Label3" runat="server" Text="password : "></asp:Label>
              <asp:TextBox ID="password" runat="server" TextMode="Password"></asp:TextBox>
              <asp:Button ID="submit" runat="server" onclick="submit_Click" Text="Submit" />
          </p>
          <p>
          <asp:Label Text="Visible albums : " runat = "server"/>
          &nbsp;
          <asp:DropDownList ID="albumList" runat="server" Height="19px" Width="134px">
            
          </asp:DropDownList>
          &nbsp;&nbsp;
                    <asp:Button ID="valid" runat="server" onclick="valid_Click" Text="ShowAlbum" 
                        Width="93px" />
          </p>        
          <br />
        <br />
    </div>
    <hr />
    <asp:Panel ID="Panel1" runat="server" Height="312px" Width="906px">
    </asp:Panel>
    </form>
</body>
</html>
