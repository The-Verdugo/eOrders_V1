<%@ Page Title="Inicio" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="eOrder._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron text-center">
        <img src="http://it.gvi.com.mx/images/caro.png" />
        <h1>e-SAC</h1>
        <p>Servicio Electrónico de Atención a Clientes</p>
    </div>
    <div class="row">
        <div class="col-md-4">
            <img src="http://it.gvi.com.mx/images/monitor.png" />
            <h2>Pedido</h2>
            <p>La ordén de venta electrónica le ayuda a generar pedidos para cliente directamente a su centro de atención.</p>
            <p>
                <a class="btn btn-default" href="eOrders.aspx">Abrir &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <img src="http://it.gvi.com.mx/images/bandeja.png" />
            <h2>Ordenes</h2>
            <p>Revise su historial de e-Orders y lleve el seguimiento a cada una de ellas.</p>
            <p>
                <a class="btn btn-default" href="Inbox.aspx">Abrir &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <img src="http://it.gvi.com.mx/images/checo.png" />
            <h2>Ubicar</h2>
            <p>Compruebe el estado de cada orden de venta electrónica.</p>
            <p>
                <a class="btn btn-default" href="Tracking.aspx">Abrir &raquo;</a>
            </p>
        </div>
    </div>

</asp:Content>
