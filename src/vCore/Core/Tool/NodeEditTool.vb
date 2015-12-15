Imports Geometry
Imports Graphics


Public Class NodeEditTool
    Implements Itool, IEditor





    Dim v As vCore
    Dim WithEvents dc As IDevice
    Dim MouseLocation As Point
    Private b As Integer = 3

    Dim mainpathBound As Rect

    Dim spath As vPath
    Dim editablepath As NodePath
    Dim noderadious As Single = 3

    Dim nodesel As nodeselection



    Public Sub New(ByRef vew As vCore)
        v = vew
    End Sub
    Public ReadOnly Property Device() As IDevice Implements Itool.Device
        Get
            Return dc
        End Get
    End Property
    Dim s As Integer = 0
    Dim svp As NodePath

    Private Sub dc_MouseDown(e As MouseEvntArg) Handles dc.MouseDown

        Me.mouse_Down(e)
    End Sub

    Private Sub dc_MouseMove(e As MouseEvntArg) Handles dc.MouseMove

        Me.mouse_Move(e)
    End Sub
    Private Sub dc_MouseUp(e As MouseEvntArg) Handles dc.MouseUp

        Me.mouse_Up(e)
    End Sub
    Public Sub DeSelectTool() Implements Itool.DeSelectTool
        dc = Nothing
    End Sub

    Public Sub SelectTool(ByRef d As IDevice) Implements Itool.SelectTool
        dc = d
        v.Editor.setIEdit(Me)


    End Sub

    Public Sub Draw(g As Canvas) Implements IEditor.Draw
        editablepath = Nothing
        If v.Editor.selection.isEmty = False Then
            g.Smooth()
            Dim p As New Pen(Color.BlueColor) ', pth As GraphicsPath = spath.GraphicsPath.ToGraphicsPath

            spath = v.Editor.getSelectionPath()
            editablepath = spath.Path.Clone
            v.View.Memory2screen(editablepath)

            g.DrawPath(editablepath, p)

            DrawNodes(g)





        End If
    End Sub


    Public Sub mouse_Down(e As MouseEvntArg)
        MouseLocation = e.Location
        If editablepath Is Nothing Then
            Dim s = v.Editor.SelectAt(MouseLocation)
            If s <> 0 Then
                v.View.Refresh()
            End If
        Else
            nodesel = GetSelectPoint(MouseLocation)
            If nodesel.selectednode IsNot Nothing Then
                v.View.BufferGraphics.Initialize()
                dc.ActiveScroll = False
            Else
                Dim s = v.Editor.SelectAt(MouseLocation)
                ' If s <> 0 Then
                v.View.Refresh()
                'End If
            End If

        End If


    End Sub

    Public Sub mouse_Move(e As MouseEvntArg)

        If editablepath Is Nothing Or nodesel.selectednode Is Nothing Then Exit Sub

        If e.Button = Windows.Forms.MouseButtons.Left Then
            Dim ml = e.Location
            SetNodePt(nodesel, ml)

            v.View.BufferGraphics.Clear()
            v.View.BufferGraphics.Graphics.DrawPath(editablepath, New Pen(Color.BlackColor))
            DrawNodes(v.View.BufferGraphics.Graphics)

            ' v.View.BufferGraphics.Graphics.DrawPath(Pens.Black, svp.ToGraphicsPath)
            v.View.BufferGraphics.Render()

            ' mdl = e.Location
        End If
    End Sub

    Public Sub mouse_Up(e As MouseEvntArg)
        If editablepath Is Nothing Then Exit Sub
        v.View.Screen2memory(editablepath)
        spath.setPath(editablepath)
        v.View.Refresh()
        dc.ActiveScroll = True
    End Sub
    Private Sub DrawEllipses(g As Canvas, ByVal pts() As Point)
        Dim wh As Integer = b * 2
        For Each pnt As Point In pts
            Dim rect As New Rect(pnt.X - b, pnt.Y - b, wh, wh)
            g.DrawEllipse(rect, , New SolidColorBrush(Color.BrownColor))
        Next
    End Sub

    Private Sub DrawNodes(g As Canvas)
        For Each sp As NodeFigure In editablepath.Figures
            For Each nd As Node In sp.Points
                If nd.Type = PathPointType.None Then
                    g.DrawEllipse(New Rect(New Point(nd.M.X - noderadious, nd.M.Y - noderadious), noderadious * 2, noderadious * 2),
                    , New SolidColorBrush(Color.RedColor))
                Else
                    g.DrawLine(nd.M, nd.C1, New Pen(Color.RedColor, 1))
                    g.DrawLine(nd.M, nd.C2, New Pen(Color.RedColor, 1))

                    g.DrawEllipse(New Rect(New Point(nd.M.X - noderadious, nd.M.Y - noderadious), noderadious * 2, noderadious * 2),
                    , New SolidColorBrush(Color.RedColor))

                    g.DrawEllipse(New Rect(New Point(nd.C1.X - noderadious, nd.C1.Y - noderadious), noderadious * 2, noderadious * 2),
                        New Pen(Color.RedColor, 1), New SolidColorBrush(Color.WhiteColor))

                    g.DrawEllipse(New Rect(New Point(nd.C2.X - noderadious, nd.C2.Y - noderadious), noderadious * 2, noderadious * 2),
                     New Pen(Color.RedColor, 1), New SolidColorBrush(Color.WhiteColor))

                End If

            Next
        Next

    End Sub


    Private Sub SetNodePt(ns As nodeselection, location As Point)
        If ns.typeid = 3 Then
            If ns.selectednode.Type = PathPointType.Sharp Then
                ns.selectednode.C2 = location
            Else
                Dim v1 = New Point(ns.selectednode.C2.X - ns.selectednode.M.X,
                                               ns.selectednode.C2.Y - ns.selectednode.M.Y)
                Dim v2 = New Point(location.X - ns.selectednode.M.X,
                                    location.Y - ns.selectednode.M.Y)

                ' = v1 / v2

                Dim a1 = Math.Atan2(v1.Y, v1.X)
                Dim a2 = Math.Atan2(v2.Y, v2.X)
                Dim a = a2 - a1
                ns.selectednode.C2 = location

                Dim deg As Double = a * (180 / Math.PI)

                ' Dim rt As New RotateTransform(deg, ns.selectednode.M.X, ns.selectednode.M.Y)

                'Dim rt As New Matrix
                'rt.RotateAt(deg, New PointF(ns.selectednode.M.X, ns.selectednode.M.Y))
                'Dim pointArray = {ns.selectednode.C1}
                'rt.TransformPoints(pointArray)

                Dim trans As Matrix = Matrix.Identity
                Dim centerpoint = ns.selectednode.M
                trans.RoatateAt(deg, centerpoint)
                Dim gp = ns.selectednode.C1
                trans.map(gp)
                ns.selectednode.C1 = gp

                ' ns.selectednode.C1 = pointArray(0)
            End If


        ElseIf ns.typeid = 2 Then
            If ns.selectednode.Type = PathPointType.Sharp Then
                ns.selectednode.C1 = location
            Else
                Dim v1 = New Point(ns.selectednode.C1.X - ns.selectednode.M.X,
                                             ns.selectednode.C1.Y - ns.selectednode.M.Y)
                Dim v2 = New Point(location.X - ns.selectednode.M.X,
                                    location.Y - ns.selectednode.M.Y)
                ' = v1 / v2

                Dim a1 = Math.Atan2(v1.Y, v1.X)
                Dim a2 = Math.Atan2(v2.Y, v2.X)
                Dim a = a2 - a1
                ns.selectednode.C1 = location

                Dim deg As Double = a * (180 / Math.PI)

                'Dim rt As New RotateTransform(deg, ns.selectednode.M.X, ns.selectednode.M.Y)
                'ns.selectednode.C2 = rt.Transform(ns.selectednode.C2)
                'Dim rt As New Matrix
                'rt.RotateAt(deg, New PointF(ns.selectednode.M.X, ns.selectednode.M.Y))
                'Dim pointArray = {ns.selectednode.C2}
                'rt.TransformPoints(pointArray)
                'ns.selectednode.C2 = pointArray(0)



                Dim trans As Matrix = Matrix.Identity
                Dim centerpoint = ns.selectednode.M
                trans.RoatateAt(deg, centerpoint)
                Dim gp = ns.selectednode.C2
                trans.map(gp)
                ns.selectednode.C2 = gp
            End If

        ElseIf ns.typeid = 1 Then
            Dim vector = New Point(location.X - ns.selectednode.M.X,
                                    location.Y - ns.selectednode.M.Y)
            ns.selectednode.M = location
            ns.selectednode.C1 = New Point(ns.selectednode.C1.X + vector.X, ns.selectednode.C1.Y + vector.Y)
            ns.selectednode.C2 = New Point(ns.selectednode.C2.X + vector.X, ns.selectednode.C2.Y + vector.Y)

        End If
    End Sub

    Private Function GetSelectPoint(location As Point) As nodeselection
        Dim nds As New nodeselection

        For Each subpth As NodeFigure In editablepath.Figures
            For Each nd As Node In subpth.Points
                Dim bnd As Rect

                If nd.Type <> PathPointType.None Then
                    bnd = getNodeptBound(nd.C2)
                    If bnd.Contain(location) Then
                        nds.selectednode = nd
                        nds.typeid = 3
                        Return nds
                    End If

                    bnd = getNodeptBound(nd.C1)
                    If bnd.Contain(location) Then
                        nds.selectednode = nd
                        nds.typeid = 2
                        Return nds
                    End If
                End If

                bnd = getNodeptBound(nd.M)
                If bnd.Contain(location) Then
                    nds.selectednode = nd
                    nds.typeid = 1
                    Return nds
                End If
            Next
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
        Public typeid As Integer
    End Structure
End Class
