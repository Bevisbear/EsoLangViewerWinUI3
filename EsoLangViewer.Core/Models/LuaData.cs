using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsoLangViewer.Core.Models;
public class LuaData
{
    public string Id { get; set; }
    public string ContentEn { get; set; }
    public string ContentZh { get; set; }
    /// <summary>
    /// LuaPreGame = 1,
    /// LuaClient,
    /// LuaBoth,
    /// </summary>
    public int Type { get; set; }
}
