Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Collections

Namespace EAD.Comparer
    Public Class BinaryComparer
        Implements IComparer
        ' Methods
        Public Function [Compare](ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
            Return String.CompareOrdinal(Conversions.ToString(x), Conversions.ToString(y))
        End Function

    End Class
End Namespace

