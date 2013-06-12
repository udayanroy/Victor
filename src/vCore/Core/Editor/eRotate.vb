Imports System.Drawing
Imports System.Windows.Forms
Imports System.Drawing.Drawing2D

Public Class eRotate
    Implements Iedtr

    Private b As Integer = 3
    Private wh As Integer = b * 2


    Dim v As vEditor
   

    Dim s As Integer = 0
    Dim svp As GraphicsPath
    Dim mda As Single
    Dim mdp As Point
    Dim rotating As Boolean = False
    Dim mainpathBound As RectangleF

    Public Sub New(ByRef edtr As vEditor)
        v = edtr
    End Sub

    Public Sub Draw(ByRef g As System.Drawing.Graphics) Implements Iedtr.Draw
        If v.selection.isEmty = False Then

            Using p As New Pen(Color.Red), pth As New GraphicsPath

                mainpathBound = v.getBoundRect()
                pth.AddRectangle(mainpathBound)
                v.View.mem2DcPath(pth)

                g.DrawPath(p, pth)

                Dim bound = Rectangle.Round(pth.GetBounds)

                Dim pointers() As Rectangle = {getRect(bound.X, bound.Y),
                                                getRect(bound.X, bound.Y + bound.Height),
                                                getRect(bound.X + bound.Width, bound.Y),
                                                getRect(bound.X + bound.Width, bound.Y + bound.Height)}


                'g.FillRectangles(Brushes.Brown, pointers)
                g.SmoothingMode = SmoothingMode.AntiAlias
                Me.DrawEllipses(g, pointers)
            End Using

        End If
    End Sub

    Public Sub mouse_Down(ByRef e As System.Windows.Forms.MouseEventArgs) Implements Iedtr.mouse_Down
        rotating = False

        If MouseButtons.Left Then


            If v.selection.isEmty = False Then
                Using pth As New GraphicsPath
                    Dim rf As RectangleF = v.getBoundRect()
                    pth.AddRectangle(rf)
                    v.View.mem2DcPath(pth)
                    Dim bound = Rectangle.Round(pth.GetBounds)

                    Dim hit = Me.hittest(e.Location, bound)

                    If hit <> -1 Then
                        mdp = Me.MiddlePoint(bound)
                        mda = Me.Angle(mdp, e.Location)

                        svp = v.getSelectionPath.GraphicsPath.Clone
                        v.View.mem2DcPath(svp)
                        v.View.BufferGraphics.Initialize()
                        rotating = True
                    Else
                        v.SelectAt(e.Location)
                    End If
                End Using
            Else

                v.SelectAt(e.Location)
            End If

        End If

    End Sub

    Public Sub mouse_Move(ByRef e As System.Windows.Forms.MouseEventArgs) Implements Iedtr.mouse_Move
        If e.Button = MouseButtons.Left And rotating Then
            Dim angl = Me.Angle(mdp, e.Location)
            v.View.BufferGraphics.Clear()
            Using mat As New Matrix, tmpth As GraphicsPath = svp.Clone
                mat.RotateAt(angl - mda, mdp)
                tmpth.Transform(mat)
                v.View.BufferGraphics.Graphics.DrawPath(Pens.Brown, tmpth)
                v.View.BufferGraphics.Render()
            End Using

        End If
    End Sub

    Public Sub mouse_Up(ByRef e As System.Windows.Forms.MouseEventArgs) Implements Iedtr.mouse_Up
        If rotating Then
            Dim angl = Me.Angle(mdp, e.Location)
            Dim cnt As PointF = Me.MiddlePointF(mainpathBound)
            Using mat As New Matrix
                mat.RotateAt(angl - mda, cnt)
                v.getSelectionPath.GraphicsPath.Transform(mat)

            End Using

            svp.Dispose()
        End If
        v.View.Refresh()
    End Sub

    Private Function getRect(ByVal x As Integer, ByVal y As Integer) As Rectangle
        Return New Rectangle(x - b, y - b, wh, wh)
    End Function

    Private Sub DrawEllipses(ByRef g As Graphics, ByRef rects() As Rectangle)
        For Each rect As Rectangle In rects
            g.FillEllipse(Brushes.Green, rect)
        Next
    End Sub

    Private Function hittest(ByVal p As Point, ByRef b As Rectangle) As Integer

        Dim pointers() As Rectangle = {getRect(b.X, b.Y),
                                        getRect(b.X, b.Y + b.Height),
                                        getRect(b.X + b.Width, b.Y),
                                        getRect(b.X + b.Width, b.Y + b.Height)
                                      }


        Dim rtn As Integer = -1

        For i As Integer = 3 To 0 Step -1
            If pointers(i).Contains(p) Then
                rtn = i
                Exit For
            End If
        Next

        Return rtn
    End Function

    Private Function Angle(ByVal o As Point, ByVal p As Point) As Single
        Dim angl As Single
        angl = Math.Atan((p.Y - o.Y) / (p.X - o.X)) * 180 / Math.PI
        Return angl
    End Function
    Private Function MiddlePoint(ByRef rect As Rectangle)
        Return New Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2)
    End Function
    Private Function MiddlePointF(ByRef rect As RectangleF)
        Return New PointF(rect.X + rect.Width / 2, rect.Y + rect.Height / 2)
    End Function
End Class
