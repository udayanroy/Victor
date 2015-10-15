Imports System.Drawing

Public Class LineTool
    Implements Itool


    Dim Core As vCore
    Dim WithEvents dc As advancedPanel
    Dim primaryLocation As Point


    Public Sub New(ByRef vcore As vCore)
        Core = vcore
    End Sub

    Public Sub DeSelectTool() Implements Itool.DeSelectTool
        dc = Nothing
    End Sub

    Public ReadOnly Property Device As advancedPanel Implements Itool.Device
        Get
            Return dc
        End Get
    End Property

    Public Sub SelectTool(ByRef d As advancedPanel) Implements Itool.SelectTool
        dc = d
        Core.Editor.setEditingType(selectionType.None)
    End Sub

    Private Sub dc_MouseDown(sender As Object, e As Windows.Forms.MouseEventArgs) Handles dc.MouseDown
        primaryLocation = e.Location
        Core.View.BufferGraphics.Initialize()
    End Sub

    Private Sub dc_MouseMove(sender As Object, e As Windows.Forms.MouseEventArgs) Handles dc.MouseMove
        If e.Button = Windows.Forms.MouseButtons.Left Then
            Core.View.BufferGraphics.Clear()
           
            Core.View.BufferGraphics.Graphics.DrawLine(Pens.Black, primaryLocation.X, primaryLocation.Y, e.Location.X, e.Location.Y)
            Core.View.BufferGraphics.Render()

        End If
    End Sub

    Private Sub dc_MouseUp(sender As Object, e As Windows.Forms.MouseEventArgs) Handles dc.MouseUp


        Dim vp As New vPath
        vp.GraphicsPath.subpaths.Clear()
        vp.GraphicsPath.AddLine(primaryLocation, e.Location)
        'Convert path to memory path
        Core.View.Dc2MemGPath(vp.GraphicsPath)

        'Add Active styles
        Dim edtr = Core.Editor
        vp.FillColor = edtr.FillColor
        vp.StrokeColor = edtr.StrokeColor
        vp.StrokWidth = edtr.strokeWidth
        vp.isFill = edtr.isFill
        vp.isStroke = edtr.isStroke

        'Add it to Memory
        Core.View.Memory.Layers(0).Item.Add(vp)

        Core.View.Refresh()
    End Sub
End Class
