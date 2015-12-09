




Public MustInherit Class Transform

    Public MustOverride ReadOnly Property Value As Matrix

    Public Sub TransformRect(ByRef rect As Rect)
        rect.Transform(Me.Value)
    End Sub

    Public Sub TransformPoint(ByRef point As Point)
        point.Transformation(Me.Value)
    End Sub

    Public Sub TransformPoints(ByRef points As Point())
        Me.Value.map(points)
    End Sub

End Class



