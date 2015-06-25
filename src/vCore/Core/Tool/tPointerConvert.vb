﻿Imports System.Drawing
Imports System.Windows.Forms
Imports System.Drawing.Drawing2D

Public Class tPointerConvert
    Implements Itool

    Dim v As vCore
    Dim WithEvents dc As advancedPanel
    Public Sub New(ByRef vew As vCore)
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
        v.Editor.setEditingType(selectionType.PointerConvert)
    End Sub
End Class
