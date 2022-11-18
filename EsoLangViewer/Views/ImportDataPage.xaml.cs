using EsoLangViewer.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace EsoLangViewer.Views;

public sealed partial class ImportDataPage : Page
{
    public ImportDataViewModel ViewModel
    {
        get;
    }

    public ImportDataPage()
    {
        ViewModel = App.GetService<ImportDataViewModel>();
        InitializeComponent();
    }
}
