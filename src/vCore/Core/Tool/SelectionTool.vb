Imports Geometry
Imports Graphics


Public Class SelectionTool
    Implements Itool, IEditor



    Dim Core As vCore
    Dim WithEvents dc As IDevice

    Private bound As Rect
    Dim noderadious As Single = 3
    Private nodewidth As Integer = noderadious * 2

    Dim MouseLocation As Point
    Dim MouseDownLocation As Point
    ' Dim mdl As Point
    ' Dim md As Point

    Dim s As Integer = 0
    Dim svp As NodePath

    Dim sizing As Boolean = False
    Dim hit As Integer = -1

    Dim nrect As Rect

    Public Sub New(ByRef vcore As vCore)
        Core = vcore
    End Sub


    Public Sub Draw(canvas As Canvas) Implements IEditor.Draw
        If Core.Editor.selection.isEmty = False Then
            canvas.Smooth()
            Dim p As New Pen(Color.RedColor)
            Dim pth As New NodePath

            Dim rf As Rect = Core.Editor.getBoundRect()
            pth.AddRectangle(rf)
            Core.View.Memory2screen(pth)

            bound = pth.GetBound

            canvas.DrawPath(pth, p)

            Dim pointers() As Rect = {getRect(bound.X, bound.Y), _
                                  getRect(bound.X, bound.Y + bound.Height / 2), _
                                  getRect(bound.X, bound.Y + bound.Height), _
                                  getRect(bound.X + bound.Width / 2, bound.Y), _
                                  getRect(bound.X + bound.Width / 2, bound.Y + bound.Height), _
                                  getRect(bound.X + bound.Width, bound.Y), _
                                  getRect(bound.X + bound.Width, bound.Y + bound.Height / 2), _
                                  getRect(bound.X + bound.Width, bound.Y + bound.Height)}



            'Draw resize pointers
            'canvas.FillRectangles('Brushes.White, pointers)
            'canvas.DrawRectangles('Pens.Brown, pointers)
            canvas.DrawRects(pointers, New Pen(Color.BlackColor), New SolidColorBrush(Color.WhiteColor))




        End If
    End Sub

    Private Function getRect(ByVal x As Integer, ByVal y As Integer) As Rect
        Return New Rect(New Point(x - noderadious, y - noderadious), nodewidth, nodewidth)
    End Function

    Private Function getRect(ByVal x As Integer, ByVal y As Integer, radious As Integer) As Rect
        Return New Rect(New Point(x - radious, y - radious), radious * 2, radious * 2)
    End Function

    Public Sub mouse_Down(e As MouseEvntArg)
        MouseDownLocation = e.Location
        MouseLocation = e.Location

        If Core.Editor.selection.isEmty Then
            s = Core.Editor.SelectAt(MouseLocation)
            If s <> 0 Then

                svp = Core.Editor.getSelectionPath.Path.Clone
                Core.View.Memory2screen(svp)
                Core.Editor.View.BufferGraphics.Initialize()
                dc.ActiveScroll = False
            End If
        Else
            hit = Me.hittest(e.Location, bound)
            If hit <> -1 Then
                sizing = True

                svp = Core.Editor.getSelectionPath.Path.Clone
                Core.View.Memory2screen(svp)
                Core.Editor.View.BufferGraphics.Initialize()
                dc.ActiveScroll = False
            Else

                sizing = False
                s = Core.Editor.SelectAt(MouseLocation)

                If s <> 0 Then

                    svp = Core.Editor.getSelectionPath.Path.Clone
                    Core.View.Memory2screen(svp)
                    Core.Editor.View.BufferGraphics.Initialize()
                    dc.ActiveScroll = False
                End If


            End If

        End If



    End Sub

    Public Sub mouse_Move(e As MouseEvntArg)
        If e.Button = Windows.Forms.MouseButtons.Left Then
            If s <> 0 Then
                Core.View.BufferGraphics.Clear()
                If sizing Then
                    If hit <> -1 Then

                        Dim gp = Me.DoAction(e.Location, hit)
                        Dim g = Core.Editor.View.BufferGraphics.Graphics

                        g.DrawPath(gp, New Pen(Color.RedColor))
                        Core.Editor.View.BufferGraphics.Render()


                    End If
                Else
                    Dim mat As Matrix = Matrix.Identity
                    mat.Translate((e.Location.X - MouseLocation.X), (e.Location.Y - MouseLocation.Y))
                    svp.Transform(mat)


                    Dim g = Core.Editor.View.BufferGraphics.Graphics

                    g.DrawPath(svp, New Pen(Color.RedColor))
                    Core.Editor.View.BufferGraphics.Render()

                End If


                MouseLocation = e.Location
            End If
        End If
    End Sub

    Public Sub mouse_Up(e As MouseEvntArg)
        If sizing Then
            Dim tr As Rect
            Dim nrctloc = nrect.Location
            Core.View.Screen2memory(nrctloc)
            tr.Location = nrctloc

            Dim pk As New Point(nrect.X + nrect.Width, nrect.Y + nrect.Height)
            Core.View.Screen2memory(pk)

            tr.Width = pk.X - tr.X
            tr.Height = pk.Y - tr.Y

            ScalePath(Core.Editor.getSelectionPath.Path, tr)

            'svp.Dispose()
        Else
            If s <> 0 Then
                Dim p1 = MouseDownLocation
                Core.View.Screen2memory(p1)
                Dim p2 = e.Location
                Core.View.Screen2memory(p2)
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

    Public ReadOnly Property Device As IDevice Implements Itool.Device
        Get
            Return dc
        End Get
    End Property

    Public Sub SelectTool(ByRef d As IDevice) Implements Itool.SelectTool
        dc = d
        Core.Editor.setIEdit(Me)
    End Sub

    Private Sub dc_MouseDown(e As MouseEvntArg) Handles dc.MouseDown
        Me.mouse_Down(e)
    End Sub

    Private Sub dc_MouseMove(e As MouseEvntArg) Handles dc.MouseMove
        Me.mouse_Move(e)
    End Sub

    Private Sub dc_MouseUp(e As MouseEvntArg) Handles dc.MouseUp
        Me.mouse_Up(e)
    End Sub




    Private Function hittest(ByRef p As Point, ByRef b As Rect) As Integer

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


        Dim pointers() As Rect = {getRect(bound.X, bound.Y), _
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
            If pointers(i).Contain(p) Then
                rtn = i
                Exit For
            End If
        Next

        Return rtn
    End Function


    Private Function DoAction(ByVal point As Point, ByVal hit As Integer) As NodePath

        If hit = 0 Then
            Dim p1 = point

            Dim bnd = svp.GetTightBound
            Dim p2 = New Point(bnd.Right, bnd.Bottom) 'New Point(bnd.X + bnd.Width, bnd.Y + bnd.Height)

            Dim n = bnd.Height / bnd.Width

            Dim rd As New Rect(p1.X, p1.Y, _
                                      (p2.X - p1.X), (p2.Y - p1.Y))

            Dim rtn As Rect
            rtn = rd



            Dim gp = svp.Clone
            ScalePath(gp, rtn)
            nrect = rtn
            Return gp
        ElseIf hit = 1 Then

            Dim bnd = svp.GetTightBound
            Dim l = (point.Y - bnd.Y)

            Dim gp = svp.Clone

            Dim trect As New Rect(point.X, bnd.Y, (bnd.X - point.X) + bnd.Width, bnd.Height)

            ScalePath(gp, trect)

            nrect = trect
            Return gp
        ElseIf hit = 2 Then


            Dim bnd = svp.GetTightBound
            Dim p1 = point
            Dim p2 = New Point(bnd.Right, bnd.Top)

            Dim n = bnd.Height / bnd.Width

            Dim rd As New Rect(p1.X, p2.Y, _
                                      (p2.X - p1.X), (p1.Y - p2.Y))

            Dim rtn As Rect

            rtn = rd



            Dim gp = svp.Clone
            ScalePath(gp, rtn)
            nrect = rtn
            Return gp
        ElseIf hit = 3 Then

            Dim bnd = svp.GetTightBound
            Dim l = (point.Y - bnd.Y)

            Dim gp = svp.Clone

            Dim trect As New Rect(bnd.X, point.Y, bnd.Width, (bnd.Y - point.Y) + bnd.Height)
            ScalePath(gp, trect)
            nrect = trect
            Return gp

        ElseIf hit = 4 Then


            Dim bnd = svp.GetTightBound
            Dim l = (point.Y - bnd.Y)

            Dim gp = svp.Clone

            Dim trect As New Rect(bnd.X, bnd.Y, bnd.Width, l)
            ScalePath(gp, trect)
            nrect = trect
            Return gp

        ElseIf hit = 5 Then

            Dim p1 = point

            Dim bnd = svp.GetTightBound
            Dim p2 = New Point(bnd.Left, bnd.Bottom) 'New Point(bnd.X + bnd.Width, bnd.Y + bnd.Height)

            Dim n = bnd.Height / bnd.Width

            Dim rd As New Rect(p2.X, p1.Y, _
                                      (p1.X - p2.X), (p2.Y - p1.Y))

            Dim rtn As Rect

            rtn = rd


            Dim gp = svp.Clone
            ScalePath(gp, rtn)
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

            Dim trect As New Rect(bnd.X, bnd.Y, l, bnd.Height)
            ScalePath(gp, trect)
            nrect = trect
            Return gp


        ElseIf hit = 7 Then

            Dim bnd = svp.GetTightBound
            Dim p1 = bnd.Location

            Dim p2 = point  'New Point(bnd.X + bnd.Width, bnd.Y + bnd.Height)

            Dim n = bnd.Height / bnd.Width

            Dim rd As New Rect(p1.X, p1.Y, _
                                      (p2.X - p1.X), (p2.Y - p1.Y))

            Dim rtn As Rect

            rtn.Location = p1
            rtn = rd



            Dim gp = svp.Clone
            ScalePath(gp, rtn)
            nrect = rtn
            Return gp

        Else
            Return Nothing
        End If

    End Function

    Private Sub ScalePath(gp As NodePath, ByVal Torect As Rect)
        Dim bnd = gp.GetBound
        Dim xinvert As Boolean = IIf(Torect.Width < 0, True, False)
        Dim yinvert As Boolean = IIf(Torect.Height < 0, True, False)


        Dim mat As Matrix = Matrix.Identity
        mat.Translate(-bnd.X, -bnd.Y)
        gp.Transform(mat)

        mat.reset()
        Dim sx = Torect.Width / bnd.Width
        Dim sy = Torect.Height / bnd.Height
        mat.Scale(sx, sy)
        gp.Transform(mat)

        mat.reset()
        mat.Translate(Torect.X, Torect.Y)
        gp.Transform(mat)



    End Sub

    'Private Sub ScaleGPath(ByRef gp As GPath, ByVal Torect As Rect)
    '    Dim bnd = gp.GetTightBound
    '    Dim xinvert As Boolean = IIf(Torect.Width < 0, True, False)
    '    Dim yinvert As Boolean = IIf(Torect.Height < 0, True, False)


    '    Using mat As New Matrix
    '        mat.Translate(-bnd.X, -bnd.Y)
    '        gp.Transform(mat)

    '        mat.reset()
    '        Dim sx = Torect.Width / bnd.Width
    '        Dim sy = Torect.Height / bnd.Height
    '        mat.Scale(sx, sy)
    '        gp.Transform(mat)

    '        mat.reset()
    '        mat.Translate(Torect.X, Torect.Y)
    '        gp.Transform(mat)


    '    End Using
    'End Sub
End Class
