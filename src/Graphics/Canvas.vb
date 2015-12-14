Imports Geometry



Public Class Canvas

    Private _graphics As Drawing.Graphics
    Private _states As New Stack(Of Drawing.Drawing2D.GraphicsState)

    Public Sub New(graphics As Drawing.Graphics)
        _graphics = graphics
    End Sub

    Public ReadOnly Property g As Drawing.Graphics
        Get
            Return _graphics
        End Get
    End Property
    Public Sub Draw(drawing As IDrawable)
        drawing.Draw(Me)
    End Sub
    Public Sub Clear(color As Color)
        Throw New NotImplementedException
    End Sub

    Public Sub DrawRects(rects() As Rect, Optional ByVal Pen As Pen = Nothing, Optional ByVal Brush As Brush = Nothing)
        For Each Rect As Rect In rects
            Me.DrawRect(Rect, Pen, Brush)
        Next
    End Sub
    Public Sub DrawLine(p1 As Point, p2 As Point, Pen As Pen)

    End Sub
    Public Sub DrawEllipse(rect As Rect, Optional ByVal Pen As Pen = Nothing, Optional ByVal Brush As Brush = Nothing)
        Throw New NotImplementedException
    End Sub

    Public Sub DrawEllipse(x As Double, y As Double, width As Double, height As Double, Optional ByVal Pen As Pen = Nothing, Optional ByVal Brush As Brush = Nothing)
        Throw New NotImplementedException
    End Sub

    Public Sub DrawRect(rect As Rect, Optional ByVal Pen As Pen = Nothing, Optional ByVal Brush As Brush = Nothing)
        If (Pen IsNot Nothing) Or (Brush IsNot Nothing) Then
            Dim rct As Drawing.RectangleF = rect.ToRectanglef
            If Brush IsNot Nothing Then
                Using nb = Brush.GetNativeBrush
                    _graphics.FillRectangle(nb, rct)
                End Using
            End If
            If Pen IsNot Nothing Then
                Using np = Pen.getNative
                    _graphics.DrawRectangle(np, rct.X, rct.Y, rct.Width, rct.Height)
                End Using
            End If
        End If
    End Sub
    Public Sub DrawPath(path As NodePath, Optional ByVal Pen As Pen = Nothing, Optional ByVal Brush As Brush = Nothing)
        If (Pen IsNot Nothing) Or (Brush IsNot Nothing) Then
            Using gp As Drawing.Drawing2D.GraphicsPath = path.ToGraphicsPath
                If Brush IsNot Nothing Then
                    Using nb = Brush.GetNativeBrush
                        _graphics.FillPath(nb, gp)
                    End Using
                End If
                If Pen IsNot Nothing Then
                    Using np = Pen.getNative
                        _graphics.DrawPath(np, gp)
                    End Using
                End If
            End Using
        End If
    End Sub

    Public Sub DrawImage(image As Image)
        _graphics.DrawImage(image.GetBitmap, image.bound)
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
