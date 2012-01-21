using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace ColeStuff.DataManipulation
{
    public class INI
    {
        private string path;

        // The kernel DLL handles the writing and saving
        [DllImport("kernel32.dll")]
        private static extern long WritePrivateProfileString(
            string section,
            string key,
            string val,
            string filePath);

        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileString(
            string section,
            string key,
            string def,
            StringBuilder retVal,
            int size,
            string filePath);

        /// <summary>
        /// INIFile Constructor
        /// </summary>
        /// 
        /// <param name="INIPath">The full path of the INI file</param>
        public INI(string INIPath)
        {
            path = INIPath;
        }

        /// <summary>
        /// Get the path of the INI file being used
        /// </summary>
        public string INIPath
        {
            get
            {
                return path;
            }
        }

        /// <summary>
        /// Write Data to the INI File
        /// </summary>
        /// 
        /// <param name="section">The section of the INI file to add the variable to</param>
        /// <param name="key">The key of the variable</param>
        /// <param name="value">The value of the variable</param>
        public void IniWriteValue(string section, string key, string value)
        {
            WritePrivateProfileString(
                section,
                key,
                value,
                path);
        }

        /// <summary>
        /// Read Data Value From the Ini File
        /// </summary>
        /// 
        /// <param name="Section">The section of the INI file to read the variable from</param>
        /// <param name="key">The key of the variable</param>
        /// <param name="value">The value of the variable</param>
        /// <returns></returns>
        public string IniReadValue(string section, string key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(
                section,
                key,
                "",
                temp,
                255,
                path);
            return temp.ToString();
        }
    }
}
