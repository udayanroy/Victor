Imports Geometry
Imports Graphics


Public Class LineTool
    Inherits Tool

    Dim primaryLocation As Point


    Public Sub New(ByRef vcore As vCore)
        MyBase.New(vcore)
    End Sub
 
  

    Private Sub dc_MouseDown(e As MouseEvntArg) Handles _device.MouseDown
        primaryLocation = e.Location
        Core.View.BufferGraphics.Initialize()
        Device.ActiveScroll = False
    End Sub

    Private Sub dc_MouseMove(e As MouseEvntArg) Handles _device.MouseMove
        If e.Button = Windows.Forms.MouseButtons.Left Then
            Core.View.BufferGraphics.Clear()

            Core.View.BufferGraphics.Graphics.DrawLine(New Point(primaryLocation.X, primaryLocation.Y),
                                                       New Point(e.Location.X, e.Location.Y),
                                                       New Pen(Color.BlackColor))
            Core.View.BufferGraphics.Render()

        End If
    End Sub

    Private Sub dc_MouseUp(e As MouseEvntArg) Handles _device.MouseUp


        Dim vp As New PathElement
        vp.Path.Figures.Clear()
        vp.Path.AddLine(primaryLocation, e.Location)
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
        Editor.ActiveLayer.Item.Add(vp)

        Core.View.Refresh()
        Device.ActiveScroll = True
    End Sub
End Class
