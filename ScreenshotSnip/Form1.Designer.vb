<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
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
        components = New ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        PictureBox1 = New PictureBox()
        TakeSnipButton = New Button()
        Timer1 = New Timer(components)
        DrawIconTimer = New Timer(components)
        OpenFolderLocationLabel = New Label()
        CType(PictureBox1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' PictureBox1
        ' 
        PictureBox1.BackColor = Color.White
        PictureBox1.BorderStyle = BorderStyle.FixedSingle
        PictureBox1.Location = New Point(12, 41)
        PictureBox1.Name = "PictureBox1"
        PictureBox1.Size = New Size(636, 252)
        PictureBox1.SizeMode = PictureBoxSizeMode.Zoom
        PictureBox1.TabIndex = 0
        PictureBox1.TabStop = False
        ' 
        ' TakeSnipButton
        ' 
        TakeSnipButton.Cursor = Cursors.Hand
        TakeSnipButton.Location = New Point(532, 12)
        TakeSnipButton.Name = "TakeSnipButton"
        TakeSnipButton.Size = New Size(116, 23)
        TakeSnipButton.TabIndex = 1
        TakeSnipButton.Text = "Print Screen"
        TakeSnipButton.UseVisualStyleBackColor = True
        ' 
        ' Timer1
        ' 
        Timer1.Interval = 1
        ' 
        ' DrawIconTimer
        ' 
        DrawIconTimer.Interval = 34
        ' 
        ' OpenFolderLocationLabel
        ' 
        OpenFolderLocationLabel.AutoSize = True
        OpenFolderLocationLabel.Cursor = Cursors.Hand
        OpenFolderLocationLabel.Font = New Font("Segoe UI", 9F, FontStyle.Underline, GraphicsUnit.Point, CByte(0))
        OpenFolderLocationLabel.Location = New Point(12, 9)
        OpenFolderLocationLabel.Name = "OpenFolderLocationLabel"
        OpenFolderLocationLabel.Size = New Size(121, 15)
        OpenFolderLocationLabel.TabIndex = 2
        OpenFolderLocationLabel.Text = "Open Folder Location"
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(660, 305)
        Controls.Add(OpenFolderLocationLabel)
        Controls.Add(TakeSnipButton)
        Controls.Add(PictureBox1)
        DoubleBuffered = True
        FormBorderStyle = FormBorderStyle.FixedSingle
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        MaximizeBox = False
        Name = "Form1"
        Text = "Screenshot Snip 03/06/2026"
        TopMost = True
        CType(PictureBox1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents TakeSnipButton As Button
    Friend WithEvents Timer1 As Timer
    Friend WithEvents DrawIconTimer As Timer
    Friend WithEvents OpenFolderLocationLabel As Label

End Class
