

Friend Class LayerManager
    Implements ILayerManager

    Private _editor As Editor
    Private _activelayer As Layer

    Public Sub New(editor As Editor)
        _editor = editor

        'set activelayer
        Dim layers = _editor.View.DOM.Layers
        Me.ActivateLayer(layers(layers.Count - 1))
    End Sub

    Private ReadOnly Property Editor As Editor
        Get
            Return Me._editor
        End Get
    End Property

    Public Sub ActivateLayer(layer As Layer) Implements ILayerManager.ActivateLayer
        Me._activelayer = layer
    End Sub

    Public ReadOnly Property ActiveLayer As Layer Implements ILayerManager.ActiveLayer
        Get
            Return Me._activelayer
        End Get
    End Property

    Public Sub AddLayer(LayrName As String) Implements ILayerManager.AddLayer

    End Sub

    Public Sub DeleteLayer() Implements ILayerManager.DeleteLayer

    End Sub

    Public Function GetLayers() As IEnumerable(Of Layer) Implements ILayerManager.GetLayers
        Return Editor.View.DOM.Layers
    End Function

    Public Sub RepositionLayer(layer As Layer, Index As Integer) Implements ILayerManager.RepositionLayer


    End Sub
End Class
