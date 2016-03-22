Imports Geometry
Imports Graphics



Public Class Editor


    Private _core As vCore
    Private _SelectionHolder As SelectionHolder
    Private _layermanager As LayerManager
    Private _ActiveTool As Tool

    Public Event SelectionChanged()

    Public Sub New(v As vCore)

        _core = v
        Me._SelectionHolder = New SelectionHolder(Me)
        Me._layermanager = New LayerManager(Me)


    End Sub

    Public Function SelectAt(ByVal p As Point) As Integer
        Dim returnvalue As SelectionType
        returnvalue = Me.SelectionHolder.SelectAt(p)
        RaiseEvent SelectionChanged()
        Return returnvalue

    End Function


    Public Sub DisSelect()
        Me.SelectionHolder.Clear()
    End Sub

    Public ReadOnly Property SelectionHolder() As ISelectionHolder
        Get
            Return Me._SelectionHolder
        End Get
    End Property

    Public ReadOnly Property View As ControlVisual
        Get
            Return _core.View
        End Get

    End Property

    Public ReadOnly Property Dom As Document
        Get
            Return _core.View.DOM
        End Get

    End Property

    Public Sub Refresh()
        _core.View.Refresh()
    End Sub

    Friend Sub SetActiveTool(tool As Tool)
        Me._ActiveTool = tool
    End Sub

    Friend Sub paint(canvas As Canvas)
        If Me._ActiveTool IsNot Nothing Then
            Me._ActiveTool.Draw(canvas)
        End If
    End Sub
    Public ReadOnly Property LayerManager() As ILayerManager
        Get
            Return Me._layermanager
        End Get
    End Property

    Public ReadOnly Property ActiveLayer As Layer
        Get
            Return Me.LayerManager.ActiveLayer
        End Get
    End Property

End Class

