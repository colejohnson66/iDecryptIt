Imports System.IO
Public Class Updater
    ' File paths
    Public tempdir As String = Path.GetTempPath + "idecryptit\"
    ' Update
    Public contacturl As String = "http://theiphonewiki.com/wiki/index.php?title=User:Balloonhead66/Latest_stable_software_release/iDecryptIt&action=raw"
    Public checkerversion As String = ""
    Public checkerarr() As String
    Public major As String = "5"
    Public updatemajor As String = ""
    Public minor As String = "00"
    Public updateminor As String = ""
    Public rev As String = "7"
    Public updaterev As String = ""
    Public build As String = "1H35"
    Public updatebuild As String = ""
    Private Sub Updater_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        ' Me.Loaded is not called unless the window is shown for some reason
        Me.Visibility = Windows.Visibility.Hidden
        Me.btnTop.Visibility = Windows.Visibility.Hidden
        Try
            ' Download the raw code
            Dim clientCheck = New System.Net.WebClient()
            clientCheck.DownloadFile(contacturl, tempdir + "update.txt")
            Dim checker As New StreamReader(tempdir + "update.txt")
            checkerversion = checker.ReadToEnd
            checker.Close()
        Catch ex As Exception
            MsgBox("Unable to contact The iPhone Wiki to download version info!", MsgBoxStyle.OkOnly, "Error!")
            Me.Close()
        End Try
        ' Split the "#.##.#.#### <small>({@} ##, 201#)</small>" by the space
        checkerarr = checkerversion.Split(" ")
        ' Split the "#.##.#.####" by the period
        checkerarr = checkerarr(0).Split(".")
        updatemajor = checkerarr(0)
        updateminor = checkerarr(1)
        updaterev = checkerarr(2)
        updatebuild = checkerarr(3)
        ' Actual comparison
        ' This is meant to be run in the background
        If build = updatebuild Then
            Me.Close()
        Else
            Me.Title = "Update Available"
            Me.TitleHeader.Text = "Update Available"
            Me.Visibility = Windows.Visibility.Visible
            Me.txtInstalled.Text = "Installed version: " + major + "." + minor + "." + rev + " (Build " + build + ")"
            Me.txtAvailable.Text = "Latest version: " + updatemajor + "." + updateminor + "." + updaterev + " (Build " + updatebuild + ")"
        End If

    End Sub
    Private Sub btnBottom_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnBottom.Click
        Me.Close()
    End Sub
    Private Sub btnTop_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnTop.Click
        ' Implement some download code using handles like above and use 7zip to extract it
        ' First need to get 7-zip working on the main EXE
        ' Download URL: "http://" + server + ".dl.sourceforge.net/project/idecryptit/" + _
        ' updatemajor + ".x/iDecryptIt_" + updatemajor + "." + updateminor + "." + updaterev + "." + updatebuild + ".patch.zip"
        ' Then we need to update the installed version key in the registry
    End Sub
End Class
