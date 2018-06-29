Public Class Quiting_Asurance

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub

    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        Dim myOwner As Login_Window = Me.Owner
        myOwner.closingDecision()
        Me.Close()
    End Sub
End Class
