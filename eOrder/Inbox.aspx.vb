Imports System.Data.SqlClient
Public Class Contact
    Inherits Page
    Public Shared eOrders As DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Session("Page") = "Inbox.aspx"
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
                            TxtUser.Text = Session("UserName").ToString
                            TxtWhsCode.Text = WhsCode.ToString
                            TxtWhsName.Text = Funciones.ObtieneWhsName(WhsCode.ToString)
                            TxtSlpCode.Text = Session("SlpCode").ToString
                            TxtSlpName.Text = Session("SlpName").ToString
                            TxtAlmacen.Text = WhsCode.ToString & " - " & TxtWhsName.Text.ToString
                            TxtDistribuidor.Text = TxtSlpCode.Text.ToString & " - " & TxtSlpName.Text.ToString
                            Funciones.ObtieneLista("AnioMes" & Session("SlpCode").ToString, LstAnioMes)
                            Try
                                LstAnioMes.SelectedIndex = 0
                            Catch ex As Exception

                            End Try
                            LoadeOrdersGrid()
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
    End Sub
    Protected Sub LoadeOrdersGrid()
        Dim constr As String = ConfigurationManager.ConnectionStrings("SQLConnection").ConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand("EXECUTE [DesarrollosGVI].[dbo].[sp_eO_RPT] '" & LstAnioMes.SelectedValue.ToString & "'," & Session("SlpCode").ToString)
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using dt As New DataTable()
                        dt.TableName = "EORDRS"
                        sda.Fill(dt)
                        eOrders = New DataTable
                        eOrders = dt.Copy()
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

    Protected Sub LstAnioMes_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LstAnioMes.SelectedIndexChanged
        LoadeOrdersGrid()
    End Sub
End Class