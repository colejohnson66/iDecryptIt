Public Class SelectLangControl
    Public langcode As Microsoft.Win32.RegistryKey
    Private Sub btnOK_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles btnOK.Click
        Dim selected As String = cmbSelect.SelectedIndex
        If selected = "0" Then
            Call enter("en")
        ElseIf selected = "1" Then
            Call enter("es")
        End If
    End Sub
    Public Sub enter(ByVal lang As String)
        langcode = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Cole Stuff\\iDecryptIt", True)
        langcode.SetValue("language", lang)
    End Sub
End Class
