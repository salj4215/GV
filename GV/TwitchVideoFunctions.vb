﻿Imports System.Net
Imports System.IO
Imports System
Imports System.Text
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Module TwitchVideoFunctions

    'This function is used to retreive a json object that contains data regarding a stream. If the stream is live, the JSON returns links to the video streams 
    'along with other data such as viewers, title of stream, game being played, etc.
    Public Function RetreiveStreams(ByVal srName As String) As String

        Dim streamURL As String
        Dim jsonResponseURL As String
        Dim liveTwitchStream As TwitchStream 'TwitchStream class is created in a seperate file
        Dim m3uResponse As WebResponse
        Dim m3uResponseFromServer As String

        'Create URL for requesting access token from Twitch
        streamURL = "http://api.twitch.tv/api/channels/" & srName & "/access_token"

        'Create instance of new TwitchStream object
        liveTwitchStream = New TwitchStream(streamURL)

        'Get JSON response Url from TwitchStream object
        jsonResponseURL = liveTwitchStream.JsonResponseUrl

        'Parse JSON using streamdata class
        Dim streamData As streamdata = JsonConvert.DeserializeObject(Of streamdata)(jsonResponseURL)

        'Create Url for requesting m3u file containing twitch streams
        streamURL = "http://usher.twitch.tv/api/channel/hls/" & srName & ".m3u8?player=twitchweb&&token=" & streamData.token & "&sig=" & streamData.Sig &
            "&allow_audio_only=true&allow_source=true&type=any&p={random}'"

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
            MessageBox.Show(srName & " is not live.", "GV", MessageBoxButton.OK, MessageBoxImage.Information)
            m3uResponseFromServer = "false"
        End Try

        Return m3uResponseFromServer
    End Function

    Public Function selectStream(strings() As String, subString As String) As String
        Dim stringIndex As Integer
        stringIndex = Array.IndexOf(strings, subString)
        subString = strings(stringIndex + 1)
        Return subString
    End Function

    Public Function playStream(ByVal m3u As String, quality As String) As String
        Dim vURL As String

        'Split m3u string at every new line and store in substrings()
        Dim substrings() As String = m3u.Split(vbLf)
        For Each substring In substrings
            If quality = "Source" And substring.Contains("http") Then
                vURL = substring
                Exit For
            ElseIf quality = "720p60" And substring.Contains("VIDEO=""720p60""") Then
                vURL = selectStream(substrings, substring)
                Exit For
            ElseIf quality = "720p" And substring.Contains("VIDEO=""720p30""") Then
                vURL = selectStream(substrings, substring)
                Exit For
            ElseIf quality = "480p" And substring.Contains("VIDEO=""480p30""") Then
                vURL = selectStream(substrings, substring)
                Exit For
            ElseIf quality = "360p" And substring.Contains("VIDEO=""360p30""") Then
                vURL = selectStream(substrings, substring)
                Exit For
            ElseIf quality = "160p" And substring.Contains("VIDEO=""160p30""") Then
                vURL = selectStream(substrings, substring)
                Exit For
            Else
                vURL = "null"
            End If
        Next
        Return vURL
    End Function
End Module
