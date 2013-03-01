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
    Public Sub New(ByRef vc As vCore)
        vcor = vc

        pantool = New tPanTool(vcor.View)
        zoomtool = New tZoomTool(vcor.View)
        selectiontool = New tsel(vcor)

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
        End Select
        itl.SelectTool(vcor.pDevice)
    End Sub
End Class


