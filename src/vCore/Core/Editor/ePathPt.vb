Imports System.Drawing
Imports System.Drawing.Drawing2D

Public Class ePathPt
    Implements Iedtr

    Private b As Integer = 3

    Dim v As vEditor

    Dim mainpathBound As RectangleF

    Public Sub New(ByRef edtr As vEditor)

        v = edtr
    End Sub

    Public Sub Draw(ByRef g As System.Drawing.Graphics) Implements Iedtr.Draw
        If v.selection.isEmty = False Then

            Using p As New Pen(Color.SkyBlue), pth As GraphicsPath = v.getSelectionPath.GraphicsPath.ToGraphicsPath

                v.View.mem2DcPath(pth)

                Dim pdata = pth.PathPoints


                g.SmoothingMode = SmoothingMode.AntiAlias
                g.DrawPath(p, pth)
                Me.DrawEllipses(g, pdata)

            End Using

        End If
    End Sub

    Public Sub mouse_Down(ByRef e As System.Windows.Forms.MouseEventArgs) Implements Iedtr.mouse_Down

    End Sub

    Public Sub mouse_Move(ByRef e As System.Windows.Forms.MouseEventArgs) Implements Iedtr.mouse_Move

    End Sub

    Public Sub mouse_Up(ByRef e As System.Windows.Forms.MouseEventArgs) Implements Iedtr.mouse_Up

    End Sub
    Private Sub DrawEllipses(ByVal g As Graphics, ByVal pts() As PointF)
        Dim wh As Integer = b * 2
        For Each pnt As PointF In pts
            Dim rect As New Rectangle(pnt.X - b, pnt.Y - b, wh, wh)
            g.FillEllipse(Brushes.Brown, rect)
        Next
    End Sub
End Class
