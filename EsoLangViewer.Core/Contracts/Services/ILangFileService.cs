using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EsoLangViewer.Core.Models;

namespace EsoLangViewer.Core.Contracts.Services;
internal interface ILangFileService
{
    Task<List<LangFile>> ReadLangWithFileMode(string FilePath);
}
