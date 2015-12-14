Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports System.Drawing

Public Class advancedPanel
    Inherits ScrollableControl
    Implements IDevice


    Dim ispaint As Boolean = True

    Public Sub New()
        'Me.SetStyle(ControlStyles.DoubleBuffer, True)
        'Me.SetStyle(ControlStyles.ResizeRedraw, True)
        'Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        'Me.SetStyle(ControlStyles.UserPaint, true)
        Me.DoubleBuffered = True

    End Sub




    Public Sub setScrollminsizepos(ByVal position As Point, ByVal size As Size)
        suspendDraw()
        Me.AutoScrollMinSize = size
        Me.AutoScrollPosition = position
        ResumeDraw()

    End Sub
    <DllImport("user32.dll")> _
    Private Shared Function SendMessage(ByVal hwnd As IntPtr, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    End Function
    Private Const WM_SETREDRAW As Integer = &HB
    Public Sub suspendDraw()
        SendMessage(Me.Handle, WM_SETREDRAW, 0, 0)

    End Sub
    Public Sub ResumeDraw()
        SendMessage(Me.Handle, WM_SETREDRAW, 1, 0)

    End Sub




    Private Sub advancedPanel_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
        Me.Focus()
        Dim ea As New Core.MouseEvntArg(e.Button, e.Clicks, e.X, e.Y, e.Delta)
        RaiseEvent MouseDown1(ea)
    End Sub

    Protected Overrides Sub OnMouseWheel(e As MouseEventArgs)

        If Me.ActiveScroll Then MyBase.OnMouseWheel(e)
    End Sub
    '  If Me.ActiveScroll Then MyBase.OnScroll(se)


    Public Property ActiveScroll As Boolean = True Implements IDevice.ActiveScroll



    Public Function ClientRect() As Geometry.Rect Implements IDevice.ClientRect
        Dim rct = Me.ClientRectangle
        Dim x = rct.X
        Dim y = rct.Y
        Dim x1 = rct.X + x
        Dim y1 = rct.Y + y
        Return New Geometry.Rect(x, y, x1, y1)
    End Function

    Public Function GetHDC() As IntPtr Implements IDevice.GetHDC
        Return NativeFunction.GetDC(Me.Handle)
    End Function

    Public Function GetHWND() As IntPtr Implements IDevice.GetHWND
        Return Me.Handle
    End Function

    Public Sub Invalidate1() Implements IDevice.Invalidate
        Me.Invalidate()
    End Sub

    Public Sub Invalidate1(rect As Geometry.Rect) Implements IDevice.Invalidate
        Me.Invalidate(Rectangle.FromLTRB(rect.Left, rect.Top, rect.Right, rect.Bottom))
    End Sub

    Public Event MouseDown1(e As MouseEvntArg) Implements IDevice.MouseDown

    Public Event MouseMove1(e As MouseEvntArg) Implements IDevice.MouseMove

    Public Event MouseUp1(e As MouseEvntArg) Implements IDevice.MouseUp

    Public Event Paint1(e As PaintEvntArg) Implements IDevice.Paint

    Public Property ScrollMinSize As Geometry.Size Implements IDevice.ScrollMinSize
        Get
            Dim value = Me.AutoScrollMinSize
            Return New Geometry.Size(value.Width, value.Height)
        End Get
        Set(value As Geometry.Size)
            Me.AutoScrollMinSize = New Size(value.Width, value.Height)
        End Set
    End Property

    Public Property ScrollPos As Geometry.Point Implements IDevice.ScrollPos
        Get
            Dim value = Me.AutoScrollPosition
            Return New Geometry.Point(value.X, value.Y)
        End Get
        Set(value As Geometry.Point)
            Me.AutoScrollPosition = New Point(value.X, value.Y)
        End Set
    End Property

    Public Sub setCurser(id As Integer) Implements IDevice.setCurser

    End Sub

    Public Sub setScrollMinsizePos1(location As Geometry.Point, size As Geometry.Size) Implements IDevice.setScrollMinsizePos
        Me.setScrollminsizepos(New Point(location.X, location.Y), New Size(size.Width, size.Height))
    End Sub

    Public Property size1 As Geometry.Size Implements IDevice.size
        Get
            Return New Geometry.Size(Me.Size.Width, Me.Size.Height)
        End Get
        Set(value As Geometry.Size)
            Throw New NotImplementedException
        End Set
    End Property

    Private Sub advancedPanel_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
        Dim ea As New Core.MouseEvntArg(e.Button, e.Clicks, e.X, e.Y, e.Delta)
        RaiseEvent MouseMove1(ea)
    End Sub

    Private Sub advancedPanel_MouseUp(sender As Object, e As MouseEventArgs) Handles Me.MouseUp
        Dim ea As New Core.MouseEvntArg(e.Button, e.Clicks, e.X, e.Y, e.Delta)
        RaiseEvent MouseUp1(ea)
    End Sub
End Class
