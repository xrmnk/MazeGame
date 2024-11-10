using MazeGame.Generators;

namespace MazeGame.Tests;

public class RecursiveBacktrackGeneratorTests
{
	[Test]
	[TestCase(5)]
	[TestCase(9)]
	[TestCase(17)]
	[TestCase(33)]
	public void Generator_WhenGenerating_ThereAreWallsAndEmptySpacesAndMoreWallsThanEmptySpacesAndEmptySpaceNextToStart(int dimension)
	{
		var grid = new int[dimension, dimension];
		var generator = new RecursiveBacktrackGenerator();
		(int x, int y) start = (1, 1);

		generator.Generate(grid, dimension, start);

		var walls = 0;
		var empties = 0;

		for (var row = 0; row < dimension; row++)
		{
			for (var column = 0; column < dimension; column++)
			{
				if (grid[row, column] == Constants.Wall)
					walls++;
				if (grid[row, column] == Constants.Empty)
					empties++;
			}
		}

		Assert.Multiple(() =>
		{
			// the starting point is empty
			Assert.That(grid[start.x, start.y], Is.EqualTo(Constants.Empty));
			// cell below or cell to the right (or both) is empty
			Assert.That(grid[start.x + 1, start.y] + grid[start.x, start.y + 1], Is.GreaterThan(0));

			Assert.That(walls, Is.GreaterThan(0));
			Assert.That(empties, Is.GreaterThan(0));
			Assert.That(walls, Is.GreaterThan(empties));
		});
	}
}