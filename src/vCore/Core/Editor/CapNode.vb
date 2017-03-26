
Imports Geometry

Public Class CapNode
    Inherits CapElement


    Dim _node As Node
    Dim _figure As NodeFigure
    Dim _path As New NodePath

    Public Sub New(editor As Editor, node As Node, figure As NodeFigure)
        MyBase.New(editor)

    End Sub

    Public Property M As Point
        Get
            Dim _m = _node.M
            Editor.View.Memory2screen(_m)
            Return _m
        End Get
        Set(value As Point)
            Editor.View.Screen2memory(value)
            _node.M = value
        End Set
    End Property
    Public Property C1 As Point
        Get
            Dim _c1 = _node.M
            Editor.View.Memory2screen(_c1)
            Return _c1
        End Get
        Set(value As Point)
            Editor.View.Screen2memory(value)
            _node.C1 = value
        End Set
    End Property

    Public Property C2 As Point
        Get
            Dim _c2 = _node.M
            Editor.View.Memory2screen(_c2)
            Return _c2
        End Get
        Set(value As Point)
            Editor.View.Screen2memory(value)
            _node.C2 = value
        End Set
    End Property

    Public Property Type As NodeType
        Get
            Return _node.Type
        End Get
        Set(value As NodeType)
            _node.Type = value
        End Set
    End Property

    Public Function Path() As NodePathsCapElement
        Throw New NotImplementedException()
    End Function

End Class
