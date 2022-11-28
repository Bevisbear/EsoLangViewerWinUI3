using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EsoLangViewer.Core.Contracts.Services;
using EsoLangViewer.Core.Models;

namespace EsoLangViewer.Core.Services;
public class LangSearchService : ILangSearchService
{
    private Dictionary<string, LangData> _LangDict;

    public Task<List<LangData>> SearchLangData(string keyword, int searchType, int searchPos)
    {

        if (_LangDict != null && _LangDict.Count > 1)
        {

        }

        return null;
    }

    public bool SetLangData(Dictionary<string, LangData> langData)
    {
        _LangDict = langData;
        return _LangDict.Count > 1;
    }
}
