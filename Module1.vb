Option Strict On
Option Infer On

Imports System.IO
Imports System.Text

Module Program

    Public TotalFiles As Integer = 0
    Public ProcessedFiles As Integer = 0

    Public UserApiKey As String = ""

    Public ConfigFile As String =
        "config.ini"

    Sub Main()

        Console.OutputEncoding =
            Encoding.UTF8

        Console.InputEncoding =
            Encoding.UTF8

        Console.Title =
            "TurboResX"

        MainLoop().
            GetAwaiter().
            GetResult()

    End Sub

    Private Async Function MainLoop() As Task

        While True

            Console.Clear()

            DrawBanner()

            Console.ForegroundColor =
                ConsoleColor.Cyan

            Console.WriteLine()
            Console.WriteLine(
                "             TurboResX Translator")

            Console.ResetColor()

            LoadApiKey()

            If String.IsNullOrWhiteSpace(
                UserApiKey) Then

                Console.WriteLine()
                Console.Write(
                    "API Key > ")

                UserApiKey =
                    Console.ReadLine().Trim()

                SaveApiKey()

            Else

                Console.WriteLine()

                Console.ForegroundColor =
                    ConsoleColor.DarkGray

                Console.WriteLine(
                    "Loaded saved API key.")

                Console.ResetColor()

            End If

            Console.WriteLine()
            Console.WriteLine(
                "Drop .resx file or folder:")

            Console.Write("> ")

            Dim input As String =
                Console.ReadLine().
                Trim(""""c)

            If input.ToLower() =
                "change key" Then

                Console.WriteLine()

                Console.Write(
                    "New API Key > ")

                UserApiKey =
                    Console.ReadLine().Trim()

                SaveApiKey()

                Console.ForegroundColor =
                    ConsoleColor.Green

                Console.WriteLine(
                    "API key updated.")

                Console.ResetColor()

                Pause()

                Continue While

            End If

            If Not File.Exists(input) AndAlso
               Not Directory.Exists(input) Then

                LogError(
                    "Invalid path.")

                Pause()

                Continue While

            End If

            Console.WriteLine()

            Console.Write(
                "Source language: ")

            Dim sourceLang As String =
                Console.ReadLine().
                Trim().
                ToLower()

            Console.Write(
                "Target language: ")

            Dim targetLang As String =
                Console.ReadLine().
                Trim().
                ToLower()

            Console.WriteLine()

            Dim translateExistingLangFiles As Boolean =
                False

            Console.WriteLine(
                $"If {sourceLang}.resx exists:")

            Console.WriteLine(
                "1 = Translate main .resx")

            Console.WriteLine(
                $"2 = Translate existing {sourceLang}.resx")

            Console.Write(
                "Select: ")

            Dim mode As String =
                Console.ReadLine()

            translateExistingLangFiles =
                (mode = "2")

            Console.WriteLine()

            Dim files As New List(Of String)

            If File.Exists(input) Then

                If IsValidResxFile(
                    input,
                    sourceLang,
                    translateExistingLangFiles) Then

                    files.Add(input)

                End If

            Else

                files.AddRange(
                    Directory.GetFiles(
                        input,
                        "*.resx",
                        SearchOption.AllDirectories).
                    Where(Function(f)

                              Return IsValidResxFile(
                                  f,
                                  sourceLang,
                                  translateExistingLangFiles)

                          End Function))

            End If

            If files.Count = 0 Then

                LogError(
                    "No valid .resx files found.")

                Pause()

                Continue While

            End If

            TotalFiles = files.Count
            ProcessedFiles = 0

            LogInfo(
                $"Found {files.Count} file(s)")

            Console.WriteLine()

            For Each file In files

                Try

                    LogInfo(
                        "Processing: " &
                        Path.GetFileName(file))

                    Await ResxTranslator.
                        TranslateFileAsync(
                            file,
                            sourceLang,
                            targetLang,
                            translateExistingLangFiles)

                    ProcessedFiles += 1

                    Dim percent As Integer =
                        CInt(
                            (ProcessedFiles /
                            TotalFiles) * 100)

                    Console.ForegroundColor =
                        ConsoleColor.Green

                    Console.WriteLine(
                        $"[OK] {Path.GetFileName(file)} ({percent}%)")

                    Console.ResetColor()

                Catch ex As Exception

                    If ex.Message =
                        "INVALID_API_KEY" Then

                        Console.ForegroundColor =
                            ConsoleColor.Red

                        Console.WriteLine()
                        Console.WriteLine(
                            "Invalid API key.")

                        Console.WriteLine(
                            "Type 'change key' in path input.")

                        Console.ResetColor()
                        Exit For
                    Else

                        LogError(
                            ex.Message)

                    End If

                End Try

            Next

            Console.WriteLine()

            Console.ForegroundColor =
                ConsoleColor.Green

            Console.WriteLine(
                "Translation completed.")

            Console.ResetColor()
            Console.WriteLine()

            Console.ForegroundColor =
    ConsoleColor.Yellow

            Console.Write(
    "Add translated languages to project? (y/n): ")

            Console.ResetColor()

            Dim addToProject =
    Console.ReadLine().
    Trim().
    ToLower()

            If addToProject = "y" Then

                Try
                    '
                    ProjectUpdater.
    UpdateProjects(
        files,
        targetLang,
        input)
                    Console.ForegroundColor =
            ConsoleColor.Green

                    Console.WriteLine(
            "Project updated successfully.")

                    Console.ResetColor()

                Catch ex As Exception

                    LogError(
            ex.Message)

                End Try

            End If
            Console.WriteLine(
                "Press any key to return menu...")

            Console.ReadKey()

        End While

    End Function

    Private Function IsValidResxFile(
        file As String,
        sourceLang As String,
        translateExistingLangFiles As Boolean) As Boolean

        Dim name As String =
            Path.GetFileName(file).
            ToLower()

        If name = "resources.resx" Then
            Return False
        End If

        Dim isLanguageFile As Boolean =
            System.Text.RegularExpressions.Regex.IsMatch(
                name,
                "\.[a-z]{2}\.resx$")

        If translateExistingLangFiles Then

            Return name.EndsWith(
                "." &
                sourceLang.ToLower() &
                ".resx")

        Else

            Return Not isLanguageFile

        End If

    End Function

    Public Sub SaveApiKey()

        File.WriteAllText(
            ConfigFile,
            UserApiKey)

    End Sub

    Public Sub LoadApiKey()

        If File.Exists(
            ConfigFile) Then

            UserApiKey =
                File.ReadAllText(
                    ConfigFile).Trim()

        End If

    End Sub

    Public Sub DrawBanner()

        Console.ForegroundColor =
            ConsoleColor.Magenta

        Console.WriteLine(
" ___________          ___.         __________              ____  ___ ")

        Console.WriteLine(
" \__    ___/_ ________\_ |__   ____\______   \ ____   _____\   \/  / ")

        Console.WriteLine(
"   |    | |  |  \_  __ \ __ \ /  _ \|       _// __ \ /  ___/\     /  ")

        Console.WriteLine(
"   |    | |  |  /|  | \/ \_\ (  <_> )    |   \  ___/ \___ \ /     \ ")

        Console.WriteLine(
"   |____| |____/ |__|  |___  /\____/|____|_  /\___  >____  >___/\  \ ")

        Console.WriteLine(
"                           \/              \/     \/     \/      \_/ ")

        Console.ResetColor()

    End Sub

    Public Sub LogInfo(
        message As String)

        Console.ForegroundColor =
            ConsoleColor.DarkGray

        Console.WriteLine(
            $"[{DateTime.Now:HH:mm:ss}] {message}")

        Console.ResetColor()

    End Sub

    Public Sub LogError(
        message As String)

        Console.ForegroundColor =
            ConsoleColor.Red

        Console.WriteLine()
        Console.WriteLine(
            "[ERROR]")

        Console.WriteLine(
            message)

        Console.ResetColor()

    End Sub

    Public Sub Pause()

        Console.WriteLine()
        Console.WriteLine(
            "Press any key to continue...")

        Console.ReadKey()

    End Sub

End Module