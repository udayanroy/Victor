
Imports Graphics
Imports Geometry

Public Class Document
    Implements IDrawable


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

    Public Property PageSize As Size

    Public Sub Draw(canvas As Canvas) Implements IDrawable.Draw
        canvas.Save()
        'canvas.Translate(page_loc.X, page_loc.Y)
        canvas.Smooth()
        For Each l As Layer In memlist
            l.Draw(canvas)
        Next

        canvas.Restore()

    End Sub

    Public Function GetArea() As Rect Implements IDrawable.GetArea
        Return New Rect(New Point(0, 0), PageSize)
    End Function
End Class