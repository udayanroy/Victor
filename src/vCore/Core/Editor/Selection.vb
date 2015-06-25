

Public Enum selectionType
    None
    Move
    Size
    Rotate
    PathEdit
    PointerConvert
    Other
End Enum

Public Structure memLoc
    Dim layer As Integer
    Dim obj As Integer
    Public Sub create(ByVal l As Integer, ByVal o As Integer)
        layer = l
        obj = o
    End Sub
End Structure

Public Class Selection

    Dim memloc As memLoc
    Dim emty As Boolean
    Public Sub New()
        emty = True
    End Sub
    Public Property isEmty() As Boolean
        Get
            Return emty
        End Get
        Set(ByVal value As Boolean)
            emty = value
        End Set
    End Property


    Public Property MemoryLocation() As memLoc
        Get
            Return memloc
        End Get
        Set(ByVal value As memLoc)
            memloc = value
        End Set
    End Property
End Class
