'
' (C) Copyright 2004-2009 by Autodesk, Inc.
' 
' 
' By using this code, you are agreeing to the terms
' and conditions of the License Agreement that appeared
' and was accepted upon download or installation
' (or in connection with the download or installation)
' of the Autodesk software in which this code is included.
' All permissions on use of this code are as set forth
' in such License Agreement provided that the above copyright
' notice appears in all authorized copies and that both that
' copyright notice and the limited warranty and
' restricted rights notice below appear in all supporting 
' documentation.
' 
' AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS. 
' AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
' MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
' DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
' UNINTERRUPTED OR ERROR FREE.
' 
' Use, duplication, or disclosure by the U.S. Government is subject to 
' restrictions set forth in FAR 52.227-19 (Commercial Computer
' Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
' (Rights in Technical Data and Computer Software), as applicable.

' ImportExportVB.vb

Imports System.IO
Imports System.Collections
Imports System.Collections.Specialized
Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.Runtime
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.Gis.Map
Imports Autodesk.Gis.Map.ImportExport
Imports Autodesk.Gis.Map.ObjectData
Imports Autodesk.Gis.Map.Project

Namespace ImportExport
    ''' <summary>
    ''' Event handlers for export.
    ''' </summary>
    Public NotInheritable Class MyExpEventHandlerVB
        Public Sub New(ByVal sw As StreamWriter)
            m_out = sw
        End Sub

        Public Shared Sub RecordReadyForExport(ByVal sender As Object, ByVal args As Autodesk.Gis.Map.ImportExport.RecordReadyForExportEventArgs)
            args.ContinueExport = True
        End Sub

        Public Shared Sub RecordExported(ByVal sender As Object, ByVal args As Autodesk.Gis.Map.ImportExport.RecordExportedEventArgs)
        End Sub

        Public Shared Sub RecordError(ByVal sender As Object, ByVal args As ExportRecordErrorEventArgs)
            Dim errorMsg As String = args.Error
            If Not (errorMsg Is Nothing) Then
                If errorMsg.Length = 0 Then
                    Dim outMsg As String = vbNewLine & "Entity"
                    m_out.WriteLine(outMsg)
                End If
            End If
        End Sub

        Private Shared m_out As StreamWriter
    End Class

    ''' <summary>
    ''' Event handlers for import.
    ''' </summary>
    Public NotInheritable Class MyImpEventHandlerVB
        Public Sub New()
            m_importedIds = New ObjectIdCollection()
        End Sub

        Private Shared m_importedIds As ObjectIdCollection
        Public ReadOnly Property ImportedEntities() As ObjectIdCollection
            Get
                Return m_importedIds
            End Get
        End Property

        Public Shared Sub RecordReadyForImport(ByVal sender As Object, ByVal args As RecordReadyForImportEventArgs)
            args.ContinueImport = True
        End Sub

        Public Shared Sub RecordImported(ByVal sender As Object, ByVal args As RecordImportedEventArgs)
            m_importedIds.Add(args.ObjectId)
        End Sub

        Public Shared Sub RecordError(ByVal sender As Object, ByVal args As ImportRecordErrorEventArgs)
        End Sub
    End Class

    Public NotInheritable Class ImportExportCommands

        Private m_ExpEventHandler As MyExpEventHandlerVB
        Private m_ImpEventHandler As MyImpEventHandlerVB


#Region " Singleton "
        Private Shared m_Instance As ImportExportCommands = New ImportExportCommands()
        Private Sub ImportExportCommands()
        End Sub
        Public Shared ReadOnly Property Instance() As ImportExportCommands
            Get
                Return m_Instance
            End Get
        End Property

        Public ReadOnly Property ImportEventManager() As MyImpEventHandlerVB
            Get
                Return m_ImpEventHandler
            End Get
        End Property

#End Region



        ''' <summary>
        ''' Exports all the entities or exporting entities layer by layer.
        ''' </summary>
        Public Sub DoExport(ByVal format As String, ByVal expFile As String, ByVal layerFilter As String, ByVal logFile As String, ByVal isTable As Boolean, ByVal isLinkTemplate As Boolean)
            Utility.ShowMsg("DoExport" & vbNewLine)

            Dim msg As String = Nothing
            Dim fs As FileStream = New FileStream(logFile, FileMode.Append)
            Dim log As StreamWriter = New StreamWriter(fs)

            Try
                'Get current time and log the time of executing exporting.
                Dim time As DateTime = DateTime.UtcNow
                log.WriteLine(time.ToLocalTime.ToString())
                log.WriteLine("Exporting file " & expFile)

                Dim mapSession As MapApplication = HostMapApplicationServices.Application
                Dim exporter As Exporter = mapSession.Exporter

                'Initiate the exporter
                exporter.Init(format, expFile)
                'Initiate myExpReactor with the log file.
                'Attach event handler to the exporter
                If m_ExpEventHandler Is Nothing Then
                    m_ExpEventHandler = New MyExpEventHandlerVB(log)
                    AddHandler exporter.RecordReadyForExport, AddressOf MyExpEventHandlerVB.RecordReadyForExport
                    AddHandler exporter.RecordExported, AddressOf MyExpEventHandlerVB.RecordExported
                    AddHandler exporter.ExportRecordError, AddressOf MyExpEventHandlerVB.RecordError
                End If

                'Get Data mapping object
                Dim dataMapping As ExpressionTargetCollection = Nothing
                dataMapping = exporter.GetExportDataMappings()
                'Set Object Data data mapping if table is true
                If isTable And Not MapODData(dataMapping) Then
                    log.WriteLine("Error in mapping OD table data!")
                End If

                'Reset Data mapping with Object data and Link template keys	
                exporter.SetExportDataMappings(dataMapping)

                'If layerFilter isn't Nothing, set the layer filter to export layer by layer
                log.WriteLine("Check for layerfilter")
                If Not (layerFilter Is Nothing) Then
                    log.WriteLine("Set layerfilter " & layerFilter)
                    exporter.LayerFilter = layerFilter
                End If
                log.WriteLine("Start Export")
                'Do the exporting and log the result
                Dim results As ExportResults
                results = exporter.Export(True)
                msg = String.Format("    {0} objects are exported.", results.EntitiesExported.ToString())
                log.WriteLine(msg)
                msg = String.Format("    {0} objects are skipped.", results.EntitiesSkippedCouldNotTransform.ToString())
                log.WriteLine(msg)

            Catch e As MapImportExportException
                log.WriteLine(e.Message)
            Finally
                log.WriteLine()
                log.Close()
            End Try

        End Sub

        ''' <summary>
        ''' Gets all the layers in the current drawing in a string collection.
        ''' </summary>
        ''' <param name="layerNames">[in] Storage of the layer names, cannot be null.</param>
        ''' <returns>Returns true if successful.</returns>
        ''' <remarks>Throws no exceptions in common condition.</remarks>
        Public Function GetLayers(ByVal layerName As StringCollection) As Boolean
            Dim layerTable As LayerTable = Nothing
            Dim db As Database = Nothing
            db = HostApplicationServices.WorkingDatabase

            Dim trans As Transaction = Nothing
            Try
                trans = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.TransactionManager.StartTransaction()
                layerTable = CType(trans.GetObject(db.LayerTableId, OpenMode.ForRead), LayerTable)

                Dim id As ObjectId
                For Each id In layerTable
                    Dim tableRecord As LayerTableRecord = Nothing
                    tableRecord = CType(trans.GetObject(id, OpenMode.ForRead), LayerTableRecord)
                    layerName.Add(tableRecord.Name)
                Next

                trans.Commit()
                trans = Nothing
            Catch e As Exception
                Return False
            Finally
                If Not trans Is Nothing Then
                    trans.Abort()
                End If
                trans = Nothing
            End Try

            Return True
        End Function

        ''' <summary>
        ''' Map Object data to attribute fields in the exported-to file.
        ''' </summary>
        Public Function MapODData(ByVal mapping As ExpressionTargetCollection)
            Dim mapApi As MapApplication = Nothing
            Dim proj As ProjectModel = Nothing
            Dim tables As Tables = Nothing
            Dim table As ObjectData.Table = Nothing
            Dim tableNames As StringCollection = Nothing
            Dim definition As ObjectData.FieldDefinitions = Nothing

            'Get map session and all the OD tables
            Dim mapSession As MapApplication = HostMapApplicationServices.Application()
            mapApi = mapSession
            proj = mapApi.ActiveProject
            tables = proj.ODTables
            tableNames = tables.GetTableNames

            'Iterate through the OD table definition and get all the field names
            Dim tblCount As Integer = tables.TablesCount
            Dim i As Integer
            Dim j As Integer
            Try
                For i = 0 To tblCount - 1
                    table = tables(tableNames(i))
                    definition = table.FieldDefinitions
                    For j = 0 To definition.Count - 1
                        Dim column As FieldDefinition = Nothing
                        column = definition(j)
                        'fieldName is the OD table field name in the data mapping. It should be 
                        'in the format:fieldName&tableName. 
                        'newFieldName is the attribute field name of exported-to file
                        'It is set to ODFieldName_ODTableName in this sample
                        Dim newFieldName As String = Nothing
                        Dim fieldName As String = Nothing
                        fieldName = String.Concat(":", column.Name)
                        fieldName = String.Concat(fieldName, "@")
                        fieldName = String.Concat(fieldName, tableNames(i))
                        newFieldName = String.Concat(column.Name, "_")
                        newFieldName = String.Concat(newFieldName, tableNames(i))
                        mapping.Add(fieldName, newFieldName)
                    Next
                Next
            Catch e As MapException
                Return False
            End Try
            Return True
        End Function

        ''' <summary>
        ''' This function executes the importing.
        ''' </summary>
        Public Function ImportDwg(ByVal format As String, ByVal fileName As String, ByVal isToLayer0 As Boolean, ByVal isImpAttToOD As Boolean)
            Dim mapSession As MapApplication = HostMapApplicationServices.Application
            Dim importer As Importer = mapSession.Importer

            Try
                importer.Init(format, fileName)

                Dim layer As InputLayer = Nothing
                For Each layer In importer
                    Dim newLayerName As String = Nothing
                    If Not ChangeLayerName(layer, newLayerName, isToLayer0) Then
                        Utility.ShowMsg("Failed to set the layer name." & vbNewLine)
                        GoTo NEXTWHILE
                    End If

                    'Set the table name as layername_OD
                    If isImpAttToOD Then
                        If Not SetTableName(layer, newLayerName) Then
                            Utility.ShowMsg("Failed to set the table name." & vbNewLine)
                            GoTo NEXTWHILE
                        End If

                        Dim col As Autodesk.Gis.Map.ImportExport.Column = Nothing
                        For Each col In layer
                            If Not SetColumnNames(col) Then
                                Utility.ShowMsg("Failed to set OD table field names" & vbNewLine)
                                GoTo NEXTWHILE
                            End If
                        Next col
                    End If ' isImpAttToOD

NEXTWHILE:
                Next layer

                'Attach event handler
                If m_ImpEventHandler Is Nothing Then
                    m_ImpEventHandler = New MyImpEventHandlerVB()
                    AddHandler importer.RecordReadyForImport, AddressOf MyImpEventHandlerVB.RecordReadyForImport
                    AddHandler importer.RecordImported, AddressOf MyImpEventHandlerVB.RecordImported
                    AddHandler importer.ImportRecordError, AddressOf MyImpEventHandlerVB.RecordError
                End If

                'Do importing and print outMsg the result.
                Dim results As ImportResults
                results = importer.Import(False)

                Dim msg As String = Nothing
                msg = String.Format("{0} entities are imported" & vbNewLine, results.EntitiesImported.ToString())
                Utility.ShowMsg(msg)
                msg = String.Format("{0} entities are skipped" & vbNewLine, results.EntitiesSkippedCouldNotTransform.ToString())
                Utility.ShowMsg(msg)
                msg = String.Format("{0} entities are with color close to the background" & vbNewLine, results.EntitiesWithColorCloseToBackground.ToString())
                Utility.ShowMsg(msg)

                Return True
            Catch e As MapException
                Return False
            End Try
        End Function

        ''' <summary>
        ''' This function sets the import-to layer name to IMP_importFileName.
        ''' If the importFileName is longer than LayerNameLength, truncate it to LayerNameLength.
        ''' If the importFileName contain characters other than alphabet, number, replace it with '_'.
        ''' </summary>
        ''' <returns>Returns true if succeeds.</returns>
        Public Function ChangeLayerName(ByVal layer As InputLayer, ByRef layerName As String, ByVal isToLayer0 As Boolean)
            Dim layerNameType As LayerNameType = LayerNameType.LayerNameDirect

            Try
                If isToLayer0 Then
                    layerName = "0"
                Else
                    Dim existingName As String = Nothing
                    existingName = layer.Name

                    Dim tmp As Char
                    For Each tmp In existingName
                        If tmp < "A" Or tmp < "Z" Then
                            If tmp < "a" Or tmp < "z" Then
                                If tmp < "0" Or tmp < "9" Then
                                    existingName.Replace(tmp, "_")
                                End If
                            End If
                        End If
                    Next

                    layerName = String.Concat("IMP_", existingName)
                End If

                layer.SetLayerName(layerNameType, layerName)
                Return True
            Catch
                Return False
            End Try
        End Function

        ''' <summary>
        ''' This function set the OD table name to layerName_OD.
        ''' </summary>
        Public Function SetTableName(ByVal layer As InputLayer, ByVal layerName As String) As Boolean
            Dim tableType As ImportDataMapping
            tableType = ImportDataMapping.NewObjectDataOnly
            Dim tableName As String = String.Concat(layerName, "_OD")
            Dim newTableName As String = Nothing
            If TableNameExist(tableName) Then
                Dim index As Integer = 1
                Do
                    newTableName = String.Concat(tableName, index.ToString())
                    index = index + 1
                Loop Until TableNameExist(newTableName) = False
                layer.SetDataMapping(tableType, newTableName)
            Else
                layer.SetDataMapping(tableType, tableName)
            End If

            Return True
        End Function

        ''' <summary>
        ''' Check if an OD table name already exist in the database.
        ''' </summary>
        Public Function TableNameExist(ByVal tableName As String) As Boolean
            Try
                Dim mapApi As MapApplication = Nothing
                Dim proj As ProjectModel = Nothing
                Dim tables As Tables = Nothing

                Dim mapSession As MapApplication = HostMapApplicationServices.Application
                mapApi = mapSession
                proj = mapApi.ActiveProject
                tables = proj.ODTables

                If tables.IsTableDefined(tableName) Then
                    Return True
                Else
                    Return False
                End If
            Catch
                Return False
            End Try
        End Function

        ''' <summary>
        ''' This function iterates all the attributes in import-from file
        ''' and set the OD field name to be IMP_attributeName
        ''' </summary>
        Public Function SetColumnNames(ByVal col As Autodesk.Gis.Map.ImportExport.Column) As Boolean
            Dim colName As String = col.ColumnName
            Dim newFieldName As String = Nothing
            newFieldName = String.Concat("IESample_", colName)
            Try
                col.SetColumnDataMapping(newFieldName)
            Catch
                Return False
            End Try
            Return True
        End Function

        ''' <summary>
        ''' Changes the entities color to the selected display color.
        ''' </summary>
        ''' <param name="entityIdCollection">[in] The entity Ids to be high-lighted. </param>
        ''' <param name="displayColor">[in] The display color index. </param>
        ''' <param name="lockedEntityCount">[out] The locked entity count. </param>
        Public Sub HighlightEntities(ByVal entityIdCollection As ObjectIdCollection, ByVal displayColor As Int32, ByRef lockedEntityCount As Long)
            Dim trans As Transaction = Nothing
            Try
                trans = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.TransactionManager.StartTransaction()
                Dim id As ObjectId
                For Each id In entityIdCollection
                    Try
                        Dim entity As Entity = trans.GetObject(id, OpenMode.ForWrite)

                        ' Additional operations for BlockReference
                        ' Should care about the entities in the block
                        If TypeOf entity Is BlockReference Then
                            Dim blkRef As BlockReference = entity

                            Dim blkTblRecord As BlockTableRecord = trans.GetObject(blkRef.BlockTableRecord, OpenMode.ForRead)

                            Dim objId As ObjectId
                            For Each objId In blkTblRecord
                                ' No catching here to simplify the logic
                                Dim blkEntity As Entity = trans.GetObject(objId, OpenMode.ForWrite)
                                If Not (blkEntity Is Nothing) Then blkEntity.ColorIndex = displayColor
                            Next
                        End If

                        ' Set the color of the entity no matter if it is a BlockReference
                        entity.ColorIndex = displayColor
                    Catch e As MapException
                        ' locked entity found
                        If e.ErrorCode = ErrorStatus.OnLockedLayer Then

                            lockedEntityCount = lockedEntityCount + 1
                        End If
                    End Try
                Next

                trans.Commit()
                trans = Nothing

            Catch e As System.Exception
                Utility.ShowMsg(e.Message)
            Finally
                If Not trans Is Nothing Then
                    trans.Abort()
                End If
                trans = Nothing
            End Try
        End Sub

    End Class

    Public NotInheritable Class Utility
        Private Sub New()
        End Sub

        Public Shared Sub ShowMsg(ByVal msg As System.String)
            AcadEditor.WriteMessage(msg)
        End Sub

        Public Shared ReadOnly Property AcadEditor() As Autodesk.AutoCAD.EditorInput.Editor
            Get
                Return Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor
            End Get
        End Property
    End Class

    Public NotInheritable Class ImportExportSampleApplication
        Implements IExtensionApplication
        Public Sub Initialize() Implements IExtensionApplication.Initialize
            Utility.ShowMsg(vbNewLine & " ImportExportVB sample application initialized. " & vbNewLine)
        End Sub

        Public Sub Terminate() Implements IExtensionApplication.Terminate
            Utility.ShowMsg(vbNewLine & " ImportExportVB sample application terminated. " & vbNewLine)
        End Sub
    End Class
End Namespace
