/* =============================================================================
 * File:   Program.cs
 * Author: Cole Johnson
 * =============================================================================
 * Copyright (c) 2014, Cole Johnson
 * 
 * This program is free software: you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation, either version 3 of the License, or
 *   (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 *   along with this program. If not, see <http://www.gnu.org/licenses/>.
 * =============================================================================
 */
using System;
using System.IO;
using Xpwn.Shared;

namespace Xpwn.Dmg
{
    class Program
    {
        static void Main(string[] args)
        {
            FileStream inFile;
            FileStream outFile;
            bool hasKey = false;

            if (args.Length < 3)
            {
                Console.WriteLine("Usage: dmg [extract|build|build2048|res|iso|dmg] <in> <out> [-k <key>] [<partition>]");
                return;
            }

            try
            {
                inFile = File.Open(args[1], FileMode.Open, FileAccess.Read);
            }
            catch (Exception)
            {
                Console.WriteLine("Cannot open source: {0}", args[1]);
                return;
            }

            try
            {
                outFile = File.Open(args[2], FileMode.Open, FileAccess.Write);
            }
            catch (Exception)
            {
                Console.WriteLine("Cannot open destination: {0}", args[2]);
                return;
            }

            if (args.Length > 5 && args[4] == "-k")
            {
                //in = createAbstractFileFromFileVault(in, argv[5]);
                hasKey = true;
            }

            if (args[0] == "extract")
            {
                int partition = -1;
                if (hasKey)
                {
                    partition = Convert.ToInt32(args[5]);
                }
                else
                {
                    partition = Convert.ToInt32(args[3]);
                }
                //extractDmg(in, out, partNum);
            }
            else if (args[0] == "build")
            {
                //buildDmg(in, out, SECTOR_SIZE);
                // SECTOR_SIZE == Xpwn.Dmg.Common.SectorSize
            }
            else if (args[0] == "build2048")
            {
                //buildDmg(in, out, 2048);
            }
            else if (args[0] == "res")
            {
                //outResources(in, out);
            }
            else if (args[0] == "iso")
            {
                //convertToISO(in, out);
            }
            else if (args[0] == "dmg")
            {
                //convertToDMG(in, out);
            }
            else
            {
                Console.WriteLine("Unknown command: {0}", args[0]);
            }
        }
    }
}