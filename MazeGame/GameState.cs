using System.Text.Json;
using Ardalis.GuardClauses;

namespace MazeGame;

/// <summary>
/// Handles the game state and persists it into a file
/// </summary>
public class GameState
{
	public const int MinLevel = 1;
	public const int MaxLevel = 5;

	private const string StateFileName = "state.json";

	private int _level = MinLevel;

	public int Level
	{
		get => _level;
		set
		{
			Guard.Against.OutOfRange(value, nameof(value), MinLevel, MaxLevel);
			_level = value;
		}
	}

	public static GameState Load()
	{
		GameState? state;

		if (!File.Exists(StateFileName))
		{
			state = new GameState();
			state.Save();
			return state;
		}

		var json = File.ReadAllText(StateFileName);

		// System.Text.Json seems to perform better than Newtonsoft
		state = JsonSerializer.Deserialize<GameState>(json);

		Guard.Against.Null(state);
		Guard.Against.OutOfRange(state.Level, nameof(state.Level), MinLevel, MaxLevel);

		return state;
	}

	public void Save()
	{
		var json = JsonSerializer.Serialize(this);
		File.WriteAllText(StateFileName, json);
	}
}