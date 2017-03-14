Imports Geometry
Imports Graphics



Public Class PointerRemoveTool
    Inherits Tool




    'Dim spath As PathElement
    Dim editablepath As NodePath
    Dim noderadious As Single = 3


    Private SelectedElements As NodePathsCapElement 'to handle transformation of selectionElements

    Public Sub New(ByRef vcore As vCore)
        MyBase.New(vcore)
        SelectedElements = New NodePathsCapElement(Editor)
    End Sub


    Public Overrides Sub Draw(g As Canvas)
        editablepath = Nothing
        If Editor.SelectionHolder.isEmpty = False Then
            g.Smooth()
            Dim p As New Pen(Color.BlueColor) ', pth As GraphicsPath = spath.GraphicsPath.ToGraphicsPath

            'spath = v.Editor.getSelectionPath()
            editablepath = SelectedElements.GetSelectionSkeliton

            g.DrawPath(editablepath, p)

            DrawNodes(g)

        End If
    End Sub

    Protected Overrides Sub MouseDown(e As MouseEvntArg)
        If Core.Editor.SelectionHolder.isEmpty Then Exit Sub

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

            'Core.View.Screen2memory(editablepath)
            'spath.setPath(editablepath)
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
