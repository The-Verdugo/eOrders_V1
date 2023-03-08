<%@ Page Title="Rastreo de e-Orders" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Tracking.aspx.vb" Inherits="eOrder.Tracking" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style id="compiled-css" type="text/css">
      @media only screen and (max-width: 800px) {
        /* Force table to not be like tables anymore */
        #no-more-tables table,
        #no-more-tables thead,
        #no-more-tables tbody,
        #no-more-tables th,
        #no-more-tables td,
        #no-more-tables tr {
        display: block;
        }
         
        /* Hide table headers (but not display: none;, for accessibility) */
        #no-more-tables thead tr {
        position: absolute;
        top: -9999px;
        left: -9999px;
        }
         
       /* #no-more-tables tr { border: 1px solid #ccc; }*/
          
        #no-more-tables td {
        /* Behave like a "row" */
        border: none;
        border-bottom: 1px solid #eee;
        position: relative;
        padding-left: 50%;
        white-space: normal;
        text-align:left;
        }
         
        #no-more-tables td:before {
        /* Now like a table header */
        position: absolute;
        /* Top/left values mimic padding */
        top: 6px;
        left: 1px;
        width: 45%;
        padding-right: 1px;
        white-space: nowrap;
        text-align:left;
        font-weight: bold;
        }
         
        /*
        Label the data
        */
        #no-more-tables td:before { content: attr(data-title); }
        }
        .search1
        {
            background: url(./find.png) no-repeat !important;
            background-position:100% 45% !important;
            /*padding-left: 22px;
            height: 20px !important;
            border: 1px solid #ccc !important;
            font-size:10pt !important;
            line-height:20px !important*/
        }
        .imgscenter {
  display: block;
  margin-left: auto;
  margin-right: auto;
  width: 150px;
}
  </style>
<div class="container">
    <br />&nbsp;
  <div class="form-row">
        <div class="form-group col-md-12"><div align="center">
<asp:TextBox ID="TxtFolio" runat="server" AutoPostBack="true" CssClass="search1 form-control"></asp:TextBox>
</div>
    </div>
  </div>
<div class="form-row">
                <% dim ln As Integer
                    Dim Id As String = "d"
                    Dim Tarea As String = "# Track Id desconocido"
                    Dim FE As DateTime
                    Dim FI As DateTime
                    Dim FF As DateTime = Now
                    ln = TrackOrder.Rows.Count
                    If ln >= 1 Then
                        For l = 0 To ln - 1
                            Id = TrackOrder.Rows.Item(l).Item(0).ToString
                            Tarea = TrackOrder.Rows.Item(l).Item(1).ToString
                        Next%>
                            <h3 id="title" class="form-group"><%=Tarea%></h3><img src="./<%=Id%>.png" alt="<%=Tarea%>" class="imgscenter"><br />&nbsp;<%End If%>
 <%         Dim e As Integer
     Dim Guia As String = ""
     e = TrackOrderEncabezado.Rows.Count
     If e >= 1 Then
         e = 0%>
    <div class="form-row">
        <div class="form-group col-md-3">
      <label for="LblDistribuidor">Fecha</label>
       <br /><%=TrackOrderEncabezado.Rows.Item(e).Item(0).ToString%>
    </div>
    <div class="form-group col-md-3">
      <label for="LblDocDate">Ref. SAP</label>
        <br /><%=TrackOrderEncabezado.Rows.Item(e).Item(1).ToString%>
    </div>
    <div class="form-group col-md-3">
      <label for="LblDistribuidor">Cliente</label>
      <br /><%=TrackOrderEncabezado.Rows.Item(e).Item(2).ToString%>&nbsp;<%=TrackOrderEncabezado.Rows.Item(e).Item(3).ToString%>
    </div>
    <div class="form-group col-md-3">
      <label for="LblDistribuidor">Total</label>
      <br />$ <%=Format(CSng(TrackOrderEncabezado.Rows.Item(e).Item(5).ToString), "#,##0.00")%>&nbsp;<%=TrackOrderEncabezado.Rows.Item(e).Item(6).ToString%> (<%=Format(CSng(TrackOrderEncabezado.Rows.Item(e).Item(4).ToString), "#,##0")%> piezas)
    </div>
  </div>
    <div class="form-row">
    <div class="form-group col-md-12">
      <label for="LblDistribuidor">Comentarios</label>
        <br /><%=TrackOrderEncabezado.Rows.Item(e).Item(7).ToString%><%Guia = TrackOrderEncabezado.Rows.Item(e).Item(8).ToString%>
    </div>
    </div>
      <% End If %>
                 <h3 id="title" class="form-group col-md-12"># <%=TxtFolio.Text.ToString %></h3>
                <div id="no-more-tables">
                    <table class="form-group table-bordered table-striped table-condensed cf">
                		<thead class="cf">
                			<tr>
        						<th style="width: 27%;">Tarea</th>
                                <th style="width: 30%;">Almacén</th>
                				<th style="width: 30%;">Usuario</th>
                                <th style="width: 8%;">Fecha</th>
                				<th style="width: 5%;">Hora</th>
                			</tr>
                		</thead>
                		<tbody>
                            <% If ln >= 1 Then
                                    For l = 0 To ln - 1 %>
                            <tr>
        						<td data-title="Tarea"><%Id = TrackOrder.Rows.Item(l).Item(0).ToString%><%=TrackOrder.Rows.Item(l).Item(1).ToString%></td>
                                <td data-title="Almacén"><%=TrackOrder.Rows.Item(l).Item(2).ToString%></td>
                				<td data-title="Usuario"><%=TrackOrder.Rows.Item(l).Item(3).ToString%></td>
                				<td data-title="Fecha"><%FE = TrackOrder.Rows.Item(l).Item(4).ToString
                                                           If l = 0 Then
                                                               FI = FE
                                                           End If
                                                           If l = ln - 1 And l >= 1 Then
                                                               FF = FE
                                                           End If
                                                           %><%=FE.ToString("dd-MM-yyyy") %></td>
                				<td data-title="Hora"><%Dim tm As String = TrackOrder.Rows.Item(l).Item(5).ToString
                                                          Dim hra As String
                                                          Dim mn As String
                                                          hra = Left(tm, tm.Length - 2)
                                                          mn = Right(tm, 2)
                                                          %><%=hra%>:<%=mn%></td>
                			</tr>
                            <%Next%>
                             <%Else %>
                            <tr>
        						<td colspan="5" style="text-align:center;"><%If TxtFolio.Text.ToString <> "" Then %>Lo sentimos no se encontró ninguna e-order con el TrackId # <b><%=TxtFolio.Text.ToString %></b>.<% Else %>Ingrese un # TrackId en la parte superior, para comenzar el rastreo. <%End If %></td>
                			</tr>
                            <%End If %>
            		</tbody>
            	</table><% Dim Usrnm As String = ""

    Try
        Usrnm = Session("UserName").ToString()
    Catch ex As Exception
        Usrnm = ""
    End Try

%>
            </div><% If ln >= 1 And TxtFolio.Text.ToString <> "" And TxtFolio.Text.ToString.Contains("-") And (TxtFolio.Text.ToString.Contains("E") Or TxtFolio.Text.ToString.Contains("M")) Then%>
<h4 class="form-group col-md-12 text-center"><b>DURACIÓN:</b> <%Dim dias As Integer = Math.Floor((FF - FI).TotalDays)%><%=dias%> Dia(s) <%dim horas As Integer = Math.Floor((FF - FI).TotalHours) - (dias * 24)%><%=horas %> Hora(s) <%dim mi As Integer = Math.Floor((FF - FI).TotalMinutes) - (horas * 60) - (dias * 24 * 60)%><%=mi%> Minuto(s) <%dim seg As Integer = Math.Floor((FF - FI).TotalSeconds) - (horas * 60 * 60) - (dias * 24 * 60 * 60) - (mi * 60)%><%=seg%> Segundo(s)</h4><% End If %>
        </div>
<% If ln >= 1 And TxtFolio.Text.ToString <> "" And TxtFolio.Text.ToString.Contains("-") And (TxtFolio.Text.ToString.Contains("E") Or TxtFolio.Text.ToString.Contains("B")) And Usrnm <> "" Then%>
    <div class="form-row">
        <div class="form-group col-md-12"><div align="center">
<input id="VerPedido" type="button" value="Ver Pedido" class="btn btn-lg btn-primary btn-block" onclick="window.location = 'eOrdersViewer.aspx?eOrder=<%=TxtFolio.Text.ToString%>'" />
</div>
    </div>
  </div>
<%End If
    If ln >= 1 And TxtFolio.Text.ToString <> "" And TxtFolio.Text.ToString.Contains("-") And (TxtFolio.Text.ToString.Contains("E") Or TxtFolio.Text.ToString.Contains("B") Or TxtFolio.Text.ToString.Contains("M") Or TxtFolio.Text.ToString.Contains("C")) Then%>
<div class="form-row">
<div align="center"><h3 id="title" class="form-group">Rastrear paquetería:</h3>
<% If Guia <> "0" And Guia <> "" And Guia <> "NULL" Then %>
      <a href="http://www.dhl.com.mx/es/express/rastreo.html?AWB=<%=Guia%>&brand=DHL" target="_blank"><img src="./dhl.png" alt="DHL" style="width:151px;"></a>&nbsp;&nbsp;&nbsp;<a href="https://www.fedex.com/apps/fedextrack/index.html?action=track&tracknumbers=<%=Guia%>&locale=es_MX&cntry_code=mx" target="_blank"><img src="./fedex.png" alt="FedEx" style="width:151px;"></a>
  <%Else%><img src="./dhlbn.png" alt="DHL" style="width:151px;">&nbsp;&nbsp;&nbsp;<img src="./fedexbn.png" alt="FedEx" style="width:151px;"><%End If%></div>
</div>
<% End If %>
    </div>
</asp:Content>