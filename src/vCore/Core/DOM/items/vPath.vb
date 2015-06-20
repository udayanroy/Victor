Imports System.Drawing.Drawing2D

Public Class vPath
    Implements vItem





    Friend pth As GPath

    Friend mode As List(Of Boolean)



    Public Sub New()
        pth = New GPath
        pth.AddEllipse(100, 100, 400, 200)
    End Sub


    Public Sub Draw(ByRef g As System.Drawing.Graphics) Implements vItem.Draw
        'g.FillPath(Drawing.Brushes.Blue, pth)
        pth.drawPath(g, Nothing, Drawing.Brushes.Blue)
    End Sub

    Public Function GetBound() As System.Drawing.RectangleF Implements vItem.GetBound
        Return pth.GetBound()
    End Function


    Public Function HitTest(ByVal p As System.Drawing.PointF) As Boolean Implements vItem.HitTest
        Return pth.IsVisible(p)
    End Function

    Public ReadOnly Property GraphicsPath As GPath
        Get
            Return pth
        End Get
    End Property
    Public Sub setPath(ByRef path As GPath)
        ' pth.Dispose()
        pth = path
    End Sub

    Public Sub Translate(ByVal x As Single, ByVal y As Single) Implements vItem.Translate
        Using mat As New Matrix
            mat.Translate(x, y)

            pth.Transform(mat)
        End Using

    End Sub
#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If

            'delete path
            ' pth.Dispose()

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
