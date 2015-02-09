/* =============================================================================
 * File:   KeyGridItem.cs
 * Author: Cole Johnson
 * =============================================================================
 * Copyright (c) 2015 Cole Johnson
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
using System.Windows.Controls;

namespace Hexware.Programs.iDecryptIt
{
    internal class KeyGridItem
    {
        internal Grid EncryptedGrid;
        internal TextBox EncryptedIV;
        internal TextBox EncryptedKey;
        internal TextBox EncryptedFileName;

        internal Grid NotEncryptedGrid;
        internal TextBox NotEncryptedFileName;

        internal KeyGridItem(Grid encryptedGrid, TextBox encryptedIV, TextBox encryptedKey,
            TextBox encryptedFileName, Grid notEncryptedGrid, TextBox notEncryptedFileName)
        {
            EncryptedGrid = encryptedGrid;
            EncryptedIV = encryptedIV;
            EncryptedKey = encryptedKey;
            EncryptedFileName = encryptedFileName;

            NotEncryptedGrid = notEncryptedGrid;
            NotEncryptedFileName = notEncryptedFileName;
        }
    }
}