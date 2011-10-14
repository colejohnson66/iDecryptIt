Imports System.IO
Imports Microsoft.Win32
Public Class MainWindow
    ' Sleep
    Private Declare Sub Sleep Lib "kernel32" (ByVal dwMilliseconds As Long)
    ' Strings
    Public wantedlang As String
    Public nokey As String = "None Published"
    Public unavailable As String = "Build not available for this device"
    Public result As Integer
    ' What is This?
    Public strArr() As String
    Public count As Integer
    Public device As String
    Public device2 As String
    Public version As String
    Public build As String
    ' File paths
    Public rundir As String = Directory.GetCurrentDirectory
    Public tempdir As String = Path.GetTempPath + "idecryptit\"
    Public helpdir As String = rundir + "help\"

    ' My Stuff
    Public Sub clear()
        Call clearkeys()
        Call cleardmgs()
    End Sub
    Public Sub clearkeys()
        ' 1.x Final
        key1a543a.Text = unavailable
        key1c25.Text = unavailable
        key1c28.Text = unavailable
        key3a100a.Text = unavailable
        key3a101a.Text = unavailable
        key3a109a.Text = unavailable
        key3a110a.Text = unavailable
        key3b48b.Text = unavailable
        key4a93.Text = unavailable
        key4a102.Text = unavailable
        key4b1.Text = unavailable
        ' 1.x Beta
        key5a147p.Text = unavailable
        ' 2.x Final
        key5a345final.Text = unavailable
        key5a347.Text = unavailable
        key5b108.Text = unavailable
        key5c1.Text = unavailable
        key5f136.Text = unavailable
        key5f137.Text = unavailable
        key5f138.Text = unavailable
        key5g77.Text = unavailable
        key5g77a.Text = unavailable
        key5h11.Text = unavailable
        key5h11a.Text = unavailable
        ' 2.x Beta
        key5a225c.Text = unavailable
        key5a240d.Text = unavailable
        key5a258f.Text = unavailable
        key5a274d.Text = unavailable
        key5a292g.Text = unavailable
        key5a308.Text = unavailable
        key5a331.Text = unavailable
        key5a345beta.Text = unavailable
        key5f90.Text = unavailable
        key5g27.Text = unavailable
        ' 3.x Final
        key7a341.Text = unavailable
        key7a400.Text = unavailable
        key7c144.Text = unavailable
        key7c145.Text = unavailable
        key7c146.Text = unavailable
        key7d11.Text = unavailable
        key7e18.Text = unavailable
        key7b367.Text = unavailable
        key7b405.Text = unavailable
        key7b500.Text = unavailable
        ' 3.x Beta
        key7a238j.Text = unavailable
        key7a259g.Text = unavailable
        key7a280f.Text = unavailable
        key7a300g.Text = unavailable
        key7a312g.Text = unavailable
        key7c97d.Text = unavailable
        key7c106c.Text = unavailable
        key7c116a.Text = unavailable
        ' 4.x Final
        key8a293final.Text = unavailable
        key8a306.Text = unavailable
        key8a400.Text = unavailable
        key8b117.Text = unavailable
        key8b118.Text = unavailable
        key8c148final.Text = unavailable
        key8c148a.Text = unavailable
        key8e128.Text = unavailable
        key8e200.Text = unavailable
        key8e303.Text = unavailable
        key8e401.Text = unavailable
        key8e501.Text = unavailable
        key8e600.Text = unavailable
        key8f190final.Text = unavailable
        key8f191.Text = unavailable
        key8g4.Text = unavailable
        key8h7.Text = unavailable
        key8h8.Text = unavailable
        key8j2.Text = unavailable
        key8j3.Text = unavailable
        key8k2.Text = unavailable
        key8l1.Text = unavailable
        ' 4.x Final ATV
        key8m89.Text = unavailable
        key8c150.Text = unavailable
        key8c154.Text = unavailable
        key8f191m.Text = unavailable
        key8f202.Text = unavailable
        key8f305.Text = unavailable
        key8f455.Text = unavailable
        key9a334v.Text = unavailable
        ' 4.x Beta
        key8a230m.Text = unavailable
        key8a248c.Text = unavailable
        key8a260b.Text = unavailable
        key8a274b.Text = unavailable
        key8a293beta.Text = unavailable
        key8b5080.Text = unavailable
        key8b5080c.Text = unavailable
        key8b5091b.Text = unavailable
        key8c5091e.Text = unavailable
        key8c5101c.Text = unavailable
        key8c5115c.Text = unavailable
        key8c134.Text = unavailable
        key8c134b.Text = unavailable
        key8c148beta.Text = unavailable
        key8f5148b.Text = unavailable
        key8f5148c.Text = unavailable
        key8f5153d.Text = unavailable
        key8f5166b.Text = unavailable
        key8f190beta.Text = unavailable
        ' 5.x Final
        key9a334final.Text = unavailable
        ' 5.x Beta
        key9a5220p.Text = unavailable
        key9a5248d.Text = unavailable
        key9a5259f.Text = unavailable
        key9a5274d.Text = unavailable
        key9a5288d.Text = unavailable
        key9a5302b.Text = unavailable
        key9a5313e.Text = unavailable
        key9a334beta.Text = unavailable
    End Sub
    Public Sub cleardmgs()
        ' 1.x Final
        dmg1a543a.Text = "XXX-XXXX-XXX.dmg"
        dmg1c25.Text = "XXX-XXXX-XXX.dmg"
        dmg1c28.Text = "XXX-XXXX-XXX.dmg"
        dmg3a100a.Text = "XXX-XXXX-XXX.dmg"
        dmg3a101a.Text = "XXX-XXXX-XXX.dmg"
        dmg3a109a.Text = "XXX-XXXX-XXX.dmg"
        dmg3a110a.Text = "XXX-XXXX-XXX.dmg"
        dmg3b48b.Text = "XXX-XXXX-XXX.dmg"
        dmg4a93.Text = "XXX-XXXX-XXX.dmg"
        dmg4a102.Text = "XXX-XXXX-XXX.dmg"
        dmg4b1.Text = "XXX-XXXX-XXX.dmg"
        ' 1.x Beta
        dmg5a147p.Text = "XXX-XXXX-XXX.dmg"
        ' 2.x Final
        dmg5a345final.Text = "XXX-XXXX-XXX.dmg"
        dmg5a347.Text = "XXX-XXXX-XXX.dmg"
        dmg5b108.Text = "XXX-XXXX-XXX.dmg"
        dmg5c1.Text = "XXX-XXXX-XXX.dmg"
        dmg5f136.Text = "XXX-XXXX-XXX.dmg"
        dmg5f137.Text = "XXX-XXXX-XXX.dmg"
        dmg5f138.Text = "XXX-XXXX-XXX.dmg"
        dmg5g77.Text = "XXX-XXXX-XXX.dmg"
        dmg5g77a.Text = "XXX-XXXX-XXX.dmg"
        dmg5h11.Text = "XXX-XXXX-XXX.dmg"
        dmg5h11a.Text = "XXX-XXXX-XXX.dmg"
        ' 2.x Beta
        dmg5a225c.Text = "XXX-XXXX-XXX.dmg"
        dmg5a240d.Text = "XXX-XXXX-XXX.dmg"
        dmg5a258f.Text = "XXX-XXXX-XXX.dmg"
        dmg5a274d.Text = "XXX-XXXX-XXX.dmg"
        dmg5a292g.Text = "XXX-XXXX-XXX.dmg"
        dmg5a308.Text = "XXX-XXXX-XXX.dmg"
        dmg5a331.Text = "XXX-XXXX-XXX.dmg"
        dmg5a345beta.Text = "XXX-XXXX-XXX.dmg"
        dmg5f90.Text = "XXX-XXXX-XXX.dmg"
        dmg5g27.Text = "XXX-XXXX-XXX.dmg"
        ' 3.x Final
        dmg7a341.Text = "XXX-XXXX-XXX.dmg"
        dmg7a400.Text = "XXX-XXXX-XXX.dmg"
        dmg7c144.Text = "XXX-XXXX-XXX.dmg"
        dmg7c145.Text = "XXX-XXXX-XXX.dmg"
        dmg7c146.Text = "XXX-XXXX-XXX.dmg"
        dmg7d11.Text = "XXX-XXXX-XXX.dmg"
        dmg7e18.Text = "XXX-XXXX-XXX.dmg"
        dmg7b367.Text = "XXX-XXXX-XXX.dmg"
        dmg7b405.Text = "XXX-XXXX-XXX.dmg"
        dmg7b500.Text = "XXX-XXXX-XXX.dmg"
        ' 3.x Beta
        dmg7a238j.Text = "XXX-XXXX-XXX.dmg"
        dmg7a259g.Text = "XXX-XXXX-XXX.dmg"
        dmg7a280f.Text = "XXX-XXXX-XXX.dmg"
        dmg7a300g.Text = "XXX-XXXX-XXX.dmg"
        dmg7a312g.Text = "XXX-XXXX-XXX.dmg"
        dmg7c97d.Text = "XXX-XXXX-XXX.dmg"
        dmg7c106c.Text = "XXX-XXXX-XXX.dmg"
        dmg7c116a.Text = "XXX-XXXX-XXX.dmg"
        ' 4.x Final
        dmg8a293final.Text = "XXX-XXXX-XXX.dmg"
        dmg8a306.Text = "XXX-XXXX-XXX.dmg"
        dmg8a400.Text = "XXX-XXXX-XXX.dmg"
        dmg8b117.Text = "XXX-XXXX-XXX.dmg"
        dmg8b118.Text = "XXX-XXXX-XXX.dmg"
        dmg8c148final.Text = "XXX-XXXX-XXX.dmg"
        dmg8c148a.Text = "XXX-XXXX-XXX.dmg"
        dmg8e128.Text = "XXX-XXXX-XXX.dmg"
        dmg8e200.Text = "XXX-XXXX-XXX.dmg"
        dmg8e303.Text = "XXX-XXXX-XXX.dmg"
        dmg8e401.Text = "XXX-XXXX-XXX.dmg"
        dmg8e501.Text = "XXX-XXXX-XXX.dmg"
        dmg8e600.Text = "XXX-XXXX-XXX.dmg"
        dmg8f190final.Text = "XXX-XXXX-XXX.dmg"
        dmg8f191.Text = "XXX-XXXX-XXX.dmg"
        dmg8g4.Text = "XXX-XXXX-XXX.dmg"
        dmg8h7.Text = "XXX-XXXX-XXX.dmg"
        dmg8h8.Text = "XXX-XXXX-XXX.dmg"
        dmg8j2.Text = "XXX-XXXX-XXX.dmg"
        dmg8j3.Text = "XXX-XXXX-XXX.dmg"
        dmg8k2.Text = "XXX-XXXX-XXX.dmg"
        dmg8l1.Text = "XXX-XXXX-XXX.dmg"
        ' 4.x Final ATV
        dmg8m89.Text = "XXX-XXXX-XXX.dmg"
        dmg8c150.Text = "XXX-XXXX-XXX.dmg"
        dmg8c154.Text = "XXX-XXXX-XXX.dmg"
        dmg8f191m.Text = "XXX-XXXX-XXX.dmg"
        dmg8f202.Text = "XXX-XXXX-XXX.dmg"
        dmg8f305.Text = "XXX-XXXX-XXX.dmg"
        dmg8f455.Text = "XXX-XXXX-XXX.dmg"
        dmg9a334v.Text = "XXX-XXXX-XXX.dmg"
        ' 4.x Beta
        dmg8a230m.Text = "XXX-XXXX-XXX.dmg"
        dmg8a248c.Text = "XXX-XXXX-XXX.dmg"
        dmg8a260b.Text = "XXX-XXXX-XXX.dmg"
        dmg8a274b.Text = "XXX-XXXX-XXX.dmg"
        dmg8a293beta.Text = "XXX-XXXX-XXX.dmg"
        dmg8b5080.Text = "XXX-XXXX-XXX.dmg"
        dmg8b5080c.Text = "XXX-XXXX-XXX.dmg"
        dmg8b5091b.Text = "XXX-XXXX-XXX.dmg"
        dmg8c5091e.Text = "XXX-XXXX-XXX.dmg"
        dmg8c5101c.Text = "XXX-XXXX-XXX.dmg"
        dmg8c5115c.Text = "XXX-XXXX-XXX.dmg"
        dmg8c134.Text = "XXX-XXXX-XXX.dmg"
        dmg8c134b.Text = "XXX-XXXX-XXX.dmg"
        dmg8c148beta.Text = "XXX-XXXX-XXX.dmg"
        dmg8f5148b.Text = "XXX-XXXX-XXX.dmg"
        dmg8f5148c.Text = "XXX-XXXX-XXX.dmg"
        dmg8f5153d.Text = "XXX-XXXX-XXX.dmg"
        dmg8f5166b.Text = "XXX-XXXX-XXX.dmg"
        dmg8f190beta.Text = "XXX-XXXX-XXX.dmg"
        ' 5.x Final
        dmg9a334final.Text = "XXX-XXXX-XXX.dmg"
        ' 5.x Beta
        dmg9a5220p.Text = "XXX-XXXX-XXX.dmg"
        dmg9a5248d.Text = "XXX-XXXX-XXX.dmg"
        dmg9a5259f.Text = "XXX-XXXX-XXX.dmg"
        dmg9a5274d.Text = "XXX-XXXX-XXX.dmg"
        dmg9a5288d.Text = "XXX-XXXX-XXX.dmg"
        dmg9a5302b.Text = "XXX-XXXX-XXX.dmg"
        dmg9a5313e.Text = "XXX-XXXX-XXX.dmg"
        dmg9a334beta.Text = "XXX-XXXX-XXX.dmg"
    End Sub
    Public Sub cleanup()
        If (Directory.Exists(tempdir + "idecryptit")) Then
            Directory.Delete(tempdir, True)
        End If
    End Sub
    Private Sub DoCMD(ByVal file As String, Optional ByVal arg As String = "")
        ' Taken from fallensn0w's iDecrypter and converted to WPF (hope you don't mind) :)
        Dim procNlite As New Process
        procNlite.StartInfo.FileName = file
        procNlite.StartInfo.Arguments = arg
        procNlite.StartInfo.WindowStyle = 1
        procNlite.Start()
        procNlite.WaitForExit()
    End Sub
    Private Function replacedmg(ByVal filename As String) As String
        Return Replace(filename, ".dmg", "_decrypted.dmg")
    End Function

    ' Click and Stuff
    Private Sub btnChangeLanguage_Click() Handles btnChangeLanguage.Click
        Dim selectlang As Window = New SelectLangControl
        selectlang.Show()
    End Sub
    Private Sub btnDecrypt_Click() Handles btnDecrypt.Click
        If (Me.textInputFileName.Text = "") Then
            MsgBox("Make sure there is an input file!", MsgBoxStyle.OkOnly, "Something went wrong!")
        Else
            If (Me.textDecryptKey.Text = "") Then
                MsgBox("Make sure these is a key inputed!", MsgBoxStyle.OkOnly, "Something went wrong!")
            Else
                If (Me.textOuputFileName.Text = "") Then
                    MsgBox("Make sure these is an output file!", MsgBoxStyle.OkOnly, "Something went wrong!")
                Else
                    DoCMD(rundir + "\vfdecrypt.exe", _
                        " -i " & Chr(34) & Me.textInputFileName.Text & Chr(34) & _
                        " -k " & Me.textDecryptKey.Text & " " & _
                        " -o " & Chr(34) & Me.textOuputFileName.Text & Chr(34))
                    MsgBox("Done!", MsgBoxStyle.OkOnly, "Done Decrypting")
                End If
            End If
        End If
    End Sub
    Private Sub btnAbout_Click() Handles btnAbout.Click
        Me.webBrowser.Navigate(New Uri(helpdir + "about_iDecryptIt.html"))
    End Sub
    Private Sub btnChangelog_Click() Handles btnChangelog.Click
        Me.webBrowser.Navigate(New Uri(helpdir + "changelog.html"))
    End Sub
    Private Sub btnREADME_Click() Handles btnREADME.Click
        Me.webBrowser.Navigate(New Uri(helpdir + "README.html"))
    End Sub
    Private Sub btnHelpOut_Click() Handles btnHelpOut.Click
        Dim submitkey As Window = New SubmitKey
        submitkey.Show()
    End Sub
    Private Sub btnSelectVFDecryptInutFile_Click() Handles btnSelectVFDecryptInutFile.Click
        Dim decrypt As New OpenFileDialog()
        decrypt.FileName = ""
        decrypt.DefaultExt = ".dmg"
        decrypt.Filter = "Apple Disk Images|*.dmg"
        Dim result? As Boolean = decrypt.ShowDialog()
        If result = True Then
            Me.textOuputFileName.Text = replacedmg(decrypt.FileName)
        End If
    End Sub
    Private Sub btnSelectExtractFile_Click() Handles btnSelectExtractFile.Click
        Dim extractofd As New OpenFileDialog()
        extractofd.FileName = ""
        extractofd.DefaultExt = ".dmg"
        extractofd.Filter = "Apple Disk Images|*.dmg"
        Dim result? As Boolean = extractofd.ShowDialog()
        If result = True Then
            Me.textExtractFileName.Text = extractofd.FileName
        End If
    End Sub
    Private Sub btnSelectWhatAmIFile_Click() Handles btnSelectWhatAmIFile.Click
        Dim whatisthis As New OpenFileDialog()
        whatisthis.FileName = ""
        whatisthis.DefaultExt = ".dmg"
        whatisthis.Filter = "iDevice Restore Images|*.ipsw"
        Dim result? As Boolean = whatisthis.ShowDialog()
        If result = True Then
            textWhatAmIFileName.Text = whatisthis.SafeFileName
        End If
    End Sub
    Private Sub btnWhatAmI_Click() Handles btnWhatAmI.Click
        ' This is a complex tree of if() statements and one switch() (select case...end select)
        ' and therefore to make sure that one and only one MsgBox() is displayed
        If (Me.textWhatAmIFileName.Text = "") Then
            MsgBox("ERROR! Make sure you select a file!", MsgBoxStyle.OkOnly, "ERROR!")
            Exit Sub
        Else
            strArr = textWhatAmIFileName.Text.Split("_")
            If (strArr.Length = 4) Then
                If (strArr(3) = "Restore.ipsw") Then
                    device = strArr(0)
                    version = strArr(1)
                    build = strArr(2)
                    strArr = device.Split(",")
                    If (strArr.Length = 2) Then
                        Select Case device
                            Case "iPad1,1"
                                device = "iPad 1G Wi-Fi/Wi-Fi+3G"
                            Case "iPad2,1"
                                device = "iPad 2 Wi-Fi"
                            Case "iPad2,2"
                                device = "iPad 2 Wi-Fi+3G GSM"
                            Case "iPad2,3"
                                device = "iPad 2 Wi-Fi+3G CDMA"
                            Case "iPhone1,1"
                                device = "iPhone 2G"
                            Case "iPhone1,2"
                                device = "iPhone 3G"
                            Case "iPhone2,1"
                                device = "iPhone 3GS"
                            Case "iPhone3,1"
                                device = "iPhone 4 GSM"
                            Case "iPhone3,3"
                                device = "iPhone 4 CDMA"
                            Case "iPod1,1"
                                device = "iPod touch 1G"
                            Case "iPod2,1"
                                device = "iPod touch 2G"
                            Case "iPod3,1"
                                device = "iPod touch 3G"
                            Case "iPod4,1"
                                device = "iPod touch 4G"
                            Case "AppleTV2,1"
                                device = "Apple TV 2G"
                            Case Else
                                MsgBox("ERROR! The supplied device: '" + device + "' does not follow the format: {iPad/iPhone/iPod/AppleTV}{#},{#}", MsgBoxStyle.OkOnly, "ERROR!")
                                Exit Sub
                        End Select
                        MsgBox("Device: " + device + Chr(13) + Chr(10) + "Version: " + version + Chr(13) + Chr(10) + "Build: " + build, MsgBoxStyle.OkOnly, "Info")
                        Exit Sub
                    Else
                        MsgBox("ERROR! The supplied device: '" + device + "' does not follow the format: {iPad/iPhone/iPod/AppleTV}{#},{#}", MsgBoxStyle.OkOnly, "ERROR!")
                        Exit Sub
                    End If
                Else
                    MsgBox("ERROR! The IPSW File that was given is not following the format:" + Chr(13) + Chr(10) + "{DEVICE}_{VERSION}_{BUILD}_Restore.ipsw", MsgBoxStyle.OkOnly, "ERROR!")
                    Exit Sub
                End If
            Else
                MsgBox("ERROR! The IPSW File that was given is not following the format:" + Chr(13) + Chr(10) + "{DEVICE}_{VERSION}_{BUILD}_Restore.ipsw", MsgBoxStyle.OkOnly, "ERROR!")
                Exit Sub
            End If
        End If
    End Sub
    Private Sub btniPad11_Click() Handles btniPad11.Click
        ' iPad 1G Wi-Fi/Wi-Fi+GSM
        Call clear()
        ' 3.x Final
        key7b367.Text = "2be8f3a0a02f2d259c9b297cb2d156a85adf79fed4ffe88c546a42c2a47aa55f70cadebd"
        key7b405.Text = "c3d15c6dc3b289db4d90b59199c485486043bb534c14d21993e35f68f2c6c1804a9125a8"
        key7b500.Text = "18ae1e76e7bcf6478321f42888606ca2d998cffab1ee8c7ca6b15d57b1a7254f8a608823"
        ' 4.x Final
        key8c148final.Text = "6380bc27ef713750c21759ce770cb6540a8e31fca4c78820fd7be3a02030365a59257582"
        key8f190final.Text = "890650c3aa3be7c4d6f3473776580acf6781688e6342ed15441a299142fe4c5e933fc89d"
        key8g4.Text = "c309657d0abe1b66b4be046bb4b03fb540741f9cbc1e49951cf21e11332d9b0b66afd31e"
        key8h7.Text = "25c0b2a27afd23b9ddc9555a28ba8e77548e62d9e2ef56700bc40d22b2c50416aee9313c"
        key8j3.Text = "765d0fecc4f714ca20fa6eceeabb454b04cd2998cc3ab3bba290866788a8c6cf555945ac"
        key8k2.Text = "aa3f737295c1d7a1e0539b8b1a02310b9ec7503be6ed05b88520e50a1a006f4b270b3e9f"
        key8l1.Text = "e002a32650a28f4ecd0793d2e36d8bc93bf4a60bb010dbe9ef2ed41821fc5463b24c791b"
        ' 4.x Beta
        key8c5091e.Text = "c26445bf3f81c6a6d0e0eeed7acbf3c4d801c9e9116504076c8cf959902233a6bf674d46"
        key8c5101c.Text = "d9874f9b69377d81523366d33aaf7cc4880bd928ba9ed536ee8a299847a8081169f37fb3"
        key8c5115c.Text = "a764534161493bdcc4545ee0640d1525edc17f8cc03b4bb1dedb98b765865d8ec12908f2"
        key8c134.Text = "d70754df24b2be5231a7b98089ef30c8699e96445fdf2b23e9b1ace0b6e1e1ed2258e957"
        key8c134b.Text = "8df8ebc5a47e60baa66dc66f8b4cdbcbe2f8591c67a287c122a66d45f0152d49838b1392"
        key8c148beta.Text = "6380bc27ef713750c21759ce770cb6540a8e31fca4c78820fd7be3a02030365a59257582"
        key8f5148b.Text = "7620a160832d8ed43aee376179d28eccf51d50ac38caccd5990db6f10849aa39e3fdc942"
        key8f5153d.Text = "3c9787acfa79fea382ec4f4d00c0f8c59f241df42cf36ab647be085476173e5625cc687a"
        key8f5166b.Text = "55f5f54a3e2e1c84b3a90a50cc2c5e9c2754f2b8bfc8abcf3d3778f8fb2ba34cfca6ea96"
        key8f190beta.Text = "890650c3aa3be7c4d6f3473776580acf6781688e6342ed15441a299142fe4c5e933fc89d"
        ' 5.x Final
        key9a334final.Text = "c7e01f3db404f325eee5062368fc6a795487d859518ee498b4d7f4950a281c5421ffbebf"
        ' 5.x Beta
        key9a5220p.Text = "7fc183f7b7fe6f1d27783e2608b7f4df74acc9d9416382d419484c66ed16b18fe2d6a3b0"
        key9a5248d.Text = "e7da8e8f233a929736e1d68a6e738c27cb44a2188cc0f06e52dbcf875446e87bbdc332a3"
        key9a5259f.Text = "7c160fc06ccd135f426e5787232cdbb77eaa73ad06939c21d67d0c16d3b0db75fedc0f49"
        key9a5274d.Text = "f2ad291291658b540675c6010fd8efd85777812414364e7fc2a91280f461ef6e10ee1ae4"
        key9a5288d.Text = "ce04ccd3ef4d97d44c3356bb23f95b49f2240ffd0d939b38e93ad63bad4e5e4a4fe484a2"
        key9a5302b.Text = "c96b7e16e1a403a7b88664fbdf46761b9a0610c1f4bfc08fe8fd6a2c6dea9b5c682fb8fd"
        key9a5313e.Text = "5cc99c325299804bd947950ba37322987ef0b769c338956815e45caed9be7e8b193da645"
        key9a334beta.Text = "c7e01f3db404f325eee5062368fc6a795487d859518ee498b4d7f4950a281c5421ffbebf"
    End Sub
    Private Sub btniPad21_Click() Handles btniPad21.Click
        ' iPad 2 Wi-Fi
        Call clear()
        ' 4.x Final
        key8f191.Text = nokey
        key8g4.Text = "6f7502e91f3239f907b6bf8955f191b276ec57c392d2beffb3fbc5392da0bc86e65d684e"
        key8h7.Text = "51e154b3f8baadceb317ad6e815b7f75bc956c1fa1f213d7a344e5a6eda4a54b7e79bb50"
        key8j2.Text = "7ac7018b57235d34fcbe0c84713ea7c6c482322559336845d299508f6a8643c2078de051"
        key8k2.Text = "dd467a5139d280e60b4ec9bfa534eae9e1d782ee74fcecd86f409e9fe799fb945ee76158"
        key8l1.Text = "07a0b5ab0e40ba4f38960274dd8c1db20854159d58761ce98dfa4c50a38b9e786b263607"
        ' 5.x Final
        key9a334final.Text = nokey
        ' 5.x Beta
        key9a5220p.Text = nokey
        key9a5248d.Text = nokey
        key9a5259f.Text = nokey
        key9a5274d.Text = nokey
        key9a5288d.Text = nokey
        key9a5302b.Text = nokey
        key9a5313e.Text = nokey
        key9a334beta.Text = nokey
    End Sub
    Private Sub btniPad22_Click() Handles btniPad22.Click
        ' iPad 2 Wi-Fi+GSM
        Call clear()
        ' 4.x Final
        key8f191.Text = nokey
        key8g4.Text = "9bf08c4054e08cff7ff96f3b0f0cb6e809aa8676653b16443445ac990906bb5439f9504d"
        key8h7.Text = "30584c8087f5b7cbc64a9fd0533cc25c69e4844b0b465092b7e30f0074356ce889914481"
        key8j2.Text = "990d84816fa06083f4fc778f3e4a03b2bc4e302d8b9998c2ac87d6c0e43cfabc1b0615d4"
        key8k2.Text = "3907dd20133e8a0bde930d9f3307d3bdf950762c25f8ae7b4f6c8f106949272ccfbf13b0"
        key8l1.Text = "33774947a7d630a80045e6f3f68005646d84efeedbca70d619a429e10e34696d254812ce"
        ' 5.x Final
        key9a334final.Text = nokey
        ' 5.x Beta
        key9a5220p.Text = nokey
        key9a5248d.Text = nokey
        key9a5259f.Text = nokey
        key9a5274d.Text = nokey
        key9a5288d.Text = nokey
        key9a5302b.Text = nokey
        key9a5313e.Text = nokey
        key9a334beta.Text = nokey
    End Sub
    Private Sub btniPad23_Click() Handles btniPad23.Click
        ' iPad 2 Wi-Fi+CDMA
        Call clear()
        ' 4.x Final
        key8f191.Text = nokey
        key8g4.Text = "1c7414fb1820c1c0a61058587661b1c5fbb68fbeafb77f86014671ee5ddac8360d8cc352"
        key8h8.Text = "1c7414fb1820c1c0a61058587661b1c5fbb68fbeafb77f86014671ee5ddac8360d8cc352"
        key8j2.Text = "18516a9744529fcf5f01cc12b86fe5db614db6d688d826f20d501b343199f2de921a6310"
        key8k2.Text = "b314630e05038f97f2d5325b11989634049c5d5d290cc87b9ea7cfd02936b92e76e8f65f"
        key8l1.Text = "369474d8df6b2c874a3fb5aa63cf23f7a891363863cf829f7e85ee631318f2674fed6733"
        ' 5.x Final
        key9a334final.Text = nokey
        ' 5.x Beta
        key9a5220p.Text = nokey
        key9a5248d.Text = nokey
        key9a5259f.Text = nokey
        key9a5274d.Text = nokey
        key9a5288d.Text = nokey
        key9a5302b.Text = nokey
        key9a5313e.Text = nokey
        key9a334beta.Text = nokey
    End Sub
    Private Sub btniPhone11_Click() Handles btniPhone11.Click
        ' iPhone 1G GSM
        Call clear()
        ' 1.x Final
        key1a543a.Text = "28c909fc6d322fa18940f03279d70880e59a4507998347c70d5b8ca7ef090ecccc15e82d"
        key1c25.Text = "7d5962d0b582ec2557c2cade50de90f4353a1c1de07b74212513fef9cc71fb890574bfe5"
        key1c28.Text = "7d5962d0b582ec2557c2cade50de90f4353a1c1de07b74212513fef9cc71fb890574bfe5"
        key3a109a.Text = "f45de7637a62b200950e550f4144696d7ff3dc5f0b19c8efdf194c88f3bc2fa808fea3b3"
        key3b48b.Text = "70e11d7209602ada5b15fbecc1709ad4910d0ad010bb9a9125b78f9f50e25f3e05c595e2"
        key4a93.Text = "11070c11d93b9be5069b643204451ed95aad37df7b332d10e48fd3d23c62fca517055816"
        key4a102.Text = "d0a0c0977bd4b6350b256d6650ec9eca419b6f961f593e74b7e5b93e010b698ca6cca1fe"
        ' 1.x Beta
        key5a147p.Text = "86bec353ddfbe3fb750e9d7905801f79791e69acf65d16930d288e697644c76f16c4f16d"
        ' 2.x Final
        key5a347.Text = "2cfca55aabb22fde7746e6a034f738b7795458be9902726002a8341995558990f41e3755"
        key5b108.Text = "2cfca55aabb22fde7746e6a034f738b7795458be9902726002a8341995558990f41e3755"
        key5c1.Text = "31e3ff09ff046d5237187346ee893015354d2135e3f0f39480be63dd2a18444961c2da5d"
        key5f136.Text = "562ca0f7963eafb462da74a9c1f01a45c30a7eb5f1f493feceecae03ee6521a334f4ff68"
        key5g77.Text = "dc39d88afe4cbd8a3f36824b8fd68acf04ac72718c09100816c5cb89889b8079e96802f0"
        key5h11.Text = "ee4eeeb62240c1378c739696dff9fef2c88834e98877f55a29c147e7d5b137967197392a"
        ' 2.x Beta
        key5a225c.Text = "ea14f3ec624c7fdbd52e108aa92d13b16f6b0b940c841f7bbc7792099dae45da928d13e7"
        key5a240d.Text = "e24bfab40a2e5d3dc25e089291846e5615b640897ae8b424946c587bcf53b201a1041d36"
        key5a258f.Text = "198d6602ba2ad2d427adf7058045fff5f20d05846622c186cca3d423ad03b5bc3f43c61c"
        key5a274d.Text = "589df25eaa4ff0a5e29e1425fb99bf50957888ff098ba2fcb72cf130f40e15e00bcf2fc7"
        key5a292g.Text = "890b1fbf22975e0d4be2ea3f9bc5c87f38fd8158394fd31cf80a43ad25547573bbd0ec4e"
        key5a308.Text = "3964ca8d8bf5d3715cdc172986f2d9606672c54d5e0aa3f3a892166b4e54e4cefef21279"
        key5a331.Text = "33d9a9832a108fc5084fc9329d6e84e38edf06e380554c49376b70e951f8a8d1ed943f819"
        key5a345beta.Text = nokey
        key5f90.Text = "f61c14aa0d53386dd42c49163686e8ccdeb86d14aafdecfe99c2e12c41a7f9f2811daa3d"
        key5g27.Text = nokey
        ' 3.x Final
        key7a341.Text = "25cce378de209d8fb6ec45ecbe7525695272b81fe38bbad76e979ac3921c3614ed162c87"
        key7a400.Text = nokey
        key7c144.Text = "dbe476ed0d8c1ecf7cd514463f2ca5a6f71b6f244d98ebaa9203fd527c1ecbf2bb5f143f"
        key7d11.Text = "fe431a1e436e5298d3c871359768aab43189fd5e7375a2ced3405dd8a223879a4d64a28e"
        key7e18.Text = "3c0f821663316c08a0a059c2979ecf47d13b363de3a44010d0de0b0a5cf878cfe39d00c3"
        ' 3.x Beta
        key7a238j.Text = "56753a471abc4e859f6d0f0157d2fea4dfb5a536154cd26b0e3a35b732bf5fce2eae96f1"
        key7a259g.Text = "a555264d0765ca442d5e8f9b2dec1c67dca018ce87d035ac82357b60a970171de04b4f87"
        key7a280f.Text = nokey
        key7a300g.Text = nokey
        key7a312g.Text = "a33171ef12e7245f5508fb6fc245ff4c8f5483af6ca73f77e68122fd6ea3ad37907c969d"
        key7c97d.Text = "dd832e7ce186077bf0b4c5934c1b38b6d55c01c1f04e1ffde721792b1fe06e68e1125f29"
        key7c106c.Text = "c02953ea2d1c99de2d59da6dddb37f6396ade34fa7ad8e1eb629fce68d51352fd1b42563"
        key7c116a.Text = nokey
    End Sub
    Private Sub btniPhone12_Click() Handles btniPhone12.Click
        ' iPhone 3G GSM
        Call clear()
        ' 2.x Final
        key5a345final.Text = nokey
        key5a347.Text = "2cfca55aabb22fde7746e6a034f738b7795458be9902726002a8341995558990f41e3755"
        key5b108.Text = "2cfca55aabb22fde7746e6a034f738b7795458be9902726002a8341995558990f41e3755"
        key5c1.Text = "31e3ff09ff046d5237187346ee893015354d2135e3f0f39480be63dd2a18444961c2da5d"
        key5f136.Text = nokey
        key5g77.Text = nokey
        key5h11.Text = nokey
        ' 2.x Beta
        key5g27.Text = nokey
        ' 3.x Final
        key7a341.Text = "8d5d1fea02d627c9e9b0d994c3cfdeaab9780c86ac908db15461efe44eddd19f8924b6b2"
        key7a400.Text = "62deb9e26f11ef6d0ce2afc85becdcf65b81486b3430a9930cccaccd6879c405e06d8ac3"
        key7c144.Text = "5bfa05ddbeb19ad7d2af3d7012adba6d82002d9d7e7b3771d11702728d15d8f8d52f3573"
        key7d11.Text = "a8a886d56011d2d98b190d0a498f6fcac719467047639cd601fd53a4a1d93c24e1b2ddc6"
        key7e18.Text = "bf5eb72cd65e9c37cf9920707cb6b4f7ecc10b38cfec6b167002ac9fd6a3ab6643e45005"
        ' 3.x Beta
        key7a238j.Text = "e14a53de98b86018e9054b567c491bea9f55235729b3728315ab0b1ed0c82e568a35c0ba"
        key7a259g.Text = "59a86b5a4fcc76fcade07fddf72c72d36a6e105bc0c727f508f2b1313eb1b74d97ca8a81"
        key7a280f.Text = nokey
        key7a300g.Text = nokey
        key7a312g.Text = "f7b1edb0ee9196a1393dccdc8d090051308b84ab322bf860cb1d3ca566ef2e29752fa79a"
        key7c97d.Text = "f526d42d44dcf61dadf5a0b4be7eb18dafd66c88ec6d91e3ac2f08d3179b63ce64108530"
        key7c106c.Text = "53d70939701dc784e38ab2861d85548937a2a187ad05006556cbc4183962a4aceafcbead"
        key7c116a.Text = nokey
        ' 4.x Final
        key8a293final.Text = "09e054a8dd6c11c7f41ad9e614a8d564aa7d0c653585f29c0b07d1f0a1e1dc0040624a16"
        key8a306.Text = "38a4937108c1c271c82013dff870bab10793292ab594ae7878175cf2bfb6bb9633419ff9"
        key8a400.Text = "aa5ea4b38e5a7d9f2d95ab7c015e5531050af66f82a30e6a83994f8f802d352e236a0250"
        key8b117.Text = "4c3c83d3899ea9bef415b1c9c656aaef966b2362494d2c9093a9283d388257562a228c86"
        key8c148final.Text = "82c9280927224637c77a96a26d22f42f2ca08fa9a798a8d06fbc8202fc83ec7f45dda79e"
        ' 4.x Beta
        key8a230m.Text = "0da2d3316d5ee7cd1858e4035e451387cd8156e97535fb09028859e68e5b7b39a6649552"
        key8a248c.Text = nokey
        key8a260b.Text = "fd5f13cf40acec55cf2c8f59b009c26cd5cc7676be5c305333650632a3898ebea060b259"
        key8a274b.Text = "21d0c050aa528124eb5e0a998fe4a7581e7325ce38c3b3ebaf36cc5b326d8c1859e49c2e"
        key8a293beta.Text = nokey
        key8b5080c.Text = "980269f302a65bf50a9f800b46da74a2e83a498c69244a618827e1ebddbfc334c3da2ea3"
        key8b5091b.Text = nokey
        key8c5091e.Text = nokey
        key8c5101c.Text = nokey
        key8c5115c.Text = nokey
        key8c134.Text = "f0db1eef22f887fd7c232812acc374d8e14cc382e4fea72766d08a96d4c175478bc6470a"
        key8c148beta.Text = "82c9280927224637c77a96a26d22f42f2ca08fa9a798a8d06fbc8202fc83ec7f45dda79e"
    End Sub
    Private Sub btniPhone21_Click() Handles btniPhone21.Click
        ' iPhone 3GS GSM
        Call clear()
        ' 3.x Final
        key7a341.Text = "7d779fed28961506ca9443de210224f211790192b2a2308b8bc0e7d4a2ca61a68e26200e"
        key7a400.Text = nokey
        key7c144.Text = "b9cd10dd88ab615c1963e8aa04950b12dd64e0e5b11ea63c79a02af6db62334c710d21da"
        key7d11.Text = "47d76295817f74953f8e557b4917fe2201e9778a9900e43fbf311a83f176fe521b996a4b"
        key7e18.Text = "9b3fd35bad7d5307d85ce4b38b8e56bd680ef5a72a8f3b615f8d4f16c14bdcf3c3b24c6c"
        ' 3.x Beta
        key7c97d.Text = nokey
        key7c106c.Text = nokey
        key7c116a.Text = nokey
        ' 4.x Final
        key8a293final.Text = "5d79765bc3233cbee58727c17a9487e5dc1e38400c2a98c30997bb02d00e97ae3ce5fab8"
        key8a306.Text = "5d9385452d9ce0fe0185dfc59a7cbb1015d086ce53ff769e78dd45bc6e4eeb48c60e2952"
        key8a400.Text = "812288d52a0845a41c3cd61e6b5a0f85731ce3fc04aa631895d40ca77d8f325ff02c70e9"
        key8b117.Text = "01155a88dc41d6bdb6ba368719853e7e68fb0076dbfaafe8e0801256c724b103f2e271ca"
        key8c148a.Text = "ec413e58ef2149a2c5a2669d93a4e1a9fe4d7d2f580af2b2ee55c399efc3c22250b8d27a"
        key8f190final.Text = "95028f5804a6d675190adedc0aa91385e17589f720c517615367d69c63e0c969121aaec6"
        key8g4.Text = "c338fb2858bd5dad4cfb073d4fab2fbed4a3f2d1541bc50d8443f3b18475cd1b62c25983"
        key8h7.Text = "69a370c1b64b35f692e87e866bcd460a98a10c56ed05055eb7c675f101894ea504f7bc46"
        key8j2.Text = "148f4fca734e973551fc8fa65a04883041854b060e3fe1e6c3ca4499a3204d1d97594a47"
        key8k2.Text = "fb9480e2b80a26cd75d923d7918539edb19caed5a72dfe7a78734cd2a82597869b9ceaf5"
        key8l1.Text = "8b04eb7e4c4c3bea36693fee2369d48c667083ae79ddea8c02f5ce9da30a74cb20707328"
        ' 4.x Beta
        key8a230m.Text = "62ea9bf9971e6c410231646f916f80330f9cbc1d1c585f0c03dab6b6f7158dc0a9c5efaf"
        key8a248c.Text = "4fea9105d8445961cbeef29f06d93685af4b7f45a02eaf7f7cdc8f78784762df3f1072c7"
        key8a260b.Text = "9623f11023fb5260b68c5982caf15591b0432f69160065fac42dcda449a3f284fbddac2a"
        key8a274b.Text = "fb604c6359adc0a52c6fabc0b70a9b11eaee45d36c906e1510b2a6a42e25228283866a5e"
        key8a293beta.Text = "9f5df1142f09cf9cb38e08af4f7f56e3d9e748a86ab7e7e556d8f1ff029e5a9a83b35211"
        key8b5080c.Text = nokey
        key8b5091b.Text = nokey
        key8c5091e.Text = nokey
        key8c5101c.Text = nokey
        key8c5115c.Text = "35ccd2de84a68950ebc166e57807a010e9985fec751869c21516011fe53d4f56fe5f7a5a"
        key8c134.Text = "7920ced8bd8d1fb160536c7e853680fe7fa6827e8f5371a5af5cbc6d2b2d92b23dc2b41b"
        key8c148beta.Text = "97a8b688acf744c09ac4afe130d96b55b8a68c4ec007dfc6a6c8f810a09b91d4d80d8f29"
        key8f5148b.Text = "15130edaeb5897edf079f89e4224f435edf3986b9f72bd3079509ab623adcb0a600200ed"
        key8f5153d.Text = "ac704d0287cb4dc835252e84a9f244bc7da16c0227776271940ae097ae52ca94bd7c6e68"
        key8f5166b.Text = "35180cdade1149fcbc061d6cafea436155b5d75540960d68ebbed56e8d0da862b8a2707d"
        key8f190beta.Text = "95028f5804a6d675190adedc0aa91385e17589f720c517615367d69c63e0c969121aaec6"
        ' 5.x Final
        key9a334final.Text = nokey
        ' 5.x Beta
        key9a5220p.Text = "b0f31d60ec84f1e3430c7f7753055bdd70d394b4fe5bb378af23d5a833584570538bb33b"
        key9a5248d.Text = "11e80b9d23f6d1ba1eea0adf759f6bfec40399edddfe37a94152e357b0c9064b09b95515"
        key9a5259f.Text = "53a43ad56f58bb6f9f226909b6663c0922b266b33e29de8cdb7af3fa5c8e93c70fa2fd4a"
        key9a5274d.Text = "f48c8fa1862b636fcc45aa25b6f2aa755656a02f475d9bd76921395f57406aa87a04dd54"
        key9a5288d.Text = "3a61db2078a658c69f7e2cf8c764bb9de3eb104ffd18905b69ebbfbdb9e0c5826ba57363"
        key9a5302b.Text = "bafe6937aa9a24a108af1fec0e24c76ad28ef4c57be971bd05a8ecd6abc2f31b8e90619f"
        key9a5313e.Text = "72bf0eca5776925b62006f3f83ef02a1d536572fc95b54f426ef0132ef65d97cd13c880e"
        key9a334beta.Text = nokey
    End Sub
    Private Sub btniPhone31_Click() Handles btniPhone31.Click
        ' iPhone 4 GSM
        Call clear()
        ' 4.x Final
        key8a293final.Text = "8b2915719d9f90ba5521faad1eadbb3d942991bd55e5a0709f26e9db3931517e054afa50"
        key8a306.Text = "ebd8aea30e78053112c4062690723fc5ee8e53865d4d6591b64a08216337c5a7aefbc806"
        key8a400.Text = "28bded3ee52eda2f36a241009a493db357b8f19543c07bd3820a35498a1788ce4aa0c54c"
        key8b117.Text = "2ab6aea67470994ec3453791ac75f6497c081edd1991e560a61dd666ac4b73f43c781739"
        key8c148final.Text = "b2ee5018ef7d02e45ef67449d9e2ed5f876efae949de64a9a93dbcf7ff9ed84e041e9167"
        key8f190final.Text = "34904e749a8c5cfabecc6c3340816d85e7fc4de61c968ca93be621a9b9520d6466a1456a"
        key8g4.Text = "f6331068497fa4741e135329c399f69b3c109854835789cc6f23f759f333f5e7bbfcdde7"
        key8h7.Text = "30804cac61ba4df96999aa4e1ea3a2a18bfbe875534a66a0bb1add095e307a19a7176c82"
        key8j2.Text = "246f17ec6660672b3207ece257938704944a83601205736409b61fc3565512559abd0f82"
        key8l1.Text = "e5e061077217c4937e14d9c4ae1eeb8d69827aa4838168033dd5f1806ab485306a8aa3cf"
        ' 4.x Beta
        key8b5080c.Text = nokey
        key8b5091b.Text = nokey
        key8c5091e.Text = "1fb04279ce6d3de5b01d48de766e3bd41db7006437cf9775d4e8685ac560d719e8864f8b"
        key8c5101c.Text = "fcf31267972d0db5e58181d547022c4832be70b309438364358ba78ad3b8764daa8270c4"
        key8c5115c.Text = "ee8c534a2cb539385c787092bd2e43dcb4040392b223d9d01d840b32348144a39f9898ef"
        key8c134.Text = "2deb36dc144666a4b4a2fb21b9412ac144427fcf587c5309bb704cbbbeaaa0ee6406c7a6"
        key8c148beta.Text = "b2ee5018ef7d02e45ef67449d9e2ed5f876efae949de64a9a93dbcf7ff9ed84e041e9167"
        key8f5148b.Text = "d457db4790e7126428ed4fb053f84f25e89f8135129d0ff81b0d6b580cde1bc5d794e5af"
        key8f5153d.Text = "da556a06a2695098f7222557ffb0ecab976995b6ca9032996eed0311fb2841c1fc59f7da"
        key8f5166b.Text = "80c9b3147d4928be874a2f920fb78595403c7ca6f9de6c877ccc07dcdfe9279c44d08e83"
        key8f190beta.Text = "34904e749a8c5cfabecc6c3340816d85e7fc4de61c968ca93be621a9b9520d6466a1456a"
        key8k2.Text = "f3b2e5122cfd8b8215ed8271d83af0183f6d6634afd63444dfd7787e274b7520fc9d5c40"
        ' 5.x Final
        key9a334final.Text = "5e5c52fd7e439936d89659b5aa4f79206cd64f09c9961e9d4712a0131075966e2271b354"
        ' 5.x Beta
        key9a5220p.Text = "ddd6f84e0450d2ea0cfb16d652a6dcc50d9a4e5be2225f9f4e1e22a7dd6cf686a34fb257"
        key9a5248d.Text = "0c5387489bd9a4380691047e1880737df22ae2c7dd689f31669d00481b11249d868d591b"
        key9a5259f.Text = "894575ea5975c4ea50083f1cb2bc3b76b89cc53851452196309368a09e90d51d3f4aa57d"
        key9a5274d.Text = "b2e3d1f334b39e92201d6a6834b3ae624f52f35b3c41c73950b770cb0ac2294673525236"
        key9a5288d.Text = "bbbef345aaa6830c7c2045146357300c6b80f07fd676efc17076025fe0278b7b9a27978b"
        key9a5302b.Text = "1da9aca5ceac97e583df8dd9e84346ac03434bc6bf9557e8a5024193cbcf9b593d33cd4d"
        key9a5313e.Text = "984ce29b96abdc525711b39bd4263c17ae327d77a79564889efcaf50d5201c361cfe30a7"
        key9a334beta.Text = "5e5c52fd7e439936d89659b5aa4f79206cd64f09c9961e9d4712a0131075966e2271b354"
    End Sub
    Private Sub btniPhone33_Click() Handles btniPhone33.Click
        ' iPhone 4 CDMA
        Call clear()
        ' 4.x Final
        key8e128.Text = nokey
        key8e200.Text = "723ded674deb1cba56a142542a0b06d2a483297f8056c0cfa70346c0724e1b0e03feded6"
        key8e303.Text = "612f78042ddc5337ab1abecfb59a07e88ed3e80665a035ef02c3c48045057fc29ab0a4b5"
        key8e401.Text = "d8e162215f27c016ed8d1849c6059f99984c766c72cec4a1df63724491c8e5b19c0e6fb2"
        key8e501.Text = "e5eed79ba8d2341dfaddeebfa38a86d8b95af4a711054a0ab2e058694c13c814fa39a4ba"
        key8e600.Text = "d36aa0ce62b84a9a31a9c33a551809213dcf5f764850c28b3885f00bc1f5664224c13a3b"
        ' 5.x Final
        key9a334final.Text = "cbb21346634c5754f3e956f09ca7c93542b87286d7b11de71f18c5d72da529746ab27094"
        ' 5.x Beta
        key9a5220p.Text = "ffb3bbda6fe1512131d167985e0515de169a7d215b271d518c15d4373bed3ae75af64e5a"
        key9a5248d.Text = "4a63f44750adb005b4252f39afd3299e68be3336f33540d15a43aebc4625f20d33f3afdf"
        key9a5259f.Text = "92d11a5be2dc74af784c8a3d34a79bf3d5e6bffac6f21e4dbd6208e8d8cccd7003f43126"
        key9a5274d.Text = "09ea260fddb12f00402a0e33b8063791a0d4728a188301933052b6285427aca18f8dcfa2"
        key9a5288d.Text = "6ef36fd78dc2e2db2e47062b6291ce9a434f3b1a8a03ba3e9fd74d8e9b674eecced2cb31"
        key9a5302b.Text = "6149a5138478d8eaaff89934260039ce02e21ef0769664ad0cd3861248108b599abc59cc"
        key9a5313e.Text = "0d236147d313acd49c584ea36818aa207ca5461855a21d1c0f8421ec314cb8e45b7b2b2a"
        key9a334beta.Text = "cbb21346634c5754f3e956f09ca7c93542b87286d7b11de71f18c5d72da529746ab27094"
    End Sub
    Private Sub btniPhone41_Click()
        ' iPhone 4S GSM/CDMA
        Call clear()
    End Sub
    Private Sub btniPod11_Click() Handles btniPod11.Click
        ' iPod touch 1G
        Call clear()
        ' 1.x Final
        key3a100a.Text = nokey
        key3a101a.Text = nokey
        key3a110a.Text = "d45b837ddd85bdae0ec82a033ba00ea03ceb8c827040034f7554c65d6376472844b8dc6a"
        key3b48b.Text = "70e11d7209602ada5b15fbecc1709ad4910d0ad010bb9a9125b78f9f50e25f3e05c595e2"
        key4a93.Text = "11070c11d93b9be5069b643204451ed95aad37df7b332d10e48fd3d23c62fca517055816"
        key4a102.Text = "d0a0c0977bd4b6350b256d6650ec9eca419b6f961f593e74b7e5b93e010b698ca6cca1fe"
        key4b1.Text = "c7973558e8f6af22e38d4573737d1533f1d5eec202bf86a32d941975d76f8906c7f0afe4"
        ' 1.x Beta
        key5a147p.Text = "86bec353ddfbe3fb750e9d7905801f79791e69acf65d16930d288e697644c76f16c4f16d"
        ' 2.x Final
        key5a347.Text = "2cfca55aabb22fde7746e6a034f738b7795458be9902726002a8341995558990f41e3755"
        key5b108.Text = "2cfca55aabb22fde7746e6a034f738b7795458be9902726002a8341995558990f41e3755"
        key5c1.Text = "31e3ff09ff046d5237187346ee893015354d2135e3f0f39480be63dd2a18444961c2da5d"
        key5f137.Text = "9714f2cb955afa550d6287a1c7dd7bd0efb3c26cf74b948de7c43cf934913df69fc5a05f"
        key5g77.Text = nokey
        key5h11.Text = nokey
        ' 2.x Beta
        key5a225c.Text = "ea14f3ec624c7fdbd52e108aa92d13b16f6b0b940c841f7bbc7792099dae45da928d13e7"
        key5a240d.Text = "e24bfab40a2e5d3dc25e089291846e5615b640897ae8b424946c587bcf53b201a1041d36"
        key5a258f.Text = "198d6602ba2ad2d427adf7058045fff5f20d05846622c186cca3d423ad03b5bc3f43c61c"
        key5a274d.Text = "589df25eaa4ff0a5e29e1425fb99bf50957888ff098ba2fcb72cf130f40e15e00bcf2fc7"
        key5a292g.Text = "890b1fbf22975e0d4be2ea3f9bc5c87f38fd8158394fd31cf80a43ad25547573bbd0ec4e"
        key5a308.Text = "3964ca8d8bf5d3715cdc172986f2d9606672c54d5e0aa3f3a892166b4e54e4cefef21279"
        key5a331.Text = "3d9a9832a108fc5084fc9329d6e84e38edf06e380554c49376b70e951f8a8d1ed943f819"
        key5a345beta.Text = nokey
        key5f90.Text = "f61c14aa0d53386dd42c49163686e8ccdeb86d14aafdecfe99c2e12c41a7f9f2811daa3d"
        key5g27.Text = nokey
        ' 3.x Final
        key7a341.Text = "16fdad25424dc17008728e89f4900b887732dcc5fb48eedc9f1c9433af558db705eb0577"
        key7c145.Text = "b609c19727813cd4e544919fb1732acb0aebeed814dce672bafe03056a1e8e1b7d4f8f71"
        key7d11.Text = nokey
        key7e18.Text = "467e695041d01e3f58886314bfe70c9b89a7f0c09d6622931f57d1cfa1f7abd9c307563a"
        ' 3.x Beta
        key7a238j.Text = "23f22d7ad042ed8afd3e712987022dc4d34921f69db9050e81148616b99319dc7faafa24"
        key7a259g.Text = "8391ccb34883271fd18c1deed24fd67cab0b3dc56e167c1852c19f49d91aa7ddac1fbc7f"
        key7a280f.Text = nokey
        key7a300g.Text = nokey
        key7a312g.Text = "52830814758d6d3b91e5fb40356c98919091C2a272ec66d9d4fd331eb33d4f60c91c15fc"
        key7c97d.Text = nokey
        key7c106c.Text = nokey
        key7c116a.Text = nokey
    End Sub
    Private Sub btniPod21_Click() Handles btniPod21.Click
        ' iPod touch 2G
        Call clear()
        ' 2.x Final
        key5f138.Text = "d1b957a0a5e56903adc523c5fa99f5d165c3963aea48274770b766b44ecdebab7b5a8f30"
        key5g77a.Text = "148025cde5c51d51d7733e74c6857dfca70d7240287d6eb039a1ed835413120b0af1e296"
        key5h11a.Text = "2611c9f73504344fb22c93791659ec92e65f914025c5814d708b2023ab67229d89c39791"
        ' 3.x Final
        key7a341.Text = "415225778e1bebf8eeff2a9050b04ce429de9680e4acba50820a3fa453897bc4a4b307e2"
        key7c145.Text = "abef664a55de10472c076fa633f47a7c3567239e9af3c73dac4c8683c75f3be27b508eb2"
        key7d11.Text = "fc68c25f1dcc929f37c2be82b94e4c92b48eac3ddd67adefd462404663265e3dca43a930"
        key7e18.Text = "2360d83b606481a5ca080fe7a6fc64f8d5a5556413dfcf3e1277fe564734ee0b188798b8"
        ' 3.x Beta
        key7a238j.Text = "f25c95fc2db995275ebc0f7786f0c5358c109db8a2af1ff3ed93d2ef3e026123f131ff0b"
        key7a259g.Text = nokey
        key7a280f.Text = nokey
        key7a300g.Text = nokey
        key7a312g.Text = nokey
        key7c97d.Text = "2e6803af2724480b1252c3420ec51da1dc8729b8f8082a72fd2fc8d4963e18fa2fd825dd"
        key7c106c.Text = nokey
        key7c116a.Text = nokey
        ' 4.x Final
        key8a293final.Text = "fcada08311f553b2d7194c97922e01c821b632bf62e64500056ea37e56343e6131a9839b"
        key8a400.Text = "5d1655d3cd7c6ffb4a5e48a52ea8a265579c655aa39ed8613239e57f20f132e4e3b5ffa1"
        key8b117.Text = "aec4d2f3c6a4e70ea6cea074f65812d2a34b180cc92b817edcd867167e7a91c5beb942f0"
        key8c148final.Text = "519ec112b4af0a65eab6ea65b222c5b7f605ce52ad9195640e3309de58dd54ab0a0c9607"
        ' 4.x Beta
        key8a230m.Text = "d88fa434f6a8c50fd49cd0923879da5c3079c59c0e534cff521a9cecbcad48d84b4daef0"
        key8a248c.Text = "80ad427daeaa34ce51d6f85510c9b69e97ac81961a0b0a7f0c1d4bc61a8d7d02d46d8a7e"
        key8a260b.Text = "f23cd8de458bf2234a84f3f0069c96cc901a30d2bed4f53e479d4033ba75026d6848f286"
        key8a274b.Text = "a11b9a603bd1bfe5f4aa2a6a708c0038b94c51b91b04ece49b842266f4afada60ff2c995"
        key8a293beta.Text = "8811ded8264fff60660128f0c29f3d7e25f1293bc6290f62c2ce28480df778b2ff1426e9"
        key8b5080.Text = "5aa1334dd6c2315554711585210a0afeb3acfab36185a13e58046e529b9aea2b75a42cec"
        key8b5091b.Text = nokey
        key8c5091e.Text = nokey
        key8c5101c.Text = "88c28b50065c1131f8898c97643b885e05c1c5cad83fbf6efc1c44baca5fbf44ea3ec155"
        key8c5115c.Text = "da0243fa2e3e42356c2453b300c4579d500023a9b939d4f19e336228663628cfa9c2e8b3"
        key8c134.Text = "6099be47ee6b867eb85aca33eab34d0713cdc7a6a017c91b537f3dfd8c8370e03b7e7a1c"
        key8c148beta.Text = "519ec112b4af0a65eab6ea65b222c5b7f605ce52ad9195640e3309de58dd54ab0a0c9607"
    End Sub
    Private Sub btniPod31_Click() Handles btniPod31.Click
        ' iPod touch 3G
        Call clear()
        ' 3.x Final
        key7c145.Text = "de14c16e21ad5bb12fe572ca9400d29a4443ff208ec49c120ad72d6c3269fd5553047cdd"
        key7c146.Text = nokey
        key7d11.Text = "1e05ef21821280869d4029a2328836b9f60bc63907c6e951c0f1c80c2d8c66aef5c39a44"
        key7e18.Text = "1402974cba4702e831fb821ef9090229f7bad6fd3084fa99bfc8a76de4d839f9bf4533eb"
        ' 4.x Final
        key8a293final.Text = "ec6eb0268c4e9f8ab9d003f601e8f4b36f4fc4311c61e5ebed07ce718424ffee7e7d924d"
        key8a400.Text = "4e164b7c39c8e0234787f7b9ae204adf1e3a66d472f1dce1db41e42ba87d1ff5722a7689"
        key8b117.Text = "69e2ca7a250765c95a703081d1195e681fbe82f31162b79fd2b70754629b0352694b9eda"
        key8c148final.Text = "abd68f16920490865a09e559123db1f471ff19743ad15ea8b970a73e28f5efc6c6e76925"
        key8f190final.Text = "cca43b420c4ffefb23a9b5e1605db40df1d89cb13d5951e22b7dda5a35a5cb2dcde85e4a"
        key8g4.Text = nokey
        key8h7.Text = "7085a2976bd57eceedcbbe88a270e1a5028133c288b8afb122c2f886830a9a641daf8bd4"
        key8j2.Text = "affbe8f884694f4a3848097fa22a71bc1de24b070aa7e79f58a0880602dd21444cd559f2"
        key8k2.Text = nokey
        key8l1.Text = "527d77b552fa1fa3708f5c3c2feff8641c7716a24df4dbb49613d0776a7afa3ab9cf95dd"
        ' 4.x Beta
        key8a230m.Text = "382dee11b9d80387b16ac2030ee1e903b78d9743a31a18bcafc922b7921eca85ab0aebf7"
        key8a248c.Text = nokey
        key8a260b.Text = nokey
        key8a274b.Text = "e37aebb23cef6c2d4b43924722a03a44f8680591a1d29112fddf16da36a275996cdf8782"
        key8a293beta.Text = "ddd5c2872ca8e460acf1159940f198df43089176cd8d10adbef8fe4b79f4f4a030fe3e86"
        key8b5080.Text = nokey
        key8b5091b.Text = nokey
        key8c5091e.Text = nokey
        key8c5101c.Text = nokey
        key8c5115c.Text = "c21dd42a12cad2f5430c1ece6f7548e8b77ce1eca344b2bb24392ac45c303f42cd76d5de"
        key8c134.Text = "a9751de0d5c47c7c67244966714e9432cc08e3d81c3a1d94483d1df5d0712ce3e7562069"
        key8c148beta.Text = "abd68f16920490865a09e559123db1f471ff19743ad15ea8b970a73e28f5efc6c6e76925"
        key8f5148b.Text = nokey
        key8f5153d.Text = nokey
        key8f5166b.Text = nokey
        key8f190beta.Text = "cca43b420c4ffefb23a9b5e1605db40df1d89cb13d5951e22b7dda5a35a5cb2dcde85e4a"
        ' 5.x Final
        key9a334final.Text = nokey
        ' 5.x Beta
        key9a5220p.Text = "a450072c20f4a07afec9d4c938a3dc08141aa86aa9073db5882313b09fd3098a90e0480e"
        key9a5248d.Text = "5cc718f5615d8a0caaef430d8589d3542b6b19aef95cc33de39e8c74b869f27781d492c7"
        key9a5259f.Text = "7e428a3eb0539eb0b24c0f9dc52b0ff1ca2381f5bc7a4981377541b8a92c4b6581303cd2"
        key9a5274d.Text = nokey
        key9a5288d.Text = "7e0fd860c3fd6daec23d840cbb0463d1027f5b356e55bea7bf7b3bc0e7f53271b1a4a5ad"
        key9a5302b.Text = "d00f6ad8af035d5331c83d60409168b9dab471ea0c2bb73f4e3ec23c194467e54e644100"
        key9a5313e.Text = "a1fab44f59d9b22e59ad7deaed305e0b14a55058f070c139087c19aff2b61420371edcb6"
        key9a334beta.Text = nokey
    End Sub
    Private Sub btniPod41_Click() Handles btniPod41.Click
        ' iPod touch 4G
        Call clear()
        ' 4.x Final
        key8b117.Text = "e7de54b25167afc66e381ade1d5e25c6392757497cfd92826a3111772731ba0b70742b90"
        key8b118.Text = "770b58765a3345004528fd9a2cbb7c3105137d0bd3a134a24679e6e173f32636d0485d06"
        key8c148final.Text = "982437b30d334c744c94b9a73ab70e0fc6ed94c181b2a8b0fde6ee03f2546cc9b2c5b01c"
        key8f190final.Text = "b5eefbaf0046a79c689ff07e66ee8045f859dab1ee16d44d15606c1918e5afd323f2db07"
        key8g4.Text = "2cce34479eeb3701b3888f81c0465d2d98133af3a2761d0a82ad5074ca8efc1c5593eccb"
        key8h7.Text = "401b22ae26cca1aa2e119c17a6c389a1ba6aea0fbff4912000a77df953f010637b35d0ee"
        key8j2.Text = "d2877c05eb72e55d52d4e5e71c523a503c5bb8c85f6c7077d821140beea967782d30858d"
        key8k2.Text = "c71cb13f356620fdf7b1ab93470e3cff4d7f0f005d36bff5a6c3f8e60ab48e5b9d93841c"
        key8l1.Text = "bb8cc0947739d77f7c800fea823f37f34ebf1dfd77fefadaef163d3556a73b20ce411ed7"
        ' 4.x Beta
        key8c5091e.Text = nokey
        key8c5101c.Text = "146f6663f4073f2963af8abcbbe863598fe17ade32b852224eafb6897d1f6a51b407b514"
        key8c5115c.Text = "b72646053d205ffe249694de3fc8a62e6041d19f7f5be36537e891bdc954672cb7078a1b"
        key8c134.Text = "9e724a5712486a6609c9a1fe3e4addd8abe41957428532a6d0733cff92de877c1efcef1b"
        key8c148beta.Text = "982437b30d334c744c94b9a73ab70e0fc6ed94c181b2a8b0fde6ee03f2546cc9b2c5b01c"
        key8f5148b.Text = "258d1254eee085dc45894ad880763cfafe692bd5b3ae1055352ae4b35a7819fcb412d388"
        key8f5153d.Text = "258342d3c55579a8228442da4724ed3090a1a202454ccbda832649ec4a3baf3754a2ffcb"
        key8f5166b.Text = "6c3d8e506b4a4bfa525afffecbe01fdd6f50a3764ad9d9d31d566a6408a586c726700680"
        key8f190beta.Text = "b5eefbaf0046a79c689ff07e66ee8045f859dab1ee16d44d15606c1918e5afd323f2db07"
        ' 5.x Final
        key9a334final.Text = "575bcb4f9290a28bc00451f7e444973fd8b0afc529d2d84db4ae227bdd779563f070eaea"
        ' 5.x Beta
        key9a5220p.Text = "c35b6897e048e3e6ea454016089891db0c34a3b30b3777d5db9b8cfaa0ceed82e771b2ed"
        key9a5248d.Text = "81aa75d2c9b0f2c777ac0879e1326c98d5fed533dac4d5cea3e9eabec96b8161b947aa6e"
        key9a5259f.Text = "93095c15c02d45aa28679ac18a31d01ec14834f19ccea33c159a9a78010963bb86babe92"
        key9a5274d.Text = "3217c37f8505a3a78e70174e686c13cc8e70174e686c13cc8e70174e686c13cc8e70174e"
        key9a5288d.Text = "31a02bd24385485793e575dc755f4f29064a8550d9e095ce5c230c5557aab8f2e08bfdef"
        key9a5302b.Text = "765bbbf5669c8cc3b7d68447c64c628e04f38ea65a61f98549bf97d52468e2c4e6153395"
        key9a5313e.Text = "938188db58e8f5e057d42036e23bd40e451ed58df600f65718b9f335e140e3f6400873e7"
        key9a334beta.Text = "575bcb4f9290a28bc00451f7e444973fd8b0afc529d2d84db4ae227bdd779563f070eaea"
    End Sub
    Private Sub btnAppleTV21_Click() Handles btnAppleTV21.Click
        ' Apple TV 2G
        Call clear()
        ' 4.x Final ATV
        key8m89.Text = "31c700a852f1877c88efc05bc5c63e8c7f081c4cb28d024ed7f9b0dbc98c7e1406e499c6"
        key8c150.Text = "fd73cd898b7e55f9dc24092a4c574f1f284087075520a7d30232b0b6af8871743a0f0b82"
        key8c154.Text = "5407d28e075f5a2e06fddb7ad00123aa5a528bd6c2850d5fa0908a4dcae7dd3e00a9cdb2"
        key8f191m.Text = "a30f67f64a546a28bfd5c6e2e43d14fd8111c55641fc6dc392891e2b14e7e8138db9344c"
        key8f202.Text = "7fb6a5a1a5d74ceb61180c8740065b79ac87a5c15e554ad4b147ae9e1446254acc9d5e4a"
        key8f305.Text = "f607711d4db94bba7a4866f095aed082c8485bfbcd0f411f1e65158f585915edd5cfeec1"
        key8f455.Text = "32c6a922fdc1a474371fcfcbf8b5bf4a87ce01b6e672c360405a0dd238ad693769f0ce77"
        key9a334v.Text = nokey
        ' 4.x Beta
        key8f5148c.Text = "74e3afbad43debe898a556fa1446740598a556fa1446740598a556fa1446740598a556fa"
        key8f5153d.Text = "85ba2b2d95c89df504f54869b98d0eb26a63f269570e8882cb323b1b753f4f41446a1f0a"
        key8f5166b.Text = "b87f853c8f45aab846ebb507fbfca1039ab3dd7aed32076599d79070bc05240f59957064"
        ' 5.x Beta
        key9a5220p.Text = "91adee4d938e7f1ab7d9aa0863d9bb58b1056f410b7c1f28444ae1a293d3262cf1622402"
        key9a5248d.Text = "7bf1338a764b9e566982f86a95e597aae247cff3b55f30aeb7f61ec35a1f5b43e6d78773"
        key9a5259f.Text = "37c0ea663c670500c99424031e54a9d4d55e1156914a95ba54a62335c9e5f13d5c2cfe14"
        key9a5288d.Text = "40f3d0718052dda2f2c6637be99dc5717938149ac1b30a979fdd31109dc8fb0e83b4373c"
        key9a5302b.Text = "5134c59a148b2151a001cc5d984bb28339a379a47f2b0a40d9f7e16db0e1c44f7e2da028"
        key9a5313e.Text = "cd5cbb28e733d7a538ad949f3ad295b8780185ff5103feb3e87eedf25ce0aff598e2ba1f"
    End Sub
    Private Sub btnClearKey_Click() Handles btnClearKey.Click
        Me.textDecryptKey.Text = ""
    End Sub

    ' Load and close
    Private Sub MainWindow_Activated() Handles Me.Activated
        If wantedlang = "en" Then
        ElseIf wantedlang = "es" Then
            Call setlanges()
        Else
            ' Iron out this bug
            ' Call deletelanguage()
        End If
    End Sub
    Private Sub MainWindow_Closing() Handles Me.Closing
        Call cleanup()
    End Sub
    Private Sub MainWindow_Loaded() Handles Me.Loaded
        Dim selectlang As Window = New SelectLangControl
        Dim i As Integer
        Dim startargs() As String = Environment.GetCommandLineArgs

        ' cleanup() is called before to clear any leftover crap from a crash while checking
        Call cleanup()
        Directory.CreateDirectory(tempdir)

        ' Because the updater downloads to .exe.new after extraction,
        ' if that file exists, delete the .exe and rename the .exe.new to .exe
        If (File.Exists(rundir + "\iDecryptIt-Updater.exe.new")) Then
            File.Delete(rundir + "\iDecryptIt-Updater.exe")
            File.Move(rundir + "\iDecryptIt-Updater.exe.new", rundir + "\iDecryptIt-Updater.exe")
        End If
        DoCMD(rundir + "\iDecryptIt-Updater.exe")

        ' Language checker
        Dim langcode As Microsoft.Win32.RegistryKey
        langcode = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Cole Stuff\\iDecryptIt", True)
        If langcode Is Nothing Then
            selectlang.Show()
        Else
            wantedlang = Registry.GetValue("HKEY_CURRENT_USER\SOFTWARE\\Cole Stuff\\iDecryptIt", "language", "en")
        End If

        ' Is there a specified DMG?
        ' TODO: Make sure the last 4 characters are .dmg (case-insensitive)
        For i = 0 To UBound(startargs)
            If i = 1 Then
                Me.textInputFileName.Text = startargs(1)
                Me.textOuputFileName.Text = replacedmg(startargs(1))
            End If
        Next i
    End Sub

    ' Language stuff
    Private Sub setlanges()
        ' NOTE: Apple TV, iPad, iPhone, and iPod touch do not translate to anything
        ' NOTE: This may contain errors as this is Google Translate
        ' NOTE: "Web" does not need to be translated
        ' NOTE: Web pages need to be translated
        '-----------------------------------------------------------------------------------
        ' Ribbon
        HomeTab.Header = "casa"
        VFDecrypt.Header = "VFDescifrar"
        btnDecrypt.Label = "Descifrar"
        textInputFileName.Label = "Archivo de entrada:"
        textOuputFileName.Label = "De salida del archivo:"
        btnSelectVFDecryptInutFile.Label = "Seleccione Archivo de entrada"
        textDecryptKey.Label = "Clave:"
        btnClearKey.Label = "tecla de borrado"
        Extract.Header = "Extraer"
        btnExtract.Label = "Extraer"
        textExtractFileName.Label = "Archivo de entrada:"
        btnSelectExtractFile.Label = "Seleccione Archivo de entrada"
        HelpTab.Header = "Ayuda"
        HelpGroup.Header = "Ayuda"
        btnAbout.Label = "Acerca de iDecryptIt"
        btnVFDecrypt.Label = "Acerca de VFDescifrar"
        btnColeStuff.Label = "Acerca de Cole Cosas"
        btnREADME.Label = "Léame"
        ExtrasGroup.Header = "Más"
        btnChangelog.Label = "Cambios"
        btnHelpOut.Label = "Publicar Clave"
        btnChangeLanguage.Label = "Cambio de idioma"
        KeyListTab.Header = "las claves"
        ' Main Area
        v1Final.Header = "1.x Pasado"
        btn1a420.Content = "Prueba"
        v1Beta.Header = "1.x Prueba"
        v2Final.Header = "2.x Pasado"
        v2Beta.Header = "2.x Prueba"
        v3Final.Header = "3.x Pasado"
        v3Beta.Header = "3.x Prueba"
        v4Final.Header = "4.x Pasado"
        v4Beta.Header = "4.x Prueba"
        v5Final.Header = "5.x Pasado"
        v5Beta.Header = "5.x Prueba"
        ' Little Tab Notes
        note1xbeta.Text = "AVISO: 1.2 nunca fue publicada. En su lugar, se cambió a 2,0."
        note2xbeta.Text = "AVISO: 2,0 prueba es en realidad un 1,2 de prueba 1."
        note4xbeta.Text = "AVISO: Por lo que el Apple TV informes de las pruebas de 4,4 (5,0), por favor consulte la ficha de prueba 5.x."
        note4xfinalatv.Text = "AVISO: En esta página, el número de versión de la izquierda es lo que los informes de Apple TV, mientras que el de la derecha es la versión de Apple"
        note5xbeta.Text = "AVISO: en el Apple TV, esta prueba será reportado como 4,4."
        ' Notes
        nokey = "Ninguno de publicación"
        unavailable = "Construir, no disponibles para este dispositivo"
    End Sub
    Private Sub deletelanguage()
        result = MsgBox("ERROR! The setting for the language is not English or Spanish! Shall I delete it?", MsgBoxStyle.YesNo, "ERROR!")
        If (result = 7) Then
            ' They said yes
            Registry.CurrentUser.DeleteSubKey("SOFTWARE\\Cole Stuff\\iDecryptIt")
        End If
    End Sub
End Class