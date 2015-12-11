Imports Graphics
Imports Geometry

Public Class ScrollVisual
    Implements IDrawable


    Public Sub New()

    End Sub

    Public Property ScrollPos As Point
    Public Property Children As IDrawable

    Public Sub Draw(canvas As Canvas) Implements IDrawable.Draw
        If Children Is Nothing Then Exit Sub
        canvas.Save()
        canvas.Translate(ScrollPos.X, ScrollPos.Y)
        canvas.Draw(Me.Children)
        canvas.Restore()
    End Sub

    Public Function GetArea() As Rect Implements IDrawable.GetArea
        If Children Is Nothing Then Return New Rect
        Return Children.GetArea
    End Function

    Public Function Outer2InnerPt(outerLocation As Point) As Point
        Return outerLocation - ScrollPos
    End Function

    Public Function Outer2Inner() As Matrix
        Dim mat As New Matrix
        mat.Translate(ScrollPos.X, ScrollPos.Y)
        mat.Invert()
        Return mat
    End Function

    Public Function Inner2OuterPt(innerlocation As Point) As Point
        Return innerlocation + ScrollPos
    End Function

    Public Function Inner2Outer() As Matrix
        Dim mat As New Matrix
        mat.Translate(ScrollPos.X, ScrollPos.Y)
        Return mat
    End Function
End Class
