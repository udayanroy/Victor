
Imports Geometry

Public Class CapNode
    Inherits CapElement



    Public Sub New(editor As Editor)
        MyBase.New(editor)

    End Sub

    Public Property M As Point
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Point)
            Throw New NotImplementedException()
        End Set
    End Property
    Public Property C1 As Point
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Point)
            Throw New NotImplementedException()
        End Set
    End Property

    Public Property C2 As Point
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Point)
            Throw New NotImplementedException()
        End Set
    End Property

    Public Property Type As NodeType
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As NodeType)
            Throw New NotImplementedException()
        End Set
    End Property

    Public Function Path() As NodePathsCapElement
        Throw New NotImplementedException()
    End Function

End Class
