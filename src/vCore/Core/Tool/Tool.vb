
Public Interface Itool

    ReadOnly Property Device() As IDevice
    Sub DeSelectTool()
    Sub SelectTool(ByRef d As IDevice)
End Interface

