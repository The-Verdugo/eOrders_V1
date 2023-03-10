Public Class detalles
    Private _codigo As String
    Private _name As String
    Private _cantidad As Double
    Public Property Codigo() As String
        Get
            Return _codigo
        End Get
        Set(ByVal value As String)
            _codigo = value
        End Set
    End Property
    Public Property Name() As String
        Get
            Return _name
        End Get
        Set(value As String)
            _name = value
        End Set
    End Property

    Public Property Cantidad As String
        Get
            Return _cantidad
        End Get
        Set(value As String)
            _cantidad = value
        End Set
    End Property
End Class
