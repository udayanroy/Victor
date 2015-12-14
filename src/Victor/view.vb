Imports System.Drawing.Imaging
Imports Core

Public Class view
    Dim core As Core.vCore
    Dim pagesz As New Size(1000, 500)

    Public Property FileName As String

    Private Sub Panel1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub view_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If FileName = "" Then
            core = New Core.vCore(Panel1, pagesz)
        Else
            core = New Core.vCore(Panel1, FileName)
        End If

        ToolBar1.core = core
        ' ToolBar1.Width = 47

        'update GUI variables
        SelectionPropertyChanged()

        AddHandler core.Editor.PropertyChanged, AddressOf SelectionPropertyChanged
        AddHandler core.Editor.SelectionChanged, AddressOf SelectionChanged
    End Sub






    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Application.Exit()
    End Sub

    Private Sub DeleteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteToolStripMenuItem.Click
        core.Editor.Delete()
    End Sub

    Private Sub ClearAllToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ClearAllToolStripMenuItem.Click
        core.Editor.ClearAll()
    End Sub

    Private Sub PastToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PastToolStripMenuItem.Click
        core.Editor.Past()
    End Sub

    Private Sub CopyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyToolStripMenuItem.Click
        core.Editor.Copy()
    End Sub

    Private Sub CutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CutToolStripMenuItem.Click
        core.Editor.Cut()
    End Sub

    Private Sub ExportToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportToolStripMenuItem.Click

        Using savedlg As New SaveFileDialog
            savedlg.Filter = "JPeg Image|*.jpg|PNG Image|*.png|Bitmap Image|*.bmp|Gif Image|*.gif"
            savedlg.Title = "Export an Image File"
            savedlg.FileName = "untitle"
            If savedlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then

                If savedlg.FileName <> "" Then
                    Dim Imgformat As ImageFormat = Nothing
                    Dim bg = False
                    Select Case savedlg.FilterIndex
                        Case 1
                            Imgformat = ImageFormat.Jpeg
                            bg = True
                        Case 2
                            Imgformat = ImageFormat.Png
                        Case 3
                            Imgformat = ImageFormat.Bmp
                            bg = True
                        Case 4
                            Imgformat = ImageFormat.Gif
                            bg = True
                        Case Else
                            Imgformat = ImageFormat.Jpeg
                            bg = True
                    End Select
                    ' Export.ExportImage(savedlg.FileName, core.Memory, core.View.GetPageSize, Imgformat, bg)
                End If

            End If
        End Using

    End Sub

    Private Sub Panel3_Click(sender As Object, e As EventArgs) Handles Panel3.Click
        Using clrDlg As New ColorDialog
            If clrDlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                Dim color = clrDlg.Color
                Panel3.BackColor = color
                'core.Editor.FillColor = color
            End If
        End Using
    End Sub

    Private Sub SelectionPropertyChanged()
        'Panel3.BackColor = core.Editor.FillColor
        'Panel4.BackColor = core.Editor.StrokeColor
        'ComboBox1.SelectedIndex = CInt(core.Editor.strokeWidth)
        'CheckBox1.Checked = core.Editor.isFill
        'CheckBox2.Checked = core.Editor.isStroke
    End Sub

    Private Sub SelectionChanged()
        Dim enable = Not core.Editor.selection.isEmty
        CutToolStripMenuItem.Enabled = enable
        CopyToolStripMenuItem.Enabled = enable
        DeleteToolStripMenuItem.Enabled = enable

    End Sub


    Private Sub Panel4_Click(sender As Object, e As EventArgs) Handles Panel4.Click
        Using clrDlg As New ColorDialog
            If clrDlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                Dim color = clrDlg.Color
                Panel4.BackColor = color
                'core.Editor.StrokeColor = color
            End If
        End Using
    End Sub

    Private Sub ComboBox1_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles ComboBox1.SelectionChangeCommitted
        ' core.Editor.strokeWidth = ComboBox1.SelectedIndex
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        'core.Editor.isFill = CheckBox1.Checked
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        ' core.Editor.isStroke = CheckBox2.Checked
    End Sub

    Private Sub MinimizeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MinimizeToolStripMenuItem.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub MaximizeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MaximizeToolStripMenuItem.Click
        Me.WindowState = FormWindowState.Maximized
    End Sub

    Private Sub NewToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewToolStripMenuItem.Click
        Using ndlg As New NewDlg
            If ndlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                Dim newwindow As New view
                newwindow.pagesz = New Size(ndlg.PageWidth, ndlg.PageHeight)
                newwindow.Show()
            End If
        End Using


    End Sub

    Private Sub SaveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveToolStripMenuItem.Click
        If core.AsociateFile = "" Then
            saveAS()
        Else
            core.save()
        End If
    End Sub

    Private Sub SaveAsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveAsToolStripMenuItem.Click
        saveAS()
    End Sub

    Private Sub saveAS()
        Using savedlg As New SaveFileDialog
            savedlg.Filter = "Victor Image|*.vimg"
            savedlg.Title = "Save As"
            savedlg.FileName = "untitle"

            If savedlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                If savedlg.FileName <> "" Then
                    core.SaveDocumentTo(savedlg.FileName)
                End If
            End If

        End Using
    End Sub

    Private Sub OpenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenToolStripMenuItem.Click
        Using opendlg As New OpenFileDialog
            opendlg.Filter = "Victor Image|*.vimg"
            opendlg.Title = "Open File"

            If opendlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                Dim newwindow As New view
                newwindow.FileName = opendlg.FileName
                newwindow.Show()
            End If

        End Using
    End Sub

    Private Sub ImportToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ImportToolStripMenuItem.Click
        Using opendlg As New OpenFileDialog
            opendlg.Filter = "Image File|*.png;*.jpg;*.BMP;*.gif"
            opendlg.Title = "Import File"

            If opendlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                core.Import(opendlg.FileName)
            End If

        End Using
    End Sub
End Class