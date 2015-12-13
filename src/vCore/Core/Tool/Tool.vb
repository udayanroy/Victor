Imports System.Windows.Forms

Public Interface Itool

    ReadOnly Property Device() As IDevice
    Sub DeSelectTool()
    Sub SelectTool(ByRef d As IDevice)
End Interface


Public Class Tools

    Dim vcor As vCore
    Dim itl As Itool

    Dim pantool As tPanTool
    Dim zoomtool As tZoomTool
    Dim seltool As tsel
    Dim ellipsetool As EllipseTool
    Dim moveTool As tMoveTool
    Dim sizeTool As tSizeTool
    Dim rotateTool As RotateTool
    Dim pathptTool As NodeEditTool
    Dim ptconvTool As PointerConverterTool
    Dim penTool As PenTool
    Dim PointerRemoveTool As PointerRemoveTool
    Dim AddPointerTool As AddPointerTool
    Dim RectangleTool As RectangleTool
    Dim LineTool As LineTool
    Dim TransformTool As TransformTool
    Dim SelectionTool As SelectionTool
    Dim ResizeTool As ResizeTool
    Dim ShearTool As ShearTool

    Public Sub New(ByRef vc As vCore)
        vcor = vc

        pantool = New tPanTool(vcor.View)
        zoomtool = New tZoomTool(vcor.View)
        seltool = New tsel(vcor)
        ellipsetool = New EllipseTool(vcor)
        moveTool = New tMoveTool(vcor)
        sizeTool = New tSizeTool(vcor)
        rotateTool = New RotateTool(vcor)
        pathptTool = New NodeEditTool(vcor)
        ptconvTool = New PointerConverterTool(vcor)
        penTool = New PenTool(vcor)
        PointerRemoveTool = New PointerRemoveTool(vcor)
        AddPointerTool = New AddPointerTool(vcor)
        RectangleTool = New RectangleTool(vcor)
        LineTool = New LineTool(vcor)
        TransformTool = New TransformTool(vcor)
        SelectionTool = New SelectionTool(vcor)
        ResizeTool = New ResizeTool(vcor)
        ShearTool = New ShearTool(vcor)

        itl = pantool
        itl.SelectTool(vcor.pDevice)

    End Sub

    Public ReadOnly Property SelectedTool() As Itool
        Get
            Return itl
        End Get
    End Property

    Public Sub SelectTool(ByVal tool As Integer)
        itl.DeSelectTool()
        Select Case tool
            Case 1
                itl = pantool
            Case 2
                itl = zoomtool
            Case 3
                itl = seltool
            Case 4
                itl = ellipsetool
            Case 5
                itl = moveTool
            Case 6
                itl = sizeTool
            Case 7
                itl = rotateTool
            Case 8
                itl = pathptTool
            Case 9
                itl = ptconvTool
            Case 10
                itl = penTool
            Case 11
                itl = PointerRemoveTool
            Case 12
                itl = AddPointerTool
            Case 13
                itl = RectangleTool
            Case 14
                itl = LineTool
            Case 15
                itl = TransformTool
            Case 16
                itl = SelectionTool
            Case 17
                itl = ResizeTool
            Case 18
                itl = ShearTool
        End Select
                itl.SelectTool(vcor.pDevice)
    End Sub
End Class


