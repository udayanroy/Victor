﻿Imports System.Drawing
Imports System.Drawing.Drawing2D

Public Class GPath
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
        Return Nothing
    End Function

    Public ReadOnly Property Items(ByVal index As Integer) As SubPath
        Get
            Return Me.spaths.Item(index)
        End Get
    End Property

    Public Sub Transform(ByVal mat As Matrix)

    End Sub

    Public Function GetBoundS() As RectangleF

    End Function

    Public Function GetBoundL() As RectangleF

    End Function

    Public Function Copy() As GPath
        Return Nothing
    End Function

    Public Function ToGraphicsPath() As GraphicsPath
        Dim gp As New GraphicsPath

        Return gp
    End Function


End Class

Public Class SubPath

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

    End Sub

    Public Function GetBoundL() As RectangleF

    End Function

    Public Function GetBoundS() As RectangleF

    End Function

    Public Function Copy() As SubPath
        Return Nothing
    End Function

End Class

Public Class PathPoint

    Public Sub New()

    End Sub

    Public Sub New(ByVal p As PointF)
        Me.M = p
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
    Public Property C1 As PointF
    Public Property C2 As PointF
    Public Property Type As PathPointType

    Public Function Copy() As PathPoint
        Return New PathPoint(Me.M, Me.C1, Me.C2, Me.Type)
    End Function

End Class

Public Enum PathPointType
    None
    Smooth
    Sharp
End Enum
