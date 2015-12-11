Imports System.Drawing.Drawing2D
Imports System.Drawing

<Serializable()> Public Class Document
    Implements IDisposable

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

    Public Sub Draw(ByRef g As Graphics, ByVal page_loc As Point)
        Dim gcon As GraphicsContainer = g.BeginContainer
        g.TranslateTransform(page_loc.X, page_loc.Y)
        g.SmoothingMode = SmoothingMode.AntiAlias
        For Each l As Layer In memlist
            l.Draw(g)
        Next

        g.EndContainer(gcon)

    End Sub



#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If
            For Each itm As IDisposable In memlist
                itm.Dispose()
            Next
            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class