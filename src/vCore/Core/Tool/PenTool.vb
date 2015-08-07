Imports System.Drawing




Public Class PenTool
    Implements Itool, Iedtr



    Dim core As vCore
    Dim WithEvents dc As advancedPanel
    Dim BufferGraphics As BufferPaint
    Dim MouseLocation As Point
    Dim Path As GPath
    Dim PathFigure As SubPath
    Dim startnode As PathPoint
    Dim CurrentNode As PathPoint
    Dim DrawingStarted As Boolean = False

    Dim noderadious As Single = 3


    Public Sub New(ByRef core As vCore)
        Me.core = core
        BufferGraphics = core.View.BufferGraphics
    End Sub


#Region "Tool Methode"
    Public ReadOnly Property Device() As advancedPanel Implements Itool.Device
        Get
            Return dc
        End Get
    End Property

    Private Sub dc_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dc.MouseDown
        Me.mouse_Down(e)
    End Sub

    Private Sub dc_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dc.MouseMove
        Me.mouse_Move(e)
    End Sub
    Private Sub dc_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dc.MouseUp
        Me.mouse_Up(e)
    End Sub

    Public Sub DeSelectTool() Implements Itool.DeSelectTool
        dc = Nothing
    End Sub

    Public Sub SelectTool(ByRef d As advancedPanel) Implements Itool.SelectTool
        dc = d
        core.Editor.setIEdit(Me)
        initCurrentPath()
    End Sub


#End Region



    Public Sub Draw(ByRef g As Drawing.Graphics) Implements Iedtr.Draw

    End Sub

    Public Sub mouse_Down(ByRef e As Windows.Forms.MouseEventArgs) Implements Iedtr.mouse_Down

        If Not DrawingStarted Then
            MouseLocation = e.Location
            CurrentNode = New PathPoint(MouseLocation)
            PathFigure.Points.Add(CurrentNode)
            startnode = CurrentNode
            DrawingStarted = True
            BufferGraphics.Initialize()
        Else
            Dim nodeBound = getNodeptBound(startnode.M)
            If nodeBound.Contains(e.Location) Then
                PathFigure.Points.Remove(CurrentNode)
                PathFigure.Closed = True
                core.View.Dc2MemGPath(Path)
                Dim vpath As New vPath()
                vpath.setPath(Path)
                core.View.Memory.Layers(0).Item.Add(vpath)

                DrawingStarted = False
                CurrentNode = Nothing
                startnode = Nothing
                initCurrentPath()

                core.View.Refresh()
            End If
        End If

    End Sub

    Public Sub mouse_Move(ByRef e As Windows.Forms.MouseEventArgs) Implements Iedtr.mouse_Move
        If DrawingStarted Then
            If e.Button = Windows.Forms.MouseButtons.Left Then

                BufferGraphics.Clear()
                Dim ml = e.Location

                CurrentNode.C2 = e.Location
                Dim m = CurrentNode.M

                Dim v1 = New PointF(ml.X - m.X,
                                  ml.Y - m.Y)
                CurrentNode.C1 = New PointF(-v1.X + m.X, -v1.Y + m.Y)

                CurrentNode.Type = PathPointType.Smooth
                Path.drawPath(BufferGraphics.Graphics, Pens.Magenta)

                BufferGraphics.Graphics.FillEllipse(Brushes.White,
                                                   startnode.M.X - noderadious, startnode.M.Y - noderadious,
                                                   noderadious * 2, noderadious * 2)
                BufferGraphics.Graphics.DrawEllipse(Pens.DarkMagenta,
                                                  startnode.M.X - noderadious, startnode.M.Y - noderadious,
                                                  noderadious * 2, noderadious * 2)

                BufferGraphics.Graphics.DrawLine(Pens.DarkMagenta, m.X, m.Y, ml.X, ml.Y)
                BufferGraphics.Graphics.DrawLine(Pens.DarkMagenta, m.X, m.Y,
                                                 CurrentNode.C1.X, CurrentNode.C1.Y)

                BufferGraphics.Graphics.FillEllipse(Brushes.DarkMagenta,
                                                    m.X - noderadious, m.Y - noderadious,
                                                    noderadious * 2, noderadious * 2)
                BufferGraphics.Graphics.FillEllipse(Brushes.DarkMagenta,
                                                   CurrentNode.C1.X - noderadious,
                                                   CurrentNode.C1.Y - noderadious,
                                                   noderadious * 2, noderadious * 2)
                BufferGraphics.Graphics.FillEllipse(Brushes.DarkMagenta,
                                                   ml.X - noderadious, ml.Y - noderadious,
                                                   noderadious * 2, noderadious * 2)
                BufferGraphics.Render()

            ElseIf e.Button = Windows.Forms.MouseButtons.None Then
                BufferGraphics.Clear()
                CurrentNode.M = e.Location
                CurrentNode.C1 = e.Location
                CurrentNode.C2 = e.Location

                Path.drawPath(BufferGraphics.Graphics, Pens.Magenta)

                BufferGraphics.Graphics.FillEllipse(Brushes.White,
                                                 startnode.M.X - noderadious, startnode.M.Y - noderadious,
                                                 noderadious * 2, noderadious * 2)
                BufferGraphics.Graphics.DrawEllipse(Pens.DarkMagenta,
                                                  startnode.M.X - noderadious, startnode.M.Y - noderadious,
                                                  noderadious * 2, noderadious * 2)

                BufferGraphics.Render()
            End If
        End If


    End Sub

    Public Sub mouse_Up(ByRef e As Windows.Forms.MouseEventArgs) Implements Iedtr.mouse_Up
        If DrawingStarted Then
            CurrentNode = New PathPoint(e.Location)
            PathFigure.Points.Add(CurrentNode)
        End If

    End Sub



    Private Sub initCurrentPath()
        Path = New GPath
        PathFigure = New SubPath
        Path.AddSubPath(PathFigure)
    End Sub

    Private Function getNodeptBound(pt As PointF) As Rectangle
        Dim l = New Point(pt.X - Me.noderadious, pt.Y - Me.noderadious)
        Dim w = Me.noderadious * 2
        Return New Rectangle(l, New Size(w, w))
    End Function
End Class
