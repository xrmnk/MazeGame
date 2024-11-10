using MazeGame.Generators;

namespace MazeGame.Tests;

public class DfsMazeSolverTests
{
	[Test]
	[TestCase(5)]
	[TestCase(9)]
	[TestCase(17)]
	[TestCase(33)]
	public void Solver_WhenSolving_PathExistsFromStartToTarget(int dimension)
	{
		var grid = new int[dimension, dimension];
		var generator = new RecursiveBacktrackGenerator();
		var solver = new DfsMazeSolver();
		(int x, int y) start = (1, 1);
		(int x, int y) target = (dimension - 2, dimension - 2);
		generator.Generate(grid, dimension, start);

		var path = solver.Solve(grid, dimension, start, target);

		Assert.Multiple(() =>
		{
			Assert.That(path, Is.Not.Empty);
			// we should see the starting position at the beginning of the path
			Assert.That(start, Is.EqualTo(path.First()));
			// we should see the target coordinates at the end of the path
			Assert.That(target, Is.EqualTo(path.Last()));
		});
	}

	[Test]
	[TestCase(5)]
	[TestCase(9)]
	[TestCase(17)]
	[TestCase(33)]
	public void Solver_WhenSolving_PathContainsNeighboringCoordinates(int dimension)
	{
		var grid = new int[dimension, dimension];
		var generator = new RecursiveBacktrackGenerator();
		var solver = new DfsMazeSolver();
		(int x, int y) start = (1, 1);
		(int x, int y) target = (dimension - 2, dimension - 2);
		generator.Generate(grid, dimension, start);

		var path = solver.Solve(grid, dimension, start, target);

		for (var i = 1; i < path.Count; i++)
		{
			var dx = path[i].Item1 - path[i - 1].Item1;
			var dy = path[i].Item2 - path[i - 1].Item2;

			Assert.That(Math.Abs(dx) + Math.Abs(dy), Is.EqualTo(1));
		}
	}
}