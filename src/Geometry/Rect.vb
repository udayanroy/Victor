


Public Structure Rect

    Private P1 As Point
    Private p2 As Point



    Public Sub New(ByVal point1 As Point, ByVal point2 As Point)
        P1 = point1
        p2 = point2
    End Sub
    Public Sub New(ByVal x As Double, ByVal y As Double, ByVal x1 As Double, ByVal y1 As Double)
        P1 = New Point(x, y)
        p2 = New Point(x1, y1)
    End Sub

    Public Sub New(ByVal location As Point, ByVal width As Double, ByVal height As Double)
        P1 = location
        p2 = New Point(P1.X + width, P1.Y + height)
    End Sub

    Public Sub New(ByVal location As Point, ByVal size As Size)
        P1 = location
        p2 = New Point(P1.X + size.Width, P1.Y + size.Height)
    End Sub

#Region "Property"



    Public Property point1 As Point
        Get
            Return P1

        End Get
        Set(ByVal value As Point)
            P1 = value
        End Set
    End Property
    Public Property Point2 As Point
        Get
            Return p2
        End Get
        Set(ByVal value As Point)
            p2 = value
        End Set
    End Property

    Public Property Location As Point
        Get
            Return P1
        End Get
        Set(ByVal value As Point)
            P1 = value
        End Set
    End Property

    Public Property Size As Size
        Get
            Return New Size(p2.X - P1.X, p2.Y - P1.Y)
        End Get
        Set(ByVal value As Size)
            p2.X = P1.X + value.Width
            p2.Y = P1.Y + value.Height
        End Set
    End Property

    Public Property X As Double
        Get
            Return P1.X
        End Get
        Set(ByVal value As Double)
            P1.X = value
        End Set
    End Property

    Public Property Y As Double
        Get
            Return P1.Y
        End Get
        Set(ByVal value As Double)
            P1.Y = value
        End Set
    End Property
    Public Property Width As Double
        Get
            Return p2.X - P1.X
        End Get
        Set(ByVal value As Double)
            p2.X = P1.X + value
        End Set
    End Property

    Public Property Height As Double
        Get
            Return p2.Y - P1.Y
        End Get
        Set(ByVal value As Double)
            p2.Y = P1.Y + value
        End Set
    End Property

    Public ReadOnly Property Top As Double
        Get
            Return Math.Min(P1.Y, p2.Y)
        End Get

    End Property

    Public ReadOnly Property Bottom As Double
        Get
            Return Math.Max(P1.Y, p2.Y)
        End Get
    End Property

    Public ReadOnly Property Left As Double
        Get
            Return Math.Min(P1.X, p2.X)

        End Get
    End Property

    Public ReadOnly Property Right As Double
        Get
            Return Math.Max(P1.X, p2.X)

        End Get
    End Property
#End Region

    Public Sub Offset(ByVal dx As Double, ByVal dy As Double)
        Dim width = Me.Width
        Dim height = Me.Height

        P1.X += dx
        P1.Y += dy
        Me.Width = width
        Me.Height = height

    End Sub
    Public Sub Offset(ByVal vector As Point)
        Offset(vector.X, vector.Y)
    End Sub


    Public Function Contain(ByVal p As Point) As Boolean
        If p.X < Me.Right And p.X > Me.Left Then
            If p.Y > Me.Top And p.Y < Me.Bottom Then
                Return True
                Exit Function
            End If
        End If
        Return False
    End Function

    Public Function midpoint() As Point
        Dim m As New Point(Me.Left + Me.Width / 2, Me.Top + Me.Height / 2)
        Return m
    End Function

    Public Function union(ByVal rect As Rect) As Rect
        Dim ret As New Rect

        ret.X = Math.Min(Me.Left, rect.Left)
        ret.Y = Math.Min(Me.Top, rect.Top)
        ret.Width = Math.Max(Me.Right, rect.Right) - ret.X
        ret.Height = Math.Max(Me.Bottom, rect.Bottom) - ret.Y

        Return ret
    End Function

    Public Sub Transform(ByVal mat As Matrix)
        mat.map(Me.P1)
        mat.map(Me.p2)
    End Sub

End Structure


