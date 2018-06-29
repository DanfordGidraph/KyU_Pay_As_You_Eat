Imports System.IO
Imports System.Data
Imports System.Windows.Forms

'This class handles all the elements found within the food items adding window.
Public Class FoodItemsAddingWindow
    'the variable NFPath is used to hold the path to an image that has been copied into images folder 
    Dim NFPath As String
    Dim databasePath As String = System.Windows.Forms.Application.StartupPath & "\KyUC_PAYE_Menu.db"
    'dim some global database manipulation variables

    Dim conStr As String = "Data Source=Meals_Menu_Database.db; Version=3;"
    Public selection As String

    Dim checkBoxCounter As Integer

    '====================================================================================================================================
    Public Sub HighlightMainDish()

        Me.mainDishColorZone.Foreground = Brushes.White
        Me.stewColorZone.Opacity = 0.3
        Me.snackColorZone.Opacity = 0.3
        Me.drinkColorZone.Opacity = 0.3

    End Sub
    '=====================================================================================================================================
    Public Sub HighlightStew()
        Me.mainDishColorZone.Opacity = 0.3
        Me.stewColorZone.Foreground = Brushes.White
        Me.snackColorZone.Opacity = 0.3
        Me.drinkColorZone.Opacity = 0.3

    End Sub
    '=====================================================================================================================================
    Public Sub HighlightSnack()
        Me.mainDishColorZone.Opacity = 0.3
        Me.stewColorZone.Opacity = 0.3
        Me.snackColorZone.Foreground = Brushes.White
        Me.drinkColorZone.Opacity = 0.3

    End Sub
    '=====================================================================================================================================
    Public Sub HighlightDrink()
        Me.mainDishColorZone.Opacity = 0.3
        Me.stewColorZone.Opacity = 0.3
        Me.snackColorZone.Opacity = 0.3
        Me.drinkColorZone.Foreground = Brushes.White

    End Sub

    '=====================================================================================================================================
    Private Sub uploadMainDishImageBtn_Click(sender As Object, e As RoutedEventArgs) Handles uploadFoodImage.Click

        Dim dlg As New Forms.OpenFileDialog
        dlg.Filter = "JPG Image|*.jpg|JPEG Image|*.jpeg|GIF Image|*.gif|PNG Image|*.png"
        dlg.Multiselect = False
        Dim result As DialogResult = dlg.ShowDialog()
        Dim oldpath As String = dlg.FileName
        Dim actualName As String = dlg.SafeFileName
        Dim AppData As String = System.Windows.Forms.Application.UserAppDataPath
        Dim Images As New DirectoryInfo(AppData & "\Images\")
        If Not Images.Exists Then
            Images.Create()
            MsgBox("Directory Created ")
        End If
        'pass values to the getImage function which will return the new path after copying the image to a new location
        getImage(Images, oldpath, actualName, result, NFPath)

    End Sub

    '=====================================================================================================================================================================
    'this function copies the image from the source to an appdata destination folder to allow the software access to its food items images unhindered
    Private Function getImage(dir As DirectoryInfo, oldPath As String, actualName As String, result As DialogResult, ByRef newFilePath As String) As String

        If result = Forms.DialogResult.OK Then
            Dim newpath As String = dir.ToString
            File.Copy(oldPath, newpath & actualName, True)
            AddFoodImage.Image = System.Drawing.Image.FromFile(newpath & actualName)
            newFilePath = newpath & actualName
        End If
        Return newFilePath
    End Function
    '=========================================================================================================================================================================
    ''' <summary>
    ''' The following function is explicitly for inserting new food Items into the database
    ''' it uses a readonly access level and is parametarized to prevent sql injection
    ''' </summary>
    ''' <param name="category"></param>
    Private Sub insertToDatabase(category As String)

        'create variables to hold values that you will insert into database
        'this variables get values from the text fields in the window depending on the value of the category
        Dim addFoodName As String = ""
        Dim addFoodPrice As Double = 0

        'Depending on what category value is passed assign the above variables values from the respective textfields on the open tab

        addFoodName = Me.addFoodNameTxtBx.Text
        addFoodPrice = Val(Me.addFoodPriceTxtBx.Text)

        'Get the values for the image path (for the current image on the Viewing Picturebox) from  and 
        Dim servingTime As String = checkStatus()
        Dim imagePath As String = NFPath
        'ensure passed image path is not empty
        Try
            If imagePath IsNot Nothing Then
                Dim con As New SQLite.SQLiteConnection(conStr)
                Dim command As New SQLite.SQLiteCommand
                With command
                    .CommandType = CommandType.Text
                    .Connection = con
                    .CommandText = "INSERT INTO MenuTable(FoodName,Category,Price,PicturePath,ServingTime) VALUES (@a,@b,@c,@d,@e)"
                    .Parameters.AddWithValue("@a", addFoodName)
                    .Parameters.AddWithValue("@b", category)
                    .Parameters.AddWithValue("@c", addFoodPrice)
                    .Parameters.AddWithValue("@d", imagePath)
                    .Parameters.AddWithValue("@e", servingTime)
                    Dim i As Integer = 0

                    con.Open()
                    i = .ExecuteNonQuery()
                    con.Close()
                    If (i > 0) Then
                        MsgBox("Successfully added")
                    End If
                End With

            Else
                MsgBox("Please add an Image first")
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    '=======================================================================================================================================
    Private Sub addFoodBtn_Click(sender As Object, e As RoutedEventArgs) Handles addFoodBtn.Click
        If addFoodNameTxtBx.Text IsNot Nothing And addFoodPriceTxtBx.Text IsNot Nothing And getCheckCount() > 0 Then
            insertToDatabase(selection)
            addFoodNameTxtBx.Clear()
            addFoodPriceTxtBx.Clear()
            addAsBreakfastChkBx.IsChecked = False
            addAsLunchChkBx.IsChecked = False
            addAsSupperChkBx.IsChecked = False
            AddFoodImage.Image = Nothing
        Else
            MsgBox("Please Fill In all Details", MsgBoxStyle.Information, Title:="Insuffecient Information")
        End If
        Dim loader As New MainWindow
        loader.callLoader()
    End Sub

    '=====================================================================================================================================    
    Private Sub attachImageGroupBox_MouseEnter(sender As Object, e As Input.MouseEventArgs) Handles attachImageGroupBox.MouseEnter
        uploadFoodImage.Opacity = 1
    End Sub
    '=====================================================================================================================================
    Private Sub attachImageGroupBox_MouseLeave(sender As Object, e As Input.MouseEventArgs) Handles attachImageGroupBox.MouseLeave
        uploadFoodImage.Opacity = 0.3
    End Sub
    '=====================================================================================================================================
    Private Function checkStatus() As String

        If Me.addAsBreakfastChkBx.IsChecked = True And addAsLunchChkBx.IsChecked = False And addAsSupperChkBx.IsChecked = False Then
            Return "Breakfast"
        ElseIf Me.addAsBreakfastChkBx.IsChecked = False And addAsLunchChkBx.IsChecked = True And addAsSupperChkBx.IsChecked = False Then
            Return "Lunch"
        ElseIf Me.addAsBreakfastChkBx.IsChecked = False And addAsLunchChkBx.IsChecked = False And addAsSupperChkBx.IsChecked = True Then
            Return "Supper"
        ElseIf Me.addAsBreakfastChkBx.IsChecked = True And addAsLunchChkBx.IsChecked = True And addAsSupperChkBx.IsChecked = True Then
            Return "BreakFast, Lunch and Supper"
        ElseIf Me.addAsBreakfastChkBx.IsChecked = False And addAsLunchChkBx.IsChecked = True And addAsSupperChkBx.IsChecked = True Then
            Return "Lunch and Supper"
        Else
            Dim exception As New Exception
            Return exception.Message.ToString
        End If

    End Function

    Private Function getCheckCount() As Integer
        Dim stat As String = checkStatus()
        If stat = "Breakfast" Or stat = "Lunch" Or stat = "Supper" Then
            checkBoxCounter = 1
            Return checkBoxCounter
        ElseIf stat = "BreakFast, Lunch and Supper" Then
            checkBoxCounter = 3
            Return checkBoxCounter
        ElseIf stat = "Lunch and Supper" Then
            checkBoxCounter = 2
            Return checkBoxCounter
        Else
            checkBoxCounter = 0
            Return checkBoxCounter
        End If
    End Function



    '=====================================================================================================================================
End Class
