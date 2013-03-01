Imports System.Windows.Forms
Imports System.Drawing
Imports System.Drawing.Drawing2D

Public Class vCore

    Friend mem As DOM
    Private vu As View
    Private edtr As vEditor
    Private WithEvents device As advancedPanel

    Private tool As Tools

    Public Sub New(ByVal dc As advancedPanel, ByVal pagesize As Size)

        device = dc
        mem = New DOM()

        mem.Layers(0).Item.Add(New vPath)
        Dim vp As New vPath
        vp.pth = New Graphicspath
        vp.pth.AddRectangle(New Rectangle(-20, 0, 30, 30))
        mem.Layers(0).Item.Add(vp)

        vu = New View(mem, device, pagesize)
        edtr = New vEditor(Me)


        tool = New Tools(Me)


    End Sub
    Public ReadOnly Property View() As View
        Get
            Return vu
        End Get
    End Property
    Public ReadOnly Property pDevice() As advancedPanel
        Get
            Return device
        End Get
    End Property
    Public ReadOnly Property Editor() As vEditor
        Get
            Return edtr
        End Get
    End Property
    Public Sub selectTool(ByVal t As Integer)
        tool.SelectTool(t)
    End Sub

    Private Sub device_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles device.Paint
        vu.paint(e.Graphics)
        edtr.paint(e.Graphics)
    End Sub
End Class
