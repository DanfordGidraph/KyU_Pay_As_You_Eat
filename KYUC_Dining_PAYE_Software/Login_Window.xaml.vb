Imports System.Data.SQLite
Imports System.Data
Public Class Login_Window
    Dim conStr As String = "Data Source = Users_Info.db; Version=3;"
    Dim con As New SQLiteConnection(conStr)

    Private Sub CloseLoginWindowBtn_Click(sender As Object, e As RoutedEventArgs) Handles CloseLoginWindowBtn.Click
        Dim quitingAsurance As New Quiting_Asurance
        quitingAsurance.Owner = Me
        quitingAsurance.ShowDialog()
    End Sub
    Public Sub closingDecision()
        clearToExit()
    End Sub
    Private Sub clearToExit()
        Me.Close()
    End Sub

    Private Sub ButtonONE_Click(sender As Object, e As RoutedEventArgs) Handles ButtonONE.Click
        PasscodeTxtBx.Password = PasscodeTxtBx.Password & "1"
    End Sub

    Private Sub ButtonTWO_Click(sender As Object, e As RoutedEventArgs) Handles ButtonTWO.Click
        PasscodeTxtBx.Password = PasscodeTxtBx.Password & "2"
    End Sub

    Private Sub ButtonTHREE_Click(sender As Object, e As RoutedEventArgs) Handles ButtonTHREE.Click
        PasscodeTxtBx.Password = PasscodeTxtBx.Password & "3"

    End Sub

    Private Sub ButtonFOUR_Click(sender As Object, e As RoutedEventArgs) Handles ButtonFOUR.Click
        PasscodeTxtBx.Password = PasscodeTxtBx.Password & "4"

    End Sub

    Private Sub ButtonFIVE_Click(sender As Object, e As RoutedEventArgs) Handles ButtonFIVE.Click
        PasscodeTxtBx.Password = PasscodeTxtBx.Password & "5"

    End Sub

    Private Sub ButtonSIX_Click(sender As Object, e As RoutedEventArgs) Handles ButtonSIX.Click
        PasscodeTxtBx.Password = PasscodeTxtBx.Password & "6"

    End Sub

    Private Sub ButtonSEVEN_Click(sender As Object, e As RoutedEventArgs) Handles ButtonSEVEN.Click
        PasscodeTxtBx.Password = PasscodeTxtBx.Password & "7"

    End Sub

    Private Sub ButtonEIGHT_Click(sender As Object, e As RoutedEventArgs) Handles ButtonEIGHT.Click
        PasscodeTxtBx.Password = PasscodeTxtBx.Password & "8"

    End Sub

    Private Sub ButtonNINE_Click(sender As Object, e As RoutedEventArgs) Handles ButtonNINE.Click
        PasscodeTxtBx.Password = PasscodeTxtBx.Password & "9"

    End Sub

    Private Sub ButtonZERO_Click(sender As Object, e As RoutedEventArgs) Handles ButtonZERO.Click
        PasscodeTxtBx.Password = PasscodeTxtBx.Password & "0"

    End Sub

    Private Sub ButtonBACKSPACE_Click(sender As Object, e As RoutedEventArgs) Handles ButtonBACKSPACE.Click
        Dim passText As String = PasscodeTxtBx.Password
        Dim int As Integer = 0
        If passText.Length > 0 Then
            int = passText.Length
            PasscodeTxtBx.Password = passText.Remove(int - 1, 1)
        End If
    End Sub

    Private Sub cancelLoginBtn_Click(sender As Object, e As RoutedEventArgs) Handles cancelLoginBtn.Click
        PasscodeTxtBx.Password = String.Empty
    End Sub

    Private Sub loginBtn_Click(sender As Object, e As RoutedEventArgs) Handles loginBtn.Click
        Dim pass As String = PasscodeTxtBx.Password
        'If pass.Length < 7 Then
        '    Dim tip As New ToolTip
        '    PasscodeTxtBx.ToolTip = "Passcode Incorrect"

        'Else
        Dim sqlQuery As String = "SELECT SirName FROM User_Login_Data WHERE ID_Number = '" & pass & "'"
            Dim dataT As New DataTable
            Dim adapter As New SQLiteDataAdapter(sqlQuery, con)
            dataT.Rows.Clear()
            adapter.Fill(dataT)

            Dim loader As New MainWindow
            loader.callLoader()

            Dim username As String = dataT.Rows(0)(0).ToString
            If dataT.Rows.Count > 0 Then
                MsgBox("Welcome " & username, MsgBoxStyle.Information, Title:="Login Success")
                Dim main As New MainWindow
                main.getServerName(username)
                Me.Hide()
            End If
        'End If

    End Sub
End Class
