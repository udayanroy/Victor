Imports System.Drawing
Imports System.Drawing.Drawing2D

Public Interface Iedtr
    Sub Draw(ByRef g As Graphics)
    Sub mouse_Down(ByRef e As System.Windows.Forms.MouseEventArgs)
    Sub mouse_Move(ByRef e As System.Windows.Forms.MouseEventArgs)
    Sub mouse_Up(ByRef e As System.Windows.Forms.MouseEventArgs)
End Interface
