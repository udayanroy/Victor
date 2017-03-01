Imports Core
Imports Geometry
Imports Graphics


Public Class RectangleTool
    Inherits Tool



    Dim primaryLocation As Point


    Public Sub New(ByRef vcore As vCore)
        MyBase.New(vcore)
    End Sub



    Protected Overrides Sub MouseDown(e As MouseEvntArg)
        primaryLocation = e.Location
        Visual.BufferGraphics.Initialize()
        Device.ActiveScroll = False
    End Sub

    Protected Overrides Sub MouseMove(e As MouseEvntArg)
        If e.Button = MouseButton.Left Then
            Visual.BufferGraphics.Clear()
            Dim width As Integer = (e.Location.X - primaryLocation.X)
            Dim height As Integer = (e.Location.Y - primaryLocation.Y)

            Visual.BufferGraphics.Graphics.DrawRect(New Rect(primaryLocation, e.Location), New Pen(Color.BlackColor))
            Visual.BufferGraphics.Render()

        End If
    End Sub

    Protected Overrides Sub MouseUp(e As MouseEvntArg)

        'Create the Element 
        Dim Rectangle As New PathElement
        Rectangle.Path.Figures.Clear()
        Rectangle.Path.AddRectangle(primaryLocation.X, primaryLocation.Y, e.Location.X - primaryLocation.X, e.Location.Y - primaryLocation.Y)

        'Add it to Dom
        Editor.AddPathToCurrentLayer(Rectangle)

        Visual.Refresh()
        Device.ActiveScroll = True
    End Sub
End Class
