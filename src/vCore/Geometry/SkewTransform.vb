


Namespace Geometry
    Public Class SkewTransform
        Inherits Transform

        Public Sub New()

        End Sub

        Public Sub New(SkewHorizontal As Double, SkewVertical As Double)
            Me.SkewHorizontal = SkewHorizontal
            Me.SkewVertical = SkewVertical
        End Sub

        Public Property SkewHorizontal As Double

        Public Property SkewVertical As Double

        Public Overrides ReadOnly Property Value As Matrix
            Get
                Dim mat = Matrix.Identity
                mat.Shear(SkewHorizontal, SkewVertical)
                Return mat
            End Get
        End Property
    End Class
End Namespace

