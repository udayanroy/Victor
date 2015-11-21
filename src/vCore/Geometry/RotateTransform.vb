



Namespace Geometry
    Public Class RotateTransform
        Inherits Transform


        Public Sub New()

        End Sub
        Public Sub New(angle As Double)
            Me.Angle = angle
        End Sub

        Public Sub New(angle As Double, center As Point)
            Me.Angle = angle
            Me.Center = center
        End Sub
        Public Property Angle As Double

        Public Property Center As Point


        Public Overrides ReadOnly Property Value As Matrix
            Get
                Dim m = Matrix.Identity

                m.RoatateAt(Angle, Center.X, Center.Y)

                Return m
            End Get
        End Property
    End Class
End Namespace

