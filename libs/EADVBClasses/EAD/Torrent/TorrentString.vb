Imports Microsoft.VisualBasic.CompilerServices
Imports System

Namespace EAD.Torrent
    Public Class TorrentString
        ' Properties
        Public ReadOnly Property BEncoded As String
            Get
                Return (Conversions.ToString(Me.thisvalue.Length) & ":" & Me.thisvalue)
            End Get
        End Property

        Public ReadOnly Property Parse(ByVal d As String) As Object
            Get
                Me.thisheaderlength = (d.IndexOf(":") + 1)
                Me.thisstringlength = Convert.ToInt32(Decimal.Parse(d.Substring(0, d.IndexOf(":"))))
                Me.thisvalue = d.Substring(Me.thisheaderlength, Me.thisstringlength)
                Me.ThisPassString = d.Substring((Me.thisheaderlength + Me.thisstringlength), (d.Length - (Me.thisheaderlength + Me.thisstringlength)))
                Return Me
            End Get
        End Property

        Public ReadOnly Property PassString As String
            Get
                Return Me.ThisPassString
            End Get
        End Property

        Public Property Value As String
            Get
                Return Me.thisvalue
            End Get
            Set(ByVal Value As String)
                Me.thisvalue = Value
            End Set
        End Property


        ' Fields
        Private thisd As String
        Private thisheaderlength As Integer
        Private ThisPassString As String
        Private thisstringlength As Integer
        Private thisvalue As String
    End Class
End Namespace

