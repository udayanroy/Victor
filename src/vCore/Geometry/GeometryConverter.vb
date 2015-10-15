Imports System.Drawing
Imports Geom.Geometry

Public Class GeometryConverter


    Public Shared Function Pointf2Gpoint(pt As PointF) As GPoint
        Return New GPoint(pt.X, pt.Y)
    End Function

    Public Shared Function Gpoint2Pointf(pt As GPoint) As PointF
        Return New PointF(pt.X, pt.Y)
    End Function
End Class
