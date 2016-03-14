Imports Geometry
Imports Graphics



Public Class Editor


    Dim vcor As vCore
    Dim _SelectionHolder As SelectionHolder
    Private _layermanager As LayerManager
    Dim ActiveTool As Tool
    Public Event SelectionChanged()

    Public Sub New(ByRef v As vCore)

        vcor = v
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

    Public ReadOnly Property SelectionHolder() As SelectionHolder
        Get
            Return Me._SelectionHolder
        End Get
    End Property

    Public ReadOnly Property View As ControlVisual
        Get
            Return vcor.View
        End Get

    End Property
    Public Sub Refresh()
        vcor.View.Refresh()
    End Sub

    Public Sub SetActiveTool(tool As Tool)
        Me.ActiveTool = tool
    End Sub

    Public Sub paint(canvas As Canvas)
        If Me.ActiveTool IsNot Nothing Then
            Me.ActiveTool.Draw(canvas)
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

