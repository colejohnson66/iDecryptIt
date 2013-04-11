using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace Hexware.Programs.iDecryptIt
{
    internal static class Program
    {
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
                else if (args[i].Length > 4 && args[i].Substring(args[i].Length - 4) == ".dmg")
                {
                    GlobalVars.ExecutionArgs["dmg"] = args[i];
                }
                else if (args[i].Length > 5 && args[i].Substring(args[i].Length - 5) == ".ipsw")
                {
                    GlobalVars.ExecutionArgs["dmg"] = args[i];
                }
            }
            if (!console)
            {
                // MainWindow.MainWindow() will FreeConsole()
                Console.WriteLine("Loading...");
                MainWindow.debug = debug;
                App.Main();
                return;
            }

            // Console Version
            Console.WriteLine("This feature is incomplete and may not work as expected or at all.");
            Thread.Sleep(500);
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
                select = new ConsoleKeyInfo('\0', ConsoleKey.PrintScreen, false, false, false);
                while (Char.IsControl(select.KeyChar))
                {
                    // don't display is just in case it is a control key
                    select = Console.ReadKey(true);
                }
                Console.WriteLine(select.KeyChar); // finish off line
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
            string input;
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
            input = Console.ReadLine();
            Console.WriteLine("Enter the build:");
            Console.Write("Input: ");
            input = input + "_" + Console.ReadLine();

            // Key stream
            Stream stream = GetStream(input);
        }
        internal static void About()
        {
            DateTime buildTime = RetrieveLinkerTimestamp();
            Console.WriteLine("iDecryptIt " + GlobalVars.Version);
            Console.WriteLine("  Copyright (c) Hexware, LLC");
            Console.WriteLine("  Built on " + buildTime.ToString("R"));
            Console.WriteLine();
            Console.WriteLine("iDecryptIt is free software licensed under GNU GPL v3 with portions under other licenses:");
            Console.WriteLine("  7 Zip --------------- GNU LGPL v2.1 with portions under unRAR restriction");
            Console.WriteLine("  Hexware.Plist ------- GNU LGPL v3");
            //Console.WriteLine("  Ionic.Zip ----------- Ms-Pl with portions under Apache and a BSD-like license");
            //Console.WriteLine("  WPFToolkit.Extended - Ms-Pl");
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
            const int c_PeHeaderOffset = 60;
            const int c_LinkerTimestampOffset = 8;
            byte[] b = new byte[2048];
            System.IO.Stream s = null;

            try
            {
                s = new FileStream(Assembly.GetCallingAssembly().Location, FileMode.Open, FileAccess.Read);
                s.Read(b, 0, 2048);
            }
            catch (Exception)
            {
            }
            finally
            {
                if (s != null)
                {
                    s.Close();
                }
            }

            int i = BitConverter.ToInt32(b, c_PeHeaderOffset);
            int secondsSince1970 = BitConverter.ToInt32(b, i + c_LinkerTimestampOffset);
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0);
            dt = dt.AddSeconds(secondsSince1970);
            dt = dt.AddHours(TimeZone.CurrentTimeZone.GetUtcOffset(dt).Hours);
            return dt;
        }
        internal static Stream GetStream(string resourceName)
        {
            try
            {
                Assembly assy = Assembly.GetExecutingAssembly();
                string[] resources = assy.GetManifestResourceNames();
                int length = resources.Length;
                for (int i = 0; i < length; i++)
                {
                    if (resources[i].ToLower().IndexOf(resourceName.ToLower()) != -1)
                    {
                        // resource found
                        return assy.GetManifestResourceStream(resources[i]);
                    }
                }
            }
            catch (Exception)
            {
            }
            return Stream.Null;
        }
    }
}