
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

    Public Shared ReadOnly Property RedColor As Color
        Get
            Return New Color(255, 0, 0)
        End Get
    End Property
    Public Shared ReadOnly Property BlackColor As Color
        Get
            Return New Color(0, 0, 0)
        End Get
    End Property
    Public Shared ReadOnly Property WhiteColor As Color
        Get
            Return New Color(255, 255, 255)
        End Get
    End Property
    Public Shared ReadOnly Property BlueColor As Color
        Get
            Return New Color(0, 0, 255)
        End Get
    End Property
    Public Shared ReadOnly Property TransparentColor As Color
        Get
            Return New Color(0, 0, 0, 0)
        End Get
    End Property
    Public Shared ReadOnly Property BrownColor As Color
        Get
            Return New Color(150, 75, 0)
        End Get
    End Property

End Structure
