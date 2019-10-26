using System;
using System.Collections.Generic;
using System.Text;

namespace MyChessTrialOne
{
    public class MoveValidationOutput
    {
        public bool Valid { get; set; }
        public bool Capture { get; set; }
        public string InvalidMessage { get; set; } = "move invalid";
        public bool Castling { get; set; }
        public bool Promoting { get; set; }
    }

    public abstract class Movement
    {
        public bool IsCapturingFriend(IPiece piece, Cell dst, Board board)
        {
            if (!board.ContainsKey(dst))
                return false;
            var dstPiece = board[dst];
            return piece.Player == piece.Player
                ? true
                : false;
        }

        public abstract MoveValidationOutput IsMoveValid(IPiece piece, Cell src, Cell dst, Board board);
    }

    public class PawnMovement : Movement
    {
        public const int OriginalWhite = 2; // Magic numbers like these obfuscate the semantics and make the code harder to maintain
        public const int OriginalBlack = 7;

        public override MoveValidationOutput IsMoveValid(IPiece piece, Cell src, Cell dst, Board board)
        {
            if (src.Y == OriginalWhite || src.Y == OriginalBlack)
            {
                if (Math.Abs(src.Y - dst.Y) > 2)
                    return new MoveValidationOutput
                    {
                        InvalidMessage = "pawn can only move 1 or 2 cell at a time for the first move"
                    };
            }
            else
            {
                if (Math.Abs(src.Y - dst.Y) > 1)
                    return new MoveValidationOutput
                    {
                        InvalidMessage = "pawn can only move 1 cell at a time"
                    };
            }


            if (board.ContainsKey(dst))
            {
                //capturing
                if(IsCapturingFriend(piece, dst, board))
                    return new MoveValidationOutput
                    {
                        InvalidMessage = "cannot capture sampe player"
                    };

                var xSrcInt = Convert.ToUInt16(src.X);
                var xDstInt = Convert.ToUInt16(dst.X);

                if (Math.Abs(xSrcInt - xDstInt) != 1)
                    //capture can only be done diagonal
                    return new MoveValidationOutput
                    {
                        InvalidMessage = "capture can only be done diagonal"
                    };

                return new MoveValidationOutput
                {
                    Valid = true,
                    Capture = true
                };
            }
            else
            {
                //non capturing
                var xSrcInt = Convert.ToUInt16(src.X);
                var xDstInt = Convert.ToUInt16(dst.X);

                if (Math.Abs(xSrcInt - xDstInt) != 0)
                    //move can only forward
                    return new MoveValidationOutput
                    {
                        InvalidMessage = "move can only forward"
                    };

                return new MoveValidationOutput
                {
                    Valid = true,
                    Capture = false
                };
            }
        }
    }

    public class KnightMovement : Movement
    {
        public override MoveValidationOutput IsMoveValid(IPiece piece, Cell src, Cell dst, Board board)
        {
            var xSrcInt = Convert.ToUInt16(src.X); // Quite a lot of redundant code between both Movement classes, surely we can 
            var xDstInt = Convert.ToUInt16(dst.X); // reduce the redundancy and apply DRY instead for the parts that bootstrap/prep
            var validMove = false;                 //  the data required for the rule evaluations
            if (Math.Abs(src.Y - dst.Y) == 1)
            {
                //forward 1, then left / right 2
                if (Math.Abs(xSrcInt - xDstInt) != 2)
                {
                    return new MoveValidationOutput
                    {
                        Valid = false,
                        InvalidMessage = "after forward 1, change direction must 2"
                    };
                }
                validMove = true;

            }
            else if (Math.Abs(src.Y - dst.Y) == 2)
            {
                if (Math.Abs(xSrcInt - xDstInt) != 1)
                {
                    return new MoveValidationOutput
                    {
                        Valid = false,
                        InvalidMessage = "after forward 1, change direction must 2"
                    };
                }
                validMove = true;
            }

            if(!validMove)
                return new MoveValidationOutput
                {
                    Valid = false,
                    InvalidMessage = "so wrong"
                };

            if (board.ContainsKey(dst))
            {
                if (IsCapturingFriend(piece, dst, board))
                    return new MoveValidationOutput
                    {
                        InvalidMessage = "cannot capture sampe player"
                    };

                return new MoveValidationOutput
                {
                    Valid = true,
                    Capture = true
                };
            }
            else
            {
                return new MoveValidationOutput
                {
                    Valid = true,
                };
            }
        }
    }


    public class MoveValidator // Creating new dictionary for movement rules smells like Movement can be defined 
    {                          // as a member of the Piece class instead of an external dict
        Dictionary<Type, Movement> MovementRules = new Dictionary<Type, Movement>();

        public MoveValidator()
        {
            MovementRules[typeof(Pawn)] = new PawnMovement();
            MovementRules[typeof(Knight)] = new KnightMovement();
        }


        public MoveValidationOutput IsMoveValid(EPlayer activePlayer, IPiece piece, Cell src, Cell dst, Board board)
        {
            if(piece.Player != activePlayer)
                return new MoveValidationOutput
                {
                    Valid = false,
                    InvalidMessage = "piece not players"
                };
            var rules = MovementRules[piece.GetType()];       // e.g., piece.GetRule().IsMoveValid(piece, src, dst, board) or 
            return rules.IsMoveValid(piece, src, dst, board); // straightforward piece.IsMoveValid(src, dst, board)
        }
    }
}
