namespace AdventureGame;

public class Room
{
	public const string NORTH = "NORTH";
	public const string SOUTH = "SOUTH";
	public const string EAST = "EAST";
	public const string WEST = "WEST";

	public string Name { get; }
	public string Description { get; set; }

	public bool IsLit { get; }

	private readonly List<Item> _items = new List<Item>();

	public IReadOnlyList<Item> Items => _items.AsReadOnly();

	public Room? North { get; private set; }
	public Room? South { get; private set; }
	public Room? East { get; private set; }
	public Room? West { get; private set; }

	public Room(string name, string description) : this(name, description, true)
	{

	}

	public Room(string name, string description, bool isLit)
	{
		Name = name;
		Description = description;
		IsLit = isLit;
	}

	public Room? GetNeighbor(string direction)
	{
		return direction switch
		{
			NORTH => North,
			SOUTH => South,
			EAST => East,
			WEST => West,
			_ => null
		};
	}

	public static string GetReciprocalDirection(string? direction)
	{
		return direction switch
		{
			NORTH => SOUTH,
			SOUTH => NORTH,
			EAST => WEST,
			WEST => EAST,
			_ => throw new ArgumentException($"Use {NORTH}, {SOUTH}, {EAST}, or {WEST}.", nameof(direction))
		};
	}

	public void Connect(string direction, Room room)
	{
		ArgumentNullException.ThrowIfNull(room, nameof(room));

		if (ReferenceEquals(this, room))
		{
			throw new InvalidOperationException("A room cannot be its own neighbor.");
		}

		string? reciprocalDirection = GetReciprocalDirection(direction);

		Room? thereFromHere = this.GetNeighbor(direction);
		Room? hereFromThere = room.GetNeighbor(reciprocalDirection);

		if (thereFromHere is not null || hereFromThere is not null)
		{
			throw new InvalidOperationException("One or both rooms have a preexisting connection.");
		}

		this.SetNeighbor(direction, room);
		room.SetNeighbor(reciprocalDirection, this);
	}

	private void SetNeighbor(string direction, Room neighbor)
	{
		switch (direction)
		{
			case NORTH: North = neighbor; break;
			case SOUTH: South = neighbor; break;
			case EAST: East = neighbor; break;
			case WEST: West = neighbor; break;
			default: throw new ArgumentException($"Use {NORTH}, {SOUTH}, {EAST}, or {WEST}.", nameof(direction));
		}
	}
	public void PlaceItem(Item item)
	{
		ArgumentNullException.ThrowIfNull(item);

		if (_items.Exists(current => ReferenceEquals(current, item)))
		{
			throw new InvalidOperationException("This room already contains the item.");
		}

		_items.Add(item);
	}

	public void RemoveItem(Item item)
	{
		ArgumentNullException.ThrowIfNull(item);

		int itemIndex = _items.FindIndex(current => ReferenceEquals(current, item));

		if (itemIndex < 0)
		{
			throw new InvalidOperationException("This room does not contain the item.");
		}

		_items.RemoveAt(itemIndex);
	}

	public Item? FindItem(string itemName)
	{
		return _items.Find(item => string.Equals(item.Name, itemName, StringComparison.OrdinalIgnoreCase));
	}
}
