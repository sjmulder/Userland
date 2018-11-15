Module Program
    Sub Main(args As String())
        If args.Any() And args(0) = "-n" Then
            Console.Write(String.Join(" ", args.Skip(1)))
        Else
            Console.WriteLine(String.Join(" ", args))
        End If
    End Sub
End Module
