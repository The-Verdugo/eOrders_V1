Imports System.Data.SqlClient
Public Class ModCustomer
    Inherits System.Web.UI.Page
    Public Shared eOrders As DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Session("Page") = "ModCustomer.aspx"
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
                            Dim cc As String
                            Try
                                cc = TxtCardCode.Text = Request.QueryString("cardcode").ToString.ToUpper
                            Catch ex As Exception
                                cc = ""
                            End Try
                            TxtCardCode.Text = cc 'Solo el codigo
                            TxtCardName.Text = Funciones.ObtieneCardName(cc, False)
                            Funciones.ObtieneLista("AllDirs" & cc, LstDirsE)
                            Try
                                LstDirsE.SelectedIndex = 0
                            Catch ex As Exception

                            End Try
                            LoadDir()
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
    Protected Sub LoadDir()
        Dim constr As String = ConfigurationManager.ConnectionStrings("SQLConnection").ConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand("EXECUTE [DesarrollosGVI].[dbo].[sp_eO_GetDir] '" & LstDirsE.SelectedValue.ToString & "','" & TxtCardCode.Text.ToString & "'")
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using dt As New DataTable()
                        dt.TableName = "ECRD1"
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

    Protected Sub LstDirsE_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LstDirsE.SelectedIndexChanged
        LoadDir()
    End Sub

End Class