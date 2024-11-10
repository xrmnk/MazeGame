using Ardalis.GuardClauses;

namespace MazeGame.Generators;

/// <summary>
/// A kind of recursive backtracker algorithm which moves 2 cells at time in random direction and carves its way if possible
/// therefore works best with odd dimensions to draw border
/// </summary>
public class RecursiveBacktrackGenerator : IMazeGenerator
{
	private int[,]? _grid;
	private int _dimension;
	private Random _random = new();

	/// <summary>
	/// Generate the maze in given grid, of given dimension and with given starting point
	/// </summary>
	public void Generate(int[,] grid, int dimension, (int x, int y) startingPoint)
	{
		Guard.Against.Null(grid);
		Guard.Against.OutOfRange(dimension, nameof(dimension), 2, 10240);

		_dimension = dimension;
		_grid = grid;
		// if randomness would be an issue, we could use RNGCryptoServiceProvider, but I think it'll do for now.
		_random = new Random(DateTime.Now.Millisecond);

		GenerateInternal(startingPoint.x, startingPoint.y);
	}

	private void GenerateInternal(int x, int y)
	{
		var directions = new List<(int dx, int dy)>
		{
			(1, 0), (0, 1), (-1, 0), (0, -1)
		};
		directions.Shuffle(_random);

		foreach (var (dx, dy) in directions)
		{
			// take cell skipping one in a random direction
			var nx = x + dx * 2;
			var ny = y + dy * 2;

			// if the cell is within the grid and it is the "wall", then let's carve the way and recurse from here
			if ((nx, ny).WithinBounds(_dimension) && _grid[nx, ny] == Constants.Wall)
			{
				_grid[x + dx, y + dy] = Constants.Empty;
				_grid[nx, ny] = Constants.Empty;
				GenerateInternal(nx, ny);
			}
		}
	}
}