Imports System.Drawing.Drawing2D
Imports System.Drawing

Public Class DOM
    Private memlist As List(Of Layer)







    Friend Sub New()
        memlist = New List(Of Layer)

        memlist.Add(New Layer)
    End Sub
    Public ReadOnly Property Layers() As List(Of Layer)
        Get
            Return memlist
        End Get
    End Property


    Public Sub Draw(ByRef g As Graphics, ByVal page_loc As Point)
        Dim gcon As GraphicsContainer = g.BeginContainer
        g.TranslateTransform(page_loc.X, page_loc.Y)
        g.SmoothingMode = SmoothingMode.AntiAlias
        For Each l As Layer In memlist
            l.Draw(g)
        Next

        g.EndContainer(gcon)

    End Sub

    

End Class