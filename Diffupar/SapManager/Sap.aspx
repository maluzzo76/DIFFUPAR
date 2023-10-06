<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sap.aspx.cs" Inherits="SapManager.Sap" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
             <h1>SAP manager</h1>
            <label>Query</label><asp:Button ID="btnRun" runat="server" Text="Ejecutar" OnClick="btnRun_Click" /><asp:Label runat="server" ID="lberror" ForeColor="Red"></asp:Label><br />     
            <asp:TextBox ID="txtTableName" runat="server" Width="100%" Height="100px" TextMode="MultiLine"></asp:TextBox>           
            <asp:DataGrid id="grdColumns" runat="server"></asp:DataGrid>
            <br />
            <asp:DataGrid id="grdData" runat="server"></asp:DataGrid>
        </div>
    </form>
</body>
</html>
