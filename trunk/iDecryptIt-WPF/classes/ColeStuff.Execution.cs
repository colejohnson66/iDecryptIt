using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace ColeStuff
{
    /// <summary>
    /// Executable execution class
    /// </summary>
    public class Execution
    {
        /// <summary>
        /// Execute a program and wait for it to finish
        /// </summary>
        /// <exception cref="System.ArgumentException">The specified file is a directory</exception>
        /// <exception cref="System.IO.FileNotFoundException">The specified file was not found</exception>
        /// <exception cref="System.InvalidOperationException">The process to start was blank</exception>
        /// <exception cref="System.ComponentModel.Win32Exception">There was an error in opening the associated file</exception>
        /// <exception cref="System.ObjectDisposedException">The process object has already been disposed</exception>
        /// <param name="prog">The path of the file to execute</param>
        public static void DoCMD(string prog)
        {
            if (Directory.Exists(prog))
            {
                throw new ArgumentException("The specified file is a directory", prog);
            }
            if (!File.Exists(prog))
            {
                throw new FileNotFoundException("The specified file does not exist", prog);
            }
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.FileName = prog;
            try
            {
                p.Start();
            }
            catch (ObjectDisposedException ex)
            {
                throw new ObjectDisposedException(ex.ObjectName, ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
            catch (Win32Exception ex)
            {
                throw new Win32Exception(ex.Message, ex);
            }
            p.WaitForExit();
        }

        /// <summary>
        /// Execute a program
        /// </summary>
        /// <exception cref="System.ArgumentException">The specified file is a directory</exception>
        /// <exception cref="System.IO.FileNotFoundException">The specified file was not found</exception>
        /// <exception cref="System.InvalidOperationException">The process to start was blank</exception>
        /// <exception cref="System.ComponentModel.Win32Exception">There was an error in opening the associated file</exception>
        /// <exception cref="System.ObjectDisposedException">The process object has already been disposed</exception>
        /// <param name="prog">The path of the file to execute</param>
        /// <param name="wait">Whether to wait for the program to finish or not</param>
        public static void DoCMD(string prog, bool wait)
        {
            if (Directory.Exists(prog))
            {
                throw new ArgumentException("The specified file is a directory", prog);
            }
            if (!File.Exists(prog))
            {
                throw new FileNotFoundException("The specified file does not exist", prog);
            }
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.FileName = prog;
            try
            {
                p.Start();
            }
            catch (ObjectDisposedException ex)
            {
                throw new ObjectDisposedException(ex.ObjectName, ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
            catch (Win32Exception ex)
            {
                throw new Win32Exception(ex.Message, ex);
            }
            if (wait)
            {
                p.WaitForExit();
            }
        }

        /// <summary>
        /// Execute a program with specified arguments
        /// </summary>
        /// <exception cref="System.ArgumentException">The specified file is a directory</exception>
        /// <exception cref="System.IO.FileNotFoundException">The specified file was not found</exception>
        /// <exception cref="System.InvalidOperationException">The process to start was blank</exception>
        /// <exception cref="System.ComponentModel.Win32Exception">There was an error in opening the associated file</exception>
        /// <exception cref="System.ObjectDisposedException">The process object has already been disposed</exception>
        /// <param name="prog">The path of the file to execute</param>
        /// <param name="args">The arguments to be passed</param>
        public static void DoCMD(string prog, string args)
        {
            if (Directory.Exists(prog))
            {
                throw new ArgumentException("The specified file is a directory", prog);
            }
            if (!File.Exists(prog))
            {
                throw new FileNotFoundException("The specified file does not exist", prog);
            }
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.FileName = prog;
            p.StartInfo.Arguments = args;
            try
            {
                p.Start();
            }
            catch (ObjectDisposedException ex)
            {
                throw new ObjectDisposedException(ex.ObjectName, ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
            catch (Win32Exception ex)
            {
                throw new Win32Exception(ex.Message, ex);
            }
            p.WaitForExit();
        }

        /// <summary>
        /// Execute a program with specified arguments
        /// </summary>
        /// <exception cref="System.ArgumentException">The specified file is a directory</exception>
        /// <exception cref="System.IO.FileNotFoundException">The specified file was not found</exception>
        /// <exception cref="System.InvalidOperationException">The process to start was blank</exception>
        /// <exception cref="System.ComponentModel.Win32Exception">There was an error in opening the associated file</exception>
        /// <exception cref="System.ObjectDisposedException">The process object has already been disposed</exception>
        /// <param name="prog">The path of the file to execute</param>
        /// <param name="args">The arguments to be passed</param>
        /// <param name="wait">Whether to wait for the program to finish or not</param>
        public static void DoCMD(string prog, string args, bool wait)
        {
            if (Directory.Exists(prog))
            {
                throw new ArgumentException("The specified file is a directory", prog);
            }
            if (!File.Exists(prog))
            {
                throw new FileNotFoundException("The specified file does not exist", prog);
            }
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.FileName = prog;
            p.StartInfo.Arguments = args;
            try
            {
                p.Start();
            }
            catch (ObjectDisposedException ex)
            {
                throw new ObjectDisposedException(ex.ObjectName, ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
            catch (Win32Exception ex)
            {
                throw new Win32Exception(ex.Message, ex);
            }
            if (wait)
            {
                p.WaitForExit();
            }
        }
    }
}
