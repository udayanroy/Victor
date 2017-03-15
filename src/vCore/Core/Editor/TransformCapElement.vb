Imports Geometry


Public Class TransformCapElement
    Inherits CapElement


    Public Sub New(editor As Editor)
        MyBase.New(editor)
    End Sub

    Public Function GetSelectionsBound() As Rect
        Dim Bound = Me.SelectionHolder.SelectionList(0).Element.GetItemBound()

        For Each ElementSelection In Me.SelectionHolder.SelectionList
            Bound = Bound.union(ElementSelection.Element.GetItemBound())
        Next
        'Dim path As New NodePath
        'path.AddRectangle(Bound)
        'Editor.View.Memory2screen(path)
        'Return path.GetBound
        Editor.View.Memory2screen(Bound)

        Return Bound
    End Function

    Public Function GetSelectionsBounds() As List(Of Rect)
        Dim rctlst As New List(Of Rect)
        For Each ElementSelection In Me.SelectionHolder.SelectionList
            Dim rct = ElementSelection.Element.GetItemBound()
            Editor.View.Memory2screen(rct)
            rctlst.Add(rct)
        Next

        Return rctlst
    End Function

    Public Function GetSelectionSkeliton() As NodePath
        ' !! upgrade code for multi selection !!
        Dim SkelitonPath = SelectionHolder.SelectionList(0).Element.GetSkeliton
        Editor.View.Memory2screen(SkelitonPath)
        Return SkelitonPath
    End Function

    Public Sub ApplyTransform(transform As Transform)
        'Dim t = New TransformGroup

        't.Items.Add(transform)
        't.Items.Add(New MatrixTransform(Editor.))
        'Me.SelectionHolder.SelectionList(0).Element.ApplyTransform(t)
        'Dim mat = Editor.View.Screen2memory()  'Matrix.Identity
        'mat.Multiply(transform.Value)
        'Me.SelectionHolder.SelectionList(0).Element.ApplyTransform(mat)
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
