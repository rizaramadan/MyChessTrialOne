using System;
using System.Collections.Generic;
using System.Text;

namespace MyChessTrialOne
{
    public class KingCastlingMovement : MovementAbility
    {
        const int CastlingDistance = 2;

        public KingCastlingMovement(Movement m) : base(m) { }

        public override void ValidMove(MoveValidationContext context)
        {
            LeftCastling(context);
            RightCastling(context);
        }

        private int PlayerOriginalY(EPlayer activePlayer) => activePlayer == EPlayer.White ? Board.StartOfY : Board.EndOfY;

        private void LeftCastling(MoveValidationContext context)
        {
            var playerOriginalY = PlayerOriginalY(context.ActivePlayer);
            if (context.Src.X == Board.KingOriginalX && context.Src.Y == playerOriginalY)
            {
                var rockCell = new Cell { X = Board.StartOfX, Y = playerOriginalY };
                var rock = context.Board.ContainsKey(rockCell)
                    ? context.Board[rockCell]
                    : null;
                var piecesInBetween = 
                    context.Board.ContainsKey(new Cell { X = Board.StartOfX.Increase(1), Y = playerOriginalY })
                    || context.Board.ContainsKey(new Cell { X = Board.StartOfX.Increase(2), Y = playerOriginalY })
                    || context.Board.ContainsKey(new Cell { X = Board.StartOfX.Increase(3), Y = playerOriginalY });
                if (rock is Rock && !piecesInBetween)
                {
                    var moveOutput = new MoveOutput { Cell = new Cell { X = Board.KingOriginalX.Decrease(CastlingDistance), Y = playerOriginalY } };
                    moveOutput.Type = EMoveOutputType.CastlingMove;
                    context.ValidMoves.Add(moveOutput);
                }
            }
        }

        private void RightCastling(MoveValidationContext context)
        {
            var playerOriginalY = PlayerOriginalY(context.ActivePlayer);
            if (context.Src.X == Board.KingOriginalX && context.Src.Y == playerOriginalY)
            {
                var rockCell = new Cell { X = Board.EndOfX, Y = playerOriginalY };
                var rock = context.Board.ContainsKey(rockCell)
                    ? context.Board[rockCell]
                    : null;
                var piecesInBetween =
                    context.Board.ContainsKey(new Cell { X = Board.EndOfX.Decrease(1), Y = playerOriginalY })
                    || context.Board.ContainsKey(new Cell { X = Board.EndOfX.Decrease(2), Y = playerOriginalY });
                if (rock is Rock && !piecesInBetween)
                {
                    var moveOutput = new MoveOutput { Cell = new Cell { X = Board.KingOriginalX.Increase(CastlingDistance), Y = playerOriginalY } };
                    moveOutput.Type = EMoveOutputType.CastlingMove;
                    context.ValidMoves.Add(moveOutput);
                }
            }
        }



    }
}
