' (C) Copyright 2011 by  
'
Imports System
Imports Autodesk.AutoCAD.Runtime
Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.Geometry
Imports Autodesk.AutoCAD.EditorInput

' This line is not mandatory, but improves loading performances
<Assembly: CommandClass(GetType(RN_Shape_Export.MyCommands))>
Namespace RN_Shape_Export

    Public Class MyCommands


        <CommandMethod("dwg2shape", CommandFlags.Modal + CommandFlags.Session)>
        Public Sub MyCommand() ' This method can have any name
            ' Put your command code here
            Dim dlg As New frmExport
            dlg.Show()
        End Sub



    End Class

End Namespace