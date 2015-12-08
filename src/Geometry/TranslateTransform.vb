


Namespace Geometry
    Public Class TranslateTransform
        Inherits Transform

        Public Sub New()

        End Sub
        Public Sub New(x As Double, y As Double)
            Me.X = x
            Me.Y = y
        End Sub
        Public Sub New(location As Point)
            Me.X = location.X
            Me.Y = location.Y
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

