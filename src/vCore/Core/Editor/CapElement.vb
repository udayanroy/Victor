

Public MustInherit Class CapElement

    Private _editor As Editor

    Public Sub New(editor As Editor)
        Me._editor = editor
    End Sub

    Protected ReadOnly Property Editor As Editor
        Get
            Return _editor
        End Get
    End Property

    Protected ReadOnly Property Visual As ControlVisual
        Get
            Return Me.Editor.View
        End Get
    End Property

    Protected ReadOnly Property Dom As Document
        Get
            Return Me.Editor.View.DOM
        End Get
    End Property

    Protected ReadOnly Property SelectionHolder As ISelectionHolder
        Get
            Return Me.Editor.SelectionHolder
        End Get
    End Property

End Class
