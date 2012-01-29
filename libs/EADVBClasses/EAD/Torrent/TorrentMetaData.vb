Imports Microsoft.VisualBasic.CompilerServices
Imports System

Namespace EAD.Torrent
    Public Class TorrentMetaData
        ' Properties
        Public Property Announce As String
            Get
                Return Conversions.ToString(NewLateBinding.LateGet(Me.thisTorrent.Value("announce"), Nothing, "value", New Object(0  - 1) {}, Nothing, Nothing, Nothing))
            End Get
            Set(ByVal Value As String)
                NewLateBinding.LateSetComplex(Me.thisTorrent.Value("announce"), Nothing, "value", New Object() { Value }, Nothing, Nothing, False, True)
            End Set
        End Property

        Public Property Comment As String
            Get
                If Me.thisTorrent.Contains("comment") Then
                    Return Conversions.ToString(NewLateBinding.LateGet(Me.thisTorrent.Value("comment"), Nothing, "value", New Object(0  - 1) {}, Nothing, Nothing, Nothing))
                End If
                Return ""
            End Get
            Set(ByVal Value As String)
                If Me.thisTorrent.Contains("comment") Then
                    NewLateBinding.LateSetComplex(Me.thisTorrent.Value("comment"), Nothing, "value", New Object() { Value }, Nothing, Nothing, False, True)
                Else
                    Dim str As New TorrentString With { _
                        .Value = Value _
                    }
                    Me.thisTorrent.Add("comment", str)
                End If
            End Set
        End Property

        Public Property CreatedBy As String
            Get
                If Me.thisTorrent.Contains("created by") Then
                    Return Conversions.ToString(NewLateBinding.LateGet(Me.thisTorrent.Value("created by"), Nothing, "value", New Object(0  - 1) {}, Nothing, Nothing, Nothing))
                End If
                Return ""
            End Get
            Set(ByVal Value As String)
                If Me.thisTorrent.Contains("created by") Then
                    NewLateBinding.LateSetComplex(Me.thisTorrent.Value("created by"), Nothing, "value", New Object() { Value }, Nothing, Nothing, False, True)
                Else
                    Dim str As New TorrentString With { _
                        .Value = Value _
                    }
                    Me.thisTorrent.Add("created by", str)
                End If
            End Set
        End Property

        Public ReadOnly Property MultiFile As Boolean
            Get
                If Conversions.ToBoolean(NewLateBinding.LateGet(Me.thisTorrent.Value("info"), Nothing, "Contains", New Object() { "length" }, Nothing, Nothing, Nothing)) Then
                    Return False
                End If
                Return True
            End Get
        End Property

        Public ReadOnly Property SingleFile As Boolean
            Get
                If Conversions.ToBoolean(NewLateBinding.LateGet(Me.thisTorrent.Value("info"), Nothing, "Contains", New Object() { "files" }, Nothing, Nothing, Nothing)) Then
                    Return False
                End If
                Return True
            End Get
        End Property

        Public Property Torrent As TorrentDictionary
            Get
                Return Me.thisTorrent
            End Get
            Set(ByVal Value As TorrentDictionary)
                Me.thisTorrent = Value
            End Set
        End Property


        ' Fields
        Private thisTorrent As TorrentDictionary
    End Class
End Namespace

