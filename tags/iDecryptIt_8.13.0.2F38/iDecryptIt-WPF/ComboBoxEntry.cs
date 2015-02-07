/* =============================================================================
 * File:   ComboBoxEntry.cs
 * Author: Cole Johnson
 * =============================================================================
 * Copyright (c) 2014, Cole Johnson
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
namespace Hexware.Programs.iDecryptIt
{
    public class ComboBoxEntry
    {
        public ComboBoxEntry(string id, string value)
        {
            ID = id;
            Value = value;
        }

        public string ID { get; set; }

        public string Value { get; set; }
    }
}
