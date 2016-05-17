Imports Graphics


Public Interface Itool
    Sub DeSelectTool()
    Sub SelectTool(d As IDevice)
End Interface

Public MustInherit Class Tool
    Implements Itool


    Dim _core As vCore
    Protected WithEvents _device As IDevice


    Public Sub New(core As vCore)
        Me._core = core
    End Sub

    Protected ReadOnly Property Core As vCore
        Get
            Return Me._core
        End Get
    End Property

    Protected ReadOnly Property Editor As Editor
        Get
            Return Me._core.Editor
        End Get
    End Property

    Protected ReadOnly Property SelectionHolder As ISelectionHolder
        Get
            Return Me._core.Editor.SelectionHolder
        End Get
    End Property

    Protected ReadOnly Property Visual As ControlVisual
        Get
            Return Me._core.View
        End Get
    End Property
    Protected ReadOnly Property Dom As Document
        Get
            Return Me._core.View.DOM
        End Get
    End Property

    Protected ReadOnly Property Device As IDevice
        Get
            Return Me._device
        End Get
    End Property

    Public Overridable Sub DeSelectTool() Implements Itool.DeSelectTool
        RemoveHandler Me.Device.MouseDown, AddressOf MouseDown
        RemoveHandler Me.Device.MouseMove, AddressOf MouseMove
        RemoveHandler Me.Device.MouseUp, AddressOf MouseUp

        Me._device = Nothing
    End Sub

    Public Overridable Sub SelectTool(d As IDevice) Implements Itool.SelectTool
        Me._device = d
        Me.Editor.SetActiveTool(Me)

        AddHandler Me.Device.MouseDown, AddressOf MouseDown
        AddHandler Me.Device.MouseMove, AddressOf MouseMove
        AddHandler Me.Device.MouseUp, AddressOf MouseUp
    End Sub

    Public Overridable Sub Draw(canvas As Canvas)

    End Sub

    Protected Overridable Sub MouseDown(e As MouseEvntArg)

    End Sub

    Protected Overridable Sub MouseMove(e As MouseEvntArg)

    End Sub

    Protected Overridable Sub MouseUp(e As MouseEvntArg)

    End Sub

End Class