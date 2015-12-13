 

Public Class BufferPaint
    Implements IDisposable

    Dim bk, mn As BufferMemory
    Dim dc As IDevice
    Dim hdc As IntPtr

    Friend Sub New(ByRef c As IDevice)
        dc = c
        hdc = GetDC(dc.Handle)
        bk = New BufferMemory(hdc, dc.Width, dc.Height)
        bk.Graphics.Clear(Color.White)
        mn = New BufferMemory(hdc, dc.Width, dc.Height)
        mn.Graphics.Clear(Color.White)
    End Sub

    Public Sub Initialize()
        If bk.size <> dc.Size Then
            '// delete memory device
            bk.Dispose()
            '// create Memory device.
            bk = New BufferMemory(hdc, dc.Width, dc.Height)
        End If
        If mn.size <> dc.Size Then
            '// delete memory device
            mn.Dispose()
            '// create Memory device.
            mn = New BufferMemory(hdc, dc.Width, dc.Height)
        End If
       

        '// update back memory device.
        Me.Update()

        '// update main memory device.
        Me.Clear()
    End Sub

    Public Sub Clear()
        bk.Render(mn.HDC)
    End Sub
    Public Sub Render()
        mn.Render(hdc)
    End Sub
    Private Sub Update()
        bk.UpdateFrmScr(hdc)
    End Sub

    Private Sub Render(ByVal _hdc As IntPtr)

        mn.Render(_hdc)

    End Sub
    Public ReadOnly Property Graphics() As Graphics.Canvas
        Get
            Return mn.Graphics
        End Get
    End Property




    Private Class BufferMemory
        Implements IDisposable

        Dim tdc, obj, bmp As IntPtr
        Dim _width, _height As Integer
        Dim g As Graphics

        Public Sub New(ByVal hdc As IntPtr, ByVal Width As Integer, ByVal Height As Integer)

            tdc = NativeFunction.CreateCompatibleDC(hdc)
            bmp = NativeFunction.CreateCompatibleBitmap(hdc, Width, Height)
            obj = NativeFunction.SelectObject(tdc, bmp)

            _width = Width
            _height = Height

            g = Graphics.FromHdcInternal(tdc)
            g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        End Sub
        Public Sub Render(ByVal hdc As IntPtr)
            BitBlt(hdc, 0, 0, _width, _height, tdc, 0, 0, SRCCOPY)
        End Sub
        Public Sub UpdateFrmScr(ByVal hdc As IntPtr)
            BitBlt(tdc, 0, 0, _width, _height, hdc, 0, 0, SRCCOPY)
        End Sub
        Public ReadOnly Property Graphics() As canvas
            Get
                Return New canvas(g)
            End Get
        End Property
        Public ReadOnly Property HDC() As IntPtr
            Get
                Return tdc
            End Get
        End Property
        Public ReadOnly Property size As Size
            Get
                Return New Size(Me._width, Me._height)
            End Get
        End Property

#Region "Dispose "
        Private disposedValue As Boolean = False        ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: free other state (managed objects).
                    g.Dispose()
                End If

                ' TODO: free your own state (unmanaged objects).
                ' TODO: set large fields to null.
                DeleteDC(tdc)
                DeleteObject(bmp)
                DeleteObject(obj)


            End If
            Me.disposedValue = True
        End Sub

#Region " IDisposable Support "
        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

#End Region


    End Class


#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            If bk IsNot Nothing Then bk.Dispose()
            If mn IsNot Nothing Then mn.Dispose()
            NativeFunction.ReleaseDC(dc.Handle, hdc)
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
