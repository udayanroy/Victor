Imports System.Drawing
Imports System.Windows.Forms
Imports System.Drawing.Drawing2D

Public Class tPointerConvert
    Implements Itool

    Dim v As View
    Dim WithEvents dc As advancedPanel
    Public Sub New(ByRef vew As View)
        v = vew
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
    End Sub
End Class
