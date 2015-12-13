Imports System.Windows.Forms
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Runtime.Serialization.Formatters.Binary
Imports Graphics

Public Class vCore

    Friend mem As Document
    Private vu As ControlVisual
    Private edtr As Editor
    Private WithEvents device As advancedPanel

    Private tool As Tools

    Public Sub New(ByVal dc As advancedPanel, ByVal pagesize As Size)

        device = dc
        mem = New Document()
        mem.PageSize = pagesize

        vu = New ControlVisual(mem, device)
        edtr = New Editor(Me)


        tool = New Tools(Me)


    End Sub
    Public Sub New(ByVal dc As advancedPanel, ByVal file As String)

        Using strm As IO.FileStream = New IO.FileStream(file, IO.FileMode.Open, IO.FileAccess.ReadWrite, IO.FileShare.None)
            Dim Formatter = New BinaryFormatter()

            device = dc
            mem = Formatter.Deserialize(strm)

            vu = New ControlVisual(mem, device)
            edtr = New Editor(Me)


            tool = New Tools(Me)

        End Using



    End Sub

    Public ReadOnly Property View() As ControlVisual
        Get
            Return vu
        End Get
    End Property
    Public ReadOnly Property pDevice() As advancedPanel
        Get
            Return device
        End Get
    End Property
    Public ReadOnly Property Editor() As Editor
        Get
            Return edtr
        End Get
    End Property
    Public ReadOnly Property Memory As Document
        Get
            Return mem
        End Get
    End Property

    Public Property AsociateFile As String

    Public Sub selectTool(ByVal t As Integer)
        tool.SelectTool(t)
    End Sub

    Private Sub device_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles device.Paint
        Dim canvas As New Canvas(e.Graphics)
        vu.Draw(canvas)
        edtr.paint(canvas)
    End Sub

    Public Sub SaveDocumentTo(file As String)
        AsociateFile = file
        Using strm As IO.FileStream = New IO.FileStream(file, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite, IO.FileShare.None)
            strm.SetLength(0)
            Dim Formatter = New BinaryFormatter()
            Formatter.Serialize(strm, Me.mem)
        End Using
    End Sub

    Public Sub save()
        Using strm As IO.FileStream = New IO.FileStream(AsociateFile, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite, IO.FileShare.None)
            strm.SetLength(0)
            Dim Formatter = New BinaryFormatter()
            Formatter.Serialize(strm, Me.mem)
        End Using
    End Sub

    Public Sub Import(file As String)
        Dim imgitem As New ImageItem(file)
        mem.Layers(0).Item.Add(imgitem)
        View.Refresh()
    End Sub
End Class
