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
End Class
