Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Data
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Xml

Public Class Login
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub BtnLogin_Click(sender As Object, e As EventArgs) Handles BtnLogin.Click
        Dim Resp As DataTable
        Dim Acceso As String
        Dim WhsCode As String
        Dim Edo As String
        Dim SlpCode As String
        Dim SlpName As String
        Try
            Resp = Funciones.IniciarSesion(Username.Text, Password.Text)
            If Resp.Rows.Count >= 1 Then
                Acceso = Resp.Rows.Item(0).Item(0).ToString
                WhsCode = Resp.Rows.Item(0).Item(1).ToString
                Edo = Resp.Rows.Item(0).Item(2).ToString
                SlpCode = Resp.Rows.Item(0).Item(3).ToString
                SlpName = Resp.Rows.Item(0).Item(4).ToString
                If Acceso = "Y" Then
                    'Redireccionamos al sitio
                    If Edo = "1" Then
                        Session("UserName") = Username.Text.ToString
                        Session("WhsCode") = WhsCode.ToString
                        Session("SlpCode") = SlpCode.ToString
                        Session("SlpName") = SlpName.ToString
                        Dim Url As String
                        Try
                            Url = Session("Page").ToString
                        Catch ex As Exception
                            Url = "Default.aspx"
                        End Try
                        Try
                            Server.Transfer(Url)
                        Catch ex As Exception
                            Response.Redirect(Url)
                        End Try
                    Else
                        ErrorTitulo.Text = "Usuario Inactivo:"
                        ErrorMsg.Text = "Por favor, contacte al administrador del sistema."
                    End If
                Else
                    ErrorTitulo.Text = "Usuario y/o contraseña incorrectos:"
                    ErrorMsg.Text = "Por favor, verifique su nombre de usuario y contraseña."
                End If
            Else
                'SQL Caido
                ErrorTitulo.Text = "Error al invocar SP:"
                ErrorMsg.Text = "No se logro obtener información del store procedure."
            End If
        Catch ex As Exception
            ErrorTitulo.Text = "Error de aplicación:"
            ErrorMsg.Text = ex.Message
        End Try
    End Sub

End Class