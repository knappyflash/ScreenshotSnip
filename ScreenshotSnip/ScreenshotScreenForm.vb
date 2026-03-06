Imports System.Drawing.Imaging

Public Class ScreenshotScreenForm

    Public WindowCaptureBitmap As Bitmap
    Public DarkBaseBitmap As Bitmap
    Public WindowCaptureBitmapWithSnipArea As Bitmap
    Public SnipBitmap As Bitmap

    Public SnipRect As New Rectangle(0, 0, 1, 1)
    Public SnipStartPosition As Point
    Public SnipEndPosition As Point

    Private Sub ScreenshotScreenForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CaptureCurrentScreenToBitmap()
    End Sub

    Public Sub CaptureCurrentScreenToBitmap()
        Dim scr As Screen = Screen.FromHandle(Me.Handle)
        Dim b As Rectangle = scr.Bounds

        If Me.WindowCaptureBitmap IsNot Nothing Then Me.WindowCaptureBitmap.Dispose()
        If Me.DarkBaseBitmap IsNot Nothing Then Me.DarkBaseBitmap.Dispose()
        If Me.WindowCaptureBitmapWithSnipArea IsNot Nothing Then Me.WindowCaptureBitmapWithSnipArea.Dispose()
        If Me.SnipBitmap IsNot Nothing Then Me.SnipBitmap.Dispose()

        WindowCaptureBitmap = New Bitmap(b.Width, b.Height, PixelFormat.Format32bppArgb)
        Using g As Graphics = Graphics.FromImage(WindowCaptureBitmap)
            g.CopyFromScreen(b.X, b.Y, 0, 0, b.Size, CopyPixelOperation.SourceCopy)
        End Using

        Me.DarkBaseBitmap = DarkenBitmapByPercent(Me.WindowCaptureBitmap, 50.0F)

        Me.WindowCaptureBitmapWithSnipArea = CType(Me.DarkBaseBitmap.Clone(), Bitmap)

        Me.Invalidate()
    End Sub

    Public Sub CloseScreenForm()
        Me.WindowCaptureBitmap = Nothing
        Me.DarkBaseBitmap = Nothing
        Me.WindowCaptureBitmapWithSnipArea = Nothing
        Me.SnipBitmap = Nothing
        Me.SnipRect = Nothing
        Me.SnipStartPosition = Nothing
        Me.SnipEndPosition = Nothing
        Me.Close()
    End Sub

    Private Sub ScreenshotScreenForm_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
        If Me.WindowCaptureBitmapWithSnipArea IsNot Nothing Then
            e.Graphics.DrawImage(Me.WindowCaptureBitmapWithSnipArea, 0, 0)
        End If

        If Me.SnipRect.Width > 1 AndAlso Me.SnipRect.Height > 1 Then
            Using pen As New Pen(Color.DeepSkyBlue, 2)
                e.Graphics.DrawRectangle(pen, Me.SnipRect)
            End Using
        End If
    End Sub

    Public Function DarkenBitmapByPercent(source As Bitmap, percent As Single) As Bitmap
        percent = Math.Max(0.0F, Math.Min(100.0F, percent))
        Dim factor As Single = 1.0F - (percent / 100.0F)

        Dim result As New Bitmap(source.Width, source.Height, PixelFormat.Format32bppArgb)

        Using g As Graphics = Graphics.FromImage(result)
            Dim matrix As New ColorMatrix(New Single()() {
                New Single() {factor, 0, 0, 0, 0},
                New Single() {0, factor, 0, 0, 0},
                New Single() {0, 0, factor, 0, 0},
                New Single() {0, 0, 0, 1, 0},
                New Single() {0, 0, 0, 0, 1}
            })

            Using attributes As New ImageAttributes()
                attributes.SetColorMatrix(matrix)
                g.DrawImage(source,
                            New Rectangle(0, 0, source.Width, source.Height),
                            0, 0, source.Width, source.Height,
                            GraphicsUnit.Pixel,
                            attributes)
            End Using
        End Using

        Return result
    End Function

    Private Sub ScreenshotScreenForm_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
        Me.SnipStartPosition = Me.PointToClient(Cursor.Position)
        Me.SnipEndPosition = Me.SnipStartPosition
        Timer1.Start()
    End Sub

    Private Sub ScreenshotScreenForm_MouseUp(sender As Object, e As MouseEventArgs) Handles Me.MouseUp
        Timer1.Stop()

        Me.SnipRect = NormalizeRect(Me.SnipStartPosition, Me.SnipEndPosition)
        Me.SnipRect.Intersect(New Rectangle(Point.Empty, Me.WindowCaptureBitmap.Size))
        If Me.SnipRect.Width > 0 AndAlso Me.SnipRect.Height > 0 Then
            If Me.SnipBitmap IsNot Nothing Then Me.SnipBitmap.Dispose()
            Me.SnipBitmap = Me.WindowCaptureBitmap.Clone(Me.SnipRect, PixelFormat.Format32bppArgb)
        End If

        If Me.SnipStartPosition = Me.SnipEndPosition Then
            Me.WindowCaptureBitmap.Save("screen_snip.png", ImageFormat.Png)
            Form1.PictureBox1.Image = Me.WindowCaptureBitmap
        Else
            Me.SnipBitmap.Save("screen_snip.png", ImageFormat.Png)
            Form1.PictureBox1.Image = Me.SnipBitmap
        End If

        Me.CloseScreenForm()

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Me.SnipEndPosition = Me.PointToClient(Cursor.Position)

        Me.SnipRect = NormalizeRect(Me.SnipStartPosition, Me.SnipEndPosition)

        Me.UpdateSnipOverlay()
        Me.Invalidate()
    End Sub

    Private Sub UpdateSnipOverlay()
        If Me.WindowCaptureBitmap Is Nothing OrElse Me.DarkBaseBitmap Is Nothing Then Return

        Dim r As Rectangle = Me.SnipRect
        r.Intersect(New Rectangle(Point.Empty, Me.WindowCaptureBitmap.Size))
        If r.Width <= 0 OrElse r.Height <= 0 Then
            If Me.WindowCaptureBitmapWithSnipArea IsNot Nothing Then Me.WindowCaptureBitmapWithSnipArea.Dispose()
            Me.WindowCaptureBitmapWithSnipArea = CType(Me.DarkBaseBitmap.Clone(), Bitmap)
            Return
        End If

        If Me.WindowCaptureBitmapWithSnipArea IsNot Nothing Then Me.WindowCaptureBitmapWithSnipArea.Dispose()
        Me.WindowCaptureBitmapWithSnipArea = CType(Me.DarkBaseBitmap.Clone(), Bitmap)

        If Me.SnipBitmap IsNot Nothing Then Me.SnipBitmap.Dispose()
        Me.SnipBitmap = Me.WindowCaptureBitmap.Clone(r, PixelFormat.Format32bppArgb)

        Using g As Graphics = Graphics.FromImage(Me.WindowCaptureBitmapWithSnipArea)
            g.DrawImage(Me.SnipBitmap, r.Location)
        End Using
    End Sub

    Private Function NormalizeRect(p1 As Point, p2 As Point) As Rectangle
        Dim x As Integer = Math.Min(p1.X, p2.X)
        Dim y As Integer = Math.Min(p1.Y, p2.Y)
        Dim w As Integer = Math.Abs(p1.X - p2.X)
        Dim h As Integer = Math.Abs(p1.Y - p2.Y)

        If w = 0 Then w = 1
        If h = 0 Then h = 1

        Return New Rectangle(x, y, w, h)
    End Function


End Class