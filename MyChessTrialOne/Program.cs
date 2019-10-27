using System;

namespace MyChessTrialOne
{
    public class Program
    {
        static void Main(string[] args)
        {
            Games games = new Games(new Board(), new BoardPresenter(),  new MoveExecutor());
            games.Start();
            while (!games.HasWinner())
            {
                Console.Write($"input move for player {games.GetActivePlayer()} (ex e2-e4):");
                var command = Console.ReadLine();
                var moves = CommandParser.Parse(command);
                if (moves.IsValid())
                    games.Move(moves.Src, moves.Dst);
                else
                    Console.WriteLine("input is not valid");
            }
            Console.WriteLine($"Winner is {games.GetWinner()}");
        }
    }
}
