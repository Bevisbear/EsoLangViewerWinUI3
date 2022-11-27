using CommunityToolkit.Mvvm.ComponentModel;
using EsoLangViewer.Core.Contracts.Services;
using EsoLangViewer.Core.Models;
using EsoLangViewer.Core.Services;
using Windows.Storage;

namespace EsoLangViewer.ViewModels;

public class MainViewModel : ObservableRecipient
{

    private bool _islangDataVaild;

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

    private ILangSearchService _langSearchService;
    private ILangFileService _langfileService;

    public MainViewModel(ILangSearchService langSearchService, ILangFileService langfileService)
    {
        _langSearchService = langSearchService;
        _langfileService = langfileService;
    }


    public async Task<bool> ParseLangFile(IReadOnlyList<StorageFile> files)
    {
        //List<LangFile> langEn;
        //List<LangFile> lang2;

        Dictionary<string, LangData> langDict = new Dictionary<string, LangData>();

        //string langEnPath = files.ElementAt(0).Path;

        var langEn = await _langfileService.ReadLangWithFileMode(files.ElementAt(0).Path);
        var lang2 = await _langfileService.ReadLangWithFileMode(files.ElementAt(1).Path);

        foreach (var en in langEn)
        {
            var uniqueId = en.Type.ToString() + "-" + en.Unknown.ToString() + "-" + en.Index.ToString();

            if (!langDict.ContainsKey(uniqueId))
            {
                langDict.Add(uniqueId, new LangData { })
            }

        }



        foreach (StorageFile file in files)
        {
            langEn = await _langfileService.ReadLangWithFileMode(file.Path);
        }


        await Task.Delay(2000);
        return true;
    }


}
