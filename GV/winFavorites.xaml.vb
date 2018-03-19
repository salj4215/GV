Public Class winFavorites

    Dim favChannels(8) As TextBlock
    Dim favChannelPreview(8) As Image
    Dim favChannelLive(8) As Label
    Dim isLive(8) As Boolean
    Dim favChannelTitle(8) As TextBlock

    Private Sub winFavorites_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Dim inFile As IO.StreamReader
        Dim streamerName As String
        Dim intFav As Integer
        Dim picFavPreview As String = " "

        favChannels = {lblFavChannel1, lblFavChannel2, lblFavChannel3, lblFavChannel4, lblFavChannel5, lblFavChannel6, lblFavChannel7, lblFavChannel8}
        favChannelPreview = {picFav1, picFav2, picFav3, picFav4, picFav5, picFav6, picFav7, picFav8}
        favChannelLive = {lblLive1, lblLive2, lblLive3, lblLive4, lblLive5, lblLive6, lblLive7, lblLive8}
        favChannelTitle = {lblLiveTitle1, lblLiveTitle2, lblLiveTitle3, lblLiveTitle4, lblLiveTitle5, lblLiveTitle6, lblLiveTitle7, lblLiveTitle8}

        If IO.File.Exists("GVFavoriteChannels.txt") = False Then
            MessageBox.Show("You do not have any favorite channels.", "GV", MessageBoxButton.OK, MessageBoxImage.Information)
            Exit Sub
        End If

        inFile = IO.File.OpenText("GVFavoriteChannels.txt")

        Do Until inFile.Peek = -1 Or intFav > 7
            streamerName = inFile.ReadLine
            FavoriteChannel(intFav).streamerName = streamerName
            displayChannel(FavoriteChannel(intFav).streamerName, picFavPreview, isLive(intFav), favChannelTitle(intFav))
            favChannelPreview(intFav).Source = New BitmapImage(New Uri(picFavPreview))
            favChannels(intFav).Text = FavoriteChannel(intFav).streamerName
            If isLive(intFav) = False Then
                favChannelLive(intFav).Visibility = Windows.Visibility.Collapsed
            End If
            intFav = intFav + 1

        Loop


    End Sub

    Private Sub displayChannel(ByVal name As String, ByRef previewPicURL As String, ByRef live As Boolean, ByRef liveTitle As TextBlock)
        Dim streamURL As String
        Dim previewTwitchStream As TwitchStream
        Dim jsonResponseURL As String
        Dim streamChannel As String
        Dim streamerName As String
        Dim streamingGame As String

        streamerName = name.ToLower
        streamURL = "https://api.twitch.tv/kraken/streams/" & streamerName

        previewTwitchStream = New TwitchStream(streamURL)
        jsonResponseURL = previewTwitchStream.JsonResponseUrl

        Dim Parsed = Newtonsoft.Json.Linq.JObject.Parse(jsonResponseURL)

        If Parsed("stream").ToString = vbNullString Then
            streamURL = "https://api.twitch.tv/kraken/channels/" & streamerName
            previewTwitchStream = New TwitchStream(streamURL)
            jsonResponseURL = previewTwitchStream.JsonResponseUrl

            Parsed = Newtonsoft.Json.Linq.JObject.Parse(jsonResponseURL)

            previewPicURL = Parsed("video_banner").ToString

            If previewPicURL = "" Then
                previewPicURL = Parsed("profile_banner").ToString
            End If

        Else

            previewPicURL = Parsed("stream")("preview")("medium").ToString
            streamChannel = Parsed("stream")("channel")("name").ToString
            streamingGame = Parsed("stream")("game").ToString
            liveTitle.Text = Parsed("stream")("channel")("status").ToString
            live = True
        End If



    End Sub

    Private Sub btnPreviewFirstFav_Click(sender As Object, e As RoutedEventArgs) Handles btnPreviewFirstFav.Click

        Dim homeWindow As MainWindow = Application.Current.Windows(0)
        homeWindow.txtStreamer.Text = favChannels(0).Text
        homeWindow.btnPreview.RaiseEvent(New RoutedEventArgs(Button.ClickEvent))
        Me.Close()

    End Sub

    Private Sub btnPreviewSecondFav_Click(sender As Object, e As RoutedEventArgs) Handles btnPreviewSecondFav.Click

        Dim homeWindow As MainWindow = Application.Current.Windows(0)
        homeWindow.txtStreamer.Text = favChannels(1).Text
        homeWindow.btnPreview.RaiseEvent(New RoutedEventArgs(Button.ClickEvent))
        Me.Close()

    End Sub

    Private Sub btnPreviewThirdFav_Click(sender As Object, e As RoutedEventArgs) Handles btnPreviewThirdFav.Click

        Dim homeWindow As MainWindow = Application.Current.Windows(0)
        homeWindow.txtStreamer.Text = favChannels(2).Text
        homeWindow.btnPreview.RaiseEvent(New RoutedEventArgs(Button.ClickEvent))
        Me.Close()

    End Sub

    Private Sub btnPreviewFourthFav_Click(sender As Object, e As RoutedEventArgs) Handles btnPreviewFourthFav.Click
        Dim homeWindow As MainWindow = Application.Current.Windows(0)
        homeWindow.txtStreamer.Text = favChannels(3).Text
        homeWindow.btnPreview.RaiseEvent(New RoutedEventArgs(Button.ClickEvent))
        Me.Close()
    End Sub

    Private Sub btnPreviewFifthFav_Click(sender As Object, e As RoutedEventArgs) Handles btnPreviewFifthFav.Click
        Dim homeWindow As MainWindow = Application.Current.Windows(0)
        homeWindow.txtStreamer.Text = favChannels(4).Text
        homeWindow.btnPreview.RaiseEvent(New RoutedEventArgs(Button.ClickEvent))
        Me.Close()
    End Sub

    Private Sub btnPreviewSixthFav_Click(sender As Object, e As RoutedEventArgs) Handles btnPreviewSixthFav.Click
        Dim homeWindow As MainWindow = Application.Current.Windows(0)
        homeWindow.txtStreamer.Text = favChannels(5).Text
        homeWindow.btnPreview.RaiseEvent(New RoutedEventArgs(Button.ClickEvent))
        Me.Close()
    End Sub

    Private Sub btnPreviewSeventhFav_Click(sender As Object, e As RoutedEventArgs) Handles btnPreviewSeventhFav.Click
        Dim homeWindow As MainWindow = Application.Current.Windows(0)
        homeWindow.txtStreamer.Text = favChannels(6).Text
        homeWindow.btnPreview.RaiseEvent(New RoutedEventArgs(Button.ClickEvent))
        Me.Close()
    End Sub

    Private Sub btnPreviewEighthFav_Click(sender As Object, e As RoutedEventArgs) Handles btnPreviewEigthFav.Click
        Dim homeWindow As MainWindow = Application.Current.Windows(0)
        homeWindow.txtStreamer.Text = favChannels(7).Text
        homeWindow.btnPreview.RaiseEvent(New RoutedEventArgs(Button.ClickEvent))
        Me.Close()
    End Sub


End Class
