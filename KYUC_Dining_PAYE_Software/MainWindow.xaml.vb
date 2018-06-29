Imports System.Data
Imports System.Data.SQLite
Imports System.Drawing.Printing
Imports System.Drawing.Graphics
Public Class MainWindow
    'create a connection string for the menu table database (Global)
    Dim sqliteStr As String = "Data Source = Meals_Menu_Database.db; Version=3;"
    Dim sqliteStr2 As String = "Data Source = Service_Statistics.db; Version=3;"
    Dim con1 As New SQLiteConnection(sqliteStr)
    Dim con2 As New SQLiteConnection(sqliteStr2)
    'create the counter to be used in allocating main meals and stews an image slot in the viewer
    Dim mainMealsCounter As Integer = 1
    'create the counter to be used in allocating snacks and drinks an image slot in the viewer
    Dim snacksAndDrinksCounter As Integer = 1
    'create the counter to be used in allocating all meals and drinks slot in the dynamic receipt viewer 
    Dim _receiptCounter As Integer = 1
    'create variable that will hold the printer's name after first print and thus make it posible to skip the printdialog in the consecutive prints
    Dim newPrinterName As String = ""
    Private serverName As String = String.Empty

    Public Sub getServerName(name As String)
        setServerName(name, serverName)
    End Sub
    Private Sub setServerName(name As String, ByRef sName As String)
        sName = name
    End Sub
    Private Sub mainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles MyBase.Loaded, MyBase.Loaded
        checkForDatabaseEntries()
        loadMainCoursesComboBox()
        loadStewsComboBox()
        loadSnacksComboBox()
        loadDrinksComboBox()
    End Sub
    'Private Sub LeftDrawerHost_ContextMenuClosing(sender As Object, e As ContextMenuEventArgs) Handles LeftDrawerHost.ContextMenuClosing
    '    checkForDatabaseEntries()
    '    loadMainCoursesComboBox()
    '    loadDrinksComboBox()
    '    loadSnacksComboBox()
    '    loadStewsComboBox()
    'End Sub

    Private Sub LeftDrawerHost_LostFocus(sender As Object, e As RoutedEventArgs) Handles LeftDrawerHost.LostFocus
        checkForDatabaseEntries()
        loadMainCoursesComboBox()
        loadDrinksComboBox()
        loadSnacksComboBox()
        loadStewsComboBox()
    End Sub


    Public Sub callLoader()
        loadElements()
    End Sub
    Public Sub callCloser()
        'closeMe()\
        Me.Close()
    End Sub
    Public Sub callResumer()
        resumeVisible()
    End Sub
    Private Sub resumeVisible()
        Me.Visibility = Visibility.Visible
    End Sub
    Private Sub closeMe()
        Me.Close()
    End Sub
    Private Sub loadElements()
        checkForDatabaseEntries()
        loadMainCoursesComboBox()
        loadStewsComboBox()
        loadSnacksComboBox()
        loadDrinksComboBox()
        loadTotals()
    End Sub

    Private Sub checkForDatabaseEntries()
        Dim nonQuery As String = "SELECT PicturePath FROM MenuTable"
        Dim dTable As New DataTable
        Dim adapt As New SQLite.SQLiteDataAdapter(nonQuery, con1)
        adapt.Fill(dTable)

        For Each row As DataRow In dTable.Rows
            Dim rowContent As String = row(0).ToString
            If Not My.Computer.FileSystem.FileExists(rowContent) Then
                Dim delQuerry As String = "DELETE FROM MenuTable WHERE PicturePath = '" & rowContent & "'"
                Dim command As New SQLiteCommand(delQuerry, con1)
                con1.Open()
                command.ExecuteNonQuery()
                con1.Close()
                adapt.Update(dTable)
            End If
        Next
        dTable.Dispose()
    End Sub
    '=====================================================================================================================================================================
    '=====================================================================================================================================================================
    '=====================================================================================================================================================================
    Private Sub loadMainCoursesComboBox()
        'MainCoursesComboBox.Items.Clear()
        'Me.MainCoursesComboBox.Items.Add("Not Selected")
        'MainCoursesComboBox.SelectedIndex = 0

        Dim con As New SQLite.SQLiteConnection(sqliteStr)
        Dim sqlQuery As String = "SELECT FoodName FROM MenuTable WHERE Category = 'Main Dish'"
        Dim adapter As New SQLiteDataAdapter(sqlQuery, con)
        Dim dSet As New DataSet
        adapter.Fill(dSet)
        ' Dim comboArray As ComboBoxItem() = {}
        For Each row As DataRow In dSet.Tables(0).Rows
            Dim cont As String = row(0).ToString
            Dim comboBItem As New ComboBoxItem
            comboBItem.Content = cont
            MainCoursesComboBox.Items.Add(comboBItem)
        Next
        findDupes(MainCoursesComboBox)
    End Sub

    Private Sub findDupes(comboB As ComboBox)
        For i As Int16 = 0 To comboB.Items.Count - 2
            For j As Int16 = comboB.Items.Count - 1 To i + 1 Step -1
                If comboB.Items(i).ToString = comboB.Items(j).ToString Then
                    comboB.Items.RemoveAt(j)
                End If
            Next
        Next
    End Sub

    Private Sub loadStewsComboBox()
        'StewsComboBox.Items.Clear()
        'Me.StewsComboBox.Items.Add("Not Selected")
        'StewsComboBox.SelectedIndex = 0

        Dim con As New SQLite.SQLiteConnection(sqliteStr)
        Dim sqlQuery As String = "SELECT FoodName FROM MenuTable WHERE Category = 'Stew'"
        con.Open()
        Dim adapter As New SQLiteDataAdapter(sqlQuery, con)
        con.Close()
        Dim dSet As New DataSet
        adapter.Fill(dSet)
        For Each row As DataRow In dSet.Tables(0).Rows
            Dim cont As String = row(0).ToString
            Dim comboBItem As New ComboBoxItem
            comboBItem.Content = cont
            Me.StewsComboBox.Items.Add(comboBItem)
        Next
        findDupes(StewsComboBox)
    End Sub

    Private Sub loadSnacksComboBox()
        'SnacksComboBox.Items.Clear()
        'Me.SnacksComboBox.Items.Add("Not Selected")
        'SnacksComboBox.SelectedIndex = 0

        Dim con As New SQLite.SQLiteConnection(sqliteStr)
        Dim sqlQuery As String = "SELECT FoodName FROM MenuTable WHERE Category = 'Snack'"
        con.Open()
        Dim adapter As New SQLiteDataAdapter(sqlQuery, con)
        con.Close()
        Dim dSet As New DataSet
        adapter.Fill(dSet)
        For Each row As DataRow In dSet.Tables(0).Rows
            Dim cont As String = row(0).ToString
            Dim comboBItem As New ComboBoxItem
            comboBItem.Content = cont
            Me.SnacksComboBox.Items.Add(comboBItem)
        Next
        findDupes(SnacksComboBox)
    End Sub

    Private Sub loadDrinksComboBox()
        'DrinksComboBox.Items.Clear()
        'Me.DrinksComboBox.Items.Add("Not Selected")
        'DrinksComboBox.SelectedIndex = 0

        Dim con As New SQLite.SQLiteConnection(sqliteStr)
        Dim sqlQuery As String = "SELECT FoodName FROM MenuTable WHERE Category = 'Drink'"
        con.Open()
        Dim adapter As New SQLiteDataAdapter(sqlQuery, con)
        con.Close()
        Dim dSet As New DataSet
        adapter.Fill(dSet)
        For Each row As DataRow In dSet.Tables(0).Rows
            Dim cont As String = row(0).ToString
            Dim comboBItem As New ComboBoxItem
            comboBItem.Content = cont
            Me.DrinksComboBox.Items.Add(comboBItem)
        Next
        findDupes(DrinksComboBox)
    End Sub
    '=====================================================================================================================================================================
    '=====================================================================================================================================================================
    '=====================================================================================================================================================================
    'dynamically changes the values of the totals depending on the added values in the receipt
    Private Sub loadTotals()
        Dim qtyTotal As Integer = getReceiptQuantities.Sum
        Me.totalQuantity.Content = qtyTotal


        Dim priceTotal As Integer = getReceiptFoodPrices.Sum
        Me.TotalAmountLbl.Content = priceTotal

        Me.balanceTxtBx.Text = Val(Me.amountPaidTxtBx.Text) - priceTotal
        If Val(balanceTxtBx.Text) < 0 Then
            Me.balanceTxtBx.Foreground = Brushes.Red
        Else
            Me.balanceTxtBx.Foreground = Brushes.Black
        End If

    End Sub
    'returns all entries in the main window to their defaults
    Private Sub clearMainWindowEntries(ByRef counter As Integer)
        ' reset the receipt counter in readiness for the next processing
        counter = 1

        receiptFoodNameLabel1.Content = ""
        receiptFoodNameLabel2.Content = ""
        receiptFoodNameLabel3.Content = ""
        receiptFoodNameLabel4.Content = ""
        receiptFoodNameLabel5.Content = ""
        receiptFoodNameLabel6.Content = ""

        receiptFoodPriceLabel1.Content = ""
        receiptFoodPriceLabel2.Content = ""
        receiptFoodPriceLabel3.Content = ""
        receiptFoodPriceLabel4.Content = ""
        receiptFoodPriceLabel5.Content = ""
        receiptFoodPriceLabel6.Content = ""

        receiptFoodQuantityLabel1.Content = ""
        receiptFoodQuantityLabel2.Content = ""
        receiptFoodQuantityLabel3.Content = ""
        receiptFoodQuantityLabel4.Content = ""
        receiptFoodQuantityLabel5.Content = ""
        receiptFoodQuantityLabel6.Content = ""

        TotalAmountLbl.Content = ""
        amountPaidTxtBx.Clear()
        totalQuantity.Content = ""
        balanceTxtBx.Clear()


        MainMealsViewPBox1.Image = Nothing
        MainMealsViewPBox2.Image = Nothing
        MainMealsViewPBox3.Image = Nothing
        MainMealsViewPBox4.Image = Nothing
        MainMealsViewPBox5.Image = Nothing
        MainMealsViewPBox6.Image = Nothing

        snacksAndDrinksViewPBox1.Image = Nothing
        snacksAndDrinksViewPBox2.Image = Nothing
        snacksAndDrinksViewPBox3.Image = Nothing
        snacksAndDrinksViewPBox4.Image = Nothing
        snacksAndDrinksViewPBox5.Image = Nothing
        snacksAndDrinksViewPBox6.Image = Nothing

        MainCoursesComboBox.SelectedIndex = 0
        StewsComboBox.SelectedIndex = 0
        SnacksComboBox.SelectedIndex = 0
        DrinksComboBox.SelectedIndex = 0

        MainCoursePlatesQuantity.Text = "1"
        stewPlatesQuantity.Text = "1"
        SnackQuantity.Text = "1"
        DrinksQuantity.Text = "1"

    End Sub
    '=====================================================================================================================================================================
    '=====================================================================================================================================================================
    '=====================================================================================================================================================================
    Private Sub menuEntriesTreeView_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs) Handles menuEntriesTreeView.MouseLeftButtonUp
        menuToggleButton.IsChecked = False
    End Sub
    Private Sub addMainDishTVI_Selected(sender As Object, e As RoutedEventArgs) Handles addMainDishTVI.Selected
        Dim foodAdder As New FoodItemsAddingWindow
        foodAdder.selection = "Main Dish"
        foodAdder.HighlightMainDish()
        foodAdder.ShowDialog()
    End Sub

    Private Sub addStewTVI_Selected(sender As Object, e As RoutedEventArgs) Handles addStewTVI.Selected
        Dim foodAdder As New FoodItemsAddingWindow
        foodAdder.selection = "Stew"
        foodAdder.HighlightStew()
        foodAdder.ShowDialog()
    End Sub
    Private Sub addSnackTVI_Selected(sender As Object, e As RoutedEventArgs) Handles addSnackTVI.Selected
        Dim foodAdder As New FoodItemsAddingWindow
        foodAdder.selection = "Snack"
        foodAdder.HighlightSnack()
        foodAdder.ShowDialog()
    End Sub

    Private Sub addDrinkTVI_Selected(sender As Object, e As RoutedEventArgs) Handles addDrinkTVI.Selected
        Dim foodAdder As New FoodItemsAddingWindow
        foodAdder.selection = "Drink"
        foodAdder.HighlightDrink()
        foodAdder.ShowDialog()
    End Sub

    Private Sub removeMainDishTVI_Selected(sender As Object, e As RoutedEventArgs) Handles removeMainDishTVI.Selected

        Dim foodRemover As New RemoveFoodItemsWindow
        foodRemover.fillMainDishListBox()
        foodRemover.ShowDialog()

    End Sub

    Private Sub RemoveStewTVI_Selected(sender As Object, e As RoutedEventArgs) Handles RemoveStewTVI.Selected
        Dim foodRemover As New RemoveFoodItemsWindow
        foodRemover.fillStewListBox()
        foodRemover.ShowDialog()
    End Sub

    Private Sub removeSnackTVI_Selected(sender As Object, e As RoutedEventArgs) Handles removeSnackTVI.Selected
        Dim foodRemover As New RemoveFoodItemsWindow
        foodRemover.fillSnackListBox()
        foodRemover.ShowDialog()
    End Sub

    Private Sub removeDrinkTVI_Selected(sender As Object, e As RoutedEventArgs) Handles removeDrinkTVI.Selected
        Dim foodRemover As New RemoveFoodItemsWindow
        foodRemover.fillDrinkListBox()
        foodRemover.ShowDialog()
    End Sub

    '=====================================================================================================================================================================
    '=====================================================================================================================================================================
    '=====================================================================================================================================================================
    Private Sub addSelectedMainDish_StewToReceipt(foodName As String, quantity As Integer, ByRef receiptCounter As Integer, ByRef mainMealsImagePositionCounter As Integer)
        If foodName = "Not Selected" Then
            MsgBox("Please Choose A food Item First", MsgBoxStyle.ApplicationModal, Title = "Submission Error")

        Else
            Dim price As Integer = 0
            Dim imagepath As String = ""
            Dim getPrice As String = "SELECT Price FROM MenuTable WHERE FoodName = '" & foodName & "'"
            Dim getImage As String = "SELECT PicturePath FROM MenuTable WHERE FoodName = '" & foodName & "'"
            Dim con As New SQLite.SQLiteConnection(sqliteStr)
            con.Open()
            Dim adapter1 As New SQLiteDataAdapter(getPrice, con)
            Dim adapter2 As New SQLiteDataAdapter(getImage, con)
            con.Close()
            Dim dSet1 As New DataSet
            Dim dSet2 As New DataSet
            adapter1.Fill(dSet1)
            adapter2.Fill(dSet2)
            price = Val(dSet1.Tables(0).Rows(0)("Price").ToString) * quantity
            imagepath = dSet2.Tables(0).Rows(0)("PicturePath").ToString

            'This if block manages how items will be shown in the dynamic receipt viewr
            If receiptCounter = 1 Then
                Me.receiptFoodNameLabel1.Content = foodName
                Me.receiptFoodQuantityLabel1.Content = quantity
                Me.receiptFoodPriceLabel1.Content = price
                receiptCounter = receiptCounter + 1
                loadTotals()
            ElseIf receiptCounter = 2 Then
                Me.receiptFoodNameLabel2.Content = foodName
                Me.receiptFoodQuantityLabel2.Content = quantity
                Me.receiptFoodPriceLabel2.Content = price
                receiptCounter = receiptCounter + 1
                loadTotals()

            ElseIf receiptCounter = 3 Then
                Me.receiptFoodNameLabel3.Content = foodName
                Me.receiptFoodQuantityLabel3.Content = quantity
                Me.receiptFoodPriceLabel3.Content = price
                receiptCounter = receiptCounter + 1
                loadTotals()

            ElseIf receiptCounter = 4 Then
                Me.receiptFoodNameLabel4.Content = foodName
                Me.receiptFoodQuantityLabel4.Content = quantity
                Me.receiptFoodPriceLabel4.Content = price
                receiptCounter = receiptCounter + 1
                loadTotals()

            ElseIf receiptCounter = 5 Then
                Me.receiptFoodNameLabel5.Content = foodName
                Me.receiptFoodQuantityLabel5.Content = quantity
                Me.receiptFoodPriceLabel5.Content = price
                receiptCounter = receiptCounter + 1
                loadTotals()

            ElseIf receiptCounter = 6 Then
                Me.receiptFoodNameLabel6.Content = foodName
                Me.receiptFoodQuantityLabel6.Content = quantity
                Me.receiptFoodPriceLabel6.Content = price
                receiptCounter = receiptCounter + 1
                loadTotals()
            ElseIf receiptCounter > 6 Then
                MsgBox("Can't add More Food Items to Receipt")

            End If

            'This If block manages how images of the selecte dfood items will be displayed on the viewing box i.e their order of placemen
            If MainMealsViewPBox1.Image Is Nothing Then
                Me.MainMealsViewPBox1.Image = System.Drawing.Image.FromFile(imagepath)
                mainMealsImagePositionCounter = 2
            ElseIf mainMealsImagePositionCounter = 2 Then
                Me.MainMealsViewPBox2.Image = System.Drawing.Image.FromFile(imagepath)
                mainMealsImagePositionCounter = 3
            ElseIf mainMealsImagePositionCounter = 3 Then
                Me.MainMealsViewPBox3.Image = System.Drawing.Image.FromFile(imagepath)
                mainMealsImagePositionCounter = 4
            ElseIf mainMealsImagePositionCounter = 4 Then
                Me.MainMealsViewPBox4.Image = System.Drawing.Image.FromFile(imagepath)
                mainMealsImagePositionCounter = 5
            ElseIf mainMealsImagePositionCounter = 5 Then
                Me.MainMealsViewPBox5.Image = System.Drawing.Image.FromFile(imagepath)
                mainMealsImagePositionCounter = 6
            ElseIf mainMealsImagePositionCounter = 6 Then
                Me.MainMealsViewPBox6.Image = System.Drawing.Image.FromFile(imagepath)

            End If
        End If
    End Sub

    Private Sub addSelectedSnack_DrinkToReceipt(foodName As String, quantity As Integer, ByRef receiptCounter As Integer, ByRef imagePositionCounter As Integer)
        If foodName = "Not Selected" Then
            MsgBox("Please Choose A food Item First", MsgBoxStyle.ApplicationModal, Title = "Submission Error")
        Else
            Dim price As Integer = 0
            Dim imagepath As String = ""

            Dim getPrice As String = "SELECT Price FROM MenuTable WHERE FoodName = '" & foodName & "'"
            Dim getImage As String = "SELECT PicturePath FROM MenuTable WHERE FoodName = '" & foodName & "'"
            Dim con As New SQLite.SQLiteConnection(sqliteStr)
            con.Open()
            Dim adapter1 As New SQLiteDataAdapter(getPrice, con)
            Dim adapter2 As New SQLiteDataAdapter(getImage, con)
            con.Close()
            Dim dSet1 As New DataSet
            Dim dSet2 As New DataSet
            adapter1.Fill(dSet1)
            adapter2.Fill(dSet2)
            price = Val(dSet1.Tables(0).Rows(0)("Price").ToString) * quantity
            imagepath = dSet2.Tables(0).Rows(0)("PicturePath").ToString

            If receiptCounter = 1 Then
                Me.receiptFoodNameLabel1.Content = foodName
                Me.receiptFoodQuantityLabel1.Content = quantity
                Me.receiptFoodPriceLabel1.Content = price
                receiptCounter = receiptCounter + 1
                loadTotals()
            ElseIf receiptCounter = 2 Then
                Me.receiptFoodNameLabel2.Content = foodName
                Me.receiptFoodQuantityLabel2.Content = quantity
                Me.receiptFoodPriceLabel2.Content = price
                receiptCounter = receiptCounter + 1
                loadTotals()

            ElseIf receiptCounter = 3 Then
                Me.receiptFoodNameLabel3.Content = foodName
                Me.receiptFoodQuantityLabel3.Content = quantity
                Me.receiptFoodPriceLabel3.Content = price
                receiptCounter = receiptCounter + 1
                loadTotals()

            ElseIf receiptCounter = 4 Then
                Me.receiptFoodNameLabel4.Content = foodName
                Me.receiptFoodQuantityLabel4.Content = quantity
                Me.receiptFoodPriceLabel4.Content = price
                receiptCounter = receiptCounter + 1
                loadTotals()

            ElseIf receiptCounter = 5 Then
                Me.receiptFoodNameLabel5.Content = foodName
                Me.receiptFoodQuantityLabel5.Content = quantity
                Me.receiptFoodPriceLabel5.Content = price
                receiptCounter = receiptCounter + 1
                loadTotals()

            ElseIf receiptCounter = 6 Then
                Me.receiptFoodNameLabel6.Content = foodName
                Me.receiptFoodQuantityLabel6.Content = quantity
                Me.receiptFoodPriceLabel6.Content = price
                receiptCounter = receiptCounter + 1
                loadTotals()
            ElseIf receiptCounter > 6 Then
                MsgBox("Can't add More Food Items to Receipt")

            End If

            If snacksAndDrinksViewPBox1.Image Is Nothing Then
                Me.snacksAndDrinksViewPBox1.Image = System.Drawing.Image.FromFile(imagepath)
                imagePositionCounter = 2
            ElseIf imagePositionCounter = 2 Then
                Me.snacksAndDrinksViewPBox2.Image = System.Drawing.Image.FromFile(imagepath)
                imagePositionCounter = 3
            ElseIf imagePositionCounter = 3 Then
                Me.snacksAndDrinksViewPBox3.Image = System.Drawing.Image.FromFile(imagepath)
                imagePositionCounter = 4
            ElseIf imagePositionCounter = 4 Then
                Me.snacksAndDrinksViewPBox4.Image = System.Drawing.Image.FromFile(imagepath)
                imagePositionCounter = 5
            ElseIf imagePositionCounter = 5 Then
                Me.snacksAndDrinksViewPBox5.Image = System.Drawing.Image.FromFile(imagepath)
                imagePositionCounter = 6
            ElseIf imagePositionCounter = 6 Then
                Me.snacksAndDrinksViewPBox6.Image = System.Drawing.Image.FromFile(imagepath)

            End If
        End If

    End Sub
    '=====================================================================================================================================================================
    '=====================================================================================================================================================================
    '=====================================================================================================================================================================
    Private Sub addSelectedMainCourseBtn_Click(sender As Object, e As RoutedEventArgs) Handles addSelectedMainCourseBtn.Click
        Dim fdName As String = Me.MainCoursesComboBox.Text
        Dim qnty As Integer = Val(Me.MainCoursePlatesQuantity.Text)
        addSelectedMainDish_StewToReceipt(fdName, qnty, _receiptCounter, mainMealsCounter)
    End Sub

    Private Sub addSelectedStewBtn_Click(sender As Object, e As RoutedEventArgs) Handles addSelectedStewBtn.Click
        Dim fdName As String = Me.StewsComboBox.Text
        Dim qnty As Integer = Val(Me.stewPlatesQuantity.Text)
        addSelectedMainDish_StewToReceipt(fdName, qnty, _receiptCounter, mainMealsCounter)
    End Sub

    Private Sub addSelectedSnackBtn_Click(sender As Object, e As RoutedEventArgs) Handles addSelectedSnackBtn.Click
        Dim fdName As String = Me.SnacksComboBox.Text
        Dim qnty As Integer = Val(Me.SnackQuantity.Text)
        addSelectedSnack_DrinkToReceipt(fdName, qnty, _receiptCounter, snacksAndDrinksCounter)
    End Sub

    Private Sub addSelectedDrinkBtn_Click(sender As Object, e As RoutedEventArgs) Handles addSelectedDrinkBtn.Click
        Dim fdName As String = Me.DrinksComboBox.Text
        Dim qnty As Integer = Val(Me.DrinksQuantity.Text)
        addSelectedSnack_DrinkToReceipt(fdName, qnty, _receiptCounter, snacksAndDrinksCounter)
    End Sub

    '=====================================================================================================================================================================
    '=====================================================================================================================================================================
    '=====================================================================================================================================================================
    Private Sub amountPaidTxtBx_TextChanged(sender As Object, e As TextChangedEventArgs) Handles amountPaidTxtBx.TextChanged
        loadTotals()
    End Sub

    '=====================================================================================================================================================================
    '=====================================================================================================================================================================
    '=====================================================================================================================================================================
    Private Function getReceiptFoodNames() As String()

        Dim fdName1 As String = Me.receiptFoodNameLabel1.Content
        Dim fdName2 As String = Me.receiptFoodNameLabel2.Content
        Dim fdName3 As String = Me.receiptFoodNameLabel3.Content
        Dim fdName4 As String = Me.receiptFoodNameLabel4.Content
        Dim fdName5 As String = Me.receiptFoodNameLabel5.Content
        Dim fdName6 As String = Me.receiptFoodNameLabel6.Content
        Return New String() {fdName1, fdName2, fdName3, fdName4, fdName5, fdName6}
    End Function
    Private Function getReceiptQuantities() As Integer()
        Dim fdQuantity1 As Integer = Val(Me.receiptFoodQuantityLabel1.Content)
        Dim fdQuantity2 As Integer = Val(Me.receiptFoodQuantityLabel2.Content)
        Dim fdQuantity3 As Integer = Val(Me.receiptFoodQuantityLabel3.Content)
        Dim fdQuantity4 As Integer = Val(Me.receiptFoodQuantityLabel4.Content)
        Dim fdQuantity5 As Integer = Val(Me.receiptFoodQuantityLabel5.Content)
        Dim fdQuantity6 As Integer = Val(Me.receiptFoodQuantityLabel6.Content)
        Return New Integer() {fdQuantity1, fdQuantity2, fdQuantity3, fdQuantity4, fdQuantity5, fdQuantity6}


    End Function
    Private Function getReceiptFoodPrices() As Integer()
        Dim fdPrice1 As Integer = Val(Me.receiptFoodPriceLabel1.Content)
        Dim fdPrice2 As Integer = Val(Me.receiptFoodPriceLabel2.Content)
        Dim fdPrice3 As Integer = Val(Me.receiptFoodPriceLabel3.Content)
        Dim fdPrice4 As Integer = Val(Me.receiptFoodPriceLabel4.Content)
        Dim fdPrice5 As Integer = Val(Me.receiptFoodPriceLabel5.Content)
        Dim fdPrice6 As Integer = Val(Me.receiptFoodPriceLabel6.Content)
        Return New Integer() {fdPrice1, fdPrice2, fdPrice3, fdPrice4, fdPrice5, fdPrice6}

    End Function
    '=====================================================================================================================================================================
    '=====================================================================================================================================================================
    '=====================================================================================================================================================================
    Private Sub printingWithDialog(ByRef printerName As String)
        'ensure that there is a zero or positive balance before printing
        If Val(balanceTxtBx.Text) >= 0 Then
            Dim printDialog As New Forms.PrintDialog()
            Dim printDoc As New PrintDocument()
            printDialog.Document = printDoc
            AddHandler printDoc.PrintPage, AddressOf Me.printReceipt
            Dim result As Forms.DialogResult = printDialog.ShowDialog
            If result = Forms.DialogResult.OK Then
                printDoc.Print()
                Dim nameofprinter As String = printDialog.PrinterSettings.PrinterName
                printerName = nameofprinter
            End If
        Else
            MsgBox("Insuffecient Funds. Please Top Up", Buttons:=MsgBoxStyle.Exclamation, Title:="Insuffecient Payment")
        End If
    End Sub

    Private Sub printWithoutDialog(printerName As String)
        'ensure a zero or positive balance before printing
        If Val(balanceTxtBx.Text) >= 0 Then
            Dim printdoc As New PrintDocument
            AddHandler printdoc.PrintPage, AddressOf Me.printReceipt
            Dim printSet As New PrinterSettings
            printdoc.PrinterSettings.PrinterName = printerName
            printSet.PrinterName = printdoc.PrinterSettings.PrinterName
            printdoc.PrinterSettings = printSet
            printdoc.Print()
        Else
            MsgBox("Insuffecient Funds. Please Top Up", Buttons:=MsgBoxStyle.Exclamation, Title:="Insuffecient Payment")
        End If

    End Sub
    '=====================================================================================================================================================================
    '=====================================================================================================================================================================
    '=====================================================================================================================================================================
    Private Sub printReceipt(sender As Object, ev As PrintPageEventArgs)
        Dim graphic = ev.Graphics
        Dim smallFont As System.Drawing.Font = New System.Drawing.Font("Courier New", 8)
        Dim midFont As System.Drawing.Font = New System.Drawing.Font("Courier New", 8, System.Drawing.FontStyle.Underline)
        Dim largeFont As System.Drawing.Font = New System.Drawing.Font("Courier New", 9, System.Drawing.FontStyle.Bold)
        Dim blackBrush As New System.Drawing.SolidBrush(System.Drawing.Color.Black)
        Dim fontHeight As Double = smallFont.GetHeight()
        Dim formarter As New System.Drawing.StringFormat
        formarter.Alignment = System.Drawing.StringAlignment.Near
        formarter.LineAlignment = System.Drawing.StringAlignment.Near


        Dim startX As Integer = 10
        Dim startY As Integer = 10
        Dim offset As Integer = 40


        Dim total As Integer = Val(Me.TotalAmountLbl.Content)
        Dim balance As Integer = Val(Me.balanceTxtBx.Text)
        Dim paid As Integer = Val(Me.amountPaidTxtBx.Text)


        graphic.DrawString("Welcome to KyUC P.A.Y.E Dining", largeFont, blackBrush, startX, startY)
        graphic.DrawString(String.Format("{0,12} {1,1} {2,4} {3,3} {4,6}", "FOOD ITEM", "", "QNTY", "", "PRICE"), midFont, blackBrush, startX, startY + fontHeight + 10, formarter)

        For i As Integer = 0 To getReceiptFoodNames().Length - 1
            Dim str1 As String = getReceiptFoodNames(i)
            Dim str2 As String = getReceiptQuantities(i)
            Dim str3 As Integer = getReceiptFoodPrices(i)
            If str1 = "" Then
                Exit For
            End If
            Dim receiptLine As String = String.Format("{0,12} {1,1} {2,4} {3,3} {4,6:C}", str1.PadRight(10), "", str2, "", str3)
            graphic.DrawString(receiptLine, smallFont, blackBrush, startX, startY + offset, formarter)
            offset = offset + fontHeight + 5
        Next
        offset = offset + 20
        graphic.DrawString(String.Format("{0,10} {1,10} {2,6:C}", "SubTotal".PadRight(10), "", total), smallFont, blackBrush, startX, startY + offset)
        offset = offset + fontHeight + 10
        graphic.DrawString(String.Format("{0,10} {1,10} {2,6:C}", "Paid".PadRight(10), "", paid), smallFont, blackBrush, startX, startY + offset)
        offset = offset + fontHeight + 10
        graphic.DrawString(String.Format("{0,10} {1,10} {2,6:C}", "Balance".PadRight(10), "", balance), smallFont, blackBrush, startX, startY + offset)
        offset = offset + fontHeight + 10
        graphic.DrawString("Thank You For Dining With Us", largeFont, blackBrush, startX, startY + offset)
        offset = offset + fontHeight + 10
        graphic.DrawString("Served By " & serverName, largeFont, blackBrush, startX, startY + offset)


        commitSaleIntoDatabase()
        clearMainWindowEntries(_receiptCounter)

    End Sub

    Private Sub commitSaleIntoDatabase()
        Dim i As Integer = 0
        For i = 0 To getReceiptFoodNames().Length - 1
            Dim str1 As String = getReceiptFoodNames(i)
            Dim str2 As String = getReceiptQuantities(i)
            Dim str3 As Integer = getReceiptFoodPrices(i)
            Dim dateToday As String = DateTime.Today.ToLongDateString
            Dim mealTime As String = getCurrentMealTime()
            Dim myTime As DateTime = DateTime.Now
            Dim dayToday As String = Today.DayOfWeek.ToString
            Dim timeServed As String = myTime.TimeOfDay.ToString().Substring(0, 8)
            Dim con As New SQLite.SQLiteConnection(sqliteStr2)
            Dim command As New SQLite.SQLiteCommand
            If str1 = "" Then
                Exit For
            End If

            command.CommandType = CommandType.Text
            command.CommandText = "INSERT INTO FoodSales_Statistics(FoodName,QuantitySold,RevenueEarned,MealTime,TimeServed,Day,Date) VALUES (@a,@b,@c,@d,@e,@f,@g)"
            command.Connection = con
            command.Parameters.AddWithValue("@a", str1)
            command.Parameters.AddWithValue("@b", str2)
            command.Parameters.AddWithValue("@c", str3)
            command.Parameters.AddWithValue("@d", mealTime)
            command.Parameters.AddWithValue("@e", timeServed)
            command.Parameters.AddWithValue("@f", dayToday)
            command.Parameters.AddWithValue("@g", dateToday)
            Dim Nq As Integer
            con.Open()
            Nq = command.ExecuteNonQuery()
            con.Close()

        Next
    End Sub
    Private Function getCurrentMealTime() As String
        Dim currentTime As DateTime = TimeOfDay
        If currentTime.Hour >= 16 Then
            Return ("Supper")
        ElseIf currentTime.Hour >= 11 Then
            Return ("Lunch")
        ElseIf currentTime.Hour >= 0 Then
            Return ("Breakfast")
        Else
            Return Nothing
        End If
    End Function

    '=====================================================================================================================================================================
    '=====================================================================================================================================================================
    '=====================================================================================================================================================================


    Private Sub printOrderBtn_Click(sender As Object, e As RoutedEventArgs) Handles printOrderBtn.Click
        If newPrinterName = "" Then
            Me.printingWithDialog(newPrinterName)
        ElseIf newPrinterName IsNot "" Then
            Me.printWithoutDialog(newPrinterName)
        End If
    End Sub

    Private Sub CancelOrderBtn_Click(sender As Object, e As RoutedEventArgs) Handles CancelOrderBtn.Click
        'call the clearing method and pass the receipt counter
        clearMainWindowEntries(_receiptCounter)
    End Sub
    '=====================================================================================================================================================================
    '=====================================================================================================================================================================
    '=====================================================================================================================================================================

    Private Sub showStatisticsBtn_Click(sender As Object, e As RoutedEventArgs) Handles showStatisticsBtn.Click
        Try
            Dim stat As New Statistics_Window
            stat.ShowDialog()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    '=====================================================================================================================================================================
    '=====================================================================================================================================================================
    '=====================================================================================================================================================================

    'Private Sub MainCoursesComboBox_GotFocus(sender As Object, e As RoutedEventArgs) Handles MainCoursesComboBox.GotFocus
    '    If MainCoursesComboBox.Items.Count < 2 Then
    '        MsgBox("Please Add Main Dishes First at The Top Left Menu", MsgBoxStyle.Information, Title:="No Main Meals Found")
    '    End If
    'End Sub
    'Private Sub StewsComboBox_GotFocus(sender As Object, e As RoutedEventArgs) Handles StewsComboBox.GotFocus
    '    If StewsComboBox.Items.Count < 2 Then
    '        MsgBox("Please Add Stews First at The Top Left Menu", MsgBoxStyle.Information, Title:="No Main Meals Found")
    '    End If
    'End Sub
    'Private Sub SnacksComboBox_GotFocus(sender As Object, e As RoutedEventArgs) Handles SnacksComboBox.GotFocus
    '    If SnacksComboBox.Items.Count < 2 Then
    '        MsgBox("Please Add Snacks First at The Top Left Menu", MsgBoxStyle.Information, Title:="No Main Meals Found")
    '    End If
    'End Sub
    'Private Sub DrinksComboBox_GotFocus(sender As Object, e As RoutedEventArgs) Handles DrinksComboBox.GotFocus
    '    If DrinksComboBox.Items.Count < 2 Then
    '        MsgBox("Please Add Drinks First at The Top Left Menu", MsgBoxStyle.Information, Title:="No Main Meals Found")
    '    End If
    'End Sub
    '=====================================================================================================================================================================
    '=====================================================================================================================================================================
    '=====================================================================================================================================================================

    Private Sub receiptLine1CancelBtn_Click(sender As Object, e As RoutedEventArgs) Handles receiptLine1CancelBtn.Click
        receiptFoodNameLabel1.Content = String.Empty
        receiptFoodPriceLabel1.Content = String.Empty
        receiptFoodQuantityLabel1.Content = String.Empty
        loadTotals()
    End Sub

    Private Sub receiptLine2CancelBtn_Click(sender As Object, e As RoutedEventArgs) Handles receiptLine2CancelBtn.Click
        receiptFoodNameLabel2.Content = String.Empty
        receiptFoodPriceLabel2.Content = String.Empty
        receiptFoodQuantityLabel2.Content = String.Empty
        loadTotals()
    End Sub

    Private Sub receiptLine3CancelBtn_Click(sender As Object, e As RoutedEventArgs) Handles receiptLine3CancelBtn.Click
        receiptFoodNameLabel3.Content = String.Empty
        receiptFoodPriceLabel3.Content = String.Empty
        receiptFoodQuantityLabel3.Content = String.Empty
        loadTotals()
    End Sub

    Private Sub receiptLine4CancelBtn_Click(sender As Object, e As RoutedEventArgs) Handles receiptLine4CancelBtn.Click
        receiptFoodNameLabel4.Content = String.Empty
        receiptFoodPriceLabel4.Content = String.Empty
        receiptFoodQuantityLabel4.Content = String.Empty
        loadTotals()
    End Sub

    Private Sub receiptLine5CancelBtn_Click(sender As Object, e As RoutedEventArgs) Handles receiptLine5CancelBtn.Click
        receiptFoodNameLabel5.Content = String.Empty
        receiptFoodPriceLabel5.Content = String.Empty
        receiptFoodQuantityLabel5.Content = String.Empty
        loadTotals()
    End Sub

    Private Sub receiptLine6CancelBtn_Click(sender As Object, e As RoutedEventArgs) Handles receiptLine6CancelBtn.Click
        receiptFoodNameLabel6.Content = String.Empty
        receiptFoodPriceLabel6.Content = String.Empty
        receiptFoodQuantityLabel6.Content = String.Empty
        loadTotals()
    End Sub

    Private Sub exportToExcel()
        Dim dayOfWeek As Integer = Today.DayOfWeek
        Dim dateYest As Date = Today.AddDays(-1)
        Dim yesturday As String = dateYest.ToLongDateString
        Dim sqlStr As String = "SELECT * FROM FoodSales_Statistics WHERE Date = '" & yesturday & "'"
        Dim adapt As New SQLiteDataAdapter(sqlStr, con2)
        Dim dataT As New DataTable
        adapt.Fill(dataT)
        Dim setD As New DataSet
        setD.Tables.Add(dataT)
        If setD.Tables(0).Rows.Count > 0 Then
            Dim exporter As New ExportToExcel
            exporter.callExporterWithDset(setD, "D:\")
        Else
            MsgBox("There is currently no data to export")
        End If

    End Sub
    Private Sub clearAndBackupDatabase()

        Dim dayOfWeek As Integer = Today.DayOfWeek

        If dayOfWeek = 1 Then
            Dim dayToday As Integer = Today.Day
            Dim datesArr(7) As String
            Dim yest As Date = Today.AddDays(-1)
            For i As Integer = 0 To 6
                datesArr(i) = yest.ToLongDateString
                yest = yest.AddDays(-1)
            Next
            For j As Integer = 0 To datesArr.Length - 1
                Dim sqlDelStr As String = "DELETE * FROM FoodSales_Statistics WHERE Date = '" & datesArr(j) & "'"
            Next
        End If

    End Sub

    Private Sub ExportToExcelBtn_Click(sender As Object, e As RoutedEventArgs) Handles ExportToExcelBtn.Click
        exportToExcel()
    End Sub


End Class
