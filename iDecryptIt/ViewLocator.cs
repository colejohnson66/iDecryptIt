using Avalonia.Controls;
using Avalonia.Controls.Templates;
using iDecryptIt.ViewModels;
using System;

namespace iDecryptIt;

public class ViewLocator : IDataTemplate
{
    public IControl Build(object data)
    {
        string name = data.GetType().FullName!.Replace("ViewModel", "View");
        Type? type = Type.GetType(name);

        if (type is not null)
            return (Control)Activator.CreateInstance(type)!;
        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object data) =>
        data is ViewModelBase;
}
