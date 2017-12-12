Public Class GVR
    Private videoURL As String
    Private streamerName As String

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs) Handles btnPlayPause.Click


    End Sub

    Private Sub GVR_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        videoURL = RetreiveStreams(streamerName)
        gvrPlayer.Source = New Uri(videoURL)
    End Sub

    Public Sub New(sName As String)
        streamerName = sName
    End Sub
End Class
