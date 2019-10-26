using System;
using System.Collections.Generic;
using System.Text;

namespace MyChessTrialOne
{
    public class MoveExecutor
    {
        public void ExecuteMove(MoveValidationOutput validationOutput, IPiece piece, Cell src, Cell dst, Board board, List<IPiece> captured)
        {
            if (validationOutput.Capture)
                captured.Add(board[dst]);
            board[src] = null;
            board[dst] = piece;
        }
    }
}
