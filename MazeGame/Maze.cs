﻿using Ardalis.GuardClauses;
using MazeGame.Generators;
using Microsoft.Extensions.DependencyInjection;

namespace MazeGame;

/// <summary>
/// Encapsulating Maze operations, rendering of the maze and objects within
/// </summary>
public class Maze(IServiceProvider serviceProvider)
{
	private const string IconWall = "\u2588"; // 🧱
	private const string IconEmpty = " ";
	private const string IconFinish = "\u2691"; // 🚪
	private const string IconPlayer = "@"; // 😐
	private const string IconSolution = "."; // 👣
	private const ConsoleColor ColorWall = ConsoleColor.DarkGray;
	private const ConsoleColor ColorEmpty = ConsoleColor.Black;
	private const ConsoleColor ColorFinish = ConsoleColor.Green;
	private const ConsoleColor ColorPlayer = ConsoleColor.Red;
	private const ConsoleColor ColorSolution = ConsoleColor.DarkCyan;

	private readonly IMazeGenerator _generator = serviceProvider.GetRequiredService<IMazeGenerator>();
	private readonly IMazeSolver _solver = serviceProvider.GetRequiredService<IMazeSolver>();
	private int[,]? _grid;
	private int _dimension;

	private (int row, int column) FinishCoordinates => (_dimension - 2, _dimension - 2);

	public void Generate(int dimension)
	{
		_dimension = dimension;
		_grid = new int[_dimension, _dimension];
		_generator.Initialize(_grid, dimension);
		_generator.Generate();

		_solver.Initialize(_grid, dimension);
	}

	public void Draw()
	{
		Guard.Against.Null(_grid, "The maze has not been generated.");
		Guard.Against.Expression(d => d <= 0, _dimension, "The maze has not been generated.");

		Console.Clear();

		//DrawBorder();

		CursorToGridStart();

		for (var row = 0; row < _dimension; row++)
		{
			CursorToGridCoordinates((row, 0));

			for (var column = 0; column < _dimension; column++)
				if (_grid[row, column] == Constants.Wall)
				{
					Console.ForegroundColor = ColorWall;
					Console.Write(IconWall);
				}
				else
				{
					Console.ForegroundColor = ColorEmpty;
					Console.Write(IconEmpty);
				}
		}

		CursorToAfterGrid();
	}

	public void DrawSolution()
	{
		var solutionGrid = _solver.Solve();

		foreach ((int row, int column) coords in solutionGrid)
		{
			CursorToGridCoordinates((coords.row, coords.column));

			Console.ForegroundColor = ColorSolution;
			Console.Write(IconSolution);
		}
		
		CursorToAfterGrid();
	}

	public void DrawPlayer(Player player)
	{
		Guard.Against.Null(player);

		CursorToGridCoordinates(player.Coordinates);
		Console.ForegroundColor = ColorPlayer;
		Console.Write(IconPlayer);

		CursorToAfterGrid();
	}

	public void DrawFinish()
	{
		CursorToGridCoordinates(FinishCoordinates);
		Console.ForegroundColor = ColorFinish;
		Console.Write(IconFinish);

		CursorToAfterGrid();
	}

	public void DrawEmptySpace((int row, int column) coordinates)
	{
		CursorToGridCoordinates(coordinates);
		Console.ForegroundColor = ColorEmpty;
		Console.Write(IconEmpty);
	}

	private void CursorToAfterGrid()
	{
		Console.SetCursorPosition(0, _dimension + 2);
	}

	private void CursorToGridStart()
	{
		CursorToGridCoordinates((0, 0));
	}

	private void CursorToGridCoordinates((int row, int column) coordinates)
	{
		Guard.Against.OutOfRange(coordinates.row, nameof(coordinates.row), 0, _dimension - 1);
		Guard.Against.OutOfRange(coordinates.column, nameof(coordinates.column), 0, _dimension - 1);

		Console.SetCursorPosition(coordinates.column, coordinates.row);
	}

	public bool MoveLeft(Player player)
	{
		return MovePlayer(player, player.Coordinates.row, player.Coordinates.column - 1);
	}

	public bool MoveRight(Player player)
	{
		return MovePlayer(player, player.Coordinates.row, player.Coordinates.column + 1);
	}

	public bool MoveUp(Player player)
	{
		return MovePlayer(player, player.Coordinates.row - 1, player.Coordinates.column);
	}

	public bool MoveDown(Player player)
	{
		return MovePlayer(player, player.Coordinates.row + 1, player.Coordinates.column);
	}

	private bool MovePlayer(Player player, int row, int column)
	{
		Guard.Against.Null(player);
		
		// check grid boundaries and whether the new coordinate is empty
		if (row < 0 || column < 0 || row >= _dimension || column >= _dimension || _grid[row, column] != Constants.Empty)
		{
			// cannot move here. Do nothing.
			Console.Beep();
			return false;
		}

		DrawEmptySpace(player.Coordinates);

		player.Coordinates = (row, column);

		DrawPlayer(player);

		return true;
	}

	public bool IsAtFinish(Player player)
	{
		return player.Coordinates.row == FinishCoordinates.row && player.Coordinates.column == FinishCoordinates.column;
	}
}