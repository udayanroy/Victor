Imports System.Drawing.Drawing2D

Public Class vPath
    Implements vItem



    Friend pth As GraphicsPath




    Public Sub New()
        pth = New GraphicsPath
        pth.AddEllipse(100, 100, 400, 200)
    End Sub


    Public Sub Draw(ByRef g As System.Drawing.Graphics) Implements vItem.Draw
        g.FillPath(Drawing.Brushes.Blue, pth)
    End Sub

    Public Function GetBound() As System.Drawing.RectangleF Implements vItem.GetBound
        Return pth.GetBounds()
    End Function


    Public Function HitTest(ByVal p As System.Drawing.PointF) As Boolean Implements vItem.HitTest
        Return pth.IsVisible(p)
    End Function
End Class
