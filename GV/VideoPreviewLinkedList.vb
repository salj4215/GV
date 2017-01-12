Public Class VideoPreviewLinkedList
    Private picVideoPreviews As Image
    Private picVideoPreviewsNext As VideoPreviewLinkedList

    Public Sub New(ByVal pic As Image)
        picVideoPreviews = pic
    End Sub

    Public Property NextItem() As VideoPreviewLinkedList
        Get
            Return picVideoPreviewsNext
        End Get
        Set(value As VideoPreviewLinkedList)
            picVideoPreviewsNext = value
        End Set
    End Property

    Public ReadOnly Property getPicVideoPreview() As Image
        Get
            Return picVideoPreviews
        End Get
    End Property
End Class
