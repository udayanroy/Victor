Public Class view
    Dim core As vCore.vCore
    Dim pagesz As New Size(500, 500)

    Private Sub Panel1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub view_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        core = New vCore.vCore(Panel1, pagesz)
    End Sub

   
   

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        core.selectTool(CInt(TextBox1.Text))
    End Sub
End Class