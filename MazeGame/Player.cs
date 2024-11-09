namespace MazeGame;

public class Player(int row, int column)
{
	public (int row, int column) Coordinates { get; set; } = (row, column);

	public void Return()
	{
		Coordinates = (0, 0);
	}
}