Imports Geometry


Public Class TransformCapElement
    Inherits CapElement


    Public Sub New(editor As Editor)
        MyBase.New(editor)
    End Sub

    Public Function GetSelectionsBound() As Rect

    End Function

    Public Function GetSelectionsBounds() As List(Of Rect)
        Throw New NotImplementedException()
    End Function

    Public Function GetSelectionSkeliton() As NodePath
        Throw New NotImplementedException()
    End Function

    Public Sub ApplyTransform(transform As Transform)
        Throw New NotImplementedException()
    End Sub

    Public Property centerpoint As Point
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Point)
            Throw New NotImplementedException()
        End Set
    End Property

End Class
