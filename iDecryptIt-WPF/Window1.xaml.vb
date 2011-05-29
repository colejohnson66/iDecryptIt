Imports System.IO
Public Class Window1
    ' Button values
    Public topbutton As String = ""
    Public bottombutton As String = ""
    ' File paths
    Public rundir As String = Directory.GetCurrentDirectory()
    ' Update
    Public build As String = "1C52"
    Public updatebuild As String = ""
    Public updatebuildurl As String = "http://theiphonewiki.com/wiki/index.php?title=User:Balloonhead66/Latest_stable_software_release/iDecryptIt/build&action=raw"
    Private Sub Window1_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            Dim clientCheck = New System.Net.WebClient()
            clientCheck.DownloadFile(updatebuildurl, rundir + "\build.txt")
            Dim buildchecker As New System.IO.StreamReader(rundir + "\build.txt")
            updatebuild = buildchecker.ReadToEnd
            ' Close lock handle on file
            buildchecker.Close()
            Call compare()
        Catch ex As Exception
            MsgBox("Unable to contact The iPhone Wiki to download version info!", MsgBoxStyle.OkOnly, "Error!")
        End Try
    End Sub
    Public Sub compare()
        Me.txtInstalled.Text = "Installed build: " + build
        Me.txtAvailable.Text = "Latest build: " + updatebuild
        If build = updatebuild Then
            ' Titles
            Me.Title = "No Update Available"
            Me.TitleChecking.Visibility = Windows.Visibility.Hidden
            Me.TitleNone.Visibility = Windows.Visibility.Visible
            ' Image
            Me.ImageCheck.Visibility = Windows.Visibility.Hidden
            Me.ImageNone.Visibility = Windows.Visibility.Visible
            ' Button
            Me.btnTop.Visibility = Windows.Visibility.Hidden
            Me.btnBottom.Visibility = Windows.Visibility.Visible
            bottombutton = "1"
        Else
            ' Titles
            Me.Title = "Update Available"
            Me.TitleChecking.Visibility = Windows.Visibility.Hidden
            Me.TitleAvailable.Visibility = Windows.Visibility.Visible
            ' Image
            Me.ImageCheck.Visibility = Windows.Visibility.Hidden
            Me.ImageAvailable.Visibility = Windows.Visibility.Visible
            ' Button
            Me.btnTop.Visibility = Windows.Visibility.Hidden
            Me.btnBottom.Visibility = Windows.Visibility.Visible
            bottombutton = "1"
        End If
    End Sub
    Private Sub btnBottom_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnBottom.Click
        If bottombutton = "1" Then
            Me.Close()
        Else
            donothing()
        End If
    End Sub
    Public Sub donothing()
    End Sub
End Class
