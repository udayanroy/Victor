
Imports Geometry

''' <summary>
''' 
''' </summary>  
Public Class NodePathsCapElement
    Inherits CapElement

    Public Sub New(editor As Editor)
        MyBase.New(editor)
    End Sub

    Public Sub New(editor As Editor, Figure As NodeFigure)
        MyBase.New(editor)

        Throw New NotImplementedException()
    End Sub
    Public Sub New(editor As Editor, Path As NodePath)
        MyBase.New(editor)

        Throw New NotImplementedException()
    End Sub

    Public Function GetSelectionSkeliton() As NodePath
        ' !! upgrade code for multi selection !!
        Dim SkelitonPath = SelectionHolder.SelectionList(0).Element.GetSkeliton
        Editor.View.Memory2screen(SkelitonPath)
        Return SkelitonPath
    End Function

    Public Function GetNodePoints() As List(Of CapNode)
        Throw New NotImplementedException()
    End Function
End Class
