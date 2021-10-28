Imports IBM.Data.DB2

Module DB2Manager_Sample
    Public Sub Sample()
        Dim connection As DB2Connection = CreateDb2Connection("TESTDB", "db2admin", "db2admi")
        Console.WriteLine(connection)
    End Sub

    Public Function CreateDb2Connection(ByVal strDataBase As String, ByVal strUserId As String, ByVal strPassword As String) As DB2Connection
        Return New DB2Connection($"DATABASE={strDataBase};USERID={strUserId};PASSWORD={strPassword}")

    End Function

    Public Function SelectData(ByVal db2Con As DB2Connection) As String

        Return ""
    End Function

    Public Sub InsertData()

    End Sub
End Module
