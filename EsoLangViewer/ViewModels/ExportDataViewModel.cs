using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using EsoLangViewer.Core.Contracts.Services;
using EsoLangViewer.Core.Models;

namespace EsoLangViewer.ViewModels;

public class ExportDataViewModel : ObservableRecipient
{
    private bool _islangDataVaild;
    private LangData _selectedLang;

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

    public string SearchKeyWord
    {
        get; set;
    }
    public bool IsLangDataVaild
    {
        get => _islangDataVaild;
        set => SetProperty(ref _islangDataVaild, value);
    }

    public LangData SelectedLang
    {
        get => _selectedLang;
        set => SetProperty(ref _selectedLang, value);
    }
    public ObservableCollection<LangData> Langdata { get; } = new ObservableCollection<LangData>();

    private readonly ILangSearchService _langSearchService;
    private readonly ILangFileService _langfileService;


    public ExportDataViewModel(ILangSearchService langSearchService, ILangFileService langfileService)
    {
        _langSearchService = langSearchService;
        _langfileService = langfileService;
    }

    public async Task<bool> SearchLang(string keyword, int searchType, int searchPos)
    {
        if (Langdata.Count > 0)
        {
            Langdata.Clear();
        }

        var list = await Task.Run(() => _langSearchService.SearchLangData(keyword, searchType, searchPos));

        if (list != null && list.Count > 0)
        {
            foreach (var lang in list)
            {
                Langdata.Add(lang);
            }
        }
        return Langdata.Count > 0;
    }


}
