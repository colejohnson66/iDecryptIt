/* =============================================================================
 * File:   PlistFormatException.cs
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
using System;
using System.Runtime.Serialization;
using System.Security;

namespace Hexware.Plist
{
    /// <summary>
    /// Plist Format Exception
    /// </summary>
    public class PlistFormatException : PlistException
    {
        /// <summary>
        /// Initializes a new instance of the Hexware.Audio.AudioFormatException class with default parameters.
        /// </summary>
        public PlistFormatException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the Hexware.Audio.AudioFormatException class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public PlistFormatException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Hexware.Audio.AudioFormatException class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the inner parameter is not null, the current exception is raised in a catch block that handles the inner exception.</param>
        public PlistFormatException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Hexware.Audio.AudioFormatException class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        [SecuritySafeCritical]
        protected PlistFormatException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
