
Public Structure Color

    Public Sub New(r As Byte, g As Byte, b As Byte, a As Byte)
        Me.Red = r
        Me.Green = g
        Me.Blue = b
        Me.Alpha = a
    End Sub
    Public Sub New(r As Byte, g As Byte, b As Byte)
        Me.Red = r
        Me.Green = g
        Me.Blue = b
        Me.Alpha = 255
    End Sub

    Public Property Red As Byte
    Public Property Green As Byte
    Public Property Blue As Byte
    Public Property Alpha As Byte

    Friend Function toDColor() As Drawing.Color


        Return Drawing.Color.FromArgb(Alpha, Red, Green, Blue)
    End Function
End Structure
