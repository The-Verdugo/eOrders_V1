Public Class _Default
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Session("Page") = "Default.aspx"
        'Limpiamos memoria
        Funciones.ClearMemory()
    End Sub

End Class