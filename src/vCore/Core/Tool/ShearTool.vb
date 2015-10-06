Imports System.Drawing
Imports System.Windows.Forms
Imports System.Drawing.Drawing2D

Public Class ShearTool
    Implements Itool, Iedtr


    Dim Core As vCore
    Dim WithEvents dc As advancedPanel

    Dim noderadious As Single = 3
    Private nodewidth As Integer = noderadious * 2

    Dim s As Integer = 0
    Dim svp As GPath

    Dim MouseDownLocation As Point


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

    Public Sub Draw(ByRef g As Graphics) Implements Iedtr.Draw
        If Core.Editor.selection.isEmty = False Then
            g.SmoothingMode = SmoothingMode.AntiAlias
            Using p As New Pen(Color.Red), pth As New GraphicsPath

                Dim rf As RectangleF = Core.Editor.getBoundRect()
                pth.AddRectangle(rf)
                Core.View.mem2DcPath(pth)

                Dim bound = Rectangle.Round(pth.GetBounds)

                g.DrawPath(p, pth)

                Dim pointers() As Rectangle = {getRect(bound.X, bound.Y + bound.Height / 2), _
                                      getRect(bound.X + bound.Width / 2, bound.Y), _
                                      getRect(bound.X + bound.Width / 2, bound.Y + bound.Height), _
                                      getRect(bound.X + bound.Width, bound.Y + bound.Height / 2)}



                'Draw resize pointers
                g.FillRectangles(Brushes.White, pointers)
                g.DrawRectangles(Pens.Brown, pointers)



            End Using
        End If
    End Sub

    Public Sub mouse_Down(ByRef e As MouseEventArgs) Implements Iedtr.mouse_Down
        If MouseButtons.Left Then


            If Core.Editor.selection.isEmty = False Then
                Using pth As New GraphicsPath
                    Dim rf As RectangleF = Core.Editor.getBoundRect()
                    pth.AddRectangle(rf)
                    Core.Editor.View.mem2DcPath(pth)
                    Dim bound = Rectangle.Round(pth.GetBounds)

                    Dim hit = Me.hittest(e.Location, bound)

                    If hit <> -1 Then
                        'mdp = Me.MiddlePoint(bound)
                        'mda = Me.Angle(mdp, e.Location)

                        svp = Core.Editor.getSelectionPath.GraphicsPath.Clone
                        Core.Editor.View.mem2DcGPath(svp)
                        Core.Editor.View.BufferGraphics.Initialize()
                         
                    End If
                End Using
                
            End If

        End If
    End Sub

    Public Sub mouse_Move(ByRef e As MouseEventArgs) Implements Iedtr.mouse_Move
        If e.Button = MouseButtons.Left Then
            '  Dim angl = Me.Angle(mdp, e.Location)
            Core.Editor.View.BufferGraphics.Clear()
            Using mat As New Matrix, tmpth As GraphicsPath = svp.ToGraphicsPath
                ' mat.Shear()
                tmpth.Transform(mat)
                Core.Editor.View.BufferGraphics.Graphics.DrawPath(Pens.Brown, tmpth)
                Core.Editor.View.BufferGraphics.Render()
            End Using

        End If
    End Sub

    Public Sub mouse_Up(ByRef e As MouseEventArgs) Implements Iedtr.mouse_Up

    End Sub

    Private Sub dc_MouseDown(sender As Object, e As Windows.Forms.MouseEventArgs) Handles dc.MouseDown
        Me.mouse_Down(e)
    End Sub

    Private Sub dc_MouseMove(sender As Object, e As Windows.Forms.MouseEventArgs) Handles dc.MouseMove
        Me.mouse_Move(e)
    End Sub

    Private Sub dc_MouseUp(sender As Object, e As Windows.Forms.MouseEventArgs) Handles dc.MouseUp
        Me.mouse_Up(e)
    End Sub


    Private Function getRect(ByVal x As Integer, ByVal y As Integer) As Rectangle
        Return New Rectangle(x - noderadious, y - noderadious, nodewidth, nodewidth)
    End Function

    Private Function getRect(ByVal x As Integer, ByVal y As Integer, radious As Integer) As Rectangle
        Return New Rectangle(x - radious, y - radious, radious * 2, radious * 2)
    End Function

    Private Function hittest(ByVal p As Point, ByRef b As Rectangle) As Integer

        Dim pointers() As Rectangle = {getRect(b.X, b.Y + b.Height / 2), _
                                      getRect(b.X + b.Width / 2, b.Y), _
                                      getRect(b.X + b.Width / 2, b.Y + b.Height), _
                                      getRect(b.X + b.Width, b.Y + b.Height / 2)
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
End Class
