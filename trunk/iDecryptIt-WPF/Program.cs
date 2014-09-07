/* =============================================================================
 * File:   Program.cs
 * Author: Cole Johnson
 * =============================================================================
 * Copyright (c) 2012-2014, Cole Johnson
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
            bool console = false;
            bool debug = false;
            for (int i = 0; i < args.Length; i++) {
                if (args[i] == "/console") {
                    //console = true;
                } else if (args[i] == "/debug") {
                    debug = true;
                } else if (args[i] == "/version") {
                    PrintLicense();
                    return;
                } else if (args[i].Length > 4 && args[i].Substring(args[i].Length - 4) == ".dmg") {
                    if (GlobalVars.ExecutionArgs.ContainsKey("dmg"))
                        GlobalVars.ExecutionArgs["dmg"] = args[i];
                    else
                        GlobalVars.ExecutionArgs.Add("dmg", args[i]);
                }
            }

            GlobalVars.Init(args);

            if (!console) {
                Console.WriteLine("Loading...");
                MainWindow.debug = debug;
                Thread.Sleep(500);
                App.Main(); // returns on close
                return;
            }

            // Console Version
            Console.WriteLine("This feature is incomplete and may not work as expected or at all.");
            Thread.Sleep(500);
            Console.WriteLine("===============================================================================");
            Console.WriteLine("  iDecryptIt " + GlobalVars.Version + GlobalVars.Version64 + " by Hexware");
            Console.WriteLine("===============================================================================");
            bool exit = false;
            ConsoleKeyInfo select;
            while (!exit) {
                Console.WriteLine();
                Console.WriteLine("What would you like to do?");
                Console.WriteLine("  d - Decrypt a firmware");
                Console.WriteLine("  e - Extract a firmware");
                Console.WriteLine("  k - View keys for a firmware");
                Console.WriteLine("  a - About iDecryptIt");
                Console.WriteLine("  r - View ReadMe");
                Console.WriteLine("  c - View Changelog");
                Console.WriteLine("  x - Exit");
                Console.Write("Input: ");
                select = new ConsoleKeyInfo('\0', ConsoleKey.PrintScreen, false, false, false);
                while (Char.IsControl(select.KeyChar)) {
                    // don't display is just in case it is a control key
                    select = Console.ReadKey(true);
                }
                Console.WriteLine(select.KeyChar); // finish off line
                Console.WriteLine(); // then add the blank one
                Thread.Sleep(250); // make it look like we're working
                Console.WriteLine("===============================================================================");
                if (select.Key == ConsoleKey.D) {
                    Decrypt();
                } else if (select.Key == ConsoleKey.E) {
                    Extract();
                } else if (select.Key == ConsoleKey.K) {
                    Keys();
                } else if (select.Key == ConsoleKey.A) {
                    About();
                } else if (select.Key == ConsoleKey.R) {
                    Readme();
                } else if (select.Key == ConsoleKey.C) {
                    Changelog();
                } else if (select.Key == ConsoleKey.X) {
                    break;
                }
                Console.WriteLine("===============================================================================");
            }
            Console.WriteLine("Exiting...");
        }
        internal static void Decrypt()
        {
        }
        internal static void Extract()
        {
        }
        internal static void Keys()
        {
            Console.WriteLine("Enter the device:");
            Console.WriteLine("  Apple TV:");
            Console.WriteLine("    AppleTV2,1 - Apple TV 2G");
            Console.WriteLine("    AppleTV3,1 - Apple TV 3G");
            Console.WriteLine("    AppleTV3,2 - Apple TV 3G Rev A");
            Console.WriteLine("  iPad:");
            Console.WriteLine("    iPad1,1 ---- iPad 1G");
            Console.WriteLine("    iPad2,1 ---- iPad 2 Wi-Fi");
            Console.WriteLine("    iPad2,2 ---- iPad 2 GSM");
            Console.WriteLine("    iPad2,3 ---- iPad 2 CDMA");
            Console.WriteLine("    iPad2,4 ---- iPad 2 Wi-Fi Rev A");
            Console.WriteLine("    iPad3,1 ---- iPad 3 Wi-Fi");
            Console.WriteLine("    iPad3,2 ---- iPad 3 CDMA");
            Console.WriteLine("    iPad3,3 ---- iPad 3 Global");
            Console.WriteLine("    iPad3,4 ---- iPad 4 Wi-Fi");
            Console.WriteLine("    iPad3,5 ---- iPad 4 GSM");
            Console.WriteLine("    iPad3,6 ---- iPad 4 Global");
            Console.WriteLine("  iPad mini:");
            Console.WriteLine("    iPad2,5 ---- iPad mini 1G Wi-Fi");
            Console.WriteLine("    iPad2,6 ---- iPad mini 1G GSM");
            Console.WriteLine("    iPad2,7 ---- iPad mini 1G Global");
            Console.WriteLine("  iPhone:");
            Console.WriteLine("    iPhone1,1 -- iPhone 2G");
            Console.WriteLine("    iPhone1,2 -- iPhone 3G");
            Console.WriteLine("    iPhone2,1 -- iPhone 3GS");
            Console.WriteLine("    iPhone3,1 -- iPhone 4 GSM");
            Console.WriteLine("    iPhone3,2 -- iPhone 4 GSM Rev A");
            Console.WriteLine("    iPhone3,3 -- iPhone 4 CDMA");
            Console.WriteLine("    iPhone4,1 -- iPhone 4S");
            Console.WriteLine("    iPhone5,1 -- iPhone 5 GSM");
            Console.WriteLine("    iPhone5,2 -- iPhone 5 Global");
            Console.WriteLine("  iPod touch:");
            Console.WriteLine("    iPod1,1 ---- iPod touch 1G");
            Console.WriteLine("    iPod2,1 ---- iPod touch 2G");
            Console.WriteLine("    iPod3,1 ---- iPod touch 3G");
            Console.WriteLine("    iPod4,1 ---- iPod touch 4G");
            Console.WriteLine("    iPod5,1 ---- iPod touch 5G");
            Console.Write("Input: ");
            string input = Console.ReadLine();
            Console.WriteLine("Enter the build:");
            Console.Write("Input: ");
            input = input + "_" + Console.ReadLine();
        }
        internal static void About()
        {
            Console.WriteLine("iDecryptIt " + GlobalVars.Version + GlobalVars.Version64);
            Console.WriteLine("  Copyright (c) 2010-2014 Cole Johnson");
            Console.WriteLine("  Built on " + GlobalVars.CompileTimestamp.ToString("R"));
            Console.WriteLine();
            Console.WriteLine("iDecryptIt is free software licensed under GNU GPL v3 with portions under other licenses:");
            Console.WriteLine("  7-zip --------- GNU LGPL v2.1 with portions under unRAR restriction");
            Console.WriteLine("  Hexware.Plist - GNU LGPL v3");
            Console.WriteLine("  xpwn dmg ------ GNU GPL v3");
        }
        internal static void Readme()
        {
        }
        internal static void Changelog()
        {
        }

        private static void PrintLicense()
        {
            Console.WriteLine("iDecryptIt " + GlobalVars.Version + GlobalVars.Version64);
            Console.WriteLine("Copyright (c) 2010-2014, Cole Johnson");
            Console.WriteLine();

            Console.Write("iDecryptIt is free software: you can redistribute it and/or modify it under ");
            Console.Write("the terms of the GNU General Public License as published by the Free ");
            Console.Write("Software Foundation, either version 3 of the License, or (at your option) ");
            Console.WriteLine("any later version.");
            Console.WriteLine();

            Console.Write("iDecryptIt is distributed in the hope that it will be useful, but WITHOUT ");
            Console.Write("ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or ");
            Console.Write("FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for ");
            Console.WriteLine("more details.");
            Console.WriteLine();

            Console.Write("You should have recieved a copy of the GNU General Public License along with ");
            Console.WriteLine("iDecryptIt. If not, see <http://www.gnu.org/licenses/>.");
            Console.WriteLine();
        }
    }
}