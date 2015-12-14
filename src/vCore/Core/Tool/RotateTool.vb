Imports Geometry
Imports Graphics

Public Class RotateTool
    Implements Itool, IEditor

    Private b As Integer = 3
    Private wh As Integer = b * 2



    Dim Core As vCore
    Dim WithEvents dc As IDevice
    Dim mdl As Point
    Dim md As Point

    Dim s As Integer = 0
    Dim svp As NodePath
    Dim mda As Single
    Dim mdp As Point
    Dim rotating As Boolean = False
    Dim mainpathBound As Rect


    Public Sub New(ByRef vew As vCore)
        Core = vew
    End Sub
    Public ReadOnly Property Device() As IDevice Implements Itool.Device
        Get
            Return dc
        End Get
    End Property
   

    Private Sub dc_MouseDown(e As MouseEvntArg) Handles dc.MouseDown

        Me.mouse_Down(e)
    End Sub

    Private Sub dc_MouseMove(e As MouseEvntArg) Handles dc.MouseMove

        Me.mouse_Move(e)
    End Sub
    Private Sub dc_MouseUp(e As MouseEvntArg) Handles dc.MouseUp

        Me.mouse_Up(e)
    End Sub
    Public Sub DeSelectTool() Implements Itool.DeSelectTool
        dc = Nothing
    End Sub

    Public Sub SelectTool(ByRef d As IDevice) Implements Itool.SelectTool
        dc = d

        Core.Editor.setIEdit(Me)
    End Sub

    Public Sub Draw(g As Canvas) Implements IEditor.Draw
        If Core.Editor.selection.isEmty = False Then

            Dim p As New Pen(Color.RedColor)
            Dim pth As New NodePath

            mainpathBound = Core.Editor.getBoundRect()
            pth.AddRectangle(mainpathBound)
            Core.Editor.View.Memory2screen(pth)

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

        If MouseButton.Left Then


            If Core.Editor.selection.isEmty = False Then
                Dim pth As New NodePath
                Dim rf As Rect = Core.Editor.getBoundRect()
                pth.AddRectangle(rf)
                Core.View.Memory2screen(pth)
                Dim bound = pth.GetBound

                Dim hit = Me.hittest(e.Location, bound)

                If hit <> -1 Then
                    mdp = Me.MiddlePoint(bound)
                    mda = Me.Angle(mdp, e.Location)

                    svp = Core.Editor.getSelectionPath.Path.Clone
                    Core.View.Memory2screen(svp)
                    Core.View.BufferGraphics.Initialize()
                    rotating = True
                    dc.ActiveScroll = False
                Else
                    Core.Editor.SelectAt(e.Location)
                End If

            Else

                Core.Editor.SelectAt(e.Location)
            End If

        End If
    End Sub

    Public Sub mouse_Move(e As MouseEvntArg)
        If e.Button = MouseButton.Left And rotating Then
            Dim angl = Me.Angle(mdp, e.Location)
            Core.View.BufferGraphics.Clear()
            Dim mat As Matrix = Matrix.Identity
            Dim tmpth = svp.Clone
            mat.RoatateAt(angl - mda, mdp)
            tmpth.Transform(mat)
            Core.View.BufferGraphics.Graphics.DrawPath(tmpth, New Pen(Color.BrownColor))
            Core.View.BufferGraphics.Render()

        End If
    End Sub

    Public Sub mouse_Up(e As MouseEvntArg)
        If rotating Then
            Dim angl = Me.Angle(mdp, e.Location)
            Dim cnt As Point = Me.MiddlePoint(mainpathBound)
            Dim mat As Matrix = Matrix.Identity
            mat.RoatateAt(angl - mda, cnt)
            Core.Editor.getSelectionPath.Path.Transform(mat)


            dc.ActiveScroll = True
            'svp.Dispose()
        End If
        Core.View.Refresh()
    End Sub

    Private Function getRect(ByVal x As Integer, ByVal y As Integer) As Rect
        Return New Rect(New Point(x - b, y - b), wh, wh)
    End Function

    Private Sub DrawEllipses(g As Canvas, ByRef rects() As Rect)
        For Each rect As Rect In rects
            g.DrawEllipse(rect, , New SolidColorBrush(Color.GreenColor))
        Next
    End Sub

    Private Function hittest(p As Point, b As Rect) As Integer

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
        angl = Math.Atan2((p.Y - o.Y), (p.X - o.X)) * 180 / Math.PI
        Return angl
    End Function
    Private Function MiddlePoint(rect As Rect)
        Return New Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2)
    End Function
    

End Class
