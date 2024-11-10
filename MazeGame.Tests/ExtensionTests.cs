using MazeGame.Generators;

namespace MazeGame.Tests;

public class ExtensionTests
{
	[Test]
	[TestCase(0, 0, 2, true)]
	[TestCase(0, 1, 2, true)]
	[TestCase(1, 0, 2, true)]
	[TestCase(1, 1, 2, true)]
	[TestCase(0, 2, 2, false)]
	[TestCase(2, 0, 2, false)]
	[TestCase(-1, -1, 2, false)]
	[TestCase(3, 3, 2, false)]
	public void Coordinates_WhenCheckedForBoundaries_ReturnCorrectResult(int x, int y, int dimension, bool expected)
	{
		(int x, int y) coords = new(x, y);
		var actual = coords.WithinBounds(dimension);

		Assert.That(actual, Is.EqualTo(expected));
	}

	[Test]
	public void List_WhenShuffled_AtLeastHalfItemsAreShuffled()
	{
		var list = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
		
		list.Shuffle(new Random(DateTime.Now.Millisecond));

		var shuffled = 0;

		for (var i = 0; i < list.Count; i++)
		{
			if (i != list[i]) shuffled++;
		}

		// as this is pseudo-random, cannot really rely on *all* items being shuffled, but most of them should
		Assert.That(shuffled, Is.GreaterThan((int)list.Count/2));
	}
}