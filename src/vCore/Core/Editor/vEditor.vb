Imports System.Drawing
Imports System.Drawing.Drawing2D
Public Enum selectionType
    MSR
    Pathedt
End Enum
Public Structure memLoc
    Dim layer As Integer
    Dim obj As Integer
    Public Sub create(ByVal l As Integer, ByVal o As Integer)
        layer = l
        obj = o
    End Sub
End Structure

Public Class Selection

    Dim memloc As memLoc
    Dim emty As Boolean
    Public Sub New()
        emty = True
    End Sub
    Public Property isEmty() As Boolean
        Get
            Return emty
        End Get
        Set(ByVal value As Boolean)
            emty = value
        End Set
    End Property


    Public Property MemoryLocation() As memLoc
        Get
            Return memloc
        End Get
        Set(ByVal value As memLoc)
            memloc = value
        End Set
    End Property
End Class
Public Class vEditor


    Dim vcor As vCore
    Dim slct As Selection


    Dim msr As New Msr


    Public Sub New(ByRef v As vCore)
        vcor = v
        slct = New Selection
    End Sub
    Public Sub SelectAt(ByVal p As Point)


        Dim memloc As memLoc
        Dim flage As Boolean = False

        Dim m As PointF = vcor.View.DCpointToMemory(p)
        Dim mp As New PointF(m.X - vcor.View.getpagerctOrg.X, m.Y - vcor.View.getpagerctOrg.Y)


        Dim len, lobj As Integer
        len = vcor.View.Memory.Layers.Count

        For l As Integer = 0 To len - 1
            lobj = vcor.View.Memory.Layers(l).Item.Count
            For k As Integer = 0 To lobj - 1
                If vcor.View.Memory.Layers(l).Item(k).HitTest(mp) Then
                    memloc.create(l, k)

                    flage = True
                End If
            Next
        Next
        If flage = True Then
            slct.MemoryLocation = memloc
            slct.isEmty = False
        Else
            slct.isEmty = True
        End If

        Refresh()


    End Sub


    Public Sub DisSelect()

    End Sub
    Public Sub Refresh()
        vcor.View.Refresh()
    End Sub
    Public Sub StartDrag(ByVal p As Point)

    End Sub
    Public Sub DragTo(ByVal p As Point)

    End Sub
    Public Sub EndDrag(ByVal p As Point)

    End Sub
    Public Property SelectionType() As selectionType
        Get

        End Get
        Set(ByVal value As selectionType)

        End Set
    End Property

    Public Sub paint(ByVal g As Graphics)
        If slct.isEmty = False Then


            Dim mat As New Matrix
            Dim pth As New GraphicsPath


            Dim rf As RectangleF = getBoundRect()
            pth.AddRectangle(rf)


            mat.Translate(vcor.View.postionFactor.X, vcor.View.postionFactor.Y)
            mat.Scale(vcor.View.zoomFactor, vcor.View.zoomFactor)
            mat.Translate(vcor.View.getpagerctOrg.X, vcor.View.getpagerctOrg.Y)


            pth.Transform(mat)

            Using p As New Pen(Color.Red)

                g.DrawPath(p, pth)

            End Using
        End If

    End Sub
    Public Function getBoundRect() As RectangleF
        Return vcor.mem.Layers(slct.MemoryLocation.layer).Item(slct.MemoryLocation.obj).GetBound
    End Function

End Class

