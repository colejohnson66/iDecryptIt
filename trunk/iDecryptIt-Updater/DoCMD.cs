using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace iDecryptIt_Updater
{
    class Run
    {
        public void DoCMD(string prog)
        {
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.FileName = prog;
            p.StartInfo.Arguments = "";
            p.Start();
            p.WaitForExit();
        }
        public void DoCMD(string prog, bool wait)
        {
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.FileName = prog;
            p.StartInfo.Arguments = "";
            p.Start();
            if (wait)
            {
                p.WaitForExit();
            }
        }
        public void DoCMD(string prog, string args)
        {
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.FileName = prog;
            p.StartInfo.Arguments = args;
            p.Start();
            p.WaitForExit();
        }
        public void DoCMD(string prog, string args, bool wait)
        {
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.FileName = prog;
            p.StartInfo.Arguments = args;
            p.Start();
            if (wait)
            {
                p.WaitForExit();
            }
        }
    }
}
