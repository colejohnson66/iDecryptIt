/* =============================================================================
 * File:   PlistFormat.cs
 * Author: Cole Johnson
 * =============================================================================
 * Copyright (c) 2012 Cole Johnson
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
namespace Hexware.Plist
{
    /// <summary>
    /// Enumeration of Plist Formats
    /// </summary>
    public enum PlistFormat
    {
        /// <summary>
        /// Represents a binary Plist
        /// </summary>
        Binary,
		
		/// <summary>
		/// Represents a Json Plist
		/// </summary>
		Json,

        /// <summary>
        /// Represents an Xml Plist
        /// </summary>
        Xml
    }
}