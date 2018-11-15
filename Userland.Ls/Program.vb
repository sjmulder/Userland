Imports System.IO
Imports Userland.Lib

Module Program
    Dim ShowHidden = False
    Dim PrintDetails = False
    Dim PrintSuffixes = False
    Dim UseHumanSizes = False

    Sub Main(args As String())
        Dim optionParser As New OptionParser()
        optionParser.AddOption("a", Sub() showHidden = True)
        optionParser.AddOption("l", Sub() PrintDetails = True)
        optionParser.AddOption("F", Sub() PrintSuffixes = True)
        optionParser.AddOption("h", Sub() UseHumanSizes = True)
        optionParser.Parse(args)

        If Not args.Any() Then
            ListDirectory(New DirectoryInfo("."))
        Else
            Dim files As New List(Of String)
            Dim directories = New List(Of String)
            For Each arg In args
                If File.Exists(arg) Then files.Add(arg) Else directories.Add(arg)
            Next

            If files.Any() Then
                For Each file In files
                    ListEntry(New FileInfo(file))
                Next
                If directories.Any() Then Console.WriteLine()
            End If

            For Each directory In directories
                Console.WriteLine("{0}:", directory)
                ListDirectory(New DirectoryInfo(directory))
                Console.WriteLine()
            Next
        End If
    End Sub

    Sub ListDirectory(directory As DirectoryInfo)
        For Each entry In directory.GetFileSystemInfos()
            If entry.Attributes.HasFlag(FileAttributes.Hidden) Then
                If Not ShowHidden Then Continue For
            End If
            ListEntry(entry)
        Next
    End Sub

    Sub ListEntry(entry As FileSystemInfo)
        If PrintDetails Then
            If TypeOf entry Is FileInfo Then
                Console.Write("- ")
                Dim file As FileInfo = entry
                Console.Write("{0} ", entry.LastWriteTime.ToShortDateString())
                Console.Write("{0} ", entry.LastWriteTime.ToShortTimeString())
                Console.Write("{0,7} ", file.Length)
            Else
                Console.Write("d ")
                Console.Write("{0} ", entry.LastWriteTime.ToShortDateString())
                Console.Write("{0} ", entry.LastWriteTime.ToShortTimeString())
                Console.Write("        ")
            End If
        End If
        Console.Write(entry.Name)
        If PrintSuffixes Then
            If TypeOf entry Is DirectoryInfo Then Console.Write("/")
        End If
        Console.WriteLine()
    End Sub
End Module
