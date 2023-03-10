Imports System.Data.SqlClient
Public Class eOrders
    Inherits Page
    Public Shared Dirs As DataTable
    Public Shared Arts As DataTable
    Protected Function GetControlThatCausedPostBack(ByVal page As Page) As Control
        Dim control As Control = Nothing

        Dim ctrlname As String = page.Request.Params.Get("__EVENTTARGET")
        If ctrlname IsNot Nothing AndAlso ctrlname <> String.Empty Then
            control = page.FindControl(ctrlname)
        Else
            For Each ctl As String In page.Request.Form
                Dim c As Control = page.FindControl(ctl)
                If TypeOf c Is System.Web.UI.WebControls.Button OrElse TypeOf c Is System.Web.UI.WebControls.ImageButton Then
                    control = c
                    Exit For
                End If
            Next ctl
        End If
        Return control

    End Function
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        'If IsPostBack Then
        '    Dim wcICausedPostBack As WebControl = CType(GetControlThatCausedPostBack(TryCast(sender, Page)), WebControl)
        '    Dim indx As Integer = wcICausedPostBack.TabIndex
        '    Dim ctrl =
        '     From control In wcICausedPostBack.Parent.Controls.OfType(Of WebControl)()
        '     Where control.TabIndex > indx
        '     Select control
        '    ctrl.DefaultIfEmpty(wcICausedPostBack).First().Focus()
        'End If


        If Not IsPostBack Then
            TimeOver()
            btnDelete_Click()
            Session("Page") = "eOrders.aspx"
            Dim Resp As DataTable
            Dim Acceso As String
            Dim WhsCode As String
            Dim Edo As String
            Try
                Resp = Funciones.ValidaUsr(Session("UserName").ToString)
                If Resp.Rows.Count >= 1 Then
                    Acceso = Resp.Rows.Item(0).Item(0).ToString
                    WhsCode = Resp.Rows.Item(0).Item(1).ToString
                    Edo = Resp.Rows.Item(0).Item(2).ToString
                    If Acceso = "Y" Then
                        'Redireccionamos al sitio
                        If Edo = "1" Then
                            'Bienvenido, pase usted!!!
                            TxtDocDate.Text = Now.ToString("dd-MM-yyyy")
                            TxtFechaHora.Text = Now.AddMinutes(ToolReadSettings("Caducidad", "15")).ToString("MMM dd, yyyy HH:mm:ss", System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))
                            TxtWhsCode.Text = WhsCode.ToString
                            TxtWhsName.Text = Funciones.ObtieneWhsName(WhsCode.ToString)
                            TxtAlmacen.Text = WhsCode.ToString & " - " & TxtWhsName.Text.ToString
                            TxtSlpCode.Text = Session("SlpCode").ToString
                            TxtSlpName.Text = Session("SlpName").ToString
                            TxtDistribuidor.Text = TxtSlpCode.Text.ToString & " - " & TxtSlpName.Text.ToString
                            LstTP.Items.Add("Seleccione un cliente...")
                            LstMV.Items.Add("Seleccione un cliente...")
                            LstDirs.Items.Add("Seleccione un cliente...")
                            TxtDir.Text = "Seleccione un cliente..."
                            TxtDir.ReadOnly = True
                            Funciones.ObtieneLista("CFDI01", LstMP)
                            Funciones.ObtieneLista("CFDI02", LstFP)
                            Funciones.ObtieneLista("CFDI03", LstUso)
                            Funciones.ObtieneLista("CFDI04", LstTipo)
                            Funciones.ObtieneLista("Promociones", LstPromos)
                            Funciones.ObtieneLista("TipoStock", LstStock)
                            Try
                                LstMP.SelectedIndex = 0
                                LstFP.SelectedIndex = 0
                                LstUso.SelectedIndex = 0
                                LstTipo.SelectedIndex = 0
                                LstPromos.SelectedIndex = 0
                                LstStock.SelectedIndex = 0
                            Catch ex As Exception

                            End Try
                            LoadItemsGrid()
                            LoadDocument()
                        Else
                            Response.Redirect("Login.aspx")
                        End If
                    Else
                        Response.Redirect("Login.aspx")
                    End If
                Else
                    Response.Redirect("Login.aspx")
                End If
            Catch ex As Exception
                Response.Redirect("Login.aspx")
            End Try
        End If
        ClearMemory()
        'Reload Items
        If IsPostBack Then
            'LoadItemsGrid()
            'LoadDocument()
        End If
    End Sub
    Protected Sub LoadItemsGrid()
        Dim constr As String = ConfigurationManager.ConnectionStrings("SQLConnection").ConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand("EXECUTE [DesarrollosGVI].[dbo].[sp_eO_RDR1_v2] '" & TxtCardCode.Text.ToString & "','S'," & Session("SlpCode").ToString & ",NULL,NULL,NULL,NULL,NULL,'O','','" & TxtTP.Text.ToString & "'")
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using dt As New DataTable()
                        dt.TableName = "CRD1"
                        sda.Fill(dt)
                        Arts = New DataTable
                        Arts = dt.Copy()
                        dt.Clear()
                        dt.Dispose()
                    End Using
                    sda.Dispose()
                End Using
                cmd.Dispose()
            End Using
            con.Close()
            con.Dispose()
            ClearMemory()
        End Using
    End Sub
    Protected Sub TxtItemCode_TextChanged(sender As Object, e As EventArgs) Handles TxtItemCode.TextChanged
        Dim Item As String
        Try
            Item = TxtItemCode.Text.ToString.Substring(0, TxtItemCode.Text.ToString.IndexOf(" | "))
        Catch ex As Exception
            Item = ""
        End Try
        TxtItemCode.Text = Item 'Solo el codigo
        Dim VD As String = ""
        Dim Itms As DataTable
        Itms = New DataTable
        Itms = Funciones.ObtieneItems(TxtCardCode.Text.ToString, Item, TxtWhsCode.Text.ToString, "", TxtSlpCode.Text.ToString, LstTP.SelectedValue.ToString)
        Try
            If Itms.Rows.Count >= 1 And TxtCardCode.Text <> "" Then
                lblErrId.Text = ""
                LblNombre.Text = Itms.Rows.Item(0).Item(0).ToString
                LblPrecio.Text = Itms.Rows.Item(0).Item(1).ToString
                lblMoneda.Text = Itms.Rows.Item(0).Item(2).ToString
                lblMon.Text = lblMoneda.Text
                TxtCantidad.Text = "0"
                Disponible.Text = Format(CSng(Itms.Rows.Item(0).Item(3).ToString), "#,##0")
                TxtDisp.Text = Itms.Rows.Item(0).Item(3).ToString
                LblImpuesto.Text = Itms.Rows.Item(0).Item(4).ToString
                TxtDescuento.Text = "0"
                LblSubTotal.Text = "0.00"
                LblPrecio.Text = Format(CSng(LblPrecio.Text), "#,##0.00")
                VD = Itms.Rows.Item(0).Item(5).ToString
                TxtTP.Text = Itms.Rows.Item(0).Item(6).ToString

                If VD <> "" Then
                    lblErrId.Text = "Error"
                    ErrorNum.Text = "Error"
                    ErrorMsg.Text = VD
                    LblNombre.Text = "NO VALIDO"
                    LblPrecio.Text = "0.00"
                    lblMoneda.Text = ""
                    lblMon.Text = ""
                    TxtCantidad.Text = "0"
                    Disponible.Text = "0"
                    TxtDisp.Text = "0"
                    LblImpuesto.Text = ""
                    TxtDescuento.Text = "0"
                    LblSubTotal.Text = "0.00"
                    LblPrecio.Text = "0.00"
                    TxtTP.Text = "-1"
                End If
            Else
                LblNombre.Text = "Seleccione un cliente y su tercera persona..."
            End If
        Catch ex As Exception
            ' = ex.Message
        End Try
        ClearMemory()
        LoadItemsGrid()
        LstPromos.Focus()
    End Sub

    Protected Sub TxtCantidad_TextChanged(sender As Object, e As EventArgs) Handles TxtCantidad.TextChanged
        CalculaSubTotal()
        'Calcula el monto del stock disponible
        If CInt(TxtDisp.Text) >= CInt(TxtCantidad.Text) Then
            Disponible.Text = Format(CSng((CInt(TxtDisp.Text) - CInt(TxtCantidad.Text)).ToString), "#,##0")
        Else
            Disponible.Text = "0"
        End If
    End Sub

    Protected Sub TxtDescuento_TextChanged(sender As Object, e As EventArgs) Handles TxtDescuento.TextChanged
        CalculaSubTotal()
    End Sub

    Protected Sub CalculaSubTotal()
        Dim disc As Double
        If IsNumeric(TxtCantidad.Text) Then
            LblSubTotal.Text = (TxtCantidad.Text * LblPrecio.Text).ToString
            If LblImpuesto.Text.ToString = "IVAV16" Then
                LblIVA.Text = (LblSubTotal.Text * 0.16).ToString
            Else
                LblIVA.Text = (0).ToString
            End If
        Else
            LblIVA.Text = "0.00"
            LblSubTotal.Text = "0.00"
        End If
        If IsNumeric(TxtDescuento.Text) Then
            disc = TxtDescuento.Text
            If disc > 100 Then
                TxtDescuento.Text = "100"
            End If
            disc = TxtDescuento.Text / 100
            LblIVA.Text = (LblIVA.Text - (LblIVA.Text * disc)).ToString
            disc = LblSubTotal.Text * disc
            LblSubTotal.Text = (LblSubTotal.Text - disc).ToString
        Else
            TxtDescuento.Text = "0"
        End If
        TxtDescuento.Text = Format(CSng(TxtDescuento.Text), "##0")
        LblIVA.Text = Format(CSng(LblIVA.Text), "#,##0.00")
        LblSubTotal.Text = Format(CSng(LblSubTotal.Text), "#,##0.00")
        ClearMemory()
        LoadItemsGrid()
        LstPromos.Focus()
    End Sub

    Protected Sub TxtCardCode_TextChanged(sender As Object, e As EventArgs) Handles TxtCardCode.TextChanged
        Dim cc As String
        Try
            cc = TxtCardCode.Text.ToString.Substring(0, TxtCardCode.Text.ToString.IndexOf(" | "))
        Catch ex As Exception
            cc = ""
        End Try
        TxtCardCode.Text = cc 'Solo el codigo
        TxtCardName.Text = Funciones.ObtieneCardName(cc, False)
        RefeBanco.Text = Funciones.ObtieneCardName(cc, True)
        Funciones.ObtieneLista("3ERAS" & cc, LstTP)
        Funciones.ObtieneLista("MEGAS" & cc, LstMV)
        Dirs = New DataTable
        Dirs = Funciones.ObtieneDirs(cc)
        Try
            If Dirs.Rows.Count >= 1 Then
                LstDirs.Items.Clear()
                LstDirs.DataSource = Dirs
                LstDirs.DataTextField = "Address"
                LstDirs.DataValueField = "Address"
                LstDirs.DataBind()
            Else
                'SQL Caido
            End If
        Catch ex As Exception
            ' = ex.Message
        End Try
        Try
            If TxtCardName.Text = "NO EXISTE" Then
                LstTP.Items.Clear()
                LstTP.Items.Add("Seleccione un cliente...")
                LstMV.Items.Clear()
                LstMV.Items.Add("Seleccione un cliente...")
                LstDirs.Items.Clear()
                LstDirs.Items.Add("Seleccione un cliente...")
                LblDE3.Text = ""
                TxtDir.Text = "Seleccione un cliente..."
                TxtDir.ReadOnly = True
                RefeBanco.Text = "Ninguna"
            Else
                LstTP.SelectedIndex = 0
                LstMV.SelectedIndex = 0
                LstDirs.SelectedIndex = 0
                CargaDireccion()
            End If
        Catch ex As Exception

        End Try
        ClearMemory()
        LoadItemsGrid()
        TxtDir.Focus()
    End Sub

    Protected Sub LstDirs_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LstDirs.SelectedIndexChanged
        CargaDireccion()
    End Sub

    Protected Sub CargaDireccion()
        Dim adr As String
        adr = LstDirs.SelectedValue
        For r = 0 To Dirs.Rows.Count - 1
            If r = LstDirs.SelectedIndex Then
                TxtDir.Text = ""
                For c = 0 To Dirs.Rows.Item(r).Table.Columns.Count - 1
                    If c = 0 Then
                        LblDE3.Text = Dirs.Rows.Item(r).Item(c).ToString
                    Else
                        TxtDir.Text += Dirs.Rows.Item(r).Item(c).ToString & vbCr
                    End If
                Next
            End If
        Next
        If adr = "Personalizada" Then
            TxtDir.ReadOnly = False
        Else
            TxtDir.ReadOnly = True
        End If
        ClearMemory()
        TxtDir.Focus()
    End Sub

    Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If TxtItemCode.Text <> "" And LblNombre.Text.ToString <> "NO VALIDO" And LblNombre.Text.ToString <> "NO EXISTE" And LblNombre.Text.ToString <> "Seleccione un cliente..." And IsNumeric(TxtCantidad.Text) And IsNumeric(TxtDescuento.Text) Then
            If TxtCantidad.Text >= 1 Then
                SaveDocument("O", "")
                CalculaSubTotal()
                Dim constr As String = ConfigurationManager.ConnectionStrings("SQLConnection").ConnectionString
                Using con As New SqlConnection(constr)
                    Using cmd As New SqlCommand("EXECUTE [DesarrollosGVI].[dbo].[sp_eO_RDR1_v2] '" & TxtCardCode.Text.ToString & "','A'," & Session("SlpCode").ToString & ",0,'" & TxtItemCode.Text.ToString & "'," & TxtCantidad.Text.ToString & "," & TxtDescuento.Text.ToString & "," & Format(CSng(LblSubTotal.Text), "###0.00") & ",'O','','" & TxtTP.Text.ToString & "'")
                        cmd.Connection = con
                        cmd.Connection.Open()
                        cmd.CommandType = CommandType.Text
                        cmd.ExecuteNonQuery()
                        cmd.Dispose()
                    End Using
                    con.Close()
                    con.Dispose()
                    ClearMemory()
                End Using
                LoadItemsGrid()
                LimpiaForma()
                LstPromos.Focus()
            End If
        End If
    End Sub
    Protected Function validarCampos() As String
        Dim valores As String = ""
        Dim camposValidos As Boolean = True

        If TextNombre.Text = "" And TextAPP.Text = "" And TextRFC.Text = "" And TextCalle.Text = "" And TextNoEx.Text = "" And TextEstado.Text = "" And TextCP.Text = "" Then
            Return "1"
        Else
            If TextNombre.Text = "" Or TextAPP.Text = "" Or TextRFC.Text = "" Or TextCalle.Text = "" Or TextNoEx.Text = "" Or TextEstado.Text = "" Or TextCP.Text = "" Then
                camposValidos = False
            End If

            If camposValidos Then
                valores = TipoTP.SelectedValue & "|" & TextNombre.Text & "|" & TextAPP.Text & "|" & TextAPM.Text & "|" & TextRFC.Text & "|" & TextCalle.Text & "|" & TextNoEx.Text & "|" & TextNoInt.Text & "|" & TextColonia.Text & "|" & TextMunicipio.Text & "|" & TextCuidad.Text & "|" & TextEstado.Text & "|" & TextPais.SelectedValue & "|" & TextCP.Text & "|" & DropDownTipoC.SelectedValue & "|" & DropDownRegimen.SelectedValue
                Return valores
            Else
                lblErrId.Text = "Error"
                ErrorNum.Text = "-50"
                ErrorMsg.Text = "Faltan campos obligatorios para tercera persona, favor de validar o limpiar"
                Return "-1"
            End If
        End If
    End Function

    Protected Sub SaveDocument(ByVal edo As String, ByVal numatcard As String)
        Try
            If TxtCardCode.Text <> "" Then
                Dim constr As String = ConfigurationManager.ConnectionStrings("SQLConnection").ConnectionString
                Using con As New SqlConnection(constr)
                    Using cmd As New SqlCommand("EXECUTE [DesarrollosGVI].[dbo].[sp_eO_ORDR] 'A','" & Session("UserName").ToString & "','" & Session("SlpCode").ToString & "','" & Session("SlpName").ToString & "','" & Session("WhsCode").ToString & "','" & TxtWhsName.Text.ToString & "','" & TxtCardCode.Text.ToString & "','" & TxtCardName.Text.ToString & "','" & LstTP.SelectedValue & "','" & LstPromos.SelectedValue & "','" & LstMV.SelectedValue & "','" & LstDirs.SelectedValue & "','" & TxtDir.Text.ToString.Replace(vbCrLf, ", ").Replace(vbCr, ", ") & "','" & LstMP.SelectedValue & "','" & LstFP.SelectedValue & "','" & LstUso.SelectedValue & "','" & LstTipo.SelectedValue & "','" & LstStock.SelectedValue & "','" & edo & "','" & numatcard & "'")
                        cmd.Connection = con
                        cmd.Connection.Open()
                        cmd.CommandType = CommandType.Text
                        cmd.ExecuteNonQuery()
                        cmd.Dispose()
                    End Using
                    con.Close()
                    con.Dispose()
                    ClearMemory()
                End Using
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub LoadDocument()
        Try
            Dim dtResp As DataTable
            Dim constr As String = ConfigurationManager.ConnectionStrings("SQLConnection").ConnectionString
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("EXECUTE [DesarrollosGVI].[dbo].[sp_eO_ORDR] 'S',NULL,'" & Session("SlpCode").ToString & "',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'O',''")
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            dt.TableName = "ORDR"
                            sda.Fill(dt)
                            dtResp = dt.Copy()
                            dt.Clear()
                            dt.Dispose()
                        End Using
                        sda.Dispose()
                    End Using
                    cmd.Dispose()
                End Using
                con.Close()
                con.Dispose()
                Try
                    If dtResp.Rows.Count >= 1 Then
                        'Inicializamos la forma
                        TxtCardCode.Text = dtResp.Rows.Item(0).Item(0).ToString
                        Dim cc As String = TxtCardCode.Text
                        TxtCardName.Text = Funciones.ObtieneCardName(cc, False)
                        RefeBanco.Text = Funciones.ObtieneCardName(cc, True)
                        Funciones.ObtieneLista("3ERAS" & cc, LstTP)
                        Funciones.ObtieneLista("MEGAS" & cc, LstMV)
                        Dirs = New DataTable
                        Dirs = Funciones.ObtieneDirs(cc)
                        Try
                            If Dirs.Rows.Count >= 1 Then
                                LstDirs.Items.Clear()
                                LstDirs.DataSource = Dirs
                                LstDirs.DataTextField = "Address"
                                LstDirs.DataValueField = "Address"
                                LstDirs.DataBind()
                            Else
                                'SQL Caido
                            End If
                        Catch ex As Exception
                            ' = ex.Message
                        End Try
                        LstTP.SelectedValue = dtResp.Rows.Item(0).Item(1).ToString
                        LstMV.SelectedValue = dtResp.Rows.Item(0).Item(2).ToString
                        LstDirs.SelectedValue = dtResp.Rows.Item(0).Item(3).ToString
                        TxtDir.Text = dtResp.Rows.Item(0).Item(4).ToString
                        If LstDirs.SelectedValue = "Personalizada" Then
                            TxtDir.ReadOnly = False
                        Else
                            TxtDir.ReadOnly = True
                        End If
                        LstMP.SelectedValue = dtResp.Rows.Item(0).Item(5).ToString
                        LstFP.SelectedValue = dtResp.Rows.Item(0).Item(6).ToString
                        LstUso.SelectedValue = dtResp.Rows.Item(0).Item(7).ToString
                        LstTipo.SelectedValue = dtResp.Rows.Item(0).Item(8).ToString
                        LstPromos.SelectedValue = dtResp.Rows.Item(0).Item(9).ToString
                        LstStock.SelectedValue = dtResp.Rows.Item(0).Item(10).ToString
                        Dim fechahora As DateTime
                        Try
                            fechahora = dtResp.Rows.Item(0).Item(12)
                        Catch ex As Exception
                            fechahora = Now
                        End Try
                        TxtFechaHora.Text = fechahora.AddMinutes(ToolReadSettings("Caducidad", "15")).ToString("MMM dd, yyyy HH:mm:ss", System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))
                    End If
                Catch ex As Exception
                    'getWhsName = ex.Message
                End Try
                ClearMemory()
            End Using
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub LimpiaForma()
        TxtItemCode.Text = ""
        LblNombre.Text = ""
        LblPrecio.Text = "0.00"
        lblMoneda.Text = "MXP"
        lblMon.Text = "MXP"
        TxtCantidad.Text = "0"
        Disponible.Text = "0"
        TxtDisp.Text = "0"
        TxtDescuento.Text = "0"
        LblImpuesto.Text = "IVAV16"
        LblIVA.Text = "0.00"
        LblSubTotal.Text = "0.00"
        TxtTP.Text = "0"
    End Sub
    Protected Sub btnDelete_Click()
        Dim nr As Integer
        Dim c As String = ""
        Try
            nr = Request.QueryString("l").ToString
            c = Request.QueryString("c").ToString
            Dim constr As String = ConfigurationManager.ConnectionStrings("SQLConnection").ConnectionString
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("EXECUTE [DesarrollosGVI].[dbo].[sp_eO_RDR1_v2] '" & c & "','D'," & Session("SlpCode").ToString & "," & nr.ToString & ",NULL,NULL,NULL,NULL,'O','','" & TxtTP.Text.ToString & "'")
                    cmd.Connection = con
                    cmd.Connection.Open()
                    cmd.CommandType = CommandType.Text
                    cmd.ExecuteNonQuery()
                    cmd.Dispose()
                End Using
                con.Close()
                con.Dispose()
                ClearMemory()
            End Using
            Response.Redirect("eOrders.aspx#deleted")
            'LstPromos.Focus()
        Catch ex As Exception
            nr = -1
        End Try
    End Sub

    Protected Sub TimeOver()
        Dim nr As String
        Try
            nr = Request.QueryString("timeover").ToString
            'If nr = "15" Then
            Dim constr As String = ConfigurationManager.ConnectionStrings("SQLConnection").ConnectionString
                Using con As New SqlConnection(constr)
                    Using cmd As New SqlCommand("EXECUTE [DesarrollosGVI].[dbo].[sp_eO_ORDR] 'T',NULL,'" & Session("SlpCode").ToString & "',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'O',''")
                        cmd.Connection = con
                        cmd.Connection.Open()
                        cmd.CommandType = CommandType.Text
                        cmd.ExecuteNonQuery()
                        cmd.Dispose()
                    End Using
                    con.Close()
                    con.Dispose()
                    ClearMemory()
                End Using
            'End If
            Response.Redirect("eOrders.aspx")
            'LstPromos.Focus()
        Catch ex As Exception
            nr = ex.Message
        End Try
    End Sub
    Protected Sub btnGenerarPedido_Click(sender As Object, e As EventArgs) Handles btnGenerarPedido.Click
        LoadItemsGrid()
        Dim ln As Integer
        ln = Arts.Rows.Count
        If ln >= 1 Then
            Try
                Dim DS As DIS.DIServer
                DS = New DIS.DIServer
                DS.Url = Funciones.ToolReadSettings("sp_eO_Url", "http://10.0.0.19/SAP/DIServer.asmx")
                DS.Timeout = Funciones.ToolReadSettings("sp_eO_TimeOut", "600000") 'Milisegundos 10 minutos
                'Datos de acceso
                'PruebaGVI: q0ffpL9I9FhEZFLCmNJphMDWTyFfNhsiNJXcYs6HTH2U58fDrDz5+rR+ZK+V75qdG6/NHERYjiGB+lxRqJkoogGr71DH1cJhG6/NHERYjiGrR9+kv0j0WIaxxlRJKBFoGyRkr07uHAINZqz7TlBH+1bSSNnX4dGQSeoLdqKXZYPh9xwBvkudlNeCLRBxMBWrZ7g7FbQmtUCxby0s2fSCgMFfKMh3CxNecq6yWjysGqOONX2BNNkRcuTVt8KbfV5GXRnLpQk78KGVqvBbN6RFZ85HNt2JJeOSmbh0IhOYomq0JcT0y/C+LM5HNt2JJeOSpPv14Tcgr7/ZuXj4fW2YcbN3RzlmYQd9Xr+UshKf+00jiC7MzSjnbUguAjsuWQmeRaaLXlG/VRsuSQCelmVH1+nELn6WBmxOgqtIfwTTr1rAdfGby3esoW+wqrvtCuWJ9M9x/TOyUjZjrB584Ynlb0RkUsKY0mmEwvoiBEGYz+s=
                'GVI: q0ffpL9I9FhEZFLCmNJphMDWTyFfNhsiNJXcYs6HTH2U58fDrDz5+rR+ZK+V75qdG6/NHERYjiEXe0oT3xAgq72O1eimV9VpSeoLdqKXZYPNEo+U5t9gcVP0QBRwD7pZFGsEbclGZOX+VyXKONKHvIxZH5Y9ezX/o0jeDJyYec5n6Eiqaekwvf6gU9mGfA5KCl5XDofr4wuh+lOz8Jz3S10Zy6UJO/ChObwcTfsPbF0prv3PY+RX4Aqs/A5Ma5osczSsMEtRoC8mU9wap9I6Mv/MNNqU2Wyx89wDqnSKMRajDyfbSo5AKrFvLSzZ9IKADDy1HSQvuebNAkTUMLDNhp7n/N+BFRhdql2Oh1vXfqXRs/l4VW8kpXDEzKzQzNYWS0OEEuHxVtygRgMuaN2lSvQmyTZI9oxXLfnjoxdle9agIM2jTRdfhuyxyAMJoGwMp2BkpI/n0FjAdfGby3esoUJWIv4O1XHV


                'Pruebas
                'Dim Token As String = Funciones.ToolReadSettings("sp_eO_Token", "q0ffpL9I9FhEZFLCmNJphMDWTyFfNhsiNJXcYs6HTH2U58fDrDz5+rR+ZK+V75qdG6/NHERYjiGB+lxRqJkoogGr71DH1cJhG6/NHERYjiGrR9+kv0j0WIaxxlRJKBFoGyRkr07uHAINZqz7TlBH+1bSSNnX4dGQSeoLdqKXZYPh9xwBvkudlNeCLRBxMBWrZ7g7FbQmtUCxby0s2fSCgMFfKMh3CxNecq6yWjysGqOONX2BNNkRcuTVt8KbfV5GXRnLpQk78KGVqvBbN6RFZ85HNt2JJeOSmbh0IhOYomq0JcT0y/C+LM5HNt2JJeOSpPv14Tcgr7/ZuXj4fW2YcbN3RzlmYQd9Xr+UshKf+00jiC7MzSjnbUguAjsuWQmeRaaLXlG/VRsuSQCelmVH1+nELn6WBmxOgqtIfwTTr1rAdfGby3esoW+wqrvtCuWJ9M9x/TOyUjZjrB584Ynlb0RkUsKY0mmEwvoiBEGYz+s=")

                'GVI
                Dim Token As String = Funciones.ToolReadSettings("sp_eO_Token", "q0ffpL9I9FhEZFLCmNJphMDWTyFfNhsiNJXcYs6HTH2U58fDrDz5+rR+ZK+V75qdG6/NHERYjiEXe0oT3xAgq72O1eimV9VpSeoLdqKXZYPNEo+U5t9gcVP0QBRwD7pZFGsEbclGZOX+VyXKONKHvIxZH5Y9ezX/o0jeDJyYec5n6Eiqaekwvf6gU9mGfA5KCl5XDofr4wuh+lOz8Jz3S10Zy6UJO/ChObwcTfsPbF0prv3PY+RX4Aqs/A5Ma5osczSsMEtRoC8mU9wap9I6Mv/MNNqU2Wyx89wDqnSKMRajDyfbSo5AKrFvLSzZ9IKADDy1HSQvuebNAkTUMLDNhp7n/N+BFRhdql2Oh1vXfqXRs/l4VW8kpXDEzKzQzNYWS0OEEuHxVtygRgMuaN2lSvQmyTZI9oxXLfnjoxdle9agIM2jTRdfhuyxyAMJoGwMp2BkpI/n0FjAdfGby3esoUJWIv4O1XHV")

                Dim SessionId As String = DS.LoginSSL(Token)
                Dim Moneda As String = "MXP"
                Dim DocE As String = ""
                Dim Normal As String = ""
                Dim VentaDirecta As String = ""
                Dim BackOrder As String = ""
                Dim r As Integer
                Dim AutNl As Boolean
                Dim AutBO As Boolean
                Dim LineasBO As String = "<Document_Lines>"
                Dim LineasNL As String = "<Document_Lines>"
                Dim array() As detalles
                array = New detalles(ln) {}
                Dim RowsHTML As String = ""
                Dim Itotal As Double
                Dim Stotal As Double
                Dim Total As Double
                For l = 0 To ln - 1
                    Moneda = Arts.Rows.Item(l).Item(4).ToString
                    Normal = Arts.Rows.Item(l).Item(12).ToString
                    BackOrder = Arts.Rows.Item(l).Item(13).ToString
                    VentaDirecta = Arts.Rows.Item(l).Item(14).ToString
                    Try
                        If CInt(Normal) > 0 Then
                            LineasNL = LineasNL & "<row><LineNum>" & r & "</LineNum><ItemCode>" & Arts.Rows.Item(l).Item(0).ToString & "</ItemCode><U_observaciones>COMPROMETIDO</U_observaciones><Quantity>" & Normal & "</Quantity><UnitPrice>" & Arts.Rows.Item(l).Item(3).ToString & "</UnitPrice><Currency>" & Moneda & "</Currency><DiscountPercent>" & Arts.Rows.Item(l).Item(5).ToString & "</DiscountPercent><TaxCode>" & Arts.Rows.Item(l).Item(6).ToString & "</TaxCode><WarehouseCode>" & Arts.Rows.Item(l).Item(11).ToString & "</WarehouseCode></row>"
                            RowsHTML += "<tr><td><font color=""#6e6e6e"">" & Arts.Rows.Item(l).Item(0).ToString & "</font></td><td><font color=""#6e6e6e"">" & Arts.Rows.Item(l).Item(1).ToString & "</font></td><td><font color=""#6e6e6e"">" & Normal & "</font></td></tr>"
                            Itotal += Arts.Rows.Item(l).Item(7)
                            Stotal += Arts.Rows.Item(l).Item(8)
                            Total = Stotal + Itotal
                            r = r + 1
                            AutNl = True
                        End If
                    Catch ex As Exception

                    End Try

                    Try
                        If CInt(BackOrder) > 0 Then
                            LineasBO = LineasBO & "<row><LineNum>" & r & "</LineNum><ItemCode>" & Arts.Rows.Item(l).Item(0).ToString & "</ItemCode><U_observaciones>BACKORDER</U_observaciones><Quantity>" & BackOrder & "</Quantity><UnitPrice>" & Arts.Rows.Item(l).Item(3).ToString & "</UnitPrice><Currency>" & Moneda & "</Currency><DiscountPercent>" & Arts.Rows.Item(l).Item(5).ToString & "</DiscountPercent><TaxCode>" & Arts.Rows.Item(l).Item(6).ToString & "</TaxCode><WarehouseCode>" & Arts.Rows.Item(l).Item(11).ToString & "</WarehouseCode></row>"
                            RowsHTML += "<tr><td><font color=""#6e6e6e"">" & Arts.Rows.Item(l).Item(0).ToString & "</font></td><td><font color=""#6e6e6e"">" & Arts.Rows.Item(l).Item(1).ToString & "</font></td><td><font color=""#6e6e6e"">" & BackOrder & "</font></td></tr>"
                            Itotal += Arts.Rows.Item(l).Item(7)
                            Stotal += Arts.Rows.Item(l).Item(8)
                            Total = Stotal + Itotal
                            r = r + 1
                            AutBO = True
                        End If
                    Catch ex As Exception

                    End Try
                    DocE = Arts.Rows.Item(l).Item(10).ToString
                Next
                LineasNL = LineasNL & "</Document_Lines>"
                LineasBO = LineasBO & "</Document_Lines>"
                Dim Entrega As String
                Dim EntregaBO As String
                If LstDirs.SelectedValue = "Personalizada" Then
                    Entrega = "<ShipToCode></ShipToCode><Address2>" & TxtDir.Text.ToString & "</Address2><Comments>" & TxtDir.Text.ToString & " | " & TextComment.Text.ToString & "</Comments>"
                    EntregaBO = "<ShipToCode></ShipToCode><Address2>" & TxtDir.Text.ToString & "</Address2>" & "<Comments>" & TextComment.Text.ToString & " | " & TextComment.Text.ToString & "</Comments>"
                Else
                    Entrega = "<ShipToCode>" & LstDirs.SelectedValue & "</ShipToCode>" & "<Comments>" & LstDirs.SelectedValue & " | " & TextComment.Text.ToString & "</Comments>"
                    EntregaBO = "<ShipToCode>" & LstDirs.SelectedValue & "</ShipToCode>" & "<Comments>" & LstDirs.SelectedValue & " | " & TextComment.Text.ToString & "</Comments>"
                End If

                'Dim Encabezado As String = "<Documents><row><DocType>dDocument_Items</DocType><SalesPersonCode>" & TxtSlpCode.Text.ToString & "</SalesPersonCode>" & Entrega & "<DocDate>" & Now.ToString("yyyyMMdd") & "</DocDate><DocDueDate>" & Now.ToString("yyyyMMdd") & "</DocDueDate><TaxDate>" & Now.ToString("yyyyMMdd") & "</TaxDate><CardCode>" & TxtCardCode.Text.ToString & "</CardCode><NumAtCard>E" & TxtSlpCode.Text.ToString & "-" & DocE & "</NumAtCard><DocCurrency>" & Moneda & "</DocCurrency><U_MetodoDePago>" & LstMP.SelectedValue & "</U_MetodoDePago><U_FormaDePago>" & LstFP.SelectedValue & "</U_FormaDePago><U_UsoCFDI>" & LstUso.SelectedValue & "</U_UsoCFDI><U_TipoDeComprobante>" & LstTipo.SelectedValue & "</U_TipoDeComprobante><U_TPersona>" & LstTP.SelectedValue & "</U_TPersona><U_Promocion>" & LstPromos.SelectedValue & "</U_Promocion><U_megavol>" & LstMV.SelectedValue & "</U_megavol><U_Stock>" & LstStock.SelectedValue & "</U_Stock><U_InvoiceId>" & VentaDirecta & "</U_InvoiceId></row></Documents>"




                'Dim Encabezado As String = "<Documents><row><DocType>dDocument_Items</DocType><SalesPersonCode>" & TxtSlpCode.Text.ToString & "</SalesPersonCode>" & Entrega & "<DocDate>" & Now.ToString("ddMMyyyy") & "</DocDate><DocDueDate>" & Now.ToString("ddMMyyyy") & "</DocDueDate><TaxDate>" & Now.ToString("ddMMyyyy") & "</TaxDate><CardCode>" & TxtCardCode.Text.ToString & "</CardCode><NumAtCard>E" & TxtSlpCode.Text.ToString & "-" & DocE & "</NumAtCard><DocCurrency>" & Moneda & "</DocCurrency><U_MetodoDePago>" & LstMP.SelectedValue & "</U_MetodoDePago><U_FormaDePago>" & LstFP.SelectedValue & "</U_FormaDePago><U_UsoCFDI>" & LstUso.SelectedValue & "</U_UsoCFDI><U_TipoDeComprobante>" & LstTipo.SelectedValue & "</U_TipoDeComprobante><U_TPersona>" & LstTP.SelectedValue & "</U_TPersona><U_Promocion>" & LstPromos.SelectedValue & "</U_Promocion><U_megavol>" & LstMV.SelectedValue & "</U_megavol><U_Stock>" & LstStock.SelectedValue & "</U_Stock><U_InvoiceId>" & VentaDirecta & "</U_InvoiceId></row></Documents>"

                Dim Fecha_jbqb As String
                'Dim Fecha_jbqb_V As String

                Fecha_jbqb = Now.ToString("yyyyMMdd")

                'Fecha_jbqb = "20190930"

                'Fecha_jbqb_V = Now.ToString + 1



                'Dim Encabezado As String = "<Documents><row><DocType>dDocument_Items</DocType><SalesPersonCode>" & TxtSlpCode.Text.ToString & "</SalesPersonCode>" & Entrega & "<DocDate>" & Now.ToString("ddMMyyyy") & "</DocDate><DocDueDate>" & Now.ToString("ddMMyyyy") & "</DocDueDate><TaxDate>" & Now.ToString("ddMMyyyy") & "</TaxDate><CardCode>" & TxtCardCode.Text.ToString & "</CardCode><NumAtCard>E" & TxtSlpCode.Text.ToString & "-" & DocE & "</NumAtCard><DocCurrency>" & Moneda & "</DocCurrency><U_MetodoDePago>" & LstMP.SelectedValue & "</U_MetodoDePago><U_FormaDePago>" & LstFP.SelectedValue & "</U_FormaDePago><U_UsoCFDI>" & LstUso.SelectedValue & "</U_UsoCFDI><U_TipoDeComprobante>" & LstTipo.SelectedValue & "</U_TipoDeComprobante><U_TPersona>" & LstTP.SelectedValue & "</U_TPersona><U_Promocion>" & LstPromos.SelectedValue & "</U_Promocion><U_megavol>" & LstMV.SelectedValue & "</U_megavol><U_Stock>" & LstStock.SelectedValue & "</U_Stock><U_InvoiceId>" & VentaDirecta & "</U_InvoiceId></row></Documents>"


                'Dim Encabezado As String = "<Documents><row><DocType>dDocument_Items</DocType><SalesPersonCode>" & TxtSlpCode.Text.ToString & "</SalesPersonCode>" & Entrega & "<DocDate>" & Fecha_jbqb & "</DocDate><DocDueDate>" & Fecha_jbqb & "</DocDueDate><TaxDate>" & Fecha_jbqb & "</TaxDate><CardCode>" & TxtCardCode.Text.ToString & "</CardCode><NumAtCard>E" & TxtSlpCode.Text.ToString & "-" & DocE & "</NumAtCard><DocCurrency>" & Moneda & "</DocCurrency><U_MetodoDePago>" & LstMP.SelectedValue & "</U_MetodoDePago><U_FormaDePago>" & LstFP.SelectedValue & "</U_FormaDePago><U_UsoCFDI>" & LstUso.SelectedValue & "</U_UsoCFDI><U_TipoDeComprobante>" & LstTipo.SelectedValue & "</U_TipoDeComprobante><U_TPersona>" & LstTP.SelectedValue & "</U_TPersona><U_Promocion>" & LstPromos.SelectedValue & "</U_Promocion><U_megavol>" & LstMV.SelectedValue & "</U_megavol><U_Stock>" & LstStock.SelectedValue & "</U_Stock><U_InvoiceId>" & VentaDirecta & "</U_InvoiceId></row></Documents>"
                Dim Encabezado As String
                If validarCampos() = "1" Then
                    Encabezado = "<Documents><row><DocType>dDocument_Items</DocType><SalesPersonCode>" & TxtSlpCode.Text.ToString & "</SalesPersonCode>" & Entrega & "<DocDate>" & Fecha_jbqb & "</DocDate><DocDueDate>" & Fecha_jbqb & "</DocDueDate><TaxDate>" & Fecha_jbqb & "</TaxDate><CardCode>" & TxtCardCode.Text.ToString & "</CardCode><NumAtCard>E" & TxtSlpCode.Text.ToString & "-" & DocE & "</NumAtCard><DocCurrency>" & Moneda & "</DocCurrency><U_MetodoDePago>" & LstMP.SelectedValue & "</U_MetodoDePago><U_FormaDePago>" & LstFP.SelectedValue & "</U_FormaDePago><U_UsoCFDI>" & LstUso.SelectedValue & "</U_UsoCFDI><U_TipoDeComprobante>" & LstTipo.SelectedValue & "</U_TipoDeComprobante><U_TPersona>" & LstTP.SelectedValue & "</U_TPersona><U_Promocion>" & LstPromos.SelectedValue & "</U_Promocion><U_megavol>" & LstMV.SelectedValue & "</U_megavol><U_Stock>" & LstStock.SelectedValue & "</U_Stock><U_InvoiceId>" & VentaDirecta & "</U_InvoiceId></row></Documents>"
                ElseIf validarCampos() IsNot "-1" Then
                    Encabezado = "<Documents><row><DocType>dDocument_Items</DocType><SalesPersonCode>" & TxtSlpCode.Text.ToString & "</SalesPersonCode>" & Entrega & "<DocDate>" & Fecha_jbqb & "</DocDate><DocDueDate>" & Fecha_jbqb & "</DocDueDate><TaxDate>" & Fecha_jbqb & "</TaxDate><CardCode>" & TxtCardCode.Text.ToString & "</CardCode><NumAtCard>E" & TxtSlpCode.Text.ToString & "-" & DocE & "</NumAtCard><DocCurrency>" & Moneda & "</DocCurrency><U_MetodoDePago>" & LstMP.SelectedValue & "</U_MetodoDePago><U_FormaDePago>" & LstFP.SelectedValue & "</U_FormaDePago><U_UsoCFDI>" & LstUso.SelectedValue & "</U_UsoCFDI><U_TipoDeComprobante>" & LstTipo.SelectedValue & "</U_TipoDeComprobante><U_TPersona>1</U_TPersona><U_Promocion>" & LstPromos.SelectedValue & "</U_Promocion><U_megavol>" & LstMV.SelectedValue & "</U_megavol><U_Stock>" & LstStock.SelectedValue & "</U_Stock><U_InvoiceId>" & VentaDirecta & "</U_InvoiceId><U_Hospitales>" & validarCampos() & "</U_Hospitales></row></Documents>"
                End If

                Dim Pedidonl As String = "<env:Envelope xmlns:env=""http://www.w3.org/2003/05/soap-envelope""><env:Header><SessionID>" & SessionId & "</SessionID></env:Header><env:Body><dis:AddObject xmlns:dis=""http//www.sap.com/SBO/DIS""><BOM><BO><AdmInfo><Object>oOrders</Object></AdmInfo>" & Encabezado & LineasNL & "</BO></BOM></dis:AddObject></env:Body></env:Envelope>"
                Dim Pedidobo2 As String = "<env:Envelope xmlns:env=""http://www.w3.org/2003/05/soap-envelope""><env:Header><SessionID>" & SessionId & "</SessionID></env:Header><env:Body><dis:AddObject xmlns:dis=""http//www.sap.com/SBO/DIS""><BOM><BO><AdmInfo><Object>oOrders</Object></AdmInfo>" & Encabezado & LineasBO & "</BO></BOM></dis:AddObject></env:Body></env:Envelope>"

                If AutNl = True And validarCampos() IsNot "-1" Then
                    Dim envio As System.Xml.XmlNode = DS.Interact(Pedidonl)
                    Dim DESAP As String

                    If envio.ChildNodes.Item(0).ChildNodes.Item(0).ChildNodes.Item(0).Name = "RetKey" Then
                        DESAP = envio.ChildNodes.Item(0).ChildNodes.Item(0).ChildNodes.Item(0).InnerText
                        lblErrId.Text = "OK"
                        OkNum.Text = "E" & TxtSlpCode.Text.ToString & "-" & DESAP
                        OkMsg.Text = "Se ha generado el pedido electrónico <b>" & "E" & TxtSlpCode.Text.ToString & "-" & DESAP & "</b> de forma satisfactoria!"
                        btnGenerarPedido.Enabled = False
                        Dim UpdDoc As String = "<env:Envelope xmlns:env=""http://www.w3.org/2003/05/soap-envelope""><env:Header><SessionID>" & SessionId & "</SessionID></env:Header><env:Body><dis:UpdateObject xmlns:dis=""http//www.sap.com/SBO/DIS""><BOM><BO><AdmInfo><Object>oOrders</Object></AdmInfo><QueryParams><DocEntry>" & DESAP & "</DocEntry></QueryParams><Documents><row><NumAtCard>" & OkNum.Text.ToString & "</NumAtCard></row></Documents></BO></BOM></dis:UpdateObject></env:Body></env:Envelope>"
                        Dim ActPedido As System.Xml.XmlNode = DS.Interact(UpdDoc)
                        If AutBO = True Then
                            Dim Pedidobo As String = "<env:Envelope xmlns:env=""http://www.w3.org/2003/05/soap-envelope""><env:Header><SessionID>" & SessionId & "</SessionID></env:Header><env:Body><dis:AddObject xmlns:dis=""http//www.sap.com/SBO/DIS""><BOM><BO><AdmInfo><Object>oOrders</Object></AdmInfo>" & "<Documents><row><DocType>dDocument_Items</DocType><SalesPersonCode>" & TxtSlpCode.Text.ToString & "</SalesPersonCode>" & EntregaBO & "<Comments>Complemento backorder del Pedido " & "E" & TxtSlpCode.Text.ToString & "-" & DESAP & ".</Comments>" & "<DocDate>" & Now.ToString("yyyyMMdd") & "</DocDate><DocDueDate>" & Now.ToString("yyyyMMdd") & "</DocDueDate><TaxDate>" & Now.ToString("yyyyMMdd") & "</TaxDate><CardCode>" & TxtCardCode.Text.ToString & "</CardCode><NumAtCard>" & "E" & TxtSlpCode.Text.ToString & "-" & DESAP & "</NumAtCard><DocCurrency>" & Moneda & "</DocCurrency><U_MetodoDePago>" & LstMP.SelectedValue & "</U_MetodoDePago><U_FormaDePago>" & LstFP.SelectedValue & "</U_FormaDePago><U_UsoCFDI>" & LstUso.SelectedValue & "</U_UsoCFDI><U_TipoDeComprobante>" & LstTipo.SelectedValue & "</U_TipoDeComprobante><U_TPersona>" & LstTP.SelectedValue & "</U_TPersona><U_Promocion>" & LstPromos.SelectedValue & "</U_Promocion><U_megavol>" & LstMV.SelectedValue & "</U_megavol><U_Stock>" & LstStock.SelectedValue & "</U_Stock><U_InvoiceId>" & VentaDirecta & "</U_InvoiceId></row></Documents>" & LineasBO & "</BO></BOM></dis:AddObject></env:Body></env:Envelope>"
                            Dim enviobo As System.Xml.XmlNode = DS.Interact(Pedidobo)
                            Dim DESAPbo As String
                            If enviobo.ChildNodes.Item(0).ChildNodes.Item(0).ChildNodes.Item(0).Name = "RetKey" Then
                                DESAPbo = enviobo.ChildNodes.Item(0).ChildNodes.Item(0).ChildNodes.Item(0).InnerText
                                Dim UpdDocbo As String = "<env:Envelope xmlns:env=""http://www.w3.org/2003/05/soap-envelope""><env:Header><SessionID>" & SessionId & "</SessionID></env:Header><env:Body><dis:UpdateObject xmlns:dis=""http//www.sap.com/SBO/DIS""><BOM><BO><AdmInfo><Object>oOrders</Object></AdmInfo><QueryParams><DocEntry>" & DESAPbo & "</DocEntry></QueryParams><Documents><row><NumAtCard>" & "C" & TxtSlpCode.Text.ToString & "-" & DESAPbo & "</NumAtCard></row></Documents></BO></BOM></dis:UpdateObject></env:Body></env:Envelope>"
                                Dim ActPedidobo As System.Xml.XmlNode = DS.Interact(UpdDocbo)
                                Dim Mailbo As String = Funciones.EnviaMail("C" & TxtSlpCode.Text.ToString & "-" & DESAPbo, TextMail.Text.ToString, RowsHTML, Stotal, Itotal, Total, TxtCardName.Text.ToString)
                                OkMsg.Text = OkMsg.Text & "<br /><br />Se ha generado el complemento de pedido electrónico para backorder <b>" & "C" & TxtSlpCode.Text.ToString & "-" & DESAPbo & "</b> de forma satisfactoria!"
                            End If
                        End If
                        SaveDocument("C", OkNum.Text.ToString)
                        Dim Mail As String = Funciones.EnviaMail(OkNum.Text.ToString, TextMail.Text.ToString, RowsHTML, Stotal, Itotal, Total, TxtCardName.Text.ToString)
                        If Mail <> "" Then
                            OkMsg.Text = OkMsg.Text & "<br /><br />" & Mail.Replace(";", "<br />")
                        End If
                    Else
                        lblErrId.Text = "Error"
                        ErrorNum.Text = "Error" & envio.ChildNodes.Item(0).ChildNodes.Item(0).ChildNodes.Item(0).InnerText.Replace("env:Receiver", "").Replace("env:Sender", "")
                        ErrorMsg.Text = "Se ha producido un error, revise la información de referencia." & "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[ " & envio.ChildNodes.Item(0).ChildNodes.Item(0).ChildNodes.Item(1).InnerText & " ]"
                    End If
                End If

                If AutBO = True And AutNl = False And validarCampos() IsNot "-1" Then
                    Dim enviobo2 As System.Xml.XmlNode = DS.Interact(Pedidobo2)
                    Dim DESAPbo2 As String

                    If enviobo2.ChildNodes.Item(0).ChildNodes.Item(0).ChildNodes.Item(0).Name = "RetKey" Then
                        DESAPbo2 = enviobo2.ChildNodes.Item(0).ChildNodes.Item(0).ChildNodes.Item(0).InnerText
                        lblErrId.Text = "OK"
                        OkNum.Text = "B" & TxtSlpCode.Text.ToString & "-" & DESAPbo2
                        OkMsg.Text = "Se ha generado el pedido electrónico para backorder <b>" & "B" & TxtSlpCode.Text.ToString & "-" & DESAPbo2 & "</b> de forma satisfactoria!"
                        btnGenerarPedido.Enabled = False
                        Dim UpdDoc As String = "<env:Envelope xmlns:env=""http://www.w3.org/2003/05/soap-envelope""><env:Header><SessionID>" & SessionId & "</SessionID></env:Header><env:Body><dis:UpdateObject xmlns:dis=""http//www.sap.com/SBO/DIS""><BOM><BO><AdmInfo><Object>oOrders</Object></AdmInfo><QueryParams><DocEntry>" & DESAPbo2 & "</DocEntry></QueryParams><Documents><row><NumAtCard>" & OkNum.Text.ToString & "</NumAtCard></row></Documents></BO></BOM></dis:UpdateObject></env:Body></env:Envelope>"
                        Dim ActPedido As System.Xml.XmlNode = DS.Interact(UpdDoc)
                        SaveDocument("C", OkNum.Text.ToString)
                        Dim Mail As String = Funciones.EnviaMail(OkNum.Text.ToString, TextMail.Text.ToString, RowsHTML, Stotal, Itotal, Total, TxtCardName.Text.ToString)
                        If Mail <> "" Then
                            OkMsg.Text = OkMsg.Text & "<br /><br />" & Mail.Replace(";", "<br />")
                        End If
                    Else

                        lblErrId.Text = "Error"
                        ErrorNum.Text = "Error" & enviobo2.ChildNodes.Item(0).ChildNodes.Item(0).ChildNodes.Item(0).InnerText.Replace("env:Receiver", "").Replace("env:Sender", "")
                        ErrorMsg.Text = "Se ha producido un error, revise la información de referencia." & "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[ " & enviobo2.ChildNodes.Item(0).ChildNodes.Item(0).ChildNodes.Item(1).InnerText & " ]"
                    End If
                End If

                Dim Logout As String = DS.Logout(SessionId)
            Catch ex As Exception
                lblErrId.Text = "Error"
                ErrorNum.Text = "Error"
                ErrorMsg.Text = ex.Message
            End Try
        End If
        ClearMemory()
    End Sub
End Class