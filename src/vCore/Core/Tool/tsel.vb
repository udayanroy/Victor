Imports System.Drawing
Imports System.Windows.Forms

Public Class tsel
    Implements Itool




    Dim v As vCore
    Dim WithEvents dc As advancedPanel
    Dim mdl As Point

    Public Sub New(ByRef vc As vCore)
        v = vc
    End Sub
    Public ReadOnly Property Device() As advancedPanel Implements Itool.Device
        Get
            Return dc
        End Get
    End Property

    Private Sub dc_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dc.MouseDown
        mdl = e.Location
        v.Editor.SelectAt(mdl)

    End Sub

    Private Sub dc_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dc.MouseMove
        
    End Sub

    Public Sub DeSelectTool() Implements Itool.DeSelectTool
        dc = Nothing
    End Sub

    Public Sub SelectTool(ByRef d As advancedPanel) Implements Itool.SelectTool
        dc = d
    End Sub
End Class
