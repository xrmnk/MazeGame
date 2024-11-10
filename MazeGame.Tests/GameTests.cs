namespace MazeGame.Tests
{
	public class GameTests
	{
		[Test]
		[TestCase(1, 5)]
		[TestCase(2, 9)]
		[TestCase(3, 17)]
		[TestCase(4, 33)]
		[TestCase(5, 65)]
		public void Game_WhenLevelProvided_ReturnsCorrectDimension(int level, int expectedDimension)
		{
			var actualDimension = Game.GetMazeDimension(level);
			
			Assert.That(actualDimension, Is.EqualTo(expectedDimension));
		}
	}
}