Imports System.IO
Imports System.Net
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
    Public rev As String = "8"
    Public updaterev As String = ""
    Public build As String = "1I76"
    Public updatebuild As String = ""
    Private Sub Updater_Loaded() Handles Me.Loaded
        ' Me.Loaded is not called unless the window is shown for some reason
        Me.Visibility = Windows.Visibility.Hidden
        Me.btnTop.IsEnabled = False
        Try
            ' Download the raw code
            Dim clientCheck = New WebClient()
            clientCheck.DownloadFile(New Uri(contacturl), tempdir + "update.txt")
            Dim checker As New StreamReader(tempdir + "update.txt")
            checkerversion = checker.ReadToEnd
            checker.Close()
        Catch ex As Exception
            MsgBox("Unable to contact The iPhone Wiki to download version info!", MsgBoxStyle.OkOnly, "Error!")
            Me.Close()
        End Try
        ' Split the "#.##.#.####" by the period
        checkerarr = checkerversion.Split(".")
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
            Me.txtHeader.Text = "Update Available"
            Me.Visibility = Windows.Visibility.Visible
            Me.txtInstalled.Text = "Installed version: " + major + "." + minor + "." + rev + " (Build " + build + ")"
            Me.txtAvailable.Text = "Latest version: " + updatemajor + "." + updateminor + "." + updaterev + " (Build " + updatebuild + ")"
        End If
    End Sub
    Private Sub btnBottom_Click() Handles btnBottom.Click
        Me.Close()
    End Sub
    Private Sub btnTop_Click() Handles btnTop.Click
    End Sub
End Class
