/* =============================================================================
 * File:   MainWindow2.xaml.cs
 * Author: Cole Johnson
 * =============================================================================
 * Copyright (c) 2018 Cole Johnson
 * 
 * This file is part of iDecryptIt.
 * 
 * iDecryptIt is free software: you can redistribute it and/or modify it under
 *   the terms of the GNU General Public License as published by the Free
 *   Software Foundation, either version 3 of the License, or (at your option)
 *   any later version.
 * 
 * iDecryptIt is distributed in the hope that it will be useful, but WITHOUT
 *   ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 *   FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for
 *   more details.
 * 
 * You should have received a copy of the GNU General Public License along with
 *   iDecryptIt. If not, see <http://www.gnu.org/licenses/>.
 * =============================================================================
 */
using Hexware.Plist;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Hexware.Programs.iDecryptIt
{
    public partial class MainWindow2 : Window
    {
        static string execDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        string execHash = new Random().Next().ToString("X");

        KeySelectionViewModel DevicesViewModel;
        string selectedDevice;
        KeySelectionViewModel ModelsViewModel;
        string selectedModel;
        KeySelectionViewModel VersionsViewModel;
        string selectedVersion;

        Process rootFSDecryptProcess;
        long rootFSDecryptFromLength;
        double rootFSDecryptProgress;
        BackgroundWorker rootFSDecryptWorker;

        public MainWindow2()
        {
            if (!Globals.Debug)
                NativeMethods.FreeConsole();

            DevicesViewModel = new KeySelectionViewModel();
            ModelsViewModel = new KeySelectionViewModel();
            VersionsViewModel = new KeySelectionViewModel();

            InitializeComponent();

            this.DataContext = this;
            KeySelectionLists.Init();
            GetKeysDeviceComboBox.ItemsSource = KeySelectionLists.Products;

#if !DEBUG
            this.Dispatcher.UnhandledException += Dispatcher_UnhandledException;
#endif
        }

        private void Dispatcher_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            FatalError("An unknown error has occured.", e.Exception);
            Close();
            e.Handled = true;
        }
        private void Error(string v, Exception ex)
        {
            throw new NotImplementedException();
        }
        private void FatalError(string v, Exception exception)
        {
            throw new NotImplementedException();
        }
        private static void Debug(string component, string message)
        {
            if (!Globals.Debug)
                return;
            Console.WriteLine("{0}{1}", component.PadRight(12), message);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Debug("[INIT]", "Opening...");
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Debug("[DEINIT]", "Closing...");

            Thread.Sleep(500);
            Application.Current.Shutdown();
        }

        // Identify IPSW Pane
        private void IdentifySelectInputFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Apple Firmware Files|*.ipsw",
                CheckFileExists = true
            };

            Debug("[IDENTIFY]", "Opening file dialog.");
            dialog.ShowDialog();
            Debug("[IDENTIFY]", "Closing file dialog.");

            if (!String.IsNullOrWhiteSpace(dialog.SafeFileName))
            {
                Debug("[IDENTIFY]", "Setting textbox to " + dialog.SafeFileName);
                IdentifyTextBox.Text = dialog.SafeFileName;
            }
        }
        private void IdentifyButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(IdentifyTextBox.Text))
                return;

            string[] strArr = IdentifyTextBox.Text.Split('_');
            if (strArr.Length != 4 || strArr[3] != "Restore.ipsw")
            {
                // TODO: Replace with Error()
                MessageBox.Show(
                    "The supplied IPSW File that was given is not in the following format:\r\n" +
                        "\t{DEVICE}_{VERSION}_{BUILD}_Restore.ipsw",
                    "iDecryptIt",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            if (!Globals.DeviceNames.TryGetValue(strArr[0], out string device))
                device = strArr[0];

            IdentifyResultsDevice.Text = "Device: " + device;
            IdentifyResultsVersion.Text = "Version: " + strArr[1];
            IdentifyResultsBuild.Text = "Build: " + strArr[2];
        }

        // Pane change buttons
        private void MainIdentifyPaneButton_Click(object sender, RoutedEventArgs e)
        {
            MainPane.Visibility = Visibility.Hidden;
            IdentifyPane.Visibility = Visibility.Visible;
        }
        private void MainGetKeysPaneButton_Click(object sender, RoutedEventArgs e)
        {
            MainPane.Visibility = Visibility.Hidden;
            GetKeysPane.Visibility = Visibility.Visible;
        }
        private void MainDecryptRootFSPaneButton_Click(object sender, RoutedEventArgs e)
        {
            MainPane.Visibility = Visibility.Hidden;
            DecryptRootFSPane.Visibility = Visibility.Visible;
        }
        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            MainPane.Visibility = Visibility.Visible;
            IdentifyPane.Visibility = Visibility.Hidden;
            GetKeysPane.Visibility = Visibility.Hidden;
            DecryptRootFSPane.Visibility = Visibility.Hidden;
        }

        // Get Keys Pane
        private void GetKeysDeviceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetKeysStatusBar.Text = "";
            GetKeysClearStackPanel();

            if (e.AddedItems.Count == 0)
                return;

            ComboBoxEntry entry = (ComboBoxEntry)e.AddedItems[0];
            selectedDevice = entry.ID;

            selectedModel = null;
            GetKeysModelComboBox.IsEnabled = true;
            GetKeysModelComboBox.ItemsSource = KeySelectionLists.ProductsHelper[selectedDevice];

            selectedVersion = null;
            GetKeysVersionComboBox.IsEnabled = false;
            GetKeysVersionComboBox.ItemsSource = null;
        }
        private void GetKeysModelComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetKeysStatusBar.Text = "";
            GetKeysClearStackPanel();

            if (e.AddedItems.Count == 0)
                return;

            ComboBoxEntry entry = (ComboBoxEntry)e.AddedItems[0];
            selectedModel = entry.ID;

            selectedVersion = null;
            GetKeysVersionComboBox.IsEnabled = true;
            GetKeysVersionComboBox.ItemsSource = KeySelectionLists.ModelsHelper[selectedModel];
        }
        private void GetKeysVersionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetKeysStatusBar.Text = "";
            GetKeysClearStackPanel();

            if (e.AddedItems.Count == 0)
                return;

            ComboBoxEntry entry = (ComboBoxEntry)e.AddedItems[0];
            selectedVersion = entry.ID;

            Stream stream = GetKeyStream(selectedModel + "_" + selectedVersion + ".plist");
            if (stream == Stream.Null)
            {
                Debug("[KEYSELECT]", "Key file doesn't exist. No keys available.");
                GetKeysStatusBar.Text = "No keys are available for that device/version combo";
                return;
            }
            LoadKeys(stream);
        }

        private void GetKeysClearStackPanel()
        {
            GetKeysStackPanel.Children.Clear();
        }
        private void GetKeysAddDeviceAndVersion(string device, string version, string build)
        {
            TextBlock deviceBlock = new TextBlock
            {
                Text = device,
                FontSize = 24
            };
            GetKeysStackPanel.Children.Add(deviceBlock);

            TextBlock versionBlock = new TextBlock
            {
                Text = String.Format("{0} (Build {1})", version, build),
                FontSize = 16,
                Margin = new Thickness(12, 6, 0, 6)
            };
            GetKeysStackPanel.Children.Add(versionBlock);
        }
        private void GetKeysAddNotEncryptedRootFS(string rootFSFilename)
        {
            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(125, GridUnitType.Pixel) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(6, GridUnitType.Pixel) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(6, GridUnitType.Pixel) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(125, GridUnitType.Pixel) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(6, GridUnitType.Pixel) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(75, GridUnitType.Pixel) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(6, GridUnitType.Pixel) });
            grid.Margin = new Thickness(0, 3, 0, 3);
            
            TextBlock rootFSLabel = new TextBlock
            {
                Text = "Root FS",
                VerticalAlignment = VerticalAlignment.Center
            };
            grid.Children.Add(rootFSLabel);

            TextBlock notEncryptedLabel = new TextBlock
            {
                Name = "RootFSNotEncrypted",
                Text = "Not Encrypted",
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            notEncryptedLabel.SetValue(Grid.ColumnProperty, 2);
            grid.Children.Add(notEncryptedLabel);

            TextBox filename = new TextBox
            {
                Name = "RootFSFilename",
                Text = rootFSFilename,
                IsReadOnly = true
            };
            filename.SetValue(Grid.ColumnProperty, 4);
            grid.Children.Add(filename);

            Button decrypt = new Button
            {
                Name = "RootFSDecrypt",
                Content = "Decrypt >"
            };
            decrypt.SetValue(Grid.ColumnProperty, 6);
            decrypt.Click += GetKeysDecryptButton_Click;
            grid.Children.Add(decrypt);

            GetKeysStackPanel.Children.Add(grid);
        }
        private void GetKeysAddRootFS(string rootFSKey, string rootFSFilename)
        {
            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(125, GridUnitType.Pixel) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(6, GridUnitType.Pixel) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(6, GridUnitType.Pixel) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(125, GridUnitType.Pixel) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(6, GridUnitType.Pixel) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(75, GridUnitType.Pixel) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(6, GridUnitType.Pixel) });
            grid.Margin = new Thickness(0, 3, 0, 3);

            TextBlock rootFSKeyLabel = new TextBlock
            {
                Text = "Root FS Key",
                VerticalAlignment = VerticalAlignment.Center
            };
            grid.Children.Add(rootFSKeyLabel);

            TextBox key = new TextBox
            {
                Name = "RootFSKey",
                Text = rootFSKey,
                IsReadOnly = true
            };
            key.SetValue(Grid.ColumnProperty, 2);
            grid.Children.Add(key);

            TextBox filename = new TextBox
            {
                Name = "RootFSFilename",
                Text = rootFSFilename,
                IsReadOnly = true
            };
            filename.SetValue(Grid.ColumnProperty, 4);
            grid.Children.Add(filename);

            Button decrypt = new Button
            {
                Name = "RootFSDecrypt",
                Content = "Decrypt >"
            };
            decrypt.SetValue(Grid.ColumnProperty, 6);
            decrypt.Click += GetKeysDecryptButton_Click;
            grid.Children.Add(decrypt);

            GetKeysStackPanel.Children.Add(grid);
        }
        private void GetKeysAddNotEncryptedItem(string firmwareItem, string id, string firmwareFilename)
        {
            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(125, GridUnitType.Pixel) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(6, GridUnitType.Pixel) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(6, GridUnitType.Pixel) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(125, GridUnitType.Pixel) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(6, GridUnitType.Pixel) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(75, GridUnitType.Pixel) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(6, GridUnitType.Pixel) });
            grid.Margin = new Thickness(0, 3, 0, 3);

            TextBlock label = new TextBlock
            {
                Text = firmwareItem,
                VerticalAlignment = VerticalAlignment.Center
            };
            grid.Children.Add(label);

            TextBlock key = new TextBlock
            {
                Name = id + "NotEncrypted",
                Text = "Not Encrypted",
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            key.SetValue(Grid.ColumnProperty, 2);
            grid.Children.Add(key);

            TextBox filename = new TextBox
            {
                Name = id + "Filename",
                Text = firmwareFilename,
                IsReadOnly = true
            };
            filename.SetValue(Grid.ColumnProperty, 4);
            grid.Children.Add(filename);

            Button decrypt = new Button
            {
                Name = id + "Decrypt",
                Content = "Decrypt >"
            };
            decrypt.SetValue(Grid.ColumnProperty, 6);
            decrypt.Click += GetKeysDecryptButton_Click;
            grid.Children.Add(decrypt);

            GetKeysStackPanel.Children.Add(grid);
        }
        private void GetKeysAddEncryptedItem(string firmwareItem, string id, string firmwareIV, string firmwareKey, string firmwareFilename)
        {
            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(125, GridUnitType.Pixel) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(6, GridUnitType.Pixel) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(6, GridUnitType.Pixel) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(125, GridUnitType.Pixel) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(6, GridUnitType.Pixel) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(75, GridUnitType.Pixel) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(6, GridUnitType.Pixel) });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(6, GridUnitType.Pixel) });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            grid.Margin = new Thickness(0, 3, 0, 3);

            TextBlock ivLabel = new TextBlock
            {
                Text = firmwareItem + " (IV)",
                VerticalAlignment = VerticalAlignment.Center
            };
            grid.Children.Add(ivLabel);

            TextBlock keyLabel = new TextBlock
            {
                Text = firmwareItem + " (Key)",
                VerticalAlignment = VerticalAlignment.Center
            };
            keyLabel.SetValue(Grid.RowProperty, 2);
            grid.Children.Add(keyLabel);

            TextBox iv = new TextBox
            {
                Name = id + "IV",
                Text = firmwareIV,
                IsReadOnly = true
            };
            iv.SetValue(Grid.ColumnProperty, 2);
            grid.Children.Add(iv);

            TextBox key = new TextBox
            {
                Name = id + "Key",
                Text = firmwareKey,
                IsReadOnly = true
            };
            key.SetValue(Grid.ColumnProperty, 2);
            key.SetValue(Grid.RowProperty, 2);
            grid.Children.Add(key);

            TextBox filename = new TextBox
            {
                Name = id + "Filename",
                Text = firmwareFilename,
                IsReadOnly = true,
                VerticalAlignment = VerticalAlignment.Center
            };
            filename.SetValue(Grid.ColumnProperty, 4);
            filename.SetValue(Grid.RowSpanProperty, 3);
            grid.Children.Add(filename);

            Button decrypt = new Button
            {
                Name = id + "Decrypt",
                Content = "Decrypt >",
                VerticalAlignment = VerticalAlignment.Center
            };
            decrypt.SetValue(Grid.ColumnProperty, 6);
            decrypt.SetValue(Grid.RowSpanProperty, 3);
            decrypt.Click += GetKeysDecryptButton_Click;
            grid.Children.Add(decrypt);

            GetKeysStackPanel.Children.Add(grid);
        }

        private UIElement GetKeysStackPanelFindObject(string name)
        {
            foreach (UIElement possibleGrid in GetKeysStackPanel.Children)
            {
                if (possibleGrid.GetType() == typeof(Grid))
                {
                    foreach (UIElement gridItem in ((Grid)possibleGrid).Children)
                    {
                        if (gridItem.GetType() == typeof(TextBlock) &&
                            ((TextBlock)gridItem).Name == name)
                            return gridItem;
                        if (gridItem.GetType() == typeof(TextBox) &&
                            ((TextBox)gridItem).Name == name)
                            return gridItem;
                        if (gridItem.GetType() == typeof(Button) &&
                            ((Button)gridItem).Name == name)
                            return gridItem;
                    }
                }
            }

            return null;
        }
        private void GetKeysDecryptButton_Click(object sender, RoutedEventArgs e)
        {
            string buttonName = ((Button)sender).Name;

            if (buttonName == "RootFSDecrypt")
            {
                if (GetKeysStackPanelFindObject("RootFSNotEncrypted") == null)
                {
                    TextBox keyBox = (TextBox)GetKeysStackPanelFindObject("RootFSKey");
                    DecryptRootFSKey.Text = keyBox.Text;
                }
            }
            else
            {

            }

            GetKeysPane.Visibility = Visibility.Hidden;
            DecryptRootFSPane.Visibility = Visibility.Visible;

            UIElement obj = GetKeysStackPanelFindObject("RootFSKey");
        }

        private void LoadKeys(Stream document)
        {
            PlistDict plist;
            try
            {
                PlistDocument doc = new PlistDocument(document);
                plist = (PlistDict)doc.RootNode;
            }
            catch (Exception ex)
            {
                Error("Error loading key file.", ex);
                return;
            }

            string device = plist.Get<PlistString>("Device").Value;
            string version = plist.Get<PlistString>("Version").Value;
            string build = plist.Get<PlistString>("Build").Value;
            GetKeysAddDeviceAndVersion(Globals.DeviceNames[device], version, build);

            PlistDict rootFS = plist.Get<PlistDict>("Root FS");
            string rootFSFilename = rootFS.Get<PlistString>("File Name").Value;
            if (!rootFS.Get<PlistBool>("Encryption").Value)
                GetKeysAddNotEncryptedRootFS(rootFSFilename);
            else
                GetKeysAddRootFS(rootFS.Get<PlistString>("Key").Value, rootFSFilename);

            ProcessFirmwareItem(plist, "Update Ramdisk", "UpdateRamdisk");
            ProcessFirmwareItem(plist, "Update Ramdisk2", "UpdateRamdisk2");
            ProcessFirmwareItem(plist, "Restore Ramdisk", "RestoreRamdisk");
            ProcessFirmwareItem(plist, "Restore Ramdisk2", "RestoreRamdisk2");
            ProcessFirmwareItem(plist, "AOPFirmware");
            ProcessFirmwareItem(plist, "AppleLogo");
            ProcessFirmwareItem(plist, "AppleLogo2");
            ProcessFirmwareItem(plist, "AppleMaggie");
            ProcessFirmwareItem(plist, "AudioDSP");
            ProcessFirmwareItem(plist, "BatteryCharging");
            ProcessFirmwareItem(plist, "BatteryCharging0");
            ProcessFirmwareItem(plist, "BatteryCharging02");
            ProcessFirmwareItem(plist, "BatteryCharging1");
            ProcessFirmwareItem(plist, "BatteryCharging12");
            ProcessFirmwareItem(plist, "BatteryFull");
            ProcessFirmwareItem(plist, "BatteryFull2");
            ProcessFirmwareItem(plist, "BatteryLow0");
            ProcessFirmwareItem(plist, "BatteryLow02");
            ProcessFirmwareItem(plist, "BatteryLow1");
            ProcessFirmwareItem(plist, "BatteryLow12");
            ProcessFirmwareItem(plist, "Dali");
            ProcessFirmwareItem(plist, "DeviceTree");
            ProcessFirmwareItem(plist, "DeviceTree2");
            ProcessFirmwareItem(plist, "GlyphCharging");
            ProcessFirmwareItem(plist, "GlyphPlugin");
            ProcessFirmwareItem(plist, "GlyphPlugin2");
            ProcessFirmwareItem(plist, "Homer");
            ProcessFirmwareItem(plist, "iBEC");
            ProcessFirmwareItem(plist, "iBEC2");
            ProcessFirmwareItem(plist, "iBoot");
            ProcessFirmwareItem(plist, "iBoot2");
            ProcessFirmwareItem(plist, "iBSS");
            ProcessFirmwareItem(plist, "iBSS2");
            ProcessFirmwareItem(plist, "Kernelcache");
            ProcessFirmwareItem(plist, "Kernelcache2");
            ProcessFirmwareItem(plist, "LiquidDetect");
            ProcessFirmwareItem(plist, "LLB");
            ProcessFirmwareItem(plist, "LLB2");
            ProcessFirmwareItem(plist, "Multitouch");
            ProcessFirmwareItem(plist, "NeedService");
            ProcessFirmwareItem(plist, "RecoveryMode");
            ProcessFirmwareItem(plist, "RecoveryMode2");
            ProcessFirmwareItem(plist, "SEP-Firmware", "SEPFirmware");
            ProcessFirmwareItem(plist, "SEP-Firmware2", "SEPFirmware2");
        }
        private Stream GetKeyStream(string fileName)
        {
            Debug("[GETSTREAM]", "Attempting read of stored resource, \"" + fileName + "\".");

            if (!Globals.KeyArchive.FileExists(fileName))
                return Stream.Null;

            try
            {
                return Globals.KeyArchive.GetFile(fileName);
            }
            catch (Exception ex)
            {
                Error("Unable to retrieve keys.", ex);
            }
            return Stream.Null;
        }
        private void ProcessFirmwareItem(PlistDict rootNode, string firmwareItem)
        {
            ProcessFirmwareItem(rootNode, firmwareItem, firmwareItem);
        }
        private void ProcessFirmwareItem(PlistDict rootNode, string firmwareItem, string id)
        {
            if (!rootNode.Exists(firmwareItem))
                return;

            PlistDict node = rootNode.Get<PlistDict>(firmwareItem);

            if (firmwareItem.EndsWith("2"))
                firmwareItem = firmwareItem.Substring(0, firmwareItem.Length - 1);

            if (node.Get<PlistBool>("Encryption").Value)
            {
                GetKeysAddEncryptedItem(
                    firmwareItem,
                    id,
                    node.Get<PlistString>("IV").Value,
                    node.Get<PlistString>("Key").Value,
                    node.Get<PlistString>("File Name").Value);
            }
            else
            {
                GetKeysAddNotEncryptedItem(
                    firmwareItem,
                    id,
                    node.Get<PlistString>("File Name").Value);
            }
        }

        // Decrypt Root FS Pane
        private void DecryptRootFSInputSelect_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Filter = "Apple Disk Images|*.dmg",
                CheckFileExists = true
            };
            dialog.ShowDialog();

            if (String.IsNullOrWhiteSpace(dialog.SafeFileName))
                return;

            DecryptRootFSInput.Text = dialog.FileName;
            DecryptRootFSOutput.Text = dialog.FileName.Substring(0, dialog.FileName.Length - 4) + "_decrypted.dmg";
        }
        private void DecryptRootFSInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = DecryptRootFSInput.Text;
            try
            {
                string folder = Path.GetDirectoryName(input);
                string file = Path.GetFileName(input);
                if (!file.EndsWith(".dmg"))
                    return;
                file = file.Substring(0, file.Length - 4) + "_decrypted.dmg";
                DecryptRootFSOutput.Text = Path.Combine(folder, file);
            }
            catch (Exception)
            { }
        }
        private void DecryptRootFSButton_Click(object sender, RoutedEventArgs e)
        {
            string input = DecryptRootFSInput.Text;
            string output = DecryptRootFSOutput.Text;
            string key = DecryptRootFSKey.Text;

            if (String.IsNullOrWhiteSpace(input) ||
                String.IsNullOrWhiteSpace(output))
                return;

            if (!File.Exists(input))
            {
                MessageBox.Show(
                    "The input file does not exist.",
                    "iDecryptIt");
                return;
            }
            if (File.Exists(output))
            {
                if (MessageBox.Show(
                    "The output file already exists. Shall I delete it?",
                    "iDecryptIt",
                    MessageBoxButton.YesNo) == MessageBoxResult.No)
                    return;
                File.Delete(output);
            }

            rootFSDecryptFromLength = new FileInfo(input).Length;

            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                FileName = Path.Combine(execDir, "dmg.exe"),
                Arguments = String.Format("extract \"{0}\" \"{1}\" -k {2}", input, output, key),
                ErrorDialog = true
            };

            rootFSDecryptProcess = new Process()
            {
                EnableRaisingEvents = true,
                StartInfo = startInfo
            };
            rootFSDecryptProcess.OutputDataReceived += RootFSDecryptProcess_OutputDataReceived;
            rootFSDecryptProcess.ErrorDataReceived += RootFSDecryptProcess_ErrorDataReceived;
            rootFSDecryptProcess.Start();
            rootFSDecryptProcess.BeginOutputReadLine(); // execution halts if buffer is full
            rootFSDecryptProcess.BeginErrorReadLine();

            // Screen mods
            DecryptRootFSGrid.IsEnabled = false;
            DecryptRootFSProgressBar.Visibility = Visibility.Visible;

            // Wait for output file to exist before starting worker as processes are async
            while (!File.Exists(output)) { }
            rootFSDecryptWorker = new BackgroundWorker()
            {
                WorkerSupportsCancellation = true,
                WorkerReportsProgress = true
            };
            rootFSDecryptWorker.DoWork += RootFSDecryptWorker_DoWork;
            rootFSDecryptWorker.ProgressChanged += RootFSDecryptWorker_ProgressChanged;
        }

        private void RootFSDecryptProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(e.Data))
                return;

            if (Globals.Debug)
                Console.WriteLine(e.Data);

            // Until iDecryptIt natively supports decrypting disk images, use this crude hack to calculate progress

            // dmg's progress is reported with each "run" of sectors on a line formatted like this:
            // run 36: start=32604160 sectors=512, length=105688, fileOffset=0x184c75
            // What we care about is `fileOffset'. Surprisingly, that's where the run begins. Because dmg progresses
            //   through the file linearly, we can simply use that number to know how much dmg has decrypted/decompressed.

            int idx = e.Data.IndexOf("fileOffset");
            if (idx == -1)
                return; // ignore this line of output
            long offset = Convert.ToInt64(e.Data.Substring(idx + "fileOffset=0x".Length), 16);
            rootFSDecryptProgress = (offset * 100.0) / rootFSDecryptFromLength;
        }
        private void RootFSDecryptProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            // dmg doesn't use stderr, but this function is still needed
            if (Globals.Debug)
                Console.WriteLine(e.Data);
        }
        private void RootFSDecryptWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!rootFSDecryptWorker.CancellationPending)
            {
                if (rootFSDecryptProcess.HasExited)
                    rootFSDecryptWorker.ReportProgress(100);
                else
                    rootFSDecryptWorker.ReportProgress(0);
                Thread.Sleep(25); // don't hog the CPU
            }
        }
        private void RootFSDecryptWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 100 && !rootFSDecryptWorker.CancellationPending)
            {
                rootFSDecryptWorker.CancelAsync();
                DecryptRootFSGrid.IsEnabled = true;
                DecryptRootFSProgressBar.Visibility = Visibility.Hidden;

                rootFSDecryptProgress = 0.0;

                return;
            }

            if (rootFSDecryptProgress > 100.0)
                DecryptRootFSProgressBar.Value = 100.0;
            else
                DecryptRootFSProgressBar.Value = rootFSDecryptProgress;
        }
    }
}
