Imports Geometry
Imports Graphics


Public Class ZoomVisual
    Implements IDrawable

    Public Property Children As IDrawable
    Public Property ZoomPercentage As UShort

    Public Sub Draw(canvas As Canvas) Implements IDrawable.Draw
        If Children Is Nothing Then Exit Sub
        canvas.Save()
        canvas.Scale(Me.ZoomFraction)
        canvas.Draw(Me.Children)
        canvas.Restore()
    End Sub

    Public Function GetArea() As Rect Implements IDrawable.GetArea
        If Children Is Nothing Then Return New Rect
        Dim childArea = Children.GetArea
        Return New Rect(childArea.Left * ZoomFraction, childArea.Top * ZoomFraction,
                         childArea.Right * ZoomFraction, childArea.Bottom * ZoomFraction)
    End Function

    Private Property ZoomFraction As Single
        Get
            Return ZoomPercentage / 100
        End Get
        Set(value As Single)
            ZoomPercentage = value * 100
        End Set
    End Property

    Public Function Outer2InnerPt(outerLocation As Point) As Point
        Return outerLocation / ZoomFraction
    End Function

    Public Function Outer2Inner() As Matrix
        Dim mat As New Matrix
        mat.Scale(ZoomFraction, ZoomFraction)
        mat.Invert()
        Return mat
    End Function

    Public Function Inner2OuterPt(innerlocation As Point) As Point
        Return innerlocation * ZoomFraction
    End Function

    Public Function Inner2Outer() As Matrix
        Dim mat As New Matrix
        mat.Scale(ZoomFraction, ZoomFraction)
        Return mat
    End Function
End Class
