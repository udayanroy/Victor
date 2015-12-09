


Public Class TransformGroup
    Inherits Transform

    Private _items As New List(Of Transform)
    Public Sub New()

    End Sub

    Public ReadOnly Property Items As List(Of Transform)
        Get
            Return _items
        End Get
    End Property

    Public Overrides ReadOnly Property Value As Matrix
        Get
            Dim matrx = Matrix.Identity
            For Each transf As Transform In Me._items
                matrx.Multiply(transf.Value)
            Next
            Return matrx
        End Get
    End Property
End Class


