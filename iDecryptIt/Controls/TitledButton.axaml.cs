using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using System.Windows.Input;

namespace iDecryptIt.Controls;

public class TitledButton : TemplatedControl
{
    public static readonly StyledProperty<ICommand> CommandProperty =
        AvaloniaProperty.Register<TitledButton, ICommand>(nameof(Command));
    public ICommand Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly StyledProperty<IImage> ImageProperty =
        AvaloniaProperty.Register<TitledButton, IImage>(nameof(Image));
    public IImage Image
    {
        get => GetValue(ImageProperty);
        set => SetValue(ImageProperty, value);
    }

    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<TitledButton, string>(nameof(Text));
    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
}
