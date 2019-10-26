using System;
using System.Collections.Generic;
using System.Text;

namespace MyChessTrialOne
{
    public static class CommandParser
    {
        public const int SrcIdx = 0;
        public const int DstIdx = 1;

        public static Cell[] Parse(string command)
        {
            var splitted = command.Split('-');
            Cell[] result = new Cell[2];
            var src = splitted[SrcIdx].ToCharArray();
            result[SrcIdx] = new Cell { X = src[0], Y = int.Parse(src[1].ToString()) };
            var dst = splitted[DstIdx].ToCharArray();
            result[DstIdx] = new Cell { X = dst[0], Y = int.Parse(dst[1].ToString()) };
            return result;
            
        }
    }
}
