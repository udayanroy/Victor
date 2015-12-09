Imports System.Drawing
Imports System.Drawing.Drawing2D

Public Class EditableGraphicsPath
    Dim _pts() As PointF
    Dim _typ() As Byte
    Dim _mod() As Byte








    Public Sub New()

    End Sub
    Public Function GetGraphicsPath() As GraphicsPath
        Dim gp As New GraphicsPath(_pts, _typ)
        Return gp
    End Function

    Public Sub Draw(ByVal g As Graphics, Optional ByVal pen As Pen = Nothing, Optional ByVal brush As Brush = Nothing)
        If (pen IsNot Nothing) Or (brush IsNot Nothing) Then
            Using gp As GraphicsPath = Me.GetGraphicsPath
                If brush IsNot Nothing Then
                    g.FillPath(brush, gp)
                End If
                If pen IsNot Nothing Then
                    g.DrawPath(pen, gp)
                End If
            End Using
        End If
    End Sub


End Class
