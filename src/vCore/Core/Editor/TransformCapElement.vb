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
        ' The logic is to tranform the DOM element into screen then apply 
        ' desaire transform and then transform it back to dom.
        '  (Memory2screen * transform * Screen2memory)
        Dim t = New TransformGroup
        t.Items.Add(New MatrixTransform(Visual.Memory2screen))
        t.Items.Add(transform)
        t.Items.Add(New MatrixTransform(Visual.Screen2memory))
        Me.SelectionHolder.SelectionList(0).Element.ApplyTransform(t)

    End Sub

    Public Sub ApplyTransformDirect(transform As Transform)
        Me.SelectionHolder.SelectionList(0).Element.ApplyTransform(transform)
    End Sub

    Public Sub ApplyTranslateTransform(p1 As Point, p2 As Point)

        Visual.Screen2memory(p1)
        Visual.Screen2memory(p2)
        Me.SelectionHolder.SelectionList(0).Element.Translate(p2.X - p1.X, p2.Y - p1.Y)
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
