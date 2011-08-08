Imports System.IO
Public Class SubmitKey
    Private Sub SubmitKey_Loaded(sender As Object, e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Me.web.Navigate(New Uri(Directory.GetCurrentDirectory() + "\help\submitkey.html"))
    End Sub
End Class
