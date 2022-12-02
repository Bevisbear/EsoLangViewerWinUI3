using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsoLangViewer.Core.Models;
public class LuaFile
{
    public string Id { get; set; }
    public string Content { get; set; }
    /// <summary>
    /// LuaPreGame = 1,
    /// LuaClient,
    /// LuaBoth,
    /// </summary>
    public int Type { get; set; }
}
