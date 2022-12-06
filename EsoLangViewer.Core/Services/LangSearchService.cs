using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Convert;
using EsoLangViewer.Core.Contracts.Services;
using EsoLangViewer.Core.Models;
using System.Diagnostics;

namespace EsoLangViewer.Core.Services;
public class LangSearchService : ILangSearchService
{
    private Dictionary<string, LangData> _LangDict;
    private Dictionary<string, LuaData> _LuaDict;

    public List<LangData> SearchLangData(string keyword, int searchType, int searchPos)
    {
        List<LangData> langtext = null;
        List<LuaData> luaResult = null;
        Debug.WriteLine(keyword);

        if (_LangDict != null && _LangDict.Count > 1)
        {
            switch (searchType)
            {
                case 1:
                    langtext = _LangDict.Where(d => d.Key == keyword).Select(d => d.Value).ToList();
                    break;
                case 2:
                    langtext = _LangDict.Where(d => d.Value.Type == ToInt32(keyword)).Select(d => d.Value).ToList();
                    break;
                case 3:
                    langtext = _LangDict.Where(d => d.Value.LangEn != null && d.Value.LangEn.Contains(keyword)).Select(d => d.Value).ToList();
                    break;
                case 4:
                    langtext = _LangDict.Where(d => d.Value.LangZh != null && d.Value.LangZh.Contains(keyword)).Select(d => d.Value).ToList();
                    break;
                default:
                    break;
            }
            //langtext = searchType switch
            //{
            //    //0 => await _LangDict.Where(d => d.Value. )
            //    1 => _LangDict.Where(d => d.Key == keyword).Select(d => d.Value).ToList(),
            //    2 => _LangDict.Where(d => d.Value.Type == ToInt32(keyword)).Select(d => d.Value).ToList(),
            //    3 => _LangDict.Where(d => d.Value.LangEn != null && d.Value.LangEn.Contains(keyword)).Select(d => d.Value).ToList(),
            //    4 => _LangDict.Where(d => d.Value.LangZh != null && d.Value.LangZh.Contains(keyword)).Select(d => d.Value).ToList(),
            //};
        }

        if (_LuaDict != null && _LuaDict.Count > 1)
        {
            if (langtext != null)
            {
                switch (searchType) 
                {
                    case 1:
                        luaResult = _LuaDict.Where(l => l.Key == keyword).Select(d => d.Value).ToList();
                        break;
                    case 3:
                        luaResult = _LuaDict.Where(l => l.Value.ContentEn != null && l.Value.ContentEn.Contains(keyword))
                            .Select(l => l.Value).ToList();
                        break;
                    case 4:
                        luaResult = _LuaDict.Where(l => l.Value.ContentZh != null && l.Value.ContentZh.Contains(keyword))
                            .Select(l => l.Value).ToList();
                        break;
                    default:
                        break;
                };

                if (luaResult != null && luaResult.Count > 0)
                {
                    foreach(var l in luaResult)
                    {
                        langtext.Add(new LangData { Id = l.Id, LangEn = l.ContentEn, LangZh = l.ContentZh });
                    }
                }

                if (searchType == 2 && ToInt32(keyword) == 100)
                {
                    foreach (var lua in _LuaDict)
                    {
                        langtext.Add(new LangData { Id = lua.Value.Id, LangEn = lua.Value.ContentEn, LangZh = lua.Value.ContentZh });
                    }
                }
            }
        }

        return langtext;
    }

    public bool SetLangData(Dictionary<string, LangData> langData, Dictionary<string, LuaData> luaData)
    {
        _LangDict = langData;
        _LuaDict = luaData;
        return _LangDict.Count > 1;
    }
}
