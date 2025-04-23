using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ratatoskr.App.Views.Shared;

/// <summary>
/// Interaktionslogik für AppToolbar.xaml
/// </summary>
public partial class AppToolbar : UserControl
{
    public AppToolbar() => InitializeComponent();

    public static readonly DependencyProperty PrintCommandProperty =
        DependencyProperty.Register(nameof(PrintCommand), typeof(ICommand), typeof(AppToolbar));
    public ICommand PrintCommand
    {
        get => (ICommand)GetValue(PrintCommandProperty);
        set => SetValue(PrintCommandProperty, value);
    }

    public static readonly DependencyProperty PrintCopyCommandProperty =
        DependencyProperty.Register(nameof(PrintCopyCommand), typeof(ICommand), typeof(AppToolbar));

    public ICommand PrintCopyCommand
    {
        get => (ICommand)GetValue(PrintCopyCommandProperty);
        set => SetValue(PrintCopyCommandProperty, value);
    }

    public static readonly DependencyProperty MarkAsPaidCommandProperty =
        DependencyProperty.Register(nameof(MarkAsPaidCommand), typeof(ICommand), typeof(AppToolbar));

    public ICommand MarkAsPaidCommand
    {
        get => (ICommand)GetValue(MarkAsPaidCommandProperty);
        set => SetValue(MarkAsPaidCommandProperty, value);
    }

    public static readonly DependencyProperty CancelCommandProperty =
        DependencyProperty.Register(nameof(CancelCommand), typeof(ICommand), typeof(AppToolbar));

    public ICommand CancelCommand
    {
        get => (ICommand)GetValue(CancelCommandProperty);
        set => SetValue(CancelCommandProperty, value);
    }

    public static readonly DependencyProperty ShowPrintButtonProperty =
        DependencyProperty.Register(nameof(ShowPrintButton), typeof(bool), typeof(AppToolbar), new PropertyMetadata(true));

    public bool ShowPrintButton
    {
        get => (bool)GetValue(ShowPrintButtonProperty);
        set => SetValue(ShowPrintButtonProperty, value);
    }

    public static readonly DependencyProperty ShowPrintCopyButtonProperty =
        DependencyProperty.Register(nameof(ShowPrintCopyButton), typeof(bool), typeof(AppToolbar), new PropertyMetadata(true));

    public bool ShowPrintCopyButton
    {
        get => (bool)GetValue(ShowPrintCopyButtonProperty);
        set => SetValue(ShowPrintCopyButtonProperty, value);
    }

    public static readonly DependencyProperty ShowMarkAsPaidButtonProperty =
        DependencyProperty.Register(nameof(ShowMarkAsPaidButton), typeof(bool), typeof(AppToolbar), new PropertyMetadata(true));

    public bool ShowMarkAsPaidButton
    {
        get => (bool)GetValue(ShowMarkAsPaidButtonProperty);
        set => SetValue(ShowMarkAsPaidButtonProperty, value);
    }

    public static readonly DependencyProperty ShowCancelButtonProperty =
        DependencyProperty.Register(nameof(ShowCancelButton), typeof(bool), typeof(AppToolbar), new PropertyMetadata(true));

    public bool ShowCancelButton
    {
        get => (bool)GetValue(ShowCancelButtonProperty);
        set => SetValue(ShowCancelButtonProperty, value);
    }
}
