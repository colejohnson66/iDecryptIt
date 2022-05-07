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

namespace iDecryptIt.Models;

/* Technically, this model is not needed; all information could be displayed using a `HasKeysEntry` object and
 *   converters. However, this is not performant. With some devices having over 100 builds, the <ComboBox> can take 5-10
 *   seconds (or more) to open *each time*. dotTrace suggests this is related to the multitude of converter calls.
 * Replacing that method with a wrapper model brings the opening time to less than a second. Hence, this class.
 */
public record VKBuildModel(
    bool HasKeys,
    string Build,
    SolidColorBrush TextColor,
    string VersionText)
{
    private static readonly SolidColorBrush BLACK = new(Colors.Black);
    private static readonly SolidColorBrush RED = new(Colors.Red);

    public static VKBuildModel FromHasKeysEntry(HasKeysEntry hkEntry) =>
        new(hkEntry.HasKeys, hkEntry.Build, hkEntry.HasKeys ? BLACK : RED, $"{hkEntry.Version} ({hkEntry.Build})");
}
