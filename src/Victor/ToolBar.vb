Public Class ToolBar
    Inherits Panel

    Dim toolwidth = 32
    Dim selectionIndex = 5
    Dim mouseOveron = 0

    Public core As vCore.vCore


    Public Sub New()
        Me.DoubleBuffered = True
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
        Dim collum As Integer = (width + 5) \ toolwidth

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
        Return 12
    End Function

    Private Sub OnToolSelectionChanged()
        If core IsNot Nothing Then core.selectTool(selectionIndex)
    End Sub

    Private Sub ToolBar_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
        Dim totalTools = GetTotalCount()

        Dim width = Me.Width
        Dim collum As Integer = (width + 5) \ toolwidth

        Dim row As Integer = totalTools / collum

        Dim x = 5
        Dim y = 5

        Dim count = 1

        For i As Integer = 1 To row
            For j As Integer = 1 To collum
                If count > totalTools Then Exit For

                If selectionIndex = count Then
                    e.Graphics.FillRectangle(Brushes.Red, x, y, toolwidth, toolwidth)
                ElseIf count = mouseOveron Then
                    e.Graphics.FillRectangle(Brushes.Green, x, y, toolwidth, toolwidth)
                Else
                    e.Graphics.FillRectangle(Brushes.Blue, x, y, toolwidth, toolwidth)
                End If


                x += toolwidth + 5
                count += 1
            Next
            x = 5
            y += toolwidth + 5
        Next
    End Sub


End Class
