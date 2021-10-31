Imports System.Collections.ObjectModel
Imports System.Text.RegularExpressions
Namespace Utils.ArgParse

    Public Class ArgumentParser

        Private Class Argument
            Public ReadOnly Property name As String
            Protected ReadOnly Property _help As String
            Public ReadOnly Property type As Type
            Public Overridable ReadOnly Property help As String
                Get
                    Return $"{name}  {_help}"
                End Get
            End Property
            Public Sub New(ByVal name As String, Optional ByVal help As String = "", Optional ByVal type As Type = Nothing)
                Me.name = name
                Me._help = help
                Me.type = If(type Is Nothing, GetType(String), type)
            End Sub

        End Class

        Private Class OptionalArgument
            Inherits Argument

            Private Shared ReadOnly rxOptArgName As New Regex("^-{1}[^-].*", RegexOptions.Compiled)
            Private Shared ReadOnly rxOptArgFullName As New Regex("^-{2}[^-].*", RegexOptions.Compiled)
            Public Shared Function IsName(ByVal argName As String) As Boolean
                Return rxOptArgName.IsMatch(argName)
            End Function
            Public Shared Function IsFullName(ByVal argFullName As String) As Boolean
                Return rxOptArgFullName.IsMatch(argFullName)
            End Function


            Public Overrides ReadOnly Property help As String
                Get
                    If isNameOnly Then
                        Return MyBase.help
                    ElseIf isFullNameOnly Then
                        Return $"{fullName}  {_help}"
                    End If
                    Return $"{name}, {fullName}  {_help}"
                End Get
            End Property
            Public ReadOnly Property isFullNameOnly As Boolean
                Get
                    Return Not String.IsNullOrEmpty(fullName) AndAlso String.IsNullOrEmpty(name)
                End Get
            End Property
            Public ReadOnly Property isNameOnly As Boolean
                Get
                    Return Not String.IsNullOrEmpty(name) AndAlso String.IsNullOrEmpty(fullName)
                End Get
            End Property
            Public ReadOnly Property fullName As String
            Public Sub New(ByVal name As String, ByVal fullName As String, Optional ByVal help As String = "", Optional ByVal type As Type = Nothing)
                MyBase.New(name, help, type)
                Me.fullName = fullName
            End Sub
        End Class

        Private _args As ReadOnlyCollection(Of String)
        Private _definePositionalArgs As List(Of Argument)
        Private _defineOptionalArgs As List(Of OptionalArgument)
        Private _description As String
        Private ReadOnly Property _helpPositional As String
            Get
                Return _BuildHelpString("positional", _definePositionalArgs)
            End Get
        End Property
        Private ReadOnly Property _helpOptional As String
            Get
                Return _BuildHelpString("optional", _defineOptionalArgs)
            End Get
        End Property

        Public Sub New(Optional ByVal description As String = "")
            Me._description = description
            _args = New ReadOnlyCollection(Of String)(Environment.GetCommandLineArgs())
            _definePositionalArgs = New List(Of Argument)
            _defineOptionalArgs = New List(Of OptionalArgument)(New OptionalArgument() {New OptionalArgument("-h", "--help", help:="show this help message and exit")})
        End Sub

        Private Function _BuildHelpString(ByVal argsType As String, ByVal args As IEnumerable(Of Argument)) As String
            If args.Count > 0 Then
                Dim sb As New Text.StringBuilder()
                sb.AppendLine($"{argsType} arguments: ")
                For Each arg As Argument In args
                    sb.AppendLine($"  {arg.help}")
                Next
                Return sb.ToString()
            End If
            Return ""
        End Function
        Private Sub _ShowHelp()
            Console.WriteLine($"usage: {System.Reflection.Assembly.GetEntryAssembly().GetName().Name} [-h]")
            If Not String.IsNullOrEmpty(_description) Then Console.WriteLine($"{vbCrLf}{_description}")
            Console.WriteLine()
            If _definePositionalArgs.Count > 0 Then Console.WriteLine(_helpPositional)
            If _defineOptionalArgs.Count > 0 Then Console.WriteLine(_helpOptional)
        End Sub

        Public Sub AddArgument(ByVal argName As String, ByVal help As String)
            If Not argName.StartsWith("-"c) Then
                If (From a As Argument In _definePositionalArgs Where a.name = argName).Count > 0 Then
                    Throw New Exception($"argparse.ArgumentError: argument {argName}: conflicting position string: {argName}")
                Else
                    _definePositionalArgs.Add(New Argument(argName, help))
                End If
                ' 位置引数
            ElseIf OptionalArgument.IsName(argName) Then
                ' 省略のみオプション引数
                If (From a As OptionalArgument In _defineOptionalArgs Where a.name = argName).Count > 0 Then
                    Throw New Exception($"argparse.ArgumentError: argument {argName}: conflicting option string: {argName}")
                Else
                    _defineOptionalArgs.Add(New OptionalArgument(argName, "", help))
                End If
            ElseIf OptionalArgument.IsFullName(argName) Then
                ' 省略無しオプション引数
                If (From a As OptionalArgument In _defineOptionalArgs Where a.fullName = argName).Count > 0 Then
                    Throw New Exception($"argparse.ArgumentError: argument {argName}: conflicting option string: {argName}")
                Else
                    _defineOptionalArgs.Add(New OptionalArgument("", argName, help))
                End If
            Else
                Throw New Exception($"ValueError: invalid option string '{argName}': must start with a character '-' or characters ‘--’")
            End If
        End Sub
        Public Sub AddArgument(ByVal argName1 As String, ByVal argName2 As String, ByVal help As String)
            If (OptionalArgument.IsName(argName1) Xor OptionalArgument.IsName(argName2)) AndAlso (OptionalArgument.IsFullName(argName1) Xor OptionalArgument.IsFullName(argName2)) Then
                Dim argName As String = If(OptionalArgument.IsName(argName1), argName1, argName2)
                Dim argFullName As String = If(OptionalArgument.IsFullName(argName1), argName1, argName2)

                If (From a As OptionalArgument In _defineOptionalArgs Where a.name = argName AndAlso a.fullName = argFullName).Count > 0 Then
                    Throw New Exception($"argparse.ArgumentError: argument {argName}: conflicting option string: {argName}")
                Else
                    _defineOptionalArgs.Add(New OptionalArgument(argName, argFullName, help))
                End If

            ElseIf OptionalArgument.IsName(argName1) Xor OptionalArgument.IsName(argName2) Then
                Throw New Exception($"ValueError: invalid option string '{If(argName1.StartsWith("-"c), argName2, argName1)}': must start with characters '--'")

            ElseIf OptionalArgument.IsFullName(argName1) Xor OptionalArgument.IsFullName(argName2) Then
                Throw New Exception($"ValueError: invalid option string '{If(argName1.StartsWith("--"), argName2, argName1)}': must start with a character '-'")

            ElseIf OptionalArgument.IsName(argName1) AndAlso OptionalArgument.IsName(argName2) Then
                Throw New Exception($"ValueError: invalid option string '{argName1}', '{argName2}': either '{argName1}' or '{argName2}' must start with characters '--'")

            ElseIf OptionalArgument.IsFullName(argName1) AndAlso OptionalArgument.IsFullName(argName2) Then
                Throw New Exception($"ValueError: invalid option string '{argName1}', '{argName2}': either '{argName1}' or '{argName2}' must start with a character '-'")

            Else
                Throw New Exception($"ValueError: invalid option string '{argName1}', '{argName2}': either '{argName1}' or '{argName2}' must start with a character '-' and one with characters ‘--’")
            End If
        End Sub


        Public Function ParseArgs() As ArgumentNamespace
            If _args.IndexOf("-h") <> -1 OrElse _args.IndexOf("--help") <> -1 Then
                _ShowHelp()
                Environment.Exit(0)
            End If

            For Each _posArg As Argument In _definePositionalArgs

            Next



            Return New ArgumentNamespace()
        End Function
    End Class

    Public Class ArgumentNamespace

    End Class
End Namespace

