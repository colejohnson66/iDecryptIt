/* =============================================================================
 * File:   Program.cs
 * Author: Cole Johnson
 * =============================================================================
 * Copyright (c) 2012-2016 Cole Johnson
 * 
 * This file is part of iDecryptIt
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
using System;
using System.Threading;

namespace Hexware.Programs.iDecryptIt
{
    internal static class Program
    {
        [STAThread]
        internal static void Main(string[] args)
        {
            //Firmware.Apple8900File file = new Firmware.Apple8900File(System.IO.File.OpenRead(
            //    @"C:\iPod1,1_1.1.5_4B1_Restore\Firmware\all_flash\all_flash.n45ap.production\applelogo.img2"));
            //byte[] payload = file.GetPayload();
            //System.IO.File.WriteAllBytes(@"C:\iPod1,1_1.1.5_4B1_Restore\applelogo.dec.img2", payload);

            Globals.Init();
            Console.WriteLine("iDecryptIt " + Globals.Version + Globals.Version64);
            
            for (int i = 0; i < args.Length; i++) {
                if (args[i] == "/d" || args[i] == "/debug") {
                    Globals.Debug = true;
                } else if (args[i] == "/v" || args[i] == "/version") {
                    PrintLicense();
                    return;
                } else if (args[i].Length > 4 && args[i].Substring(args[i].Length - 4) == ".dmg") {
                    if (Globals.ExecutionArgs.ContainsKey("dmg"))
                        Globals.ExecutionArgs["dmg"] = args[i];
                    else
                        Globals.ExecutionArgs.Add("dmg", args[i]);
                }
            }

            Console.WriteLine("Loading...");
            Thread.Sleep(500);
            App.Main();
        }

        private static void PrintLicense()
        {
            Console.WriteLine("Copyright (c) 2010-2016 Cole Johnson");
            Console.WriteLine();
            Console.WriteLine("iDecryptIt is free software: you can redistribute it and/or modify it under");
            Console.WriteLine("  the terms of the GNU General Public License as published by the Free");
            Console.WriteLine("  Software Foundation, either version 3 of the License, or (at your option)");
            Console.WriteLine("  any later version.");
            Console.WriteLine();
            Console.WriteLine("iDecryptIt is distributed in the hope that it will be useful, but WITHOUT");
            Console.WriteLine("  ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or");
            Console.WriteLine("  FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for");
            Console.WriteLine("  more details.");
            Console.WriteLine();
            Console.WriteLine("You should have received a copy of the GNU General Public License along with");
            Console.WriteLine("  iDecryptIt. If not, see <http://www.gnu.org/licenses/>.");
        }
    }
}