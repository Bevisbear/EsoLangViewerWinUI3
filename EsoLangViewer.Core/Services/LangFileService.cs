using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EsoLangViewer.Core.Contracts.Services;
using EsoLangViewer.Core.Models;
using static System.Convert;

namespace EsoLangViewer.Core.Services;
public class LangFileService : ILangFileService
{
    public List<LangFile> ReadLangWithFileMode(string FilePath)
    {
        int _filesize;
        const int _textIdRecoredSize = 16;
        int _recoredCount;
        int _fileId;
        byte[] buffer = new byte[8];
        byte[] langIdBuffer = new byte[16];
        int textBeginOffset;

        byte[] data = File.ReadAllBytes(FilePath);

        //Dictionary<string, LangFileMode> _data = new();
        List<LangFile> _data = new();

        _filesize = data.Length;

        if (data == null || data.Length <= 0)
        {
            throw new Exception("Error: Invaild data!");
        }

        if (_filesize < 8)
        {
            throw new Exception("Error: Invaild Lang file size!");
        }

        if (_filesize > int.MaxValue)
        {
            throw new Exception("Error: Lang file too big!");
        }

        Array.Copy(data, buffer, 8);
        Array.Reverse(buffer, 0, buffer.Length);  //Reverse bytes order, new readed on head.

        _fileId = BitConverter.ToInt32(buffer, 4);
        _recoredCount = BitConverter.ToInt32(buffer, 0);

        Console.WriteLine("field Id: {0}", _fileId);
        Console.WriteLine("count int: {0}", _recoredCount);

        textBeginOffset = _recoredCount * _textIdRecoredSize + 8;
        Console.WriteLine("textBeginOffset: {0}", textBeginOffset);

        byte[] textUtf8Buffer = new byte[1];

        for (int i = 0; i < _recoredCount; ++i)
        {
            int offset = 8 + i * _textIdRecoredSize;

            //Debug.WriteLine("Offset: {0}", offset);

            Array.Copy(data, offset, langIdBuffer, 0, langIdBuffer.Length);
            Array.Reverse(langIdBuffer, 0, langIdBuffer.Length);

            int langId = BitConverter.ToInt32(langIdBuffer, 12);
            int unknown = BitConverter.ToInt32(langIdBuffer, 8);
            int index = BitConverter.ToInt32(langIdBuffer, 4);
            int offeset = BitConverter.ToInt32(langIdBuffer, 0);
            string text;

            LangFile lang = new()
            {
                Type = langId,
                Unknown= unknown,
                Index = index,
            };

            int textOffset = offeset + textBeginOffset;

            if (textOffset < _filesize)
            {
                string textbuffer = "";

                for (int c = 0; c + textOffset < _filesize; ++c)
                {
                    int ost = c + textOffset;
                    Array.Copy(data, ost, textUtf8Buffer, 0, textUtf8Buffer.Length);

                    var hex = BitConverter.ToString(textUtf8Buffer);

                    if (hex == "00")
                    {
                        break;
                    }
                    else
                    {
                        textbuffer += hex;
                    }
                }

                byte[] stringByte = FromHex(textbuffer);
                text = Encoding.UTF8.GetString(stringByte);

                text = text.Replace("\x0a", @"\n");
                text = text.Replace("\x0d", @"\r");

                lang.Lang = text;

                //Debug.WriteLine("text: {0}", text);
            }

            _data.Add(lang);

            //Debug.WriteLine("id: {0}, unknwon: {1}, index: {2}, offset: {3}, text: {4}",
            //    langId, unknown, index, offeset, lang.TextEn);
        }

        return _data;


    }

    public List<LuaFile> ReadLuaWithFileMode(string FilePath, bool isPreGame = false)
    {
        //string input;
        string pattern = @"^[\s]*SafeAddString\(?[\s]*(\w+)[,\s]+\""(.+)\""[^\""]+$";

        var luaResult = new List<LuaFile>();
        var data = File.ReadAllLines(FilePath);

        foreach (var line in data)
        {
            if (line != null || line != "")
            {
                foreach (Match match in Regex.Matches(line, pattern, RegexOptions.IgnoreCase))
                {
                    string id = match.Groups[1].Value;
                    string text = match.Groups[2].Value;

                    if (isPreGame)
                    {
                        luaResult.Add(new LuaFile { Id = id, Content = text, Type = 1 });
                    }
                    else
                    {
                        luaResult.Add(new LuaFile { Id = id, Content = text, Type = 2 });
                    }
                }
            }
        }

        return luaResult;
    }

    /// <summary>
    /// Turn hex byte string to byte array.
    /// 
    /// From https://stackoverflow.com/a/724905
    /// 
    /// <para>For example: 49-44-4C-45 is string IDLE.</para>
    /// </summary>
    /// <param name="hex"></param>
    /// <returns></returns>
    private static byte[] FromHex(string hex)
    {
        hex = hex.Replace("-", "");
        byte[] raw = new byte[hex.Length / 2];
        for (int i = 0; i < raw.Length; i++)
        {
            raw[i] = ToByte(hex.Substring(i * 2, 2), 16);
        }
        return raw;
    }

    
}
