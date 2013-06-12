Imports System.Windows.Forms

Public Interface Itool

    ReadOnly Property Device() As advancedPanel
    Sub DeSelectTool()
    Sub SelectTool(ByRef d As advancedPanel)
End Interface


Public Class Tools

    Dim vcor As vCore
    Dim itl As Itool

    Dim pantool As tPanTool
    Dim zoomtool As tZoomTool
    Dim selectiontool As tsel
    Dim ellipsetool As tEllipseTool
    Dim moveTool As tMoveTool
    Dim sizeTool As tSizeTool
    Dim rotateTool As tRotate
    Dim pathptTool As tPathptTool

    Public Sub New(ByRef vc As vCore)
        vcor = vc

        pantool = New tPanTool(vcor.View)
        zoomtool = New tZoomTool(vcor.View)
        selectiontool = New tsel(vcor)
        ellipsetool = New tEllipseTool(vcor.View)
        moveTool = New tMoveTool(vcor)
        sizeTool = New tSizeTool(vcor)
        rotateTool = New tRotate(vcor)
        pathptTool = New tPathptTool(vcor)

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
                itl = selectiontool
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
        End Select
        itl.SelectTool(vcor.pDevice)
    End Sub
End Class


