/* =============================================================================
 * File:   VKBuildModel.cs
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

using Avalonia.Media;
using iDecryptIt.Shared;
using System;
using System.Diagnostics;

namespace iDecryptIt.Models;

/* Technically, this model is not needed; all information could be displayed using a `HasKeysEntry` object and
 *   converters. However, this is not performant. With some devices having over 100 builds, the <ComboBox> can take 5-10
 *   seconds (or more) to open *each time*. dotTrace suggests this is related to the multitude of converter calls.
 * Replacing that method with a wrapper model brings the opening time to less than a second. Hence, this class.
 */
public class VKBuildModel : IComparable<VKBuildModel>, IEquatable<VKBuildModel>
{
    private static readonly SolidColorBrush BLACK = new(Colors.Black);
    private static readonly SolidColorBrush RED = new(Colors.Red);

    public bool HasKeys { get; }
    public string Build { get; }
    public SolidColorBrush TextColor { get; }
    public string VersionText { get; }

    private readonly int _buildMajor;
    private readonly char _buildSep;
    private readonly int _buildMinor;
    private readonly char _buildSuffix;

    public VKBuildModel(HasKeysEntry hkEntry)
    {
        HasKeys = hkEntry.HasKeys;
        Build = hkEntry.Build;
        TextColor = hkEntry.HasKeys ? BLACK : RED;
        VersionText = $"{hkEntry.Version} ({hkEntry.Build})";

        // TODO: this doesn't handle builds with parenthesis or periods (from Apple TV)

        /* Parse out the build string according to the regex:
         * (\d+)([A-Z])(\d+)([a-z]?)
         * └─┬─┘└──┬──┘└─┬─┘└──┬───┘
         *   └─────┼─────┼─────┼───── "major"
         *         └─────┼─────┼───── "separator"
         *               └─────┼───── "minor"
         *                     └───── "suffix" (optional; default is null)
         *
         * Examples:
         * 1A453a                     4A102
         * ││└┬┘│                     ││└┬┘
         * └┼─┼─┼─ 1     major     4 ─┘│ │
         *  └─┼─┼─ A   separator   A ──┘ │
         *    └─┼─ 453   minor   102 ────┘
         *      └─ a    suffix   '\0'
         */
        string build = hkEntry.Build;
        int i = 0;

        _buildMajor = 0;
        while (build[i] is >= '0' and <= '9')
        {
            _buildMajor *= 10;
            _buildMajor += build[i] - '0';
            i++;
            Debug.Assert(i < build.Length);
        }

        _buildSep = build[i];
        Debug.Assert(_buildSep is >= 'A' and <= 'Z');
        i++;
        Debug.Assert(i < build.Length);

        _buildMinor = 0;
        while (i < build.Length && build[i] is >= '0' and <= '9')
        {
            _buildMinor *= 10;
            _buildMinor += build[i] - '0';
            i++;
        }

        _buildSuffix = '\0';
        if (i < build.Length - 1)
        {
            // the suffix is only one character
            Debug.Assert(i == build.Length - 1);
            _buildSuffix = build[i];
        }
    }

    public int CompareTo(VKBuildModel? other)
    {
        if (ReferenceEquals(this, other))
            return 0;
        if (other is null)
            return 1; // nulls come before all

        int major = _buildMajor.CompareTo(other._buildMajor);
        if (major is not 0)
            return major;

        int sep = _buildSep.CompareTo(other._buildSep);
        if (sep is not 0)
            return sep;

        int minor = _buildMinor.CompareTo(other._buildMinor);
        // ReSharper disable once ConvertIfStatementToReturnStatement
        if (minor is not 0)
            return minor;

        return _buildSuffix.CompareTo(other._buildSuffix);
    }

    public bool Equals(VKBuildModel? other)
    {
        if (ReferenceEquals(this, other))
            return true;
        if (other is null)
            return false;

        return HasKeys == other.HasKeys && VersionText == other.VersionText;
    }

    public override bool Equals(object? obj) =>
        Equals(obj as VKBuildModel);

    public override int GetHashCode() =>
        HashCode.Combine(_buildMajor, _buildSep, _buildMinor, _buildSuffix, HasKeys);
}
