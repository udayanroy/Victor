


Public Class MatrixTransform
    Inherits Transform


    Public Sub New()

    End Sub
    Public Sub New(m11 As Double, m12 As Double, m21 As Double,
             m22 As Double, dX As Double, dY As Double)

        Me.Matix = New Matrix(m11, m12, m21, m22, dX, dY)

    End Sub
    Public Sub New(mat As Matrix)
        Me.Matix = mat
    End Sub

    Public Property Matix As Matrix

    Public Overrides ReadOnly Property Value As Matrix
        Get
            Return Me.Matix
        End Get
    End Property
End Class


