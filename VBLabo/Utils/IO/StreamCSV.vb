Imports System.IO


#Region "TODO"
'TODO: 標準リスト、List, Array, ArrayListの違い
' Arrayは標準配列はじめにメモリを確保する必要がある（静的配列）
' Listは一つの型のオブジェクトを格納（動的配列）
' ArrayListは複数の方のオブジェクトを格納（動的配列）

'TODO: 多次元配列
'TODO: CSVパーサー
'TODO: NULLチェック
#End Region

Namespace Utils.IO
    Public Class StreamCSV
        Private Shared Sub SetDefault(ByRef append As Boolean, ByRef encoding As Text.Encoding, ByRef bufferSize As Integer)
            If encoding Is Nothing Then
                encoding = Text.Encoding.Default
            End If
            If bufferSize < 0 Then
                bufferSize = 4096
            End If
        End Sub

        Public Shared Sub WriteField(Of T)(items As List(Of List(Of T)), sep As String, path As String, Optional append As Boolean = False, Optional encoding As Text.Encoding = Nothing, Optional bufferSize As Integer = -1)
            SetDefault(append, encoding, bufferSize)

            Using w As New StreamWriter(path, append, encoding, bufferSize)
                'For i =
                '    w.Write()
                'Next
            End Using
        End Sub
    End Class
End Namespace