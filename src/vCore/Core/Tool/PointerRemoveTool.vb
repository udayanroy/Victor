Imports Geometry
Imports Graphics



Public Class PointerRemoveTool
    Implements Itool, IEditor


    Dim Core As vCore
    Dim WithEvents dc As IDevice

    Dim spath As PathElement
    Dim editablepath As NodePath
    Dim noderadious As Single = 3

    Public Sub New(ByRef vcore As vCore)
        Core = vcore
    End Sub

    Public Sub DeSelectTool() Implements Itool.DeSelectTool
        dc = Nothing
    End Sub

    Public ReadOnly Property Device As IDevice Implements Itool.Device
        Get
            Return dc
        End Get
    End Property

    Public Sub SelectTool(ByRef d As IDevice) Implements Itool.SelectTool
        dc = d
        Core.Editor.setIEdit(Me)
    End Sub



    Private Sub dc_MouseDown(e As MouseEvntArg) Handles dc.MouseDown

        Me.mouse_Down(e)
    End Sub

    Private Sub dc_MouseMove(e As MouseEvntArg) Handles dc.MouseMove

        Me.mouse_Move(e)
    End Sub
    Private Sub dc_MouseUp(e As MouseEvntArg) Handles dc.MouseUp

        Me.mouse_Up(e)
    End Sub

    Public Sub Draw(g As Canvas) Implements IEditor.Draw
        If Core.Editor.selection.isEmty = False Then
            g.Smooth()
            Dim p As New Pen(Color.SkyBlueColor)
            spath = Core.Editor.getSelectionPath()
            editablepath = spath.Path.Clone
            Core.View.Memory2screen(editablepath)
            g.DrawPath(editablepath, p)
            DrawNodes(g)

        End If
    End Sub

    Public Sub mouse_Down(e As MouseEvntArg)
        If Core.Editor.selection.isEmty Then Exit Sub

        Dim selNode As Node = Nothing
        Dim selFigure As NodeFigure = Nothing

        For Each subpth As NodeFigure In editablepath.Figures
            For Each nd As Node In subpth.Points
                Dim bnd = getNodeptBound(nd.M)
                If bnd.Contain(e.Location) Then
                    selNode = nd
                    selFigure = subpth
                    GoTo EndLoop
                End If
            Next
        Next

EndLoop:
        If selNode IsNot Nothing Then
            selFigure.Points.Remove(selNode)

            Core.View.Screen2memory(editablepath)
            spath.setPath(editablepath)
            Core.View.Refresh()
        End If


    End Sub

    Public Sub mouse_Move(e As MouseEvntArg)

    End Sub

    Public Sub mouse_Up(e As MouseEvntArg)

    End Sub


    Private Sub DrawNodes(g As Canvas)
        Dim Pen As New Pen(Color.RedColor, 1)
        For Each sp As NodeFigure In editablepath.Figures
            For Each nd As Node In sp.Points
                ' g.FillEllipse(Brushes.White, nd.M.X - noderadious, nd.M.Y - noderadious, noderadious * 2, noderadious * 2)
                ' g.DrawEllipse(Pen, nd.M.X - noderadious, nd.M.Y - noderadious, noderadious * 2, noderadious * 2)

                g.DrawRect(New Rect(New Point(nd.M.X - noderadious, nd.M.Y - noderadious), noderadious * 2, noderadious * 2),
                    Pen, New SolidColorBrush(Color.WhiteColor))
            Next
        Next

    End Sub

    Private Function getNodeptBound(pt As Point) As Rect
        Dim l = New Point(pt.X - Me.noderadious, pt.Y - Me.noderadious)
        Dim w = Me.noderadious * 2
        Return New Rect(l, New Size(w, w))
    End Function
End Class
