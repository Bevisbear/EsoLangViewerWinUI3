using EsoLangViewer.Helpers;

namespace EsoLangViewer;

public sealed partial class MainWindow : WindowEx
{
    public MainWindow()
    {
        InitializeComponent();

        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/BearIcon.ico"));
        Content = null;
        Title = "AppDisplayName".GetLocalized();
    }
}
