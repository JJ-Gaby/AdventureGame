using static AdventureGame.Room;

namespace AdventureGame.Tests;

public class RoomTests
{
	public static (Room Entrance, Room Gallery, Room Archive) CreateDungeon()
	{
		Room entrance = new Room("Entrance", "A dry stone chamber with daylight spilling through the doorway.");
		Room gallery = new Room("Gallery", "A long gallery lined with faded banners.");
		Room archive = new Room("Archive", "An archive whose shelves hold dust-covered chronicles.");

		entrance.Connect(EAST, gallery);
		gallery.Connect(NORTH, archive);

		return (entrance, gallery, archive);
	}

	[Fact]
	public void CreateDungeon_CreatesThreeDistinctRooms()
	{
		var (entrance, gallery, archive) = CreateDungeon();

		Assert.NotSame(entrance, gallery);
		Assert.NotSame(entrance, archive);
		Assert.NotSame(gallery, archive);
	}

	[Fact]
	public void CreateDungeon_EntranceEastAndArchiveSouthPointToTheSameGalleryInstance()
	{
		var (entrance, gallery, archive) = CreateDungeon();

		Assert.Same(gallery, entrance.East);
		Assert.Same(gallery, archive.South);
	}

	[Fact]
	public void CreateDungeon_ConnectsEntranceAndGalleryBothWays()
	{
		var (entrance, gallery, _) = CreateDungeon();

		Assert.Same(entrance, gallery.West);
		Assert.Same(gallery, entrance.East);
	}

	[Fact]
	public void CreateDungeon_LeavesMissingGalleryExitsNull()
	{
		var (_, gallery, _) = CreateDungeon();

		Assert.Null(gallery.East);
		Assert.Null(gallery.South);
	}

	[Fact]
	public void ReadingNeighbor_DoesNotChangeGalleryNorthConnection()
	{
		var (_, gallery, archive) = CreateDungeon();
		Room? originalNorth = gallery.North;

		string? neighborName = gallery.North?.Name;

		Assert.Equal("Archive", neighborName);

		Assert.Same(archive, originalNorth);
		Assert.Same(originalNorth, gallery.North);
	}
}
