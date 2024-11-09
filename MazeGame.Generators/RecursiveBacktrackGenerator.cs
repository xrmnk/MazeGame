using Ardalis.GuardClauses;

namespace MazeGame.Generators;

public class RecursiveBacktrackGenerator : IMazeGenerator
{
	private int[,]? _grid;
	private int _dimension;
	private Random _random = new();

	public void Initialize(int[,] grid, int dimension)
	{
		_dimension = dimension;
		_grid = grid;

		_random = new Random(DateTime.Now.Millisecond);
	}

	public void Generate()
	{
		Guard.Against.Null(_grid, "Generator has not been initialized.");
		Guard.Against.Expression(d => d <= 0, _dimension, "Generator has not been initialized.");

		GenerateInternal(0, 0);
	}


	private void GenerateInternal(int x, int y)
	{
		// Directions (right, down, left, up)
		var directions = new List<(int dx, int dy)>
		{
			(1, 0), (0, 1), (-1, 0), (0, -1)
		};
		directions.Shuffle(_random); // Randomize directions

		foreach (var (dx, dy) in directions)
		{
			var nx = x + dx * 2;
			var ny = y + dy * 2;

			if (IsInBounds(nx, ny) && _grid[nx, ny] == 0)
			{
				_grid[x + dx, y + dy] = Constants.Empty;
				_grid[nx, ny] = Constants.Empty;
				GenerateInternal(nx, ny);
			}
		}
	}

	private bool IsInBounds(int x, int y) =>
		x >= 0 && y >= 0 && x < _dimension && y < _dimension;
}

public static class Extensions
{
	public static void Shuffle<T>(this IList<T> list, Random rng)
	{
		for (var i = list.Count - 1; i > 0; i--)
		{
			var swapIndex = rng.Next(i + 1);
			(list[i], list[swapIndex]) = (list[swapIndex], list[i]);
		}
	}
}