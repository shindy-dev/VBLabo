Imports System.Collections.ObjectModel

Namespace Utils.IO
    Public Class OS
        Public Shared Function ListDir(ByVal path As String) As ReadOnlyCollection(Of String)
            Return New ReadOnlyCollection(Of String)(System.IO.Directory.GetFiles(path).Concat(System.IO.Directory.GetDirectories(path)).ToList())
        End Function

        ''' <summary>
        ''' �p�X����iPython�̊֐����Č��j
        ''' </summary>
        Public Class Path
            ''' <summary>
            ''' �p�X�̋�ؕ����iWin�n�F�u\�v�o�b�N�X���b�V��, Unix�n�F�u/�v�X���b�V���j
            ''' </summary>
            ''' <returns></returns>
            Public Shared ReadOnly Property Sep() As Char
                Get
                    Return System.IO.Path.DirectorySeparatorChar
                End Get
            End Property

            ''' <summary>
            ''' ��΃p�X�擾
            ''' </summary>
            ''' <param name="path"></param>
            ''' <returns></returns>
            Public Shared Function AbsPath(ByVal path As String) As String
                Return System.IO.Path.GetFullPath(path)
            End Function

            ''' <summary>
            ''' �t�@�C�����^�t�H���_���̎擾
            ''' </summary>
            ''' <param name="path"></param>
            ''' <returns></returns>
            Public Shared Function BaseName(ByVal path As String) As String
                Return System.IO.Path.GetFileName(path)
            End Function

            ''' <summary>
            ''' �e�f�B���N�g���̎擾
            ''' </summary>
            ''' <param name="path"></param>
            ''' <returns></returns>
            Public Shared Function DirName(ByVal path As String) As String
                Return System.IO.Path.GetDirectoryName(path)
            End Function

            ''' <summary>
            ''' �t�@�C���^�t�H���_�̑��݂��邩����
            ''' </summary>
            ''' <param name="path"></param>
            ''' <returns></returns>
            Public Shared Function Exists(ByVal path As String) As Boolean
                Return System.IO.File.Exists(path) OrElse System.IO.Directory.Exists(path)
            End Function

            ''' <summary>
            ''' ��΃p�X������
            ''' </summary>
            ''' <param name="path"></param>
            ''' <returns></returns>
            Public Shared Function IsAbs(ByVal path As String) As Boolean
                Return System.IO.Path.IsPathFullyQualified(path)
            End Function

            ''' <summary>
            ''' �t�@�C��������
            ''' </summary>
            ''' <param name="path"></param>
            ''' <returns></returns>
            Public Shared Function IsFile(ByVal path As String) As Boolean
                Return System.IO.File.Exists(path)
            End Function

            ''' <summary>
            ''' �f�B���N�g��������
            ''' </summary>
            ''' <param name="path"></param>
            ''' <returns></returns>
            Public Shared Function IsDir(ByVal path As String) As Boolean
                Return System.IO.Directory.Exists(path)
            End Function

            ''' <summary>
            ''' �p�X�̌���
            ''' </summary>
            ''' <param name="paths"></param>
            ''' <returns></returns>
            Public Shared Function Join(ByVal ParamArray paths As String()) As String
                Return System.IO.Path.Combine(paths)
            End Function

            ''' <summary>
            ''' �p�X���p�X�̋�ؕ����ŕ���
            ''' </summary>
            ''' <param name="path"></param>
            ''' <returns></returns>
            Public Shared Function Split(ByVal path As String) As String()
                Return path.Split(System.IO.Path.DirectorySeparatorChar)
            End Function

            ''' <summary>
            ''' �t�@�C�����Ɗg���q�𕪊�
            ''' </summary>
            ''' <param name="path"></param>
            ''' <returns>[�t�@�C�����i�p�X�w��̏ꍇ�͑O�Ƀp�X���t���j, �g���q]</returns>
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