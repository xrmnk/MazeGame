namespace MazeGame.Generators;

public static class Extensions
{
	public static void Shuffle<T>(this IList<T> list, Random rng)
	{
		// we know that we're going to use this on 4 items only, but this is faster than
		// OrderBy(x => rng.Next()) anyway
		// https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle

		var n = list.Count;
		while (n > 1)
		{
			n--;
			var swapAt = rng.Next(n + 1);
			// swap using tuple
			(list[swapAt], list[n]) = (list[n], list[swapAt]);
		}
	}

	public static bool WithinBounds(this (int x, int y) coords, int dimension)
	{
		return coords is { x: >= 0, y: >= 0 } && coords.x < dimension && coords.y < dimension;
	}
}