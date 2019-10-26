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
        Board Board { get; }
        MoveValidator MoveValidator { get; set; }
        MoveExecutor MoveExecutor { get; set; }
        public EPlayer ActivePlayer { get; set; }
        public EPlayer? Winner { get; set; }

        public List<IPiece> Captured { get; set; } = new List<IPiece>();

        public Games(Board b, BoardPresenter bp, MoveValidator mv, MoveExecutor me)
        {
            Board = b;
            BoardPresenter = bp;
            MoveValidator = mv;
            MoveExecutor = me;
            ActivePlayer = EPlayer.White;
        }

        public string GetActivePlayer()
        {
            return ActivePlayer == EPlayer.White
                ? "White"
                : "Black";
        }

        public void Start()
        {
            Console.WriteLine("init board");
            var startX = 'a';
            //init pawn
            for (var i = 0; i < MaxBoard; i++)
            {
                var theX = (char)(Convert.ToUInt16(startX) + i);
                Board[new Cell { X = theX, Y = 2 }] = new Pawn { Player = EPlayer.White };
                Board[new Cell { X = theX, Y = 7 }] = new Pawn { Player = EPlayer.Black };
            }
            //init rock
            Board[new Cell { X = 'a', Y = 1 }] = new Rock { Player = EPlayer.White };
            Board[new Cell { X = 'h', Y = 1 }] = new Rock { Player = EPlayer.White };
            Board[new Cell { X = 'a', Y = 8 }] = new Rock { Player = EPlayer.Black };
            Board[new Cell { X = 'h', Y = 8 }] = new Rock { Player = EPlayer.Black };
            //init knight a b c d e f g h
            Board[new Cell { X = 'b', Y = 1 }] = new Knight { Player = EPlayer.White };
            Board[new Cell { X = 'g', Y = 1 }] = new Knight { Player = EPlayer.White };
            Board[new Cell { X = 'b', Y = 8 }] = new Knight { Player = EPlayer.Black };
            Board[new Cell { X = 'g', Y = 8 }] = new Knight { Player = EPlayer.Black };
            //init bishop a b c d e f g h
            Board[new Cell { X = 'c', Y = 1 }] = new Bishop { Player = EPlayer.White };
            Board[new Cell { X = 'f', Y = 1 }] = new Bishop { Player = EPlayer.White };
            Board[new Cell { X = 'c', Y = 8 }] = new Bishop { Player = EPlayer.Black };
            Board[new Cell { X = 'f', Y = 8 }] = new Bishop { Player = EPlayer.Black };
            //init queen a b c d e f g h
            Board[new Cell { X = 'd', Y = 1 }] = new Queen { Player = EPlayer.White };
            Board[new Cell { X = 'd', Y = 8 }] = new Queen { Player = EPlayer.Black };
            //init king a b c d e f g h
            Board[new Cell { X = 'e', Y = 1 }] = new King { Player = EPlayer.White };
            Board[new Cell { X = 'e', Y = 8 }] = new King { Player = EPlayer.Black };

            BoardPresenter.Print(Board);

        }

        public void Move(Cell src, Cell dst)
        {
            Console.WriteLine($"move from {src} to {dst}");
            //get piece
            var piece = Board.ContainsKey(src)
                ? Board[src]
                : null;

            if (piece == null)
            {
                Console.WriteLine($"no piece at {src}");
            }
            else
            {
                //is move valid?
                var validationOutput = MoveValidator.IsMoveValid(ActivePlayer, piece, src, dst, Board);
                if (!validationOutput.Valid)
                {
                    Console.WriteLine($"piece at {src} cannot move to {dst}, {validationOutput.InvalidMessage}");
                }
                else
                {
                    //execute move
                    MoveExecutor.ExecuteMove(validationOutput, piece, src, dst, Board, Captured);
                    //toggle player
                    ActivePlayer = ActivePlayer == EPlayer.White ? EPlayer.Black : EPlayer.White;

                    if (Captured.Count(x => x.Player == EPlayer.White && x is King) == 1)
                    {
                        Winner = EPlayer.Black;
                    }
                    else if (Captured.Count(x => x.Player == EPlayer.White && x is King) == 1)
                    {
                        Winner = EPlayer.White;
                    }
                }
            }
            BoardPresenter.Print(Board);
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
