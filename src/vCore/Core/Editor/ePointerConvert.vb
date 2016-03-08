Imports Geometry
Imports Graphics



Public Class ePointerConvert
    Implements IEditor

    Dim v As Editor
    Dim spath As PathElement
    Dim editablepath As NodePath
    Dim noderadious As Single = 3

    Dim nodesel As nodeselection

    Public Sub New(ByRef edtr As Editor)
        v = edtr
    End Sub

    Public Sub Draw(g As Canvas) Implements IEditor.Draw
        If v.selection.isEmty = False Then
            g.Smooth()
            Dim p As New Pen(Color.BlueColor)
            spath = v.getSelectionPath()
            editablepath = spath.Path.Clone
            v.View.Memory2screen(editablepath)

            g.DrawPath(editablepath, p)

            DrawNodes(g)



        End If
    End Sub

    Public Sub mouse_Down(e As MouseEvntArg)
        Dim md = e.Location
        nodesel = GetSelectPoint(md)
        If nodesel.selectednode IsNot Nothing Then
            v.View.BufferGraphics.Initialize()

            If nodesel.typeid = 1 Then
                nodesel.selectednode.Type = PathPointType.None
                nodesel.selectednode.C1 = nodesel.selectednode.M
                nodesel.selectednode.C2 = nodesel.selectednode.M
                v.View.BufferGraphics.Clear()
                Dim g = v.View.BufferGraphics.Graphics
                g.drawPath(editablepath, New Pen(Color.BlackColor))
                DrawNodes(v.View.BufferGraphics.Graphics)
                v.View.BufferGraphics.Render()
            End If

        End If

    End Sub

    Public Sub mouse_Move(ByRef e As MouseEvntArg)
        If nodesel.selectednode Is Nothing Then Exit Sub

        If e.Button = Windows.Forms.MouseButtons.Left Then
            Dim ml = e.Location

            If nodesel.typeid = 1 Then
                nodesel.selectednode.Type = PathPointType.Smooth
                nodesel.selectednode.C2 = ml

                Dim m = nodesel.selectednode.M

                Dim v1 = New Point(ml.X - m.X,
                                  ml.Y - m.Y)
                nodesel.selectednode.C1 = New Point(-v1.X + m.X, -v1.Y + m.Y)

            ElseIf nodesel.typeid = 2 Then
                nodesel.selectednode.Type = PathPointType.Sharp
                nodesel.selectednode.C1 = ml

            ElseIf nodesel.typeid = 3 Then
                nodesel.selectednode.Type = PathPointType.Sharp
                nodesel.selectednode.C2 = ml
            End If


            v.View.BufferGraphics.Clear()
            Dim g = v.View.BufferGraphics.Graphics
            g.drawPath(editablepath, New Pen(Color.BlackColor))
            DrawNodes(v.View.BufferGraphics.Graphics)
            v.View.BufferGraphics.Render()
        End If

    End Sub

    Public Sub mouse_Up(e As MouseEvntArg)

        v.View.Screen2memory(editablepath)
        spath.setPath(editablepath)

        v.View.Refresh()
    End Sub


    Private Sub DrawNodes(g As Canvas)
        For Each sp As NodeFigure In editablepath.Figures
            For Each nd As Node In sp.Points
                If nd.Type = PathPointType.None Then

                    g.DrawEllipse(nd.M.X - noderadious, nd.M.Y - noderadious, noderadious * 2, noderadious * 2, , New SolidColorBrush(Color.RedColor))

                Else
                    g.DrawLine(nd.M, nd.C1, New Pen(Color.RedColor))
                    g.DrawLine(nd.M, nd.C2, New Pen(Color.RedColor))

                    g.DrawEllipse(nd.M.X - noderadious, nd.M.Y - noderadious, noderadious * 2, noderadious * 2,
                                    , New SolidColorBrush(Color.RedColor))

                    g.DrawEllipse(nd.C1.X - noderadious, nd.C1.Y - noderadious, noderadious * 2, noderadious * 2,
                                  New Pen(Color.RedColor, 2), New SolidColorBrush(Color.WhiteColor))

                    g.DrawEllipse(nd.C2.X - noderadious, nd.C2.Y - noderadious, noderadious * 2, noderadious * 2,
                                 New Pen(Color.RedColor, 2), New SolidColorBrush(Color.WhiteColor))


                End If

            Next
        Next

    End Sub


    Private Function GetSelectPoint(location As Point) As nodeselection
        Dim nds As New nodeselection

        Dim ndIndex = 0
        Dim figIndex = 0

        For Each subpth As NodeFigure In editablepath.Figures
            ndIndex = 0
            For Each nd As Node In subpth.Points
                Dim bnd As Rect

                If nd.Type <> NodeType.None Then
                    bnd = getNodeptBound(nd.C2)
                    If bnd.Contain(location) Then
                        nds.selectednode = nd
                        nds.typeid = 3
                        nds.nodeIndex = ndIndex
                        nds.figureIndex = figIndex
                        Return nds
                    End If

                    bnd = getNodeptBound(nd.C1)
                    If bnd.Contain(location) Then
                        nds.selectednode = nd
                        nds.typeid = 2
                        nds.nodeIndex = ndIndex
                        nds.figureIndex = figIndex
                        Return nds
                    End If
                End If

                bnd = getNodeptBound(nd.M)
                If bnd.Contain(location) Then
                    nds.selectednode = nd
                    nds.typeid = 1
                    nds.nodeIndex = ndIndex
                    nds.figureIndex = figIndex
                    Return nds
                End If
                ndIndex += 1
            Next
            figIndex += 1
        Next

        Return nds
    End Function

    Private Function getNodeptBound(pt As Point) As Rect
        Dim l = New Point(pt.X - Me.noderadious, pt.Y - Me.noderadious)
        Dim w = Me.noderadious * 2
        Return New Rect(l, New Size(w, w))
    End Function

    Private Structure nodeselection
        Public selectednode As Node
        Public figureIndex As Integer
        Public nodeIndex As Integer
        Public typeid As Integer
    End Structure
End Class
