using Microsoft.Extensions.DependencyInjection;

namespace MazeGame;

/// <summary>
/// The main Game object
/// </summary>
/// <param name="serviceProvider"></param>
public class Game(IServiceProvider serviceProvider)
{
	private readonly GameState _gameState = GameState.Load();
	private Maze? _maze;
	private readonly Player _player = new(Player.DefaultPosition);

	public void Start()
	{
		var quit = false;

		while (!quit) // outer game loop - until the player quits
		{
			var dimension = GetMazeDimension(_gameState.Level);
			FixConsoleBuffer(dimension);

			_player.Return();
			_maze = serviceProvider.GetRequiredService<Maze>();
			_maze.Generate(dimension);
			_maze.Draw();
			_maze.DrawFinish();
			_maze.DrawPlayer(_player);
			DrawMenu();

			var nextLevel = false;


			while (!quit && !nextLevel) // inner game loop (1 level)
			{
				var hasPlayerMoved = false;
				var keyInfo = Console.ReadKey(true);

				switch (keyInfo.Key)
				{
					case ConsoleKey.Q:
					case ConsoleKey.Escape:
						quit = true;
						FinishScreen();
						break;

					case ConsoleKey.S:
						_maze.Draw();
						_maze.DrawSolution();
						_maze.DrawFinish();
						_maze.DrawPlayer(_player);
						DrawMenu();
						break;

					case ConsoleKey.R:
						nextLevel = true;
						_gameState.Level = GameState.MinLevel;
						break;

					case ConsoleKey.LeftArrow:
						hasPlayerMoved = _maze.MoveLeft(_player);
						break;

					case ConsoleKey.RightArrow:
						hasPlayerMoved = _maze.MoveRight(_player);
						break;

					case ConsoleKey.UpArrow:
						hasPlayerMoved = _maze.MoveUp(_player);
						break;

					case ConsoleKey.DownArrow:
						hasPlayerMoved = _maze.MoveDown(_player);
						break;
				}

				if (hasPlayerMoved && _maze.IsAtTarget(_player))
				{
					nextLevel = true;
					GoToNextLevelScreen();

					if (_gameState.Level == GameState.MaxLevel)
					{
						FinishScreen();
						_gameState.Level = GameState.MinLevel;
					}
					else
					{
						_gameState.Level++;
					}
				}

				if (nextLevel)
				{
					_gameState.Save();
				}
			}
		}
	}

	public static int GetMazeDimension(int level)
	{
		return (int)Math.Pow(2, level + 1) + 1;
	}

	private void FixConsoleBuffer(int dimension)
	{
		// extra height for menu etc.
		var extraHeight = 10;

		while (dimension > Console.BufferWidth || dimension + extraHeight> Console.BufferHeight)
		{
			Console.Clear();
			Console.WriteLine("Apologies, the current level won't fit into your console window.\n" +
			                  "Please, zoom out and then press any key to continue or press R to revert to the first level.");
			var key = Console.ReadKey(true);
			if (key.Key == ConsoleKey.R)
			{
				_gameState.Level = GameState.MinLevel;
				_gameState.Save();
				return;
			}
		}
	}

	private static void DrawMenu()
	{
		Console.WriteLine("Arrow keys - navigate through the maze\n" +
		                  "S - show the solution\n" +
		                  "R - reset to level 1\n" +
		                  "Esc or Q - quit");
	}

	private static void FinishScreen()
	{
		Console.Clear();
		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.WriteLine("Thank you for playing! Press any Key to continue...");
		Console.ReadKey(true);
	}

	private void GoToNextLevelScreen()
	{
		Console.Clear();
		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.WriteLine($"Congratulations, you've finished level {_gameState.Level}! Press any Key to continue...");
		Console.ReadKey(true);
	}
}