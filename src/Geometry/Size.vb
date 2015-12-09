




Public Structure Size

    Public Sub New(ByVal width As Double, ByVal height As Double)
        _Width = width
        _Height = height
    End Sub
    Public Sub New(ByVal p As Point)
        _Width = p.X
        _Height = p.Y
    End Sub
    Public Sub New(ByVal p1 As Point, ByVal p2 As Point)
        _Width = p2.X - p1.X
        _Height = p2.Y - p1.Y
    End Sub

    Public Property Width As Double
    Public Property Height As Double

End Structure


