using System;
using System.Collections.Generic;
using System.Text;

namespace MyChessTrialOne
{
    public class KnightMovement : MovementAbility
    {
        /// <summary>
        /// Knight consist of 2 aspek of movement, move 2 cell, than 1 cell
        /// thus we need to consider those 2 distance to make a valid knight movement
        /// </summary>
        const int DistanceOfTwo = 2;
        const int DistanceOfOne = 1;

        public KnightMovement(Movement m) : base(m) { }

        public override void ValidMove(MoveValidationContext context)
        {
            NorthLeftMove(context);
            NorthRightMove(context);

            EastUpMove(context);
            EastDownMove(context);

            SouthLeftMove(context);
            SouthRightMove(context);

            WestUpMove(context);
            WestDownMove(context);
        }

        private void WestDownMove(MoveValidationContext context)
        {
            var newX = context.Src.X.Decrease(DistanceOfTwo);
            var newY = context.Src.Y - DistanceOfOne;
            AddValidMove(context, newX, newY);
        }

        private void WestUpMove(MoveValidationContext context)
        {
            var newX = context.Src.X.Decrease(DistanceOfTwo);
            var newY = context.Src.Y + DistanceOfOne;
            AddValidMove(context, newX, newY);
        }

        private void SouthRightMove(MoveValidationContext context)
        {
            var newX = context.Src.X.Increase(DistanceOfOne);
            var newY = context.Src.Y - DistanceOfTwo;
            AddValidMove(context, newX, newY);
        }

        private void SouthLeftMove(MoveValidationContext context)
        {
            var newX = context.Src.X.Decrease(DistanceOfOne);
            var newY = context.Src.Y - DistanceOfTwo;
            AddValidMove(context, newX, newY);
        }

        private void EastDownMove(MoveValidationContext context)
        {
            var newX = context.Src.X.Increase(DistanceOfTwo);
            var newY = context.Src.Y - DistanceOfOne;
            AddValidMove(context, newX, newY);
        }

        private void EastUpMove(MoveValidationContext context)
        {
            var newX = context.Src.X.Increase(DistanceOfTwo);
            var newY = context.Src.Y + DistanceOfOne;
            AddValidMove(context, newX, newY);
        }

        private void NorthRightMove(MoveValidationContext context)
        {
            var newX = context.Src.X.Increase(DistanceOfOne);
            var newY = context.Src.Y + DistanceOfTwo;
            AddValidMove(context, newX, newY);
        }

        private void NorthLeftMove(MoveValidationContext context)
        {
            var newX = context.Src.X.Decrease(DistanceOfOne);
            var newY = context.Src.Y + DistanceOfTwo;
            AddValidMove(context, newX, newY);
        }

        private static void AddValidMove(MoveValidationContext context, char newX, int newY)
        {
            if (Cell.IsXValid(newX) && Cell.IsYValid(newY))
            {
                var moveOutput = new MoveOutput { Cell = new Cell { X = newX, Y = newY } };
                var piece = context.Board.ContainsKey(moveOutput.Cell)
                                        ? context.Board[moveOutput.Cell]
                                        : null;
                if (piece == null)
                {
                    moveOutput.Type = EMoveOutputType.NormalMove;
                    context.ValidMoves.Add(moveOutput);
                }
                else
                {
                    if (piece.Player != context.Piece.Player)
                    {
                        moveOutput.Type = EMoveOutputType.CaptureMove;
                        context.ValidMoves.Add(moveOutput);
                    }
                }
            }
        }
    }
}
