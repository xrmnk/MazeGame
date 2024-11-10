namespace MazeGame.Generators;

public interface IMazeSolver
{
	List<(int, int)> Solve(int[,] grid, int dimension, (int x, int y) startingPoint, (int x, int y) targetCoordinates);
}