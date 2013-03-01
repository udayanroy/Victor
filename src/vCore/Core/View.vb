Imports System.Windows.Forms
Imports System.Drawing
Imports System.Drawing.Drawing2D

Public Class View
    Private mem As DOM
    Private device As advancedPanel
    Private zom As Single = 1.0


    Dim canvas_size As New Size(2000, 2000)
    Dim page_size As Size
    Dim page_location As New Point
    Dim Canvas_rect, page_rect As Rectangle





    Friend Sub New(ByRef Dom As DOM, ByRef dc As advancedPanel, ByVal pageSize As Size)
        page_size = pageSize
        mem = Dom
        device = dc
        Initialize_View()  'initialize view
        StartView()


    End Sub
    Public Sub Refresh()
        device.Invalidate()
    End Sub
    Public ReadOnly Property Memory() As DOM
        Get
            Return mem
        End Get
    End Property
    Public Sub paint(ByVal g As Graphics)

        Dim gc As GraphicsContainer
        gc = g.BeginContainer

        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        '///////set position & zoom/////
        g.TranslateTransform(device.AutoScrollPosition.X, device.AutoScrollPosition.Y)
        'g.TranslateTransform(-750, -750)

        g.ScaleTransform(zom, zom)
        '//////////Draw////////////////////
        '   draw page
        g.FillRectangle(Brushes.White, page_rect)
        ' draw DOM

        mem.Draw(g, page_location)

        '/////////////////////////////////
        g.EndContainer(gc)
    End Sub
    Public Property Zoom() As Integer
        Get
            Return zom * 100  'to parcentage
        End Get
        Set(ByVal value As Integer)
            If value > 5 Then
                'from parsentage 
                'setZoom(value)
                'device.AutoScrollMinSize = New Size(viewportRect.Width * zom, viewportRect.Size.Height * zom)
            End If
            ' Me.Refress()
        End Set
    End Property
    Public Sub setZoom(ByVal z As Integer, ByVal p As Point)
        Dim mp = DCpointToMemory(p)
        ' Dim pz As Single = zom
        zom = z / 100
        'Dim az As Single = zom
        Dim x = mp.X * zom - p.X
        Dim y = mp.Y * zom - p.Y
        device.setScrollminsizepos(New Point(x, y), Me.getCanvasrct.Size.ToSize)

        device.Invalidate()

    End Sub
    Public Sub panmove(ByVal m As Point)
        device.AutoScrollPosition = New Point(-device.AutoScrollPosition.X + m.X, _
                                             -device.AutoScrollPosition.Y + m.Y)
    End Sub
    Public Function DCpointToMemory(ByVal p As PointF) As PointF
        Dim kx As Double = p.X / zom
        Dim ky As Double = p.Y / zom
        Return New PointF(kx - (device.AutoScrollPosition.X / zom), ky - (device.AutoScrollPosition.Y / zom))
    End Function

    Public Function MemorypointToDC(ByVal p As PointF) As PointF
        Return New PointF()
    End Function
    

    

    Private Sub Initialize_View()
        '// Initialize View

        '// check if page size is not greater than canvas size
        If canvas_size.Width <= page_size.Width _
         Or canvas_size.Height <= page_size.Height Then _
         Throw New Exception("page size can't greater than Canvas size")
        '// Calculate page location 
        page_location.X = (canvas_size.Width - page_size.Width) / 2
        page_location.Y = (canvas_size.Height - page_size.Height) / 2
        '// create canvas Rect
        Canvas_rect = New Rectangle(New Point(0, 0), canvas_size)
        '// Create page rect
        page_rect = New Rectangle(page_location, page_size)
        '// set minimum Scroll Rect
        device.AutoScrollMinSize = New System.Drawing.Size(2000, 2000)
        

    End Sub
    Private Sub StartView()
        Dim viwrct As Rectangle = Me.viewport_rect
        Dim pageloc As New Point()
        pageloc.X = (viwrct.Width - page_size.Width) / 2
        pageloc.Y = (viwrct.Height - page_size.Height) / 2

        device.AutoScrollPosition = New Point(page_location.X - pageloc.X, _
                                         page_location.Y - pageloc.Y)

    End Sub
    Public ReadOnly Property viewport_rect() As Rectangle
        Get
            Dim loc As New Point(-device.AutoScrollPosition.X, -device.AutoScrollPosition.Y)
            Return New Rectangle(loc, device.Size)
        End Get
    End Property
    Private ReadOnly Property getCanvasrct() As RectangleF
        Get
            Return New RectangleF(0, 0, Canvas_rect.Width * zom, _
                                  Canvas_rect.Height * zom)

        End Get
    End Property
    Private ReadOnly Property getpagerct() As RectangleF
        Get
            Return New RectangleF(page_rect.X * zom, _
                                  page_rect.X * zom, page_rect.Width * zom, _
                                  page_rect.Height * zom)

        End Get
    End Property
    Friend ReadOnly Property getpagerctOrg() As RectangleF
        Get
            Return page_rect

        End Get
    End Property
    Friend ReadOnly Property zoomFactor() As Single
        Get
            Return zom
        End Get
    End Property
    Friend ReadOnly Property postionFactor() As Point
        Get
            Return New Point(device.AutoScrollPosition.X, device.AutoScrollPosition.Y)
        End Get
    End Property


    Public Class BufferPaint
        Friend Sub New()

        End Sub
        Public Sub Initialise()

        End Sub
        Public Sub Render()

        End Sub
    End Class
End Class
