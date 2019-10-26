using System;
using System.Collections.Generic;
using System.Text;

namespace MyChessTrialOne
{
    public class MoveExecutionInput
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
        //public void ExecuteMove(MoveValidationContext context, Piece piece, Cell src, Cell dst, Board board, List<Piece> captured)
        //{
        //if (validationOutput.Capture)
        //    captured.Add(board[dst]);
        //board[src] = null;
        //board[dst] = piece;internal void ExecuteMove(MoveOutput moveOutput)

        public void ExecuteMove(MoveExecutionInput input)
        {
            Action<Board, Piece, Cell, Cell> move = (board, piece, src, dst) => 
            {
                board[src] = null;
                board[dst] = piece;
            };

            if (input.Type == EMoveOutputType.NormalMove)
            {
                move(input.Board, input.Piece, input.Src, input.Dst);
            }
            else if (input.Type == EMoveOutputType.CaptureMove)
            {
                input.Captured.Add(input.Board[input.Dst]);
                move(input.Board, input.Piece, input.Src, input.Dst);

            }
        }

    }
}
