

Public MustInherit Class Brush

    Friend MustOverride Function GetNativeBrush() As Drawing.Brush

End Class


Public Class SolidColorBrush
    Inherits Brush

    Public Sub New()

    End Sub
    Public Sub New(color As Color)
        color = color
    End Sub

    Public Property Color As Color

    Friend Overrides Function GetNativeBrush() As Drawing.Brush
        Return New Drawing.SolidBrush(Me.Color.toDColor())
    End Function
End Class
