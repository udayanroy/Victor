Imports System.Drawing.Drawing2D
Imports System.Drawing

Public Class TransformTool
    Implements Itool, IEditor





    Dim Core As vCore
    Dim WithEvents dc As advancedPanel

    Private bound As Rectangle
    Dim noderadious As Single = 3
    Private nodewidth As Integer = noderadious * 2

    Dim mdl As Point
    Dim md As Point

    Dim s As Integer = 0
    Dim svp As GPath

    Dim sizing As Boolean = False
    Dim hit As Integer = -1

    Dim nrect As RectangleF

    Public Sub New(ByRef vcore As vCore)
        Core = vcore
    End Sub

    Public Sub Draw(ByRef g As Drawing.Graphics) Implements Iedtr.Draw

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

                g.DrawLine(p, CInt(bound.X + bound.Width / 2), CInt(bound.Y), CInt(bound.X + bound.Width / 2), CInt(bound.Y - 50))

                'Draw rotation Pointer
                Dim rpointer = getRect(bound.X + bound.Width / 2, bound.Y - 50, 5)
                g.FillEllipse(Brushes.LightGreen, rpointer)
                g.DrawEllipse(Pens.Green, rpointer)


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
        md = e.Location
        mdl = md

        Using pth As New GraphicsPath


            Dim rf As RectangleF = Core.Editor.getBoundRect()
            pth.AddRectangle(rf)
            Core.Editor.View.mem2DcPath(pth)

            bound = Rectangle.Round(pth.GetBounds)

            hit = Me.hittest(e.Location, bound)
            If hit <> -1 Then
                sizing = True
                'Debug.Print("true")
                svp = Core.Editor.getSelectionPath.GraphicsPath.Clone
                Core.Editor.View.mem2DcGPath(svp)
                Core.Editor.View.BufferGraphics.Initialize()
            Else

                sizing = False
                ' s = Core.Editor.SelectAt(mdl)
            End If

        End Using
    End Sub

    Public Sub mouse_Move(ByRef e As Windows.Forms.MouseEventArgs) Implements Iedtr.mouse_Move
        If e.Button = Windows.Forms.MouseButtons.Left Then
            If hit <> -1 Then
                Core.Editor.View.BufferGraphics.Clear()
                Using gp = Me.DoAction(e.Location, hit)
                    Core.Editor.View.BufferGraphics.Graphics.DrawPath(Pens.Red, gp)
                    Core.Editor.View.BufferGraphics.Render()
                End Using



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
        End If


        Core.View.Refresh()
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


    Private Function DoAction(ByVal point As Point, ByVal hit As Integer) As GraphicsPath

        If hit = 0 Then
            Dim p1 = point

            Dim bnd = svp.GetBound
            Dim p2 = New Point(bnd.Right, bnd.Bottom) 'New Point(bnd.X + bnd.Width, bnd.Y + bnd.Height)

            Dim n = bnd.Height / bnd.Width

            Dim rd As New RectangleF(p1.X, p1.Y, _
                                      (p2.X - p1.X), (p2.Y - p1.Y))

            Dim rtn As RectangleF


            If ((Math.Abs(rd.Width * n)) <= Math.Abs(rd.Height)) Then
                rtn.Width = rd.Width
                If rd.Height < 0 Then
                    rtn.Height = -Math.Abs(rd.Width * n)
                Else
                    rtn.Height = Math.Abs(rd.Width * n)
                End If

                rtn.Location = New Point(p1.X, p2.Y - rtn.Height)
            Else
                rtn.Height = rd.Height
                If rd.Width < 0 Then
                    rtn.Width = -Math.Abs(rd.Height / n)
                Else
                    rtn.Width = Math.Abs(rd.Height / n)
                End If

                rtn.Location = New Point(p2.X - rtn.Width, p1.Y)
            End If




            Dim gp As GraphicsPath = svp.ToGraphicsPath
            ScalePath(gp, rtn)
            nrect = rtn
            Return gp
        ElseIf hit = 1 Then

            Dim bnd = svp.GetBound
            Dim l = (point.Y - bnd.Y)

            Dim gp As GraphicsPath = svp.ToGraphicsPath

            Dim trect As New RectangleF(point.X, bnd.Y, (bnd.X - point.X) + bnd.Width, bnd.Height)

            ScalePath(gp, trect)

            nrect = trect
            Return gp
        ElseIf hit = 2 Then


            Dim bnd = svp.GetBound
            Dim p1 = point
            Dim p2 = New Point(bnd.Right, bnd.Top)

            Dim n = bnd.Height / bnd.Width

            Dim rd As New RectangleF(p1.X, p2.Y, _
                                      (p2.X - p1.X), (p1.Y - p2.Y))

            Dim rtn As RectangleF


            If ((Math.Abs(rd.Width * n)) <= Math.Abs(rd.Height)) Then
                rtn.Width = rd.Width
                If rd.Height < 0 Then
                    rtn.Height = -Math.Abs(rd.Width * n)
                Else
                    rtn.Height = Math.Abs(rd.Width * n)
                End If

                rtn.Location = New Point(p1.X, p2.Y)
            Else
                rtn.Height = rd.Height
                If rd.Width < 0 Then
                    rtn.Width = -Math.Abs(rd.Height / n)
                Else
                    rtn.Width = Math.Abs(rd.Height / n)
                End If

                rtn.Location = New Point(p2.X - rtn.Width, p2.Y)
            End If




            Dim gp As GraphicsPath = svp.ToGraphicsPath
            ScalePath(gp, rtn)
            nrect = rtn
            Return gp
        ElseIf hit = 3 Then

            Dim bnd = svp.GetBound
            Dim l = (point.Y - bnd.Y)

            Dim gp As GraphicsPath = svp.ToGraphicsPath

            Dim trect As New RectangleF(bnd.X, point.Y, bnd.Width, (bnd.Y - point.Y) + bnd.Height)
            ScalePath(gp, trect)
            nrect = trect
            Return gp

        ElseIf hit = 4 Then


            Dim bnd = svp.GetBound
            Dim l = (point.Y - bnd.Y)

            Dim gp As GraphicsPath = svp.ToGraphicsPath

            Dim trect As New RectangleF(bnd.X, bnd.Y, bnd.Width, l)
            ScalePath(gp, trect)
            nrect = trect
            Return gp

        ElseIf hit = 5 Then

            Dim p1 = point

            Dim bnd = svp.GetBound
            Dim p2 = New Point(bnd.Left, bnd.Bottom) 'New Point(bnd.X + bnd.Width, bnd.Y + bnd.Height)

            Dim n = bnd.Height / bnd.Width

            Dim rd As New RectangleF(p2.X, p1.Y, _
                                      (p1.X - p2.X), (p2.Y - p1.Y))

            Dim rtn As RectangleF


            If ((Math.Abs(rd.Width * n)) <= Math.Abs(rd.Height)) Then
                rtn.Width = rd.Width
                If rd.Height < 0 Then
                    rtn.Height = -Math.Abs(rd.Width * n)
                Else
                    rtn.Height = Math.Abs(rd.Width * n)
                End If

                rtn.Location = New Point(p2.X, p2.Y - rtn.Height)
            Else
                rtn.Height = rd.Height
                If rd.Width < 0 Then
                    rtn.Width = -Math.Abs(rd.Height / n)
                Else
                    rtn.Width = Math.Abs(rd.Height / n)
                End If

                rtn.Location = New Point(p2.X, p1.Y)
            End If




            Dim gp As GraphicsPath = svp.ToGraphicsPath
            ScalePath(gp, rtn)
            nrect = rtn
            Return gp
        ElseIf hit = 6 Then

            Dim bnd = svp.GetBound
            Dim l = (point.X - bnd.X)
            ' Dim s = l / bnd.Width
            'Debug.Print(s)
            ' mat.Scale(s, 1)
            ' svp.Transform(mat)
            'Dim k As Single = bnd.X * (s - 1)
            'mat.Reset()
            'mat.Translate(-k, 0)
            ' svp.Transform(mat)
            Dim gp As GraphicsPath = svp.ToGraphicsPath

            Dim trect As New RectangleF(bnd.X, bnd.Y, l, bnd.Height)
            ScalePath(gp, trect)
            nrect = trect
            Return gp


        ElseIf hit = 7 Then

            Dim bnd = svp.GetBound
            Dim p1 = bnd.Location

            Dim p2 = point  'New Point(bnd.X + bnd.Width, bnd.Y + bnd.Height)

            Dim n = bnd.Height / bnd.Width

            Dim rd As New RectangleF(p1.X, p1.Y, _
                                      (p2.X - p1.X), (p2.Y - p1.Y))

            Dim rtn As RectangleF

            rtn.Location = p1

            If ((Math.Abs(rd.Width * n)) <= Math.Abs(rd.Height)) Then
                rtn.Width = rd.Width
                If rd.Height < 0 Then
                    rtn.Height = -Math.Abs(rd.Width * n)
                Else
                    rtn.Height = Math.Abs(rd.Width * n)
                End If

                ' rtn.Location = New Point(p1.X, p2.Y - rtn.Height)
            Else
                rtn.Height = rd.Height
                If rd.Width < 0 Then
                    rtn.Width = -Math.Abs(rd.Height / n)
                Else
                    rtn.Width = Math.Abs(rd.Height / n)
                End If

                'rtn.Location = New Point(p2.X - rtn.Width, p1.Y)
            End If




            Dim gp As GraphicsPath = svp.ToGraphicsPath
            ScalePath(gp, rtn)
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
        Dim bnd = gp.GetBound
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
