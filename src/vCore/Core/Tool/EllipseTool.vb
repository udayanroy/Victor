Imports Geometry
Imports Graphics



Public Class EllipseTool
    Implements Itool




    Dim v As vCore
    Dim WithEvents dc As IDevice
    Dim mdl As Point

    Public Sub New(ByRef vcore As vCore)
        v = vcore
    End Sub
    Public ReadOnly Property Device() As IDevice Implements Itool.Device
        Get
            Return dc
        End Get
    End Property

    Private Sub dc_MouseDown(e As MouseEvntArg) Handles dc.MouseDown
        mdl = e.Location
        v.View.BufferGraphics.Initialize()
        dc.ActiveScroll = False
    End Sub

    Private Sub dc_MouseMove(e As MouseEvntArg) Handles dc.MouseMove
        If e.Button = MouseButton.Left Then
            v.View.BufferGraphics.Clear()
            Dim width As Integer = (e.Location.X - mdl.X)
            Dim height As Integer = (e.Location.Y - mdl.Y)

            v.View.BufferGraphics.Graphics.DrawEllipse(mdl.X, mdl.Y, width, height, New Pen(Color.BlackColor))
            v.View.BufferGraphics.Render()



        End If
    End Sub
    Private Sub dc_MouseUp(e As MouseEvntArg) Handles dc.MouseUp



        Dim vp As New PathElement
        vp.Path.Figures.Clear()
        vp.Path.AddEllipse(mdl.X, mdl.Y, e.Location.X - mdl.X, e.Location.Y - mdl.Y)

        'Convert path to memory path
        v.View.Screen2memory(vp.Path)

        'Add Active styles
        Dim edtr = v.Editor
        'vp.FillColor = edtr.FillColor
        'vp.StrokeColor = edtr.StrokeColor
        'vp.StrokWidth = edtr.strokeWidth
        'vp.isFill = edtr.isFill
        'vp.isStroke = edtr.isStroke
        vp.Brush = New SolidColorBrush(Color.BlueColor)
        vp.Pen = New Pen(Color.BlackColor, 3)
        'Add it to Memory
        v.mem.Layers(0).Item.Add(vp)

        v.View.Refresh()
        dc.ActiveScroll = True
    End Sub
    Public Sub DeSelectTool() Implements Itool.DeSelectTool
        dc = Nothing

    End Sub

    Public Sub SelectTool(ByRef d As IDevice) Implements Itool.SelectTool
        dc = d
        v.Editor.setIEdit(Nothing)
    End Sub


End Class
