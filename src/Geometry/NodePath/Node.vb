
Public Enum NodeType
    None
    Smooth
    Sharp
End Enum

Public Class Node

    Public Sub New()
        _m = New Point
        _c1 = New Point
        _c2 = New Point
    End Sub

    Public Sub New(ByVal p As Point)
        _m = p
        _C1 = p
        _C2 = p
        Me.Type = NodeType.None
    End Sub

    Public Sub New(ByVal main As Point, ByVal pControlPoint As Point, _
                    ByVal nControlPoint As Point, Optional ByVal pType As NodeType = NodeType.Sharp)

        Me._m = main
        Me._c1 = pControlPoint
        Me._c2 = nControlPoint
        Me.Type = pType

    End Sub

    Public Property M As Point
        
    Public Property C1 As Point
      
    Public Property C2 As Point
       

    Public Property Type As NodeType

    Public Sub setValue(ByVal m As Point)
        Me.M = m
    End Sub

    Public Sub setValue(ByVal m As Point, ByVal c1 As Point, ByVal c2 As Point)
        Me.M = m
        Me.C1 = c1
        Me.C2 = c2
    End Sub

    Public Function Clone() As Node
        Dim rp As New Node()
        rp.setValue(Me.M, Me.C1, Me.C2)
        Return rp
    End Function

    Public Sub Transform(ByVal mat As Matrix)
        mat.map(M)
        mat.map(C1)
        mat.map(C2)

    End Sub
End Class
