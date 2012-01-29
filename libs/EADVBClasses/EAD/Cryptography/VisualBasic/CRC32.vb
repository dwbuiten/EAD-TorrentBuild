Imports System
Imports System.IO

Namespace EAD.Cryptography.VisualBasic
    Public Class CRC32
        ' Methods
        Public Sub New()
            Dim num2 As Integer = -306674912
            Me.crc32Table = New Integer(&H101  - 1) {}
            Dim index As Integer = 0
            Do
                Dim num As Integer = index
                Dim num4 As Integer = 8
                Do
                    If ((num And 1) > 0) Then
                        num = CInt(((CLng((num And -2)) / 2) And &H7FFFFFFF))
                        num = (num Xor num2)
                    Else
                        num = CInt(((CLng((num And -2)) / 2) And &H7FFFFFFF))
                    End If
                    num4 = (num4 + -1)
                Loop While (num4 >= 1)
                Me.crc32Table(index) = num
                index += 1
            Loop While (index <= &HFF)
        End Sub

        Public Function GetCrc32(ByRef stream As Stream) As Integer
            Dim num2 As Integer = -1
            Dim buffer As Byte() = New Byte(&H401  - 1) {}
            Dim count As Integer = &H400
            Dim i As Integer = stream.Read(buffer, 0, count)
            Do While (i > 0)
                Dim num8 As Integer = (i - 1)
                Dim j As Integer = 0
                Do While (j <= num8)
                    Dim index As Integer = ((num2 And &HFF) Xor buffer(j))
                    num2 = (((num2 And -256) / &H100) And &HFFFFFF)
                    num2 = (num2 Xor Me.crc32Table(index))
                    j += 1
                Loop
                i = stream.Read(buffer, 0, count)
            Loop
            Return Not num2
        End Function


        ' Fields
        Private Const BUFFER_SIZE As Integer = &H400
        Private crc32Table As Integer()
    End Class
End Namespace

