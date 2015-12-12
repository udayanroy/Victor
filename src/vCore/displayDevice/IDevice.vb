Imports Geometry
Imports Graphics

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

    Event Paint(e As PaintEvntArg)
    Event MouseDown(e As MouseEvntArg)
    Event MouseUp(e As MouseEvntArg)
    Event MouseMove(e As MouseEvntArg)
End Interface


Public Class PaintEvntArg

    Private _area As Rect
    Private _canvas As Canvas

    Public Sub New(canvas As Canvas, InvalidArea As Rect)
        _canvas = canvas
        _area = InvalidArea
    End Sub

    Public ReadOnly Property canvas As Canvas
        Get
            Return _canvas
        End Get
    End Property
    Public ReadOnly Property PaintArea() As Rect
        Get
            Return _area
        End Get
    End Property
End Class

Public Class MouseEvntArg

    Private _location As Point
    Private _delta As Integer
    Private _clicks As Integer
    Private _button As MouseButton

    Public Sub New(button As MouseButton, clicks As Integer,
                   x As Integer, y As Integer, delta As Integer)
        _location = New Point(x, y)
        _delta = delta
        _clicks = clicks
        _button = button
    End Sub

    Public ReadOnly Property Button As MouseButton
        Get
            Return _button
        End Get
    End Property
    Public ReadOnly Property Clicks As Integer
        Get
            Return _clicks
        End Get
    End Property
    Public ReadOnly Property Delta As Integer
        Get
            Return _delta
        End Get
    End Property
    Public ReadOnly Property Location As Point
        Get
            Return _location
        End Get
    End Property
End Class

Public Enum MouseButton As Integer
    Left = 1048576
    Right = 2097152
    Middle = 4194304
    None = 0
End Enum