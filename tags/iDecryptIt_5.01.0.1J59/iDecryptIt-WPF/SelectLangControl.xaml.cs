using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace iDecryptIt_WPF
{
    /// <summary>
    /// Interaction logic for SelectLangControl.xaml
    /// </summary>
    public partial class SelectLangControl : Window
    {
        public SelectLangControl()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string lang;
            RegistryKey langcode;
            langcode = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Cole Stuff\\iDecryptIt", true);
            lang = (string)langcode.GetValue("language");
            
            // Failsafe for transition
            if (lang == "en")
            {
                lang = "eng";
            }
            if (lang == "es")
            {
                lang = "spa";
            }

            // Change language
            if (lang == "eng")
            {
                cmbSelect.SelectedIndex = 0;
            }
            else if (lang == "spa")
            {
                cmbSelect.SelectedIndex = 1;
            }
            else if (lang == "hin")
            {
                cmbSelect.SelectedIndex = 2;
            }
            else
            {
                // Fall back to English if is not any of the above
                Registry.CurrentUser.DeleteSubKey("SOFTWARE\\Cole Stuff\\iDecryptIt");
                cmbSelect.SelectedIndex = 0;
            }
        }
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            MainWindow myObject = new MainWindow();
            int selected = cmbSelect.SelectedIndex;
            if (selected == 0)
            {
                enter("eng");
                MessageBox.Show("You must restart iDecryptIt for changes to take effect", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            if (selected == 1)
            {
                enter("spa");
                myObject.setlang_spa();
            }
            if (selected == 2)
            {
                enter("hin");
                myObject.setlang_hin();
            }
        }
        private void enter(string lang)
        {
            RegistryKey langcode;
            langcode = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Cole Stuff\\iDecryptIt");
            langcode = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Cole Stuff\\iDecryptIt", true);
            Registry.SetValue("HKEY_CURRENT_USER\\SOFTWARE\\Cole Stuff\\iDecryptIt", "language", lang, RegistryValueKind.String);
            Close();
        }
    }
}
