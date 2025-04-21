using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Ratatoskr.App.Helpers;

public static class ViewLoader
{
    public static TView Load<TView>() where TView : UserControl
    {
        return App.AppHost.Services.GetRequiredService<TView>();
    }

    public static UserControl LoadView<TView>() where TView : UserControl
    {
        return Load<TView>();
    }
}
