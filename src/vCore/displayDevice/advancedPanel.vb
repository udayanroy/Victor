Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports System.Drawing

Public Class advancedPanel
    Inherits ScrollableControl
   

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
End Class
