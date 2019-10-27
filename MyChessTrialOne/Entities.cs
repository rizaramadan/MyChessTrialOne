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

        public static bool IsXValid(char x) => x >= Board.StartOfX && x <= Board.EndOfX;
        public static bool IsYValid(int y) => y >= Board.StartOfY && y <= Board.EndOfY;
    }

    public static class CharExt
    {
        public static char Increase(this char c, int i)
        {
            var srcInt = Convert.ToInt16(c);
            return (char)(srcInt + i);
        }

        public static char Decrease(this char c, int i)
        {
            var srcInt = Convert.ToInt16(c);
            return (char)(srcInt - i);
        }
    }

    public enum EPlayer
    {
        White,
        Black,
    }

    public class Board : Dictionary<Cell, Piece>
    {
        public const int StartOfY = 1;
        public const int EndOfY = 8;

        public const char StartOfX = 'a';
        public const char EndOfX = 'h';

        public const char KingOriginalX = 'e';
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
            Movement = new PawnTwoCellMovement(Movement);
            Movement = new DiagonalMovement(Movement, 1, EDiagonalMode.ForwardCaptureOnly);
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
            Movement = new KnightMovement(Movement);
        }
    }

    public class Bishop : Piece
    {
        public override void InitMovement()
        {
            Movement = new Default();
            Movement = new DiagonalMovement(Movement, 8, EDiagonalMode.Any);
        }
    }

    public class Queen : Piece
    {
        public override void InitMovement()
        {
            Movement = new Default();
            Movement = new VerticalMovement(Movement, 8, EVerticalMode.Any);
            Movement = new HorizontalMovement(Movement, 8);
            Movement = new DiagonalMovement(Movement, 8, EDiagonalMode.Any);
        }
    }

    public class King : Piece
    {
        public override void InitMovement()
        {
            Movement = new Default();
            Movement = new VerticalMovement(Movement, 1, EVerticalMode.Any);
            Movement = new HorizontalMovement(Movement, 1);
            Movement = new DiagonalMovement(Movement, 1, EDiagonalMode.Any);
            Movement = new KingCastlingMovement(Movement);
        }
    }


}
