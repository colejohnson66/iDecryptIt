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

            //InitKeyGridDictionary();
        }

        private void Dispatcher_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Trace(nameof(Dispatcher_UnhandledException), sender, e);

            FatalError("An unknown error has occured.", e.Exception);
            Close();
            e.Handled = true;
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
        }
    }
}
