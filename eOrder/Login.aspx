<%@ Page Title="Acceder al sistema" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.vb" Inherits="eOrder.Login" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron text-center">
        <img src="http://it.gvi.com.mx/images/benja.png" />
        <h1><%: Title %></h1>
        <p><asp:Label ID="Label1" runat="server" Text="Nombre de usuario:"></asp:Label><br /><asp:TextBox ID="Username" runat="server"></asp:TextBox></p>
        <p><asp:Label ID="Label2" runat="server" Text="Clave de acceso:"></asp:Label><br /><asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox></p>
 <% If Me.ErrorTitulo.Text <> "" Then %><div class="alert alert-danger" role="alert">
  <strong><asp:Label ID="ErrorTitulo" runat="server" Text=""></asp:Label></strong> <asp:Label ID="ErrorMsg" runat="server" Text=""></asp:Label>
  <button type="button" class="close" data-dismiss="alert" aria-label="Close">
    <span aria-hidden="true">&times;</span>
  </button>
</div><% End If %>
        <asp:Button ID="BtnLogin" runat="server" Text="Iniciar sesión" CssClass="btn btn-primary btn-lg" />
        <br />
    </div>
</asp:Content>
