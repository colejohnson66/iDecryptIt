using System;
using System.Runtime.InteropServices;

namespace Hexware.Programs.iDecryptIt
{
    internal class Program
    {
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        private static extern bool FreeConsole();

        internal static bool debug = false;

        [STAThread]
        internal static void Main(string[] args)
        {
            // Initialization logic
            bool console = false;
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "/console")
                {
                    console = true;
                }
                else if (args[i] == "/debug")
                {
                    debug = true;
                }
                else if (args[i].Length > 5 && args[i].Substring(args[i].Length - 4) == ".dmg")
                {
                    GlobalVars.ExecutionArgs["dmg"] = args[i];
                }
                else if (args[i].Length > 6 && args[i].Substring(args[i].Length - 5) == ".ipsw")
                {
                    GlobalVars.ExecutionArgs["dmg"] = args[i];
                }
            }
            if (!console)
            {
                if (!debug)
                {
                    FreeConsole();
                }
                App.Main();
                return;
            }

            // Console Version
            Console.WriteLine("===============================================================================");
            Console.WriteLine("  iDecryptIt " + GlobalVars.Version + " by Hexware, LLC");
            Console.WriteLine("===============================================================================");
            bool exit = false;
            ConsoleKeyInfo select;
            while (!exit)
            {
                Console.WriteLine();
                Console.WriteLine("What would you like to do?");
                Console.WriteLine("  d - Decrypt a firmware");
                Console.WriteLine("  e - Extract a firmware");
                Console.WriteLine("  k - View keys for a firmware");
                Console.WriteLine("  a - View \"About iDecryptIt\"");
                Console.WriteLine("  r - View \"README\"");
                Console.WriteLine("  c - What's New?");
                Console.WriteLine("  s - Submit keys");
                Console.WriteLine("  x - Exit");
                Console.Write("Input: ");
                select = Console.ReadKey();
                Console.WriteLine(); // finish off line
                Console.WriteLine(); // then add the blank one
                Console.WriteLine("===============================================================================");
                if (select.Key == ConsoleKey.D)
                {
                    Decrypt();
                }
                else if (select.Key == ConsoleKey.E)
                {
                    Extract();
                }
                else if (select.Key == ConsoleKey.K)
                {
                    Keys();
                }
                else if (select.Key == ConsoleKey.A)
                {
                    About();
                }
                else if (select.Key == ConsoleKey.R)
                {
                    Readme();
                }
                else if (select.Key == ConsoleKey.C)
                {
                    Changelog();
                }
                else if (select.Key == ConsoleKey.S)
                {
                    Submit();
                }
                else if (select.Key == ConsoleKey.X)
                {
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
        }
        internal static void About()
        {
            DateTime buildTime = RetrieveLinkerTimestamp();
            Console.WriteLine("iDecryptIt " + GlobalVars.Version);
            Console.WriteLine("  Copyright (c) Hexware, LLC");
            Console.WriteLine("  Built on {0:R}", buildTime);
            Console.WriteLine();
            Console.WriteLine("iDecryptIt is free software licensed under GNU GPL v3 with portions under other licenses:");
            Console.WriteLine("  7 Zip---------------- GNU LGPL v2.1 with portions under unRAR restriction");
            Console.WriteLine("  Hexware.Plist ------- GNU LGPL v3");
            Console.WriteLine("  Ionic.Zip ----------- Ms-Pl with portions under Apache and a BSD-like license");
            Console.WriteLine("  WPFToolkit.Extended - Ms-Pl");
            Console.WriteLine("  xpwn dmg ------------ GNU GPL v3");
        }
        internal static void Readme()
        {
        }
        internal static void Changelog()
        {
        }
        internal static void Submit()
        {
        }
        internal static DateTime RetrieveLinkerTimestamp()
        {
            string filePath = System.Reflection.Assembly.GetCallingAssembly().Location;
            const int c_PeHeaderOffset = 60;
            const int c_LinkerTimestampOffset = 8;
            byte[] b = new byte[2048];
            System.IO.Stream s = null;

            try
            {
                s = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                s.Read(b, 0, 2048);
            }
            finally
            {
                if (s != null)
                {
                    s.Close();
                }
            }

            int i = System.BitConverter.ToInt32(b, c_PeHeaderOffset);
            int secondsSince1970 = System.BitConverter.ToInt32(b, i + c_LinkerTimestampOffset);
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0);
            dt = dt.AddSeconds(secondsSince1970);
            dt = dt.AddHours(TimeZone.CurrentTimeZone.GetUtcOffset(dt).Hours);
            return dt;
        }
    }
}