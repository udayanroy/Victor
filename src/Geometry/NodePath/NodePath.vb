

Public Class NodePath
    Private _FigureList As New List(Of NodeFigure)

    Private Const fmindist = 0.4


    Public Sub New()

    End Sub

    'Add shapes to the Path.
    'Every shape has added will be a NodeFigure.

    'Add a NodeFigure of line.
    Public Sub AddLine(ByVal p1 As Point, ByVal p2 As Point)
        Dim sp As New NodeFigure

        Dim pp1 As New Node(p1)
        sp.Points.Add(pp1)

        Dim pp2 As New Node(p2)
        sp.Points.Add(pp2)

        Me._FigureList.Add(sp)
    End Sub
    Public Sub AddLines(ByVal points() As Point)
        Dim sp As New NodeFigure

        For Each p As Point In points
            Dim pp As New Node(p)
            sp.Points.Add(pp)
        Next

        Me._FigureList.Add(sp)
    End Sub

    'Add a NodeFigure of Rectangle
    Public Sub AddRectangle(ByVal rect As Rect)
        Me.AddRectangle(rect.X, rect.Y, rect.Width, rect.Height)
    End Sub
    Public Sub AddRectangle(ByVal x As Single, ByVal y As Single, ByVal Width As Single, ByVal Height As Single)
        Dim sp As New NodeFigure
        sp.Closed = True

        Dim p1 As New Node(New Point(x, y))
        sp.Points.Add(p1)
        Dim p2 As New Node(New Point(x + Width, y))
        sp.Points.Add(p2)
        Dim p3 As New Node(New Point(x + Width, y + Height))
        sp.Points.Add(p3)
        Dim p4 As New Node(New Point(x, y + Height))
        sp.Points.Add(p4)

        Me._FigureList.Add(sp)
    End Sub
    Public Sub AddRectangle(ByVal p1 As Point, ByVal p2 As Point)
        Me.AddRectangle(p1.X, p1.Y, p2.X - p1.X, p2.Y - p1.Y)
    End Sub

    'Add a NodeFigure of Ellipse.
    Public Sub AddEllipse(ByVal p1 As Point, ByVal p2 As Point)
        Me.AddEllipse(p1.X, p1.Y, p2.X - p1.X, p2.Y - p1.Y)
    End Sub
    Public Sub AddEllipse(ByVal x1 As Single, ByVal y1 As Single, ByVal width As Single, ByVal height As Single)

        Dim pathkpa = 0.5522847498
        Dim w = width
        Dim h = height

        Dim w2 = w / 2
        Dim h2 = h / 2

        Dim wk = w2 * pathkpa
        Dim hk = h2 * pathkpa

        Dim sp As New NodeFigure
        sp.Closed = True

        Dim p1 As New Node(New Point(x1 + w2, y1), New Point(x1 + w2 - wk, y1), _
                                New Point(x1 + w2 + wk, y1), NodeType.Smooth)
        sp.Points.Add(p1)
        Dim p2 As New Node(New Point(x1 + w, y1 + h2), New Point(x1 + w, y1 + h2 - hk), _
                                New Point(x1 + w, y1 + h2 + hk), NodeType.Smooth)
        sp.Points.Add(p2)
        Dim p3 As New Node(New Point(x1 + w2, y1 + h), New Point(x1 + w2 + wk, y1 + h), _
                                New Point(x1 + w2 - wk, y1 + h), NodeType.Smooth)
        sp.Points.Add(p3)
        Dim p4 As New Node(New Point(x1, y1 + h2), New Point(x1, y1 + h2 + hk), _
                                New Point(x1, y1 + h2 - hk), NodeType.Smooth)
        sp.Points.Add(p4)

        Me._FigureList.Add(sp)
    End Sub
    Public Sub AddEllipse(ByVal rect As Rect)
        Me.AddEllipse(rect.X, rect.Y, rect.Width, rect.Height)
    End Sub

    'Add a NodeFigure
    Public Sub AddNodeFigure(ByVal path As NodeFigure)
        Me._FigureList.Add(path)
    End Sub

    Public Function Nodes() As IList(Of Node)

        Dim np As Integer = 0

        For Each sp As NodeFigure In Me._FigureList
            np += sp.CountPoints
        Next

        Dim parray As New List(Of Node)(np)
        For Each sp As NodeFigure In Me._FigureList
            parray.AddRange(sp.Points)
        Next
        Return parray
    End Function

    Public ReadOnly Property Figures As List(Of NodeFigure)
        Get
            Return Me._FigureList
        End Get
    End Property

    Public ReadOnly Property Items(ByVal index As Integer) As NodeFigure
        Get
            Return Me._FigureList.Item(index)
        End Get
    End Property

    Public Function CountItems() As Integer
        Return Me._FigureList.Count
    End Function

    Public Sub Transform(ByVal mat As Matrix)
        For Each sp As NodeFigure In Me._FigureList
            sp.Transform(mat)

        Next
    End Sub

    Public Function GetTightBound() As Rect
        Dim flag As Boolean = True
        Dim bound As Rect
        For Each sp As NodeFigure In Me._FigureList
            If flag Then
                bound = sp.GetTightBound
                flag = False
            Else
                bound = bound.union(sp.GetTightBound)
            End If
        Next
        Return bound
    End Function

    Public Function GetBound() As Rect
        Dim flag As Boolean = True
        Dim bound As Rect
        For Each sp As NodeFigure In Me._FigureList
            If flag Then
                bound = sp.GetBound
                flag = False
            Else
                bound = bound.union(sp.GetBound)
            End If
        Next
        Return bound
    End Function

    Public Function Clone() As NodePath
        Dim gp As New NodePath
        For Each sp As NodeFigure In Me._FigureList
            gp._FigureList.Add(sp.Clone)
        Next
        Return gp
    End Function



    Public Function isOutlineVisible(ByVal p As Point, ByVal width As Single) As Boolean
        Dim mindist As Double = Double.MaxValue

        For Each sp As NodeFigure In Me._FigureList
            Dim nb = sp.countBezier
            For i As Integer = 0 To nb - 1
                Dim bez = sp.getBezier(i)
                Dim dist = bez.DistancefromPoint(p)
                mindist = Math.Min(dist, mindist)

            Next
        Next

        Return IIf(mindist <= fmindist, True, False)
    End Function

    Public Function isVisible(ByVal p As Point) As Boolean
        Dim nint As Integer = 0
        Dim bound = Me.GetBound
        Dim p1 = bound.Location - New Point(20, 20)

        For Each sp As NodeFigure In Me._FigureList
            Dim nb = sp.countBezier
            For i As Integer = 0 To nb - 1
                Dim bez = sp.getBezier(i)
                nint += Intersection.intersectBezier3Line(bez.P1, bez.C1, bez.C2, bez.P2, p1, p).nCount()
            Next
        Next

        If nint Mod 2 = 0 Then
            Return False
        Else
            Return True
        End If
    End Function

End Class
