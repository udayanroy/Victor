Imports System.Drawing.Drawing2D
Imports System.Drawing
Imports Geometry

<Serializable()> Public Class vPath
    Implements DrawingElement



    Friend pth As GPath


    Public Sub New()
        pth = New GPath
        pth.AddEllipse(100, 100, 400, 200)
    End Sub

    Public Property FillColor As Color = Color.Blue
    Public Property StrokeColor As Color = Color.Black
    Public Property StrokWidth As Single = 1
    Public Property isFill As Boolean = True
    Public Property isStroke As Boolean = True


    Public Sub Draw(ByRef g As System.Drawing.Graphics) Implements DrawingElement.Draw
        'g.FillPath(Drawing.Brushes.Blue, pth)
        Dim brush As Brush = Nothing
        Dim pen As Pen = Nothing
        If isFill Then brush = New SolidBrush(FillColor)
        If isStroke Then
            Dim penBrush As New SolidBrush(StrokeColor)
            pen = New Pen(penBrush, StrokWidth)
        End If

        pth.drawPath(g, pen, brush)


    End Sub

    Public Function GetBound() As System.Drawing.RectangleF Implements DrawingElement.GetBound
        Return pth.GetTightBound()
    End Function


    Public Function HitTest(ByVal location As System.Drawing.PointF) As Boolean Implements DrawingElement.HitTest
        Dim isbodyvisible, isOutline As Boolean

        Dim bound = Me.GraphicsPath.GetBound
        If bound.Contains(location) Then
            Using gp As GraphicsPath = Me.pth.ToGraphicsPath
                isbodyvisible = gp.IsVisible(location)
                If Not isbodyvisible Then
                    Dim penwidth = IIf(Me.isStroke, Me.StrokWidth, 1)
                    Using Pen = New Pen(Brushes.Black, penwidth)
                        isOutline = gp.IsOutlineVisible(location, Pen)
                    End Using
                End If
            End Using
        End If


        Return isbodyvisible Or isOutline


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

    Public Sub Translate(ByVal x As Single, ByVal y As Single) Implements DrawingElement.Translate
        Using mat As New Drawing2D.Matrix
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
