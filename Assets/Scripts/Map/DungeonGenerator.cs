using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    private int width, height;
    private int maxRoomSize, minRoomSize;
    private int maxRooms;
    private int maxEnemies;
    private int maxItems;
    private int currentFloor;

    private List<string> enemyNames = new List<string>() {
        "Beetle", "Wasp", "Snake", "RedWasp", "Witch", "ClawBeast", "BatBeast", "DragonMan", "Angel", "Cyclops"
    };

    List<Room> rooms = new List<Room>();

    public void SetSize(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    public void SetRoomSize(int min, int max)
    {
        minRoomSize = min;
        maxRoomSize = max;
    }

    public void SetMaxRooms(int max)
    {
        maxRooms = max;
    }

    public void SetMaxEnemies(int max)
    {
        maxEnemies = max;
    }

    public void SetMaxItems(int max)
    {
        maxItems = max;
    }

    public void SetCurrentFloor(int floor)
    {
        currentFloor = floor;
    }

    public void Generate()
    {
        rooms.Clear();

        for (int roomNum = 0; roomNum < maxRooms; roomNum++)
        {
            int roomWidth = Random.Range(minRoomSize, maxRoomSize);
            int roomHeight = Random.Range(minRoomSize, maxRoomSize);

            int roomX = Random.Range(0, width - roomWidth - 1);
            int roomY = Random.Range(0, height - roomHeight - 1);

            var room = new Room(roomX, roomY, roomWidth, roomHeight);

            // Als de kamer overlapt met een andere kamer, verwijder deze
            if (room.Overlaps(rooms))
            {
                continue;
            }

            // Voeg tegels toe om de kamer zichtbaar te maken op de tilemap
            for (int x = roomX; x < roomX + roomWidth; x++)
            {
                for (int y = roomY; y < roomY + roomHeight; y++)
                {
                    if (x == roomX
                        || x == roomX + roomWidth - 1
                        || y == roomY
                        || y == roomY + roomHeight - 1)
                    {
                        if (!TrySetWallTile(new Vector3Int(x, y, 0)))
                        {
                            continue;
                        }
                    }
                    else
                    {
                        SetFloorTile(new Vector3Int(x, y, 0));
                    }
                }
            }

            // Maak een gang tussen kamers
            if (rooms.Count != 0)
            {
                TunnelBetween(rooms[rooms.Count - 1], room);
            }

            // Plaats vijanden in de kamer
            PlaceEnemies(room, maxEnemies);

            // Plaats items in de kamer
            PlaceItems(room, maxItems);

            rooms.Add(room);
        }

        // Plaats een ladder naar beneden in het midden van de laatste kamer
        if (currentFloor > 0)
        {
            PlaceLadder(new Vector3(rooms[rooms.Count - 1].X + rooms[rooms.Count - 1].Width / 2, rooms[rooms.Count - 1].Y + rooms[rooms.Count - 1].Height / 2, 0), false);
        }

        // Als de speler al bestaat, pas dan zijn positie aan en zet hem in het midden van de eerste kamer
        if (MapManager.Get.GetActor("Player") != null)
        {
            MapManager.Get.GetActor("Player").transform.position = new Vector3(rooms[0].X + rooms[0].Width / 2, rooms[0].Y + rooms[0].Height / 2, 0);
        }
        // Als de speler nog niet bestaat, maak dan net zoals eerst een nieuwe speler
        else
        {
            var player = MapManager.Get.CreateActor("Player", new Vector3(rooms[0].X + rooms[0].Width / 2, rooms[0].Y + rooms[0].Height / 2, 0));
        }

        // Als currentFloor groter is dan 0, plaats dan een ladder naar boven in het midden van de eerste kamer
        if (currentFloor > 0)
        {
            PlaceLadder(new Vector3(rooms[0].X + rooms[0].Width / 2, rooms[0].Y + rooms[0].Height / 2, 0), true);
        }
    }

    private bool TrySetWallTile(Vector3Int pos)
    {
        // Als dit een vloer is, zou het geen muur moeten zijn
        if (MapManager.Get.FloorMap.GetTile(pos))
        {
            return false;
        }
        else
        {
            // Zo niet, dan kan het een muur zijn
            MapManager.Get.ObstacleMap.SetTile(pos, MapManager.Get.WallTile);
            return true;
        }
    }

    private void SetFloorTile(Vector3Int pos)
    {
        // Deze tegel moet begaanbaar zijn, dus verwijder elk obstakel
        if (MapManager.Get.ObstacleMap.GetTile(pos))
        {
            MapManager.Get.ObstacleMap.SetTile(pos, null);
        }
        // Stel de vloer tegel in
        MapManager.Get.FloorMap.SetTile(pos, MapManager.Get.FloorTile);
    }

    private void TunnelBetween(Room oldRoom, Room newRoom)
    {
        Vector2Int oldRoomCenter = oldRoom.Center();
        Vector2Int newRoomCenter = newRoom.Center();
        Vector2Int tunnelCorner;

        if (Random.value < 0.5f)
        {
            // Beweeg horizontaal, dan verticaal
            tunnelCorner = new Vector2Int(newRoomCenter.x, oldRoomCenter.y);
        }
        else
        {
            // Beweeg verticaal, dan horizontaal
            tunnelCorner = new Vector2Int(oldRoomCenter.x, newRoomCenter.y);
        }

        // Genereer de coördinaten voor deze tunnel
        List<Vector2Int> tunnelCoords = new List<Vector2Int>();
        BresenhamLine.Compute(oldRoomCenter, tunnelCorner, tunnelCoords);
        BresenhamLine.Compute(tunnelCorner, newRoomCenter, tunnelCoords);

        // Stel de tegels in voor deze tunnel
        for (int i = 0; i < tunnelCoords.Count; i++)
        {
            SetFloorTile(new Vector3Int(tunnelCoords[i].x, tunnelCoords[i].y, 0));

            for (int x = tunnelCoords[i].x - 1; x <= tunnelCoords[i].x + 1; x++)
            {
                for (int y = tunnelCoords[i].y - 1; y <= tunnelCoords[i].y + 1; y++)
                {
                    if (!TrySetWallTile(new Vector3Int(x, y, 0)))
                    {
                        continue;
                    }
                }
            }
        }
    }

    private void PlaceEnemies(Room room, int maxEnemies)
    {
        int num = Random.Range(0, maxEnemies + 1);

        // Maak een lijst van vijanden in omgekeerde volgorde van sterkte
        string[] enemyNames = { "Cyclops", "Angel", "DragonMan", "BatBeast", "ClawBeast", "Witch", "RedWasp", "Snake", "Wasp", "Beetle" };

        for (int counter = 0; counter < num; counter++)
        {
            // Bepaal de maximale index op basis van de huidige verdieping
            int maxIndex = Mathf.Clamp(currentFloor, 0, enemyNames.Length - 1);
            // Kies een willekeurig monster uit de lijst tot de maximale index
            int index = Random.Range(0, maxIndex + 1);

            // De randen van de kamer zijn muren, dus tel op en trek af met 1
            int x = Random.Range(room.X + 1, room.X + room.Width - 1);
            int y = Random.Range(room.Y + 1, room.Y + room.Height - 1);

            // Maak het geselecteerde monster aan
            GameManager.Get.CreateActor(enemyNames[index], new Vector2(x, y));
        }
    }


    private string GetEnemyName(int counter)
    {
        // We verdelen de enemylijst in secties, afhankelijk van het aantal vijanden dat we willen plaatsen
        float section = (float)enemyNames.Count / (float)maxEnemies;
        int index = Mathf.FloorToInt(counter * section);

        // Zorg ervoor dat de index binnen het bereik van de enemyNames-lijst blijft
        index = Mathf.Clamp(index, 0, enemyNames.Count - 1);

        return enemyNames[index];
    }

    private void PlaceItems(Room room, int maxItems)
    {
        // Het aantal items dat we willen
        int num = Random.Range(0, maxItems + 1);

        for (int counter = 0; counter < num; counter++)
        {
            // De randen van de kamer zijn muren, dus tel op en trek af met 1
            int x = Random.Range(room.X + 1, room.X + room.Width - 1);
            int y = Random.Range(room.Y + 1, room.Y + room.Height - 1);

            // Maak verschillende items aan
            float randomValue = Random.value;
            if (randomValue < 0.33f)
            {
                GameManager.Get.CreateActor("HealthPotion", new Vector2(x, y));
            }
            else if (randomValue < 0.66f)
            {
                GameManager.Get.CreateActor("Fireball", new Vector2(x, y));
            }
            else
            {
                GameManager.Get.CreateActor("ScrollOfConfusion", new Vector2(x, y));
            }
        }
    }

    private void PlaceLadder(Vector3 position, bool isUp)
    {
        if (isUp)
        {
            Instantiate(Resources.Load<GameObject>("Prefabs/LadderUp"), position, Quaternion.identity);
        }
        else
        {
            Instantiate(Resources.Load<GameObject>("Prefabs/LadderDown"), position, Quaternion.identity);
        }
    }
}
