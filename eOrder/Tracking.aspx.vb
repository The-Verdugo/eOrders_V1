Imports System.Data.SqlClient

Public Class Tracking
    Inherits System.Web.UI.Page

    Public Shared TrackOrder As DataTable
    Public Shared TrackOrderEncabezado As DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If IsPostBack Then
            Response.Redirect("Tracking.aspx?eOrder=" & TxtFolio.Text.ToString.ToUpper)
        End If
        If Not IsPostBack Then
            Session("Page") = "Tracking.aspx"
            '    Dim Resp As DataTable
            '    Dim Acceso As String
            '    Dim WhsCode As String
            '    Dim Edo As String
            '    Try
            '        Resp = Funciones.ValidaUsr(Session("UserName").ToString)
            '        If Resp.Rows.Count >= 1 Then
            '            Acceso = Resp.Rows.Item(0).Item(0).ToString
            '            WhsCode = Resp.Rows.Item(0).Item(1).ToString
            '            Edo = Resp.Rows.Item(0).Item(2).ToString
            '            If Acceso = "Y" Then
            '                'Redireccionamos al sitio
            '                If Edo = "1" Then
            '                    'Bienvenido, pase usted!!!
            'TxtUser.Text = Session("UserName").ToString
            'TxtWhsCode.Text = WhsCode.ToString
            'TxtWhsName.Text = Funciones.ObtieneWhsName(WhsCode.ToString)
            'TxtSlpCode.Text = Session("SlpCode").ToString
            'TxtSlpName.Text = Session("SlpName").ToString
            'TxtAlmacen.Text = WhsCode.ToString & " - " & TxtWhsName.Text.ToString
            'TxtDistribuidor.Text = TxtSlpCode.Text.ToString & " - " & TxtSlpName.Text.ToString
            'Funciones.ObtieneLista("AnioMes" & Session("SlpCode").ToString, LstAnioMes)
            'Try
            '    LstAnioMes.SelectedIndex = 0
            'Catch ex As Exception

            'End Try
            Try
                TxtFolio.Text = Request.QueryString("eOrder").ToString.ToUpper
            Catch ex As Exception
                'TxtFolio.Text = ""
            End Try

            If TxtFolio.Text.ToString <> "" And TxtFolio.Text.ToString.Contains("-") And (TxtFolio.Text.ToString.Contains("E") Or TxtFolio.Text.ToString.Contains("B") Or TxtFolio.Text.ToString.Contains("M") Or TxtFolio.Text.ToString.Contains("C")) Then
                LoadeOrdersGrid(TxtFolio.Text.ToString, "0")
                LoadeOrdersGrid(TxtFolio.Text.ToString, "1")
            Else
                LoadeOrdersGrid("X000-00000", "0")
                LoadeOrdersGrid("X000-00000", "1")
            End If
            '                Else
            '                    Response.Redirect("Login.aspx")
            '                End If
            '            Else
            '                Response.Redirect("Login.aspx")
            '            End If
            '        Else
            '            Response.Redirect("Login.aspx")
            '        End If
            '    Catch ex As Exception
            '        Response.Redirect("Login.aspx")
            '    End Try
        End If
    End Sub
    Protected Sub LoadeOrdersGrid(ByVal TrackId As String, ByVal Detalle As String)
        Dim constr As String = ConfigurationManager.ConnectionStrings("SQLConnection").ConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand("EXECUTE [DesarrollosGVI].[dbo].[sp_eO_Track] '" & TrackId & "','" & Detalle & "'")
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using dt As New DataTable()
                        dt.TableName = "TRACK"
                        sda.Fill(dt)
                        If Detalle = "1" Then
                            TrackOrderEncabezado = New DataTable
                            TrackOrderEncabezado = dt.Copy()
                        Else
                            TrackOrder = New DataTable
                            TrackOrder = dt.Copy()
                        End If
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
End Class