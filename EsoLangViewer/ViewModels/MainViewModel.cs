using System.Collections.ObjectModel;
using System.Linq;
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
        Dictionary<string, LangData> langDict = new();
        Dictionary<string, LuaData> luaData = new();

        List<string> langFiles = new();
        List<string> luaFiles = new();

        List<LangFile> langEn = new();
        List<LangFile> langZh = new();

        List<LuaFile> luaEn = new();
        List<LuaFile> luaZh = new();


        //分析已选择的文件名
        foreach (var file in files)
        {
            if (file.Name.ToLower().EndsWith(".lang"))
            {
                langFiles.Add(file.Path);
            }
            else if (file.Name.ToLower().EndsWith(".lua"))
            {
                luaFiles.Add(file.Path);
            }
        }

        //读取选择的 .lang 文件
        foreach (var langPath in langFiles)
        {
            if (langPath.ToLower().EndsWith("en.lang"))
            {
                langEn = _langfileService.ReadLangWithFileMode(langPath);
            }
            else if (langPath.ToLower().EndsWith("zh.lang"))
            {
                langZh = _langfileService.ReadLangWithFileMode(langPath);
            }
        }

        //分析 en.lang 文件
        if (langEn.Count > 1)
        {
            foreach (var en in langEn)
            {
                var uniqueId = en.Type.ToString() + "-" + en.Unknown.ToString() + "-" + en.Index.ToString();

                if (!langDict.ContainsKey(uniqueId))
                {
                    langDict.Add(uniqueId, new LangData { Id = uniqueId, Type = en.Type, LangEn = en.Lang });
                }
            }
        }

        //分析 zh.lang 文件
        if (langZh.Count > 1)
        {
            foreach (var zh in langZh)
            {
                var uniqueId = zh.Type.ToString() + "-" + zh.Unknown.ToString() + "-" + zh.Index.ToString();

                if (langDict.ContainsKey(uniqueId))
                {
                    langDict[uniqueId].LangZh = zh.Lang;
                }
            }
        }

        //读取 lua 文件
        foreach (var luaPath in luaFiles)
        {
            if (luaPath.ToLower().EndsWith("en_client.lua"))
            {
                if (luaEn.Count < 1)
                {
                    luaEn = _langfileService.ReadLuaWithFileMode(luaPath);
                }
                else
                {
                    luaEn.AddRange(_langfileService.ReadLuaWithFileMode(luaPath));
                }
            }
            else if (luaPath.ToLower().EndsWith("en_pregame.lua"))
            {
                if (luaEn.Count < 1)
                {
                    luaEn = _langfileService.ReadLuaWithFileMode(luaPath, true);
                }
                else
                {
                    luaEn.AddRange(_langfileService.ReadLuaWithFileMode(luaPath, true));
                }
            }
            else if (luaPath.ToLower().EndsWith("zh_client.lua"))
            {
                if (luaZh.Count < 1)
                {
                    luaZh = _langfileService.ReadLuaWithFileMode(luaPath);
                }
                else
                {
                    luaZh.AddRange(_langfileService.ReadLuaWithFileMode(luaPath));
                }
            }
            else if (luaPath.ToLower().EndsWith("zh_pregame.lua"))
            {
                if (luaZh.Count < 1)
                {
                    luaZh = _langfileService.ReadLuaWithFileMode(luaPath, true);
                }
                else
                {
                    luaZh.AddRange(_langfileService.ReadLuaWithFileMode(luaPath, true));
                }
            }
        }

        //分析 Lua 文件
        if(luaEn.Count > 1)
        {
            foreach (var lua in luaEn)
            {
                if (!luaData.ContainsKey(lua.Id))
                {
                    luaData.Add(lua.Id, new LuaData { Id = lua.Id, ContentEn = lua.Content, Type = lua.Type });
                }
                else
                {
                    luaData[lua.Id].Type = 3;
                }
            }
        }

        if (luaZh.Count > 1)
        {
            foreach (var lua in luaZh)
            {
                if (luaData.ContainsKey(lua.Id))
                {
                    luaData[lua.Id].ContentZh = lua.Content;
                }
            }
                
        }

        //var first10k = langDict.Values.Take(10000);

        //foreach (var lang in first10k)
        //{
        //    Langdata.Add(lang);
        //}

        return _langSearchService.SetLangData(langDict, luaData);
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
