Module Meuk
    'Public Sub LoopLayers(iStart As Integer)
    '    TextBox1.AppendText("Start Loop with Value: " & iStart.ToString & " - " & iCurrLayer.ToString & vbCrLf)
    '    TextBox1.Update()
    '    Me.Update()
    '    Dim sc As ImportExportCommands = New ImportExportCommands()
    '    Dim layerNames As StringCollection = New StringCollection()
    '    Dim iTeller As Integer = 0
    '    iCurrLayer = iStart
    '    If sc.GetLayers(layerNames) Then
    '        TextBox1.AppendText("LayerCount: " & layerNames.Count.ToString & vbCrLf)
    '        TextBox1.Update()
    '        Me.Update()
    '        If iCurrLayer = layerNames.Count Then
    '            MsgBox("Voltooid")
    '            Exit Sub
    '        End If
    '        Dim layerName As String = Nothing
    '        acEd.Regen()
    '        For Each layerName In layerNames

    '            If iTeller = iCurrLayer Then
    '                If layerName = "0" Or layerName = "Defpoints" Then
    '                    iCurrLayer = iCurrLayer + 1
    '                Else
    '                    currLayer = layerName
    '                    SetCurrentLayer(currLayer)
    '                    TextBox1.AppendText("Start Isolating: " & currLayer & vbCrLf)
    '                    TextBox1.Update()
    '                    Me.Update()
    '                    If IsolateLayer(layerName) Then

    '                    End If
    '                    acEd.ApplyCurDwgLayerTableChanges()
    '                    acEd.Regen()
    '                    Threading.Thread.Sleep(5000)
    '                    TextBox1.AppendText("Start Boundary generation: " & vbCrLf)
    '                    TextBox1.Update()
    '                    Me.Update()
    '                    If HatchToBoundary() = True Then
    '                        'success
    '                        acEd.Regen()
    '                    End If
    '                    IsolateLayer("", True)
    '                    'cancel last active command
    '                    acDoc.SendStringToExecute(Chr(27), True, False, True)
    '                    acEd.Regen()
    '                    TextBox1.AppendText("Ending Loop: " & iStart.ToString & vbCrLf)
    '                    TextBox1.Update()
    '                    Me.Update()
    '                    Threading.Thread.Sleep(5000)
    '                    Exit Sub
    '                End If
    '            End If
    '            iTeller = iTeller + 1
    '        Next

    '        'If DeleteHatches() Then
    '        '    MsgBox("Voltooid")
    '        'End If

    '    End If
    'End Sub

    'Public Sub SetCurrentLayer(sLayer As String)
    '    Using acLockDoc As DocumentLock = acDoc.LockDocument()
    '        '' Start a transaction
    '        Using acTrans As Transaction = acCurDb.TransactionManager.StartTransaction()
    '            '' Open the Layer table for read
    '            Dim LayerTable As LayerTable = acTrans.GetObject(acCurDb.LayerTableId, OpenMode.ForRead)
    '            For Each ObjectId In LayerTable
    '                Dim LayerTableRecord As LayerTableRecord = acTrans.GetObject(ObjectId, OpenMode.ForWrite)
    '                Try
    '                    If LayerTableRecord.Name = sLayer Then
    '                        LayerTableRecord.IsOff = False
    '                        LayerTableRecord.IsFrozen = False
    '                        acCurDb.Clayer = LayerTable(sLayer)
    '                        TextBox1.AppendText("### Set Curr Layer: " & sLayer & vbCrLf)
    '                        Exit For
    '                    End If
    '                    TextBox1.Update()
    '                    Me.Update()
    '                Catch ex As Exception

    '                End Try
    '            Next
    '            '' Save the changes and dispose of the transaction
    '            acTrans.Commit()
    '        End Using
    '    End Using
    'End Sub


    'Public Function IsolateLayer(sLayer As String, Optional bUnisoAll As Boolean = False)
    '    Using acLockDoc As DocumentLock = acDoc.LockDocument()
    '        '' Start a transaction
    '        Using acTrans As Transaction = acCurDb.TransactionManager.StartTransaction()
    '            '' Open the Layer table for read
    '            Dim tmpLayer As String
    '            Dim LayerTable As LayerTable = acTrans.GetObject(acCurDb.LayerTableId, OpenMode.ForWrite)
    '            For Each ObjectId In LayerTable
    '                Dim LayerTableRecord As LayerTableRecord = acTrans.GetObject(ObjectId, OpenMode.ForWrite)
    '                Try
    '                    tmpLayer = LayerTableRecord.Name
    '                    If bUnisoAll = True Then
    '                        LayerTableRecord.IsOff = False
    '                        LayerTableRecord.IsFrozen = False
    '                        acEd.WriteMessage("Curr layer: " & sLayer & vbCr)
    '                    Else
    '                        If LayerTableRecord.Name = sLayer Then
    '                            LayerTableRecord.IsOff = False
    '                            LayerTableRecord.IsFrozen = False
    '                            'acCurDb.Clayer() = LayerTable(sLayer)
    '                            TextBox1.AppendText("Curr Layer: " & sLayer & vbCrLf)
    '                        Else
    '                            LayerTableRecord.IsOff = True
    '                            LayerTableRecord.IsFrozen = True
    '                            TextBox1.AppendText("Off / Freeze Layer: " & LayerTableRecord.Name & vbCrLf)
    '                        End If
    '                    End If
    '                    TextBox1.Update()
    '                    Me.Update()
    '                Catch ex As Exception
    '                    TextBox1.AppendText("EXCEPTION FOR LAYER: " & tmpLayer & vbCr)
    '                    TextBox1.Update()
    '                    Me.Update()
    '                End Try
    '            Next
    '            '' Save the changes and dispose of the transaction
    '            acTrans.Commit()
    '            acEd.ApplyCurDwgLayerTableChanges()
    '            acDoc.SendStringToExecute("REGENALL" & vbCr, True, False, True)
    '        End Using
    '    End Using
    '    listLayerState()
    '    Return True
    'End Function

    'Public Function listLayerState()
    '    Using acLockDoc As DocumentLock = acDoc.LockDocument()
    '        '' Start a transaction
    '        TextBox1.AppendText("$$$$$ Layer State List: " & currLayer.ToUpper & vbCrLf)
    '        Using acTrans As Transaction = acCurDb.TransactionManager.StartTransaction()
    '            '' Open the Layer table for read
    '            Dim LayerTable As LayerTable = acTrans.GetObject(acCurDb.LayerTableId, OpenMode.ForWrite)
    '            For Each ObjectId In LayerTable
    '                Dim LayerTableRecord As LayerTableRecord = acTrans.GetObject(ObjectId, OpenMode.ForWrite)
    '                Try
    '                    If LayerTableRecord.Name = currLayer Then
    '                        TextBox1.AppendText("CURRENT ")
    '                    End If
    '                    TextBox1.AppendText("Layer: " & LayerTableRecord.Name & " " & LayerTableRecord.IsOff.ToString & " " & LayerTableRecord.IsFrozen.ToString & vbCrLf)
    '                    TextBox1.Update()
    '                    Me.Update()
    '                Catch ex As Exception

    '                End Try
    '            Next
    '            '' Save the changes and dispose of the transaction
    '            acTrans.Commit()
    '        End Using
    '    End Using
    '    Return True
    'End Function


    'Public Sub FreezeAllLayers(sLayer As String, sPrefLayer As String)
    '    Using acLockDoc As DocumentLock = acDoc.LockDocument()
    '        '' Start a transaction
    '        Using acTrans As Transaction = acCurDb.TransactionManager.StartTransaction()
    '            '' Open the Layer table for read
    '            Dim LayerTable As LayerTable = acTrans.GetObject(acCurDb.LayerTableId, OpenMode.ForRead)
    '            For Each ObjectId In LayerTable
    '                Dim LayerTableRecord As LayerTableRecord = acTrans.GetObject(ObjectId, OpenMode.ForWrite)
    '                Try
    '                    If LayerTableRecord.Name = sLayer Then
    '                        LayerTableRecord.IsOff = False
    '                        LayerTableRecord.IsFrozen = False
    '                        'acCurDb.Clayer() = LayerTable(sLayer)
    '                        TextBox1.AppendText("Curr Layer: " & sLayer & vbCrLf)
    '                    Else
    '                        LayerTableRecord.IsOff = True
    '                        LayerTableRecord.IsFrozen = True
    '                        TextBox1.AppendText("Off / Freeze Layer: " & LayerTableRecord.Name & vbCrLf)
    '                    End If
    '                    TextBox1.Update()
    '                    Me.Update()
    '                Catch ex As Exception
    '                    'TextBox1.AppendText("EXCEPTION FOT LAYER: " & tmpLayer & vbCr)
    '                    'TextBox1.Update()
    '                    'Me.Update()
    '                End Try
    '            Next
    '            '' Save the changes and dispose of the transaction
    '            acTrans.Commit()
    '        End Using
    '    End Using
    'End Sub

    'Public Function HatchToBoundary()
    '    '_HATCHGENERATEBOUNDARY
    '    TextBox1.AppendText("Boundary for: " & currLayer & vbCrLf)
    '    TextBox1.Update()
    '    Me.Update()
    '    AddHandlers()
    '    acDoc.SendStringToExecute("_HATCHGENERATEBOUNDARY" & vbCr & "ALL" & vbCr & vbCr, True, False, True)
    '    While m_Command IsNot Nothing

    '    End While
    '    TextBox1.AppendText("Sleep mode entering: " & currLayer & vbCrLf)
    '    TextBox1.Update()
    '    Me.Update()
    '    Threading.Thread.Sleep(500)
    '    TextBox1.AppendText("End boundary: " & currLayer & vbCrLf)
    '    TextBox1.Update()
    '    Me.Update()
    '    Return True
    'End Function

    'Public Function DeleteHatches(Optional bGetSelectionCount As Boolean = False)
    '    Dim acTypValAr(0) As TypedValue
    '    acTypValAr.SetValue(New TypedValue(DxfCode.Start, "HATCH"), 0)

    '    Dim acSelFtr As SelectionFilter = New SelectionFilter(acTypValAr)

    '    Dim acSSPrompt As PromptSelectionResult
    '    Using acLockDoc As DocumentLock = acDoc.LockDocument()
    '        '' Start a transaction
    '        Using acTrans As Transaction = acCurDb.TransactionManager.StartTransaction()
    '            acSSPrompt = acEd.GetSelection(acSelFtr)
    '            If acSSPrompt.Status = PromptStatus.OK Then
    '                Dim acSSet As SelectionSet = acSSPrompt.Value
    '                If bGetSelectionCount = True Then
    '                    Return acSSet.Count
    '                Else
    '                    'Autodesk.AutoCAD.ApplicationServices.Application.ShowAlertDialog("Number of objects selected: " & acSSet.Count.ToString())
    '                    For Each acSSObj As SelectedObject In acSSet
    '                        '' Check to make sure a valid SelectedObject object was returned
    '                        If Not IsDBNull(acSSObj) Then
    '                            '' Open the selected object for write
    '                            Dim acEnt As Entity = acTrans.GetObject(acSSObj.ObjectId, OpenMode.ForWrite)

    '                            If Not IsDBNull(acEnt) Then
    '                                '' Change the object's color to Green
    '                                'acEd.WriteMessage("Ent: " & acEnt.GetType.ToString & vbCr)
    '                                TextBox1.AppendText(acEnt.GetType.ToString & vbCrLf)
    '                                acEnt.Erase()
    '                            End If
    '                        End If
    '                    Next
    '                    acTrans.Commit()
    '                    Return True
    '                End If

    '            Else
    '                'Autodesk.AutoCAD.ApplicationServices.Application.ShowAlertDialog("Number of objects selected: 0")
    '                If bGetSelectionCount = True Then
    '                    Return 0
    '                Else
    '                    Return True
    '                End If
    '            End If
    '        End Using
    '    End Using


    'End Function

    'Private m_Command As String = Nothing

    'Sub AddHandlers()
    '    Dim d As Document = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument
    '    AddHandler d.CommandWillStart, AddressOf CommandWillStart
    '    AddHandler d.CommandEnded, AddressOf CommandEnded
    '    AddHandler d.CommandFailed, AddressOf CommandFailed
    '    AddHandler d.CommandCancelled, AddressOf CommandCancelled
    '    commandFailled = False
    '    commandActive = True
    'End Sub

    'Sub RemoveHandlers()
    '    Dim d As Document = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument
    '    RemoveHandler d.CommandWillStart, AddressOf CommandWillStart
    '    RemoveHandler d.CommandEnded, AddressOf CommandEnded
    '    RemoveHandler d.CommandFailed, AddressOf CommandFailed
    '    RemoveHandler d.CommandCancelled, AddressOf CommandCancelled
    '    m_Command = Nothing
    '    commandActive = False
    '    TextBox1.AppendText("#######  Remove Handler" & vbCrLf)
    '    TextBox1.Update()
    '    If commandFailled = True Then
    '        FailedLayers = currLayer & vbCrLf
    '    End If
    '    iCurrLayer = iCurrLayer + 1
    '    LoopLayers(iCurrLayer)
    'End Sub

    'Sub CommandWillStart(ByVal sender As Object, ByVal e As Autodesk.AutoCAD.ApplicationServices.CommandEventArgs)
    '    If m_Command Is Nothing Then m_Command = e.GlobalCommandName
    'End Sub

    'Sub CommandEnded(ByVal sender As Object, ByVal e As Autodesk.AutoCAD.ApplicationServices.CommandEventArgs)
    '    If e.GlobalCommandName = m_Command Then

    '        RemoveHandlers()
    '    End If
    'End Sub

    'Sub CommandCancelled(ByVal sender As Object, ByVal e As Autodesk.AutoCAD.ApplicationServices.CommandEventArgs)
    '    commandFailled = True
    '    RemoveHandlers()
    'End Sub

    'Sub CommandFailed(ByVal sender As Object, ByVal e As Autodesk.AutoCAD.ApplicationServices.CommandEventArgs)
    '    commandFailled = True
    '    RemoveHandlers()
    'End Sub

    'Public Sub CreateSweepCurb(Optional dBreedte As Double = 0.2, Optional dHoogte As Double = 0.2)
    '    'set focus to modal space
    '    'Autodesk.AutoCAD.Internal.Utils.SetFocusToDwgView()
    '    Dim dStartX As Double
    '    Dim dStartY As Double
    '    Using acLockDoc As DocumentLock = acDoc.LockDocument()
    '        '' Start a transaction
    '        Dim per As PromptEntityResult
    '        Dim peo2 As PromptEntityOptions = New PromptEntityOptions(vbLf & "Select path along which to sweep: ")
    '        peo2.SetRejectMessage(vbLf & "Entity must be a curve.")
    '        peo2.AddAllowedClass(GetType(Curve), False)
    '        per = acEd.GetEntity(peo2)
    '        Using acTrans As Transaction = acCurDb.TransactionManager.StartTransaction()
    '            Dim sweepEnt As Entity = TryCast(acTrans.GetObject(per.ObjectId, OpenMode.ForRead), Entity)
    '            Dim pathEnt As Curve = TryCast(acTrans.GetObject(per.ObjectId, OpenMode.ForRead), Curve)
    '            Dim acBlkTbl As BlockTable = acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForWrite)
    '            Dim acBlkTblRec As BlockTableRecord = acTrans.GetObject(acBlkTbl(BlockTableRecord.ModelSpace), OpenMode.ForWrite)

    '            Dim bandPL As Polyline = New Polyline()
    '            dStartX = pathEnt.StartPoint.X
    '            dStartY = pathEnt.StartPoint.Y
    '            bandPL.AddVertexAt(0, New Point2d(dStartX, dStartY), 0, 0, 0)
    '            bandPL.AddVertexAt(1, New Point2d(dStartX - (dBreedte / 2), dStartY), 0, 0, 0)
    '            bandPL.AddVertexAt(2, New Point2d(dStartX - (dBreedte / 2), dStartY + dHoogte), 0, 0, 0)
    '            bandPL.AddVertexAt(3, New Point2d(dStartX + (dBreedte / 2), dStartY + dHoogte), 0, 0, 0)
    '            bandPL.AddVertexAt(4, New Point2d(dStartX + (dBreedte / 2), dStartY), 0, 0, 0)
    '            bandPL.AddVertexAt(5, New Point2d(dStartX, dStartY), 0, 0, 0)

    '            Dim band3D As New Solid3d()
    '            Dim sob As SweepOptionsBuilder = New SweepOptionsBuilder()
    '            sob.Align = SweepOptionsAlignOption.AlignSweepEntityToPath
    '            sob.BasePoint = pathEnt.StartPoint
    '            sob.Bank = True
    '            band3D.CreateSweptSolid(bandPL, pathEnt, sob.ToSweepOptions())
    '            acBlkTblRec.AppendEntity(band3D)
    '            acTrans.AddNewlyCreatedDBObject(band3D, True)
    '            acTrans.Commit()
    '            MsgBox("Sweep voltooid")
    '        End Using
    '    End Using

    'End Sub

    'Public Sub SweepAlongPath()
    '    'set focus to modal space
    '    Autodesk.AutoCAD.Internal.Utils.SetFocusToDwgView()
    '    Using acLockDoc As DocumentLock = acDoc.LockDocument()
    '        Dim doc As Document = acDoc
    '        Dim db As Database = doc.Database
    '        Dim ed As Editor = doc.Editor
    '        Dim peo1 As PromptEntityOptions = New PromptEntityOptions(vbLf & "Select profile or curve to sweep: ")
    '        peo1.SetRejectMessage(vbLf & "Entity must be a region, curve or planar surface.")
    '        peo1.AddAllowedClass(GetType(Region), False)
    '        peo1.AddAllowedClass(GetType(Curve), False)
    '        peo1.AddAllowedClass(GetType(PlaneSurface), False)
    '        Dim per As PromptEntityResult = ed.GetEntity(peo1)
    '        If per.Status <> PromptStatus.OK Then Return
    '        Dim regId As ObjectId = per.ObjectId
    '        Dim peo2 As PromptEntityOptions = New PromptEntityOptions(vbLf & "Select path along which to sweep: ")
    '        peo2.SetRejectMessage(vbLf & "Entity must be a curve.")
    '        peo2.AddAllowedClass(GetType(Curve), False)
    '        per = ed.GetEntity(peo2)
    '        If per.Status <> PromptStatus.OK Then Return
    '        Dim splId As ObjectId = per.ObjectId
    '        Dim pko As PromptKeywordOptions = New PromptKeywordOptions(vbLf & "Sweep a solid or a surface?")
    '        pko.AllowNone = True
    '        pko.Keywords.Add("SOlid")
    '        pko.Keywords.Add("SUrface")
    '        pko.Keywords.[Default] = "SOlid"
    '        Dim pkr As PromptResult = ed.GetKeywords(pko)
    '        Dim createSolid As Boolean = (pkr.StringResult = "SOlid")
    '        If pkr.Status <> PromptStatus.OK Then Return
    '        Dim tr As Transaction = db.TransactionManager.StartTransaction()
    '        Using tr
    '            Try
    '                Dim sweepEnt As Entity = TryCast(tr.GetObject(regId, OpenMode.ForRead), Entity)
    '                Dim pathEnt As Curve = TryCast(tr.GetObject(splId, OpenMode.ForRead), Curve)
    '                If sweepEnt Is Nothing OrElse pathEnt Is Nothing Then
    '                    ed.WriteMessage(vbLf & "Problem opening the selected entities.")
    '                    Return
    '                End If

    '                Dim sob As SweepOptionsBuilder = New SweepOptionsBuilder()
    '                sob.Align = SweepOptionsAlignOption.AlignSweepEntityToPath
    '                sob.BasePoint = pathEnt.StartPoint
    '                sob.Bank = True
    '                Dim ent As Entity
    '                If createSolid Then
    '                    Dim sol As Solid3d = New Solid3d()
    '                    sol.CreateSweptSolid(sweepEnt, pathEnt, sob.ToSweepOptions())
    '                    ent = sol
    '                Else
    '                    Dim ss As SweptSurface = New SweptSurface()
    '                    ss.CreateSweptSurface(sweepEnt, pathEnt, sob.ToSweepOptions())
    '                    ent = ss
    '                End If

    '                Dim bt As BlockTable = CType(tr.GetObject(db.BlockTableId, OpenMode.ForRead), BlockTable)
    '                Dim ms As BlockTableRecord = CType(tr.GetObject(bt(BlockTableRecord.ModelSpace), OpenMode.ForWrite), BlockTableRecord)
    '                Dim acBlkTbl As BlockTable = tr.GetObject(acCurDb.BlockTableId, OpenMode.ForWrite)
    '                Dim acBlkTblRec As BlockTableRecord = tr.GetObject(acBlkTbl(BlockTableRecord.ModelSpace), OpenMode.ForWrite)
    '                'ms.AppendEntity(ent)
    '                acBlkTblRec.AppendEntity(ent)
    '                tr.AddNewlyCreatedDBObject(ent, True)
    '                tr.Commit()
    '            Catch
    '            End Try
    '        End Using
    '    End Using
    'End Sub


    'Public Sub CreateSweepCurbTest()
    '    Dim acDoc As Document = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument
    '    Dim acCurDb As Database = acDoc.Database
    '    Dim acEd As Editor = acDoc.Editor

    '    Using acLockDoc As DocumentLock = acDoc.LockDocument()
    '        Dim per As PromptEntityResult
    '        Dim peo As PromptEntityOptions = New PromptEntityOptions(vbLf & "Select path along which to sweep: ")
    '        peo.SetRejectMessage(vbLf & "Entity must be a curve.")
    '        peo.AddAllowedClass(GetType(Curve), False)
    '        per = acEd.GetEntity(peo)
    '        Using acTrans As Transaction = acCurDb.TransactionManager.StartTransaction()
    '            Try
    '                Dim pathEnt As Curve = TryCast(acTrans.GetObject(per.ObjectId, OpenMode.ForRead), Curve)
    '                Dim acBlkTbl As BlockTable = acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForWrite)
    '                Dim acBlkTblRec As BlockTableRecord = acTrans.GetObject(acBlkTbl(BlockTableRecord.ModelSpace), OpenMode.ForWrite)

    '                Dim sweepEnt As Polyline = New Polyline()
    '                sweepEnt.AddVertexAt(0, New Point2d(0, 0), 0, 0, 0)
    '                sweepEnt.AddVertexAt(1, New Point2d(0, 0.1), 0, 0, 0)
    '                sweepEnt.AddVertexAt(2, New Point2d(0.1, 0.1), 0, 0, 0)
    '                sweepEnt.AddVertexAt(3, New Point2d(0.1, 0), 0, 0, 0)
    '                sweepEnt.AddVertexAt(4, New Point2d(0, 0), 0, 0, 0)

    '                Dim ent3D As New Solid3d()
    '                Dim sob As SweepOptionsBuilder = New SweepOptionsBuilder()
    '                sob.Align = SweepOptionsAlignOption.AlignSweepEntityToPath
    '                sob.BasePoint = sweepEnt.StartPoint
    '                sob.Bank = True

    '                ent3D.CreateSweptSolid(sweepEnt, pathEnt, sob.ToSweepOptions())
    '                acBlkTblRec.AppendEntity(ent3D)
    '                acTrans.AddNewlyCreatedDBObject(ent3D, True)
    '                acTrans.Commit()
    '            Catch ex As Autodesk.AutoCAD.Runtime.Exception
    '                MsgBox("Error: " & ex.Message & vbCrLf & ex.Source & vbCrLf & ex.ErrorStatus)
    '            End Try
    '        End Using
    '    End Using
    'End Sub

    'Private Function PolylineToSolid(Optional dBreedte As Double = 0.2, Optional dHoogte As Double = 0.2)
    '    Using acLockDoc As DocumentLock = acDoc.LockDocument()
    '        Dim dStartX, dStartY As Double
    '        Dim per As PromptEntityResult
    '        Dim band3D As New Solid3d()
    '        Dim peo2 As PromptEntityOptions = New PromptEntityOptions(vbLf & "Select path along which to sweep: ")
    '        peo2.SetRejectMessage(vbLf & "Entity must be a curve.")
    '        peo2.AddAllowedClass(GetType(Curve), False)
    '        per = acEd.GetEntity(peo2)
    '        Using acTrans As Transaction = acCurDb.TransactionManager.StartTransaction()
    '            Dim pathEnt As Curve = TryCast(acTrans.GetObject(per.ObjectId, OpenMode.ForRead), Curve)
    '            Dim pathPoly As Polyline = TryCast(acTrans.GetObject(per.ObjectId, OpenMode.ForRead), Polyline)
    '            Dim acBlkTbl As BlockTable = acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForWrite)
    '            Dim acBlkTblRec As BlockTableRecord = acTrans.GetObject(acBlkTbl(BlockTableRecord.ModelSpace), OpenMode.ForWrite)

    '            Dim bandPL As Polyline = New Polyline()
    '            dStartX = pathPoly.StartPoint.X
    '            dStartY = pathPoly.StartPoint.Y
    '            bandPL.AddVertexAt(0, New Point2d(dStartX, dStartY), 0, 0, 0)
    '            bandPL.AddVertexAt(1, New Point2d(dStartX - (dBreedte / 2), dStartY), 0, 0, 0)
    '            bandPL.AddVertexAt(2, New Point2d(dStartX - (dBreedte / 2), dStartY + dHoogte), 0, 0, 0)
    '            bandPL.AddVertexAt(3, New Point2d(dStartX + (dBreedte / 2), dStartY + dHoogte), 0, 0, 0)
    '            bandPL.AddVertexAt(4, New Point2d(dStartX + (dBreedte / 2), dStartY), 0, 0, 0)
    '            bandPL.AddVertexAt(5, New Point2d(dStartX, dStartY), 0, 0, 0)
    '            acBlkTblRec.AppendEntity(bandPL)
    '            acTrans.AddNewlyCreatedDBObject(bandPL, True)
    '            Using pathPoly
    '                Using bandPL
    '                    Try

    '                    Catch exception As Exception
    '                        'Log.Logger.Error(exception, "Unable to extrude the 3D Solid")
    '                        MsgBox("Oepsie")
    '                        Return False
    '                    End Try
    '                End Using
    '            End Using
    '            acTrans.Commit()
    '        End Using

    '    End Using
    '    Return True
    'End Function

    'Private Function CreateUBolt(uBoltDiameter As Double, pipeDiameter As Double, extendedLength As Double) As Solid3d

    '    If uBoltDiameter <= MinimumExtrusionLength Then
    '        Throw New ArgumentOutOfRangeException(NameOf(uBoltDiameter), "UBolt Diameter is too small.")
    '    End If
    '    If pipeDiameter <= MinimumExtrusionLength Then
    '        Throw New ArgumentOutOfRangeException(NameOf(pipeDiameter), "Pipe Diameter is to small.")
    '    End If
    '    Dim centerToCenter = (pipeDiameter / 2) + uBoltDiameter

    '    ' While not probable, it is possible that the sum value is greater then the largest double value.  Input values for
    '    ' extended length should take this into account.
    '    Dim straightLength = (pipeDiameter / 2) + extendedLength
    '    Dim uBolt = New Solid3d()
    '    Dim polylineSegmentCollection = New PolylineSegmentCollection()
    '    Using lineSegment1 = New LineSegment2d(New Point2d(-1 * centerToCenter, straightLength), New Point2d(-1 * centerToCenter, straightLength - minimumExtrusionLength))
    '        Dim lineSegment = New PolylineSegment(lineSegment1)
    '        polylineSegmentCollection.Add(lineSegment)
    '    End Using
    '    Using lineSegment2 = New LineSegment2d(New Point2d(-1 * centerToCenter, straightLength - minimumExtrusionLength), New Point2d(-1 * centerToCenter, 0))
    '        Dim lineSegment = New PolylineSegment(lineSegment2)
    '        polylineSegmentCollection.Add(lineSegment)
    '    End Using
    '    Dim arcSegment = New PolylineSegment(New Point2d(-1 * centerToCenter, 0), New Point2d(centerToCenter, 0), 1, 0)
    '    polylineSegmentCollection.Add(arcSegment)
    '    Using lineSegment3 = New LineSegment2d(New Point2d(centerToCenter, 0), New Point2d(centerToCenter, straightLength))
    '        Dim lineSegment = New PolylineSegment(lineSegment3)
    '        polylineSegmentCollection.Add(lineSegment)
    '    End Using
    '    Using centerLine = polylineSegmentCollection.ToPolyline()
    '        Using profile = New Circle(centerLine.StartPoint, Vector3d.YAxis, uBoltDiameter)
    '            Try
    '                Dim profileCollection = New DBObjectCollection()
    '                profileCollection.Add(profile)
    '                Dim profileRegionCollection = Autodesk.AutoCAD.DatabaseServices.Region.CreateFromCurves(profileCollection)
    '                Dim profileRegion As Region = profileRegionCollection(0)
    '                uBolt.ExtrudeAlongPath(profileRegion, centerLine, 0.0)
    '                uBolt.CleanBody()
    '            Catch exception As Exception
    '                'Log.Logger.Error(exception, "Unable to extrude the 3D Solid")
    '                MsgBox("Oepsie")
    '                Return Nothing
    '            End Try

    '        End Using
    '    End Using
    '    Return uBolt
    'End Function

    'Public Sub SweepAlongPathTEST()
    '    'set focus to modal space
    '    Autodesk.AutoCAD.Internal.Utils.SetFocusToDwgView()
    '    'Dim doc As Document = acDoc
    '    'Dim db As Database = doc.Database
    '    'Dim ed As Editor = doc.Editor

    '    ' Ask the user to select a region to extrude
    '    Dim peo1 As New PromptEntityOptions(vbLf & "Select profile or curve to sweep: ")

    '    peo1.SetRejectMessage(vbLf & "Entity must be a region, curve or planar surface.")

    '    peo1.AddAllowedClass(GetType(Autodesk.AutoCAD.DatabaseServices.Region), False)

    '    peo1.AddAllowedClass(GetType(Autodesk.AutoCAD.DatabaseServices.Curve), False)

    '    peo1.AddAllowedClass(GetType(PlaneSurface), False)

    '    Dim per As PromptEntityResult = acEd.GetEntity(peo1)

    '    If per.Status <> PromptStatus.OK Then

    '        Return
    '    End If

    '    Dim regId As ObjectId = per.ObjectId

    '    ' Ask the user to select an extrusion path
    '    Dim peo2 As New PromptEntityOptions(vbLf & "Select path along which to sweep: ")

    '    peo2.SetRejectMessage(vbLf & "Entity must be a curve.")

    '    peo2.AddAllowedClass(GetType(Autodesk.AutoCAD.DatabaseServices.Curve), False)

    '    per = acEd.GetEntity(peo2)

    '    If per.Status <> PromptStatus.OK Then
    '        Return
    '    End If

    '    Dim splId As ObjectId = per.ObjectId

    '    Dim pko As New PromptKeywordOptions(vbLf & "Sweep a solid or a surface?")

    '    pko.AllowNone = True

    '    pko.Keywords.Add("SOlid")

    '    pko.Keywords.Add("SUrface")

    '    pko.Keywords.[Default] = "SOlid"

    '    Dim pkr As PromptResult = acEd.GetKeywords(pko)

    '    Dim createSolid As Boolean = (pkr.StringResult = "SOlid")

    '    If pkr.Status <> PromptStatus.OK Then

    '        Return
    '    End If

    '    ' Now let's create our swept surface
    '    Dim tr As Transaction = acCurDb.TransactionManager.StartTransaction()

    '    Using tr
    '        Try
    '            Dim sweepEnt As Autodesk.AutoCAD.DatabaseServices.Entity = TryCast(tr.GetObject(regId, OpenMode.ForRead), Autodesk.AutoCAD.DatabaseServices.Entity)

    '            Dim pathEnt As Autodesk.AutoCAD.DatabaseServices.Curve = TryCast(tr.GetObject(splId, OpenMode.ForRead), Autodesk.AutoCAD.DatabaseServices.Curve)

    '            If sweepEnt Is Nothing OrElse pathEnt Is Nothing Then

    '                acEd.WriteMessage(vbLf & "Problem opening the selected entities.")

    '                Return
    '            End If

    '            ' We use a builder object to create
    '            ' our SweepOptions

    '            Dim sob As New SweepOptionsBuilder()
    '            ' Align the entity to sweep to the path
    '            sob.Align = SweepOptionsAlignOption.AlignSweepEntityToPath
    '            ' The base point is the start of the path
    '            sob.BasePoint = pathEnt.StartPoint

    '            ' The profile will rotate to follow the path
    '            sob.Bank = True

    '            ' Now generate the solid or surface...
    '            Dim ent As Autodesk.AutoCAD.DatabaseServices.Entity

    '            If createSolid Then

    '                Dim sol As New Solid3d()
    '                sol.RecordHistory = True
    '                sol.CreateSweptSolid(sweepEnt, pathEnt, sob.ToSweepOptions())

    '                ent = sol
    '            Else
    '                Dim ss As New SweptSurface()
    '                ss.CreateSweptSurface(sweepEnt, pathEnt, sob.ToSweepOptions())
    '                ent = ss
    '            End If

    '            ' ... and add it to the modelspace

    '            Dim bt As BlockTable = DirectCast(tr.GetObject(acCurDb.BlockTableId, OpenMode.ForRead), BlockTable)

    '            Dim ms As BlockTableRecord = DirectCast(tr.GetObject(bt(BlockTableRecord.ModelSpace), OpenMode.ForWrite), BlockTableRecord)

    '            Dim acBlkTbl As BlockTable = tr.GetObject(acCurDb.BlockTableId, OpenMode.ForWrite)
    '            Dim acBlkTblRec As BlockTableRecord = tr.GetObject(acBlkTbl(BlockTableRecord.ModelSpace), OpenMode.ForWrite)

    '            acBlkTblRec.AppendEntity(ent)

    '            tr.AddNewlyCreatedDBObject(ent, True)

    '            tr.Commit()

    '        Catch
    '            MsgBox("Fout")
    '        End Try
    '    End Using

    'End Sub


    'Public Sub SweepAlongPath()
    '    'set focus to modal space
    '    Autodesk.AutoCAD.Internal.Utils.SetFocusToDwgView()
    '    Using acLockDoc As DocumentLock = acDoc.LockDocument()
    '        Dim doc As Document = acDoc
    '        Dim db As Database = doc.Database
    '        Dim ed As Editor = doc.Editor
    '        Dim peo1 As PromptEntityOptions = New PromptEntityOptions(vbLf & "Select profile or curve to sweep: ")
    '        peo1.SetRejectMessage(vbLf & "Entity must be a region, curve or planar surface.")
    '        peo1.AddAllowedClass(GetType(Region), False)
    '        peo1.AddAllowedClass(GetType(Curve), False)
    '        peo1.AddAllowedClass(GetType(PlaneSurface), False)
    '        Dim per As PromptEntityResult = ed.GetEntity(peo1)
    '        If per.Status <> PromptStatus.OK Then Return
    '        Dim regId As ObjectId = per.ObjectId
    '        Dim peo2 As PromptEntityOptions = New PromptEntityOptions(vbLf & "Select path along which to sweep: ")
    '        peo2.SetRejectMessage(vbLf & "Entity must be a curve.")
    '        peo2.AddAllowedClass(GetType(Curve), False)
    '        per = ed.GetEntity(peo2)
    '        If per.Status <> PromptStatus.OK Then Return
    '        Dim splId As ObjectId = per.ObjectId
    '        Dim pko As PromptKeywordOptions = New PromptKeywordOptions(vbLf & "Sweep a solid or a surface?")
    '        pko.AllowNone = True
    '        pko.Keywords.Add("SOlid")
    '        pko.Keywords.Add("SUrface")
    '        pko.Keywords.[Default] = "SOlid"
    '        Dim pkr As PromptResult = ed.GetKeywords(pko)
    '        Dim createSolid As Boolean = (pkr.StringResult = "SOlid")
    '        If pkr.Status <> PromptStatus.OK Then Return
    '        Dim tr As Transaction = db.TransactionManager.StartTransaction()
    '        Using tr
    '            Try
    '                Dim sweepEnt As Entity = TryCast(tr.GetObject(regId, OpenMode.ForRead), Entity)
    '                Dim pathEnt As Curve = TryCast(tr.GetObject(splId, OpenMode.ForRead), Curve)
    '                If sweepEnt Is Nothing OrElse pathEnt Is Nothing Then
    '                    ed.WriteMessage(vbLf & "Problem opening the selected entities.")
    '                    Return
    '                End If

    '                Dim sob As SweepOptionsBuilder = New SweepOptionsBuilder()
    '                sob.Align = SweepOptionsAlignOption.AlignSweepEntityToPath
    '                sob.BasePoint = pathEnt.StartPoint
    '                sob.Bank = True
    '                Dim ent As Entity
    '                If createSolid Then
    '                    Dim sol As Solid3d = New Solid3d()
    '                    sol.CreateSweptSolid(sweepEnt, pathEnt, sob.ToSweepOptions())
    '                    ent = sol
    '                Else
    '                    Dim ss As SweptSurface = New SweptSurface()
    '                    ss.CreateSweptSurface(sweepEnt, pathEnt, sob.ToSweepOptions())
    '                    ent = ss
    '                End If

    '                Dim bt As BlockTable = CType(tr.GetObject(db.BlockTableId, OpenMode.ForRead), BlockTable)
    '                Dim ms As BlockTableRecord = CType(tr.GetObject(bt(BlockTableRecord.ModelSpace), OpenMode.ForWrite), BlockTableRecord)
    '                Dim acBlkTbl As BlockTable = tr.GetObject(acCurDb.BlockTableId, OpenMode.ForWrite)
    '                Dim acBlkTblRec As BlockTableRecord = tr.GetObject(acBlkTbl(BlockTableRecord.ModelSpace), OpenMode.ForWrite)
    '                'ms.AppendEntity(ent)
    '                acBlkTblRec.AppendEntity(ent)
    '                tr.AddNewlyCreatedDBObject(ent, True)
    '                tr.Commit()
    '            Catch ex As Autodesk.AutoCAD.Runtime.Exception
    '                MsgBox(ex.Message)

    '            End Try
    '        End Using
    '    End Using
    'End Sub
End Module
