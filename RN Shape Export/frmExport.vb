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
            ConvertHatches()
            acEd.Regen()
            MsgBox("Omzetten van Hatches voltooid!")
        End If
    End Sub

    Public Function ConvertHatches()
        Dim acTypValAr(0) As TypedValue
        acTypValAr.SetValue(New TypedValue(DxfCode.Start, "HATCH"), 0)

        Dim acSelFtr As SelectionFilter = New SelectionFilter(acTypValAr)

        Dim acSSPrompt As PromptSelectionResult
        Using acLockDoc As DocumentLock = acDoc.LockDocument()
            '' Start a transaction
            Using acTrans As Transaction = acCurDb.TransactionManager.StartTransaction()
                'acSSPrompt = acEd.GetSelection(acSelFtr)
                acSSPrompt = acEd.SelectAll(acSelFtr)
                If acSSPrompt.Status = PromptStatus.OK Then
                    Dim acSSet As SelectionSet = acSSPrompt.Value
                    For Each acSSObj As SelectedObject In acSSet
                        If Not IsDBNull(acSSObj) Then
                            If RHB(acSSObj.ObjectId) Then
                                Dim acEnt As Entity = acTrans.GetObject(acSSObj.ObjectId, OpenMode.ForWrite)
                                If Not IsDBNull(acEnt) Then
                                    acEnt.Erase()
                                End If
                            End If
                        End If
                    Next
                    acTrans.Commit()
                Else
                    Return False
                End If
            End Using
        End Using
        Return True
    End Function

    ''' <summary>
    ''' 'Convert Hatch Object to Polyline
    ''' </summary>
    ''' <param name="oObjectID"></param>
    ''' <returns></returns>

    Public Function RHB(oObjectID As ObjectId)
        'source: https://forums.autodesk.com/t5/net/restore-hatch-boundaries-if-they-have-been-lost-with-net/m-p/3786748#M33601
        Dim doc As Document = acDoc
        Dim ed As Editor = doc.Editor
        Dim oIdColl As ObjectIdCollection = New ObjectIdCollection()
        Dim bIsPolyBound As Boolean = False
        Dim sLayer As String
        Using acLockDoc As DocumentLock = acDoc.LockDocument()
            Using tr As Transaction = doc.TransactionManager.StartTransaction()
                Dim hatch As Hatch = TryCast(tr.GetObject(oObjectID, OpenMode.ForRead), Hatch)
                sLayer = hatch.Layer
                If hatch IsNot Nothing Then
                    Dim btr As BlockTableRecord = TryCast(tr.GetObject(hatch.OwnerId, OpenMode.ForWrite), BlockTableRecord)
                    If btr Is Nothing Then Return False
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
                                oIdColl.Add(poly.ObjectId)
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
                                        oIdColl.Add(ent.ObjectId)
                                    End Using
                                ElseIf arc2d IsNot Nothing Then
                                    If Math.Abs(arc2d.StartAngle - arc2d.EndAngle) < 0.00001 Then
                                        Using ent As Circle = New Circle(New Point3d(plane, arc2d.Center), plane.Normal, arc2d.Radius)
                                            btr.AppendEntity(ent)
                                            tr.AddNewlyCreatedDBObject(ent, True)
                                            oIdColl.Add(ent.ObjectId)
                                        End Using
                                    Else
                                        Dim angle As Double = New Vector3d(plane, arc2d.ReferenceVector).AngleOnPlane(plane)
                                        Using ent As Arc = New Arc(New Point3d(plane, arc2d.Center), arc2d.Radius, arc2d.StartAngle + angle, arc2d.EndAngle + angle)
                                            btr.AppendEntity(ent)
                                            tr.AddNewlyCreatedDBObject(ent, True)
                                            oIdColl.Add(ent.ObjectId)
                                        End Using
                                    End If
                                ElseIf ellipse2d IsNot Nothing Then
                                    Using ent As Ellipse = New Ellipse(New Point3d(plane, ellipse2d.Center), plane.Normal, New Vector3d(plane, ellipse2d.MajorAxis) * ellipse2d.MajorRadius, ellipse2d.MinorRadius / ellipse2d.MajorRadius, ellipse2d.StartAngle, ellipse2d.EndAngle)
                                        ent.[GetType]().InvokeMember("StartParam", BindingFlags.SetProperty, Nothing, ent, New Object() {ellipse2d.StartAngle})
                                        ent.[GetType]().InvokeMember("EndParam", BindingFlags.SetProperty, Nothing, ent, New Object() {ellipse2d.EndAngle})
                                        btr.AppendEntity(ent)
                                        tr.AddNewlyCreatedDBObject(ent, True)
                                        oIdColl.Add(ent.ObjectId)
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
                                                oIdColl.Add(ent.ObjectId)
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
                                                oIdColl.Add(ent.ObjectId)
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
            If convertToPoly(oIdColl, sLayer) Then
                Return True
            End If
        End Using
        Return True
    End Function

    Private Function convertToPoly(oIdColl As ObjectIdCollection, sLayer As String)
        Using acLockDoc As DocumentLock = acDoc.LockDocument()
            '' Start a transaction
            Using acTrans As Transaction = acCurDb.TransactionManager.StartTransaction()
                Dim btr As BlockTableRecord = CType(acTrans.GetObject(acCurDb.CurrentSpaceId, OpenMode.ForWrite), BlockTableRecord)
                Dim psc As PolylineSegmentCollection = New PolylineSegmentCollection()
                Dim plane As Plane = New Plane(Point3d.Origin, Vector3d.ZAxis)
                For Each id As ObjectId In oIdColl
                    Dim ent As Entity = CType(acTrans.GetObject(id, OpenMode.ForRead), Entity)
                    Select Case ent.[GetType]().Name
                        Case "Arc"
                            Dim arc As Arc = CType(ent, Arc)
                            psc.Add(New PolylineSegment(New CircularArc2d(arc.Center.Convert2d(plane), arc.Radius, arc.StartAngle, arc.EndAngle, Vector2d.XAxis, False)))
                        Case "Ellipse"
                            Dim el As Ellipse = CType(ent, Ellipse)
                            psc.AddRange(New PolylineSegmentCollection(el))
                        Case "Line"
                            Dim l As Line = CType(ent, Line)
                            psc.Add(New PolylineSegment(New LineSegment2d(l.StartPoint.Convert2d(plane), l.EndPoint.Convert2d(plane))))
                        Case "Polyline"
                            Dim pl As Polyline = CType(ent, Polyline)
                            psc.AddRange(New PolylineSegmentCollection(pl))
                        Case "Spline"
                            Try
                                Dim spl As Spline = CType(ent, Spline)
                                psc.AddRange(New PolylineSegmentCollection(CType(spl.ToPolyline(), Polyline)))
                            Catch
                            End Try

                        Case Else
                    End Select
                    ent.Erase()
                Next

                For Each segs As PolylineSegmentCollection In psc.Join()
                    Dim pline As Polyline = segs.ToPolyline()
                    pline.Layer = sLayer
                    btr.AppendEntity(pline)
                    acTrans.AddNewlyCreatedDBObject(pline, True)
                Next

                acTrans.Commit()
            End Using
        End Using
        Return True
    End Function

    Private Sub cmdSweep_Click(sender As Object, e As EventArgs) Handles cmdSweep.Click
        If txtHoogte.Text = "" Or txtBreedte.Text = "" Then
            MsgBox("Vul eerst een breedte en hoogte in!")
        Else
            If cmbLayer.Text = "" Then
                MsgBox("Selecteer een te verwerken laag")
            Else

                createSolidCurb(cmbLayer.Text)
            End If
        End If
    End Sub

    Public Function createSolidCurb(ByVal sLayer As String)
        Dim oIds As ObjectIdCollection = GetEntitiesOnLayer(sLayer)
        If oIds.Count = 0 Then
            MsgBox("Geen objecten op de laag " & sLayer & " Selecteer een andere laag")
            Return False
        End If

        Dim dBreedte As Double = CDbl(txtBreedte.Text.Replace(",", "."))
        Dim dHoogte As Double = CDbl(txtHoogte.Text.Replace(",", "."))

        CreateSweepCurb(oIds, sLayer, dBreedte, dHoogte)
        MsgBox("Aanmaken van 3D Bandenlijn voltooid!")
        Return True
    End Function

    Public Function CreateSweepCurb(Optional dBreedte As Double = 0.2, Optional dHoogte As Double = 0.2)
        'set focus to modal space
        Autodesk.AutoCAD.Internal.Utils.SetFocusToDwgView()
        Dim per As PromptEntityResult
        Dim peo2 As PromptEntityOptions = New PromptEntityOptions(vbLf & "Selecteer een Pad voor de Sweep: ")
        Try
            Dim dStartX As Double
            Dim dStartY As Double
            Using acLockDoc As DocumentLock = acDoc.LockDocument()
                '' Start a transaction
                peo2.SetRejectMessage(vbLf & "Object moet een lijn-object zijn.")
                peo2.AddAllowedClass(GetType(Curve), False)
                per = acEd.GetEntity(peo2)
                Using acTrans As Transaction = acCurDb.TransactionManager.StartTransaction()
                    Dim sweepEnt As Entity = TryCast(acTrans.GetObject(per.ObjectId, OpenMode.ForRead), Entity)
                    Dim pathEnt As Curve = TryCast(acTrans.GetObject(sweepEnt.ObjectId, OpenMode.ForRead), Curve)
                    If sweepEnt Is Nothing OrElse pathEnt Is Nothing Then
                        acEd.WriteMessage(vbLf & "Geselecteerde Object kon niet geopend worden! Laag gelockt?.")
                    Else


                        Dim acBlkTbl As BlockTable = acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForWrite)
                        Dim acBlkTblRec As BlockTableRecord = acTrans.GetObject(acBlkTbl(BlockTableRecord.ModelSpace), OpenMode.ForWrite)

                        Dim bandPL As Polyline = New Polyline()
                        dStartX = pathEnt.StartPoint.X
                        dStartY = pathEnt.StartPoint.Y
                        bandPL.AddVertexAt(0, New Point2d(dStartX, dStartY), 0, 0, 0)
                        bandPL.AddVertexAt(1, New Point2d(dStartX - (dBreedte / 2), dStartY), 0, 0, 0)
                        bandPL.AddVertexAt(2, New Point2d(dStartX - (dBreedte / 2), dStartY + dHoogte), 0, 0, 0)
                        bandPL.AddVertexAt(3, New Point2d(dStartX + (dBreedte / 2), dStartY + dHoogte), 0, 0, 0)
                        bandPL.AddVertexAt(4, New Point2d(dStartX + (dBreedte / 2), dStartY), 0, 0, 0)
                        bandPL.Closed = True

                        Dim band3D As New Solid3d()
                        Dim sob As SweepOptionsBuilder = New SweepOptionsBuilder()
                        sob.Align = SweepOptionsAlignOption.AlignSweepEntityToPath
                        sob.BasePoint = bandPL.StartPoint
                        sob.Bank = True

                        band3D.CreateSweptSolid(bandPL, pathEnt, sob.ToSweepOptions())
                        acBlkTblRec.AppendEntity(band3D)
                        acTrans.AddNewlyCreatedDBObject(band3D, True)
                        acTrans.Commit()
                        acEd.Regen()
                    End If
                End Using
            End Using
            Return True
        Catch ex As Autodesk.AutoCAD.Runtime.Exception
            If SweepClosePolyline(per.ObjectId, dBreedte, dHoogte) = False Then
                MsgBox("Er ging iets fout, probeer de lijn te exploderen en probeer het opnieuw!" & vbCrLf & ex.Message)
                Me.BringToFront()
                Return False
            End If

        End Try
        Me.BringToFront()
    End Function

    Public Function CreateSweepCurb(ByVal oOidColl As ObjectIdCollection, ByVal sLayer As String, Optional dBreedte As Double = 0.2, Optional dHoogte As Double = 0.2)
        'set focus to modal space
        'Autodesk.AutoCAD.Internal.Utils.SetFocusToDwgView()
        'set current layer
        setCurrentLayer(sLayer)
        Dim dStartX As Double
        Dim dStartY As Double
        Using acLockDoc As DocumentLock = acDoc.LockDocument()
            Using acTrans As Transaction = acCurDb.TransactionManager.StartTransaction()
                Dim acBlkTbl As BlockTable = acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForWrite)
                Dim acBlkTblRec As BlockTableRecord = acTrans.GetObject(acBlkTbl(BlockTableRecord.ModelSpace), OpenMode.ForWrite)
                For Each oOid As ObjectId In oOidColl
                    Dim sweepEnt As Entity
                    Dim pathEnt As Curve
                    Dim bandPL As Polyline = New Polyline()
                    Try
                        sweepEnt = TryCast(acTrans.GetObject(oOid, OpenMode.ForRead), Entity)
                        pathEnt = TryCast(acTrans.GetObject(sweepEnt.ObjectId, OpenMode.ForRead), Curve)

                        dStartX = pathEnt.StartPoint.X
                        dStartY = pathEnt.StartPoint.Y
                        bandPL.AddVertexAt(0, New Point2d(dStartX, dStartY), 0, 0, 0)
                        bandPL.AddVertexAt(1, New Point2d(dStartX - (dBreedte / 2), dStartY), 0, 0, 0)
                        bandPL.AddVertexAt(2, New Point2d(dStartX - (dBreedte / 2), dStartY + dHoogte), 0, 0, 0)
                        bandPL.AddVertexAt(3, New Point2d(dStartX + (dBreedte / 2), dStartY + dHoogte), 0, 0, 0)
                        bandPL.AddVertexAt(4, New Point2d(dStartX + (dBreedte / 2), dStartY), 0, 0, 0)
                        'bandPL.AddVertexAt(5, New Point2d(dStartX, dStartY), 0, 0, 0)
                        bandPL.Closed = True

                        Dim band3D As New Solid3d()
                        Dim sob As SweepOptionsBuilder = New SweepOptionsBuilder()
                        sob.Align = SweepOptionsAlignOption.AlignSweepEntityToPath
                        sob.BasePoint = bandPL.StartPoint
                        sob.Bank = True
                        band3D.CreateSweptSolid(bandPL, pathEnt, sob.ToSweepOptions())
                        acBlkTblRec.AppendEntity(band3D)
                        acTrans.AddNewlyCreatedDBObject(band3D, True)
                    Catch ex As Autodesk.AutoCAD.Runtime.Exception
                        'fout, waarschijnlijk vanwege gesloten polyline
                        'polyline omzetten naar lines en los sweepen
                        If SweepClosePolyline(oOid, dBreedte, dHoogte) = False Then
                            MsgBox("Er ging iets fout!" & vbCrLf & ex.Message)
                        End If
                    End Try
                Next
                acTrans.Commit()
                acEd.Regen()
            End Using
        End Using
        Return True
    End Function

    Public Function SweepClosePolyline(ByVal oOid As ObjectId, Optional dBreedte As Double = 0.2, Optional dHoogte As Double = 0.2)
        Dim dStartX As Double
        Dim dStartY As Double
        Dim bExitLoop As Boolean = False
        Using acLockDoc As DocumentLock = acDoc.LockDocument()
            Using acTrans As Transaction = acCurDb.TransactionManager.StartTransaction()
                Dim acBlkTbl As BlockTable = acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForWrite)
                Dim acBlkTblRec As BlockTableRecord = acTrans.GetObject(acBlkTbl(BlockTableRecord.ModelSpace), OpenMode.ForWrite)

                Dim plineSource As Polyline = TryCast(acTrans.GetObject(oOid, OpenMode.ForRead), Polyline)

                For i As Integer = 0 To plineSource.NumberOfVertices - 1
                    Dim pathEnt As Curve
                    Dim sweLine As Line
                    Dim type As String = "line"
                    If plineSource.GetSegmentType(i) = SegmentType.Arc Then
                        Dim tmpArc As CircularArc2d = plineSource.GetArcSegment2dAt(i)
                        Dim nwArc As Arc = New Arc()
                        nwArc.StartPoint = New Point3d(tmpArc.StartPoint.X, tmpArc.StartPoint.Y, 0)
                        nwArc.EndPoint = New Point3d(tmpArc.EndPoint.X, tmpArc.EndPoint.Y, 0)
                        nwArc.EndAngle = tmpArc.EndAngle
                        nwArc.StartAngle = tmpArc.StartAngle
                        nwArc.Radius = tmpArc.Radius
                        nwArc.Center = New Point3d(tmpArc.Center.X, tmpArc.Center.Y, 0)
                        pathEnt = TryCast(nwArc, Curve)
                    ElseIf plineSource.GetSegmentType(i) = SegmentType.Line Then
                        sweLine = New Line()
                        Dim pStart As Point3d
                        Dim pEind As Point3d
                        If i = plineSource.NumberOfVertices - 1 Then
                            pEind = plineSource.GetPoint3dAt(i)
                            pStart = plineSource.GetPoint3dAt(0)
                            bExitLoop = True
                        Else
                            pStart = plineSource.GetPoint3dAt(i)
                            pEind = plineSource.GetPoint3dAt(i + 1)
                        End If
                        sweLine.StartPoint = pStart
                        sweLine.EndPoint = pEind
                        pathEnt = TryCast(sweLine, Curve)
                    End If

                    Dim bandPL As Polyline = New Polyline()
                    Try
                        dStartX = pathEnt.StartPoint.X
                        dStartY = pathEnt.StartPoint.Y
                        bandPL.AddVertexAt(0, New Point2d(dStartX, dStartY), 0, 0, 0)
                        bandPL.AddVertexAt(1, New Point2d(dStartX - (dBreedte / 2), dStartY), 0, 0, 0)
                        bandPL.AddVertexAt(2, New Point2d(dStartX - (dBreedte / 2), dStartY + dHoogte), 0, 0, 0)
                        bandPL.AddVertexAt(3, New Point2d(dStartX + (dBreedte / 2), dStartY + dHoogte), 0, 0, 0)
                        bandPL.AddVertexAt(4, New Point2d(dStartX + (dBreedte / 2), dStartY), 0, 0, 0)
                        'bandPL.AddVertexAt(5, New Point2d(dStartX, dStartY), 0, 0, 0)
                        bandPL.Closed = True

                        Dim band3D As New Solid3d()
                        Dim sob As SweepOptionsBuilder = New SweepOptionsBuilder()
                        sob.Align = SweepOptionsAlignOption.AlignSweepEntityToPath
                        sob.BasePoint = bandPL.StartPoint
                        sob.Bank = True
                        band3D.CreateSweptSolid(bandPL, pathEnt, sob.ToSweepOptions())
                        acBlkTblRec.AppendEntity(band3D)
                        acTrans.AddNewlyCreatedDBObject(band3D, True)
                        If bExitLoop = True Then Exit For
                    Catch ex As Exception
                        MsgBox(ex.Message)
                    End Try
                Next
                acTrans.Commit()
            End Using
        End Using
        Return True
    End Function


    Private Sub frmExport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        loadLayers()
    End Sub

    Public Sub loadLayers()
        'load layers for Dropdown
        Dim layerNames As StringCollection = New StringCollection()
        Dim iTeller As Integer = 0
        Dim sc As ImportExportCommands = New ImportExportCommands()
        If sc.GetLayers(layerNames) Then
            cmdSweep.Enabled = True
            Dim layerName As String = Nothing
            cmbLayer.Items.Clear()
            For Each layerName In layerNames
                cmbLayer.Items.Add(layerName)
            Next
        Else
            cmdSweep.Enabled = False
        End If
    End Sub

    Public Sub setCurrentLayer(ByVal sLayer As String)
        Using acLockDoc As DocumentLock = acDoc.LockDocument()
            '' Start a transaction
            Using acTrans As Transaction = acCurDb.TransactionManager.StartTransaction()
                '' Open the Layer table for read
                Dim LayerTable As LayerTable = acTrans.GetObject(acCurDb.LayerTableId, OpenMode.ForRead)
                If LayerTable.Has(sLayer) Then
                    acCurDb.Clayer = LayerTable(sLayer)
                End If
                '' Save the changes and dispose of the transaction
                acTrans.Commit()
            End Using
        End Using
    End Sub

    Public Function GetEntitiesOnLayer(ByVal layerName As String) As ObjectIdCollection
        ' Build a filter list so that only entities
        ' on the specified layer are selected
        Dim tvs() As TypedValue = New TypedValue() {New TypedValue(CType(DxfCode.LayerName, Integer), layerName)}
        Dim sf As SelectionFilter = New SelectionFilter(tvs)
        Dim psr As PromptSelectionResult = acEd.SelectAll(sf)
        If (psr.Status = PromptStatus.OK) Then
            Return New ObjectIdCollection(psr.Value.GetObjectIds)
        Else
            Return New ObjectIdCollection
        End If

    End Function

    Private Sub cmdReloadLayers_Click(sender As Object, e As EventArgs) Handles cmdReloadLayers.Click
        loadLayers()
    End Sub

    Private Sub cmdManual_Click(sender As Object, e As EventArgs) Handles cmdManual.Click
        Dim dBreedte As Double = CDbl(txtBreedte.Text.Replace(",", "."))
        Dim dHoogte As Double = CDbl(txtHoogte.Text.Replace(",", "."))
        CreateSweepCurb()

    End Sub





End Class