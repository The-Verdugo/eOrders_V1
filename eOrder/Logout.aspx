<%@ Page Title="Salida" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Logout.aspx.vb" Inherits="eOrder.Logout" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
 <div class="jumbotron text-center">
        <img src="Content/overflow.png" />
        <h1>Saliendo del sistema</h1>
<div class="alert alert-success" role="alert">
  <strong>Cerrando sesión:</strong> Por favor, espere...
  <button type="button" class="close" data-dismiss="alert" aria-label="Close">
    <span aria-hidden="true">&times;</span>
  </button>
</div>
    </div>
</asp:Content>
