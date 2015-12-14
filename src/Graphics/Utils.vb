Imports Geometry
Imports System.Runtime.CompilerServices


Public Module Utils

    <Extension()>
    Public Function Topoint(pt As Drawing.PointF) As Point
        Return New Point(pt.X, pt.Y)
    End Function

    <Extension()>
    Public Function ToPointf(pt As Point) As Drawing.PointF
        Return New Drawing.PointF(pt.X, pt.Y)
    End Function

    <Extension()>
    Public Function ToRectanglef(pt As Rect) As Drawing.RectangleF
        Return Drawing.Rectangle.FromLTRB(pt.Left, pt.Top, pt.Right, pt.Bottom)
    End Function

    <Extension()>
    Friend Function ToGraphicsPath(figure As NodeFigure) As Drawing.Drawing2D.GraphicsPath

        Dim np = figure.CountPoints
        If np < 2 Then
            Return Nothing
            Exit Function
        End If


        Dim lst As New List(Of Drawing.PointF)
        Dim gp As New Drawing.Drawing2D.GraphicsPath

        Dim p0 = figure.Points(0)

        If p0.Type = NodeType.None Then
            lst.Add(p0.M.ToPointf())
            lst.Add(p0.M.ToPointf)
        Else
            lst.Add(p0.M.ToPointf)
            lst.Add(p0.C2.ToPointf)
        End If

        If np > 2 Then
            For i As Integer = 1 To np - 2
                Dim p As Node = figure.Points(i)

                If p.Type = NodeType.None Then
                    lst.Add(p.M.ToPointf)
                    lst.Add(p.M.ToPointf)
                    lst.Add(p.M.ToPointf)
                Else
                    lst.Add(p.C1.ToPointf)
                    lst.Add(p.M.ToPointf)
                    lst.Add(p.C2.ToPointf)
                End If


            Next
        End If


        Dim pl = figure.Points(np - 1)

        If pl.Type = NodeType.None Then
            lst.Add(pl.M.ToPointf)
            lst.Add(pl.M.ToPointf)
        Else
            lst.Add(pl.C1.ToPointf)
            lst.Add(pl.M.ToPointf)
        End If

        If figure.Closed Then
            lst.Add(pl.C2.ToPointf)

            lst.Add(p0.C1.ToPointf)
            lst.Add(p0.M.ToPointf)

            gp.AddBeziers(lst.ToArray)
            gp.CloseFigure()
        Else
            gp.AddBeziers(lst.ToArray)
            gp.SetMarkers()
        End If




        Return (gp)
    End Function


    <Extension()>
    Friend Function ToGraphicsPath(path As NodePath) As Drawing.Drawing2D.GraphicsPath
        Dim gp As New Drawing.Drawing2D.GraphicsPath

        For Each fig As NodeFigure In path.Figures
            Dim tempGP = fig.ToGraphicsPath
            If tempGP IsNot Nothing Then gp.AddPath(tempGP, False)
        Next

        Return (gp)
    End Function

End Module
