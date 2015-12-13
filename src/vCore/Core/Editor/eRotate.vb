Imports Geometry
Imports Graphics



Public Class eRotate
    Implements IEditor

    Private b As Integer = 3
    Private wh As Integer = b * 2


    Dim v As Editor


    Dim s As Integer = 0
    Dim svp As NodePath
    Dim mda As Single
    Dim mdp As Point
    Dim rotating As Boolean = False
    Dim mainpathBound As Rect

    Public Sub New(ByRef edtr As Editor)
        v = edtr
    End Sub

    Public Sub Draw(g As Canvas) Implements IEditor.Draw
        If v.selection.isEmty = False Then

            Dim p As New Pen(Color.RedColor)
            Dim pth As New NodePath

            mainpathBound = v.getBoundRect()
            pth.AddRectangle(mainpathBound)
            v.View.Memory2screen(pth)

            g.DrawPath(pth, p)

            Dim bound = pth.GetBound

            Dim pointers() As Rect = {getRect(bound.X, bound.Y),
                                            getRect(bound.X, bound.Y + bound.Height),
                                            getRect(bound.X + bound.Width, bound.Y),
                                            getRect(bound.X + bound.Width, bound.Y + bound.Height)}


            'g.FillRectangles(Brushes.Brown, pointers)
            g.Smooth()
            Me.DrawEllipses(g, pointers)


        End If
    End Sub

    Public Sub mouse_Down(e As MouseEvntArg)
        rotating = False

        If e.Button = MouseButton.Left Then


            If v.selection.isEmty = False Then
                Dim pth As New NodePath
                Dim rf As Rect = v.getBoundRect()
                pth.AddRectangle(rf)
                v.View.Memory2screen(pth)
                Dim bound = pth.GetBound

                Dim hit = Me.hittest(e.Location, bound)

                If hit <> -1 Then
                    mdp = Me.MiddlePoint(bound)
                    mda = Me.Angle(mdp, e.Location)

                    svp = v.getSelectionPath.Path.Clone
                    v.View.Memory2screen(svp)
                    v.View.BufferGraphics.Initialize()
                    rotating = True
                Else
                    v.SelectAt(e.Location)
                End If

            Else

                v.SelectAt(e.Location)
            End If

        End If

    End Sub

    Public Sub mouse_Move(e As MouseEvntArg)
        If e.Button = MouseButton.Left And rotating Then
            Dim angl = Me.Angle(mdp, e.Location)
            v.View.BufferGraphics.Clear()
            Dim mat As Matrix = Matrix.Identity
            Dim tmpth = svp.Clone
            mat.RoatateAt(angl - mda, mdp)
            tmpth.Transform(mat)
            v.View.BufferGraphics.Graphics.DrawPath(tmpth, New Pen(Color.BrownColor), )
            v.View.BufferGraphics.Render()


        End If
    End Sub

    Public Sub mouse_Up(e As MouseEvntArg)
        If rotating Then
            Dim angl = Me.Angle(mdp, e.Location)
            Dim cnt As Point = Me.MiddlePoint(mainpathBound)
            Dim mat = Matrix.Identity
            mat.RoatateAt(angl - mda, cnt)
            v.getSelectionPath.Path.Transform(mat)



            'svp.Dispose()
        End If
        v.View.Refresh()
    End Sub

    Private Function getRect(ByVal x As Integer, ByVal y As Integer) As Rect
        Return New Rect(x - b, y - b, wh, wh)
    End Function

    Private Sub DrawEllipses(ByRef g As Canvas, ByRef rects() As Rect)
        For Each rect As Rect In rects
            g.DrawEllipse(rect, , New SolidColorBrush(Color.GreenColor))
        Next
    End Sub

    Private Function hittest(ByVal p As Point, ByRef b As Rect) As Integer

        Dim pointers() As Rect = {getRect(b.X, b.Y),
                                        getRect(b.X, b.Y + b.Height),
                                        getRect(b.X + b.Width, b.Y),
                                        getRect(b.X + b.Width, b.Y + b.Height)
                                      }


        Dim rtn As Integer = -1

        For i As Integer = 3 To 0 Step -1
            If pointers(i).Contain(p) Then
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
    Private Function MiddlePoint(rect As Rect) As Point
        Return New Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2)
    End Function
    
End Class
