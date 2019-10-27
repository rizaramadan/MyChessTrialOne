using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyChessTrialOne
{
    public class Games
    {
        public const int MaxBoard = 8;

        BoardPresenter BoardPresenter { get; set; }
        CapturedPresenter CapturedPresenter { get; set; }
        Board Board { get; }
        
        MoveExecutor MoveExecutor { get; set; }
        public EPlayer ActivePlayer { get; set; }
        public EPlayer? Winner { get; set; }

        public List<Piece> Captured { get; set; } = new List<Piece>();

        public Games(Board b, BoardPresenter bp, MoveExecutor me)
        {
            Board = b;
            BoardPresenter = bp;
            //MoveValidator = mv;
            MoveExecutor = me;
            ActivePlayer = EPlayer.White;
            CapturedPresenter = new CapturedPresenter();

        }

        public string GetActivePlayer()
        {
            return ActivePlayer == EPlayer.White
                ? "White"
                : "Black";
        }

        public void TogglePlayerTurn() => ActivePlayer = ActivePlayer == EPlayer.White ? EPlayer.Black : EPlayer.White;

        public bool WhiteKingCaptured() => Captured.Count(x => x.Player == EPlayer.White && x is King) == 1;

        public bool BlackKingCaptured() => Captured.Count(x => x.Player == EPlayer.White && x is King) == 1;

        public void Start()
        {
            Console.WriteLine("init board");
            //init pawn
            var pawnChar = 'P';
            for (var i = 0; i < MaxBoard; i++)
            {
                var theX = Board.StartOfX.Increase(i);
                Board[new Cell { X = theX, Y = Board.StartOfY + 1 }] = new Pawn { Player = EPlayer.White , BoardChar = pawnChar };
                Board[new Cell { X = theX, Y = Board.EndOfY - 1 }] = new Pawn { Player = EPlayer.Black, BoardChar = char.ToLower(pawnChar) };
            }
            //init rock
            var rockChar = 'R';
            Board[new Cell { X = Board.StartOfX, Y = Board.StartOfY }] = new Rock { Player = EPlayer.White, BoardChar = rockChar };
            Board[new Cell { X = Board.EndOfX, Y = Board.StartOfY }] = new Rock { Player = EPlayer.White, BoardChar = rockChar };
            Board[new Cell { X = Board.StartOfX, Y = Board.EndOfY }] = new Rock { Player = EPlayer.Black, BoardChar = char.ToLower(rockChar) };
            Board[new Cell { X = Board.EndOfX, Y = Board.EndOfY }] = new Rock { Player = EPlayer.Black, BoardChar = char.ToLower(rockChar) };
            //init knight 
            var knightChar = 'T';
            Board[new Cell { X = 'b', Y = Board.StartOfY }] = new Knight { Player = EPlayer.White, BoardChar = knightChar };
            Board[new Cell { X = 'g', Y = Board.StartOfY }] = new Knight { Player = EPlayer.White, BoardChar = knightChar };
            Board[new Cell { X = 'b', Y = Board.EndOfY }] = new Knight { Player = EPlayer.Black, BoardChar = char.ToLower(knightChar) };
            Board[new Cell { X = 'g', Y = Board.EndOfY }] = new Knight { Player = EPlayer.Black, BoardChar = char.ToLower(knightChar) };
            //init bishop 
            var bishopChar = 'B';
            Board[new Cell { X = 'c', Y = Board.StartOfY }] = new Bishop { Player = EPlayer.White, BoardChar = bishopChar };
            Board[new Cell { X = 'f', Y = Board.StartOfY }] = new Bishop { Player = EPlayer.White, BoardChar = bishopChar };
            Board[new Cell { X = 'c', Y = Board.EndOfY }] = new Bishop { Player = EPlayer.Black, BoardChar = char.ToLower(bishopChar) };
            Board[new Cell { X = 'f', Y = Board.EndOfY }] = new Bishop { Player = EPlayer.Black, BoardChar = char.ToLower(bishopChar) };
            //init queen 
            var queenChar = 'Q';
            Board[new Cell { X = 'd', Y = Board.StartOfY }] = new Queen { Player = EPlayer.White, BoardChar = queenChar };
            Board[new Cell { X = 'd', Y = Board.EndOfY }] = new Queen { Player = EPlayer.Black, BoardChar = char.ToLower(queenChar) };
            //init king 
            var kingChar = 'K';
            Board[new Cell { X = Board.KingOriginalX, Y = Board.StartOfY }] = new King { Player = EPlayer.White, BoardChar = kingChar };
            Board[new Cell { X = Board.KingOriginalX, Y = Board.EndOfY }] = new King { Player = EPlayer.Black, BoardChar = char.ToLower(kingChar) };

            Board.Values.ToList().ForEach(x => x.InitMovement());

            BoardPresenter.Print(Board);

        }

        public void Move(Cell src, Cell dst)
        {
            Console.WriteLine($"move from {src} to {dst}");
            var piece = Board.ContainsKey(src) ? Board[src] : null;
            if (piece == null)
            {
                Console.WriteLine($"no piece at {src}");
            }
            else
            {
                var context = new MoveValidationContext { ActivePlayer = ActivePlayer, Src = src, Board = Board, Piece = piece };
                piece.ValidatingMovement(context);
                var moveOutput = context.Find(dst);
                if (moveOutput != null)
                {
                    MoveExecutor.ExecuteMove
                    (
                        new MoveExecutionContext
                        {
                            Type = moveOutput.Type, Src = src, Dst = moveOutput.Cell, Piece = piece, Board = Board, Captured = Captured
                        }
                    );
                    TogglePlayerTurn();

                    if (WhiteKingCaptured())
                        Winner = EPlayer.Black;
                    else if (BlackKingCaptured())
                        Winner = EPlayer.White;
                }
                else
                {
                    Console.WriteLine($"no valid move from {src} to {dst}, reason: {context.InvalidMessage}");
                }
            }
            BoardPresenter.Print(Board);
            CapturedPresenter.Print(Captured);
        }


        public bool HasWinner()
        {
            return Winner != null;
        }


        public EPlayer GetWinner()
        {
            return Winner.Value;
        }
    }
}
