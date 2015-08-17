Imports System.Drawing.Drawing2D
Imports System.Drawing

Public Class TransformTool
    Implements Itool, Iedtr





    Dim Core As vCore
    Dim WithEvents dc As advancedPanel

    Private bound As Rectangle
    Dim noderadious As Single = 3
    Private nodewidth As Integer = noderadious * 2

    Public Sub New(ByRef vcore As vCore)
        Core = vcore
    End Sub

    Public Sub Draw(ByRef g As Drawing.Graphics) Implements Iedtr.Draw

        If Core.Editor.selection.isEmty = False Then
            g.SmoothingMode = SmoothingMode.AntiAlias
            Using p As New Pen(Color.SkyBlue), pth As New GraphicsPath

                Dim rf As RectangleF = Core.Editor.getBoundRect()
                pth.AddRectangle(rf)
                Core.View.mem2DcPath(pth)

                bound = Rectangle.Round(pth.GetBounds)

                g.DrawPath(p, pth)

                Dim pointers() As Rectangle = {getRect(bound.X, bound.Y), _
                                      getRect(bound.X, bound.Y + bound.Height / 2), _
                                      getRect(bound.X, bound.Y + bound.Height), _
                                      getRect(bound.X + bound.Width / 2, bound.Y), _
                                      getRect(bound.X + bound.Width / 2, bound.Y + bound.Height), _
                                      getRect(bound.X + bound.Width, bound.Y), _
                                      getRect(bound.X + bound.Width, bound.Y + bound.Height / 2), _
                                      getRect(bound.X + bound.Width, bound.Y + bound.Height)}


                g.FillRectangles(Brushes.Brown, pointers)





            End Using

        End If
    End Sub

    Private Function getRect(ByVal x As Integer, ByVal y As Integer) As Rectangle
        Return New Rectangle(x - noderadious, y - noderadious, nodewidth, nodewidth)
    End Function



    Public Sub mouse_Down(ByRef e As Windows.Forms.MouseEventArgs) Implements Iedtr.mouse_Down

    End Sub

    Public Sub mouse_Move(ByRef e As Windows.Forms.MouseEventArgs) Implements Iedtr.mouse_Move

    End Sub

    Public Sub mouse_Up(ByRef e As Windows.Forms.MouseEventArgs) Implements Iedtr.mouse_Up

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



  
End Class
