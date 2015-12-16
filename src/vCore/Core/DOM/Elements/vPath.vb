 
Imports Geometry
Imports Graphics

<Serializable()> Public Class vPath
    Implements DrawingElement




    Private _Path As NodePath


    Public Sub New()
        _Path = New NodePath
    End Sub

    Public ReadOnly Property Path As NodePath
        Get
            Return _Path
        End Get
    End Property
     
  
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


#Region "Element Drawing"

    Public Property Brush As Graphics.Brush Implements DrawingElement.Brush
    Public Property Pen As Graphics.Pen Implements DrawingElement.Pen
    Public Property Opacity As Single Implements DrawingElement.Opacity

    Public Sub Draw(canvas As Canvas) Implements DrawingElement.Draw

        canvas.DrawPath(Me.Path, Pen, Brush)

    End Sub

    Public Function GetElementBound() As Rect Implements DrawingElement.GetElementBound
        Dim mat As Matrix = Matrix.Identity
        mat.Rotate(Me.Rotation)
        Dim Duplicatepath = Me.Path.Clone
        Duplicatepath.Transform(mat)
        Return Duplicatepath.GetBound()
    End Function

    Public Function GetElementType() As ElementType Implements DrawingElement.GetElementType
        Return ElementType.PathElement
    End Function

#End Region

#Region "HitTesting"
    Public Function isBoundVisible(p As Geometry.Point) As Boolean Implements DrawingElement.isBoundVisible
        Throw New NotImplementedException()
    End Function

    Public Function isBoundVisible(rect As Rect) As Boolean Implements DrawingElement.isBoundVisible
        Throw New NotImplementedException()
    End Function

    Public Function isVisible(p As Geometry.Point) As Boolean Implements DrawingElement.isVisible
        Return Path.isVisible(p)
    End Function

    Public Function isVisible(rect As Rect) As Boolean Implements DrawingElement.isVisible
        Throw New NotImplementedException()
    End Function
#End Region

#Region "Transform Function"

    Public Function GetItemBound() As Rect Implements DrawingElement.GetItemBound
        Throw New NotImplementedException()
    End Function

    Public Function GetSkeliton() As NodePath Implements DrawingElement.GetSkeliton
        Dim mat As Matrix = Matrix.Identity
        mat.Rotate(Me.Rotation)
        Dim Duplicatepath = Me.Path.Clone
        Duplicatepath.Transform(mat)
        Return Duplicatepath
    End Function

    Public Sub ReArrange(x As Single, y As Single, width As Single, height As Single) Implements DrawingElement.ReArrange
        Throw New NotImplementedException()
    End Sub

    Public Sub Resize(width As Single, height As Single) Implements DrawingElement.Resize
        Throw New NotImplementedException()
    End Sub

    Public Property Rotation As Single Implements DrawingElement.Rotation

    Public Sub Translate(ByVal x As Single, ByVal y As Single) Implements DrawingElement.Translate
        Dim mat As Matrix = Matrix.Identity
        mat.Translate(x, y)
        ApplyTransform(mat)
    End Sub

    Public Sub ApplyTransform(mat As Geometry.Matrix) Implements DrawingElement.ApplyTransform
        Path.Transform(mat)
    End Sub

#End Region

    Public Sub setPath(editablepath As NodePath)
        Me._Path = editablepath
    End Sub


End Class
