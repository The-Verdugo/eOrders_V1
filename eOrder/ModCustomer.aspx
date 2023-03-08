<%@ Page Title="Modificación de Cliente" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ModCustomer.aspx.vb" Inherits="eOrder.ModCustomer" %>

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
  </style>
<div class="container">
    <br />&nbsp;
  <div class="form-row">
        <div class="form-group col-md-3">
      <label for="LblDistribuidor">Usuario</label>
        <asp:TextBox ID="TxtUser" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
    </div>
    <div class="form-group col-md-3">
      <label for="LblDocDate">Almacén</label>
        <asp:TextBox ID="TxtAlmacen" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
     <asp:TextBox ID="TxtWhsCode" runat="server" CssClass="form-control" ReadOnly="true" Visible="false"></asp:TextBox>
    <asp:TextBox ID="TxtWhsName" runat="server" CssClass="form-control" ReadOnly="true" Visible="false"></asp:TextBox>
    </div>
    <div class="form-group col-md-3">
      <label for="LblDistribuidor">Distribuidor</label>
        <asp:TextBox ID="TxtDistribuidor" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
      <asp:TextBox ID="TxtSlpCode" runat="server" CssClass="form-control" ReadOnly="true" Visible="false"></asp:TextBox>
    <asp:TextBox ID="TxtSlpName" runat="server" CssClass="form-control" ReadOnly="true" Visible="false"></asp:TextBox>
    </div>
  </div>
 <div class="form-row">
    <div class="form-group col-md-3">
      <label for="LblCardCode">Código de cliente</label>
      <asp:TextBox ID="TxtCardCode" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
    </div>
    <div class="form-group col-md-3">
      <label for="LblCardName">Nombre del cliente</label>
      <asp:TextBox ID="TxtCardName" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
    </div>
    <div class="form-group col-md-3">
      <label for="LblMes">Direcciones de Entrega</label>
     <asp:ListBox ID="LstDirsE" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="LstDirsE_SelectedIndexChanged"></asp:ListBox>
    </div>
  </div>
            <div class="form-row">
                 <h3 id="title" class="form-group col-md-12">Bandeja de e-Orders</h3>
                <div id="no-more-tables">
                    <table class="form-group table-bordered table-striped table-condensed cf">
                		<thead class="cf">
                			<tr>
        						<th style="width: 9%;">Folio</th>
                                <th style="width: 8%;">Fecha</th>
                				<th style="width: 6%;">Ref. SAP</th>
                                <th style="width: 10%;">Código</th>
                				<th style="width: 17%;">Nombre de cliente</th>
                				<th style="width: 6%;">Cantidad</th>
                				<th style="width: 11%;">Monto</th>
                				<th style="width: 18%;">Comentarios</th>
                                <th style="width: 15%;">Ultimo estado</th>
                			</tr>
                		</thead>
                		<tbody>
                            <% dim ln As Integer
                                Dim Total As Double
                                Dim fol As String
                                Total = 0.00
                                ln = eOrders.Rows.Count
                                If ln >= 1 Then
                                    For l = 0 To ln - 1 %>
                            <tr>
        						<td data-title="Folio"><%Fol=eOrders.Rows.Item(l).Item(0).ToString%><a href="Tracking.aspx?eOrder=<%=fol%>"><%=fol%></a></td>
                                <td data-title="Fecha"><%=eOrders.Rows.Item(l).Item(1).ToString%></td>
                				<td data-title="Ref. SAP"><%=eOrders.Rows.Item(l).Item(2).ToString%></td>
                				<td data-title="Código"><%=eOrders.Rows.Item(l).Item(3).ToString%></td>
                				<td data-title="Nombre de cliente"><%=eOrders.Rows.Item(l).Item(4).ToString%></td>
                				<td data-title="Cantidad"><%=Format(CSng(eOrders.Rows.Item(l).Item(5).ToString), "#,##0")%></td>
                				<td data-title="Monto">$<%=Format(CSng(eOrders.Rows.Item(l).Item(6).ToString), "#,##0.00")%>&nbsp;<%= eOrders.Rows.Item(l).Item(7).ToString%></td>
                                <td data-title="Comentarios"><%=eOrders.Rows.Item(l).Item(8).ToString%></td>
                                <td data-title="Último Estado"><%=eOrders.Rows.Item(l).Item(9).ToString%></td>
                			</tr>
                            <%Next%>
                             <%Else %>
                            <tr>
        						<td colspan="9" style="text-align:center;">Lo sentimos no se encontró ninguna e-order.</td>
                			</tr>
                            <%End If %>
            		</tbody>
            	</table>
            </div>
<h4 class="form-group col-md-12 text-right">Usted ha realizado <%=ln%> e-orders.</h4>
        </div>
</div>
</asp:Content>
