/* =============================================================================
 * File:   ValidatingTextBox.cs
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
using Avalonia.Controls.Primitives;
using ReactiveUI;
using System;

namespace iDecryptIt.Controls;

public class ValidatingTextBox : TemplatedControl
{
    public static readonly StyledProperty<bool> ErrorIconVisibleProperty =
        AvaloniaProperty.Register<ValidatingTextBox, bool>(nameof(ErrorIconVisible));
    public bool ErrorIconVisible
    {
        get => GetValue(ErrorIconVisibleProperty);
        private set => SetValue(ErrorIconVisibleProperty, value);
    }

    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<ValidatingTextBox, string>(nameof(Text), "");
    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly StyledProperty<Func<string, bool>?> ValidatorProperty =
        AvaloniaProperty.Register<ValidatingTextBox, Func<string, bool>?>(nameof(Validator));
    public Func<string, bool>? Validator
    {
        get => GetValue(ValidatorProperty);
        set => SetValue(ValidatorProperty, value);
    }

    public static readonly StyledProperty<string?> ValidatorErrorMessageProperty =
        AvaloniaProperty.Register<ValidatingTextBox, string?>(nameof(ValidatorErrorMessage));
    public string? ValidatorErrorMessage
    {
        get => GetValue(ValidatorErrorMessageProperty);
        set => SetValue(ValidatorErrorMessageProperty, value);
    }

    public static readonly StyledProperty<string> WatermarkProperty =
        AvaloniaProperty.Register<ValidatingTextBox, string>(nameof(Watermark), "");
    public string Watermark
    {
        get => GetValue(WatermarkProperty);
        set => SetValue(WatermarkProperty, value);
    }

    public ValidatingTextBox()
    {
        this.WhenAnyValue(control => control.Text)
            .Subscribe(
                value =>
                {
                    if (Validator is null || string.IsNullOrWhiteSpace(value))
                        ErrorIconVisible = false;
                    else
                        ErrorIconVisible = !Validator(value);
                });
    }
}
