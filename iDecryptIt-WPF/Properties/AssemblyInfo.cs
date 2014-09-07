/* =============================================================================
 * File:   AssemblyInfo.cs
 * Author: Cole Johnson
 * =============================================================================
 * Copyright (c) 2010-2014 Cole Johnson
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
using System.Runtime.InteropServices;
using System.Resources;

[assembly: AssemblyTitle("iDecryptIt")]
[assembly: AssemblyDescription("iOS firmware tools")]
[assembly: AssemblyCompany("Cole Johnson")]
[assembly: AssemblyProduct("iDecryptIt")]
[assembly: AssemblyCopyright("Copyright (C) 2010-2014")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: ComVisible(true)]
[assembly: Guid("ba12c942-94e7-4c25-b4b9-29c3e37dbf14")]

[assembly: AssemblyVersion("7.04.0.1424")]
[assembly: AssemblyFileVersion("7.04.0.1424")]
[assembly: NeutralResourcesLanguageAttribute("en-US")]
