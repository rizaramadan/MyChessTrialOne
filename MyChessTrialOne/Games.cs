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
            var startX = 'a';
            //init pawn
            for (var i = 0; i < MaxBoard; i++)
            {
                var theX = (char)(Convert.ToUInt16(startX) + i);
                Board[new Cell { X = theX, Y = 2 }] = new Pawn { Player = EPlayer.White , BoardChar = 'P' };
                Board[new Cell { X = theX, Y = 7 }] = new Pawn { Player = EPlayer.Black, BoardChar = 'p' };
            }
            //init rock
            var rockChar = 'R';
            Board[new Cell { X = 'a', Y = 1 }] = new Rock { Player = EPlayer.White, BoardChar = rockChar };
            Board[new Cell { X = 'h', Y = 1 }] = new Rock { Player = EPlayer.White, BoardChar = rockChar };
            Board[new Cell { X = 'a', Y = 8 }] = new Rock { Player = EPlayer.Black, BoardChar = char.ToLower(rockChar) };
            Board[new Cell { X = 'h', Y = 8 }] = new Rock { Player = EPlayer.Black, BoardChar = char.ToLower(rockChar) };
            //init knight 
            var knightChar = 'T';
            Board[new Cell { X = 'b', Y = 1 }] = new Knight { Player = EPlayer.White, BoardChar = knightChar };
            Board[new Cell { X = 'g', Y = 1 }] = new Knight { Player = EPlayer.White, BoardChar = knightChar };
            Board[new Cell { X = 'b', Y = 8 }] = new Knight { Player = EPlayer.Black, BoardChar = char.ToLower(knightChar) };
            Board[new Cell { X = 'g', Y = 8 }] = new Knight { Player = EPlayer.Black, BoardChar = char.ToLower(knightChar) };
            //init bishop 
            var bishopChar = 'B';
            Board[new Cell { X = 'c', Y = 1 }] = new Bishop { Player = EPlayer.White, BoardChar = bishopChar };
            Board[new Cell { X = 'f', Y = 1 }] = new Bishop { Player = EPlayer.White, BoardChar = bishopChar };
            Board[new Cell { X = 'c', Y = 8 }] = new Bishop { Player = EPlayer.Black, BoardChar = char.ToLower(bishopChar) };
            Board[new Cell { X = 'f', Y = 8 }] = new Bishop { Player = EPlayer.Black, BoardChar = char.ToLower(bishopChar) };
            //init queen 
            Board[new Cell { X = 'd', Y = 1 }] = new Queen { Player = EPlayer.White, BoardChar = 'Q'};
            Board[new Cell { X = 'd', Y = 8 }] = new Queen { Player = EPlayer.Black, BoardChar = 'q'};
            //init king 
            Board[new Cell { X = 'e', Y = 1 }] = new King { Player = EPlayer.White, BoardChar = 'K'};
            Board[new Cell { X = 'e', Y = 8 }] = new King { Player = EPlayer.Black, BoardChar = 'k' };

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
                        new MoveExecutionInput
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
