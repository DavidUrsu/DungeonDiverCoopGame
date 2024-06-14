using DataStructures.PriorityQueue;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

// !TODO de facut ca camere sa fie unice
// !TODO daca un perete e la margine nu se plaseaza ca iese din harta
public class DungeonGenerator : MonoBehaviour
{
	// Scene tilemap
	public Tilemap sceneTilemap;

	// Wall tilemap
	public Tilemap wallTilemap;

	// Debug tileBase
	public TileBase pathTile;

	// Map seed
	public int seed = 0;

	// Map size
	public int SizeX = 10, SizeY = 10;

	// Spawn tries
	public int spawnTries = 10;

	// Chance to add an extra vertice to the minimum spanning tree
	public int chanceToAddAnExtraVertice = 100;

	//Shops gameobjects
	public GameObject[] shopRooms;

	//Boss room gameobjects
	public GameObject[] bossRooms;

	//Enemyrooms gameobjects
	public GameObject[] enemiesRooms;

	//Reviverooms gameobjects
	public GameObject[] reviveRooms;

	//Endroom gameobjects
	public GameObject[] endRooms;

	//Startroom gameobjects
	public GameObject[] startRooms;

	// Map array
	List<List<int>> mapArray;

	// Is painted array
	List<List<bool>> isPaintedArray;

	// Tile dictionary
	Dictionary<string, List<TileBase>> tileDictionary;

	// Players
	public GameObject players;

	public GameObject[] enemys;

	public GameObject buyableItemPrefab;

	public GameObject[] shopKeepers;

	public int bossesRemaining = 0;

	public GameObject portal;

	public GameObject[] bosses;

	public class RoomData
	{
		public string RoomType { get; set; }
		public int DistanceFromStart { get; set; }
		public List<Vector2Int> TilePositions { get; set; }
		public int ID { get; private set; }

		private static int nextID = 0;

		public RoomData(string roomType, int distanceFromStart = 0, List<Vector2Int> tilePositions = null)
		{
			RoomType = roomType;
			DistanceFromStart = distanceFromStart;
			TilePositions = tilePositions ?? new List<Vector2Int>();
			ID = nextID++;
		}

		public void AddTilePosition(Vector2Int tilePosition)
		{
			TilePositions.Add(tilePosition);
		}

		public void AddTilePositions(List<Vector2Int> tilePositions)
		{
			TilePositions.AddRange(tilePositions);
		}

		public void SetTilePositions(List<Vector2Int> tilePositions)
		{
			TilePositions = tilePositions;
		}

		public void SetDistanceFromStart(int distanceFromStart)
		{
			DistanceFromStart = distanceFromStart;
		}
	}

	public List<RoomData> roomsData = new List<RoomData>();

	// Choose a random room from the array and remove it from the array
	// Return the room
	public GameObject ChooseRandomRoomWithoutRepetition(GameObject[] rooms)
	{
		var roomList = new List<GameObject>(rooms);
		var randomIndex = Random.Range(0, roomList.Count);
		var chosenRoom = roomList[randomIndex];
		roomList.RemoveAt(randomIndex);
		rooms = roomList.ToArray();
		return chosenRoom;
	}

	// Choose a random room from the array without deleting it
	// Return the room
	public GameObject ChooseRandomRoom(GameObject[] rooms)
	{
		var randomIndex = Random.Range(0, rooms.Length);
		return rooms[randomIndex];
	}

	// For debug purposes print the map array to a .txt file
	public void PrintMapArray()
	{
		string path = "Assets/Debug/map.txt";

		//Write some text to the test.txt file
		System.IO.StreamWriter writer = new System.IO.StreamWriter(path);
		for (int j = SizeY - 1; j >= 0; j--)
		{
			for (int i = 0; i < SizeX; i++)
			{
				if (mapArray[i][j] == 0)
				{
					writer.Write(' ');
				}
				else
				{
					writer.Write(mapArray[i][j]);
				}
			}
			writer.WriteLine();
		}
		writer.Close();

	}

	public bool CheckRoomOverlap(GameObject roomPrefab, Vector3 position)
	{
		// Get the size of the room
		Tilemap tilemap = roomPrefab.GetComponent<Tilemap>();
		BoundsInt bounds = tilemap.cellBounds;

		Vector3 roomMin = bounds.min + position;
		Vector3 roomMax = bounds.max + position;

		// Iterate over the cells of the scene tilemap
		for (int x = Mathf.FloorToInt(roomMin.x); x <= Mathf.CeilToInt(roomMax.x); x++)
		{
			for (int y = Mathf.FloorToInt(roomMin.y); y <= Mathf.CeilToInt(roomMax.y); y++)
			{
				// Check if the current cell is within the bounds of the map
				if (x < 0 || x >= SizeX || y < 0 || y >= SizeY)
				{
					return true;
				}

				// Check if the current cell is occupied
				TileBase tile = sceneTilemap.GetTile(new Vector3Int(x, y, 0));
				if (tile != null)
				{
					return true;
				}
			}
		}

		return false;
	}

	public void DrawTheRoomOnTheTileMap(GameObject roomPrefab, Vector3 position)
	{
		// Get the size of the room
		Tilemap tilemap = roomPrefab.GetComponent<Tilemap>();
		BoundsInt bounds = tilemap.cellBounds;

		Vector3 roomMin = bounds.min + position;
		Vector3 roomMax = bounds.max + position;

		// Iterate over the cells of the scene tilemap
		for (int x = Mathf.FloorToInt(roomMin.x); x <= Mathf.CeilToInt(roomMax.x); x++)
		{
			for (int y = Mathf.FloorToInt(roomMin.y); y <= Mathf.CeilToInt(roomMax.y); y++)
			{
				// Copy the tile from the room tilemap to the scene tilemap
				TileBase tile = tilemap.GetTile(new Vector3Int(x - (int)position.x, y - (int)position.y, 0));

				sceneTilemap.SetTile(new Vector3Int(x, y, 0), tile);

				if (tile != null)
				{
					mapArray[x][y] = 1;
				}
			}
		}
	}

	public void GenerateRooms(GameObject[] rooms, int numberOfRoomsToSpawn, bool repetition, int roomMapSymbol = 1)
	{
		for (int i = 0; i < numberOfRoomsToSpawn; i++)
		{
			// Choose a random shop room from the shop rooms array
			GameObject roomToBeSpawned;
			if (repetition)
			{
				roomToBeSpawned = ChooseRandomRoom(rooms);
			}
			else
			{
				roomToBeSpawned = ChooseRandomRoomWithoutRepetition(rooms);
			}

			int thisRoomSpawnTries = spawnTries;
			while (thisRoomSpawnTries > 0)
			{
				// Choose a random position for the shop room
				float positionX = Random.Range(0, SizeX);
				float positionY = Random.Range(0, SizeY);

				var spawnPosition = new Vector3(positionX, positionY, 0f);

				// Check if the room overlaps with another room
				if (CheckRoomOverlap(roomToBeSpawned, spawnPosition))
				{
					// If the room overlaps with another room, try again
					thisRoomSpawnTries--;
				}
				else
				{
					// If the room does not overlap with another room, spawn the room
					// V0.1 
					// GameObject roomInstance = Instantiate(roomToBeSpawned, spawnPosition, Quaternion.identity);
					// roomInstance.transform.SetParent(gridParent.transform);


					// Mark the cells of the map array as occupied
					//MarkOccipiedCells(roomToBeSpawned, spawnPosition, roomMapSymbol);

					// V0.2
					// Spawn the room
					DrawTheRoomOnTheTileMap(roomToBeSpawned, spawnPosition);

					break;
				}
			}

			if (thisRoomSpawnTries == 0)
			{
				//Debug.Log($"Could not spawn room with symbol: {roomMapSymbol}");
			}
		}

	}

	// Generate a minimum spanning tree of the rooms
	// Return a list of vertices
	// chanceToAddAnExtraVertice - the probability of adding an extra vertice to the minimum spanning tree
	public List<List<Vector2Int>> MinimumSpaningTree(List<Vector2Int> roomsCenters, int chanceToAddAnExtraVertice)
	{
		List<List<Vector2Int>> minimumSpanningTree = new List<List<Vector2Int>>();

		List<int> visited = new List<int>();

		PriorityQueue<List<Vector2Int>, int> priorityQueue;
		priorityQueue = new PriorityQueue<List<Vector2Int>, int>(0);

		int idOfCurrentRoom = 0;
		while (visited.Count < roomsCenters.Count - 1)
		{
			visited.Add(idOfCurrentRoom);

			// Calculate the distance between the current room and all the other rooms
			// Add the vertices to the priority queue
			for (int i = 0; i < roomsCenters.Count; i++)
			{
				if (i == idOfCurrentRoom)
				{
					continue;
				}

				int distance = (int)Mathf.Sqrt(Mathf.Pow(roomsCenters[i].x - roomsCenters[idOfCurrentRoom].x, 2) + Mathf.Pow(roomsCenters[i].y - roomsCenters[idOfCurrentRoom].y, 2));

				priorityQueue.Insert(new List<Vector2Int> { roomsCenters[idOfCurrentRoom], roomsCenters[i] }, distance);
			}

			// Get the minimum distance
			List<Vector2Int> currentEdge = priorityQueue.Pop();

			// Check if the second room is already visited
			while (visited.Contains(roomsCenters.IndexOf(currentEdge[1])))
			{
				currentEdge = priorityQueue.Pop();
			}

			// Add the edge to the minimum spanning tree
			minimumSpanningTree.Add(currentEdge);

			// Get the id of the next room
			idOfCurrentRoom = roomsCenters.IndexOf(currentEdge[1]);
		}

		// Choose the second vertices of some rooms
		// Iterate over the rooms
		foreach (Vector2Int roomCenter in roomsCenters)
		{
			int minimumDistance = int.MaxValue;
			int secondMinimumDistance = int.MaxValue;
			Vector2Int closestRoom = new Vector2Int();
			Vector2Int secondClosestRoom = new Vector2Int();

			// Calculate the distance between the current room and all the other rooms
			// Add the closest room to the minimum spanning tree
			foreach (Vector2Int otherRoomCenter in roomsCenters)
			{
				if (roomCenter != otherRoomCenter)
				{
					int distance = (int)Mathf.Sqrt(Mathf.Pow(roomCenter.x - otherRoomCenter.x, 2) + Mathf.Pow(roomCenter.y - otherRoomCenter.y, 2));
					if (distance < minimumDistance)
					{
						// Update the second closest room before updating the closest
						secondMinimumDistance = minimumDistance;
						secondClosestRoom = closestRoom;

						// Update the closest room
						minimumDistance = distance;
						closestRoom = otherRoomCenter;
					}
					else if (distance < secondMinimumDistance && distance != minimumDistance)
					{
						// Update the second closest room
						secondMinimumDistance = distance;
						secondClosestRoom = otherRoomCenter;
					}
				}
			}

			// Add a new edge to the minimum spanning tree
			if (Random.Range(0, 100) < chanceToAddAnExtraVertice)
			{
				minimumSpanningTree.Add(new List<Vector2Int> { roomCenter, secondClosestRoom });
			}
		}

		return minimumSpanningTree;
	}

	public List<Vector2Int> GenerateListOfRoomsCenter()
	{
		int numberOfRooms = 0;
		List<Vector2Int> visited = new List<Vector2Int>();
		List<Vector2Int> roomsCenters = new List<Vector2Int>();

		Queue<Vector2Int> roomQueue = new Queue<Vector2Int>();

		// Iterate over the cells of the map array
		// When do I find a room, put the first cell in a queue, then start a bfs to find all the cells of the room
		for (int i = 0; i < SizeX; i++)
		{
			for (int j = 0; j < SizeY; j++)
			{
				if (!visited.Contains(new Vector2Int(i, j)) && mapArray[i][j] != 0)
				{
					// Get the cell type
					string roomType = sceneTilemap.GetTile(new Vector3Int(i, j, 0)).name;

					// Add the room to the list of rooms
					roomsData.Add(new RoomData(roomType));

					numberOfRooms++;
					int X = 0;
					int Y = 0;
					int numberOfCells = 0;

					roomQueue.Enqueue(new Vector2Int(i, j));
					while (roomQueue.Count > 0)
					{
						Vector2Int currentCell = roomQueue.Dequeue();
						if (visited.Contains(currentCell))
						{
							continue;
						}
						visited.Add(currentCell);
						roomsData[numberOfRooms - 1].AddTilePosition(currentCell);

						X += currentCell.x;
						Y += currentCell.y;
						numberOfCells++;

						// Check if the current cell is within the bounds of the map
						// Check if the current cell is occupied
						// Check if the current cell is not visited
						if (currentCell.x - 1 >= 0 && mapArray[currentCell.x - 1][currentCell.y] != 0 && !visited.Contains(new Vector2Int(currentCell.x - 1, currentCell.y)))
						{
							roomQueue.Enqueue(new Vector2Int(currentCell.x - 1, currentCell.y));
						}

						if (currentCell.x + 1 < SizeX && mapArray[currentCell.x + 1][currentCell.y] != 0 && !visited.Contains(new Vector2Int(currentCell.x + 1, currentCell.y)))
						{
							roomQueue.Enqueue(new Vector2Int(currentCell.x + 1, currentCell.y));
						}

						if (currentCell.y - 1 >= 0 && mapArray[currentCell.x][currentCell.y - 1] != 0 && !visited.Contains(new Vector2Int(currentCell.x, currentCell.y - 1)))
						{
							roomQueue.Enqueue(new Vector2Int(currentCell.x, currentCell.y - 1));
						}

						if (currentCell.y + 1 < SizeY && mapArray[currentCell.x][currentCell.y + 1] != 0 && !visited.Contains(new Vector2Int(currentCell.x, currentCell.y + 1)))
						{
							roomQueue.Enqueue(new Vector2Int(currentCell.x, currentCell.y + 1));
						}
					}
					// Calculate the center of the room
					int centerX = X / numberOfCells;
					int centerY = Y / numberOfCells;

					// Mark the center of the room
					roomsCenters.Add(new Vector2Int(centerX, centerY));
				}
			}
		}
		return roomsCenters;
	}

	public List<Vector2Int> BresenhamLine(Vector2Int p0, Vector2Int p1)
	{
		List<Vector2Int> points = new List<Vector2Int>();

		int dx = Mathf.Abs(p1.x - p0.x);
		int dy = Mathf.Abs(p1.y - p0.y);
		int sx = (p0.x < p1.x) ? 1 : -1;
		int sy = (p0.y < p1.y) ? 1 : -1;
		int err = dx - dy;

		while (true)
		{
			points.Add(p0);

			if (p0.x == p1.x && p0.y == p1.y) break;

			int e2 = 2 * err;
			if (e2 > -dy)
			{
				err -= dy;
				p0.x += sx;
			}
			if (e2 < dx)
			{
				err += dx;
				p0.y += sy;
			}
		}

		return points;
	}

	public void DrawThePathsBetweenRooms(List<List<Vector2Int>> corridors)
	{
		foreach (List<Vector2Int> corridor in corridors)
		{
			int leftTheRoom = 0;
			// When the path leaves the room, make leftTheRoom = 1
			// When a path enters a room, break the loop
			List<Vector2Int> linePoints = BresenhamLine(corridor[0], corridor[1]);
			foreach (Vector2Int point in linePoints)
			{
				// Check if the current cell is within the bounds of the map
				if (mapArray[point.x][point.y] == 0)
				{
					leftTheRoom = 1;
				}

				if (leftTheRoom == 1 && mapArray[point.x][point.y] != 0)
				{
					break;
				}

				// Draw a 3x3 square centered on the point
				for (int dx = -1; dx <= 1; dx++)
				{
					for (int dy = -1; dy <= 1; dy++)
					{
						int x = point.x + dx;
						int y = point.y + dy;

						// Check if the current cell is within the bounds of the map and if it is not occupied
						if (x >= 0 && x < SizeX && y >= 0 && y < SizeY && mapArray[x][y] == 0)
						{
							mapArray[x][y] = 9; // Or whatever value you want for the line

							// Mark the tile
							sceneTilemap.SetTile(new Vector3Int(x, y, 0), pathTile);
						}
					}
				}
			}
		}
	}

	public void GeneratePathsBetweenRooms()
	{
		List<Vector2Int> roomsCenters = GenerateListOfRoomsCenter();
		List<List<Vector2Int>> corridors = MinimumSpaningTree(roomsCenters, chanceToAddAnExtraVertice);

		DrawThePathsBetweenRooms(corridors);
	}

	public void LoadTiles()
	{
		// Initialize the isPaintedArray
		isPaintedArray = new List<List<bool>>();
		for (int i = 0; i < SizeX; i++)
		{
			isPaintedArray.Add(new List<bool>());
			for (int j = 0; j < SizeY; j++)
			{
				isPaintedArray[i].Add(false);
			}
		}

		tileDictionary = new Dictionary<string, List<TileBase>>();

		string[] tileDirectories = new string[] { "Boss", "End", "Enemy", "MainMenu", "Path", "Revive", "Shop", "Spawn", "Test Camere" };

		foreach (string tileDirectory in tileDirectories)
		{
			string tileCategory = Path.GetFileName(tileDirectory);
			string tileCategoryPath = "Tiles/" + tileCategory + '/';
			TileBase[] tiles = Resources.LoadAll<TileBase>(tileCategoryPath);

			// Add each tile to the corresponding category
			for (int i = 0; i < tiles.Length; i++)
			{
				// for each tile, extract the name before the underscore
				string[] parts = tiles[i].name.Split('_');
				string nameBeforeUnderscore = parts[0];
				string nameOfKey = tileCategory + "_" + nameBeforeUnderscore;

				// Check if the category exists in the dictionary
				if (!tileDictionary.ContainsKey(nameOfKey))
				{
					tileDictionary.Add(nameOfKey, new List<TileBase>());
					tileDictionary[nameOfKey].Add(tiles[i]);
				}
				else
				{
					tileDictionary[nameOfKey].Add(tiles[i]);
				}
			}
		}

	}

	public void PaintTheTile(string tileName, Vector3 position, string type)
	{
		List<TileBase> tiles;
		Tilemap targetTilemap = sceneTilemap; // Default to the sceneTilemap

		// If it is a top, also use the top files and the wall files
		if (type == "top")
		{
			// Check if the tile exists in the dictionary
			if (tileDictionary.ContainsKey(tileName + "_top"))
			{
				// Choose with a 15% to keep a top tile
				if (Random.Range(0, 100) > 15)
				{
					type = "wall";
				}
			} else
			{
				type = "wall";
			}
		}

		if (type == "wall")
		{
			targetTilemap = wallTilemap; // Use the wallTilemap for wall tiles
		}

		// Check if the tile exists in the dictionary
		if (!tileDictionary.ContainsKey(tileName + "_" + type))
		{
			Debug.Log($"Tile {tileName + "_" + type} not found in the dictionary");
			return;
		}

		tiles = tileDictionary[tileName + "_" + type];

		// Choose a random tile from the list
		int randomIndex = Random.Range(0, tiles.Count);
		targetTilemap.SetTile(new Vector3Int((int)position.x, (int)position.y, 0), tileDictionary[tileName + "_" + type][randomIndex]);
	}

	public Tilemap CopyOfTheTileMap()
	{
		// Create a new GameObject and add a Tilemap component to it
		GameObject newGameObject = new GameObject("CopiedTilemap");
		Tilemap newTilemap = newGameObject.AddComponent<Tilemap>();

		// Get the bounds of the original Tilemap
		BoundsInt bounds = sceneTilemap.cellBounds;

		// Copy the tiles from the original Tilemap to the new one
		foreach (Vector3Int position in bounds.allPositionsWithin)
		{
			newTilemap.SetTile(position, sceneTilemap.GetTile(position));
		}

		return newTilemap;
	}

	public void PaintTheMap()
	{
		//Get a copy of the initial tilemap
		Tilemap copyOfInitialTileMap = CopyOfTheTileMap();

		for (int i = 0; i < SizeX; i++)
		{
			for (int j = 0; j < SizeY; j++)
			{
				// Check if the current cell is already painted
				if (isPaintedArray[i][j])
				{
					continue;
				}

				TileBase title = copyOfInitialTileMap.GetTile(new Vector3Int(i, j, 0));
				string type = "floor";

				// Check if the cell is occupied
				if (title == null)
				{
					// Check if the cell is a wall
					for (int dx = -1; dx <= 1; dx++)
					{
						for (int dy = -1; dy <= 1; dy++)
						{
							if (i + dx < 0 || i + dx >= SizeX || j + dy < 0 || j + dy >= SizeY)
							{
								continue;
							}
							if (mapArray[i + dx][j + dy] != 0)
							{
								type = "wall";
								title = copyOfInitialTileMap.GetTile(new Vector3Int(i + dx, j + dy, 0));
								break;
							}
						}
						if (type == "wall")
						{
							break;
						}
					}
				}

				// Check if it is a top
				if (type == "wall" && j - 1 > 0 && mapArray[i][j - 1] != 0)
				{
					type = "top";
				}

				// If the cell still does not have a tile, continue
				if (title == null)
				{
					continue;
				}
				string tileName = title.name;
				string[] parts = tileName.Split('_');
				string nameBeforeUnderscore = parts[0];

				PaintTheTile(nameBeforeUnderscore, new Vector3(i, j, 0), type);
			}
		}
	}

	public List<Vector2> GetPositionsToSpawnPlayers(int numberOfPlayers)
	{
		// Find the Spawn Room in the roomsData
		RoomData spawnRoom = roomsData.Find(room => room.RoomType == "Spawn");

		List<Vector2> spawnRoomPositions = new List<Vector2>();

		while (spawnRoomPositions.Count < numberOfPlayers)
		{
			// Choose a random position from the room
			int randomIndex = Random.Range(0, spawnRoom.TilePositions.Count);
			Vector2 position = spawnRoom.TilePositions[randomIndex];

			// Check if the position is already occupied
			if (spawnRoomPositions.Contains(position))
			{
				continue;
			}

			spawnRoomPositions.Add(position);
		}

		return spawnRoomPositions;
	}


	public void SpawnPlayers()
	{
		//The good version in case of multiplayer
		// Get the number of players
		//int numberOfPlayers = players.transform.childCount;

		// Get the positions to spawn the players
		//List<Vector2> spawnPositions = GetPositionsToSpawnPlayers(numberOfPlayers);

		// Spawn the players
		//for (int i = 0; i < numberOfPlayers; i++)
		//{
		//Debug.Log(spawnPositions[i]);
		//Transform player = players.transform.GetChild(i);
		//player.position = new Vector3(spawnPositions[i].x, spawnPositions[i].y, 0);
		//}
		// This is just a shit hole

		// Get the selected character type from PlayerPrefs
		string selectedCharacter = PlayerPrefs.GetString("SelectedCharacter", "Mage");

		// Find the child prefab with the name matching the selected character
		Transform selectedPrefab = players.transform.Find(selectedCharacter);

		if (selectedPrefab != null)
		{
			// Instantiate or activate the selected prefab
			// Assuming you want to instantiate it at a specific position, you can modify this part
			List<Vector2> spawnPositions = GetPositionsToSpawnPlayers(1);
			selectedPrefab.position = new Vector3(spawnPositions[0].x, spawnPositions[0].y, 0);

			// Optionally set the instantiated player as a child of another GameObject in your scene
			// instantiatedPlayer.transform.SetParent(someParentTransform, false);
		}
		else
		{
			Debug.LogError("Selected character prefab not found among players' children.");
		}

		// Destroy or deactivate other prefabs
		foreach (Transform child in players.transform)
		{
			if (child != selectedPrefab)
			{
				Destroy(child.gameObject);
			}
		}
	}

	public int GenerateRandomEnemy()
    {
		int num = Random.Range(0, 100);

		if (num < 25)
			return 1;

        return 0;
    }
    public void SpawnEnemies()
	{
		// Iterate over the rooms
		// If the type of the room is "Enemy", select numberOfTiles/6 random tiles and spawn an enemy on each of them

		foreach (RoomData room in roomsData)
		{
			if (room.RoomType == "Enemy")
			{
				int numberOfTiles = room.TilePositions.Count;
				int numberOfEnemies = numberOfTiles / 10;

				for (int i = 0; i < numberOfEnemies; i++)
				{
					// Choose a random position from the room
					int randomIndex = Random.Range(0, room.TilePositions.Count);
					Vector2 position = room.TilePositions[randomIndex];

					// Spawn the enemy
					Instantiate(enemys[GenerateRandomEnemy()], new Vector3(position.x, position.y, 0), Quaternion.identity);
				}
			}
		}
	}

	public void GenerateShopItems()
	{
		foreach (RoomData room in roomsData)
		{
			if (room.RoomType == "Shop")
			{
				// Spawn a random number of shop items
				int numberOfItems = Random.Range(1, 4);
				for (int i = 0; i < numberOfItems; i++)
				{
					// Choose a random position from the room
					int randomIndex = Random.Range(0, room.TilePositions.Count);
					Vector2 position = room.TilePositions[randomIndex];

					// Spawn the shop item
					GameObject newBuyableItems = Instantiate(buyableItemPrefab, new Vector3(position.x, position.y, 0), Quaternion.identity);
				}

				// spawn the shopkeeper
				int randomIndexShopKeeper = Random.Range(0, shopKeepers.Length);
				// Choose a random position from the room
				int randomTileIndex = Random.Range(0, room.TilePositions.Count);
				Vector2 positionShopKeeper = room.TilePositions[randomTileIndex];

				// Spawn the shopkeeper
				GameObject shopKeeper = Instantiate(shopKeepers[randomIndexShopKeeper], new Vector3(positionShopKeeper.x, positionShopKeeper.y, 0), Quaternion.identity);
			}
		}
	}

	void SpawnBosses()
	{
		int bossIndex = 0;

		foreach(RoomData room in roomsData)
		{
			if (room.RoomType == "Boss")
			{
				// Choose a random position from the room
				int randomIndex = Random.Range(0, room.TilePositions.Count);
				Vector2 position = room.TilePositions[randomIndex];

				// Spawn the boss
				Instantiate(bosses[bossIndex], new Vector3(position.x, position.y, 0), Quaternion.identity);
				bossIndex++;
			}
		}
	}

	// Start is called before the first frame update
	void Start()
	{
		System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
		stopwatch.Start();

		// Set the seed
		if (seed == 0)
		{
			seed = Random.Range(0, 1000000);
		}
		Random.InitState(seed);

		// Initialize the map array
		mapArray = new List<List<int>>();

		// Initialize the map array with 0
		for (int i = 0; i < SizeX; i++)
		{
			mapArray.Add(new List<int>());
			for (int j = 0; j < SizeY; j++)
			{
				mapArray[i].Add(0);
			}
		}

		// Generate start room
		GenerateRooms(startRooms, 1, false, 1);

		// Generate end room
		GenerateRooms(endRooms, 1, true, 2);

		// Generate boss rooms
		GenerateRooms(bossRooms, 2, false, 3);
		bossesRemaining = 2;

		// Generate shop rooms
		GenerateRooms(shopRooms, 12, true, 4);

		// Generate revive rooms
		GenerateRooms(reviveRooms, 4, true, 5);

		// Generate enemy rooms
		GenerateRooms(enemiesRooms, 60, true, 6);

		GeneratePathsBetweenRooms();

		// PrintMapArray();

		// Paint the map with the tiles
		LoadTiles();
		PaintTheMap();

		// Set the spawn point of the players
		SpawnPlayers();

		// spawn the bosses
		SpawnBosses();

		// Generate shop items
		GenerateShopItems();

		// Spawn the enemies
		SpawnEnemies();

		stopwatch.Stop();
		//Debug.Log($"Generation time: {stopwatch.ElapsedMilliseconds} ms");
	}

	void Update()
	{
		// check if the bossesRemaning == 0 then spawns a portal
		if (bossesRemaining == 0)
		{
			// Find the End Room in the roomsData
			RoomData endRoom = roomsData.Find(room => room.RoomType == "End");

			// Choose a random position from the room
			int randomIndex = Random.Range(0, endRoom.TilePositions.Count);
			Vector2 position = endRoom.TilePositions[randomIndex];

			// Spawn the portal
			Instantiate(portal, new Vector3(position.x, position.y, 0), Quaternion.identity);

			bossesRemaining = -1;
		}

		// if the portal is spawned, spin the portal
		if (GameObject.Find("Portal(Clone)") != null)
		{
			GameObject portal = GameObject.Find("Portal(Clone)");
			portal.transform.Rotate(0, 0, 0.1f);
		}
	}
}