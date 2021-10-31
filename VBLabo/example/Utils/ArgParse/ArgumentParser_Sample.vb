Module ArgumentParser_Sample
    Public Sub Sample()
        Dim parser As New Utils.ArgParse.ArgumentParser("test")
        parser.AddArgument("hoge", "test argument")
        parser.AddArgument("-u", "--update", "update data")
        Dim args As Utils.ArgParse.ArgumentNamespace = parser.ParseArgs()

    End Sub
End Module
