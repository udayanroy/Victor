Imports Geometry
Imports Graphics

Public Class RotateTool
    Inherits Tool

    Private b As Integer = 3
    Private wh As Integer = b * 2



    Dim MouseLocation As Point
    Dim MouseDownLocation As Point
    'Dim mdl As Point
    'Dim md As Point

    Dim s As Integer = 0
    Dim svp As NodePath
    Dim mda As Single
    Dim mdp As Point
    Dim rotating As Boolean = False
    Dim mainpathBound As Rect

    Dim SelectedElements As TransformCapElement 'to handle transformation of selectionElements

    Public Sub New(ByRef Core As vCore)
        MyBase.New(Core)
        SelectedElements = New TransformCapElement(Editor)
    End Sub


    Public Overrides Sub Draw(canvas As Canvas)
        If Not Editor.SelectionHolder.isEmpty Then

            Dim p As New Pen(Color.RedColor) 'Pen should be load from configData
            Dim pth As New NodePath

            'Get the combine bounds of all selected Elements
            mainpathBound = SelectedElements.GetSelectionsBound

            'Draw bounds of all selected Elements
            canvas.DrawRects(SelectedElements.GetSelectionsBounds.ToArray, p)
            canvas.DrawRect(mainpathBound, p) 'Draw combine bounds of all selected Elements

            Dim bound = mainpathBound

            Dim pointers() As Rect = {getRect(bound.X, bound.Y),
                                            getRect(bound.X, bound.Y + bound.Height),
                                            getRect(bound.X + bound.Width, bound.Y),
                                            getRect(bound.X + bound.Width, bound.Y + bound.Height)}


            'Draw rotate pointers
            canvas.Smooth()
            canvas.DrawEllipses(pointers, New Pen(Color.BlackColor),
                            New SolidColorBrush(Color.WhiteColor))

        End If
    End Sub

    Protected Overrides Sub MouseDown(e As MouseEvntArg)
        rotating = False
        MouseDownLocation = e.Location
        MouseLocation = e.Location

        If MouseButton.Left Then


            If Not Editor.SelectionHolder.isEmpty Then
                'Dim pth As New NodePath
                'Dim rf As Rect = Core.Editor.getSelectionPath().Path.GetTightBound
                'pth.AddRectangle(rf)
                'Core.View.Memory2screen(pth)
                'Dim bound = pth.GetTightBound

                Dim hit = Me.hittest(e.Location, mainpathBound)

                If hit <> -1 Then
                    mdp = Me.MiddlePoint(mainpathBound)
                    mda = Me.Angle(mdp, e.Location)

                    svp = SelectedElements.GetSelectionSkeliton
                    Core.View.BufferGraphics.Initialize()
                    rotating = True
                    Device.ActiveScroll = False
                Else
                    s = Core.Editor.SelectAt(MouseLocation)
                    If s <> 0 Then
                        svp = SelectedElements.GetSelectionSkeliton

                        Core.Editor.View.BufferGraphics.Initialize()
                        Device.ActiveScroll = False
                    End If
                End If

            Else
                s = Core.Editor.SelectAt(MouseLocation)
                If s <> 0 Then
                    svp = SelectedElements.GetSelectionSkeliton

                    Core.Editor.View.BufferGraphics.Initialize()
                    Device.ActiveScroll = False
                End If

            End If

        End If
    End Sub

    Protected Overrides Sub MouseMove(e As MouseEvntArg)
        If e.Button = MouseButton.Left And rotating Then
            Dim angl = Me.Angle(mdp, e.Location)
            Core.View.BufferGraphics.Clear()
            Dim mat As Matrix = Matrix.Identity
            Dim tmpth = svp.Clone
            mat.RotateAt(angl - mda, mdp)
            tmpth.Transform(mat)
            Core.View.BufferGraphics.Graphics.DrawPath(tmpth, New Pen(Color.BrownColor))
            Core.View.BufferGraphics.Render()

        End If
    End Sub

    Protected Overrides Sub MouseUp(e As MouseEvntArg)
        If rotating Then
            Dim angl = Me.Angle(mdp, e.Location)
            Dim cnt As Point = Me.MiddlePoint(mainpathBound)
            'Dim mat As Matrix = Matrix.Identity
            'mat.RotateAt(angl - mda, cnt)
            'Core.Editor.getSelectionPath.Path.Transform(mat)
            Dim Rotation As New RotateTransform(angl - mda, cnt)
            SelectedElements.ApplyTransform(Rotation)

            Device.ActiveScroll = True
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
