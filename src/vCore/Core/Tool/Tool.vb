

Public Interface Itool

    ReadOnly Property Device() As IDevice
    Sub DeSelectTool()
    Sub SelectTool(ByRef d As IDevice)
End Interface

Public MustInherit Class Tool
    Implements Itool

   
    Dim Core As vCore
    Dim WithEvents dc As IDevice
     

    Public Sub New(ByRef vcore As vCore)
        Core = vcore
    End Sub

    Public Sub DeSelectTool() Implements Itool.DeSelectTool

    End Sub

    Public ReadOnly Property Device As IDevice Implements Itool.Device
        Get

        End Get
    End Property

    Public Sub SelectTool(ByRef d As IDevice) Implements Itool.SelectTool

    End Sub
End Class