Imports Geometry

Public Interface ISelectionHolder

    ReadOnly Property SelectionList As List(Of Selection)
    Function Count() As Integer
    Sub Clear()
    Function isEmpty() As Boolean

    Function SelectAt(location As Point) As SelectionType
    Sub AddSelectionAt(location As Point)
    Sub RemoveSelectionAt(location As Point)
    Sub MultiSelectionAt(location As Point)

    Function CheckSelectionAt(location As Point) As Selection
    Function IsSelected(selection As Selection) As Boolean
    Function IsSelected(element As DrawingElement) As Boolean

    Event SelectionChanged(sender As Object)
End Interface

Public Enum SelectionType
    Empty = 0
    Already_Exist = 1
    New_Selection = 2
    Selection_removed = 3
End Enum