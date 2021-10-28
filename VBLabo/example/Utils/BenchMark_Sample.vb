Public Class TestCallByNameClass
#Region "【メンバ変数】"
    Private ReadOnly m_A As String = "A"
    Private ReadOnly m_B As String = "B"
    Private ReadOnly m_C As String = "C"
    Private ReadOnly m_D As String = "D"
    Private ReadOnly m_E As String = "E"
#End Region
#Region "【プロパティ】"
    Public ReadOnly Property A() As String
        Get
            Return m_A
        End Get
    End Property
    Public ReadOnly Property B() As String
        Get
            Return m_B
        End Get
    End Property
    Public ReadOnly Property C() As String
        Get
            Return m_C
        End Get
    End Property
    Public ReadOnly Property D() As String
        Get
            Return m_D
        End Get
    End Property
    Public ReadOnly Property E() As String
        Get
            Return m_E
        End Get
    End Property
#End Region
#Region "【メソッド】"
    ''' <summary>
    ''' CallByNameを使用してプロパティを呼び出す
    ''' </summary>
    Public Sub CallMembersWithCallByName()
        For Each m In New String() {"A", "B", "C", "D", "E"}
            CallByName(Me, m, CallType.Get)
        Next
    End Sub

    ''' <summary>
    ''' 通常通りにプロパティを呼び出す
    ''' </summary>
    Public Function CallMembers() As Boolean
        For Each m In New String() {A, B, C, D, E}  ' ←ここで呼び出し

        Next
        Return True
    End Function
#End Region
End Class

Module BenchMark_Sample
    Sub Sample()
        ' 処理回数
        Dim times As Long = 1000000

        ' テストクラス
        Dim clsTest As New TestCallByNameClass()

        ' ベンチマーク(処理時間(s)を受け取る)
        Dim CallByNameResult As Utils.BenchMark = Utils.BenchMark.Method(times, NameOf(clsTest.CallMembersWithCallByName), Sub() clsTest.CallMembersWithCallByName())
        Dim BetaGakiResult As Utils.BenchMark = Utils.BenchMark.Method(times, NameOf(clsTest.CallMembers), Sub() clsTest.CallMembers())

        If (CallByNameResult < BetaGakiResult) Then
            Console.WriteLine($"{CallByNameResult.MethodName} is faster than {BetaGakiResult.MethodName}.")
        ElseIf (CallByNameResult > BetaGakiResult) Then
            Console.WriteLine($"{BetaGakiResult.MethodName} is faster than {CallByNameResult.MethodName}.")
        Else
            Console.WriteLine("same time.")
        End If

    End Sub


End Module
