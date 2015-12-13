Imports Geometry
Imports Graphics

Public Class ControlVisual
    Implements IDrawable


    Dim _document As Document
    Dim _dc As IDevice

    Dim zoomvisual As ZoomVisual
    Dim marginvisual As MarginVisual
    Dim scrollvisual As ScrollVisual

    Dim bffgraphics As BufferPaint

    Public Sub New(Dom As Document, dc As IDevice)
        _document = Dom
        _dc = dc

        zoomvisual = New ZoomVisual
        zoomvisual.Children = _document
        zoomvisual.ZoomPercentage = 100

        marginvisual = New MarginVisual
        marginvisual.Children = zoomvisual
        marginvisual.Margin = 100

        scrollvisual = New ScrollVisual
        scrollvisual.Children = marginvisual
        scrollvisual.ScrollPos = Device.ScrollPos

        Device.ScrollMinSize = scrollvisual.GetArea.Size

        bffgraphics = New BufferPaint(dc)
    End Sub

    Public ReadOnly Property BufferGraphics As BufferPaint
        Get
            Return Me.bffgraphics
        End Get
    End Property

    Public Sub Draw(canvas As Canvas) Implements IDrawable.Draw
        'canvas.clear(BackGroundColor)  ' Draw control background

        scrollvisual.ScrollPos = Device.ScrollPos
        scrollvisual.Draw(canvas)
    End Sub

    Public Function getArea() As Rect Implements IDrawable.GetArea
        Return scrollvisual.GetArea
    End Function

    Public Sub Refresh()
        Device.Invalidate()
    End Sub

    Public Sub setZoom(ByVal zoomPersentage As UShort, ByVal ScreenLocation As Point)
        Dim scrloc = ScreenLocation
        Me.Screen2memory(ScreenLocation)
        Dim memloc = ScreenLocation

        zoomvisual.ZoomPercentage = zoomPersentage
        scrollvisual.ScrollPos = New Point(0, 0)

        Dim newscrloc = memloc
        Me.Memory2screen(newscrloc)

        Dim vector = (newscrloc - scrloc)

        '   Dim loc = New Point(-Device.ScrollPos.X + vector.X, _
        ' -Device.ScrollPos.Y + vector.Y)
        Device.setScrollMinsizePos(vector, Me.GetArea.Size)

    End Sub

    Public Sub panmove(ByVal m As Point)
        Device.ScrollPos = New Point(-Device.ScrollPos.X + m.X, _
                                             -Device.ScrollPos.Y + m.Y)
    End Sub

#Region "TransFormation"

    Public Sub Memory2screen(ByRef pt As Point)
        Dim mat = Memory2screen()
        mat.map(pt)
    End Sub

    Public Sub Memory2screen(ByRef rct As Rect)
        Dim mat = Memory2screen()
        rct.Transform(mat)
    End Sub

    Public Sub Memory2screen(ByRef pth As NodePath)
        Dim mat = Memory2screen()
        pth.Transform(mat)
    End Sub

    Public Function Memory2screen() As Matrix
        Dim mat As Matrix = zoomvisual.Inner2Outer
        mat.Multiply(marginvisual.Inner2Outer)
        mat.Multiply(scrollvisual.Inner2Outer)
        Return mat
    End Function

    Public Sub Screen2memory(ByRef pt As Point)
        Dim mat = Screen2memory()
        mat.map(pt)
    End Sub

    Public Sub Screen2memory(ByRef rct As Rect)
        Dim mat = Screen2memory()
        rct.Transform(mat)
    End Sub

    Public Sub Screen2memory(ByRef pth As NodePath)
        Dim mat = Screen2memory()
        pth.Transform(mat)
    End Sub

    Public Function Screen2memory() As Matrix
        Dim mat As Matrix = zoomvisual.Inner2Outer
        mat.Multiply(marginvisual.Inner2Outer)
        mat.Multiply(scrollvisual.Inner2Outer)
        mat.Invert()
        Return mat
    End Function

#End Region

    Public ReadOnly Property TennisPanel As Document
        Get
            Return _document
        End Get
    End Property

    Public ReadOnly Property Device As IDevice
        Get
            Return _dc
        End Get
    End Property

    Public Property ZoomPercentage As UShort
        Get
            Return zoomvisual.ZoomPercentage
        End Get
        Set(value As UShort)
            'you can't set this value directly due to minScrollsize
            '    zoomvisual.ZoomPercentage = value
        End Set
    End Property

    Public Property Margin As UShort
        Get
            Return marginvisual.Margin
        End Get
        Set(value As UShort)
            marginvisual.Margin = value
        End Set
    End Property


    
End Class
