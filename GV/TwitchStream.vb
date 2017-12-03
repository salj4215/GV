Imports System.Net
Imports System.IO
Imports System
Imports System.Text
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class TwitchStream
    Private _strStreamUrl As String
    Private _jsonRequest As WebRequest
    Private _jsonResponse As HttpWebResponse
    Private _jsonStream As Stream
    Private _strJsonResponseUrl As String
    Private _jsonReader As StreamReader

    'Stream URL property
    Public Property StreamUrl As String
        Get
            Return _strStreamUrl
        End Get
        Set(value As String)
            _strStreamUrl = value
        End Set
    End Property

    'Json URL property
    Public ReadOnly Property JsonResponseUrl As String
        Get
            'Request access token
            _jsonRequest = WebRequest.Create(_strStreamUrl)
            _jsonRequest.Headers.Add("Client-ID", "5972mihwdpuicb99ys8j86bn2ely2qw")
            'Get response
            Try
                _jsonResponse = CType(_jsonRequest.GetResponse(), HttpWebResponse)
            Catch ex As Exception
                MessageBox.Show("Could not connect to Twitch server. Please check your network connection. GV will now close.", "Connection Error", MessageBoxButton.OK, MessageBoxImage.Information)
                End
            End Try
            'Get stream
            _jsonStream = _jsonResponse.GetResponseStream()
            'Create StreamReader object
            _jsonReader = New StreamReader(_jsonStream)
            'Assign JSON response as string
            _strJsonResponseUrl = _jsonReader.ReadToEnd()
            Console.WriteLine(_strJsonResponseUrl)
            'Close reader and stream
            _jsonReader.Close()
            _jsonStream.Close()
            Return _strJsonResponseUrl
        End Get
    End Property

    Public Sub New(strSUrl As String)
        _strStreamUrl = strSUrl
    End Sub

End Class
