
Namespace Geometry
    Public Structure Point
        Private x1, y1 As Double



        Public Sub New(ByVal x As Double, ByVal y As Double)
            x1 = x
            y1 = y
        End Sub

        Public Sub New(ByVal x As Integer, ByVal y As Integer)
            x1 = x
            y1 = y
        End Sub



        Public Property X() As Double
            Get
                Return x1
            End Get
            Set(ByVal value As Double)
                x1 = value
            End Set
        End Property
        Public Property Y() As Double
            Get
                Return y1
            End Get
            Set(ByVal value As Double)
                y1 = value
            End Set
        End Property

         

        Public Overrides Function toString() As String
            Return "(" & Me.X & "," & Me.Y & ")"
        End Function

#Region "Operator Overloading "
        Public Function Dot(ByVal p2 As Point) As Double
            Return X * p2.X + Y * p2.Y
        End Function





        Public Shared Operator +(ByVal p1 As Point, ByVal p2 As Point) As Point
            Return New Point(p1.X + p2.X, p1.Y + p2.Y)
        End Operator
        Public Shared Operator -(ByVal p1 As Point, ByVal p2 As Point) As Point
            Return New Point(p1.X - p2.X, p1.Y - p2.Y)
        End Operator
        Public Shared Operator *(ByVal p1 As Point, ByVal p2 As Point) As Point
            Return New Point(p1.X * p2.X, p1.Y * p2.Y)
        End Operator

        Public Shared Operator /(ByVal p1 As Point, ByVal p2 As Point) As Point
            Return New Point(p1.X / p2.X, p1.Y / p2.Y)
        End Operator

        Public Shared Operator >=(ByVal p1 As Point, ByVal p2 As Point) As Boolean
            Return (p1.X >= p2.X And p1.Y >= p2.Y)
        End Operator
        Public Shared Operator <=(ByVal p1 As Point, ByVal p2 As Point) As Boolean
            Return (p1.X <= p2.X And p1.Y <= p2.Y)
        End Operator

        Public Shared Operator +(ByVal p1 As Point, ByVal n As Double) As Point
            Return New Point(p1.X + n, p1.Y + n)
        End Operator
        Public Shared Operator -(ByVal p1 As Point, ByVal n As Double) As Point
            Return New Point(p1.X - n, p1.Y - n)
        End Operator
        Public Shared Operator *(ByVal n As Double, ByVal p1 As Point) As Point
            Return New Point(p1.X * n, p1.Y * n)
        End Operator
        Public Shared Operator *(ByVal p1 As Point, ByVal n As Double) As Point
            Return New Point(p1.X * n, p1.Y * n)
        End Operator
        Public Shared Operator /(ByVal p1 As Point, ByVal n As Double) As Point
            Return New Point(p1.X / n, p1.Y / n)
        End Operator

        Public Shared Operator /(ByVal n As Double, ByVal p1 As Point) As Point
            Return New Point(n / p1.X, n / p1.Y)
        End Operator

        Public Shared Operator -(ByVal p1 As Point) As Point
            Return New Point(-p1.X, -p1.Y)
        End Operator





        Public Shared Function lerp(ByVal p1 As Point, ByVal p2 As Point, ByVal t As Double) As Point
            Return New Point(p1.X + (p2.X - p1.X) * t, p1.Y + (p2.Y - p1.Y) * t)
        End Function
        Public Shared Function Dot(ByVal p1 As Point, ByVal p2 As Point) As Double
            Return p1.X * p2.X + p1.Y * p2.Y
        End Function
        Public Shared Function Cross(ByVal p1 As Point, ByVal p2 As Point) As Double
            Return Dot(p1, CW(p2))
        End Function
        Public Shared Function CW(ByVal P As Point) As Point
            Return New Point(-P.Y, P.X)
        End Function

        Public Function Min(ByVal pt As Point) As Point
            Return New Point(Math.Min(Me.X, pt.X), Math.Min(Me.Y, pt.Y))
        End Function
        Public Function Max(ByVal pt As Point) As Point
            Return New Point(Math.Max(Me.X, pt.X), Math.Max(Me.Y, pt.Y))
        End Function


#End Region

        Public Shared Function Distance(ByVal p1 As Point, ByVal p2 As Point) As Double
            Return Math.Sqrt((p2.X - p1.X) ^ 2 + (p2.Y - p1.Y) ^ 2)
        End Function

        Function modulus() As Integer
            Return Me.X + Me.Y
        End Function

        'Public Sub Transformation(ByVal mat As GMatrix)
        '    Dim tempP As New Point
        '    tempP = Me
        '    mat.map(tempP)
        '    Me.SetValue(tempP)
        'End Sub

    End Structure
End Namespace

