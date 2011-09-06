Imports System.IO
Public Class Updater
    ' Button values
    Public topbutton As String = ""
    Public bottombutton As String = ""
    ' File paths
    Public rundir As String = Directory.GetCurrentDirectory()
    ' Update
    Public contacturl As String = "http://theiphonewiki.com/wiki/index.php?title=User:Balloonhead66/Latest_stable_software_release/iDecryptIt/"
    Public major As String = "5"
    Public updatemajor As String = ""
    Public updatemajorurl As String = contacturl + "major&action=raw"
    Public minor As String = "00"
    Public updateminor As String = ""
    Public updateminorurl As String = contacturl + "minor&action=raw"
    Public rev As String = "6"
    Public updaterev As String = ""
    Public updaterevurl As String = contacturl + "revision&action=raw"
    Public build As String = "1G28"
    Public updatebuild As String = ""
    Public updatebuildurl As String = contacturl + "build&action=raw"
    Private Sub Updater_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Me.Visibility = Windows.Visibility.Hidden
        Try
            Dim clientCheck = New System.Net.WebClient()
            ' Major
            clientCheck.DownloadFile(updatemajorurl, rundir + "\major.txt")
            Dim majorchecker As New System.IO.StreamReader(rundir + "\major.txt")
            updatemajor = majorchecker.ReadToEnd
            ' Minor
            clientCheck.DownloadFile(updateminorurl, rundir + "\minor.txt")
            Dim minorchecker As New System.IO.StreamReader(rundir + "\minor.txt")
            updateminor = minorchecker.ReadToEnd
            ' Revision
            clientCheck.DownloadFile(updaterevurl, rundir + "\rev.txt")
            Dim revchecker As New System.IO.StreamReader(rundir + "\rev.txt")
            updaterev = revchecker.ReadToEnd
            ' Build
            clientCheck.DownloadFile(updatebuildurl, rundir + "\build.txt")
            Dim buildchecker As New System.IO.StreamReader(rundir + "\build.txt")
            updatebuild = buildchecker.ReadToEnd
            ' Close lock handle on file
            majorchecker.Close()
            minorchecker.Close()
            revchecker.Close()
            buildchecker.Close()
            Call compare()
        Catch ex As Exception
            MsgBox("Unable to contact The iPhone Wiki to download version info!", MsgBoxStyle.OkOnly, "Error!")
            Me.Close()
        End Try
    End Sub
    Public Sub compare()
        Me.Visibility = Windows.Visibility.Visible
        Me.txtInstalled.Text = "Installed version: " + major + "." + minor + "." + rev + " (Build " + build + ")"
        Me.txtAvailable.Text = "Latest version: " + updatemajor + "." + updateminor + "." + updaterev + " (Build " + updatebuild + ")"
        If build = updatebuild Then
            ' This is meant to be run in the background
            Me.Close()
        Else
            ' Titles
            Me.Title = "Update Available"
            Me.TitleHeader.Text = "Update Available"
            ' Image
            Me.ImageCheck.Visibility = Windows.Visibility.Hidden
            Me.ImageAvailable.Visibility = Windows.Visibility.Visible
            ' Buttons
            Me.btnTop.Visibility = Windows.Visibility.Visible
            Me.btnBottom.Visibility = Windows.Visibility.Visible
        End If
    End Sub
    Private Sub btnBottom_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnBottom.Click
        Me.Close()
    End Sub
    Private Sub btnTop_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnTop.Click
        ' Implement some download code using handles like above and use 7zip to extract it
        ' First need to get 7-zip working on the main EXE
    End Sub
End Class
