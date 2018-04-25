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

    Private Sub cmdHatchToBoundary_Click(sender As Object, e As EventArgs) Handles cmdHatchToBoundary.Click
        If MsgBox("Hiermee worden de Arceringen omgezet naar Poylylines, de originele Arceringen worden verwijderd!" & vbCrLf & "Wilt u doorgaan?", vbYesNo) = vbYes Then
            Me.Hide()
            'set focus to modal space
            Autodesk.AutoCAD.Internal.Utils.SetFocusToDwgView()
            RHB()
            Me.Show()
        End If
    End Sub

    Public Sub LoopLayers()
        Dim sc As ImportExportCommands = New ImportExportCommands()
        Dim layerNames As StringCollection = New StringCollection()
        If sc.GetLayers(layerNames) Then
            Dim layerName As String = Nothing
            For Each layerName In layerNames


            Next
        End If
    End Sub

    Public Sub convertHatch(sLayer As String)
        Dim acDoc As Document = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument
        Dim acCurDb As Database = acDoc.Database
        Dim acEd As Editor = acDoc.Editor
        Using acTrans As Transaction = acCurDb.TransactionManager.StartTransaction()
            Dim acLyrTbl As LayerTable
            acLyrTbl = acTrans.GetObject(acCurDb.LayerTableId, OpenMode.ForRead)
            acCurDb.Clayer() = acLyrTbl(sLayer)
            acTrans.Commit()

            'select all objects on layer
            Dim tvs As TypedValue() = New TypedValue(0) {New TypedValue(CInt(DxfCode.LayerName), sLayer)}
            Dim sf As SelectionFilter = New SelectionFilter(tvs)
            Dim psr As PromptSelectionResult = acEd.SelectAll(sf)
            If psr.Status = PromptStatus.OK Then
                Dim oObjectIDS As ObjectIdCollection = New ObjectIdCollection(psr.Value.GetObjectIds())



            Else

            End If
        End Using
    End Sub

    Public Sub RHB()
        'Source: https://forums.autodesk.com/t5/net/restore-hatch-boundaries-if-they-have-been-lost-with-net/m-p/3779514#M33429
        Dim doc As Document = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument
        Dim ed As Editor = doc.Editor
        Dim prOps As PromptEntityOptions = New PromptEntityOptions("" & vbLf & "Select Hatch: ")
        prOps.SetRejectMessage("" & vbLf & "Not a Hatch")
        prOps.AddAllowedClass(GetType(Hatch), False)
        Dim prRes As PromptEntityResult = ed.GetEntity(prOps)
        If (prRes.Status <> PromptStatus.OK) Then
            Return
        End If


        Using acLockDoc As DocumentLock = doc.LockDocument()
            Dim tr As Transaction = doc.TransactionManager.StartTransaction
            Dim hatch As Hatch = CType(tr.GetObject(prRes.ObjectId, OpenMode.ForRead), Hatch)
            If (Not (hatch) Is Nothing) Then
                Dim btr As BlockTableRecord = CType(tr.GetObject(hatch.OwnerId, OpenMode.ForWrite), BlockTableRecord)
                If (btr Is Nothing) Then
                    Return
                End If

                Dim plane As Plane = hatch.GetPlane
                Dim nLoops As Integer = hatch.NumberOfLoops
                Dim i As Integer = 0
                Do While (i < nLoops)
                    Dim Hloop As HatchLoop = hatch.GetLoopAt(i)
                    If Hloop.IsPolyline Then
                        Dim poly As Polyline = New Polyline
                        Dim iVertex As Integer = 0
                        For Each bv As BulgeVertex In Hloop.Polyline
                            poly.AddVertexAt(iVertex, bv.Vertex, bv.Bulge, 0, 0)
                            iVertex = iVertex + 1
                        Next
                        btr.AppendEntity(poly)
                        tr.AddNewlyCreatedDBObject(poly, True)
                    Else
                        For Each cv As Curve2d In Hloop.Curves
                            'Dim line2d As LineSegment2d = Nothing
                            'Dim arc2d As CircularArc2d = Nothing
                            'Dim ellipse2d As EllipticalArc2d = Nothing
                            'Dim spline2d As NurbCurve2d = Nothing
                            'Try
                            '    line2d = CType(cv, LineSegment2d)
                            '    arc2d = CType(cv, CircularArc2d)
                            '    ellipse2d = CType(cv, EllipticalArc2d)
                            '    spline2d = CType(cv, NurbCurve2d)
                            'Catch ex As Exception

                            'End Try
                            Dim line2d As LineSegment2d = TryCast(cv, LineSegment2d)
                            Dim arc2d As CircularArc2d = TryCast(cv, CircularArc2d)
                            Dim ellipse2d As EllipticalArc2d = TryCast(cv, EllipticalArc2d)
                            Dim spline2d As NurbCurve2d = TryCast(cv, NurbCurve2d)
                            If (Not (line2d) Is Nothing) Then
                                Dim ent As Line = New Line
                                ent.StartPoint = New Point3d(plane, line2d.StartPoint)
                                ent.EndPoint = New Point3d(plane, line2d.EndPoint)
                                btr.AppendEntity(ent)
                                tr.AddNewlyCreatedDBObject(ent, True)
                            ElseIf (Not (arc2d) Is Nothing) Then
                                If (Math.Abs((arc2d.StartAngle - arc2d.EndAngle)) < 0.00001) Then
                                    Dim ent As Circle = New Circle(New Point3d(plane, arc2d.Center), plane.Normal, arc2d.Radius)
                                    btr.AppendEntity(ent)
                                    tr.AddNewlyCreatedDBObject(ent, True)
                                Else
                                    Dim angle As Double = (New Vector3d(plane, arc2d.ReferenceVector).AngleOnPlane(plane))
                                    Dim ent As Arc = New Arc(New Point3d(plane, arc2d.Center), arc2d.Radius, (arc2d.StartAngle + angle), (arc2d.EndAngle + angle))
                                    btr.AppendEntity(ent)
                                    tr.AddNewlyCreatedDBObject(ent, True)
                                End If

                            ElseIf (Not (ellipse2d) Is Nothing) Then
                                '-------------------------------------------------------------------------------------------
                                ' Bug: Can not assign StartParam and EndParam of Ellipse:
                                ' Ellipse ent = new Ellipse(new Point3d(plane, e2d.Center), plane.Normal, 
                                '      new Vector3d(plane,e2d.MajorAxis) * e2d.MajorRadius,
                                '      e2d.MinorRadius / e2d.MajorRadius, e2d.StartAngle, e2d.EndAngle);
                                ' ent.StartParam = e2d.StartAngle; 
                                ' ent.EndParam = e2d.EndAngle;
                                ' error CS0200: Property or indexer 'Autodesk.AutoCAD.DatabaseServices.Curve.StartParam' cannot be assigned to -- it is read only
                                ' error CS0200: Property or indexer 'Autodesk.AutoCAD.DatabaseServices.Curve.EndParam' cannot be assigned to -- it is read only
                                '---------------------------------------------------------------------------------------------
                                ' Workaround is using Reflection
                                ' 
                                Dim ent As Ellipse = New Ellipse(New Point3d(plane, ellipse2d.Center), plane.Normal, (New Vector3d(plane, ellipse2d.MajorAxis) * ellipse2d.MajorRadius), (ellipse2d.MinorRadius / ellipse2d.MajorRadius), ellipse2d.StartAngle, ellipse2d.EndAngle)
                                ent.GetType.InvokeMember("StartParam", BindingFlags.SetProperty, Nothing, ent, New Object() {ellipse2d.StartAngle})
                                ent.GetType.InvokeMember("EndParam", BindingFlags.SetProperty, Nothing, ent, New Object() {ellipse2d.EndAngle})
                                btr.AppendEntity(ent)
                                tr.AddNewlyCreatedDBObject(ent, True)
                            ElseIf (Not (spline2d) Is Nothing) Then
                                If spline2d.HasFitData Then
                                    Dim n2fd As NurbCurve2dFitData = spline2d.FitData
                                    Dim p3ds As Point3dCollection = New Point3dCollection
                                    For Each p As Point2d In n2fd.FitPoints
                                        p3ds.Add(New Point3d(plane, p))
                                    Next
                                    Dim ent As Spline = New Spline(p3ds, New Vector3d(plane, n2fd.StartTangent), New Vector3d(plane, n2fd.EndTangent), n2fd.Degree, n2fd.FitTolerance.EqualPoint)
                                    btr.AppendEntity(ent)
                                    tr.AddNewlyCreatedDBObject(ent, True)
                                Else
                                    Dim n2fd As NurbCurve2dData = spline2d.DefinitionData
                                    Dim p3ds As Point3dCollection = New Point3dCollection
                                    Dim knots As DoubleCollection = New DoubleCollection(n2fd.Knots.Count)
                                    For Each p As Point2d In n2fd.ControlPoints
                                        p3ds.Add(New Point3d(plane, p))
                                    Next
                                    For Each k As Double In n2fd.Knots
                                        knots.Add(k)
                                    Next
                                    Dim period As Double = 0
                                    Dim ent As Spline = New Spline(n2fd.Degree, n2fd.Rational, spline2d.IsClosed, spline2d.IsPeriodic(period), p3ds, knots, n2fd.Weights, n2fd.Knots.Tolerance, n2fd.Knots.Tolerance)
                                    btr.AppendEntity(ent)
                                    tr.AddNewlyCreatedDBObject(ent, True)
                                End If

                            End If

                        Next
                    End If

                    i = (i + 1)
                Loop

            End If

            tr.Commit()
        End Using
    End Sub
End Class