Imports System.Drawing

<Serializable()> Public Class Layer
    Implements IDisposable

    Private _lock, vwble As Boolean
    Private lc As Color
    Private nme As String

    '/////////////////////
    Private _items As List(Of vItem)

    Public Sub New()

        Static Dim nmc As Integer = 0
        nme = "Layer #" & nmc


        Visible = True
        Lock = True
        _items = New List(Of vItem)
    End Sub



    Public Property Name() As String
        Get
            Return nme
        End Get
        Set(ByVal value As String)
            nme = value
        End Set
    End Property

    Public Property Lock() As Boolean
        Get
            Return _lock
        End Get
        Set(ByVal value As Boolean)
            _lock = value
        End Set
    End Property

    Public Property LayerColor() As Color
        Get
            Return lc
        End Get
        Set(ByVal value As Color)
            value = lc
        End Set
    End Property

    Public Property Visible() As Boolean
        Get
            Return vwble
        End Get
        Set(ByVal value As Boolean)
            vwble = value
        End Set
    End Property


    Public ReadOnly Property Item() As List(Of vItem)
        Get
            Return _items
        End Get
    End Property



    Public Sub Draw(ByVal g As Graphics)
        For Each i As vItem In _items
            i.Draw(g)
        Next
    End Sub

    '  Public Function Copy() As Layer

    'End Function




#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
            For Each itm As IDisposable In _items
                itm.Dispose()
            Next
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