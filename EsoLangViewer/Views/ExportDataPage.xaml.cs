using EsoLangViewer.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace EsoLangViewer.Views;

public sealed partial class ExportDataPage : Page
{
    public ExportDataViewModel ViewModel
    {
        get;
    }

    public ExportDataPage()
    {
        ViewModel = App.GetService<ExportDataViewModel>();
        InitializeComponent();
    }
}
