Imports Geometry
Imports Graphics


Public Class RectangleTool
    Implements Itool


    Dim Core As vCore
    Dim WithEvents dc As IDevice
    Dim primaryLocation As Point


    Public Sub New(ByRef vcore As vCore)
        Core = vcore
    End Sub

    Public Sub DeSelectTool() Implements Itool.DeSelectTool
        dc = Nothing
    End Sub

    Public ReadOnly Property Device As IDevice Implements Itool.Device
        Get
            Return dc
        End Get
    End Property

    Public Sub SelectTool(ByRef d As IDevice) Implements Itool.SelectTool
        dc = d
        Core.Editor.setEditingType(selectionType.None)
    End Sub

    Private Sub dc_MouseDown(e As MouseEvntArg) Handles dc.MouseDown
        primaryLocation = e.Location
        Core.View.BufferGraphics.Initialize()
        dc.ActiveScroll = False
    End Sub

    Private Sub dc_MouseMove(e As MouseEvntArg) Handles dc.MouseMove
        If e.Button = Windows.Forms.MouseButtons.Left Then
            Core.View.BufferGraphics.Clear()
            Dim width As Integer = (e.Location.X - primaryLocation.X)
            Dim height As Integer = (e.Location.Y - primaryLocation.Y)

            Core.View.BufferGraphics.Graphics.DrawRect(New Rect(primaryLocation, e.Location), New Pen(Color.BlackColor))
            Core.View.BufferGraphics.Render()

        End If
    End Sub

    Private Sub dc_MouseUp(e As MouseEvntArg) Handles dc.MouseUp


        Dim vp As New vPath
        vp.Path.Figures.Clear()
        vp.Path.AddRectangle(primaryLocation.X, primaryLocation.Y, e.Location.X - primaryLocation.X, e.Location.Y - primaryLocation.Y)
        'Convert path to memory path
        Core.View.Screen2memory(vp.Path)

        'Add Active styles
        Dim edtr = Core.Editor
        'vp.FillColor = edtr.FillColor
        'vp.StrokeColor = edtr.StrokeColor
        'vp.StrokWidth = edtr.strokeWidth
        'vp.isFill = edtr.isFill
        'vp.isStroke = edtr.isStroke

        'Add it to Memory
        Core.Memory.Layers(0).Item.Add(vp)

        Core.View.Refresh()
        dc.ActiveScroll = True
    End Sub
End Class
