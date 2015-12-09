Imports System.Drawing

Public Interface DrawingElement
    Inherits IDisposable

    Sub Draw(g As Graphics)
    Function HitTest(ByVal p As PointF) As Boolean
    Function GetElementType() As ElementType

    'TransForm Function
    Function GetBound() As RectangleF
    Sub Translate(ByVal x As Single, ByVal y As Single)
    Sub Resize(width As Single, height As Single)
    Property Rotation As Single
    Function GetSkeliton() As GPath
End Interface


Public Enum ElementType
    PathElement
    ImageElement
    Textelement
    GroupElement
End Enum


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

