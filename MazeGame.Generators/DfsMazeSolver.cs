using Ardalis.GuardClauses;

namespace MazeGame.Generators;

/// <summary>
/// Depth-First Search algorithm (https://en.wikipedia.org/wiki/Depth-first_search)
/// </summary>
public class DfsMazeSolver : IMazeSolver
{
	private const int Visited = -1;

	private int _dimension;
	private int[,] _tempGrid;
	private (int x, int y) _targetCoordinates;

	public List<(int, int)> Solve(int[,] grid, int dimension, (int x, int y) startingPoint, (int x, int y) targetCoordinates)
	{
		Guard.Against.Null(grid);
		Guard.Against.OutOfRange(dimension, nameof(dimension), 2, 10240);

		_dimension = dimension;
		_targetCoordinates = targetCoordinates;
		_tempGrid = new int[_dimension, _dimension];

		// let's copy the grid to a temporary one, where we will overwrite cells which we already visited.
		for (var x = 0; x < _dimension; x++)
		{
			for (var y = 0; y < _dimension; y++)
			{
				_tempGrid[x, y] = grid[x, y];
			}
		}

		var path = new List<(int, int)>();
		SolveInternal(startingPoint.x, startingPoint.y, path);
		return path;
	}

	private bool SolveInternal(int x, int y, List<(int, int)> path)
	{
		if (x == _targetCoordinates.x && y == _targetCoordinates.y)
		{
			// we have reached the target, save the coordinates and return true
			path.Add((x, y));
			return true;
		}

		if (!(x, y).WithinBounds(_dimension) || _tempGrid[x, y] == Constants.Wall || _tempGrid[x, y] == Visited)
		{
			// we are either out of the grid, or the cell we're looking at is a wall - or we already have visited it.
			return false;
		}

		// we have visited this cell, so let's add its coordinates to the path
		_tempGrid[x, y] = Visited;
		path.Add((x, y));

		var directions = new List<(int dx, int dy)>
		{
			(1, 0), (0, 1), (-1, 0), (0, -1)
		};

		// from every cell, we will try to go each way and recurse from there
		foreach (var (dx, dy) in directions)
		{
			var nx = x + dx;
			var ny = y + dy;

			if (SolveInternal(nx, ny, path))
				return true;
		}

		// after all directions were examined, let's step back and remove this cell from the path
		path.RemoveAt(path.Count - 1);
		return false;
	}
}