<%@ Page Title="Nueva e-Order" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="eOrders.aspx.vb" Inherits="eOrder.eOrders" %>
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
        display:block;
        }
        .campo-vacio {
            border-color: red !important;
            background-color: #ffe6e6 !important;
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

.typeahead,
.tt-query,
.tt-hint {
 /*width: 100%;
  height: 35px;
  padding: 2px 2px;	
  font-size: 12px;
  line-height: 2px;*/
  /*border: 2px solid #ccc;
  -webkit-border-radius: 8px;
     -moz-border-radius: 8px;
          border-radius: 8px;*/
  outline: none;
}

.typeahead {
  background-color: #fff;
}

.typeahead:focus {
  /*border: 1px solid #26a0da;*/
}

.tt-query {
  -webkit-box-shadow: inset 0 1px 1px rgba(0, 0, 0, 0.075);
     -moz-box-shadow: inset 0 1px 1px rgba(0, 0, 0, 0.075);
          box-shadow: inset 0 1px 1px rgba(0, 0, 0, 0.075);
}

.tt-hint {
  color: #999
}

.tt-menu {
  width: 500px; /*310*/
  height: 300px; /*220*/
  /*width: 100%;*/
  margin: 7px 0;
  padding: 4px 0;
  background-color: #fff;
  -webkit-border-radius: 8px;
     -moz-border-radius: 8px;
          border-radius: 8px;
  -webkit-box-shadow: 0 5px 10px rgba(0,0,0,.2);
     -moz-box-shadow: 0 5px 10px rgba(0,0,0,.2);
          box-shadow: 0 5px 10px rgba(0,0,0,.2);
}

.tt-suggestion {
  padding: 3px 20px;
  /*font-size: 12px;*/
  line-height: 24px;
}

.tt-suggestion:hover {
  cursor: pointer;
  color: #fff;
  background-color: #26a0da;
}

.tt-suggestion.tt-cursor {
  color: #fff;
  background-color:#26a0da;

}

.tt-suggestion p {
  margin: 0;
}

.gist {
  font-size: 12px;
}

#scrollable-dropdown-menu .tt-menu {
  /*max-height: 150px;*/
  overflow-y: auto;
}

#scrollable-dropdown-menu2 .tt-menu {
  /*max-height: 150px;*/
  overflow-y: auto;
}
  </style>
<div class="container">
<% If Me.lblErrId.Text = "Error" Then %><br />&nbsp;<div class="alert alert-danger" role="alert">
  <strong><asp:Label ID="ErrorNum" runat="server" Text=""></asp:Label>:</strong> <asp:Label ID="ErrorMsg" runat="server" Text=""></asp:Label>
  <button type="button" class="close" data-dismiss="alert" aria-label="Close">
    <span aria-hidden="true">&times;</span>
  </button>
</div><% End If %>
<% If Me.lblErrId.Text = "OK" Then %><br />&nbsp;<div class="alert alert-success" role="alert">
  <strong><asp:Label ID="OkNum" runat="server" Text=""></asp:Label>:</strong> <asp:Label ID="OkMsg" runat="server" Text=""></asp:Label>
  <button type="button" class="close" data-dismiss="alert" aria-label="Close">
    <span aria-hidden="true">&times;</span>
  </button>
</div><% End If %>
<% If btnGenerarPedido.Enabled = True Then %>
    <div class="form-row"><asp:TextBox ID="TxtFechaHora" runat="server" ReadOnly="true" Visible="false"></asp:TextBox>
    <h3 id="reloj" class="form-group col-md-12" style="text-align:center;">Calculando...</h3>
    </div><script>
              // Set the date we're counting down to
              var countDownDate = new Date("<%=TxtFechaHora.Text.ToString %>").getTime();
              /*var countDownDate = new Date().getTime();
              var diff = InitDate - countDownDate;
              countDownDate += diff;
              countDownDate = new Date(countDownDate);*/

              // Update the count down every 1 second
              var x = setInterval(function () {

                  // Get today's date and time
                  var now = new Date().getTime();

                  // Find the distance between now and the count down date
                  var distance = countDownDate - now;

                  // Time calculations for days, hours, minutes and seconds
                  var days = Math.floor(distance / (1000 * 60 * 60 * 24));
                  var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
                  var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
                  var seconds = Math.floor((distance % (1000 * 60)) / 1000);

                  // Output the result in an element with id="demo"
                  document.getElementById("reloj").innerHTML = "Calcular stock:&nbsp;<b>" + minutes + "m " + seconds + "s </b>";

                  // If the count down is over, write some text 
                  if (distance < 0) {
                      clearInterval(x);
                      document.getElementById("reloj").innerHTML = "Calculando...";
                      window.location = 'eOrders.aspx?timeover=15'
                  }
              }, 1000);
</script><% End If %>
  <div class="form-row">
    <h3 class="form-group col-md-12">Datos Generales</h3>
      <div class="form-group col-md-3">
          <label for="LblCardCode">Código de cliente</label>
          <div id="scrollable-dropdown-menu">
              <asp:TextBox ID="TxtCardCode" runat="server" CssClass="typeahead form-control" AutoPostBack="True" OnTextChanged="TxtCardCode_TextChanged" onblur="javascript:setTimeout('__doPostBack(\'ctl00$MainContent$TxtCardCode\',\'\')', 0)"></asp:TextBox></div>
      </div>
    <div class="form-group col-md-3">
      <label for="LblDocDate">Fecha de solicitud</label>
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
  </div>
 <div class="form-row">
    <div class="form-group col-md-3">
      <label for="LblCardName">Nombre del cliente</label>
      <asp:TextBox ID="TxtCardName" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
    </div>
    <div class="form-group col-md-3">
      <label for="LblTP">Tercera persona</label>
        <asp:ListBox ID="LstTP" runat="server" CssClass="form-control"></asp:ListBox>
        <a id="btnAddTP" runat="server" class="btn btn-primary btn-sm" data-toggle="modal" data-target="#ModalTP">Agregar Nuevo</a>
    </div>
    <div class="form-group col-md-3">
      <label for="LblDE">Dirección de envío</label>&nbsp;<a href="ModCustomer.aspx?cardcode=<%=TxtCardCode.Text.ToString%>">[Modificar]</a>
        <asp:ListBox ID="LstDirs" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="LstDirs_SelectedIndexChanged"></asp:ListBox>
    </div>
    <div class="form-group col-md-3">
        <label for="LblDE2"><asp:Label ID="LblDE3" runat="server" Text=""></asp:Label></label>
        <asp:TextBox ID="TxtDir" runat="server" CssClass="form-control" Height="98px" TextMode="MultiLine"></asp:TextBox>
    </div>
      <div class="form-group col-md-4">
                <label for="Lblmail">e-mail cliente</label>
                <asp:TextBox ID="TextMail" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
  </div>
     <div class="form-row">
            <div class="form-group col-md-4">
                <label for="LblCardName">Comentario</label>
                <asp:TextBox ID="TextComment" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
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
                                <th style="width: 3%;"></th>
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
                                <td data-title=""><input id="del.<%=nrow.ToString%>" type="button" value="x" class="btn btn-labeled btn-danger" onclick="window.location = 'eOrders.aspx?l=<%=nrow.ToString%>&c=<%=TxtCardCode.Text.ToString%>'" /></td>
                			</tr>
                            <%Next%>
                            <%End If %>
                            <tr>
        						<td data-title="#"><%=ln + 1%></td>
                				<td data-title="Artículo"><div id="scrollable-dropdown-menu2"><asp:TextBox ID="TxtItemCode" runat="server" CssClass="typeahead form-control" AutoPostBack="True" OnTextChanged="TxtItemCode_TextChanged" onblur="javascript:setTimeout('__doPostBack(\'ctl00$MainContent$TxtItemCode\',\'\')', 0)" Width="100%"></asp:TextBox></div></td>
                				<td data-title="Nombre"><asp:Label ID="LblNombre" runat="server" Text=""></asp:Label></td>
                                <td data-title="Cantidad">Disponible:<br /><span style="color:#27a41e"><asp:Label ID="Disponible" runat="server" Text="0"></asp:Label></span><br />Solicitado:<br /><asp:TextBox ID="TxtCantidad" runat="server" CssClass="form-control" AutoPostBack="True" OnTextChanged="TxtCantidad_TextChanged" Width="100%"></asp:TextBox></td>
                				<td data-title="Precio">$ <asp:Label ID="LblPrecio" runat="server" Text="0.00"></asp:Label>&nbsp;<asp:Label ID="lblMoneda" runat="server" Text="MXP"></asp:Label></td>
                				<td data-title="Descuento"><asp:TextBox ID="TxtDescuento" runat="server" CssClass="form-control" AutoPostBack="True" OnTextChanged="TxtDescuento_TextChanged" Width="100%"></asp:TextBox></td>
                                <td data-title="Impuesto"><asp:Label ID="LblImpuesto" runat="server" Text=""></asp:Label></td>
                                <td data-title="I.V.A.">$ <asp:Label ID="LblIVA" runat="server" Text="0.00"></asp:Label></td>
                                <td data-title="Sub-Total">$ <asp:Label ID="LblSubTotal" runat="server" Text="0.00"></asp:Label>&nbsp;<asp:Label ID="lblMon" runat="server" Text="MXP"></asp:Label></td>
                                <td data-title=""><asp:TextBox ID="TxtDisp" runat="server" Visible="false"></asp:TextBox><asp:TextBox ID="TxtTP" runat="server" Visible="false"></asp:TextBox><asp:Button ID="btnAdd" runat="server" Text="+" CssClass="btn btn-labeled btn-success" /></td>
                			</tr>
            		</tbody>
            	</table>
            </div>
<h4 class="form-group col-md-12 text-right">Sub-Total: $ <%=Format(CSng(Total.ToString), "#,##0.00")%><br />Impuestos: $ <%=Format(CSng((ITotal).ToString), "#,##0.00")%><br />Total: $ <%=Format(CSng((Total + ITotal).ToString), "#,##0.00")%></h4>
        </div>
 <div class="form-row">
<h3 class="form-group col-md-12">Datos de Facturación</h3>
    <div class="form-group col-md-3">
      <label for="LblPromos">Promociones</label>
      <asp:ListBox ID="LstPromos" runat="server" CssClass="form-control"></asp:ListBox>
    </div>
    <div class="form-group col-md-3">
      <label for="LblMega">Folio Megavolumen</label>
      <asp:ListBox ID="LstMV" runat="server" CssClass="form-control"></asp:ListBox>
    </div>
    <div class="form-group col-md-3">
      <label for="LblStock">Tipo de stock</label>
        <asp:ListBox ID="LstStock" runat="server" CssClass="form-control"></asp:ListBox>
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
      <asp:ListBox ID="LstMP" runat="server" CssClass="form-control"></asp:ListBox>
    </div>
    <div class="form-group col-md-3">
      <label for="LblFP">Forma de pago</label>
      <asp:ListBox ID="LstFP" runat="server" CssClass="form-control"></asp:ListBox>
    </div>
    <div class="form-group col-md-3">
      <label for="LblUso">Uso del CFDI</label>
        <asp:ListBox ID="LstUso" runat="server" CssClass="form-control"></asp:ListBox>
    </div>
    <div class="form-group col-md-3">
      <label for="LblTipo">Tipo de comprobante</label>
        <asp:ListBox ID="LstTipo" runat="server" CssClass="form-control"></asp:ListBox>
    </div>
  </div>
 <div align="center"><br />&nbsp;<asp:Label ID="lblErrId" runat="server" Text="" Visible="false"></asp:Label><asp:Button ID="btnGenerarPedido" runat="server" Text="Generar" CssClass="btn btn-lg btn-primary btn-block" /><br />&nbsp;</div>
</div>

       <!-- Modal -->
        <div class="modal fade" id="ModalTP" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
            <div class="modal-dialog modal-sm" role="form">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title" id="myModalLabel">Agregar Tercera persona</h4>
                    </div>
                    <div class="modal-body">
                        <div class="row-no-gutters">
                            <label>Tipo*</label>
                            <asp:DropDownList runat="server" ID="TipoTP" CssClass="form-control">
                                <asp:ListItem Text="M - Mostrador" Value="M"></asp:ListItem>
                                <asp:ListItem Text="ME - Mostrador Externo" Value="ME"></asp:ListItem>
                                <asp:ListItem Text="P - Propios" Value="P" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="T - Tercero" Value="T"></asp:ListItem>
                            </asp:DropDownList>

                            <label>Nombre*</label>
                            <asp:TextBox ID="TextNombre" runat="server" CssClass="form-control"></asp:TextBox>

                            <label>Apellido paterno*</label>
                            <asp:TextBox ID="TextAPP" runat="server" CssClass="form-control"></asp:TextBox>

                            <label>Apellido Materno</label>
                            <asp:TextBox ID="TextAPM" runat="server" CssClass="form-control"></asp:TextBox>

                            <label>RFC*</label>
                            <asp:TextBox ID="TextRFC" runat="server" CssClass="form-control"></asp:TextBox>
                            <hr />
                            <h4>Domicilio</h4>
                            <label>Calle*</label>
                            <asp:TextBox ID="TextCalle" runat="server" CssClass="form-control"></asp:TextBox>
                            <label>No. Exterior*</label>
                            <asp:TextBox ID="TextNoEx" runat="server" CssClass="form-control"></asp:TextBox>
                            <label>No. Interior</label>
                            <asp:TextBox ID="TextNoInt" runat="server" CssClass="form-control"></asp:TextBox>
                            <label>Colonia</label>
                            <asp:TextBox ID="TextColonia" runat="server" CssClass="form-control"></asp:TextBox>
                            <label>Municipio</label>
                            <asp:TextBox ID="TextMunicipio" runat="server" CssClass="form-control"></asp:TextBox>
                            <label>Cuidad</label>
                            <asp:TextBox ID="TextCuidad" runat="server" CssClass="form-control"></asp:TextBox>
                            <label>Estado*</label>
                            <asp:TextBox ID="TextEstado" runat="server" CssClass="form-control"></asp:TextBox>
                            <label>Pais*</label>
                            <asp:DropDownList ID="TextPais" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Mexico" Value="MX"></asp:ListItem>
                            </asp:DropDownList>
                            <label>C.P.*</label>
                            <asp:TextBox ID="TextCP" runat="server" CssClass="form-control"></asp:TextBox>
                            <hr />
                            <label>Tipo C*</label>
                            <asp:DropDownList runat="server" ID="DropDownTipoC" CssClass="form-control">
                                <asp:ListItem Text="M - Medicos" Value="M"></asp:ListItem>
                                <asp:ListItem Text="H - Hospitales" Value="H"></asp:ListItem>
                                <asp:ListItem Text="A - Aseguradoras" Value="A"></asp:ListItem>
                                <asp:ListItem Text="E - Empresas" Value="E"></asp:ListItem>
                                <asp:ListItem Text="F - Farmacias" Value="F"></asp:ListItem>
                            </asp:DropDownList>
                            <label>Regimen Fiscal*</label>
                            <asp:DropDownList runat="server" ID="DropDownRegimen" CssClass="form-control">
                                <asp:ListItem Text="601 - General de ley personas morales" Value="601"></asp:ListItem>
                                <asp:ListItem Text="603 - Personas Morales con Fines no lucrativos" Value="603"></asp:ListItem>
                                <asp:ListItem Text="605 - Sueldos y Salarios e Ingresos Asimilados a Salarios" Value="605"></asp:ListItem>
                                <asp:ListItem Text="606 - Arrendamiento" Value="606"></asp:ListItem>
                                <asp:ListItem Text="607 - Régimen de Enajenación o Adquisicion de Bienes" Value="607"></asp:ListItem>
                                <asp:ListItem Text="608 - Demás ingresos" Value="608"></asp:ListItem>
                                <asp:ListItem Text="609 - Consolidación" Value="609"></asp:ListItem>
                                <asp:ListItem Text="610 - Residentes en el Extrangero sin establecimiento Permanente en México" Value="610"></asp:ListItem>
                                <asp:ListItem Text="611 - Ingresos por Dividendos (socios y accionistas)" Value="611"></asp:ListItem>
                                <asp:ListItem Text="612 - Personas fisicas con actividades empresariales y profesionales" Value="612"></asp:ListItem>
                                <asp:ListItem Text="614 - Ingresos por intereses" Value="614"></asp:ListItem>
                                <asp:ListItem Text="615 - Régimen de los ingresos por obtencion de premios" Value="615"></asp:ListItem>
                                <asp:ListItem Text="616 - Sin obligaciones fiscales" Value="616"></asp:ListItem>
                                <asp:ListItem Text="620 - Sociedades Cooperativas de Producción que optan por diferir sus ingresos" Value="620"></asp:ListItem>
                                <asp:ListItem Text="621 - Incorporación fiscal" Value="621"></asp:ListItem>
                                <asp:ListItem Text="622 - Actividades agrícolas, Ganaderas, Silvícolas y Pesqueras" Value="622"></asp:ListItem>
                                <asp:ListItem Text="623 - Opcional para grupos de sociedades" Value="623"></asp:ListItem>
                                <asp:ListItem Text="624 - Coordinados" Value="624"></asp:ListItem>
                                <asp:ListItem Text="625 - Régimen de las actividades Empresariales con ingresos a través de plataformas tecnológicas" Value="625"></asp:ListItem>
                                <asp:ListItem Text="626 - Régimen simplificado de confianza" Value="626"></asp:ListItem>
                                <asp:ListItem Text="628 - Hidrocarburos" Value="628"></asp:ListItem>
                                <asp:ListItem Text="629 - De los regimenes fiscales preferentes y de las empresas multinacionales" Value="629"></asp:ListItem>
                                <asp:ListItem Text="630 - Enajenación de acciones en la bolsa de valores" Value="630"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                        <%--<asp:Button runat="server" type="button" ID="btnsaveContacto" OnClick="btnsaveContacto_Click" class="btn btn-primary" Text="Guardar"></asp:Button>--%>
                    </div>
                </div>
            </div>
        </div>

<script src="https://twitter.github.io/typeahead.js/js/handlebars.js"></script>
<script src="https://twitter.github.io/typeahead.js/js/jquery-1.10.2.min.js"></script>
<script src="https://twitter.github.io/typeahead.js/releases/latest/typeahead.bundle.js"></script>
<script src="http://it.gvi.com.mx/codigos/cust.data.json.asp"></script>
<script src="http://it.gvi.com.mx/codigos/items.data.json.asp"></script>
<script>
    $(document).ready(function () {

        $('#scrollable-dropdown-menu .typeahead').typeahead(null, {
            name: 'CardCode',
            limit: 35,
            source: cardcodes
        });
        
        $('#scrollable-dropdown-menu2 .typeahead').typeahead(null, {
            name: 'Items',
            limit: 35,
            display: 'product',
            source: itemcodes,
            templates: {
                empty: ['<div>&nbsp;&nbsp;&nbsp;&nbsp;<img src="http://it.gvi.com.mx/images/system-error-icon.png" width="91px" /> No se ha encontrado el artículo! </div>'].join('\n'),
                suggestion: Handlebars.compile('<div class="row" style="width:100%;" ><div class= "col-xs-4 col-sm-4 col-lg-4" style="width:100px;"><img src="{{img}}" width="90px" /></div><div class="col-xs-8 col-sm-8 col-lg-8"><p>{{product}}</p></div></div>')
            }
        });

    });

    function agregarTerceraPersona() {
        // Obtener los campos del formulario
        var tipoTP = document.getElementById("TipoTP");
        var nombre = document.getElementById("TextNombre");
        var app = document.getElementById("TextAPP");
        var apm = document.getElementById("TextAPM");
        var rfc = document.getElementById("TextRFC");
        var calle = document.getElementById("TextCalle");
        var noEx = document.getElementById("TextNoEx");
        var noInt = document.getElementById("TextNoInt");
        var colonia = document.getElementById("TextColonia");
        var municipio = document.getElementById("TextMunicipio");
        var cuidad = document.getElementById("TextCuidad");
        var estado = document.getElementById("TextEstado");
        var pais = document.getElementById("TextPais");
        var cp = document.getElementById("TextCP");
        var tipoC = document.getElementById("DropDownTipoC");
        var regimen = document.getElementById("DropDownRegimen");

        // Establecer los campos como solo lectura
        tipoTP.setAttribute("readonly", true);
        nombre.setAttribute("readonly", true);
        app.setAttribute("readonly", true);
        apm.setAttribute("readonly", true);
        rfc.setAttribute("readonly", true);
        calle.setAttribute("readonly", true);
        noEx.setAttribute("readonly", true);
        noInt.setAttribute("readonly", true);
        colonia.setAttribute("readonly", true);
        municipio.setAttribute("readonly", true);
        cuidad.setAttribute("readonly", true);
        estado.setAttribute("readonly", true);
        pais.setAttribute("readonly", true);
        cp.setAttribute("readonly", true);
        tipoC.setAttribute("readonly", true);
        regimen.setAttribute("readonly", true);

        // Verificar los campos obligatorios
        var camposObligatorios = [tipoTP, nombre, app, rfc, calle, noEx, estado, pais, cp];
        for (var i = 0; i < camposObligatorios.length; i++) {
            if (camposObligatorios[i].value.trim() === "") {
                camposObligatorios[i].classList.add("campo-vacio");
            }
        }
    }
</script>

<!-- Agregar esta clase CSS al archivo de estilo -->

</asp:Content>
