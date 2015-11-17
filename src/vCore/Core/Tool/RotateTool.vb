Imports System.Drawing
Imports System.Windows.Forms
Imports System.Drawing.Drawing2D

Public Class RotateTool
    Implements Itool, Iedtr

    Private b As Integer = 3
    Private wh As Integer = b * 2



    Dim Core As vCore
    Dim WithEvents dc As advancedPanel
    Dim mdl As Point
    Dim md As Point

    Dim s As Integer = 0
    Dim svp As GPath
    Dim mda As Single
    Dim mdp As Point
    Dim rotating As Boolean = False
    Dim mainpathBound As RectangleF


    Public Sub New(ByRef vew As vCore)
        Core = vew
    End Sub
    Public ReadOnly Property Device() As advancedPanel Implements Itool.Device
        Get
            Return dc
        End Get
    End Property
   

    Private Sub dc_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dc.MouseDown

        Me.mouse_Down(e)
    End Sub

    Private Sub dc_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dc.MouseMove

        Me.mouse_Move(e)
    End Sub
    Private Sub dc_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dc.MouseUp

        Me.mouse_Up(e)
    End Sub
    Public Sub DeSelectTool() Implements Itool.DeSelectTool
        dc = Nothing
    End Sub

    Public Sub SelectTool(ByRef d As advancedPanel) Implements Itool.SelectTool
        dc = d

        Core.Editor.setIEdit(Me)
    End Sub

    Public Sub Draw(ByRef g As Graphics) Implements Iedtr.Draw
        If Core.Editor.selection.isEmty = False Then

            Using p As New Pen(Color.Red), pth As New GraphicsPath

                mainpathBound = Core.Editor.getBoundRect()
                pth.AddRectangle(mainpathBound)
                Core.Editor.View.mem2DcPath(pth)

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

    Public Sub mouse_Down(ByRef e As MouseEventArgs) Implements Iedtr.mouse_Down
        rotating = False

        If MouseButtons.Left Then


            If Core.Editor.selection.isEmty = False Then
                Using pth As New GraphicsPath
                    Dim rf As RectangleF = Core.Editor.getBoundRect()
                    pth.AddRectangle(rf)
                    Core.View.mem2DcPath(pth)
                    Dim bound = Rectangle.Round(pth.GetBounds)

                    Dim hit = Me.hittest(e.Location, bound)

                    If hit <> -1 Then
                        mdp = Me.MiddlePoint(bound)
                        mda = Me.Angle(mdp, e.Location)

                        svp = Core.Editor.getSelectionPath.GraphicsPath.Clone
                        Core.View.mem2DcGPath(svp)
                        Core.View.BufferGraphics.Initialize()
                        rotating = True
                        dc.ActiveScroll = False
                    Else
                        Core.Editor.SelectAt(e.Location)
                    End If
                End Using
            Else

                Core.Editor.SelectAt(e.Location)
            End If

        End If
    End Sub

    Public Sub mouse_Move(ByRef e As MouseEventArgs) Implements Iedtr.mouse_Move
        If e.Button = MouseButtons.Left And rotating Then
            Dim angl = Me.Angle(mdp, e.Location)
            Core.View.BufferGraphics.Clear()
            Using mat As New Matrix, tmpth As GraphicsPath = svp.ToGraphicsPath
                mat.RotateAt(angl - mda, mdp)
                tmpth.Transform(mat)
                Core.View.BufferGraphics.Graphics.DrawPath(Pens.Brown, tmpth)
                Core.View.BufferGraphics.Render()
            End Using

        End If
    End Sub

    Public Sub mouse_Up(ByRef e As MouseEventArgs) Implements Iedtr.mouse_Up
        If rotating Then
            Dim angl = Me.Angle(mdp, e.Location)
            Dim cnt As PointF = Me.MiddlePointF(mainpathBound)
            Using mat As New Matrix
                mat.RotateAt(angl - mda, cnt)
                Core.Editor.getSelectionPath.GraphicsPath.Transform(mat)

            End Using
            dc.ActiveScroll = True
            'svp.Dispose()
        End If
        Core.View.Refresh()
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
