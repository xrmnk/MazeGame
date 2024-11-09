using Ardalis.GuardClauses;

namespace MazeGame.Generators;

public class DfsMazeSolver : IMazeSolver
{
	private int _dimension;
	private int[,] _grid;
	private int[,] _tempGrid;

	public void Initialize(int[,] grid, int dimension)
	{
		_dimension = dimension;
		_grid = grid;
	}

	public List<(int, int)> Solve()
	{
		_tempGrid = new int[_dimension, _dimension];

		for (var x = 0; x < _dimension; x++)
		{
			for (var y = 0; y < _dimension; y++)
			{
				_tempGrid[x, y] = _grid[x, y];
			}
		}

		Guard.Against.Null(_grid, "Solver has not been initialized.");
		Guard.Against.Expression(d => d <= 0, _dimension, "Solver has not been initialized.");

		var path = new List<(int, int)>();
		SolveInternal(0, 0, path);
		return path;
	}

	private bool SolveInternal(int x, int y, List<(int, int)> path)
	{
		if (x == _dimension - 2 && y == _dimension - 2)
		{
			path.Add((x, y));
			return true;
		}

		if (!IsInBounds(x, y) || _tempGrid[x, y] == Constants.Wall || _tempGrid[x, y] == 2)
			return false;

		_tempGrid[x, y] = 2;
		path.Add((x, y));

		var directions = new List<(int dx, int dy)>
		{
			(1, 0), (0, 1), (-1, 0), (0, -1)
		};

		foreach (var (dx, dy) in directions)
		{
			var nx = x + dx;
			var ny = y + dy;

			if (SolveInternal(nx, ny, path))
				return true;
		}

		path.RemoveAt(path.Count - 1);
		return false;
	}

	private bool IsInBounds(int x, int y) =>
		x >= 0 && y >= 0 && x < _dimension && y < _dimension;
}