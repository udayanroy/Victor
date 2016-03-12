
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

    Private _element As DrawingElement
    Private _index As Integer
    Private _layer As Layer
    Private _layerIndex As Integer

    Public Sub New(element As DrawingElement, index As Integer, layer As Layer, layerindex As Integer)
        Me._element = element
        Me._index = index
        Me._layer = layer
        Me._layerIndex = layerindex
    End Sub

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

    ''' <summary>
    ''' Return selected Element
    ''' </summary>
    ''' <value></value>
    ''' <returns>selected DrawingElement</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Element As DrawingElement
        Get
            Return Me._element
        End Get
    End Property

    Public ReadOnly Property ElementIndex As Integer
        Get
            Return Me._index
        End Get
    End Property
    Public ReadOnly Property Layer As Layer
        Get
            Return Me._layer
        End Get
    End Property

    ''' <summary>
    ''' Return the Index of parent layer
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property LayerIndex As Integer
        Get
            Return Me._layerIndex
        End Get
    End Property

    Public Sub UpdateIndex(layerindex As Integer)
        Me._layerIndex = layerindex
    End Sub
End Class
