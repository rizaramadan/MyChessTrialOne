using System;
using System.Collections.Generic;
using System.Text;

namespace MyChessTrialOne
{
    public class MoveExecutionContext
    {
        public EMoveOutputType Type { get; set; }
        public Cell Src { get; set; }
        public Cell Dst { get; set; }
        public Piece Piece { get; set; }
        public Board Board { get; set; }
        public List<Piece> Captured { get; set; }
    }

    public class MoveExecutor
    {
        static Action<Board, Piece, Cell, Cell> Move = (board, piece, src, dst) =>
        {
            board[src] = null;
            board[dst] = piece;
        };

        public void ExecuteMove(MoveExecutionContext input)
        {
            if (input.Type == EMoveOutputType.NormalMove)
            {
                Move(input.Board, input.Piece, input.Src, input.Dst);
            }
            else if (input.Type == EMoveOutputType.CaptureMove)
            {
                input.Captured.Add(input.Board[input.Dst]);
                Move(input.Board, input.Piece, input.Src, input.Dst);
            }
           
        }

    }
}
