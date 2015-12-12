Imports Geometry

Public Interface IDevice

    Sub Invalidate()
    Sub Invalidate(ByVal rect As Rect)
    Sub setScrollMinsizePos(ByVal location As Point, ByVal size As Size)
    Function GetHDC() As IntPtr
    Function GetHWND() As IntPtr
    Function ClientRect() As Rect

    Property ScrollPos As Point
    Property ScrollMinSize As Size
    Property size As Size
    Property WheelScrollEnable As Boolean
    Sub setCurser(id As Integer)

    Event Paint()
    Event MouseDown()
    Event MouseUp()
    Event MouseMove()
End Interface
