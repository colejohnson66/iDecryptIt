/* =============================================================================
 * File:   DeviceConverters.cs
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

using Avalonia;
using Avalonia.Data.Converters;
using iDecryptIt.Shared;
using System;
using System.Globalization;

namespace iDecryptIt.Converters;

public class DeviceGroupConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not DeviceGroup group)
            return AvaloniaProperty.UnsetValue;

        return group switch
        {
            DeviceGroup.AppleWatch => "Apple Watch",
            DeviceGroup.AppleTV => "Apple TV",
            DeviceGroup.AudioAccessory => "HomePod",
            DeviceGroup.IBridge => "iBridge",
            DeviceGroup.IPad => "iPad",
            DeviceGroup.IPadAir => "iPad Air",
            DeviceGroup.IPadMini => "iPad mini",
            DeviceGroup.IPadPro => "iPad Pro",
            DeviceGroup.IPhone => "iPhone",
            DeviceGroup.IPodTouch => "iPod touch",
            _ => AvaloniaProperty.UnsetValue,
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        AvaloniaProperty.UnsetValue;
}
