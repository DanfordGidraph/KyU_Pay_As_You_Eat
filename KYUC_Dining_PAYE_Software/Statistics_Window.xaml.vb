Imports XamlGeneratedNamespace
Imports System.Windows.Controls.DataVisualization.Charting
Imports System.Windows.Controls.DataVisualization
Imports System.Collections.ObjectModel
Imports System.Data.SQLite
Imports System.Data
Public Class Statistics_Window
    Dim conStrOne As String = "Data Source=Meals_Menu_Database.db; Version=3;"
    Dim conStrTwo As String = "Data Source=Service_Statistics.db; Version=3;"

    Dim timeSpanSelected As String = ""
    Dim mealTimeSelected As String = ""

    Dim conToDbOne As New SQLiteConnection(conStrOne)
    Dim conToDbTwo As New SQLiteConnection(conStrTwo)

    Private Sub Statistics_Window()
        InitializeComponent()
    End Sub
    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        setMealTimecBoxValues(timeSpancBox.SelectedIndex)
    End Sub

    Private Sub setMealTime(ByRef mTime As String)
        mealTimeSelected = mTime
    End Sub
    Private Sub setTimeSpan(ByRef span As String)
        timeSpanSelected = span
    End Sub
    Private Sub loadChartByMealTime()
        'get the food names from the menu table
        Dim getFoodNamesQuery As String = "SELECT FoodName From MenuTable"
        Dim fNamesDTable As New DataTable
        Dim fNamesAdapter As New SQLite.SQLiteDataAdapter(getFoodNamesQuery, conToDbOne)
        fNamesDTable.Rows.Clear()
        fNamesAdapter.Fill(fNamesDTable)
        'store them into an array
        Dim fNamesArray(fNamesDTable.Rows.Count - 1) As String
        For i As Integer = 0 To (fNamesDTable.Rows.Count - 1)
            fNamesArray(i) = fNamesDTable.Rows(i)(0).ToString
        Next

        'get the text of the currently selected meal time
        Dim mealTime As String = ""
        If mealTimeCBox.SelectedIndex = 0 Then
            mealTime = "Breakfast"
        ElseIf mealTimeCBox.SelectedIndex = 1 Then
            mealTime = "Lunch"
        ElseIf mealTimeCBox.SelectedIndex = 2 Then
            mealTime = "Supper"
        End If

        'get the date today
        Dim dateToday As String = DateTime.Today.ToLongDateString
        Me.ShowDateLabel.Content = dateToday

        'retrieve the revenue earned for each food item that has been sold on that particular mealtime
        'store the values returned into an array of the same length as the food names array created above
        Dim fRevenueArray(fNamesDTable.Rows.Count - 1) As Integer
        For j As Integer = 0 To fNamesDTable.Rows.Count - 1
            Dim getFoodRevenueQuery As String = "SELECT RevenueEarned FROM FoodSales_Statistics WHERE FoodName = '" &
                                                    fNamesArray(j) & "'" & " AND " & "MealTime = '" & mealTime & "'" &
                                                    " AND " & "Date = '" & dateToday & "'"
            Dim fRevenueDTable As New DataTable
            fRevenueDTable.Rows.Clear()
            Dim fRevenueAdapter As New SQLite.SQLiteDataAdapter(getFoodRevenueQuery, conToDbTwo)
            fRevenueAdapter.Fill(fRevenueDTable)

            Dim tempSum As Integer = 0
            For Each row As DataRow In fRevenueDTable.Rows
                tempSum = tempSum + Val(row(0).ToString)
            Next
            fRevenueArray(j) = tempSum
        Next


        'create an array of key value pairs that will be used to feed data to a chart(columnchart)
        'the keys and values are from the food names array and the revenue array respectvely
        Dim kvpArray(fRevenueArray.Length - 1) As KeyValuePair(Of String, Integer)

        For k As Integer = 0 To fRevenueArray.Length - 1
            If fRevenueArray(k) = 0 Then
                fNamesArray(k) = " "
            End If
            kvpArray(k) = New KeyValuePair(Of String, Integer)(fNamesArray(k), fRevenueArray(k))
        Next

        'cast the array (key value pair array) onto the chart to generate culumns and their values i.e draw the graph
        DirectCast(Me.IncomeChart.Series(0), ColumnSeries).ItemsSource = kvpArray

    End Sub

    Private Sub loadChartByDay()
        Dim getFoodNamesQuery As String = "SELECT FoodName From MenuTable"
        Dim fNamesDTable As New DataTable
        Dim fNamesAdapter As New SQLite.SQLiteDataAdapter(getFoodNamesQuery, conToDbOne)
        fNamesDTable.Rows.Clear()
        fNamesAdapter.Fill(fNamesDTable)
        'store them into an array
        Dim fNamesArray(fNamesDTable.Rows.Count - 1) As String
        For i As Integer = 0 To (fNamesDTable.Rows.Count - 1)
            fNamesArray(i) = fNamesDTable.Rows(i)(0).ToString
        Next

        'get the text of the currently selected Day
        Dim daySelected As String = ""
        If mealTimeCBox.SelectedIndex = 0 Then
            daySelected = "Monday"
        ElseIf mealTimeCBox.SelectedIndex = 1 Then
            daySelected = "Tuesday"
        ElseIf mealTimeCBox.SelectedIndex = 2 Then
            daySelected = "Wednesday"
        ElseIf mealTimeCBox.SelectedIndex = 3 Then
            daySelected = "Thursday"
        ElseIf mealTimeCBox.SelectedIndex = 4 Then
            daySelected = "Friday"
        ElseIf mealTimeCBox.SelectedIndex = 5 Then
            daySelected = "Saturday"
        ElseIf mealTimeCBox.SelectedIndex = 6 Then
            daySelected = "Sunday"
        End If

        Dim mealTime As String = ""
        If byDayResultsRefinementcBox.SelectedIndex = 0 Then
            mealTime = "All"
        ElseIf byDayResultsRefinementcBox.SelectedIndex = 1 Then
            mealTime = "Breakfast"
        ElseIf byDayResultsRefinementcBox.SelectedIndex = 2 Then
            mealTime = "Lunch"
        ElseIf byDayResultsRefinementcBox.SelectedIndex = 3 Then
            mealTime = "Supper"
        End If



        'retrieve the revenue earned for each food item that has been sold on that particular mealtime
        'store the values returned into an array of the same length as the food names array created above
        Dim fRevenueArray(fNamesDTable.Rows.Count - 1) As Integer
        For j As Integer = 0 To fNamesDTable.Rows.Count - 1
            Dim revenueQuery1 As String = "SELECT RevenueEarned FROM FoodSales_Statistics WHERE FoodName = '" &
                                                    fNamesArray(j) & "'" & " AND " & "Day = '" & daySelected & "'"

            Dim revenuueQuery2 As String = "SELECT RevenueEarned FROM FoodSales_Statistics WHERE FoodName = '" &
                                                    fNamesArray(j) & "'" & " AND " & "Day = '" & daySelected & "'" & " AND " &
                                                    "MealTime = '" & mealTime & "'"

            Dim getFoodRevenueQuery As String = ""
            If byDayResultsRefinementcBox.SelectedIndex < 1 Then
                getFoodRevenueQuery = revenueQuery1
            ElseIf byDayResultsRefinementcBox.SelectedIndex >= 1 Then
                getFoodRevenueQuery = revenuueQuery2
                'ElseIf mealTime = "Breakfast" Then
                '    getFoodRevenueQuery = revenuueQuery1
                'ElseIf mealTime = "Breakfast" Then
                '    getFoodRevenueQuery = revenuueQuery1
            End If

            Dim fRevenueDTable As New DataTable
            fRevenueDTable.Rows.Clear()
            Dim fRevenueAdapter As New SQLite.SQLiteDataAdapter(getFoodRevenueQuery, conToDbTwo)
            fRevenueAdapter.Fill(fRevenueDTable)

            Dim tempSum As Integer = 0
            For Each row As DataRow In fRevenueDTable.Rows
                tempSum = tempSum + Val(row(0).ToString)
            Next
            fRevenueArray(j) = tempSum
        Next


        'create an array of key value pairs that will be used to feed data to a chart(columnchart)
        'the keys and values are from the food names array and the revenue array respectvely
        Dim kvpArray(fRevenueArray.Length - 1) As KeyValuePair(Of String, Integer)

        For k As Integer = 0 To fRevenueArray.Length - 1
            If fRevenueArray(k) = 0 Then
                fNamesArray(k) = " "
            End If
            kvpArray(k) = New KeyValuePair(Of String, Integer)(fNamesArray(k), fRevenueArray(k))
        Next

        'cast the array (key value pair array) onto the chart to generate culumns and their values i.e draw the graph
        DirectCast(Me.IncomeChart.Series(0), ColumnSeries).ItemsSource = kvpArray
    End Sub

    Private Sub setMealTimecBoxValues(span As Integer)
        If span = 1 Then
            Dim cbItem1, cbItem2, cbItem3 As New ComboBoxItem
            cbItem1.Content = "Breakfast"
            cbItem2.Content = "Lunch"
            cbItem3.Content = "Supper"
            cbItem1.IsSelected = True

            mealTimeCBox.Items.Clear()
            mealTimeCBox.Items.Add(cbItem1)
            mealTimeCBox.Items.Add(cbItem2)
            mealTimeCBox.Items.Add(cbItem3)
            loadChartByMealTime()

            byDayResultsRefinementcBox.Visibility = Visibility.Hidden

        ElseIf span = 2 Then
            Dim cbItem1, cbItem2, cbItem3, cbItem4, cbItem5, cbItem6, cbItem7 As New ComboBoxItem
            cbItem1.Content = "Monday"
            cbItem2.Content = "Tuesday"
            cbItem3.Content = "Wednesday"
            cbItem4.Content = "Thursday"
            cbItem5.Content = "Friday"
            cbItem6.Content = "Saturday"
            cbItem7.Content = "Sunday"

            cbItem1.IsSelected = True

            mealTimeCBox.Items.Clear()
            mealTimeCBox.Items.Add(cbItem1)
            mealTimeCBox.Items.Add(cbItem2)
            mealTimeCBox.Items.Add(cbItem3)
            mealTimeCBox.Items.Add(cbItem4)
            mealTimeCBox.Items.Add(cbItem5)
            mealTimeCBox.Items.Add(cbItem6)
            mealTimeCBox.Items.Add(cbItem7)
            loadChartByDay()

            Dim cbItem1a, cbItem1b, cbItem1c, cbItem1d As New ComboBoxItem
            cbItem1a.Content = "All Meals"
            cbItem1b.Content = "Breakfast"
            cbItem1c.Content = "Lunch"
            cbItem1d.Content = "Supper"
            cbItem1a.IsSelected = True

            byDayResultsRefinementcBox.Items.Clear()
            byDayResultsRefinementcBox.Items.Add(cbItem1a)
            byDayResultsRefinementcBox.Items.Add(cbItem1b)
            byDayResultsRefinementcBox.Items.Add(cbItem1c)
            byDayResultsRefinementcBox.Items.Add(cbItem1d)

            byDayResultsRefinementcBox.Visibility = Visibility.Visible

        End If
    End Sub
    Private Sub timeSpancBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles timeSpancBox.SelectionChanged
        setMealTimecBoxValues(timeSpancBox.SelectedIndex)
    End Sub

    Private Sub mealTimeCBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles mealTimeCBox.SelectionChanged
        If mealTimeCBox.Items.Count < 4 Then
            loadChartByMealTime()
        ElseIf mealTimeCBox.Items.Count > 3 Then
            loadChartByDay()
        End If
    End Sub

    Private Sub byDayResultsRefinementcBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles byDayResultsRefinementcBox.SelectionChanged
        loadChartByDay()
    End Sub
End Class
