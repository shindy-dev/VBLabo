Module StreamCSV_Sample
    Sub Sample()
        Dim data As String(,) = {{"a", "b", "c"}, {"d", "e", "f"}, {"g", "h", "i"}}
        Dim dataL As New List(Of List(Of String))

        For i = 0 To data.GetLength(0) - 1
            dataL.Add(New List(Of String))
            For j = 0 To data.GetLength(1) - 1
                dataL(i).Add((i + j).ToString())
            Next
        Next

        For Each i In dataL
            For Each j In i
                Console.Write(j)
            Next
            Console.WriteLine()
        Next
        VBLabo.Utils.IO.StreamCSV.WriteField(dataL, ",", "test.csv")
    End Sub
End Module