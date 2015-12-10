Imports Geometry



Public Class Canvas

    Private _graphics As Drawing.Graphics
    Private _states As New Stack(Of Drawing.Drawing2D.GraphicsState)

    Public Sub New(graphics As Drawing.Graphics)
        _graphics = graphics
    End Sub

    Public Sub Draw(drawing As IDrawable)
        drawing.Draw(Me)
    End Sub

    Public Sub DrawPath(path As NodePath)

        Using gp As Drawing.Drawing2D.GraphicsPath = path.ToGraphicsPath

            _graphics.FillPath(Drawing.Brushes.Red, gp)



            _graphics.DrawPath(Drawing.Pens.Black, gp)

        End Using

    End Sub

    Public Sub Save()
        _states.Push(_graphics.Save())
    End Sub

    Public Sub Restore()
        _graphics.Restore(_states.Pop)
    End Sub

    Public Sub Translate(dx As Double, dy As Double)
        _graphics.TranslateTransform(dx, dy)
    End Sub

    Public Sub Scale(s As Double)
        _graphics.ScaleTransform(s, s)
    End Sub

    Public Sub Rotate(angle As Single)
        _graphics.RotateTransform(angle)
    End Sub

    Public Sub RotateAt(angle As Single, center As Point)
        _graphics.TranslateTransform(center.X, center.Y)
        _graphics.RotateTransform(angle)
        _graphics.TranslateTransform(-center.X, -center.Y)
    End Sub

    Public Sub Smooth()
        _graphics.SmoothingMode = Drawing.Drawing2D.SmoothingMode.AntiAlias
    End Sub
    Public Sub NoSmooth()
        _graphics.SmoothingMode = Drawing.Drawing2D.SmoothingMode.None
    End Sub
End Class
