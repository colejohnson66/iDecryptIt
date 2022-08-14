/* =============================================================================
 * File:   HfsFileSystem.cs
 * Author: Cole Tobin
 * =============================================================================
 * Copyright (c) 2022 Cole Tobin
 *
 * This file is part of iDecryptIt.
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

using iDecryptIt.IO.Formats;

namespace iDecryptIt.IO.FileSystem;

public class HfsFileSystem : FileSystemReaderBase
{
    private readonly DmgFile _file;
    private readonly bool _hfsx;

    internal HfsFileSystem(DmgFile file, bool isX)
    {
        _file = file;
        _hfsx = isX;
    }
}
