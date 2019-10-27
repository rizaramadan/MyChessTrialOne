using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyChessTrialOne
{

    public abstract class Movement
    {
        public abstract void ValidMove(MoveValidationContext context);
    }

    public class Default : Movement
    {
        public override void ValidMove(MoveValidationContext context) { }
    }

    public abstract class MovementAbility : Movement
    {
        protected Movement Movement { get; set; }
        public MovementAbility(Movement m)
        {
            Movement = m;
        }

        public override void ValidMove(MoveValidationContext context)
        {
            if (Movement != null)
               Movement.ValidMove(context);
        }
    }

    public enum EVerticalMode
    {
        Any,
        ForwardOnlyCannotCapture
    }

    public enum EMoveOutputType
    {
        NormalMove,
        CaptureMove,
        CastlingMove
    }

    public class MoveOutput
    {
        public Cell Cell { get;set; }
        public EMoveOutputType Type { get; set; }
    }

    public class MoveValidationContext
    {
        public EPlayer ActivePlayer { get; set; }
        public Cell Src { get; set; }
        public Board Board { get; set; }
        public Piece Piece { get; set; }
        public List<MoveOutput> ValidMoves { get; } = new List<MoveOutput>();
        public string InvalidMessage { get; set; } = "move invalid";

        public MoveOutput Find(Cell dst) => ValidMoves.FirstOrDefault(x => x.Cell.ToString() == dst.ToString());
    }
}
