Imports System.Drawing.Drawing2D
Imports System.Drawing


Public Class ePointerConvert
    Implements Iedtr

    Dim v As vEditor
    Dim spath As vPath
    Dim editablepath As GPath
    Dim noderadious As Single = 3

    Dim nodesel As nodeselection

    Public Sub New(ByRef edtr As vEditor)
        v = edtr
    End Sub

    Public Sub Draw(ByRef g As Graphics) Implements Iedtr.Draw
        If v.selection.isEmty = False Then
            g.SmoothingMode = SmoothingMode.AntiAlias
            Using p As New Pen(Color.SkyBlue)
                spath = v.getSelectionPath()
                editablepath = spath.GraphicsPath.Clone
                v.View.mem2DcGPath(editablepath)

                editablepath.drawPath(g, p)

                DrawNodes(g)

            End Using

        End If
    End Sub

    Public Sub mouse_Down(ByRef e As Windows.Forms.MouseEventArgs) Implements Iedtr.mouse_Down
        Dim md = e.Location
        nodesel = GetSelectPoint(md)
        If nodesel.selectednode IsNot Nothing Then
            v.View.BufferGraphics.Initialize()

            If nodesel.typeid = 1 Then
                nodesel.selectednode.Type = PathPointType.None
                nodesel.selectednode.C1 = nodesel.selectednode.M
                nodesel.selectednode.C2 = nodesel.selectednode.M
                v.View.BufferGraphics.Clear()
                editablepath.drawPath(v.View.BufferGraphics.Graphics, Pens.Black)
                DrawNodes(v.View.BufferGraphics.Graphics)
                v.View.BufferGraphics.Render()
            End If

        End If

    End Sub

    Public Sub mouse_Move(ByRef e As Windows.Forms.MouseEventArgs) Implements Iedtr.mouse_Move
        If nodesel.selectednode Is Nothing Then Exit Sub

        If e.Button = Windows.Forms.MouseButtons.Left Then
            Dim ml = e.Location

            If nodesel.typeid = 1 Then
                nodesel.selectednode.Type = PathPointType.Smooth
                nodesel.selectednode.C2 = ml

                Dim m = nodesel.selectednode.M

                Dim v1 = New PointF(ml.X - m.X,
                                  ml.Y - m.Y)
                nodesel.selectednode.C1 = New PointF(-v1.X + m.X, -v1.Y + m.Y)

            ElseIf nodesel.typeid = 2 Then
                nodesel.selectednode.Type = PathPointType.Sharp
                nodesel.selectednode.C1 = ml

            ElseIf nodesel.typeid = 3 Then
                nodesel.selectednode.Type = PathPointType.Sharp
                nodesel.selectednode.C2 = ml
            End If


            v.View.BufferGraphics.Clear()
            editablepath.drawPath(v.View.BufferGraphics.Graphics, Pens.Black)
            DrawNodes(v.View.BufferGraphics.Graphics)
            v.View.BufferGraphics.Render()
        End If

    End Sub

    Public Sub mouse_Up(ByRef e As Windows.Forms.MouseEventArgs) Implements Iedtr.mouse_Up

        v.View.Dc2MemGPath(editablepath)
        spath.setPath(editablepath)

        v.View.Refresh()
    End Sub


    Private Sub DrawNodes(g As System.Drawing.Graphics)
        For Each sp As SubPath In editablepath.subpaths
            For Each nd As PathPoint In sp.Points
                If nd.Type = PathPointType.None Then
                    g.FillEllipse(Brushes.Red, nd.M.X - noderadious, nd.M.Y - noderadious, noderadious * 2, noderadious * 2)

                Else
                    g.DrawLine(New Pen(Brushes.Red, 1), nd.M, nd.C1)
                    g.DrawLine(New Pen(Brushes.Red, 1), nd.M, nd.C2)

                    g.FillEllipse(Brushes.Red, nd.M.X - noderadious, nd.M.Y - noderadious, noderadious * 2, noderadious * 2)

                    g.DrawEllipse(New Pen(Brushes.Red, 2), nd.C1.X - noderadious, nd.C1.Y - noderadious, noderadious * 2, noderadious * 2)
                    g.FillEllipse(Brushes.White, nd.C1.X - noderadious, nd.C1.Y - noderadious, noderadious * 2, noderadious * 2)
                    g.DrawEllipse(New Pen(Brushes.Red, 2), nd.C2.X - noderadious, nd.C2.Y - noderadious, noderadious * 2, noderadious * 2)
                    g.FillEllipse(Brushes.White, nd.C2.X - noderadious, nd.C2.Y - noderadious, noderadious * 2, noderadious * 2)

                End If

            Next
        Next

    End Sub


    Private Function GetSelectPoint(location As Point) As nodeselection
        Dim nds As New nodeselection

        Dim ndIndex = 0
        Dim figIndex = 0

        For Each subpth As SubPath In editablepath.subpaths
            ndIndex = 0
            For Each nd As PathPoint In subpth.Points
                Dim bnd As Rectangle

                If nd.Type <> PathPointType.None Then
                    bnd = getNodeptBound(nd.C2)
                    If bnd.Contains(location) Then
                        nds.selectednode = nd
                        nds.typeid = 3
                        nds.nodeIndex = ndIndex
                        nds.figureIndex = figIndex
                        Return nds
                    End If

                    bnd = getNodeptBound(nd.C1)
                    If bnd.Contains(location) Then
                        nds.selectednode = nd
                        nds.typeid = 2
                        nds.nodeIndex = ndIndex
                        nds.figureIndex = figIndex
                        Return nds
                    End If
                End If

                bnd = getNodeptBound(nd.M)
                If bnd.Contains(location) Then
                    nds.selectednode = nd
                    nds.typeid = 1
                    nds.nodeIndex = ndIndex
                    nds.figureIndex = figIndex
                    Return nds
                End If
                ndIndex += 1
            Next
            figIndex += 1
        Next

        Return nds
    End Function

    Private Function getNodeptBound(pt As PointF) As Rectangle
        Dim l = New Point(pt.X - Me.noderadious, pt.Y - Me.noderadious)
        Dim w = Me.noderadious * 2
        Return New Rectangle(l, New Size(w, w))
    End Function

    Private Structure nodeselection
        Public selectednode As PathPoint
        Public figureIndex As Integer
        Public nodeIndex As Integer
        Public typeid As Integer
    End Structure
End Class
