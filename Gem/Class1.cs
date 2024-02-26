using System;
using System.Runtime.ConstrainedExecution;

public class Position
{
    public int X { get; set; }
    public int Y { get; set; }
    //Position Constructor
    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }
}

public class Player
{
    public string Name { get; }
    public Position Position { get; set; }
    public int GemCount { get; set; }
    //Player Constructor
    public Player(string name, Position position)
    {
        Name = name;
        Position = position;
        GemCount = 0;
    }
    //Updating player's Direction based on input.
    public void Move(char direction)
    {
        switch (direction)
        {
            case 'U':
                Position.Y--;
                break;
            case 'D':
                Position.Y++;
                break;
            case 'L':
                Position.X--;
                break;
            case 'R':
                Position.X++;
                break;
        }
    }
}

public class Cell
{
    public string Occupant { get; set; }

    public Cell(string occupant)
    {
        Occupant = occupant;
    }
}

public class Board
{
    public Cell[,] Grid { get; }

    public Board()
    {
        Grid = new Cell[6, 6];
        // Initialize the board with empty spaces
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                Grid[i, j] = new Cell("-");
            }
        }
        Random random = new Random();

        // Place players
        Grid[0, 0].Occupant = "P1";
        Grid[5, 5].Occupant = "P2";

        // Place gems
        for (int i = 0; i < 5; i++)
        {
            int x = random.Next(1, 5);
            int y = random.Next(1, 5);
            Grid[x, y].Occupant = "G";
        }

        // Place obstacles
        for (int i = 0; i < 2; i++)
        {
            int x = random.Next(1, 5);
            int y = random.Next(1, 5);
            Grid[x, y].Occupant = "O";
        }
    }
    //Current position of the board
    public void Display()
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                Console.Write(Grid[i, j].Occupant + " ");
            }
            Console.WriteLine();
        }
    }
    //Checks the valid Move
    public bool IsValidMove(Player player, char direction)
    {
        int newX = player.Position.X;
        int newY = player.Position.Y;

        switch (direction)
        {
            case 'U':
                newY--;
                break;
            case 'D':
                newY++;
                break;
            case 'L':
                newX--;
                break;
            case 'R':
                newX++;
                break;
        }

        // Check if the new position is within the bounds and not an obstacle
        return newX >= 0 && newX < 6 && newY >= 0 && newY < 6 && Grid[newY, newX].Occupant != "O";
    }
    //Checks if the player's new position contains a gem and updates the player's GemCount.
    public void CollectGem(Player player)
    {
        if (Grid[player.Position.Y, player.Position.X].Occupant == "G")
        {
            player.GemCount++;
            Grid[player.Position.Y, player.Position.X].Occupant = "-";
        }
    }
}

public class Game
{
    public Board Board { get; }
    public Player Player1 { get; }
    public Player Player2 { get; }
    public Player CurrentTurn { get; private set; }
    public int TotalTurns { get; private set; }

    public Game()
    {
        Board = new Board();
        Player1 = new Player("P1", new Position(0, 0));
        Player2 = new Player("P2", new Position(5, 5));
        CurrentTurn = Player1;
        TotalTurns = 0;
    }
    //game begins, start game
    public void Start()
    {
        while (!IsGameOver())
        {


            Board.Display();
            Console.WriteLine($"{CurrentTurn.Name}'s turn Enter move (U/D/L/R): ");
            char move = Char.ToUpper(Console.ReadKey().KeyChar);
            Console.WriteLine();
            if (Board.IsValidMove(CurrentTurn, move))
            {
                CurrentTurn.Move(move);
                Board.CollectGem(CurrentTurn);
                SwitchTurn();
                TotalTurns++;
            }
            else
            {
                Console.WriteLine("Invalid move! Try again.");
            }
        }

        AnnounceWinner();
    }
    //Switches between player1 and Player2
    public void SwitchTurn()
    {
        CurrentTurn = (CurrentTurn == Player1) ? Player2 : Player1;
    }
    //Checks if the game has reached its end condition.
    public bool IsGameOver()
    {
        return TotalTurns >= 30;
    }
    //Announce the game-winner based on the GemCount of both player
    public void AnnounceWinner()
    {
        Board.Display();
        Console.WriteLine("Game Over!");
        Console.WriteLine($"Player 1 Gems: {Player1.GemCount}");
        Console.WriteLine($"Player 2 Gems: {Player2.GemCount}");

        if (Player1.GemCount > Player2.GemCount)
        {
            Console.WriteLine("Player 1 wins!");
        }
        else if (Player1.GemCount < Player2.GemCount)
        {
            Console.WriteLine("Player 2 wins!");
        }
        else
        {
            Console.WriteLine("It's a tie!");
        }
    }
}

class Program
{
    public static void Main()
    {
        Game gemHunters = new Game();
        gemHunters.Start();
    }
}
