using System;
using System.Collections.Generic;
using System.Text;

namespace MyChessTrialOne
{
    public class ParseOutput
    {
        public Cell Src { get; set; }
        public Cell Dst { get; set; }

        public bool IsValid() =>
            Cell.IsXValid(Src.X)
            && Cell.IsYValid(Src.Y)
            && Cell.IsXValid(Dst.X)
            && Cell.IsYValid(Dst.Y);
    }

    public static class CommandParser
    {

        public static ParseOutput Parse(string command)
        {
            var splitted = command.Split('-');
            var result = new ParseOutput();
            var src = splitted[0].ToCharArray();
            result.Src = new Cell { X = src[0], Y = int.Parse(src[1].ToString()) };
            var dst = splitted[1].ToCharArray();
            result.Dst = new Cell { X = dst[0], Y = int.Parse(dst[1].ToString()) };
            return result;
            
        }
    }
}
