Imports System.Drawing
Imports System.Windows.Forms
Imports System.Drawing.Drawing2D

Public Class RotateTool
    Implements Itool




    Dim v As vCore
    Dim WithEvents dc As advancedPanel
    Dim mdl As Point
    Dim md As Point
    Public Sub New(ByRef vew As vCore)
        v = vew
    End Sub
    Public ReadOnly Property Device() As advancedPanel Implements Itool.Device
        Get
            Return dc
        End Get
    End Property
    Dim s As Integer = 0
    Dim svp As GraphicsPath

    Private Sub dc_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dc.MouseDown

        v.Editor.mouse_Down(e)
    End Sub

    Private Sub dc_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dc.MouseMove

        v.Editor.mouse_Move(e)
    End Sub
    Private Sub dc_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dc.MouseUp

        v.Editor.mouse_Up(e)
    End Sub
    Public Sub DeSelectTool() Implements Itool.DeSelectTool
        dc = Nothing
    End Sub

    Public Sub SelectTool(ByRef d As advancedPanel) Implements Itool.SelectTool
        dc = d

        v.Editor.setEditingType(selectionType.Rotate)
    End Sub
End Class
