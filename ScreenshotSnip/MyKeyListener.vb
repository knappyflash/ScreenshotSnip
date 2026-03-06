
Imports System.Runtime.InteropServices

Public Class MyKeyListener
    Implements IDisposable

    Public Event EscapePressed()

    Private Const WH_KEYBOARD_LL As Integer = 13
    Private Const HC_ACTION As Integer = 0
    Private Const WM_KEYDOWN As Integer = &H100
    Private Const WM_SYSKEYDOWN As Integer = &H104

    Private _hookId As IntPtr = IntPtr.Zero
    Private _proc As LowLevelKeyboardProc

    Public Sub New()
        _proc = AddressOf HookCallback
        _hookId = SetWindowsHookEx(WH_KEYBOARD_LL, _proc, IntPtr.Zero, 0)
        If _hookId = IntPtr.Zero Then
            Throw New System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error())
        End If
    End Sub

    Private Delegate Function LowLevelKeyboardProc(nCode As Integer, wParam As IntPtr, lParam As IntPtr) As IntPtr

    <StructLayout(LayoutKind.Sequential)>
    Private Structure KBDLLHOOKSTRUCT
        Public vkCode As UInteger
        Public scanCode As UInteger
        Public flags As UInteger
        Public time As UInteger
        Public dwExtraInfo As UIntPtr
    End Structure

    Private Function HookCallback(nCode As Integer, wParam As IntPtr, lParam As IntPtr) As IntPtr
        If nCode = HC_ACTION Then
            Dim msg As Integer = wParam.ToInt32()
            If msg = WM_KEYDOWN OrElse msg = WM_SYSKEYDOWN Then
                Dim data As KBDLLHOOKSTRUCT = Marshal.PtrToStructure(Of KBDLLHOOKSTRUCT)(lParam)

                If CType(data.vkCode, Keys) = Keys.Escape Then
                    RaiseEvent EscapePressed()

                    Return CType(1, IntPtr)
                End If
            End If
        End If

        Return CallNextHookEx(_hookId, nCode, wParam, lParam)
    End Function

    Public Sub Dispose() Implements IDisposable.Dispose
        If _hookId <> IntPtr.Zero Then
            UnhookWindowsHookEx(_hookId)
            _hookId = IntPtr.Zero
        End If
    End Sub

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
    Private Shared Function SetWindowsHookEx(idHook As Integer,
                                           lpfn As LowLevelKeyboardProc,
                                           hMod As IntPtr,
                                           dwThreadId As UInteger) As IntPtr
    End Function

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
    Private Shared Function CallNextHookEx(hhk As IntPtr,
                                         nCode As Integer,
                                         wParam As IntPtr,
                                         lParam As IntPtr) As IntPtr
    End Function

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
    Private Shared Function UnhookWindowsHookEx(hhk As IntPtr) As Boolean
    End Function
End Class
