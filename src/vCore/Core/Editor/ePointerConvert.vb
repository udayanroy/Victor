Imports System.Drawing.Drawing2D
Imports System.Drawing


Public Class ePointerConvert
    Implements Iedtr

    Dim v As vEditor
    Dim spath As vPath
    Dim editablepath As GPath
    Dim noderadious As Single = 3


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

    End Sub

    Public Sub mouse_Move(ByRef e As Windows.Forms.MouseEventArgs) Implements Iedtr.mouse_Move

    End Sub

    Public Sub mouse_Up(ByRef e As Windows.Forms.MouseEventArgs) Implements Iedtr.mouse_Up

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

End Class
