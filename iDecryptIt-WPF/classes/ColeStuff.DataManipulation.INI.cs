using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace ColeStuff.DataManipulation
{
    /// <summary>
    /// INI file manipulation class
    /// </summary>
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
        /// <param name="INIPath">The full path of the INI file</param>
        /// <exception cref="System.IO.FileNotFoundException">The file was not found</exception>
        public INI(string INIPath)
        {
            if (!File.Exists(INIPath))
            {
                throw new FileNotFoundException("The specified file: \"" + INIPath + "\" was not found", INIPath);
            }
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
        /// <param name="section">The section of the INI file to add the variable to</param>
        /// <param name="key">The key of the variable</param>
        /// <param name="value">The value of the variable</param>
        /// <returns>The return code of writing the value</returns>
        public long IniWriteValue(string section, string key, string value)
        {
            return WritePrivateProfileString(
                section,
                key,
                value,
                path);
        }

        /// <summary>
        /// Read data value from the Ini File
        /// </summary>
        /// <param name="section">The section of the INI file to read the variable from</param>
        /// <param name="key">The key of the variable</param>
        /// <returns>The value of the specified key in the specified section</returns>
        public string IniReadValue(string section, string key)
        {
            StringBuilder temp = new StringBuilder(255);
            GetPrivateProfileString(
                section,
                key,
                "",
                temp,
                255,
                path);
            return temp.ToString();
        }

        /// <summary>
        /// Get the contents of the INI file
        /// </summary>
        /// <exception cref="System.IO.IOException">Error reading INI file</exception>
        /// <exception cref="System.OutOfMemoryException">Insufficient memory</exception>
        /// <returns>The contents of the INI file</returns>
        public override string ToString()
        {
            try
            {
                StreamReader file = new StreamReader(path);
                return file.ReadToEnd();
            }
            catch (IOException ex)
            {
                throw new IOException("Error reading INI File", ex);
            }
            catch (OutOfMemoryException ex)
            {
                throw new OutOfMemoryException("Insufficient memory", ex);
            }
        }

        /// <summary>
        /// Get a 32-bit integer hash of the contents of the INI file
        /// </summary>
        /// <exception cref="System.IO.IOException">Error reading INI file</exception>
        /// <exception cref="System.OutOfMemoryException">Insufficient memory</exception>
        /// <returns>32-bit integer hash of INI file contents</returns>
        public override int GetHashCode()
        {
            try
            {
                return ToString().GetHashCode();
            }
            catch (IOException ex)
            {
                throw new IOException("Error reading INI file", ex);
            }
            catch (OutOfMemoryException ex)
            {
                throw new OutOfMemoryException("Insufficient memory", ex);
            }
        }

        /// <summary>
        /// Determine if a System.Object and ColeStuff.DataManipulation.INI classes are the same
        /// </summary>
        /// <param name="obj">The object used for comparison</param>
        /// <returns>true if same; else false</returns>
        public override bool Equals(object obj)
        {
            // If hashes equal, then it is equal
            return obj.GetHashCode() == GetHashCode();
        }

        /// <summary>
        /// Determine if two ColeStuff.DataManipulation.INI classes are the same
        /// </summary>
        /// <param name="obj">The INI file used for comparison</param>
        /// <returns>true if same; else false</returns>
        public bool Equals(INI obj)
        {
            // If paths equal, then it is equal
            return obj.INIPath == INIPath;
        }
    }
}
