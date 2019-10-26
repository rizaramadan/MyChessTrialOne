using System;
using System.Collections.Generic;
using System.Text;

namespace MyChessTrialOne
{
    public class Cell
    {
        public char X { get; set; }
        public int Y { get; set; }

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

    public class Board : Dictionary<Cell, IPiece>
    {

    }

    public interface IPiece
    {
        EPlayer Player { get; set; }
        string PrintToBoard();
    }

    public class Pawn : IPiece
    {
        public EPlayer Player { get; set; }

        public string PrintToBoard() // This method is 98% similar across all Pieces, can be more DRY by just setting a PieceChar
        {                            // to the requisite char and implement the PrintToBoard method on the base class
            var result = "P"; 
            return Player == EPlayer.White
                ? result
                : result.ToLower();
        }
    }

    public class Rock : IPiece
    {
        public EPlayer Player { get; set; }
        public string PrintToBoard()
        {
            var result = "R";
            return Player == EPlayer.White
                ? result
                : result.ToLower();
        }
    }

    public class Knight : IPiece
    {
        public EPlayer Player { get; set; }
        public string PrintToBoard()
        {
            var result = "T";
            return Player == EPlayer.White
                ? result
                : result.ToLower();
        }
    }

    public class Bishop : IPiece
    {
        public EPlayer Player { get; set; }
        public string PrintToBoard()
        {
            var result = "B";
            return Player == EPlayer.White
                ? result
                : result.ToLower();
        }
    }

    public class Queen : IPiece
    {
        public EPlayer Player { get; set; }
        public string PrintToBoard()
        {
            var result = "Q";
            return Player == EPlayer.White
                ? result
                : result.ToLower();
        }
    }

    public class King : IPiece
    {
        public EPlayer Player { get; set; }
        public string PrintToBoard()
        {
            var result = "K";
            return Player == EPlayer.White
                ? result
                : result.ToLower();
        }
    }


}
