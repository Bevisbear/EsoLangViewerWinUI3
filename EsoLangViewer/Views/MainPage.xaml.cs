using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using CommunityToolkit.WinUI;
using EsoLangViewer.Controls;
using EsoLangViewer.Core.Models;
using EsoLangViewer.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.AppLifecycle;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace EsoLangViewer.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
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

    private async void OpenFileButton_Click(object sender, RoutedEventArgs e)
    {
        IReadOnlyList<StorageFile> files = await OpenFileBrowser();

        if (files != null && files.Count > 1)
        {
            var dialog = new ContentDialog
            {
                // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
                XamlRoot = this.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                Title = "Dialog Test",
                PrimaryButtonText = "Save",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                Content = "Ask",
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                OpenFileButton.IsEnabled = false;
                FileInfoBar.Message = "Loding...";
                DataInProgress.Visibility = Visibility.Visible;

                var parseDone = await Task.Run(() => ViewModel.ParseLangFile(files));

                if (parseDone)
                {
                    ViewModel.IsLangDataVaild = true;
                    OpenFileButton.IsEnabled = true;
                    FileInfoBar.Message = "Loding Completed!";
                    FileInfoBar.Severity = InfoBarSeverity.Success;
                    DataInProgress.Visibility = Visibility.Collapsed;
                }
            }
        }
        else
        {
            var dialog = new ContentDialog
            {
                // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
                XamlRoot = this.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                Title = "Dialog Test",
                //PrimaryButtonText = "Save",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                Content = "Fail",
            };

            await dialog.ShowAsync();
        }

    }

    private async Task<IReadOnlyList<StorageFile>> OpenFileBrowser()
    {
        var picker = new FileOpenPicker();   //WinUI3 获取文件列表窗口的新API。

        picker.FileTypeFilter.Add(".lang");
        picker.FileTypeFilter.Add(".lua");

        //必须指定浏览文件的新窗口依赖，别想着 GetWindowHandle() 填 this 就能获取到窗口 Handle 了！
        //基于 Page 的页面指定程序主窗口，否则崩溃。
        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
        WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

        return await picker.PickMultipleFilesAsync();
    }

    private async void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        FileInfoBar.Message = "Searching".GetLocalized();
        FileInfoBar.Severity = InfoBarSeverity.Informational;

        if (sender.Text != null && sender.Text.Length > 1)
        {
            DataInProgress.Visibility = Visibility.Visible;

            bool searchDone = await ViewModel.SearchLang(sender.Text, SearchTypeComboBox.SelectedIndex, SearchPosComboBox.SelectedIndex);

            if (searchDone)
            {
                FileInfoBar.Message = "Search Completed, list count: " + ViewModel.Langdata.Count.ToString();
                DataInProgress.Visibility = Visibility.Collapsed;
                ViewModel.CanExportText = true;
            }
        }
        else
        {
            FileInfoBar.Message = "Plase text content or a word.";
            FileInfoBar.Severity = InfoBarSeverity.Warning;
        }
        
    }

    private void ExportButton_Click(object sender, RoutedEventArgs e)
    {
        List<LangData> exportData = new();

        if (ViewModel.ExportSelectedLangdata)
        {
            if (LangDataGrid.SelectedItems != null && LangDataGrid.SelectedItems.Count >= 1)
            {
                exportData = (List<LangData>)LangDataGrid.SelectedItems;
            }
            else
            {
                FileInfoBar.Message = "null list".GetLocalized();
                FileInfoBar.Severity = InfoBarSeverity.Warning;
            }
        }
        else
        {
            if (ViewModel.Langdata != null && ViewModel.Langdata.Count >= 1)
            {
                exportData = ViewModel.Langdata.ToList();
            }
            else
            {
                FileInfoBar.Message = "null list".GetLocalized();
                FileInfoBar.Severity = InfoBarSeverity.Warning;
            }

        }

        //var selectedItems = (List<LangData>)LangDataGrid.SelectedItems;
        var firstLangType = exportData.FirstOrDefault();
        bool isHaveDifferentType = false;

        if (exportData.Count >= 1)
        {
            if (firstLangType != null)
            {
                foreach (var lang in exportData)
                {
                    if (lang.Type != firstLangType.Type)
                    {
                        FileInfoBar.Message = "not the same type".GetLocalized();
                        FileInfoBar.Severity = InfoBarSeverity.Warning;
                        isHaveDifferentType = true;
                        break;
                    }
                }

                if (!isHaveDifferentType)
                {
                    ViewModel.ExportToFile(exportData);
                }

            }
        }
        else
        {
            FileInfoBar.Message = "null list".GetLocalized();
            FileInfoBar.Severity = InfoBarSeverity.Error;
        }
    }

}
