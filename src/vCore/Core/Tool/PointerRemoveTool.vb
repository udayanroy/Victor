Imports System.Drawing
Imports System.Drawing.Drawing2D



Public Class PointerRemoveTool
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
        
        Dim selNode As PathPoint = Nothing
        Dim selFigure As SubPath = Nothing

        For Each subpth As SubPath In editablepath.subpaths
            For Each nd As PathPoint In subpth.Points
                Dim bnd = getNodeptBound(nd.M)
                If bnd.Contains(e.Location) Then
                    selNode = nd
                    selFigure = subpth
                    GoTo EndLoop
                End If
            Next
        Next

EndLoop:
        If selNode IsNot Nothing Then
            selFigure.Points.Remove(selNode)

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
