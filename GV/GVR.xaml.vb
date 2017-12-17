Public Class GVR
    Private videoURL As String
    Private streamerName As String
    Private videoStreams As String
    Private vidQuality As String

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs) Handles btnPlayPause.Click


    End Sub

    Private Sub GVR_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        videoStreams = RetreiveStreams(streamerName)
        If videoStreams = "false" Then
            Exit Sub
        End If
        videoURL = playStream(videoStreams, vidQuality)
        gvrPlayer.Source = New Uri(videoURL)
    End Sub

    Public Sub New(sName As String, quality As String)
        streamerName = sName
        vidQuality = quality
    End Sub
End Class
