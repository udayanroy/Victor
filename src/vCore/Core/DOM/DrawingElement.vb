Imports Graphics
Imports Geometry

Public Interface DrawingElement
    Inherits IDisposable

    Sub Draw(canvas As Canvas)
    Property Brush As Brush
    Property Pen As Pen
    Property Opacity As Single


    Function isVisible(ByVal p As Point) As Boolean
    Function isVisible(ByVal rect As Rect) As Boolean
    Function isBoundVisible(ByVal rect As Rect) As Boolean
    Function isBoundVisible(ByVal p As Point) As Boolean
    Function GetElementType() As ElementType

    'Get loosly claculated ElementBound
    Function GetElementBound() As Rect

    'Transform Function
    Function GetItemBound() As Rect
    Sub Translate(ByVal x As Single, ByVal y As Single)
    Sub Resize(width As Single, height As Single)
    Sub ReArrange(ByVal x As Single, ByVal y As Single, ByVal width As Single, ByVal height As Single)
    Property Rotation As Single
    Function GetSkeliton() As NodePath
    Sub ApplyTransform(mat As Matrix)
    Sub ApplyTransform(Transform As Transform)
End Interface


Public Enum ElementType
    PathElement
    ImageElement
    TextElement
    GroupElement
End Enum
 