Imports VBLabo.Utils.IO
Module Path_Sample
    Sub Sample()
        For Each path As String In OS.ListDir("C:\Users\shindy\source\repos\VBLabo\VBLabo\example\Utils\")
            Console.WriteLine(path)
        Next
    End Sub
End Module
