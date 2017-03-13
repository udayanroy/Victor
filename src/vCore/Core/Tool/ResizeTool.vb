Imports Geometry
Imports Graphics


Public Class ResizeTool
    Inherits Tool




    Private bound As Rect
    Dim noderadious As Single = 3
    Private nodewidth As Integer = noderadious * 2

    Dim MouseLocation As Point
    Dim MouseDownLocation As Point
    'Dim mdl As Point
    'Dim md As Point

    Dim s As Integer = 0
    Dim svp As NodePath

    Dim sizing As Boolean = False
    Dim hit As Integer = -1

    Dim nrect As Rect

    Dim SelectedElements As TransformCapElement 'to handle transformation of selectionElements

    Public Sub New(ByRef core As vCore)
        MyBase.New(core)
        SelectedElements = New TransformCapElement(Editor)
    End Sub

    Public Overrides Sub Draw(canvas As Canvas)

        If Not Editor.SelectionHolder.isEmpty Then
            canvas.Smooth()

            Dim p As New Pen(Color.RedColor) 'Pen should be load from configData

            'Get the combine bounds of all selected Elements
            bound = SelectedElements.GetSelectionsBound

            'Draw bounds of all selected Elements
            canvas.DrawRects(SelectedElements.GetSelectionsBounds.ToArray, p)
            canvas.DrawRect(bound, p) 'Draw combine bounds of all selected Elements


            Dim pointers() As Rect = {getRect(bound.X, bound.Y),
                                  getRect(bound.X, bound.Y + bound.Height / 2),
                                  getRect(bound.X, bound.Y + bound.Height),
                                  getRect(bound.X + bound.Width / 2, bound.Y),
                                  getRect(bound.X + bound.Width / 2, bound.Y + bound.Height),
                                  getRect(bound.X + bound.Width, bound.Y),
                                  getRect(bound.X + bound.Width, bound.Y + bound.Height / 2),
                                  getRect(bound.X + bound.Width, bound.Y + bound.Height)}





            'Draw resize pointers
            canvas.DrawRects(pointers, New Pen(Color.BlackColor),
                             New SolidColorBrush(Color.WhiteColor))



        End If
    End Sub

    Private Function getRect(ByVal x As Integer, ByVal y As Integer) As Rect
        Return New Rect(New Point(x - noderadious, y - noderadious), nodewidth, nodewidth)
    End Function

    Private Function getRect(ByVal x As Integer, ByVal y As Integer, radious As Integer) As Rect
        Return New Rect(New Point(x - radious, y - radious), radious * 2, radious * 2)
    End Function

    Protected Overrides Sub MouseDown(e As MouseEvntArg)



        If Core.Editor.SelectionHolder.isEmpty Then Exit Sub

        'md = e.Location
        'mdl = md
        MouseDownLocation = e.Location
        MouseLocation = e.Location



        hit = Me.hittest(e.Location, bound)
        If hit <> -1 Then
            sizing = True
            'Debug.Print("true")
            svp = SelectedElements.GetSelectionSkeliton
            Core.Editor.View.BufferGraphics.Initialize()

            Device.ActiveScroll = False
        Else

            sizing = False
            ' s = Core.Editor.SelectAt(mdl)
        End If


    End Sub

    Public Sub mouse_Move(e As MouseEvntArg)
        If e.Button = Windows.Forms.MouseButtons.Left Then
            If hit <> -1 Then
                Core.Editor.View.BufferGraphics.Clear()
                Dim gp = Me.DoAction(e.Location, hit)
                Dim g = Core.Editor.View.BufferGraphics.Graphics
                g.DrawPath(gp, New Pen(Color.RedColor))
                Core.Editor.View.BufferGraphics.Render()




            End If
        End If
    End Sub

    Public Sub mouse_Up(e As MouseEvntArg)
        If sizing Then
            Dim tr As Rect

            Dim rloc = nrect.Location
            ' Core.View.Screen2memory(rloc)
            tr.Location = rloc

            Dim pk = New Point(nrect.X + nrect.Width, nrect.Y + nrect.Height)


            tr.Width = pk.X - tr.X
            tr.Height = pk.Y - tr.Y


            SelectedElements.ApplyTransform(GetScaleTransform(bound, tr))

            Device.ActiveScroll = True
            'svp.Dispose()
        End If


        Core.View.Refresh()
    End Sub


    Private Function hittest(p As Point, b As Rect) As Integer

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

            Dim rd As New Rect(New Point(p1.X, p1.Y), _
                                      (p2.X - p1.X), (p2.Y - p1.Y))

            Dim rtn As Rect


            If ((Math.Abs(rd.Width * n)) <= Math.Abs(rd.Height)) Then
                Dim width = rd.Width
                Dim height As Double
                If rd.Height < 0 Then
                    height = -Math.Abs(rd.Width * n)
                Else
                    height = Math.Abs(rd.Width * n)
                End If

                Dim Location = New Point(p1.X, p2.Y - height)

                rtn = New Rect(Location, width, height)
            Else
                Dim height = rd.Height
                Dim width As Double
                If rd.Width < 0 Then
                    width = -Math.Abs(rd.Height / n)
                Else
                    width = Math.Abs(rd.Height / n)
                End If

                Dim Location = New Point(p2.X - width, p1.Y)

                rtn = New Rect(Location, width, height)
            End If




            Dim gp = svp.Clone
            ScalePath(gp, rtn)
            nrect = rtn
            Return gp
        ElseIf hit = 1 Then

            Dim bnd = svp.GetTightBound
            Dim l = (point.Y - bnd.Y)

            Dim gp = svp.Clone

            Dim trect As New Rect(New Point(point.X, bnd.Y), (bnd.X - point.X) + bnd.Width, bnd.Height)

            ScalePath(gp, trect)

            nrect = trect
            Return gp
        ElseIf hit = 2 Then


            Dim bnd = svp.GetTightBound
            Dim p1 = point
            Dim p2 = New Point(bnd.Right, bnd.Top)

            Dim n = bnd.Height / bnd.Width

            Dim rd As New Rect(New Point(p1.X, p2.Y), _
                                      (p2.X - p1.X), (p1.Y - p2.Y))

            Dim rtn As Rect


            If ((Math.Abs(rd.Width * n)) <= Math.Abs(rd.Height)) Then
                Dim width = rd.Width
                Dim height As Double
                If rd.Height < 0 Then
                    height = -Math.Abs(rd.Width * n)
                Else
                    height = Math.Abs(rd.Width * n)
                End If

                Dim Location = New Point(p1.X, p2.Y)
                rtn = New Rect(Location, width, height)
            Else
                Dim height = rd.Height
                Dim Width As Double
                If rd.Width < 0 Then
                    Width = -Math.Abs(rd.Height / n)
                Else
                    Width = Math.Abs(rd.Height / n)
                End If

                Dim Location = New Point(p2.X - Width, p2.Y)
                rtn = New Rect(Location, Width, height)
            End If




            Dim gp = svp.Clone
            ScalePath(gp, rtn)
            nrect = rtn
            Return gp
        ElseIf hit = 3 Then

            Dim bnd = svp.GetTightBound
            Dim l = (point.Y - bnd.Y)

            Dim gp = svp.Clone

            Dim trect As New Rect(New Point(bnd.X, point.Y), bnd.Width, (bnd.Y - point.Y) + bnd.Height)
            ScalePath(gp, trect)
            nrect = trect
            Return gp

        ElseIf hit = 4 Then


            Dim bnd = svp.GetTightBound
            Dim l = (point.Y - bnd.Y)

            Dim gp = svp.Clone

            Dim trect As New Rect(New Point(bnd.X, bnd.Y), bnd.Width, l)
            ScalePath(gp, trect)
            nrect = trect
            Return gp

        ElseIf hit = 5 Then

            Dim p1 = point

            Dim bnd = svp.GetTightBound
            Dim p2 = New Point(bnd.Left, bnd.Bottom) 'New Point(bnd.X + bnd.Width, bnd.Y + bnd.Height)

            Dim n = bnd.Height / bnd.Width

            Dim rd As New Rect(New Point(p2.X, p1.Y), _
                                      (p1.X - p2.X), (p2.Y - p1.Y))

            Dim rtn As Rect


            If ((Math.Abs(rd.Width * n)) <= Math.Abs(rd.Height)) Then
                Dim Width = rd.Width
                Dim Height As Double
                If rd.Height < 0 Then
                    Height = -Math.Abs(rd.Width * n)
                Else
                    Height = Math.Abs(rd.Width * n)
                End If

                Dim Location = New Point(p2.X, p2.Y - Height)
                rtn = New Rect(Location, Width, Height)
            Else
                Dim Height = rd.Height
                Dim Width As Double
                If rd.Width < 0 Then
                    Width = -Math.Abs(rd.Height / n)
                Else
                    Width = Math.Abs(rd.Height / n)
                End If

                Dim Location = New Point(p2.X, p1.Y)
                rtn = New Rect(Location, Width, height)
            End If




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

            Dim trect As New Rect(New Point(bnd.X, bnd.Y), l, bnd.Height)
            ScalePath(gp, trect)
            nrect = trect
            Return gp


        ElseIf hit = 7 Then

            Dim bnd = svp.GetTightBound
            Dim p1 = bnd.Location

            Dim p2 = point  'New Point(bnd.X + bnd.Width, bnd.Y + bnd.Height)

            Dim n = bnd.Height / bnd.Width

            Dim rd As New Rect(p1, p2)

            Dim rtn As Rect

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




            Dim gp = svp.Clone
            ScalePath(gp, rtn)
            nrect = rtn
            Return gp

        Else
            Return Nothing
        End If

    End Function

    Private Sub ScalePath(gp As NodePath, Torect As Rect)
        Dim bnd = gp.GetTightBound
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

    'Private Sub ScaleGPath(ByRef gp As GPath, ByVal Torect As RectangleF)
    '    Dim bnd = gp.GetTightBound
    '    Dim xinvert As Boolean = IIf(Torect.Width < 0, True, False)
    '    Dim yinvert As Boolean = IIf(Torect.Height < 0, True, False)


    '    Using mat As New Matrix
    '        mat.Translate(-bnd.X, -bnd.Y)
    '        gp.Transform(mat)

    '        mat.Reset()
    '        Dim sx = Torect.Width / bnd.Width
    '        Dim sy = Torect.Height / bnd.Height
    '        mat.Scale(sx, sy)
    '        gp.Transform(mat)

    '        mat.Reset()
    '        mat.Translate(Torect.X, Torect.Y)
    '        gp.Transform(mat)


    '    End Using
    'End Sub
    Private Function GetScaleTransform(bound As Rect, ByVal Torect As Rect) As Transform
        Dim bnd = bound
        Dim xinvert As Boolean = IIf(Torect.Width < 0, True, False)
        Dim yinvert As Boolean = IIf(Torect.Height < 0, True, False)

        Dim transform As New TransformGroup


        Dim translate As New TranslateTransform(-bnd.X, -bnd.Y)
        transform.Items.Add(translate)

        Dim sx = Torect.Width / bnd.Width
        Dim sy = Torect.Height / bnd.Height
        Dim scale As New ScaleTransform(sx, sy)
        transform.Items.Add(scale)

        Dim retranslate As New TranslateTransform(Torect.X, Torect.Y)
        transform.Items.Add(retranslate)

        Return transform
    End Function
End Class
