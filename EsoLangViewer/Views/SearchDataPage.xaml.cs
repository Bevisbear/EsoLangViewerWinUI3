using EsoLangViewer.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace EsoLangViewer.Views;

public sealed partial class SearchDataPage : Page
{
    public SearchDataViewModel ViewModel
    {
        get;
    }

    public SearchDataPage()
    {
        ViewModel = App.GetService<SearchDataViewModel>();
        InitializeComponent();
    }
}
