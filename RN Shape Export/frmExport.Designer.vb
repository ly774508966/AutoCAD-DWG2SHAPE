<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmExport
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmExport))
        Me.buttonCancel = New System.Windows.Forms.Button()
        Me.buttonOK = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.radioButtonDiffLayers = New System.Windows.Forms.RadioButton()
        Me.radioButtonAllToOne = New System.Windows.Forms.RadioButton()
        Me.checkBoxExpOD = New System.Windows.Forms.CheckBox()
        Me.buttonLogFile = New System.Windows.Forms.Button()
        Me.buttonExpFile = New System.Windows.Forms.Button()
        Me.textBoxLogFile = New System.Windows.Forms.TextBox()
        Me.textBoxExpFile = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmdHatchToBoundary = New System.Windows.Forms.Button()
        Me.cmdSweep = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.cmbLayer = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtBreedte = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtHoogte = New System.Windows.Forms.TextBox()
        Me.cmdReloadLayers = New System.Windows.Forms.Button()
        Me.cmdManual = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'buttonCancel
        '
        Me.buttonCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.buttonCancel.Location = New System.Drawing.Point(242, 456)
        Me.buttonCancel.Name = "buttonCancel"
        Me.buttonCancel.Size = New System.Drawing.Size(120, 24)
        Me.buttonCancel.TabIndex = 19
        Me.buttonCancel.Text = "Annuleren"
        '
        'buttonOK
        '
        Me.buttonOK.Location = New System.Drawing.Point(12, 258)
        Me.buttonOK.Name = "buttonOK"
        Me.buttonOK.Size = New System.Drawing.Size(333, 31)
        Me.buttonOK.TabIndex = 18
        Me.buttonOK.Text = "Start Exporteren van Vlakken"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.radioButtonDiffLayers)
        Me.GroupBox1.Controls.Add(Me.radioButtonAllToOne)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 164)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(336, 88)
        Me.GroupBox1.TabIndex = 17
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Selecteer een export methode"
        '
        'radioButtonDiffLayers
        '
        Me.radioButtonDiffLayers.Location = New System.Drawing.Point(24, 48)
        Me.radioButtonDiffLayers.Name = "radioButtonDiffLayers"
        Me.radioButtonDiffLayers.Size = New System.Drawing.Size(280, 24)
        Me.radioButtonDiffLayers.TabIndex = 1
        Me.radioButtonDiffLayers.Text = "Exporteer objecten per laag naar een bestand"
        '
        'radioButtonAllToOne
        '
        Me.radioButtonAllToOne.Checked = True
        Me.radioButtonAllToOne.Location = New System.Drawing.Point(24, 24)
        Me.radioButtonAllToOne.Name = "radioButtonAllToOne"
        Me.radioButtonAllToOne.Size = New System.Drawing.Size(264, 16)
        Me.radioButtonAllToOne.TabIndex = 0
        Me.radioButtonAllToOne.TabStop = True
        Me.radioButtonAllToOne.Text = "Exporteer alle objecten naar één bestand"
        '
        'checkBoxExpOD
        '
        Me.checkBoxExpOD.Location = New System.Drawing.Point(20, 140)
        Me.checkBoxExpOD.Name = "checkBoxExpOD"
        Me.checkBoxExpOD.Size = New System.Drawing.Size(240, 24)
        Me.checkBoxExpOD.TabIndex = 16
        Me.checkBoxExpOD.Text = "Exporteer Object Data Tables"
        '
        'buttonLogFile
        '
        Me.buttonLogFile.Location = New System.Drawing.Point(276, 115)
        Me.buttonLogFile.Name = "buttonLogFile"
        Me.buttonLogFile.Size = New System.Drawing.Size(72, 24)
        Me.buttonLogFile.TabIndex = 15
        Me.buttonLogFile.Text = "Bladeren"
        '
        'buttonExpFile
        '
        Me.buttonExpFile.Location = New System.Drawing.Point(276, 65)
        Me.buttonExpFile.Name = "buttonExpFile"
        Me.buttonExpFile.Size = New System.Drawing.Size(72, 24)
        Me.buttonExpFile.TabIndex = 13
        Me.buttonExpFile.Text = "Bladeren"
        '
        'textBoxLogFile
        '
        Me.textBoxLogFile.Location = New System.Drawing.Point(20, 117)
        Me.textBoxLogFile.Name = "textBoxLogFile"
        Me.textBoxLogFile.ReadOnly = True
        Me.textBoxLogFile.Size = New System.Drawing.Size(248, 20)
        Me.textBoxLogFile.TabIndex = 14
        '
        'textBoxExpFile
        '
        Me.textBoxExpFile.Location = New System.Drawing.Point(20, 67)
        Me.textBoxExpFile.Name = "textBoxExpFile"
        Me.textBoxExpFile.ReadOnly = True
        Me.textBoxExpFile.Size = New System.Drawing.Size(248, 20)
        Me.textBoxExpFile.TabIndex = 12
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 97)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(140, 13)
        Me.Label2.TabIndex = 11
        Me.Label2.Text = "Bestandsnaam Log Bestand"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 48)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(110, 13)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "Export Bestandsnaam"
        '
        'cmdHatchToBoundary
        '
        Me.cmdHatchToBoundary.Location = New System.Drawing.Point(14, 8)
        Me.cmdHatchToBoundary.Name = "cmdHatchToBoundary"
        Me.cmdHatchToBoundary.Size = New System.Drawing.Size(333, 31)
        Me.cmdHatchToBoundary.TabIndex = 20
        Me.cmdHatchToBoundary.Text = "Arceringen omzetten naar Boundary"
        Me.cmdHatchToBoundary.UseVisualStyleBackColor = True
        '
        'cmdSweep
        '
        Me.cmdSweep.Location = New System.Drawing.Point(8, 84)
        Me.cmdSweep.Name = "cmdSweep"
        Me.cmdSweep.Size = New System.Drawing.Size(322, 23)
        Me.cmdSweep.TabIndex = 21
        Me.cmdSweep.Text = "Convert Layer naar 3D Solid"
        Me.cmdSweep.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.cmdManual)
        Me.GroupBox2.Controls.Add(Me.cmdReloadLayers)
        Me.GroupBox2.Controls.Add(Me.txtHoogte)
        Me.GroupBox2.Controls.Add(Me.txtBreedte)
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.cmbLayer)
        Me.GroupBox2.Controls.Add(Me.cmdSweep)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 304)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(336, 146)
        Me.GroupBox2.TabIndex = 22
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Layer omzetten naar 3D Bandenlijn"
        '
        'cmbLayer
        '
        Me.cmbLayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbLayer.FormattingEnabled = True
        Me.cmbLayer.Location = New System.Drawing.Point(8, 28)
        Me.cmbLayer.Name = "cmbLayer"
        Me.cmbLayer.Size = New System.Drawing.Size(296, 21)
        Me.cmbLayer.TabIndex = 22
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(14, 59)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(78, 13)
        Me.Label3.TabIndex = 23
        Me.Label3.Text = "Afmeting band:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(98, 59)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(47, 13)
        Me.Label4.TabIndex = 24
        Me.Label4.Text = "Breedte:"
        '
        'txtBreedte
        '
        Me.txtBreedte.Location = New System.Drawing.Point(148, 56)
        Me.txtBreedte.Name = "txtBreedte"
        Me.txtBreedte.Size = New System.Drawing.Size(49, 20)
        Me.txtBreedte.TabIndex = 25
        Me.txtBreedte.Text = "0.1"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(203, 59)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(45, 13)
        Me.Label5.TabIndex = 24
        Me.Label5.Text = "Hoogte:"
        '
        'txtHoogte
        '
        Me.txtHoogte.Location = New System.Drawing.Point(249, 56)
        Me.txtHoogte.Name = "txtHoogte"
        Me.txtHoogte.Size = New System.Drawing.Size(49, 20)
        Me.txtHoogte.TabIndex = 26
        Me.txtHoogte.Text = "0.1"
        '
        'cmdReloadLayers
        '
        Me.cmdReloadLayers.BackgroundImage = Global.RN_Shape_Export.My.Resources.Resources.refresh
        Me.cmdReloadLayers.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.cmdReloadLayers.Location = New System.Drawing.Point(308, 27)
        Me.cmdReloadLayers.Name = "cmdReloadLayers"
        Me.cmdReloadLayers.Size = New System.Drawing.Size(25, 25)
        Me.cmdReloadLayers.TabIndex = 27
        Me.cmdReloadLayers.UseVisualStyleBackColor = True
        '
        'cmdManual
        '
        Me.cmdManual.Location = New System.Drawing.Point(8, 113)
        Me.cmdManual.Name = "cmdManual"
        Me.cmdManual.Size = New System.Drawing.Size(75, 23)
        Me.cmdManual.TabIndex = 28
        Me.cmdManual.Text = "Pick Object"
        Me.cmdManual.UseVisualStyleBackColor = True
        '
        'frmExport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(365, 492)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.cmdHatchToBoundary)
        Me.Controls.Add(Me.buttonCancel)
        Me.Controls.Add(Me.buttonOK)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.checkBoxExpOD)
        Me.Controls.Add(Me.buttonLogFile)
        Me.Controls.Add(Me.buttonExpFile)
        Me.Controls.Add(Me.textBoxLogFile)
        Me.Controls.Add(Me.textBoxExpFile)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(379, 386)
        Me.Name = "frmExport"
        Me.Text = "Export DWG to Shapefile"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents buttonCancel As Windows.Forms.Button
    Friend WithEvents buttonOK As Windows.Forms.Button
    Friend WithEvents GroupBox1 As Windows.Forms.GroupBox
    Friend WithEvents radioButtonDiffLayers As Windows.Forms.RadioButton
    Friend WithEvents radioButtonAllToOne As Windows.Forms.RadioButton
    Friend WithEvents checkBoxExpOD As Windows.Forms.CheckBox
    Friend WithEvents buttonLogFile As Windows.Forms.Button
    Friend WithEvents buttonExpFile As Windows.Forms.Button
    Friend WithEvents textBoxLogFile As Windows.Forms.TextBox
    Friend WithEvents textBoxExpFile As Windows.Forms.TextBox
    Friend WithEvents Label2 As Windows.Forms.Label
    Friend WithEvents Label1 As Windows.Forms.Label
    Friend WithEvents cmdHatchToBoundary As Windows.Forms.Button
    Friend WithEvents cmdSweep As Windows.Forms.Button
    Friend WithEvents GroupBox2 As Windows.Forms.GroupBox
    Friend WithEvents cmbLayer As Windows.Forms.ComboBox
    Friend WithEvents txtHoogte As Windows.Forms.TextBox
    Friend WithEvents txtBreedte As Windows.Forms.TextBox
    Friend WithEvents Label5 As Windows.Forms.Label
    Friend WithEvents Label4 As Windows.Forms.Label
    Friend WithEvents Label3 As Windows.Forms.Label
    Friend WithEvents cmdReloadLayers As Windows.Forms.Button
    Friend WithEvents cmdManual As Windows.Forms.Button
End Class
