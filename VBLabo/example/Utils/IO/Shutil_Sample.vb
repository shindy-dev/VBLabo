Module Shutil_Sample
    Public Sub Sample()
        Dim src As String = "C:\Users\shindy\source\repos\STLib\Test\test_src.txt"
        Dim dst As String = "C:\Users\shindy\source\repos\STLib\Test\Test2"
        VBLabo.Utils.IO.Shutil.Copy(src, dst, override:=True)
    End Sub
End Module