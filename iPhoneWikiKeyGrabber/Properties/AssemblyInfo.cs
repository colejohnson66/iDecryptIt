/* =============================================================================
 * File:   AssemblyInfo.cs
 * Author: Cole Johnson
 * =============================================================================
 * Copyright (c) 2012-2015, Cole Johnson
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
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("iPhoneWikiKeyGrabber")]
[assembly: AssemblyDescription("Grab decryption keys from The iPhone Wiki")]
[assembly: AssemblyCompany("Hexware")]
[assembly: AssemblyProduct("iPhoneWikiKeyGrabber")]
[assembly: AssemblyCopyright("Copyright (C) 2012-2015")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: ComVisible(false)]
[assembly: Guid("25e3f6af-4b61-4250-aaac-2f894698acbf")]

[assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyFileVersion("1.0.0.0")]
