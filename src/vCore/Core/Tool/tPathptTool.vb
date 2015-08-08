Imports System.Drawing
Imports System.Drawing.Drawing2D

Public Class NodeEditTool
    Implements Itool, Iedtr





    Dim v As vCore
    Dim WithEvents dc As advancedPanel
    Dim mdl As Point
    Dim md As Point
    Private b As Integer = 3

    Dim mainpathBound As RectangleF

    Dim spath As vPath
    Dim editablepath As GPath
    Dim noderadious As Single = 3

    Dim nodesel As nodeselection



    Public Sub New(ByRef vew As vCore)
        v = vew
    End Sub
    Public ReadOnly Property Device() As advancedPanel Implements Itool.Device
        Get
            Return dc
        End Get
    End Property
    Dim s As Integer = 0
    Dim svp As GraphicsPath

    Private Sub dc_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dc.MouseDown

        v.Editor.mouse_Down(e)
    End Sub

    Private Sub dc_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dc.MouseMove

        v.Editor.mouse_Move(e)
    End Sub
    Private Sub dc_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dc.MouseUp

        v.Editor.mouse_Up(e)
    End Sub
    Public Sub DeSelectTool() Implements Itool.DeSelectTool
        dc = Nothing
    End Sub

    Public Sub SelectTool(ByRef d As advancedPanel) Implements Itool.SelectTool
        dc = d
        v.Editor.setIEdit(Me)
    End Sub

    Public Sub Draw(ByRef g As Graphics) Implements Iedtr.Draw
        If v.Editor.selection.isEmty = False Then
            g.SmoothingMode = SmoothingMode.AntiAlias
            Using p As New Pen(Color.SkyBlue) ', pth As GraphicsPath = spath.GraphicsPath.ToGraphicsPath
                spath = v.Editor.getSelectionPath()
                editablepath = spath.GraphicsPath.Clone
                v.View.mem2DcGPath(editablepath)

                editablepath.drawPath(g, p)

                DrawNodes(g)



            End Using

        End If
    End Sub

    Public Sub mouse_Down(ByRef e As Windows.Forms.MouseEventArgs) Implements Iedtr.mouse_Down
        Dim md = e.Location
        nodesel = GetSelectPoint(md)
        If nodesel.selectednode IsNot Nothing Then v.View.BufferGraphics.Initialize()
    End Sub

    Public Sub mouse_Move(ByRef e As Windows.Forms.MouseEventArgs) Implements Iedtr.mouse_Move
        If nodesel.selectednode Is Nothing Then Exit Sub

        If e.Button = Windows.Forms.MouseButtons.Left Then
            Dim ml = e.Location
            SetNodePt(nodesel, ml)

            v.View.BufferGraphics.Clear()
            editablepath.drawPath(v.View.BufferGraphics.Graphics, Pens.Black)
            DrawNodes(v.View.BufferGraphics.Graphics)

            ' v.View.BufferGraphics.Graphics.DrawPath(Pens.Black, svp.ToGraphicsPath)
            v.View.BufferGraphics.Render()

            ' mdl = e.Location
        End If
    End Sub

    Public Sub mouse_Up(ByRef e As Windows.Forms.MouseEventArgs) Implements Iedtr.mouse_Up
        v.View.Dc2MemGPath(editablepath)
        spath.setPath(editablepath)
        v.View.Refresh()
    End Sub
    Private Sub DrawEllipses(ByVal g As Graphics, ByVal pts() As PointF)
        Dim wh As Integer = b * 2
        For Each pnt As PointF In pts
            Dim rect As New Rectangle(pnt.X - b, pnt.Y - b, wh, wh)
            g.FillEllipse(Brushes.Brown, rect)
        Next
    End Sub

    Private Sub DrawNodes(g As System.Drawing.Graphics)
        For Each sp As SubPath In editablepath.subpaths
            For Each nd As PathPoint In sp.Points
                If nd.Type = PathPointType.None Then
                    g.FillEllipse(Brushes.Red, nd.M.X - noderadious, nd.M.Y - noderadious, noderadious * 2, noderadious * 2)

                Else
                    g.DrawLine(New Pen(Brushes.Red, 1), nd.M, nd.C1)
                    g.DrawLine(New Pen(Brushes.Red, 1), nd.M, nd.C2)

                    g.FillEllipse(Brushes.Red, nd.M.X - noderadious, nd.M.Y - noderadious, noderadious * 2, noderadious * 2)

                    g.DrawEllipse(New Pen(Brushes.Red, 2), nd.C1.X - noderadious, nd.C1.Y - noderadious, noderadious * 2, noderadious * 2)
                    g.FillEllipse(Brushes.White, nd.C1.X - noderadious, nd.C1.Y - noderadious, noderadious * 2, noderadious * 2)
                    g.DrawEllipse(New Pen(Brushes.Red, 2), nd.C2.X - noderadious, nd.C2.Y - noderadious, noderadious * 2, noderadious * 2)
                    g.FillEllipse(Brushes.White, nd.C2.X - noderadious, nd.C2.Y - noderadious, noderadious * 2, noderadious * 2)

                End If

            Next
        Next

    End Sub


    Private Sub SetNodePt(ns As nodeselection, location As Point)
        If ns.typeid = 3 Then
            If ns.selectednode.Type = PathPointType.Sharp Then
                ns.selectednode.C2 = location
            Else
                Dim v1 = New PointF(ns.selectednode.C2.X - ns.selectednode.M.X,
                                               ns.selectednode.C2.Y - ns.selectednode.M.Y)
                Dim v2 = New PointF(location.X - ns.selectednode.M.X,
                                    location.Y - ns.selectednode.M.Y)

                ' = v1 / v2

                Dim a1 = Math.Atan2(v1.Y, v1.X)
                Dim a2 = Math.Atan2(v2.Y, v2.X)
                Dim a = a2 - a1
                ns.selectednode.C2 = location

                Dim deg As Double = a * (180 / Math.PI)

                ' Dim rt As New RotateTransform(deg, ns.selectednode.M.X, ns.selectednode.M.Y)
                Dim rt As New Matrix
                rt.RotateAt(deg, New PointF(ns.selectednode.M.X, ns.selectednode.M.Y))
                Dim pointArray = {ns.selectednode.C1}
                rt.TransformPoints(pointArray)
                ns.selectednode.C1 = pointArray(0)
            End If


        ElseIf ns.typeid = 2 Then
            If ns.selectednode.Type = PathPointType.Sharp Then
                ns.selectednode.C1 = location
            Else
                Dim v1 = New PointF(ns.selectednode.C1.X - ns.selectednode.M.X,
                                             ns.selectednode.C1.Y - ns.selectednode.M.Y)
                Dim v2 = New PointF(location.X - ns.selectednode.M.X,
                                    location.Y - ns.selectednode.M.Y)
                ' = v1 / v2

                Dim a1 = Math.Atan2(v1.Y, v1.X)
                Dim a2 = Math.Atan2(v2.Y, v2.X)
                Dim a = a2 - a1


                Dim deg As Double = a * (180 / Math.PI)

                'Dim rt As New RotateTransform(deg, ns.selectednode.M.X, ns.selectednode.M.Y)
                'ns.selectednode.C2 = rt.Transform(ns.selectednode.C2)
                Dim rt As New Matrix
                rt.RotateAt(deg, New PointF(ns.selectednode.M.X, ns.selectednode.M.Y))
                Dim pointArray = {ns.selectednode.C2}
                rt.TransformPoints(pointArray)
                ns.selectednode.C2 = pointArray(0)

                ns.selectednode.C1 = location
            End If

        ElseIf ns.typeid = 1 Then
            Dim vector = New PointF(location.X - ns.selectednode.M.X,
                                    location.Y - ns.selectednode.M.Y)
            ns.selectednode.M = location
            ns.selectednode.C1 = New PointF(ns.selectednode.C1.X + vector.X, ns.selectednode.C1.Y + vector.Y)
            ns.selectednode.C2 = New PointF(ns.selectednode.C2.X + vector.X, ns.selectednode.C2.Y + vector.Y)

        End If
    End Sub

    Private Function GetSelectPoint(location As Point) As nodeselection
        Dim nds As New nodeselection

        For Each subpth As SubPath In editablepath.subpaths
            For Each nd As PathPoint In subpth.Points
                Dim bnd As Rectangle

                If nd.Type <> PathPointType.None Then
                    bnd = getNodeptBound(nd.C2)
                    If bnd.Contains(location) Then
                        nds.selectednode = nd
                        nds.typeid = 3
                        Return nds
                    End If

                    bnd = getNodeptBound(nd.C1)
                    If bnd.Contains(location) Then
                        nds.selectednode = nd
                        nds.typeid = 2
                        Return nds
                    End If
                End If

                bnd = getNodeptBound(nd.M)
                If bnd.Contains(location) Then
                    nds.selectednode = nd
                    nds.typeid = 1
                    Return nds
                End If
            Next
        Next



        Return nds
    End Function


    Private Function getNodeptBound(pt As PointF) As Rectangle
        Dim l = New Point(pt.X - Me.noderadious, pt.Y - Me.noderadious)
        Dim w = Me.noderadious * 2
        Return New Rectangle(l, New Size(w, w))
    End Function
    Private Structure nodeselection
        Public selectednode As PathPoint
        Public typeid As Integer
    End Structure
End Class
