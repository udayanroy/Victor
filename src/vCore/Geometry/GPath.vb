Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports Geom.Geometry

<Serializable()> Public Class GPath
    Private spaths As New List(Of SubPath)

    Public Sub New()

    End Sub

    'Add shapes to the Path.
    'Every shape has added will be a SubPath.

    'Add a subpath of line.
    Public Sub AddLine(ByVal p1 As PointF, ByVal p2 As PointF)
        Dim sp As New SubPath

        Dim pp1 As New PathPoint(p1)
        sp.Points.Add(pp1)

        Dim pp2 As New PathPoint(p2)
        sp.Points.Add(pp2)

        Me.spaths.Add(sp)
    End Sub
    Public Sub AddLines(ByVal points() As PointF)
        Dim sp As New SubPath

        For Each p As PointF In points
            Dim pp As New PathPoint(p)
            sp.Points.Add(pp)
        Next

        Me.spaths.Add(sp)
    End Sub

    'Add a subpath of Rectangle
    Public Sub AddRectangle(ByVal rect As RectangleF)
        Me.AddRectangle(rect.X, rect.Y, rect.Width, rect.Height)
    End Sub
    Public Sub AddRectangle(ByVal x As Single, ByVal y As Single, ByVal Width As Single, ByVal Height As Single)
        Dim sp As New SubPath
        sp.Closed = True

        Dim p1 As New PathPoint(New PointF(x, y))
        sp.Points.Add(p1)
        Dim p2 As New PathPoint(New Point(x + Width, y))
        sp.Points.Add(p2)
        Dim p3 As New PathPoint(New Point(x + Width, y + Height))
        sp.Points.Add(p3)
        Dim p4 As New PathPoint(New Point(x, y + Height))
        sp.Points.Add(p4)

        Me.spaths.Add(sp)
    End Sub
    Public Sub AddRectangle(ByVal p1 As PointF, ByVal p2 As PointF)
        Me.AddRectangle(p1.X, p1.Y, p2.X - p1.X, p2.Y - p1.Y)
    End Sub

    'Add a subpath of Ellipse.
    Public Sub AddEllipse(ByVal p1 As PointF, ByVal p2 As PointF)
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

        Dim sp As New SubPath
        sp.Closed = True

        Dim p1 As New PathPoint(New PointF(x1 + w2, y1), New PointF(x1 + w2 - wk, y1), _
                                New PointF(x1 + w2 + wk, y1), PathPointType.Smooth)
        sp.Points.Add(p1)
        Dim p2 As New PathPoint(New PointF(x1 + w, y1 + h2), New PointF(x1 + w, y1 + h2 - hk), _
                                New PointF(x1 + w, y1 + h2 + hk), PathPointType.Smooth)
        sp.Points.Add(p2)
        Dim p3 As New PathPoint(New PointF(x1 + w2, y1 + h), New PointF(x1 + w2 + wk, y1 + h), _
                                New PointF(x1 + w2 - wk, y1 + h), PathPointType.Smooth)
        sp.Points.Add(p3)
        Dim p4 As New PathPoint(New PointF(x1, y1 + h2), New PointF(x1, y1 + h2 + hk), _
                                New PointF(x1, y1 + h2 - hk), PathPointType.Smooth)
        sp.Points.Add(p4)

        Me.spaths.Add(sp)
    End Sub
    Public Sub AddEllipse(ByVal rect As RectangleF)
        Me.AddEllipse(rect.X, rect.Y, rect.Width, rect.Height)
    End Sub

    'Add a SubPath
    Public Sub AddSubPath(ByVal path As SubPath)
        Me.spaths.Add(path)
    End Sub

    Public Function Points() As PathPoint()

        Dim np As Integer = 0

        For Each sp As SubPath In Me.spaths
            np += sp.CountPoints
        Next

        Dim parray As New List(Of PathPoint)(np)

        For Each sp As SubPath In Me.spaths
            parray.AddRange(sp.Points)
        Next
        Return Nothing
    End Function

    Public ReadOnly Property Items(ByVal index As Integer) As SubPath
        Get
            Return Me.spaths.Item(index)
        End Get
    End Property

    Public ReadOnly Property subpaths As List(Of SubPath)
        Get
            Return Me.spaths
        End Get
    End Property

    Public Sub Transform(ByVal mat As Matrix)

        For Each sp As SubPath In Me.spaths
            sp.Transform(mat)
        Next

    End Sub

    Public Function GetTightBound() As RectangleF
        Dim flag As Boolean = True
        Dim bound As GRect
        For Each sp As SubPath In Me.spaths
            If flag Then
                bound = sp.GetTightBound()
                flag = False
            Else
                bound = bound.union(sp.GetTightBound)
            End If
        Next
        Return GeometryConverter.Grect2Rectanglef(bound)
    End Function

    Public Function GetBound() As RectangleF
        Dim bnd As RectangleF

        Using gp As GraphicsPath = Me.ToGraphicsPath
            bnd = gp.GetBounds
        End Using

        Return bnd
    End Function

    Public Function Clone() As GPath
        Dim gp As New GPath
        For Each sp As SubPath In Me.spaths
            gp.spaths.Add(sp.Clone)
        Next
        Return gp
    End Function

    Public Function ToGraphicsPath() As GraphicsPath
        Dim gp As New GraphicsPath

        For Each sp As SubPath In Me.spaths
            Dim tempGP = sp.ToGraphicsPath
            If tempGP IsNot Nothing Then gp.AddPath(tempGP, False)

        Next

        Return (gp)
    End Function

    Public Function isOutlineVisible(ByVal p As PointF, ByVal pen As Pen) As Boolean
        Dim isv As Boolean

        Using gp As GraphicsPath = Me.ToGraphicsPath
            isv = gp.IsOutlineVisible(p, pen)
        End Using

        Return isv
    End Function

    Public Function isVisible(ByVal p As PointF) As Boolean
        Dim isv As Boolean

        Using gp As GraphicsPath = Me.ToGraphicsPath
            isv = gp.IsVisible(p)
        End Using

        Return isv
    End Function
    Public Sub drawPath(ByVal g As Graphics, Optional ByVal Pen As Pen = Nothing, Optional ByVal Brush As Brush = Nothing)
        If (Pen IsNot Nothing) Or (Brush IsNot Nothing) Then
            Using gp As GraphicsPath = Me.ToGraphicsPath
                If Brush IsNot Nothing Then
                    g.FillPath(Brush, gp)
                End If
                If Pen IsNot Nothing Then
                    g.DrawPath(Pen, gp)
                End If
            End Using
        End If
    End Sub
End Class

<Serializable()> Public Class SubPath

    Private pts As New List(Of PathPoint)

    Public Sub New()
        Me.Closed = False
    End Sub

    Public ReadOnly Property Points As List(Of PathPoint)
        Get
            Return pts
        End Get

    End Property

    Default Public Property Item(ByVal i As Integer) As PathPoint
        Get
            Return Me.Points(i)
        End Get
        Set(ByVal value As PathPoint)
            Me.Points(i) = value
        End Set
    End Property

    Public Function CountPoints() As Integer
        Return Me.Points.Count
    End Function

    Public Property Closed As Boolean

    Public Sub Transform(ByVal mat As Matrix)
        For Each pt As PathPoint In Me.Points
            pt.Transform(mat)
        Next
    End Sub

    Public Function GetBound() As RectangleF
        Dim bnd As RectangleF

        Using gp As GraphicsPath = Me.ToGraphicsPath
            bnd = gp.GetBounds
        End Using

        Return bnd
    End Function

    Public Function GetTightBound() As GRect
        Dim result As GRect


        Dim n = countBezier()

        result = getBezier(0).GetCozyBound
        If n > 1 Then
            For i As Integer = 1 To n - 1
                Dim bound = getBezier(i).GetCozyBound
                result = result.union(bound)
            Next
        End If

        Return result
    End Function

    Public Function countBezier() As Integer

        Dim n As Integer
        n = CountPoints()
        If Not Closed Then
            If n <> 0 Then n -= 1
        End If
        Return n

    End Function

    Public Function getBezier(ByVal i As Integer) As GCubicBezier
        Dim n = countBezier()
        Dim pp1 = Points(i)

        Dim np2 = i + 1
        Dim npts = CountPoints()

        If np2 = npts Then
            If Closed Then
                i = 0
                np2 = 0
            End If
        Else
            np2 = i + 1
        End If
        Dim pp2 = Points(np2)

        Dim c1, c2, m1, m2 As GPoint
        m1 = GeometryConverter.Pointf2Gpoint(pp1.M)
        m2 = GeometryConverter.Pointf2Gpoint(pp2.M)
        If pp1.Type = PathPointType.None Then
            c1 = GeometryConverter.Pointf2Gpoint(pp1.M)
        Else
            c1 = GeometryConverter.Pointf2Gpoint(pp1.C2)
        End If
        If pp2.Type = PathPointType.None Then
            c2 = GeometryConverter.Pointf2Gpoint(pp2.M)
        Else
            c2 = GeometryConverter.Pointf2Gpoint(pp2.C1)
        End If

        Dim bez As New GCubicBezier(m1, c1, c2, m2)

        Return bez
    End Function

    Public Function Clone() As SubPath
        Dim sp As New SubPath
        sp.Closed = Me.Closed

        For Each pt As PathPoint In Me.Points
            sp.Points.Add(pt.Clone)
        Next

        Return sp
    End Function

    Public Function ToGraphicsPath() As GraphicsPath

        Dim np = Me.CountPoints
        If np < 2 Then
            Return Nothing
            Exit Function
        End If


        Dim lst As New List(Of PointF)
        Dim gp As New GraphicsPath

        Dim p0 = pts(0)

        If p0.Type = PathPointType.None Then
            lst.Add(p0.M)
            lst.Add(p0.M)
        Else
            lst.Add(p0.M)
            lst.Add(p0.C2)
        End If

        If np > 2 Then
            For i As Integer = 1 To np - 2
                Dim p As PathPoint = Me.pts(i)

                If p.Type = PathPointType.None Then
                    lst.Add(p.M)
                    lst.Add(p.M)
                    lst.Add(p.M)
                Else
                    lst.Add(p.C1)
                    lst.Add(p.M)
                    lst.Add(p.C2)
                End If


            Next
        End If


        Dim pl = pts(np - 1)

        If pl.Type = PathPointType.None Then
            lst.Add(pl.M)
            lst.Add(pl.M)
        Else
            lst.Add(pl.C1)
            lst.Add(pl.M)
        End If

        If Me.Closed Then
            lst.Add(pl.C2)

            lst.Add(p0.C1)
            lst.Add(p0.M)

            gp.AddBeziers(lst.ToArray)
            gp.CloseFigure()
        Else
            gp.AddBeziers(lst.ToArray)
            gp.SetMarkers()
        End If




        Return (gp)
    End Function
End Class

<Serializable()> Public Class PathPoint

    Dim pts(3) As PointF

    Public Sub New()

    End Sub

    Public Sub New(ByVal p As PointF)
        Me.M = p
        Me.C1 = p
        Me.C2 = p
        Me.Type = PathPointType.None
    End Sub

    Public Sub New(ByVal main As PointF, ByVal pControlPoint As PointF, _
                    ByVal nControlPoint As PointF, Optional ByVal pType As PathPointType = PathPointType.Sharp)

        Me.M = main
        Me.C1 = pControlPoint
        Me.C2 = nControlPoint
        Me.Type = pType

    End Sub

    Public Property M As PointF
        Get
            Return Me.pts(0)
        End Get
        Set(ByVal value As PointF)
            Me.pts(0) = value
        End Set
    End Property
    Public Property C1 As PointF
        Get
            Return Me.pts(1)
        End Get
        Set(ByVal value As PointF)
            Me.pts(1) = value
        End Set
    End Property
    Public Property C2 As PointF
        Get
            Return Me.pts(2)
        End Get
        Set(ByVal value As PointF)
            Me.pts(2) = value
        End Set
    End Property

    Public Property Type As PathPointType

    Public Function Clone() As PathPoint
        Return New PathPoint(Me.M, Me.C1, Me.C2, Me.Type)
    End Function

    Public Sub Transform(ByVal mat As Matrix)
        mat.TransformPoints(Me.pts)
    End Sub
End Class

Public Enum PathPointType
    None
    Smooth
    Sharp
End Enum
