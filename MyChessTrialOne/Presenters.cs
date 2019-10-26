using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyChessTrialOne
{
    public class BoardPresenter
    {
        public const int MaxBoard = 8;
        public void Print(Board board)
        {
            Console.WriteLine("------------------------");
            for (var i = MaxBoard; i > 0; i--)
            {
                var startX = 'a';
                for (var j = 0; j < MaxBoard; j++)
                {
                    var theX = (char)(Convert.ToUInt16(startX) + j);
                    var theCell = new Cell { X = theX, Y = i };
                    if (board.ContainsKey(theCell))
                    {
                        var piece = board[theCell];
                        if (piece != null)
                        {
                            var onBoard = piece.BoardChar;
                            Console.Write($"{onBoard} ");
                        }
                        else
                        {
                            Console.Write("- ");
                        }
                    }
                    else
                    {
                        Console.Write("- ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine("------------------------");
        }
    }

    public class CapturedPresenter
    {
        public void Print(List<Piece> captured)
        {
            if (captured == null || captured.Count == 0)
                return;
            Console.WriteLine($"Captured black: {string.Join(',',captured.Where(x => x.Player == EPlayer.Black).Select(x => $"{x.BoardChar}"))}");
            Console.WriteLine($"Captured white: {string.Join(',', captured.Where(x => x.Player == EPlayer.White).Select(x => $"{x.BoardChar}"))}");
            Console.WriteLine("------------------------");
        }
    }
}
