Imports System.Windows.Forms

Public Class NewDlg

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Dim text1 = TextBox1.Text
        Dim text2 = TextBox2.Text

        If Integer.TryParse(text1, PageHeight) And Integer.TryParse(text2, PageWidth) Then
            If PageHeight < 1 Or PageWidth < 1 Or PageHeight > 2000 Or PageWidth > 2000 Then
                MessageBox.Show("Please enter within 1 to 2000")
            Else
                Me.DialogResult = System.Windows.Forms.DialogResult.OK
                Me.Close()
            End If
        Else
            MessageBox.Show("Please enter a number")
        End If
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Public Property PageWidth As Integer = 400

    Public Property PageHeight As Integer = 400

End Class
