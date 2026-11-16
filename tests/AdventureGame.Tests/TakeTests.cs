using static AdventureGame.Room;

namespace AdventureGame.Tests;

public class TakeTests
{
	private static (Room Gallery, Room Archive, Player Player, Item Coin, Item Ring, Item Token) CreateScenario()
	{
		var (entrance, gallery, archive) = RoomTests.CreateDungeon();
		Item coin = new Item("Old Coin", "A tarnished coin.");
		Item ring = new Item("Copper Ring", "A copper ring.");
		Item token = new Item("Brass Token", "A brass token.");
		gallery.PlaceItem(coin);
		gallery.PlaceItem(ring);
		archive.PlaceItem(token);
		return (gallery, archive, new Player(entrance), coin, ring, token);
	}

	[Fact]
	public void TryTake_CarriesOriginalItemsFromDifferentRooms()
	{
		var (gallery, archive, player, coin, ring, token) = CreateScenario();

		Assert.True(player.TryMove(EAST, out _));
		Assert.True(player.TryTake("old coin", out _));
		Assert.True(player.TryMove(NORTH, out _));
		Assert.True(player.TryTake("BRASS TOKEN", out _));

		Assert.Single(gallery.Items);
		Assert.Same(ring, gallery.Items[0]);
		Assert.Empty(archive.Items);
		Assert.Equal(2, player.Inventory.Count);
		Assert.Same(coin, player.Inventory[0]);
		Assert.Same(token, player.Inventory[1]);
	}

	[Fact]
	public void TryTake_WithMatchingNamesTransfersTheFirstReference()
	{
		var (_, gallery, _) = RoomTests.CreateDungeon();
		Item firstCoin = new Item("Old Coin", "The first coin.");
		Item secondCoin = new Item("Old Coin", "The second coin.");
		gallery.PlaceItem(firstCoin);
		gallery.PlaceItem(secondCoin);
		Player player = new Player(gallery);

		Assert.Same(firstCoin, gallery.FindItem("old coin"));
		Assert.True(player.TryTake("OLD COIN", out _));
		Assert.Single(player.Inventory);
		Assert.Same(firstCoin, player.Inventory[0]);
		Assert.Single(gallery.Items);
		Assert.Same(secondCoin, gallery.Items[0]);
	}

	[Fact]
	public void TryDrop_PersistsThroughTravelAndCanBeRetaken()
	{
		var (gallery, archive, player, coin, ring, token) = CreateScenario();

		Assert.True(player.TryMove(EAST, out _));
		Assert.True(player.TryTake("Old Coin", out _));
		Assert.True(player.TryMove(NORTH, out _));
		Assert.True(player.TryTake("Brass Token", out _));
		Assert.True(player.TryDrop("old coin", out _));

		Assert.Single(archive.Items);
		Assert.Same(coin, archive.Items[0]);
		Assert.Single(player.Inventory);
		Assert.Same(token, player.Inventory[0]);
		Assert.True(player.TryMove(SOUTH, out _));
		Assert.True(player.TryMove(NORTH, out _));
		Assert.Same(coin, archive.Items[0]);
		Assert.True(player.TryTake("Old Coin", out _));
		Assert.Empty(archive.Items);
		Assert.Same(token, player.Inventory[0]);
		Assert.Same(coin, player.Inventory[1]);
		Assert.Same(ring, gallery.Items[0]);
	}

	[Fact]
	public void RejectedTakeAndDrop_PreserveExistingState()
	{
		var (gallery, _, player, coin, ring, _) = CreateScenario();
		Room startingRoom = player.CurrentRoom;

		Assert.False(player.TryTake("Old Coin", out _));
		Assert.False(player.TryTake("  ", out _));
		Assert.False(player.TryDrop(null!, out _));
		Assert.False(player.TryDrop("Old Coin", out _));
		Assert.Same(startingRoom, player.CurrentRoom);
		Assert.Same(coin, gallery.Items[0]);
		Assert.Same(ring, gallery.Items[1]);
		Assert.Empty(player.Inventory);

		Assert.True(player.TryMove(EAST, out _));
		Assert.True(player.TryTake("Old Coin", out _));
		Assert.False(player.TryTake("Old Coin", out _));
		Assert.True(player.TryDrop("Old Coin", out _));
		Assert.False(player.TryDrop("Old Coin", out _));
		Assert.Empty(player.Inventory);
		Assert.Same(coin, gallery.Items[1]);
	}

	[Fact]
	public void RetainedReadOnlyViews_ReflectCompletedTransfers()
	{
		var (gallery, _, player, coin, ring, _) = CreateScenario();
		Assert.True(player.TryMove(EAST, out _));
		IReadOnlyList<Item> roomView = gallery.Items;
		IReadOnlyList<Item> inventoryView = player.Inventory;

		Assert.True(player.TryTake("Old Coin", out _));
		Assert.Single(roomView);
		Assert.Same(ring, roomView[0]);
		Assert.Same(coin, inventoryView[0]);
		Assert.True(player.TryDrop("Old Coin", out _));
		Assert.Equal(2, roomView.Count);
		Assert.Same(coin, roomView[1]);
		Assert.Empty(inventoryView);
	}
}
