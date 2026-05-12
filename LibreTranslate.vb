Option Strict On
Option Infer On

Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports Newtonsoft.Json.Linq

Public Class LibreTranslate

    Private Shared ReadOnly client As HttpClient

    Private Const API_URL As String =
        "https://ironhand.store/resx/translate_resx.php"

    Private Shared ReadOnly Property API_KEY As String
        Get
            Return Program.UserApiKey
        End Get
    End Property

    Shared Sub New()

        ServicePointManager.SecurityProtocol =
            SecurityProtocolType.Tls12

        ServicePointManager.ServerCertificateValidationCallback =
            Function(
                sender,
                certificate,
                chain,
                sslPolicyErrors) True

        client = New HttpClient()

        client.Timeout =
            TimeSpan.FromMinutes(5)

        client.DefaultRequestHeaders.UserAgent.Add(
            New ProductInfoHeaderValue(
                "TurboResX",
                "1.0"))

    End Sub

    Public Shared Async Function TranslateResxAsync(
        filePath As String,
        sourceLang As String,
        targetLang As String) As Task(Of String)

        Try

            Dim form As New MultipartFormDataContent()

            form.Add(
                New StringContent(API_KEY),
                "key")

            form.Add(
                New StringContent(sourceLang),
                "source")

            form.Add(
                New StringContent(targetLang),
                "target")

            Dim bytes =
                IO.File.ReadAllBytes(filePath)

            Dim fileContent =
                New ByteArrayContent(bytes)

            fileContent.Headers.ContentType =
                MediaTypeHeaderValue.Parse(
                    "application/octet-stream")

            form.Add(
                fileContent,
                "file",
                IO.Path.GetFileName(filePath))

            Dim response =
                Await client.PostAsync(
                    API_URL,
                    form)

            Dim responseText =
                Await response.Content.ReadAsStringAsync()

            If Not response.IsSuccessStatusCode Then

                Throw New Exception(
                    "HTTP ERROR: " &
                    response.StatusCode &
                    vbCrLf &
                    responseText)

            End If

            Dim obj =
                JObject.Parse(responseText)

            If obj("success") Is Nothing Then

                Throw New Exception(
                    "Invalid JSON response:" &
                    vbCrLf &
                    responseText)

            End If

            If obj("success").ToString().ToLower() <> "true" Then

                Throw New Exception(
                    obj("message").ToString())

            End If

            Return obj("translated_resx").ToString()

        Catch ex As Exception

            Throw New Exception(
                "API CONNECTION ERROR" &
                vbCrLf &
                ex.ToString())

        End Try

    End Function

End Class