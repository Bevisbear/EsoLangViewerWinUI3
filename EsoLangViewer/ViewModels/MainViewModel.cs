using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using EsoLangViewer.Core.Contracts.Services;
using EsoLangViewer.Core.Models;
using EsoLangViewer.Core.Services;
using Windows.Storage;

namespace EsoLangViewer.ViewModels;

public class MainViewModel : ObservableRecipient
{

    private bool _islangDataVaild;
    private LangData _selectedLang;
    //private ObservableCollection<LangData> _langdata;

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

    public MainViewModel(ILangSearchService langSearchService, ILangFileService langfileService)
    {
        _langSearchService = langSearchService;
        _langfileService = langfileService;
    }

    public bool ParseLangFile(IReadOnlyList<StorageFile> files)
    {
        Dictionary<string, LangData> langDict = new Dictionary<string, LangData>();

        var langEn = _langfileService.ReadLangWithFileMode(files.ElementAt(0).Path);
        var langZh = _langfileService.ReadLangWithFileMode(files.ElementAt(1).Path);

        foreach (var en in langEn)
        {
            var uniqueId = en.Type.ToString() + "-" + en.Unknown.ToString() + "-" + en.Index.ToString();

            if (!langDict.ContainsKey(uniqueId))
            {
                langDict.Add(uniqueId, new LangData { Id = uniqueId, Type = en.Type, LangEn = en.Lang });
            }
        }

        foreach (var zh in langZh)
        {
            var uniqueId = zh.Type.ToString() + "-" + zh.Unknown.ToString() + "-" + zh.Index.ToString();

            if (langDict.ContainsKey(uniqueId))
            {
                langDict[uniqueId].LangZh = zh.Lang;
            }
        }

        //var first10k = langDict.Values.Take(10000);

        //foreach (var lang in first10k)
        //{
        //    Langdata.Add(lang);
        //}

        return _langSearchService.SetLangData(langDict);
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
