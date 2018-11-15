Public Class UnhandledOptionException
    Inherits Exception

    Sub New(character As Char)
        MyBase.New("Unknown option: -" + character)
    End Sub
End Class

Public Class MissingOptionArgumentException
    Inherits Exception

    Sub New(character As Char)
        MyBase.New("Missing option argument: -" + character)
    End Sub
End Class

Public Class OptionParser
    Delegate Sub OptionHandler()
    Delegate Sub OptionWithArgumentHandler(argument As String)

    Dim IgnoredOptions As New HashSet(Of Char)
    Dim OptionHandlers As New Dictionary(Of Char, OptionHandler)
    Dim OptionWithArgumentHandlers As New Dictionary(Of Char, OptionWithArgumentHandler)

    Sub IgnoreOption(character As Char)
        IgnoredOptions.Add(character)
    End Sub

    Sub AddOption(character As Char, handler As OptionHandler)
        OptionHandlers.Add(character, handler)
    End Sub

    Sub AddOption(character As Char, handler As OptionWithArgumentHandler)
        OptionWithArgumentHandlers.Add(character, handler)
    End Sub

    Sub Parse(ByRef arguments As String())
        Dim head As String = Nothing
        Dim tail As New Queue(Of String)(arguments)

        Do While head <> Nothing Or tail.Any()
            If head = "" Or head = Nothing Then
                head = tail.Peek()
                If head = "" Or head(0) <> "-" Then Exit Do
                tail.Dequeue()
                If head = "--" Then Exit Do
                head = head.Substring(1)
            End If

            Dim character = head(0)
            Dim optionHandler As OptionHandler = Nothing
            Dim optionWithArgumentHandler As OptionWithArgumentHandler = Nothing

            If IgnoredOptions.Contains(character) Then
                'Nothing
            ElseIf OptionHandlers.TryGetValue(character, optionHandler) Then
                optionHandler()
                head = head.Substring(1)
            ElseIf (OptionWithArgumentHandlers.TryGetValue(character, optionWithArgumentHandler)) Then
                Dim argument = head.Substring(1)
                If argument = "" Then
                    If Not tail.TryDequeue(argument) Then Throw New MissingOptionArgumentException(character)
                End If
            Else
                Throw New UnhandledOptionException(character)
            End If
        Loop

        arguments = tail.ToArray()
    End Sub
End Class
