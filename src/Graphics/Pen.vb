Public Class Pen

    Private _brush As Brush
    Private _width As Single
    Public Sub New(width As Single)
        Me._brush = New SolidColorBrush()
        Me._width = width
    End Sub

    Public Sub New(color As Color, width As Single)
        Me._brush = New SolidColorBrush(color)
        Me._width = width
    End Sub

    Public Sub New(brush As Brush, width As Single)
        Me._brush = brush
        Me._width = width
    End Sub

    Friend Function getNative() As Drawing.Pen
        Return New Drawing.Pen(_brush.GetNativeBrush, _width)
    End Function

End Class
