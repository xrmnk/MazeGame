using MazeGame.Generators;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace MazeGame.Tests;

public class MazeTests
{
	private IMazeGenerator _generator;
	private IMazeSolver _solver;
	private IServiceProvider _sp;

	[SetUp]
	public void Setup()
	{
		_generator = Substitute.For<IMazeGenerator>();
		_solver = Substitute.For<IMazeSolver>();
		_sp = new ServiceCollection()
			.AddSingleton(_generator)
			.AddSingleton(_solver)
			.BuildServiceProvider();
	}

	[Test]
	public void Maze_WhenPlayerAtTarget_AndMazeNotGenerated_ThrowsArgumentException()
	{
		var maze = new Maze(_sp);
		var player = new Player((0, 0));

		Assert.Throws<ArgumentException>(() => maze.IsAtTarget(player));
	}

	[Test]
	public void Maze_WhenPlayerAtTarget_AndMazeGenerated_ReturnsTrue()
	{
		const int dimension = 16;
			
		var maze = new Maze(_sp);
		maze.Generate(dimension);
		var player = new Player((dimension - 2, dimension - 2));

		var result = maze.IsAtTarget(player);

		Assert.That(result, Is.True);
	}

	[Test]
	public void Maze_WhenMovingPlayer_AndMazeNotGenerated_ThrowsArgumentNullException()
	{
		var maze = new Maze(_sp);
		var player = new Player((0, 0));

		Assert.Throws<ArgumentNullException>(() => maze.MoveLeft(player));
	}
}