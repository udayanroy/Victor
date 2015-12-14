
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
        Dim c = Drawing.Color.FromArgb(CInt(Me.Red), CInt(Me.Green), CInt(Me.Blue))
        Return c
    End Function

    Public Shared Function FromArgb(r As Byte, g As Byte, b As Byte) As Color
        Dim c As Color
        c.Red = r
        c.Green = g
        c.Blue = b
        c.Alpha = 255
        Return c
    End Function


    Public Shared ReadOnly Property RedColor As Color
        Get
            Return Color.FromArgb(255, 0, 0)
        End Get
    End Property
    Public Shared ReadOnly Property BlackColor As Color
        Get
            Return Color.FromArgb(0, 0, 0)
        End Get
    End Property
    Public Shared ReadOnly Property WhiteColor As Color
        Get
            Return Color.FromArgb(255, 255, 255)
        End Get
    End Property
    Public Shared ReadOnly Property BlueColor As Color
        Get
            Return Color.FromArgb(0, 0, 255)
        End Get
    End Property
    Public Shared ReadOnly Property GreenColor As Color
        Get
            Return Color.FromArgb(0, 255, 0)
        End Get
    End Property
    Public Shared ReadOnly Property TransparentColor As Color
        Get
            Throw New NotImplementedException
        End Get
    End Property
    Public Shared ReadOnly Property BrownColor As Color
        Get
            Return Color.FromArgb(150, 75, 0)
        End Get
    End Property

    Public Shared ReadOnly Property MagentaColor() As Color
        Get
            Throw New NotImplementedException
        End Get
    End Property

    Public Shared ReadOnly Property DarkMagentaColor() As Color
        Get
            Throw New NotImplementedException
        End Get
    End Property
    Public Shared ReadOnly Property SkyBlueColor() As Color
        Get
            Throw New NotImplementedException
        End Get
    End Property
End Structure
