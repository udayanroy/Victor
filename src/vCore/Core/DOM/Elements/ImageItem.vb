Imports System.Drawing

Public Class ImageItem
    Implements DrawingElement





    Dim _img As Image


    Public Sub New()

    End Sub
    Public Sub New(file As String)
        _img = Image.FromFile(file)
        bound = _img.GetBounds(GraphicsUnit.Pixel)
    End Sub

    Public Property url As String
    Public Property bound As RectangleF


    Public Sub Translate(x As Single, y As Single) Implements DrawingElement.Translate

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

    Public Sub ApplyTransform(mat As Geometry.Matrix) Implements DrawingElement.ApplyTransform

    End Sub

    Public Property Brush As Graphics.Brush Implements DrawingElement.Brush

    Public Sub Draw(canvas As Graphics.Canvas) Implements DrawingElement.Draw

    End Sub

    Public Function GetElementBound() As Geometry.Rect Implements DrawingElement.GetElementBound

    End Function

    Public Function GetElementType() As ElementType Implements DrawingElement.GetElementType

    End Function

    Public Function GetItemBound() As Geometry.Rect Implements DrawingElement.GetItemBound

    End Function

    Public Function GetSkeliton() As Geometry.NodePath Implements DrawingElement.GetSkeliton
        Return Nothing
    End Function

    Public Function isBoundVisible(p As Geometry.Point) As Boolean Implements DrawingElement.isBoundVisible

    End Function

    Public Function isBoundVisible(rect As Geometry.Rect) As Boolean Implements DrawingElement.isBoundVisible

    End Function

    Public Function isVisible(p As Geometry.Point) As Boolean Implements DrawingElement.isVisible

    End Function

    Public Function isVisible(rect As Geometry.Rect) As Boolean Implements DrawingElement.isVisible

    End Function

    Public Property Opacity As Single Implements DrawingElement.Opacity

    Public Property Pen As Graphics.Pen Implements DrawingElement.Pen

    Public Sub ReArrange(x As Single, y As Single, width As Single, height As Single) Implements DrawingElement.ReArrange

    End Sub

    Public Sub Resize(width As Single, height As Single) Implements DrawingElement.Resize

    End Sub

    Public Property Rotation As Single Implements DrawingElement.Rotation

    Public Sub ApplyTransform(Transform As Geometry.Transform) Implements DrawingElement.ApplyTransform

    End Sub
End Class
