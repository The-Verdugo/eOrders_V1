Imports System.Data.SqlClient
Public Class eOrdersViewer
    Inherits System.Web.UI.Page
    Public Shared Dirs As DataTable
    Public Shared Arts As DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'btnDelete_Click()
            Session("Page") = "Tracking.aspx"
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
                            TxteOrder.Text = Request.QueryString("eOrder").ToString.ToUpper
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
    End Sub

    Protected Sub LoadItemsGrid()
        Dim constr As String = ConfigurationManager.ConnectionStrings("SQLConnection").ConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand("EXECUTE [DesarrollosGVI].[dbo].[sp_eO_RDR1_v2] '" & TxtCardCode.Text.ToString & "','S'," & Session("SlpCode").ToString & ",NULL,NULL,NULL,NULL,NULL,'C','" & TxteOrder.Text.ToString & "',''")
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

    Protected Sub LoadDocument()
        Try
            Dim dtResp As DataTable
            Dim constr As String = ConfigurationManager.ConnectionStrings("SQLConnection").ConnectionString
            Using con As New SqlConnection(constr)
                Using cmd As New SqlCommand("EXECUTE [DesarrollosGVI].[dbo].[sp_eO_ORDR] 'S',NULL,'" & Session("SlpCode").ToString & "',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'C','" & TxteOrder.Text.ToString & "'")
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
                        Funciones.ObtieneLista("MEG@S" & cc, LstMV)
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
                        LblDE3.Text = LstDirs.SelectedValue.ToString
                        TxtDir.Text = dtResp.Rows.Item(0).Item(4).ToString
                        LstMP.SelectedValue = dtResp.Rows.Item(0).Item(5).ToString
                        LstFP.SelectedValue = dtResp.Rows.Item(0).Item(6).ToString
                        LstUso.SelectedValue = dtResp.Rows.Item(0).Item(7).ToString
                        LstTipo.SelectedValue = dtResp.Rows.Item(0).Item(8).ToString
                        LstPromos.SelectedValue = dtResp.Rows.Item(0).Item(9).ToString
                        LstStock.SelectedValue = dtResp.Rows.Item(0).Item(10).ToString
                        TxtDocDate.Text = dtResp.Rows.Item(0).Item(11).ToString
                    End If
                Catch ex As Exception
                    'getWhsName = ex.Message
                End Try
                ClearMemory()
            End Using
        Catch ex As Exception

        End Try
    End Sub
End Class