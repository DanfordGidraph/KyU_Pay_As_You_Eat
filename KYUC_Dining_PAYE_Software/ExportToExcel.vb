Imports System.IO
Imports System.Data.SQLite
Imports System.Data
Imports EXCEL = Microsoft.Office.Interop.Excel
Public Class ExportToExcel


    Public Sub callExporterWithDset(dataS As DataSet, strPath As String)
        exportDataSetToExcel(dataS, strPath)
    End Sub
    Public Sub callExporterWithDtable()

    End Sub

    Private Sub exportDataSetToExcel(dset As DataSet, path As String)
        Dim inColumn As Integer = 0
        Dim inHeaderLength As Integer = 3
        Dim inRow As Integer = 0
        Dim def As System.Reflection.Missing = Reflection.Missing.Value

        path += "\Excel" & DateTime.Now.ToString().Replace(":", "-") & ".xlsx"

        Dim excelApp As New EXCEL.Application
        Dim excelWorkBook As New EXCEL.Workbook

        For Each dataT As DataTable In dset.Tables

            'create a worksheet
            Dim excelWorkSheet As EXCEL.Worksheet = excelWorkBook.Sheets.Add(def, excelWorkBook.Sheets(excelWorkBook.Sheets.Count), 1, def)
            excelWorkSheet.Name = dataT.TableName

            'name the columns in the sheet
            For i As Integer = 0 To dataT.Columns.Count - 1
                excelWorkSheet.Cells()(inHeaderLength + 1, i + 1) = dataT.Columns(i).ColumnName.ToUpper

            Next

            'write rows onto the sheet
            For j As Integer = 0 To dataT.Rows.Count
                For k As Integer = 0 To dataT.Columns.Count
                    inColumn = k + 1
                    inRow = inHeaderLength + 2 + j
                    excelWorkSheet.Cells()(inRow, inColumn) = dataT.Rows(j).ItemArray(k).ToString
                    If (j Mod 2 = 0) Then
                        excelWorkSheet.get_Range("A" & inRow.ToString, "C" + inRow.ToString).Interior.color = System.Drawing.Color.GreenYellow
                    End If
                Next
            Next

            'create the sheet header
            Dim cellRange As EXCEL.Range = excelWorkSheet.get_Range("A1", "C3")
            cellRange.Merge(False)
            cellRange.Interior.Color = System.Drawing.Color.Wheat
            cellRange.Font.Color = System.Drawing.Color.Gray
            cellRange.HorizontalAlignment = EXCEL.XlHAlign.xlHAlignCenter
            cellRange.VerticalAlignment = EXCEL.XlHAlign.xlHAlignCenter
            cellRange.Font.Size = 20
            excelWorkSheet.Cells()(1, 1) = Today.Date.ToLongDateString & " Food Sales"

            'style the column names
            cellRange = excelWorkSheet.get_Range("A4", "C4")
            cellRange.Font.Bold = True
            cellRange.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Wheat)
            cellRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.SandyBrown)
            excelWorkSheet.get_Range("C4").EntireColumn.HorizontalAlignment = EXCEL.XlHAlign.xlHAlignRight
            'format the price column
            excelWorkSheet.get_Range("C5").EntireColumn.NumberFormart = "0.00"
            excelWorkSheet.Columns.AutoFit()

        Next

        'delete the first sheet
        excelApp.DisplayAlerts = False
        Dim lastWorksheet As EXCEL.Worksheet = excelWorkBook.Worksheets(1)
        lastWorksheet.Delete()
        excelApp.DisplayAlerts = True

        excelWorkBook.SaveAs(path, def, def, def, False, def, EXCEL.XlSaveAsAccessMode.xlNoChange, def, def, def, def, def)
        excelWorkBook.Close()
        excelApp.Quit()

        MsgBox("Export Complete \n Find file at:\n " & path, MsgBoxStyle.Information, Title:="Excel Export Report")
    End Sub
End Class
