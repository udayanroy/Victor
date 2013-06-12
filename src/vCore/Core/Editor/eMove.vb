Imports System.Drawing.Drawing2D
Imports System.Drawing

Public Class eMove
    Implements Iedtr

    Dim v As vEditor
    Dim mdl As Point
    Dim md As Point

    Dim s As Integer = 0
    Dim svp As GraphicsPath

    Public Sub New(ByRef edtr As vEditor)
        v = edtr
    End Sub

    Public Sub Draw(ByRef g As System.Drawing.Graphics) Implements Iedtr.Draw
        If v.selection.isEmty = False Then

            Using p As New Pen(Color.Red), pth As New GraphicsPath

                Dim rf As RectangleF = v.getBoundRect()
                pth.AddRectangle(rf)
                v.View.mem2DcPath(pth)

                g.DrawPath(p, pth)
            End Using

        End If
    End Sub

    Public Sub mouse_Down(ByRef e As System.Windows.Forms.MouseEventArgs) Implements Iedtr.mouse_Down
        md = e.Location
        mdl = e.Location
        s = v.SelectAt(mdl)

        If s <> 0 Then
            svp = v.getSelectionPath.GraphicsPath.Clone
            v.View.mem2DcPath(svp)
            v.View.BufferGraphics.Initialize()
        End If
    End Sub

    Public Sub mouse_Move(ByRef e As System.Windows.Forms.MouseEventArgs) Implements Iedtr.mouse_Move
        If e.Button = Windows.Forms.MouseButtons.Left Then
            If s <> 0 Then
                v.View.BufferGraphics.Clear()
               
                Using mat As New Matrix
                    mat.Translate((e.Location.X - mdl.X), (e.Location.Y - mdl.Y))
                    svp.Transform(mat)
                End Using

                v.View.BufferGraphics.Graphics.DrawPath(Pens.Black, svp)
                v.View.BufferGraphics.Render()

                mdl = e.Location
            End If
        End If
    End Sub

    Public Sub mouse_Up(ByRef e As System.Windows.Forms.MouseEventArgs) Implements Iedtr.mouse_Up
        If s <> 0 Then
            Dim p1 = v.View.Dc2memPt(md)
            Dim p2 = v.View.Dc2memPt(e.Location)
            v.getSelectionPath.Translate(p2.X - p1.X, p2.Y - p1.Y)

            svp.Dispose()
        End If
        v.View.Refresh()
    End Sub


End Class
