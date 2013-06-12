
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Drawing.Drawing2D

Public Class tMoveTool
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
        ' md = e.Location
        ' mdl = e.Location
        's = v.Editor.SelectAt(mdl)

        'If s <> 0 Then
        'svp = v.Editor.getSelectionPath.GraphicsPath.Clone
        ' v.View.mem2DcPath(svp)
        ' v.View.BufferGraphics.Initialize()
        ' End If
        v.Editor.mouse_Down(e)
    End Sub

    Private Sub dc_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dc.MouseMove
        'If e.Button = MouseButtons.Left Then
        ' If s <> 0 Then
        'v.View.BufferGraphics.Clear()
        ' Dim gc As GraphicsContainer
        ' gc = v.View.BufferGraphics.Graphics.BeginContainer
        'svp.
        ' Using mat As New Matrix
        'mat.Translate((e.Location.X - mdl.X), (e.Location.Y - mdl.Y))
        'svp.Transform(mat)
        ' End Using
        ' v.View.BufferGraphics.Graphics.TranslateClip(-(e.Location.X - mdl.X), -(e.Location.Y - mdl.Y))
        'v.View.BufferGraphics.Graphics.DrawPath(Pens.Black, svp)

        'v.View.BufferGraphics.Graphics.EndContainer(gc)
        'v.View.BufferGraphics.Render()

        'mdl = e.Location
        'End If
        'End If
        v.Editor.mouse_Move(e)
    End Sub
    Private Sub dc_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dc.MouseUp

        
        ' Dim p1 = v.View.Dc2memPt(md)
        ' Dim p2 = v.View.Dc2memPt(e.Location)
        ' v.Editor.getSelectionPath.Translate(p2.X - p1.X, p2.Y - p1.Y)
        'v.View.Refresh()
        'svp.Dispose()
        v.Editor.mouse_Up(e)
    End Sub
    Public Sub DeSelectTool() Implements Itool.DeSelectTool
        dc = Nothing
    End Sub

    Public Sub SelectTool(ByRef d As advancedPanel) Implements Itool.SelectTool
        dc = d

        v.Editor.setEditingType(selectionType.Move)
    End Sub
End Class
