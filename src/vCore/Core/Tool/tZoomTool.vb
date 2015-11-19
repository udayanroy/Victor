Imports System.Windows.Forms
Imports System.Drawing


Public Class tZoomTool
    Implements Itool


    Dim v As View
    Dim WithEvents dc As advancedPanel

    Public Sub New(ByRef vew As View)
        v = vew
    End Sub
    Public ReadOnly Property Device() As advancedPanel Implements Itool.Device
        Get
            Return dc
        End Get
    End Property

    Private Sub dc_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dc.MouseDown
        If e.Button = MouseButtons.Left Then
            v.setZoom(v.Zoom + 10, e.Location)
        ElseIf e.Button = MouseButtons.Right Then
            v.setZoom(v.Zoom - 10, e.Location)
        End If
    End Sub

    Public Sub DeSelectTool() Implements Itool.DeSelectTool
        dc = Nothing
    End Sub

    Public Sub SelectTool(ByRef d As advancedPanel) Implements Itool.SelectTool
        dc = d
    End Sub
End Class
