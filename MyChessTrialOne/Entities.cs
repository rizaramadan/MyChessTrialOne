using System;
using System.Collections.Generic;
using System.Text;

namespace MyChessTrialOne
{
    public class Cell
    {
        public char X { get; set; }
        public int Y { get; set; }

        public int XToInt() => Convert.ToUInt16(X);

        public override string ToString()
        {
            return $"{X}{Y}";
        }

        public override bool Equals(object obj)
        {
            return ToString().Equals(obj?.ToString());
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }

    public enum EPlayer
    {
        White,
        Black,
    }

    public class Board : Dictionary<Cell, Piece>
    {

    }

    public abstract class Piece
    {
        public EPlayer Player { get; set; }
        public char BoardChar { get; set; }

        public Movement Movement { get; set; }

        public abstract void InitMovement();

        public void ValidatingMovement(MoveValidationContext context)
        {
            if (context.ActivePlayer == Player)
                Movement.ValidMove(context);
            else
                context.InvalidMessage = "not players piece";
        }
    }

    public class Pawn : Piece
    {
        public override void InitMovement()
        {
            Movement = new Default();
            Movement = new VerticalMovement(Movement, 1, EVerticalMode.ForwardOnlyCannotCapture);
            Movement = new PawnSpecialMovement(Movement);
        }
    }

    public class Rock : Piece
    {
        public override void InitMovement()
        {
            Movement = new Default();
            Movement = new VerticalMovement(Movement, 8, EVerticalMode.Any);
            Movement = new HorizontalMovement(Movement, 8);
        }
    }

    public class Knight : Piece
    {
        public override void InitMovement()
        {
            Movement = new Default();
        }
    }

    public class Bishop : Piece
    {
        public override void InitMovement()
        {
            Movement = new Default();
        }
    }

    public class Queen : Piece
    {
        public override void InitMovement()
        {
            Movement = new Default();
            Movement = new VerticalMovement(Movement, 8, EVerticalMode.Any);
            Movement = new HorizontalMovement(Movement, 8);
        }
    }

    public class King : Piece
    {
        public override void InitMovement()
        {
            Movement = new Default();
            Movement = new VerticalMovement(Movement, 1, EVerticalMode.Any);
            Movement = new HorizontalMovement(Movement, 1);
        }
    }


}
