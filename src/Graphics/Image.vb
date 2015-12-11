Imports System.Drawing

Public Class Image
    Private _bmp As Bitmap

    Public Sub New(width As Integer, height As Integer)
        _bmp = New Bitmap(width, height)
        bound = _bmp.GetBounds(GraphicsUnit.Pixel)
    End Sub
    Public Sub New(file As String)
        _bmp = New Bitmap(file)
        bound = _bmp.GetBounds(GraphicsUnit.Pixel)
    End Sub

    Public Property bound As RectangleF

    Friend Function GetBitmap() As Bitmap
        Return _bmp
    End Function

     

End Class
