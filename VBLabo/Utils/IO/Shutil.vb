Namespace Utils.IO
    ''' <summary>
    ''' 高水準のファイル操作（Pythonの関数を再現）
    ''' </summary>
    Public Class Shutil
        Public Class SameFileError
            Inherits Exception
            Sub New(ByVal message As String)
                MyBase.New(message)
            End Sub
            Sub New(ByVal src As String, ByVal dst As String)
                MyBase.New(String.Format("{0} and {1} are the same file", src, dst))
            End Sub
        End Class

        ''' <summary>
        ''' コピー可能可否
        ''' </summary>
        ''' <param name="src"></param>
        ''' <param name="dst"></param>
        ''' <returns></returns>
        Private Shared Function CheckCopyable(ByVal src As String, ByVal dst As String) As Boolean
            If os.Path.AbsPath(src) = os.Path.AbsPath(dst) Then
                ' コピー元のパス = コピー先のパス
                Throw New SameFileError(src, dst)
            ElseIf os.Path.IsDir(src) AndAlso os.Path.IsFile(dst) Then
                ' コピー元がディレクトリでコピー先がファイル
                Throw New System.IO.IOException("can't copy destination directory into source file")
            ElseIf Not os.Path.Exists(src) Then
                ' コピー元のパスが不正
                Throw New System.IO.FileNotFoundException("Invalid source path", src)
            ElseIf Not os.Path.Exists(os.Path.DirName(dst)) Then
                ' コピー先の親パスが不正
                Throw New System.IO.FileNotFoundException("Invalid destination parent path", os.Path.DirName(dst))
            End If
            Return True
        End Function

        ''' <summary>
        ''' フォルダのコピー
        ''' </summary>
        ''' <param name="src"></param>
        ''' <param name="dst"></param>
        ''' <param name="override"></param>
        Public Shared Sub Copy(ByVal src As String, ByVal dst As String, Optional ByVal override As Boolean = False)
            If (os.Path.IsFile(src)) Then
                CopyFile(src, dst, override)
            ElseIf CheckCopyable(src, dst) Then
                FileIO.FileSystem.CopyDirectory(src, dst, override)
            End If
        End Sub

        ''' <summary>
        ''' ファイルのコピー
        ''' </summary>
        ''' <param name="src"></param>
        ''' <param name="dst"></param>
        ''' <param name="override"></param>
        Public Shared Sub CopyFile(ByVal src As String, ByVal dst As String, Optional ByVal override As Boolean = False)
            ' コピー元がファイルパス、コピー先がディレクトリパスの場合はコピー先のパス + コピー元のファイル名とし、CopyFileを呼ぶ
            dst = If(os.Path.IsDir(dst), System.IO.Path.Combine(dst, os.Path.BaseName(src)), dst)
            If CheckCopyable(src, dst) Then FileIO.FileSystem.CopyFile(src, dst, override)
        End Sub

        Shared Sub Test()
            Dim strMsg As String = "Success"
            Try
                Dim src As String = "C:\Users\shindy\source\repos\STLib\Test\test_src.txt"
                Dim dst As String = "C:\Users\shindy\source\repos\STLib\Test\Test2"
                Copy(src, dst, override:=True)
            Catch ex As Exception
                strMsg = "Failed"
                Throw
            Finally
                Console.WriteLine(strMsg)
                Debug.WriteLine(strMsg)
            End Try
        End Sub

    End Class
End Namespace