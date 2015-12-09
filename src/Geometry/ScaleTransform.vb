



Public Class ScaleTransform
    Inherits Transform

    Public Sub New()

    End Sub

    Public Sub New(sx As Double, sy As Double)
        ScaleX = sx
        ScaleY = sy
    End Sub

    Public Property ScaleX As Double = 1
    Public Property ScaleY As Double = 1


    Public Overrides ReadOnly Property Value As Matrix
        Get
            Dim mat = Matrix.Identity
            mat.Scale(ScaleX, ScaleY)
            Return mat
        End Get
    End Property
End Class


