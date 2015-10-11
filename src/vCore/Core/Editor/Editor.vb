Imports System.Drawing
Imports System.Drawing.Drawing2D

Public Class Editor


    Dim vcor As vCore
    Dim slct As Selection
    Dim iedt As Iedtr

    Dim move As eMove
    Dim size As eSize
    Dim rotate As eRotate
    Dim ptconvert As ePointerConvert

    Dim type As selectionType

    Dim _fillcolor As Color = Color.Blue
    Dim _strokecolor As Color = Color.Black
    Dim _strokewidth As Single = 1

    Public Event PropertyChanged()
    Public Event SelectionChanged()

    Public Sub New(ByRef v As vCore)

        vcor = v
        slct = New Selection
        move = New eMove(Me)
        size = New eSize(Me)
        rotate = New eRotate(Me)
        ptconvert = New ePointerConvert(Me)

        type = SelectionType.None

    End Sub
    Public Function HittestAt(ByVal p As Point) As Selection
        Dim retn As New Selection()

        Dim memloc As memLoc
        Dim flage As Boolean = False

        Dim mp = vcor.View.Dc2memPt(p)

        Dim len, lobj As Integer
        len = vcor.View.Memory.Layers.Count

        For l As Integer = 0 To len - 1
            lobj = vcor.View.Memory.Layers(l).Item.Count
            For k As Integer = 0 To lobj - 1
                If vcor.View.Memory.Layers(l).Item(k).HitTest(mp) Then
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
        Dim r As Integer = 0

        Dim memloc As memLoc
        Dim flage As Boolean = False

        'Dim m As PointF = vcor.View.DCpointToMemory(p)
        'Dim mp As New PointF(m.X - vcor.View.getpagerctOrg.X, m.Y - vcor.View.getpagerctOrg.Y)
        Dim mp = vcor.View.Dc2memPt(p)

        Dim len, lobj As Integer
        len = vcor.View.Memory.Layers.Count

        For l As Integer = 0 To len - 1
            lobj = vcor.View.Memory.Layers(l).Item.Count
            For k As Integer = 0 To lobj - 1
                If vcor.View.Memory.Layers(l).Item(k).HitTest(mp) Then
                    memloc.create(l, k)

                    flage = True
                End If
            Next
        Next
        If flage = True Then
            If memloc.Equals(slct.MemoryLocation) Then
                r = 1
            Else
                r = 2
            End If
            slct.MemoryLocation = memloc
            slct.isEmty = False
            Me.OnSelectionChanged()

        Else
            slct.isEmty = True
            r = 0
        End If

        RaiseEvent SelectionChanged()
        Return r

    End Function

    Private Sub OnSelectionChanged()
        Dim path = Me.getSelectionPath
        Me._fillcolor = path.FillColor
        Me._strokecolor = path.StrokeColor
        Me._strokewidth = path.StrokWidth
        RaiseEvent PropertyChanged()
    End Sub

    Public Sub DisSelect()
        slct.isEmty = True
    End Sub

    Public ReadOnly Property selection() As Selection
        Get
            Return Me.slct
        End Get
    End Property

    Public ReadOnly Property View As View
        Get
            Return vcor.View
        End Get

    End Property
    Public Sub Refresh()
        vcor.View.Refresh()
    End Sub

    Public Property EditingType() As selectionType
        Get
            Return Me.type
        End Get
        Set(ByVal value As selectionType)

            Select Case value
                Case selectionType.None
                    Me.iedt = Nothing
                Case selectionType.Move
                    Me.iedt = move
            End Select
        End Set
    End Property
    Public Sub setEditingType(ByVal typ As selectionType)


        Select Case typ
            Case selectionType.None
                Me.iedt = Nothing
            Case selectionType.Move
                Me.iedt = move
            Case selectionType.Size
                Me.iedt = size
            Case selectionType.Rotate
                Me.iedt = rotate
            Case selectionType.PointerConvert
                Me.iedt = ptconvert
        End Select
        type = typ

        Refresh()
    End Sub
    Public Sub setIEdit(ByVal Editor As Iedtr)

        Me.iedt = Editor
        type = selectionType.other
        Refresh()

    End Sub

    Public Function getSelectionPath() As vPath
        Return vcor.mem.Layers(slct.MemoryLocation.layer).Item(slct.MemoryLocation.obj)
    End Function
    Public Sub paint(ByVal g As Graphics)

        If Me.type <> selectionType.None Then
            Me.iedt.Draw(g)
        End If

    End Sub
    Public Sub mouse_Down(ByRef e As System.Windows.Forms.MouseEventArgs)
        If Me.type <> selectionType.None Then
            Me.iedt.mouse_Down(e)
        End If
    End Sub
    Public Sub mouse_Move(ByRef e As System.Windows.Forms.MouseEventArgs)
        If Me.type <> selectionType.None Then
            Me.iedt.mouse_Move(e)
        End If
    End Sub
    Public Sub mouse_Up(ByRef e As System.Windows.Forms.MouseEventArgs)
        If Me.type <> selectionType.None Then
            Me.iedt.mouse_Up(e)
        End If
    End Sub
    Public Function getBoundRect() As RectangleF
        Return vcor.mem.Layers(slct.MemoryLocation.layer).Item(slct.MemoryLocation.obj).GetBound
    End Function

    Public Sub Cut()
        If (Not selection.isEmty) Then
            Dim obj As vPath = vcor.mem.Layers(slct.MemoryLocation.layer).Item(slct.MemoryLocation.obj)
            Windows.Forms.Clipboard.SetData("vcimg", obj)
            vcor.mem.Layers(slct.MemoryLocation.layer).Item.RemoveAt(slct.MemoryLocation.obj)
            slct.isEmty = True
            Me.Refresh()
        End If
    End Sub

    Public Sub Copy()
        If (Not selection.isEmty) Then
            Dim obj As vPath = vcor.mem.Layers(slct.MemoryLocation.layer).Item(slct.MemoryLocation.obj)
            Windows.Forms.Clipboard.SetData("vcimg", obj)
        End If

    End Sub

    Public Sub Past()

        If Windows.Forms.Clipboard.ContainsData("vcimg") Then
            Dim path As vPath = Windows.Forms.Clipboard.GetData("vcimg")
            View.Memory.Layers(0).Item.Add(path)
            Me.Refresh()
        End If

    End Sub

    Public Sub Delete()
        If Not selection.isEmty Then
            vcor.mem.Layers(slct.MemoryLocation.layer).Item.RemoveAt(slct.MemoryLocation.obj)
            slct.isEmty = True
            Me.Refresh()
        End If

    End Sub

    Public Sub ClearAll()
        For Each Layer As Layer In vcor.mem.Layers
            Layer.Item.Clear()
        Next
        slct.isEmty = True
        Me.Refresh()
    End Sub

    Public Property FillColor As Color
        Get
            Return Me._fillcolor
        End Get
        Set(value As Color)
            Me._fillcolor = value
            If Not Me.selection.isEmty Then
                Dim path = Me.getSelectionPath
                path.FillColor = value
                Me.Refresh()
            End If
        End Set
    End Property

    Public Property StrokeColor As Color
        Get
            Return Me._strokecolor
        End Get
        Set(value As Color)
            Me._strokecolor = value

            If Not Me.selection.isEmty Then
                Dim path = Me.getSelectionPath
                path.StrokeColor = value
                Me.Refresh()
            End If
        End Set
    End Property

    Public Property strokeWidth As Single
        Get
            Return Me._strokewidth
        End Get
        Set(value As Single)
            Me._strokewidth = value
            If Not Me.selection.isEmty Then
                Dim path = Me.getSelectionPath
                path.StrokWidth = value
                Me.Refresh()
            End If
        End Set
    End Property

  
End Class

