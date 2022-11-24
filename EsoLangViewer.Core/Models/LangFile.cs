using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsoLangViewer.Core.Models;
public class LangFile
{
    //public Guid UniqueId
    //{
    //    get; set;
    //}
    //public int Id
    //{
    //    get; set; 
    //}
    public int Type
    {
        get; set;
    }
    public int Unknown
    {
        get; set; 
    }
    public int Index
    {
        get; set;
    }
    public string Lang
    {
        get; set;
    }
}
