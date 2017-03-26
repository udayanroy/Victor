Imports System.Windows.Forms
Imports System.Drawing


Public Class tZoomTool
    Inherits Tool




    Public Sub New(core As vCore)
        MyBase.New(core)
    End Sub

    Protected Overrides Sub MouseDown(e As MouseEvntArg)
        If e.Button = MouseButtons.Left Then
            Visual.setZoom(Visual.ZoomPercentage + 10, e.Location)
            Visual.Refresh()
        ElseIf e.Button = MouseButtons.Right Then
            Visual.setZoom(Visual.ZoomPercentage - 10, e.Location)
            Visual.Refresh()
        End If
    End Sub


End Class
