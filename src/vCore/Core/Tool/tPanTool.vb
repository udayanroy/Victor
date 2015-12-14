Imports Geometry
Imports Graphics

Public Class tPanTool
    Implements Itool




    Dim core As vCore
    Dim WithEvents dc As IDevice
    Dim mdl As Point

    Public Sub New(core As vCore)
        Me.core = core
    End Sub
    Public ReadOnly Property Device() As IDevice Implements Itool.Device
        Get
            Return dc
        End Get
    End Property

    Private Sub dc_MouseDown(e As MouseEvntArg) Handles dc.MouseDown
        mdl = e.Location
    End Sub

    Private Sub dc_MouseMove(e As MouseEvntArg) Handles dc.MouseMove
        If e.Button = MouseButton.Left Then

            core.View.panmove(New Point(mdl.X - e.Location.X, mdl.Y - e.Location.Y))
            mdl = e.Location

        End If
    End Sub

    Public Sub DeSelectTool() Implements Itool.DeSelectTool
        dc = Nothing
    End Sub

    Public Sub SelectTool(ByRef d As IDevice) Implements Itool.SelectTool
        dc = d
    End Sub
End Class
