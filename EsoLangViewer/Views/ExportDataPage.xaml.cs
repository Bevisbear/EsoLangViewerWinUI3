using CommunityToolkit.WinUI;
using EsoLangViewer.ViewModels;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using Microsoft.UI.Xaml;
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
    private void SearchTypeComboBox_Loaded(object sender, RoutedEventArgs e)
    {
        SearchTypeComboBox.SelectedIndex = 1;
    }

    private void SearchPosComboBox_Loaded(object sender, RoutedEventArgs e)
    {
        SearchPosComboBox.SelectedIndex = 0;
    }

    private async void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        FileInfoBar.Message = "Searching".GetLocalized();
        FileInfoBar.Severity = InfoBarSeverity.Informational;

        if (sender.Text != null && sender.Text.Length > 1)
        {
            LoadLangData_InProgress.Visibility = Visibility.Visible;

            bool searchDone = await ViewModel.SearchLang(sender.Text, SearchTypeComboBox.SelectedIndex, SearchPosComboBox.SelectedIndex);

            if (searchDone)
            {
                FileInfoBar.Message = "Search Completed";
                LoadLangData_InProgress.Visibility = Visibility.Collapsed;
            }
        }
        else
        {
            FileInfoBar.Message = "Plase text content or a word.";
            FileInfoBar.Severity = InfoBarSeverity.Warning;
        }
    }

    private async void ExportButton_Click(object sender, RoutedEventArgs e)
    {
    
    }
}
