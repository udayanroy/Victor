Imports System.Drawing
Imports System.Drawing.Imaging

Public Class Export


    Public Shared Sub ExportImage(filename As String, doc As DOM, size As Size, imgtype As ImageFormat, Optional background As Boolean = False)

        Using bmp As New Bitmap(size.Width, size.Height), gp = Graphics.FromImage(bmp)
            If background Then
                gp.Clear(Color.White)
            End If
            doc.Draw(gp, New Point(0, 0))
            bmp.Save(filename, imgtype)
        End Using

    End Sub

 
End Class
