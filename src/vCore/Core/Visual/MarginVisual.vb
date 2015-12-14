Imports Graphics
Imports Geometry

Public Class MarginVisual
    Implements IDrawable


    Public Sub Draw(canvas As Canvas) Implements IDrawable.Draw
        If Children Is Nothing Then Exit Sub
        canvas.Save()
        canvas.Translate(Margin, Margin)
        canvas.Draw(Children)
        canvas.Restore()
    End Sub

    Public Function GetArea() As Geometry.Rect Implements IDrawable.GetArea
        If Children Is Nothing Then Return New Rect
        Dim childArea = Children.GetArea
        Return New Rect(New Point(0, 0), childArea.Width + (2 * Margin), childArea.Height + (2 * Margin))
    End Function

    Public Function Outer2InnerPt(outerLocation As Point) As Point
        Return outerLocation - New Point(Margin, Margin)
    End Function

    Public Function Outer2Inner() As Matrix
        Dim mat As Matrix = Matrix.Identity
        mat.Translate(Margin, Margin)
        mat.Invert()
        Return mat
    End Function

    Public Function Inner2OuterPt(innerlocation As Point) As Point
        Return innerlocation + New Point(Margin, Margin)
    End Function

    Public Function Inner2Outer() As Matrix
        Dim mat As Matrix = Matrix.Identity
        mat.Translate(Margin, Margin)
        Return mat
    End Function

    Public Property Margin As UShort
    Public Property Children As IDrawable


End Class
