Imports System.Drawing.Imaging

Public Class view
    Dim core As vCore.vCore
    Dim pagesz As New Size(500, 500)

    Private Sub Panel1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub view_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        core = New vCore.vCore(Panel1, pagesz)
        ToolBar1.core = core
        ' ToolBar1.Width = 47
        AddHandler core.Editor.PropertyChanged, AddressOf SelectionPropertyChanged
    End Sub




    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        core.selectTool(CInt(TextBox1.Text))
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
                    Export.ExportImage(savedlg.FileName, core.View.Memory, core.View.GetPageSize, Imgformat, bg)
                End If

            End If
        End Using

    End Sub

    Private Sub Panel3_Click(sender As Object, e As EventArgs) Handles Panel3.Click
        Using clrDlg As New ColorDialog
            If clrDlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                Dim color = clrDlg.Color
                Panel3.BackColor = color
                core.Editor.FillColor = color
            End If
        End Using
    End Sub

    Private Sub SelectionPropertyChanged()
        Panel3.BackColor = core.Editor.FillColor
    End Sub
    
End Class