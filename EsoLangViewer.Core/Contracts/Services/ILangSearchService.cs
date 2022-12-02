using System.Threading.Tasks;
using EsoLangViewer.Core.Models;

namespace EsoLangViewer.Core.Contracts.Services;
public interface ILangSearchService
{
    List<LangData> SearchLangData(string keyword, int searchType, int searchPos);
    bool SetLangData(Dictionary<string, LangData> langData, Dictionary<string, LuaData> luaData);
}
