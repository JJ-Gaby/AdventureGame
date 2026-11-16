using static AdventureGame.Room;

namespace AdventureGame;

public class Program
{
	public const string QUIT = "QUIT";
	public const string LOOK = "LOOK";
	public const string GO = "GO";
	public const string TAKE = "TAKE";
	public const string DROP = "DROP";
	public const string INVENTORY = "INVENTORY";

	public static void Main()
	{
		Room entrance = new Room("Entrance", "A dry stone chamber with daylight spilling through the doorway.");
		Room gallery = new Room("Gallery", "A long gallery lined with faded banners.");
		Room archive = new Room("Archive", "An archive whose shelves hold dust-covered chronicles.", false);

		entrance.Connect(EAST, gallery);
		gallery.Connect(NORTH, archive);

		Item oldCoin = new Item("Old Coin", "A tarnished coin stamped with a forgotten crest.");
		Item copperRing = new Item("Copper Ring", "A small ring with a dull copper finish.");
		Item brassToken = new Item("Brass Token", "A square brass token etched with a small tower.");

		gallery.PlaceItem(oldCoin);
		gallery.PlaceItem(copperRing);
		archive.PlaceItem(brassToken);

		Player player = new Player(entrance);

		Console.WriteLine("Adventure Game — Darkness and Grue Lab");
		Console.WriteLine($"Commands: {GO} <direction>, {LOOK}, {TAKE} <item name>, {DROP} <item name>, {INVENTORY}, {QUIT}.");

		ShowRoom(player.CurrentRoom);

		while (true)
		{
			Console.Write("> ");
			string normalized = NormalizeInput(Console.ReadLine());

			if (normalized.Length == 0)
			{
				Console.WriteLine($"Enter {GO} <direction>, {LOOK}, {TAKE} <item name>, {DROP} <item name>, {INVENTORY}, or {QUIT}.");
				continue;
			}

			string[] words = normalized.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
			string command = words[0];
			string target = words.Length > 1 ? string.Join(" ", words, 1, words.Length - 1) : "";
			string? syntaxError = null;

			// Phase 1: recognize the verb and check argument structure only.
			if (command == QUIT || command == LOOK || command == INVENTORY)
			{
				if (words.Length != 1)
				{
					syntaxError = $"{command} takes no arguments.";
				}
			}
			else if (command == GO)
			{
				if (words.Length != 2)
				{
					syntaxError = $"Use {GO} followed by one direction.";
				}
			}
			else if (command == TAKE || command == DROP)
			{
				if (words.Length < 2)
				{
					syntaxError = $"Use {command} followed by an item name.";
				}
			}
			else
			{
				syntaxError = $"Unknown command. Use {GO} <direction>, {LOOK}, {TAKE} <item name>, " +
					$"{DROP} <item name>, {INVENTORY}, or {QUIT}.";
			}

			if (syntaxError is not null)
			{
				Console.WriteLine(syntaxError);
				continue;
			}

			// Phase 2: identify safe exceptions and resolve the Grue hazard.
			bool safeInDarkness = command == QUIT || (command == GO && target == player.ReturnDirection);

			if (!player.CurrentRoom.IsLit && !safeInDarkness)
			{
				Console.WriteLine(DescribeGrueEncounter(command));
				Console.WriteLine("A Grue devours you. You have been defeated.");
				break;
			}

			// Phase 3: let existing domain operations decide normal success or failure.
			if (command == QUIT)
			{
				Console.WriteLine("You leave the dungeon.");
				break;
			}
			else if (command == LOOK)
			{
				ShowRoom(player.CurrentRoom);
			}
			else if (command == INVENTORY)
			{
				ShowInventory(player);
			}
			else if (command == GO)
			{
				bool moved = player.TryMove(target, out string message);
				Console.WriteLine(message);

				if (moved)
				{
					ShowRoom(player.CurrentRoom);
				}
				else
				{
					Console.WriteLine($"You remain in {player.CurrentRoom.Name}.");
				}
			}
			else if (command == TAKE)
			{
				player.TryTake(target, out string message);
				Console.WriteLine(message);
			}
			else if (command == DROP)
			{
				player.TryDrop(target, out string message);
				Console.WriteLine(message);
			}
		}
	}

	public static string NormalizeInput(string? input)
	{
		return input?.Trim().ToUpperInvariant() ?? QUIT;
	}

	public static void ShowRoom(Room room)
	{
		if (!room.IsLit)
		{
			Console.WriteLine("It is dark. You cannot see the room or its contents.");
			return;
		}

		Console.WriteLine($"{room.Name}: {room.Description}");
		Console.WriteLine($"  {NORTH}: {room.North?.Name ?? "(none)"}");
		Console.WriteLine($"  {SOUTH}: {room.South?.Name ?? "(none)"}");
		Console.WriteLine($"   {EAST}: {room.East?.Name ?? "(none)"}");
		Console.WriteLine($"   {WEST}: {room.West?.Name ?? "(none)"}");

		if (room.Items.Count == 0)
		{
			Console.WriteLine("  Items: (none)");
			return;
		}

		string itemList = string.Join(", ", room.Items.Select(item => item.Name));
		Console.WriteLine($"  Items: {itemList}");
	}

	public static void ShowInventory(Player player)
	{
		if (player.Inventory.Count == 0)
		{
			Console.WriteLine("Inventory: (empty)");
			return;
		}

		Console.WriteLine("Inventory:");

		foreach (Item item in player.Inventory)
		{
			Console.WriteLine($"  - {item.Name}: {item.Description}");
		}
	}

	private static string DescribeGrueEncounter(string command)
	{
		if (command == GO)
		{
			return "You rush blindly ahead and stumble upon an angry woken-up beast.";
		}

		if (command == INVENTORY)
		{
			return "You rummage through your belongings looking for something, but something else finds you first.";
		}

		if (command == DROP)
		{
			return "You drop one of your belongings. The sound it makes echoes and serves as a beacon for the thing that lurks in the darkness.";
		}

		if (command == TAKE)
		{
			return "You reach into the blackness. Something reaches back.";
		}

		// if (command == LOOK)
		return "As you look into the darkness, the darkness stares back at you.";
	}
}
