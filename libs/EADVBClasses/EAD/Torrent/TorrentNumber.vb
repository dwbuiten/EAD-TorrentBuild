Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Reflection

Namespace EAD.Torrent
    <DefaultMember("Parse")> _
    Public Class TorrentNumber
        ' Properties
        Public ReadOnly Property BEncoded As String
            Get
                Return ("i" & Conversions.ToString(Me.thisNumber) & "e")
            End Get
        End Property

        Public ReadOnly Default Property Parse(ByVal d As String) As Object
            Get
                Me.thisd = d
                Me.thisNumber = Convert.ToInt64(Decimal.Parse(d.Substring((d.IndexOf("i") + 1), ((d.IndexOf("e") - d.IndexOf("i")) - 1))))
                Return Me
            End Get
        End Property

        Public ReadOnly Property PassString As String
            Get
                Return Me.thisd.Substring((Me.thisd.IndexOf("e") + 1), (Me.thisd.Length - (Me.thisd.IndexOf("e") + 1)))
            End Get
        End Property

        Public Property Value As Long
            Get
                Return Me.thisNumber
            End Get
            Set(ByVal Value As Long)
                Me.thisNumber = Value
            End Set
        End Property


        ' Fields
        Private thisd As String
        Private thisNumber As Long
    End Class
End Namespace

