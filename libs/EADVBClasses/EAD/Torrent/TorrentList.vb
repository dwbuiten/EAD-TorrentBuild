Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Collections
Imports System.Reflection
Imports System.Runtime.CompilerServices

Namespace EAD.Torrent
    <DefaultMember("Parse")> _
    Public Class TorrentList
        Inherits CollectionBase
        ' Properties
        Public ReadOnly Property BEncoded As String
            Get
                Dim enumerator As IEnumerator
                Dim left As String = "l"
                Try 
                    enumerator = Me.InnerList.GetEnumerator
                    Do While enumerator.MoveNext
                        Dim objectValue As Object = RuntimeHelpers.GetObjectValue(enumerator.Current)
                        left = Conversions.ToString(Operators.ConcatenateObject(left, NewLateBinding.LateGet(objectValue, Nothing, "BEncoded", New Object(0  - 1) {}, Nothing, Nothing, Nothing)))
                    Loop
                Finally
                    If TypeOf enumerator Is IDisposable Then
                        TryCast(enumerator,IDisposable).Dispose
                    End If
                End Try
                Return (left & "e")
            End Get
        End Property

        Public ReadOnly Default Property Parse(ByVal d As String) As TorrentList
            Get
                d = d.Substring(1, (d.Length - 1))
                Do While (d.IndexOf("e") <> 0)
                    Dim str2 As String = d.Substring(0, 1)
                    If (str2 = "d") Then
                        Dim dictionary As New TorrentDictionary
                        Me.InnerList.Add(dictionary.Parse(d))
                        Me.thispassstring = dictionary.PassString
                        d = Me.thispassstring
                    Else
                        If (str2 = "l") Then
                            Dim list2 As New TorrentList
                            Me.InnerList.Add(list2.Parse(d))
                            Me.thispassstring = list2.PassString
                            d = Me.thispassstring
                            Continue Do
                        End If
                        If (str2 = "i") Then
                            Dim number As New TorrentNumber
                            Me.InnerList.Add(RuntimeHelpers.GetObjectValue(number.Parse(d)))
                            d = number.PassString
                            Me.thispassstring = d
                            Continue Do
                        End If
                        Dim str As New TorrentString
                        Me.InnerList.Add(RuntimeHelpers.GetObjectValue(str.Parse(d)))
                        d = str.PassString
                        Me.thispassstring = d
                    End If
                Loop
                Me.thispassstring = Me.PassString.Substring(1, (Me.PassString.Length - 1))
                d = Me.thispassstring
                Return Me
            End Get
        End Property

        Public ReadOnly Property PassString As String
            Get
                Return Me.thispassstring
            End Get
        End Property

        Public Property Value As ArrayList
            Get
                Return Me.InnerList
            End Get
            Set(ByVal Value As ArrayList)
                Dim enumerator As IEnumerator
                Me.InnerList.Clear
                Try 
                    enumerator = Value.GetEnumerator
                    Do While enumerator.MoveNext
                        Dim objectValue As Object = RuntimeHelpers.GetObjectValue(enumerator.Current)
                        Me.InnerList.Add(RuntimeHelpers.GetObjectValue(objectValue))
                    Loop
                Finally
                    If TypeOf enumerator Is IDisposable Then
                        TryCast(enumerator,IDisposable).Dispose
                    End If
                End Try
            End Set
        End Property


        ' Fields
        Private thispassstring As String
    End Class
End Namespace

