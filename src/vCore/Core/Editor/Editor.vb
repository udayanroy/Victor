Imports Geometry
Imports Graphics



Public Class Editor


    Dim vcor As vCore
    Dim _SelectionHolder As SelectionHolder
    Dim iedt As IEditor
    Dim ActiveTool As Tool
    Public Event SelectionChanged()

    Public Sub New(ByRef v As vCore)

        vcor = v
        Me._SelectionHolder = New SelectionHolder(Me)

    End Sub
   
    Public Function SelectAt(ByVal p As Point) As Integer
        Dim returnvalue As SelectionType
        returnvalue = Me.SelectionHolder.SelectAt(p)
        RaiseEvent SelectionChanged()
        Return returnvalue

    End Function


    Public Sub DisSelect()
        Me.SelectionHolder.Clear()
    End Sub

    Public ReadOnly Property SelectionHolder() As SelectionHolder
        Get
            Return Me._SelectionHolder
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

    Public Sub SetActiveTool(tool As Tool)
        Me.ActiveTool = tool
    End Sub

    Public Sub paint(canvas As Canvas)
        If Me.ActiveTool IsNot Nothing Then
            Me.ActiveTool.Draw(canvas)
        End If
    End Sub

    'Public Function getSelectionPath() As PathElement
    '    Return vcor.mem.Layers(_selections.MemoryLocation.layer).Item(_selections.MemoryLocation.obj)
    'End Function


    'Public Sub Cut()
    '    If (Not Selection.isEmty) Then
    '        Dim obj As PathElement = vcor.mem.Layers(_selections.MemoryLocation.layer).Item(_selections.MemoryLocation.obj)
    '        Windows.Forms.Clipboard.SetData("vcimg", obj)
    '        vcor.mem.Layers(_selections.MemoryLocation.layer).Item.RemoveAt(_selections.MemoryLocation.obj)
    '        _selections.isEmty = True
    '        Me.Refresh()
    '    End If
    'End Sub

    'Public Sub Copy()
    '    If (Not Selection.isEmty) Then
    '        Dim obj As PathElement = vcor.mem.Layers(_selections.MemoryLocation.layer).Item(_selections.MemoryLocation.obj)
    '        Windows.Forms.Clipboard.SetData("vcimg", obj)
    '    End If

    'End Sub

    'Public Sub Past()

    '    If Windows.Forms.Clipboard.ContainsData("vcimg") Then
    '        Dim path As PathElement = Windows.Forms.Clipboard.GetData("vcimg")
    '        vcor.Memory.Layers(0).Item.Add(path)
    '        Me.Refresh()
    '    End If

    'End Sub

    'Public Sub Delete()
    '    If Not Selection.isEmty Then
    '        vcor.mem.Layers(_selections.MemoryLocation.layer).Item.RemoveAt(_selections.MemoryLocation.obj)
    '        _selections.isEmty = True
    '        Me.Refresh()
    '    End If

    'End Sub

    'Public Sub ClearAll()
    '    For Each Layer As Layer In vcor.mem.Layers
    '        Layer.Item.Clear()
    '    Next
    '    _selections.isEmty = True
    '    Me.Refresh()
    'End Sub

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

