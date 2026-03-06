Imports System.ComponentModel

Public Class Form1
    Private myKeyListener As MyKeyListener = New MyKeyListener
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AddHandler Me.myKeyListener.EscapePressed, AddressOf KeyEscapePressed
        DrawIconTimer.Start()
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

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        End
    End Sub



    Private myIcon As New Bitmap(256, 256)
    Private myIconLineCounter As Integer = 50
    Private scissorsOpen As Boolean = False
    Private Sub DrawIconTimer_Tick(sender As Object, e As EventArgs) Handles DrawIconTimer.Tick

        Dim myPen As New Pen(Brushes.Blue, 10)
        Using g As Graphics = Graphics.FromImage(myIcon)
            g.Clear(Color.White)

            If scissorsOpen Then
                g.DrawImage(My.Resources.Resource1.Scissors1, 150, 75, 100, 100)
            Else
                g.DrawImage(My.Resources.Resource1.Scissors2, 150, 75, 100, 100)
            End If

            For i As Integer = 0 To 50 Step 2
                g.DrawLine(myPen, (i * 10) + myIconLineCounter, 10, ((i * 10) + 10) + myIconLineCounter, 10)
                g.DrawLine(myPen, (i * 10) - myIconLineCounter, 245, ((i * 10) + 10) - myIconLineCounter, 245)

                g.DrawLine(myPen, 10, (i * 10) - myIconLineCounter, 10, ((i * 10) + 10) - myIconLineCounter)
                g.DrawLine(myPen, 245, (i * 10) + myIconLineCounter, 245, ((i * 10) + 10) + myIconLineCounter)
            Next

        End Using

        myIconLineCounter += 1
        If myIconLineCounter >= 20 Then
            myIconLineCounter = 0
            scissorsOpen = Not scissorsOpen
        End If

        PictureBox1.Image = myIcon

    End Sub

End Class
