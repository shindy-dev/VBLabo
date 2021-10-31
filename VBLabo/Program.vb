Module Program
    Sub Main(args As String())
        ExecuteSampleFunctions()
    End Sub


    Public Delegate Sub SampleFunc()
    Private ReadOnly SampleFunctions As New List(Of SampleFunc)(New SampleFunc() {
                                                       AddressOf BenchMark_Sample.Sample,
                                                       AddressOf Path_Sample.Sample,
                                                       AddressOf Shutil_Sample.Sample,
                                                       AddressOf StreamCSV_Sample.Sample,
                                                       AddressOf DB2Manager_Sample.Sample,
                                                       AddressOf ArgumentParser_Sample.Sample
                                                       })
    Sub ExecuteSampleFunctions()
        If SampleFunctions.Count > 0 Then
            Dim strInputMsg1 As String = $"Select Sample Function No.1{If(SampleFunctions.Count > 1, $" to {SampleFunctions.Count}.", "")}(use ""exit"" to exit.){vbCrLf}>> "
            Dim strInputMsg2 As String = $" Try Again. (Please enter No.1{If(SampleFunctions.Count > 1, $" to {SampleFunctions.Count}.", "")}(use ""exit"" to exit.)){vbCrLf}>> "
            Dim inputProc As Func(Of String, String) = Function(ByVal msg As String)
                                                           Console.Write(msg)
                                                           Return Console.ReadLine()
                                                       End Function

            Console.WriteLine("===== Sample Functions =====")
            For index As Integer = 0 To SampleFunctions.Count - 1
                Console.WriteLine($"{(index + 1).ToString.PadLeft(SampleFunctions.Count.ToString().Length)}:{SampleFunctions(index).Method.ReflectedType.Name}")
            Next
            Console.WriteLine($"===== Sample Functions ====={vbCrLf}")

            Dim strFuncNo As String = inputProc(strInputMsg1)
            While strFuncNo <> "exit"
                While Not ((IsNumeric(strFuncNo) AndAlso (0 < Integer.Parse(strFuncNo) AndAlso Integer.Parse(strFuncNo) <= SampleFunctions.Count)) OrElse strFuncNo = "exit")
                    strFuncNo = inputProc(strInputMsg2)
                End While

                If strFuncNo <> "exit" Then
                    Try
                        SampleFunctions(Integer.Parse(strFuncNo) - 1)()
                    Catch ex As Exception
                        Console.WriteLine(ex)
                    End Try
                    Console.WriteLine()
                    strFuncNo = inputProc(strInputMsg1)
                End If
            End While
        End If
    End Sub
End Module
