

Public Interface ILayerManager

    Property ActiveLayer As Layer

    Sub AddLayer(LayrName As String)
    Sub ActivateLayer(layer As Layer)
    Function GetLayers() As IEnumerable(Of Layer)
    Sub DeleteLayer()
    Sub RepositionLayer(layer As Layer, Index As Integer)
End Interface
