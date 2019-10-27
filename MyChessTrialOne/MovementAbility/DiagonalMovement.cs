using System;
using System.Collections.Generic;
using System.Text;

namespace MyChessTrialOne
{
    public enum EDiagonalMode
    {
        Any,
        ForwardCaptureOnly
    }

    public class DiagonalMovement : MovementAbility
    {
        int MaxDistance { get; set; }
        EDiagonalMode Mode { get; set; }

        public DiagonalMovement(Movement m, int maxDistance, EDiagonalMode mode) : base(m)
        {
            MaxDistance = maxDistance;
            Mode = mode;
        }

        public override void ValidMove(MoveValidationContext context)
        {
            Movement.ValidMove(context);
            if (Mode == EDiagonalMode.Any)
            {
                NorthEastMove(context);
                NorthWestMove(context);
                SouthEastMove(context);
                SouthWestMove(context);
            }
            else
            {
                if (context.ActivePlayer == EPlayer.White)
                {
                    NorthEastMove(context);
                    NorthWestMove(context);
                }
                else
                {
                    SouthEastMove(context);
                    SouthWestMove(context);
                }
            }
        }

        protected void NorthEastMove(MoveValidationContext context)
        {
            ValidMoveAction
            (
                context,
                (src, i) => src.Increase(i),
                (src, i) => src + i
            );
        }

        protected void NorthWestMove(MoveValidationContext context)
        {
            ValidMoveAction
            (
                context,
                (src, i) => src.Decrease(i),
                (src, i) => src + i
            );
        }

        protected void SouthEastMove(MoveValidationContext context)
        {
            ValidMoveAction
            (
                context,
                (src, i) => src.Increase(i),
                (src, i) => src - i
            );
        }

        protected void SouthWestMove(MoveValidationContext context)
        {
            ValidMoveAction
            (
                context,
                (src, i) => src.Decrease(i),
                (src, i) => src - i
            );
        }



        protected void ValidMoveAction(MoveValidationContext context, Func<char, int, char> funcNextX,  Func<int, int, int> funcNextY)
        {
            var stop = false;
            var i = 1;
            do
            {
                if (i > MaxDistance)
                {
                    stop = true;
                }
                else
                {
                    var nextX = funcNextX(context.Src.X, i);
                    var nextY = funcNextY(context.Src.Y, i);
                    var moveOutput = new MoveOutput { Cell = new Cell { X = nextX, Y = nextY } };
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
                        stop = true;
                        if (piece.Player != context.Piece.Player && Mode == EDiagonalMode.Any)
                        {
                            moveOutput.Type = EMoveOutputType.CaptureMove;
                            context.ValidMoves.Add(moveOutput);
                        }
                    }
                }
                i++;
            } while (!stop);
        }
    }
}
