using System.Threading.Tasks;
using EsoLangViewer.Core.Models;

namespace EsoLangViewer.Core.Contracts.Services;
public interface ILangSearchService
{
    Task<List<LangData>> SearchLangData(string keyword, ushort searchType, ushort searchPos);
}
