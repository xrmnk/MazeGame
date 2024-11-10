namespace MazeGame.Generators
{
	public interface IMazeGenerator
	{
		void Generate(int[,] grid, int dimension, (int x, int y) startingPoint);
	}
}
