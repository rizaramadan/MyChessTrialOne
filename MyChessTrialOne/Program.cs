using System;

namespace MyChessTrialOne
{
    class Program
    {
        static void Main(string[] args)
        {
            Games games = new Games(new Board(), new BoardPresenter(), new MoveValidator(), new MoveExecutor());
            games.Start();
            while (!games.HasWinner())
            {
                Console.Write($"input move for player {games.GetActivePlayer()} (ex e2-e4):");
                var command = Console.ReadLine();
                var moves = CommandParser.Parse(command);
                games.Move(moves[CommandParser.SrcIdx], moves[CommandParser.DstIdx]);
            }
            Console.WriteLine($"Winner is {games.GetWinner()}");
        }
    }
}
