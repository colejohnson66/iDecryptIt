Public Class SelectLangControl
    Private Sub btnOK_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles btnOK.Click
        Dim selected As String = cmbSelect.SelectedIndex
        If selected = "0" Then
            Call enter("en")
            MsgBox("You must restart iDecryptIt for changes to take effect.", MsgBoxStyle.Information)
        ElseIf selected = "1" Then
            Call enter("es")
            MsgBox("Debe reiniciar iDecryptIt que los cambios surtan efecto.", MsgBoxStyle.Information)
        End If
        Me.Close()
    End Sub
    Public Sub enter(ByVal lang As String)
        Dim langcode As Microsoft.Win32.RegistryKey
        langcode = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Cole Stuff\\iDecryptIt")
        langcode = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Cole Stuff\\iDecryptIt", True)
        Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\SOFTWARE\\Cole Stuff\\iDecryptIt", "language", lang, Microsoft.Win32.RegistryValueKind.String)
    End Sub
End Class
