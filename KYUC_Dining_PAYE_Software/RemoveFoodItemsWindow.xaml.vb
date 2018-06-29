Imports System.Data
Imports System.IO
Imports System.Windows.Forms
Imports System.Data.SQLite
''' <summary>
''' This class is the handler for all elements contained within the remove foods window. 
''' </summary>
Public Class RemoveFoodItemsWindow
    Dim conStr As String = "Data Source=Meals_Menu_Database.db; Version=3;"
    Dim connection As New SQLiteConnection(conStr)

    '=====================================================================================================================================
    Private Sub findDupes(listB As ListBox)
        For i As Int16 = 0 To listB.Items.Count - 2
            For j As Int16 = listB.Items.Count - 1 To i + 1 Step -1
                If listB.Items(i).ToString.ToLower = listB.Items(j).ToString.ToLower Then
                    listB.Items.RemoveAt(j)
                End If
            Next
        Next
    End Sub
    Public Sub fillMainDishListBox()
        'create a dataset from the menu database that is filled by the items in the maindish category of the Menu Table
        Dim ds As New DataSet
        Dim adapt As New SQLite.SQLiteDataAdapter("SELECT FoodName FROM MenuTable WHERE Category = 'Main Dish'", connection)
        adapt.Fill(ds)
        MainDishTab.Width = Me.Width - 30
        StewsTab.Width = 1
        DrinksTab.Width = 1
        SnacksTab.Width = 1
        'add each new row from the database to the listbox as a listbox item
        For Each row As DataRow In ds.Tables(0).Rows
            Dim lbItem As New ListBoxItem
            lbItem.Content = row(0).ToString
            If Not (MainDishRemoverListBox.Items.Contains(lbItem)) Then
                MainDishRemoverListBox.Items.Add(lbItem)
            End If
        Next

    End Sub

    '=====================================================================================================================================
    Public Sub fillStewListBox()
        'create a dataset from the menu database that is filled by the items in the stew category of the Menu Table
        Dim ds As New DataSet
        Dim adapt As New SQLite.SQLiteDataAdapter("SELECT FoodName FROM MenuTable WHERE Category = 'Stew'", connection)
        adapt.Fill(ds)
        StewsTab.IsSelected = True
        MainDishTab.Width = 1
        StewsTab.Width = Me.Width - 30
        DrinksTab.Width = 1
        SnacksTab.Width = 1
        'add each new row from the database to the listbox as a listbox item

        For Each row As DataRow In ds.Tables(0).Rows
            Dim lbItem As New ListBoxItem
            lbItem.Content = row(0).ToString
            If Not StewRemoverListBox.Items.Contains(lbItem) Then
                StewRemoverListBox.Items.Add(lbItem)
            End If
        Next

    End Sub
    '=====================================================================================================================================

    Public Sub fillSnackListBox()
        'create a dataset from the menu database that is filled by the items in the snacks category of the Menu Table

        Dim ds As New DataSet
        Dim adapt As New SQLite.SQLiteDataAdapter("SELECT FoodName FROM MenuTable WHERE Category = 'Snack'", connection)
        adapt.Fill(ds)
        SnacksTab.IsSelected = True
        MainDishTab.Width = 1
        StewsTab.Width = 1
        DrinksTab.Width = 1
        SnacksTab.Width = Me.Width - 30

        For Each row As DataRow In ds.Tables(0).Rows
            Dim lbItem As New ListBoxItem
            lbItem.Content = row(0).ToString
            If Not (SnackRemoverListBox.Items.Contains(lbItem)) Then
                SnackRemoverListBox.Items.Add(lbItem)
            End If
        Next

    End Sub
    '=====================================================================================================================================
    Public Sub fillDrinkListBox()
        'create a dataset from the menu database that is filled by the items in the drink category of the Menu Table

        Dim ds As New DataSet
        Dim adapt As New SQLite.SQLiteDataAdapter("SELECT FoodName FROM MenuTable WHERE Category = 'Drink'", connection)
        adapt.Fill(ds)
        DrinksTab.IsSelected = True
        MainDishTab.Width = 1
        StewsTab.Width = 1
        DrinksTab.Width = Me.Width - 30
        SnacksTab.Width = 1
        'add each new row from the database to the listbox as a listbox item

        For Each row As DataRow In ds.Tables(0).Rows
            Dim lbItem As New ListBoxItem
            lbItem.Content = row(0).ToString
            If Not (DrinkRemoverListBox.Items.Contains(lbItem)) Then
                DrinkRemoverListBox.Items.Add(lbItem)
            End If
        Next

        For i As Int16 = 0 To DrinkRemoverListBox.Items.Count - 1
            For j As Int16 = DrinkRemoverListBox.Items.Count - 1 To i + 1 Step -1
                If DrinkRemoverListBox.Items(i).ToString.ToLower = DrinkRemoverListBox.Items(j).ToString.ToLower Then
                    DrinkRemoverListBox.Items.RemoveAt(j)
                End If
            Next
        Next
    End Sub
    '=====================================================================================================================================
    Private Sub MainDishRemoverListBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles MainDishRemoverListBox.SelectionChanged
        Try
            Dim ds As New DataSet
            Dim foodName As String = MainDishRemoverListBox.SelectedValue.Content.ToString()
            Dim da As New SQLite.SQLiteDataAdapter("SELECT * FROM MenuTable WHERE FoodName = '" & foodName & "';", connection)
            da.Fill(ds)
            removeMainDishPriceTxtBx.Text = ds.Tables(0).Rows(0)("Price").ToString & "/="
            Dim picturePath As String = ds.Tables(0).Rows(0)("PicturePath").ToString
            removeMainDishFoodNameTxtBx.Text = foodName
            selectedMainDishImage.Image = System.Drawing.Image.FromFile(picturePath)
        Catch ex As Exception
            MsgBox(ex.Message)

        End Try
    End Sub

    Private Sub StewRemoverListBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles StewRemoverListBox.SelectionChanged
        Try
            Dim ds As New DataSet
            Dim foodName As String = StewRemoverListBox.SelectedValue.Content.ToString()

            Dim da As New SQLite.SQLiteDataAdapter("SELECT * FROM MenuTable WHERE FoodName = '" & foodName & "';", connection)
            da.Fill(ds)
            removeStewPriceTxtBx.Text = ds.Tables(0).Rows(0)("Price").ToString & "/="
            Dim picturePath As String = ds.Tables(0).Rows(0)("PicturePath").ToString
            removeStewFoodNameTxtBx.Text = foodName
            selectedStewImage.Image = System.Drawing.Image.FromFile(picturePath)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub SnackRemoverListBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles SnackRemoverListBox.SelectionChanged
        Try
            Dim ds As New DataSet
            Dim foodName As String = SnackRemoverListBox.SelectedValue.Content.ToString()
            Dim da As New SQLite.SQLiteDataAdapter("SELECT * FROM MenuTable WHERE FoodName = '" & foodName & "';", connection)
            da.Fill(ds)
            removeSnackPriceTxtBx.Text = ds.Tables(0).Rows(0)("Price").ToString & "/="
            Dim picturePath As String = ds.Tables(0).Rows(0)("PicturePath").ToString
            removeSnackFoodNameTxtBx.Text = foodName
            selectedSnackImage.Image = System.Drawing.Image.FromFile(picturePath)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub DrinkRemoverListBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles DrinkRemoverListBox.SelectionChanged
        Try
            Dim ds As New DataSet
            Dim foodName As String = DrinkRemoverListBox.SelectedValue.Content.ToString()
            Dim da As New SQLite.SQLiteDataAdapter("SELECT * FROM MenuTable WHERE FoodName = '" & foodName & "';", connection)
            da.Fill(ds)
            removeDrinkPriceTxtBx.Text = ds.Tables(0).Rows(0)("Price").ToString & "/="
            Dim picturePath As String = ds.Tables(0).Rows(0)("PicturePath").ToString
            removeDrinkFoodNameTxtBx.Text = foodName
            selectedDrinkImage.Image = System.Drawing.Image.FromFile(picturePath)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub


    Private Sub RemoveMainDishItemBtn_Click(sender As Object, e As RoutedEventArgs) Handles RemoveMainDishItemBtn.Click
        Dim foodname As String = Me.removeMainDishFoodNameTxtBx.Text
        Dim delQuerry As String = "DELETE FROM MenuTable WHERE FoodName = '" & foodname & "'"
        Dim command As New SQLiteCommand(delQuerry, connection)
        Dim i As Integer = 0
        connection.Open()
        i = command.ExecuteNonQuery()
        connection.Close()

        If i > 0 Then
            removeMainDishFoodNameTxtBx.Clear()
            removeMainDishPriceTxtBx.Clear()
            selectedMainDishImage.Image = Nothing
            fillMainDishListBox()
            Dim main As New MainWindow
            main.callLoader()
            MsgBox("Deleted Successfully")
        End If
    End Sub

    Private Sub RemoveStewItemBtn_Click(sender As Object, e As RoutedEventArgs) Handles RemoveStewItemBtn.Click
        Dim foodname As String = Me.removeStewFoodNameTxtBx.Text
        Dim delQuerry As String = "DELETE FROM MenuTable WHERE FoodName = '" & foodname & "'"
        Dim command As New SQLiteCommand(delQuerry, connection)
        Dim i As Integer = 0
        connection.Open()
        i = command.ExecuteNonQuery()
        connection.Close()

        If i > 0 Then
            removeStewFoodNameTxtBx.Clear()
            removeStewPriceTxtBx.Clear()
            selectedStewImage.Image = Nothing
            fillStewListBox()
            MsgBox("Deleted Successfully")
            Dim main As New MainWindow
            main.callLoader()
        End If
    End Sub

    Private Sub RemoveSnackItemBtn_Click(sender As Object, e As RoutedEventArgs) Handles RemoveSnackItemBtn.Click
        Dim foodname As String = Me.removeSnackFoodNameTxtBx.Text
        Dim delQuerry As String = "DELETE FROM MenuTable WHERE FoodName = '" & foodname & "'"
        Dim command As New SQLiteCommand(delQuerry, connection)
        Dim i As Integer = 0
        connection.Open()
        i = command.ExecuteNonQuery()
        connection.Close()

        If i > 0 Then
            removeSnackFoodNameTxtBx.Clear()
            removeSnackPriceTxtBx.Clear()
            selectedSnackImage.Image = Nothing
            fillStewListBox()
            MsgBox("Deleted Successfully")
            Dim main As New MainWindow
            main.callLoader()
        End If
    End Sub

    Private Sub CancelMainDishRemovalBtn_Click(sender As Object, e As RoutedEventArgs) Handles CancelMainDishRemovalBtn.Click
        Dim main As New MainWindow
        main.callLoader()
        Me.Close()
    End Sub

    Private Sub CancelStewRemovalBtn_Click(sender As Object, e As RoutedEventArgs) Handles CancelStewRemovalBtn.Click
        Dim main As New MainWindow
        main.callLoader()
        Me.Close()
    End Sub

    Private Sub CancelSnackRemovalBtn_Click(sender As Object, e As RoutedEventArgs) Handles CancelSnackRemovalBtn.Click
        Dim main As New MainWindow
        main.callLoader()
        Me.Close()
    End Sub

    Private Sub CancelDrinkRemovalBtn_Click(sender As Object, e As RoutedEventArgs) Handles CancelDrinkRemovalBtn.Click
        Dim main As New MainWindow
        main.callLoader()
        Me.Close()
    End Sub
End Class
