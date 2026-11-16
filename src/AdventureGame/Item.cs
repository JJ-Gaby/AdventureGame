namespace AdventureGame;

public class Item
{
	public string Name { get; }
	public string Description { get; }

	public Item(string name, string description)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			throw new ArgumentException("Item name cannot be blank.", nameof(name));
		}

		Name = name;
		Description = description;
	}
}
