Imports System.Data


Public Class SQL_Handler
    'create a connection and a command runner
    Dim connection As New SQLite.SQLiteConnection("Data Source=Meals_Menu_Database.db; Version=3;")
    '=====================================================================================================================================
    'create a method that can be used by instance objects to create new connections
    Public Function createAccessConnection() As SQLite.SQLiteConnection
        Dim conect As New SQLite.SQLiteConnection("Data Source=Meals_Menu_Database.db; Version=3;")
        conect.Open()
        Return conect
    End Function
    'create a new instance of the command class to facilitate comands to the database(s)
    Private sqlcommand As New SQLite.SQLiteCommand
    'create a data adapter to manage the data in the database
    Public SQLdataAdapter As New SQLite.SQLiteDataAdapter
    Public SQLdataSet As New DataSet

    'Make a parameters list to prevent SQL injections
    Public parameters As New List(Of OleDb.OleDbParameter)

    '=====================================================================================================================================
    'make the query execution method
    Public Sub ExecuteQuerry(query As String)
        connection.Open()

        'create the command body
        sqlcommand = New SQLite.SQLiteCommand(query, connection)

        'create the parameters management
        parameters.ForEach(Sub(x) sqlcommand.Parameters.Add(x))

        parameters.Clear()

        'execute commands to fill dataset
        SQLdataSet = New DataSet
        SQLdataAdapter = New SQLite.SQLiteDataAdapter(sqlcommand)

        SQLdataAdapter.Fill(SQLdataSet)

        connection.Close()
    End Sub


    '=====================================================================================================================================
    'method to run non queries
    Public Sub nonQuery(query As String)
        connection.Open()
        Dim cmd As New SQLite.SQLiteCommand(query, connection)

        Dim i As Integer = cmd.ExecuteNonQuery()
        If (i > 0) Then
            MsgBox("Command Successfull")

        End If
        cmd.Dispose()
    End Sub
End Class
