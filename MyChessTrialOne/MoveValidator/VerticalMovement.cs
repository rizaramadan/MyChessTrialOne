using System;
using System.Collections.Generic;
using System.Text;

namespace MyChessTrialOne
{
    public class VerticalMovement : Decorator
    {
        int MaxDistance { get; }
        EVerticalMode Mode { get; }

        public VerticalMovement(Movement m, int maxDistance, EVerticalMode mode) : base(m)
        {
            Movement = m;
            MaxDistance = maxDistance;
            Mode = mode;
        }

        public override void ValidMove(MoveValidationContext context)
        {
            Movement.ValidMove(context);
            if (Mode == EVerticalMode.Any)
            {
                //up
                ValidMoveAction(context, (src, i) => src + i);
                //down
                ValidMoveAction(context, (src, i) => src - i);
            }
            else
            {
                if (context.ActivePlayer == EPlayer.White)
                    ValidMoveAction(context, (src, i) => src + i); //up for white
                else
                    ValidMoveAction(context, (src, i) => src - i); //down for black
            }
        }

        protected void ValidMoveAction(MoveValidationContext context, Func<int, int, int> funcNextY)
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
                    var nextY = funcNextY(context.Src.Y, i);
                    var moveOutput = new MoveOutput { Cell = new Cell { X = context.Src.X, Y = nextY } };
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
                        if (piece.Player != context.Piece.Player && Mode == EVerticalMode.Any)
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
