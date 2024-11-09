using WMPLib;

namespace MazeGame
{
	public static class Sountrack
	{
		public static void Start()
		{
			var player = new WindowsMediaPlayer
			{
				URL = "Resources\\blockman.mp3",
				settings = { autoStart = true }
			};

			player.settings.setMode("loop", true);
			player.controls.play();
		}
	}
}
