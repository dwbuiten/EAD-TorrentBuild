Imports EAD.Comparer
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Collections
Imports System.Collections.Specialized
Imports System.Reflection
Imports System.Runtime.CompilerServices

Namespace EAD.Torrent
    <DefaultMember("Value")> _
    Public Class TorrentDictionary
        Inherits NameObjectCollectionBase
        ' Methods
        Public Sub Add(ByVal key As String, ByVal value As Object)
            Me.BaseAdd(key, RuntimeHelpers.GetObjectValue(value))
        End Sub

        Public Function Contains(ByVal key As String) As Boolean
            Dim list As New ArrayList
            Dim str As String
            For Each str In Me.BaseGetAllKeys
                list.Add(str)
            Next
            Return list.Contains(key)
        End Function

        Public Sub GetConsole()
            Dim str As String
            Do While True
                Dim charCode As Integer = Console.Read
                If (charCode = -1) Then
                    Exit Do
                End If
                str = (str & Conversions.ToString(Strings.ChrW(charCode)))
            Loop
            Me.Parse(str)
        End Sub

        Public Function Parse(ByVal ThisTor As String) As TorrentDictionary
            Me.TorDict = ThisTor.Substring(1, (ThisTor.Length - 1))
            Do While (Me.TorDict.IndexOf("e") > 0)
                Dim str As New TorrentString
                Me.keyobj = RuntimeHelpers.GetObjectValue(str.Parse(Me.TorDict))
                Me.tkey = Conversions.ToString(NewLateBinding.LateGet(Me.keyobj, Nothing, "Value", New Object(0  - 1) {}, Nothing, Nothing, Nothing))
                Me.TorDict = str.PassString
                Dim str3 As String = Me.TorDict.Substring(0, 1)
                If (str3 = "d") Then
                    Dim dictionary2 As New TorrentDictionary
                    Me.Add(Me.tkey, dictionary2.Parse(Me.TorDict))
                    Me.TorDict = dictionary2.PassString
                Else
                    If (str3 = "i") Then
                        Dim number As New TorrentNumber
                        Me.Add(Me.tkey, RuntimeHelpers.GetObjectValue(number.Parse(Me.TorDict)))
                        Me.TorDict = number.PassString
                        Continue Do
                    End If
                    If (str3 = "l") Then
                        Dim list As New TorrentList
                        Me.Add(Me.tkey, list.Parse(Me.TorDict))
                        Me.TorDict = list.PassString
                        Continue Do
                    End If
                    Dim str2 As New TorrentString
                    Me.Add(Me.tkey, RuntimeHelpers.GetObjectValue(str2.Parse(Me.TorDict)))
                    Me.TorDict = str2.PassString
                End If
            Loop
            If (Me.TorDict.Length <> 0) Then
                Me.TorDict = Me.TorDict.Substring(1, (Me.TorDict.Length - 1))
            End If
            Return Me
        End Function

        Public Sub Remove(ByVal key As String)
            Me.BaseRemove(key)
        End Sub


        ' Properties
        Public ReadOnly Property BEncoded As String
            Get
                Dim enumerator As IEnumerator
                Dim left As String = "d"
                Dim list As New ArrayList
                Console.WriteLine("Getting Keys")
                Dim str3 As String
                For Each str3 In Me.BaseGetAllKeys
                    list.Add(str3)
                Next
                Console.WriteLine("Sorting Keys")
                Dim obj2 As Object = New BinaryComparer
                list.Sort(DirectCast(obj2, IComparer))
                Console.WriteLine("Generating Dictionary")
                Try 
                    enumerator = list.GetEnumerator
                    Do While enumerator.MoveNext
                        Dim str4 As String = Conversions.ToString(enumerator.Current)
                        Console.WriteLine(("Dictionary Key: " & str4))
                        Dim str5 As New TorrentString With { _
                            .Value = str4 _
                        }
                        Console.WriteLine("Key Encoded")
                        left = (left & str5.BEncoded)
                        Console.WriteLine("Encoding conversion")
                        Console.WriteLine(("Encoding Member of type: " & Me.Value(str4).GetType.ToString))
                        left = Conversions.ToString(Operators.ConcatenateObject(left, NewLateBinding.LateGet(Me.Value(str4), Nothing, "BEncoded", New Object(0  - 1) {}, Nothing, Nothing, Nothing)))
                        Console.WriteLine("Generated Encoded String")
                        str5 = Nothing
                        Console.WriteLine("Reset key name")
                    Loop
                Finally
                    If TypeOf enumerator Is IDisposable Then
                        TryCast(enumerator,IDisposable).Dispose
                    End If
                End Try
                Console.WriteLine("Outputting Dictionary")
                Return (left & "e")
            End Get
        End Property

        Public ReadOnly Property Keys As ICollection
            Get
                Return Me.Keys
            End Get
        End Property

        Public ReadOnly Property PassString As String
            Get
                Return Me.TorDict
            End Get
        End Property

        Public Default Property Value(ByVal key As String) As Object
            Get
                Return Me.BaseGet(key)
            End Get
            Set(ByVal Value As Object)
                Me.BaseSet(key, RuntimeHelpers.GetObjectValue(Value))
            End Set
        End Property

        Public ReadOnly Property Values As ICollection
            Get
                Return Me.BaseGetAllValues
            End Get
        End Property


        ' Fields
        Private keyobj As Object
        Private tkey As String
        Private TorDict As String
    End Class
End Namespace

