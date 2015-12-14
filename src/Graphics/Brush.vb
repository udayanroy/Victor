

Public MustInherit Class Brush

    Friend MustOverride Function GetNativeBrush() As Drawing.Brush

End Class


Public Class SolidColorBrush
    Inherits Brush

    Public Sub New()

    End Sub
    Public Sub New(color As Color)
        Me.Color = color
    End Sub

    Public Property Color As Color

    Friend Overrides Function GetNativeBrush() As Drawing.Brush
        Dim cl = Me.Color.toDColor()
        Dim brs = New Drawing.SolidBrush(cl)
        Return brs
    End Function
End Class
