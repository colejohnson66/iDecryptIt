Class MainWindow

    Private Sub btnVFDecryptSite_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnVFDecryptSite.Click
        Me.webBrowser.Navigate(New Uri("http://theiphonewiki.com/wiki/index.php?title=VFDecrypt"))
    End Sub
    Private Sub btnIDecryptItSite_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnIDecryptItSite.Click
        Me.webBrowser.Navigate(New Uri("http://cole.freehostingcloud.com/wiki/iDecryptIt"))
    End Sub
    Private Sub btnColeStuffSite_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnColeStuffSite.Click
        Me.webBrowser.Navigate(New Uri("http://cole.freehostingcloud.com/wiki/Cole_Stuff"))
    End Sub
    Private Sub btnWikipediaPage_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnWikipediaPage.Click
        Me.webBrowser.Navigate(New Uri("http://en.wikipedia.org/wiki/User:Colejohnson66"))
    End Sub
    Private Sub btnAbout_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnAbout.Click
        Me.webBrowser.Navigate(New Uri("file:///C:/Program%20Files/Cole%20Stuff/help/about_iDecryptIt.html"))
    End Sub
    Private Sub btnSelectVFDecryptInutFile_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnSelectVFDecryptInutFile.Click
        Dim decrypt As New Microsoft.Win32.OpenFileDialog()
        decrypt.FileName = ""
        decrypt.DefaultExt = ".dmg"
        decrypt.Filter = "DMG Files (DMG)|*.dmg"
        Dim result? As Boolean = decrypt.ShowDialog()

        If result = True Then
            Me.textInputFileName.Text = decrypt.FileName
        End If
    End Sub
    Private Sub btnSelectExtractFile_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnSelectExtractFile.Click
        Dim extractofd As New Microsoft.Win32.OpenFileDialog()
        extractofd.FileName = ""
        extractofd.DefaultExt = ".dmg"
        extractofd.Filter = "DMG Files (DMG)|*.dmg"
        Dim result? As Boolean = extractofd.ShowDialog()

        If result = True Then
            Me.textExtractFileName.Text = extractofd.FileName
        End If
    End Sub

    Private Sub btniPhone33_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles btniPhone33.Click
        ' 1.x Final
        key1a420.Text = "Unavailable"
        key1a543a.Text = "Unavailable"
        key1c25.Text = "Unavailable"
        key1c28.Text = "Unavailable"
        key3a100a.Text = "Unavailable"
        key3a101a.Text = "Unavailable"
        key3a109a.Text = "Unavailable"
        key3a110a.Text = "Unavailable"
        key3b48b.Text = "Unavailable"
        key4a93.Text = "Unavailable"
        key4a102.Text = "Unavailable"
        key4b1.Text = "Unavailable"
        ' 1.x Beta
        key5a147p.Text = "Unavailable"
        ' 2.x Final
        key5a345final.Text = "Unavailable"
        key5a347.Text = "Unavailable"
        key5b108.Text = "Unavailable"
    End Sub
End Class
