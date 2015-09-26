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
End Class