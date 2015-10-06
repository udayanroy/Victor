Public Class view
    Dim core As vCore.vCore
    Dim pagesz As New Size(500, 500)

    Private Sub Panel1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub view_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        core = New vCore.vCore(Panel1, pagesz)
        ToolBar1.core = core
        ' ToolBar1.Width = 47
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
End Class