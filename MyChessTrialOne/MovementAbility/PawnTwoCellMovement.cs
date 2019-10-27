using System;
using System.Collections.Generic;
using System.Text;

namespace MyChessTrialOne
{
    public class PawnTwoCellMovement : VerticalMovement
    {
        private const int WhitePawnOriginalY = 2;
        private const int BlackPawnOriginalY = 7;

        public PawnTwoCellMovement(Movement m) : base(m, 2, EVerticalMode.ForwardOnlyCannotCapture) { }

        public override void ValidMove(MoveValidationContext context)
        {
            Movement.ValidMove(context);
            if (PawnInStartingPosition(context.Piece.Player, context.Src))
            {
                base.ValidMove(context);
            }
        }

        private bool PawnInStartingPosition(EPlayer player, Cell src)
        {
            return (player == EPlayer.White && src.Y == WhitePawnOriginalY)
                || (player == EPlayer.Black && src.Y == BlackPawnOriginalY);
        }
    }
}
