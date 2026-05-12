Option Strict On
Option Infer On

Imports System.IO

Public Class ResxTranslator

    Public Shared Async Function TranslateFileAsync(
        filePath As String,
        sourceLang As String,
        targetLang As String,
        translateExistingLangFiles As Boolean) As Task

        Program.LogInfo("Uploading RESX to server...")

        Dim base64 =
            Await LibreTranslate.TranslateResxAsync(
                filePath,
                sourceLang,
                targetLang)

        Program.LogInfo("Server translated file.")

        Dim xmlBytes =
            Convert.FromBase64String(base64)

        Dim xmlText =
            Text.Encoding.UTF8.GetString(xmlBytes)

        Dim directory =
            Path.GetDirectoryName(filePath)

        Dim fileName =
            Path.GetFileNameWithoutExtension(filePath)

        If fileName.EndsWith(
            "." & sourceLang,
            StringComparison.OrdinalIgnoreCase) Then

            fileName =
                fileName.Substring(
                    0,
                    fileName.Length - sourceLang.Length - 1)

        End If

        Dim outputFile =
            Path.Combine(
                directory,
                fileName & "." &
                targetLang &
                ".resx")

        File.WriteAllText(
            outputFile,
            xmlText,
            Text.Encoding.UTF8)

        Program.LogInfo(
            "Saved: " &
            Path.GetFileName(outputFile))

    End Function

End Class