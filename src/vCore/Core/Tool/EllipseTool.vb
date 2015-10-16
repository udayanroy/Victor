Imports System.Drawing
Imports System.Windows.Forms
Imports System.Drawing.Drawing2D

Public Class EllipseTool
    Implements Itool




    Dim v As vCore
    Dim WithEvents dc As advancedPanel
    Dim mdl As Point

    Public Sub New(ByRef vcore As vCore)
        v = vcore
    End Sub
    Public ReadOnly Property Device() As advancedPanel Implements Itool.Device
        Get
            Return dc
        End Get
    End Property

    Private Sub dc_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dc.MouseDown
        mdl = e.Location
        v.View.BufferGraphics.Initialize()
        dc.ActiveScroll = False
    End Sub

    Private Sub dc_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dc.MouseMove
        If e.Button = MouseButtons.Left Then
            v.View.BufferGraphics.Clear()
            Dim width As Integer = (e.Location.X - mdl.X)
            Dim height As Integer = (e.Location.Y - mdl.Y)

            v.View.BufferGraphics.Graphics.DrawEllipse(Pens.Black, mdl.X, mdl.Y, width, height)
            v.View.BufferGraphics.Render()



        End If
    End Sub
    Private Sub dc_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dc.MouseUp



        Dim vp As New vPath
        vp.GraphicsPath.subpaths.clear()
        vp.GraphicsPath.AddEllipse(mdl.X, mdl.Y, e.Location.X - mdl.X, e.Location.Y - mdl.Y)

        'Convert path to memory path
        v.View.Dc2MemGPath(vp.GraphicsPath)

        'Add Active styles
        Dim edtr = v.Editor
        vp.FillColor = edtr.FillColor
        vp.StrokeColor = edtr.StrokeColor
        vp.StrokWidth = edtr.strokeWidth
        vp.isFill = edtr.isFill
        vp.isStroke = edtr.isStroke

        'Add it to Memory
        v.View.Memory.Layers(0).Item.Add(vp)

        v.View.Refresh()
        dc.ActiveScroll = True
    End Sub
    Public Sub DeSelectTool() Implements Itool.DeSelectTool
        dc = Nothing

    End Sub

    Public Sub SelectTool(ByRef d As advancedPanel) Implements Itool.SelectTool
        dc = d
        v.Editor.setEditingType(selectionType.None)
    End Sub


End Class
