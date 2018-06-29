
Imports Microsoft.VisualBasic
Imports System.Windows
Namespace KYUC_Dining_PAYE_Software

    Partial Public Class App
        Private Sub App_Startup(ByVal sender As Object, ByVal e As StartupEventArgs)
            Dim main As New MainWindow
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose
            Application.Current.MainWindow = main
            Dim login As New Login_Window
            login.ShowDialog()
            main.Show()
        End Sub

    End Class
End Namespace

