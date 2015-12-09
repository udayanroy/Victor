Imports System.Drawing
Imports Geom.Geometry

Public Class GeometryConverter


    Public Shared Function Pointf2Gpoint(pt As PointF) As GPoint
        Return New GPoint(pt.X, pt.Y)
    End Function

    Public Shared Function Gpoint2Pointf(pt As GPoint) As PointF
        Return New PointF(pt.X, pt.Y)
    End Function

    Public Shared Function Grect2Rectanglef(pt As GRect) As RectangleF
        Return New RectangleF(pt.X, pt.Y, pt.Width, pt.Height)
    End Function
End Class
