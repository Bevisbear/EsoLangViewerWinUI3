using System.Diagnostics;
using System.Text;
using EsoLangViewer.Controls;
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
                LoadLangData_InProgress.Visibility = Visibility.Visible;

                var parseDone = await Task.Run(() => ViewModel.ParseLangFile(files));

                if (parseDone)
                {
                    ViewModel.IsLangDataVaild = true;
                    OpenFileButton.IsEnabled = true;
                    FileInfoBar.Message = "Loding Completed!";
                    FileInfoBar.Severity = InfoBarSeverity.Success;
                    LoadLangData_InProgress.Visibility = Visibility.Collapsed;
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
        //IReadOnlyList<StorageFile> files = new List<StorageFile>();

        picker.FileTypeFilter.Add(".lang");
        picker.FileTypeFilter.Add(".lua");

        //必须指定浏览文件的新窗口依赖，别想着 GetWindowHandle() 填 this 就能获取到窗口 Handle 了！
        //基于 Page 的页面指定程序主窗口，否则崩溃。
        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
        WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

        return await picker.PickMultipleFilesAsync();

        //if (files.Count > 0)
        //{
        //    StringBuilder output = new StringBuilder("Picked files:\n");
        //    // Application now has read/write access to the picked file(s)
        //    foreach (StorageFile file in files)SearchPosComboBox
        //    {
        //        output.Append(file.Name + "\n");
                
        //    }
        //    Debug.WriteLine(output);
        //}

        //return files;
    }

    private async void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        FileInfoBar.Message = "Loding...";
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
}
