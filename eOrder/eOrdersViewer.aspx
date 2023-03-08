<%@ Page Title="Ver detalles de e-Order" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="eOrdersViewer.aspx.vb" Inherits="eOrder.eOrdersViewer" %>
<asp:Content ID="Pedidos" ContentPlaceHolderID="MainContent" runat="server">
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
<div class="container"><asp:TextBox ID="TxteOrder" runat="server" CssClass="form-control" ReadOnly="true" Visible="false"></asp:TextBox>
  <div class="form-row">
    <h3 class="form-group col-md-12">e-Order # <%=TxteOrder.Text.ToString %></h3>
    <div class="form-group col-md-3">
      <label for="LblDocDate">Fecha</label>
      <asp:TextBox ID="TxtDocDate" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
    </div>
    <div class="form-group col-md-3">
      <label for="LblAlmacen">Almacén</label>
      <asp:TextBox ID="TxtAlmacen" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox><asp:TextBox ID="TxtWhsCode" runat="server" CssClass="form-control" ReadOnly="true" Visible="false"></asp:TextBox><asp:TextBox ID="TxtWhsName" runat="server" CssClass="form-control" ReadOnly="true" Visible="false"></asp:TextBox>
    </div>
    <div class="form-group col-md-3">
      <label for="LblDistribuidor">Distribuidor</label>
      <asp:TextBox ID="TxtDistribuidor" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox><asp:TextBox ID="TxtSlpCode" runat="server" CssClass="form-control" ReadOnly="true" Visible="false"></asp:TextBox><asp:TextBox ID="TxtSlpName" runat="server" CssClass="form-control" ReadOnly="true" Visible="false"></asp:TextBox>
    </div>
    <div class="form-group col-md-3">
      <label for="LblCardCode">Código de cliente</label>
      <asp:TextBox ID="TxtCardCode" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
    </div>
  </div>
 <div class="form-row">
    <div class="form-group col-md-3">
      <label for="LblCardName">Nombre del cliente</label>
      <asp:TextBox ID="TxtCardName" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
    </div>
    <div class="form-group col-md-3">
      <label for="LblTP">Tercera persona</label>
        <asp:ListBox ID="LstTP" runat="server" CssClass="form-control" ReadOnly="true"></asp:ListBox>
    </div>
    <div class="form-group col-md-3">
      <label for="LblDE">Dirección de envío</label>
        <asp:ListBox ID="LstDirs" runat="server" CssClass="form-control" ReadOnly="true"></asp:ListBox>
    </div>
    <div class="form-group col-md-3">
        <label for="LblDE2"><asp:Label ID="LblDE3" runat="server" Text=""></asp:Label></label>
        <asp:TextBox ID="TxtDir" runat="server" CssClass="form-control" Height="98px" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
    </div>
  </div>
            <div class="form-row">
                 <h3 id="deleted" class="form-group col-md-12">Artículos</h3>
                <div id="no-more-tables">
                    <table class="form-group table-bordered table-striped table-condensed cf">
                		<thead class="cf">
                			<tr>
        						<th style="width: 1%;">#</th>
                				<th style="width: 13%;">Artículo</th>
                				<th style="width: 30%;">Nombre</th>
                				<th style="width: 6%;">Cantidad</th>
                				<th style="width: 13%;">Precio</th>
                				<th style="width: 6%;">Descuento</th>
                                <th style="width: 6%;">Impuesto</th>
                                <th style="width: 9%;">I.V.A.</th>
                                <th style="width: 13%;">Sub-Total</th>
                			</tr>
                		</thead>
                		<tbody>
                            <% dim ln As Integer
                                Dim Total As Double
                                Dim STotal As Double
                                Dim Iva As Double
                                Dim ITotal As Double
                                Dim Mon As String = ""
                                Total = 0.00
                                ln = Arts.Rows.Count
                                If ln >= 1 Then
                                    For l = 0 To ln - 1 %>
                            <tr>
        						<td data-title="#"><%=l + 1%></td>
                				<td data-title="Artículo"><%=Arts.Rows.Item(l).Item(0).ToString%></td>
                				<td data-title="Nombre"><%=Arts.Rows.Item(l).Item(1).ToString%></td>
                				<td data-title="Cantidad"><span style="color:#27a41e"><%=Format(CSng(Arts.Rows.Item(l).Item(12).ToString), "#,##0")%></span>&nbsp;<span style="color:#b41f1f">[<%=Format(CSng(Arts.Rows.Item(l).Item(13).ToString), "#,##0")%>]</span></td>
                				<td data-title="Precio">$ <%=Format(CSng(Arts.Rows.Item(l).Item(3).ToString), "#,##0.00")%><% Mon = Arts.Rows.Item(l).Item(4).ToString%>&nbsp;<%=Mon%></td>
                				<td data-title="Descuento"><%=Format(CSng(Arts.Rows.Item(l).Item(5).ToString), "##0")%>%</td>
                                <td data-title="Impuesto"><%=Arts.Rows.Item(l).Item(6).ToString%></td>
                                <td data-title="I.V.A.">$ <%Iva = Arts.Rows.Item(l).Item(7)
                    ITotal = ITotal + Iva%><%=Format(CSng(Iva.ToString), "#,##0.00")%></td>
                                <td data-title="Sub-Total">$ <% 
                    STotal = Arts.Rows.Item(l).Item(8)
                    Total = Total + STotal%><%=Format(CSng(STotal.ToString), "#,##0.00")%>&nbsp;<%=Mon%></td><%Dim nrow As Integer = Arts.Rows.Item(l).Item(9).ToString%>
                			</tr>
                            <%Next%>
                            <%End If %>
            		</tbody>
            	</table>
            </div>
<h4 class="form-group col-md-12 text-right">Sub-Total: $ <%=Format(CSng(Total.ToString), "#,##0.00")%><br />Impuestos: $ <%=Format(CSng((ITotal).ToString), "#,##0.00")%><br />Total: $ <%=Format(CSng((Total + ITotal).ToString), "#,##0.00")%></h4>
        </div>
 <div class="form-row">
<h3 class="form-group col-md-12">Datos de Facturación</h3>
    <div class="form-group col-md-3">
      <label for="LblPromos">Promociones</label>
      <asp:ListBox ID="LstPromos" runat="server" CssClass="form-control" ReadOnly="true"></asp:ListBox>
    </div>
    <div class="form-group col-md-3">
      <label for="LblMega">Folio Megavolumen</label>
      <asp:ListBox ID="LstMV" runat="server" CssClass="form-control" ReadOnly="true"></asp:ListBox>
    </div>
    <div class="form-group col-md-3">
      <label for="LblStock">Tipo de stock</label>
        <asp:ListBox ID="LstStock" runat="server" CssClass="form-control" ReadOnly="true"></asp:ListBox>
    </div>
    <div class="form-group col-md-3">
      <label for="LblRefBanco">Referencía bancaría</label>
        <asp:TextBox ID="RefeBanco" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
    </div>
  </div>
 <div class="form-row">
     <div class="form-group col-md-3">&nbsp;</div><div class="form-group col-md-3">&nbsp;</div>
    <div class="form-group col-md-3">
      <label for="LblMP">Método de pago</label>
      <asp:ListBox ID="LstMP" runat="server" CssClass="form-control" ReadOnly="true"></asp:ListBox>
    </div>
    <div class="form-group col-md-3">
      <label for="LblFP">Forma de pago</label>
      <asp:ListBox ID="LstFP" runat="server" CssClass="form-control" ReadOnly="true"></asp:ListBox>
    </div>
    <div class="form-group col-md-3">
      <label for="LblUso">Uso del CFDI</label>
        <asp:ListBox ID="LstUso" runat="server" CssClass="form-control" ReadOnly="true"></asp:ListBox>
    </div>
    <div class="form-group col-md-3">
      <label for="LblTipo">Tipo de comprobante</label>
        <asp:ListBox ID="LstTipo" runat="server" CssClass="form-control" ReadOnly="true"></asp:ListBox>
    </div>
  </div>
<div align="center"><input id="VerPedido" type="button" value="Regresar" class="btn btn-lg btn-primary btn-block" onclick="window.location = 'Tracking.aspx?eOrder=<%=TxteOrder.Text.ToString%>'" /></div>
</div>
</asp:Content>

