Imports System.Drawing.Drawing2D
Imports System.Drawing


Public Class SelectionTool
    Implements Itool, IEditor



    Dim Core As vCore
    Dim WithEvents dc As advancedPanel

    Private bound As Rectangle
    Dim noderadious As Single = 3
    Private nodewidth As Integer = noderadious * 2

    Dim MouseLocation As Point
    Dim MouseDownLocation As Point
    ' Dim mdl As Point
    ' Dim md As Point

    Dim s As Integer = 0
    Dim svp As GPath

    Dim sizing As Boolean = False
    Dim hit As Integer = -1

    Dim nrect As RectangleF

    Public Sub New(ByRef vcore As vCore)
        Core = vcore
    End Sub


    Public Sub Draw(ByRef g As Graphics) Implements Iedtr.Draw
        If Core.Editor.selection.isEmty = False Then
            g.SmoothingMode = SmoothingMode.AntiAlias
            Using p As New Pen(Color.Red), pth As New GraphicsPath

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



                'Draw resize pointers
                g.FillRectangles(Brushes.White, pointers)
                g.DrawRectangles(Pens.Brown, pointers)



            End Using

        End If
    End Sub

    Private Function getRect(ByVal x As Integer, ByVal y As Integer) As Rectangle
        Return New Rectangle(x - noderadious, y - noderadious, nodewidth, nodewidth)
    End Function

    Private Function getRect(ByVal x As Integer, ByVal y As Integer, radious As Integer) As Rectangle
        Return New Rectangle(x - radious, y - radious, radious * 2, radious * 2)
    End Function

    Public Sub mouse_Down(ByRef e As Windows.Forms.MouseEventArgs) Implements Iedtr.mouse_Down
        MouseDownLocation = e.Location
        MouseLocation = e.Location

        If Core.Editor.selection.isEmty Then
            s = Core.Editor.SelectAt(MouseLocation)
            If s <> 0 Then

                svp = Core.Editor.getSelectionPath.GraphicsPath.Clone
                Core.Editor.View.mem2DcGPath(svp)
                Core.Editor.View.BufferGraphics.Initialize()
                dc.ActiveScroll = False
            End If
        Else
            hit = Me.hittest(e.Location, bound)
            If hit <> -1 Then
                sizing = True

                svp = Core.Editor.getSelectionPath.GraphicsPath.Clone
                Core.Editor.View.mem2DcGPath(svp)
                Core.Editor.View.BufferGraphics.Initialize()
                dc.ActiveScroll = False
            Else

                sizing = False
                s = Core.Editor.SelectAt(MouseLocation)

                If s <> 0 Then

                    svp = Core.Editor.getSelectionPath.GraphicsPath.Clone
                    Core.Editor.View.mem2DcGPath(svp)
                    Core.Editor.View.BufferGraphics.Initialize()
                    dc.ActiveScroll = False
                End If


            End If

        End If



    End Sub

    Public Sub mouse_Move(ByRef e As Windows.Forms.MouseEventArgs) Implements Iedtr.mouse_Move
        If e.Button = Windows.Forms.MouseButtons.Left Then
            If s <> 0 Then
                Core.Editor.View.BufferGraphics.Clear()
                If sizing Then
                    If hit <> -1 Then

                        Dim gp = Me.DoAction(e.Location, hit)
                        Dim g = Core.Editor.View.BufferGraphics.Graphics
                        gp.drawPath(g, Pens.Red)
                        Core.Editor.View.BufferGraphics.Render()


                    End If
                Else
                    Using mat As New Matrix
                        mat.Translate((e.Location.X - MouseLocation.X), (e.Location.Y - MouseLocation.Y))
                        svp.Transform(mat)
                    End Using

                    Dim g = Core.Editor.View.BufferGraphics.Graphics
                    svp.drawPath(g, Pens.Red)
                    Core.Editor.View.BufferGraphics.Render()

                End If


                MouseLocation = e.Location
            End If
        End If
    End Sub

    Public Sub mouse_Up(ByRef e As Windows.Forms.MouseEventArgs) Implements Iedtr.mouse_Up
        If sizing Then
            Dim tr As RectangleF

            tr.Location = Core.Editor.View.Dc2memPt(nrect.Location)

            Dim pk = Core.View.Dc2memPt(New Point(nrect.X + nrect.Width, nrect.Y + nrect.Height))
            tr.Width = pk.X - tr.X
            tr.Height = pk.Y - tr.Y

            ScaleGPath(Core.Editor.getSelectionPath.GraphicsPath, tr)

            'svp.Dispose()
        Else
            If s <> 0 Then
                Dim p1 = Core.Editor.View.Dc2memPt(MouseDownLocation)
                Dim p2 = Core.Editor.View.Dc2memPt(e.Location)
                Core.Editor.getSelectionPath.Translate(p2.X - p1.X, p2.Y - p1.Y)

                'svp.Dispose()
            End If
        End If

        Core.Editor.View.Refresh()
        dc.ActiveScroll = True
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

    Private Sub dc_MouseDown(sender As Object, e As Windows.Forms.MouseEventArgs) Handles dc.MouseDown
        Me.mouse_Down(e)
    End Sub

    Private Sub dc_MouseMove(sender As Object, e As Windows.Forms.MouseEventArgs) Handles dc.MouseMove
        Me.mouse_Move(e)
    End Sub

    Private Sub dc_MouseUp(sender As Object, e As Windows.Forms.MouseEventArgs) Handles dc.MouseUp
        Me.mouse_Up(e)
    End Sub




    Private Function hittest(ByRef p As Point, ByRef b As Rectangle) As Integer

        ' Sequence of Points..
        '
        '       1---------4-------6
        '       |                 |
        '       |                 |
        '       2                 7
        '       |                 |
        '       |                 |
        '       3---------5-------8
        '
        '-------------------------------------------------


        Dim pointers() As Rectangle = {getRect(bound.X, bound.Y), _
                                       getRect(bound.X, bound.Y + bound.Height / 2), _
                                       getRect(bound.X, bound.Y + bound.Height), _
                                       getRect(bound.X + bound.Width / 2, bound.Y), _
                                       getRect(bound.X + bound.Width / 2, bound.Y + bound.Height), _
                                       getRect(bound.X + bound.Width, bound.Y), _
                                       getRect(bound.X + bound.Width, bound.Y + bound.Height / 2), _
                                       getRect(bound.X + bound.Width, bound.Y + bound.Height),
                                 getRect(bound.X + bound.Width / 2, bound.Y - 50, 5)}

        Dim rtn As Integer = -1

        For i As Integer = 8 To 0 Step -1
            If pointers(i).Contains(p) Then
                rtn = i
                Exit For
            End If
        Next

        Return rtn
    End Function


    Private Function DoAction(ByVal point As Point, ByVal hit As Integer) As GPath

        If hit = 0 Then
            Dim p1 = point

            Dim bnd = svp.GetTightBound
            Dim p2 = New Point(bnd.Right, bnd.Bottom) 'New Point(bnd.X + bnd.Width, bnd.Y + bnd.Height)

            Dim n = bnd.Height / bnd.Width

            Dim rd As New RectangleF(p1.X, p1.Y, _
                                      (p2.X - p1.X), (p2.Y - p1.Y))

            Dim rtn As RectangleF
            rtn = rd



            Dim gp As GPath = svp.Clone
            ScaleGPath(gp, rtn)
            nrect = rtn
            Return gp
        ElseIf hit = 1 Then

            Dim bnd = svp.GetTightBound
            Dim l = (point.Y - bnd.Y)

            Dim gp As GPath = svp.Clone

            Dim trect As New RectangleF(point.X, bnd.Y, (bnd.X - point.X) + bnd.Width, bnd.Height)

            ScaleGPath(gp, trect)

            nrect = trect
            Return gp
        ElseIf hit = 2 Then


            Dim bnd = svp.GetTightBound
            Dim p1 = point
            Dim p2 = New Point(bnd.Right, bnd.Top)

            Dim n = bnd.Height / bnd.Width

            Dim rd As New RectangleF(p1.X, p2.Y, _
                                      (p2.X - p1.X), (p1.Y - p2.Y))

            Dim rtn As RectangleF

            rtn = rd



            Dim gp As GPath = svp.Clone
            ScaleGPath(gp, rtn)
            nrect = rtn
            Return gp
        ElseIf hit = 3 Then

            Dim bnd = svp.GetTightBound
            Dim l = (point.Y - bnd.Y)

            Dim gp As GPath = svp.Clone

            Dim trect As New RectangleF(bnd.X, point.Y, bnd.Width, (bnd.Y - point.Y) + bnd.Height)
            ScaleGPath(gp, trect)
            nrect = trect
            Return gp

        ElseIf hit = 4 Then


            Dim bnd = svp.GetTightBound
            Dim l = (point.Y - bnd.Y)

            Dim gp As GPath = svp.Clone

            Dim trect As New RectangleF(bnd.X, bnd.Y, bnd.Width, l)
            ScaleGPath(gp, trect)
            nrect = trect
            Return gp

        ElseIf hit = 5 Then

            Dim p1 = point

            Dim bnd = svp.GetTightBound
            Dim p2 = New Point(bnd.Left, bnd.Bottom) 'New Point(bnd.X + bnd.Width, bnd.Y + bnd.Height)

            Dim n = bnd.Height / bnd.Width

            Dim rd As New RectangleF(p2.X, p1.Y, _
                                      (p1.X - p2.X), (p2.Y - p1.Y))

            Dim rtn As RectangleF

            rtn = rd


            Dim gp = svp.Clone
            ScaleGPath(gp, rtn)
            nrect = rtn
            Return gp
        ElseIf hit = 6 Then

            Dim bnd = svp.GetTightBound
            Dim l = (point.X - bnd.X)
            ' Dim s = l / bnd.Width
            'Debug.Print(s)
            ' mat.Scale(s, 1)
            ' svp.Transform(mat)
            'Dim k As Single = bnd.X * (s - 1)
            'mat.Reset()
            'mat.Translate(-k, 0)
            ' svp.Transform(mat)
            Dim gp = svp.Clone

            Dim trect As New RectangleF(bnd.X, bnd.Y, l, bnd.Height)
            ScaleGPath(gp, trect)
            nrect = trect
            Return gp


        ElseIf hit = 7 Then

            Dim bnd = svp.GetTightBound
            Dim p1 = bnd.Location

            Dim p2 = point  'New Point(bnd.X + bnd.Width, bnd.Y + bnd.Height)

            Dim n = bnd.Height / bnd.Width

            Dim rd As New RectangleF(p1.X, p1.Y, _
                                      (p2.X - p1.X), (p2.Y - p1.Y))

            Dim rtn As RectangleF

            rtn.Location = p1
            rtn = rd



            Dim gp As GPath = svp.Clone
            ScaleGPath(gp, rtn)
            nrect = rtn
            Return gp

        Else
            Return Nothing
        End If

    End Function

    Private Sub ScalePath(ByRef gp As GraphicsPath, ByVal Torect As RectangleF)
        Dim bnd = gp.GetBounds
        Dim xinvert As Boolean = IIf(Torect.Width < 0, True, False)
        Dim yinvert As Boolean = IIf(Torect.Height < 0, True, False)


        Using mat As New Matrix
            mat.Translate(-bnd.X, -bnd.Y)
            gp.Transform(mat)

            mat.Reset()
            Dim sx = Torect.Width / bnd.Width
            Dim sy = Torect.Height / bnd.Height
            mat.Scale(sx, sy)
            gp.Transform(mat)

            mat.Reset()
            mat.Translate(Torect.X, Torect.Y)
            gp.Transform(mat)


        End Using
    End Sub

    Private Sub ScaleGPath(ByRef gp As GPath, ByVal Torect As RectangleF)
        Dim bnd = gp.GetTightBound
        Dim xinvert As Boolean = IIf(Torect.Width < 0, True, False)
        Dim yinvert As Boolean = IIf(Torect.Height < 0, True, False)


        Using mat As New Matrix
            mat.Translate(-bnd.X, -bnd.Y)
            gp.Transform(mat)

            mat.Reset()
            Dim sx = Torect.Width / bnd.Width
            Dim sy = Torect.Height / bnd.Height
            mat.Scale(sx, sy)
            gp.Transform(mat)

            mat.Reset()
            mat.Translate(Torect.X, Torect.Y)
            gp.Transform(mat)


        End Using
    End Sub
End Class
