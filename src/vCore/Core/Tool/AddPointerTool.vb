Imports Graphics
Imports Geometry




Public Class AddPointerTool
    Inherits Tool





    ' Dim spath As PathElement
    Dim editablepath As NodePath
    Dim noderadious As Single = 3

    Private SelectedElements As NodePathsCapElement 'to handle transformation of selectionElements

    Public Sub New(ByRef vcore As vCore)
        MyBase.New(vcore)
        SelectedElements = New NodePathsCapElement(Editor)
    End Sub



    Public Overrides Sub Draw(g As Canvas)
        If Not Editor.SelectionHolder.isEmpty Then
            g.Smooth()
            Dim p As New Pen(Color.RedColor)

            editablepath = SelectedElements.GetSelectionSkeliton

            g.DrawPath(editablepath, p)
            DrawNodes(g)

        End If
    End Sub

    Protected Overrides Sub MouseDown(e As MouseEvntArg)
        If Core.Editor.SelectionHolder.isEmpty Then Exit Sub

        Dim mouseLocation = e.Location

        Dim Mindistance = Double.MaxValue
        Dim n1 As Node = Nothing
        Dim n2 As Node = Nothing
        Dim fig As NodeFigure = Nothing

        For Each figure As NodeFigure In editablepath.Figures
            If figure.Points.Count <= 1 Then Continue For
            For nodeIndex As Integer = 0 To figure.Points.Count - 1
                Dim node1, node2 As Node
                If nodeIndex = figure.Points.Count - 1 Then
                    If figure.Closed Then
                        node1 = figure.Points(nodeIndex)
                        node2 = figure.Points(0)
                    Else
                        Continue For
                    End If
                Else
                    node1 = figure.Points(nodeIndex)
                    node2 = figure.Points(nodeIndex + 1)
                End If


                Dim bez As New CubicBezier()
                bez.P1 = New Point(node1.M.X, node1.M.Y)
                bez.C1 = New Point(node1.C2.X, node1.C2.Y)

                bez.C2 = New Point(node2.C1.X, node2.C1.Y)
                bez.P2 = New Point(node2.M.X, node2.M.Y)

                Dim dist = bez.DistancefromPoint(New Point(mouseLocation.X, mouseLocation.Y))

                If dist < Mindistance Then
                    Mindistance = dist
                    n1 = node1
                    n2 = node2
                    fig = figure
                End If
            Next
        Next

        If Mindistance <= 2 Then
            Dim pointerIndex = fig.Points.IndexOf(n1)
            Dim nPointer As New Node

            Dim bez As New CubicBezier()
            bez.P1 = New Point(n1.M.X, n1.M.Y)
            bez.C1 = New Point(n1.C2.X, n1.C2.Y)
            bez.C2 = New Point(n2.C1.X, n2.C1.Y)
            bez.P2 = New Point(n2.M.X, n2.M.Y)

            Dim closept = bez.closestPointToBezier(New Point(mouseLocation.X, mouseLocation.Y))
            Dim left = bez.divLeft(closept)
            Dim right = bez.divRight(closept)

            n1.C2 = New Point(left.C1.X, left.C1.Y)
            nPointer.C1 = New Point(left.C2.X, left.C2.Y)
            nPointer.M = New Point(left.P2.X, left.P2.Y)
            nPointer.C2 = New Point(right.C1.X, right.C1.Y)
            n2.C1 = New Point(right.C2.X, right.C2.Y)

            nPointer.Type = NodeType.Smooth

            fig.Points.Insert(pointerIndex + 1, nPointer)

            'Core.View.Screen2memory(editablepath)
            'spath.setPath(editablepath)
            Core.View.Refresh()
        End If

    End Sub




    Private Sub DrawNodes(g As Canvas)
        Dim Pen As New Pen(Color.RedColor, 1)
        Dim brush As New SolidColorBrush(Color.RedColor)
        For Each sp As NodeFigure In editablepath.Figures
            For Each nd As Node In sp.Points
                ' g.FillEllipse(Brushes.White, nd.M.X - noderadious, nd.M.Y - noderadious, noderadious * 2, noderadious * 2)
                ' g.DrawEllipse(Pen, nd.M.X - noderadious, nd.M.Y - noderadious, noderadious * 2, noderadious * 2)
                Dim rct As New Rect(nd.M.X - noderadious, nd.M.Y - noderadious, noderadious * 2, noderadious * 2)
                g.DrawRect(rct, Pen, brush)
            Next
        Next

    End Sub

    Private Function getNodeptBound(pt As Point) As Rect
        Dim l = New Point(pt.X - Me.noderadious, pt.Y - Me.noderadious)
        Dim w = Me.noderadious * 2
        Return New Rect(l, New Size(w, w))
    End Function
End Class
