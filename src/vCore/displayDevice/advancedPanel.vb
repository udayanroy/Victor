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
    End Sub

    Protected Overrides Sub OnMouseWheel(e As MouseEventArgs)

        If Me.ActiveScroll Then MyBase.OnMouseWheel(e)
    End Sub
    '  If Me.ActiveScroll Then MyBase.OnScroll(se)


    Public Property ActiveScroll As Boolean = True

    Public Property ActiveScroll1 As Boolean Implements IDevice.ActiveScroll

    Public Function ClientRect() As Geometry.Rect Implements IDevice.ClientRect

    End Function

    Public Function GetHDC() As IntPtr Implements IDevice.GetHDC

    End Function

    Public Function GetHWND() As IntPtr Implements IDevice.GetHWND

    End Function

    Public Sub Invalidate1() Implements IDevice.Invalidate

    End Sub

    Public Sub Invalidate1(rect As Geometry.Rect) Implements IDevice.Invalidate

    End Sub

    Public Event MouseDown1(e As MouseEvntArg) Implements IDevice.MouseDown

    Public Event MouseMove1(e As MouseEvntArg) Implements IDevice.MouseMove

    Public Event MouseUp1(e As MouseEvntArg) Implements IDevice.MouseUp

    Public Event Paint1(e As PaintEvntArg) Implements IDevice.Paint

    Public Property ScrollMinSize As Geometry.Size Implements IDevice.ScrollMinSize

    Public Property ScrollPos As Geometry.Point Implements IDevice.ScrollPos

    Public Sub setCurser(id As Integer) Implements IDevice.setCurser

    End Sub

    Public Sub setScrollMinsizePos1(location As Geometry.Point, size As Geometry.Size) Implements IDevice.setScrollMinsizePos

    End Sub

    Public Property size1 As Geometry.Size Implements IDevice.size
End Class
