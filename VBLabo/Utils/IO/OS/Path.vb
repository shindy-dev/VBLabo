Imports System.Collections.ObjectModel

Namespace Utils.IO
    Public Class OS
        Public Shared Function ListDir(ByVal path As String) As ReadOnlyCollection(Of String)
            Return New ReadOnlyCollection(Of String)(System.IO.Directory.GetFiles(path).Concat(System.IO.Directory.GetDirectories(path)).ToList())
        End Function

        ''' <summary>
        ''' パス操作（Pythonの関数を再現）
        ''' </summary>
        Public Class Path
            ''' <summary>
            ''' パスの区切文字（Win系：「\」バックスラッシュ, Unix系：「/」スラッシュ）
            ''' </summary>
            ''' <returns></returns>
            Public Shared ReadOnly Property Sep() As Char
                Get
                    Return System.IO.Path.DirectorySeparatorChar
                End Get
            End Property

            ''' <summary>
            ''' 絶対パス取得
            ''' </summary>
            ''' <param name="path"></param>
            ''' <returns></returns>
            Public Shared Function AbsPath(ByVal path As String) As String
                Return System.IO.Path.GetFullPath(path)
            End Function

            ''' <summary>
            ''' ファイル名／フォルダ名の取得
            ''' </summary>
            ''' <param name="path"></param>
            ''' <returns></returns>
            Public Shared Function BaseName(ByVal path As String) As String
                Return System.IO.Path.GetFileName(path)
            End Function

            ''' <summary>
            ''' 親ディレクトリの取得
            ''' </summary>
            ''' <param name="path"></param>
            ''' <returns></returns>
            Public Shared Function DirName(ByVal path As String) As String
                Return System.IO.Path.GetDirectoryName(path)
            End Function

            ''' <summary>
            ''' ファイル／フォルダの存在するか判定
            ''' </summary>
            ''' <param name="path"></param>
            ''' <returns></returns>
            Public Shared Function Exists(ByVal path As String) As Boolean
                Return System.IO.File.Exists(path) OrElse System.IO.Directory.Exists(path)
            End Function

            ''' <summary>
            ''' 絶対パスか判定
            ''' </summary>
            ''' <param name="path"></param>
            ''' <returns></returns>
            Public Shared Function IsAbs(ByVal path As String) As Boolean
                Return System.IO.Path.IsPathFullyQualified(path)
            End Function

            ''' <summary>
            ''' ファイルか判定
            ''' </summary>
            ''' <param name="path"></param>
            ''' <returns></returns>
            Public Shared Function IsFile(ByVal path As String) As Boolean
                Return System.IO.File.Exists(path)
            End Function

            ''' <summary>
            ''' ディレクトリか判定
            ''' </summary>
            ''' <param name="path"></param>
            ''' <returns></returns>
            Public Shared Function IsDir(ByVal path As String) As Boolean
                Return System.IO.Directory.Exists(path)
            End Function

            ''' <summary>
            ''' パスの結合
            ''' </summary>
            ''' <param name="paths"></param>
            ''' <returns></returns>
            Public Shared Function Join(ByVal ParamArray paths As String()) As String
                Return System.IO.Path.Combine(paths)
            End Function

            ''' <summary>
            ''' パスをパスの区切文字で分割
            ''' </summary>
            ''' <param name="path"></param>
            ''' <returns></returns>
            Public Shared Function Split(ByVal path As String) As String()
                Return path.Split(System.IO.Path.DirectorySeparatorChar)
            End Function

            ''' <summary>
            ''' ファイル名と拡張子を分割
            ''' </summary>
            ''' <param name="path"></param>
            ''' <returns>[ファイル名（パス指定の場合は前にパスが付く）, 拡張子]</returns>
            Public Shared Function SplitExt(ByVal path As String) As String()
                Return New String() {System.IO.Path.Combine(DirName(path), System.IO.Path.GetFileNameWithoutExtension(path)), System.IO.Path.GetExtension(path)}
            End Function

            Shared Sub Test()
                Dim strMsg As String = "Success"
                Try
                    Dim targetPath As String = "C:\Users\shindy\source\repos\STLib\Test"
                    Console.WriteLine(IsAbs(targetPath))
                    For Each v As String In SplitExt(targetPath)
                        Console.WriteLine(v)
                    Next
                Catch ex As Exception
                    strMsg = "Failed"
                    Throw
                Finally
                    Console.WriteLine(strMsg)
                    Debug.WriteLine(strMsg)
                End Try
            End Sub
        End Class
    End Class
End Namespace