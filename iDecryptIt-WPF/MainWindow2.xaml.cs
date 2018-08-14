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
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace Hexware.Programs.iDecryptIt
{
    /// <summary>
    /// Interaction logic for MainWindow2.xaml
    /// </summary>
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

            this.Dispatcher.UnhandledException += Dispatcher_UnhandledException;
        }

        private void Dispatcher_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                Trace(nameof(Dispatcher_UnhandledException), sender, e);
            }
            catch (Exception ex)
            { }

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
        private static void Trace(string functionName, params object[] args)
        {
            if (!Globals.Trace)
                return;

            if (args.Length == 0)
            {
                Console.WriteLine("{0}{1}()", "[TRACE]".PadRight(12), functionName);
                return;
            }
            Console.WriteLine("{0}{1}(", "[TRACE]".PadRight(12), functionName);
            
            for (int i = 0; i < args.Length; i++)
            {
                Console.Write("                {0}", args[i]); // 16 spaces for a 4 space indent
                if (i != args.Length - 1)
                    Console.WriteLine(", ");
            }
            Console.WriteLine(")");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Trace(nameof(Window_Loaded), sender, e);

            Debug("[INIT]", "Opening...");
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Trace(nameof(Window_Closing), sender, e);

            Debug("[DEINIT]", "Closing...");

            Thread.Sleep(500);
            Application.Current.Shutdown();
        }

        // Identify IPSW Pane
        private void IdentifySelectInputFileButton_Click(object sender, RoutedEventArgs e)
        {
            Trace(nameof(IdentifySelectInputFileButton_Click), sender, e);

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
            Trace(nameof(IdentifyButton_Click), sender, e);
            
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

            string device;
            if (!Globals.DeviceNames.TryGetValue(strArr[0], out device))
                device = strArr[0];

            IdentifyResultsDevice.Text = "Device: " + device;
            IdentifyResultsVersion.Text = "Version: " + strArr[1];
            IdentifyResultsBuild.Text = "Build: " + strArr[2];
        }

        // Pane change buttons
        private void MainIdentifyPaneButton_Click(object sender, RoutedEventArgs e)
        {
            Trace(nameof(MainIdentifyPaneButton_Click), sender, e);

            MainPane.Visibility = Visibility.Hidden;
            IdentifyPane.Visibility = Visibility.Visible;
        }
        private void MainGetKeysPaneButton_Click(object sender, RoutedEventArgs e)
        {
            Trace(nameof(MainGetKeysPaneButton_Click), sender, e);

            MainPane.Visibility = Visibility.Hidden;
            GetKeysPane.Visibility = Visibility.Visible;
        }
        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            Trace(nameof(GoBackButton_Click), sender, e);

            MainPane.Visibility = Visibility.Visible;
            IdentifyPane.Visibility = Visibility.Hidden;
            GetKeysPane.Visibility = Visibility.Hidden;
        }

        // Get Keys Pane
        private void GetKeysDeviceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Trace(nameof(GetKeysDeviceComboBox_SelectionChanged), sender, e);

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
            Trace(nameof(GetKeysModelComboBox_SelectionChanged), sender, e);

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
            Trace(nameof(GetKeysVersionComboBox_SelectionChanged), sender, e);

            if (e.AddedItems.Count == 0)
                return;

            ComboBoxEntry entry = (ComboBoxEntry)e.AddedItems[0];
            selectedVersion = entry.ID;

            Stream stream = GetKeyStream(selectedModel + "_" + selectedVersion + ".plist");
            if (stream == Stream.Null)
            {
                Debug("[KEYSELECT]", "Key file doesn't exist. No keys available.");
                //MessageBox.Show(
                //    "Sorry, but that version doesn't have any published keys.",
                //    "iDecryptIt",
                //    MessageBoxButton.OK,
                //    MessageBoxImage.Information);
                return;
            }
            LoadKeys(stream);
        }

        private void LoadKeys(Stream document)
        {
            Trace(nameof(LoadKeys), document);
            
            GetKeysClearStackPanel();
            GetKeysAddDeviceAndVersion("Device", "Version", "Build");
            GetKeysAddRootFS();
            GetKeysAddEncryptedItem("Restore Ramdisk", "RestoreRamdisk");
            GetKeysAddNotEncryptedItem("Update Ramdisk", "UpdateRamdisk");
        }
        internal Stream GetKeyStream(string fileName)
        {
            Trace(nameof(GetKeyStream), fileName);

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

        private void GetKeysClearStackPanel()
        {
            Trace(nameof(GetKeysClearStackPanel));

            GetKeysStackPanel.Children.Clear();
        }
        private void GetKeysAddDeviceAndVersion(string device, string version, string build)
        {
            Trace(nameof(GetKeysAddDeviceAndVersion), device, version, build);

            //<TextBlock Text="Device" x:Name="txtDevice" FontSize="24" />
            //<TextBlock Text="Version (Build)" x:Name="txtVersion" FontSize="16" Margin="12,6,0,6" />
            TextBlock deviceBlock = new TextBlock();
            deviceBlock.Text = device;
            deviceBlock.FontSize = 24;
            GetKeysStackPanel.Children.Add(deviceBlock);

            TextBlock versionBlock = new TextBlock();
            versionBlock.Text = String.Format("{0} ({1})", version, build);
            versionBlock.FontSize = 16;
            versionBlock.Margin = new Thickness(12, 6, 0, 6);
            GetKeysStackPanel.Children.Add(versionBlock);
        }
        private void GetKeysAddRootFS()
        {
            Trace(nameof(GetKeysAddRootFS));

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

            TextBlock rootFSKeyLabel = new TextBlock();
            rootFSKeyLabel.Text = "Root FS Key";
            rootFSKeyLabel.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(rootFSKeyLabel);

            TextBox key = new TextBox();
            key.Name = "RootFSKey";
            key.IsReadOnly = true;
            key.SetValue(Grid.ColumnProperty, 2);
            grid.Children.Add(key);

            TextBox filename = new TextBox();
            filename.Name = "RootFSFilename";
            filename.IsReadOnly = true;
            filename.SetValue(Grid.ColumnProperty, 4);
            grid.Children.Add(filename);

            Button decrypt = new Button();
            decrypt.Name = "RootFSDecrypt";
            decrypt.Content = "Decrypt >";
            decrypt.SetValue(Grid.ColumnProperty, 6);
            decrypt.Click += GetKeysDecryptButton_Click;
            grid.Children.Add(decrypt);

            GetKeysStackPanel.Children.Add(grid);
        }
        private void GetKeysAddNotEncryptedItem(string firmwareItem, string firmwareKey)
        {
            Trace(nameof(GetKeysAddNotEncryptedItem), firmwareItem, firmwareKey);

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

            TextBlock label = new TextBlock();
            label.Text = firmwareItem;
            label.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(label);

            TextBlock key = new TextBlock();
            key.Name = firmwareKey + "NotEncrypted";
            key.Text = "Not Encrypted";
            key.VerticalAlignment = VerticalAlignment.Center;
            key.HorizontalAlignment = HorizontalAlignment.Center;
            key.SetValue(Grid.ColumnProperty, 2);
            grid.Children.Add(key);

            TextBox filename = new TextBox();
            filename.Name = firmwareKey + "Filename";
            filename.IsReadOnly = true;
            filename.SetValue(Grid.ColumnProperty, 4);
            grid.Children.Add(filename);

            Button decrypt = new Button();
            decrypt.Name = firmwareKey + "Decrypt";
            decrypt.Content = "Decrypt >";
            decrypt.SetValue(Grid.ColumnProperty, 6);
            decrypt.Click += GetKeysDecryptButton_Click;
            grid.Children.Add(decrypt);

            GetKeysStackPanel.Children.Add(grid);
        }
        private void GetKeysAddEncryptedItem(string firmwareItem, string firmwareKey)
        {
            Trace(nameof(GetKeysAddEncryptedItem), firmwareItem, firmwareKey);

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

            TextBlock ivLabel = new TextBlock();
            ivLabel.Text = firmwareItem + " (IV)";
            ivLabel.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(ivLabel);

            TextBlock keyLabel = new TextBlock();
            keyLabel.Text = firmwareItem + " (Key)";
            keyLabel.VerticalAlignment = VerticalAlignment.Center;
            keyLabel.SetValue(Grid.RowProperty, 2);
            grid.Children.Add(keyLabel);

            TextBox iv = new TextBox();
            iv.Name = firmwareKey + "IV";
            iv.IsReadOnly = true;
            iv.SetValue(Grid.ColumnProperty, 2);
            grid.Children.Add(iv);

            TextBox key = new TextBox();
            key.Name = firmwareKey + "Key";
            key.IsReadOnly = true;
            key.SetValue(Grid.ColumnProperty, 2);
            key.SetValue(Grid.RowProperty, 2);
            grid.Children.Add(key);

            TextBox filename = new TextBox();
            filename.Name = firmwareKey + "Filename";
            filename.IsReadOnly = true;
            filename.SetValue(Grid.ColumnProperty, 4);
            filename.SetValue(Grid.RowSpanProperty, 3);
            filename.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(filename);

            Button decrypt = new Button();
            decrypt.Name = firmwareKey + "Decrypt";
            decrypt.Content = "Decrypt >";
            decrypt.SetValue(Grid.ColumnProperty, 6);
            decrypt.SetValue(Grid.RowSpanProperty, 3);
            decrypt.VerticalAlignment = VerticalAlignment.Center;
            decrypt.Click += GetKeysDecryptButton_Click;
            grid.Children.Add(decrypt);

            GetKeysStackPanel.Children.Add(grid);
        }

        private void GetKeysDecryptButton_Click(object sender, RoutedEventArgs e)
        {
            Trace(nameof(GetKeysDecryptButton_Click), sender, e);

            // ((Button)sender).Name contains the button name

            object obj = GetKeysStackPanelFindObject("RootFSKey");
        }
        private UIElement GetKeysStackPanelFindObject(string name)
        {
            Trace(nameof(GetKeysStackPanelFindObject), name);

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
    }
}
