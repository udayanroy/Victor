


Public Class NodeFigure
    Private pts As New List(Of Node)

    Public Sub New()
        Me.Closed = False
    End Sub

    Public ReadOnly Property Points As List(Of Node)
        Get
            Return pts
        End Get

    End Property

    Default Public Property Item(ByVal i As Integer) As Node
        Get
            Return Me.Points(i)
        End Get
        Set(ByVal value As Node)
            Me.Points(i) = value
        End Set
    End Property

    Public Function CountPoints() As Integer
        Return Me.Points.Count
    End Function

    Public Property Closed As Boolean

    Public Sub Transform(ByVal mat As Matrix)
        For Each pt As Node In Me.Points
            pt.Transform(mat)
        Next
    End Sub

    Public Function GetBound() As Rect

        Dim result As Rect

        Dim n = countBezier()

        result = getBezier(0).GetBound
        If n > 1 Then
            For i As Integer = 1 To n - 1
                Dim bound = getBezier(i).GetBound
                result = result.union(bound)
            Next
        End If

        Return result

    End Function

    Public Function GetTightBound() As Rect
        Dim result As Rect


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

    Public Function Clone() As NodeFigure
        Dim sp As New NodeFigure
        sp.Closed = Me.Closed

        For Each pt As Node In Me.Points
            sp.Points.Add(pt.Clone)
        Next

        Return sp
    End Function

    Public Function countBezier() As Integer

        Dim n As Integer
        n = CountPoints()
        If Not Closed Then
            If n <> 0 Then n -= 1
        End If
        Return n

    End Function

    Public Function getBezier(ByVal i As Integer) As CubicBezier
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

        Dim c1, c2 As Point
        If pp1.Type = NodeType.None Then
            c1 = pp1.M
        Else
            c1 = pp1.C2
        End If
        If pp2.Type = NodeType.None Then
            c2 = pp2.M
        Else
            c2 = pp2.C1
        End If
        Dim bez As New CubicBezier(pp1.M, c1, c2, pp2.M)

        Return bez
    End Function
End Class
