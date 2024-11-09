namespace MazeGame.Generators
{
	public interface IMazeGenerator
	{
		void Initialize(int[,] grid, int dimension);

		void Generate();
	}
}
