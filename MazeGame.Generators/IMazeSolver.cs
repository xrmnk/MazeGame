namespace MazeGame.Generators;

public interface IMazeSolver
{
	void Initialize(int[,] grid, int dimension);

	List<(int, int)> Solve();
}