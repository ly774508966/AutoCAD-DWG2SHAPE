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

Imports System.Collections.Specialized
Imports System.IO
Imports System.Reflection
Imports System.Windows.Forms
Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.EditorInput
Imports Autodesk.AutoCAD.Geometry
Imports RN_Shape_Export.ImportExport
Imports GeometryExtensions

Public Class frmExport
    Dim acDoc As Document = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument
    Dim acCurDb As Database = acDoc.Database
    Dim acEd As Editor = acDoc.Editor
    Dim commandActive As Boolean = True
    Dim commandFailled As Boolean = False
    Dim FailedLayers As String = ""
    Dim currLayer As String = ""
    Dim iCurrLayer As Integer = 0

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

    Private Sub cmdHatchToBoundary_Click(sender As Object, e As EventArgs) Handles cmdHatchToBoundary.Click
        If MsgBox("Hiermee worden de Arceringen omgezet naar Poylylines, de originele Arceringen worden verwijderd!" & vbCrLf & "Wilt u doorgaan?", vbYesNo) = vbYes Then
            'set focus to modal space
            'Autodesk.AutoCAD.Internal.Utils.SetFocusToDwgView()
            'RHB()
            LoopLayers(0)
        End If
    End Sub

    Public Sub LoopLayers(iStart As Integer)
        TextBox1.AppendText("Start Loop with Value: " & iStart.ToString & " - " & iCurrLayer.ToString & vbCrLf)
        TextBox1.Update()
        Me.Update()
        Dim sc As ImportExportCommands = New ImportExportCommands()
        Dim layerNames As StringCollection = New StringCollection()
        Dim iTeller As Integer = 0
        iCurrLayer = iStart
        If sc.GetLayers(layerNames) Then
            TextBox1.AppendText("LayerCount: " & layerNames.Count.ToString & vbCrLf)
            TextBox1.Update()
            Me.Update()
            If iCurrLayer = layerNames.Count Then
                MsgBox("Voltooid")
                Exit Sub
            End If
            Dim layerName As String = Nothing
            acEd.Regen()
            For Each layerName In layerNames

                If iTeller = iCurrLayer Then
                    If layerName = "0" Or layerName = "Defpoints" Then
                        iCurrLayer = iCurrLayer + 1
                    Else
                        currLayer = layerName
                        SetCurrentLayer(currLayer)
                        TextBox1.AppendText("Start Isolating: " & currLayer & vbCrLf)
                        TextBox1.Update()
                        Me.Update()
                        If IsolateLayer(layerName) Then

                        End If
                        acEd.ApplyCurDwgLayerTableChanges()
                        acEd.Regen()
                        Threading.Thread.Sleep(5000)
                        TextBox1.AppendText("Start Boundary generation: " & vbCrLf)
                        TextBox1.Update()
                        Me.Update()
                        If HatchToBoundary() = True Then
                            'success
                            acEd.Regen()
                        End If
                        IsolateLayer("", True)
                        'cancel last active command
                        acDoc.SendStringToExecute(Chr(27), True, False, True)
                        acEd.Regen()
                        TextBox1.AppendText("Ending Loop: " & iStart.ToString & vbCrLf)
                        TextBox1.Update()
                        Me.Update()
                        Threading.Thread.Sleep(5000)
                        Exit Sub
                    End If
                End If
                iTeller = iTeller + 1
            Next

            'If DeleteHatches() Then
            '    MsgBox("Voltooid")
            'End If

        End If
    End Sub

    Public Sub SetCurrentLayer(sLayer As String)
        Using acLockDoc As DocumentLock = acDoc.LockDocument()
            '' Start a transaction
            Using acTrans As Transaction = acCurDb.TransactionManager.StartTransaction()
                '' Open the Layer table for read
                Dim LayerTable As LayerTable = acTrans.GetObject(acCurDb.LayerTableId, OpenMode.ForRead)
                For Each ObjectId In LayerTable
                    Dim LayerTableRecord As LayerTableRecord = acTrans.GetObject(ObjectId, OpenMode.ForWrite)
                    Try
                        If LayerTableRecord.Name = sLayer Then
                            LayerTableRecord.IsOff = False
                            LayerTableRecord.IsFrozen = False
                            acCurDb.Clayer = LayerTable(sLayer)
                            TextBox1.AppendText("### Set Curr Layer: " & sLayer & vbCrLf)
                            Exit For
                        End If
                        TextBox1.Update()
                        Me.Update()
                    Catch ex As Exception

                    End Try
                Next
                '' Save the changes and dispose of the transaction
                acTrans.Commit()
            End Using
        End Using
    End Sub


    Public Function IsolateLayer(sLayer As String, Optional bUnisoAll As Boolean = False)
        Using acLockDoc As DocumentLock = acDoc.LockDocument()
            '' Start a transaction
            Using acTrans As Transaction = acCurDb.TransactionManager.StartTransaction()
                '' Open the Layer table for read
                Dim tmpLayer As String
                Dim LayerTable As LayerTable = acTrans.GetObject(acCurDb.LayerTableId, OpenMode.ForWrite)
                For Each ObjectId In LayerTable
                    Dim LayerTableRecord As LayerTableRecord = acTrans.GetObject(ObjectId, OpenMode.ForWrite)
                    Try
                        tmpLayer = LayerTableRecord.Name
                        If bUnisoAll = True Then
                            LayerTableRecord.IsOff = False
                            LayerTableRecord.IsFrozen = False
                            acEd.WriteMessage("Curr layer: " & sLayer & vbCr)
                        Else
                            If LayerTableRecord.Name = sLayer Then
                                LayerTableRecord.IsOff = False
                                LayerTableRecord.IsFrozen = False
                                'acCurDb.Clayer() = LayerTable(sLayer)
                                TextBox1.AppendText("Curr Layer: " & sLayer & vbCrLf)
                            Else
                                LayerTableRecord.IsOff = True
                                LayerTableRecord.IsFrozen = True
                                TextBox1.AppendText("Off / Freeze Layer: " & LayerTableRecord.Name & vbCrLf)
                            End If
                        End If
                        TextBox1.Update()
                        Me.Update()
                    Catch ex As Exception
                        TextBox1.AppendText("EXCEPTION FOR LAYER: " & tmpLayer & vbCr)
                        TextBox1.Update()
                        Me.Update()
                    End Try
                Next
                '' Save the changes and dispose of the transaction
                acTrans.Commit()
                acEd.ApplyCurDwgLayerTableChanges()
                acDoc.SendStringToExecute("REGENALL" & vbCr, True, False, True)
            End Using
        End Using
        listLayerState()
        Return True
    End Function

    Public Function listLayerState()
        Using acLockDoc As DocumentLock = acDoc.LockDocument()
            '' Start a transaction
            TextBox1.AppendText("$$$$$ Layer State List: " & currLayer.ToUpper & vbCrLf)
            Using acTrans As Transaction = acCurDb.TransactionManager.StartTransaction()
                '' Open the Layer table for read
                Dim LayerTable As LayerTable = acTrans.GetObject(acCurDb.LayerTableId, OpenMode.ForWrite)
                For Each ObjectId In LayerTable
                    Dim LayerTableRecord As LayerTableRecord = acTrans.GetObject(ObjectId, OpenMode.ForWrite)
                    Try
                        If LayerTableRecord.Name = currLayer Then
                            TextBox1.AppendText("CURRENT ")
                        End If
                        TextBox1.AppendText("Layer: " & LayerTableRecord.Name & " " & LayerTableRecord.IsOff.ToString & " " & LayerTableRecord.IsFrozen.ToString & vbCrLf)
                        TextBox1.Update()
                        Me.Update()
                    Catch ex As Exception

                    End Try
                Next
                '' Save the changes and dispose of the transaction
                acTrans.Commit()
            End Using
        End Using
        Return True
    End Function


    Public Sub FreezeAllLayers(sLayer As String, sPrefLayer As String)
        Using acLockDoc As DocumentLock = acDoc.LockDocument()
            '' Start a transaction
            Using acTrans As Transaction = acCurDb.TransactionManager.StartTransaction()
                '' Open the Layer table for read
                Dim LayerTable As LayerTable = acTrans.GetObject(acCurDb.LayerTableId, OpenMode.ForRead)
                For Each ObjectId In LayerTable
                    Dim LayerTableRecord As LayerTableRecord = acTrans.GetObject(ObjectId, OpenMode.ForWrite)
                    Try
                        If LayerTableRecord.Name = sLayer Then
                            LayerTableRecord.IsOff = False
                            LayerTableRecord.IsFrozen = False
                            'acCurDb.Clayer() = LayerTable(sLayer)
                            TextBox1.AppendText("Curr Layer: " & sLayer & vbCrLf)
                        Else
                            LayerTableRecord.IsOff = True
                            LayerTableRecord.IsFrozen = True
                            TextBox1.AppendText("Off / Freeze Layer: " & LayerTableRecord.Name & vbCrLf)
                        End If
                        TextBox1.Update()
                        Me.Update()
                    Catch ex As Exception
                        'TextBox1.AppendText("EXCEPTION FOT LAYER: " & tmpLayer & vbCr)
                        'TextBox1.Update()
                        'Me.Update()
                    End Try
                Next
                '' Save the changes and dispose of the transaction
                acTrans.Commit()
            End Using
        End Using
    End Sub

    Public Function HatchToBoundary()
        '_HATCHGENERATEBOUNDARY
        TextBox1.AppendText("Boundary for: " & currLayer & vbCrLf)
        TextBox1.Update()
        Me.Update()
        AddHandlers()
        acDoc.SendStringToExecute("_HATCHGENERATEBOUNDARY" & vbCr & "ALL" & vbCr & vbCr, True, False, True)
        While m_Command IsNot Nothing

        End While
        TextBox1.AppendText("Sleep mode entering: " & currLayer & vbCrLf)
        TextBox1.Update()
        Me.Update()
        Threading.Thread.Sleep(500)
        TextBox1.AppendText("End boundary: " & currLayer & vbCrLf)
        TextBox1.Update()
        Me.Update()
        Return True
    End Function

    Public Function DeleteHatches(Optional bGetSelectionCount As Boolean = False)
        Dim acTypValAr(0) As TypedValue
        acTypValAr.SetValue(New TypedValue(DxfCode.Start, "HATCH"), 0)

        Dim acSelFtr As SelectionFilter = New SelectionFilter(acTypValAr)

        Dim acSSPrompt As PromptSelectionResult
        Using acLockDoc As DocumentLock = acDoc.LockDocument()
            '' Start a transaction
            Using acTrans As Transaction = acCurDb.TransactionManager.StartTransaction()
                acSSPrompt = acEd.GetSelection(acSelFtr)
                If acSSPrompt.Status = PromptStatus.OK Then
                    Dim acSSet As SelectionSet = acSSPrompt.Value
                    If bGetSelectionCount = True Then
                        Return acSSet.Count
                    Else
                        'Autodesk.AutoCAD.ApplicationServices.Application.ShowAlertDialog("Number of objects selected: " & acSSet.Count.ToString())
                        For Each acSSObj As SelectedObject In acSSet
                            '' Check to make sure a valid SelectedObject object was returned
                            If Not IsDBNull(acSSObj) Then
                                '' Open the selected object for write
                                Dim acEnt As Entity = acTrans.GetObject(acSSObj.ObjectId, OpenMode.ForWrite)

                                If Not IsDBNull(acEnt) Then
                                    '' Change the object's color to Green
                                    'acEd.WriteMessage("Ent: " & acEnt.GetType.ToString & vbCr)
                                    TextBox1.AppendText(acEnt.GetType.ToString & vbCrLf)
                                    acEnt.Erase()
                                End If
                            End If
                        Next
                        acTrans.Commit()
                        Return True
                    End If

                Else
                    'Autodesk.AutoCAD.ApplicationServices.Application.ShowAlertDialog("Number of objects selected: 0")
                    If bGetSelectionCount = True Then
                        Return 0
                    Else
                        Return True
                    End If
                End If
            End Using
        End Using


    End Function

    Private m_Command As String = Nothing

    Sub AddHandlers()
        Dim d As Document = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument
        AddHandler d.CommandWillStart, AddressOf CommandWillStart
        AddHandler d.CommandEnded, AddressOf CommandEnded
        AddHandler d.CommandFailed, AddressOf CommandFailed
        AddHandler d.CommandCancelled, AddressOf CommandCancelled
        commandFailled = False
        commandActive = True
    End Sub

    Sub RemoveHandlers()
        Dim d As Document = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument
        RemoveHandler d.CommandWillStart, AddressOf CommandWillStart
        RemoveHandler d.CommandEnded, AddressOf CommandEnded
        RemoveHandler d.CommandFailed, AddressOf CommandFailed
        RemoveHandler d.CommandCancelled, AddressOf CommandCancelled
        m_Command = Nothing
        commandActive = False
        TextBox1.AppendText("#######  Remove Handler" & vbCrLf)
        TextBox1.Update()
        If commandFailled = True Then
            FailedLayers = currLayer & vbCrLf
        End If
        iCurrLayer = iCurrLayer + 1
        LoopLayers(iCurrLayer)
    End Sub

    Sub CommandWillStart(ByVal sender As Object, ByVal e As Autodesk.AutoCAD.ApplicationServices.CommandEventArgs)
        If m_Command Is Nothing Then m_Command = e.GlobalCommandName
    End Sub

    Sub CommandEnded(ByVal sender As Object, ByVal e As Autodesk.AutoCAD.ApplicationServices.CommandEventArgs)
        If e.GlobalCommandName = m_Command Then

            RemoveHandlers()
        End If
    End Sub

    Sub CommandCancelled(ByVal sender As Object, ByVal e As Autodesk.AutoCAD.ApplicationServices.CommandEventArgs)
        commandFailled = True
        RemoveHandlers()
    End Sub

    Sub CommandFailed(ByVal sender As Object, ByVal e As Autodesk.AutoCAD.ApplicationServices.CommandEventArgs)
        commandFailled = True
        RemoveHandlers()
    End Sub

    Public Sub RHB()
        'source: https://forums.autodesk.com/t5/net/restore-hatch-boundaries-if-they-have-been-lost-with-net/m-p/3786748#M33601
        Dim doc As Document = acDoc
        Dim ed As Editor = doc.Editor
        Dim prOps As PromptEntityOptions = New PromptEntityOptions(vbLf & "Select Hatch: ")
        prOps.SetRejectMessage(vbLf & "Not a Hatch")
        prOps.AddAllowedClass(GetType(Hatch), False)
        Dim prRes As PromptEntityResult = ed.GetEntity(prOps)
        If prRes.Status <> PromptStatus.OK Then Return
        Dim oIdColl As ObjectIdCollection = New ObjectIdCollection()
        Dim bIsPolyBound As Boolean = False
        Using tr As Transaction = doc.TransactionManager.StartTransaction()
            Dim hatch As Hatch = TryCast(tr.GetObject(prRes.ObjectId, OpenMode.ForRead), Hatch)
            If hatch IsNot Nothing Then
                Dim btr As BlockTableRecord = TryCast(tr.GetObject(hatch.OwnerId, OpenMode.ForWrite), BlockTableRecord)
                If btr Is Nothing Then Return
                Dim plane As Plane = hatch.GetPlane()
                Dim nLoops As Integer = hatch.NumberOfLoops
                For i As Integer = 0 To nLoops - 1
                    Dim [loop] As HatchLoop = hatch.GetLoopAt(i)
                    If [loop].IsPolyline Then
                        Using poly As Polyline = New Polyline()
                            Dim iVertex As Integer = 0
                            For Each bv As BulgeVertex In [loop].Polyline
                                poly.AddVertexAt(Math.Min(System.Threading.Interlocked.Increment(iVertex), iVertex - 1), bv.Vertex, bv.Bulge, 0, 0)
                            Next
                            btr.AppendEntity(poly)
                            tr.AddNewlyCreatedDBObject(poly, True)
                            oIdColl.Add(btr.ObjectId)
                        End Using
                    Else
                        For Each cv As Curve2d In [loop].Curves
                            Dim line2d As LineSegment2d = TryCast(cv, LineSegment2d)
                            Dim arc2d As CircularArc2d = TryCast(cv, CircularArc2d)
                            Dim ellipse2d As EllipticalArc2d = TryCast(cv, EllipticalArc2d)
                            Dim spline2d As NurbCurve2d = TryCast(cv, NurbCurve2d)
                            If line2d IsNot Nothing Then
                                Using ent As Line = New Line()
                                    ent.StartPoint = New Point3d(plane, line2d.StartPoint)
                                    ent.EndPoint = New Point3d(plane, line2d.EndPoint)
                                    btr.AppendEntity(ent)
                                    tr.AddNewlyCreatedDBObject(ent, True)
                                    oIdColl.Add(btr.ObjectId)
                                End Using
                            ElseIf arc2d IsNot Nothing Then
                                If Math.Abs(arc2d.StartAngle - arc2d.EndAngle) < 0.00001 Then
                                    Using ent As Circle = New Circle(New Point3d(plane, arc2d.Center), plane.Normal, arc2d.Radius)
                                        btr.AppendEntity(ent)
                                        tr.AddNewlyCreatedDBObject(ent, True)
                                    End Using
                                Else
                                    Dim angle As Double = New Vector3d(plane, arc2d.ReferenceVector).AngleOnPlane(plane)
                                    Using ent As Arc = New Arc(New Point3d(plane, arc2d.Center), arc2d.Radius, arc2d.StartAngle + angle, arc2d.EndAngle + angle)
                                        btr.AppendEntity(ent)
                                        tr.AddNewlyCreatedDBObject(ent, True)
                                        oIdColl.Add(btr.ObjectId)
                                    End Using
                                End If
                            ElseIf ellipse2d IsNot Nothing Then
                                Using ent As Ellipse = New Ellipse(New Point3d(plane, ellipse2d.Center), plane.Normal, New Vector3d(plane, ellipse2d.MajorAxis) * ellipse2d.MajorRadius, ellipse2d.MinorRadius / ellipse2d.MajorRadius, ellipse2d.StartAngle, ellipse2d.EndAngle)
                                    ent.[GetType]().InvokeMember("StartParam", BindingFlags.SetProperty, Nothing, ent, New Object() {ellipse2d.StartAngle})
                                    ent.[GetType]().InvokeMember("EndParam", BindingFlags.SetProperty, Nothing, ent, New Object() {ellipse2d.EndAngle})
                                    btr.AppendEntity(ent)
                                    tr.AddNewlyCreatedDBObject(ent, True)
                                    oIdColl.Add(btr.ObjectId)
                                End Using
                            ElseIf spline2d IsNot Nothing Then
                                If spline2d.HasFitData Then
                                    Dim n2fd As NurbCurve2dFitData = spline2d.FitData
                                    Using p3ds As Point3dCollection = New Point3dCollection()
                                        For Each p As Point2d In n2fd.FitPoints
                                            p3ds.Add(New Point3d(plane, p))
                                        Next

                                        Using ent As Spline = New Spline(p3ds, New Vector3d(plane, n2fd.StartTangent), New Vector3d(plane, n2fd.EndTangent), n2fd.Degree, n2fd.FitTolerance.EqualPoint)
                                            btr.AppendEntity(ent)
                                            tr.AddNewlyCreatedDBObject(ent, True)
                                            oIdColl.Add(btr.ObjectId)
                                        End Using
                                    End Using
                                Else
                                    Dim n2fd As NurbCurve2dData = spline2d.DefinitionData
                                    Using p3ds As Point3dCollection = New Point3dCollection()
                                        Dim knots As DoubleCollection = New DoubleCollection(n2fd.Knots.Count)
                                        For Each p As Point2d In n2fd.ControlPoints
                                            p3ds.Add(New Point3d(plane, p))
                                        Next

                                        For Each k As Double In n2fd.Knots
                                            knots.Add(k)
                                        Next

                                        Dim period As Double = 0
                                        Using ent As Spline = New Spline(n2fd.Degree, n2fd.Rational, spline2d.IsClosed(), spline2d.IsPeriodic(period), p3ds, knots, n2fd.Weights, n2fd.Knots.Tolerance, n2fd.Knots.Tolerance)
                                            btr.AppendEntity(ent)
                                            tr.AddNewlyCreatedDBObject(ent, True)
                                            oIdColl.Add(btr.ObjectId)
                                        End Using
                                    End Using
                                End If
                            End If
                        Next
                    End If
                Next
            End If

            tr.Commit()
        End Using
    End Sub



    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        RHB()
    End Sub
End Class