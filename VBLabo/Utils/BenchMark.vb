Namespace Utils
    Public Class BenchMark
#Region "【MemberVariable】"
        ''' <summary>
        ''' 関数名
        ''' </summary>
        Private mstrMethodName As String
        ''' <summary>
        ''' 処理時間[ms]
        ''' </summary>
        Private mdblMilliseconds As Double

        ''' <summary>
        ''' ログの仕切り文字
        ''' </summary>
        Private Shared ReadOnly mstrLine As String = $" {StrDup(10, "="c)} "

#End Region

#Region "【Property】"
        ''' <summary>
        ''' 関数名
        ''' </summary>
        Public ReadOnly Property MethodName() As String
            Get
                Return mstrMethodName
            End Get
        End Property
        ''' <summary>
        ''' 処理時間[ms]
        ''' </summary>
        Public ReadOnly Property Milliseconds() As Double
            Get
                Return mdblMilliseconds
            End Get
        End Property
#End Region

#Region "【Delegate】"
        ''' <summary>
        ''' デリゲート
        ''' </summary>
        Public Delegate Sub MethodWrapper()
#End Region

#Region "【Shared Method】"
        ''' <summary>
        ''' ベンチマーク関数
        ''' </summary>
        ''' <param name="lngTimes"></param>
        ''' <param name="strMethodName"></param>
        ''' <param name="mtdWrapper"></param>
        ''' <returns></returns>
        Public Shared Function Method(ByVal lngTimes As Long, ByVal strMethodName As String, ByVal mtdWrapper As MethodWrapper) As BenchMark
            StartProc(lngTimes, strMethodName)  ' 前処理
            Dim sw As New Stopwatch()
            sw.Start()
            For i = 1 To lngTimes
                mtdWrapper()  ' 実行
            Next
            sw.Stop()
            Return EndProc(sw, strMethodName)  ' 後処理
        End Function

        ''' <summary>
        ''' 後処理
        ''' </summary>
        ''' <returns></returns>
        Private Shared Function EndProc(ByVal sw As Stopwatch, ByVal strMethodName As String) As BenchMark
            Console.WriteLine($" - TotalTime : {sw.ElapsedMilliseconds}[ms]{vbCrLf}")
            Console.WriteLine($"{mstrLine}{strMethodName} ---  End  {mstrLine}{vbCrLf}")
            Dim Result As New BenchMark With {
                .mstrMethodName = strMethodName,
                .mdblMilliseconds = sw.ElapsedMilliseconds
            }
            Return Result
        End Function

        ''' <summary>
        ''' 前処理
        ''' </summary>
        Private Shared Sub StartProc(ByVal lngTimes As Long, ByVal strMethodName As String)
            Console.WriteLine($"{vbCrLf}{mstrLine}{strMethodName} --- Start {mstrLine}{vbCrLf}")
            Console.WriteLine($" - MethodName: {strMethodName}")
            Console.WriteLine($" - ProcTimes : {lngTimes:#,0}")
#If DEBUG Then
            Console.WriteLine(" - Debug     : True")
#Else
        Console.WriteLine(" - Debug     : False")
#End If
        End Sub

        Public Shared Operator <(ByVal left As BenchMark, ByVal right As BenchMark) As Boolean
            Return left.mdblMilliseconds < right.mdblMilliseconds
        End Operator
        Public Shared Operator >(ByVal left As BenchMark, ByVal right As BenchMark) As Boolean
            Return left.mdblMilliseconds > right.mdblMilliseconds
        End Operator
        Public Shared Operator =(ByVal left As BenchMark, ByVal right As BenchMark) As Boolean
            Return left.mdblMilliseconds = right.mdblMilliseconds
        End Operator
        Public Shared Operator <>(ByVal left As BenchMark, ByVal right As BenchMark) As Boolean
            Return left.mdblMilliseconds <> right.mdblMilliseconds
        End Operator



#End Region
#Region "【Method】"

#End Region
    End Class
End Namespace