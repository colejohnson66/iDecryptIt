Imports Microsoft.Win32
Public Class SelectLangControl
    Private Sub btnOK_Click() Handles btnOK.Click
        Dim home As Window = New MainWindow
        Dim selected As Integer = cmbSelect.SelectedIndex
        If selected = 0 Then
            Call enter("eng")
            MsgBox("You must restart iDecryptIt for changes to take effect.", MsgBoxStyle.Information)
        ElseIf selected = 1 Then
            Call enter("spa")
            MsgBox("Debe reiniciar iDecryptIt que los cambios surtan efecto.", MsgBoxStyle.Information)
        ElseIf selected = 2 Then
            Call enter("hin")
            MsgBox("", MsgBoxStyle.Information)
        End If
        Me.Close()
    End Sub
    Private Sub SelectLangControl_Loaded() Handles Me.Loaded
        Dim lang As String
        Dim langcode As RegistryKey
        langcode = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Cole Stuff\\iDecryptIt", True)
        lang = Registry.GetValue("HKEY_CURRENT_USER\SOFTWARE\\Cole Stuff\\iDecryptIt", "language", "eng")
        ' Failsafe for transition
        If lang = "en" Then
            lang = "eng"
        ElseIf lang = "es" Then
            lang = "spa"
        End If
        ' Change language
        If lang = "eng" Then
            cmbSelect.SelectedIndex = 0
        ElseIf lang = "spa" Then
            cmbSelect.SelectedIndex = 1
        ElseIf lang = "hin" Then
            cmbSelect.SelectedIndex = 2
        Else
            ' Fall back to English if is not any of the above
            Registry.CurrentUser.DeleteSubKey("SOFTWARE\\Cole Stuff\\iDecryptIt")
            cmbSelect.SelectedIndex = 0
        End If
    End Sub
    Public Sub enter(ByVal lang As String)
        Dim langcode As RegistryKey
        langcode = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Cole Stuff\\iDecryptIt")
        langcode = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Cole Stuff\\iDecryptIt", True)
        Registry.SetValue("HKEY_CURRENT_USER\SOFTWARE\\Cole Stuff\\iDecryptIt", "language", lang, RegistryValueKind.String)
    End Sub
End Class
