Imports Geometry
Imports Graphics




Public Class PenTool
    Implements Itool, IEditor



    Dim core As vCore
    Dim WithEvents dc As IDevice
    Dim BufferGraphics As BufferPaint
    Dim MouseLocation As Point
    Dim Path As NodePath
    Dim PathFigure As NodeFigure
    Dim startnode As Node
    Dim CurrentNode As Node
    Dim DrawingStarted As Boolean = False

    Dim noderadious As Single = 3


    Public Sub New(ByRef core As vCore)
        Me.core = core
        BufferGraphics = core.View.BufferGraphics
    End Sub


#Region "Tool Methode"
    Public ReadOnly Property Device() As IDevice Implements Itool.Device
        Get
            Return dc
        End Get
    End Property

    Private Sub dc_MouseDown(e As MouseEvntArg) Handles dc.MouseDown
        Me.mouse_Down(e)
    End Sub

    Private Sub dc_MouseMove(e As MouseEvntArg) Handles dc.MouseMove
        Me.mouse_Move(e)
    End Sub
    Private Sub dc_MouseUp(e As MouseEvntArg) Handles dc.MouseUp
        Me.mouse_Up(e)
    End Sub

    Public Sub DeSelectTool() Implements Itool.DeSelectTool
        dc = Nothing
    End Sub

    Public Sub SelectTool(ByRef d As IDevice) Implements Itool.SelectTool
        dc = d
        core.Editor.setIEdit(Me)
        initCurrentPath()
    End Sub


#End Region



    Public Sub Draw(g As Canvas) Implements IEditor.Draw

    End Sub

    Public Sub mouse_Down(e As MouseEvntArg)

        If Not DrawingStarted Then
            MouseLocation = e.Location
            CurrentNode = New Node(MouseLocation)
            PathFigure.Points.Add(CurrentNode)
            startnode = CurrentNode
            DrawingStarted = True
            BufferGraphics.Initialize()
            dc.ActiveScroll = False
        Else
            Dim nodeBound = getNodeptBound(startnode.M)
            If nodeBound.Contain(e.Location) Then
                PathFigure.Points.Remove(CurrentNode)
                PathFigure.Closed = True
                core.View.Screen2memory(Path)
                Dim vpath As New PathElement()
                vpath.setPath(Path)
                core.Memory.Layers(0).Item.Add(vpath)

                'Add Active styles
                Dim edtr = core.Editor
                'vpath.FillColor = edtr.FillColor
                'vpath.StrokeColor = edtr.StrokeColor
                'vpath.StrokWidth = edtr.strokeWidth
                'vpath.isFill = edtr.isFill
                'vpath.isStroke = edtr.isStroke

                'Set global states
                DrawingStarted = False
                CurrentNode = Nothing
                startnode = Nothing

                dc.ActiveScroll = True

                initCurrentPath()

                core.View.Refresh()
            End If
        End If

    End Sub

    Public Sub mouse_Move(e As MouseEvntArg)
        If DrawingStarted Then
            If e.Button = Windows.Forms.MouseButtons.Left Then

                BufferGraphics.Clear()
                Dim ml = e.Location

                CurrentNode.C2 = e.Location
                Dim m = CurrentNode.M

                Dim v1 = New Point(ml.X - m.X,
                                  ml.Y - m.Y)
                CurrentNode.C1 = New Point(-v1.X + m.X, -v1.Y + m.Y)

                CurrentNode.Type = NodeType.Smooth
                BufferGraphics.Graphics.DrawPath(Path, New Pen(Color.MagentaColor))

                BufferGraphics.Graphics.DrawEllipse(New Rect(New Point(startnode.M.X - noderadious, startnode.M.Y - noderadious),
                                                   noderadious * 2, noderadious * 2), , New SolidColorBrush(Color.WhiteColor))
                BufferGraphics.Graphics.DrawEllipse(New Rect(New Point(startnode.M.X - noderadious, startnode.M.Y - noderadious),
                                                  noderadious * 2, noderadious * 2), New Pen(Color.DarkMagentaColor))

                BufferGraphics.Graphics.DrawLine(m, ml, New Pen(Color.DarkMagentaColor))
                BufferGraphics.Graphics.DrawLine(m, CurrentNode.C1, New Pen(Color.DarkMagentaColor))

                BufferGraphics.Graphics.DrawEllipse(New Rect(New Point(m.X - noderadious, m.Y - noderadious),
                                                  noderadious * 2, noderadious * 2), , New SolidColorBrush(Color.DarkMagentaColor))
                BufferGraphics.Graphics.DrawEllipse(New Rect(New Point(CurrentNode.C1.X - noderadious,
                                                   CurrentNode.C1.Y - noderadious),
                                                 noderadious * 2, noderadious * 2), , New SolidColorBrush(Color.DarkMagentaColor))
                BufferGraphics.Graphics.DrawEllipse(New Rect(New Point(ml.X - noderadious, ml.Y - noderadious),
                                                 noderadious * 2, noderadious * 2), , New SolidColorBrush(Color.DarkMagentaColor))
 
                BufferGraphics.Render()

            ElseIf e.Button = Windows.Forms.MouseButtons.None Then
                BufferGraphics.Clear()
                CurrentNode.M = e.Location
                CurrentNode.C1 = e.Location
                CurrentNode.C2 = e.Location

                BufferGraphics.Graphics.DrawPath(Path, New Pen(Color.MagentaColor))

                BufferGraphics.Graphics.DrawEllipse(New Rect(New Point(startnode.M.X - noderadious, startnode.M.Y - noderadious),
                                                noderadious * 2, noderadious * 2), New Pen(Color.WhiteColor), New SolidColorBrush(Color.WhiteColor))


                BufferGraphics.Render()
            End If
        End If


    End Sub

    Public Sub mouse_Up(e As MouseEvntArg)
        If DrawingStarted Then
            CurrentNode = New Node(e.Location)
            PathFigure.Points.Add(CurrentNode)
        End If

    End Sub



    Private Sub initCurrentPath()
        Path = New NodePath
        PathFigure = New NodeFigure
        Path.Figures.Add(PathFigure)
    End Sub

    Private Function getNodeptBound(pt As Point) As Rect
        Dim l = New Point(pt.X - Me.noderadious, pt.Y - Me.noderadious)
        Dim w = Me.noderadious * 2
        Return New Rect(l, New Size(w, w))
    End Function
End Class
