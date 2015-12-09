Imports System.Drawing

Public Interface DrawingElement
    Inherits IDisposable

    Sub Draw(ByRef g As Graphics)
    Function GetBound() As RectangleF
    Function HitTest(ByVal p As PointF) As Boolean
    Sub Translate(ByVal x As Single, ByVal y As Single)
End Interface



Public Class vPen
    Dim c As Color

    Public Sub New()
        c = color.Black

    End Sub
    Public Property color() As Color
        Get
            Return c
        End Get
        Set(ByVal value As Color)
            c = value
        End Set
    End Property
End Class

Public MustInherit Class vBrush


End Class

