Namespace Utils.DataBase.DB2
    Class DB2Manager
        Implements IDisposable

        Public ReadOnly Property Connection As IBM.Data.DB2.DB2Connection


        Private disposedValue As Boolean

        Public Sub New(ByVal strDataBase As String, ByVal strUser As String, ByVal strPassword As String)
            Connection = CreateDb2Connection(strDataBase, strUser, strPassword)
        End Sub

        Private Function CreateDb2Connection(ByVal strDataBase As String, ByVal strUser As String, ByVal strPassword As String) As IBM.Data.DB2.DB2Connection
            Return New IBM.Data.DB2.DB2Connection($"DATABASE={strDataBase};USER={strUser};PASSWORD={strPassword}")
        End Function

        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    ' TODO: マネージド状態を破棄します (マネージド オブジェクト)
                End If

                If Connection.IsOpen() Then Connection.Close()
                Connection.Dispose()


                ' TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
                ' TODO: 大きなフィールドを null に設定します
                disposedValue = True
            End If
        End Sub

        ' TODO: 'Dispose(disposing As Boolean)' にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします
        Protected Overrides Sub Finalize()
            ' このコードを変更しないでください。クリーンアップ コードを 'Dispose(disposing As Boolean)' メソッドに記述します
            Dispose(disposing:=False)
            MyBase.Finalize()
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            ' このコードを変更しないでください。クリーンアップ コードを 'Dispose(disposing As Boolean)' メソッドに記述します
            Dispose(disposing:=True)
            GC.SuppressFinalize(Me)
        End Sub
    End Class
End Namespace