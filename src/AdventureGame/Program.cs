using static AdventureGame.Room;

namespace AdventureGame;

public class Program
{
	public const string QUIT = "QUIT";
	public const string LOOK = "LOOK";
	public const string GO = "GO";

	public static void Main()
	{
		Room entrance = new Room("Entrance", "A dry stone chamber with daylight spilling through the doorway.");
		Room gallery = new Room("Gallery", "A long gallery lined with faded banners.");
		Room archive = new Room("Archive", "An archive whose shelves hold dust-covered chronicles.");

		entrance.Connect(EAST, gallery);
		gallery.Connect(NORTH, archive);

		Player player = new Player(entrance);

		Console.WriteLine("Adventure Game — Movement Lab");
		Console.WriteLine($"Commands: {GO} <direction>, {LOOK}, {QUIT}.");

		ShowRoom(player.CurrentRoom);

		while (true)
		{
			Console.Write("> ");

			string normalized = NormalizeInput(Console.ReadLine());

			if (normalized.Length == 0)
			{
				Console.WriteLine($"Enter {GO} <direction>, {LOOK}, or {QUIT}.");
				continue;
			}

			string[] words = normalized.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
			string command = words[0];

			if (command == QUIT)
			{
				if (words.Length != 1)
				{
					Console.WriteLine($"{QUIT} takes no arguments.");
					continue;
				}

				Console.WriteLine("You leave the dungeon.");
				break;
			}

			if (command == LOOK)
			{
				if (words.Length != 1)
				{
					Console.WriteLine($"{LOOK} takes no arguments.");
					continue;
				}

				ShowRoom(player.CurrentRoom);
				continue;
			}

			if (command == GO)
			{
				if (words.Length != 2)
				{
					Console.WriteLine($"Use {GO} followed by one direction: {NORTH}, {SOUTH}, {EAST}, or {WEST}.");
					continue;
				}

				string direction = words[1];
				bool moved = player.TryMove(direction, out string message);
				Console.WriteLine(message);

				if (moved)
				{
					ShowRoom(player.CurrentRoom);
				}
				else
				{
					Console.WriteLine($"You remain in {player.CurrentRoom.Name}.");
				}

				continue;
			}

			Console.WriteLine($"Unknown command. Use {GO} <direction>, {LOOK}, or {QUIT}.");
		}
	}

	public static string NormalizeInput(string? input)
	{
		return input?.Trim().ToUpperInvariant() ?? QUIT;
	}

	public static void ShowRoom(Room room)
	{
		Console.WriteLine($"{room.Name}: {room.Description}");
		Console.WriteLine($"  {NORTH}: {room.North?.Name ?? "(none)"}");
		Console.WriteLine($"  {SOUTH}: {room.South?.Name ?? "(none)"}");
		Console.WriteLine($"   {EAST}: {room.East?.Name ?? "(none)"}");
		Console.WriteLine($"   {WEST}: {room.West?.Name ?? "(none)"}");
	}
}
