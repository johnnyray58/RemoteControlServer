﻿Imports System.Windows.Forms

Module Keyboard

    'Keycodes, as used in Android
    'http://developer.android.com/reference/android/view/KeyEvent.html

    Public Const KEYCODE_BACK As Integer = 4
    Public Const KEYCODE_CAPS_LOCK As Integer = 115
    Public Const KEYCODE_DEL As Integer = 67
    Public Const KEYCODE_ENTER As Integer = 66
    Public Const KEYCODE_ESCAPE As Integer = 111
    Public Const KEYCODE_INSERT As Integer = 124
    Public Const KEYCODE_MOVE_END As Integer = 123
    Public Const KEYCODE_MOVE_HOME As Integer = 122
    Public Const KEYCODE_PAGE_DOWN As Integer = 93
    Public Const KEYCODE_PAGE_UP As Integer = 92
    Public Const KEYCODE_SPACE As Integer = 62
    Public Const KEYCODE_TAB As Integer = 61
    Public Const KEYCODE_F1 As Integer = 131
    Public Const KEYCODE_F2 As Integer = 132
    Public Const KEYCODE_F3 As Integer = 133
    Public Const KEYCODE_F4 As Integer = 134
    Public Const KEYCODE_F5 As Integer = 135
    Public Const KEYCODE_F6 As Integer = 136
    Public Const KEYCODE_F7 As Integer = 137
    Public Const KEYCODE_F8 As Integer = 138
    Public Const KEYCODE_F9 As Integer = 139
    Public Const KEYCODE_F10 As Integer = 140
    Public Const KEYCODE_F11 As Integer = 141
    Public Const KEYCODE_F12 As Integer = 142

    Public Sub sendKeyPress(ByVal key As Keys)
        If Not key = Nothing Then
            sendKeyDown(key)
            sendKeyUp(key)
        End If
    End Sub

    Public Sub sendUnicodeKeyPress(ByVal character As Char)
        Dim unicode As Integer = AscW(character)
        sendUnicodeKeyPress(unicode)
    End Sub

    Public Sub sendUnicodeKeyPress(ByVal unicode As Integer)
        SendInput.SendUniCodeKey(unicode)
    End Sub

    Public Sub sendKeyDown(ByVal key As Keys)
        Try
            Remote.keybd_event(key, 0, 0, 0) 'Down
        Catch ex As Exception
        End Try
    End Sub

    Public Sub sendKeyUp(ByVal key As Keys)
        Remote.keybd_event(key, 0, 2, 0) 'Up
    End Sub

    Public Sub sendEachKey(ByVal message As String)
        Dim key As Keys
        Dim character As Char
        For i As Integer = 0 To message.Length - 1
            Try
                character = message.Chars(i)
                key = stringToKey(Char.ToUpper(character))
                If key = Nothing Then
                    'Character is unicode, no Windows Key available
                    sendUnicodeKeyPress(character)
                Else
                    If Char.IsUpper(character) Then
                        sendKeyDown(Keys.ShiftKey)
                        sendKeyPress(key)
                        sendKeyUp(Keys.ShiftKey)
                    Else
                        sendKeyPress(key)
                    End If
                End If
            Catch ex As Exception

            End Try
        Next
    End Sub

    Public Sub sendKeys(ByVal message As String)
        System.Windows.Forms.SendKeys.SendWait(message)
    End Sub

    Public Sub sendShortcut(ByVal keys As List(Of Key))
        'Press each key down
        For Each singleKey As Key In keys
            sendKeyDown(singleKey)
        Next

        'Release keys in reverse order
        keys.Reverse()
        For Each singleKey As Key In keys
            sendKeyUp(singleKey)
        Next
    End Sub

    Public Function stringToKey(ByVal key As String) As Keys
        Dim kc As KeysConverter = New KeysConverter()
        Try
            Return CType(kc.ConvertFromString(key), Keys)
        Catch
            Return Keys.None
        End Try
    End Function

    Public Function keycodeToKey(ByVal keyCode As Integer) As Key
        Select Case keyCode
            Case KEYCODE_BACK
                Return Keys.Back
            Case KEYCODE_CAPS_LOCK
                Return Keys.CapsLock
            Case KEYCODE_DEL
                Return Keys.Back
            Case KEYCODE_ENTER
                Return Keys.Return
            Case KEYCODE_ESCAPE
                Return Keys.Escape
            Case KEYCODE_INSERT
                Return Keys.Insert
            Case KEYCODE_MOVE_END
                Return Keys.End
            Case KEYCODE_MOVE_HOME
                Return Keys.Home
            Case KEYCODE_PAGE_DOWN
                Return Keys.PageDown
            Case KEYCODE_PAGE_UP
                Return Keys.PageUp
            Case KEYCODE_SPACE
                Return Keys.Space
            Case KEYCODE_TAB
                Return Keys.Tab
        End Select
        Return Nothing
    End Function

    Public Sub hibernate()
        Process.Start("rundll32.exe", "powrprof.dll,SetSuspendState 1,1,0")
    End Sub

    Public Sub standby()
        'If hibernate is enabled, this will call hibernate instead
        Process.Start("rundll32.exe", "powrprof.dll,SetSuspendState 0,1,0")
    End Sub

    Public Sub shutdown()
        Process.Start("shutdown.exe", "-s -t 00")
    End Sub

    Public Sub restart()
        Process.Start("shutdown.exe", "-r -t 00")
    End Sub



End Module
