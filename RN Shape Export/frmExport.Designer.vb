<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmExport
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
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
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'buttonCancel
        '
        Me.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.buttonCancel.Location = New System.Drawing.Point(190, 302)
        Me.buttonCancel.Name = "buttonCancel"
        Me.buttonCancel.Size = New System.Drawing.Size(120, 24)
        Me.buttonCancel.TabIndex = 19
        Me.buttonCancel.Text = "Annuleren"
        '
        'buttonOK
        '
        Me.buttonOK.Location = New System.Drawing.Point(30, 302)
        Me.buttonOK.Name = "buttonOK"
        Me.buttonOK.Size = New System.Drawing.Size(120, 24)
        Me.buttonOK.TabIndex = 18
        Me.buttonOK.Text = "OK"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.radioButtonDiffLayers)
        Me.GroupBox1.Controls.Add(Me.radioButtonAllToOne)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 206)
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
        Me.checkBoxExpOD.Location = New System.Drawing.Point(20, 166)
        Me.checkBoxExpOD.Name = "checkBoxExpOD"
        Me.checkBoxExpOD.Size = New System.Drawing.Size(240, 24)
        Me.checkBoxExpOD.TabIndex = 16
        Me.checkBoxExpOD.Text = "Exporteer Object Data Tables"
        '
        'buttonLogFile
        '
        Me.buttonLogFile.Location = New System.Drawing.Point(276, 124)
        Me.buttonLogFile.Name = "buttonLogFile"
        Me.buttonLogFile.Size = New System.Drawing.Size(72, 24)
        Me.buttonLogFile.TabIndex = 15
        Me.buttonLogFile.Text = "Bladeren"
        '
        'buttonExpFile
        '
        Me.buttonExpFile.Location = New System.Drawing.Point(276, 70)
        Me.buttonExpFile.Name = "buttonExpFile"
        Me.buttonExpFile.Size = New System.Drawing.Size(72, 24)
        Me.buttonExpFile.TabIndex = 13
        Me.buttonExpFile.Text = "Bladeren"
        '
        'textBoxLogFile
        '
        Me.textBoxLogFile.Location = New System.Drawing.Point(20, 126)
        Me.textBoxLogFile.Name = "textBoxLogFile"
        Me.textBoxLogFile.ReadOnly = True
        Me.textBoxLogFile.Size = New System.Drawing.Size(248, 20)
        Me.textBoxLogFile.TabIndex = 14
        '
        'textBoxExpFile
        '
        Me.textBoxExpFile.Location = New System.Drawing.Point(20, 72)
        Me.textBoxExpFile.Name = "textBoxExpFile"
        Me.textBoxExpFile.ReadOnly = True
        Me.textBoxExpFile.Size = New System.Drawing.Size(248, 20)
        Me.textBoxExpFile.TabIndex = 12
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 102)
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
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(389, 73)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TextBox1.Size = New System.Drawing.Size(318, 254)
        Me.TextBox1.TabIndex = 21
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(623, 16)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 22
        Me.Button1.Text = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'frmExport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(719, 347)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.TextBox1)
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
    Friend WithEvents TextBox1 As Windows.Forms.TextBox
    Friend WithEvents Button1 As Windows.Forms.Button
End Class
