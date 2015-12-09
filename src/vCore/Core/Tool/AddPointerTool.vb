Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports Geom.Geometry



Public Class AddPointerTool
    Implements Itool, Iedtr


    Dim Core As vCore
    Dim WithEvents dc As advancedPanel

    Dim spath As vPath
    Dim editablepath As GPath
    Dim noderadious As Single = 3

    Public Sub New(ByRef vcore As vCore)
        Core = vcore
    End Sub

    Public Sub DeSelectTool() Implements Itool.DeSelectTool
        dc = Nothing
    End Sub

    Public ReadOnly Property Device As advancedPanel Implements Itool.Device
        Get
            Return dc
        End Get
    End Property

    Public Sub SelectTool(ByRef d As advancedPanel) Implements Itool.SelectTool
        dc = d
        Core.Editor.setIEdit(Me)
    End Sub


    Private Sub dc_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dc.MouseDown

        Me.mouse_Down(e)
    End Sub

    Private Sub dc_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dc.MouseMove

        Me.mouse_Move(e)
    End Sub
    Private Sub dc_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dc.MouseUp

        Me.mouse_Up(e)
    End Sub

    Public Sub Draw(ByRef g As Drawing.Graphics) Implements Iedtr.Draw
        If Core.Editor.selection.isEmty = False Then
            g.SmoothingMode = SmoothingMode.AntiAlias
            Using p As New Pen(Color.SkyBlue)
                spath = Core.Editor.getSelectionPath()
                editablepath = spath.GraphicsPath.Clone
                Core.View.mem2DcGPath(editablepath)
                editablepath.drawPath(g, p)
                DrawNodes(g)
            End Using
        End If
    End Sub

    Public Sub mouse_Down(ByRef e As Windows.Forms.MouseEventArgs) Implements Iedtr.mouse_Down
        If Core.Editor.selection.isEmty Then Exit Sub

        Dim mouseLocation = e.Location

        Dim Mindistance = Double.MaxValue
        Dim n1 As PathPoint = Nothing
        Dim n2 As PathPoint = Nothing
        Dim fig As SubPath = Nothing

        For Each figure As SubPath In editablepath.subpaths
            If figure.Points.Count <= 1 Then Continue For
            For nodeIndex As Integer = 0 To figure.Points.Count - 1
                Dim node1, node2 As PathPoint
                If nodeIndex = figure.Points.Count - 1 Then
                    If figure.Closed Then
                        node1 = figure.Points(nodeIndex)
                        node2 = figure.Points(0)
                    Else
                        Continue For
                    End If
                Else
                    node1 = figure.Points(nodeIndex)
                    node2 = figure.Points(nodeIndex + 1)
                End If


                Dim bez As New GCubicBezier()
                bez.P1 = New GPoint(node1.M.X, node1.M.Y)
                bez.C1 = New GPoint(node1.C2.X, node1.C2.Y)

                bez.C2 = New GPoint(node2.C1.X, node2.C1.Y)
                bez.P2 = New GPoint(node2.M.X, node2.M.Y)

                Dim dist = bez.DistancefromPoint(New GPoint(mouseLocation.X, mouseLocation.Y))

                If dist < Mindistance Then
                    Mindistance = dist
                    n1 = node1
                    n2 = node2
                    fig = figure
                End If
            Next
        Next

        If Mindistance <= 2 Then
            Dim pointerIndex = fig.Points.IndexOf(n1)
            Dim nPointer As New PathPoint

            Dim bez As New GCubicBezier()
            bez.P1 = New GPoint(n1.M.X, n1.M.Y)
            bez.C1 = New GPoint(n1.C2.X, n1.C2.Y)
            bez.C2 = New GPoint(n2.C1.X, n2.C1.Y)
            bez.P2 = New GPoint(n2.M.X, n2.M.Y)

            Dim closept = bez.closestPointToBezier(New GPoint(mouseLocation.X, mouseLocation.Y))
            Dim left = bez.divLeft(closept)
            Dim right = bez.divRight(closept)

            n1.C2 = New PointF(left.C1.X, left.C1.Y)
            nPointer.C1 = New PointF(left.C2.X, left.C2.Y)
            nPointer.M = New PointF(left.P2.X, left.P2.Y)
            nPointer.C2 = New PointF(right.C1.X, right.C1.Y)
            n2.C1 = New PointF(right.C2.X, right.C2.Y)

            nPointer.Type = PathPointType.Smooth

            fig.Points.Insert(pointerIndex + 1, nPointer)

            Core.View.Dc2MemGPath(editablepath)
            spath.setPath(editablepath)
            Core.View.Refresh()
        End If

    End Sub

    Public Sub mouse_Move(ByRef e As Windows.Forms.MouseEventArgs) Implements Iedtr.mouse_Move

    End Sub

    Public Sub mouse_Up(ByRef e As Windows.Forms.MouseEventArgs) Implements Iedtr.mouse_Up

    End Sub


    Private Sub DrawNodes(g As System.Drawing.Graphics)
        Using Pen As New Pen(Brushes.Red, 1)
            For Each sp As SubPath In editablepath.subpaths
                For Each nd As PathPoint In sp.Points
                    ' g.FillEllipse(Brushes.White, nd.M.X - noderadious, nd.M.Y - noderadious, noderadious * 2, noderadious * 2)
                    ' g.DrawEllipse(Pen, nd.M.X - noderadious, nd.M.Y - noderadious, noderadious * 2, noderadious * 2)
                    g.FillRectangle(Brushes.White, nd.M.X - noderadious, nd.M.Y - noderadious, noderadious * 2, noderadious * 2)
                    g.DrawRectangle(Pen, nd.M.X - noderadious, nd.M.Y - noderadious, noderadious * 2, noderadious * 2)
                Next
            Next
        End Using
    End Sub

    Private Function getNodeptBound(pt As PointF) As Rectangle
        Dim l = New Point(pt.X - Me.noderadious, pt.Y - Me.noderadious)
        Dim w = Me.noderadious * 2
        Return New Rectangle(l, New Size(w, w))
    End Function
End Class
