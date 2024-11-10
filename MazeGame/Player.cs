namespace MazeGame;

public class Player((int row, int column) coordinates)
{
	public static readonly (int row, int column) DefaultPosition = (1, 1);

	public (int row, int column) Coordinates { get; set; } = coordinates;

	public void Return()
	{
		Coordinates = DefaultPosition;
	}
}