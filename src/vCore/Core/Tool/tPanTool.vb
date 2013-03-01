Imports System.Windows.Forms
Imports System.Drawing

Public Class tPanTool
    Implements Itool




    Dim v As View
    Dim WithEvents dc As advancedPanel
    Dim mdl As Point

    Public Sub New(ByRef vew As View)
        v = vew
    End Sub
    Public ReadOnly Property Device() As advancedPanel Implements Itool.Device
        Get
            Return dc
        End Get
    End Property

    Private Sub dc_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dc.MouseDown
        mdl = e.Location
    End Sub

    Private Sub dc_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dc.MouseMove
        If e.Button = MouseButtons.Left Then

            v.panmove(New Point(mdl.X - e.Location.X, mdl.Y - e.Location.Y))
            mdl = e.Location

        End If
    End Sub

    Public Sub DeSelectTool() Implements Itool.DeSelectTool
        dc = Nothing
    End Sub

    Public Sub SelectTool(ByRef d As advancedPanel) Implements Itool.SelectTool
        dc = d
    End Sub
End Class
