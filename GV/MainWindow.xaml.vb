Imports System.Net
Imports System.IO
Imports System
Imports System.Text
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Class MainWindow

    'Declare variables for URl of stream and game streaming
    Private videoURL As String
    Private streamingGame As String

    Private Sub btnConnect_Click(sender As Object, e As EventArgs) Handles btnConnect.Click

        Dim streamerName As String
        Dim streamURL As String
        Dim jsonResponseURL As String
        Dim liveTwitchStream As TwitchStream 'TwitchStream class is created in a seperate file
        Dim m3uResponse As WebResponse
        Dim m3uResponseFromServer As String

        'set streamerName to streamer's name entered by user in txtStreamer textbox and set all letters to lower case
        streamerName = txtStreamer.Text.ToLower

        'Create URL for requesting access token from Twitch
        streamURL = "http://api.twitch.tv/api/channels/" & streamerName & "/access_token"

        'Create instance of new TwitchStream object
        liveTwitchStream = New TwitchStream(streamURL)

        'Get JSON response Url from TwitchStream object
        jsonResponseURL = liveTwitchStream.JsonResponseUrl

        'Parse JSON using streamdata class
        Dim streamData As streamdata = JsonConvert.DeserializeObject(Of streamdata)(jsonResponseURL)

        'Create Url for requesting m3u file containing twitch streams
        streamURL = "http://usher.twitch.tv/api/channel/hls/" & streamerName & ".m3u8?player=twitchweb&&token=" & streamData.token & "&sig=" & streamData.Sig & "&allow_audio_only=true&allow_source=true&type=any&p={random}'"

        'Request m3u file
        Dim m3uRequest As WebRequest = WebRequest.Create(streamURL)

        'Catch exception if streamer is not live
        Try
            m3uResponse = m3uRequest.GetResponse
            Console.WriteLine(CType(m3uResponse, HttpWebResponse).StatusDescription)
            Dim dataStream As Stream = m3uResponse.GetResponseStream()

            'Open the stream using a StreamReader for easy access
            Dim reader As New StreamReader(dataStream)

            'Read the content
            m3uResponseFromServer = reader.ReadToEnd

            'Display the content
            Console.WriteLine(m3uResponseFromServer)

            'Clean up the streams and the response
            reader.Close()
            m3uResponse.Close()
        Catch ex As Exception

            'Show message if streamer is not live and exit
            MessageBox.Show(txtStreamer.Text & " is not live.", "GV", MessageBoxButton.OK, MessageBoxImage.Information)
            Exit Sub
        End Try

        'Send m3u to PlayStream sub
        playStream(m3uResponseFromServer)
    End Sub

    Private Sub playStream(ByVal m3u As String)

        'Play Stream

        'Split m3u string at every new line and store in substrings()
        Dim substrings() As String = m3u.Split(vbLf)
        For Each substring In substrings
            If cboQuality.Text = "Source" Then
                If substring.Contains("http") Then 'first line is a link to source quality stream
                    videoURL = substring
                    startPlayer()
                    Exit For
                End If
            End If

            If cboQuality.Text = "High" Then
                If substring.Contains("http") And substring.Contains("high") Then
                    videoURL = substring
                    startPlayer()
                    Exit For
                End If
            End If

            If cboQuality.Text = "Medium" Then
                If substring.Contains("http") And substring.Contains("medium") Then
                    videoURL = substring
                    startPlayer()
                    Exit For
                End If
            End If

            If cboQuality.Text = "Low" Then
                If substring.Contains("http") And substring.Contains("low") Then
                    videoURL = substring
                    startPlayer()
                    Exit For
                End If
            End If

            If cboQuality.Text = "Mobile" Then
                If substring.Contains("http") And substring.Contains("mobile") Then
                    videoURL = substring
                    startPlayer()
                    Exit For
                End If
            End If
        Next
    End Sub

    Private Sub startPlayer()
        If tsmVLC.IsChecked Then
            Try
                System.Diagnostics.Process.Start("C:\Program Files (x86)\VideoLAN\VLC\vlc.exe", videoURL)
            Catch ex As Exception
                MessageBox.Show("You do not have VLC installed. Your default browser will be used", "GV", MessageBoxButton.OK, MessageBoxImage.Information)
                System.Diagnostics.Process.Start(videoURL)
            End Try
        Else
            System.Diagnostics.Process.Start(videoURL)
        End If
    End Sub

    Private Sub GV_WinClosed(sender As Object, e As EventArgs) Handles Me.Closing
        If tsmVLC.IsChecked Then
            My.Settings.VlcUse = tsmVLC.IsChecked
        Else
            My.Settings.VlcUse = tsmVLC.IsChecked = False
        End If

        If tsmChrome.IsChecked Then
            My.Settings.gChromeUse = tsmChrome.IsChecked
        Else
            My.Settings.gChromeUse = tsmChrome.IsChecked = False
        End If

    End Sub

    Private Sub GV_Win_Load(sender As Object, e As EventArgs) Handles Me.Loaded


        cboQuality.Items.Add("Source")
        cboQuality.Items.Add("High")
        cboQuality.Items.Add("Medium")
        cboQuality.Items.Add("Low")
        cboQuality.Items.Add("Mobile")

        cboSearchChoice.Items.Add("Direct Connect")
        cboSearchChoice.Items.Add("Streamers")
        cboSearchChoice.Items.Add("Games")

        'set stream quality to medium (default)
        cboQuality.Text = cboQuality.Items(2)
        cboSearchChoice.Text = cboSearchChoice.Items(0)
        txtStreamer.Focus()
        label1.Visibility = Windows.Visibility.Hidden
        cboQuality.Visibility = Windows.Visibility.Hidden
        btnConnect.Visibility = Windows.Visibility.Hidden
        btnChat.Visibility = Windows.Visibility.Hidden
        Label13.Visibility = Windows.Visibility.Hidden
        btnGameSearch.Visibility = Windows.Visibility.Hidden
        btnFavorite.Visibility = Windows.Visibility.Hidden
        lblSearchGame.Visibility = Windows.Visibility.Hidden
        lblSearchStreamer.Visibility = Windows.Visibility.Visible
        btnGVR.Visibility = Windows.Visibility.Hidden



    End Sub

    Private Sub btnPreview_Click(sender As Object, e As EventArgs) Handles btnPreview.Click
        If cboSearchChoice.Text = "Direct Connect" Then
            Dim streamURL As String
            Dim previewTwitchStream As TwitchStream
            Dim jsonResponseURL As String
            Dim previewPicURL As String
            Dim channelLogoURL As String
            Dim streamChannel As String
            Dim streamTitle As String
            Dim viewers As String
            Dim streamerName As String

            If txtStreamer.Text = String.Empty Then
                MessageBox.Show("Please enter a twitch streamer name.", "No Name Entered.", MessageBoxButton.OK, MessageBoxImage.Information)
                txtStreamer.Focus()
                Exit Sub
            End If

            streamerName = txtStreamer.Text.ToLower
            streamURL = "https://api.twitch.tv/kraken/streams/" & streamerName
            Try
                previewTwitchStream = New TwitchStream(streamURL)
                jsonResponseURL = previewTwitchStream.JsonResponseUrl

                Dim Parsed = Newtonsoft.Json.Linq.JObject.Parse(jsonResponseURL)

                previewPicURL = Parsed("stream")("preview")("large").ToString
                channelLogoURL = Parsed("stream")("channel")("logo").ToString
                streamChannel = Parsed("stream")("channel")("name").ToString
                streamingGame = Parsed("stream")("game").ToString
                streamTitle = Parsed("stream")("channel")("status").ToString
                viewers = Parsed("stream")("viewers").ToString

            Catch ex As Exception
                Try
                    streamURL = "https://api.twitch.tv/kraken/channels/" & streamerName
                    previewTwitchStream = New TwitchStream(streamURL)
                    jsonResponseURL = previewTwitchStream.JsonResponseUrl
                    MessageBox.Show(txtStreamer.Text & " is offline.", "Channel Offline", MessageBoxButton.OK, MessageBoxImage.Information)
                    Console.WriteLine(jsonResponseURL)

                    Dim Parsed = Newtonsoft.Json.Linq.JObject.Parse(jsonResponseURL)

                    previewPicURL = Parsed("video_banner").ToString
                    picStreamPreview.Source = New BitmapImage(New Uri(previewPicURL))
                Catch exDNE As Exception
                    MessageBox.Show("Streamer name not found.", "Live Gaming Player", MessageBoxButton.OK, MessageBoxImage.Information)
                End Try
                label1.Visibility = Windows.Visibility.Hidden
                cboQuality.Visibility = Windows.Visibility.Hidden
                btnConnect.Visibility = Windows.Visibility.Hidden
                btnChat.Visibility = Windows.Visibility.Hidden
                picLogo.Visibility = Windows.Visibility.Hidden
                lblStreamStatus.Visibility = Windows.Visibility.Hidden
                lblStreamTitle.Visibility = Windows.Visibility.Hidden
                lblViewers.Visibility = Windows.Visibility.Hidden
                btnWeb.Visibility = Windows.Visibility.Visible
                btnGameSearch.Visibility = Windows.Visibility.Hidden
                btnFavorite.Visibility = Windows.Visibility.Hidden
                btnGVR.Visibility = Windows.Visibility.Hidden

                Exit Sub

            End Try
            label1.Visibility = Windows.Visibility.Visible
            cboQuality.Visibility = Windows.Visibility.Visible
            btnConnect.Visibility = Windows.Visibility.Visible
            btnChat.Visibility = Windows.Visibility.Visible
            picLogo.Visibility = Windows.Visibility.Visible
            lblStreamStatus.Visibility = Windows.Visibility.Visible
            lblStreamTitle.Visibility = Windows.Visibility.Visible
            lblViewers.Visibility = Windows.Visibility.Visible
            btnWeb.Visibility = Windows.Visibility.Visible
            btnGameSearch.Visibility = Windows.Visibility.Visible
            picStreamPreview.Source = New BitmapImage(New Uri(previewPicURL))
            picLogo.Source = New BitmapImage(New Uri(channelLogoURL))
            lblStreamTitle.Text = streamChannel.ToUpper & " " & "playing " & streamingGame
            lblStreamStatus.Text = streamTitle
            lblViewers.Content = viewers
            Label13.Visibility = Windows.Visibility.Visible
            btnFavorite.Visibility = Windows.Visibility.Visible
            btnGVR.Visibility = Windows.Visibility.Visible
        End If


    End Sub

    Private Sub btnWeb_Click(sender As Object, e As EventArgs) Handles btnWeb.Click

        Dim streamerName = txtStreamer.Text.ToLower
        Process.Start("www.twitch.tv/" & streamerName)

    End Sub

    Private Sub btnChat_Click(sender As Object, e As EventArgs) Handles btnChat.Click

        Dim streamerName As String = txtStreamer.Text.ToLower
        If streamerName = String.Empty Then
            MessageBox.Show("Please enter streamer's name into textbox", "GV", MessageBoxButton.OK, MessageBoxImage.Information)
            Exit Sub
        End If
        Dim psInfo As Object
        Dim chatLink As String = "http://www.twitch.tv/" & streamerName & "/chat?popout="

        If tsmChrome.IsChecked Then
            Try
                psInfo = New System.Diagnostics.ProcessStartInfo("C:\Program Files (x86)\Google\Chrome\Application\chrome.exe", chatLink)
                psInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal
            Catch exNoChrome As Exception
                MessageBox.Show("Chrome is not installed or not found in the Program Files (x86) folder. Now opening chat with default browser", "Live Gaming Player", MessageBoxButton.OK, MessageBoxImage.Information)
                psInfo = New System.Diagnostics.ProcessStartInfo(chatLink)
                psInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal
            End Try
        Else
            psInfo = New System.Diagnostics.ProcessStartInfo(chatLink)
            psInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal
        End If

        Dim myProcess As Process = System.Diagnostics.Process.Start(psInfo)

    End Sub

    Private Sub btnGameSearch_Click(sender As Object, e As EventArgs) Handles btnGameSearch.Click
        Process.Start("https://www.google.com/?gws_rd=ssl#q=" & streamingGame)
    End Sub

    Private Sub mbtnFavorite_Click(sender As Object, e As EventArgs) Handles mbtnFavorite.Click

        Dim winFavorites As New winFavorites
        winFavorites.Show()
        winFavorites.Topmost = True

    End Sub

    Private Sub btnFavorite_Click(sender As Object, e As RoutedEventArgs) Handles btnFavorite.Click

        Dim outFile As IO.StreamWriter
        Dim strOutput As String

        If IO.File.Exists("GVFavoriteChannels.txt") = False Then
            outFile = IO.File.CreateText("GVFavoriteChannels.txt")
        Else
            outFile = IO.File.AppendText("GVFavoriteChannels.txt")
        End If

        strOutput = txtStreamer.Text.ToLower
        outFile.WriteLine(strOutput)
        outFile.Close()


    End Sub

    Private Sub cboSearchChoice_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cboSearchChoice.SelectionChanged
        If cboSearchChoice.Text = "Streamers" Then
            lblSearchStreamer.Visibility = Windows.Visibility.Collapsed
            lblSearchGame.Visibility = Windows.Visibility.Visible
        ElseIf cboSearchChoice.Text = "Games" Then
            lblSearchGame.Visibility = Windows.Visibility.Collapsed
            lblSearchStreamer.Visibility = Windows.Visibility.Visible
        End If
    End Sub
End Class
