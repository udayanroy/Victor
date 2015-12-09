Imports System.Drawing

Public Interface DrawingElement
    Inherits IDisposable

    Sub Draw(g As Graphics)
    Function isVisible(ByVal p As PointF) As Boolean
    Function isVisible(ByVal rect As RectangleF) As Boolean
    Function isBoundVisible(ByVal rect As RectangleF) As Boolean
    Function isBoundVisible(ByVal p As PointF) As Boolean
    Function GetElementType() As ElementType

    'Get loosly claculated ElementBound
    Function GetElementBound() As RectangleF

    'Transform Function
    Function GetItemBound() As RectangleF
    Sub Translate(ByVal x As Single, ByVal y As Single)
    Sub Resize(width As Single, height As Single)
    Sub ReArrange(ByVal x As Single, ByVal y As Single, ByVal width As Single, ByVal height As Single)
    Property Rotation As Single
    Function GetSkeliton() As GPath
    Sub ApplyTransform(mat As Drawing2D.Matrix)
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

