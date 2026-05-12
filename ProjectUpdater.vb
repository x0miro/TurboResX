Option Strict On
Option Infer On

Imports System.IO
Imports System.Xml.Linq

Public Class ProjectUpdater

    Public Shared Sub UpdateProjects(
    translatedFiles As List(Of String),
    targetLang As String,
    searchPath As String)

        Dim rootPath As String

        If File.Exists(searchPath) Then

            rootPath =
        Path.GetDirectoryName(searchPath)

        Else

            rootPath = searchPath

        End If

        Dim projectFiles =
    Directory.GetFiles(
        rootPath,
        "*.vbproj",
        SearchOption.AllDirectories)

        If projectFiles.Length = 0 Then

            Throw New Exception(
                "No .vbproj found.")

        End If

        For Each projectFile In projectFiles

            Dim doc As XDocument =
                XDocument.Load(projectFile)

            Dim project =
                doc.Root

            If project Is Nothing Then
                Continue For
            End If

            Dim itemGroup =
                project.Elements().
                FirstOrDefault(
                    Function(x)
                        Return x.Name.LocalName =
                            "ItemGroup"
                    End Function)

            If itemGroup Is Nothing Then

                itemGroup =
                    New XElement(
                        project.Name.Namespace +
                        "ItemGroup")

                project.Add(itemGroup)

            End If

            For Each translatedFile
                In translatedFiles

                Dim directory =
                    Path.GetDirectoryName(
                        translatedFile)

                Dim fileName =
                    Path.GetFileNameWithoutExtension(
                        translatedFile)

                If fileName.EndsWith(
                    "." & targetLang,
                    StringComparison.OrdinalIgnoreCase) Then

                    fileName =
                        fileName.Substring(
                            0,
                            fileName.Length -
                            targetLang.Length - 1)

                End If

                Dim translatedResx =
                    fileName &
                    "." &
                    targetLang &
                    ".resx"

                Dim formFile =
                    fileName &
                    ".vb"

                Dim alreadyExists As Boolean =
    itemGroup.Elements().
    Any(Function(x)

            Dim includeAttr =
                x.Attribute("Include")

            If includeAttr Is Nothing Then
                Return False
            End If

            Return includeAttr.Value.Equals(
                translatedResx,
                StringComparison.OrdinalIgnoreCase)

        End Function)

                If alreadyExists Then
                    Continue For
                End If

                Dim embeddedResource =
                    New XElement(
                        project.Name.Namespace +
                        "EmbeddedResource",
                        New XAttribute(
                            "Include",
                            translatedResx),
                        New XElement(
                            project.Name.Namespace +
                            "DependentUpon",
                            formFile))

                itemGroup.Add(
                    embeddedResource)

                Console.ForegroundColor =
                    ConsoleColor.Cyan

                Console.WriteLine(
                    $"[PROJECT] Added {translatedResx}")

                Console.ResetColor()

            Next

            doc.Save(projectFile)

        Next

    End Sub

End Class