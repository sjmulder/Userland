Imports System.IO
Imports Userland.Lib

Module Program
    Sub Main(args As String())
        Dim optionParser As New OptionParser()
        optionParser.Parse(args)

        Using output = Console.OpenStandardOutput()
            If args.Any() Then
                For Each path In args
                    Using input = File.OpenRead(path)
                        Cat(input, output)
                    End Using
                Next
            Else
                Using input = Console.OpenStandardInput()
                    Cat(input, output)
                End Using
            End If
        End Using
    End Sub

    Sub Cat(input As Stream, output As Stream)
        While True
            Dim byteVal = input.ReadByte()
            If byteVal = -1 Then Exit While
            output.WriteByte(byteVal)
        End While
    End Sub
End Module
