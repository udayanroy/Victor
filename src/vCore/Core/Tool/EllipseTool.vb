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
        'Create the Element 
        Dim Ellipse As New PathElement
        Ellipse.Path.Figures.Clear()
        Ellipse.Path.AddEllipse(MouseLocation.X, MouseLocation.Y, e.Location.X - MouseLocation.X, e.Location.Y - MouseLocation.Y)

        'Add it to Dom
        Editor.AddPathToCurrentLayer(Ellipse)

        Visual.Refresh()
        Device.ActiveScroll = True
    End Sub
   

End Class
