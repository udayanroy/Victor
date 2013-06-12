Imports System.Drawing

Public Class GPath
    Private spaths As New List(Of SubPath)

    Public Sub New()

    End Sub

    'Add shapes to the Path.
    'Every shape has added will be a SubPath.

    'Add a subpath of line.
    Public Sub AddLine(ByVal p1 As PointF, ByVal p2 As PointF)

    End Sub
    Public Sub AddLines(ByVal points() As PointF)

    End Sub

    'Add a subpath of Rectangle
    Public Sub AddRectangle(ByVal rect As RectangleF)

    End Sub
    Public Sub AddRectangle(ByVal x1 As Single, ByVal y1 As Single, ByVal Width As Single, ByVal Height As Single)

    End Sub

    'Add a subpath of Ellipse.
    Public Sub AddEllipse(ByVal p1 As PointF, ByVal p2 As PointF)

    End Sub
    Public Sub AddEllipse(ByVal x1 As Single, ByVal y1 As Single, ByVal width As Single, ByVal height As Single)

    End Sub
    Public Sub AddEllipse(ByVal rect As RectangleF)

    End Sub

    'Add a SubPath
    Public Sub AddSubPath(ByVal path As SubPath)

    End Sub

    Public Function getPoints() As PointF

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
       
End Class

Public Class PathPoint
    Public Property M As PointF
    Public Property C1 As PointF
    Public Property C2 As PointF
    Public Property Type As PathPointType
End Class

Public Enum PathPointType
    None
    Smooth
    Sharp
End Enum
