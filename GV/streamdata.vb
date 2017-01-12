Public Class streamdata

    Public Property token As String
        Get
            Return s_token
        End Get
        Set(value As String)
            s_token = value
        End Set
    End Property
    Private s_token As String

    Public Property Sig As String
        Get
            Return s_sig
        End Get
        Set(value As String)
            s_sig = value
        End Set
    End Property
    Private s_sig As String
End Class
