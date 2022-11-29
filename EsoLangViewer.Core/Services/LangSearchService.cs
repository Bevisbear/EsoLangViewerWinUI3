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

    public List<LangData> SearchLangData(string keyword, int searchType, int searchPos)
    {
        List<LangData> langtext = null;
        Debug.WriteLine(keyword);

        if (_LangDict != null && _LangDict.Count > 1)
        {
            langtext = searchType switch
            {
                //0 => await _LangDict.Where(d => d.Value. )
                1 => _LangDict.Where(d => d.Key == keyword).Select(d => d.Value).ToList(),
                2 => _LangDict.Where(d => d.Value.Type == ToInt32(keyword)).Select(d => d.Value).ToList(),
                3 => _LangDict.Where(d => d.Value.LangEn != null && d.Value.LangEn.Contains(keyword)).Select(d => d.Value).ToList(),
                4 => _LangDict.Where(d => d.Value.LangZh != null && d.Value.LangZh.Contains(keyword)).Select(d => d.Value).ToList(),
            };
        }

        return langtext;
    }

    public bool SetLangData(Dictionary<string, LangData> langData)
    {
        _LangDict = langData;
        return _LangDict.Count > 1;
    }
}
