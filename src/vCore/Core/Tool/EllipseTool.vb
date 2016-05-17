Imports Geometry
Imports Graphics



Public Class EllipseTool
    Inherits Tool

    Dim MouseLocation As Point
 

    Public Sub New(ByRef vcore As vCore)
        MyBase.New(vcore)
    End Sub


    Private Sub dc_MouseDown(e As MouseEvntArg) Handles _device.MouseDown
        MouseLocation = e.Location
        Visual.BufferGraphics.Initialize()
        Device.ActiveScroll = False
    End Sub

    Private Sub dc_MouseMove(e As MouseEvntArg) Handles _device.MouseMove
        If e.Button = MouseButton.Left Then
            Visual.BufferGraphics.Clear()
            Dim width As Integer = (e.Location.X - MouseLocation.X)
            Dim height As Integer = (e.Location.Y - MouseLocation.Y)

            Visual.BufferGraphics.Graphics.DrawEllipse(MouseLocation.X, MouseLocation.Y, width, height, New Pen(Color.BlackColor))
            Visual.BufferGraphics.Render()



        End If
    End Sub
    Private Sub dc_MouseUp(e As MouseEvntArg) Handles _device.MouseUp

        Dim PathElement As New PathElement
        PathElement.Path.Figures.Clear()
        PathElement.Path.AddEllipse(MouseLocation.X, MouseLocation.Y, e.Location.X - MouseLocation.X, e.Location.Y - MouseLocation.Y)

        'Convert path to memory path
        Visual.Screen2memory(PathElement.Path)

        'Add Active styles

        'vp.FillColor = edtr.FillColor
        'vp.StrokeColor = edtr.StrokeColor
        'vp.StrokWidth = edtr.strokeWidth
        'vp.isFill = edtr.isFill
        'vp.isStroke = edtr.isStroke
        PathElement.Brush = New SolidColorBrush(Color.BlueColor)
        PathElement.Pen = New Pen(Color.BlackColor, 3)
        'Add it to Memory
        Editor.ActiveLayer.Item.Add(PathElement)

        Visual.Refresh()
        Device.ActiveScroll = True
    End Sub
   

End Class
