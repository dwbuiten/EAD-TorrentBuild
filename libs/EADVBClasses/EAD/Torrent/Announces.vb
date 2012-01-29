Imports EAD.VisualBasic
Imports Microsoft.VisualBasic
Imports System

Namespace EAD.Torrent
    Public Class Announces
        Inherits EAD.VisualBasic.Constants
        ' Methods
        Public Shared Function HTTPToUDPAnnounce(ByVal AnnounceURL As String, ByVal ProtocolType As Integer) As String
            Return ("udp://" & Strings.Right(AnnounceURL, (Strings.Len(AnnounceURL) - ProtocolType)))
        End Function

        Public Shared Function IsValidAnnounce(ByVal AnnounceURL As String) As Integer
            If (Strings.Left(AnnounceURL, 6) = "udp://") Then
                Return 6
            End If
            If (Strings.Left(AnnounceURL, 7) = "http://") Then
                Return 7
            End If
            If (Strings.Left(AnnounceURL, 8) = "https://") Then
                Return 8
            End If
            Return 0
        End Function

        Public Shared Function IsValidHTTPAnnounce(ByVal AnnounceURL As String) As Integer
            If (Strings.Left(AnnounceURL, 7) = "http://") Then
                Return 7
            End If
            If (Strings.Left(AnnounceURL, 8) = "https://") Then
                Return 8
            End If
            Return 0
        End Function

    End Class
End Namespace

