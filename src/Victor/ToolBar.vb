﻿Imports System.Reflection

Public Class ToolBar
    Inherits Panel

    Dim toolwidth = 16
    Dim selectionIndex = 5
    Dim mouseOveron = 0

    Public core As vCore.vCore

    Private Tiles As List(Of ToolTile)


    Public Sub New()
        Me.DoubleBuffered = True

        InitTools()
    End Sub

    Private Sub InitTools()
        
        Tiles = New List(Of ToolTile)()

        Tiles.Add(New ToolTile(My.Resources.Path_selection_tool, 16))
        Tiles.Add(New ToolTile(My.Resources.Direct_selection_tool, 8))
        Tiles.Add(New ToolTile(My.Resources.Ellipse_tool, 4))
        Tiles.Add(New ToolTile(My.Resources.Square, 13))
        Tiles.Add(New ToolTile(My.Resources.Line, 14))
        Tiles.Add(New ToolTile(My.Resources.Pen_tool, 10))
        Tiles.Add(New ToolTile(My.Resources.Transform_2_icon__1_, 17))
        Tiles.Add(New ToolTile(My.Resources.Rotate_icon, 7))
        Tiles.Add(New ToolTile(My.Resources.Transform_2_icon__1_, 18))
        Tiles.Add(New ToolTile(My.Resources.Hand_tool, 1))
        Tiles.Add(New ToolTile(My.Resources.Connect_point_tool, 9))
        Tiles.Add(New ToolTile(My.Resources.Add_anchor_point_tool, 12))
        Tiles.Add(New ToolTile(My.Resources.Delete_anchor_point_tool, 11))
        ' Tiles.Add(New ToolTile(IconBase & "Transform.png", 6))

    End Sub



    Private Sub ToolBar_MouseLeave(sender As Object, e As EventArgs) Handles Me.MouseLeave
        mouseOveron = 0
        Me.Refresh()
    End Sub

    Private Sub ToolBar_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
        Dim sindex = GetSelectionIndex(e.Location)

        If sindex <> mouseOveron Then
            mouseOveron = sindex
            Me.Refresh()
        End If
    End Sub

    Private Sub ToolBar_MouseUp(sender As Object, e As MouseEventArgs) Handles Me.MouseUp
        Dim sindex = GetSelectionIndex(e.Location)

        If sindex > 0 AndAlso sindex <> selectionIndex Then
            selectionIndex = sindex
            Me.Refresh()
            OnToolSelectionChanged()
        End If
    End Sub

    Private Function GetSelectionIndex(loc As Point) As Integer
        Dim index = -1

        Dim totalTools = GetTotalCount()

        Dim width = Me.Width
        Dim collum As Integer = width \ (toolwidth + 5)

        Dim row As Integer = totalTools / collum

        Dim x = 5
        Dim y = 5

        Dim count = 1

        For i As Integer = 1 To row
            For j As Integer = 1 To collum
                If count > totalTools Then Exit For

                Dim rect As New Rectangle(x, y, toolwidth, toolwidth)
                If rect.Contains(loc) Then
                    Return count
                End If

                x += toolwidth + 5
                count += 1
            Next
            x = 5
            y += toolwidth + 5
        Next

        Return index

    End Function

    Private Function GetTotalCount() As Integer
        Return Tiles.Count
    End Function

    Private Sub OnToolSelectionChanged()
        If core IsNot Nothing Then core.selectTool(Tiles(selectionIndex - 1).ToolId)
    End Sub

    Private Sub ToolBar_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
        Dim totalTools = GetTotalCount()

        Dim width = Me.Width
        Dim collum As Integer = width \ (toolwidth + 5)

        Dim row As Integer = totalTools / collum

        Dim x = 5
        Dim y = 5

        Dim count = 1

        For i As Integer = 1 To row
            For j As Integer = 1 To collum
                If count > totalTools Then Exit For

                If selectionIndex = count Then
                    e.Graphics.FillRectangle(Brushes.SkyBlue, x - 2, y - 2, toolwidth + 4, toolwidth + 4)
                ElseIf count = mouseOveron Then
                    e.Graphics.DrawRectangle(Pens.Green, x - 2, y - 2, toolwidth + 4, toolwidth + 4)
                End If
                If Tiles(count - 1).Icon IsNot Nothing Then
                    e.Graphics.DrawImage(Tiles(count - 1).Icon, x, y, 16, 16)
                End If


                x += toolwidth + 5
                count += 1
            Next
            x = 5
            y += toolwidth + 5
        Next
    End Sub

    Private Class ToolTile

        Public Sub New(url As String, id As Integer)
            Me.iconUrl = url
            Me.ToolId = id
            '
            Icon = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory & Me.iconUrl)
        End Sub
        Public Sub New(bmp As Bitmap, id As Integer)
            Me.ToolId = id
            Icon = bmp
        End Sub

        Public Property Name As String
        Public Property iconUrl As String
        Public Property ToolId As Integer
        Public Property Icon As Image
    End Class
End Class
