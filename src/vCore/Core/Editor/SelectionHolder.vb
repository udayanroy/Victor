
Public Class SelectionHolder
    Implements ISelectionHolder

    Public Event SelectionChanged(sender As Object) Implements ISelectionHolder.SelectionChanged

    Private _editor As Editor
    Private _selectionlist As New List(Of Selection)

    Public Sub New(editor As Editor)
        Me._editor = editor
    End Sub

    Private ReadOnly Property Editor As Editor
        Get
            Return Me._editor
        End Get
    End Property

    Public Sub AddSelectionAt(location As Geometry.Point) Implements ISelectionHolder.AddSelectionAt
        Throw New NotImplementedException()
    End Sub

    Public Function CheckSelectionAt(location As Geometry.Point) As Selection Implements ISelectionHolder.CheckSelectionAt
        Throw New NotImplementedException()
    End Function

    Public Sub Clear() Implements ISelectionHolder.Clear
        Me.SelectionList.Clear()
    End Sub

    Public Function Count() As Integer Implements ISelectionHolder.Count
        Return Me._selectionlist.Count
    End Function

    Public Function isEmpty() As Boolean Implements ISelectionHolder.isEmpty
        Return IIf(Me.Count = 0, True, False)
    End Function

    Public Function IsSelected(element As DrawingElement) As Boolean Implements ISelectionHolder.IsSelected
        Throw New NotImplementedException()
    End Function

    Public Function IsSelected(selection As Selection) As Boolean Implements ISelectionHolder.IsSelected
        Throw New NotImplementedException()
    End Function

    Public Sub MultiSelectionAt(location As Geometry.Point) Implements ISelectionHolder.MultiSelectionAt
        Throw New NotImplementedException()
    End Sub

    Public Sub RemoveSelectionAt(location As Geometry.Point) Implements ISelectionHolder.RemoveSelectionAt
        Throw New NotImplementedException()
    End Sub

    Public Function SelectAt(location As Geometry.Point) As SelectionType Implements ISelectionHolder.SelectAt

        'Clear Already Selected Elements
        Me.Clear()

        'Initialize variables
        Dim returnvalue As SelectionType

        Dim SelectionFound As Boolean = False
        Dim DocumentLocation = location
        Me.Editor.View.Screen2memory(DocumentLocation)
        Dim SelectedElementlayer As Layer = Nothing
        Dim SelectedElement As DrawingElement = Nothing

        'Search Element
        Dim Dom As Document = Me.Editor.View.DOM
        For Each layer As Layer In Dom.Layers
            For Each Element As DrawingElement In layer.Item
                If Element.isVisible(DocumentLocation) Then
                    SelectionFound = True
                    SelectedElementlayer = layer
                    SelectedElement = Element
                End If
            Next
        Next

        'Now return the found value
        If SelectionFound Then
            returnvalue = SelectionType.New_Selection
            Dim Selection As New Selection(SelectedElement, 0, SelectedElementlayer, 0)
            Me.SelectionList.Add(Selection)
        Else
            returnvalue = SelectionType.Empty
        End If
        Me.OnSelectionChanged() ' raise event that selection has changed

        Return returnvalue
    End Function

    Public ReadOnly Property SelectionList As List(Of Selection) Implements ISelectionHolder.SelectionList
        Get
            Return Me._selectionlist
        End Get
    End Property

    Private Sub OnSelectionChanged()
        RaiseEvent SelectionChanged(Me)
    End Sub

End Class
