

Public MustInherit Class Brush

    Protected MustOverride Function GetNativeBrush() As Drawing.Brush

End Class


Public Class SolidColorBrush
    Inherits Brush

    Public Property Color As Color

    Protected Overrides Function GetNativeBrush() As Drawing.Brush
        Return New Drawing.SolidBrush(Me.Color.toDColor())
    End Function
End Class
