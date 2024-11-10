# MazeGame

Game has 5 levels, from 1 to 5. It is controlled with keyboard, menu is displayed.

Uses recursive backtracker algorithm for generation and depth-first search for solving.

## Dependencies justification

- `Ardalis.GuardClauses` - validation, argument checking
- `Microsoft.Extensions.DependencyInjection` - used to allow introduction of alternative algorithms, plus logging
- `Microsoft.Extensions.Logging` - for logging to the EventLog
- `NSubstitute` - used for mocking in unit tests
- `NUnit` - test framework of choice

## Drawbacks

- not optimal with console
- for simplicity, works with equal dimensions - could use more of the screen space otherwise

## Sountrack attribution

Music from #Uppbeat (free for Creators!):
https://uppbeat.io/t/kevin-macleod/blockman
License code: LOGIMKATJHWZRUVF

