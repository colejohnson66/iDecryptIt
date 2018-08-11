﻿/* =============================================================================
 * File:   GlobalVars.cs
 * Author: Hexware
 * =============================================================================
 * Copyright (c) 2011-2013, Hexware
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 *   along with this program. If not, see <http://www.gnu.org/licenses/>.
 * =============================================================================
 */
using System.Collections.Generic;

namespace Hexware.Programs.iDecryptIt
{
    internal static class GlobalVars
    {
        internal const string Version = "7.02.0.2C60";
        internal static Dictionary<string, object> ExecutionArgs = new Dictionary<string, object>();
    }
}