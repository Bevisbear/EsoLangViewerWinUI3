using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsoLangViewer.Core.Models;
public class LangData
{
    public string Id
    {
        get; set; 
    }
    public int Type
    {
        get; set; 
    }
    public string? LangEn
    {
        get; set;
    }
    public string? LangZh
    {
        get; set;
    }
}
