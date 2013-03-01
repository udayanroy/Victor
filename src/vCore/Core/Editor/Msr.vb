Imports System.Drawing
Imports System.Drawing.Drawing2D

Public Class Msr
    Implements Iedtr


    Private bound As New Rectangle(30, 30, 200, 200)
    Private b As Integer = 3


    Public Sub New()

    End Sub



    Public Sub Draw(ByRef g As System.Drawing.Graphics, ByVal mat As matrix) Implements Iedtr.Draw
        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        g.DrawRectangle(Pens.Black, bound)

        drawcircle(g, bound.X, bound.Y)
        drawcircle(g, bound.X, bound.Y + bound.Height / 2)
        drawcircle(g, bound.X, bound.Y + bound.Height)

        drawcircle(g, bound.X + bound.Width / 2, bound.Y)
        drawcircle(g, bound.X + bound.Width / 2, bound.Y + bound.Height)

        drawcircle(g, bound.X + bound.Width, bound.Y)
        drawcircle(g, bound.X + bound.Width, bound.Y + bound.Height / 2)
        drawcircle(g, bound.X + bound.Width, bound.Y + bound.Height)

    End Sub
    Private Sub drawcircle(ByVal g As Graphics, ByVal x As Integer, ByVal y As Integer)
        g.FillEllipse(Brushes.Blue, x - b, y - b, b * 2, b * 2)
    End Sub


End Class
