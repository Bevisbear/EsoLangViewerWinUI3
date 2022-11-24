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
    private Dictionary<string, LangData> _LangDict = new();


    public Task<List<LangData>> SearchLangData(string keyword, ushort searchType, ushort searchPos)
    {

        if (_LangDict.Count >= 0)
        {

        }

        return null;
    }
}
