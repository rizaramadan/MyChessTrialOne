using System;
using System.Collections.Generic;
using System.Text;

namespace MyChessTrialOne
{
    public class HorizontalMovement : MovementAbility
    {
        int MaxDistance { get; }

        public HorizontalMovement(Movement m, int maxDistance) : base(m)
        {
            Movement = m;
            MaxDistance = maxDistance;
        }

        public override void ValidMove(MoveValidationContext context)
        {
            Movement.ValidMove(context);
            //left
            ValidMoveAction(context, (src, i) => src.Increase(i));
            //right
            ValidMoveAction(context, (src, i) => src.Decrease(i));
        }

        protected void ValidMoveAction(MoveValidationContext context, Func<char, int, char> funcNextX)
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
                    var moveOutput = new MoveOutput { Cell = new Cell { X = nextX, Y = context.Src.Y } };
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
                        if (piece.Player != context.Piece.Player)
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
