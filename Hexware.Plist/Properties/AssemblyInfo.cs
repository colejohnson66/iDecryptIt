/* =============================================================================
 * File:   AssemblyInfo.cs
 * Author: Cole Johnson
 * =============================================================================
 * Copyright (c) 2012, 2014-2016 Cole Johnson
 * 
 * This file is part of Hexware.Plist
 * 
 * Hexware.Plist is free software: you can redistribute it and/or modify it
 *   under the terms of the GNU Lesser General Public License as published by
 *   the Free Software Foundation, either version 3 of the License, or (at your
 *   option) any later version.
 * 
 * Hexware.Plist is distributed in the hope that it will be useful, but WITHOUT
 *   ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 *   FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public
 *   License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public License
 *   along with Hexware.Plist. If not, see <http://www.gnu.org/licenses/>.
 * =============================================================================
 */
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("Hexware.Plist")]
[assembly: AssemblyDescription("Hexware.Plist")]
[assembly: AssemblyCompany("Hexware")]
[assembly: AssemblyProduct("Hexware.Plist")]
[assembly: AssemblyCopyright("Copyright (c) 2012, 2014-2016 Cole Johnson")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: ComVisible(true)]
[assembly: Guid("8d367e2e-1afe-48d0-b2aa-4bb0cc65bbd3")]

// TODO: Change to 2.0 when serializing works
[assembly: AssemblyVersion("1.2.*")]
[assembly: AssemblyFileVersion("1.2.1.0")]
[assembly: NeutralResourcesLanguage("en-US")]