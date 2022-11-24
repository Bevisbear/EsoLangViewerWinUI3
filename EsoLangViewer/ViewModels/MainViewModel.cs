using CommunityToolkit.Mvvm.ComponentModel;
using EsoLangViewer.Core.Contracts.Services;

namespace EsoLangViewer.ViewModels;

public class MainViewModel : ObservableRecipient
{
    public List<Tuple<ushort, string>> SearchType
    {
        get;
    } = new List<Tuple<ushort, string>>()
    {
        new Tuple<ushort, string> (0, "Guid"),
        new Tuple<ushort, string> (1, "UniqueID"),
        new Tuple<ushort, string> (2, "Type"),
        new Tuple<ushort, string> (3, "English"),
        new Tuple<ushort, string> (4, "ChineseS"),
    };

    public List<Tuple<ushort, string>> SearchPos
    {
        get;
    } = new List<Tuple<ushort, string>>()
    {
        new Tuple<ushort, string> (0, "Full"),
        new Tuple<ushort, string> (1, "Only Front"),
        new Tuple<ushort, string> (2, "Only End"),
    };

    public string SearchKeyWord { get; set; }

    private ILangSearchService langSearchService;

    public MainViewModel(ILangSearchService _langSearchService)
    {
        langSearchService = _langSearchService;
    }
}
