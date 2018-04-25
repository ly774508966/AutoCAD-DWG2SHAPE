Imports System.Collections.Specialized
Imports System.IO
Imports System.Windows.Forms
Imports RN_Shape_Export.ImportExport

Public Class frmExport
    Private Sub buttonExpFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonExpFile.Click
        Dim saveFileDialog1 As SaveFileDialog = New SaveFileDialog()

        saveFileDialog1.InitialDirectory = "c:\"
        'regel uitgezet, .tab werkt niet
        'saveFileDialog1.Filter = "MIF files (*.mif)|*.mif|MAPINFO files (*.tab)|*.tab"
        saveFileDialog1.Filter = "MIF files (*.mif)|*.mif"
        saveFileDialog1.FilterIndex = 1
        saveFileDialog1.RestoreDirectory = False
        saveFileDialog1.CheckFileExists = False

        If (saveFileDialog1.ShowDialog() = DialogResult.OK) Then
            textBoxExpFile.Text = saveFileDialog1.FileName
            textBoxLogFile.Text = Path.GetDirectoryName(saveFileDialog1.FileName) & "\LogFile.txt"
        End If
    End Sub

    Private Sub buttonLogFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonLogFile.Click
        Dim saveFileDialog1 As SaveFileDialog = New SaveFileDialog()

        saveFileDialog1.InitialDirectory = "c:\"
        saveFileDialog1.Filter = "log files (*.log)|*.log|text files (*.txt)|*.txt"
        saveFileDialog1.FilterIndex = 1
        saveFileDialog1.RestoreDirectory = False
        saveFileDialog1.CheckFileExists = False

        If (saveFileDialog1.ShowDialog() = DialogResult.OK) Then
            textBoxLogFile.Text = saveFileDialog1.FileName
        End If
    End Sub

    Private Sub buttonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonOK.Click
        Dim sc As ImportExportCommands = New ImportExportCommands()
        'Check if user want to export OD table
        Dim isODTable As Boolean = Me.checkBoxExpOD.Checked
        Dim expFileName As String = Me.textBoxExpFile.Text
        Dim logFileName As String = Me.textBoxLogFile.Text
        Dim format As String = Nothing
        Dim extension As String = Nothing

        ' Check the file names
        If expFileName.Length = 0 Then
            MessageBox.Show("Please specify the export file name.")
            Return
        End If
        If logFileName.Length = 0 Then
            MessageBox.Show("Please specify the log file name.")
            Return
        End If

        'If file extension is ".mif" then the import format is "MIF"
        If (expFileName.Chars(expFileName.Length - 1) = "f" Or expFileName.Chars(expFileName.Length - 1) = "F") Then
            format = "MIF"
            extension = ".mif"
            'If file extension is ".tab" then the import format is "MAPINFO"
        ElseIf (expFileName.Chars(expFileName.Length - 1) = "b" Or expFileName.Chars(expFileName.Length - 1) = "B") Then
            format = "MAPINFO"
            extension = ".tab"
        Else
            MessageBox.Show("Invalid file name" & vbNewLine)
            Return
        End If

        If Me.radioButtonAllToOne.Checked Then
            sc.DoExport(format, expFileName, Nothing, logFileName, isODTable, False)
        Else
            Dim layerNames As StringCollection = New StringCollection()
            If sc.GetLayers(layerNames) Then
                Dim layerName As String = Nothing
                For Each layerName In layerNames
                    Dim expLayerFileName As String = String.Copy(expFileName)
                    expLayerFileName = expLayerFileName.Insert(expLayerFileName.Length - 4, "_")
                    expLayerFileName = expLayerFileName.Insert(expLayerFileName.Length - 4, layerName)
                    sc.DoExport(format, expLayerFileName, layerName, logFileName, isODTable, False)
                Next
            End If
        End If

        Close()
    End Sub

    Private Sub buttonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonCancel.Click
        Close()
    End Sub
End Class