Imports Geometry
Imports Graphics

Public Class tPanTool
    Inherits Tool





    Dim primaryLocation As Point

    Public Sub New(core As vCore)
        MyBase.New(core)
    End Sub


    Protected Overrides Sub MouseDown(e As MouseEvntArg)
        primaryLocation = e.Location
    End Sub

    Protected Overrides Sub MouseMove(e As MouseEvntArg)
        If e.Button = MouseButton.Left Then

            Core.View.panmove(New Point(primaryLocation.X - e.Location.X, primaryLocation.Y - e.Location.Y))
            primaryLocation = e.Location

        End If
    End Sub


End Class
