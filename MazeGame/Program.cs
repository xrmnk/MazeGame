using MazeGame.Generators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MazeGame;

internal class Program
{
	private static async Task Main()
	{
		Console.OutputEncoding = System.Text.Encoding.UTF8;

		// Introducing DI here only in order to have the option of resolving different maze generators
		
		var serviceProvider = CreateServices();
		var game = serviceProvider.GetRequiredService<Game>();

		var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
		logger.LogInformation("MazeGame starting.");

		try
		{
			// start the game in one thread and let the soundtrack play in the other one.
			// no need to use the cancellation token to cancel the soundtrack when game finishes in this case

			var task1 = Task.Factory.StartNew(game.Start);
			var task2 = Task.Factory.StartNew(Sountrack.Start);

			await Task.WhenAll(task1, task2);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "An error has occurred: {message}", ex.Message);
			Console.ResetColor();
			Console.Clear();
			Console.WriteLine("An error has occurred. Details have been logged into the Windows Event Log. Press any key to exit.");
			Console.ReadKey(true);
		}
	}

	private static ServiceProvider CreateServices()
	{
		var serviceProvider = new ServiceCollection()
			.AddSingleton(implementationFactory: sp => new Game(sp))
			.AddSingleton(implementationFactory: sp => new Maze(sp))
			.AddSingleton<IMazeGenerator,RecursiveBacktrackGenerator>()
			.AddSingleton<IMazeSolver, DfsMazeSolver>()
			.AddLogging(builder => builder.AddEventLog())
			.BuildServiceProvider();

		return serviceProvider;
	}
}
