


Namespace Geometry
    Public Class TranslateTransform
        Inherits Transform

        Public Sub New()

        End Sub
        Public Sub New(x As Double, y As Double)

        End Sub
        Public Property X As Double
        Public Property Y As Double
        Public Overrides ReadOnly Property Value As Matrix
            Get
                Dim m = Matrix.Identity
                m.Translate(X, Y)
                Return m
            End Get
        End Property
    End Class
End Namespace

