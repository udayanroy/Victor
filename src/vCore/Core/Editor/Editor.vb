Imports Geometry
Imports Graphics



Public Class Editor


    Dim vcor As vCore
    Dim _selections As Selections
    Dim iedt As IEditor

    'global DrwingElement propertys
    'Dim _fillBrush As Brush
    'Dim _strokecolor As Pen
    'Dim _strokewidth As Single = 1
    'Dim _isfill As Boolean = True
    'Dim _isstroke As Boolean = True


    Public Event PropertyChanged()
    Public Event SelectionChanged()

    Public Sub New(ByRef v As vCore)

        vcor = v
        _selections = New Selections(Me)

    End Sub
    Public Function HittestAt(ByVal p As Point) As Selection
        Dim retn As New Selection()

        Dim memloc As memLoc
        Dim flage As Boolean = False

        Dim mp = p
        vcor.View.Screen2memory(mp)

        Dim len, lobj As Integer
        len = vcor.Memory.Layers.Count

        For l As Integer = 0 To len - 1
            lobj = vcor.Memory.Layers(l).Item.Count
            For k As Integer = 0 To lobj - 1
                If vcor.Memory.Layers(l).Item(k).isVisible(mp) Then
                    memloc.create(l, k)

                    flage = True
                End If
            Next
        Next

        If flage = True Then
            retn.MemoryLocation = memloc
            retn.isEmty = False
        Else
            retn.isEmty = True
        End If

        Return retn
    End Function
    Public Function SelectAt(ByVal p As Point) As Integer
        Dim returnvalue As SelectionType
        returnvalue = Me.selections.SelectAt(p)
        RaiseEvent SelectionChanged()
        Return returnvalue

    End Function

    Private Sub OnSelectionChanged()
        Dim path = Me.getSelectionPath
        'Me._fillcolor = path.FillColor
        'Me._strokecolor = path.StrokeColor
        'Me._strokewidth = path.StrokWidth
        'Me._isfill = path.isFill
        'Me._isstroke = path.isStroke
        RaiseEvent PropertyChanged()
    End Sub

    Public Sub DisSelect()
        selections.Clear()
    End Sub

    Public ReadOnly Property selections() As Selections
        Get
            Return Me._selections
        End Get
    End Property

    Public ReadOnly Property View As ControlVisual
        Get
            Return vcor.View
        End Get

    End Property
    Public Sub Refresh()
        vcor.View.Refresh()
    End Sub


    Public Sub setIEdit(ByVal Editor As IEditor)
        Me.iedt = Editor
        Refresh()
    End Sub

    Public Function getSelectionPath() As PathElement
        Return vcor.mem.Layers(_selections.MemoryLocation.layer).Item(_selections.MemoryLocation.obj)
    End Function

    Public Function getSelection() As DrawingElement
        Return vcor.mem.Layers(_selections.MemoryLocation.layer).Item(_selections.MemoryLocation.obj)
    End Function


    Public Sub paint(canvas As Canvas)

        If Me.iedt IsNot Nothing Then
            Me.iedt.Draw(canvas)
        End If

    End Sub
    'Public Sub mouse_Down(ByRef e As System.Windows.Forms.MouseEventArgs)
    '    If Me.type <> selectionType.None Then
    '        'Me.iedt.mouse_Down(e)
    '    End If
    'End Sub
    'Public Sub mouse_Move(ByRef e As System.Windows.Forms.MouseEventArgs)
    '    If Me.type <> selectionType.None Then
    '        Me.iedt.mouse_Move(e)
    '    End If
    'End Sub
    'Public Sub mouse_Up(ByRef e As System.Windows.Forms.MouseEventArgs)
    '    If Me.type <> selectionType.None Then
    '        Me.iedt.mouse_Up(e)
    '    End If
    'End Sub
    Public Function getBoundRect() As Rect
        Return vcor.Memory.Layers(_selections.MemoryLocation.layer).Item(_selections.MemoryLocation.obj).GetElementBound
    End Function

    Public Sub Cut()
        If (Not selection.isEmty) Then
            Dim obj As PathElement = vcor.mem.Layers(_selections.MemoryLocation.layer).Item(_selections.MemoryLocation.obj)
            Windows.Forms.Clipboard.SetData("vcimg", obj)
            vcor.mem.Layers(_selections.MemoryLocation.layer).Item.RemoveAt(_selections.MemoryLocation.obj)
            _selections.isEmty = True
            Me.Refresh()
        End If
    End Sub

    Public Sub Copy()
        If (Not selection.isEmty) Then
            Dim obj As PathElement = vcor.mem.Layers(_selections.MemoryLocation.layer).Item(_selections.MemoryLocation.obj)
            Windows.Forms.Clipboard.SetData("vcimg", obj)
        End If

    End Sub

    Public Sub Past()

        If Windows.Forms.Clipboard.ContainsData("vcimg") Then
            Dim path As PathElement = Windows.Forms.Clipboard.GetData("vcimg")
            vcor.Memory.Layers(0).Item.Add(path)
            Me.Refresh()
        End If

    End Sub

    Public Sub Delete()
        If Not selection.isEmty Then
            vcor.mem.Layers(_selections.MemoryLocation.layer).Item.RemoveAt(_selections.MemoryLocation.obj)
            _selections.isEmty = True
            Me.Refresh()
        End If

    End Sub

    Public Sub ClearAll()
        For Each Layer As Layer In vcor.mem.Layers
            Layer.Item.Clear()
        Next
        _selections.isEmty = True
        Me.Refresh()
    End Sub

    'Public Property FillColor As Color
    '    Get
    '        Return Me._fillcolor
    '    End Get
    '    Set(value As Color)
    '        Me._fillcolor = value
    '        If Not Me.selection.isEmty Then
    '            Dim path = Me.getSelectionPath
    '            path.FillColor = value
    '            Me.Refresh()
    '        End If
    '    End Set
    'End Property

    'Public Property StrokeColor As Color
    '    Get
    '        Return Me._strokecolor
    '    End Get
    '    Set(value As Color)
    '        Me._strokecolor = value

    '        If Not Me.selection.isEmty Then
    '            Dim path = Me.getSelectionPath
    '            path.StrokeColor = value
    '            Me.Refresh()
    '        End If
    '    End Set
    'End Property

    'Public Property strokeWidth As Single
    '    Get
    '        Return Me._strokewidth
    '    End Get
    '    Set(value As Single)
    '        Me._strokewidth = value
    '        If Not Me.selection.isEmty Then
    '            Dim path = Me.getSelectionPath
    '            path.StrokWidth = value
    '            Me.Refresh()
    '        End If
    '    End Set
    'End Property

    'Public Property isFill As Boolean
    '    Get
    '        Return Me._isfill
    '    End Get
    '    Set(value As Boolean)
    '        Me._isfill = value
    '        If Not Me.selection.isEmty Then
    '            Dim path = Me.getSelectionPath
    '            path.isFill = value
    '            Me.Refresh()
    '        End If
    '    End Set
    'End Property

    'Public Property isStroke As Boolean
    '    Get
    '        Return Me._isstroke
    '    End Get
    '    Set(value As Boolean)
    '        Me._isstroke = value
    '        If Not Me.selection.isEmty Then
    '            Dim path = Me.getSelectionPath
    '            path.isStroke = value
    '            Me.Refresh()
    '        End If
    '    End Set
    'End Property
End Class

