Imports Geometry
Imports Graphics




Public Class PenTool
    Inherits Tool




    Dim MouseLocation As Point
    Dim Path As NodePath
    Dim PathFigure As NodeFigure
    Dim startnode As Node
    Dim CurrentNode As Node
    Dim DrawingStarted As Boolean = False

    Dim noderadious As Single = 3


    Public Sub New(ByRef core As vCore)
        MyBase.New(core)
    End Sub


    '#Region "Tool Methode"

    '    Public Sub SelectTool(ByRef d As IDevice) Implements Itool.SelectTool
    '        dc = d
    '        core.Editor.setIEdit(Me)
    '        initCurrentPath()
    '    End Sub


    '#End Region


    Protected Overrides Sub MouseDown(e As MouseEvntArg)

        If Not DrawingStarted Then
            MouseLocation = e.Location
            CurrentNode = New Node(MouseLocation)
            PathFigure.Points.Add(CurrentNode)
            startnode = CurrentNode
            DrawingStarted = True
            Visual.BufferGraphics.Initialize()
            Device.ActiveScroll = False
        Else
            Dim nodeBound = getNodeptBound(startnode.M)
            If nodeBound.Contain(e.Location) Then
                PathFigure.Points.Remove(CurrentNode)
                PathFigure.Closed = True


                'Create the Element 
                Dim pathElm As New PathElement()
                pathElm.setPath(Path)
                'Add it to Dom
                Editor.AddPathToCurrentLayer(pathElm)

                'Set global states
                DrawingStarted = False
                CurrentNode = Nothing
                startnode = Nothing

                Device.ActiveScroll = True

                initCurrentPath()

                Core.View.Refresh()
            End If
        End If

    End Sub

    Public Sub mouse_Move(e As MouseEvntArg)
        If DrawingStarted Then
            If e.Button = Windows.Forms.MouseButtons.Left Then

                Visual.BufferGraphics.Clear()
                Dim ml = e.Location

                CurrentNode.C2 = e.Location
                Dim m = CurrentNode.M

                Dim v1 = New Point(ml.X - m.X,
                                  ml.Y - m.Y)
                CurrentNode.C1 = New Point(-v1.X + m.X, -v1.Y + m.Y)

                CurrentNode.Type = NodeType.Smooth
                Visual.BufferGraphics.Graphics.DrawPath(Path, New Pen(Color.MagentaColor))

                Visual.BufferGraphics.Graphics.DrawEllipse(New Rect(New Point(startnode.M.X - noderadious, startnode.M.Y - noderadious),
                                                   noderadious * 2, noderadious * 2), , New SolidColorBrush(Color.WhiteColor))
                Visual.BufferGraphics.Graphics.DrawEllipse(New Rect(New Point(startnode.M.X - noderadious, startnode.M.Y - noderadious),
                                                  noderadious * 2, noderadious * 2), New Pen(Color.DarkMagentaColor))

                Visual.BufferGraphics.Graphics.DrawLine(m, ml, New Pen(Color.DarkMagentaColor))
                Visual.BufferGraphics.Graphics.DrawLine(m, CurrentNode.C1, New Pen(Color.DarkMagentaColor))

                Visual.BufferGraphics.Graphics.DrawEllipse(New Rect(New Point(m.X - noderadious, m.Y - noderadious),
                                                  noderadious * 2, noderadious * 2), , New SolidColorBrush(Color.DarkMagentaColor))
                Visual.BufferGraphics.Graphics.DrawEllipse(New Rect(New Point(CurrentNode.C1.X - noderadious,
                                                   CurrentNode.C1.Y - noderadious),
                                                 noderadious * 2, noderadious * 2), , New SolidColorBrush(Color.DarkMagentaColor))
                Visual.BufferGraphics.Graphics.DrawEllipse(New Rect(New Point(ml.X - noderadious, ml.Y - noderadious),
                                                 noderadious * 2, noderadious * 2), , New SolidColorBrush(Color.DarkMagentaColor))

                Visual.BufferGraphics.Render()

            ElseIf e.Button = Windows.Forms.MouseButtons.None Then
                Visual.BufferGraphics.Clear()
                CurrentNode.M = e.Location
                CurrentNode.C1 = e.Location
                CurrentNode.C2 = e.Location

                Visual.BufferGraphics.Graphics.DrawPath(Path, New Pen(Color.MagentaColor))

                Visual.BufferGraphics.Graphics.DrawEllipse(New Rect(New Point(startnode.M.X - noderadious, startnode.M.Y - noderadious),
                                                noderadious * 2, noderadious * 2), New Pen(Color.WhiteColor), New SolidColorBrush(Color.WhiteColor))


                Visual.BufferGraphics.Render()
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
