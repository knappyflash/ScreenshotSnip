Imports System.ComponentModel

Public Class Form1
    Private myKeyListener As MyKeyListener = New MyKeyListener

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        OpenFolderLocationLabel.ForeColor = Color.FromArgb(100, 100, 255)

        AddHandler Me.myKeyListener.EscapePressed, AddressOf KeyEscapePressed
        AddHandler Me.myKeyListener.PrintScreenPressed, AddressOf KeyPrintScreenPressed

        DrawIconTimer.Start()
        PictureBox1.Image = Me.myIcon
        Me.Icon = Icon
    End Sub

    Private Sub TakeSnipButton_Click(sender As Object, e As EventArgs) Handles TakeSnipButton.Click
        DrawIconTimer.Stop()
        ScreenshotScreenForm.Show()
        ScreenshotScreenForm.WindowState = FormWindowState.Maximized
        Me.Timer1.Start()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Dim targetScreen As Screen = Screen.FromPoint(Cursor.Position)

        Dim currentScreen As Screen = Screen.FromHandle(ScreenshotScreenForm.Handle)
        If currentScreen.DeviceName = targetScreen.DeviceName Then Return

        ScreenshotScreenForm.Opacity = 0
        ScreenshotScreenForm.WindowState = FormWindowState.Normal
        ScreenshotScreenForm.StartPosition = FormStartPosition.Manual
        ScreenshotScreenForm.Location = targetScreen.Bounds.Location
        ScreenshotScreenForm.Size = targetScreen.Bounds.Size
        ScreenshotScreenForm.CaptureCurrentScreenToBitmap()
        ScreenshotScreenForm.WindowState = FormWindowState.Maximized
        ScreenshotScreenForm.Opacity = 100
    End Sub

    Private Sub KeyEscapePressed()
        Me.Timer1.Stop()
        ScreenshotScreenForm.Close()
    End Sub

    Private Sub KeyPrintScreenPressed()
        TakeSnipButton.PerformClick()
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        End
    End Sub



    Private myIcon As New Bitmap(256, 256)
    Private myIconLineCounter As Integer = 50
    Private scissorsOpen As Boolean = False
    Private myPen As New Pen(Brushes.Blue, 10)
    Private myIconGraphic As Graphics = Graphics.FromImage(Me.myIcon)
    Private myScissors1 As Image = My.Resources.Resource1.Scissors1
    Private myScissors2 As Image = My.Resources.Resource1.Scissors2

    Private Sub DrawIconTimer_Tick(sender As Object, e As EventArgs) Handles DrawIconTimer.Tick


        Me.myIconGraphic.Clear(Color.White)

        If Me.scissorsOpen Then
            Me.myIconGraphic.DrawImage(Me.myScissors1, 150, 75, 100, 100)
        Else
            Me.myIconGraphic.DrawImage(Me.myScissors2, 150, 75, 100, 100)
        End If

        For i As Integer = 0 To 50 Step 2
            Me.myIconGraphic.DrawLine(Me.myPen, (i * 10) + Me.myIconLineCounter, 10, ((i * 10) + 10) + Me.myIconLineCounter, 10)
            Me.myIconGraphic.DrawLine(Me.myPen, (i * 10) - Me.myIconLineCounter, 245, ((i * 10) + 10) - Me.myIconLineCounter, 245)

            Me.myIconGraphic.DrawLine(Me.myPen, 10, (i * 10) - Me.myIconLineCounter, 10, ((i * 10) + 10) - Me.myIconLineCounter)
            Me.myIconGraphic.DrawLine(Me.myPen, 245, (i * 10) + Me.myIconLineCounter, 245, ((i * 10) + 10) + Me.myIconLineCounter)
        Next


        Me.myIconLineCounter += 2
        If Me.myIconLineCounter >= 20 Then
            Me.myIconLineCounter = 0
            Me.scissorsOpen = Not Me.scissorsOpen
        End If

        PictureBox1.Refresh()

    End Sub

    Private Sub OpenFolderLocationLabel_MouseEnter(sender As Object, e As EventArgs) Handles OpenFolderLocationLabel.MouseEnter
        OpenFolderLocationLabel.ForeColor = Color.FromArgb(200, 200, 255)
    End Sub

    Private Sub OpenFolderLocationLabel_MouseLeave(sender As Object, e As EventArgs) Handles OpenFolderLocationLabel.MouseLeave
        OpenFolderLocationLabel.ForeColor = Color.FromArgb(100, 100, 255)
    End Sub

    Private Sub OpenFolderLocationLabel_MouseDown(sender As Object, e As MouseEventArgs) Handles OpenFolderLocationLabel.MouseDown
        OpenFolderLocationLabel.ForeColor = Color.FromArgb(0, 0, 255)
    End Sub

    Private Sub OpenFolderLocationLabel_MouseUp(sender As Object, e As MouseEventArgs) Handles OpenFolderLocationLabel.MouseUp
        OpenFolderLocationLabel.ForeColor = Color.FromArgb(100, 100, 255)
        Process.Start("explorer.exe", Application.StartupPath)
    End Sub

End Class
