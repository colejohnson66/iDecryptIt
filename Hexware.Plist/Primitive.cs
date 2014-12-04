/* =============================================================================
 * File:   Primitive.cs
 * Author: Cole Johnson
 * =============================================================================
 * Copyright (c) 2012, 2014 Cole Johnson
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
    /// Enumeration of Primitives
    /// </summary>
    public enum Primitive
    {
        /// <summary>
        /// &lt;true /&gt; or &lt;false /&gt; represents a <see cref="System.Boolean"/> value
        /// </summary>
        Bool = 101,

        /// <summary>
        /// &lt;data /&gt; represents a base64-encoded <see cref="System.String"/>
        /// </summary>
        Data,

        /// <summary>
        /// &lt;array /&gt; represents a <see cref="System.DateTime"/>
        /// </summary>
        Date,

        /// <summary>
        /// &lt;fill /&gt; represents a fill element
        /// </summary>
        Fill,

        /// <summary>
        /// &lt;integer /&gt; represents a 64-bit signed integer (<see cref="System.Int64"/>)
        /// </summary>
        Integer,

        /// <summary>
        /// &lt;null /&gt; represents a <c>null</c> value
        /// </summary>
        Null,

        /// <summary>
        /// &lt;real /&gt; represents a <see cref="System.Double"/>
        /// </summary>
        Real,

        /// <summary>
        /// TO BE IMPLEMENTED
        /// </summary>
        Set,

        /// <summary>
        /// &lt;string /&gt; represents a <see cref="System.String"/> encoded with UTF8 (ASCII)
        /// </summary>
        String,

        /// <summary>
        /// &lt;uid /&gt; represents a <see cref="System.Byte"/> array with a maximum length of 16 (0x10)
        /// </summary>
        Uid,

        /// <summary>
        /// &lt;ustring /&gt; represents a <see cref="System.String"/> encoded with UTF16 (Unicode)
        /// </summary>
        UString
    }
}