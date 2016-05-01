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
            /*Firmware.Apple8900Stream logo = new Firmware.Apple8900Stream(System.IO.File.OpenRead(
                @"C:\test\iPhone1,1_1.0_1A543a_Restore\Firmware\all_flash\all_flash.m68ap.production\applelogo.img2"));
            Firmware.Img2Stream logo2 = new Firmware.Img2Stream(logo);
            Firmware.IBootImageStream logo3 = new Firmware.IBootImageStream(logo2);

            byte[] logoPayload = new byte[logo3.Length];
            logo3.Read(logoPayload, 0, logoPayload.Length);
            System.IO.File.WriteAllBytes(@"C:\test\dec.applelogo.bin", logoPayload);
            System.Drawing.Bitmap logoImg = new System.Drawing.Bitmap(logo3.Width, logo3.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            int pixelNum = 0;
            for (int y = 0; y < logo3.Height; y++)
            {
                for (int x = 0; x < logo3.Width; x++)
                {
                    byte pixel = logoPayload[pixelNum];
                    System.Drawing.Color color = System.Drawing.Color.FromArgb(
                        255 - logoPayload[pixelNum + 1],
                        pixel, pixel, pixel);
                    logoImg.SetPixel(x, y, color);
                    pixelNum += 2;
                }
            }
            logoImg.Save(@"C:\test\dec.applelogo.bmp");*/

            //Firmware.Apple8900Stream krnl = new Firmware.Apple8900Stream(System.IO.File.OpenRead(
            //    @"C:\test\iPhone1,1_1.0_1A543a_Restore\kernelcache.restore.release.s5l8900xrb"));
            //Firmware.CompStream krnl2 = new Firmware.CompStream(krnl);
            //byte[] krnlPayload = new byte[krnl2.Length];
            //krnl2.Read(krnlPayload, 0, krnlPayload.Length);
            //System.IO.File.WriteAllBytes(@"C:\test\dec.kernelcache.bin", krnlPayload);

            //System.IO.FileStream stream = new System.IO.FileStream(@"E:\iDecryptIt\trunk\iDecryptIt-WPF\keys\keys.tar", System.IO.FileMode.Open);
            //Firmware.TarFile tar = new Firmware.TarFile(stream);
            //System.IO.MemoryStream memStream = tar.GetFile("AppleTV2,1_8C150.plist");

            Globals.Init();
            PrintLicense();

            for (int i = 0; i < args.Length; i++) {
                if (args[i] == "/d" || args[i] == "/debug") {
                    Globals.Debug = true;
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
            Console.WriteLine("iDecryptIt " + Globals.Version + Globals.Version64);
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
            Console.WriteLine();
        }
    }
}