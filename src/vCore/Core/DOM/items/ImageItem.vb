Imports System.Drawing

Public Class ImageItem
    Implements vItem


    Dim _img As Image


    Public Sub New()

    End Sub
    Public Sub New(file As String)
        _img = Image.FromFile(file)
        bound = _img.GetBounds(GraphicsUnit.Pixel)
    End Sub

    Public Property url As String
    Public Property bound As RectangleF


    Public Sub Draw(ByRef g As Drawing.Graphics) Implements vItem.Draw
        g.DrawImage(_img, bound)
    End Sub

    Public Function GetBound() As Drawing.RectangleF Implements vItem.GetBound
        Return bound
    End Function

    Public Function HitTest(p As Drawing.PointF) As Boolean Implements vItem.HitTest
        Return bound.Contains(p)
    End Function

    Public Sub Translate(x As Single, y As Single) Implements vItem.Translate

    End Sub

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If
            _img.Dispose()
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
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
