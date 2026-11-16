namespace AdventureGame.Tests;

public class ItemTests
{
	[Fact]
	public void Constructor_CreatesDistinctObjectsWithEqualNames()
	{
		Item first = new Item("Old Coin", "A tarnished coin.");
		Item second = new Item("Old Coin", "A tarnished coin.");

		Assert.Equal(first.Name, second.Name);
		Assert.Equal(first.Description, second.Description);
		Assert.NotSame(first, second);
	}

	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData("  ")]
	public void Constructor_RejectsBlankNames(string? name)
	{
		Assert.Throws<ArgumentException>(() => new Item(name!, "A collectible."));
	}

	[Fact]
	public void Room_StoresOriginalReferencesAndRejectsDuplicatePlacement()
	{
		var (_, gallery, _) = RoomTests.CreateDungeon();
		Item coin = new Item("Old Coin", "A tarnished coin.");
		Item ring = new Item("Copper Ring", "A copper ring.");

		gallery.PlaceItem(coin);
		gallery.PlaceItem(ring);

		Assert.Equal(2, gallery.Items.Count);
		Assert.Same(coin, gallery.Items[0]);
		Assert.Same(ring, gallery.Items[1]);
		Assert.Same(coin, gallery.FindItem("old coin"));
		Assert.Throws<InvalidOperationException>(() => gallery.PlaceItem(coin));
		Assert.Equal(2, gallery.Items.Count);
	}

	[Fact]
	public void PublicCollectionViews_RejectDirectAdditions()
	{
		var (_, gallery, _) = RoomTests.CreateDungeon();
		Item coin = new Item("Old Coin", "A tarnished coin.");
		Player player = new Player(gallery);
		IList<Item> roomView = (IList<Item>)gallery.Items;
		IList<Item> inventoryView = (IList<Item>)player.Inventory;

		Assert.Throws<NotSupportedException>(() => roomView.Add(coin));
		Assert.Throws<NotSupportedException>(() => inventoryView.Add(coin));
		Assert.Empty(gallery.Items);
		Assert.Empty(player.Inventory);
	}
}
