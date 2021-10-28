Imports System.IO
Imports System.Text

Namespace Utils.IO

    Public Class JSONDecodeException
        Inherits Exception
        Sub New(ByVal strMsg As String)
            MyBase.New(strMsg)
        End Sub
    End Class

    Public Class JSONEncodeException
        Inherits Exception
        Sub New(ByVal strMsg As String)
            MyBase.New(strMsg)
        End Sub

    End Class

    Public NotInheritable Class JSON
        Private Sub New()

        End Sub

        ''' <summary>
        ''' 指定のファイルを読み込み、全文を取得します
        ''' </summary>
        ''' <param name="fp"></param>
        ''' <returns></returns>
        Private Shared Function ReadJson(ByVal fp As String) As String
            Using r As New StreamReader(fp)
                Return r.ReadToEnd()
            End Using
        End Function


#Region "【GetTypeFromJson】"
        Private Shared Function GetTypeFromString(ByVal strValue As String) As Type
            Dim typeValue As Type
            If strValue.StartsWith(""""c) Then
                typeValue = GetType(String)
            ElseIf strValue.StartsWith("["c) Then
                typeValue = GetType(List(Of))
            ElseIf strValue.StartsWith("{"c) Then
                typeValue = GetType(Dictionary(Of ,))
            ElseIf strValue = "true" OrElse strValue = "false" Then
                typeValue = GetType(Boolean)
            ElseIf strValue = "null" Then
                typeValue = Nothing
            ElseIf Double.TryParse(strValue, 0) Then
                typeValue = If(strValue.Contains("."c), GetType(Double), GetType(Integer))
            Else
                Throw New JSONDecodeException("Type Error")
            End If
            Return typeValue
        End Function
        Private Shared Function GetValueFromString(ByVal strValue As String) As Object
            Dim value As Object
            Select Case GetTypeFromString(strValue)
                Case GetType(String)
                    value = strValue.Trim(""""c).Replace("\"c, "")
                Case GetType(Boolean)
                    value = strValue = "true"
                Case GetType(Double)
                    value = Double.Parse(strValue)
                Case GetType(Integer)
                    value = Integer.Parse(strValue)
                Case GetType(List(Of))
                    value = ParseList(strValue & "]"c)
                Case GetType(Dictionary(Of,))
                    value = ParseDictionary(strValue & "}"c)
                Case Nothing
                    value = Nothing
                Case Else
                    Throw New JSONDecodeException("Type Error")
            End Select
            Return value
        End Function
#End Region

#Region "【EnabledStarts/EndsWith】"
        Private Shared Function EnabledStartsWith(ByVal str As String, ByVal character As Char) As Boolean
            Dim flag As Boolean = False
            Dim disableChar As Char() = New Char() {" "c, Chr(0), Chr(8), Chr(9), Chr(10), Chr(11), Chr(12), Chr(13)}
            For Each c As Char In str
                If Array.IndexOf(disableChar, c) = -1 Then
                    If c = character Then flag = True
                    Exit For
                End If
            Next
            Return flag
        End Function
        Private Shared Function EnabledEndsWith(ByVal str As String, ByVal character As Char) As Boolean
            Dim flag As Boolean = False
            Dim disableChar As Char() = New Char() {" "c, Chr(0), Chr(8), Chr(9), Chr(10), Chr(11), Chr(12), Chr(13)}
            For Each c As Char In str.Reverse()
                If Array.IndexOf(disableChar, c) = -1 Then
                    If c = character Then flag = True
                    Exit For
                End If
            Next
            Return flag
        End Function
#End Region

#Region "【ParseMethod】"
        Private Shared Function ParseDictionary(ByVal strJson As String) As Dictionary(Of String, Object)
            If Not (EnabledStartsWith(strJson, "{"c) AndAlso EnabledEndsWith(strJson, "}"c)) Then Throw New JSONDecodeException("not Dictionary")
            Dim disableChar As Char() = New Char() {" "c, Chr(0), Chr(8), Chr(9), Chr(10), Chr(11), Chr(12), Chr(13)}

            ' RootObject
            Dim dctRoot As New Dictionary(Of String, Object)
            ' Key
            Dim key As String = String.Empty
            ' Previous Character
            Dim privChar As Char = Chr(0)

#Region "       【走査フラグ】"
            ' 文字列判断フラグ
            Dim blnString As Boolean = False
            ' リスト判断フラグ
            Dim blnList As Boolean = False
#End Region
#Region "       【文字列ビルダ】"
            ' Key
            Dim sbKey As New StringBuilder
            ' Value
            Dim sbValue As New StringBuilder
#End Region

            For Each c As Char In strJson
                ' 文字列判定（一つ前の文字がエスケープ文字でなければ文字列の開始/終了と判断）
                If c = """"c AndAlso privChar <> "\"c Then blnString = Not blnString

                ' 文字列でないとき
                If Not blnString Then
                    ' リスト判定
                    If c = "["c OrElse c = "]"c Then blnList = Not blnList

                    ' オブジェクト型のKeyとValueの区切り文字
                    If c = ":"c AndAlso key = String.Empty Then
                        key = sbKey.ToString().Trim().Trim(""""c).Replace("\"c, "")
                        sbKey.Clear()
                    ElseIf (c = ","c OrElse c = "}"c OrElse c = "]"c) AndAlso Not blnList AndAlso key <> String.Empty AndAlso sbValue.Length <> 0 Then
                        dctRoot.Add(key, GetValueFromString(sbValue.ToString().Trim(":"c).Trim()))
                        key = String.Empty
                        sbValue.Clear()
                    End If
                ElseIf key = String.Empty Then
                    sbKey.Append(c)
                End If

                If key <> String.Empty AndAlso Not ((c = ","c OrElse c = "}"c OrElse c = "]"c) AndAlso sbValue.Length = 0) Then
                    If Not ((c = ","c OrElse c = "}"c OrElse c = "]"c) AndAlso sbValue.Length = 0) Then
                        sbValue.Append(c)
                    End If
                End If

                privChar = c
            Next
            Return dctRoot
        End Function
        Private Shared Function ParseList(ByVal strJson As String) As List(Of Object)
            If Not (EnabledStartsWith(strJson, "["c) AndAlso EnabledEndsWith(strJson, "]"c)) Then Throw New JSONDecodeException("not List")
            Dim disableChar As Char() = New Char() {" "c, Chr(0), Chr(8), Chr(9), Chr(10), Chr(11), Chr(12), Chr(13)}

            ' RootObject
            Dim lstRoot As New List(Of Object)
            ' Previous Character
            Dim privChar As Char

#Region "       【走査フラグ】"
            ' 文字列判断フラグ
            Dim blnString As Boolean = False
            ' リスト判断フラグ
            Dim blnDict As Boolean = False
            ' 開始ブラケットが過ぎたかどうか
            Dim blnFirstBracket As Boolean = False
#End Region
#Region "       【文字列ビルダ】"
            ' Value
            Dim sbValue As New StringBuilder
#End Region

            For Each c As Char In strJson

                ' 文字列判定（一つ前の文字がエスケープ文字でなければ文字列の開始/終了と判断）
                If c = """"c AndAlso privChar <> "\"c Then blnString = Not blnString

                ' 文字列でないとき
                If Not blnString Then
                    ' オブジェクト判定
                    If c = "{"c OrElse c = "}"c Then blnDict = Not blnDict

                    If (c = ","c OrElse c = "}"c OrElse c = "]"c) AndAlso Not blnDict AndAlso sbValue.Length <> 0 Then
                        lstRoot.Add(GetValueFromString(sbValue.ToString().Trim()))
                        sbValue.Clear()
                    End If
                End If

                If blnFirstBracket AndAlso Array.IndexOf(disableChar, c) = -1 Then
                    If Not ((c = ","c OrElse c = "}"c OrElse c = "]"c) AndAlso sbValue.Length = 0) Then
                        sbValue.Append(c)
                    End If
                End If

                If Not blnFirstBracket AndAlso c = "["c Then blnFirstBracket = True
                privChar = c
            Next
            Return lstRoot
        End Function
#End Region

#Region "【ShowMethod】"
        Public Shared Sub ShowDictionary(ByVal dctData As Dictionary(Of String, Object), Optional ByVal depth As Integer = 0)
            Dim indent As New String(" "c, 4 * depth)
            Console.WriteLine("{")
            For i As Integer = 0 To dctData.Keys.Count - 1
                Dim strKey As String = dctData.Keys(i)
                Dim objValue As Object = dctData(strKey)
                Console.Write($"{New String(" "c, 4 * (depth + 1))}""{strKey}"": ")
                If TypeOf objValue Is Dictionary(Of String, Object) Then
                    ShowDictionary(DirectCast(objValue, Dictionary(Of String, Object)), depth + 1)
                ElseIf TypeOf objValue Is List(Of Object) Then
                    ShowList(DirectCast(objValue, List(Of Object)), depth + 1)
                Else
                    Dim strValue As String = If(objValue IsNot Nothing, If(TypeOf objValue Is String, $"""{objValue}""", objValue.ToString()), "Nothing")
                    Console.Write(strValue)
                End If
                Console.WriteLine($"{If(i <> dctData.Keys.Count - 1, ",", "")}")
            Next
            Console.Write($"{indent}{"}"}{If(depth = 0, vbCrLf, "")}")
        End Sub
        Public Shared Sub ShowList(ByVal lstData As List(Of Object), Optional ByVal depth As Integer = 0)
            Dim indent As New String(" "c, 4 * depth)
            Console.WriteLine("[")
            For i As Integer = 0 To lstData.Count - 1
                Dim objValue As Object = lstData(i)
                Console.Write($"{New String(" "c, 4 * (depth + 1))}")
                If TypeOf objValue Is Dictionary(Of String, Object) Then
                    ShowDictionary(DirectCast(objValue, Dictionary(Of String, Object)), depth + 1)
                ElseIf TypeOf objValue Is List(Of Object) Then
                    ShowList(DirectCast(objValue, List(Of Object)), depth + 1)
                Else
                    Dim strValue As String = If(objValue IsNot Nothing, If(TypeOf objValue Is String, $"""{objValue}""", objValue.ToString()), "Nothing")
                    Console.Write(strValue)
                End If
                Console.WriteLine(If(i <> lstData.Count - 1, ",", ""))
            Next
            Console.Write($"{indent}{"]"}{If(depth = 0, vbCrLf, "")}")
        End Sub
#End Region

#Region "【LoadJson】"
        ''' <summary>
        ''' 文字列からJson読み込み
        ''' </summary>
        ''' <param name="data"></param>
        ''' <returns></returns>
        Public Shared Function Loads(data As String) As Object
            Return 1
        End Function
        ''' <summary>
        ''' ファイルからJson読み込み
        ''' </summary>
        ''' <param name="fp"></param>
        ''' <returns></returns>
        Public Shared Function Load(path As String) As Object
            Dim strJson As String = ReadJson(path)
            Dim obj As Object
            'Console.WriteLine($"原本：{vbCrLf}{strJson}")
            'Console.WriteLine()
            'Console.WriteLine()
            'Console.WriteLine()

            If EnabledStartsWith(strJson, "{"c) Then
                obj = ParseDictionary(strJson)
                ShowDictionary(DirectCast(obj, Dictionary(Of String, Object)))
            ElseIf EnabledStartsWith(strJson, "["c) Then
                obj = ParseList(strJson)
                ShowList(DirectCast(obj, List(Of Object)))
            Else
                Throw New JSONDecodeException(" not Json ")
            End If
            Return obj
        End Function
#End Region
#Region "【DumpJson】"
        ''' <summary>
        ''' データからJson文字列を作成
        ''' </summary>
        ''' <param name="data"></param>
        ''' <returns></returns>
        Public Shared Function Dumps(data As Dictionary(Of Object, Object)) As String
            Return ""
        End Function

        ''' <summary>
        ''' データからJsonファイルを出力
        ''' </summary>
        ''' <param name="path"></param>
        Public Shared Sub Dump(path As String)

        End Sub
#End Region
    End Class
End Namespace